using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Import
{
    public interface iTransformationInterface
    {
        System.Windows.Forms.DataGridView DataGridCopy(int FristLines);
        void initControl();
        void RemoveFilterConditin(DiversityWorkbench.Import.TransformationFilter Filter);
    }
    

    /// <summary>
    /// the transformations of values when moved between file and database 
    /// </summary>
    public class Transformation
    {
        #region Properties and parameter

        #region Datacolumn

        private DiversityWorkbench.Import.iDataColumn _iDataColumn;

        public DiversityWorkbench.Import.iDataColumn iDataColumn
        {
            get { return _iDataColumn; }
            set { _iDataColumn = value; }
        }
        
        #endregion

        #region Type of the transformation

        public enum TransformationType { Split, Translation, RegularExpression, Replacement, Calculation, Filter, Color }
        private TransformationType _TypeOfTransformation;

        public TransformationType TypeOfTransformation
        {
            get { return _TypeOfTransformation; }
            //set { _TypeOfTransformation = value; }
        }

        #endregion

        #region Splitting the value

        private System.Collections.Generic.List<string> _SplitterList;

        public System.Collections.Generic.List<string> SplitterList
        {
            get { if (this._SplitterList == null) this._SplitterList = new List<string>(); return _SplitterList; }
            set { _SplitterList = value; }
        }
        private int _SplitterPosition;

        public int SplitterPosition
        {
            get { if (this._SplitterPosition == null || this._SplitterPosition == 0) this._SplitterPosition = 1; return _SplitterPosition; }
            set { _SplitterPosition = value; }
        }

        private bool _ReverseSequence;

        public bool ReverseSequence
        {
            get { if (this._ReverseSequence == null) this._ReverseSequence = false; return _ReverseSequence; }
            set { _ReverseSequence = value; }
        }

        #endregion

        #region Translation

        private System.Collections.Generic.SortedDictionary<string, string> _TranslationDictionary;

        public System.Collections.Generic.SortedDictionary<string, string> TranslationDictionary
        {
            get 
            {
                if (this._TranslationDictionary == null)
                    this._TranslationDictionary = new SortedDictionary<string, string>();
                return _TranslationDictionary; 
            }
            set { _TranslationDictionary = value; }
        }

        private string _TranslationSourceTable;
        public string TranslationSourceTable { get => _TranslationSourceTable; set => _TranslationSourceTable = value; }

        private string _TranslationFromColumn;
        public string TranslationFromColumn { get => _TranslationFromColumn; set => _TranslationFromColumn = value; }

        private string _TranslationIntoColumn;
        public string TranslationIntoColumn { get => _TranslationIntoColumn; set => _TranslationIntoColumn = value; }

        private System.Collections.Generic.SortedDictionary<string, string> _TranslationDictionarySourceTable;

        public System.Collections.Generic.SortedDictionary<string, string> TranslationDictionarySourceTable
        {
            get
            {
                if (this._TranslationDictionarySourceTable == null)
                    this._TranslationDictionarySourceTable = new SortedDictionary<string, string>();
                return _TranslationDictionarySourceTable;
            }
            set { _TranslationDictionarySourceTable = value; }
        }

        public void TranslationDictionarySourceTableReadData()
        {
            try
            {
                if (this.TranslationDictionarySourceTable != null && this.TranslationSourceTable.Length > 0 && this.TranslationIntoColumn.Length > 0 && this.TranslationFromColumn.Length > 0)
                {
                    string SQL = "SELECT " + this.TranslationFromColumn + ", MIN(" + this.TranslationIntoColumn + ") AS " + this.TranslationIntoColumn + " FROM " + this.TranslationSourceTable +
                        " WHERE " + this.TranslationFromColumn + " <> '' AND " + this.TranslationIntoColumn + " <> '' " +
                        " GROUP BY " + this.TranslationFromColumn + 
                        " HAVING COUNT(*) = 1";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        if (!this.TranslationDictionary.ContainsKey(row[this.TranslationFromColumn].ToString()) && !this.TranslationDictionarySourceTable.ContainsKey(row[this.TranslationFromColumn].ToString()))
                        {
                            this.TranslationDictionarySourceTable.Add(row[this.TranslationFromColumn].ToString(), row[this.TranslationIntoColumn].ToString());
                        }
                    }
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region RegularExpession

        private string _RegularExpression;

        public string RegularExpression
        {
            get { return _RegularExpression; }
            set { _RegularExpression = value; }
        }
        private string _RegularExpressionReplacement;

        public string RegularExpressionReplacement
        {
            get { return _RegularExpressionReplacement; }
            set { _RegularExpressionReplacement = value; }
        }
        
        #endregion

        #region Replacement

        private string _Replace;

        public string Replace
        {
            get { return _Replace; }
            set { _Replace = value; }
        }
        private string _ReplaceWith;

        public string ReplaceWith
        {
            get { return _ReplaceWith; }
            set { _ReplaceWith = value; }
        }

        #endregion

        #region Calculation

        #region Operator

        private System.Collections.Generic.List<string> _CalculationOperators;

        public System.Collections.Generic.List<string> CalculationOperators
        {
            get
            {
                if (_CalculationOperators == null)
                {
                    this._CalculationOperators = new List<string>();
                    this._CalculationOperators.Add("+");
                    this._CalculationOperators.Add("-");
                    this._CalculationOperators.Add("*");
                    this._CalculationOperators.Add("/");
                }
                return _CalculationOperators;
            }
        }

        private string _CalulationOperator;

        public string CalulationOperator
        {
            get { return _CalulationOperator; }
            set { _CalulationOperator = value; }
        }
        
        #endregion

        #region Factor

        private string _CalculationFactor;

        public string CalculationFactor
        {
            get { return _CalculationFactor; }
            set { _CalculationFactor = value; }
        }
        
        #endregion

        #region Condition

        private string _CalculationConditionOperator;

        public string CalculationConditionOperator
        {
            get { return _CalculationConditionOperator; }
            set { _CalculationConditionOperator = value; }
        }

        private System.Collections.Generic.List<string> _CalculationConditionOperators;

        public System.Collections.Generic.List<string> CalculationConditionOperators
        {
            get
            {
                if (_CalculationConditionOperators == null)
                {
                    this._CalculationConditionOperators = new List<string>();
                    this._CalculationConditionOperators.Add("");
                    this._CalculationConditionOperators.Add("<");
                    this._CalculationConditionOperators.Add("<=");
                    this._CalculationConditionOperators.Add("=");
                    this._CalculationConditionOperators.Add(">=");
                    this._CalculationConditionOperators.Add(">");
                    this._CalculationConditionOperators.Add("≠");
                }
                return _CalculationConditionOperators;
            }
        }

        private string _CalculationConditionValue;

        public string CalculationConditionValue
        {
            get { return _CalculationConditionValue; }
            set { _CalculationConditionValue = value; }
        }
        
        #endregion

        #region Apply on data

        private bool _CalculationApplyOnData;

        public bool CalculationApplyOnData
        {
            get { return _CalculationApplyOnData; }
            set { _CalculationApplyOnData = value; }
        }

        private string _CalculationApplyOnDataOperator;

        public string CalculationApplyOnDataOperator
        {
            get { return _CalculationApplyOnDataOperator; }
            set { _CalculationApplyOnDataOperator = value; }
        }

        #endregion

        #endregion

        #region Color

        public enum ColorFormat { RGBhex, RGBdec, ARGBint }

        private ColorFormat _ColorFrom;
        public ColorFormat ColorFrom
        {
            get { return _ColorFrom; }
            set { _ColorFrom = value; }
        }

        private ColorFormat _ColorInto;
        public ColorFormat ColorInto
        {
            get { return _ColorInto; }
            set { _ColorInto = value; }
        }

        #endregion

        #region Filter

        private System.Collections.Generic.List<DiversityWorkbench.Import.TransformationFilter> _FilterConditions;

        public System.Collections.Generic.List<DiversityWorkbench.Import.TransformationFilter> FilterConditions
        {
            get 
            {
                if (this._FilterConditions == null)
                    this._FilterConditions = new List<TransformationFilter>();
                return _FilterConditions; 
            }
            set { _FilterConditions = value; }
        }
        
        //private string _Filter;
        ///// <summary>
        ///// The value that should be compared with the contents of the file
        ///// </summary>
        //public string Filter
        //{
        //    get 
        //    {
        //        if (this._Filter == null)
        //            this._Filter = "";
        //        return _Filter; 
        //    }
        //    set { _Filter = value; }
        //}

        ////private string _FilterOperator;
        /////// <summary>
        /////// The kind of comparision with the contents of the file
        /////// </summary>
        ////public string FilterOperator
        ////{
        ////    get 
        ////    {
        ////        if (this._FilterOperator == null)
        ////            this._FilterOperator = "=";
        ////        return _FilterOperator; 
        ////    }
        ////    set { _FilterOperator = value; }
        ////}

        ////private System.Collections.Generic.List<string> _FilterOperators;
        /////// <summary>
        /////// The operators available for the comparision
        /////// </summary>
        ////public System.Collections.Generic.List<string> FilterOperators
        ////{
        ////    get
        ////    {
        ////        if (_FilterOperators == null)
        ////        {
        ////            this._FilterOperators = new List<string>();
        ////            //this._FilterOperators.Add("<");
        ////            //this._FilterOperators.Add("<=");
        ////            //this._FilterOperators.Add(">=");
        ////            //this._FilterOperators.Add(">");
        ////            this._FilterOperators.Add("=");
        ////            this._FilterOperators.Add("≠");
        ////            //this._FilterOperators.Add("Ø");
        ////            //this._FilterOperators.Add("•");
        ////        }
        ////        return _FilterOperators;
        ////    }
        ////}

        ////private int? _FilterColumn;
        /////// <summary>
        /////// The column of the file that should be compared with the filter
        /////// </summary>
        ////public int FilterColumn
        ////{
        ////    get
        ////    {
        ////        if (this._FilterColumn == null)
        ////            this._FilterColumn = this.iDataColumn.PositionInFile();
        ////        return (int)_FilterColumn;
        ////    }
        ////    set { _FilterColumn = value; }
        ////}

        public enum FilterConditionsOperators { And, Or }

        private FilterConditionsOperators _FilterConditionsOperator;

        public FilterConditionsOperators FilterConditionsOperator
        {
            get 
            {
                if (_FilterConditionsOperator == null)
                    _FilterConditionsOperator = FilterConditionsOperators.And;
                return _FilterConditionsOperator; 
            }
            set { _FilterConditionsOperator = value; }
        }

        private bool _FilterUseFixedValue;
        /// <summary>
        /// True if the result of the filtering should be a fixed value
        /// otherwise the content of a column is used as result
        /// </summary>
        public bool FilterUseFixedValue
        {
            get
            {
                if (_FilterUseFixedValue == null)
                    this._FilterUseFixedValue = false;
                return _FilterUseFixedValue;
            }
            set { _FilterUseFixedValue = value; }
        }

        private string _FilterFixedValue;
        /// <summary>
        /// A fixed value that should be used if the conditions are fulfilled
        /// </summary>
        public string FilterFixedValue
        {
            get
            {
                if (this._FilterFixedValue == null)
                    this._FilterFixedValue = "";
                return _FilterFixedValue;
            }
            set { _FilterFixedValue = value; }
        }


        #endregion

        #endregion

        #region Construction

        public Transformation(DiversityWorkbench.Import.iDataColumn iDataColumn, DiversityWorkbench.Import.Transformation.TransformationType Type)
        {
            try
            {
                this._iDataColumn = iDataColumn;
                //DiversityWorkbench.Import.Import.Tables[iDataColumn.
                this._TypeOfTransformation = Type;
                if (iDataColumn.GetType() == typeof(DiversityWorkbench.Import.ColumnMulti))
                {
                    DiversityWorkbench.Import.ColumnMulti CM = (DiversityWorkbench.Import.ColumnMulti)iDataColumn;
                    CM.Transformations.Add(this);
                    //DiversityWorkbench.Import.Import.Tables[CM.DataColumn.DataTable.TableAlias].DataColumns[CM.DataColumn.ColumnName].MultiColumns.Add(;
                }
                else if (iDataColumn.GetType() == typeof(DiversityWorkbench.Import.DataColumn))
                {
                    DiversityWorkbench.Import.DataColumn DC = (DiversityWorkbench.Import.DataColumn)iDataColumn;
                    DC.Transformations.Add(this);
                }
            }
            catch (System.Exception ex)
            {
            }
        }
        
        #endregion

    }
}
