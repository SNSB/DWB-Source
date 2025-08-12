using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormQueryOptions : Form
    {

        #region Parameter

        private DiversityWorkbench.QueryCondition[] Conditions;
        private System.Windows.Forms.ImageList _ImageList;
        private System.Collections.Generic.Dictionary<string, System.Drawing.Color> _ConditionColors;
        private System.Collections.Generic.Dictionary<string, int> _ConditionImageIndex;
        private System.Windows.Forms.TreeNode _FirstFoundNode;

        #endregion

        #region Construction

        private FormQueryOptions()
        {
            InitializeComponent();
            int i = 100;
            if (DiversityWorkbench.Settings.QueryMaxResults > 0)
                i = DiversityWorkbench.Settings.QueryMaxResults;
            this.textBoxMaxItems.Text = i.ToString();
            this.textBoxQueryLimitDropdownList.Text = DiversityWorkbench.Settings.QueryLimitDropdownList.ToString();
            this.textBoxQueryLimitHierarchy.Text = DiversityWorkbench.Settings.QueryLimitHierarchy.ToString();
            string Path = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.Length - 4) + ".chm";
            this.helpProvider.HelpNamespace = Path;
            this.userControlDialogPanel.buttonOK.Click += new System.EventHandler(this.SaveSettings);
        }

        private FormQueryOptions(int MaxNumOfResult)
        {
            InitializeComponent();
            //string test = DiversityWorkbench.WorkbenchSettings.Default.DatabaseName;
            int i = 100;
            if (DiversityWorkbench.Settings.QueryMaxResults > 0)
                i = DiversityWorkbench.Settings.QueryMaxResults;
            else i = MaxNumOfResult;
            this.textBoxMaxItems.Text = i.ToString();
            this.textBoxQueryLimitDropdownList.Text = DiversityWorkbench.Settings.QueryLimitDropdownList.ToString();
            this.textBoxQueryLimitHierarchy.Text = DiversityWorkbench.Settings.QueryLimitHierarchy.ToString();
            string Path = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.Length - 4) + ".chm";
            this.helpProvider.HelpNamespace = Path;
            this.userControlDialogPanel.buttonOK.Click += new System.EventHandler(this.SaveSettings);
        }

        public FormQueryOptions(DiversityWorkbench.QueryCondition[] QueryConditions, int MaxNumOfResult)
            : this(MaxNumOfResult)
        {
            this.Conditions = QueryConditions;
            this.buildTree();
        }

        public FormQueryOptions(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions, int MaxNumOfResult)
            : this(MaxNumOfResult)
        {
            if (QueryConditions != null)
            {
                this.Conditions = new QueryCondition[QueryConditions.Count];
                for (int i = 0; i < QueryConditions.Count; i++)
                {
                    this.Conditions[i] = QueryConditions[i];
                }
                this.buildTree();
            }
        }

        public FormQueryOptions(
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions
            , int MaxNumOfResult
            , System.Windows.Forms.ImageList ImageList
            , System.Collections.Generic.Dictionary<string, System.Drawing.Color> ConditionColors
            , System.Collections.Generic.Dictionary<string, int> ConditionImageIndex
            )
            : this(MaxNumOfResult)
        {
            this._ImageList = ImageList;
            this.treeViewQueryOptions.ImageList = this._ImageList;
            this._ConditionImageIndex = ConditionImageIndex;
            this._ConditionColors = ConditionColors;
            if (QueryConditions != null)
            {
                this.Conditions = new QueryCondition[QueryConditions.Count];
                for (int i = 0; i < QueryConditions.Count; i++)
                {
                    this.Conditions[i] = QueryConditions[i];
                }
                this.buildTree();
            }
        }

        public FormQueryOptions(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions, string Visibility)
            : this(100)
        {
            this.Conditions = new QueryCondition[QueryConditions.Count];
            for (int i = 0; i < QueryConditions.Count; i++)
            {
                this.Conditions[i] = QueryConditions[i];
            }
            this.labelMaxItems.Visible = false;
            this.textBoxMaxItems.Visible = false;
            this.labelQueryLimitDropdownList.Visible = false;
            this.textBoxQueryLimitDropdownList.Visible = false;
            this.buildTree(Visibility);
        }

        public FormQueryOptions(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions, string Visibility, string Title)
            : this(100)
        {
            this.Text = Title;
            this.Conditions = new QueryCondition[QueryConditions.Count];
            for (int i = 0; i < QueryConditions.Count; i++)
            {
                this.Conditions[i] = QueryConditions[i];
            }
            this.labelMaxItems.Visible = false;
            this.textBoxMaxItems.Visible = false;
            this.labelQueryLimitDropdownList.Visible = false;
            this.textBoxQueryLimitDropdownList.Visible = false;
            this.buildTree(Visibility);
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

        #region Events

        private void SaveSettings(object sender, EventArgs e)
        {
            int i = 0;
            if (int.TryParse(this.textBoxMaxItems.Text, out i))
                DiversityWorkbench.Settings.QueryMaxResults = i;
            if (int.TryParse(this.textBoxQueryLimitDropdownList.Text, out i))
                DiversityWorkbench.Settings.QueryLimitDropdownList = i;
            if (int.TryParse(this.textBoxQueryLimitHierarchy.Text, out i))
                DiversityWorkbench.Settings.QueryLimitHierarchy = i;
            if (this.checkBoxLimitCharacterCount.Checked && int.TryParse(this.numericUpDownLimitCharacterCount.Value.ToString(), out i))
                DiversityWorkbench.Settings.QueryLimitCharaterCount = i;
            else
                DiversityWorkbench.Settings.QueryLimitCharaterCount = -1;
            DiversityWorkbench.WorkbenchSettings.Default.Save();
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

        #region Tree view

        private void buildTree()
        {
            int i = this.Conditions.Length;
            string Group = "";
            string[] Groups = new string[i];
            int iG = 0;
            for (int iC = 0; iC < this.Conditions.Length; iC++)
            {
                bool GroupPresent = false;
                foreach (string G in Groups)
                {
                    if (G == this.Conditions[iC].QueryGroup) GroupPresent = true;
                }
                if (!GroupPresent)
                {
                    Groups[iG] = this.Conditions[iC].QueryGroup;
                    iG++;
                    System.Windows.Forms.TreeNode n = new TreeNode(this.Conditions[iC].QueryGroup);
                    if (this._ImageList != null
                        && this._ConditionImageIndex != null)
                    {
                        if (this._ConditionImageIndex.ContainsKey(this.Conditions[iC].Table))
                        {
                            n.ImageIndex = this._ConditionImageIndex[this.Conditions[iC].Table];
                            n.SelectedImageIndex = this._ConditionImageIndex[this.Conditions[iC].Table];
                        }
                        if (this._ConditionImageIndex.ContainsKey(this.Conditions[iC].QueryGroup))
                        {
                            n.ImageIndex = this._ConditionImageIndex[this.Conditions[iC].QueryGroup];
                            n.SelectedImageIndex = this._ConditionImageIndex[this.Conditions[iC].QueryGroup];
                        }
                    }
                    if (this._ConditionColors != null)
                    {
                        if (this._ConditionColors.ContainsKey(this.Conditions[iC].Table))
                            n.ForeColor = this._ConditionColors[this.Conditions[iC].Table];
                    }
                    this.treeViewQueryOptions.Nodes.Add(n);
                    n.Checked = this.addChildNodes(n);
                    Group = this.Conditions[iC].QueryGroup;
                }
            }
            this.treeViewQueryOptions.ExpandAll();
        }

        private void buildTree(string Visibility)
        {
            int i = this.Conditions.Length;
            string Group = "";
            string[] Groups = new string[i];
            int iG = 0;
            for (int iC = 0; iC < this.Conditions.Length; iC++)
            {
                if (Visibility.Length > iC)
                {
                    if (Visibility[iC].ToString() == "0")
                        this.Conditions[iC].showCondition = false;
                    else
                        this.Conditions[iC].showCondition = true;
                }
                else
                    this.Conditions[iC].showCondition = false;
            }
            for (int iC = 0; iC < this.Conditions.Length; iC++)
            {
                bool GroupPresent = false;
                foreach (string G in Groups)
                {
                    if (G == this.Conditions[iC].QueryGroup) GroupPresent = true;
                }
                if (!GroupPresent)
                {
                    Groups[iG] = this.Conditions[iC].QueryGroup;
                    iG++;
                    System.Windows.Forms.TreeNode n = new TreeNode(this.Conditions[iC].QueryGroup);
                    this.treeViewQueryOptions.Nodes.Add(n);
                    n.Checked = this.addChildNodes(n);
                    Group = this.Conditions[iC].QueryGroup;
                }
            }
            this.treeViewQueryOptions.ExpandAll();
        }

        private bool addChildNodes(System.Windows.Forms.TreeNode N)
        {
            bool Checked = true;
            for (int i = 0; i < this.Conditions.Length; i++) //DiversityWorkbench.QueryCondition C in this.Conditions)
            {
                if (this.Conditions[i].QueryGroup == N.Text)
                {
                    if (!this.Conditions[i].showCondition) Checked = false;

                    string TreeNodeDisplayText = this.Conditions[i].DisplayText; // this.Conditions[i].Display(this.textBoxMaxItems, DiversityWorkbench.Entity.EntityInformationField.DisplayText);
                    string Description = this.Conditions[i].Description;
                    if (DiversityWorkbench.Entity.EntityTablesExist)
                    {
                        string TC = this.Conditions[i].Table;
                        if (TC.IndexOf("_Core") > -1)
                            TC = TC.Substring(0, TC.IndexOf("_Core"));
                        TC += "." + this.Conditions[i].Column;
                        System.Collections.Generic.Dictionary<string, string> Entity = DiversityWorkbench.Entity.EntityInformation(TC);
                        if (DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, Entity).Length > 0)
                            TreeNodeDisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, Entity);
                        if (DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Description, Entity).Length > 0)
                            Description = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Description, Entity);
                    }
                    System.Windows.Forms.TreeNode n = new TreeNode(TreeNodeDisplayText);
                    n.Checked = this.Conditions[i].showCondition;
                    n.Tag = i;
                    if (this._ConditionColors != null)
                    {
                        if (this._ConditionColors.ContainsKey(this.Conditions[i].Table))
                            n.ForeColor = this._ConditionColors[this.Conditions[i].Table];
                    }
                    N.Nodes.Add(n);
                }
            }
            return Checked;
        }

        private void treeViewQueryOptions_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                System.Windows.Forms.TreeNode N = this.treeViewQueryOptions.GetNodeAt(e.X, e.Y);
                if (N == null)
                    return;
                //bool OK = N.Checked;
                //if (N.IsExpanded)
                //{
                //    foreach (System.Windows.Forms.TreeNode tn in N.Nodes)
                //    {
                //        tn.Checked = OK;
                //        if (tn.Tag != null)
                //        {
                //            int i = (int)tn.Tag;
                //            this.Conditions[i].showCondition = OK;
                //        }
                //    }
                //}
                this.setDescription(N);
            }
            catch (System.Exception ex) { }
        }

        private void setDescription(System.Windows.Forms.TreeNode N)
        {
            if (N.Tag != null)
            {
                int n = (int)N.Tag;
                string Description = "Table: ";
                if (this.Conditions[n].Table.IndexOf("_") > -1) Description += this.Conditions[n].Table.Substring(0, this.Conditions[n].Table.IndexOf("_"));
                else Description += this.Conditions[n].Table;
                Description += "\r\nColumn: " + this.Conditions[n].Column + "\r\n\r\n" + this.Conditions[n].Description;
                this.textBoxDescription.Text = Description;
                this.Conditions[n].showCondition = N.Checked;
            }
            else
            {
                this.textBoxDescription.Text = "";// N.Text;
            }
        }

        private void treeViewQueryOptions_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (this.AnyQueryOptionSelected)
                this.userControlDialogPanel.buttonOK.Enabled = true;
            else
                this.userControlDialogPanel.buttonOK.Enabled = false;
            System.Windows.Forms.TreeNode N = e.Node;
            if (N == null)
                return;
            bool OK = N.Checked;
            if (N.IsExpanded)
            {
                foreach (System.Windows.Forms.TreeNode tn in N.Nodes)
                {
                    tn.Checked = OK;
                    if (tn.Tag != null)
                    {
                        int i = (int)tn.Tag;
                        this.Conditions[i].showCondition = OK;
                    }
                }
            }
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.TreeNode N in this.treeViewQueryOptions.Nodes)
                this.setNodeState(true, N);
        }

        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.TreeNode N in this.treeViewQueryOptions.Nodes)
                this.setNodeState(false, N);
        }

        private void setNodeState(bool NodeChecked, System.Windows.Forms.TreeNode N)
        {
            N.Checked = NodeChecked;
            if (N.Tag != null)
            {
                int n = (int)N.Tag;
                this.Conditions[n].showCondition = N.Checked;
            }
            foreach (System.Windows.Forms.TreeNode NC in N.Nodes)
                this.setNodeState(NodeChecked, NC);
        }

        private bool AnyQueryOptionSelected
        {
            get
            {
                foreach (System.Windows.Forms.TreeNode N in this.treeViewQueryOptions.Nodes)
                {
                    if (N.Checked) return true;
                    foreach (System.Windows.Forms.TreeNode NC in N.Nodes)
                        if (NC.Checked) return true;
                }
                return false;
            }
        }

        private void buttonCollapse_Click(object sender, EventArgs e)
        {
            this.treeViewQueryOptions.CollapseAll();
        }

        private void buttonExpand_Click(object sender, EventArgs e)
        {
            this.treeViewQueryOptions.ExpandAll();
        }

        #endregion

        #region Properties

        /// Markus 27.2.2017 - offenbar nicht benutzt
        //public DiversityWorkbench.QueryCondition[] QueryConditions { get { return this.Conditions; } }
        //public DiversityWorkbench.QueryCondition[] QueryConditions() { return this.Conditions; }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditionList
        {
            get
            {
                if (this.Conditions == null)
                    this.Conditions = new QueryCondition[0];
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> Q = new List<QueryCondition>();
                for (int i = 0; i < this.Conditions.Length; i++)
                    Q.Add(this.Conditions[i]);
                return Q;
            }
        }

        public string QueryConditionVisibility
        {
            get
            {
                string V = "";
                foreach (DiversityWorkbench.QueryCondition Q in this.Conditions)
                {
                    if (Q.showCondition)
                    {
                        V += Q.SetCount.ToString();// "1";
                    }
                    else V += "0";
                }
                return V;
            }
        }

        public int MaximalNumberOfResults
        {
            get
            {
                int i = 100;
                if (DiversityWorkbench.Settings.QueryMaxResults > 0)
                    return DiversityWorkbench.Settings.QueryMaxResults;
                if (this.textBoxMaxItems.Text.Length > 0)
                {
                    try
                    {
                        i = System.Int32.Parse(this.textBoxMaxItems.Text);
                    }
                    catch { }
                }
                return i;
            }
        }

        #endregion

        #region Search

        private void ResetMark(System.Windows.Forms.TreeNode N)
        {
            N.BackColor = System.Drawing.SystemColors.Window;
            foreach (System.Windows.Forms.TreeNode NN in N.Nodes)
                ResetMark(NN);
        }

        private bool FindNode(string SearchString, System.Windows.Forms.TreeNode ParentNode)
        {
            bool ContainsFits = false;
            if (ParentNode == null)
            {
                foreach (System.Windows.Forms.TreeNode N in this.treeViewQueryOptions.Nodes)
                {
                    if (this.NodeFitsSelection(N, SearchString))
                    {
                        if (this._FirstFoundNode == null)
                            this._FirstFoundNode = N;
                        N.BackColor = System.Drawing.Color.Yellow;
                        ContainsFits = true;
                    }
                    ContainsFits = this.FindNode(SearchString, N);
                    if (ContainsFits) N.Expand();
                    else N.Collapse();
                }
            }
            else
            {
                foreach (System.Windows.Forms.TreeNode N in ParentNode.Nodes)
                {
                    if (this.NodeFitsSelection(N, SearchString))
                    {
                        if (this._FirstFoundNode == null)
                            this._FirstFoundNode = N;
                        N.BackColor = System.Drawing.Color.Yellow;
                        ContainsFits = true;
                    }
                }
            }
            return ContainsFits;
        }

        private bool NodeFitsSelection(System.Windows.Forms.TreeNode N, string SearchString)
        {
            if (N.Text.IndexOf(SearchString) > -1 || N.Text.ToLower().IndexOf(SearchString.ToLower()) > -1)
            {
                return true;
            }
            if (N.Tag != null)
            {
                int n = (int)N.Tag;
                string Description = "";
                if (this.Conditions[n].Table.IndexOf("_") > -1) Description += this.Conditions[n].Table.Substring(0, this.Conditions[n].Table.IndexOf("_"));
                else Description += this.Conditions[n].Table;
                Description += " " + this.Conditions[n].Column + " " + this.Conditions[n].Description;
                string DisplayText = this.Conditions[n].DisplayText;
                if (Description.ToLower().IndexOf(SearchString.ToLower()) > -1)
                {
                    return true;
                }
                string Abbreviation = "";
                if (this.checkBoxIncludeAbbreviation.Checked)
                {
                    DiversityWorkbench.UserControls.UserControlQueryCondition UCQC = new UserControls.UserControlQueryCondition();
                    Abbreviation = UCQC.ConditionAbbreviation(DisplayText);
                    UCQC.Dispose();
                    if (Abbreviation.ToLower().IndexOf(SearchString.ToLower()) > -1 || SearchString.ToLower().IndexOf(Abbreviation.ToLower()) > -1)
                        return true;
                }
            }
            return false;
        }

        private void buttonSearchNode_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.TreeNode N in this.treeViewQueryOptions.Nodes)
                this.ResetMark(N);
            this._FirstFoundNode = null;
            if (this.textBoxSearch.Text.Length == 0)
                System.Windows.Forms.MessageBox.Show("Please enter a search string");
            else
                this.FindNode(this.textBoxSearch.Text, null);
            if (this._FirstFoundNode != null)
            {
                this._FirstFoundNode.EnsureVisible();
                this.setDescription(this._FirstFoundNode);
            }

        }

        #endregion

    }
}