using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormProjects : Form
    {
        #region Parameter
        
        private int _ProjectID;
        private System.Data.DataTable _dtProjects;
        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private string _ImageDescriptionTemplate;
        
        #endregion

        #region Construction
        
        public FormProjects()
        {
            InitializeComponent();
            this.initForm();
        }
        
        #endregion

        #region Form
        
        private void initForm()
        {
            this._dtProjects = new DataTable();
            this.initProjectList(null);
            this.listBoxProjects.DataSource = this._dtProjects;
            this.listBoxProjects.DisplayMember = "Project";
            this.listBoxProjects.ValueMember = "ProjectID";

            bool OK;
            string TableName = "ProjectProxy";
            string SQL = "SELECT dbo.ReplicationPublisherConnection()";
            string ReplicationPublisherConnection = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (ReplicationPublisherConnection.Length > 0)
            {
                this.toolStripButtonNewProject.Enabled = false;
                //this.toolTip.SetToolTip(this.toolStripButtonNewProject, "New local projects can only be added in the publisher database");
            }
            else
            {
                OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                this.toolStripButtonNewProject.Enabled = OK;
            }

            OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
            if (this._ProjectID > 0)
                OK = false;
            this.toolStripButtonRemoveProject.Enabled = OK;
            if (OK) this.toolStripButtonRemoveProject.Tag = "Can delete";

            OK = this.FormFunctions.getObjectPermissions(TableName, "ImageDescriptionTemplate", "UPDATE");
            this.buttonEditImageDescriptionTemplate.Enabled = OK;

            this.setInterfaceForSettingTheProjectDatabase();
        }

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private void setInterfaceForSettingTheProjectDatabase()
        {
            // Check if the scalar function is available and returns the link to the DiversityProjects Database
            // Then show or hide the controls for setting the connection, depending on the user role
            string SQL = "";
        }
        
        #endregion

        #region Tree and Template
        
        private void buildTemplateTree(string XML)
        {
            try
            {
                this.treeViewImageDescriptionTemplate.Nodes.Clear();
                System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(XML, System.Xml.XmlNodeType.Element, null);
                System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                dom.Load(tr);
                if (dom.DocumentElement == null) return;
                if (dom.DocumentElement.ChildNodes.Count >= 0)
                {
                    // SECTION 2. Initialize the TreeView control.
                    this.treeViewImageDescriptionTemplate.Nodes.Clear();
                    //if (dom.DocumentElement.Name.Length > 0)
                    this.treeViewImageDescriptionTemplate.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name));
                    System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
                    tNode = this.treeViewImageDescriptionTemplate.Nodes[0];
                    // SECTION 3. Populate the TreeView with the DOM nodes.
                    this.AddNode(dom.DocumentElement, tNode);
                    this.treeViewImageDescriptionTemplate.ExpandAll();
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
        
        private void buttonEditImageDescriptionTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                string Template = this._ImageDescriptionTemplate;
                if (Template.Length == 0)
                    Template = "<Description></Description>";
                string Header = "Please edit the image description template for the current project";
                System.Data.DataTable dtTemplate = new DataTable();
                string SQL = "SELECT P.Project, CAST(P.ImageDescriptionTemplate as nvarchar(MAX)) AS ImageDescriptionTemplate " +
                    "FROM         ProjectProxy AS P INNER JOIN " +
                    "ProjectList AS L ON P.ProjectID = L.ProjectID " +
                    "WHERE     (NOT (P.ImageDescriptionTemplate IS NULL)) " +
                    "ORDER BY P.Project";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtTemplate);
                DiversityWorkbench.Forms.FormEditXmlTemplate f = new DiversityWorkbench.Forms.FormEditXmlTemplate(Template, Header, dtTemplate, "Project", "ImageDescriptionTemplate");
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    Template = f.TemplateWithoutEncoding;
                    SQL = "UPDATE P SET ImageDescriptionTemplate = '" + Template + "' FROM ProjectProxy P WHERE P.ProjectID = " + this._ProjectID.ToString();
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    C.ExecuteNonQuery();
                    con.Close();
                    this.initProjectList(this._ProjectID);
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Projects
        
        private void initProjectList(int? ProjectID)
        {
            this._dtProjects.Clear();
            string SQL = "SELECT Project, ProjectID " +
                "FROM ProjectList " +
                "ORDER BY Project";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this._dtProjects);
            if (ProjectID != null)
            {
                int i = 0;
                foreach (System.Data.DataRow R in this._dtProjects.Rows)
                {
                    if (R[1].ToString() == ProjectID.ToString())
                        break;
                    i++;
                }
                this.listBoxProjects.SelectedIndex = i;
            }
        }

        private void listBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
                this._ProjectID = int.Parse(R[1].ToString());
                this.labelProject.Text = R[0].ToString();
                if (this._ProjectID < 0)
                {
                    this.toolTip.SetToolTip(this.pictureBoxProject, "Local project");
                    this.pictureBoxProject.Image = this.imageListProject.Images[1];
                }
                else
                {
                    this.toolTip.SetToolTip(this.pictureBoxProject, "Global project");
                    this.pictureBoxProject.Image = this.imageListProject.Images[0];
                }
                string SQL = "SELECT COUNT(*) AS Number FROM CollectionProject WHERE ProjectID = " + this._ProjectID.ToString();
                this.textBoxSpecimen.Text = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                SQL = "SELECT CASE WHEN ImageDescriptionTemplate IS NULL THEN '' ELSE ImageDescriptionTemplate END FROM ProjectProxy WHERE ProjectID = " + this._ProjectID.ToString();
                this._ImageDescriptionTemplate = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                this.buildTemplateTree(this._ImageDescriptionTemplate);
                if (this.textBoxSpecimen.Text == "0" &&
                    this.toolStripButtonRemoveProject.Tag != null &&
                    toolStripButtonRemoveProject.Tag.ToString() == "Can delete")
                    this.toolStripButtonRemoveProject.Enabled = true;
                else this.toolStripButtonRemoveProject.Enabled = false;

            }
            catch { }
        }

        private void toolStripButtonNewProject_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Create project", "Create a new local project", "");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                try
                {
                    string Project = f.String.Trim();
                    string SQL = "SELECT COUNT(*) FROM ProjectProxy WHERE Project = '" + Project + "' OR Project = ' " + Project + "'";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL) == "0")
                    {
                        string NewProjectID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT MIN(ProjectID) - 1 FROM ProjectList");
                        if (int.Parse(NewProjectID) >= 0)
                            NewProjectID = "-1";
                        SQL = "INSERT INTO ProjectProxy " +
                            "(ProjectID, Project) " +
                            "VALUES (" + NewProjectID + ", ' " + Project + "')";
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        SQL = "INSERT INTO ProjectUser " +
                            "(LoginName, ProjectID) " +
                            "VALUES (USER_NAME(), " + NewProjectID.ToString() + ")";
                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                        {
                            int ProjectID = int.Parse(NewProjectID);
                            this.initProjectList(ProjectID);
                        }
                        else System.Windows.Forms.MessageBox.Show("Creation of project failed");
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("A project with the name " + Project + " allready exists or the name contains illegal signs");
                        return;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void toolStripButtonRemoveProject_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Availble in upcoming version");
        }
        
        #endregion


        #region Connetion to DiversityProjects

        /// <summary>
        /// this function defines the connection to the DiversityProjects database. 
        /// this can only be done by the dbo or a user with proper rights
        /// only one DiversityProjects database can be defined or one database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSetServerConnection_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase(DiversityWorkbench.Settings.ServerConnection);
                //DiversityWorkbench.FormDatabaseConnection f = new DiversityWorkbench.FormDatabaseConnection(DiversityWorkbench.Settings.ServerConnection);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK &&
                    f.ServerConnection.DatabaseName != DiversityWorkbench.Settings.DatabaseName &&
                    f.ServerConnection.ConnectionIsValid &&
                    f.ServerConnection.ConnectionString.Length > 0)
                {
                    string SQL = "CREATE FUNCTION [dbo].[DiversityProjectsConnection] ()  " +
                        "RETURNS  varchar (255) AS  " +
                        "BEGIN " +
                        "declare @URL varchar(255) " +
                        "set @URL =  'Data Source=" + f.ServerConnection.DatabaseServer +
                        "," + f.ServerConnection.DatabaseServerPort.ToString() +
                        ";Initial Catalog=" + f.ServerConnection.DatabaseName + "' " +
                        "return @URL " +
                        "END ";
                    string Grant = "GRANT EXECUTE ON [dbo].[DiversityProjectsConnection] TO [USER]";
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.setInterfaceForSettingTheProjectDatabase();
        }
        
        #endregion

    }
}
