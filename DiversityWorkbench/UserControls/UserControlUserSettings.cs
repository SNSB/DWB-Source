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
    /// <summary>
    /// Display the settings stored in table UserProxy in column Settings
    /// </summary>
    public partial class UserControlUserSettings : UserControl
    {

        #region Parameter

        private bool _HasBeenReset = false;

        #endregion

        #region Construction

        public UserControlUserSettings()
        {
            InitializeComponent();
            this.buildSettingsTree();
            this.treeViewSettings.Enabled = true;
        }

        #endregion

        #region Treeview

        public enum SettingType { ModuleSource, ModuleSourceTable, ModuleSourceColumn, ModuleSourceColumnValue }

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
                        ttNode.ImageIndex = 0;
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
        }

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
                    //if (Title.EndsWith("URI"))
                    //    Title = Title.Substring(0, Title.Length - 3);

                    System.Windows.Forms.TreeNode ttNode = new TreeNode(Title);
                    System.Collections.Generic.List<string> LL = new List<string>();
                    if (inXmlNode.ParentNode.NodeType == System.Xml.XmlNodeType.Element)
                        LL.Add(inXmlNode.ParentNode.Name);
                    LL.Add(inXmlNode.Name);
                    LL.Add(xNode.Name);
                    int iImage = (int)TreeImage.Null;
                    if (ttNode.Text == "ModuleSource")
                    {
                        iImage = (int)TreeImage.Module;
                        SettingType S = SettingType.ModuleSource;
                        ttNode.Tag = S;
                    }
                    else if (inXmlNode.Name == "ModuleSource")
                    {
                        iImage = (int)TreeImage.Table;
                        SettingType S = SettingType.ModuleSourceTable;
                        ttNode.Tag = S;

                    }
                    else if (inTreeNode.Tag != null && inTreeNode.Tag.GetType() == typeof(SettingType))
                    {
                        SettingType ST = (SettingType)inTreeNode.Tag;
                        switch (ST)
                        {
                            case SettingType.ModuleSourceTable:
                                SettingType C = SettingType.ModuleSourceColumn;
                                ttNode.Tag = C;
                                iImage = (int)TreeImage.Column;
                                break;
                            case SettingType.ModuleSourceColumn:
                                SettingType V = SettingType.ModuleSourceColumnValue;
                                ttNode.Tag = V;
                                iImage = (int)TreeImage.FixedSource;
                                break;
                        }
                    }
                    else
                    {
                    }
                    if (iImage > 0)
                    {
                        ttNode.ImageIndex = iImage;
                        ttNode.SelectedImageIndex = iImage;
                    }
                    inTreeNode.Nodes.Add(ttNode);
                    if (i >= inTreeNode.Nodes.Count)
                        continue;
                    tNode = inTreeNode.Nodes[i];
                    this.AddTreeNode(xNode, tNode);
                }
            }
            else
            {
                inTreeNode.ImageIndex = (int)TreeImage.FixedSource;
                inTreeNode.SelectedImageIndex = inTreeNode.ImageIndex;
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
                            if (inXmlNode.ParentNode.Attributes[iA].Name == "ProjectID" ||
                                inXmlNode.ParentNode.Attributes[iA].Name == "SectionID")
                                continue;
                            string DisplayText = inXmlNode.ParentNode.Attributes[iA].Name + ": " + inXmlNode.ParentNode.Attributes[iA].Value;
                            System.Windows.Forms.TreeNode aTreeNode = this.getTreeNode(DisplayText, inXmlNode, iA, inXmlNode.ParentNode.Attributes[iA].Name);// new TreeNode(DisplayText);
                            inTreeNode.Parent.Nodes.Add(aTreeNode);
                        }
                    }
                    inTreeNode.Remove();
                }
                else if (inXmlNode.Attributes.Count > 0)
                {
                    for (int iA = 0; iA < inXmlNode.Attributes.Count; iA++)
                    {
                        if (inXmlNode.Attributes[iA].Name == "ProjectID" || inXmlNode.Attributes[iA].Name == "SectionID")
                            continue;
                        string DisplayText = inXmlNode.Attributes[iA].Name + ": " + inXmlNode.Attributes[iA].Value;
                        System.Windows.Forms.TreeNode aTreeNode = this.getTreeNode(DisplayText, inXmlNode, iA, inXmlNode.Attributes[iA].Name);// new TreeNode(DisplayText);
                        inTreeNode.Nodes.Add(aTreeNode);
                    }
                }
            }
        }

        private System.Windows.Forms.TreeNode getTreeNode(string DisplayText, System.Xml.XmlNode inXmlNode, int iA, string Name)
        {
            System.Windows.Forms.TreeNode aTreeNode = new TreeNode(DisplayText);
            switch (Name)
            {
                case "Project":
                    aTreeNode.ImageIndex = (int)TreeImage.Project;
                    aTreeNode.SelectedImageIndex = aTreeNode.ImageIndex;
                    break;
                case "Section":
                    aTreeNode.ImageIndex = (int)TreeImage.Section;
                    aTreeNode.SelectedImageIndex = aTreeNode.ImageIndex;
                    break;
                case "Checklist":
                    aTreeNode.ImageIndex = (int)TreeImage.Checklist;
                    aTreeNode.SelectedImageIndex = aTreeNode.ImageIndex;
                    break;
                case "Database":
                    if (inXmlNode.Attributes[iA].Value.IndexOf("DiversityCollectionCache") > -1)
                    {
                        aTreeNode.ImageIndex = (int)TreeImage.CacheDB;
                        aTreeNode.SelectedImageIndex = aTreeNode.ImageIndex;
                    }
                    else
                    {
                        aTreeNode.ImageIndex = (int)TreeImage.Database;
                        aTreeNode.SelectedImageIndex = aTreeNode.ImageIndex;
                    }
                    break;
                case "Webservice":
                    aTreeNode.ImageIndex = (int)TreeImage.Webservice;
                    aTreeNode.SelectedImageIndex = aTreeNode.ImageIndex;
                    aTreeNode.ForeColor = System.Drawing.Color.Blue;
                    break;
                case "Module":
                    aTreeNode.ImageIndex = (int)TreeImage.Module;
                    aTreeNode.SelectedImageIndex = aTreeNode.ImageIndex;
                    break;
                case "":
                    break;
            }
            return aTreeNode;
        }

        private enum TreeImage { Setting, Module, Table, Column, Value, FixedSource, Database, CacheDB, Project, Webservice, Null, Checklist, Section }

        private void treeViewSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        #endregion

        #region Button events

        public void AllowReset(bool DoAllow)
        {
            this.buttonResetAll.Visible = DoAllow;
            this.labelUserSettingsReset.Visible = DoAllow;
        }

        private void buttonResetAll_Click(object sender, EventArgs e)
        {
            string SQL = "update u set [Settings] = null " +
                "FROM [dbo].[UserProxy] " +
                "U where U.LoginName = USER_NAME()";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            this.buildSettingsTree();
            this._HasBeenReset = true;
        }

        public bool HasBeenReset() { return this._HasBeenReset; }

        private void buttonSetNode_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Marking

        public void MarkSettings(System.Collections.Generic.List<string> SettingList)
        {
            foreach (System.Windows.Forms.TreeNode N in this.treeViewSettings.Nodes)
            {
                if (N.Text == SettingList[0])
                {
                    N.BackColor = System.Drawing.SystemColors.Info;
                    System.Collections.Generic.List<string> Settings = new List<string>();
                    for (int i = 1; i < SettingList.Count; i++)
                        Settings.Add(SettingList[i]);
                    this.MarkCurrentSetting(N, Settings);
                    return;
                }
                foreach (System.Windows.Forms.TreeNode Nchild in N.Nodes)
                {
                    if (Nchild.Text == SettingList[0])
                    {
                        Nchild.BackColor = System.Drawing.Color.Yellow;
                        System.Collections.Generic.List<string> Settings = new List<string>();
                        for (int i = 1; i < SettingList.Count; i++)
                            Settings.Add(SettingList[i]);
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
                    for (int i = 1; i < Settings.Count; i++)
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

        #endregion

    }
}
