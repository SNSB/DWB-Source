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
    public partial class FormEditXmlTemplate : Form
    {

        #region Parameter

        private string _Template = "";
        private string _DisplayColumn = "";
        private string _TemplateColumn = "";
        private System.Data.DataTable _dtSource;
        
        #endregion

        #region Construction and form

        private FormEditXmlTemplate()
        {
            InitializeComponent();
        }

        public FormEditXmlTemplate(System.Xml.XmlDocument Doc)
            : this()
        {
        }

        public FormEditXmlTemplate(string Template, string Header)
            : this()
        {
            this._Template = Template;
            this.labelHeader.Text = Header;
            this.buildTemplateTree();
        }

        public FormEditXmlTemplate(string Template, string Header, System.Data.DataTable DtSource, string DisplayColumn, string TemplateColumn)
            : this(Template, Header)
        {
            this._dtSource = DtSource;
            this._DisplayColumn = DisplayColumn;
            this._TemplateColumn = TemplateColumn;
            if (this._dtSource.Rows.Count > 0)
                this.buttonSearchTemplate.Visible = true;
            else
                this.buttonSearchTemplate.Visible = false;
        }

        private void FormEditXmlTemplate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
                this._Template = this.XmlFromTree();
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

        public string Template 
        { 
            get { return this._Template; }
            set
            {
                this._Template = value;
                this.buildTemplateTree();
            }
        }

        public string TemplateWithoutEncoding
        {
            get
            {
                string XML = this._Template;
                if (XML.IndexOf("\"?>") > -1)
                    XML = XML.Substring(XML.IndexOf("\"?>") + 3);
                return XML;
            }
        }

        #endregion

        #region Edit template

        private void buildTemplateTree()
        {
            try
            {
                this.buildTemplateTree(this._Template);
            }
            catch { }
        }

        private void buildTemplateTree(string XML)
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
                    inTreeNode.Parent.Tag = inXmlNode.Value;
                    inTreeNode.Parent.Text = inTreeNode.Parent.Text + ": " + inXmlNode.Value;
                    inTreeNode.Remove();
                }
            }
        }

        private string XmlFromTree()
        {
            System.IO.FileInfo XmlFile = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation) + "ImageDescriptionTemplate.xml");

            string XML = "";
            if (this.treeView.Nodes.Count == 0) return "";
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            //settings.Encoding = System.Text.Encoding.Unicode;
            W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
            W.WriteStartElement(this.treeView.Nodes[0].Text);
            foreach (System.Windows.Forms.TreeNode N in this.treeView.Nodes[0].Nodes)
            {
                this.XmlFromTreeAddChild(N, ref W);
            }
            W.WriteEndElement();
            W.Flush();
            W.Close();
            System.IO.StreamReader R = new System.IO.StreamReader(XmlFile.FullName);
            XML = R.ReadToEnd();
            R.Close();
            return XML;
        }

        private void XmlFromTreeAddChild(System.Windows.Forms.TreeNode TreeNode, ref System.Xml.XmlWriter W)
        {
            string Node = TreeNode.Text.Replace(" ", "_");
            if (Node.IndexOf(":") > -1) Node = Node.Substring(0, Node.IndexOf(":"));
            if (TreeNode.Tag != null)
                W.WriteElementString(Node, TreeNode.Tag.ToString());
            else
            {
                W.WriteStartElement(Node);
                foreach (System.Windows.Forms.TreeNode NChild in TreeNode.Nodes)
                    this.XmlFromTreeAddChild(NChild, ref W);
                W.WriteEndElement();
            }
        }

        private void buttonAddNode_Click(object sender, EventArgs e)
        {
            if (this.treeView.Nodes.Count > 0
                && this.treeView.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the parent entry for the new node");
                return;
            }
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New node", "Please enter the name of the new node", "");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Collections.Generic.List<char> NotAllowedSigns = new List<char>();
                NotAllowedSigns.Add(' ');
                NotAllowedSigns.Add('(');
                NotAllowedSigns.Add(')');
                NotAllowedSigns.Add('<');
                NotAllowedSigns.Add('>');
                NotAllowedSigns.Add('"');
                NotAllowedSigns.Add('\'');
                NotAllowedSigns.Add('/');
                foreach(char C in NotAllowedSigns)
                {
                    if (f.String.IndexOf(C) > -1)
                    {
                        System.Windows.Forms.MessageBox.Show("Names can not contain " + C.ToString());
                        return;
                    }
                }
                if (this.treeView.Nodes.Count == 0)
                    this.treeView.Nodes.Add(f.String);
                else
                {
                    if (this.treeView.SelectedNode == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Please select the parent entry for the new node");
                        return;
                    }
                    else
                    {
                        this.treeView.SelectedNode.Nodes.Add(f.String);
                        this.treeView.SelectedNode.Expand();
                    }
                }
            }
        }

        private void buttonRemoveNode_Click(object sender, EventArgs e)
        {
            if (this.treeView.SelectedNode != null)
            {
                this.treeView.SelectedNode.Remove();
            }
            else
                System.Windows.Forms.MessageBox.Show("Please select the node you want to remove");
        }

        private void buttonSearchTemplate_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormSelectXmlTemplate f = new DiversityWorkbench.Forms.FormSelectXmlTemplate(
                this._dtSource, this._DisplayColumn, this._TemplateColumn);
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.Template.Length > 0)
            {
                this.Template = f.Template;
            }
        }

       #endregion


    }
}
