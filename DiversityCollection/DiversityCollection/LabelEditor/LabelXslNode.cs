using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.LabelEditor
{
    public class LabelXslNode
    {
        #region Parameter

        private System.Collections.Generic.Dictionary<string, string> _Attributes;
        private System.Collections.Generic.List<DiversityCollection.LabelEditor.LabelXslNode> _XslNodes;
        private string _Name;
        private string _XslNodeType;

        #endregion

        #region Construction

        public LabelXslNode(System.Collections.Generic.Dictionary<string, string> Attributes)
        {
            this._Attributes = Attributes;
        }

        public LabelXslNode(string Name, System.Collections.Generic.Dictionary<string, string> Attributes)
        {
            this._Name = Name;
            this._Attributes = Attributes;
        }

        public LabelXslNode(string XslNodeType, string Name, System.Collections.Generic.Dictionary<string, string> Attributes)
        {
            this._Name = Name;
            this._XslNodeType = XslNodeType;
            this._Attributes = Attributes;
        }

        #endregion

        #region Properties

        public string Value = "";

        public System.Collections.Generic.Dictionary<string, string> Attributes
        {
            get { return this._Attributes; }
        }

        public string Name
        {
            get
            {
                if (this._Name == null)
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Attributes)
                        if (KV.Key == "name" || KV.Key == "match")
                            this._Name = KV.Value;
                return this._Name;
            }
        }

        public System.Collections.Generic.List<DiversityCollection.LabelEditor.LabelXslNode> XslNodes
        {
            get
            {
                if (this._XslNodes == null)
                    this._XslNodes = new List<LabelXslNode>();
                return _XslNodes;
            }
            //set { _XslNodes = value; }
        }

        public string XslNodeType
        {
            get 
            {
                if (this._XslNodeType == null)
                    this._XslNodeType = "";
                return _XslNodeType; 
            }
            //set { _XslNodeType = value; }
        }

        #endregion
    }
}
