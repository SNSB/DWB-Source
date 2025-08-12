using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Import
{
    public class TransformationFilter
    {

        #region Properties
        
        private string _Filter;
        /// <summary>
        /// The value that should be compared with the contents of the file
        /// </summary>
        public string Filter
        {
            get
            {
                if (this._Filter == null)
                    this._Filter = "";
                return _Filter;
            }
            set { _Filter = value; }
        }

        private string _FilterOperator;
        /// <summary>
        /// The kind of comparision with the contents of the file
        /// </summary>
        public string FilterOperator
        {
            get
            {
                if (this._FilterOperator == null)
                    this._FilterOperator = "=";
                return _FilterOperator;
            }
            set { _FilterOperator = value; }
        }

        private System.Collections.Generic.List<string> _FilterOperators;
        /// <summary>
        /// The operators available for the comparision
        /// </summary>
        public System.Collections.Generic.List<string> FilterOperators
        {
            get
            {
                if (_FilterOperators == null)
                {
                    this._FilterOperators = new List<string>();
                    //this._FilterOperators.Add("<");
                    //this._FilterOperators.Add("<=");
                    //this._FilterOperators.Add(">=");
                    //this._FilterOperators.Add(">");
                    this._FilterOperators.Add("=");
                    this._FilterOperators.Add("≠");
                    //this._FilterOperators.Add("Ø");
                    //this._FilterOperators.Add("•");
                }
                return _FilterOperators;
            }
        }

        private int? _FilterColumn;
        /// <summary>
        /// The column of the file that should be compared with the filter
        /// </summary>
        public int FilterColumn
        {
            get
            {
                if (this._FilterColumn == null)
                    this._FilterColumn = this._Transformation.iDataColumn.PositionInFile();
                return (int)_FilterColumn;
            }
            set { _FilterColumn = value; }
        }

        //private bool _FilterUseFixedValue;
        ///// <summary>
        ///// True if the result of the filtering should be a fixed value
        ///// otherwise the content of a column is used as result
        ///// </summary>
        //public bool FilterUseFixedValue
        //{
        //    get
        //    {
        //        if (_FilterUseFixedValue == null)
        //            this._FilterUseFixedValue = false;
        //        return _FilterUseFixedValue;
        //    }
        //    set { _FilterUseFixedValue = value; }
        //}

        //private string _FilterFixedValue;
        ///// <summary>
        ///// A fixed value that should be used if the conditions are fulfilled
        ///// </summary>
        //public string FilterFixedValue
        //{
        //    get
        //    {
        //        if (this._FilterFixedValue == null)
        //            this._FilterFixedValue = "";
        //        return _FilterFixedValue;
        //    }
        //    set { _FilterFixedValue = value; }
        //}

        //private DiversityWorkbench.Import.iDataColumn _iDataColumn;

        //public DiversityWorkbench.Import.iDataColumn iDataColumn
        //{
        //    get { return _iDataColumn; }
        //    set { _iDataColumn = value; }
        //}

        private DiversityWorkbench.Import.Transformation _Transformation;

        public DiversityWorkbench.Import.Transformation Transformation
        {
            get { return _Transformation; }
            //set { _Transformation = value; }
        }
        
        #endregion

        #region Construction

        public TransformationFilter(DiversityWorkbench.Import.Transformation Transformation)
        {
            this._Transformation = Transformation;
        }
        
        #endregion

    }
}
