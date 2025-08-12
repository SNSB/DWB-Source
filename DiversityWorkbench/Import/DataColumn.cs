using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Import
{
    public interface iDataColumnInterface
    {
        void initControl();
    }

    public interface iDataColumn
    {
        void setCopyPrevious(bool Copy);
        void setIsDecisive(bool IsDecisive);
        void setColumnInFile(int ColumnInFile);
        void setPrefix(string Prefix);
        void setPostfix(string Postfix);
        System.Collections.Generic.List<DiversityWorkbench.Import.Transformation> TransformationList();
        System.Data.DataTable getMandatoryList();
        string getMandatoryListDisplayColumn();
        string getMandatoryListValueColumn();
        string TransformedValue();
        int? PositionInFile();
    }

    // Toni: Delegates for prepare processing
    /// <summary>
    /// Prepare processing delegate
    /// </summary>
    /// <param name="Table">Table name</param>
    /// <param name="Column">Column name</param>
    /// <param name="Value">Column value for input</param>
    /// <param name="ParentKey">Parent table key</param>
    /// <param name="ImportConnection">Database connection for import</param>
    /// <param name="ImportTransaction">Database transaction for import</param>
    /// <param name="Message">Error message</param>
    /// <returns>Column value for output</returns>
    public delegate string PrepareProcessingDelegate(string Table, string Column, string Value, string ParentKey, Microsoft.Data.SqlClient.SqlConnection ImportConnection, Microsoft.Data.SqlClient.SqlTransaction ImportTransaction, ref string Message);

    /// <summary>
    /// Check if value exists and get value delegate
    /// </summary>
    /// <param name="Table">Table name</param>
    /// <param name="Column">Column name</param>
    /// <param name="Value">Column value for input and output</param>
    /// <param name="ParentKey">Parent table key</param>
    /// <param name="Message">Error message</param>
    /// <returns>'true' if value is present</returns>
    public delegate bool CheckGetValueDelegate(string Table, string Column, ref string Value, string ParentKey, ref string Message);

    /// <summary>
    /// Check if is possible delegate
    /// </summary>
    /// <param name="Table">Table name</param>
    /// <param name="Column">Column name</param>
    /// <param name="Value">Column value for input</param>
    /// <param name="ParentKey">Parent table key</param>
    /// <returns>'true' if insert is possible</returns>
    public delegate bool CheckInsertDelegate(string Table, string Column, string Value, string ParentKey);

    public class DataColumn : DiversityWorkbench.Import.iDataColumn
    {

        #region Interface

        private DiversityWorkbench.Import.iDataColumnInterface _iDataColumnInterface;
        public DiversityWorkbench.Import.iDataColumnInterface iDataColumnInterface
        {
            get
            {
                return this._iDataColumnInterface;
            }
            set
            {
                this._iDataColumnInterface = value;
            }
        }

        #endregion

        #region Interface functions

        public void setCopyPrevious(bool Copy)
        {
            this._CopyPrevious = Copy;
        }

        public void setIsDecisive(bool IsDecisive)
        {
            this.IsDecisive = IsDecisive;
        }

        public void setColumnInFile(int ColumnInFile)
        {
            this.FileColumn = ColumnInFile;
        }

        public void setPrefix(string Prefix)
        {
            this.Prefix = Prefix;
        }

        public void setPostfix(string Postfix)
        {
            this.Postfix = Postfix;
        }

        public System.Collections.Generic.List<DiversityWorkbench.Import.Transformation> TransformationList()
        {
            return this.Transformations;
        }

        public System.Data.DataTable getMandatoryList()
        {
            return this.MandatoryList;
        }

        public string getMandatoryListDisplayColumn()
        {
            return this.MandatoryListDisplayColumn;
        }

        public string getMandatoryListValueColumn()
        {
            return this.MandatoryListValueColumn;
        }

        public int? PositionInFile() { return this.FileColumn; }

        #endregion

        #region Properties for handling data

        private bool _IsDecisive = false;
        /// <summary>
        /// if in the file any column of this type contains values, the data will be imported resp. updated
        /// </summary>
        public bool IsDecisive
        {
            get { if (this._IsDecisive == null) this._IsDecisive = false; return _IsDecisive; }
            set { _IsDecisive = value; }
        }

        private bool _CopyPrevious = false;
        /// <summary>
        /// if in the file the value is missing, the value of the previous line will be used
        /// </summary>
        public bool CopyPrevious
        {
            get { if (this._CopyPrevious == null) this._CopyPrevious = false; return _CopyPrevious; }
            set { _CopyPrevious = value; }
        }

        private bool _CompareKey = false;
        /// <summary>
        /// the columns of this type will be used to compare values with the contents in the database to decide
        /// if data will be imported or updated if the primary key alone is not enough for this decision
        /// this may be the case if e.g. in the table Identification the content of the column TaxonomicName should be compared
        /// </summary>
        public bool CompareKey
        {
            get { if (this._CompareKey == null) this._CompareKey = false; return _CompareKey; }
            set { _CompareKey = value; }
        }

        private bool _IsSelected = false;
        /// <summary>
        /// if the column is selected for the import
        /// </summary>
        public bool IsSelected
        {
            get
            {
                if (this._IsSelected == null)
                    this._IsSelected = false;
                if (!this.IsNullable
                    && (this.ColumnDefault == null || this.ColumnDefault.Length == 0)
                    && this._DataTable.MergeHandling == DiversityWorkbench.Import.DataTable.Merging.Insert)
                    this._IsSelected = true;
                return this._IsSelected;
            }
            set
            {
                this._IsSelected = value;
            }
        }

        private bool _SelectParallelForeignRelationTableAlias = false;
        /// <summary>
        /// If the ForeignRelationTableAlias must be selected from parallel tables
        /// e.g. for the column SpecimenPartID in table CollectionSpecimenImage where the ForeignRelationTable CollectionSpecimenPart may exist in parallels
        /// then the user must choose which of the tables should be the source from which the content of the parent table must be copied
        /// </summary>
        public bool SelectParallelForeignRelationTableAlias
        {
            get { return _SelectParallelForeignRelationTableAlias; }
            set { _SelectParallelForeignRelationTableAlias = value; }
        }

        // Markus: For display text in interface etc.
        private string _SourceFunctionDisplayText = "";
        /// <summary>
        /// if the value may be generated by a function e.g. in the database, the text shown in the interface
        /// </summary>
        public string SourceFunctionDisplayText
        {
            get { return _SourceFunctionDisplayText; }
            set { _SourceFunctionDisplayText = value; }
        }

        public enum RetrievalType { Default, FunctionInDatabase, RunBeforeFunctionInDatabase, IDorIDviaTextFromFile }
        private RetrievalType _DataRetrievalType = RetrievalType.Default;
        /// <summary>
        /// The way data are retrieved during the import
        /// </summary>
        public RetrievalType DataRetrievalType
        {
            get { return _DataRetrievalType; }
            set
            {
                _DataRetrievalType = value;
            }
        }

        // If the option to enter a value for all entries or generate an entry is available
        //public bool ForAllIsAvailable = true;

        // Toni: Prepare insert delegate 
        private PrepareProcessingDelegate _PrepareInsertFunction = null;

        /// <summary>
        /// Prepare insert function may only be set
        /// </summary>
        public PrepareProcessingDelegate PrepareInsertFunction
        {
            //get { return _PrepareInsertFunction; }
            set { _PrepareInsertFunction = value; }
        }

        /// <summary>
        /// Indicates if a prepare insert delegate has been specified
        /// </summary>
        public bool PrepareInsertDefined
        {
            get { return _PrepareInsertFunction != null; }
        }

        // Toni: Prepare update delegate 
        private PrepareProcessingDelegate _PrepareUpdateFunction = null;

        /// <summary>
        /// Prepare update function may only be set
        /// </summary>
        public PrepareProcessingDelegate PrepareUpdateFunction
        {
            //get { return _PrepareUpdateFunction; }
            set { _PrepareUpdateFunction = value; }
        }

        /// <summary>
        /// Indicates if a prepare update delegate has been specified
        /// </summary>
        public bool PrepareUpdateDefined
        {
            get { return _PrepareUpdateFunction != null; }
        }

        // Toni: Check and get value delegate 
        private CheckGetValueDelegate _CheckGetValueFunction = null;

        /// <summary>
        ///  Check and get value function may only be set
        /// </summary>
        public CheckGetValueDelegate CheckGetValueFunction
        {
            //get { return _CheckGetValueFunction; }
            set { _CheckGetValueFunction = value; }
        }


        // Markus 16.5.23: Ob auf Werte aus der Datenquelle eingeschraenkt werden muss
        private bool _RestrictToDatasourceValues = false;

        public bool RestrictToDatasourceValues
        {
            set { if (this.SqlDataSource != null && this.SqlDataSource.Length > 0) _RestrictToDatasourceValues = value; }
            get => _RestrictToDatasourceValues;
        }

        /// <summary>
        /// Indicates if a check and get value delegate has been specified
        /// </summary>
        public bool CheckGetValueDefined
        {
            get { return _CheckGetValueFunction != null; }
        }

        // Toni: Check insert delegate 
        private CheckInsertDelegate _CheckInsertFunction = null;

        /// <summary>
        ///  Check insert function may only be set
        /// </summary>
        public CheckInsertDelegate CheckInsertFunction
        {
            //get { return _CheckInsertFunction; }
            set { _CheckInsertFunction = value; }
        }

        /// <summary>
        /// Indicates if a check insert delegate has been specified
        /// </summary>
        public bool CheckInsertDefined
        {
            get { return _CheckInsertFunction != null; }
        }

        // Toni: Multi column processing flag 
        /// <summary>
        /// Enables multi column processing if delegates for prepare insert/update and check/get value
        /// are defined, even if column type does not meet the regular conditions. 
        /// </summary>
        private bool _MultiColumnProcessing = false;

        public bool MultiColumnProcessing
        {
            get { return _MultiColumnProcessing; }
            set { _MultiColumnProcessing = value; }
        }

        // Markus 2023-12-13: Fuer Spalten in die nichts importiert werden soll weil sie e.g. von einem Trigger gefüllt werden
        // Beispiel: Die Spalte AgentName in DA. User hat versucht diese zu fuellen und sich gewundert warum nichts ankommt
        private bool _AllowInsert = true;

        /// <summary>
        /// If the insert is allowed
        /// </summary>
        public bool AllowInsert { get => _AllowInsert; set => _AllowInsert = value; }

        #endregion

        #region Attachment

        private string _ValueAttachmentParent;
        /// <summary>
        /// If the attachment relates to a child parent relation within the table, 
        /// e.g. in Table Analysis in DiversityCollection where the column AnalysisParentID relates to AnalysisID
        /// here the attachment should be done via the column DisplayText while this column should be imported as well
        /// so the values of the parent are stored here and used to retrieve the AnalysisParentID from the database
        /// </summary>
        public string ValueAttachmentParent
        {
            get { return _ValueAttachmentParent; }
            set
            {
                _ValueAttachmentParent = value;

            }
        }

        // unten unsinn - wird in AttachmentColumn gespeichert

        //private string _ValueAttachmentParentColumnName;
        ///// <summary>
        ///// If the attachment relates to a child parent relation within the table, 
        ///// e.g. in Table Analysis in DiversityCollection where the column AnalysisParentID relates to AnalysisID
        ///// here the attachment should be done via the column DisplayText while this column should be imported as well
        ///// The name of the attachment column (e.g. DisplayText) is stored here
        ///// </summary>
        //public string ValueAttachmentParentColumnName
        //{
        //    get { return _ValueAttachmentParentColumnName; }
        //    set { _ValueAttachmentParentColumnName = value; }
        //}


        private bool _IsParentAttachmentColumn;
        /// <summary>
        /// if the column is used to hold the values of the attachment parent
        /// </summary>
        public bool IsParentAttachmentColumn
        {
            get { return _IsParentAttachmentColumn; }
            set { _IsParentAttachmentColumn = value; }
        }

        #endregion

        #region Value handling

        private string _ValueInDatabase;
        /// <summary>
        /// if a dataset is present, the current value of the column as found in the database
        /// </summary>
        public string ValueInDatabase
        {
            get { return _ValueInDatabase; }
            set { _ValueInDatabase = value; }
        }

        // Toni: Processed value provided by prepare insert/update or get key value delegate
        private string _ProcessedValue = null;
        /// <summary>
        /// Processed value after call of prepare insert/update or get key value delegate, can only be reset to null from extern
        /// </summary>
        public string ProcessedValue
        {
            get { return _ProcessedValue; }
            set
            {
                _ProcessedValue = value;

                if (_Value == null)
                    _Value = "";

                // setting the values in dependent tables
                if (_ProcessedValue != null && _ProcessedValue.Length > 0 && this._DataTable.PrimaryKeyColumnList.Contains(this.ColumnName))
                {
                    foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                    {
                        if (DiversityWorkbench.Import.Import.Tables[T].ParentTableAlias == this._DataTable.TableAlias)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> C in DiversityWorkbench.Import.Import.Tables[T].DataColumns)
                            {
                                if (C.Value.ForeignRelationColumn != null
                                    && C.Value.ForeignRelationColumn == this.ColumnName
                                    && C.Value.ForeignRelationTableAlias != null
                                    && C.Value.ForeignRelationTableAlias == this.DataTable.TableAlias)
                                {
                                    C.Value.Value = _ProcessedValue;
                                    C.Value.TypeOfSource = SourceType.ParentTable;
                                    C.Value.IsSelected = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private string _Value;
        /// <summary>
        /// the current value as found in the file, derived from the database or preset
        /// </summary>
        public string Value
        {
            get
            {
                //if ((_Value == null || _Value.Length == 0) &&
                //    this._DefaultValueQuery != null)
                //    return this._DefaultValueQuery;
                // Toni: If prepare insert or prepare update has already been called take this value instead
                if ((this.PrepareInsertDefined || this.PrepareUpdateDefined) && this.ProcessedValue != null)
                    return ProcessedValue;
                return _Value;
            }
            set
            {
                // Toni: Initialization for processed values;
                this.ProcessedValue = null;

                // Markus 2015-12-10: init OriginalValue
                this._OriginalValue = null;

                if (!this.ValueIsPreset)
                {
                    _Value = value;

                    // Markus 2015-12-10: Save original value here if transformations change the value
                    if (this.Transformations.Count > 0)
                    {
                        this._OriginalValue = value;
                        this._Value = this.TransformedValue();
                    }

                    // setting the previous value
                    if (value != null && value.Length > 0)
                        this.PreviousValue = value;
                    if (value.Length == 0 && this.CopyPrevious && this.PreviousValue != null && this.PreviousValue.Length > 0)
                        _Value = this.PreviousValue;

                    // setting the values in dependent tables
                    if (_Value.Length > 0 && this._DataTable.PrimaryKeyColumnList.Contains(this.ColumnName))
                    {
                        foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                        {
                            if (DiversityWorkbench.Import.Import.Tables[T].ParentTableAlias == this._DataTable.TableAlias)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> C in DiversityWorkbench.Import.Import.Tables[T].DataColumns)
                                {
                                    if (C.Value.ForeignRelationColumn != null
                                        && C.Value.ForeignRelationColumn == this.ColumnName
                                        && C.Value.ForeignRelationTableAlias != null
                                        && C.Value.ForeignRelationTableAlias == this.DataTable.TableAlias)
                                    {
                                        // Markus 18.9.2015: hier wurde der nicht transformierte Wert in die abhaengige Tabelle geschrieben
                                        if (this.Transformations.Count > 0)
                                            C.Value.Value = this.TransformedValue();
                                        else
                                            C.Value.Value = _Value;

                                        C.Value.TypeOfSource = SourceType.ParentTable;
                                        C.Value.IsSelected = true;
                                    }
                                    //else if (C.Value.ForeignRelationColumn != null
                                    //    && C.Value.ForeignRelationColumn == this.ColumnName
                                    //    && C.Value.ForeignRelationTableAlias == null)
                                    //{
                                    //    if (DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.Tables[T].ParentTableAlias].DataColumns.ContainsKey(this.ColumnName))
                                    //    {
                                    //        if (DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.Tables[T].ParentTableAlias].DataColumns[this.ColumnName].ForeignRelationTableAlias
                                    //    }
                                    //}
                                }
                            }
                            else if (DiversityWorkbench.Import.Import.Tables[T].SecondParentTableAlias != null &&
                                DiversityWorkbench.Import.Import.Tables[T].SecondParentTableAlias == this._DataTable.TableAlias)
                            {
                            }
                        }
                    }
                    else if (_Value.Length > 0)
                    {
                    }
                }
                else if (this.ValueIsPreset && this.Value == null)
                {
                    _Value = value;
                }
            }
        }

        // Toni: Provide unconverted value for supply of data column user control
        // Markus 2015-12-10: Save original value here if transformations change the value
        // Markus 2016-04-04: Original value may have been null. Return value only if there transfromations
        private string _OriginalValue;
        public string OriginalValue
        {
            //get { return _Value; }
            get
            {
                if (_OriginalValue == null)// && this._Transformations.Count > 0) 
                    return _Value;
                else return _OriginalValue;
            }
        }

        private string _PreviousValue;
        /// <summary>
        /// the value of the previous line in the file
        /// </summary>
        public string PreviousValue
        {
            get { return _PreviousValue; }
            set { if (value.Length > 0) _PreviousValue = value; }
        }

        private bool _ValueIsPreset;
        /// <summary>
        /// If the value for this column is preset
        /// </summary>
        public bool ValueIsPreset
        {
            get { return _ValueIsPreset; }
            set { _ValueIsPreset = value; }
        }

        private System.Collections.Generic.List<DiversityWorkbench.Import.Transformation> _Transformations;
        /// <summary>
        /// The list of transformations applied on the values in this column
        /// </summary>
        public System.Collections.Generic.List<DiversityWorkbench.Import.Transformation> Transformations
        {
            get { if (this._Transformations == null) this._Transformations = new List<Transformation>(); return _Transformations; }
            set { _Transformations = value; }
        }

        private string _Prefix;
        /// <summary>
        /// A text placed before the value in the file (after an optional transformation)
        /// </summary>
        public string Prefix
        {
            get 
            { /*if (this._Prefix == null) this._Prefix = "";*/
                // Markus 24.11.2023 - Bugfix falls ' in string - geht nicht - laeuft in Schleife - muss vor Ort korrigiert werden
                //if (!string.IsNullOrEmpty(_Prefix))  { _Prefix = _Prefix.Replace("'", "''"); }
                return _Prefix;  
            }
            set { _Prefix = value; }
        }

        private string _Postfix;
        /// <summary>
        /// A text placed after the value in the file (after an optional transformation)
        /// </summary>
        public string Postfix
        {
            get 
            { /*if (this._Postfix == null) this._Postfix = "";*/
                // Markus 24.11.2023 - Bugfix falls ' in string - geht nicht - laeuft in Schleife - muss vor Ort korrigiert werden
                //if (!string.IsNullOrEmpty(_Postfix)) { _Postfix = _Postfix.Replace("'", "''"); }
                return _Postfix; 
            }
            set { _Postfix = value; }
        }

        private int? _FileColumn;
        /// <summary>
        /// The position in the file
        /// </summary>
        public int? FileColumn
        {
            get { return _FileColumn; }
            set { _FileColumn = value; }
        }

        public enum SourceType { NotDecided, File, Interface, Preset, Database, ParentTable }
        private SourceType _TypeOfSource;
        /// <summary>
        /// The type of source of this column
        /// </summary>
        public SourceType TypeOfSource
        {
            get
            {
                if (this._TypeOfSource == null)
                    this._TypeOfSource = SourceType.NotDecided;
                if (this.IsIdentity)
                    this._TypeOfSource = SourceType.Database;
                if (this.IsIdentityInParent)
                    this._TypeOfSource = SourceType.Database;
                if (this.ValueIsPreset && this.Value != null && this.Value.Length > 0)
                    this._TypeOfSource = SourceType.Preset;
                return _TypeOfSource;
            }
            set { _TypeOfSource = value; }
        }

        /// <summary>
        /// The transformed value of the column
        /// </summary>
        /// <returns>The value after the transformation</returns>
        public string TransformedValue()
        {
            if (this._ValueIsPreset)
                return this.DataTable.PresetValues[this.ColumnName]; //this._Value;
            if (this._TypeOfSource == SourceType.Interface)
            {
                if (this.SourceFunctionDisplayText.Length > 0 && this.PrepareInsertDefined && this._Value == null)
                {
                    this._Value = "";
                    //this.PrepareInsertFunction(this._DataTable.TableName, this._ColumnName, "", "", DiversityWorkbench.Settings.Connection, null, ref Message);
                }
                return this._Value;
            }
            else if (this._TypeOfSource == SourceType.File)
            {
                if (this.SourceFunctionDisplayText.Length > 0 && this.PrepareInsertDefined && this.DataRetrievalType != RetrievalType.Default && this.DataType == "int")
                {
                    //this._Value = "";
                    //this.PrepareInsertFunction(this._DataTable.TableName, this._ColumnName, "", "", DiversityWorkbench.Settings.Connection, null, ref Message);
                }
            }
            string Result = "";
            // Toni: If no value is stored and preprocessing is defined, read file value 
            try
            {
                bool readFromFile = (_Value == null || _Value.Length == 0) && (this.PrepareInsertDefined || this.PrepareUpdateDefined);
                if ((this._TypeOfSource == SourceType.File || readFromFile) && this._FileColumn != null && DiversityWorkbench.Import.Import.LineValuesFromFile().ContainsKey((int)this._FileColumn))
                {
                    Result = DiversityWorkbench.Import.Import.LineValuesFromFile()[(int)this._FileColumn];
                    if (this._CopyPrevious)
                    {
                        if (Result.Length > 0)
                            this.PreviousValue = Result;
                        Result = this.PreviousValue;
                    }
                    if (this.Transformations.Count > 0)
                    {
                        Result = DiversityWorkbench.Import.DataColumn.TransformedValue(Result, this.Transformations, DiversityWorkbench.Import.Import.LineValuesFromFile());
                        // Markus 9.5.2017: Calculation with values in database
                        if (this._DataTable.MergeHandling == DiversityWorkbench.Import.DataTable.Merging.Update &&
                            this.Transformations.Count == 1 &&
                            this.Transformations[0].TypeOfTransformation == Transformation.TransformationType.Calculation &&
                            this.Transformations[0].CalculationApplyOnData &&
                            this.Transformations[0].CalculationApplyOnDataOperator != null)
                        {
                            Result = this._ColumnName + " " + this.Transformations[0].CalculationApplyOnDataOperator + " " + Result;
                        }
                    }
                    if (Result.Length > 0)
                        Result = this.Prefix + Result + this.Postfix;
                    if (this.IsMultiColumn && this.MultiColumns.Count > 0)
                    {
                        foreach (DiversityWorkbench.Import.ColumnMulti M in this.MultiColumns)
                        {
                            Result += M.TransformedValue();
                        }
                    }
                }
                else if ((this._TypeOfSource == SourceType.Database || this._TypeOfSource == SourceType.ParentTable || this.TypeOfSource == SourceType.NotDecided) && this._Value != null && this._Value.Length > 0)
                {
                    Result = this._Value;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            //if (Result.IndexOf("'") > -1)
            //    Result = Result.Replace("'", "''");
            return Result;
        }

        /// <summary>
        /// Transformed value
        /// </summary>
        /// <param name="Value">the string that should be transformed</param>
        /// <param name="Transformations">The list of transformations that should be applied on the string</param>
        /// <returns>The transformed string</returns>
        public static string TransformedValue(string Value,
            System.Collections.Generic.List<DiversityWorkbench.Import.Transformation> Transformations,
            System.Collections.Generic.Dictionary<int, string> CurrentLineValues)
        {
            string Result = Value;
            foreach (DiversityWorkbench.Import.Transformation T in Transformations)
            {
                switch (T.TypeOfTransformation)
                {
                    case Transformation.TransformationType.RegularExpression:
                        Result = System.Text.RegularExpressions.Regex.Replace(Result, T.RegularExpression, T.RegularExpressionReplacement);
                        break;
                    case Transformation.TransformationType.Replacement:
                        if (T.Replace != null && T.ReplaceWith != null)
                            Result = Result.Replace(T.Replace, T.ReplaceWith);
                        break;
                    case Transformation.TransformationType.Split:
                        string[] VV = new string[] { };
                        int ss = T.SplitterList.Count;
                        string[] Separators;
                        Separators = new string[T.SplitterList.Count];
                        for (int i = 0; i < T.SplitterList.Count; i++)
                            Separators[i] = T.SplitterList[i];
                        VV = Result.Split(Separators, StringSplitOptions.None);
                        if (T.ReverseSequence)
                        {
                            if (VV.Length > T.SplitterPosition - 1 && T.SplitterPosition > 0)
                            {
                                Result = VV[VV.Length - T.SplitterPosition];
                            }
                            else
                                Result = "";
                        }
                        else
                        {
                            if (VV.Length > T.SplitterPosition - 1 && T.SplitterPosition > 0)
                            {
                                Result = VV[T.SplitterPosition - 1];
                            }
                            else
                                Result = "";
                        }
                        break;
                    case Transformation.TransformationType.Translation:
                        if (Result != null
                            && T.TranslationDictionary != null
                            && T.TranslationDictionary.Count > 0
                            && T.TranslationDictionary.ContainsKey(Result))
                            Result = T.TranslationDictionary[Result];
                        else if (Result != null
                            && T.TranslationDictionarySourceTable != null
                            && T.TranslationDictionarySourceTable.Count > 0
                            && T.TranslationDictionarySourceTable.ContainsKey(Result))
                            Result = T.TranslationDictionarySourceTable[Result];
                        else if (Result == null)
                            Result = "";
                        break;
                    case Transformation.TransformationType.Calculation:
                        if (T.CalculationFactor != null &&
                            T.CalculationFactor.Length > 0 &&
                            T.CalulationOperator != null &&
                            T.CalulationOperator.Length > 0)
                        {
                            double ResultAsDouble;
                            double FResult;
                            if (double.TryParse(Result, out ResultAsDouble) && double.TryParse(T.CalculationFactor.Replace(" ", ""), out FResult))
                            {
                                double CResult = 0;
                                if (T.CalulationOperator == "*" || T.CalulationOperator == "/")
                                    CResult = 1;
                                try
                                {
                                    bool PerformCaluculation = true;
                                    if (T.CalculationConditionValue != null &&
                                        T.CalculationConditionValue.Length > 0 &&
                                        T.CalculationConditionOperator != null &&
                                        T.CalculationConditionOperator.Length > 0)
                                    {
                                        double ConditionValue;
                                        if (double.TryParse(T.CalculationConditionValue.Replace(" ", ""), out ConditionValue))
                                        {
                                            PerformCaluculation = false;
                                            switch (T.CalculationConditionOperator)
                                            {
                                                case "<":
                                                    if (ResultAsDouble < ConditionValue)
                                                        PerformCaluculation = true;
                                                    break;
                                                case "<=":
                                                    if (ResultAsDouble <= ConditionValue)
                                                        PerformCaluculation = true;
                                                    break;
                                                case "=":
                                                    if (ResultAsDouble == ConditionValue)
                                                        PerformCaluculation = true;
                                                    break;
                                                case ">=":
                                                    if (ResultAsDouble >= ConditionValue)
                                                        PerformCaluculation = true;
                                                    break;
                                                case ">":
                                                    if (ResultAsDouble > ConditionValue)
                                                        PerformCaluculation = true;
                                                    break;
                                                case "≠":
                                                    if (ResultAsDouble != ConditionValue)
                                                        PerformCaluculation = true;
                                                    break;
                                            }
                                        }
                                    }
                                    if (PerformCaluculation)
                                    {
                                        switch (T.CalulationOperator)
                                        {
                                            case "+":
                                                CResult = ResultAsDouble + FResult;
                                                break;
                                            case "-":
                                                CResult = ResultAsDouble - FResult;
                                                break;
                                            case "*":
                                                CResult = ResultAsDouble * FResult;
                                                break;
                                            case "x":
                                                CResult = ResultAsDouble * FResult;
                                                break;
                                            case "/":
                                                CResult = ResultAsDouble / FResult;
                                                break;
                                        }
                                        Result = CResult.ToString();
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                }
                            }
                        }
                        break;
                    case Transformation.TransformationType.Filter:
                        bool FilterPassed = false;
                        if (T.FilterConditionsOperator == Transformation.FilterConditionsOperators.And)
                            FilterPassed = true;

                        foreach (DiversityWorkbench.Import.TransformationFilter TF in T.FilterConditions)
                        {
                            string FilterString = "";
                            if (CurrentLineValues.ContainsKey(TF.FilterColumn))// || T.FilterColumn == )
                            {
                                System.Collections.Generic.Dictionary<int, string> LL = DiversityWorkbench.Import.Import.LineValuesFromFile();
                                if (LL.ContainsKey(TF.FilterColumn))
                                    FilterString = LL[TF.FilterColumn];
                                else if (CurrentLineValues.ContainsKey(TF.FilterColumn))
                                    FilterString = CurrentLineValues[TF.FilterColumn];
                                switch (TF.FilterOperator)
                                {
                                    case "=":
                                        if (FilterString != TF.Filter)
                                        {
                                            if (T.FilterConditionsOperator == Transformation.FilterConditionsOperators.And)
                                            {
                                                FilterPassed = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (T.FilterConditionsOperator == Transformation.FilterConditionsOperators.Or)
                                            {
                                                FilterPassed = true;
                                                break;
                                            }
                                        }
                                        break;
                                    case "≠":
                                        if (FilterString == TF.Filter)
                                        {
                                            if (T.FilterConditionsOperator == Transformation.FilterConditionsOperators.And)
                                            {
                                                FilterPassed = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (T.FilterConditionsOperator == Transformation.FilterConditionsOperators.Or)
                                            {
                                                FilterPassed = true;
                                                break;
                                            }
                                        }

                                        break;

                                }
                            }
                        }
                        if (FilterPassed)
                        {
                            if (T.FilterUseFixedValue)
                                Result = T.FilterFixedValue;
                        }
                        else
                            Result = "";
                        break;
                    case Transformation.TransformationType.Color:
                        try
                        {
                            System.Drawing.Color color = System.Drawing.Color.Transparent;
                            switch (T.ColorFrom)
                            {
                                case Transformation.ColorFormat.RGBdec:
                                    string[] RGB = Result.Split(new char[] { ',', '/', '-' });
                                    int R = int.Parse(RGB[0].Trim());
                                    int G = int.Parse(RGB[1].Trim());
                                    int B = int.Parse(RGB[2].Trim());
                                    color = System.Drawing.Color.FromArgb(R, G, B);
                                    break;
                                case Transformation.ColorFormat.ARGBint:
                                    color = System.Drawing.Color.FromArgb(int.Parse(Result));
                                    break;
                                case Transformation.ColorFormat.RGBhex:
                                    int iR = Convert.ToInt16(Result.Substring(1, 2), 16);
                                    int iG = Convert.ToInt16(Result.Substring(3, 2), 16);
                                    int iB = Convert.ToInt16(Result.Substring(5, 2), 16);
                                    color = System.Drawing.Color.FromArgb(iR, iG, iB);
                                    break;
                            }
                            if (color != System.Drawing.Color.Transparent)
                            {
                                switch (T.ColorInto)
                                {
                                    case Transformation.ColorFormat.ARGBint:
                                        Result = color.ToArgb().ToString();
                                        break;
                                    case Transformation.ColorFormat.RGBhex:
                                        byte[] vs = { color.R, color.G, color.B };
                                        Result = "#" + BitConverter.ToString(vs).ToString().Replace("-", "");
                                        break;
                                    case Transformation.ColorFormat.RGBdec:
                                        Result = Convert.ToInt16(color.R).ToString() + "," + Convert.ToInt16(color.G).ToString() + "," + Convert.ToInt16(color.B).ToString();
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                        break;
                }
            }
            return Result;
        }

        /// <summary>
        /// The value of the column, prepared for direct use in an SQL statement
        /// </summary>
        /// <returns></returns>
        public string ValueFormatedForSQL()
        {
            // Toni: Take processed value if present
            string Result = this.ProcessedValue != null ? this.ProcessedValue : this.TransformedValue();

            // Markus 17.12.2018 - enable removal of values
            if (Result == "NULL")
                return Result;

            string ResultForTest = Result;

            try
            {
                // Markus 9.5.2017: Apply calculation on contents in database
                if (this._DataTable.MergeHandling == DiversityWorkbench.Import.DataTable.Merging.Update &&
                    this.Transformations.Count == 1 &&
                    this.Transformations[0].TypeOfTransformation == Transformation.TransformationType.Calculation &&
                    this.Transformations[0].CalculationApplyOnData &&
                    this.Transformations[0].CalculationApplyOnDataOperator != null &&
                    this.Transformations[0].CalculationApplyOnDataOperator.Length > 0)
                {
                    ResultForTest = Result.Substring(this._ColumnName.Length + 2 + this.Transformations[0].CalculationApplyOnDataOperator.Length).Trim();
                }

                switch (this.DataType.ToLower())
                {
                    case "":
                        break;
                    case "bit":
                        Boolean B;
                        if (!Boolean.TryParse(ResultForTest, out B) && ResultForTest != "1" && ResultForTest != "0")
                        {
                            this._DataTable.AddMessage("Column " + this.ColumnName + ": The value  '" + ResultForTest + "'  is not boolean");
                        }
                        break;
                    case "int":
                        int i;
                        if (this.PrepareInsertDefined && this.IsMultiColumn && this.DataRetrievalType == RetrievalType.IDorIDviaTextFromFile && this.SourceFunctionDisplayText.Length > 0)
                        {
                        }
                        else if (!int.TryParse(ResultForTest, out i))
                        {
                            this._DataTable.AddMessage("Column " + this.ColumnName + ": The value  '" + ResultForTest + "'  is no number");
                        }
                        break;
                    case "tinyint":
                        Byte By;
                        if (!Byte.TryParse(ResultForTest, out By))
                        {
                            this._DataTable.AddMessage("Column " + this.ColumnName + ": The value  '" + ResultForTest + "'  is no number");
                        }
                        break;
                    case "smallint":
                        Int16 i16;
                        if (!Int16.TryParse(ResultForTest, out i16))
                        {
                            this._DataTable.AddMessage("Column " + this.ColumnName + ": The value  '" + ResultForTest + "'  is no number");
                        }
                        break;
                    case "float":
                        double d;
                        try
                        {
                            // Toni 20150910: Conversion with .TryParse expects culture specific number format! Use XmlConvert instead!
                            d = System.Xml.XmlConvert.ToDouble(ResultForTest);
                        }
                        catch (Exception)
                        {
                            // Neutral format could not be converted, try selected culture
                            System.Globalization.CultureInfo culture = DiversityWorkbench.Import.Import.LanguageCode == "iv" ? System.Globalization.CultureInfo.InvariantCulture : System.Globalization.CultureInfo.CreateSpecificCulture(DiversityWorkbench.Import.Import.LanguageCode);
                            if (double.TryParse(ResultForTest, System.Globalization.NumberStyles.Float, culture, out d))
                                ResultForTest = System.Xml.XmlConvert.ToString(d);
                            else
                            {
                                this._DataTable.AddMessage("Column " + this.ColumnName + ": The value  '" + ResultForTest + "'  is no numeric value");
                                break;
                            }
                        }
                        // Toni 20171113: Assign conversion result 
                        Result = ResultForTest;
                        break;
                    case "real":
                        Single s;
                        try
                        {
                            // Toni 20150910: Conversion with .TryParse expects culture specific number format! Use XmlConvert instead!
                            s = System.Xml.XmlConvert.ToSingle(ResultForTest);
                        }
                        catch (Exception)
                        {
                            // Neutral format could not be converted, try selected culture
                            System.Globalization.CultureInfo culture = DiversityWorkbench.Import.Import.LanguageCode == "iv" ? System.Globalization.CultureInfo.InvariantCulture : System.Globalization.CultureInfo.CreateSpecificCulture(DiversityWorkbench.Import.Import.LanguageCode);
                            if (Single.TryParse(ResultForTest, System.Globalization.NumberStyles.Float, culture, out s))
                                ResultForTest = System.Xml.XmlConvert.ToString(s);
                            else
                            {
                                this._DataTable.AddMessage("Column " + this.ColumnName + ": The value  '" + ResultForTest + "'  is no numeric value");
                                break;
                            }
                        }
                        // Toni 20171113: Assign conversion result 
                        Result = ResultForTest;
                        break;
                    case "varbinary":
                    case "image":
                        break;
                    case "xml":
                    case "varchar":
                    case "nvarchar":
                    case "text":
                    case "ntext":
                    case "char":
                    case "nchar":
                    case "uniqueidentifier":
                        if (ResultForTest.IndexOf("'") > -1)
                            ResultForTest = ResultForTest.Replace("'", "''");
                        //if (!ResultForTest.StartsWith("'")) Toni 20210624: String must always start with ' - If it was already included in '...' it has been changes to ''...'' in statement before
                        ResultForTest = "'" + ResultForTest;
                        if (ResultForTest.StartsWith("'") &&
                            (this.DataType.ToLower().StartsWith("n") || this.DataType.ToLower() == "xml"))
                            ResultForTest = "N" + ResultForTest;
                        //if (!ResultForTest.EndsWith("'")) Toni 20210624: String must always end with ' - If it was already included in '...' it has been changes to ''...'' in statement before
                        ResultForTest += "'";
                        //else if (ResultForTest.EndsWith("''")
                        //    && !ResultForTest.EndsWith("'''"))
                        //    ResultForTest += "'";
                        //else if (ResultForTest.EndsWith("''''")
                        //    && !ResultForTest.EndsWith("'''''"))
                        //    ResultForTest += "'";
                        //if (ResultForTest == "'")
                        //    ResultForTest = "''";
                        Result = ResultForTest;

                        break;
                    case "smalldatetime":
                    case "datetime":
                    case "datetime2":
                        if (!ResultForTest.StartsWith("CONVERT(DATETIME"))
                        {
                            System.Globalization.CultureInfo culture = DiversityWorkbench.Import.Import.LanguageCode == "iv" ? System.Globalization.CultureInfo.InvariantCulture : System.Globalization.CultureInfo.CreateSpecificCulture(DiversityWorkbench.Import.Import.LanguageCode);
                            System.Globalization.DateTimeStyles styles = System.Globalization.DateTimeStyles.AssumeLocal;
                            System.DateTime DT;
                            bool DateCouldBeConverted = false;
                            if (System.DateTime.TryParse(ResultForTest, culture, styles, out DT))
                                DateCouldBeConverted = true;
                            else
                            {
                                styles = System.Globalization.DateTimeStyles.None;
                                if (System.DateTime.TryParse(ResultForTest, culture, styles, out DT))
                                    DateCouldBeConverted = true;
                            }
                            if (DateCouldBeConverted)
                            {
                                string checkString = ResultForTest;
                                ResultForTest = "CONVERT(DATETIME, '" + DT.Year.ToString() + "-" + DT.Month.ToString() + "-" + DT.Day.ToString();
                                if (DT.Hour == 0 && DT.Minute == 0 && DT.Second == 0)
                                {
                                    // Toni: Check if generated string matches original one
                                    string dateString = DT.ToString(culture.DateTimeFormat.ShortDatePattern);
                                    if (checkString.Trim() != dateString.Trim())
                                        ResultForTest += " " + DT.Hour.ToString() + ":" + DT.Minute.ToString() + ":" + DT.Second.ToString();
                                }
                                else
                                    ResultForTest += " " + DT.Hour.ToString() + ":" + DT.Minute.ToString() + ":" + DT.Second.ToString();
                                ResultForTest += "', 102)";
                                if (this._DataTable.MergeHandling == DiversityWorkbench.Import.DataTable.Merging.Update &&
                                    this.Transformations.Count == 1 &&
                                    this.Transformations[0].TypeOfTransformation == Transformation.TransformationType.Calculation &&
                                    this.Transformations[0].CalculationApplyOnData &&
                                    this.Transformations[0].CalculationApplyOnDataOperator != null &&
                                    this.Transformations[0].CalculationApplyOnDataOperator.Length > 0)
                                {
                                    Result = ResultForTest;
                                }
                                else if (this.Transformations.Count == 0)
                                {
                                    Result = ResultForTest;
                                }
                                else // Markus 3.8.2017 - kein Grund das nicht zu uebernehmen bekannt. Fuehrt sonst zu fehlerhaftem Datum
                                    Result = ResultForTest;
                            }
                            else
                            {
                                string Message = "Column " + this.ColumnName + ": Conversion of date  '" + ResultForTest + "'  failed. Current language = " + DiversityWorkbench.Import.Import.LanguageCode;
                                this._DataTable.AddMessage(Message);
                            }
                        }
                        break;
                    case "geography":
                        if (!ResultForTest.StartsWith("geography::"))
                        {
                        }
                        if (!ResultForTest.EndsWith(", 4326)"))
                        {
                        }
                        break;
                    case "geometry":
                        break;
                }
            }
            catch (System.Exception ex)
            {
            }

            // Markus 9.5.2017: Apply calculation on contents in database
            if (this._DataTable.MergeHandling == DiversityWorkbench.Import.DataTable.Merging.Update &&
                this.Transformations.Count == 1 &&
                this.Transformations[0].TypeOfTransformation == Transformation.TransformationType.Calculation &&
                this.Transformations[0].CalculationApplyOnData &&
                this.Transformations[0].CalculationApplyOnDataOperator != null &&
                this.Transformations[0].CalculationApplyOnDataOperator.Length > 0)
            {
                Result = this._ColumnName + " " + this.Transformations[0].CalculationApplyOnDataOperator + ResultForTest;
            }

            return Result;
        }

        // Toni: Copy function is required to prevent direct external access on prepare insert and prepare update delegates
        /// <summary>
        /// Copy own prepare insert and prepare update delegates to target
        /// </summary>
        /// <param name="Target column">Target column</param>
        public void CopyPrepareProcessingDelegates(DiversityWorkbench.Import.DataColumn TargetColumn)
        {
            TargetColumn._PrepareInsertFunction = this._PrepareInsertFunction;
            TargetColumn._PrepareUpdateFunction = this._PrepareUpdateFunction;
            TargetColumn._CheckGetValueFunction = this._CheckGetValueFunction;
            TargetColumn._CheckInsertFunction = this._CheckInsertFunction;
            TargetColumn._MultiColumnProcessing = this._MultiColumnProcessing;
        }

        // Toni: Get keys of "parent" table
        /// <summary>
        /// Keys values of related parent tables
        /// </summary>
        private string parentKey()
        {
            {
                string KeyValue = "";
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataTable.DataColumns)
                {
                    if (KV.Value.IsSelected &&
                        KV.Value.ForeignRelationColumn != null && KV.Value.ForeignRelationColumn.Length > 0 &&
                        KV.Value.ForeignRelationTableAlias != null && KV.Value.ForeignRelationTableAlias.Length > 0)
                    {
                        if (DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value != null
                            && DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value.Length > 0)
                        {
                            KeyValue += string.Format("[{0}].[{1}]=[{2}]", KV.Value.ForeignRelationTableAlias, KV.Value.ForeignRelationColumn, DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value);
                        }
                        else if (DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].ValueInDatabase != null
                            && DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].ValueInDatabase.Length > 0)
                        {
                            KeyValue += string.Format("[{0}].[{1}]=[{2}]", KV.Value.ForeignRelationTableAlias, KV.Value.ForeignRelationColumn, DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].ValueInDatabase);
                        }
                    }
                }
                return KeyValue;
            }
        }

        /// <summary>
        /// Perform prepare insert processing
        /// This delegate is called exactly one time before the target table entry is inserted.
        /// The converted column value is stored in field ProcessedValue.
        /// </summary>
        /// <param name="ImportConnection">SQL connection</param>
        /// <param name="ImportTransaction">SQL transaction</param>
        /// <param name="Message">Error message</param>
        public void PrepareInsert(Microsoft.Data.SqlClient.SqlConnection ImportConnection, Microsoft.Data.SqlClient.SqlTransaction ImportTransaction, ref string Message)
        {
            if (this.PrepareInsertDefined) // && this.ProcessedValue == null)
            {
                string tranformedValue = this.TransformedValue();
                string parent = parentKey();
                this._ProcessedValue = _PrepareInsertFunction(this.DataTable.TableName, this.ColumnName, tranformedValue, parent, ImportConnection, ImportTransaction, ref Message);
            }
        }

        /// <summary>
        /// Perform prepare update processing
        /// This delegate is called exactly one time before the target table entry is updated.
        /// The converted column value is stored in field ProcessedValue.
        /// </summary>
        /// <param name="ImportConnection">SQL connection</param>
        /// <param name="ImportTransaction">SQL transaction</param>
        /// <param name="Message">Error message</param>
        public void PrepareUpdate(Microsoft.Data.SqlClient.SqlConnection ImportConnection, Microsoft.Data.SqlClient.SqlTransaction ImportTransaction, ref string Message)
        {
            if (this.PrepareUpdateDefined) // && this.ProcessedValue == null)
            {
                string tranformedValue = this.TransformedValue();
                string parent = parentKey();
                this._ProcessedValue = _PrepareUpdateFunction(this.DataTable.TableName, this.ColumnName, tranformedValue, parent, ImportConnection, ImportTransaction, ref Message);
            }
        }

        /// <summary>
        /// Check if processed value exists in database end get it
        /// This delegate is called before insert or update processing to check and get the key value.
        /// The sucessfully converted column value is stored in field ProcessedValue.
        /// </summary>
        /// <param name="Message">Error message</param>
        /// <returns>'true' if processed value is present</returns>
        public bool CheckGetProcessedValue(ref string Message)
        {
            if (this.CheckGetValueDefined)
            {
                if (this.ProcessedValue == null)
                {
                    string value = this.TransformedValue();
                    string parent = parentKey();

                    if (_CheckGetValueFunction(this.DataTable.TableName, this.ColumnName, ref value, parent, ref Message))
                        this._ProcessedValue = value;
                }
            }
            return (this.ProcessedValue != null);
        }

        /// <summary>
        /// Check if processed value exists in database end get it
        /// This delegate is called during attachment processing to convert the attachment value.
        /// The sucessfully converted column value is stored in field ProcessedValue.
        /// </summary>
        /// <param name="value">Value to be processed</param>
        /// <param name="Message">Error message</param>
        /// <returns>'true' if processed value is present</returns>
        public string CheckGetProcessedValue(string value, ref string Message)
        {
            string parent = parentKey();

            if (this.CheckGetValueDefined)
            {
                if (this.ProcessedValue == null)
                {
                    if (_CheckGetValueFunction(this.DataTable.TableName, this.ColumnName, ref value, parent, ref Message))
                        this._ProcessedValue = value;
                    else
                        value = "";
                }
                else
                    value = "";
            }
            return value;
        }

        // Toni: Extended insert test for decisive columnst
        public bool InsertPossible()
        {
            if (this.CheckInsertDefined)
            {
                string tranformedValue = this.TransformedValue();
                string parent = parentKey();
                string message = "";
                bool result = this._CheckInsertFunction(this.DataTable.TableName, this.ColumnName, tranformedValue, parent);
                return result;
            }
            return true;
        }

        //#region Default value set by the user - may become necessary

        //private string _DefaultValue;
        ///// <summary>
        ///// If no value is given, the default is taken if missing e.g. in a PK
        ///// If the default value should be retireved via a select statement, the table containing the restriction has the alias T
        ///// </summary>
        //public string DefaultValue
        //{
        //    get
        //    {
        //        return _DefaultValue;
        //    }
        //    set { _DefaultValue = value; }
        //}

        //private string _DefaultValueQuery;

        //public string DefaultValueQuery
        //{
        //    get
        //    {
        //        if (this.DefaultValueRestrictionColumns.Count > 0
        //            && this.DefaultValue.Length > 0)
        //        {
        //            this._DefaultValueQuery = this.DefaultValue + " WHERE 1 = 1 ";
        //            foreach (string s in this._DefaultValueRestrictionColumns)
        //            {
        //                if (this._DataTable.DataColumns[s].Value != null && this._DataTable.DataColumns[s].Value.Length > 0)
        //                    this._DefaultValueQuery += " AND T." + s + " = '" + this._DataTable.DataColumns[s].Value + "'";
        //            }
        //        }
        //        return _DefaultValueQuery;
        //    }
        //    //set { _DefaultValueQuery = value; }
        //}

        //private System.Collections.Generic.List<string> _DefaultValueRestrictionColumns;
        ///// <summary>
        ///// If the Defaultvalue is generated via a select statement during the import, the columns that should be included in the query
        ///// </summary>
        //public System.Collections.Generic.List<string> DefaultValueRestrictionColumns
        //{
        //    get
        //    {
        //        if (this._DefaultValueRestrictionColumns == null)
        //            this._DefaultValueRestrictionColumns = new List<string>();
        //        return _DefaultValueRestrictionColumns;
        //    }
        //    set { _DefaultValueRestrictionColumns = value; }
        //}

        //#endregion

        #endregion

        #region Multicolumn

        private System.Collections.Generic.List<DiversityWorkbench.Import.ColumnMulti> _MultiColumns;
        /// <summary>
        /// The list of additional columns from the file that should be added after the main column
        /// </summary>
        public System.Collections.Generic.List<DiversityWorkbench.Import.ColumnMulti> MultiColumns
        {
            get { if (this._MultiColumns == null) this._MultiColumns = new List<ColumnMulti>(); return _MultiColumns; }
            set { _MultiColumns = value; }
        }

        #endregion

        #region Informations derived from the database or the template

        private string _DisplayText;
        /// <summary>
        /// the display text as shown in the interface
        /// </summary>
        public string DisplayText
        {
            get
            {
                if (this._DisplayText == null || this._DisplayText.Length == 0)
                    return this._ColumnName;
                else return this._DisplayText;
            }
            set { this._DisplayText = value; }
        }

        /// <summary>
        /// if the column can have additional columns from the file
        /// </summary>
        public bool IsMultiColumn
        {
            get
            {
                if (this.DataType == "nvarchar"
                    || this.DataType == "varchar"
                    || this.DataType == "char"
                    || this.DataType == "datetime"
                    || this.DataType == "smalldate"
                    || this.DataType == "datetime2"
                    || this.DataType == "geography")
                {
                    if (this.ForeignRelationColumn == null
                        || this.ForeignRelationColumn.Length == 0)
                        return true;
                    else return false;
                }
                else if (this.MultiColumnProcessing && this.PrepareInsertDefined && this.PrepareUpdateDefined && this.CheckGetValueDefined) // Toni: Allow multi column if transformation processing is defined
                    return true;
                else if (this.PrepareInsertDefined && this.DataRetrievalType == RetrievalType.IDorIDviaTextFromFile)
                    return true;
                else return false;
            }
        }

        private bool _IsNullable;
        public bool IsNullable
        {
            get
            {
                return this._IsNullable;
            }
            set { this._IsNullable = value; }
        }

        private string _ColumnName;
        /// <summary>
        /// the name of the column as defined in the database
        /// </summary>
        public string ColumnName
        {
            get { return _ColumnName; }
            //set { _ColumnName = value; }
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

        private string _ColumnDefault;
        /// <summary>
        /// a default of the column as defined in the database
        /// </summary>
        public string ColumnDefault
        {
            get { return _ColumnDefault; }
            set { _ColumnDefault = value; }
        }

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
                            "and c.object_id = t.object_id and t.name = '" + this._DataTable.TableName + "'";
                            string IdentityColumn = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            if (IdentityColumn.Length > 0 && IdentityColumn == this._ColumnName)
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

        private bool? _IsIdentityInParent;
        /// <summary>
        /// If the column is an identity column where the value will be automatically be generated by the database
        /// </summary>
        public bool IsIdentityInParent
        {
            get
            {
                if (this._IsIdentityInParent == null)
                {
                    if (this.IsIdentity)
                        this._IsIdentityInParent = true;
                    else
                    {
                        if (this._DataType != "int")
                            this._IsIdentityInParent = false;
                        else
                        {
                            if (this._DataTable.ParentTableAlias != null && this._DataTable.ParentTableAlias.Length > 0)
                            {
                                try
                                {
                                    if ((this.ForeignRelationTableAlias == this._DataTable.ParentTableAlias ||
                                        this.ForeignRelationTable == this._DataTable.ParentTableAlias) &&
                                        DiversityWorkbench.Import.Import.Tables[this._DataTable.ParentTableAlias].DataColumns[this.ForeignRelationColumn].IsIdentityInParent)
                                        this._IsIdentityInParent = true;
                                    else if ((this._DataTable.ParentTableAlias.StartsWith(this.ForeignRelationTableAlias + "_") ||
                                        this._DataTable.ParentTableAlias.StartsWith(this.ForeignRelationTable + "_")) &&
                                        this._ColumnName == "ReferencedID" &&
                                        DiversityWorkbench.Import.Import.Tables[this._DataTable.ParentTableAlias].DataColumns[this.ForeignRelationColumn].IsIdentityInParent)
                                    {
                                        //Markus 30.9.2016 - for referencing tables
                                        this._IsIdentityInParent = true;
                                    }
                                    else
                                    {
                                        if (DiversityWorkbench.Import.Import.Tables.ContainsKey(this._DataTable.ParentTableAlias) && DiversityWorkbench.Import.Import.Tables[this._DataTable.ParentTableAlias].ParentTableAlias != null)
                                        {
                                           
                                            //    //string ParentTable = DiversityWorkbench.Import.Import.Tables[this._DataTable.ParentTableAlias].ParentTableAlias;
                                            
                                            //        //string ParentColumn = DiversityWorkbench.Import.Import.Tables[this._DataTable.ParentTableAlias].DataColumns[this.ForeignRelationColumn].ForeignRelationColumn;
                                            //        if (DiversityWorkbench.Import.Import
                                            //            .Tables[parentTable.ParentTableAlias].DataColumns[parentColumn]
                                            //            .IsIdentity)
                                            //            this._IsIdentityInParent = true;
                                            //        else
                                            var parentTable = DiversityWorkbench.Import.Import.Tables[this._DataTable.ParentTableAlias];
                                            if (parentTable.ParentTableAlias != null && parentTable.DataColumns.ContainsKey(this.ForeignRelationColumn))
                                            {
                                                var parentColumn = parentTable.DataColumns[this.ForeignRelationColumn].ForeignRelationColumn;
                                                if (!string.IsNullOrEmpty(parentColumn) &&
                                                    DiversityWorkbench.Import.Import.Tables[parentTable.ParentTableAlias].DataColumns.ContainsKey(parentColumn))
                                                {
                                                    var column = DiversityWorkbench.Import.Import.Tables[parentTable.ParentTableAlias].DataColumns[parentColumn];
                                                    this._IsIdentityInParent = column.IsIdentity;
                                                }
                                                else
                                                    this._IsIdentityInParent = false;
                                            }
                                            else
                                                this._IsIdentityInParent = false;
                                        }
                                        else
                                            this._IsIdentityInParent = false;
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    this._IsIdentityInParent = false;
                                }
                            }
                            else
                                this._IsIdentityInParent = false;
                        }
                    }
                }
                return (bool)_IsIdentityInParent;
            }
            set { _IsIdentityInParent = value; }
        }

        private bool _IsPartOfPrimaryKey = false;
        /// <summary>
        /// If the column is part of the primary key of the table
        /// </summary>
        public bool IsPartOfPrimaryKey
        {
            get { return _IsPartOfPrimaryKey; }
            set { _IsPartOfPrimaryKey = value; }
        }

        private int _MaximumLength;
        /// <summary>
        /// the maximal length of the content as defined in the database
        /// </summary>
        public int MaximumLength
        {
            get { return _MaximumLength; }
            set { _MaximumLength = value; }
        }

        private string _ForeignRelationTable;
        /// <summary>
        /// The name of the parent table of a foreign relation as defined in the database
        /// </summary>
        public string ForeignRelationTable
        {
            get { return _ForeignRelationTable; }
            set { _ForeignRelationTable = value; }
        }

        private string _ForeignRelationColumn;
        /// <summary>
        /// The name of the column in the foreign relation table as defined in the database
        /// </summary>
        public string ForeignRelationColumn
        {
            get
            {
                if (this._ForeignRelationColumn == null)
                    this._ForeignRelationColumn = "";
                return _ForeignRelationColumn;
            }
            set { _ForeignRelationColumn = value; }
        }

        private string _ForeignRelationTableAlias;
        /// <summary>
        /// The alias of the parent table of a foreign relation
        /// </summary>
        public string ForeignRelationTableAlias
        {
            get
            {
                if (this._ForeignRelationTableAlias == null
                    && this._ForeignRelationTable != null
                    && this._ForeignRelationTable.Length > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
                    {
                        if (KV.Value.TableName == this._ForeignRelationTable)
                        {
                            if (KV.Value.TypeOfParallelity == DiversityWorkbench.Import.DataTable.Parallelity.unique)
                            {
                                this._ForeignRelationTableAlias = KV.Value.TableAlias;
                                break;
                            }
                            else if (KV.Value.TypeOfParallelity == DiversityWorkbench.Import.DataTable.Parallelity.parallel)
                            {
                                if (this.DataTable.ParentTableAlias != null
                                    && DiversityWorkbench.Import.Import.Tables.ContainsKey(this.DataTable.ParentTableAlias)
                                    && DiversityWorkbench.Import.Import.Tables[this.DataTable.ParentTableAlias].TableName == this._ForeignRelationTable)
                                    this._ForeignRelationTableAlias = this.DataTable.ParentTableAlias;
                                //if (this.DataTable.Step != null 
                                //    && this.DataTable.Step.ParentStep != null
                                //    && this.DataTable.Step.ParentStep.DataTable.TableName == this._ForeignRelationTable)
                                //{
                                //    this._ForeignRelationTableAlias = this.DataTable.Step.ParentStep.DataTable.TableAlias;
                                //    break;
                                //}
                            }
                        }
                    }
                }
                return _ForeignRelationTableAlias;
            }
            set { _ForeignRelationTableAlias = value; }
        }

        private string _ParentColumn;

        public string ParentColumn
        {
            get { return _ParentColumn; }
            set { _ParentColumn = value; }
        }

        private DiversityWorkbench.Import.DataTable _DataTable;
        /// <summary>
        /// the data table this column belongs to
        /// </summary>
        public DiversityWorkbench.Import.DataTable DataTable
        {
            get { return _DataTable; }
            //set { _DataTable = value; }
        }

        private string _SqlDataSource;
        /// <summary>
        /// a SQL-query that returns the list of values available for this column containing 2 columns: DisplayText and Value
        /// </summary>
        public string SqlDataSource
        {
            get
            {
                if (this._SqlDataSource == null &&
                    this._DataTable.DataSources.ContainsKey(this.ColumnName))
                    this.SqlDataSource = this._DataTable.DataSources[this.ColumnName];

                if (this._SqlDataSource == null
                    && this._ForeignRelationTable != null
                    && this._ForeignRelationColumn != null
                    && this._ForeignRelationColumn.Length > 0
                    && this._ForeignRelationTable.Length > 0
                    && this._ForeignRelationTableAlias == null)
                {
                    if (this.IsIdentity || this.IsIdentityInParent)
                        this._SqlDataSource = "";
                    else if (this.ForeignRelationTable == this._DataTable.TableName)
                        this._SqlDataSource = "";
                    else if (this.PrepareInsertDefined || this.PrepareUpdateDefined) // Toni: Do not read preselected values if insert conversion is defined
                        this._SqlDataSource = "";
                    else
                    {
                        string DisplayColumn = this._ForeignRelationColumn;
                        string SQL = "SELECT C.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS C " +
                            "WHERE C.TABLE_NAME = '" + this._ForeignRelationTable + "'";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        ad.Fill(dt);
                        System.Data.DataRow[] rr = dt.Select("COLUMN_NAME = 'DisplayText'");
                        if (rr.Length > 0) DisplayColumn = rr[0][0].ToString();
                        else
                        {
                            System.Data.DataRow[] rrN = dt.Select("COLUMN_NAME = '" + this._ForeignRelationTable + "Name'");
                            if (rrN.Length > 0) DisplayColumn = rrN[0][0].ToString();
                            else
                            {
                                System.Data.DataRow[] rrT = dt.Select("COLUMN_NAME = '" + this._ForeignRelationTable + "Title'");
                                if (rrT.Length > 0) DisplayColumn = rrT[0][0].ToString();
                            }
                        }
                        this._SqlDataSource = "SELECT NULL AS DisplayText, NULL AS Value UNION SELECT " + DisplayColumn + " AS DisplayText, " + this._ForeignRelationColumn + " AS Value FROM " + this._ForeignRelationTable + " ORDER BY DisplayText";
                    }
                }
                return _SqlDataSource;
            }
            set { _SqlDataSource = value; }
        }

        private System.Data.DataTable _MandatoryList;
        /// <summary>
        /// if only destinct values are allowed, a table that contains these values
        /// </summary>
        public System.Data.DataTable MandatoryList
        {
            get
            {
                if (this._MandatoryList == null && this.SqlDataSource != null && this.SqlDataSource.Length > 0)
                {
                    try
                    {
                        this._MandatoryList = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(this.SqlDataSource, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(this._MandatoryList);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return _MandatoryList;
            }
            //set { _MandatoryList = value; }
        }

        private string _MandatoryListDisplayColumn;
        /// <summary>
        /// the display column of the mandatory list as shown in the interface
        /// </summary>
        public string MandatoryListDisplayColumn
        {
            get
            {
                if (this.MandatoryList != null)
                    this._MandatoryListDisplayColumn = "DisplayText";
                return _MandatoryListDisplayColumn;
            }
            //set { _MandatoryListDisplayColumn = value; }
        }
        private string _MandatoryListValueColumn;
        /// <summary>
        /// the value column of the mandatory list as used for the entry into the database
        /// </summary>
        public string MandatoryListValueColumn
        {
            get
            {
                if (this.MandatoryList != null)
                    this._MandatoryListValueColumn = "Value";
                return _MandatoryListValueColumn;
            }
            //set { _MandatoryListValueColumn = value; }
        }

        private bool? _CanEditColumnContent;
        /// <summary>
        /// if the content of this column may be changed, e.g. transformed or adding of pre- and postfix
        /// </summary>
        public bool CanEditColumnContent
        {
            get
            {
                if (_CanEditColumnContent == null)
                {
                    if (this.DataType == "nvarchar"
                        || this.DataType == "varchar"
                        || this.DataType == "geography"
                        || this.DataType == "datetime"
                        || this.DataType == "smalldate"
                        || this.DataType == "datetime2"
                        || this.DataType == "int"
                        || this.DataType == "smallint"
                        || this.DataType == "tinyint")
                        _CanEditColumnContent = true;
                    else
                        _CanEditColumnContent = false;
                }
                return (bool)_CanEditColumnContent;
            }
            //set { _CanEditColumnContent = value; }
        }

        #endregion

        #region Construction

        public DataColumn(DiversityWorkbench.Import.DataTable Table, string ColumnName)
        {
            this._ColumnName = ColumnName;
            this._DataTable = Table;
        }

        #endregion
    }
}
