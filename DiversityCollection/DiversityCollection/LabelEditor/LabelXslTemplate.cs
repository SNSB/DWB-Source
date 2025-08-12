using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.LabelEditor
{
    public class LabelXslTemplate
    {
        #region Parameter

        private System.Collections.Generic.Dictionary<string, string> _Attributes;
        private System.Collections.Generic.List<DiversityCollection.LabelEditor.LabelXslNode> _XslNodes;

        #endregion

        #region Construction

        public LabelXslTemplate(System.Collections.Generic.Dictionary<string, string> Attributes)
        {
            this._Attributes = Attributes;
        }
        
        #endregion
    }
}
