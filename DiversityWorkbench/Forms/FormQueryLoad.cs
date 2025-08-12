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
    public partial class FormQueryLoad : Form
    {

        #region Parameter
        private string _XML;
        private Query _Query;

        #endregion

        #region Construction

        public FormQueryLoad(string XML)
        {
            InitializeComponent();
            this._XML = XML;
            this.initForm();
        }


        #endregion

        #region Form

        private void initForm()
        {
            this.buildQueryTree();
        }

        private void buttonDeleteQuery_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region XML-Tree

        private void buildQueryTree()
        {
            try
            {
                this.treeView.Nodes.Clear();
                System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(_XML, System.Xml.XmlNodeType.Element, null);
                System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                dom.Load(tr);
                if (dom.DocumentElement == null) return;
                if (dom.DocumentElement.ChildNodes.Count >= 0)
                {
                    // SECTION 2. Initialize the TreeView control.
                    this.treeView.Nodes.Clear();
                    //if (dom.DocumentElement.Name.Length > 0)
                    this.treeView.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name));
                    System.Windows.Forms.TreeNode Nbase = this.treeView.Nodes[0];
                    this.SetNodeFont(ref Nbase);
                    System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
                    this.SetNodeFont(ref tNode);
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
            if (inXmlNode.HasChildNodes && inXmlNode.Name != "Query")
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    if (xNode.Name == "Query")
                        this.AddNode(xNode, inTreeNode);
                    else
                        inTreeNode.Nodes.Add(new System.Windows.Forms.TreeNode(xNode.Name));
                    if (i >= inTreeNode.Nodes.Count)
                        continue;
                    tNode = inTreeNode.Nodes[i];
                    this.SetNodeFont(ref tNode);
                    if (xNode.Name != "Query")
                        this.AddNode(xNode, tNode);
                }
            }
            else if (inXmlNode.Name == "Query")
            {
                Query _Query = new Query();
                _Query.Optimized = false;
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    switch (xNode.Name)
                    {
                        case "Title":
                            _Query.Title = xNode.InnerText;
                            break;
                        case "SQL":
                            _Query.SQL = xNode.InnerText;
                            break;
                        case "Description":
                            _Query.Description = xNode.InnerText;
                            break;
                        case "QueryTable":
                            _Query.QueryTable = xNode.InnerText;
                            break;
                        case "Optimized":
                            _Query.Optimized = bool.Parse(xNode.InnerText);
                            break;
                    }
                }
                tNode = new TreeNode(_Query.Title);
                tNode.Tag = _Query;
                this.SetNodeFont(ref tNode);
                inTreeNode.Nodes.Add(tNode);
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

        #endregion

        #region Tree

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeView.SelectedNode != null
                && this.treeView.SelectedNode.Tag != null)
            {
                try
                {
                    DiversityWorkbench.Forms.Query Q = (Query)this.treeView.SelectedNode.Tag;
                    if ((Q.Optimized && DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing) ||
                        (!Q.Optimized && !DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing))
                        this.Query = (Query)this.treeView.SelectedNode.Tag;
                    else if (!Q.Optimized && DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing)
                        System.Windows.Forms.MessageBox.Show("Not optimized queries can only be used if optimize queries is not set");
                    else if (Q.Optimized && !DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing)
                        System.Windows.Forms.MessageBox.Show("Optimized queries can only be used if optimize queries is set");
                }
                catch { }
            }
            else
            {
                this.QueryReset();
            }
        }

        private void SetNodeFont(ref System.Windows.Forms.TreeNode N)
        {
            if (N.Tag == null)
            {
                N.NodeFont = this.GroupFont;
                N.ForeColor = System.Drawing.Color.Gray;
                N.ImageIndex = 0;
                N.SelectedImageIndex = 0;
            }
            else
            {
                N.ForeColor = System.Drawing.Color.Blue;
                try
                {
                    DiversityWorkbench.Forms.Query Q = (DiversityWorkbench.Forms.Query)N.Tag;
                    if (Q.Optimized)
                    {
                        N.SelectedImageIndex = 2;
                        N.ImageIndex = 2;
                        if (DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing)
                        {
                            N.BackColor = System.Drawing.Color.Yellow;
                            N.ForeColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            N.BackColor = System.Drawing.Color.White;
                            N.ForeColor = System.Drawing.Color.LightGray;
                        }
                    }
                    else
                    {
                        N.SelectedImageIndex = 1;
                        N.ImageIndex = 1;
                        if (DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing)
                        {
                            N.BackColor = System.Drawing.Color.White;
                            N.ForeColor = System.Drawing.Color.LightGray;
                        }
                        else
                        {
                            N.BackColor = System.Drawing.Color.White;
                            N.ForeColor = System.Drawing.Color.Blue;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        private System.Drawing.Font GroupFont
        {
            get
            {
                System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8, System.Drawing.FontStyle.Bold);
                return F;
            }
        }

        #endregion

        #region Query

        public Query Query
        {
            get { return _Query; }
            set
            {
                _Query = value;
                this.textBoxQuery.Text = _Query.Title;
                this.textBoxDescription.Text = _Query.Description;
                this.textBoxSQL.Text = _Query.SQL;
                this.labelQueryTable.Text = _Query.QueryTable;
            }
        }

        private void QueryReset()
        {
            Query Q = new Query();
            Q.SQL = "";
            Q.QueryTable = "";
            Q.Title = "";
            Q.Description = "";
            this.Query = Q;
        }

        #endregion

    }
}
