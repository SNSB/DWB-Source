using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Import
{
    public class StepColumnGroup
    {
        private System.Drawing.Image _Image;

        public System.Drawing.Image Image
        {
            get { return _Image; }
            //set { _Image = value; }
        }
        private string _DisplayText;

        public string DisplayText
        {
            get { return _DisplayText; }
            //set { _DisplayText = value; }
        }
        private System.Collections.Generic.List<string> _Columns;

        public System.Collections.Generic.List<string> Columns
        {
            get { return _Columns; }
            //set { _Columns = value; }
        }

        public StepColumnGroup(System.Drawing.Image Image, string DisplayText, System.Collections.Generic.List<string> Columns)
        {
            this._Image = Image;
            this._DisplayText = DisplayText;
            this._Columns = Columns;
        }
    }
}
