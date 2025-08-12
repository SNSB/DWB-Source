using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{

    public struct XmlNode
    {
        public string Name;
        public string Value;
    }

    public partial class FormEditXmlContent : Form
    {
        #region Parameter
        
        private string _XML = "";
        private string _Template = "";
        System.Windows.Forms.TreeView _TreeViewTemplate;

        #endregion

        #region Construction and Form
        
        private FormEditXmlContent()
        {
            InitializeComponent();
        }

        public FormEditXmlContent(string XML, string Header, string FormTitle)
            : this()
        {
            this._XML = XML;
            if (this._XML.StartsWith("<rdf:RDF"))
            {
                this.buttonMergeTemplates.Visible = false;
                this.textBoxContent.Visible = false;
                this.labelContent.Visible = false;
                this.labelHeader.Visible = false;
                this.Text = "EXIF information";
                this.userControlDialogPanel.Visible = false;
            }
            else
            {
                this.Text = FormTitle;
                this.labelHeader.Text = Header;
            }
            this.buildTree();
        }

        public FormEditXmlContent(string XML, string Template, string Header, string FormTitle)
            : this(XML, Header, FormTitle)
        {
            this._Template = Template;
            if (this._Template.Length > 0)
                this.buttonMergeTemplates.Visible = true;
        }

        private void FormEditXmlContent_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                string XML = this.XmlFromTree();
                if (XML.Length > 0)
                    this._XML = XML;
            }
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Interface

        public string XML { get { return this._XML; } }

        public string XmlWithoutEncoding 
        { 
            get 
            {
                string XML = this._XML;
                if (XML.IndexOf("\"?>") > -1)
                    XML = XML.Substring(XML.IndexOf("\"?>") + 3);
                return XML; 
            } 
        }

        #endregion
        
        #region Edit template

        private void buildTree()
        {
            try
            {
                this.buildTree(this._XML);
            }
            catch { }
        }

        private void buildTree(string XML)
        {
            try
            {
                this.treeView.Nodes.Clear();
                System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(XML, System.Xml.XmlNodeType.Element, null);
                System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                dom.Load(tr);
                if (dom.DocumentElement == null) return;
                if (dom.DocumentElement.ChildNodes.Count >= 0)
                {
                    // SECTION 2. Initialize the TreeView control.
                    this.treeView.Nodes.Clear();
                    //if (dom.DocumentElement.Name.Length > 0)
                    this.treeView.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name));
                    System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
                    tNode = this.treeView.Nodes[0];
                    // SECTION 3. Populate the TreeView with the DOM nodes.
                    this.AddNode(dom.DocumentElement, tNode);
                    this.treeView.ExpandAll();
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
            catch { }
        }

        private void AddNode(System.Xml.XmlNode inXmlNode, System.Windows.Forms.TreeNode inTreeNode)
        {
            System.Xml.XmlNode xNode;
            System.Windows.Forms.TreeNode tNode;
            System.Xml.XmlNodeList nodeList;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new System.Windows.Forms.TreeNode(xNode.Name));
                    if (i >= inTreeNode.Nodes.Count)
                        continue;
                    tNode = inTreeNode.Nodes[i];
                    this.AddNode(xNode, tNode);
                }
            }
            else
            {
                if (inXmlNode.Value != null)
                {
                    XmlNode N = new XmlNode();
                    if (inXmlNode.Name == "#text")
                        N.Name = inXmlNode.ParentNode.Name;
                    else
                        N.Name = inXmlNode.Name;
                    N.Value = inXmlNode.Value;
                    inTreeNode.Parent.Tag = N;
                    inTreeNode.Parent.Text = inTreeNode.Parent.Text + ": " + inXmlNode.Value;
                    inTreeNode.Remove();
                }
            }
        }

        private string XmlFromTree()
        {
            System.IO.FileInfo XmlFile = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation) + "ImageDescription.xml");

            string XML = "";
            if (this.treeView.Nodes.Count == 0) 
                return "";
            try
            {
                bool WriterInErrorState = false;
                System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
                System.Xml.XmlWriter W;
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                //settings.Encoding = System.Text.Encoding.Unicode;
                W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
                //W = System.Xml.XmlWriter.Create(XmlFile.FullName);
                W.WriteStartElement(this.treeView.Nodes[0].Text);
                foreach (System.Windows.Forms.TreeNode N in this.treeView.Nodes[0].Nodes)
                {
                    if (!this.XmlFromTreeAddChild(N, ref W))
                    {
                        WriterInErrorState = true;
                        return "";
                    }
                }
                W.WriteEndElement();
                W.Flush();
                W.Close();
                System.IO.StreamReader R = new System.IO.StreamReader(XmlFile.FullName);
                XML = R.ReadToEnd();
                R.Close();
            }
            catch (System.Exception ex) { }
            return XML;
        }

        private bool XmlFromTreeAddChild(System.Windows.Forms.TreeNode TreeNode, ref System.Xml.XmlWriter W)
        {
            try
            {
                if (TreeNode.Tag != null)
                {
                    try
                    {
                        XmlNode N = (XmlNode)TreeNode.Tag;
                        W.WriteElementString(N.Name, N.Value);
                    }
                    catch { }
                }
                else
                {
                    string Node = TreeNode.Text.Replace(" ", "_");
                    if (Node.IndexOf(":") > -1) 
                        Node = Node.Substring(0, Node.IndexOf(":"));
                    W.WriteStartElement(Node);
                    foreach (System.Windows.Forms.TreeNode NChild in TreeNode.Nodes)
                    {
                        if (!this.XmlFromTreeAddChild(NChild, ref W))
                            return false;
                    }
                    W.WriteEndElement();
                }
                return true;
            }
            catch (System.Exception ex) { return false; }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeView.SelectedNode.Tag != null)
            {
                XmlNode N = (XmlNode)this.treeView.SelectedNode.Tag;
                if (N.Name == "#text")
                {
                    if (this.treeView.SelectedNode.Text.IndexOf(":") > -1)
                        this.labelContent.Text = this.treeView.SelectedNode.Text.Substring(0, this.treeView.SelectedNode.Text.IndexOf(":")+1);
                    else
                        this.labelContent.Text = this.treeView.SelectedNode.Text + ":";
                }
                else
                    this.labelContent.Text = N.Name + ":";
                this.textBoxContent.Text = N.Value;
            }
            else
            {
                this.textBoxContent.Text = "";
                string Node = this.treeView.SelectedNode.Text;
                if (Node.IndexOf(":") > -1)
                    Node = Node.Substring(0, Node.IndexOf(":"));
                this.labelContent.Text = Node;
            }
        }

        private void textBoxContent_Leave(object sender, EventArgs e)
        {
            XmlNode N;
            if (this.treeView.SelectedNode.Tag != null)
            {
                N = (XmlNode)this.treeView.SelectedNode.Tag;
            }
            else
            {
                N = new XmlNode();
                string Name = this.treeView.SelectedNode.Text;
                if (Name.IndexOf(":") > -1)
                {
                    Name = Name.Substring(0, Name.IndexOf(":"));
                }
                N.Name = Name;
            }
            N.Value = this.textBoxContent.Text;
            this.treeView.SelectedNode.Tag = N;
            this.treeView.SelectedNode.Text = N.Name + ": " + N.Value;

            //bool SettingsMustBeSaved = false;
            //if ((this.treeView.SelectedNode.Tag == null && this.textBoxContent.Text.Length > 0)
            //    || this.treeView.SelectedNode.Tag.ToString() != this.textBoxContent.Text)
            //    SettingsMustBeSaved = true;
            //this.treeView.SelectedNode.Tag = this.textBoxContent.Text;
            //if (this.treeView.SelectedNode.Text.IndexOf(":") > -1)
            //    this.treeView.SelectedNode.Text = this.treeView.SelectedNode.Text.Substring(0, this.treeView.SelectedNode.Text.IndexOf(":")) + ": " + this.textBoxContent.Text;
            //else
            //    this.treeView.SelectedNode.Text = this.treeView.SelectedNode.Text + ": " + this.textBoxContent.Text;
            //if (SettingsMustBeSaved)
            //    this.SaveSettings();
        }

        #region Template merge
        
        private void buttonMergeTemplates_Click(object sender, EventArgs e)
        {
            this.buildTemplateTree();
            this.MergeTemplateTree();
        }

        private void buildTemplateTree()
        {
            try
            {
                this._TreeViewTemplate = new TreeView();
                System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(this._Template, System.Xml.XmlNodeType.Element, null);
                System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                dom.Load(tr);
                if (dom.DocumentElement == null) return;
                if (dom.DocumentElement.ChildNodes.Count >= 0)
                {
                    // SECTION 2. Initialize the TreeView control.
                    this._TreeViewTemplate.Nodes.Clear();
                    //if (dom.DocumentElement.Name.Length > 0)
                    this._TreeViewTemplate.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name));
                    System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
                    tNode = this._TreeViewTemplate.Nodes[0];
                    // SECTION 3. Populate the TreeView with the DOM nodes.
                    this.AddNode(dom.DocumentElement, tNode);
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
            catch { }
        }

        private void MergeTemplateTree()
        {
            // in the old version only for the first node - name of root will not be checked
            // after inclusion of exif information, the first node has to be checked as well
            if (this._TreeViewTemplate.Nodes.Count > 0)
            {
                System.Windows.Forms.TreeNode NStart = new TreeNode();
                foreach (System.Windows.Forms.TreeNode Ntemplate in this._TreeViewTemplate.Nodes)
                {
                    foreach (System.Windows.Forms.TreeNode Ncontent in this.treeView.Nodes)
                    {
                        if (Ntemplate.Text == Ncontent.Text)
                        {
                            NStart = Ncontent;
                            break;
                        }
                    }
                }
                if (NStart.Text.Length == 0) // The root nodes do not contain the root node of the template
                {
                    System.Windows.Forms.MessageBox.Show("Not possible for " + this.treeView.Nodes[0].Text);
                    return;
                    //NStart.Text = this._TreeViewTemplate.Nodes[0].Text;
                    //this.treeView.Nodes.Add(NStart);
                    //this.AddChildNodes(this._TreeViewTemplate.Nodes[0], NStart);
                }
                else
                {
                    foreach (System.Windows.Forms.TreeNode Ntemplate in this._TreeViewTemplate.Nodes[0].Nodes)
                    {
                        bool NodePresent = false;
                        foreach (System.Windows.Forms.TreeNode Ncontent in this.treeView.Nodes[0].Nodes)
                        {
                            string ContentNode = Ncontent.Text;
                            if (ContentNode.IndexOf(":") > -1)
                                ContentNode = ContentNode.Substring(0, ContentNode.IndexOf(":"));
                            if (Ntemplate.Text == ContentNode)
                            {
                                this.MergeTemplateNode(Ntemplate, Ncontent);
                                NodePresent = true;
                                break;
                            }
                        }
                        if (!NodePresent)
                        {
                            System.Windows.Forms.TreeNode N = new TreeNode(Ntemplate.Text);
                            this.treeView.Nodes[0].Nodes.Add(N);
                            this.AddChildNodes(Ntemplate, N);
                        }
                    }
                }
            }
        }

        private void MergeTemplateNode(System.Windows.Forms.TreeNode NodeTemplate, System.Windows.Forms.TreeNode Node)
        {
            foreach (System.Windows.Forms.TreeNode Ntemplate in NodeTemplate.Nodes)
            {
                bool NodePresent = false;
                foreach (System.Windows.Forms.TreeNode Ncontent in Node.Nodes)
                {
                    string ContentNode = Ncontent.Text;
                    if (ContentNode.IndexOf(":") > -1)
                        ContentNode = ContentNode.Substring(0, ContentNode.IndexOf(":"));
                    if (Ntemplate.Text == ContentNode)
                    {
                        this.MergeTemplateNode(Ntemplate, Ncontent);
                        NodePresent = true;
                        break;
                    }
                }
                if (!NodePresent)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(Ntemplate.Text);
                    Node.Nodes.Add(N);
                    this.AddChildNodes(Ntemplate, N);
                }
            }
        }

        private void AddChildNodes(System.Windows.Forms.TreeNode NodeTemplate, System.Windows.Forms.TreeNode Node)
        {
            foreach (System.Windows.Forms.TreeNode Ntemplate in NodeTemplate.Nodes)
            {
                System.Windows.Forms.TreeNode Nnode = new TreeNode(Ntemplate.Text);
                Node.Nodes.Add(Nnode);
                this.AddChildNodes(Ntemplate, Nnode);
            }
        }
        
        #endregion

        #endregion


    }
}
