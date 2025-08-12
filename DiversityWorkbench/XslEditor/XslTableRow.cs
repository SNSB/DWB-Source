using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.XslEditor
{
    public class XslTableRow
    {

        #region Parameter

        //private DiversityCollection.LabelEditor.UserControlLabelRow _UserControlLabelRow;
        //private int _RowHeight;
        private DiversityWorkbench.XslEditor.XslNode _XslNode;
        
        #endregion

        #region Construction

        //public XslTableRow(int RowHeight)//, DiversityCollection.LabelEditor.UserControlLabelRow UserControlLabelRow
        //{
        //    //this._UserControlLabelRow = UserControlLabelRow;
        //    this._RowHeight = RowHeight;
        //}
        
        public XslTableRow(DiversityWorkbench.XslEditor.XslNode XslNode)//, DiversityCollection.LabelEditor.UserControlLabelRow UserControlLabelRow
        {
            this._XslNode = XslNode;
        }
        
        #endregion

        #region Public properties

        public int RowHeight
        {
            get 
            {
                int Height = 10;
                if (this._XslNode.Attributes.Count > 0)
                {
                    if (this._XslNode.Attributes.ContainsKey("height"))
                        int.TryParse(this._XslNode.Attributes["height"], out Height);
                }
                return Height;

                
                //return this._RowHeight; 
            }
            set 
            {
                if (this._XslNode.Attributes.ContainsKey("wiheightdth"))
                    this._XslNode.Attributes["height"] = value.ToString();
                //this._RowHeight = value; 
            }
        }

        public DiversityWorkbench.XslEditor.XslNode XslNode
        {
            get { return this._XslNode; }
        }
        
        #endregion

    }
}
