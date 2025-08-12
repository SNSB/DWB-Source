using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.LabelEditor
{
    public class LabelColumn
    {
        #region Parameter

        private int _ColumnWidth;

        #endregion

        #region Construction

        public LabelColumn(int Width)
        {
            this._ColumnWidth = Width;
        }

        #endregion

        #region Public properties

        public int ColumnWidth
        {
            get { return this._ColumnWidth; }
            set { this._ColumnWidth = value; }
        }

        #endregion

    }
}
