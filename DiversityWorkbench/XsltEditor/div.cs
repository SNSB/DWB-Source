using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.XsltEditor
{
    /// <summary>
    /// corresponding to a div tag in HTML page containing the template
    /// </summary>
    public class div
    {
        #region Parameter

        private int _Top;
        private int _Left;
        private int _Height;
        private int _Width;
        private string _BorderColor;
        private string _BorderType;
        private float _BorderWidth;
        
        #endregion

        #region Construction

        public div()
        {
        }
        
        #endregion

    }
}
