using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection
{
    public interface iImportColumnControl
    {
        void setColumnInSourceFile(int? ColumnInSourceFile);
        int? getColumnInSourceFile();
        string Error();
        void setTitle(string Title);
        void setInterface();
        void setValue(string Value);
        Import getImport();
        DiversityCollection.Import_Column ImportColumn();
        void AddMultiColumn(DiversityCollection.Import_Column C);
    }

    public class Import_Column
    {
        #region Parameter, Properties and Interface

        #region DisplayTitle
        
        private string _DisplayTitle = "";

        /// <summary>
        /// the title as shown in a user interface
        /// </summary>
        public string getDisplayTitle()
        {
            string Entity = this.Table + "." + this.Column;
            this._DisplayTitle = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation(Entity));
            if (this._DisplayTitle.Length == 0)
            {
                string Title = "";
                try
                {
                    for (int i = 0; i < this.Column.Length; i++)
                    {
                        // adding a space in front of a upper case letter
                        if (this.Column[i].ToString().ToLower() != this.Column[i].ToString()
                            && Title.Length > 0)
                            Title += " ";

                        // removing the space if the next letter is UPPER CASE as well

                        // adding a space in front of a number
                        int I = 0;
                        if (int.TryParse(this.Column[i].ToString(), out I)
                            && Title.Length > 0
                            && !Title.EndsWith(" "))
                            Title += " ";

                        // leaving the first letter upper case
                        if (Title.Length == 0)
                            Title += this.Column[i].ToString().ToUpper();
                        else
                        {
                            if (this.Column.Length > i + 1
                                && this.Column[i].ToString().ToLower() != this.Column[i].ToString()
                                && this.Column[i + 1].ToString().ToLower() != this.Column[i + 1].ToString())
                            {
                                if (i > 0
                                && this.Column[i].ToString().ToLower() != this.Column[i].ToString()
                                && this.Column[i - 1].ToString().ToLower() != this.Column[i - 1].ToString())
                                    Title = Title.Trim();
                                Title += this.Column[i].ToString();
                            }
                            else if (i > 0
                                && this.Column[i].ToString().ToLower() != this.Column[i].ToString()
                                && this.Column[i - 1].ToString().ToLower() != this.Column[i - 1].ToString())
                                Title = Title.Trim() + this.Column[i].ToString();
                            else
                                Title += this.Column[i].ToString().ToLower();
                        }
                    }
                }
                catch (System.Exception ex) { }
                Title = Title.Trim();
                this._DisplayTitle = Title;
            }
            return _DisplayTitle;
        }

        public void setDisplayTitle(string Title)
        {
            this._DisplayTitle = Title;
            this.ImportColumnControl.setTitle(Title);
        }
        
        #endregion

        #region Lookup table
        
        private System.Data.DataTable _LookUpTable;
        private string _DisplayColumn;
        public string DisplayColumn
        {
            get { return _DisplayColumn; }
            set { _DisplayColumn = value; }
        }
        private string _ValueColumn;

        public string ValueColumn
        {
            get { return _ValueColumn; }
            set { _ValueColumn = value; }
        }
        
        #endregion

        /// <summary>
        /// the key for the import step
        /// </summary>
        public string StepKey;

        private bool _IsSelected = false;
        /// <summary>
        /// if the column is selected for the import
        /// </summary>
        public bool IsSelected
        {
            get { return this._IsSelected; }
            set
            {
                this._IsSelected = value;
            }
        }

        private bool _MustSelect = false;
        /// <summary>
        /// if a column must have a value, e.g. Project
        /// </summary>
        public bool MustSelect
        {
            get
            {
                return this._MustSelect;
            }
            set
            {
                this._MustSelect = value;
                if (this._MustSelect)
                {
                    this.IsSelected = true;
                    //if (this.TypeOfSource == SourceType.Any)
                    //    this.TypeOfSource = SourceType.File;
                }
            }
        }

        private bool _IsDecisionColumn = false;

        public bool IsDecisionColumn
        {
            get { return _IsDecisionColumn; }
            set { _IsDecisionColumn = value; }
        }

        private bool _CanSetDecisionColumn = true;

        public bool CanSetDecisionColumn
        {
            get { return _CanSetDecisionColumn; }
            set { _CanSetDecisionColumn = value; }
        }

        /// <summary>
        /// The target table where the data should be imported
        /// </summary>
        public string Table;
        /// <summary>
        /// the alias for a table if several dataset should be imported in the same table, e.g. several collectors
        /// </summary>
        public string TableAlias;
        //private string _Column;
        /// <summary>
        /// The name of the column in the database
        /// </summary>
        public string Column;
        //{
        //    get { return this._Column; }
        //    set
        //    {
        //        this._Column = value;
        //        this.initDatabaseProperties();
        //    }
        //}

        /// <summary>
        /// If the column is used to match the new data with data in the database
        /// </summary>
        //public bool isAttachmentColumn; - unsinn

        private bool? _IsPartOfPK;
        public bool IsPartOfPK()
        {
            if (this._IsPartOfPK == null)
            {
                string SQL = "SELECT Count(*) " +
                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE K " +
                    ", INFORMATION_SCHEMA.TABLE_CONSTRAINTS T " +
                    "where K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                    "and T.CONSTRAINT_TYPE = 'PRIMARY KEY' " +
                    "and T.TABLE_NAME = '" + this.Table + "' " +
                    "and K.COLUMN_NAME = '" + this.Column + "'";
                int i;
                if (int.TryParse(DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL), out i))
                {
                    if (i == 1) this._IsPartOfPK = true;
                    else this._IsPartOfPK = false;
                }
            }
            return (bool)this._IsPartOfPK;
        }

        private bool? _IsNullable;
        public bool IsNullable()
        {
            if (this._IsNullable == null)
            {
                string SQL = "SELECT IS_NULLABLE " +
                    "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                    "WHERE (TABLE_NAME = '" + this.Table + "') AND (COLUMN_NAME = '" + this.Column + "')  ";
                try
                {
                    string Result = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result.ToUpper() == "YES")
                        this._IsNullable = true;
                    else this._IsNullable = false;
                }
                catch { this._IsNullable = true; }
            }
            return (bool)_IsNullable;
        }

        private bool? _IsIdentityColumn;
        /// <summary>
        /// if this column is an Identity column
        /// </summary>
        public bool IsIdentityColumn()
        {
            if (this._IsIdentityColumn != null)
                return (bool)this._IsIdentityColumn;
            else
            {
                this._IsIdentityColumn = false;
                string SqlIdentiy = "select COUNT(*) from sys.columns c, sys.tables t where c.is_identity = 1 " +
                    "and c.object_id = t.object_id and t.name = '" + this.Table + "' " +
                    "and c.name = '" + this.Column + "'";
                try
                {
                    {
                        int i;
                        if (int.TryParse(DiversityWorkbench.FormFunctions.SqlExecuteScalar(SqlIdentiy), out i))
                        {
                            if (i == 1)
                                this._IsIdentityColumn = true;
                            else this._IsIdentityColumn = false;
                        }
                    }
                }
                catch { this._IsIdentityColumn = false; }
            }
            return (bool)this._IsIdentityColumn;
        }

        #region ForeignKeyRelation
        
        private string _ParentTable;
        /// <summary>
        /// If the column relates to a superior table via a foreign key, the name of the table
        /// </summary>
        public string ParentTable()
        {
            if (this.StepKey.Length > 0
                && DiversityCollection.Import.ImportSteps.ContainsKey(this.StepKey))
            {
                if (DiversityCollection.Import.ImportSteps[this.StepKey].SuperiorImportStep != null)
                {
                }
            }
            if (this._ParentTable == null)
            {
                string SQL = "SELECT DISTINCT P.TABLE_NAME " +
                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK,  " +
                    "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK,  " +
                    "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK, " +
                    "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF, " +
                    "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R, " +
                    "INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE P " +
                    "WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') " +
                    "AND TF.TABLE_NAME = '" + this.Table + "' " +
                    "AND TPK.TABLE_NAME = '" + this.Table + "' " +
                    "AND PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME " +
                    "AND FK.CONSTRAINT_NAME = TF.CONSTRAINT_NAME  " +
                    "AND FK.COLUMN_NAME = PK.COLUMN_NAME " +
                    "AND R.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                    "AND P.CONSTRAINT_NAME = R.UNIQUE_CONSTRAINT_NAME " +
                    "AND FK.COLUMN_NAME = '" + this.Column + "'";
                this._ParentTable = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
            }
            return this._ParentTable;
        }

        private string _ParentTableAlias;
        /// <summary>
        /// If the column relates to a superior table via a foreign key, the alias of the table
        /// must be set when creating dependent ImportColumns, otherwise returns default
        /// </summary>
        public string ParentTableAlias()
        {
                if (this._ParentTableAlias == null)
                    this._ParentTableAlias = this.ParentTable();
                return this._ParentTableAlias;
            //set { this._ParentTableAlias = value; }
        }
        public void ParentTableAlias(string ParentTableAlias) { this._ParentTableAlias = ParentTableAlias; }

        private string _ParentColumn;
        /// <summary>
        /// If the column relates to a superior table via a foreign key, the name of the column in the superior table
        /// </summary>
        public string ParentColumn()
        {
            if (this.ParentTable().Length == 0)
                this._ParentColumn = "";
            if (this._ParentColumn == null)
            {
                string SQL = "SELECT C.COLUMN_NAME AS RelatedColumn " +
                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kref INNER JOIN  " +
                    "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN  " +
                    "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON K.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                    "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS C ON R.UNIQUE_CONSTRAINT_NAME = C.CONSTRAINT_NAME ON  " +
                    "Kref.TABLE_NAME = C.TABLE_NAME AND Kref.ORDINAL_POSITION = K.ORDINAL_POSITION " +
                    "WHERE  Kref.CONSTRAINT_NAME = C.CONSTRAINT_NAME  " +
                    "AND C.COLUMN_NAME = Kref.COLUMN_NAME " +
                    "AND  (K.TABLE_NAME = '" + this.Table + "') " +
                    "AND (C.TABLE_NAME = '" + this.ParentTable() + "') " +
                    "AND (K.COLUMN_NAME = '" + this.Column + "');";
                this._ParentColumn = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
            }
            return this._ParentColumn;
        }

        private string _ParentImportColumnKey;
        public string ParentImportColumnKey()
        {
            if (this._ParentImportColumnKey == null)
                this._ParentImportColumnKey = this.ParentTableAlias() + "." + this.ParentColumn() + ".1";
            return this._ParentImportColumnKey;
        }
        public void ParentImportColumnKey(string ParentImportColumnKey) { this._ParentImportColumnKey = ParentImportColumnKey; }

        private bool _IsInternalRelation;
        public bool IsInternalRelation
        {
            get
            {
                try
                {
                    if (this.ParentTable() == this.Table)
                        this._IsInternalRelation = true;
                    else this._IsInternalRelation = false;
                    return this._IsInternalRelation;
                }
                catch (System.Exception ex)
                {
                    return false;
                }
            }
        }
       
        #endregion

        /// <summary>
        /// The datatype as defined in the database
        /// </summary>
        private string _ColumnDefault;
        public string ColumnDefault()
        {
            if (this._ColumnDefault == null)
            {
                string SQL = "SELECT COLUMN_DEFAULT " +
                    "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                    "WHERE (TABLE_NAME = '" + this.Table + "') AND (COLUMN_NAME = '" + this.Column + "')  ";
                try
                {
                    this._ColumnDefault = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
                }
                catch { this._ColumnDefault = ""; }
            }
            return _ColumnDefault;
        }

        /// <summary>
        /// The datatype as defined in the database
        /// </summary>
        private string _DataType;
        public string DataType()
        {
            if (this._DataType == null)
            {
                string SQL = "SELECT DATA_TYPE " +
                    "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                    "WHERE (TABLE_NAME = '" + this.Table + "') AND (COLUMN_NAME = '" + this.Column + "')  ";
                try
                {
                    this._DataType = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
                }
                catch { this._DataType = null; }
            }
            return _DataType;
        }

        /// <summary>
        /// The datatype as defined in the database
        /// </summary>
        private bool _IsTextDataType;
        public bool IsTextDataType()
        {
            switch (this.DataType())
            {
                case "char":
                case "ntext":
                case "nvarchar":
                case "varchar":
                    this._IsTextDataType = true;
                    break;
                default:
                    this._IsTextDataType = false;
                    break;
            }
            //if (this._DataType == null)
            //{
            //    if (this.DataType() == "char" || this.DataType() ==  "ntext" || this.DataType() ==  "nvarchar" || this.DataType() ==  "varchar")
            //        this._IsTextDataType = true;
            //    else
            //        this._IsTextDataType = false;
            //}
            return _IsTextDataType;
        }  

        /// <summary>
        /// an alternative column for the values in case the analysis method returns an error
        /// </summary>
        public string AlternativeColumn;
        /// <summary>
        /// testing if a value should be imported in the alternative column
        /// </summary>
        /// <returns></returns>
        public virtual bool ImportIntoAlternativeColumn()
        { return false; }


        /// <summary>
        /// the analysis method for columns that should be converted or analysed
        /// </summary>
        //public string AnalysisMethod;

        /// <summary>
        /// the formats available for a value in the source file, e.g. for dates
        /// </summary>
        public System.Collections.Generic.List<string> Formats;
        /// <summary>
        /// the format of a value, e.g. for dates: year/month/day etc.
        /// as defined in Formats
        /// </summary>
        public string Format;


        #region Transformation of the values

        /// <summary>
        /// If the values in the data can be transformed by a regular expression
        /// </summary>
        public bool CanBeTransformed = true;

        /// <summary>
        /// The strings for splitting the field in the file
        /// </summary>
        public System.Collections.Generic.List<string> Splitters = new List<string>();

        /// <summary>
        /// The position of a splitted field that should be imported in the DB
        /// </summary>
        public int SplitPosition = 1;
        public void setSplitColumn(int Position, System.Collections.Generic.List<string> Splitters)
        {
            //this.CanBeTransformed = CanBeTransformed;
            this.SplitPosition = Position;
            this.Splitters = Splitters;
            this.ImportColumnControl.setInterface();
        }

        /// <summary>
        /// A regular expression pattern for the transformation of the content in the file
        /// </summary>
        public string RegularExpressionPattern;
        /// <summary>
        /// The string by which the pattern found via the regular expression should be replaced
        /// </summary>
        public string RegularExpressionReplacement;

        /// <summary>
        /// a dictionary translating values from the file into values of e.g. a enum table like CollectionTypeStatus_Enum
        /// </summary>
        private System.Collections.Generic.Dictionary<string, string> _TranslationDictionary;
        public System.Collections.Generic.Dictionary<string, string> TranslationDictionary
        {
            get
            {
                if (this._TranslationDictionary == null)
                    this._TranslationDictionary = new Dictionary<string, string>();
                return this._TranslationDictionary;
            }
            set
            {
                this._TranslationDictionary = value;
            }
        }
        /// <summary>
        /// If a translation is possible, e.g. for month where Feb. or II is translated to 2
        /// </summary>
        //public bool TranslationPossible = false;

        #endregion

        /// <summary>
        /// a preset value, e.g. the LocalitySystemID in the table CollectionEventLocalisation
        /// </summary>
        public string PresetValue;
        /// <summary>
        /// the name of the column of the preset value, e.g. LocalitySystemID
        /// </summary>
        public string PresetValueColumn;
        /// <summary>
        /// the value for a file column if one value for all entries should be used
        /// </summary>
        public string Value;
        /// <summary>
        /// if the value should be kept for the import schema
        /// null = No fixing possible
        /// false = not fixed
        /// true = fixed
        /// </summary>
        public bool ValueIsFixed;


        /// <summary>
        /// if several columns can be used for one column in the database
        /// </summary>
        public bool MultiColumn = false;
        private int _Sequence = 1;
        /// <summary>
        /// if several file columns should be used for one column in the database, the sequence of the columns
        /// </summary>
        public int Sequence() { return this._Sequence; }
        public void setSequence(int Sequence)
        {
            this._Sequence = Sequence;
            if (this._Sequence > 1)
                this.MultiColumn = true;
        }
        /// <summary>
        /// if several columns should be used for one column in the database, the separator to the previous column
        /// </summary>
        public string Separator = " ";

        public DiversityCollection.iImportColumnControl ImportColumnControl;

        private int? _ColumnInSourceFile;
        /// <summary>
        /// the column in the source file. If null this is to be done
        /// </summary>
        public int? ColumnInSourceFile
        {
            get { return this._ColumnInSourceFile; }
            set
            {
                try
                {
                    int? oldValue = this._ColumnInSourceFile;
                    int? newValue = value;
                    this._ColumnInSourceFile = value;
                    if (this.ImportColumnControl != null)
                    {
                        if (oldValue != null || newValue != null)
                        {
                            if (oldValue != null && (oldValue != newValue
                                || (oldValue != null && newValue == null)))
                            {
                                this.ImportColumnControl.getImport().ImportInterface.setColumnHeaeder((int)oldValue);
                            }
                            if (newValue != null && (oldValue != newValue
                                || (oldValue == null && newValue != null)))
                            {
                                this.ImportColumnControl.getImport().ImportInterface.setColumnHeaeder((int)newValue);
                            }
                        }
                    }
                }
                catch (System.Exception ex) { }
            }
        }

        /// <summary>
        /// An error, e.g. a missing assignement to a column in the source file
        /// </summary>
        public string Error;

        /// <summary>
        /// Testing if values can be imported, e.g. a column in the file is selected, the values are valid etc.
        /// </summary>
        /// <returns>The message containing details of the error</returns>
        public virtual string getError()
        {
            return "";
        }

        /// <summary>
        /// The unique key for the column, composed of Tablealias + Column + Sequence
        /// </summary>
        public string Key
        {
            get
            {
                string K = this.TableAlias;
                if (this.TableAlias == "Identification") 
                { }
                if (K == null || K.Length == 0)
                    K = this.Table;
                K += "." + this.Column;
                K += "." + this.Sequence().ToString();
                return K;
            }
        }

        /// <summary>
        /// The list of values as found in the file
        /// </summary>
        private System.Collections.Generic.List<string> _ValueList;
        public System.Collections.Generic.List<string> ValueList
        {
            get
            {
                if (this._ValueList == null || this._ValueList.Count == 0)
                {
                    this._ValueList = new List<string>();
                    if (this.ImportColumnControl == null)
                    {
                        if (DiversityCollection.Import.ImportColumnControls.ContainsKey(this.Key))
                            this.ImportColumnControl = DiversityCollection.Import.ImportColumnControls[this.Key];
                    }
                    if (this.ImportColumnControl != null)
                    {
                        for (int i = this.ImportColumnControl.getImport().ImportInterface.FirstLineForImport() - 1; i < this.ImportColumnControl.getImport().ImportInterface.LastLineForImport(); i++)
                        {
                            string strValue = "";
                            if (this.ImportColumnControl.getImport().ImportInterface.Grid().Rows[i].Cells[(int)this.ColumnInSourceFile].Value != null)
                                strValue = this.ImportColumnControl.getImport().ImportInterface.Grid().Rows[i].Cells[(int)this.ColumnInSourceFile].Value.ToString();
                            if (strValue.Length > 0 && !_ValueList.Contains(strValue))
                                _ValueList.Add(strValue);
                        }
                    }
                }
                return this._ValueList;
            }
        }

        public string TransformedValue(int Row, System.Windows.Forms.DataGridView Grid)
        {
            string Value = "";
            if (Row < 0)
                return "";
            if (Grid.Rows.Count < Row)
                return "";
            if (this.MultiColumn)
            {
                if (this.Sequence() == 1)
                {
                    bool isLastMulitColumn = false;
                    int iLastMultiColumn = 2;
                    System.Collections.Generic.SortedDictionary<int, DiversityCollection.Import_Column> MultiColumns = new SortedDictionary<int, Import_Column>();
                    while (!isLastMulitColumn)
                    {
                        string Key = this.Key.Substring(0, this.Key.Length - 1) + iLastMultiColumn.ToString();
                        if (DiversityCollection.Import.ImportColumns.ContainsKey(Key))
                        {
                            MultiColumns.Add((int)DiversityCollection.Import.ImportColumns[Key].Sequence(), DiversityCollection.Import.ImportColumns[Key]);
                        }
                        else isLastMulitColumn = true;
                        iLastMultiColumn++;
                    }
                    if (Grid.Rows[Row].Cells[(int)this.ColumnInSourceFile].Value != null)
                        Value = this.TransformedValue(Grid.Rows[Row].Cells[(int)this.ColumnInSourceFile].Value.ToString());
                    if (this.Separator.Trim().Length > 0)
                        Value = this.Separator + Value;
                    foreach (System.Collections.Generic.KeyValuePair<int, DiversityCollection.Import_Column> KV in MultiColumns)
                    {
                        if (KV.Value.ColumnInSourceFile != null
                            && Grid.Rows[Row].Cells[(int)KV.Value.ColumnInSourceFile].Value != null)
                        {
                            string ToTransform = Grid.Rows[Row].Cells[(int)KV.Value.ColumnInSourceFile].Value.ToString();
                            Value += KV.Value.Separator + KV.Value.TransformedValue(ToTransform);
                        }
                    }
                }
                else
                {
                    Value = this.Separator + Value;
                }
            }
            else
            {
                if (Grid.Rows[Row].Cells[(int)this.ColumnInSourceFile].Value != null)
                    Value = this.TransformedValue(Grid.Rows[Row].Cells[(int)this.ColumnInSourceFile].Value.ToString());
            }
            return Value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns>Value after transformation according to the setting</returns>
        public string TransformedValue(string Value)
        {
            string Result = Value;
            string[] VV = new string[] { };
            if (this.Splitters != null && this.Splitters.Count > 0)
            {
                int ss = this.Splitters.Count;
                string[] Separators;
                Separators = new string[this.Splitters.Count];
                for (int i = 0; i < this.Splitters.Count; i++)
                    Separators[i] = this.Splitters[i];
                VV = Value.Split(Separators, StringSplitOptions.None);
                if (VV.Length > this.SplitPosition - 1)
                {
                    Result = VV[this.SplitPosition - 1];
                }
                else
                    Result = "";
            }
            if (this.RegularExpressionPattern != null &&
                this.RegularExpressionPattern.Length > 0 &&
                this.RegularExpressionReplacement != null &&
                this.RegularExpressionReplacement.Length > 0)
            {
                Result = System.Text.RegularExpressions.Regex.Replace(Result, this.RegularExpressionPattern, this.RegularExpressionReplacement);
            }
            if (this.TranslationDictionary != null
                && this.TranslationDictionary.Count > 0
                && this.TranslationDictionary.ContainsKey(Result))
                Result = this.TranslationDictionary[Result];
            //if (this.Separator != null && this.Separator.Length > 0)
            //    Result = Separator + Result;
            return Result;
        }

        public DiversityCollection.Import_Column.EntryType TypeOfEntry = EntryType.Text;
        public DiversityCollection.Import_Column.FixingType TypeOfFixing;
        public DiversityCollection.Import_Column.SourceType TypeOfSource = SourceType.Any;
        /// <summary>
        /// The type of the column entry, e.g. if the user can select from a list or enter a text instead of a column in the file
        /// </summary>
        public enum EntryType { Boolean, Date, Time, Text, ListAndText, MandatoryList, Database }
        /// <summary>
        /// The type in which the value can be fixed. None = No fixing is possible, Import = the value can be set for the current import, Schema = the value may be written in the schema file
        /// </summary>
        public enum FixingType { None, Import, Schema }
        /// <summary>
        /// The type of the source. 
        /// Any = FileOrInterface, 
        /// File = Values only from the file, 
        /// Interface = Values only from the interface, 
        /// Database = created while importing the data
        /// </summary>
        public enum SourceType { Any, File, Interface, Database }

        #endregion

        #region Construction
        
        public Import_Column()
        {
        }

        private Import_Column(string StepKey, string Table, string TableAlias, string Column
            , DiversityCollection.iImportColumnControl ImportColumnControl)
        {
            this.StepKey = StepKey;
            this.Table = Table;
            this.TableAlias = TableAlias;
            this.Column = Column;
            this.ImportColumnControl = ImportColumnControl;
            if (!Import.ImportColumns.ContainsKey(this.Key))
                Import.ImportColumns.Add(Key, this);
            if (!Import.ImportColumnControls.ContainsKey(this.Key))
                Import.ImportColumnControls.Add(this.Key, this.ImportColumnControl);
        }

        private Import_Column(string StepKey, string Table, string Column
            , DiversityCollection.iImportColumnControl ImportColumnControl):
            this(StepKey, Table, Table, Column, ImportColumnControl)
        {
            this.StepKey = StepKey;
            this.Table = Table;
            this.TableAlias = Table;
            this.Column = Column;
            this.ImportColumnControl = ImportColumnControl;
            if (!Import.ImportColumnControls.ContainsKey(this.Key))
                Import.ImportColumnControls.Add(this.Key, this.ImportColumnControl);
        }

        private Import_Column(string StepKey, string Table, string TableAlias, string Column
            , DiversityCollection.iImportColumnControl ImportColumnControl
            , DiversityCollection.Import_Column.SourceType SourceType
            , DiversityCollection.Import_Column.FixingType FixingType
            , DiversityCollection.Import_Column.EntryType EntryType) :
            this(StepKey, Table, TableAlias, Column, ImportColumnControl)
        {
            this.StepKey = StepKey;
            this.StepKey = StepKey;
            this.Table = Table;
            this.TableAlias = TableAlias;
            this.Column = Column;
            this.TypeOfEntry = EntryType;
            this.TypeOfFixing = FixingType;
            this.TypeOfSource = SourceType;
            if (!Import.ImportColumns.ContainsKey(this.Key))
                Import.ImportColumns.Add(Key, this);
            if (ImportColumnControl != null)
            {
                this.ImportColumnControl = ImportColumnControl;
                if (!Import.ImportColumnControls.ContainsKey(this.Key))
                    Import.ImportColumnControls.Add(this.Key, this.ImportColumnControl);
            }
        }

        public static Import_Column GetImportColumn(string StepKey, string Table, string TableAlias, string Column, int Sequence
            , DiversityCollection.iImportColumnControl ImportColumnControl
            , DiversityCollection.Import_Column.SourceType SourceType
            , DiversityCollection.Import_Column.FixingType FixingType
            , DiversityCollection.Import_Column.EntryType EntryType)
        {
            Import_Column IC = new Import_Column(StepKey, Table, TableAlias, Column, ImportColumnControl, SourceType, FixingType, EntryType);
            IC.setSequence(Sequence);
            if (!DiversityCollection.Import.ImportColumns.ContainsKey(IC.Key))
            {
                //IC.Sequence = Position;
                DiversityCollection.Import.ImportColumns.Add(IC.Key, IC);
            }






            return DiversityCollection.Import.ImportColumns[IC.Key];
        }

        public static Import_Column GetImportColumn(string StepKey, string Table, string TableAlias, string Column
            , DiversityCollection.iImportColumnControl ImportColumnControl)
        {
            Import_Column IC = new Import_Column(StepKey, Table, TableAlias, Column, ImportColumnControl);
            if (!DiversityCollection.Import.ImportColumns.ContainsKey(IC.Key))
            {
                IC.setSequence(1);
                DiversityCollection.Import.ImportColumns.Add(IC.Key, IC);
            }
            return DiversityCollection.Import.ImportColumns[IC.Key];
        }

        public static Import_Column GetImportColumn(string StepKey, string Table, string Column 
            , DiversityCollection.iImportColumnControl ImportColumnControl)
        {
            Import_Column IC = new Import_Column(StepKey, Table, Column, ImportColumnControl);
            if (!DiversityCollection.Import.ImportColumns.ContainsKey(IC.Key))
            {
                IC.setSequence(1);
                DiversityCollection.Import.ImportColumns.Add(IC.Key, IC);
            }
            return DiversityCollection.Import.ImportColumns[IC.Key];
        }

        public static Import_Column GetImportColumn(string StepKey, string Table, string Column, int Sequence
            , DiversityCollection.iImportColumnControl ImportColumnControl)
        {
            Import_Column IC = new Import_Column(StepKey, Table, Column, ImportColumnControl);
            IC.setSequence(Sequence);
            if (!DiversityCollection.Import.ImportColumns.ContainsKey(IC.Key))
            {
                //IC.Sequence = Position;
                DiversityCollection.Import.ImportColumns.Add(IC.Key, IC);
            }
            return DiversityCollection.Import.ImportColumns[IC.Key];
        }

        #endregion

        private void initDatabaseProperties()
        {
        }
        
        #region Properties

        public void setLookupTable(System.Data.DataTable LookupTable, string DisplayColumn, string ValueColumn)
        {
            try
            {
                this._LookUpTable = LookupTable;
                this._DisplayColumn = DisplayColumn;
                this._ValueColumn = ValueColumn;
            }
            catch (System.Exception ex) { }
        }
        
        public System.Data.DataTable getLookUpTable()
        {
            if (this._LookUpTable == null ||
                this._LookUpTable.Rows.Count == 0)
            {
                try
                {
                    string SQL = "SELECT C.TABLE_NAME, C.COLUMN_NAME " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN  " +
                        "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON K.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN  " +
                        "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS C ON R.UNIQUE_CONSTRAINT_NAME = C.CONSTRAINT_NAME " +
                        "WHERE (K.TABLE_NAME = '" + this.Table + "') " +
                        "AND K.COLUMN_NAME = '" + this.Column + "'";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        this._LookUpTable = new System.Data.DataTable();
                        SQL = "SELECT " + dt.Rows[0][1].ToString() + " FROM " + dt.Rows[0][0].ToString();
                        ad.SelectCommand.CommandText = SQL;
                        ad.Fill(this._LookUpTable);
                    }

                }
                catch (System.Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }

            return _LookUpTable;
        }

        #endregion
    }
}
