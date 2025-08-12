using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.LabelEditor
{
    public class LabelRow
    {

        #region Parameter

        //private DiversityCollection.LabelEditor.UserControlLabelRow _UserControlLabelRow;
        private int _RowHeight;
        
        #endregion

        #region Construction

        public LabelRow(int RowHeight)//, DiversityCollection.LabelEditor.UserControlLabelRow UserControlLabelRow
        {
            //this._UserControlLabelRow = UserControlLabelRow;
            this._RowHeight = RowHeight;
        }
        
        #endregion

        #region Public properties

        public int RowHeight
        {
            get { return this._RowHeight; }
            set { this._RowHeight = value; }
        }
        
        #endregion

    }
}
