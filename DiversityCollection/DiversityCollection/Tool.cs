using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection
{
    class Tool : HierarchicalEntity
    {
        #region Parameter

        private DiversityWorkbench.QueryCondition _QueryConditionProject;

        #endregion

        #region Construction

        public Tool(
            ref System.Data.DataSet Dataset,
            System.Data.DataTable DataTable,
            ref System.Windows.Forms.TreeView TreeView,
            System.Windows.Forms.Form Form,
            DiversityWorkbench.UserControls.UserControlQueryList UserControlQueryList,
            System.Windows.Forms.SplitContainer SplitContainerMain,
            System.Windows.Forms.SplitContainer SplitContainerData,
            System.Windows.Forms.ToolStripButton ToolStripButtonSpecimenList,
            //System.Windows.Forms.ImageList ImageListSpecimenList,
            DiversityCollection.UserControls.UserControlSpecimenList UserControlSpecimenList,
            System.Windows.Forms.HelpProvider HelpProvider,
            System.Windows.Forms.ToolTip ToolTip,
            ref System.Windows.Forms.BindingSource BindingSource)
            : base(ref Dataset, DataTable, ref TreeView, Form, UserControlQueryList, SplitContainerMain,
            SplitContainerData, ToolStripButtonSpecimenList, /*ImageListSpecimenList,*/ UserControlSpecimenList,
            HelpProvider, ToolTip, ref BindingSource, DiversityCollection.LookupTable.DtToolHierarchy, DiversityCollection.LookupTable.DtToolHierarchy)
        {
            //this._SpecimenTable = "IdentificationUnitAnalysis";
            this._sqlItemFieldList = " ToolID, ToolParentID, Name, Description, ToolURI, ToolUsageTemplate, OnlyHierarchy, Notes ";
            this._MainTable = "Tool";
        }
        
        #endregion

        #region Functions and properties

        protected override string SqlSpecimenCount(int ID)
        {
            return "";//SELECT COUNT(*) FROM IdentificationUnitAnalysis WHERE AnalysisID = " + ID.ToString();
        }

        #endregion

        #region Datahandling of dependent tables

        protected virtual void setDependentTable(int ID)
        {
            this.saveDependentTables();
        }

        public override void saveDependentTables() 
        {
            //this.SaveXmlTree();
            this.FormFunctions.updateTable(this._DataSet, "ToolForAnalysis", this._SqlDataAdapterDepend, this._BindingSource);
            this.FormFunctions.updateTable(this._DataSet, "ToolForProcessing", this._SqlDataAdapterDepend_2, this._BindingSource);
        }

        public override void fillDependentTables(int ID) 
        {
            try
            {
                string SQL = "SELECT AnalysisID, ToolID FROM ToolForAnalysis WHERE ToolID = " + ID.ToString();
                if (this._SqlDataAdapterDepend == null)
                    this._SqlDataAdapterDepend = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                else
                    this._SqlDataAdapterDepend.SelectCommand.CommandText = SQL;
                FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend, SQL, this._DataSet.Tables["ToolForAnalysis"]);
                this._DataSet.Tables["ToolForAnalysis"].Clear();
                this._SqlDataAdapterDepend.Fill(this._DataSet.Tables["ToolForAnalysis"]);

                SQL = "SELECT T.AnalysisID, ToolID, HierarchyDisplayText AS DisplayText FROM ToolForAnalysis T, dbo.AnalysisHierarchyAll() A WHERE A.AnalysisID = T.AnalysisID AND ToolID = " + ID.ToString();
                this._DataSet.Tables["ToolForAnalysisList"].Clear();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this._DataSet.Tables["ToolForAnalysisList"]);

                SQL = "SELECT ProcessingID, ToolID FROM ToolForProcessing WHERE ToolID = " + ID.ToString();
                if (this._SqlDataAdapterDepend_2 == null)
                    this._SqlDataAdapterDepend_2 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                else
                    this._SqlDataAdapterDepend_2.SelectCommand.CommandText = SQL;
                FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_2, SQL, this._DataSet.Tables["ToolForProcessing"]);
                this._DataSet.Tables["ToolForProcessing"].Clear();
                this._SqlDataAdapterDepend_2.Fill(this._DataSet.Tables["ToolForProcessing"]);

                SQL = "SELECT T.ProcessingID, ToolID, HierarchyDisplayText AS DisplayText FROM ToolForProcessing T, dbo.ProcessingHierarchyAll() A WHERE A.ProcessingID = T.ProcessingID AND ToolID = " + ID.ToString();
                this._DataSet.Tables["ToolForProcessingList"].Clear();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(this._DataSet.Tables["ToolForProcessingList"]);

                this.setXmlTree();
            }
            catch (System.Exception ex) { }
        }

        private void setXmlTree()
        {
            try
            {
                DiversityCollection.Forms.FormTool FT = (DiversityCollection.Forms.FormTool)this._Form;
                FT.buildToolTree();
            }
            catch (Exception ex)
            {
            }
        }

        private void SaveXmlTree()
        {
            try
            {
                DiversityCollection.Forms.FormTool FT = (DiversityCollection.Forms.FormTool)this._Form;
                FT.writeToolTreeToXml();
            }
            catch (Exception ex)
            {
            }
        }
        
        #endregion

        #region Interface

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions
        {
            get
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();
                string Description = "";
                string SQL = "";

                #region Analysis

                //System.Data.DataTable dtAnalysis = new System.Data.DataTable();
                //SQL = "SELECT AnalysisID AS [Value], DisplayText AS Display " +
                //    "FROM Analysis " +
                //    "ORDER BY Display";
                //Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                //if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                //{
                //    try { a.Fill(dtAnalysis); }
                //    catch { }
                //}
                //if (dtAnalysis.Columns.Count > 0)
                //{
                //    Description = DiversityWorkbench.Functions.ColumnDescription("ProjectProxy", "Project");
                //    this._QueryConditionProject = new DiversityWorkbench.QueryCondition(true, "ToolForAnalysis", "ToolID", "AnalysisID", "Analysis", "Analysis", "Analysis", Description, dtAnalysis, true);
                //    QueryConditions.Add(this._QueryConditionProject);
                //}

                #endregion

                #region Tool

                Description = this.FormFunctions.ColumnDescription("Tool", "Name");
                DiversityWorkbench.QueryCondition q0 = new DiversityWorkbench.QueryCondition(true, "Tool", "ToolID", "Name", "Tool", "Name", "Name", Description);
                QueryConditions.Add(q0);

                Description = this.FormFunctions.ColumnDescription("Tool", "Description");
                DiversityWorkbench.QueryCondition q1 = new DiversityWorkbench.QueryCondition(true, "Tool", "ToolID", "Description", "Tool", "Description", "Description", Description);
                QueryConditions.Add(q1);

                //Description = this.FormFunctions.ColumnDescription("Tool", "MeasurementUnit");
                //DiversityWorkbench.QueryCondition qm = new DiversityWorkbench.QueryCondition(true, "Tool", "ToolID", "MeasurementUnit", "Tool", "Unit", "Measurement unit", Description);
                //QueryConditions.Add(qm);

                Description = this.FormFunctions.ColumnDescription("Tool", "Notes");
                DiversityWorkbench.QueryCondition q2 = new DiversityWorkbench.QueryCondition(true, "Tool", "ToolID", "Notes", "Tool", "Notes", "Notes", Description);
                QueryConditions.Add(q2);

                Description = this.FormFunctions.ColumnDescription("Tool", "ToolURI");
                DiversityWorkbench.QueryCondition q3 = new DiversityWorkbench.QueryCondition(true, "Tool", "ToolID", "ToolURI", "Tool", "URI", "URI", Description);
                QueryConditions.Add(q3);
                
                #endregion

                return QueryConditions;
            }
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns
        {
            get
            {
                DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
                QueryDisplayColumns[0].DisplayText = "Tool";
                QueryDisplayColumns[0].DisplayColumn = "Name";
                QueryDisplayColumns[0].OrderColumn = "Name";
                QueryDisplayColumns[0].IdentityColumn = "ToolID";
                QueryDisplayColumns[0].TableName = "Tool";
                return QueryDisplayColumns;
            }
        }

        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> ToolForAnalysis(int AnalysisID)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> Dict = new Dictionary<string, List<int>>();
            try
            {
                string SQL = "SELECT T.ToolID, T.Name " +
                    "FROM ToolForAnalysis A, Tool T  WHERE AnalysisID = " + AnalysisID.ToString() + " " +
                    "AND T.ToolID = A.ToolID";
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Tool = R[1].ToString();
                    int ToolID = int.Parse(R[0].ToString());
                    if (!Dict.ContainsKey(Tool))
                    {
                        System.Collections.Generic.List<int> L = new List<int>();
                        L.Add(ToolID);
                        Dict.Add(Tool, L);
                    }
                    else
                    {
                        Dict[Tool].Add(ToolID);
                    }
                }

            }
            catch (System.Exception ex)
            {
            }
            return Dict;
        }

        #endregion

        #region ToolUsage
        
        public static void BuildToolTreeFromXmlContent(System.Windows.Forms.TreeView TreeView, string XmlContent)
        {
            try
            {
                TreeView.Nodes.Clear();
                System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(XmlContent, System.Xml.XmlNodeType.Element, null);
                System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                dom.Load(tr);
                if (dom.DocumentElement == null) return;
                if (dom.DocumentElement.ChildNodes.Count >= 0)
                {
                    // SECTION 2. Initialize the TreeView control.
                    TreeView.Nodes.Clear();
                    //if (dom.DocumentElement.Name.Length > 0)
                    TreeView.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name));
                    System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
                    tNode = TreeView.Nodes[0];
                    // SECTION 3. Populate the TreeView with the DOM nodes.
                    DiversityCollection.Tool.AddToolNodeFromXmlContent(dom.DocumentElement, tNode);
                    TreeView.ExpandAll();
                }
                else
                {
                    if (dom.DocumentElement.Attributes["error_message"].InnerText.Length > 0)
                    {
                        string Message = dom.DocumentElement.Attributes["error_message"].InnerText;
                        System.Windows.Forms.MessageBox.Show(Message);
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        public static void AddToolNodeFromXmlContent(System.Xml.XmlNode inXmlNode, System.Windows.Forms.TreeNode inTreeNode)
        {
            System.Xml.XmlNode xNode;
            System.Windows.Forms.TreeNode tNode;
            System.Xml.XmlNodeList nodeList;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            try
            {
                if (inXmlNode.HasChildNodes)
                {
                    nodeList = inXmlNode.ChildNodes;
                    for (i = 0; i <= nodeList.Count - 1; i++)
                    {
                        xNode = inXmlNode.ChildNodes[i];
                        string TreeNodeName = xNode.Name;
                        string TreeNodeValue = xNode.Value;
                        DiversityWorkbench.UserControls.XMLNode N = new DiversityWorkbench.UserControls.XMLNode();
                        N.Name = xNode.Name;
                        if (xNode.Attributes != null && xNode.Attributes.Count > 0)
                        {
                            System.Xml.XmlAttribute attrName = (System.Xml.XmlAttribute)(xNode.Attributes.GetNamedItem("Name"));
                            if (attrName != null)
                                TreeNodeName = attrName.Value;
                            System.Xml.XmlAttribute attrValue = (System.Xml.XmlAttribute)(xNode.Attributes.GetNamedItem("Value"));
                            if (attrValue != null)
                            {
                                TreeNodeValue = attrValue.Value;
                                TreeNodeName += ": " + TreeNodeValue;
                            }
                            N.Attributes = new List<DiversityWorkbench.UserControls.XMLAttribute>();
                            foreach (System.Xml.XmlAttribute A in xNode.Attributes)
                            {
                                DiversityWorkbench.UserControls.XMLAttribute a = new DiversityWorkbench.UserControls.XMLAttribute();
                                a.Name = A.Name;
                                a.Value = A.Value;
                                N.Attributes.Add(a);
                            }
                        }
                        else if (N.Name == "ValueEnum")
                        {
                            TreeNodeName = xNode.InnerText;
                            N.Value = xNode.InnerText;
                        }
                        System.Windows.Forms.TreeNode TN = new System.Windows.Forms.TreeNode(TreeNodeName);
                        if (xNode.Name == "Tool")
                            TN.BackColor = System.Drawing.Color.Yellow;
                        if (N.Name == "ValueEnum")
                            TN.ForeColor = System.Drawing.Color.Gray;
                        TN.Tag = N;
                        inTreeNode.Nodes.Add(TN);
                        if (i >= inTreeNode.Nodes.Count)
                            continue;
                        tNode = inTreeNode.Nodes[i];
                        AddToolNodeFromXmlContent(xNode, tNode);
                    }
                }
                else
                {
                    if (inXmlNode.Value != null)
                    {
                        DiversityWorkbench.UserControls.XMLNode N = new DiversityWorkbench.UserControls.XMLNode();
                        if (inXmlNode.Name == "#text")
                            N.Name = inXmlNode.ParentNode.Name;
                        else
                            N.Name = inXmlNode.Name;
                        string TreeNodeValue = inXmlNode.Value;
                        if (inXmlNode.Attributes != null && inXmlNode.Attributes.Count > 0)
                        {
                            System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute)(inXmlNode.Attributes.GetNamedItem("Value"));
                            if (attr != null)
                                TreeNodeValue = attr.Value;
                        }
                        N.Value = inXmlNode.Value;
                        inTreeNode.Parent.Tag = N;
                        if (N.Name == "ValueEnum")
                        {
                            inTreeNode.Text = N.Value;
                        }
                        else
                        {
                            inTreeNode.Parent.Text = inTreeNode.Parent.Text + ": " + inXmlNode.Value;
                        }
                        inTreeNode.Remove();
                    }
                }
            }
            catch (System.Exception ex) { }

        }

        public static string XmlFromToolTree(System.Windows.Forms.TreeView TreeView)
        {
            System.IO.FileInfo XmlFile = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Temp.xml");

            string XML = "";
            if (TreeView.Nodes.Count == 0) return "";
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            //settings.Encoding = System.Text.Encoding.Unicode;
            W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
            W.WriteStartElement(TreeView.Nodes[0].Text);
            //W.WriteAttributeString("xmlns", "http://diversityworkbench.net/Schema/tools");
            foreach (System.Windows.Forms.TreeNode N in TreeView.Nodes[0].Nodes)
            {
                if (!XmlFromToolTreeAddChild(N, ref W))
                    return "";
            }
            W.WriteEndElement();
            W.Flush();
            W.Close();
            System.IO.StreamReader R = new System.IO.StreamReader(XmlFile.FullName);
            XML = R.ReadToEnd();
            R.Close();
            R.Dispose();
            XmlFile.Delete();
            if (XML.IndexOf("?>") > -1)
                XML = XML.Substring(XML.IndexOf("?>") + 2);
            if (XML.IndexOf("<Tools>") > -1)
                XML = XML.Replace("<Tools>", "<Tools xmlns=\"http://diversityworkbench.net/Schema/tools\">");
            return XML;
        }

        public static bool XmlFromToolTreeAddChild(System.Windows.Forms.TreeNode TreeNode, ref System.Xml.XmlWriter W)
        {
            try
            {
                if (TreeNode.Tag != null)
                {
                    try
                    {
                        DiversityWorkbench.UserControls.XMLNode N = (DiversityWorkbench.UserControls.XMLNode)TreeNode.Tag;

                        if (N.Attributes != null)
                        {
                            W.WriteStartElement(N.Name);
                            foreach (DiversityWorkbench.UserControls.XMLAttribute A in N.Attributes)
                            {
                                W.WriteAttributeString(A.Name, A.Value);
                            }
                            if (TreeNode.Nodes.Count > 0)
                            {
                                foreach (System.Windows.Forms.TreeNode TC in TreeNode.Nodes)
                                    DiversityCollection.Tool.XmlFromToolTreeAddChild(TC, ref W);
                            }
                            W.WriteEndElement();
                        }
                        else
                        {
                            if (TreeNode.Nodes.Count > 0)
                            {
                                W.WriteStartElement(N.Name);
                                DiversityCollection.Tool.XmlFromToolTreeAddChild(TreeNode, ref W);
                                W.WriteEndElement();
                            }
                            else
                                W.WriteElementString(N.Name, N.Value);
                        }

                    }
                    catch (System.Exception ex) { return false; }
                }
                else
                {
                    string Node = TreeNode.Text.Replace(" ", "_");
                    if (Node.IndexOf(":") > -1)
                        Node = Node.Substring(0, Node.IndexOf(":"));
                    W.WriteStartElement(Node);
                    foreach (System.Windows.Forms.TreeNode NChild in TreeNode.Nodes)
                    {
                        if (!XmlFromToolTreeAddChild(NChild, ref W))
                            return false;
                    }
                    W.WriteEndElement();
                }
                return true;
            }
            catch (System.Exception ex) { return false; }
        }

        public static void AddTool(System.Windows.Forms.TreeView TreeView, int ToolID)
        {

        }

        #endregion

    }
}
