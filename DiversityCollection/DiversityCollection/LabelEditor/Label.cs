using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.LabelEditor
{
    public class Label
    {

        #region Parameter

        private System.Collections.Generic.List<DiversityCollection.LabelEditor.LabelRow> _LabelRows;
        private System.Collections.Generic.List<DiversityCollection.LabelEditor.LabelColumn> _LabelColums;
        //private System.Collections.Generic.Dictionary<string, string> _LabelTableAttributes;
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _XslVariables;
        private System.Collections.Generic.Dictionary<string, DiversityCollection.LabelEditor.LabelXslNode> _XslTemplates;
        private System.Collections.Generic.Dictionary<string, DiversityCollection.LabelEditor.LabelXslNode> _XslNodes;
        private System.Collections.Generic.List<string> _XslIncludes;

        #endregion

        #region Construction
        
        public Label()
        {
        }
        
        #endregion

        #region Public properties

        public System.Collections.Generic.List<string> XslIncludes
        {
            get
            {
                if (this._XslIncludes == null)
                    this._XslIncludes = new List<string>();
                return _XslIncludes;
            }
            //set { _XslIncludes = value; }
        }

        //public System.Collections.Generic.List<System.Xml.XmlElement> XslVariables
        //{
        //    get
        //    {
        //        if (this._XslVariables == null)
        //            this._XslVariables = new List<System.Xml.XmlElement>();
        //        return _XslVariables;
        //    }
        //    //set { _XslVariables = value; }
        //}

        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> XslVariables
        {
            get 
            {
                if (this._XslVariables == null)
                    this._XslVariables = new Dictionary<string, List<string>>();
                return _XslVariables; 
            }
            //set { _XslValiables = value; }
        }

        public System.Collections.Generic.Dictionary<string, DiversityCollection.LabelEditor.LabelXslNode> XslTemplates
        {
            get
            { 
                if (this._XslTemplates == null)
                    this._XslTemplates = new Dictionary<string, DiversityCollection.LabelEditor.LabelXslNode>();
                return _XslTemplates;
            }
            //set { _XslTemplates = value; }
        }


        //public System.Collections.Generic.Dictionary<string, string> LabelTableAttributes
        //{
        //    get
        //    {
        //        if (this._LabelTableAttributes == null)
        //            this._LabelTableAttributes = new Dictionary<string, string>();
        //        return _LabelTableAttributes;
        //    }
        //}

        public System.Collections.Generic.List<DiversityCollection.LabelEditor.LabelColumn> LabelColums
        {
            get
            {
                if (this._LabelColums == null)
                {
                    this._LabelColums = new List<LabelEditor.LabelColumn>();
                    if (this.XslTemplates.ContainsKey("Label"))
                    {
                        if (this.XslTemplates["Label"].XslNodes.Count > 0 && this.XslTemplates["Label"].XslNodes[0].Name == "table")
                        {
                            if (this.XslTemplates["Label"].XslNodes.Count > 0 && this.XslTemplates["Label"].XslNodes[0].XslNodes.Count > 0)
                            {
                                if (this.XslTemplates["Label"].XslNodes[0].XslNodes[0].XslNodes.Count > 0)
                                {
                                    foreach (DiversityCollection.LabelEditor.LabelXslNode N in this.XslTemplates["Label"].XslNodes[0].XslNodes[0].XslNodes)
                                    {
                                        if (N.Attributes.Count > 0 && N.Name == "th")
                                        {
                                            DiversityCollection.LabelEditor.LabelColumn C = new LabelColumn(int.Parse(N.Attributes["width"]));
                                            this._LabelColums.Add(C);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return _LabelColums;
            }
        }

        public System.Collections.Generic.List<DiversityCollection.LabelEditor.LabelRow> LabelRows
        {
            get
            {
                if (this._LabelRows == null)
                {
                    this._LabelRows = new List<LabelEditor.LabelRow>();
                    if (this.XslTemplates.ContainsKey("Label"))
                    {
                        if (this.XslTemplates["Label"].XslNodes.Count > 0 && this.XslTemplates["Label"].XslNodes[0].Name == "table")
                        {
                            if (this.XslTemplates["Label"].XslNodes.Count > 0 && this.XslTemplates["Label"].XslNodes[0].XslNodes.Count > 0)
                            {
                                foreach (DiversityCollection.LabelEditor.LabelXslNode N in this.XslTemplates["Label"].XslNodes[0].XslNodes)
                                {
                                    if (N.Attributes.Count > 0 && N.Name == "tr")
                                    {
                                        DiversityCollection.LabelEditor.LabelRow R = new LabelRow(int.Parse(N.Attributes["height"]));
                                        this._LabelRows.Add(R);
                                    }
                                }
                            }
                        }
                    }
                }
                return _LabelRows;
            }
        }

        public int LabelWidth
        {
            get
            {
                int i = 0;
                foreach (DiversityCollection.LabelEditor.LabelColumn C in this.LabelColums)
                {
                    i = i + C.ColumnWidth;
                }
                return i;
            }
        }
        public int LabelHeight
        {
            get
            {
                int i = 0;
                foreach (DiversityCollection.LabelEditor.LabelRow R in this.LabelRows)
                {
                    i = i + R.RowHeight;
                }
                return i;
            }
        }

        public int ColumnCount { get { return this.LabelColums.Count; } }
        public int RowCount { get { return this.LabelRows.Count; } }


        #endregion

        #region Analyse XSLT
        
        public void AnalyseXSLTfile(string XsltFile)
        {
            if (XsltFile.Length == 0)
                return;
            System.Xml.XmlTextReader R = new System.Xml.XmlTextReader(XsltFile);
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "ImportSchedule"
                        || R.Name == "xml"
                        || R.Name == "xsl:stylesheet"
                        || R.Name == "xsl:output")
                        continue;
                    if (R.Name == "xsl:include")
                    {
                        this.ReadInclude(ref R);
                    }
                    else if (R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "xsl:variable")
                    {
                        this.ReadVariable(ref R);
                    }
                    else if (R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "xsl:template")
                    {
                        this.ReadXslTemplate(ref R);
                    }
                    else
                    {

                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                R.ResetState();
                R.Close();
            }
        }

        private void ReadInclude(ref System.Xml.XmlTextReader R)
        {
            try
            {
                string IncludedFile = R.GetAttribute(0);
                this.XslIncludes.Add(IncludedFile);
                return;
            }
            catch (System.Exception ex) { }
        }

        private void ReadVariable(ref System.Xml.XmlTextReader R)
        {
            try
            {
                string VarialbleName = R.GetAttribute(0);
                R.Read();
                string[] AttributList = R.Value.ToString().Split(new char[] { ';' });
                System.Collections.Generic.List<string> Att = new List<string>();
                foreach (string s in AttributList)
                {
                    Att.Add(s.Trim());
                }
                this.XslVariables.Add(VarialbleName, Att);
                return;
            }
            catch (System.Exception ex) { }
        }

        private void ReadXslTemplate(ref System.Xml.XmlTextReader R)
        {
            try
            {
                string TemplateName = R.GetAttribute(0);
                System.Collections.Generic.Dictionary<string, string> Att = new Dictionary<string, string>();
                for (int i = 0; i < R.AttributeCount; i++)
                {
                    R.MoveToAttribute(i);
                    Att.Add(R.Name, R.Value);
                    if (R.Name == "name" || R.Name == "match")
                        TemplateName = R.Value;
                }
                DiversityCollection.LabelEditor.LabelXslNode T = new LabelEditor.LabelXslNode(Att);
                this.XslTemplates.Add(TemplateName, T);
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "xsl:template")
                        return;
                    if (R.NodeType == System.Xml.XmlNodeType.Element)
                        this.ReadXslNode(ref R, ref T);
                    else
                    {
                    }
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "xsl:template")
                        return;
                }
            }
            catch (System.Exception ex) { }
        }

        private void ReadXslNode(ref System.Xml.XmlTextReader R, ref DiversityCollection.LabelEditor.LabelXslNode ParentNode)
        {
            try
            {
                string XslNodeType = R.Name;
                string NodeName = R.Name;
                System.Collections.Generic.Dictionary<string, string> Att = new Dictionary<string, string>();
                DiversityCollection.LabelEditor.LabelXslNode N;
                if (R.AttributeCount > 0)
                {
                    for (int i = 0; i < R.AttributeCount; i++)
                    {
                        R.MoveToAttribute(i);
                        Att.Add(R.Name, R.Value);
                        if (R.Name == "name" || R.Name == "match")
                            NodeName = R.Value;
                    }
                }
                if (XslNodeType == NodeName)
                {
                    N = new LabelXslNode(NodeName, Att);
                }
                else
                {
                    N = new LabelXslNode(XslNodeType, NodeName, Att);
                }
                ParentNode.XslNodes.Add(N);
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                    {
                        if (R.Name == NodeName && R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return;
                        if (R.Name == ParentNode.Name && R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return;
                        if (R.Name == "xsl:template" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return;
                        else if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return;
                        else
                            return;
                    }
                    else if (R.NodeType == System.Xml.XmlNodeType.Element)
                        this.ReadXslNode(ref R, ref N);
                    else if (R.NodeType == System.Xml.XmlNodeType.Text)
                    {
                        N.Value = R.Value;
                    }
                    else if (R.NodeType == System.Xml.XmlNodeType.Comment)
                    {
                        System.Collections.Generic.Dictionary<string, string> AttComment = new Dictionary<string, string>();
                        DiversityCollection.LabelEditor.LabelXslNode NC = new LabelXslNode("Comment", AttComment);
                        NC.Value = R.Value;
                        N.XslNodes.Add(NC);
                    }
                    else
                    {
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        #endregion

    }

}
