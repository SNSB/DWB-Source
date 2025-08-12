using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class FormFixedSources : Form
    {
        #region Parameter

        private Sheet _Sheet;
        private System.Collections.Generic.List<string> _Settings;
        #endregion

        #region Construction

        public FormFixedSources(Sheet Sheet, System.Collections.Generic.List<string> Settings)
        {
            InitializeComponent();
            this._Sheet = Sheet;
            this._Settings = Settings;
            this.initSettings();
        }
        
        #endregion

        #region Settings

        private void initSettings()
        {
            try
            {
                this.buildSettingsTree();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buildSettingsTree()
        {
            try
            {
                this.treeViewSettings.Nodes.Clear();
                string SQL = "SELECT cast([Settings] as nvarchar(MAX)) FROM [dbo].[UserProxy] U where u.LoginName = USER_NAME();";
                string XML = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (XML.Length > 0)
                {
                    System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(XML, System.Xml.XmlNodeType.Element, null);
                    System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                    dom.Load(tr);
                    if (dom.DocumentElement == null) return;
                    if (dom.DocumentElement.ChildNodes.Count >= 0)
                    {
                        // SECTION 2. Initialize the TreeView control.
                        this.treeViewSettings.Nodes.Clear();
                        //if (dom.DocumentElement.Name.Length > 0)
                        System.Windows.Forms.TreeNode ttNode = new System.Windows.Forms.TreeNode(dom.DocumentElement.Name);
                        System.Collections.Generic.List<string> LL = new List<string>();
                        LL.Add(dom.DocumentElement.Name);
                        ttNode.ImageIndex = (int)TreeImage.Setting;// 0;
                        this.treeViewSettings.Nodes.Add(ttNode);
                        //this.treeViewSettings.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name));
                        System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
                        tNode = this.treeViewSettings.Nodes[0];
                        // SECTION 3. Populate the TreeView with the DOM nodes.
                        this.AddTreeNode(dom.DocumentElement, tNode);
                        //this.AddNode(dom.DocumentElement, tNode);
                        this.treeViewSettings.ExpandAll();
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
            }
            catch (System.Exception ex) { }
            this.MarkCurrentSetting();
        }

        private enum TreeImage { Setting, Module, Table, Row, FixedSource, Database, Project, Webservice }

        private void AddTreeNode(System.Xml.XmlNode inXmlNode, System.Windows.Forms.TreeNode inTreeNode)
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
                    string Title = xNode.Name;

                    System.Windows.Forms.TreeNode ttNode = new TreeNode(Title);
                    System.Collections.Generic.List<string> LL = new List<string>();
                    if (inXmlNode.ParentNode.NodeType == System.Xml.XmlNodeType.Element)
                        LL.Add(inXmlNode.ParentNode.Name);
                    LL.Add(inXmlNode.Name);
                    LL.Add(xNode.Name);
                    this.setImage(ttNode, inTreeNode);

                    inTreeNode.Nodes.Add(ttNode);
                    if (i >= inTreeNode.Nodes.Count)
                        continue;
                    tNode = inTreeNode.Nodes[i];
                    this.AddTreeNode(xNode, tNode);
                }
            }
            else
            {
                if (inXmlNode.Value != null)
                {
                    DiversityWorkbench.Forms.XmlNode N = new DiversityWorkbench.Forms.XmlNode();
                    if (inXmlNode.Name == "#text")
                        N.Name = inXmlNode.ParentNode.Name;
                    else
                        N.Name = inXmlNode.Name;
                    N.Value = inXmlNode.Value;
                    inTreeNode.Parent.Tag = N;
                    inTreeNode.Parent.Text = inTreeNode.Parent.Text;
                    if (inXmlNode.Value != "Database")
                        inTreeNode.Parent.Text += ": " + inXmlNode.Value;
                    if (inXmlNode.ParentNode.Attributes != null && inXmlNode.ParentNode.Attributes.Count > 0)
                    {
                        for (int iA = 0; iA < inXmlNode.ParentNode.Attributes.Count; iA++)
                        {
                            if (inXmlNode.ParentNode.Attributes[iA].Name == "ProjectID")
                                continue;
                            string DisplayText = inXmlNode.ParentNode.Attributes[iA].Name + ": " + inXmlNode.ParentNode.Attributes[iA].Value;
                            System.Windows.Forms.TreeNode aTreeNode = new TreeNode(DisplayText);
                            if (inXmlNode.ParentNode.Attributes[iA].Name == "Project")
                            {
                                System.Collections.Generic.List<string> PP = new List<string>();
                                PP.Add("CollectionProject");
                                aTreeNode.ImageIndex = (int)TreeImage.Project;// 3;// Specimen.ImageIndex(PP, false);
                                aTreeNode.SelectedImageIndex = 3;//Specimen.ImageIndex(PP, false);
                            }
                            else if (inXmlNode.ParentNode.Attributes[iA].Name == "Database")
                            {
                                System.Collections.Generic.List<string> PP = new List<string>();
                                PP.Add("Database");
                                aTreeNode.ImageIndex = (int)TreeImage.Database;// 2;//Specimen.ImageIndex(PP, false);
                                aTreeNode.SelectedImageIndex = (int)TreeImage.Database;//2;//Specimen.ImageIndex(PP, false);
                            }
                            else if (inXmlNode.ParentNode.Attributes[iA].Name == "Webservice")
                            {
                                System.Collections.Generic.List<string> PP = new List<string>();
                                PP.Add("Webservice");
                                aTreeNode.ImageIndex = (int)TreeImage.Webservice;//4;//Specimen.ImageIndex(PP, false);
                                aTreeNode.SelectedImageIndex = (int)TreeImage.Webservice;//4;//Specimen.ImageIndex(PP, false);
                                aTreeNode.ForeColor = System.Drawing.Color.Blue;
                            }
                            inTreeNode.Parent.Nodes.Add(aTreeNode);
                        }
                    }
                    inTreeNode.Remove();
                }
                else if (inXmlNode.Attributes.Count > 0)
                {
                    for (int iA = 0; iA < inXmlNode.Attributes.Count; iA++)
                    {
                        if (inXmlNode.Attributes[iA].Name == "ProjectID")
                            continue;
                        string DisplayText = inXmlNode.Attributes[iA].Name + ": " + inXmlNode.Attributes[iA].Value;
                        System.Windows.Forms.TreeNode aTreeNode = new TreeNode(DisplayText);
                        if (inXmlNode.Attributes[iA].Name == "Project")
                        {
                            System.Collections.Generic.List<string> PP = new List<string>();
                            PP.Add("CollectionProject");
                            aTreeNode.ImageIndex = (int)TreeImage.Project;//3;// Specimen.ImageIndex(PP, false);
                            aTreeNode.SelectedImageIndex = (int)TreeImage.Project;//3;// Specimen.ImageIndex(PP, false);
                        }
                        else if (inXmlNode.Attributes[iA].Name == "Database")
                        {
                            System.Collections.Generic.List<string> PP = new List<string>();
                            PP.Add("Database");
                            aTreeNode.ImageIndex = (int)TreeImage.Database;//2;// Specimen.ImageIndex(PP, false);
                            aTreeNode.SelectedImageIndex = (int)TreeImage.Database;//2;// Specimen.ImageIndex(PP, false);
                        }
                        else if (inXmlNode.Attributes[iA].Name == "Webservice")
                        {
                            System.Collections.Generic.List<string> PP = new List<string>();
                            PP.Add("Webservice");
                            aTreeNode.ImageIndex = (int)TreeImage.Webservice;//4;// Specimen.ImageIndex(PP, false);
                            aTreeNode.SelectedImageIndex = (int)TreeImage.Webservice;//4;// Specimen.ImageIndex(PP, false);
                            aTreeNode.ForeColor = System.Drawing.Color.Blue;
                        }
                        inTreeNode.Nodes.Add(aTreeNode);
                    }
                }
            }
        }

        private void setImage(System.Windows.Forms.TreeNode Node, System.Windows.Forms.TreeNode ParentNode)
        {
            if (Node.Text.ToLower() == "modulesource")
            {
                Node.ImageIndex = (int)TreeImage.Module;
                Node.SelectedImageIndex = (int)TreeImage.Module;
            }
            else
            {
                string SQL = "select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = '" + Node.Text + "'";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result == "1")
                {
                    Node.ImageIndex = (int)TreeImage.Table;
                    Node.SelectedImageIndex = (int)TreeImage.Table;
                }
                else
                {
                    SQL = "select count(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = '" + ParentNode.Text + "' AND T.COLUMN_NAME = '" + Node.Text + "'";
                    Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result == "1")
                    {
                        Node.ImageIndex = (int)TreeImage.FixedSource;
                        Node.SelectedImageIndex = (int)TreeImage.FixedSource;
                    }
                    else
                    {
                        if (ParentNode.ImageIndex == (int)TreeImage.Table)
                        {
                            Node.ImageIndex = (int)TreeImage.Row;
                            Node.SelectedImageIndex = (int)TreeImage.Row;
                        }
                        else
                        {
                            Node.ImageIndex = (int)TreeImage.FixedSource;
                            Node.SelectedImageIndex = (int)TreeImage.FixedSource;
                        }
                    }
                }
            }
        }

        private void MarkCurrentSetting()
        {
            foreach (System.Windows.Forms.TreeNode N in this.treeViewSettings.Nodes)
            {
                if (N.Text == this._Settings[0])
                {
                    N.BackColor = System.Drawing.SystemColors.Info;
                    System.Collections.Generic.List<string> Settings = new List<string>();
                    for (int i = 1; i < this._Settings.Count; i++ )
                        Settings.Add(this._Settings[i]);
                    this.MarkCurrentSetting(N, Settings);
                    return;
                }
                foreach (System.Windows.Forms.TreeNode Nchild in N.Nodes)
                {
                    if (Nchild.Text == this._Settings[0])
                    {
                        Nchild.BackColor = System.Drawing.Color.Yellow;
                        System.Collections.Generic.List<string> Settings = new List<string>();
                        for (int i = 1; i < this._Settings.Count; i++)
                            Settings.Add(this._Settings[i]);
                        this.MarkCurrentSetting(Nchild, Settings);
                        return;
                    }
                }
            }
        }

        private void MarkCurrentSetting(System.Windows.Forms.TreeNode Node, System.Collections.Generic.List<string> Settings)
        {
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                if (N.Text == Settings[0])
                {
                    N.BackColor = System.Drawing.Color.Yellow;
                    System.Collections.Generic.List<string> NextSettings = new List<string>();
                    for (int i = 1; i < Settings.Count; i++ )
                        NextSettings.Add(Settings[i]);
                    if (NextSettings.Count > 0)
                        this.MarkCurrentSetting(N, NextSettings);
                    else
                    {
                        foreach (System.Windows.Forms.TreeNode Nchild in N.Nodes)
                            Nchild.BackColor = System.Drawing.Color.Yellow;
                    }
                }
            }
        }

        //private void treeViewSettings_AfterSelect(object sender, TreeViewEventArgs e)
        //{

        //}

        //private void buttonUserSettingsReset_Click(object sender, EventArgs e)
        //{
        //    string SQL = "update u set [Settings] = null " +
        //        "FROM [dbo].[UserProxy] " +
        //        "U where U.LoginName = USER_NAME()";
        //    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
        //    this.buildSettingsTree();
        //}

        #endregion


    }
}
