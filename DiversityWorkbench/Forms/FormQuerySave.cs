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


    public struct Query
    {
        public string Title;
        public string Description;
        public string SQL;
        public string QueryTable;
        public bool Optimized;
    }

    public partial class FormQuerySave : Form
    {

        public static readonly string QueryBaseNode = "Queries";

        #region Parameter

        private enum ProcessState { SetGroup, SetQuery, Edit };
        private ProcessState _ProcessState;
        private string _SQL;
        private string _QueryTable;
        private Query _Query;

        #endregion

        #region Construction

        public FormQuerySave(string SQL, string QueryTable)
        {
            InitializeComponent();
            this._SQL = SQL;
            if (this._SQL.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Forms.FormQuerySaveText.No_query_was_defined);//"No query was defined");
                this.Close();
            }
            this._QueryTable = QueryTable;
            if (this._SQL.Length == 0)
                this._ProcessState = ProcessState.Edit;
            else
                this._ProcessState = ProcessState.SetQuery;
            this.initForm();
        }

        #endregion

        #region Form

        private void initForm()
        {
            if (XML.Length == 0)
                XML = "<" + DiversityWorkbench.Forms.FormQuerySaveText.Queries + "></" + DiversityWorkbench.Forms.FormQuerySaveText.Queries + ">";
            string xml = this.XML;
            this.buildQueryTree(xml);
            this.setControls();
            this.textBoxSQL.Text = this._SQL;
            this.labelQueryTable.Text = this._QueryTable;
            this.setTitle();
        }

        /// <summary>
        /// set the title of the form according to the current state
        /// </summary>
        private void setTitle()
        {
            switch (this._ProcessState)
            {
                case ProcessState.Edit:
                    if (this._SQL.Length == 0)
                        this.Text = DiversityWorkbench.Forms.FormQuerySaveText.Edit_the_queries;// "Edit the queries";
                    else this.Text = DiversityWorkbench.Forms.FormQuerySaveText.Save_and_edit_the_queries;// "Save and edit the queries";
                    break;
                case ProcessState.SetGroup:
                    this.Text = DiversityWorkbench.Forms.FormQuerySaveText.Choose_group;// "Choose group";
                    break;
                case ProcessState.SetQuery:
                    this.Text = DiversityWorkbench.Forms.FormQuerySaveText.Define_query;// "Define query";
                    break;
            }
        }

        private void setControls()
        {
            this.toolStrip.Visible = false;
            this.treeView.Visible = false;
            this.toolStripButtonDelete.Visible = false;

            this.buttonSaveGroup.Visible = false;
            this.textBoxGroup.Visible = false;
            this.labelGroup.Visible = false;

            switch (this._ProcessState)
            {
                case ProcessState.SetQuery:
                    this.labelHeader.Text = DiversityWorkbench.Forms.FormQuerySaveText.Please_enter_the_name_and_descrition_of_the_query;// "Please enter the title and the description of the new query";
                    this.textBoxQuery.ReadOnly = false;
                    this.textBoxDescription.ReadOnly = false;
                    this.buttonSaveQuery.Visible = false;

                    break;
                case ProcessState.SetGroup:
                    this.labelHeader.Text = DiversityWorkbench.Forms.FormQuerySaveText.Please_select_a_group_or_create_a_new_one;// "Please select a query group from the tree or create a new one";
                    this.treeView.Visible = true;
                    this.toolStrip.Visible = true;
                    this.textBoxDescription.ReadOnly = true;
                    this.textBoxQuery.ReadOnly = true;
                    break;
                case ProcessState.Edit:
                    this.labelHeader.Text = DiversityWorkbench.Forms.FormQuerySaveText.Edit_the_query_groups_and_the_queries;// "Edit the query groups and the queries";
                    this.treeView.Visible = true;
                    this.toolStrip.Visible = true;
                    this.toolStripButtonDelete.Visible = true;
                    this.textBoxQuery.ReadOnly = false;
                    this.textBoxDescription.ReadOnly = false;
                    this.buttonSaveQuery.Visible = true;

                    this.buttonSaveGroup.Visible = true;
                    this.textBoxGroup.Visible = true;
                    this.labelGroup.Visible = true;
                    break;
            }
        }

        #region Button events

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// switching through the states to save and edit queries and the groups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            switch (this._ProcessState)
            {
                case ProcessState.SetQuery:
                    if (this.textBoxDescription.Text.Length == 0
                        || this.textBoxQuery.Text.Length == 0)
                        System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Forms.FormQuerySaveText.Please_enter_the_name_and_descrition_of_the_query);//"Please enter the name and descrition of the query");
                    else
                    {
                        this._ProcessState = ProcessState.SetGroup;
                        this.buildQueryTree(this.XML);
                        this._Query.Description = this.textBoxDescription.Text;
                        this._Query.Title = this.textBoxQuery.Text.Replace(" ", "_");
                        this._Query.SQL = this._SQL;
                        this._Query.QueryTable = this._QueryTable;
                        this._Query.Optimized = DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing;
                        //this.setControls();
                    }
                    break;
                case ProcessState.SetGroup:
                    if (this.treeView.SelectedNode == null
                        && this.treeView.Nodes.Count == 1
                        && this.treeView.Nodes[0].Nodes.Count == 0)
                    {
                        System.Windows.Forms.TreeNode N = new TreeNode(this._Query.Title);
                        N.Tag = this._Query;
                        this.SetNodeFont(ref N);
                        this.treeView.Nodes[0].Nodes.Add(N);
                        this.treeView.Nodes[0].Expand();
                        this._ProcessState = ProcessState.Edit;
                    }
                    else if (this.treeView.SelectedNode != null
                        && this.treeView.SelectedNode.Tag == null)
                    {
                        System.Windows.Forms.TreeNode N = new TreeNode(this._Query.Title);
                        N.Tag = this._Query;
                        this.SetNodeFont(ref N);
                        this.treeView.SelectedNode.Nodes.Add(N);
                        this.treeView.SelectedNode.Expand();
                        this._ProcessState = ProcessState.Edit;
                        //this.buildQueryTree(this.XML);
                    }
                    else if (this.treeView.SelectedNode != null
                        && this.treeView.SelectedNode.Tag != null)
                    {
                        System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Forms.FormQuerySaveText.You_selected_a_query_Please_select_a_group);//"You selected a query. Please select a group");
                        this._ProcessState = ProcessState.SetGroup;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Forms.FormQuerySaveText.Please_select_a_group_or_create_a_new_one);//"Please select a group or create a new one");
                        this._ProcessState = ProcessState.SetGroup;
                    }
                    break;
                case ProcessState.Edit:
                    this.SaveData();
                    this.Close();
                    break;
            }
            this.setControls();
            this.setTitle();
        }

        private void toolStripButtonQueryGroupNew_Click(object sender, EventArgs e)
        {
            string Group = "";
            DiversityWorkbench.Forms.FormGetString f = new FormGetString("Name", DiversityWorkbench.Forms.FormQuerySaveText.Please_enter_the_name_of_the_new_group, "");//"Please enter the name of the new group", "");
            if (f.ShowDialog() == DialogResult.OK)
            {
                Group = f.String.Replace(" ", "_");
            }
            else return;
            if (this.treeView.Nodes.Count == 0)
            {
                this.treeView.Nodes.Add(FormQuerySave.QueryBaseNode);
                this.treeView.Nodes[0].Nodes.Add(Group);
            }
            else
            {
                System.Windows.Forms.TreeNode N = new TreeNode(Group);
                this.SetNodeFont(ref N);
                if (this.treeView.SelectedNode == null)
                {
                    this.treeView.Nodes[0].Nodes.Add(N);
                }
                else if (this.treeView.SelectedNode.Tag == null)
                {
                    this.treeView.SelectedNode.Nodes.Add(N);
                    this.treeView.SelectedNode.Expand();
                }
                else
                    System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Forms.FormQuerySaveText.You_can_not_add_a_group_to_the_selected_item);//"You can not add a group to the selected item");
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (this.treeView.SelectedNode != null)
            {
                string Message = DiversityWorkbench.Forms.FormQuerySaveText.Do_you_really_want_to_delete_the + " ";
                if (this.treeView.SelectedNode.Tag == null)
                    Message += DiversityWorkbench.Forms.FormQuerySaveText.group + " ";
                else
                    Message += DiversityWorkbench.Forms.FormQuerySaveText.query + " ";
                Message += "\r\n" + this.treeView.SelectedNode.Text;
                if (this.treeView.SelectedNode.Tag == null
                    && treeView.SelectedNode.Nodes.Count > 0)
                    Message += "\r\n" + DiversityWorkbench.Forms.FormQuerySaveText.and_all_depending_groups_and_queries;// and all depending groups and queries";
                Message += "?";
                if (System.Windows.Forms.MessageBox.Show(Message, DiversityWorkbench.Forms.FormQuerySaveText.Delete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.treeView.SelectedNode.Remove();
            }
        }

        private void buttonSaveQuery_Click(object sender, EventArgs e)
        {
            this._Query.Title = this.textBoxQuery.Text.Replace(" ", "_");
            this._Query.Description = this.textBoxDescription.Text;
            this.treeView.SelectedNode.Tag = this._Query;
            this.treeView.SelectedNode.Text = this._Query.Title;
        }

        private void buttonSaveGroup_Click(object sender, EventArgs e)
        {
            this.treeView.SelectedNode.Text = this.textBoxGroup.Text.Replace(" ", "_");
        }

        #endregion

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

        #region Query

        public Query Query
        {
            get { return _Query; }
            set
            {
                if (this._ProcessState == ProcessState.Edit)
                {
                    _Query = value;
                    this.textBoxDescription.Text = _Query.Description;
                    this.textBoxQuery.Text = _Query.Title;
                    this.labelQueryTable.Text = _Query.QueryTable;
                    this.textBoxSQL.Text = _Query.SQL;
                }
            }
        }

        private void QueryReset()
        {
            if (this._ProcessState == ProcessState.Edit)
            {
                Query Q = new Query();
                Q.SQL = "";
                Q.QueryTable = "";
                Q.Title = "";
                Q.Description = "";
                this.Query = Q;
            }
        }

        #endregion

        #region Data and XML

        private void SaveData()
        {
            this.XML = this.XmlFromTree();
        }

        private string XML
        {
            get
            {
                string SQL = "SELECT CASE WHEN [Queries] IS NULL THEN '' ELSE [" + FormQuerySave.QueryBaseNode + "] END AS Queries " +
                    "FROM UserProxy " +
                    "WHERE (LoginName = USER_NAME())";
                string xml = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, DiversityWorkbench.Settings.ConnectionString);
                return xml;
            }
            set
            {
                string XML = value.Replace("'", "''");// = value.Replace("'", "' + CHAR(39) + '");
                XML = XML.Replace("\"", "' + CHAR(34) + '");
                string SQL = "declare @Query nvarchar(max); " +
                    "set @Query = '" + XML + "'; " +
                    "declare @XML xml; " +
                    "set @XML = @Query; " +
                    "UPDATE UserProxy SET Queries = @XML " +
                    "FROM UserProxy " +
                    "WHERE (LoginName = USER_NAME())";
                //SQL = SQL.Replace("\"", "' + CHAR(34) + '");
                if (!FormFunctions.SqlExecuteNonQuery(SQL))
                    System.Windows.Forms.MessageBox.Show("Failed to save query");
            }
        }

        private void XmlFromTreeAddChild(System.Windows.Forms.TreeNode TreeNode, ref System.Xml.XmlWriter W)
        {
            string Node = TreeNode.Text;
            if (Node.IndexOf(":") > -1)
                Node = Node.Substring(0, Node.IndexOf(":"));
            if (TreeNode.Tag != null)
            {
                try
                {
                    W.WriteStartElement("Query");
                    Query Q = (Query)TreeNode.Tag;
                    W.WriteElementString("Title", Q.Title);
                    W.WriteElementString("Description", Q.Description);
                    W.WriteElementString("SQL", Q.SQL);
                    W.WriteElementString("QueryTable", Q.QueryTable);
                    W.WriteElementString("Optimized", Q.Optimized.ToString());

                    W.WriteEndElement();//Query
                }
                catch { }
            }
            else
            {
                W.WriteStartElement(Node);
                foreach (System.Windows.Forms.TreeNode NChild in TreeNode.Nodes)
                    this.XmlFromTreeAddChild(NChild, ref W);
                W.WriteEndElement();
            }
        }

        private string XmlFromTree()
        {
            System.IO.FileInfo XmlFile = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Query) + FormQuerySave.QueryBaseNode + ".xml");
            string XML = "";
            try
            {
                if (this.treeView.Nodes.Count == 0) return "";
                System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
                System.Xml.XmlWriter W;
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.Unicode;
                W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
                if (this.treeView.Nodes[0].Text == FormQuerySave.QueryBaseNode)
                {
                    W.WriteStartElement(FormQuerySave.QueryBaseNode);
                    foreach (System.Windows.Forms.TreeNode N in this.treeView.Nodes[0].Nodes)
                    {
                        this.XmlFromTreeAddChild(N, ref W);
                    }
                    W.WriteEndElement();
                }
                else
                {
                    W.WriteStartElement(FormQuerySave.QueryBaseNode);
                    foreach (System.Windows.Forms.TreeNode N in this.treeView.Nodes)
                        this.XmlFromTreeAddChild(N, ref W);
                    W.WriteEndElement();
                }
                W.Flush();
                W.Close();
                System.IO.StreamReader R = new System.IO.StreamReader(XmlFile.FullName);
                XML = R.ReadToEnd();
                R.Close();
            }
            catch (System.Exception ex)
            {
            }
            return XML;
        }

        #endregion

        #region XML-Tree

        private void buildQueryTree(string XML)
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
                    this.treeView.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name, 0, 0));
                    System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
                    tNode = this.treeView.Nodes[0];
                    this.SetNodeFont(ref tNode);
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
                    {
                        this.AddNode(xNode, inTreeNode);
                    }
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

        private System.Drawing.Font GroupFont
        {
            get
            {
                System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)9, System.Drawing.FontStyle.Bold);
                return F;
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

                //N.SelectedImageIndex = 1;
                //N.ImageIndex = 1;
                try
                {
                    DiversityWorkbench.Forms.Query Q = (DiversityWorkbench.Forms.Query)N.Tag;
                    if (Q.Optimized)
                    {
                        N.SelectedImageIndex = 2;
                        N.ImageIndex = 2;
                        N.BackColor = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        N.SelectedImageIndex = 1;
                        N.ImageIndex = 1;
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeView.SelectedNode.Tag != null)
            {
                try
                {
                    this.textBoxGroup.Visible = false;
                    this.labelGroup.Visible = false;
                    this.buttonSaveGroup.Visible = false;

                    this.textBoxQuery.Enabled = true;
                    this.textBoxDescription.Enabled = true;

                    if (this._ProcessState == ProcessState.Edit)
                    {
                        this.buttonSaveQuery.Enabled = true;
                    }
                    else
                    {
                        this.buttonSaveQuery.Enabled = false;
                    }

                    this.Query = (Query)this.treeView.SelectedNode.Tag;
                }
                catch { }
            }
            else
            {
                this.textBoxGroup.Visible = true;
                this.labelGroup.Visible = true;
                if (this._ProcessState == ProcessState.Edit)
                    this.buttonSaveGroup.Visible = true;
                else this.buttonSaveGroup.Visible = false;
                this.textBoxGroup.Text = this.treeView.SelectedNode.Text;

                this.textBoxDescription.Enabled = false;
                this.textBoxQuery.Enabled = false;
                this.buttonSaveQuery.Enabled = false;

                this.QueryReset();
            }
        }

        #endregion


    }
}
