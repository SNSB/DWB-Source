using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection
{
    class Method : HierarchicalEntity
    {

        #region Parameter

        private DiversityWorkbench.QueryCondition _QueryConditionProject;

        #endregion

        #region Construction

        public Method(
            ref System.Data.DataSet Dataset,
            System.Data.DataTable DataTable,
            ref System.Windows.Forms.TreeView TreeView,
            System.Windows.Forms.Form Form,
            DiversityWorkbench.UserControls.UserControlQueryList UserControlQueryList,
            System.Windows.Forms.SplitContainer SplitContainerMain,
            System.Windows.Forms.SplitContainer SplitContainerData,
            System.Windows.Forms.ToolStripButton ToolStripButtonSpecimenList,
            DiversityCollection.UserControls.UserControlSpecimenList UserControlSpecimenList,
            System.Windows.Forms.HelpProvider HelpProvider,
            System.Windows.Forms.ToolTip ToolTip,
            ref System.Windows.Forms.BindingSource BindingSource)
            : base(ref Dataset, DataTable, ref TreeView, Form, UserControlQueryList, SplitContainerMain,
            SplitContainerData, ToolStripButtonSpecimenList, UserControlSpecimenList,
            HelpProvider, ToolTip, ref BindingSource, DiversityCollection.LookupTable.DtMethodHierarchy, DiversityCollection.LookupTable.DtMethodHierarchy)
        {
            this._sqlItemFieldList = " MethodID, MethodParentID, DisplayText, Description, MethodURI, OnlyHierarchy, ForCollectionEvent, Notes ";
            this._MainTable = "Method";
        }
        
        #endregion

        #region Functions and properties

        protected override string SqlSpecimenCount(int ID)
        {
            return "declare @i int " +
                "SET @i = (SELECT COUNT(*) FROM [dbo].[IdentificationUnitAnalysisMethod] A where A.MethodID = " + ID.ToString() + ") "+
                "SET @i = @i + (SELECT COUNT(*) FROM [dbo].[CollectionEventMethod] A where A.MethodID = " + ID.ToString() + ") " +
                "select @i";
        }

        #endregion

        #region Datahandling of dependent tables

        protected virtual void setDependentTable(int ID)
        {
            this.saveDependentTables();
        }

        public override void saveDependentTables() 
        {
            try
            {
                string Message = "";
                bool OK = true;
                OK = this.FormFunctions.updateTable(this._DataSet, "MethodForAnalysis", this._SqlDataAdapterDepend, this._BindingSource, ref Message);
                if (!OK && Message.Length > 0)
                {
                    System.Windows.Forms.MessageBox.Show("Saving changes in MethodForAnalysis failed: " + Message);
                    return;
                }
                OK = this.FormFunctions.updateTable(this._DataSet, "MethodForProcessing", this._SqlDataAdapterDepend_2, this._BindingSource, ref Message);
                if (!OK && Message.Length > 0)
                {
                    System.Windows.Forms.MessageBox.Show("Saving changes in MethodForProcessing failed: " + Message);
                    return;
                }
                foreach (System.Data.DataRow R in this._DataSet.Tables["Parameter"].Rows)
                {
                    if (R.RowState != System.Data.DataRowState.Deleted)
                    {
                        R.BeginEdit();
                        R.EndEdit();
                    }
                }
                this.FormFunctions.updateTable(this._DataSet, "Parameter", this._SqlDataAdapterDepend_3, this._BindingSource);
                foreach (System.Data.DataRow R in this._DataSet.Tables["ParameterValue_Enum"].Rows)
                {
                    if (R.RowState != System.Data.DataRowState.Deleted)
                    {
                        R.BeginEdit();
                        R.EndEdit();
                    }
                }
                this.FormFunctions.updateTable(this._DataSet, "ParameterValue_Enum", this._SqlDataAdapterDepend_4, this._BindingSource);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public override void fillDependentTables(int ID) 
        {
            try
            {
                // ANALYSIS
                string SQL = "SELECT MethodID, AnalysisID FROM MethodForAnalysis WHERE MethodID = " + ID.ToString();
                if (this._SqlDataAdapterDepend == null)
                    this._SqlDataAdapterDepend = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                else
                    this._SqlDataAdapterDepend.SelectCommand.CommandText = SQL;
                FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend, SQL, this._DataSet.Tables["MethodForAnalysis"]);
                this._DataSet.Tables["MethodForAnalysis"].Clear();
                this._SqlDataAdapterDepend.Fill(this._DataSet.Tables["MethodForAnalysis"]);

                SQL = "SELECT     M.AnalysisID, M.MethodID, A.DisplayText " +
                    "FROM         MethodForAnalysis M, Analysis A " +
                    "WHERE  A.AnalysisID = M.AnalysisID AND MethodID = " + ID.ToString();
                this._DataSet.Tables["MethodForAnalysisList"].Clear();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this._DataSet.Tables["MethodForAnalysisList"]);

                // PROCESSING
                SQL = "SELECT     ProcessingID, MethodID " +
                    "FROM         MethodForProcessing " +
                    "WHERE     MethodID = " + ID.ToString();
                if (this._SqlDataAdapterDepend_2 == null)
                    this._SqlDataAdapterDepend_2 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                else
                    this._SqlDataAdapterDepend_2.SelectCommand.CommandText = SQL;
                FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_2, SQL, this._DataSet.Tables["MethodForProcessing"]);
                this._DataSet.Tables["MethodForProcessing"].Clear();
                this._SqlDataAdapterDepend_2.Fill(this._DataSet.Tables["MethodForProcessing"]);

                SQL = "SELECT     M.ProcessingID, M.MethodID, A.DisplayText " +
                    "FROM         MethodForProcessing M, Processing A " +
                    "WHERE  A.ProcessingID = M.ProcessingID AND MethodID = " + ID.ToString();
                this._DataSet.Tables["MethodForProcessingList"].Clear();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(this._DataSet.Tables["MethodForProcessingList"]);

                // PARAMETER
                SQL = "SELECT     MethodID, ParameterID, DisplayText, Description, ParameterURI, DefaultValue, Notes " +
                    "FROM         Parameter " +
                    "WHERE     MethodID = " + ID.ToString() +
                    " ORDER BY DisplayText";
                if (this._SqlDataAdapterDepend_3 == null)
                    this._SqlDataAdapterDepend_3 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                else
                    this._SqlDataAdapterDepend_3.SelectCommand.CommandText = SQL;
                FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_3, SQL, this._DataSet.Tables["Parameter"]);
                this._DataSet.Tables["Parameter"].Clear();
                this._SqlDataAdapterDepend_3.Fill(this._DataSet.Tables["Parameter"]);

                // PARAMETER VALUES
                SQL = "SELECT     MethodID, ParameterID, Value, DisplayText, Description, URI " +
                    "FROM         ParameterValue_Enum " +
                    "WHERE     MethodID = " + ID.ToString() +
                    " ORDER BY DisplayText"; ;
                if (this._SqlDataAdapterDepend_4 == null)
                    this._SqlDataAdapterDepend_4 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                else
                    this._SqlDataAdapterDepend_4.SelectCommand.CommandText = SQL;
                FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_4, SQL, this._DataSet.Tables["ParameterValue_Enum"]);
                this._DataSet.Tables["ParameterValue_Enum"].Clear();
                this._SqlDataAdapterDepend_4.Fill(this._DataSet.Tables["ParameterValue_Enum"]);

                //this.setXmlTree();
                this.ItemChanged();
            }
            catch (System.Exception ex) { }
        }

        public override bool deleteDependentData(int ID) 
        {
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                con.Open();
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand("", con);

                // Testing existence of links
                int i = 0;
                C.CommandText = "SELECT COUNT(*) FROM IdentificationUnitAnalysisMethod M WHERE MethodID = " + ID.ToString();
                string Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (Result != "0")
                {
                    if (System.Windows.Forms.MessageBox.Show("This method is still used for " + Result + " analysis of organisms.\r\nDo you really want to delete it?", "Delete?", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                        return false;
                }
                C.CommandText = "SELECT COUNT(*) FROM CollectionEventMethod M WHERE MethodID = " + ID.ToString();
                Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (Result != "0")
                {
                    if (System.Windows.Forms.MessageBox.Show("This method is still used for " + Result + " collection event(s).\r\nDo you really want to delete it?", "Delete?", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                        return false;
                }

                // IdentificationUnitAnalysisMethodParameter
                C.CommandText = "DELETE M FROM IdentificationUnitAnalysisMethodParameter M WHERE MethodID = " + ID.ToString();
                C.ExecuteNonQuery();
                // IdentificationUnitAnalysisMethod
                C.CommandText = "DELETE M FROM IdentificationUnitAnalysisMethod M WHERE MethodID = " + ID.ToString();
                C.ExecuteNonQuery();

                // CollectionSpecimenProcessingMethodParameter
                C.CommandText = "DELETE M FROM CollectionSpecimenProcessingMethodParameter M WHERE MethodID = " + ID.ToString();
                C.ExecuteNonQuery();
                // IdentificationUnitAnalysisMethod
                C.CommandText = "DELETE M FROM CollectionSpecimenProcessingMethod M WHERE MethodID = " + ID.ToString();
                C.ExecuteNonQuery();

                // CollectionEventParameterValue
                C.CommandText = "DELETE M FROM CollectionEventParameterValue M WHERE MethodID = " + ID.ToString();
                C.ExecuteNonQuery();
                // CollectionEventMethod
                C.CommandText = "DELETE M FROM CollectionEventMethod M WHERE MethodID = " + ID.ToString();
                C.ExecuteNonQuery();

                // ANALYSIS
                C.CommandText = "DELETE M FROM MethodForAnalysis M WHERE MethodID = " + ID.ToString();
                C.ExecuteNonQuery();

                // PROCESSING
                C.CommandText = "DELETE M FROM MethodForProcessing M WHERE MethodID = " + ID.ToString();
                C.ExecuteNonQuery();

                // METHOD
                C.CommandText = "DELETE M FROM MethodForProcessing M WHERE MethodID = " + ID.ToString();
                C.ExecuteNonQuery();

                // PARAMETER VALUES
                C.CommandText = "DELETE M FROM ParameterValue_Enum M WHERE MethodID = " + ID.ToString();
                C.ExecuteNonQuery();

                // PARAMETER
                C.CommandText = "DELETE M FROM Parameter M WHERE MethodID = " + ID.ToString();
                C.ExecuteNonQuery();

                con.Close();
                con.Dispose();
            }
            catch (System.Exception ex) 
            {
                System.Windows.Forms.MessageBox.Show("Deleting dependent data failed: " + ex.Message);
                return false;
            }
            return true;
        }

        //private void setXmlTree()
        //{
        //    try
        //    {
        //        DiversityCollection.Forms.FormMethod FT = (DiversityCollection.Forms.FormMethod)this._Form;
        //        FT.buildMethodTree();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void SaveXmlTree()
        //{
        //    try
        //    {
        //        DiversityCollection.Forms.FormMethod FT = (DiversityCollection.Forms.FormMethod)this._Form;
        //        FT.writeMethodTreeToXml();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        
        #endregion

        #region Interface

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions
        {
            get
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();
                string Description = "";
                string SQL = "";

                #region Method

                Description = this.FormFunctions.ColumnDescription("Method", "DisplayText");
                DiversityWorkbench.QueryCondition q0 = new DiversityWorkbench.QueryCondition(true, "Method", "MethodID", "DisplayText", "Method", "DisplayText", "DisplayText", Description);
                QueryConditions.Add(q0);

                Description = this.FormFunctions.ColumnDescription("Method", "Description");
                DiversityWorkbench.QueryCondition q1 = new DiversityWorkbench.QueryCondition(true, "Method", "MethodID", "Description", "Method", "Description", "Description", Description);
                QueryConditions.Add(q1);

                Description = this.FormFunctions.ColumnDescription("Method", "Notes");
                DiversityWorkbench.QueryCondition q2 = new DiversityWorkbench.QueryCondition(true, "Method", "MethodID", "Notes", "Method", "Notes", "Notes", Description);
                QueryConditions.Add(q2);

                Description = this.FormFunctions.ColumnDescription("Method", "MethodURI");
                DiversityWorkbench.QueryCondition q3 = new DiversityWorkbench.QueryCondition(true, "Method", "MethodID", "MethodURI", "Method", "URI", "URI", Description);
                QueryConditions.Add(q3);
                
                Description = this.FormFunctions.ColumnDescription("Method", "MethodID");
                DiversityWorkbench.QueryCondition q4 = new DiversityWorkbench.QueryCondition(true, "Method", "MethodID", "MethodID", "Method", "ID", "ID", Description);
                QueryConditions.Add(q4);
                
                #endregion

                #region Processing

                System.Data.DataTable dtProcessing = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display UNION " +
                    "SELECT ProcessingID, ProcessingParentID, DisplayText " +
                    "FROM Processing " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aProcessing = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aProcessing.Fill(dtProcessing); }
                    catch { }
                }
                if (dtProcessing.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtProcessing.Columns.Add(Value);
                    dtProcessing.Columns.Add(ParentValue);
                    dtProcessing.Columns.Add(Display);
                }
                Description = "The Processings available for a method";
                DiversityWorkbench.QueryCondition qProcessing = new DiversityWorkbench.QueryCondition(false, "MethodForProcessing", "MethodID", "ProcessingID", "Display", "Value", "ParentValue", "Display", "Processing", "Type", "Type of the Processing", Description, dtProcessing, false);
                QueryConditions.Add(qProcessing);
                
                #endregion                

                #region Analysis
                
                System.Data.DataTable dtAnalysis = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display UNION " +
                    "SELECT AnalysisID, AnalysisParentID, DisplayText " +
                    "FROM Analysis " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aAnalysis = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aAnalysis.Fill(dtAnalysis); }
                    catch { }
                }
                if (dtAnalysis.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtAnalysis.Columns.Add(Value);
                    dtAnalysis.Columns.Add(ParentValue);
                    dtAnalysis.Columns.Add(Display);
                }
                Description = "The analysis applied on a specimen";
                DiversityWorkbench.QueryCondition qAnalysis = new DiversityWorkbench.QueryCondition(false, "MethodForAnalysis", "MethodID", "AnalysisID", "Display", "Value", "ParentValue", "Display", "Analysis", "Type", "Type of the analysis", Description, dtAnalysis, false);
                //qAnalysis.ForeignKeySecondColumn = "IdentificationUnitID";
                //qAnalysis.IntermediateTable = "IdentificationUnit";
                //qAnalysis.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                qAnalysis.IsNumeric = true;
                QueryConditions.Add(qAnalysis);
                
                #endregion

                #region Parameter

                Description = this.FormFunctions.TableDescription("Parameter");
                DiversityWorkbench.QueryCondition qParameter = new DiversityWorkbench.QueryCondition(true, "Parameter", "MethodID", "DisplayText", "Parameter", "Parameter", "Parameter", Description);
                QueryConditions.Add(qParameter);
                
                #endregion

                return QueryConditions;
            }
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns
        {
            get
            {
                DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
                QueryDisplayColumns[0].DisplayText = "Method";
                QueryDisplayColumns[0].DisplayColumn = "DisplayText";
                QueryDisplayColumns[0].OrderColumn = "DisplayText";
                QueryDisplayColumns[0].IdentityColumn = "MethodID";
                QueryDisplayColumns[0].TableName = "Method";
                return QueryDisplayColumns;
            }
        }

        public override void ItemChanged() 
        {
            if (this._formMethod != null)
                this._formMethod.SetFormAccordingToItem();
        }

        private DiversityCollection.Forms.FormMethod _formMethod;
        public void setFormMethod(DiversityCollection.Forms.FormMethod f)
        {
            this._formMethod = f;
        }

        #endregion

        #region MethodUsage - deprecated
        
        public static void BuildMethodTreeFromXmlContent(System.Windows.Forms.TreeView TreeView, string XmlContent)
        {
            //try
            //{
            //    TreeView.Nodes.Clear();
            //    System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(XmlContent, System.Xml.XmlNodeType.Element, null);
            //    System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
            //    dom.Load(tr);
            //    if (dom.DocumentElement == null) return;
            //    if (dom.DocumentElement.ChildNodes.Count >= 0)
            //    {
            //        // SECTION 2. Initialize the TreeView control.
            //        TreeView.Nodes.Clear();

            //        TreeView.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name));
            //        System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
            //        tNode = TreeView.Nodes[0];
            //        // SECTION 3. Populate the TreeView with the DOM nodes.
            //        DiversityCollection.Method.AddMethodNodeFromXmlContent(dom.DocumentElement, tNode);
            //        TreeView.ExpandAll();
            //    }
            //    else
            //    {
            //        if (dom.DocumentElement.Attributes["error_message"].InnerText.Length > 0)
            //        {
            //            string Message = dom.DocumentElement.Attributes["error_message"].InnerText;
            //            System.Windows.Forms.MessageBox.Show(Message);
            //        }
            //    }
            //}
            //catch (System.Exception ex) { }
        }

        public static void AddMethodNodeFromXmlContent(System.Xml.XmlNode inXmlNode, System.Windows.Forms.TreeNode inTreeNode)
        {
            //System.Xml.XmlNode xNode;
            //System.Windows.Forms.TreeNode tNode;
            //System.Xml.XmlNodeList nodeList;
            //int i;

            //// Loop through the XML nodes until the leaf is reached.
            //// Add the nodes to the TreeView during the looping process.
            //try
            //{
            //    if (inXmlNode.HasChildNodes)
            //    {
            //        nodeList = inXmlNode.ChildNodes;
            //        for (i = 0; i <= nodeList.Count - 1; i++)
            //        {
            //            xNode = inXmlNode.ChildNodes[i];
            //            string TreeNodeName = xNode.Name;
            //            string TreeNodeValue = xNode.Value;
            //            DiversityWorkbench.UserControls.XMLNode N = new DiversityWorkbench.UserControls.XMLNode();
            //            N.Name = xNode.Name;
            //            if (xNode.Attributes != null && xNode.Attributes.Count > 0)
            //            {
            //                System.Xml.XmlAttribute attrName = (System.Xml.XmlAttribute)(xNode.Attributes.GetNamedItem("Name"));
            //                if (attrName != null)
            //                    TreeNodeName = attrName.Value;
            //                System.Xml.XmlAttribute attrValue = (System.Xml.XmlAttribute)(xNode.Attributes.GetNamedItem("Value"));
            //                if (attrValue != null)
            //                {
            //                    TreeNodeValue = attrValue.Value;
            //                    TreeNodeName += ": " + TreeNodeValue;
            //                }
            //                N.Attributes = new List<DiversityWorkbench.UserControls.XMLAttribute>();
            //                foreach (System.Xml.XmlAttribute A in xNode.Attributes)
            //                {
            //                    DiversityWorkbench.UserControls.XMLAttribute a = new DiversityWorkbench.UserControls.XMLAttribute();
            //                    a.Name = A.Name;
            //                    a.Value = A.Value;
            //                    N.Attributes.Add(a);
            //                }
            //            }
            //            else if (N.Name == "ValueEnum")
            //            {
            //                TreeNodeName = xNode.InnerText;
            //                N.Value = xNode.InnerText;
            //            }
            //            System.Windows.Forms.TreeNode TN = new System.Windows.Forms.TreeNode(TreeNodeName);
            //            if (xNode.Name == "Method")
            //                TN.BackColor = System.Drawing.Color.Yellow;
            //            if (N.Name == "ValueEnum")
            //                TN.ForeColor = System.Drawing.Color.Gray;
            //            TN.Tag = N;
            //            inTreeNode.Nodes.Add(TN);
            //            if (i >= inTreeNode.Nodes.Count)
            //                continue;
            //            tNode = inTreeNode.Nodes[i];
            //            AddMethodNodeFromXmlContent(xNode, tNode);
            //        }
            //    }
            //    else
            //    {
            //        if (inXmlNode.Value != null)
            //        {
            //            DiversityWorkbench.UserControls.XMLNode N = new DiversityWorkbench.UserControls.XMLNode();
            //            if (inXmlNode.Name == "#text")
            //                N.Name = inXmlNode.ParentNode.Name;
            //            else
            //                N.Name = inXmlNode.Name;
            //            string TreeNodeValue = inXmlNode.Value;
            //            if (inXmlNode.Attributes != null && inXmlNode.Attributes.Count > 0)
            //            {
            //                System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute)(inXmlNode.Attributes.GetNamedItem("Value"));
            //                if (attr != null)
            //                    TreeNodeValue = attr.Value;
            //            }
            //            N.Value = inXmlNode.Value;
            //            inTreeNode.Parent.Tag = N;
            //            if (N.Name == "ValueEnum")
            //            {
            //                inTreeNode.Text = N.Value;
            //            }
            //            else
            //            {
            //                inTreeNode.Parent.Text = inTreeNode.Parent.Text + ": " + inXmlNode.Value;
            //            }
            //            inTreeNode.Remove();
            //        }
            //    }
            //}
            //catch (System.Exception ex) { }

        }

        public static string XmlFromMethodTree(System.Windows.Forms.TreeView TreeView)
        {
            //System.IO.FileInfo XmlFile = new System.IO.FileInfo(...Windows.Forms.Application.StartupPath + "\\Temp.xml");

            //string XML = "";
            //if (TreeView.Nodes.Count == 0) return "";
            //System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            //System.Xml.XmlWriter W;
            //System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            //W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
            //W.WriteStartElement(TreeView.Nodes[0].Text);
            //foreach (System.Windows.Forms.TreeNode N in TreeView.Nodes[0].Nodes)
            //{
            //    if (!XmlFromMethodTreeAddChild(N, ref W))
            //        return "";
            //}
            //W.WriteEndElement();
            //W.Flush();
            //W.Close();
            //System.IO.StreamReader R = new System.IO.StreamReader(XmlFile.FullName);
            //XML = R.ReadToEnd();
            //R.Close();
            //R.Dispose();
            //XmlFile.Delete();
            //if (XML.IndexOf("?>") > -1)
            //    XML = XML.Substring(XML.IndexOf("?>") + 2);
            //if (XML.IndexOf("<Methods>") > -1)
            //    XML = XML.Replace("<Methods>", "<Methods xmlns=\"http://diversityworkbench.net/Schema/tools\">");
            //return XML;
            return "";
        }

        public static bool XmlFromMethodTreeAddChild(System.Windows.Forms.TreeNode TreeNode, ref System.Xml.XmlWriter W)
        {
            //try
            //{
            //    if (TreeNode.Tag != null)
            //    {
            //        try
            //        {
            //            DiversityWorkbench.UserControls.XMLNode N = (DiversityWorkbench.UserControls.XMLNode)TreeNode.Tag;

            //            if (N.Attributes != null)
            //            {
            //                W.WriteStartElement(N.Name);
            //                foreach (DiversityWorkbench.UserControls.XMLAttribute A in N.Attributes)
            //                {
            //                    W.WriteAttributeString(A.Name, A.Value);
            //                }
            //                if (TreeNode.Nodes.Count > 0)
            //                {
            //                    foreach (System.Windows.Forms.TreeNode TC in TreeNode.Nodes)
            //                        DiversityCollection.Method.XmlFromMethodTreeAddChild(TC, ref W);
            //                }
            //                W.WriteEndElement();
            //            }
            //            else
            //            {
            //                if (TreeNode.Nodes.Count > 0)
            //                {
            //                    W.WriteStartElement(N.Name);
            //                    DiversityCollection.Method.XmlFromMethodTreeAddChild(TreeNode, ref W);
            //                    W.WriteEndElement();
            //                }
            //                else
            //                    W.WriteElementString(N.Name, N.Value);
            //            }

            //        }
            //        catch (System.Exception ex) { return false; }
            //    }
            //    else
            //    {
            //        string Node = TreeNode.Text.Replace(" ", "_");
            //        if (Node.IndexOf(":") > -1)
            //            Node = Node.Substring(0, Node.IndexOf(":"));
            //        W.WriteStartElement(Node);
            //        foreach (System.Windows.Forms.TreeNode NChild in TreeNode.Nodes)
            //        {
            //            if (!XmlFromMethodTreeAddChild(NChild, ref W))
            //                return false;
            //        }
            //        W.WriteEndElement();
            //    }
            //    return true;
            //}
            //catch (System.Exception ex) { return false; }
            return false;
        }

        public static void AddMethod(System.Windows.Forms.TreeView TreeView, int MethodID)
        {

        }

        #endregion

    }
}
