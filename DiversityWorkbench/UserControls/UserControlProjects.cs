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
    public partial class UserControlProjects : UserControl
    {

        #region Parameter

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProject;
        private DiversityWorkbench.Hierarchy _Hierarchy;

        string SqlProjects = "SELECT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, " +
            "ProjectEditors, ProjectInstitution, ProjectNotes, ProjectCopyright, ProjectVersion, ProjectURL, " +
            "ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen " +
            "FROM Project WHERE ProjectID IN (SELECT ProjectID FROM ProjectList) ORDER BY Project";

        #endregion

        #region Construction

        public UserControlProjects()
        {
            InitializeComponent();
            try
            {
                try { DiversityWorkbench.Settings.ModuleName = "DiversityProjects"; }
                catch { }
                this.setDatabase();
                this.initForm();
                //this.initQuery();
                this._Hierarchy = new DiversityWorkbench.Hierarchy(this.treeViewProjects, this.dataSetProjects.Project, this.projectBindingSource, "ProjectID", "ProjectParentID", "Project", "Project");
                // online manual
                //this.helpProviderDiversityProjects.HelpNamespace = ...Windows.Forms.Application.StartupPath + "\\DiversityProjects.chm";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        #endregion

        #region Database

        private void setDatabase()
        {
            if (DiversityWorkbench.Settings.ConnectionString != "" && this.FormFunctions.DatabaseAccessible)
            {
                try
                {
                    this.resetSqlAdapters();
                    //this.projectApplicationTableAdapter.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                    //this.projectTableAdapter.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            this.FormFunctions.setTitle();
            //this.userControlQueryList.toolStripButtonConnection.ToolTipText = this.FormFunctions.ConnectionInfo();
            //this.userControlQueryList.setConnection(DiversityWorkbench.Settings.ConnectionString, "Project");
            this.splitContainerData.Visible = false;
        }

        private void resetSqlAdapters()
        {
            this._sqlDataAdapterProject = null;
        }


        private void setConnection(string[] args)
        {
            try
            {
                DiversityWorkbench.Settings.IsTrustedConnection = bool.Parse(args[1]);
                DiversityWorkbench.Settings.DatabaseServer = args[2];
                DiversityWorkbench.Settings.DatabaseName = args[3];
                if (!DiversityWorkbench.Settings.IsTrustedConnection)
                {
                    DiversityWorkbench.Settings.DatabaseUser = args[4];
                    DiversityWorkbench.Settings.Password = args[5];
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Form

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                //if (this._FormFunctions == null)
                //    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private void initForm()
        {
            try
            {
                //this.AcceptButton = this.userControlDialogPanel.buttonOK;
                //this.CancelButton = this.userControlDialogPanel.buttonCancel;
                this.FormFunctions.addEditOnDoubleClickToTextboxes();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Properties

        internal DiversityWorkbench.Project DiversityProject
        {
            get
            {
                DiversityWorkbench.Project P = new DiversityWorkbench.Project(this.ServerConnection);
                return P;
            }
        }

        public int ID { get { return this.ProjectID; } }

        public int ProjectID
        {
            get
            {
                int ID = -1;
                if (this.dataSetProjects.Project.Rows.Count > 0)
                {
                    if (this.treeViewProjects.SelectedNode != null)
                    {
                        if (this.treeViewProjects.SelectedNode.Tag != null)
                        {
                            System.Data.DataRow R = (System.Data.DataRow)this.treeViewProjects.SelectedNode.Tag;
                            ID = int.Parse(R["ProjectID"].ToString());
                            return ID;
                        }
                    }
                    ID = System.Int32.Parse(this.dataSetProjects.Project.Rows[0]["ProjectID"].ToString());
                }
                return ID;
            }
        }

        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get
            {
                DiversityWorkbench.ServerConnection S = DiversityWorkbench.Settings.ServerConnection;
                return S;
            }
        }

        #endregion

        #region Project

        private bool setProject(int ProjectID)
        {
            bool OK = true;
            try
            {
                if (this.dataSetProjects.Project.Rows.Count > 0)
                {
                    this.updateProject();
                }
                this.fillProject(ProjectID);
                if (this.dataSetProjects.Project.Rows.Count > 0)
                {
                    this.splitContainerData.Visible = true;
                    int i = 0;
                    foreach (System.Data.DataRow r in this.dataSetProjects.Project.Rows)
                    {
                        if (r[0].ToString() == ProjectID.ToString())
                            break;
                        i++;
                    }
                    //this.projectBindingSource.Position = i;
                }
                else
                {
                    this.splitContainerData.Visible = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private void fillProject(int ProjectID)
        {
            try
            {
                this.dataSetProjects.Clear();
                this.treeViewProjects.Nodes.Clear();
                this.textBoxSettings.Text = "";
                this.textBoxSetting.Text = "";
                string WhereClause = " WHERE ProjectID = " + ProjectID.ToString();

                //this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProject, DiversityProjects.Project.SqlProject + WhereClause, this.dataSetProjects.Project);
                //this.dataSetProjects.Project.Clear();
                //this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectApplication, DiversityProjects.Project.SqlProjectApplication + WhereClause, this.dataSetProjects.ProjectApplication);
                //this.dataSetProjects.ProjectApplication.Clear();
                //Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT " + DiversityProjects.Project.SqlProjectFields + " FROM ProjectHierarchy (" + ProjectID.ToString() + ")", DiversityWorkbench.Settings.ConnectionString);
                //a.Fill(this.dataSetProjects.Project);
                //this.projectApplicationTableAdapter.Fill(this.dataSetProjects.ProjectApplication);

                this._Hierarchy.buildHierarchy(ProjectID);


                System.Data.DataRow[] RR = this.dataSetProjects.Project.Select("ProjectID = " + ProjectID.ToString());
                if (RR.Length > 0)
                {
                    if (!RR[0]["ProjectSettings"].Equals(System.DBNull.Value))
                    {
                        string Settings = RR[0]["ProjectSettings"].ToString();
                        //this.buildSettingsTree(Settings);
                    }
                    else this.treeViewSettings.Nodes.Clear();
                }
                else this.treeViewSettings.Nodes.Clear();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the Project", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        private void updateProject()
        {
            try
            {
                //if (this.projectBindingSource.Current != null)
                //{
                //    System.Data.DataRowView RI = (System.Data.DataRowView)this.projectBindingSource.Current;
                //    RI.EndEdit();
                //}
                //this.projectBindingSource.EndEdit();
                //this.projectApplicationBindingSource.EndEdit();
            }
            catch { }
            this.FormFunctions.updateTable(this.dataSetProjects, "Project", this._sqlDataAdapterProject, this.BindingContext);
            //this.FormFunctions.updateTable(this.dataSetProjects, "ProjectApplication", this._sqlDataAdapterProjectApplication, this.projectApplicationBindingSource);
        }

        private void buttonURL_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURL.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxURL.Text = f.URL;
        }

        #endregion

        #region History

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            string Title = "History of " + this.dataSetProjects.Project.Rows[0]["Project"].ToString() + " (ProjectID: " + this.dataSetProjects.Project.Rows[0]["ProjectID"].ToString() + ")";
            try
            {
                //System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();
                //System.Data.DataTable dtProject_Log = new DataTable("Project");
                //string SqlCurrent = "SELECT " + DiversityProjects.Project.SqlProjectFields + ", 'current version' AS KindOfChange, " +
                //      "LogUpdatedWhen AS DateOfChange, case when LogUpdatedBy = 'dbo' then 'Administator' else LogUpdatedBy end AS ResponsibleUser " +
                //      "FROM Project " +
                //      "WHERE ProjectID = " + this.ID.ToString();
                //string SQL = "SELECT " + DiversityProjects.Project.SqlProjectFields + ", CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS KindOfChange, " +
                //      "LogDate AS DateOfChange, case when LogUser = 'dbo' then 'Administator' else LogUser end AS ResponsibleUser " +
                //      "FROM Project_log " +
                //      "WHERE ProjectID = " + this.ID.ToString() +
                //      " ORDER BY DateOfChange DESC";
                //try
                //{
                //    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                //    a.Fill(dtProject_Log);
                //    a.SelectCommand.CommandText = SQL;
                //    a.Fill(dtProject_Log);
                //    LogTables.Add(dtProject_Log);
                //}

                //catch (Exception ex)
                //{
                //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                //}
                //DiversityWorkbench.FormHistory f = new DiversityWorkbench.FormHistory(Title, LogTables, ...Windows.Forms.Application.StartupPath + "\\DiversityTaxonNames.chm");
                //f.ShowDialog();
            }

            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Hierarchy

        private void toolStripButtonProjectSetParent_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT NULL AS Project, NULL AS ProjectID UNION SELECT Project, ProjectID FROM Project WHERE ProjectID <> " + this.ID.ToString() + " ORDER BY Project"; ;
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Select a project from the list");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                int ParentID;
                if (int.TryParse(f.SelectedValue.ToString(), out ParentID))
                {
                    if (ParentID != this.ID)
                    {
                        //System.Data.DataRow R = this.dataSetDiversityProjects.Project.FindByProjectID(this.ID);
                        //R["ProjectParentID"] = ParentID;
                        //this.setProject(this.ID);
                    }
                }
                else if (f.SelectedValue.ToString() == "NULL" || f.SelectedValue.ToString() == "")
                {
                    //System.Data.DataRow R = this.dataSetDiversityProjects.Project.FindByProjectID(this.ID);
                    //R.BeginEdit();
                    //R["ProjectParentID"] = System.DBNull.Value;
                    //R.EndEdit();
                    //this.setProject(this.ID);
                }
            }
        }

        private void toolStripButtonProjectNew_Click(object sender, EventArgs e)
        {
            if (this.treeViewProjects.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return;
            }
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewProjects.SelectedNode.Tag;
                int ParentID = int.Parse(R["ProjectID"].ToString());
                string Parent = R["Project"].ToString();
                string Project = "";
                if (System.Windows.Forms.MessageBox.Show(
                    "Do you want to create a new collection Project as child of the project " + Parent,
                    "New Project?",
                    System.Windows.Forms.MessageBoxButtons.OKCancel,
                    System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    //int ID = this.InsertNewProject(ref Project, ParentID);
                    //if (ID > -1)
                    //{
                    //    this.setProject(ID);
                    //    if (Project.Length > 0)
                    //        this.userControlQueryList.AddListItem(ID, Project);
                    //    else
                    //        this.userControlQueryList.AddListItem(ID, "ID: " + ID.ToString());
                    //}
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonProjectDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewProjects.SelectedNode.Tag;
                string Project = R["Project"].ToString();
                if (System.Windows.Forms.MessageBox.Show(
                    "Do you want to delete the Project " + Project,
                    "Delete Project?",
                    System.Windows.Forms.MessageBoxButtons.OKCancel,
                    System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    R.Delete();
                    //this.userControlQueryList.RemoveListItem(ID);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void treeViewProjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.buildSettingsTree();
        }

        #endregion

        #region Settings

        private void buildSettingsTree()
        {
            try
            {
                //System.Data.DataRow[] RR = this.dataSetDiversityProjects.Project.Select("ProjectID = " + ProjectID.ToString());
                //if (RR.Length > 0)
                //{
                //    if (!RR[0]["ProjectSettings"].Equals(System.DBNull.Value))
                //    {
                //        string Settings = RR[0]["ProjectSettings"].ToString();
                //        this.buildSettingsTree(Settings);
                //    }
                //    else this.treeViewSettings.Nodes.Clear();
                //}
                //else this.treeViewSettings.Nodes.Clear();
            }
            catch { }
        }

        private void buildSettingsTree(string XML)
        {
            try
            {
                this.treeViewSettings.Nodes.Clear();
                System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(XML, System.Xml.XmlNodeType.Element, null);
                System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                dom.Load(tr);
                if (dom.DocumentElement == null) return;
                if (dom.DocumentElement.ChildNodes.Count >= 0)
                {
                    // SECTION 2. Initialize the TreeView control.
                    this.treeViewSettings.Nodes.Clear();
                    //if (dom.DocumentElement.Name.Length > 0)
                    this.treeViewSettings.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name));
                    System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
                    tNode = this.treeViewSettings.Nodes[0];
                    // SECTION 3. Populate the TreeView with the DOM nodes.
                    this.AddNode(dom.DocumentElement, tNode);
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
            this.SaveSettings();
        }

        private void treeViewSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeViewSettings.SelectedNode.Tag != null)
                this.textBoxSettings.Text = this.treeViewSettings.SelectedNode.Tag.ToString();
            else this.textBoxSettings.Text = "";
            string Node = this.treeViewSettings.SelectedNode.Text;
            if (Node.IndexOf(":") > -1)
                Node = Node.Substring(0, Node.IndexOf(":"));
            this.textBoxSetting.Text = Node;
        }

        private void buttonSettingsAdd_Click(object sender, EventArgs e)
        {
            if (this.treeViewSettings.Nodes.Count > 0
                && this.treeViewSettings.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the parent entry for the new setting");
                return;
            }
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New setting", "Please enter the name of the new setting", "");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                if (this.treeViewSettings.Nodes.Count == 0)
                    this.treeViewSettings.Nodes.Add(f.String);
                else
                {
                    if (this.treeViewSettings.SelectedNode == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Please select the parent entry for the new setting");
                        return;
                    }
                    else
                    {
                        this.treeViewSettings.SelectedNode.Nodes.Add(f.String);
                        this.treeViewSettings.SelectedNode.Expand();
                    }
                }
                this.SaveSettings();
            }
        }

        private void textBoxSettings_Leave(object sender, EventArgs e)
        {
            bool SettingsMustBeSaved = false;
            if ((this.treeViewSettings.SelectedNode.Tag == null && this.textBoxSettings.Text.Length > 0)
                || this.treeViewSettings.SelectedNode.Tag.ToString() != this.textBoxSettings.Text)
                SettingsMustBeSaved = true;
            this.treeViewSettings.SelectedNode.Tag = this.textBoxSettings.Text;
            if (this.treeViewSettings.SelectedNode.Text.IndexOf(":") > -1)
                this.treeViewSettings.SelectedNode.Text = this.treeViewSettings.SelectedNode.Text.Substring(0, this.treeViewSettings.SelectedNode.Text.IndexOf(":")) + ": " + this.textBoxSettings.Text;
            else
                this.treeViewSettings.SelectedNode.Text = this.treeViewSettings.SelectedNode.Text + ": " + this.textBoxSettings.Text;
            if (SettingsMustBeSaved)
                this.SaveSettings();
        }

        private void textBoxSetting_Leave(object sender, EventArgs e)
        {
            bool SettingsMustBeSaved = false;
            string Node = this.treeViewSettings.SelectedNode.Text;
            if (Node.IndexOf(":") > -1)
                Node = Node.Substring(0, Node.IndexOf(":"));
            if ((this.textBoxSetting.Text.Length > 0)
                || Node.ToString() != this.textBoxSetting.Text)
                SettingsMustBeSaved = true;
            this.treeViewSettings.SelectedNode.Text = this.textBoxSetting.Text;
            if (this.textBoxSettings.Text.Length > 0)
                this.treeViewSettings.SelectedNode.Text += ": " + this.textBoxSettings.Text;
            if (SettingsMustBeSaved)
                this.SaveSettings();
        }

        private string XmlFromTree()
        {
            System.IO.FileInfo XmlFile = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Settings) + "ProjectSettings.xml");

            string XML = "";
            if (this.treeViewSettings.Nodes.Count == 0) return "";
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
            W.WriteStartElement(this.treeViewSettings.Nodes[0].Text);
            foreach (System.Windows.Forms.TreeNode N in this.treeViewSettings.Nodes[0].Nodes)
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
            string Node = TreeNode.Text;
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

        private void buttonSettingsRemove_Click(object sender, EventArgs e)
        {
            if (this.treeViewSettings.SelectedNode != null)
            {
                this.treeViewSettings.SelectedNode.Remove();
                this.SaveSettings();
            }
            else
                System.Windows.Forms.MessageBox.Show("Please select the setting you want to remove");
        }

        private void treeViewSettings_Leave(object sender, EventArgs e)
        {
            //this.SaveSettings();
        }

        private void buttonSettingsSave_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
        }

        private void SaveSettings()
        {
            // Save settings
            //System.Data.DataRowView RV = (System.Data.DataRowView)this.projectBindingSource.Current;
            //if (RV["ProjectID"].ToString() == this.ProjectID.ToString())
            //{
            //    string XML = this.XmlFromTree();
            //    RV["ProjectSettings"] = this.XmlFromTree();
            //}
        }

        private void buttonSettingsCopyFromParent_Click(object sender, EventArgs e)
        {
            //System.Data.DataRowView RV = (System.Data.DataRowView)this.projectBindingSource.Current;
            //if (RV["ProjectParentID"].Equals(System.DBNull.Value))
            //{
            //    System.Windows.Forms.MessageBox.Show("No parent found");
            //    return;
            //}
            //System.Data.DataRow[] RR = this.dataSetDiversityProjects.Project.Select("ProjectID = " + RV["ProjectParentID"].ToString());
            //if (RR.Length > 0)
            //{
            //    if (RR[0]["ProjectSettings"].Equals(System.DBNull.Value)
            //        ||
            //        RR[0]["ProjectSettings"].ToString().Length == 0)
            //        System.Windows.Forms.MessageBox.Show("Parent contains no settings");
            //    else
            //    {
            //        RV["ProjectSettings"] = RR[0]["ProjectSettings"].ToString();
            //        this.buildSettingsTree();
            //    }
            //}
            //else
            //    System.Windows.Forms.MessageBox.Show("No parent found");
        }

        #endregion

    }
}
