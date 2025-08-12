using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Export
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

        private int? _PositionOfFilterColumn;
        /// <summary>
        /// The position of the FileColumn that should be compared with the filter
        /// </summary>
        public int PositionOfFilterColumn
        {
            get
            {
                if (this._PositionOfFilterColumn == null)
                    this._PositionOfFilterColumn = this._Transformation.FileColumn.Position;
                return (int)_PositionOfFilterColumn;
            }
            set { _PositionOfFilterColumn = value; }
        }
        //private double? _PositionOfFilterColumn;
        ///// <summary>
        ///// The position of the FileColumn that should be compared with the filter
        ///// </summary>
        //public double PositionOfFilterColumn
        //{
        //    get
        //    {
        //        if (this._PositionOfFilterColumn == null)
        //            this._PositionOfFilterColumn = this._Transformation.FileColumn.Position;
        //        return (double)_PositionOfFilterColumn;
        //    }
        //    set { _PositionOfFilterColumn = value; }
        //}


        private DiversityWorkbench.Export.Transformation _Transformation;

        public DiversityWorkbench.Export.Transformation Transformation
        {
            get { return _Transformation; }
            //set { _Transformation = value; }
        }

        #endregion

        #region Construction

        public TransformationFilter(DiversityWorkbench.Export.Transformation Transformation)
        {
            this._Transformation = Transformation;
        }

        #endregion

    }
}
