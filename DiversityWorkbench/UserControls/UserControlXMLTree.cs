using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public struct XMLNode
    {
        public string Name;
        public string Value;
        public System.Collections.Generic.List<XMLAttribute> Attributes;
    }

    public struct XMLAttribute
    {
        public string Name;
        public string Value;
    }


    public partial class UserControlXMLTree : UserControl
    {
        #region Properties

        private string _XML;

        #endregion

        #region Construction and init

        public UserControlXMLTree()
        {
            InitializeComponent();
        }

        public void initControl(bool CanSeeContent, bool CanEditContent, bool CanEditNode, bool CanAddNode, bool CanDeleteNode)
        {
            this.textBoxContent.Visible = CanSeeContent;
            this.textBoxContent.Enabled = CanEditContent;

            this.textBoxNode.Visible = CanEditNode;
            this.labelNode.Visible = CanEditNode;

            this.buttonAddNode.Visible = CanAddNode;
            this.buttonRemoveNode.Visible = CanDeleteNode;
        }

        public void setToDisplayOnly()
        {
            this.textBoxContent.Visible = false;
            this.textBoxNode.Visible = false;
            this.labelNode.Visible = false;
            this.labelContent.Visible = false;
            this.buttonAddNode.Visible = false;
            this.buttonRemoveNode.Visible = false;
            this.treeView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.treeView.ForeColor = System.Drawing.Color.Gray;
            this.treeView.BorderStyle = BorderStyle.FixedSingle;
        }

        #endregion

        #region Interface

        public string XML
        {
            get
            {
                return this.XmlFromTree();
            }
            set
            {
                this._XML = value;
                this.buildTree();
            }
        }

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

        //public System.Xml.XmlNode CurrentNode
        //{
        //    //get { return this. }
        //}

        public string CurrentContent { get { return this.textBoxContent.Text; } }

        #endregion

        #region Tree

        private void buildTree()
        {
            try
            {
                this.treeView.Nodes.Clear();
                System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(this._XML, System.Xml.XmlNodeType.Element, null);
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

        private string XmlFromTree()
        {
            System.IO.FileInfo XmlFile = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation) + "ImageDescription.xml");

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
            R.Dispose();
            XmlFile.Delete();
            return XML;
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
                    XMLNode N = new XMLNode();
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

        private bool XmlFromTreeAddChild(System.Windows.Forms.TreeNode TreeNode, ref System.Xml.XmlWriter W)
        {
            try
            {
                if (TreeNode.Tag != null)
                {
                    try
                    {
                        XMLNode N = (XMLNode)TreeNode.Tag;
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
                foreach (char C in NotAllowedSigns)
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

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeView.SelectedNode.Tag != null)
            {
                if (typeof(XMLNode) == this.treeView.SelectedNode.Tag.GetType())
                {
                    XMLNode N = (XMLNode)this.treeView.SelectedNode.Tag;
                    if (N.Name == "#text")
                    {
                        if (this.treeView.SelectedNode.Text.IndexOf(":") > -1)
                            this.labelContent.Text = this.treeView.SelectedNode.Text.Substring(0, this.treeView.SelectedNode.Text.IndexOf(":") + 1);
                        else
                            this.labelContent.Text = this.treeView.SelectedNode.Text + ":";
                    }
                    else
                        this.labelContent.Text = N.Name + ":";
                    this.textBoxContent.Text = N.Value;
                }
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
            XMLNode N;
            if (this.treeView.SelectedNode.Tag != null)
            {
                N = (XMLNode)this.treeView.SelectedNode.Tag;
            }
            else
            {
                N = new XMLNode();
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
        }

        #endregion

    }
}
