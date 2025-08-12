using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.XslEditor
{
    //[Serializable()]
    public class XslEditor
    {
        #region Parameter

        private static System.IO.FileInfo _XMLFile;
        private static System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslNode> _XMLFileNodes;

        //public static readonly System.Drawing.Color ColorIncludedTemplate = System.Drawing.Color.LimeGreen;
        //public static readonly System.Drawing.Color ColorIncludedVariable = System.Drawing.Color.Chocolate;
        public static readonly System.Drawing.Color ColorIncluded = System.Drawing.SystemColors.Control;

        public static readonly System.Drawing.Color ColorTemplate = System.Drawing.Color.Green;
        public static readonly System.Drawing.Color ColorVariable = System.Drawing.Color.Brown;

        public static readonly System.Drawing.Color ColorHTML = System.Drawing.Color.Blue;
        public static readonly System.Drawing.Color ColorComment = System.Drawing.Color.Gray;

        private System.IO.FileInfo _XsltFile;

        public System.IO.FileInfo XsltFile
        {
            get { return _XsltFile; }
            //set { _XsltFile = value; }
        }

        private System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslNode> _XslDocumentNodes;

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> _XslIncludes;

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslEditor> _XlsIncludedFiles;

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> _XslIncludedVariables;

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> _XslIncludedTemplates;

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> _XslVariables;
        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> _XslTemplates;

        //public enum XsltElement {Template, ApplyTemplates, ValueOf, ForEach, Sort, If, Choose};

        #endregion

        #region Construction

        public XslEditor()
        {
            //this._XslDocumentNodes = new System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslNode>();
        }

        #endregion

        #region Public properties

        public void Reset()
        {
            DiversityWorkbench.XslEditor.XslEditor._XMLFile = null;
            DiversityWorkbench.XslEditor.XslEditor._XMLFileNodes = null;
        }

        public static System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslNode> XMLFileNodes
        {
            get
            {
                if (DiversityWorkbench.XslEditor.XslEditor._XMLFileNodes == null)
                    DiversityWorkbench.XslEditor.XslEditor._XMLFileNodes = new List<XslNode>();
                return _XMLFileNodes; }
            set { _XMLFileNodes = value; }
        }

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> XslIncludedVariables
        {
            get
            {
                if (this._XslIncludedVariables == null)
                    this._XslIncludedVariables = new Dictionary<string, XslNode>();
                return _XslIncludedVariables;
            }
            set { _XslIncludedVariables = value; }
        }

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> XslIncludedTemplates
        {
            get
            {
                if (this._XslIncludedTemplates == null)
                    this._XslIncludedTemplates = new Dictionary<string, XslNode>();
                return _XslIncludedTemplates;
            }
            set { _XslIncludedTemplates = value; }
        }

        public System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslNode> XslDocumentNodes
        {
            get
            {
                if (this._XslDocumentNodes == null)
                    this._XslDocumentNodes = new System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslNode>();
                return _XslDocumentNodes;
            }
            set { _XslDocumentNodes = value; }
        }

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> XslIncludes
        {
            get
            {
                if (this._XslIncludes == null)
                    this._XslIncludes = new Dictionary<string, DiversityWorkbench.XslEditor.XslNode>();
                return _XslIncludes;
            }
        }

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> XslVariables
        {
            get
            {
                if (this._XslVariables == null)
                    this._XslVariables = new Dictionary<string, DiversityWorkbench.XslEditor.XslNode>();
                return _XslVariables;
            }
        }

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> XslTemplates
        {
            get
            {
                if (this._XslTemplates == null)
                    this._XslTemplates = new Dictionary<string, DiversityWorkbench.XslEditor.XslNode>();
                return _XslTemplates;
            }
        }

        private System.Collections.Generic.List<string> _XslFonts;
        public System.Collections.Generic.List<string> XslFonts
        {
            get
            {
                if (this._XslFonts == null)
                {
                    this._XslFonts = new List<string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> KV in this.XslVariables)
                    {
                        if (KV.Value.XslNodes.Count > 0 && KV.Value.XslNodes[0].Content.IndexOf("font") > -1)
                        {
                            this._XslFonts.Add("{$" + KV.Key + "}");
                        }
                    }
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> KV in this.XslIncludedVariables)
                    {
                        if (KV.Value.XslNodes.Count > 0 && KV.Value.XslNodes[0].Content.IndexOf("font") > -1)
                        {
                            this._XslFonts.Add("{$" + KV.Key + "}");
                        }
                    }
                }
                return this._XslFonts;
            }
        }

        //public System.Collections.Generic.List<DiversityWorkbench.XslEditor.LabelColumn> LabelColums
        //{
        //    get
        //    {
        //        if (this._LabelColums == null)
        //        {
        //            this._LabelColums = new List<LabelEditor.LabelColumn>();
        //            if (this.XslTemplates.ContainsKey("Label"))
        //            {
        //                if (this.XslTemplates["Label"].XslNodes.Count > 0 && this.XslTemplates["Label"].XslNodes[0].Name == "table")
        //                {
        //                    if (this.XslTemplates["Label"].XslNodes.Count > 0 && this.XslTemplates["Label"].XslNodes[0].XslNodes.Count > 0)
        //                    {
        //                        if (this.XslTemplates["Label"].XslNodes[0].XslNodes[0].XslNodes.Count > 0)
        //                        {
        //                            foreach (DiversityWorkbench.XslEditor.XslNode N in this.XslTemplates["Label"].XslNodes[0].XslNodes[0].XslNodes)
        //                            {
        //                                if (N.Attributes.Count > 0 && N.Name == "th")
        //                                {
        //                                    DiversityWorkbench.XslEditor.LabelColumn C = new LabelColumn(int.Parse(N.Attributes["width"]));
        //                                    this._LabelColums.Add(C);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return _LabelColums;
        //    }
        //}

        //public System.Collections.Generic.List<DiversityWorkbench.XslEditor.LabelRow> LabelRows
        //{
        //    get
        //    {
        //        if (this._LabelRows == null)
        //        {
        //            this._LabelRows = new List<LabelEditor.LabelRow>();
        //            if (this.XslTemplates.ContainsKey("Label"))
        //            {
        //                if (this.XslTemplates["Label"].XslNodes.Count > 0 && this.XslTemplates["Label"].XslNodes[0].Name == "table")
        //                {
        //                    if (this.XslTemplates["Label"].XslNodes.Count > 0 && this.XslTemplates["Label"].XslNodes[0].XslNodes.Count > 0)
        //                    {
        //                        foreach (DiversityWorkbench.XslEditor.XslNode N in this.XslTemplates["Label"].XslNodes[0].XslNodes)
        //                        {
        //                            if (N.Attributes.Count > 0 && N.Name == "tr")
        //                            {
        //                                DiversityWorkbench.XslEditor.LabelRow R = new LabelRow(int.Parse(N.Attributes["height"]));
        //                                this._LabelRows.Add(R);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return _LabelRows;
        //    }
        //}

        //public int LabelWidth
        //{
        //    get
        //    {
        //        int i = 0;
        //        foreach (DiversityWorkbench.XslEditor.LabelColumn C in this.LabelColums)
        //        {
        //            i = i + C.ColumnWidth;
        //        }
        //        return i;
        //    }
        //}
        //public int LabelHeight
        //{
        //    get
        //    {
        //        int i = 0;
        //        foreach (DiversityWorkbench.XslEditor.LabelRow R in this.LabelRows)
        //        {
        //            i = i + R.RowHeight;
        //        }
        //        return i;
        //    }
        //}

        //public int ColumnCount { get { return this.LabelColums.Count; } }
        //public int RowCount { get { return this.LabelRows.Count; } }

        public System.Collections.Generic.Dictionary <string, DiversityWorkbench.XslEditor.XslEditor> XslIncludedFiles
        {
            get 
            {
                if (this._XlsIncludedFiles == null)
                {
                    this._XlsIncludedFiles = new Dictionary<string, XslEditor>();
                }
                return _XlsIncludedFiles; 
            }
            //set { _XlsIncludedFiles = value; }
        }

        #endregion

        #region XML file
        
        /// <summary>
        /// Analysing the xml file and creating the static object XMLFile
        /// </summary>
        /// <param name="XmlFile">the full name of the file</param>
        public void AnalyseXmlFile(string XmlFile)
        {
            if (XmlFile.Length == 0)
                return;
            DiversityWorkbench.XslEditor.XslEditor._XMLFile = new System.IO.FileInfo(XmlFile);
            System.Xml.XmlTextReader R = new System.Xml.XmlTextReader(XmlFile);
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    else
                    {
                        DiversityWorkbench.XslEditor.XslNode P = new XslNode();
                        DiversityWorkbench.XslEditor.XslNode ND = this.ReadXslNode(ref R, ref P);
                        if (ND != null)
                            DiversityWorkbench.XslEditor.XslEditor.XMLFileNodes.Add(ND);
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

        //private static System.Collections.Generic.List<string> _XmlFilePaths;
        //public static System.Collections.Generic.List<string> XmlFilePaths
        //{
        //    get
        //    {
        //        if (DiversityWorkbench.XslEditor.XslEditor._XmlFilePaths == null)
        //        {
        //            DiversityWorkbench.XslEditor.XslEditor._XmlFilePaths = new List<string>();
        //            foreach (DiversityWorkbench.XslEditor.XslNode N in DiversityWorkbench.XslEditor.XslEditor.XMLFileNodes)
        //            {
        //                if (N.Attributes != null && N.Attributes.Count > 0 && N.Attributes.ContainsKey("select"))
        //                {
        //                }
        //                if (N.XslNodes != null && N.XslNodes.Count > 0)
        //                {
        //                    //Di
        //                }
        //            }
        //        }
        //        return DiversityWorkbench.XslEditor.XslEditor._XmlFilePaths;
        //    }
        //}

        //private static void XmlPathsAddChildren(DiversityWorkbench.XslEditor.XslNode N)
        //{
        //}

        //public static System.Collections.Generic.List<string> XmlPathsFromRoot(string RootPath, System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslNode> XmlFileNodes)
        //{
        //    System.Collections.Generic.List<string> Paths = new List<string>();
        //    foreach (string s in DiversityWorkbench.XslEditor.XslEditor.XmlFilePaths)
        //    {

        //    }
        //    foreach (DiversityWorkbench.XslEditor.XslNode N in DiversityWorkbench.XslEditor.XslEditor._XMLFileNodes)
        //    {
        //    }
        //    //foreach (string s in XmlPaths)
        //    //{
        //    //    if (s.StartsWith(RootPath))
        //    //    {
        //    //    }
        //    //}
        //    return Paths;
        //}

        /// <summary>
        /// Getting all paths starting at a certain point in the XML tree
        /// </summary>
        /// <param name="RootPath">The xml path of the starting point</param>
        /// <returns>The list of paths underneath the starting point</returns>
        public static System.Collections.Generic.List<string> XmlPathsFromRoot(string RootPath, DiversityWorkbench.XslEditor.XslNode XslNode)
        {
            System.Collections.Generic.List<string> Paths = new List<string>();

            //if (XslNode.XslNodeType == "test")
            //{
            //    Paths.Add("position()");
            //    Paths.Add("last()");
            //}

            // Search all nodes for a potential match of the path
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> KV in DiversityWorkbench.XslEditor.XslEditor.XmlNodePaths)
            {
                if (RootPath.StartsWith("."))
                    RootPath = RootPath.Substring(1);
                if (KV.Key.IndexOf(RootPath) > -1)
                {
                    foreach (string s in DiversityWorkbench.XslEditor.XslEditor.XmlPathsOfChildren(KV.Key))
                    {
                        if (s.Length > 0)
                        {
                            string Path = s;
                            if (!Path.StartsWith("."))
                                Path = "." + Path;
                            if (!Paths.Contains(Path))
                            {
                                Paths.Add(Path);
                            }
                        }
                    }
                }
            }
            return Paths;
        }

        /// <summary>
        /// The paths of all children underneath a starting point
        /// </summary>
        /// <param name="RootPath">the starting point</param>
        /// <returns>the list of paths</returns>
        private static System.Collections.Generic.List<string> XmlPathsOfChildren(string RootPath)
        {
            System.Collections.Generic.List<string> XmlChildren = new List<string>();
            if (RootPath.Length > 0)
            {
                try
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> KV in DiversityWorkbench.XslEditor.XslEditor.XmlNodePaths)
                    {
                        if (KV.Key.StartsWith(RootPath) && KV.Key.Length > RootPath.Length)
                        {
                            if (KV.Value.XslNodes.Count > 0)
                            {
                                foreach (DiversityWorkbench.XslEditor.XslNode N in KV.Value.XslNodes)
                                {
                                    if (N.XmlNodePath.Length > RootPath.Length && N.XmlNodePath.StartsWith(RootPath))
                                    {
                                        string Path = N.XmlNodePath.Substring(RootPath.Length);
                                        if (!XmlChildren.Contains(Path))
                                            XmlChildren.Add("." + Path);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex) { }
            }
            return XmlChildren;
        }

        private static System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> _XmlNodePaths;
        /// <summary>
        /// the (different) paths of all nodes of the xml document
        /// </summary>
        public static System.Collections.Generic.Dictionary<string, DiversityWorkbench.XslEditor.XslNode> XmlNodePaths
        {
            get 
            {
                if (DiversityWorkbench.XslEditor.XslEditor._XmlNodePaths == null)
                {
                    DiversityWorkbench.XslEditor.XslEditor._XmlNodePaths = new Dictionary<string, XslNode>();
                    foreach (DiversityWorkbench.XslEditor.XslNode N in DiversityWorkbench.XslEditor.XslEditor.XMLFileNodes)
                    {
                        string Path = N.Name;
                        if (N.ParentXslNode != null)
                            Path = N.ParentXslNode.XsltXmlPath + "/" + Path;
                        if (!DiversityWorkbench.XslEditor.XslEditor._XmlNodePaths.ContainsKey(Path))
                        {
                            DiversityWorkbench.XslEditor.XslEditor._XmlNodePaths.Add(Path, N);
                            if (N.XslNodes.Count > 0)
                                DiversityWorkbench.XslEditor.XslEditor.XmlNodePathAddChildren(N);
                        }
                    }
                }
                return XslEditor._XmlNodePaths; 
            }
            //set { XslEditor._XmlNodePaths = value; }
        }

        private static void XmlNodePathAddChildren(DiversityWorkbench.XslEditor.XslNode XslNode)
        {
            if (XslNode.XslNodes.Count > 0)
            {
                foreach (DiversityWorkbench.XslEditor.XslNode N in XslNode.XslNodes)
                {
                    string Path = N.Name;
                    if (N.ParentXslNode != null)
                        Path = N.ParentXslNode.XmlNodePath + "/" + Path;
                    if (!DiversityWorkbench.XslEditor.XslEditor._XmlNodePaths.ContainsKey(Path))
                    {
                        DiversityWorkbench.XslEditor.XslEditor._XmlNodePaths.Add(Path, N);
                        if (N.XslNodes.Count > 0)
                            DiversityWorkbench.XslEditor.XslEditor.XmlNodePathAddChildren(N);
                    }
                }
            }
        }


        #endregion

        #region Analyse XSLT

        public void AnalyseXSLTfile(string XsltFile)
        {
            if (XsltFile.Length == 0)
                return;
            this._XsltFile = new System.IO.FileInfo(XsltFile);
            System.Xml.XmlTextReader R = new System.Xml.XmlTextReader(XsltFile);
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    else if (R.NodeType == System.Xml.XmlNodeType.Comment)
                    {
                        DiversityWorkbench.XslEditor.XslNode P = new XslNode();
                        DiversityWorkbench.XslEditor.XslNode ND = this.ReadXslNode(ref R, ref P);
                        ND.XslNodeType = "Comment";
                        this.XslDocumentNodes.Add(ND);
                    }
                    else
                    {
                        switch (R.Name)
                        {
                            case "xml":
                            case "xsl:stylesheet":
                            case "xsl:output":
                                DiversityWorkbench.XslEditor.XslNode P = new XslNode();
                                DiversityWorkbench.XslEditor.XslNode ND = this.ReadXslNode(ref R, ref P);
                                if (ND != null)
                                    this.XslDocumentNodes.Add(ND);
                                break;
                            //case "xsl:include":
                            //    this.ReadInclude(ref R);
                            //    break;
                            case "xsl:include":
                                this.ReadIncludedFile(ref R);
                                break;
                            case "xsl:variable":
                                this.ReadVariable(ref R);
                                break;
                            case "xsl:template":
                                this.ReadXslTemplate(ref R);
                                break;
                            default:
                                continue;
                                break;
                        }
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

        private void ReadDocumentPart(ref System.Xml.XmlTextReader R)
        {
            try
            {
                string DocumentPart = R.GetAttribute(0);
                DiversityWorkbench.XslEditor.XslNode P = new XslNode();
                DiversityWorkbench.XslEditor.XslNode N = this.ReadXslNode(ref R, ref P);
                this.XslIncludes.Add(DocumentPart, N);
                this.XslDocumentNodes.Add(N);
                return;
            }
            catch (System.Exception ex) { }
        }

        private void ReadInclude(ref System.Xml.XmlTextReader R)
        {
            try
            {
                //string IncludedFile = R.GetAttribute(0);
                DiversityWorkbench.XslEditor.XslNode P = new XslNode();
                DiversityWorkbench.XslEditor.XslNode N = this.ReadXslNode(ref R, ref P);
                this.XslIncludes.Add(N.DisplayText, N);
                //this.XslIncludes.Add(IncludedFile, N);
                this.XslDocumentNodes.Add(N);
                
                System.IO.FileInfo Include = new System.IO.FileInfo(this._XsltFile.DirectoryName + "\\" + N.DisplayText);
                if (Include.Exists)
                {
                    DiversityWorkbench.XslEditor.XslEditor XslInclude = new XslEditor();
                    XslInclude.AnalyseXSLTfile(Include.FullName);
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> KV in XslInclude.XslVariables)
                    {
                        try
                        {
                            this.XslIncludedVariables.Add(KV.Key, KV.Value);
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                        }
                    }
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> KV in XslInclude.XslTemplates)
                    {
                        try
                        {
                            this.XslIncludedTemplates.Add(KV.Key, KV.Value);
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                        }
                    }
                }
                return;
            }
            catch (System.Exception ex) { }
        }

        private void ReadIncludedFile(ref System.Xml.XmlTextReader R)
        {
            try
            {
                //string IncludedFile = R.GetAttribute(0);
                DiversityWorkbench.XslEditor.XslNode P = new XslNode();
                DiversityWorkbench.XslEditor.XslNode N = this.ReadXslNode(ref R, ref P);
                //this.XslIncludes.Add(N.DisplayText, N);
                //this.XslIncludes.Add(IncludedFile, N);
                this.XslDocumentNodes.Add(N);

                System.IO.FileInfo Include = new System.IO.FileInfo(this._XsltFile.DirectoryName + "\\" + N.DisplayText);
                if (Include.Exists)
                {
                    DiversityWorkbench.XslEditor.XslEditor XslInclude = new XslEditor();
                    this.XslIncludedFiles.Add(Include.Name, XslInclude);
                    XslInclude.AnalyseXSLTfile(Include.FullName);
                    //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> KV in XslInclude.XslVariables)
                    //{
                    //    try
                    //    {
                    //        this.XslIncludedVariables.Add(KV.Key, KV.Value);
                    //    }
                    //    catch (System.Exception ex)
                    //    {
                    //        System.Windows.Forms.MessageBox.Show(ex.Message);
                    //    }
                    //}
                    //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> KV in XslInclude.XslTemplates)
                    //{
                    //    try
                    //    {
                    //        this.XslIncludedTemplates.Add(KV.Key, KV.Value);
                    //    }
                    //    catch (System.Exception ex)
                    //    {
                    //        System.Windows.Forms.MessageBox.Show(ex.Message);
                    //    }
                    //}
                }
                return;
            }
            catch (System.Exception ex) { }
        }

        private void ReadVariable(ref System.Xml.XmlTextReader R)
        {
            try
            {
                DiversityWorkbench.XslEditor.XslNode P = new XslNode();
                DiversityWorkbench.XslEditor.XslNode N = this.ReadXslNode(ref R, ref P);
                this.XslVariables.Add(N.DisplayText, N);
                this.XslDocumentNodes.Add(N);
                return;
            }
            catch (System.Exception ex) { }
        }

        private void ReadXslTemplate(ref System.Xml.XmlTextReader R)
        {
            try
            {
                //string TemplateName = R.GetAttribute(0);
                DiversityWorkbench.XslEditor.XslNode P = new XslNode();
                DiversityWorkbench.XslEditor.XslNode N = this.ReadXslNode(ref R, ref P);
                this.XslTemplates.Add(N.DisplayText, N);
                this.XslDocumentNodes.Add(N);
            }
            catch (System.Exception ex) { }
        }

        private DiversityWorkbench.XslEditor.XslNode ReadXslNode(ref System.Xml.XmlTextReader R, ref DiversityWorkbench.XslEditor.XslNode ParentNode)
        {
            DiversityWorkbench.XslEditor.XslNode N = new XslNode(); ;
            try
            {
                if (R.NodeType == System.Xml.XmlNodeType.Comment)
                {
                    N = new XslNode(R.NodeType.ToString(), R.Value.ToString());
                    N.Content = R.Value;
                    //DiversityWorkbench.XslEditor.XslNode NContent = new XslNode(R.Value.ToString());
                    //NContent.XslNodeType = "Comment";
                    //NContent.ParentXslNode = N;
                    //N.XslNodes.Add(NContent);
                    N.XslNodeType = "Comment";
                    return N;
                }
                if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                {
                    return null;
                }
                string XslNodeType = R.Name;
                string NodeName = R.Name;
                System.Collections.Generic.Dictionary<string, string> Att = new Dictionary<string, string>();
                if (R.AttributeCount > 0)
                {
                    for (int i = 0; i < R.AttributeCount; i++)
                    {
                        R.MoveToAttribute(i);
                        Att.Add(R.Name, R.Value);
                        //if (R.Name == "name" || R.Name == "match")
                        //    NodeName = R.Value;
                    }
                }
                R.MoveToElement();

                if (XslNodeType == NodeName)
                {
                    N = new XslNode(NodeName, Att, this);
                }
                else
                {
                    N = new XslNode(XslNodeType, NodeName, Att);
                }
                if (ParentNode.Name.Length > 0)
                {
                    ParentNode.XslNodes.Add(N);
                    N.ParentXslNode = ParentNode;
                }
                if (R.IsEmptyElement)
                    return N;
                else if (N.Name == "xml"
                    || N.Name == "xsl:stylesheet"
                    || N.Name == "xsl:output")
                    return N;
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                    {
                        if (R.Name == NodeName && R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return N;
                        if (R.Name == ParentNode.Name && R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return N;
                        if (R.Name == "xsl:template" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return N;
                        else if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return N;
                        else
                            return N;
                    }
                    else if (R.NodeType == System.Xml.XmlNodeType.Element)
                    {
                        this.ReadXslNode(ref R, ref N);
                    }
                    else if (R.NodeType == System.Xml.XmlNodeType.Text)
                    {
                        DiversityWorkbench.XslEditor.XslNode NContent = new XslNode(R.Value);
                        N.XslNodes.Add(NContent);
                        //N.Value = R.Value;
                    }
                    else if (R.NodeType == System.Xml.XmlNodeType.Comment)
                    {
                        //System.Collections.Generic.Dictionary<string, string> AttComment = new Dictionary<string, string>();
                        DiversityWorkbench.XslEditor.XslNode NC = new XslNode("Comment", R.Value);
                        NC.XslNodeType = "Comment";
                        NC.Content = R.Value;
                        //DiversityWorkbench.XslEditor.XslNode NContent = new XslNode(R.Value);
                        //NContent.XslNodeType = "Content";
                        //NContent.ParentXslNode = NC;
                        //NC.XslNodes.Add(NContent);
                        N.XslNodes.Add(NC);
                    }
                    else
                    {
                    }
                }
            }
            catch (System.Exception ex) { }
            return N;
        }

        #endregion

        #region Write XSLT file
        
        public void WriteXsltFile(string XsltFile)
        {
            System.IO.StreamWriter sw;
            if (System.IO.File.Exists(XsltFile))
            {
                //System.Windows.Forms.MessageBox.Show("File exists, will be changed");
                sw = new System.IO.StreamWriter(XsltFile, false, Encoding.UTF8);
            }
            else
                sw = new System.IO.StreamWriter(XsltFile, false, Encoding.UTF8);
            try
            {
                string Text = "";
                foreach (DiversityWorkbench.XslEditor.XslNode N in this.XslDocumentNodes)
                {
                    if (N.Name == "xml")
                    {
                        Text = "<?" + N.Name;
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in N.Attributes)
                        {
                            if (Text.Length > 0) Text += " ";
                            Text += KV.Key + "=\"" + KV.Value + "\"";
                        }
                        Text += "?>";
                        sw.WriteLine(Text);
                    }
                    else
                    {
                        int Indent = 0;
                        Text = this.WriteXsltNode(ref sw, N, ref Indent);
                        if (Text.Length > 0)
                            sw.WriteLine(Text);
                    }
                    //    if (N.Name.StartsWith("xsl:"))
                    //    Text = "\t<";
                    //else if (N.XslNodeType == "Comment" || N.Name == "Comment")
                    //    Text = "\t<!--";
                    //if (N.XslNodeType != "Comment" || N.Name != "Comment")
                    //    Text += N.Name;
                    //if (N.Attributes != null && N.Attributes.Count > 0)
                    //{
                    //    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in N.Attributes)
                    //    {
                    //        if (Text.Length > 0) Text += " ";
                    //        Text += KV.Key + "=\"" + KV.Value + "\"";
                    //    }
                    //}
                    //if (N.XslNodeType == "xsl:template")
                    //{
                    //    if (N.XslNodes != null && N.XslNodes.Count > 0)
                    //    {
                    //        Text += ">";
                    //        sw.WriteLine(Text);
                    //        int Indent = 1;
                    //        foreach (DiversityWorkbench.XslEditor.XslNode Nchild in N.XslNodes)
                    //            this.WriteXsltNode(ref sw, Nchild, ref Indent);
                    //        Text = "\t</" + N.Name + ">";
                    //        sw.WriteLine(Text);
                    //    }
                    //}
                    //else
                    //{
                    //    if (N.Name == "xml")
                    //        Text += "?>";
                    //    else
                    //    {
                    //        if (N.Name.StartsWith("xsl:"))
                    //        {
                    //            if (N.Value != null && N.Value.Length > 0)
                    //                Text += ">" + N.Value + "</" + N.Name + ">";
                    //            else
                    //            {
                    //                if (N.Name != "xsl:stylesheet")
                    //                    Text += "/";
                    //                Text += ">";
                    //            }
                    //        }
                    //        else if (N.XslNodeType == "Comment" || N.Name == "Comment")
                    //        {
                    //            if (N.Value != null && N.Value.Length > 0)
                    //                Text += N.Value;
                    //            Text += "-->";
                    //        }
                    //    }
                    //    sw.WriteLine(Text);
                    //}
                }
                sw.WriteLine("</xsl:stylesheet>");
            }
            catch (System.Exception ex) { }
            finally
            {
                sw.Close();
            }
        }

        /// <summary>
        /// Writing a XSLT node
        /// </summary>
        /// <param name="sw">the stream writer for writing in the file</param>
        /// <param name="N">The Node that should be written</param>
        /// <param name="Indent">The number of indet tabs</param>
        /// <returns>If the node does not write itself, e.g. if only contains content or nodes without content, it returns a text that must be written by the calling node</returns>
        private string WriteXsltNode(ref System.IO.StreamWriter sw, DiversityWorkbench.XslEditor.XslNode N, ref int Indent)
        {
            bool NodeWritesItself = false;
            string Text = "";
            //for (int i = 0; i < Indent; i++)
            //    Text += "\t";
            string TextIndent = Text;
            if (N.Name == "Comment" || N.XslNodeType == "Comment")
            {
                Text += "<!--";
                foreach (DiversityWorkbench.XslEditor.XslNode NC in N.XslNodes)
                    Text += NC.Content;
                Text += "-->";
                sw.WriteLine(Text);
                NodeWritesItself = true;
            }
            else if ((N.Name == "" || N.XslNodeType == "Content") && N.Content != null && N.Content.Length > 0)
            {
                Text = N.Content;
                //sw.Write(N.Content);
                //NodeWritesItself = true;
            }
            else
            {
                //if (N.Name != "Comment" && N.XslNodeType != "Comment")
                Text += "<" + N.Name;
                //else if (N.Value != null && N.Value.Length > 0)
                //    Text += "<!--" + N.Value + "-->";
                if (N.Attributes != null && N.Attributes.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in N.Attributes)
                    {
                        if (Text.Length > 0) Text += " ";
                        Text += KV.Key + "=\"" + KV.Value + "\"";
                    }
                }
                if (N.XslNodes != null && N.XslNodes.Count > 0)
                {
                    if (N.XslNodes.Count == 1 && N.XslNodes[0].Name == "" && N.XslNodes[0].Content != null)
                    {
                        Text += ">" + N.XslNodes[0].Content + "</" + N.Name + ">";
                        sw.WriteLine(Text);
                        NodeWritesItself = true;
                    }
                    else if (N.XslNodes.Count == 1 && N.XslNodes[0].XslNodes.Count == 0 && N.XslNodes[0].Name == null && N.XslNodes[0].Content.Length > 0)
                    {
                        Text += ">";
                        Text += N.XslNodes[0].Content;
                        Text += "</" + N.Name + ">";
                    }
                    else if (N.XslNodes.Count == 1 && N.XslNodes[0].XslNodes.Count == 0)
                    {
                        Text += ">";
                        Text += this.WriteXsltNode(ref sw, N.XslNodes[0], ref Indent);
                        Text += "</" + N.Name + ">";
                    }
                    else
                    {
                        Text += ">";
                        sw.WriteLine(Text);
                        Indent++;
                        {
                            //Text = this.WriteXsltNode(ref sw, N, ref Indent);
                            //if (Text.Length > 0)
                            //    sw.WriteLine(Text);
                            Text = "";
                            foreach (DiversityWorkbench.XslEditor.XslNode Nchild in N.XslNodes)
                                Text += this.WriteXsltNode(ref sw, Nchild, ref Indent);
                            if (Text.Length > 0)
                                sw.WriteLine(Text);
                            Text = "</" + N.Name + ">";
                            //Text = TextIndent + "</" + N.Name + ">";
                        }
                        sw.WriteLine(Text);
                        NodeWritesItself = true;
                    }
                }
                else
                {
                    if (N.Name != "xsl:stylesheet")
                    {
                        Text += "/>";
                    }
                    else
                    {
                        Text += ">";
                        sw.WriteLine(Text);
                        NodeWritesItself = true;
                    }
                }
            }
            if (NodeWritesItself)
                Text = "";
            return Text;
        }

        #endregion

        #region Template

        public void CreateTemplateHierarchy(string Template, System.Windows.Forms.TreeView Tree, bool FromIncluded)
        {
            Tree.Nodes.Clear();
            try
            {
                System.Windows.Forms.TreeNode TN = new System.Windows.Forms.TreeNode(Template);
                TN.NodeFont = DiversityWorkbench.XslEditor.XslEditor.FontTemplate;
                DiversityWorkbench.XslEditor.XslNode StartNode;
                if (this.XslTemplates.ContainsKey(Template) && !FromIncluded)
                {
                    TN.Tag = this.XslTemplates[Template];
                    StartNode = this.XslTemplates[Template];
                    TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
                }
                else if (this.XslIncludedTemplates.ContainsKey(Template) && FromIncluded)
                {
                    TN.Tag = this.XslIncludedTemplates[Template];
                    StartNode = this.XslIncludedTemplates[Template];
                    TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
                    TN.BackColor = DiversityWorkbench.XslEditor.XslEditor.ColorIncluded;
                }
                else
                    StartNode = new XslNode();

                Tree.Nodes.Add(TN);
                foreach (DiversityWorkbench.XslEditor.XslNode XN in StartNode.XslNodes)
                {
                    System.Windows.Forms.TreeNode CN = DiversityWorkbench.XslEditor.XslEditor.XslTreeNode(XN, this);
                    CN.Tag = XN;
                    TN.Nodes.Add(CN);
                    DiversityWorkbench.XslEditor.XslEditor.AddChildNodes(CN, XN, this);
                }
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Tree for nodes
        
        public static void CreateNodeHierarchy(DiversityWorkbench.XslEditor.XslNode XslNode, System.Windows.Forms.TreeView Tree, DiversityWorkbench.XslEditor.XslEditor Editor)
        {
            Tree.Nodes.Clear();
            try
            {
                System.Windows.Forms.TreeNode TN = DiversityWorkbench.XslEditor.XslEditor.XslTreeNode(XslNode, Editor);
                Tree.Nodes.Add(TN);
                TN.Tag = XslNode;
                foreach (DiversityWorkbench.XslEditor.XslNode XN in XslNode.XslNodes)
                {
                    if (XN.ParentXslNode == null)
                        XN.ParentXslNode = XslNode;
                    System.Windows.Forms.TreeNode CN = DiversityWorkbench.XslEditor.XslEditor.XslTreeNode(XN, Editor);
                    CN.Tag = XN;
                    XN.TreeNode = CN;
                    TN.Nodes.Add(CN);
                    DiversityWorkbench.XslEditor.XslEditor.AddChildNodes(CN, XN, Editor);
                }
            }
            catch (System.Exception ex) { }
        }

        private static void AddChildNodes(System.Windows.Forms.TreeNode TeeNode, DiversityWorkbench.XslEditor.XslNode XslNode, DiversityWorkbench.XslEditor.XslEditor XslEditor)
        {
            try
            {
                foreach (DiversityWorkbench.XslEditor.XslNode XN in XslNode.XslNodes)
                {
                    if (XN.ParentXslNode == null)
                        XN.ParentXslNode = XslNode;
                    System.Windows.Forms.TreeNode CN = DiversityWorkbench.XslEditor.XslEditor.XslTreeNode(XN, XslEditor);
                    CN.Tag = XN;
                    XN.TreeNode = CN;
                    TeeNode.Nodes.Add(CN);
                    DiversityWorkbench.XslEditor.XslEditor.AddChildNodes(CN, XN, XslEditor);
                }
            }
            catch (System.Exception ex) { }
        }

        public static System.Windows.Forms.TreeNode XslTreeNode(DiversityWorkbench.XslEditor.XslNode XslNode, DiversityWorkbench.XslEditor.XslEditor XslEditor)
        {
            string NodeText = XslNode.DisplayText;
            System.Windows.Forms.TreeNode TN = new System.Windows.Forms.TreeNode(NodeText);

            try
            {
                // Color and font
                if (XslNode.XslNodeType == "xsl:call-template"
                    || XslNode.Name == "xsl:apply-templates"
                    || XslNode.XslNodeType == "xsl:template")
                {
                    TN.NodeFont = DiversityWorkbench.XslEditor.XslEditor.FontTemplate;
                    if (XslEditor._XslTemplates.ContainsKey(NodeText))
                        TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
                    else if (XslEditor._XslIncludedTemplates != null
                        && XslEditor._XslIncludedTemplates.ContainsKey(NodeText))
                    {
                        TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
                        TN.BackColor = DiversityWorkbench.XslEditor.XslEditor.ColorIncluded;
                    }
                    else
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> KV in XslEditor._XslTemplates)
                        {
                        }
                    }
                }
                if (XslNode.XslNodeType == "xsl:variable")
                {
                    TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
                    if (XslEditor._XslIncludedVariables != null
                        && XslEditor._XslIncludedVariables.ContainsKey(NodeText))
                    {
                        TN.BackColor = DiversityWorkbench.XslEditor.XslEditor.ColorIncluded;
                    }
                }
                if (XslNode.Name == "Comment" || XslNode.XslNodeType == "Comment")
                    TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorComment;
                else if (XslNode.ParentXslNode != null && (XslNode.ParentXslNode.Name == "Comment" || XslNode.ParentXslNode.XslNodeType == "Comment"))
                    TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorComment;
                else if (XslNode.Name != null
                    && !XslNode.Name.StartsWith("xsl:")
                    && !XslNode.XslNodeType.StartsWith("xsl:")
                    && XslNode.XslNodeType != "Comment")
                    TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorHTML;
                else if (XslNode.Name.StartsWith("xsl:"))
                {
                    if (XslNode.Attributes.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in XslNode.Attributes)
                        {
                            if (KV.Value.StartsWith("$"))
                            {
                                string Variable = KV.Value.Substring(1);
                                if (Variable.IndexOf(" ") > -1)
                                    Variable = Variable.Substring(0, Variable.IndexOf(" "));
                                if (XslNode.XslEditor._XslVariables.ContainsKey(Variable))
                                    TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
                                else if (XslNode.XslEditor._XslIncludedVariables.ContainsKey(Variable))
                                {
                                    TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
                                    TN.BackColor = DiversityWorkbench.XslEditor.XslEditor.ColorIncluded;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
            return TN;
        }

        private static System.Drawing.Font FontTemplate
        {
            get
            {
                System.Drawing.Font F = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                return F;
            }
        }
        
        #endregion

        private static System.Collections.Generic.Dictionary<string, string> _XsltElements;
        public static System.Collections.Generic.Dictionary<string, string> XsltElements
        {
            get
            {
                if (DiversityWorkbench.XslEditor.XslEditor._XsltElements == null)
                {
                    DiversityWorkbench.XslEditor.XslEditor._XsltElements = new Dictionary<string, string>();
                    DiversityWorkbench.XslEditor.XslEditor._XsltElements.Add("xsl:apply-templates", "Apply template");
                    DiversityWorkbench.XslEditor.XslEditor._XsltElements.Add("xsl:call-templates", "Call template");
                    DiversityWorkbench.XslEditor.XslEditor._XsltElements.Add("xsl:value-of", "Value of");
                    DiversityWorkbench.XslEditor.XslEditor._XsltElements.Add("xsl:choose", "Choose");
                    DiversityWorkbench.XslEditor.XslEditor._XsltElements.Add("xsl:for-each", "For each");
                    DiversityWorkbench.XslEditor.XslEditor._XsltElements.Add("xsl:if", "If");
                    DiversityWorkbench.XslEditor.XslEditor._XsltElements.Add("xsl:sort", "Sort");
                }
                return DiversityWorkbench.XslEditor.XslEditor._XsltElements;
            }
        }

        private static System.Collections.Generic.Dictionary<string, string> _XsltElementsForChoose;
        public static System.Collections.Generic.Dictionary<string, string> XsltElementsForChoose
        {
            get
            {
                if (DiversityWorkbench.XslEditor.XslEditor._XsltElementsForChoose == null)
                {
                    DiversityWorkbench.XslEditor.XslEditor._XsltElementsForChoose = new Dictionary<string, string>();
                    DiversityWorkbench.XslEditor.XslEditor._XsltElementsForChoose.Add("xsl:when", "When");
                    DiversityWorkbench.XslEditor.XslEditor._XsltElementsForChoose.Add("xsl:otherwise", "Otherwise");
                }
                return DiversityWorkbench.XslEditor.XslEditor._XsltElementsForChoose;
            }
        }

        public static System.Collections.Generic.Dictionary<string, string> XsltElementsAll
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> D = new Dictionary<string, string>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.XslEditor.XslEditor.XsltElements)
                    D.Add(KV.Key, KV.Value);
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.XslEditor.XslEditor.XsltElementsForChoose)
                    D.Add(KV.Key, KV.Value);
                return D;
            }
        }

        public static System.Collections.Generic.Dictionary<string, string> NewNodeNames(DiversityWorkbench.XslEditor.XslNode N)
        {
            System.Collections.Generic.Dictionary<string, string> NewNames = new Dictionary<string, string>();
            switch (N.Name)
            {
                case "body":
                    break;
                case "table":
                    NewNames.Add("tr", "Table row");
                    break;
                case "tr":
                    NewNames.Add("td", "Table data field");
                    NewNames.Add("th", "Table header");
                    break;
                case "td":
                    break;
            }
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.XslEditor.XslEditor.XsltElements)
                NewNames.Add(KV.Key, KV.Value);
            return NewNames;
        }

        public static System.Collections.Generic.List<string> AdditionalAttributesForXsltNodes(DiversityWorkbench.XslEditor.XslNode N)
        {
            System.Collections.Generic.List<string> Items = new List<string>();
            switch (N.Name)
            {
                case "xsl:apply-templates":
                    if (!N.Attributes.ContainsKey("select"))
                        Items.Add("select");
                    break;
                case "xsl:call-templates":
                    if (!N.Attributes.ContainsKey("name"))
                        Items.Add("name");
                    break;
                case "xsl:value-of":
                    if (!N.Attributes.ContainsKey("select"))
                        Items.Add("select");
                    break;
                case "xsl:choose":
                    break;
                case "xsl:when":
                    if (!N.Attributes.ContainsKey("test"))
                        Items.Add("test");
                    break;
                case "xsl:otherwise":
                    break;
                case "xsl:for-each":
                    if (!N.Attributes.ContainsKey("select"))
                        Items.Add("select");
                    break;
                case "xsl:if":
                    if (!N.Attributes.ContainsKey("test"))
                        Items.Add("test");
                    break;
                case "xsl:sort":
                    if (!N.Attributes.ContainsKey("select"))
                        Items.Add("select");
                    break;
                case "td":
                    if (!N.Attributes.ContainsKey("align"))
                        Items.Add("align");
                    if (!N.Attributes.ContainsKey("valign"))
                        Items.Add("valign");
                    if (!N.Attributes.ContainsKey("colspan"))
                        Items.Add("colspan");
                    break;
                default:
                    break;
            }
            return Items;
        }

        public static System.Collections.Generic.List<string> HtmlAttributes(string NodeName)
        {
            System.Collections.Generic.List<string> L = new List<string>();
            switch (NodeName)
            {
                case "align":
                    L.Add("left");
                    L.Add("center");
                    L.Add("right");
                    L.Add("char");
                    break;
                case "valign":
                    L.Add("top");
                    L.Add("middle");
                    L.Add("buttom");
                    L.Add("baseline");
                    break;
                case "text-transform":
                    L.Add("capitalize");
                    L.Add("uppercase");
                    L.Add("lowercase");
                    L.Add("none");
                    break;
                case "font-family":
                    L.Add("serif");
                    L.Add("sans-serif");
                    L.Add("cursive");
                    L.Add("fantasy");
                    L.Add("monospace");
                    L.Add("Arial");
                    L.Add("Helvetica");
                    L.Add("Verdana");
                    L.Add("Times New Roman");
                    break;
                case "font-style":
                    L.Add("italic");
                    L.Add("oblique");
                    L.Add("normal");
                    break;
            }
            return L;
        }

        public static System.Collections.Generic.List<string> HtmlFontFormats()
        {
            System.Collections.Generic.List<string> L = new List<string>();
            L.Add("color");

            L.Add("font");
            L.Add("font-family");
            L.Add("font-size");
            L.Add("font-stretch");
            L.Add("font-style");
            L.Add("font-variant");
            L.Add("font-weight");

            L.Add("letter-spacing");

            L.Add("text-decoration");
            L.Add("text-shadow");
            L.Add("text-transform");

            L.Add("word-spacing");
            return L;
        }

        public static System.Collections.Generic.List<string> HtmlFontFormatAttributes(string NodeName)
        {
            System.Collections.Generic.List<string> L = new List<string>();
            switch (NodeName)
            {
                case "align":
                    L.Add("left");
                    L.Add("center");
                    L.Add("right");
                    L.Add("char");
                    break;
                case "valign":
                    L.Add("top");
                    L.Add("middle");
                    L.Add("buttom");
                    L.Add("baseline");
                    break;
                case "text-transform":
                    L.Add("capitalize");
                    L.Add("uppercase");
                    L.Add("lowercase");
                    L.Add("none");
                    break;
                case "font-family":
                    L.Add("serif");
                    L.Add("sans-serif");
                    L.Add("cursive");
                    L.Add("fantasy");
                    L.Add("monospace");
                    L.Add("Arial");
                    L.Add("Helvetica");
                    L.Add("Verdana");
                    L.Add("Times New Roman");
                    break;
                case "font-style":
                    L.Add("italic");
                    L.Add("oblique");
                    L.Add("normal");
                    break;
            }
            return L;
        }

    }
}
