using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.XslEditor
{
    //[Serializable()]		
    public class XslNode
    {
        #region Parameter

        private System.Collections.Generic.Dictionary<string, string> _Attributes;
        private System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslNode> _XslNodes;
        private string _Name;
        private string _Content;
        private System.Collections.Generic.Dictionary<string, string> _ContentParts;

        private string _XslNodeType;
        private bool _IsEmptyElement;
        private DiversityWorkbench.XslEditor.XslNode _ParentXslNode;
        private DiversityWorkbench.XslEditor.XslEditor _XslEditor;

        public System.Windows.Forms.TreeNode TreeNode;

        public DiversityWorkbench.XslEditor.XslEditor XslEditor
        {
            get { return _XslEditor; }
            //set { _XslEditor = value; }
        }

        #endregion

        #region Construction

        public XslNode()
        {
            this._Name = "";
        }

        public XslNode(string Name, string Value)
        {
            this._Name = Name;
            this._XslNodeType = Name;
        }

        public XslNode(string Content)
        {
            this._Content = Content;
        }

        public XslNode(System.Collections.Generic.Dictionary<string, string> Attributes)
        {
            this._Attributes = Attributes;
        }

        public XslNode(string Name, System.Collections.Generic.Dictionary<string, string> Attributes, DiversityWorkbench.XslEditor.XslEditor XslEditor)
        {
            //this._XmlFilePathNodes = XmlFilePathNodes;
            this._Name = Name;
            if (Name.StartsWith("xsl:"))
                this._XslNodeType = Name;
            else
                this._XslNodeType = "html";
            this._Attributes = Attributes;
            this._XslEditor = XslEditor;
        }

        public XslNode(string XslNodeType, string Name, System.Collections.Generic.Dictionary<string, string> Attributes)
        {
            this._Name = Name;
            //this._XmlFilePathNodes = XmlFilePathNodes;
            this._XslNodeType = XslNodeType;
            this._Attributes = Attributes;
        }

        #endregion

        #region Properties

        public string Content
        {
            get 
            {
                if (this._Content == null)
                    this._Content = "";
                return this._Content; 
            }
            set { this._Content = value; }
        }

        public string ContentFromContentParts()
        {
            string Content = "";
            if (this._ContentParts != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._ContentParts)
                {
                    if (Content.Length > 0)
                        Content += this._ContentPartSplitter + " ";
                    Content += KV.Key;
                    if (KV.Value.Length > 0)
                        Content += this._ContentPartValueSplitter + KV.Value;
                }
            }
            return Content;
        }

        public System.Collections.Generic.Dictionary<string, string> ContentParts()
        {
                if (this.Content.Length > 0)
                {
                    bool IsFont = false;
                    foreach (string s in DiversityWorkbench.XslEditor.XslEditor.HtmlFontFormats())
                    {
                        if (this.Content != null
                            && this.Content.Length > 0
                            && this.Content.IndexOf(s) > -1)
                        {
                            IsFont = true;
                            break;
                        }
                    }
                    if (IsFont)
                    {
                        this._ContentParts = new Dictionary<string, string>();
                        string[] Values = this.Content.Split(new char[] { ';' });
                        this._ContentPartSplitter = ";";
                        for (int i = 0; i < Values.Length; i++)
                        {
                            string[] ValuePart = Values[i].Split(new char[] { ':' });
                            this._ContentPartValueSplitter = ":";
                            if (ValuePart.Length == 2)
                            {
                                this._ContentParts.Add(ValuePart[0].Trim(), ValuePart[1].Trim());
                            }
                            else if (ValuePart.Length == 1 && ValuePart[0].Length > 0)
                            {
                                this._ContentParts.Add(ValuePart[0].Trim(), "");
                            }
                        }
                    }
                }
                return _ContentParts; 
            //set { _ContentParts = value; }
        }

        private string _ContentPartSplitter;
        private string _ContentPartValueSplitter;

        private System.Collections.Generic.List<string> _XmlPathsBelowRoot;
        public System.Collections.Generic.List<string> XmlPathsBelowRoot
        {
            get 
            {
                if (this._XmlPathsBelowRoot == null)
                {
                    this._XmlPathsBelowRoot = new List<string>();
                    if (this._ParentXslNode == null)
                        this._XmlPathsBelowRoot = DiversityWorkbench.XslEditor.XslEditor.XmlPathsFromRoot("", this);
                    else
                        this._XmlPathsBelowRoot = DiversityWorkbench.XslEditor.XslEditor.XmlPathsFromRoot(this._ParentXslNode.XsltXmlRootPath, this);
                }
                return _XmlPathsBelowRoot; 
            }
            //set { _XmlPathsFromRoot = value; }
        }

        private System.Collections.Generic.List<string> _XmlPathBelowRootForValue;
        public System.Collections.Generic.List<string> XmlPathBelowRootForValue
        {
            get
            {
                if (this._XmlPathBelowRootForValue == null)
                {
                    this._XmlPathBelowRootForValue = new List<string>();
                    this._XmlPathBelowRootForValue.Add("position()");
                    foreach (string s in this.XmlPathsBelowRoot)
                        this._XmlPathBelowRootForValue.Add(s);
                }
                return this._XmlPathBelowRootForValue;
            }
        }

        private System.Collections.Generic.List<string> _XmlPathBelowRootForCompareTo;
        public System.Collections.Generic.List<string> XmlPathBelowRootForCompareTo
        {
            get
            {
                if (this._XmlPathBelowRootForCompareTo == null)
                {
                    this._XmlPathBelowRootForCompareTo = new List<string>();
                    _XmlPathBelowRootForCompareTo.Add("last()");
                    foreach (string s in this.XmlPathsBelowRoot)
                        _XmlPathBelowRootForCompareTo.Add(s);
                }
                return _XmlPathBelowRootForCompareTo;
            }
        }

        private DiversityWorkbench.XslEditor.XslNode _XslTemplate;
        private DiversityWorkbench.XslEditor.XslNode XslTemplate
        {
            get
            {
                if (this._XslTemplate == null)
                {
                    bool TopReached = false;
                    DiversityWorkbench.XslEditor.XslNode Ncurrent = this;
                    while (!TopReached && this._XslTemplate == null)
                    {
                        if (Ncurrent.Name == "xsl:template")
                        {
                            this._XslTemplate = Ncurrent;
                            break;
                        }
                        if (Ncurrent.ParentXslNode == null)
                            TopReached = true;
                        Ncurrent = Ncurrent.ParentXslNode;
                    }
                }
                return this._XslTemplate;
            }
        }

        private System.Collections.Generic.List<string> _Templates;
        public System.Collections.Generic.List<string> Templates
        {
            get
            {
                if (this._Templates == null)
                {
                    this._Templates = new List<string>();
                    if (this._XslEditor != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> KV in this._XslEditor.XslTemplates)
                        {
                            if (!this._Templates.Contains(KV.Key))
                                this._Templates.Add(KV.Key);
//#
                        }
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> KV in this._XslEditor.XslIncludedTemplates)
                        {
                            if (!this._Templates.Contains(KV.Key))
                                this._Templates.Add(KV.Key);
//#
                        }
                    }
                }
                return this._Templates;
            }
        }

        private System.Collections.Generic.List<string> _Variables;
        public System.Collections.Generic.List<string> Variables
        {
            get
            {
                if (this._Variables == null)
                {
                    this._Variables = new List<string>();
                }
                return this._Variables;
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //public System.Collections.Generic.List< DiversityWorkbench.XslEditor.XslNode> XmlFilePathNodes
        //{
        //    get 
        //    {
        //        if (this._XmlFilePathNodes == null)
        //        {
        //            this._XmlFilePathNodes = new List<XslNode>();
        //        }
        //        return this._XmlFilePathNodes; 
        //    }
        //}

        /// <summary>
        /// the XML path of the current xslt node up to the root
        /// </summary>
        public string XsltXmlRootPath
        {
            get
            {
                string RootPath = "";// this.XmlParentRootPath;
                this.XmlPathOfRoot(ref RootPath);
                return RootPath;
            }
        }

        /// <summary>
        /// the XML Path of the current node
        /// </summary>
        public string XsltXmlPath
        {
            get
            {
                string Path = "";
                if (this.Attributes.Count > 0 && this.Attributes.ContainsKey("select"))
                {
                    Path += this.Attributes["select"];
                }
                return Path;
            }
        }

        /// <summary>
        /// the path of the xml node (not xslt node)
        /// </summary>
        public string XmlNodePath
        {
            get
            {
                string Path = this.Name;
                if (this.ParentXslNode != null)
                    Path = this.ParentXslNode.XmlNodePath + "/" + Path;
                return Path;
            }
        }

        /// <summary>
        /// the path of the parent xml node (not xslt)
        /// </summary>
        /// <param name="Path"></param>
        public void XmlPathOfRoot(ref string Path)
        {
            if (this.ParentXslNode != null)
            {
                Path += this.ParentXslNode.XsltXmlPath;
                this.ParentXslNode.XmlPathOfRoot(ref Path);
            }
        }

        public System.Collections.Generic.Dictionary<string, string> Attributes
        {
            get 
            {
                if (this._Attributes == null)
                    this._Attributes = new Dictionary<string, string>();
                return this._Attributes; 
            }
        }

        public string Name
        {
            get
            {
                if (this._Name == null)
                {
                    if (this._Attributes != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Attributes)
                            if (KV.Key == "name" || KV.Key == "match")
                                this._Name = KV.Value;
                    }
                    else this._Name = "";
                }
                return this._Name;
            }
        }

        public string DisplayText
        {
            get
            {
                string Display = "";
                if (this._Name != null)
                    Display = this._Name;
                else if (this.Content != null && Content.Length > 0)
                {
                    Display = this.Content;
                }
                if (this._Name == null && this._Attributes != null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Attributes)
                        if (KV.Key == "name" || KV.Key == "match")
                            this._Name = KV.Value;
                }
                try
                {
                    switch (this._XslNodeType)
                    {
                        case "xsl:include":
                            if (this._Attributes != null && this._Attributes.Count > 0 && this._Attributes.ContainsKey("href"))
                                Display = this._Attributes["href"];
                            break;
                        case "xsl:variable":
                            if (this._Attributes != null && this._Attributes.Count > 0 && this._Attributes.ContainsKey("name"))
                                Display = this._Attributes["name"];
                            break;
                        case "xsl:template":
                            if (this._Attributes != null && this._Attributes.Count > 0)
                            {
                                if (this._Attributes.ContainsKey("name"))
                                    Display = this._Attributes["name"];
                                else if (this._Attributes.ContainsKey("match"))
                                    Display = this._Attributes["match"];
                            }
                            break;
                        case "xsl:call-template":
                            if (this._Attributes != null && this._Attributes.Count > 0)
                            {
                                if (this._Attributes.ContainsKey("name"))
                                    Display = this._Attributes["name"];
                                else if (this._Attributes.ContainsKey("match"))
                                    Display = this._Attributes["match"];
                            }
                            break;
                        case "xsl:apply-templates":
                            if (this._Attributes != null && this._Attributes.Count > 0)
                            {
                                if (this._Attributes.ContainsKey("name"))
                                    Display = this._Attributes["name"];
                                else if (this._Attributes.ContainsKey("match"))
                                    Display = this._Attributes["match"];
                                else if (this._Attributes.ContainsKey("select"))
                                    Display = this._Attributes["select"];
                            }
                            break;
                        case "xsl:value-of":
                            if (this._Attributes != null && this._Attributes.Count > 0 && this._Attributes.ContainsKey("select"))
                                Display = this._Attributes["select"];
                            break;
                        case "xsl:for-each":
                            if (this._Attributes != null && this._Attributes.Count > 0 && this._Attributes.ContainsKey("select"))
                                Display = DiversityWorkbench.XslEditor.XslEditor.XsltElements["xsl:for-each"] + " " + this._Attributes["select"];
                            break;
                        case "xsl:if":
                            if (this._Attributes != null && this._Attributes.Count > 0 && this._Attributes.ContainsKey("test"))
                            {
                                Display = DiversityWorkbench.XslEditor.XslEditor.XsltElements["xsl:if"] + " " + this._Attributes["test"];
                            }
                            break;
                        case "xsl:text":
                            //Display += ": ";
                            if (this.XslNodes != null && this.XslNodes.Count > 0)
                            {
                                //Display += ": ";
                                foreach (DiversityWorkbench.XslEditor.XslNode NC in this.XslNodes)
                                    Display += NC.Content;
                            }
                            //if (this._Value != null && this._Value.Length > 0)
                            //    Display = this._Value;
                            break;
                        case "Comment":
                            if (this.XslNodes != null && this.XslNodes.Count > 0)
                            {
                                Display = "<!--Comment";
                                Display += "-->";
                            }
                            else if (this.Content != null && this.Content.Length > 0)
                            {
                                Display = "<!--" + this.Content;
                                if (this.Content.Length > 30)
                                    Display = "<!--" + this.Content.Substring(0, 30) + " ... ";
                                Display += "-->";
                            }
                            break;
                        case "html":
                            switch (this._Name)
                            {
                                case "th":
                                    if (this._Attributes != null && this._Attributes.Count > 0 && this._Attributes.ContainsKey("width"))
                                        Display += " width: " + this._Attributes["width"];
                                    break;
                                case "td":
                                    //if (this.XslNodes != null && this.XslNodes.Count > 0)
                                    //{
                                    //    Display += " ";
                                    //    foreach (DiversityWorkbench.XslEditor.XslNode NC in this.XslNodes)
                                    //        Display += NC.Content;
                                    //}
                                    //if (this._Value != null && this._Value.Length > 0)
                                    //    Display += " " + this._Value;
                                    break;
                                case "tr":
                                    break;
                                case "Comment":
                                    if (this.XslNodes != null && this.XslNodes.Count > 0)
                                    {
                                        Display = "<!--";
                                        foreach (DiversityWorkbench.XslEditor.XslNode NC in this.XslNodes)
                                            Display += NC.Content;
                                        if (Display.Length > 30)
                                            Display = Display.Substring(0, 30);
                                        Display += "-->";
                                    }
                                    //if (this.Value.Length > 0)
                                    //{
                                    //    if (this.Value.Length > 30)
                                    //        Display = "<!--" + this.Value.Substring(0, 30) + "-->";
                                    //    else
                                    //        Display = "<!--" + this.Value + "-->";
                                    //}
                                    break;
                                default:
                                    if (Display.Length == 0 && this._Content != null && this._Content.Length > 0)
                                        Display = this._Content;
                                    break;
                            }
                            break;
                        default:
                            if (Display.Length == 0 && this._Content != null && this._Content.Length > 0)
                                Display = this._Content;
                            break;
                    }
                }
                catch (System.Exception ex) { }
                /*
                 *             if (XslNode.Name == "xsl:if" && XslNode.Attributes.ContainsKey("test"))
                NodeText += " " + XslNode.Attributes["test"];
            else if (XslNode.Name == "xsl:value-of" && XslNode.Attributes.ContainsKey("select"))
                NodeText += " " + XslNode.Attributes["select"];
            else if (XslNode.Name == "xsl:apply-templates" && XslNode.Attributes.ContainsKey("select"))
                NodeText += " " + XslNode.Attributes["select"];
            else if (XslNode.Name == "Comment" && XslNode.Value.Length > 0)
                NodeText = XslNode.Value;
            else if (XslNode.Name == "xsl:text" && XslNode.Value.Length > 0)
                NodeText = XslNode.Value;
            else if (XslNode.XslNodeType == "Comment" && XslNode.Value.Length > 0)
                NodeText = XslNode.Value;

                 * */
                return Display;
            }
        }

        public System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslNode> XslNodes
        {
            get
            {
                if (this._XslNodes == null)
                    this._XslNodes = new List<XslNode>();
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
            set { _XslNodeType = value; }
        }

        public string XmlPathOfParentNodes
        {
            get
            {
                string Path = "";
                return Path;
            }
        }

        public bool IsEmptyElement
        {
            get
            {
                if (this._IsEmptyElement == null)
                    this._IsEmptyElement = false;
                return _IsEmptyElement;
            }
            set { _IsEmptyElement = value; }
        }

        public DiversityWorkbench.XslEditor.XslNode ParentXslNode
        {
            get { return _ParentXslNode; }
            set { _ParentXslNode = value; }
        }

        public string Path()
        {
            string Path = ".";
            return Path;
        }

        public int? ColumnSpan
        {
            get
            {
                int i = 1;
                if (this.Attributes != null && this.Attributes.Count > 0)
                {
                    if (this.Attributes.ContainsKey("colspan"))
                        int.TryParse(this.Attributes["colspan"], out i);
                    return i;
                }
                return null;
            }
        }

        public int? RowSpan
        {
            get
            {
                int i = 1;
                if (this.Attributes != null && this.Attributes.Count > 0)
                {
                    if (this.Attributes.ContainsKey("rowspan"))
                        int.TryParse(this.Attributes["rowspan"], out i);
                    return i;
                }
                return null;
            }
        }
        

        #endregion
    }
}
