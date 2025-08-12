using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Export
{
    public class FileColumn
    {

        #region Properties

        #region Position

        //private double _Position;

        ///// <summary>
        ///// The position within the columns of the export
        ///// </summary>
        //public double Position
        //{
        //    get { return _Position; }
        //    set { _Position = value; }
        //}

        private int _Position;

        /// <summary>
        /// The position within the columns of the export
        /// </summary>
        public int Position
        {
            get { return _Position; }
            set { _Position = value; }
        }


        #endregion

        #region Separation

        private bool _IsSeparatedFromPreviousColumn = true;

        /// <summary>
        /// If the column is separated from the previous column
        /// </summary>
        public bool IsSeparatedFromPreviousColumn
        {
            get
            {
                return _IsSeparatedFromPreviousColumn;
            }
            set { _IsSeparatedFromPreviousColumn = value; }
        }

        #endregion

        #region Pre- and Postfix

        private string _Prefix = "";

        public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }
        private string _Postfix = "";

        public string Postfix
        {
            get { return _Postfix; }
            set { _Postfix = value; }
        }


        #endregion

        #region Transformations

        private System.Collections.Generic.List<Export.Transformation> _Transformations;

        public System.Collections.Generic.List<Export.Transformation> Transformations
        {
            get
            {
                if (this._Transformations == null)
                    this._Transformations = new List<Transformation>();
                return _Transformations;
            }
            set { _Transformations = value; }
        }

        #endregion

        #region Table column and unit value

        private TableColumn _TableColumn;
        private TableColumnUnitValue _TableColumnUnitValue;

        public TableColumnUnitValue TableColumnUnitValue
        {
            get { return _TableColumnUnitValue; }
            set { _TableColumnUnitValue = value; }
        }

        public TableColumn TableColumn
        {
            get { return _TableColumn; }
            set { _TableColumn = value; }
        }

        #endregion

        #region Header

        private string _Header;

        public string Header
        {
            get
            {
                if (_Header == null)
                    _Header = this.TableColumn.DisplayText;
                return _Header;
            }
            set { _Header = value; }
        }

        #endregion
        
        #endregion

        #region Construction

        public FileColumn(Export.TableColumn TC)
        {
            this._TableColumn = TC;
        }

        public FileColumn(Export.TableColumnUnitValue TCUV)
        {
            this._TableColumn = TCUV.TableColumn;
            this._TableColumnUnitValue = TCUV;
        }
        
        #endregion

        #region Current value

        private string _CurrentValue;

        public string CurrentValue
        {
            get { return _CurrentValue; }
            set
            {
                _CurrentValue = value;
            }
        }

        //public string TransformedValue()
        //{
        //    string Value = this._CurrentValue;
        //    if (this._TableColumnUnitValue != null)
        //    {
        //        Value = this._TableColumnUnitValue.GetSourceValue(this._CurrentValue);
        //    }
        //    if (this.Transformations.Count > 0)
        //    {
        //    }
        //    if (this.Prefix.Length > 0)
        //        Value = this.Prefix + Value;
        //    if (this.Postfix.Length > 0)
        //        Value += this.Postfix;
        //    return Value;
        //}

        public async System.Threading.Tasks.Task<string> TransformedValue(string Value)
        {
            if (this._TableColumnUnitValue != null)
            {
                Value = await this._TableColumnUnitValue.GetSourceValue(Value);
            }
            if (this.Transformations.Count > 0)
            {
                foreach (Export.Transformation T in this.Transformations)
                {
                    Value = T.TransformValue(Value);
                }
            }
            if (Value.Length > 0)
            {
                if (this.Prefix.Length > 0)
                    Value = this.Prefix + Value;
                if (this.Postfix.Length > 0)
                    Value += this.Postfix;
            }
            return Value;
        }

        #endregion

    }
}
