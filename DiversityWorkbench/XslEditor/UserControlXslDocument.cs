using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.XslEditor
{
    public partial class UserControlXslDocument : UserControl
    {

        #region Parameter and properties

        private DiversityWorkbench.XslEditor.XslEditor _XslEditor;

        private bool _IsIncluded;

        #endregion

        #region Construction

        public UserControlXslDocument()
        {
            InitializeComponent();
            this.setNodeControls();
        }
        
        #endregion

        #region Document

        private void toolStripButtonDocumentAdd_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonDocumentDelete_Click(object sender, EventArgs e)
        {

        }
        
        private void treeViewDocument_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                string Template = this.treeViewDocument.SelectedNode.Text;
                DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewDocument.SelectedNode.Tag;
                DiversityWorkbench.XslEditor.XslEditor.CreateNodeHierarchy(N, this.treeViewNode, this._XslEditor);
                //this.XslEditor.CreateTemplateHierarchy(Template, this.treeViewNode, false);
                this.treeViewNode.ExpandAll();
                this.treeViewNode_AfterSelect(null, null);
                this.setNodeControls();
                this.markCurrentTemplateTreeNode(this.treeViewDocument.SelectedNode, this.treeViewDocument);
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Node

        private void setNodeControls()
        {
            if (this.treeViewNode.SelectedNode == null)
            {
                this.labelNodeAttributes.Visible = false;
                this.labelNodeContent.Visible = false;
                this.labelNodeValue.Visible = false;
                this.toolStripNodes.Visible = false;
                this.toolStripNodeAttributes.Visible = false;
                this.toolStripNodeValues.Visible = false;
                this.textBoxNodeContent.Visible = false;
                this.panelNodeAttributes.Visible = false;
                this.panelNodeValues.Visible = false;
            }
            else
            {
                this.labelNodeAttributes.Visible = true;
                this.labelNodeContent.Visible = true;
                this.toolStripNodes.Visible = true;
                this.toolStripNodeAttributes.Visible = true;
                this.textBoxNodeContent.Visible = true;
                this.panelNodeAttributes.Visible = true;

                this.toolStripButtonTableEditor.Visible = false;
                this.toolStripButtonNodeDown.Visible = false;
                this.toolStripButtonNodeUp.Visible = false;
                this.toolStripButtonNodeAdd.Visible = false;
                this.toolStripButtonNodeCopy.Visible = false;
                this.toolStripButtonNodeNew.Visible = false;
                this.panelNodeValues.Visible = false;
                this.panelNodeValues.Controls.Clear();
                this.labelNodeValue.Visible = false;
                this.toolStripNodeValues.Visible = false;

                DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewNode.SelectedNode.Tag;

                if (N != null)
                {
                    // toolStripButtonTableEditor
                    if (N.Name == "table" && N.XslNodeType == "html")
                    {
                        this.toolStripButtonTableEditor.Visible = true;
                    }

                    // toolStripButtonNodeUp & toolStripButtonNodeDown
                    if ((N.Name == "td"
                        || N.Name == "tr"
                        || N.Name == "th")
                        && N.XslNodeType == "html")
                    {
                        if (N.ParentXslNode.XslNodes.Count > 1)
                        {
                            if (N.ParentXslNode.XslNodes[0] != N)
                                this.toolStripButtonNodeUp.Visible = true;
                            if (N.ParentXslNode.XslNodes[N.ParentXslNode.XslNodes.Count - 1] != N)
                                this.toolStripButtonNodeDown.Visible = true;
                        }
                    }

                    // toolStripButtonNodeAdd
                    if ((N.Name == "table" || N.Name == "tr" || N.Name == "td") && N.XslNodeType == "html")
                    {
                        this.toolStripButtonNodeAdd.Visible = true;
                    }

                    // toolStripButtonNodeCopy
                    if ((N.Name == "tr" || N.Name == "td" || N.Name == "th") && N.XslNodeType == "html")
                    {
                        this.toolStripButtonNodeCopy.Visible = true;
                    }

                    // toolStripButtonNodeNew
                    if ((N.Name == "tr" || N.Name == "td" || N.Name == "th") && N.XslNodeType == "html")
                    {
                        this.toolStripButtonNodeNew.Visible = true;
                    }

                    if (N.Name == "")
                    {
                        this.labelNodeAttributes.Visible = false;
                        this.toolStripNodeAttributes.Visible = false;
                    }

                    //bool IsFont = false;
                    //foreach (string s in DiversityWorkbench.XslEditor.XslEditor.HtmlFontFormats())
                    //{
                    //    if (N.Content != null
                    //        && N.Content.Length > 0
                    //        && N.Content.IndexOf(s) > -1)
                    //    {
                    //        IsFont = true;
                    //        break;
                    //    }
                    //}

                    if (N.ParentXslNode != null
                        && N.ParentXslNode.XslNodes.Count == 1
                        && N.ParentXslNode.XslNodeType == "xsl:variable"
                        && N.ContentParts() != null
                        && N.ContentParts().Count > 0)
                    {
                        this.panelNodeValues.Visible = true;
                        this.panelNodeValues.Controls.Clear();
                        this.labelNodeValue.Text = "Font formatting";
                        this.labelNodeValue.Visible = true;
                        this.toolStripNodeValues.Visible = true;
                        this.textBoxNodeContent.Text = "";
                        this.textBoxNodeContent.Visible = false;
                        this.labelNodeContent.Visible = false;
                        int i = 1;
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in N.ContentParts())
                        {
                            DiversityWorkbench.XslEditor.UserControlXslAttribute U = new UserControlXslAttribute(KV.Key, KV.Value, N);
                            this.panelNodeValues.Controls.Add(U);
                            U.Dock = DockStyle.Top;
                            U.BringToFront();
                            this.panelNodeValues.Height = U.Height * i;
                            i++;
                        }

                        //string VariableValue = N.Content;
                        //string[] Values = VariableValue.Split(new char[] { ';' });
                        //for (int i = 0; i < Values.Length; i++)
                        //{
                        //    string[] ValuePart = Values[i].Split(new char[] { ':' });
                        //    if (ValuePart.Length == 2)
                        //    {
                        //        DiversityWorkbench.XslEditor.UserControlXslAttribute U = new UserControlXslAttribute(ValuePart[0].Trim(), ValuePart[1].Trim(), N);
                        //        this.panelNodeValues.Controls.Add(U);
                        //        U.Dock = DockStyle.Top;
                        //        U.BringToFront();
                        //        this.panelNodeValues.Height = U.Height * i;
                        //    }
                        //    else if (ValuePart.Length == 1 && ValuePart[0].Length > 0)
                        //    {
                        //        DiversityWorkbench.XslEditor.UserControlXslAttribute U = new UserControlXslAttribute("", ValuePart[0], N);
                        //        this.panelNodeValues.Controls.Add(U);
                        //        U.Dock = DockStyle.Top;
                        //        U.BringToFront();
                        //        this.panelNodeValues.Height = U.Height * i;
                        //    }
                        //}
                    }
                    else
                    {
                        this.panelNodeValues.Visible = false;
                        this.panelNodeValues.Controls.Clear();
                        this.labelNodeValue.Visible = false;
                        this.toolStripNodeValues.Visible = false;
                        this.textBoxNodeContent.Text = N.Content;
                        this.textBoxNodeContent.Visible = true;
                        this.labelNodeContent.Visible = true;
                    }

                }
            }
        }

        private void toolStripButtonNodeNew_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewNode.SelectedNode.Tag;
            DiversityWorkbench.XslEditor.XslNode NParent = N.ParentXslNode;
            System.Collections.Generic.Dictionary<string, string> NewNames = DiversityWorkbench.XslEditor.XslEditor.NewNodeNames(NParent);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(NewNames, "New node", "Please select a name for the new node", false);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityWorkbench.XslEditor.XslNode Nnew = new XslNode(f.String);
            }
        }

        private void toolStripButtonNodeCopy_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewNode.SelectedNode.Tag;
            DiversityWorkbench.XslEditor.XslNode NParent = N.ParentXslNode;
            DiversityWorkbench.XslEditor.XslNode Nnew = new XslNode(N.XslNodeType, N.Name, N.Attributes);
            NParent.XslNodes.Add(Nnew);
            this.treeViewDocument_AfterSelect(null, null);
            this.moveToTreeNode(Nnew, this.treeViewNode);
        }

        private void toolStripButtonNodeAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewNode.SelectedNode.Tag;
            System.Collections.Generic.Dictionary<string, string> NewNames = DiversityWorkbench.XslEditor.XslEditor.NewNodeNames(N);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(NewNames, "New node", "Please select a name for the new node", false);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityWorkbench.XslEditor.XslNode Nnew = new XslNode(f.String);
            }
        }

        private void toolStripButtonNodeUp_Click(object sender, EventArgs e)
        {
            int OriginalPosition = 0;
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewNode.SelectedNode.Tag;
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
            this.treeViewDocument_AfterSelect(null, null);
            this.moveToTreeNode(N, this.treeViewNode);
            this.markCurrentTemplateTreeNode(N, this.treeViewNode);
        }

        private void toolStripButtonNodeDown_Click(object sender, EventArgs e)
        {
            int OriginalPosition = 0;
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewNode.SelectedNode.Tag;
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
            this.treeViewDocument_AfterSelect(null, null);
            this.moveToTreeNode(N, this.treeViewNode);
            this.markCurrentTemplateTreeNode(N, this.treeViewNode);
        }

        private void toolStripButtonTableEditor_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewNode.SelectedNode.Tag;
                DiversityWorkbench.XslEditor.FormXslTableEditor F = new FormXslTableEditor(N);
                F.StartPosition = FormStartPosition.CenterParent;
                F.Width = this.Width - 10;
                F.Height = this.Height - 10;
                F.ShowDialog();
            }
            catch (System.Exception ex) { }
        }

        private void toolStripButtonNodeDelete_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonNodeAttributeNew_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewNode.SelectedNode.Tag;
            System.Collections.Generic.Dictionary<string, string> L = DiversityWorkbench.XslEditor.XslEditor.NewNodeNames(N);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(L, "New attribute", "Please select the name of the new attribute", true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                N.Attributes.Add(f.String, "");
            }
            this.treeViewNode_AfterSelect(null, null);
        }

        private void treeViewNode_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.panelNodeAttributes.Controls.Clear();
                this.textBoxNodeContent.Text = "";
                if (this.treeViewNode.SelectedNode == null)
                {
                    return;
                }
                this.markCurrentTemplateTreeNode(this.treeViewNode.SelectedNode, this.treeViewNode);
                DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewNode.SelectedNode.Tag;
                if (N != null)
                {
                    if (N.Attributes != null && N.Attributes.Count > 0)
                    {
                        int FinalHeight = 0;
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in N.Attributes)
                        {
                            DiversityWorkbench.XslEditor.UserControlXslAttribute U = new UserControlXslAttribute(KV.Key, KV.Value, N);
                            this.panelNodeAttributes.Controls.Add(U);
                            U.Dock = DockStyle.Top;
                            U.Visible = true;
                            FinalHeight += U.Height;
                        }
                        this.panelNodeAttributes.Height = FinalHeight;
                    }
                    if (N.Content != null && N.Content.Length > 0)
                    {
                        this.textBoxNodeContent.Text = N.Content;
                    }
                }
                this.setNodeControls();
            }
            catch (System.Exception ex) { }
        }

        private void toolStripButtonNodeValuesAdd_Click(object sender, EventArgs e)
        {

        }
        
        #endregion

        #region Auxillary

        private void moveToTreeNode(DiversityWorkbench.XslEditor.XslNode N, System.Windows.Forms.TreeView Tree)
        {
            System.Windows.Forms.TreeNode T = new TreeNode();
            bool NodeFound = false;
            foreach (System.Windows.Forms.TreeNode TN in Tree.Nodes)
            {
                DiversityWorkbench.XslEditor.XslNode Ntag = (DiversityWorkbench.XslEditor.XslNode)TN.Tag;
                if (Ntag == N)
                {
                    Tree.SelectedNode = TN;
                    TN.EnsureVisible();
                    NodeFound = true;
                    break;
                }
            }
            if (!NodeFound)
            {
                foreach (System.Windows.Forms.TreeNode TN in Tree.Nodes)
                {
                    T = this.GetTreeNode(N, TN);
                    if (T != null)
                    {
                        T.EnsureVisible();
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

        private void markCurrentTemplateTreeNode(System.Windows.Forms.TreeNode Tmark, System.Windows.Forms.TreeView Tree)
        {
            foreach (System.Windows.Forms.TreeNode TN in Tree.Nodes)
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

        private void markCurrentTemplateTreeNode(DiversityWorkbench.XslEditor.XslNode Nmark, System.Windows.Forms.TreeView Tree)
        {
            foreach (System.Windows.Forms.TreeNode TN in Tree.Nodes)
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

        private void textBoxNodeContent_Leave(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslNode N = (DiversityWorkbench.XslEditor.XslNode)this.treeViewNode.SelectedNode.Tag;
            N.Content = this.textBoxNodeContent.Text;
        }

        #endregion

        #region Interface
        
        public void SetIsIncluded(bool IsIncluded)
        {
            this._IsIncluded = IsIncluded;
            if (this._IsIncluded)
            {
                this.treeViewDocument.BackColor = System.Drawing.SystemColors.Control;
                this.treeViewNode.BackColor = System.Drawing.SystemColors.Control;
            }
            this.textBoxNodeContent.Enabled = !IsIncluded;
            this.toolStripNodeAttributes.Enabled = !IsIncluded;
            this.toolStripDocument.Enabled = !IsIncluded;
            this.toolStripNodes.Enabled = !IsIncluded;
            this.toolStripNodeValues.Enabled = !IsIncluded;
            this.panelNodeAttributes.Enabled = !IsIncluded;
            this.panelNodeValues.Enabled = !IsIncluded;
            this.labelIncludedFile.Visible = IsIncluded;
            if (IsIncluded && this._XslEditor != null && this._XslEditor.XsltFile != null)
                this.labelIncludedFile.Text += this._XslEditor.XsltFile.FullName;
        }

        public void setXslEditor(DiversityWorkbench.XslEditor.XslEditor Editor)
        {
            _XslEditor = Editor;
            this.treeViewDocument.Nodes.Clear();
            if (this._XslEditor != null)
            {
                foreach (DiversityWorkbench.XslEditor.XslNode N in this._XslEditor.XslDocumentNodes)
                {
                    System.Windows.Forms.TreeNode TN = DiversityWorkbench.XslEditor.XslEditor.XslTreeNode(N, this._XslEditor);
                    TN.Tag = N;
                    this.treeViewDocument.Nodes.Add(TN);
                    if (N.Name == "xsl:include")
                        TN.BackColor = DiversityWorkbench.XslEditor.XslEditor.ColorIncluded;
                    else if (N.Name == "xsl:variable")
                        TN.ForeColor = DiversityWorkbench.XslEditor.XslEditor.ColorVariable;
                }
            }
        }
        
        #endregion

    }
}
