using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection
{
    public class Import_Table
    {
        #region Parameter

        public enum LineImportState { Success, NoData, Failed };

        public enum DataTreatment { Merge, Insert, Update };
        private DataTreatment _DataTreatment = DataTreatment.Insert;
        public DataTreatment TreatmentOfData
        {
            get { return _DataTreatment; }
            set { _DataTreatment = value; }
        }

        public enum ErrorTreatment { Manual, AutoNone, AutoFirst, AutoLast };
        public ErrorTreatment TreatmentOfErrors = ErrorTreatment.AutoNone;
        //public bool CheckErrors = false;

        private string _TableAlias;
        public string TableAlias
        {
            get { return _TableAlias; }
            set { _TableAlias = value; }
        }
        private string _TableName;
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }

        private string _WhereClause = "";
        public string WhereClause()
        {
            try
            {
                if (this._WhereClause == null)
                    this._WhereClause = "";
                if (this._WhereClause.Length == 0)
                {
                    if (DiversityCollection.Import.AttachmentKeyImportColumn != null &&
                        this.TableAlias == DiversityCollection.Import.AttachmentKeyImportColumn.TableAlias)
                    {
                        this._WhereClause = DiversityCollection.Import.AttachmentKeyImportColumn.Column + " = '" +
                        this._ColumnValueDictionary[DiversityCollection.Import.AttachmentKeyImportColumn.Column].Trim() + "'";
                        ///TODO: werte trimmen
                        ///TODO: Wert aus Spalte des Attachmentkeys
                    }
                    foreach (string s in this.PrimaryKeyColumnList)
                    {
                        if (this._ColumnValueDictionary.ContainsKey(s))
                        {
                            if (this._ColumnValueDictionary[s].Length > 0)
                            {
                                if (this._WhereClause.Length > 0)
                                    this._WhereClause += " AND ";
                                this._WhereClause += s + " = '" + this._ColumnValueDictionary[s].Trim() + "'";
                            }
                        }
                    }
                }
                return _WhereClause;
            }
            catch (System.Exception ex) { }
            return "";
        }

        public void WhereClause(string WhereClause)
        {
            this._WhereClause = WhereClause;
        }
        public enum NeededAction { NoAction, Update, Insert, SelectTarget };
        private NeededAction _NeededAction = NeededAction.NoAction;
        public NeededAction ActionNeeded { get { return this._NeededAction; } 
            set { this._NeededAction = value; } }

        private int _iLine;
        public int iLine { get { return this._iLine; } }
        //private string _ParentTableAlias;
        //private DiversityCollection.Import_Table _SuperiorTable;
        private System.Collections.Generic.List<string> _ForeignPrimaryKeyColumns;
        private System.Windows.Forms.DataGridView _DataGridView;
        private System.Collections.Generic.Dictionary<string, string> _ColumnValueDictionary;

        private System.Collections.Generic.Dictionary<string, string> _ColumnValuesInDatabase;
        private System.Collections.Generic.Dictionary<string, string> ColumnValuesInDatabase
        {
            get 
            {
                if (this._ColumnValuesInDatabase == null)
                    this._ColumnValuesInDatabase = new Dictionary<string, string>();
                return _ColumnValuesInDatabase; 
            }
            //set { _ColumnValuesInDatabase = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _ColumnValueErros;
        public System.Collections.Generic.Dictionary<string, string> ColumnValueErros
        {
            get
            {
                if (this._ColumnValueErros == null)
                    this._ColumnValueErros = new Dictionary<string, string>();
                return _ColumnValueErros;
            }
        }

        private System.Collections.Generic.List<string> _Columns;
        private System.Collections.Generic.List<string> _PrimaryKeyColumns;
        private string _IdentityColumn;
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.SortedList<int, DiversityCollection.Import_Column>> _ImportColumnDictionary;
        private System.Collections.Generic.Dictionary<string, string> _ParentTableNameForColumnDictionary;
        private System.Collections.Generic.Dictionary<string, string> _ColumnNameInRelatedTable;

        private System.Collections.Generic.Dictionary<int, string> _ImportErrors;
        private System.Collections.Generic.Dictionary<int, string> ImportErrors
        {
            get 
            { 
                if (this._ImportErrors == null)
                this._ImportErrors = new Dictionary<int,string>();
                return _ImportErrors; 
            }
            //set { _ImportErrors = value; }
        }

        public bool AddedForImportAsSuperiorTable = false;

        private string _LogCreatedWhenColumn;
        public string LogCreatedWhenColumn()
        {
            if (this._LogCreatedWhenColumn != null)
                return this._LogCreatedWhenColumn;
            string SelectStatementForOrderColumn = "select top 1 C.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = '" + this.TableName + "' " +
                "and C.COLUMN_NAME like 'Log%When' " +
                "and not C.COLUMN_NAME like 'LogUpdatedWhen'";
            this._LogCreatedWhenColumn = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SelectStatementForOrderColumn);
            return this._LogCreatedWhenColumn;
        }

        //public string AttachmentKeyColumnValue;

        #endregion

        #region Construction

        public Import_Table(string TableAlias, string TableName, System.Windows.Forms.DataGridView DatagridView)
        {
            this._TableAlias = TableAlias;
            this._TableName = TableName;
            this._DataGridView = DatagridView;
        }

        #region Basic informations
        
        private void initTable()
        {
            this.CreateSuperiorImportColumnsForPK();
            this.CreateMissingPKColumns();
        }

        private void CreateSuperiorImportColumnsForPK()
        {
            this.FindPKColumnsWithMissingForeignRelations();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KV in this._PKColumnsWithMissingForeignRelations)
            {

            }
        }

        private void CompleteMissingSuperiorTable(DiversityCollection.Import_Column ImportColumn)
        {
        }

        public void CreateMissingPKColumns()
        {
            try
            {
                foreach (string s in this.PrimaryKeyColumnList)
                {
                    DiversityCollection.Import_Column IcForTest = new Import_Column();
                    IcForTest.Table = this.TableName;
                    IcForTest.TableAlias = this.TableAlias;
                    IcForTest.Column = s;
                    IcForTest.IsSelected = true;
                    IcForTest.TypeOfSource = Import_Column.SourceType.Database;
                    if (DiversityCollection.Import.ImportColumns.ContainsKey(IcForTest.Key))
                    {
                        continue;
                    }
                    else
                    {
                        DiversityCollection.Import_Column IC = DiversityCollection.Import_Column.GetImportColumn("", this.TableName, this.TableAlias, s, 1, null
                            , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database);//._ new Import_Column();
                        IC.Table = this.TableName;
                        IC.TableAlias = this.TableAlias;
                        IC.Column = s;
                        IC.IsSelected = true;
                        IC.TypeOfSource = Import_Column.SourceType.Database;
                        //DiversityCollection.Import.ImportColumns.Add(IC.Key, IC);
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        //private System.Collections.Generic.List<string> MissingParentColumns
        //{
        //    get
        //    {
        //        System.Collections.Generic.List<string> MPC = new List<string>();
        //        //foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ParentTableNameForColumnDictionary)
        //        //{
        //        //    string TableColumn = KV.Key;
        //        //    string ParentTable = KV.Value;
        //        //    bool ParentColumnIsMissing = true;
        //        //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> TableColumns in DiversityCollection.Import.ImportColumns)
        //        //    {
        //        //        if (TableColumns.Value.TableAlias == ParentTable) // Table present
        //        //        {
        //        //            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.SortedList<int, DiversityCollection.Import_Column>> CD in this.ImportColumnDictionary)
        //        //            {
        //        //                if (CD.Value[0].ParentTable == TableColumns.Value.Table
        //        //                    && CD.Value[0].ParentTableAlias == TableColumns.Value.TableAlias
        //        //                    && CD.Value[0].ParentColumn == TableColumns.Value.Column)
        //        //                    ParentColumnIsMissing = false;
        //        //                break;
        //        //            }
        //        //        }
        //        //    }
        //        //    MPC.Add(KV.Key);
        //        //}
        //        return MPC;
        //    }
        //}

        //private void completePK()
        //{
        //    foreach (string s in this.PrimaryKeyColumnList)
        //    {

        //    }
        //}
        
        #endregion        
        
        #endregion

        #region Interface

        public System.Collections.Generic.Dictionary<int, string> getImportErrors()
        { return this.ImportErrors; }

        public void ResetImportErrors() { this._ImportErrors = null; }
        
        public void FillTableWithDataFromFile(int iLine)
        {
            try
            {
                this._iLine = iLine;
                this.ClearValues();
                this.FillValues();
                this.FillColumnsFromSuperiorTables();
            }
            catch (System.Exception ex) { }
        }

        public void ImportTableData(int iLine, System.Data.SqlClient.SqlConnection ImportConnection, System.Data.SqlClient.SqlTransaction ImportTransaction)
        {
            try
            {
                if (DiversityCollection.Import.AttachmentKeyImportColumn != null
                    && DiversityCollection.Import.AttachmentKeyImportColumn.Table == this._TableName
                    && this.ColumnList.Contains(DiversityCollection.Import.AttachmentKeyImportColumn.Column))
                {
                    if (this._DataTreatment == DataTreatment.Insert)
                    {
                        this.InsertData(this._iLine, ImportConnection, ImportTransaction);
                    }
                    else if(this.CompareAndInsertValuesViaAttachmentKey(iLine, ImportConnection, ImportTransaction))// this.FillColumnsInAttachedTableFromDatabase();
                        this.UpdateDataInAttachedTable(iLine, ImportConnection, ImportTransaction);
                }
                else
                {
                    DiversityCollection.Import_Table.DataTreatment DT = DiversityCollection.Import.ImportTables[this._TableAlias].TreatmentOfData;
                    switch (this.TreatmentOfData)
                    {
                        case DataTreatment.Merge:
                            if (this.PKisComplete)  // the PK is complete so an update should be possible
                            {
                                this.CompareAndInsertValues(this._iLine);
                                switch(this._NeededAction)
                                {
                                    case NeededAction.NoAction:
                                        break;
                                    case NeededAction.Update:
                                        this.UpdateData(iLine, ImportConnection, ImportTransaction);
                                        break;
                                    case NeededAction.Insert:
                                        this.InsertData(this._iLine, ImportConnection, ImportTransaction);
                                        break;
                                }
                            }
                            else
                                this.UpdateOrInsertData(iLine, ImportConnection, ImportTransaction);
                            break;
                        case DataTreatment.Insert:
                            this.InsertData(iLine, ImportConnection, ImportTransaction);
                            break;
                        case DataTreatment.Update:
                            if (this.PKisComplete)
                                this.UpdateData(iLine, ImportConnection, ImportTransaction);
                            else
                                this.UpdateOrInsertData(iLine, ImportConnection, ImportTransaction);
                            break;
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        public bool FillMissingPKvaluesWithValidKey()
        {
            bool OK = true;
            string SQL = "";
            foreach (string s in this.PrimaryKeyColumnList)
            {
                if (SQL.Length > 0)
                    SQL += ", ";
                SQL += s;
            }
            SQL = "SELECT TOP 1 " + SQL + " FROM " + this.TableName + " WHERE " + this.WhereClause() + " ORDER BY " + this.LogCreatedWhenColumn();
            if (this.TreatmentOfErrors == ErrorTreatment.AutoFirst)
                SQL += " ASC";
            else if (this.TreatmentOfErrors == ErrorTreatment.AutoLast)
                SQL += " DESC";
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                foreach (System.Data.DataColumn C in dt.Columns)
                {
                    this._ColumnValueDictionary[C.ColumnName] = dt.Rows[0][C.ColumnName].ToString();
                }
            }
            else// if (this.TreatmentOfErrors == ErrorTreatment.Manual)
            {
                switch(this.TreatmentOfErrors)
                {
                    case ErrorTreatment.Manual:
                        string Message = "Please select the dataset that should be updated";
                        DiversityCollection.Forms.FormImportWizardCheckError f = new Forms.FormImportWizardCheckError(this, this._iLine, Message);
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                        }
                        break;
                    case ErrorTreatment.AutoFirst:
                        break;
                    case ErrorTreatment.AutoLast:
                        break;
                    case ErrorTreatment.AutoNone:
                        break;
                }
            }
            //else OK = false;
            return OK;
        }

        public string ValueOfColumn(string ColumnName)
        {
            string Value = "";
            if (this._ColumnValueDictionary.ContainsKey(ColumnName))
                Value = this._ColumnValueDictionary[ColumnName];
            return Value;
        }

        public void FillColumnsFromSuperiorTables()
        {
            try
            {
                System.Collections.Generic.Dictionary<string, string> newValues = new Dictionary<string, string>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> CV in this._ColumnValueDictionary)
                {
                    if (CV.Value.Length > 0)
                        continue;
                    foreach (System.Collections.Generic.KeyValuePair<int, DiversityCollection.Import_Column> IC in this._ImportColumnDictionary[CV.Key])
                    {
                        if (IC.Value.TypeOfSource == Import_Column.SourceType.Database
                            && IC.Value.ParentTableAlias().Length > 0
                            && !IC.Value.IsIdentityColumn())
                        {
                            try
                            {
                                if (DiversityCollection.Import.ImportTables.ContainsKey(IC.Value.ParentTableAlias()))
                                {
                                    if (IC.Value.ParentColumn() != IC.Value.Column)
                                    {
                                        //string ParentColumn = IC.Value.ParentColumn();
                                        //string ParentTable = IC.Value.ParentTableAlias();
                                        string Value = DiversityCollection.Import.ImportTables[IC.Value.ParentTableAlias()].ValueOfColumn(IC.Value.ParentColumn());
                                        newValues.Add(CV.Key, Value);
                                    }
                                    else
                                    {
                                        string Value = DiversityCollection.Import.ImportTables[IC.Value.ParentTableAlias()].ValueOfColumn(IC.Value.Column);
                                        newValues.Add(CV.Key, Value);
                                    }
                                }
                            }
                            catch (System.Exception ex) { }
                        }
                    }
                }
                if (newValues.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in newValues)
                        this._ColumnValueDictionary[KV.Key] = KV.Value;
                }
            }
            catch (System.Exception ex) { }
        }
       
        #endregion

        #region Handling the data from the file
        
        /// <summary>
        /// all columns of the table with the values
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> ColumnValueDictionary()
        {
            if (this._ColumnValueDictionary == null || this._ColumnValueDictionary.Count == 0)
            {
                this._ColumnValueDictionary = new Dictionary<string, string>();
            }
            return this._ColumnValueDictionary;
        }

        /// <summary>
        /// the PK of the current row
        /// </summary>
        private System.Collections.Generic.Dictionary<string, string> PrimaryKey
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> _PK = new Dictionary<string, string>();
                foreach (string s in this.PrimaryKeyColumnList)
                {
                    if (this._ColumnValueDictionary.ContainsKey(s))
                        _PK.Add(s, this.ColumnValueDictionary()[s]);
                }
                return _PK;
            }
        }

        /// <summary>
        /// clear all values of the column dictionary
        /// </summary>
        public void ClearValues()
        {
            try
            {
                this.ColumnValueErros.Clear();
                this.ColumnValueDictionary().Clear();
                this.ColumnValuesInDatabase.Clear();
                this._WhereClause = null;
            }
            catch (System.Exception ex) { }
        }

        public void AddColumnValue(string Column, string Value)
        {
            if (this.ColumnValueDictionary().ContainsKey(Column))
                this.ColumnValueDictionary()[Column] = Value;
            else
                this._ColumnValueDictionary.Add(Column, Value);
        }

        private void FillValues()
        {
            try
            {
                foreach (string s in this.ImportColumnList)
                {
                    if (!this.ColumnValueDictionary().ContainsKey(s)
                        && s != this.IdentityColumn())
                        this.ColumnValueDictionary().Add(s, this.ColumnValue(s));
                }
            }
            catch (System.Exception ex) { }
        }
        
        /// <summary>
        /// fill in the values from the datagrid
        /// </summary>
        //public void FillValues(System.Windows.Forms.DataGridView Grid, int Row)
        //{
        //    foreach (string s in this.ImportColumnList)
        //    {
        //        if (!this.ColumnValueDictionary().ContainsKey(s))
        //            this.ColumnValueDictionary().Add(s, this.ColumnValue(s, Row));
        //    }
        //}

        private string ColumnValue(string ColumnName)
        {
            string Value = "";
            try
            {
                if (this._ImportColumnDictionary == null)
                    this.initImportColumnDictionary();
                if (this._ImportColumnDictionary.ContainsKey(ColumnName))
                {
                    foreach (System.Collections.Generic.KeyValuePair<int, DiversityCollection.Import_Column> IC in this._ImportColumnDictionary[ColumnName])
                    {
                        if (IC.Value.Sequence() == 0)
                            continue;
                        try
                        {
                            if (/*IC.Value.TypeOfSource == Import_Column.SourceType.File &&*/ IC.Value.ColumnInSourceFile != null)
                            {
                                int iCell = (int)IC.Value.ColumnInSourceFile;
                                if (this._DataGridView.Rows[this._iLine].Cells[iCell].Value != null)
                                {
                                    if (this._DataGridView.Rows[this._iLine].Cells[iCell].Value.ToString().Length > 0)
                                    {
                                        Value += IC.Value.Separator;
                                    }
                                    string ToTransform = this._DataGridView.Rows[this._iLine].Cells[iCell].Value.ToString();
                                    Value += IC.Value.TransformedValue(ToTransform);
                                }
                            }
                            else if (IC.Value.Column == IC.Value.PresetValueColumn
                                && IC.Value.PresetValue.Length > 0)
                            {
                                Value = IC.Value.PresetValue;
                            }
                            else if (IC.Value.Value != null && IC.Value.Value.Length > 0)
                            {
                                Value = IC.Value.Value;
                                break;
                            }
                            string Data_Type = IC.Value.DataType();
                            string Message = "";
                            bool ValueFitsDataType = true;
                            if (Value != null && Value.Trim().Length > 0)
                            {
                                switch (Data_Type)
                                {
                                    case "bit":
                                        bool b;
                                        if (!bool.TryParse(Value, out b))
                                            ValueFitsDataType = false;
                                        break;

                                    case "tinyint":
                                        int t;
                                        if (!int.TryParse(Value, out t) || t < 0 || t > 255)
                                            ValueFitsDataType = false;
                                        break;
                                    case "smallint":
                                        short si;
                                        if (!short.TryParse(Value, out si))
                                            ValueFitsDataType = false;
                                        break;
                                    case "int":
                                        int i;
                                        if (!int.TryParse(Value, out i))
                                            ValueFitsDataType = false;
                                        break;
                                    case "bigint":
                                        Int64 bi;
                                        if (!Int64.TryParse(Value, out bi))
                                            ValueFitsDataType = false;
                                        break;

                                    case "float":
                                    case "real":
                                        double f;
                                        if (!double.TryParse(Value, out f))
                                            ValueFitsDataType = false;
                                        break;
                                    case "datetime":
                                        System.DateTime d;
                                        if (!System.DateTime.TryParse(Value, out d))
                                        {
                                            ValueFitsDataType = false;
                                        }
                                        //else
                                        //{
                                        //    int y;
                                        //    if (int.TryParse(Value, out y))
                                        //    {
                                        //        if (y > 1550 && y <= System.DateTime.Now.Year + 1)
                                        //        {
                                        //        }
                                        //    }
                                        //}
                                        //string x = "";
                                        break;
                                    case "smalldatetime":

                                    case "char":
                                    case "nvarchar":
                                    case "varchar":

                                    case "xml":

                                    case "image":
                                    case "varbinary":

                                    case "geography":
                                    case "geometry":

                                    case "uniqueidentifier":
                                        break;
                                }
                                if (!ValueFitsDataType)
                                {
                                    Message = "The column " + IC.Value.Column + " expects a value of the datatype " + IC.Value.DataType() + ". The value found in the file is: " + Value.Trim() + ".";
                                    if (!this.ImportErrors.ContainsKey(this._iLine))
                                        this.ImportErrors.Add(this._iLine, Message);
                                    if (!this.ColumnValueErros.ContainsKey(IC.Value.Column))
                                        this.ColumnValueErros.Add(IC.Value.Column, Message);
                                }
                            }
                            if (IC.Value.DataType() == "DateTime")
                            {
                            }
                        }
                        catch (System.Exception ex) { }
                    }
                }
            }
            catch (System.Exception ex) { }
            if (DiversityCollection.Import.GetSetting(DiversityCollection.Import.Setting.TrimValues) == "True")
            {
                Value = Value.Trim();
            }
            return Value;
        }
        
        /// <summary>
        /// getting the value for a column according to the settings
        /// </summary>
        /// <param name="ColumnName">The name of the column</param>
        /// <param name="Row">The number of the row in the datagrid</param>
        /// <returns>The value of the column</returns>
        private string ColumnValue(string ColumnName, int Row)
        {
            string Value = "";
            foreach (System.Collections.Generic.KeyValuePair<int, DiversityCollection.Import_Column> KV in this.ImportColumns(ColumnName))
            {
                if (Value.Length > 0)
                    Value += KV.Value.Separator;
                try
                {
                    int iCell = (int)KV.Value.ColumnInSourceFile;
                    if (this._DataGridView.Rows[Row].Cells[iCell].Value != null)
                    {
                        string ToTransform = this._DataGridView.Rows[Row].Cells[iCell].Value.ToString();
                        Value += KV.Value.TransformedValue(ToTransform);
                    }
                }
                catch (System.Exception ex) { }
            }
            return Value;
        }

        private System.Collections.Generic.SortedList<int, DiversityCollection.Import_Column> ImportColumnParts(string ColumnName)
        {
            System.Collections.Generic.SortedList<int, DiversityCollection.Import_Column> SL = new SortedList<int, Import_Column>();

            return SL;
        }
        
        #endregion

        #region getting data from the database
		
        /// <summary>
        /// if the import is based on a column in the table, 
        /// fill the values in the table according to the value of the attachment column
        /// Options: 
        ///     No Parent-Child relation -> Update
        ///     With Parent-Child relation
        ///         Same column for attachment key and new data -> Update
        ///         No column for new data match attachment key column -> Update
        ///         Different column for attachment key and new data -> Insert
        /// </summary>
        /// <returns>If an update is needed</returns>
        public bool CompareAndInsertValuesViaAttachmentKey(int iLine, System.Data.SqlClient.SqlConnection ImportConnection, System.Data.SqlClient.SqlTransaction ImportTransaction)
        {
            bool UpdateNeeded = false;
            this._iLine = iLine;
            try
            {
                string Message = "";
                if (DiversityCollection.Import.AttachmentKeyImportColumn != null)
                {
                    if (this.ChildParentColumn.Length > 0 && 
                        this.ColumnValueDictionary().ContainsKey(DiversityCollection.Import.AttachmentKeyImportColumn.Column))
                    {
                        string SQL = "";
                        if (this.PrimaryKeyColumnList.Count == 1 &&
                            !this.ColumnValueDictionary().ContainsKey(this.PrimaryKeyColumnList[0]))
                        {
                            /// Das Anhängen an die SeriesID hat hier nicht funktioniert. 
                            /// es wurde die SeriesID als SeriesParentID genommen (s. else - Zweig)
                            /// muss noch getestet werden unter welchen Umständen der else Zweig der richtige ist
                            /// evtl. Series an Series
                            SQL = "SELECT " + this._PrimaryKeyColumns[0] + " FROM " + this._TableName +
                                " WHERE " + DiversityCollection.Import.AttachmentKeyImportColumn.Column + " = '" + this._DataGridView.Rows[this._iLine].Cells[(int)DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile].Value.ToString() + "'";
                        }
                        else
                        {
                            foreach (string s in this.PrimaryKeyColumnList)
                            {
                                if (s == this.ParentColumn) continue;
                                if (SQL.Length > 0) SQL += ", ";
                                SQL += s;
                            }
                            if (!this.PrimaryKeyColumnList.Contains(this.ChildParentColumn))
                            {
                                if (SQL.Length > 0) SQL += ", ";
                                SQL += this.ParentColumn + " AS " + this.ChildParentColumn;
                            }

                            SQL = "SELECT " + SQL + " FROM " + this._TableName +
                                " WHERE " + DiversityCollection.Import.AttachmentKeyImportColumn.Column + " = '" + this._DataGridView.Rows[this._iLine].Cells[(int)DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile].Value.ToString() + "'";
                        }
                        System.Data.DataTable dt = new System.Data.DataTable();
                        System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        if (dt.Rows.Count == 1)
                        {
                            foreach (System.Data.DataColumn C in dt.Columns)
                            {
                                if (!this.ColumnValuesInDatabase.ContainsKey(C.ColumnName)
                                    && !dt.Rows[0][C.ColumnName].Equals(System.DBNull.Value)
                                    && dt.Rows[0][C.ColumnName].ToString().Length > 0)
                                    this.ColumnValuesInDatabase.Add(C.ColumnName, dt.Rows[0][C.ColumnName].ToString());
                                if (!this._ColumnValueDictionary.ContainsKey(C.ColumnName)
                                    && !dt.Rows[0][C.ColumnName].Equals(System.DBNull.Value)
                                    && dt.Rows[0][C.ColumnName].ToString().Length > 0)
                                    this._ColumnValueDictionary.Add(C.ColumnName, dt.Rows[0][C.ColumnName].ToString());
                            }
                        }
                        else
                        {
                            if (dt.Rows.Count == 0)
                            {
                                Message = "No datasets where found for the value " + this._ColumnValueDictionary[DiversityCollection.Import.AttachmentKeyImportColumn.Column] +
                                    " for the column " + DiversityCollection.Import.AttachmentKeyImportColumn.Column;
                                this._NeededAction = NeededAction.Insert;
                            }
                            else
                            {
                                Message = "The value " + this._ColumnValueDictionary[DiversityCollection.Import.AttachmentKeyImportColumn.Column] +
                                    " for the column " + DiversityCollection.Import.AttachmentKeyImportColumn.Column +
                                    " is not unique in the database";
                                this._NeededAction = NeededAction.SelectTarget;
                            }
                            if (this.TreatmentOfErrors == ErrorTreatment.Manual)
                            {
                                DiversityCollection.Forms.FormImportWizardCheckError f = new Forms.FormImportWizardCheckError(this, this._iLine, Message);
                                f.ShowDialog();
                                if (f.DialogResult == System.Windows.Forms.DialogResult.Ignore)
                                {
                                    Message = "";
                                }
                                else if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    if (this.ActionNeeded == NeededAction.Insert)
                                    {
                                        this.InsertData(this._iLine, ImportConnection, ImportTransaction);
                                        Message = "";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // e.g. this._ColumnValueDictionary.Count == 0
                        string SQL = "";
                        foreach (string s in this.ColumnList)
                        {
                            if (SQL.Length > 0) SQL += ", ";
                            SQL += s;
                        }

                        SQL = "SELECT " + SQL
                            + " FROM " + this._TableName
                            + " WHERE " + DiversityCollection.Import.AttachmentKeyImportColumn.Column + " = '" 
                            + this._DataGridView.Rows[this._iLine].Cells[(int)DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile].Value.ToString() + "'";
                        // e.g. "SELECT SeriesID, SeriesParentID, Description, SeriesCode, Notes, Geography, DateStart, DateEnd, DateCache FROM CollectionEventSeries WHERE SeriesCode = 'E_1_1'"
                        System.Data.DataTable dt = new System.Data.DataTable();
                        System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        if (dt.Rows.Count == 1)
                        {
                            foreach(System.Data.DataColumn C in dt.Columns)
                            {
                                if (!this.ColumnValuesInDatabase.ContainsKey(C.ColumnName)
                                    && !dt.Rows[0][C.ColumnName].Equals(System.DBNull.Value)
                                    && dt.Rows[0][C.ColumnName].ToString().Length > 0)
                                {
                                    this.ColumnValuesInDatabase.Add(C.ColumnName, dt.Rows[0][C.ColumnName].ToString());
                                }
                            }
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ColumnValuesInDatabase)
                            {
                                if (!this.ColumnValueDictionary().ContainsKey(KV.Key))
                                    this.ColumnValueDictionary().Add(KV.Key, KV.Value.Trim());
                                else if ((this.ColumnValueDictionary()[KV.Key] != KV.Value && this.ColumnValueDictionary()[KV.Key].Trim().Length == 0 && KV.Value.Trim().Length > 0))
                                    this.ColumnValueDictionary()[KV.Key] = KV.Value.Trim();
                                else if (this.ColumnValueDictionary()[KV.Key].Trim() != KV.Value.Trim() && this.ColumnValueDictionary()[KV.Key].Trim().Length > 0 && KV.Value.Trim().Length > 0)
                                    UpdateNeeded = true;
                            }
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._ColumnValueDictionary)
                            {
                                if (!this.ColumnValuesInDatabase.ContainsKey(KV.Key))
                                {
                                    UpdateNeeded = true;
                                    break;
                                }
                            }
                            ///TODO: Eingefuegt da sonst e.g. CollectionEvent nochmal neu eingelesen wird obwohl schon vorhanden
                            if (DiversityCollection.Import.ImportTables.ContainsKey(DiversityCollection.Import.AttachmentKeyImportColumn.TableAlias))
                                DiversityCollection.Import.ImportTables[DiversityCollection.Import.AttachmentKeyImportColumn.Table].TreatmentOfData = Import_Table.DataTreatment.Update;
                        }
                        else
                        {
                            if (dt.Rows.Count == 0)
                            {
                                string Value = "";
                                try
                                {
                                    Value = this._DataGridView.Rows[this._iLine].Cells[(int)DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile].Value.ToString();
                                }
                                catch (System.Exception ex) { }
                                if (Value.Length == 0 && this.ColumnValueDictionary().ContainsKey(DiversityCollection.Import.AttachmentKeyImportColumn.Column))
                                    Value = this.ColumnValueDictionary()[DiversityCollection.Import.AttachmentKeyImportColumn.Column];
                                Message = "No datasets where found for the value " + Value +
                                    " for the column " + DiversityCollection.Import.AttachmentKeyImportColumn.Column;
                                if (this.TableName == DiversityCollection.Import.AttachmentKeyImportColumn.Table)
                                    this._NeededAction = NeededAction.NoAction;
                                else
                                    this._NeededAction = NeededAction.Insert;     
                            }
                            else
                            {
                                if (this.ColumnValueDictionary().ContainsKey(DiversityCollection.Import.AttachmentKeyImportColumn.Column))
                                    Message = "The value " + this.ColumnValueDictionary()[DiversityCollection.Import.AttachmentKeyImportColumn.Column] +
                                    " for the column " + DiversityCollection.Import.AttachmentKeyImportColumn.Column +
                                    " is not unique in the database";
                                else
                                    Message = "The value " + this._DataGridView.Rows[this._iLine].Cells[(int)DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile].Value.ToString() +
                                        " for the column " + DiversityCollection.Import.AttachmentKeyImportColumn.Column +
                                        " is not unique in the database";
                                this._NeededAction = NeededAction.SelectTarget;
                            }
                            if (this.TreatmentOfErrors == ErrorTreatment.Manual)
                            {
                                DiversityCollection.Forms.FormImportWizardCheckError f = new Forms.FormImportWizardCheckError(this, this._iLine, Message);
                                f.ShowDialog();
                                if (f.DialogResult == System.Windows.Forms.DialogResult.Ignore)
                                {
                                    Message = "";
                                }
                                else if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    if (this.ActionNeeded == NeededAction.Insert)
                                    {
                                        this.InsertData(this._iLine, ImportConnection, ImportTransaction);
                                        Message = "";
                                    }
                                    else if (this.ActionNeeded == NeededAction.Update)
                                    {
                                        if (f.WhereClause().Length > 0)
                                            this.WhereClause(f.WhereClause());
                                        this.UpdateData(this._iLine, ImportConnection, ImportTransaction);
                                        Message = "";
                                    }
                                }
                            }
                        }
                    }
                }
                if (Message.Length > 0)
                    this.ImportErrors.Add(iLine, Message);
            }
            catch (System.Exception ex) { }
            return UpdateNeeded;
        }

        public void CompareAndInsertValues(int iLine)
        {
            try
            {
                string Message = "";
                if (DiversityCollection.Import.AttachmentKeyImportColumn != null)
                {
                    string SQL = "";
                    foreach (string s in this.ColumnList)
                    {
                        if (SQL.Length > 0) SQL += ", ";
                        SQL += s;
                    }
                    string WhereClause = this.WhereClause();
                    if (this._IdentityColumn != null && this._IdentityColumn.Length > 0 && this._ColumnValueDictionary.ContainsKey(this._IdentityColumn) && this._ColumnValueDictionary[this._IdentityColumn].Length > 0)
                        WhereClause += " AND " + this._IdentityColumn + " = " + this._ColumnValueDictionary[this._IdentityColumn];
                    if (WhereClause.Length == 0)
                    {
                        // the current table seems to be above the attachement table and the key could not be found
                        if (this.IsParentTableOfAttachmentTable)
                        {
                            this._NeededAction = NeededAction.Insert;
                            return;
                        }
                    }
                    SQL = "SELECT " + SQL
                        + " FROM " + this._TableName
                        + " WHERE " + WhereClause;
                    
                    //if (this._IdentityColumn != null && this._IdentityColumn.Length > 0 && this._ColumnValueDictionary.ContainsKey(this._IdentityColumn) && this._ColumnValueDictionary[this._IdentityColumn].Length > 0)
                    //    SQL += " AND " + this._IdentityColumn + " = " + this._ColumnValueDictionary[this._IdentityColumn];

                    System.Data.DataTable dt = new System.Data.DataTable();
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        foreach (System.Data.DataColumn C in dt.Columns)
                        {
                            if (!this.ColumnValuesInDatabase.ContainsKey(C.ColumnName)
                                && !dt.Rows[0][C.ColumnName].Equals(System.DBNull.Value)
                                && dt.Rows[0][C.ColumnName].ToString().Length > 0)
                                this.ColumnValuesInDatabase.Add(C.ColumnName, dt.Rows[0][C.ColumnName].ToString());
                        }
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ColumnValuesInDatabase)
                        {
                            if (!this._ColumnValueDictionary.ContainsKey(KV.Key))
                                this._ColumnValueDictionary.Add(KV.Key, KV.Value);
                            else if (this._ColumnValueDictionary[KV.Key] != KV.Value)
                                this._NeededAction = NeededAction.Update;
                        }
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._ColumnValueDictionary)
                        {
                            if (!this.ColumnValuesInDatabase.ContainsKey(KV.Key))
                            {
                                this._NeededAction = NeededAction.Update;
                                break;
                            }
                        }
                        if (this._NeededAction == NeededAction.SelectTarget)
                            this._NeededAction = NeededAction.NoAction;
                    }
                    else if (dt.Rows.Count == 0)
                    {
                        this._NeededAction = NeededAction.Insert;
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        this._NeededAction = NeededAction.SelectTarget;
                    }
                }
                if (Message.Length > 0)
                    this.ImportErrors.Add(iLine, Message);
            }
            catch (System.Exception ex) { }
           // return UpdateNeeded;
        }

        public void FillSuperiorKeyColumns(int iLine)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> CC = this.FindColumnsWithForeignRelations();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KV in CC)
                {
                    if (this.ColumnValuesInDatabase.ContainsKey(KV.Value.Column))
                    {
                        System.Collections.Generic.List<string> TT = new List<string>();
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> IC in DiversityCollection.Import.ImportColumns)
                        {
                            if (!TT.Contains(IC.Value.TableAlias)
                                && DiversityCollection.Import.ImportTables.ContainsKey(IC.Value.TableAlias)
                                && DiversityCollection.Import.ImportTables[IC.Value.TableAlias].DatabaseColumnList().Contains(KV.Value.Column))
                                TT.Add(IC.Value.TableAlias);
                        }
                        foreach (string T in TT)
                        {
                            if (DiversityCollection.Import.ImportTables.ContainsKey(T)
                                && DiversityCollection.Import.ImportTables[T].DatabaseColumnList().Contains(KV.Value.Column))
                                DiversityCollection.Import.ImportTables[T].AddColumnValue(KV.Value.Column, this.ColumnValuesInDatabase[KV.Key]);
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        /// <summary>
        /// Filling missing values by values from the superior tables
        /// </summary>
        private string FillColumnFromSuperiorTable()
        {
            try
            {
                //for every column of the PK that relates to the PK of a superior table
                foreach (string s in this.ColumnList)
                {
                    if (!this._ColumnValueDictionary.ContainsKey(s)) // value is missing
                    {
                        if (this._ColumnsWithForeignRelations.ContainsKey(s)) // Column has a relation to a superior table
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._ParentTableNameForColumnDictionary)
                            {
                                //string ParentTableAlias = DiversityCollection.Import.ParentTableAlias(this.TableAlias, KV.Key);
                                if (DiversityCollection.Import.ImportTables.ContainsKey(KV.Key))
                                {
                                }
                            }
                    }
                    else if(this._ColumnValueDictionary.ContainsKey(s) 
                        && this._ColumnValueDictionary[s].Length == 0)
                    {
                        if (this._PKColumnsWithMissingForeignRelations.ContainsKey(s))
                        {
                            string ParentTable = this._PKColumnsWithMissingForeignRelations[s].ParentTableAlias();
                            if (DiversityCollection.Import.ImportTables[ParentTable].ColumnValueDictionary().ContainsKey(s))
                            {
                                string Value = DiversityCollection.Import.ImportTables[ParentTable].ColumnValueDictionary()[s];
                                if (Value.Length > 0)
                                    this._ColumnValueDictionary[s] = Value;
                            }
                            else if (DiversityCollection.Import.AttachmentKeyImportColumn != null &&
                                DiversityCollection.Import.AttachmentKeyImportColumn.Column == s &&
                                DiversityCollection.Import.AttachmentKeyImportColumn.Table == ParentTable)
                            {
                                string Value = this._DataGridView.Rows[this._iLine].Cells[(int)DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile].Value.ToString();
                                this._ColumnValueDictionary[s] = Value;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
            return "";
        }

	    #endregion        

        #region writing the data in the database

        /// <summary>
        /// for data in the table containing the attachment key, set the non-PK-Columns to new values if present
        /// </summary>
        /// <returns>the error message if the update was not successful</returns>
        private void UpdateDataInAttachedTable(int iLine, System.Data.SqlClient.SqlConnection ImportConnection, System.Data.SqlClient.SqlTransaction ImportTransaction)
        {
            string Message = "";
            string SQL = "";
            string SqlColumnValues = "";
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ColumnValueDictionary())
            {
                if (KV.Key == this.IdentityColumn())
                    continue;
                if (KV.Value.Length == 0)
                    continue;
                if (KV.Key == DiversityCollection.Import.AttachmentKeyImportColumn.Column)
                    continue;
                if (!this._ImportColumnList.Contains(KV.Key))
                    continue;
                if (SqlColumnValues.Length > 0) 
                    SqlColumnValues += ", ";
                SqlColumnValues += KV.Key + " = '" + KV.Value + "'";
            }
            if (SqlColumnValues.Length == 0 )
                Message = "No values found";
            SQL = "UPDATE " + this._TableName + " SET " + SqlColumnValues + " WHERE " + this.WhereClause();
            //SQL = "UPDATE " + this._TableName + " SET " + SqlColumnValues + " WHERE " + DiversityCollection.Import.AttachmentKeyImportColumn.Column + " = '" +
            //    this._ColumnValueDictionary[DiversityCollection.Import.AttachmentKeyImportColumn.Column] + "'";
            try
            {
                Message = this.SqlExecuteNonQuery(SQL, ImportConnection, ImportTransaction);
                //this.SqlExecuteScalar(SQL, ImportConnection, ImportTransaction, ref Message);
                       // Message = "Update failed";
            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
            }
            if (Message.Length > 0)
                this.ImportErrors.Add(iLine, Message);
        }


        private void UpdateOrInsertData(int iLine, System.Data.SqlClient.SqlConnection ImportConnection, System.Data.SqlClient.SqlTransaction ImportTransaction)
        {
            string Message = this.NonIdentityPKisComplete;
            try
            {
                if (Message.Length > 0)
                {
                    string X = "";
                    string Y = "";
                    if (!this.ValuesFound(ref X, ref Y))
                    {
                        Message = "";
                        return;
                    }
                }
                if (Message.Length == 0)
                {
                    this.CompareAndInsertValues(iLine);
                    switch (this._NeededAction)
                    {
                        case NeededAction.Insert:
                            this.InsertData(iLine, ImportConnection, ImportTransaction);
                            break;
                        case NeededAction.Update:
                            this.UpdateData(iLine, ImportConnection, ImportTransaction);
                            break;
                        case NeededAction.SelectTarget:
                            if (this.TreatmentOfErrors == ErrorTreatment.Manual)
                            {
                                string fMessage = "Please select the dataset that should be updated";
                                DiversityCollection.Forms.FormImportWizardCheckError f = new Forms.FormImportWizardCheckError(this, this._iLine, fMessage);
                                f.ShowDialog();
                                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    switch (this._NeededAction)
                                    {
                                        case NeededAction.Insert:
                                            this.InsertData(iLine, ImportConnection, ImportTransaction);
                                            break;
                                        case NeededAction.Update:
                                            this.UpdateData(iLine, ImportConnection, ImportTransaction);
                                            break;
                                    }
                                }
                                else if (f.DialogResult == System.Windows.Forms.DialogResult.Ignore) 
                                    Message = "";
                            }
                            else
                            {
                                this.FillMissingPKvaluesWithValidKey();
                            }
                            break;
                        case NeededAction.NoAction:
                            Message = "";
                            break;
                    }
                }
            }
            catch (System.Exception ex) { }
            if (Message.Length > 0 && this.ActionNeeded != NeededAction.NoAction)
                this.ImportErrors.Add(iLine, Message);
        }

        /// <summary>
        /// inserting the data of the current row
        /// </summary>
        /// <returns>the error message if the insert was not successful</returns>
        private void InsertData(int iLine, System.Data.SqlClient.SqlConnection ImportConnection, System.Data.SqlClient.SqlTransaction ImportTransaction)
        {
            try
            {
                string Message = this.NonIdentityPKisComplete;

                // check if there are values in superior tables. If there are no values neither in the current table nor in the superior table no data need to be imported
                if (Message.Length > 0)
                {
                    foreach (string s in this.PrimaryKeyColumnList)
                    {
                        if ((this.ColumnValueDictionary().ContainsKey(s)
                            && this.ColumnValueDictionary()[s].Length > 0)
                            || s == this.IdentityColumn())
                            continue;
                        else
                        {
                            try
                            {
                                if (this._PKColumnsWithMissingForeignRelations == null)
                                    this.FindPKColumnsWithMissingForeignRelations();
                                if (this._PKColumnsWithMissingForeignRelations.ContainsKey(s))
                                {
                                    DiversityCollection.Import_Column C = this._PKColumnsWithMissingForeignRelations[s];
                                    string X = "";
                                    string Y = "";
                                    string ParentTableAlias = C.ParentTableAlias();
                                    if (DiversityCollection.Import.ImportTables.ContainsKey(ParentTableAlias))
                                    {
                                        if ((!DiversityCollection.Import.ImportTables[C.ParentTableAlias()].ValuesFound(ref X, ref Y)
                                            && !DiversityCollection.Import.ImportTables[C.ParentTableAlias()].AddedForImportAsSuperiorTable))
                                        {
                                            Message = "";
                                            return;
                                        }
                                    }
                                    string Key = this.TableAlias + "." + s + ".1";
                                    if (DiversityCollection.Import.ImportColumns.ContainsKey(Key))
                                    {
                                        if (DiversityCollection.Import.ImportColumns[Key].Value != null)
                                        {
                                            if (this._ColumnValueDictionary.ContainsKey(s) && this._ColumnValueDictionary[s].Length == 0)
                                                this._ColumnValueDictionary[s] = DiversityCollection.Import.ImportColumns[Key].Value;
                                            else this._ColumnValueDictionary.Add(s, DiversityCollection.Import.ImportColumns[Key].Value);
                                            Message = "";
                                        }
                                    }

                                    // hier fliegt die Funktion raus weil eine "LocalisationSystemID" bei Altitude fehlt - darf eigentlich gar nicht sein, da die gesetzt ist
                                    //if (!DiversityCollection.Import.ImportTables.ContainsKey(ParentTableAlias) ||
                                    //    (!DiversityCollection.Import.ImportTables[C.ParentTableAlias()].ValuesFound(ref X, ref Y)
                                    //    && !DiversityCollection.Import.ImportTables[C.ParentTableAlias()].AddedForImportAsSuperiorTable))
                                    //{
                                    //    Message = "";
                                    //    return;
                                    //}
                                }
                                else
                                {
                                    /// e.g. IdentificationUnitInPart may miss the UnitID if Unit is not imported
                                }
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                    }
                }

                if (Message.Length == 0)
                {
                    string SQL = "";
                    if (this.ColumnValueDictionary().Count == 0 && !this.AddedForImportAsSuperiorTable)
                    {
                        return;
                    }

                    if (this.FindColumnsWithForeignRelations().Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KVcolumn in this._ColumnsWithForeignRelations)
                        {
                            if (DiversityCollection.Import.ImportTables.ContainsKey(KVcolumn.Value.ParentTableAlias()))
                            {
                                if (!this._ColumnValueDictionary.ContainsKey(KVcolumn.Key))
                                {
                                    try
                                    {
                                        string ParentTableAlias = KVcolumn.Value.ParentTableAlias();
                                        string ParentColumn = KVcolumn.Value.ParentColumn();
                                        this._ColumnValueDictionary.Add(KVcolumn.Key, DiversityCollection.Import.ImportTables[KVcolumn.Value.ParentTableAlias()].ColumnValueDictionary()[KVcolumn.Value.ParentColumn()]);
                                    }
                                    catch (System.Exception ex)
                                    {
                                        Message = "Adding value for " + KVcolumn.Key + " from superior table " + KVcolumn.Value.ParentTable() + " failed";
                                        this.ImportErrors.Add(iLine, Message);
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    string SqlColumns = "";
                    string SqlValues = "";
                    bool ValuesFound = this.ValuesFound(ref SqlColumns, ref SqlValues);

                    // the table IdentificationUnitInPart is added
                    if (!ValuesFound && this.TableName == "IdentificationUnitInPart")
                        ValuesFound = true;

                    if (!ValuesFound && !this.AddedForImportAsSuperiorTable)
                    {
                        Message = "No values found";
                        return;
                    }

                    if (this.AddedForImportAsSuperiorTable)
                    {
                        SqlColumns = "LogCreatedBy";
                        SqlValues = "User_Name()";
                    }

                    SQL = "INSERT INTO [" + this._TableName + "] (" + SqlColumns + ") VALUES (" + SqlValues + ")";
                    if (IdentityColumn().Length > 0)
                        SQL += "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                    try
                    {
                        if (IdentityColumn().Length > 0)
                        {
                            string Identity = this.SqlExecuteScalar(SQL, ImportConnection, ImportTransaction, ref Message);
                            if (this.ColumnValueDictionary().ContainsKey(IdentityColumn()))
                                this.ColumnValueDictionary()[IdentityColumn()] = Identity;
                            else
                                this._ColumnValueDictionary.Add(IdentityColumn(), Identity);
                        }
                        else
                        {
                            string SqlCheckCount = "SELECT COUNT(*) FROM [" + this.TableName + "] WHERE " + this.WhereClause();
                            int i;
                            if (int.TryParse(DiversityWorkbench.FormFunctions.SqlExecuteScalar(SqlCheckCount), out i)
                            && i > 0)
                            {
                                this.CompareAndInsertValues(this._iLine);
                                if (this.TreatmentOfErrors == ErrorTreatment.Manual && this._NeededAction != NeededAction.NoAction)
                                {
                                    Message = "Dataset for " + this.WhereClause() + " allready exists";
                                    DiversityCollection.Forms.FormImportWizardCheckError f = new Forms.FormImportWizardCheckError(this, this._iLine, Message);
                                    f.ShowDialog();
                                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK && this._NeededAction == NeededAction.Update)
                                    {
                                        this.UpdateData(iLine, ImportConnection, ImportTransaction);
                                    }
                                    else if (f.DialogResult == System.Windows.Forms.DialogResult.Ignore && this._NeededAction == NeededAction.NoAction)
                                        Message = "";
                                }
                                else if (this.TreatmentOfErrors == ErrorTreatment.AutoNone)
                                {
                                    Message = "Dataset for " + this.WhereClause() + " allready exists";
                                }
                                else if (this.TreatmentOfData == DataTreatment.Merge)
                                {
                                    if (this.TreatmentOfErrors == ErrorTreatment.AutoLast)
                                    {
                                    }
                                    else if (this.TreatmentOfErrors == ErrorTreatment.AutoFirst)
                                    {
                                    }
                                }
                            }
                            else
                            {
                                Message = this.SqlExecuteNonQuery(SQL, ImportConnection, ImportTransaction);
                                if (Message.Length > 0)
                                    Message = "Insert failed: " + Message;
                                //if (!DiversityWorkbench.FormFunctions.SqlExecuteNonQuery(SQL))
                                //    Message = ;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Message = ex.Message;
                    }
                }
                if (Message.Length > 0)
                {
                    bool ValuesFound = false;
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ColumnValueDictionary())
                    {
                        if (KV.Key == this.IdentityColumn())
                            continue;
                        if (KV.Value.Length == 0)
                            continue;
                        string ColumnKey = this.TableAlias + "." + KV.Key + ".1";
                        if (DiversityCollection.Import.ImportColumns.ContainsKey(ColumnKey) &&
                            DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfEntry != Import_Column.EntryType.Database)
                            ValuesFound = true;
                    }
                    if (ValuesFound)
                        this.ImportErrors.Add(iLine, Message);
                }
            }
            catch (System.Exception ex) { }
        }

        /// <summary>
        /// Check if there are values to import from the file that are not retrieved from the database or otherwise generated
        /// </summary>
        /// <param name="SqlColumns">the SQL-String containing the columns that should be imported</param>
        /// <param name="SqlValues">the SQL-String with the column corresponding values that should be imported</param>
        /// <returns></returns>
        private bool ValuesFound(ref string SqlColumns, ref string SqlValues)
        {
            bool ValuesFound = false;
            try
            {
                if (this.AddedForImportAsSuperiorTable)
                    return true;
                else
                {
                    /// getting the step key of the table
                    string StepKey = "";
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> IC in DiversityCollection.Import.ImportColumns)
                    {
                        if (IC.Value.TableAlias == this.TableAlias)
                        {
                            StepKey = IC.Value.StepKey;
                            break;
                        }
                    }

                    /// check if a superior import step was not imported due to empty decision columns
                    foreach (string s in DiversityCollection.Import.DecisionStepsWithNoValues)
                    {
                        if (StepKey.StartsWith(s))
                            return false;
                    }

                    /// Check if there are decision columns and if they contain data
                    if (this.DecisionColumns.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> IC in this.DecisionColumns)
                        {
                            if (this.ColumnValueDictionary().ContainsKey(IC.Value.Column) &&
                                this.ColumnValueDictionary()[IC.Value.Column].ToString().Length > 0)
                            {
                                ValuesFound = true;
                                break;
                            }
                        }
                        /// if there are decision columns, but they contain no data
                        if (!ValuesFound)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> IC in this.DecisionColumns)
                            {
                                DiversityCollection.Import.DecisionStepsWithNoValues.Add(IC.Value.StepKey);
                                break;
                            }
                            return ValuesFound;
                        }
                    }

                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ColumnValueDictionary())
                    {
                        if (KV.Key == this.IdentityColumn())
                            continue;
                        if (KV.Value.Length == 0)
                            continue;
                        if (SqlColumns.Length > 0) SqlColumns += ", ";
                        SqlColumns += KV.Key;
                        if (SqlValues.Length > 0) SqlValues += ", ";
                        SqlValues += this.FormatedValue(KV.Key, KV.Value).Trim();
                        string ColumnKey = this.TableAlias + "." + KV.Key + ".1";
                        if (DiversityCollection.Import.ImportColumns.ContainsKey(ColumnKey))
                        {
                            if (DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfSource == Import_Column.SourceType.File)
                                ValuesFound = true;
                            if (DiversityCollection.Import.ImportColumns[ColumnKey].ColumnInSourceFile != null
                                && DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfEntry != Import_Column.EntryType.Database)
                                ValuesFound = true;
                            // TODO: Unklar warum die parameter nicht als value erkannt werden
                            if (DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfSource == Import_Column.SourceType.Any &&
                                DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfEntry != Import_Column.EntryType.Database &&
                                this.TableAlias.StartsWith("CollectionEventParameterValue") &&
                                DiversityCollection.Import.ImportColumns[ColumnKey].Value != null &&
                                DiversityCollection.Import.ImportColumns[ColumnKey].Value.Length > 0)
                                ValuesFound = true;
                            ///TODO: Ursachen nicht klar - MethodID vorhanden, aber ValuesFound false, TypeOfSource = Database, sollte aber Interface sein
                            if (ColumnKey == "CollectionEventMethod.MethodID.1"
                                && KV.Value.Length > 0)
                                ValuesFound = true;
                            ///ToDo: StorageLocation for all wurde hier nicht als Wert erkannt. Source sollte auf Interface stehen
                            if ((DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfSource == Import_Column.SourceType.Any ||
                                DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfSource == Import_Column.SourceType.Interface) &&
                                DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfEntry != Import_Column.EntryType.Database &&
                                DiversityCollection.Import.ImportColumns[ColumnKey].IsSelected &&
                                //DiversityCollection.Import.ImportColumns[ColumnKey].IsTextDataType() &&
                                DiversityCollection.Import.ImportColumns[ColumnKey].Value != null &&
                                DiversityCollection.Import.ImportColumns[ColumnKey].Value.Length > 0 &&
                                DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfEntry != Import_Column.EntryType.MandatoryList)
                                ValuesFound = true;
                        }
                    }
                    if (!ValuesFound)
                    {
                        if (this.DependentTableContainsData())
                            ValuesFound = true;
                    }
                    if (!ValuesFound)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                        {
                            if (KV.Value.TableAlias() == this.TableAlias
                                && KV.Value.MustImport)
                                ValuesFound = true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return ValuesFound;
        }

        private System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> _DecisionColumns;
        public System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> DecisionColumns
        {
            get
            {
                if (this._DecisionColumns == null)
                {
                    this._DecisionColumns = new Dictionary<string, Import_Column>();
                    foreach(System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KV in DiversityCollection.Import.ImportColumns)
                    {
                        if (KV.Value.TableAlias == this.TableAlias && KV.Value.IsDecisionColumn)
                            this._DecisionColumns.Add(KV.Key, KV.Value);
                    }
                }
                return this._DecisionColumns;
            }
        }

        private bool DependentTableContainsData()
        {
            bool DependentDataPresent = false;
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> KVtable in DiversityCollection.Import.ImportTables)
                {
                    string ParentPKColumn = this.PrimaryKeyColumnList[0];
                    if (KVtable.Value._PKColumnsWithMissingForeignRelations.Count > 0)
                    {
                        if (KVtable.Value._PKColumnsWithMissingForeignRelations.ContainsKey(this.PrimaryKeyColumnList[0]))
                        {
                            string ParentTable = KVtable.Value._PKColumnsWithMissingForeignRelations[ParentPKColumn].ParentTableAlias();
                            if (ParentTable == this.TableAlias)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KVcolumn in KVtable.Value.ColumnValueDictionary())
                                {
                                    string ColumnKey = KVtable.Key + "." + KVcolumn.Key + ".1";
                                    if (KVcolumn.Key == this.IdentityColumn())
                                        continue;
                                    if (KVcolumn.Value.Length == 0)
                                    {
                                        continue;
                                    }
                                    if (DiversityCollection.Import.ImportColumns.ContainsKey(ColumnKey)
                                        && DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfEntry != Import_Column.EntryType.Database)
                                    {
                                        if (DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfSource != Import_Column.SourceType.Interface)
                                        {
                                            if (DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfSource != Import_Column.SourceType.Database)
                                            {
                                                DependentDataPresent = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (DependentDataPresent)
                                    break;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
            return DependentDataPresent;
        }

        private void UpdateData(int iLine, System.Data.SqlClient.SqlConnection ImportConnection, System.Data.SqlClient.SqlTransaction ImportTransaction)
        {
            string Message = this.NonIdentityPKisComplete;
            //UpdateNeeded = false;
            try
            {
                if (Message.Length == 0)
                {
                    string SQL = "";
                    string SqlColumnValues = "";
                    //this.CompareAndInsertValues(iLine);
                    switch (this._NeededAction)
                    {
                        case NeededAction.Update:
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ColumnValueDictionary())
                            {
                                if (KV.Key == this.IdentityColumn())
                                    continue;
                                if (KV.Value.Length == 0)
                                    continue;
                                if (this.PrimaryKeyColumnList.Contains(KV.Key))
                                    continue;
                                if (this.ColumnValuesInDatabase.ContainsKey(KV.Key) && this._ColumnValueDictionary[KV.Key] == this.ColumnValuesInDatabase[KV.Key])
                                    continue;
                                if (SqlColumnValues.Length > 0)
                                    SqlColumnValues += ", ";
                                if (KV.Value.StartsWith("getdate()"))
                                {
                                    string Key = this._TableAlias + "." + KV.Key + ".1";
                                    if (DiversityCollection.Import.ImportColumns.ContainsKey(Key) &&
                                        DiversityCollection.Import.ImportColumns[Key].TypeOfEntry == Import_Column.EntryType.Database &&
                                        DiversityCollection.Import.ImportColumns[Key].TypeOfSource == Import_Column.SourceType.Database)
                                        continue;
                                    SqlColumnValues += KV.Key + " = " + KV.Value;
                                }
                                else
                                    SqlColumnValues += KV.Key + " = '" + KV.Value + "'";
                            }
                            if (SqlColumnValues.Length == 0)
                            {
                                return;
                                //Message = "No values found";
                            }
                            SQL = "UPDATE " + this._TableName + " SET " + SqlColumnValues + " WHERE ";
                            SQL += " " + this.WhereClause();
                            if (this.IdentityColumn() != null
                                && this.IdentityColumn().Length > 0
                                && this.ColumnValueDictionary().ContainsKey(this.IdentityColumn())
                                && this.ColumnValueDictionary()[this.IdentityColumn()].Length > 0)
                            {
                                SQL += " AND " + this.IdentityColumn() + " = " + this.ColumnValueDictionary()[this.IdentityColumn()];
                            }

                            try
                            {
                                Message = this.SqlExecuteNonQuery(SQL, ImportConnection, ImportTransaction);
                                if (Message.Length > 0)
                                    Message = "Update failed: " + Message;
                                //if (!DiversityWorkbench.FormFunctions.SqlExecuteNonQuery(SQL))
                                //    Message = "Update failed";
                            }
                            catch (System.Exception ex)
                            {
                                Message = ex.Message;
                            }
                            break;
                    }
                }
            }
            catch (System.Exception ex) { }
            if (Message.Length > 0)
                this.ImportErrors.Add(iLine, Message);
        }

        /// <summary>
        /// Check all values of the PK columns excluding the identity columns
        /// </summary>
        private string NonIdentityPKisComplete
        {
            get
            {
                //bool IsComplete = true;
                foreach (string s in this.PrimaryKeyColumnList)
                {
                    if ((this.ColumnValueDictionary().ContainsKey(s)
                        && this.ColumnValueDictionary()[s].Length > 0)
                        || s == this.IdentityColumn())
                        continue;
                    else
                    {
                        this.FillColumnFromSuperiorTable();
                    }
                    if ((this._ColumnValueDictionary.ContainsKey(s)
                        && this.ColumnValueDictionary()[s].Length == 0)
                        || s != this.IdentityColumn())
                        return s + " is missing";
                }
                return "";
            }
        }

        /// <summary>
        /// Check if all values of the PK columns are filled
        /// </summary>
        private bool PKisComplete
        {
            get
            {
                foreach (string s in this.PrimaryKeyColumnList)
                {
                    if ((this.ColumnValueDictionary().ContainsKey(s)
                        && this.ColumnValueDictionary()[s].Length > 0))
                        continue;
                    else
                    {
                        this.FillColumnFromSuperiorTable();
                    }
                    if ((this._ColumnValueDictionary.ContainsKey(s)
                        && this.ColumnValueDictionary()[s].Length == 0))
                        return false;
                    else if (!this._ColumnValueDictionary.ContainsKey(s))
                        return false;
                }
                return true;
            }
        }

        #region Auxillary functions

        private string FormatedValue(string Column, string Value)
        {
            string Result = Value;
            try
            {
                string DataType = "";
                string ColumnKey = this.TableAlias + "." + Column + ".1";
                if (DiversityCollection.Import.ImportColumns.ContainsKey(ColumnKey))
                    DataType = DiversityCollection.Import.ImportColumns[ColumnKey].DataType();
                switch (DataType)
                {
                    case "":
                        break;
                    case "int":
                    case "float":
                    case "real":
                    case "bit":
                    case "tinyint":
                    case "smallint":
                    case "varbinary":
                    case "image":
                        break;
                    case "xml":
                    case "nvarchar":
                    case "char":
                    case "uniqueidentifier":
                    case "varchar":
                        if (Value.IndexOf("'") > -1)
                            Value = Value.Replace("'", "''");
                        if (!Value.StartsWith("'"))
                            Value = "'" + Value;
                        if (!Value.EndsWith("'"))
                            Value += "'";
                        else if (Value.EndsWith("''")
                            && !Value.EndsWith("'''"))
                            Value += "'";
                        break;
                    case "datetime":
                        if (!Value.StartsWith("CONVERT(DATETIME"))
                        {
                            System.DateTime DT;
                            if (System.DateTime.TryParse(Value, out DT))
                            {
                                Value = "CONVERT(DATETIME, " + DT.Year.ToString() + "-" + DT.Month.ToString() + "-" + DT.Day.ToString();
                                if (DT.Hour > 0)
                                    Value += " " + DT.Hour.ToString() + ":" + DT.Minute.ToString() + ":" + DT.Second.ToString();
                                Value += ", 102)";
                            }
                        }
                        break;
                    case "geography":
                    case "geometry":
                        break;
                }
            }
            catch (System.Exception ex)
            {
            }
            return Value;
        }

        private string ReplaceApostrophe(string SQL)
        {
            if (SQL.IndexOf("'") > -1)
            {
                SQL = SQL.Replace("'", "' + char(39) + '");
            }
            return SQL;
        }

        private string SqlExecuteScalar(string SqlCommand, System.Data.SqlClient.SqlConnection ImportConnection, System.Data.SqlClient.SqlTransaction ImportTransaction, ref string Message)
        {
            string Result = "";
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SqlCommand, ImportConnection);
            try
            {
                if (ImportConnection.State == System.Data.ConnectionState.Closed)
                    ImportConnection.Open();
                C.Transaction = ImportTransaction;
                Result = C.ExecuteScalar().ToString();
            }
            catch (System.Exception ex) { Message = ex.Message; }
            finally
            {
            }
            return Result;
        }

        private string SqlExecuteNonQuery(string SqlCommand, System.Data.SqlClient.SqlConnection ImportConnection, System.Data.SqlClient.SqlTransaction ImportTransaction)
        {
            //bool OK = true;
            string Error = "";
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SqlCommand, ImportConnection);
            try
            {
                if (ImportConnection.State == System.Data.ConnectionState.Closed)
                    ImportConnection.Open();
                C.Transaction = ImportTransaction;
                C.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            { Error = ex.Message; }
            finally
            {
            }
            return Error;
        }
        
        #endregion

        #endregion
        
        #region Informations about the table according to the settings
        
        /// <summary>
        /// Reset the table definitions dependent upon the settings in the import columns
        /// </summary>
        public void ResetTable()
        {
            this._ImportColumnDictionary = null;
        }

        /// <summary>
        /// Adding the relations for dataset where a table is not unique, e.g. many units
        /// </summary>
        /// <param name="Column">The name of the column</param>
        /// <param name="TableAlias">The alias of the table, the column relates to</param>
        public void AddParentTableNameForColumnDictionary(string Column, string TableAlias)
        {
            //if (this.ParentTableNameForColumnDictionary.ContainsKey(Column))
            //    this._ParentTableNameForColumnDictionary.Remove(Column);
            //this.ParentTableNameForColumnDictionary.Add(Column, TableAlias);
        }

        /// <summary>
        /// init the column dictionary of a table with the name of the column as key
        /// </summary>
        public void initImportColumnDictionary()
        {
            if (this._ImportColumnDictionary == null)
            {
                this._ImportColumnDictionary = new Dictionary<string, SortedList<int, Import_Column>>();
                foreach (string s in this.ImportColumnList)
                {
                    if (!this._ImportColumnDictionary.ContainsKey(s))
                        this._ImportColumnDictionary.Add(s, this.ImportColumns(s));
                }
            }
        }

        /// <summary>
        /// The columns of the table with the assigned columns in the file
        /// </summary>
        //private System.Collections.Generic.Dictionary<string, System.Collections.Generic.SortedList<int, DiversityCollection.Import_Column>> ImportColumnDictionary
        //{
        //    get
        //    {
        //        if (this._ImportColumnDictionary == null)
        //        {
        //            this._ImportColumnDictionary = new Dictionary<string, SortedList<int, Import_Column>>();
        //            foreach (string s in this.ColumnList)
        //            {
        //                if (!this._ImportColumnDictionary.ContainsKey(s))
        //                    this._ImportColumnDictionary.Add(s, this.ImportColumns(s));
        //            }
        //        }
        //        return this._ImportColumnDictionary;
        //    }
        //}

        /// <summary>
        /// The list of the import columns for a table column
        /// </summary>
        /// <param name="ColumnName">The name of the column</param>
        /// <returns>The list of Import_Column s sorted according to the sequence</returns>
        private System.Collections.Generic.SortedList<int, DiversityCollection.Import_Column> ImportColumns(string ColumnName)
        {
            System.Collections.Generic.SortedList<int, DiversityCollection.Import_Column> CC = new SortedList<int, Import_Column>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KV in DiversityCollection.Import.ImportColumns)
            {
                if (KV.Value.TableAlias == this._TableAlias && KV.Value.Column == ColumnName)
                {
                    if (!CC.ContainsKey(KV.Value.Sequence()) && KV.Value.IsSelected)
                    {
                        CC.Add(KV.Value.Sequence(), KV.Value);
                    }
                }
            }
            return CC;
        }

        private System.Collections.Generic.List<string> _ImportColumnList;
        /// <summary>
        /// The columns of a table as selected in the interface
        /// </summary>
        private System.Collections.Generic.List<string> ImportColumnList
        {
            get
            {
                if (this._ImportColumnList != null)
                    return this._ImportColumnList;
                this._ImportColumnList = new List<string>();
                try
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KV in DiversityCollection.Import.ImportColumns)
                    {

                        if (KV.Value.TableAlias == this.TableAlias
                            && KV.Value.IsSelected
                            && !this._ImportColumnList.Contains(KV.Value.Column))
                            this._ImportColumnList.Add(KV.Value.Column);
                    }
                }
                catch (System.Exception ex) { }
                return this._ImportColumnList;
            }
        }

        //private string ParentTableAlias
        //{
        //    get
        //    {
        //        if (this._ParentTableAlias == null)
        //        {

        //        }
        //        return this._ParentTableAlias;
        //    }
        //}
        
        #endregion

        #region Informations about the table as found in the database
        
        /// <summary>
        /// The list of the columns of the table
        /// </summary>
        public System.Collections.Generic.List<string> ColumnList
        {
            get
            {
                if (this._Columns == null)
                {
                    this._Columns = new List<string>();
                    System.Data.DataTable dt = new System.Data.DataTable();
                    string SQL = "SELECT COLUMN_NAME " +
                        "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                        "WHERE (TABLE_NAME = '" + this._TableName + "') " +
                        "AND COLUMN_NAME NOT LIKE 'xx%' " +
                        "AND COLUMN_NAME NOT LIKE 'Log%By' " +
                        "AND COLUMN_NAME NOT LIKE 'Log%When' " +
                        "AND COLUMN_NAME <> 'RowGUID' ";
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                        this._Columns.Add(R[0].ToString());
                }
                return this._Columns;
            }
        }

        public bool IsParentTableOfAttachmentTable
        {
            get
            {
                bool isParent = false;
                if (DiversityCollection.Import.AttachmentKeyImportColumn == null)
                    return false;

                System.Collections.Generic.Dictionary<string, string> DD = this.ColumnsWithForeignRelations(DiversityCollection.Import.AttachmentKeyImportColumn.Table);
                isParent = true;
                foreach(string P in this.PrimaryKeyColumnList)
                {
                    if (!DD.ContainsKey(P))
                    {
                        isParent = false;
                        break;
                    }
                    else
                    {
                        if (DD[P] != this.TableName)
                        {
                            isParent = false;
                            break;
                        }
                    }
                }

                return isParent;
            }
        }

        public System.Collections.Generic.List<string> DatabaseColumnList() { return this.ColumnList; }

        public string IdentityColumn()
        {
            if (this._IdentityColumn != null)
                return this._IdentityColumn;
            else
            {
                try
                {
                    if (this._ImportColumnDictionary == null)
                        this.initImportColumnDictionary();

                    foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.SortedList<int, DiversityCollection.Import_Column>> IC in this._ImportColumnDictionary)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<int, DiversityCollection.Import_Column> SL in IC.Value)
                        {
                            if (SL.Value.IsIdentityColumn())
                            {
                                this._IdentityColumn = SL.Value.Column;
                                break;
                            }
                        }
                        if (this._IdentityColumn != null)
                            break;
                    }
                    if (this._IdentityColumn == null)
                    {
                        foreach (string s in this.PrimaryKeyColumnList)
                        {
                            if (!this._ImportColumnDictionary.ContainsKey(s))
                            {
                                DiversityCollection.Import_Column IC = new Import_Column();
                                IC.Table = this._TableName;
                                IC.Column = s;
                                if (IC.IsIdentityColumn())
                                {
                                    this._IdentityColumn = s;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex) { }
                if (this._IdentityColumn == null)
                    this._IdentityColumn = "";
                return this._IdentityColumn;
            }
       }
        
        ///// <summary>
        ///// if the table contains an Identity column, the name of this column
        ///// </summary>
        //private string IdentityColumn
        //{
        //    get
        //    {
        //        if (this._IdentityColumn != null) return this._IdentityColumn;
        //        else
        //        {
        //            this._IdentityColumn = "";
        //            string SqlIdentiy = "select c.name from sys.columns c, sys.tables t where c.is_identity = 1 " +
        //                "and c.object_id = t.object_id and t.name = '" + this._TableName + "'";
        //            try
        //            {
        //                {
        //                    System.Data.SqlClient.SqlCommand Com = new System.Data.SqlClient.SqlCommand(SqlIdentiy, DiversityWorkbench.Settings.Connection);//, con);
        //                    this._IdentityColumn = Com.ExecuteScalar().ToString();
        //                }
        //            }
        //            catch { }
        //            return this._IdentityColumn;
        //        }
        //    }
        //}

        /// <summary>
        /// Parts of the primary key, that relate to another table
        /// </summary>
        //private System.Collections.Generic.List<string> ForeignPrimaryKeyColumns
        //{
        //    get
        //    {
        //        if (this._ForeignPrimaryKeyColumns != null) return this._ForeignPrimaryKeyColumns;
        //        else
        //        {
        //            this._ForeignPrimaryKeyColumns = new List<string>();
        //            string SqlPK = "SELECT COLUMN_NAME " +
        //                "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
        //                "WHERE (TABLE_NAME = '" + this._TableName + "') AND (EXISTS " +
        //                "(SELECT * " +
        //                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
        //                "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
        //                "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME))) " +
        //                "AND (EXISTS " +
        //                "(SELECT * " +
        //                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
        //                "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
        //                "WHERE (T.CONSTRAINT_TYPE = 'FOREIGN KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)));";
        //            try
        //            {
        //                System.Data.DataTable dtPK = new System.Data.DataTable();
        //                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SqlPK, DiversityWorkbench.Settings.ConnectionString);
        //                ad.Fill(dtPK);
        //                foreach (System.Data.DataRow R in dtPK.Rows)
        //                    this._ForeignPrimaryKeyColumns.Add(R[0].ToString());
        //            }
        //            catch { }
        //            return this._ForeignPrimaryKeyColumns;
        //        }
        //    }
        //}
        
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
                    System.Data.DataTable dt = new System.Data.DataTable();
                    string SQL = "SELECT COLUMN_NAME " +
                        "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                        "WHERE (TABLE_NAME = '" + this._TableName + "') AND (EXISTS " +
                        "(SELECT * " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                        "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                        "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    foreach(System.Data.DataRow R in dt.Rows)
                        this._PrimaryKeyColumns.Add(R[0].ToString());
                }
                return this._PrimaryKeyColumns;
            }
        }

        private System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> _PKColumnsWithMissingForeignRelations;
        public System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> FindPKColumnsWithMissingForeignRelations()
        {
            if (_PKColumnsWithMissingForeignRelations == null)
            {
                this._PKColumnsWithMissingForeignRelations = new Dictionary<string, Import_Column>();
                string SQL = "SELECT DISTINCT P.TABLE_NAME, FK.COLUMN_NAME " +
                   "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK, " +
                   "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK, " +
                   "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK, " +
                   "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF, " +
                   "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R, " +
                   "INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE P " +
                   "WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') " +
                   "AND TF.TABLE_NAME = '" + this._TableName + "' " +
                   "AND (TPK.CONSTRAINT_TYPE = 'PRIMARY KEY') " +
                   "AND TPK.TABLE_NAME = '" + this._TableName + "' " +
                   "AND PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME " +
                   "AND FK.CONSTRAINT_NAME = TF.CONSTRAINT_NAME  " +
                   "AND FK.COLUMN_NAME = PK.COLUMN_NAME " +
                   "AND R.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                   "AND P.CONSTRAINT_NAME = R.UNIQUE_CONSTRAINT_NAME";
                try
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        DiversityCollection.Import_Column IC = DiversityCollection.Import_Column.GetImportColumn("", this.TableName, this.TableAlias, R[1].ToString(), 1, null,
                            DiversityCollection.Import_Column.SourceType.Database, DiversityCollection.Import_Column.FixingType.None, DiversityCollection.Import_Column.EntryType.Database);// new Import_Column();
                        IC.Table = this.TableName;
                        IC.TableAlias = this.TableAlias;
                        IC.Column = R[1].ToString();
                        if (DiversityCollection.Import.ImportColumns.ContainsKey(IC.Key))
                            this._PKColumnsWithMissingForeignRelations.Add(R[1].ToString(), DiversityCollection.Import.ImportColumns[IC.Key]);
                        else
                        {
                            if (IC.ColumnDefault().Length > 0)
                                continue;
                            if (IC.IsPartOfPK() || !IC.IsNullable())
                                this._PKColumnsWithMissingForeignRelations.Add(R[1].ToString(), IC);
                        }
                    }
                }
                catch { }
            }
            return this._PKColumnsWithMissingForeignRelations;
        }


        private System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> _ColumnsWithForeignRelations;
        public System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> FindColumnsWithForeignRelations()
        {
            if (_ColumnsWithForeignRelations == null)
            {
                this._ColumnsWithForeignRelations = new Dictionary<string, Import_Column>();
                string SQL = "SELECT DISTINCT P.TABLE_NAME, FK.COLUMN_NAME " +
                   "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK, " +
                   "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK, " +
                   "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK, " +
                   "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF, " +
                   "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R, " +
                   "INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE P " +
                   "WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') " +
                   "AND TF.TABLE_NAME = '" + this._TableName + "' " +
                   "AND TPK.TABLE_NAME = '" + this._TableName + "' " +
                   "AND PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME " +
                   "AND FK.CONSTRAINT_NAME = TF.CONSTRAINT_NAME  " +
                   "AND FK.COLUMN_NAME = PK.COLUMN_NAME " +
                   "AND R.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                   "AND P.CONSTRAINT_NAME = R.UNIQUE_CONSTRAINT_NAME";
                try
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        DiversityCollection.Import_Column IC = DiversityCollection.Import_Column.GetImportColumn("", this.TableName, this.TableAlias, R[1].ToString(), 1, null
                            ,Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database);// new Import_Column();
                        IC.Table = this.TableName;
                        IC.TableAlias = this.TableAlias;
                        IC.Column = R[1].ToString();
                        if (!IC.IsSelected && IC.ParentTable().Length > 0 && DiversityCollection.Import.ImportTables.ContainsKey(IC.ParentTable()))
                            IC.IsSelected = true;
                        else
                        {
                        }
                        if (!IC.IsSelected && IC.ParentTable().Length == 0)
                            IC.IsSelected = true;
                        if (IC.ParentTable().Length > 0)
                        {
                            DiversityCollection.Import_Column ICparent = DiversityCollection.Import_Column.GetImportColumn("", IC.ParentTable(), IC.ParentTableAlias(), IC.ParentColumn(), 1, null
                                , IC.TypeOfSource, IC.TypeOfFixing, IC.TypeOfEntry);// new Import_Column();
                            ICparent.Table = IC.ParentTable();
                            ICparent.Column = IC.ParentColumn();
                            if (DiversityCollection.Import.ImportColumns.ContainsKey(ICparent.Key))
                                this._ColumnsWithForeignRelations.Add(R[1].ToString(), IC);
                        }
                   }
                }
                catch (System.Exception ex) { }
            }
            return this._ColumnsWithForeignRelations;
        }

        public System.Collections.Generic.Dictionary<string, string> ColumnsWithForeignRelations(string TableName)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            string SQL = "SELECT DISTINCT P.TABLE_NAME, FK.COLUMN_NAME " +
               "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK, " +
               "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK, " +
               "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK, " +
               "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF, " +
               "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R, " +
               "INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE P " +
               "WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') " +
               "AND TF.TABLE_NAME = '" + TableName + "' " +
               "AND TPK.TABLE_NAME = '" + TableName + "' " +
               "AND PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME " +
               "AND FK.CONSTRAINT_NAME = TF.CONSTRAINT_NAME  " +
               "AND FK.COLUMN_NAME = PK.COLUMN_NAME " +
               "AND R.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
               "AND P.CONSTRAINT_NAME = R.UNIQUE_CONSTRAINT_NAME";
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    DD.Add(R[1].ToString(), R[0].ToString());
                }
            }
            catch (System.Exception ex) { }
            return DD;
        }

        private string _ChildParentColumn;
        /// <summary>
        /// if the table contains a child parent relation, the name of the child column
        /// </summary>
        public string ChildParentColumn
        {
            get
            {
                if (this._ChildParentColumn != null) return this._ChildParentColumn;
                else
                {
                    this._ChildParentColumn = "";
                    string SQL = "SELECT Kc.COLUMN_NAME AS ChildColumn " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kc INNER JOIN " +
                        "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON Kc.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                        "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kp ON R.UNIQUE_CONSTRAINT_NAME = Kp.CONSTRAINT_NAME " +
                        "WHERE (Kc.TABLE_NAME = '" + this._TableName + "') AND (Kp.TABLE_NAME = '" + this._TableName + "')" +
                        "AND  Kc.COLUMN_NAME <> Kp.COLUMN_NAME AND Kc.ORDINAL_POSITION = Kp.ORDINAL_POSITION";
                    this._ChildParentColumn = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL).ToString();
                    return this._ChildParentColumn;
                }
            }
        }

        private string _ParentColumn;
        /// <summary>
        /// if the table contains a child parent relation, the name of the parent column
        /// </summary>
        public string ParentColumn
        {
            get
            {
                if (this._ParentColumn != null) return this._ParentColumn;
                else
                {
                    this._ParentColumn = "";
                    string SQL = "SELECT Kp.COLUMN_NAME AS ParentColumn " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kc INNER JOIN " +
                        "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON Kc.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                        "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kp ON R.UNIQUE_CONSTRAINT_NAME = Kp.CONSTRAINT_NAME " +
                        "WHERE (Kc.TABLE_NAME = '" + this._TableName + "') AND (Kp.TABLE_NAME = '" + this._TableName + "')" +
                        "AND  Kc.COLUMN_NAME <> Kp.COLUMN_NAME AND Kc.ORDINAL_POSITION = Kp.ORDINAL_POSITION";
                    this._ParentColumn = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
                    return this._ParentColumn;
                }
            }
        }

        //private System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> _ColumnsWithForeignRelations;
        //private void FindColumnsWithForeignRelations()
        //{
        //    if (_ColumnsWithForeignRelations == null)
        //    {
        //        this._ColumnsWithForeignRelations = new Dictionary<string, Import_Column>();
        //        string SQL = "SELECT DISTINCT P.TABLE_NAME, FK.COLUMN_NAME " +
        //           "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK, " +
        //           "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK, " +
        //           "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK, " +
        //           "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF, " +
        //           "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R, " +
        //           "INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE P " +
        //           "WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') " +
        //           "AND TF.TABLE_NAME = '" + this._TableName + "' " +
        //           "AND TPK.TABLE_NAME = '" + this._TableName + "' " +
        //           "AND PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME " +
        //           "AND FK.CONSTRAINT_NAME = TF.CONSTRAINT_NAME  " +
        //           "AND FK.COLUMN_NAME = PK.COLUMN_NAME " +
        //           "AND R.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
        //           "AND P.CONSTRAINT_NAME = R.UNIQUE_CONSTRAINT_NAME";
        //        try
        //        {
        //            System.Data.DataTable dt = new System.Data.DataTable();
        //            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //            ad.Fill(dt);
        //            foreach (System.Data.DataRow R in dt.Rows)
        //            {
        //                DiversityCollection.Import_Column IC = new Import_Column();
        //                IC.Table = this.TableName;
        //                IC.TableAlias = this.TableAlias;
        //                IC.Column = R[1].ToString();
        //                if (DiversityCollection.Import.ImportColumns.ContainsKey(IC.Key))
        //                    this._ColumnsWithForeignRelations.Add(R[1].ToString(), DiversityCollection.Import.ImportColumns[IC.Key]);
        //                else
        //                {
        //                    if (IC.ColumnDefault().Length > 0)
        //                        continue;
        //                    if (IC.IsPartOfPK() || !IC.IsNullable())
        //                        this._ColumnsWithForeignRelations.Add(R[1].ToString(), IC);
        //                }
        //            }
        //        }
        //        catch { }
        //    }
        //}

        /// <summary>
        /// The name of the parent table as defined in the database (not the alias!)
        /// </summary>
        //private System.Collections.Generic.Dictionary<string, string> ParentTableNameForColumnDictionary
        //{
        //    get
        //    {
        //        if (_ParentTableNameForColumnDictionary != null)
        //            return this._ParentTableNameForColumnDictionary;

        //        this._ParentTableNameForColumnDictionary = new Dictionary<string, string>();
        //        string SQL = "SELECT DISTINCT P.TABLE_NAME, FK.COLUMN_NAME " +
        //            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK, " +
        //            "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK, " +
        //            "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK, " +
        //            "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF, " +
        //            "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R, " +
        //            "INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE P " +
        //            "WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') " +
        //            "AND TF.TABLE_NAME = '" + this._TableName + "' " +
        //            //"AND (TPK.CONSTRAINT_TYPE = 'PRIMARY KEY') " +
        //            "AND TPK.TABLE_NAME = '" + this._TableName + "' " +
        //            "AND PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME " +
        //            "AND FK.CONSTRAINT_NAME = TF.CONSTRAINT_NAME  " +
        //            "AND FK.COLUMN_NAME = PK.COLUMN_NAME " +
        //            "AND R.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
        //            "AND P.CONSTRAINT_NAME = R.UNIQUE_CONSTRAINT_NAME";
        //        try
        //        {
        //            System.Data.DataTable dt = new System.Data.DataTable();
        //            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //            ad.Fill(dt);
        //            foreach (System.Data.DataRow R in dt.Rows)
        //            {
        //                this._ParentTableNameForColumnDictionary.Add(R[1].ToString(), R[0].ToString());
        //            }
        //        }
        //        catch { }
        //        return this._ParentTableNameForColumnDictionary;
        //    }
        //}

        /// <summary>
        /// The names of the columns in the related tables
        /// </summary>
        //private System.Collections.Generic.Dictionary<string, string> ColumnNameInRelatedTable
        //{
        //    get
        //    {
        //        if (this._ColumnNameInRelatedTable != null)
        //            return this._ColumnNameInRelatedTable;

        //        this._ColumnNameInRelatedTable = new Dictionary<string, string>();
        //        string SQL = "SELECT DISTINCT K.COLUMN_NAME, C.COLUMN_NAME " +
        //            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kref   " +
        //            "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K " +
        //            "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON K.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
        //            "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS C ON R.UNIQUE_CONSTRAINT_NAME = C.CONSTRAINT_NAME ON  " +
        //            "Kref.TABLE_NAME = C.TABLE_NAME AND Kref.ORDINAL_POSITION = K.ORDINAL_POSITION " +
        //            "WHERE  Kref.CONSTRAINT_NAME = C.CONSTRAINT_NAME  AND C.COLUMN_NAME = Kref.COLUMN_NAME " +
        //            "AND  (K.TABLE_NAME = '" + this.TableName + "') ";
        //        try
        //        {
        //            System.Data.DataTable dt = new System.Data.DataTable();
        //            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //            ad.Fill(dt);
        //            foreach (System.Data.DataRow R in dt.Rows)
        //            {
        //                this._ColumnNameInRelatedTable.Add(R[0].ToString(), R[1].ToString());
        //            }
        //        }
        //        catch { }
        //        return this._ColumnNameInRelatedTable;
        //    }
        //}

        #endregion

    }
}
