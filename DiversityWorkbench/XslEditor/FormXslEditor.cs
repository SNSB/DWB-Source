using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.XslEditor
{
    public partial class FormXslEditor : Form
    {

        /*
         * 
         * Orientierung an 
         * http://de.selfhtml.org/xml/darstellung/xsltelemente.htm#attribute
         * 
         * Umbau, so dass nur noch Document sichtbar und alles von dort gesteuert wird, also keine separaten Templates und Variablen mehr
         * Grund: es gibt noch mehr Basiselemente, e.g. xsl:attribute-set, xsl:attribute
         * Variablen koennen groessere Abschnitte enthalten, auch Baum
         * 
         * Eingebundene Dokumente dann als separates Document darstellen
         * 
         * */


        #region Parameter

        private string _XmlFileName;

        private System.IO.FileInfo _XmlFile;

        public System.IO.FileInfo XmlFile
        {
            get
            {
                if (this._XmlFile == null)
                {
                    try
                    {
                        this._XmlFile = new System.IO.FileInfo(this._XmlFileName);
                    }
                    catch (System.Exception ex) { }
                }
                return _XmlFile;
            }
            //set { _XmlFile = value; }
        }

        private System.IO.FileInfo _XslFile;

        public System.IO.FileInfo XslFile
        {
            get
            {
                if (this.textBoxXslFile.Text.Length > 0)
                    this._XslFile = new System.IO.FileInfo(this.textBoxXslFile.Text);
                return _XslFile;
            }
            //set { _XslFile = value; }
        }

        private DiversityWorkbench.XslEditor.XslEditor _XslEditor;

        #endregion

        #region Construction
        /// <summary>
        /// Editing the schemas for xml files
        /// </summary>
        /// <param name="Xmlfile">The path and name of an xml file for processing with the schemata</param>
        public FormXslEditor(string XmlFile, bool IsIncludedFile)
        {
            InitializeComponent();
            this._XmlFileName = XmlFile;
            this.AnalyseXmlFile();
            System.Uri U = new Uri(XmlFile);
            this.webBrowserXmlFile.Url = U;
            if (IsIncludedFile)
            {
                this.splitContainerMain.Panel2Collapsed = true;
                this.buttonCompile.Enabled = false;
                this.buttonOpenXslFile.Enabled = false;
            }
            this.listBoxTemplates.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
            this.labelTemplateNodeAttributes.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
            this.labelTemplateNodeContent.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
            this.labelTemplatesContained.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;

            this.listBoxTemplatesIncluded.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
            this.listBoxTemplatesIncluded.BackColor = DiversityWorkbench.XslEditor.XslEditor.ColorIncluded;
            this.labelTemplatesIncluded.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;

            this.listBoxVariables.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
            this.labelVariableAttributes.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
            this.labelVariableContent.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
            this.labelVariablesContained.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;

            this.listBoxVariablesIncluded.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
            this.listBoxVariablesIncluded.BackColor = DiversityWorkbench.XslEditor.XslEditor.ColorIncluded;
            this.labelVariablesIncluded.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;

            this.tabControlXslFile.TabPages.Remove(this.tabPageTemplates);
            this.tabControlXslFile.TabPages.Remove(this.tabPageVariables);
            this.tabControlXslFile.TabPages.Remove(this.tabPageDocument);
            this.tabControlXslFile.TabPages.Remove(this.tabPageNodes);

            this.userControlXslDocumentNodes.SetIsIncluded(false);
        }
        
        #endregion

        #region Properties

        public DiversityWorkbench.XslEditor.XslEditor XslEditor
        {
            get {
                if (this._XslEditor == null)
                    this._XslEditor = new XslEditor();
                return _XslEditor; }
            set { _XslEditor = value; }
        }

        //public DiversityWorkbench.XslEditor.XslEditor XslEditorForXml
        //{
        //    get
        //    {
        //        if (this._XslEditorForXml == null)
        //            this._XslEditorForXml = new XslEditor();
        //        return _XslEditorForXml;
        //    }
        //    set { _XslEditorForXml = value; }
        //}

        #endregion

        #region XSLT schema

        private void buttonOpenXslFile_Click(object sender, EventArgs e)
        {
            string XmlFile = this._XmlFileName;
            if (XmlFile.StartsWith("file:///"))
                XmlFile = this._XmlFileName.Substring(8);
            System.IO.FileInfo F = new System.IO.FileInfo(XmlFile);
            string Path = F.DirectoryName;
            if (this.textBoxXslFile.Text.Length > 0)
            {
                try
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxXslFile.Text);
                    if (FI.Exists)
                        Path = FI.DirectoryName;
                }
                catch { }
            }
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            this.openFileDialog.InitialDirectory = Path;
            this.openFileDialog.Filter = "XSLT Files|*.xslt";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    this.textBoxXslFile.Tag = this.openFileDialog.FileName;
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxXslFile.Text = f.FullName;
                    this.AnalyseSchemaFile(f);
                    this.labelXsltIsMissing.Visible = false;
                    //this.tabControlXslFile.TabPages.Add(this.tabPageNodes);
                    this.tabControlXslFile.TabPages.Insert(0, this.tabPageNodes);
                    this.tabControlXslFile.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void buttonSaveXslFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.XslFile == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a valid path for the stylesheet");
                    return;
                }
                string Path = this.XslFile.FullName;
                //string Path = this.XslFile.DirectoryName + "\\" + this.XslFile.Name.Substring(0, this.XslFile.Name.Length - this.XslFile.Extension.Length) + "_Copy" + this.XslFile.Extension;

                if (System.IO.File.Exists(Path))
                {
                    //System.Windows.Forms.MessageBox.Show("File exists, will be changed");
                }
                this.XslEditor.WriteXsltFile(Path);
                this.AnalyseSchemaFile(Path);
                //this.TransformToHtml();
            }
            catch (System.Exception ex) { }
        }

        private void AnalyseSchemaFile(string Path)
        {
            System.IO.FileInfo f = new System.IO.FileInfo(Path);
            this.AnalyseSchemaFile(f);
        }

        private void AnalyseSchemaFile(System.IO.FileInfo f)
        {
            try
            {
                System.Uri U = new Uri(f.FullName);
                this.webBrowserXsltSchema.Url = U;
                this._XslEditor = null;
                this.XslEditor.AnalyseXSLTfile(f.FullName);

                this.listBoxTemplates.Items.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> T in this.XslEditor.XslTemplates)
                    this.listBoxTemplates.Items.Add(T.Key);

                this.listBoxTemplatesIncluded.Items.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> T in this.XslEditor.XslIncludedTemplates)
                    this.listBoxTemplatesIncluded.Items.Add(T.Key);

                this.listBoxVariables.Items.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> V in this.XslEditor.XslVariables)
                    this.listBoxVariables.Items.Add(V.Key);

                this.listBoxVariablesIncluded.Items.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslNode> V in this.XslEditor.XslIncludedVariables)
                    this.listBoxVariablesIncluded.Items.Add(V.Key);

                this.treeViewDocument.Nodes.Clear();
                foreach (DiversityWorkbench.XslEditor.XslNode N in this.XslEditor.XslDocumentNodes)
                {
                    System.Windows.Forms.TreeNode TN = DiversityWorkbench.XslEditor.XslEditor.XslTreeNode(N, this._XslEditor);
                    TN.Tag = N;
                    this.treeViewDocument.Nodes.Add(TN);
                    if (N.Name == "xsl:include")
                        TN.BackColor = DiversityWorkbench.XslEditor.XslEditor.ColorIncluded;
                    else if (N.Name == "xsl:variable")
                        TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
                }

                foreach (System.Windows.Forms.TabPage T in this.tabControlXslFile.TabPages)
                {
                    if (T.Tag != null && T.Tag.GetType() == typeof(DiversityWorkbench.XslEditor.XslEditor))
                        this.tabControlXslFile.TabPages.Remove(T);
                }

                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.XslEditor.XslEditor> KV in this.XslEditor.XslIncludedFiles)
                {
                    System.Windows.Forms.TabPage T = new TabPage(KV.Key);
                    T.Tag = KV.Value;
                    DiversityWorkbench.XslEditor.UserControlXslDocument Uincluded = new UserControlXslDocument();
                    Uincluded.setXslEditor(KV.Value);
                    Uincluded.SetIsIncluded(true);
                    T.Controls.Add(Uincluded);
                    Uincluded.Dock = DockStyle.Fill;
                    this.tabControlXslFile.TabPages.Add(T);
                }

                this.TransformToHtml();

                DiversityWorkbench.XslEditor.XslNode NN = new XslNode();
                this.setVariableInfos(NN);
                this.treeViewTemplate.Nodes.Clear();
                this.treeViewTemplate.SelectedNode = null;
                this.treeViewTemplate_AfterSelect(null, null);
                this.treeViewDocument.SelectedNode = null;
                this.treeViewDocument_AfterSelect(null, null);

                this.userControlXslDocumentNodes.setXslEditor(this.XslEditor);
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region XML file
        
        private void AnalyseXmlFile()
        {
            try
            {
                this.XslEditor.AnalyseXmlFile(this.XmlFile.FullName);
            }
            catch (System.Exception ex) { }
        }
        
        #endregion

        #region HTML generation
        
        private void buttonCompile_Click(object sender, EventArgs e)
        {
            this.TransformToHtml();
        }

        private void TransformToHtml()
        {
            if (this.XslFile != null && this.XslFile.Exists)
            {
                System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                XSLT.Load(this.XslFile.FullName);

                // Load the file to transform.
                System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(this.XmlFile.FullName);

                // The output file:
                string OutputFile = this.XmlFile.FullName.Substring(0, this.XmlFile.FullName.Length
                    - this.XmlFile.Extension.Length) + ".htm";

                // Create the writer.             
                System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                // Transform the file and send the output to the console.
                XSLT.Transform(doc, writer);
                writer.Close();
                System.Uri U = new Uri(OutputFile);
                this.webBrowserHtmlFile.Url = U;
            }
        }
        
        #endregion

        #region Templates

        #region The lists
        
        private void listBoxTemplatesIncluded_Click(object sender, EventArgs e)
        {
            this.listBoxTemplates.SelectedIndex = -1;
            this.splitContainerTemplate.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
            this.toolStripTemplateNodes.Enabled = false;
            this.toolStripTemplateNodeAttributes.Enabled = false;
            this.panelTemplateNodeAttributes.Enabled = false;
            this.textBoxTemplateNodeContent.Enabled = false;
            this.labelTemplateNodeAttributes.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
            this.labelTemplateNodeContent.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
            string Template = this.listBoxTemplatesIncluded.SelectedItem.ToString();
            this.XslEditor.CreateTemplateHierarchy(Template, this.treeViewTemplate, true);
            this.treeViewTemplate.ExpandAll();
            this.treeViewTemplate_AfterSelect(null, null);
            this.setTemplateControls();
        }

        private void listBoxTemplates_Click(object sender, EventArgs e)
        {
            try
            {
                this.listBoxTemplatesIncluded.SelectedIndex = -1;
                this.splitContainerTemplate.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
                this.toolStripTemplateNodes.Enabled = true;
                this.toolStripTemplateNodeAttributes.Enabled = true;
                this.panelTemplateNodeAttributes.Enabled = true;
                this.textBoxTemplateNodeContent.Enabled = true;
                this.labelTemplateNodeAttributes.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
                this.labelTemplateNodeContent.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorTemplate;
                string Template = this.listBoxTemplates.SelectedItem.ToString();
                this.XslEditor.CreateTemplateHierarchy(Template, this.treeViewTemplate, false);
                this.treeViewTemplate.ExpandAll();
                this.treeViewTemplate_AfterSelect(null, null);
                this.setTemplateControls();
            }
            catch (System.Exception ex) { }
        }
        
        #endregion

        private void setTemplateControls()
        {
            if (this.treeViewTemplate.SelectedNode == null)
            {
                this.labelTemplateNodeAttributes.Visible = false;
                this.labelTemplateNodeContent.Visible = false;
                this.toolStripTemplateNodes.Visible = false;
                this.toolStripTemplateNodeAttributes.Visible = false;
                this.textBoxTemplateNodeContent.Visible = false;
                this.panelTemplateNodeAttributes.Visible = false;
            }
            else
            {
                this.labelTemplateNodeAttributes.Visible = true;
                this.labelTemplateNodeContent.Visible = true;
                this.toolStripTemplateNodes.Visible = true;
                this.toolStripTemplateNodeAttributes.Visible = true;
                this.textBoxTemplateNodeContent.Visible = true;
                this.panelTemplateNodeAttributes.Visible = true;

                DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewTemplate.SelectedNode.Tag;

                this.toolStripButtonTableEditor.Visible = false;
                this.toolStripButtonTemplateNodeDown.Visible = false;
                this.toolStripButtonTemplateNodeUp.Visible = false;
                this.toolStripButtonTemplateNodeAdd.Visible = false;
                this.toolStripButtonTemplateNodeCopy.Visible = false;
                this.toolStripButtonTemplateNodeNew.Visible = false;

                if (N != null)
                {
                    // toolStripButtonTableEditor
                    if (N.Name == "table" && N.XslNodeType == "html")
                    {
                        this.toolStripButtonTableEditor.Visible = true;
                    }

                    // toolStripButtonTemplateNodeUp & toolStripButtonTemplateNodeDown
                    if ((N.Name == "td"
                        || N.Name == "tr"
                        || N.Name == "th")
                        && N.XslNodeType == "html")
                    {
                        if (N.ParentXslNode.XslNodes.Count > 1)
                        {
                            if (N.ParentXslNode.XslNodes[0] != N)
                                this.toolStripButtonTemplateNodeUp.Visible = true;
                            if (N.ParentXslNode.XslNodes[N.ParentXslNode.XslNodes.Count - 1] != N)
                                this.toolStripButtonTemplateNodeDown.Visible = true;
                        }
                    }

                    // toolStripButtonTemplateNodeAdd
                    if ((N.Name == "table" || N.Name == "tr" || N.Name == "td") && N.XslNodeType == "html")
                    {
                        this.toolStripButtonTemplateNodeAdd.Visible = true;
                    }

                    // toolStripButtonTemplateNodeCopy
                    if ((N.Name == "tr" || N.Name == "td" || N.Name == "th") && N.XslNodeType == "html")
                    {
                        this.toolStripButtonTemplateNodeCopy.Visible = true;
                    }

                    // toolStripButtonTemplateNodeNew
                    if ((N.Name == "tr" || N.Name == "td" || N.Name == "th") && N.XslNodeType == "html")
                    {
                        this.toolStripButtonTemplateNodeNew.Visible = true;
                    }

                }
            }
        }

        #region Handling the templates
        
        private void toolStripButtonTemplateNew_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonTemplateDelete_Click(object sender, EventArgs e)
        {

        }

        private void treeViewTemplate_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.panelTemplateNodeAttributes.Controls.Clear();
                this.textBoxTemplateNodeContent.Text = "";
                if (this.treeViewTemplate.SelectedNode == null)
                {
                    return;
                }
                this.markCurrentTemplateTreeNode(this.treeViewTemplate.SelectedNode);
                DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewTemplate.SelectedNode.Tag;
                if (N != null)
                {
                    if (N.Attributes != null && N.Attributes.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in N.Attributes)
                        {
                            DiversityWorkbench.XslEditor.UserControlXslAttribute U = new UserControlXslAttribute(KV.Key, KV.Value, N);
                            this.panelTemplateNodeAttributes.Controls.Add(U);
                            U.Dock = DockStyle.Top;
                            U.Visible = true;
                        }
                    }
                    if (N.Content != null && N.Content.Length > 0)
                    {
                        this.textBoxTemplateNodeContent.Text = N.Content;
                    }
                }
                this.setTemplateControls();
            }
            catch (System.Exception ex) { }
        }
        
        #endregion

        #region Handling the nodes
        
        private void toolStripButtonTemplateNodeNew_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewTemplate.SelectedNode.Tag;
            DiversityWorkbench.XslEditor.XslNode NParent = N.ParentXslNode;
            System.Collections.Generic.Dictionary<string, string> NewNames = this.ListForNewNodeNames(NParent);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(NewNames, "New node", "Please select a name for the new node", false);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityWorkbench.XslEditor.XslNode Nnew = new XslNode(f.String);
            }
        }

        private void toolStripButtonTemplateNodeCopy_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewTemplate.SelectedNode.Tag;
            DiversityWorkbench.XslEditor.XslNode NParent = N.ParentXslNode;
            DiversityWorkbench.XslEditor.XslNode Nnew = new XslNode(N.XslNodeType, N.Name, N.Attributes);
            NParent.XslNodes.Add(Nnew);
            this.listBoxTemplates_Click(null, null);
            this.moveToTreeNode(Nnew);
        }

        private void toolStripButtonTemplateNodeAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewTemplate.SelectedNode.Tag;
            System.Collections.Generic.Dictionary<string, string> NewNames = this.ListForNewNodeNames(N);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(NewNames, "New node", "Please select a name for the new node", false);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityWorkbench.XslEditor.XslNode Nnew = new XslNode(f.String);
            }
        }

        private System.Collections.Generic.Dictionary<string, string> ListForNewNodeNames(DiversityWorkbench.XslEditor.XslNode N)
        {
            System.Collections.Generic.Dictionary<string, string> NewNames = new Dictionary<string, string>();
            switch (N.Name)
            {
                case "body":
                    break;
                case "table":
                    NewNames.Add("Table row","tr");
                    break;
                case "tr":
                    NewNames.Add("Table data field", "td");
                    NewNames.Add("Table header", "th");
                    //for(int i = 0; i < DiversityWorkbench.XslEditor.XslEditor.XsltElement
                    break;
                case "td":
                    break;
            }
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.XslEditor.XslEditor.XsltElements)
                NewNames.Add(KV.Key, KV.Value);
            return NewNames;
        }

        private void toolStripButtonTableEditor_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewTemplate.SelectedNode.Tag;
                DiversityWorkbench.XslEditor.FormXslTableEditor F = new FormXslTableEditor(N);
                F.StartPosition = FormStartPosition.CenterParent;
                F.Width = this.Width - 10;
                F.Height = this.Height - 10;
                F.ShowDialog();
            }
            catch (System.Exception ex) { }
        }

        private void toolStripButtonTemplateNodeDelete_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonTemplateNodeAttributeNew_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewTemplate.SelectedNode.Tag;
            System.Collections.Generic.Dictionary<string, string> L = new Dictionary<string, string>();
            switch (N.Name)
            {
                case "td":
                    if (!N.Attributes.ContainsKey("align"))
                        L.Add("align", "align");
                    if (!N.Attributes.ContainsKey("vertical align"))
                        L.Add("valign", "valign");
                    if (!N.Attributes.ContainsKey("column span"))
                        L.Add("colspan", "colspan");
                    break;
            }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(L, "New attribute", "Please select the name of the new attribute", true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                N.Attributes.Add(f.String, "");
            }
            this.treeViewTemplate_AfterSelect(null, null);
        }

        private void textBoxTemplateNodeContent_Leave(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewTemplate.SelectedNode.Tag;
            N.Content = this.textBoxTemplateNodeContent.Text;
        }
        
        #region Moving the nodes
       
        private void toolStripButtonTemplateNodeUp_Click(object sender, EventArgs e)
        {
            int OriginalPosition = 0;
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewTemplate.SelectedNode.Tag;
            System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslNode> OriginalSequence = new List<XslNode>();
            DiversityWorkbench.XslEditor.XslNode NParent = N.ParentXslNode;
            for (int i = 0; i < NParent.XslNodes.Count; i++)
            {
                OriginalSequence.Add(NParent.XslNodes[i]);
                if (NParent.XslNodes[i] == N)
                {
                    OriginalPosition = i;
                }
            }
            NParent.XslNodes.Clear();
            for (int i = 0; i < OriginalSequence.Count; i++)
            {
                if (i == OriginalPosition - 1)
                {
                    NParent.XslNodes.Add(OriginalSequence[i + 1]);
                }
                else if (i == OriginalPosition)
                {
                    NParent.XslNodes.Add(OriginalSequence[i - 1]);
                }
                else
                {
                    NParent.XslNodes.Add(OriginalSequence[i]);
                }
            }
            this.listBoxTemplates_Click(null, null);
            this.moveToTreeNode(N);
            this.markCurrentTemplateTreeNode(N);
        }

        private void toolStripButtonTemplateNodeDown_Click(object sender, EventArgs e)
        {
            int OriginalPosition = 0;
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewTemplate.SelectedNode.Tag;
            System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslNode> OriginalSequence = new List<XslNode>();
            DiversityWorkbench.XslEditor.XslNode NParent = N.ParentXslNode;
            for (int i = 0; i < NParent.XslNodes.Count; i++)
            {
                OriginalSequence.Add(NParent.XslNodes[i]);
                if (NParent.XslNodes[i] == N)
                {
                    OriginalPosition = i;
                }
            }
            NParent.XslNodes.Clear();
            for (int i = 0; i < OriginalSequence.Count; i++)
            {
                if (i == OriginalPosition)
                {
                    NParent.XslNodes.Add(OriginalSequence[i + 1]);
                }
                else if (i == OriginalPosition + 1)
                {
                    NParent.XslNodes.Add(OriginalSequence[i - 1]);
                }
                else
                {
                    NParent.XslNodes.Add(OriginalSequence[i]);
                }
            }
            this.listBoxTemplates_Click(null, null);
            this.moveToTreeNode(N);
            this.markCurrentTemplateTreeNode(N);
        }
        
        #endregion

        #region Auxillary
        
        private void moveToTreeNode(DiversityWorkbench.XslEditor.XslNode N)
        {
            System.Windows.Forms.TreeNode T = new TreeNode();
            bool NodeFound = false;
            foreach (System.Windows.Forms.TreeNode TN in this.treeViewTemplate.Nodes)
            {
                DiversityWorkbench.XslEditor.XslNode Ntag = (DiversityWorkbench.XslEditor.XslNode)TN.Tag;
                if (Ntag == N)
                {
                    this.treeViewTemplate.SelectedNode = TN;
                    TN.EnsureVisible();
                    //this.markCurrentTemplateTreeNode(TN);
                    NodeFound = true;
                    break;
                }
            }
            if (!NodeFound)
            {
                foreach (System.Windows.Forms.TreeNode TN in this.treeViewTemplate.Nodes)
                {
                    T = this.GetTreeNode(N, TN);
                    if (T != null)
                    {
                        T.EnsureVisible();
                        //this.markCurrentTemplateTreeNode(T);
                        NodeFound = true;
                        break;
                    }
                }
            }
        }

        private System.Windows.Forms.TreeNode GetTreeNode(DiversityWorkbench.XslEditor.XslNode N, System.Windows.Forms.TreeNode T)
        {
            System.Windows.Forms.TreeNode TNchild = new TreeNode();
            if (T.Nodes.Count > 0)
            {
                foreach (System.Windows.Forms.TreeNode TN in T.Nodes)
                {
                    DiversityWorkbench.XslEditor.XslNode Ntag = (DiversityWorkbench.XslEditor.XslNode)TN.Tag;
                    if (Ntag == N)
                    {
                        return TN;
                    }
                    else
                    {
                        TNchild = this.GetTreeNode(N, TN);
                        if (TNchild != null)
                            return TNchild;
                    }
                }
            }
            else
                return null;
            return null;
        }

        private void markCurrentTemplateTreeNode(System.Windows.Forms.TreeNode Tmark)
        {
            foreach (System.Windows.Forms.TreeNode TN in this.treeViewTemplate.Nodes)
            {
                if (TN.BackColor != System.Drawing.SystemColors.Control)
                {
                    if (TN == Tmark)
                        TN.BackColor = System.Drawing.Color.Yellow;
                    else TN.BackColor = System.Drawing.Color.White;
                    foreach (System.Windows.Forms.TreeNode TNchild in TN.Nodes)
                        this.markCurrentTemplateTreeNodeChildren(TNchild, ref Tmark);
                }
            }
        }

        private void markCurrentTemplateTreeNodeChildren(System.Windows.Forms.TreeNode T, ref System.Windows.Forms.TreeNode Tmark)
        {
            if (T == Tmark)
                T.BackColor = System.Drawing.Color.Yellow;
            else T.BackColor = System.Drawing.Color.White;

            foreach (System.Windows.Forms.TreeNode TN in T.Nodes)
            {
                if (TN.BackColor != System.Drawing.SystemColors.Control)
                {
                    if (TN == Tmark)
                        TN.BackColor = System.Drawing.Color.Yellow;
                    else TN.BackColor = System.Drawing.Color.White;
                    foreach (System.Windows.Forms.TreeNode TNchild in TN.Nodes)
                        this.markCurrentTemplateTreeNodeChildren(TNchild, ref Tmark);
                }
            }
        }

        private void markCurrentTemplateTreeNode(DiversityWorkbench.XslEditor.XslNode Nmark)
        {
            foreach (System.Windows.Forms.TreeNode TN in this.treeViewTemplate.Nodes)
            {
                if (TN.BackColor != System.Drawing.SystemColors.Control)
                {
                    DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)TN.Tag;
                    if (N == Nmark)
                        TN.BackColor = System.Drawing.Color.Yellow;
                    else TN.BackColor = System.Drawing.Color.White;
                    foreach (System.Windows.Forms.TreeNode TNchild in TN.Nodes)
                        this.markCurrentTemplateTreeNodeChildren(TNchild, Nmark);
                }
            }
        }

        private void markCurrentTemplateTreeNodeChildren(System.Windows.Forms.TreeNode T, DiversityWorkbench.XslEditor.XslNode Nmark)
        {
            foreach (System.Windows.Forms.TreeNode TN in T.Nodes)
            {
                if (TN.BackColor != System.Drawing.SystemColors.Control)
                {
                    DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)TN.Tag;
                    if (N == Nmark)
                        TN.BackColor = System.Drawing.Color.Yellow;
                    else TN.BackColor = System.Drawing.Color.White;
                    foreach (System.Windows.Forms.TreeNode TNchild in TN.Nodes)
                        this.markCurrentTemplateTreeNodeChildren(TNchild, Nmark);
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region Variables

        private void listBoxVariablesIncluded_Click(object sender, EventArgs e)
        {
            try
            {
                this.listBoxVariables.SelectedIndex = -1;
                this.tableLayoutPanelVariable.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
                this.labelVariableAttributes.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
                this.panelVariableAttributes.Enabled = false;
                this.panelVariableValues.Enabled = false;
                this.toolStripVariableValues.Enabled = false;
                //this.tableLayoutPanelVariable.Enabled = false;
                DiversityWorkbench.XslEditor.XslNode N = new XslNode();
                if (this.XslEditor.XslIncludedVariables.ContainsKey(this.listBoxVariablesIncluded.SelectedItem.ToString()))
                {
                    N = this.XslEditor.XslIncludedVariables[this.listBoxVariablesIncluded.SelectedItem.ToString()];
                }
                this.setVariableInfos(N);
            }
            catch (System.Exception ex) { }
        }

        private void listBoxVariables_Click(object sender, EventArgs e)
        {
            try
            {
                this.listBoxVariablesIncluded.SelectedIndex = -1;
                this.tableLayoutPanelVariable.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
                this.labelVariableAttributes.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
                this.panelVariableAttributes.Enabled = true;
                this.panelVariableValues.Enabled = true;
                this.toolStripVariableValues.Enabled = true;
                //this.tableLayoutPanelVariable.Enabled = true;
                DiversityWorkbench.XslEditor.XslNode N = new XslNode();
                if (this.XslEditor.XslVariables.ContainsKey(this.listBoxVariables.SelectedItem.ToString()))
                {
                    N = this.XslEditor.XslVariables[this.listBoxVariables.SelectedItem.ToString()];
                }
                this.setVariableInfos(N);
            }
            catch (System.Exception ex) { }
        }
        
        private void listBoxVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        
        private void listBoxVariablesIncluded_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void setVariableInfos(DiversityWorkbench.XslEditor.XslNode XslNode)
        {
            try
            {
                this.panelVariableValues.Visible = false;
                this.labelVariableValue.Visible = false;
                this.toolStripVariableValues.Visible = false;
                this.panelVariableAttributes.Controls.Clear();
                if (XslNode.Attributes != null
                    && XslNode.Attributes.Count > 0)
                {
                    int H = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in XslNode.Attributes)
                    {
                        DiversityWorkbench.XslEditor.UserControlXslAttribute U = new UserControlXslAttribute(KV.Key, KV.Value, XslNode);
                        this.panelVariableAttributes.Controls.Add(U);
                        U.Dock = DockStyle.Top;
                        H = U.Height;
                    }
                    this.panelVariableAttributes.Height = this.panelVariableAttributes.Controls.Count * H;
                    this.labelVariableAttributes.Visible = true;
                }
                else
                    this.labelVariableAttributes.Visible = false;
                if (XslNode.Content != null
                    && XslNode.Content.Length > 0)
                {
                    this.textBoxVariableContent.Text = XslNode.Content;
                    this.textBoxVariableContent.Visible = true;
                    this.labelVariableContent.Visible = true;
                }
                else if (XslNode.XslNodes.Count > 0)
                {
                    this.panelVariableValues.Visible = true;
                    this.panelVariableValues.Controls.Clear();
                    this.labelVariableValue.Visible = true;
                    this.toolStripVariableValues.Visible = true;
                    if (XslNode.XslNodes[0].Content != null && XslNode.XslNodes[0].Content.Length > 0)
                    {
                        string VariableValue = XslNode.XslNodes[0].Content;
                        string[] Values = VariableValue.Split(new char[] { ';' });
                        for (int i = 0; i < Values.Length; i++)
                        {
                            string[] ValuePart = Values[i].Split(new char[] { ':' });
                            if (ValuePart.Length == 2)
                            {
                                DiversityWorkbench.XslEditor.UserControlXslAttribute U = new UserControlXslAttribute(ValuePart[0], ValuePart[1], XslNode.XslNodes[0]);
                                this.panelVariableValues.Controls.Add(U);
                                U.Dock = DockStyle.Top;
                                U.BringToFront();
                                this.panelVariableValues.Height = U.Height * i;
                            }
                            else if (ValuePart.Length == 1 && ValuePart[0].Length > 0)
                            {
                                DiversityWorkbench.XslEditor.UserControlXslAttribute U = new UserControlXslAttribute("", ValuePart[0], XslNode.XslNodes[0]);
                                this.panelVariableValues.Controls.Add(U);
                                U.Dock = DockStyle.Top;
                                U.BringToFront();
                                this.panelVariableValues.Height = U.Height * i;
                            }
                        }
                        this.textBoxVariableContent.Text = "";
                        this.textBoxVariableContent.Visible = false;
                        this.labelVariableContent.Visible = false;
                    }
                }
                else
                {
                    this.textBoxVariableContent.Text = "";
                    this.textBoxVariableContent.Visible = false;
                    this.labelVariableContent.Visible = false;
                }
            }
            catch (System.Exception ex) { }
        }

        private void toolStripButtonVariableValuesAdd_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonVariableAdd_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonVariableDelete_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Includes
        
        private void listBoxIncludes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        #endregion

        #region Document
        
        private void treeViewDocument_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.panelDocumentAttributes.Controls.Clear();
                this.labelDocumentAttributes.Visible = false;
                this.textBoxDocumentContent.Visible = false;
                this.labelDocumentContent.Visible = false;
                this.toolStripDocument.Visible = false;
                this.toolStripDocumentAttributes.Visible = false;
                this.panelDocumentAttributes.Controls.Clear();
                if (this.treeViewDocument.SelectedNode == null)
                {
                    return;
                }
                DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewDocument.SelectedNode.Tag;
                if (N == null)
                {
                    return;
                }
                else
                {
                    if (N.Attributes !=null && N.Attributes.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in N.Attributes)
                        {
                            DiversityWorkbench.XslEditor.UserControlXslAttribute U = new UserControlXslAttribute(KV.Key, KV.Value, N);
                            this.panelDocumentAttributes.Controls.Add(U);
                            U.Dock = DockStyle.Top;
                        }
                        this.labelDocumentAttributes.Visible = true;
                        this.toolStripDocumentAttributes.Visible = true;
                    }
                    //else
                    //{
                    //    this.labelDocumentAttributes.Visible = false;
                    //    this.toolStripDocumentAttributes.Visible = false;
                    //}
                    if (N.Content !=null && N.Content.Length > 0)
                    {
                        this.textBoxDocumentContent.Visible = true;
                        this.textBoxDocumentContent.Text = N.Content;
                        this.labelDocumentContent.Visible = true;
                    }
                    //else
                    //{
                    //    this.textBoxDocumentContent.Visible = false;
                    //    this.labelDocumentContent.Visible = false;
                    //}
                }
            }
            catch (System.Exception ex) { }
        }
        
        #endregion

    }
}
