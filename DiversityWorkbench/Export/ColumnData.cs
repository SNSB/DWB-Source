using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Export
{
    public class ColumnData
    {
        private TableColumn _TableColumn;

        public TableColumn TableColumn
        {
            get { return _TableColumn; }
            set { _TableColumn = value; }
        }
        private string _Prefix;

        public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }
        private string _Postfix;

        public string Postfix
        {
            get { return _Postfix; }
            set { _Postfix = value; }
        }
        private System.Collections.Generic.List<Transformation> _TransformationList;
        private string _UnitValue;

        public string UnitValue
        {
            get { return _UnitValue; }
            set { _UnitValue = value; }
        }

        public ColumnData(TableColumn TC)
        {
            this._TableColumn = TC;
        }

        //public ColumnData(TableColumn TC, string UnitValue)
        //{
        //    this._TableColumn = TC;
        //    this._UnitValue = UnitValue;
        //}

        public ColumnData(TableColumnUnitValue UV)
        {
            this._TableColumn = UV.TableColumn;
            this._UnitValue = UV.UnitValue;
        }

    }
}
