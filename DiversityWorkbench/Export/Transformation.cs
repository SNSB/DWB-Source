using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Export
{
    public class Transformation
    {

        #region FileColumn

        private DiversityWorkbench.Export.FileColumn _FileColumn;

        public DiversityWorkbench.Export.FileColumn FileColumn
        {
            get { return _FileColumn; }
            set { _FileColumn = value; }
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

        private bool _SplitterIsStartPosition;

        public bool SplitterIsStartPosition
        {
            get { if (_SplitterIsStartPosition == null) _SplitterIsStartPosition = false; return _SplitterIsStartPosition; }
            set { _SplitterIsStartPosition = value; }
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

        private System.Collections.Generic.List<string> _CalulationOperators;

        public System.Collections.Generic.List<string> CalulationOperators
        {
            get
            {
                if (_CalulationOperators == null)
                {
                    this._CalulationOperators = new List<string>();
                    this._CalulationOperators.Add("+");
                    this._CalulationOperators.Add("-");
                    this._CalulationOperators.Add("x");
                    this._CalulationOperators.Add("/");
                }
                return _CalulationOperators;
            }
        }

        private string _CalulationOperator;

        public string CalulationOperator
        {
            get { return _CalulationOperator; }
            set { _CalulationOperator = value; }
        }

        private string _CalculationFactor;

        public string CalculationFactor
        {
            get { return _CalculationFactor; }
            set { _CalculationFactor = value; }
        }

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

        #region Filter

        private System.Collections.Generic.List<DiversityWorkbench.Export.TransformationFilter> _FilterConditions;

        public System.Collections.Generic.List<DiversityWorkbench.Export.TransformationFilter> FilterConditions
        {
            get
            {
                if (this._FilterConditions == null)
                    this._FilterConditions = new List<TransformationFilter>();
                return _FilterConditions;
            }
            set { _FilterConditions = value; }
        }

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

        #region Color

        public enum ColorFormat { RGB, ARGB }

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

        #region Construction

        public Transformation(DiversityWorkbench.Export.FileColumn FileColumn, DiversityWorkbench.Export.Transformation.TransformationType Type)
        {
            try
            {
                this.FileColumn = FileColumn;
                this._TypeOfTransformation = Type;
                DiversityWorkbench.Export.FileColumn FC = FileColumn;
                FC.Transformations.Add(this);
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        public string TransformValue(string Value)
        {
            string Result = Value;
            switch (this.TypeOfTransformation)
            {
                case Transformation.TransformationType.RegularExpression:
                    Result = System.Text.RegularExpressions.Regex.Replace(Result, this.RegularExpression, this.RegularExpressionReplacement);
                    break;
                case Transformation.TransformationType.Replacement:
                    if (this.Replace != null && this.ReplaceWith != null)
                        Result = Result.Replace(this.Replace, this.ReplaceWith);
                    break;
                case Transformation.TransformationType.Split:
                    string[] VV = new string[] { };
                    int ss = this.SplitterList.Count;
                    string[] Separators;
                    Separators = new string[this.SplitterList.Count];
                    for (int i = 0; i < this.SplitterList.Count; i++)
                        Separators[i] = this.SplitterList[i];
                    VV = Result.Split(Separators, StringSplitOptions.None);
                    if (this.ReverseSequence)
                    {
                        if (VV.Length > this.SplitterPosition - 1 && this.SplitterPosition > 0)
                        {
                            Result = VV[VV.Length - this.SplitterPosition];
                            if (this.SplitterIsStartPosition)
                            {
                                for (int i = VV.Length - this.SplitterPosition - 1; i >= 0; i--)
                                {
                                    Result = VV[i] + " " + Result;
                                }
                            }
                        }
                        else
                            Result = "";
                    }
                    else
                    {
                        if (VV.Length > this.SplitterPosition - 1 && this.SplitterPosition > 0)
                        {
                            Result = VV[this.SplitterPosition - 1];
                            if (this.SplitterIsStartPosition)
                            {
                                for (int i = this.SplitterPosition; i < VV.Length; i++)
                                {
                                    Result += " " + VV[i];
                                }
                            }
                        }
                        else
                            Result = "";
                    }
                    break;
                case Transformation.TransformationType.Translation:
                    if (this.TranslationDictionary != null
                        && this.TranslationDictionary.Count > 0
                        && this.TranslationDictionary.ContainsKey(Result))
                        Result = this.TranslationDictionary[Result];
                    break;
                case Transformation.TransformationType.Calculation:
                    if (this.CalculationFactor != null &&
                        this.CalculationFactor.Length > 0 &&
                        this.CalulationOperator != null &&
                        this.CalulationOperator.Length > 0)
                    {
                        double ResultAsDouble;
                        double FResult;
                        if (double.TryParse(Result, out ResultAsDouble) && double.TryParse(this.CalculationFactor.Replace(" ", ""), out FResult))
                        {
                            double CResult = 0;
                            if (this.CalulationOperator == "*" || this.CalulationOperator == "/")
                                CResult = 1;
                            try
                            {
                                bool PerformCaluculation = true;
                                if (this.CalculationConditionValue != null &&
                                    this.CalculationConditionValue.Length > 0 &&
                                    this.CalculationConditionOperator != null &&
                                    this.CalculationConditionOperator.Length > 0)
                                {
                                    double ConditionValue;
                                    if (double.TryParse(this.CalculationConditionValue.Replace(" ", ""), out ConditionValue))
                                    {
                                        PerformCaluculation = false;
                                        switch (this.CalculationConditionOperator)
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
                                    switch (this.CalulationOperator)
                                    {
                                        case "+":
                                            CResult = ResultAsDouble + FResult;
                                            break;
                                        case "-":
                                            CResult = ResultAsDouble - FResult;
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
                    if (this.FilterConditionsOperator == Transformation.FilterConditionsOperators.And)
                        FilterPassed = true;

                    foreach (DiversityWorkbench.Export.TransformationFilter TF in this.FilterConditions)
                    {
                        string FilterString = "";
                        if (DiversityWorkbench.Export.Exporter.FileColumnList.ContainsKey(TF.PositionOfFilterColumn))// || this.FilterColumn == )
                        {
                            System.Collections.Generic.Dictionary<int, string> LL;
                            if (DiversityWorkbench.Export.Exporter.FileColumnList[TF.PositionOfFilterColumn].TableColumnUnitValue != null)
                                LL = DiversityWorkbench.Export.Exporter.CurrentFileColumnsTransformedContent;
                            else
                                LL = DiversityWorkbench.Export.Exporter.CurrentFileColumnsContent;
                            if (LL.ContainsKey(TF.PositionOfFilterColumn))
                            {
                                FilterString = LL[TF.PositionOfFilterColumn];
                            }
                            else if (DiversityWorkbench.Export.Exporter.CurrentFileColumnsContent.ContainsKey(TF.PositionOfFilterColumn))
                                FilterString = DiversityWorkbench.Export.Exporter.CurrentFileColumnsContent[TF.PositionOfFilterColumn];
                            switch (TF.FilterOperator)
                            {
                                case "=":
                                    if (FilterString != TF.Filter)
                                    {
                                        if (this.FilterConditionsOperator == Transformation.FilterConditionsOperators.And)
                                        {
                                            FilterPassed = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (this.FilterConditionsOperator == Transformation.FilterConditionsOperators.Or)
                                        {
                                            FilterPassed = true;
                                            break;
                                        }
                                    }
                                    break;
                                case "≠":
                                    if (FilterString == TF.Filter)
                                    {
                                        if (this.FilterConditionsOperator == Transformation.FilterConditionsOperators.And)
                                        {
                                            FilterPassed = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (this.FilterConditionsOperator == Transformation.FilterConditionsOperators.Or)
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
                        if (this.FilterUseFixedValue)
                            Result = this.FilterFixedValue;
                    }
                    else
                        Result = "";
                    break;
                case TransformationType.Color:
                    break;
            }
            return Result;
        }


    }
}
