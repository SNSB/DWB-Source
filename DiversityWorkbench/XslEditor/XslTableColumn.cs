using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.XslEditor
{
    public class XslTableColumn
    {

        #region Parameter

        //private int _ColumnWidth;
        private DiversityWorkbench.XslEditor.XslNode _XslNode;

        #endregion

        #region Construction

        //public XslTableColumn(int Width)
        //{
        //    this._ColumnWidth = Width;
        //}

        public XslTableColumn(DiversityWorkbench.XslEditor.XslNode XslNode)
        {
            this._XslNode = XslNode;            
            
            //int Width = 10;
            //if (this._XslNode.Attributes.Count > 0)
            //{
            //    if (this._XslNode.Attributes.ContainsKey("width"))
            //        int.TryParse(this._XslNode.Attributes["width"], out Width);
            //}
            //this._ColumnWidth = Width;
        }

        #endregion

        #region Public properties

        public int ColumnWidth
        {
            get 
            {
                int Width = 10;
                if (this._XslNode.Attributes.Count > 0)
                {
                    if (this._XslNode.Attributes.ContainsKey("width"))
                        int.TryParse(this._XslNode.Attributes["width"], out Width);
                }
                return Width;
                //return this._ColumnWidth; 
            }
            set 
            {
                    if (this._XslNode.Attributes.ContainsKey("width"))
                        this._XslNode.Attributes["width"] = value.ToString();

                //this._ColumnWidth = value; 
            }
        }

        public DiversityWorkbench.XslEditor.XslNode XslNode { get { return this._XslNode; } }

        #endregion

    }
}
