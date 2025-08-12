using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.PostgreSQL
{
    public class Column
    {
        #region Parameter and properties

        private string _Name;
        public string Name() { return this._Name; }
        private int _OrdinalPosition;
        private string _Default;
        private bool _IsNullable;
        private string _DataType;
        private int _MaxLengthOrPrecision;
        
        #endregion

        #region Construction

        public Column(string Name)
        {
            this._Name = Name;
        }
        public Column(string Name, int Position, string Default, bool IsNullable, string DataType, int Length)
        {
            this._Name = Name;
            this._OrdinalPosition = Position;
            this._Default = Default;
            this._IsNullable = IsNullable;
            this._DataType = DataType;
            this._MaxLengthOrPrecision = Length;
        }
        
        #endregion

    }
}
