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
    public partial class FormLoginAdminGetProjects : Form
    {

        #region Parameter and properties

        private System.Data.DataSet _dsProjects;
        private System.Data.DataTable _dtProjects;
        private System.Windows.Forms.BindingSource _projectBindingSource;
        private DiversityWorkbench.ServerConnection _ServerConnection;
        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> _ServerConnections;
        private bool _IncludeLinkedServer = false;

        private DiversityWorkbench.Hierarchy _Hierarchy;

        private System.Data.DataTable _dtProjectProxy;
        private DiversityWorkbench.ServerConnection _CallingServerConnection;
        private System.Collections.Generic.List<System.Data.DataRow> _AddedProjectRows;

        public System.Collections.Generic.List<System.Data.DataRow> AddedProjectRows
        {
            get
            {
                if (this._AddedProjectRows == null)
                    this._AddedProjectRows = new List<DataRow>();
                return _AddedProjectRows;
            }
            set { _AddedProjectRows = value; }
        }

        private System.Collections.Generic.List<System.Data.DataRow> _DifferingProjectRows;

        public System.Collections.Generic.List<System.Data.DataRow> DifferingProjectRows
        {
            get
            {
                if (this._DifferingProjectRows == null)
                    this._DifferingProjectRows = new List<DataRow>();
                return _DifferingProjectRows;
            }
            //set { _DifferingProjectRows = value; }
        }

        public System.Data.DataTable DtProjects
        {
            get
            {
                if (this._dtProjects == null)
                {
                    this._dtProjects = new DataTable();
                    if (this._ServerConnection != null && this._ServerConnection.ConnectionString.Length > 0)
                    {
                        string SQL = "SELECT Project + CASE WHEN [ProjectTitle] IS NULL OR ProjectTitle = '' THEN '' ELSE ': ' + ProjectTitle END AS ProjectTitle, ProjectID, Project, ProjectType, ProjectParentID, dbo.BaseURL() + cast(ProjectID as varchar) AS ProjectURI " +
                            "FROM Project_Core ORDER BY Project";
                        if (this._ServerConnection.LinkedServer != null && this._ServerConnection.LinkedServer.Length > 0)
                        {
                            string Prefix = "[" + this._ServerConnection.LinkedServer + "].[" + this._ServerConnection.DatabaseName + "].[dbo].";
                            SQL = "SELECT Project + CASE WHEN [ProjectTitle] IS NULL OR ProjectTitle = '' THEN '' ELSE ': ' + ProjectTitle END AS ProjectTitle, " +
                                "ProjectID, Project, ProjectType, ProjectParentID, B.BaseURL + cast(ProjectID as varchar) AS ProjectURI " +
                                "FROM " + Prefix + "Project_Core P, " + Prefix + "[ViewBaseURL] " +
                                "B ORDER BY Project";
                        }
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                        ad.Fill(this._dtProjects);
                    }
                    if (this._dtProjects.Rows.Count == 0)
                        System.Windows.Forms.MessageBox.Show("You have no access to any project");
                }
                return _dtProjects;
            }
            set { _dtProjects = value; }
        }

        #endregion

        #region Construction

        public FormLoginAdminGetProjects(System.Data.DataTable DtProjectProxy, DiversityWorkbench.ServerConnection ServerConnection, string HelpNamespace,
            bool IncludeLinkedServer = false)
        {
            InitializeComponent();
            this._dtProjectProxy = DtProjectProxy;
            this._CallingServerConnection = ServerConnection;
            this.labelProjectsMissing.Text = "Projects missing in database " + this._CallingServerConnection.DatabaseName;
            this.helpProvider.HelpNamespace = HelpNamespace;
            if (IncludeLinkedServer)
                this._IncludeLinkedServer = IncludeLinkedServer;
            this.initDatabaseList();
            this.initProjectProxy();
        }

        public FormLoginAdminGetProjects(System.Data.DataTable DtProjectProxy, DiversityWorkbench.ServerConnection ServerConnection)
        {
            InitializeComponent();
            this._dtProjectProxy = DtProjectProxy;
            this._CallingServerConnection = ServerConnection;
            this.labelProjectsMissing.Text = "Projects missing in database " + this._CallingServerConnection.DatabaseName;
            this.initDatabaseList();
        }

        public FormLoginAdminGetProjects(System.Data.DataTable DtProjectProxy, DiversityWorkbench.ServerConnection ServerConnection, string ProjectsDatabase, string HelpNamespace)
            : this(DtProjectProxy, ServerConnection, HelpNamespace)
        {
            int s = -1;
            System.Collections.Generic.List<object> ItemsToDelete = new List<object>();
            for (int i = 0; i < this.comboBoxDatabase.Items.Count; i++)
            {
                if (this.comboBoxDatabase.Items[i].ToString() == ProjectsDatabase)
                {
                    s = i;
                    //break;
                }
                else
                    ItemsToDelete.Add(this.comboBoxDatabase.Items[i]);
            }
            if (s > -1 && ProjectsDatabase != null
                && ItemsToDelete.Count == this.comboBoxDatabase.Items.Count - 1)
            {
                this.comboBoxDatabase.Items.Clear();
                this.comboBoxDatabase.Items.Add(ProjectsDatabase);
                this.comboBoxDatabase.SelectedIndex = 0;
            }
            else
            {
                s = 0;
                this.comboBoxDatabase.SelectedIndex = s;
            }
            this.comboBoxDatabase_SelectionChangeCommitted(null, null);
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

        #region Database

        private void initDatabaseList()
        {
            if (this.ServerConnections.Count == 0)
            {
                this._ServerConnections = null;
                DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                P.DatabaseList();
                P.ServerConnections();
                this.ServerConnections = P.ServerConnections();
                //DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryProject = new UserControlModuleRelatedEntry();
                //userControlModuleRelatedEntryProject.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)P;
            }
            if (this.ServerConnections.Count == 0)
                System.Windows.Forms.MessageBox.Show("You have no access to any project database");
            this.comboBoxDatabase.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in this.ServerConnections)
            {
                // Markus 23.6.2020: Do not include databases located on linked servers
                if (!this._IncludeLinkedServer && KV.Value.LinkedServer != null && KV.Value.LinkedServer.Length > 0)
                    continue;
                this.comboBoxDatabase.Items.Add(KV.Key);
            }
        }

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> ServerConnections
        {
            get
            {
                if (this._ServerConnections == null)
                {
                    this._ServerConnections = new Dictionary<string, ServerConnection>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in KV.Value.ServerConnectionList())
                        {
                            if (KV.Value.ServerConnection.ModuleName == "DiversityProjects")
                            {
                                if (!_ServerConnections.ContainsKey(KVconn.Value.DisplayText))
                                    this._ServerConnections.Add(KVconn.Value.DisplayText, KVconn.Value);
                                else if (!_ServerConnections.ContainsKey(KVconn.Value.DatabaseName))
                                    this._ServerConnections.Add(KVconn.Value.DatabaseName, KVconn.Value);
                            }
                        }
                    }
                }
                return _ServerConnections;
            }
            set { _ServerConnections = value; }
        }


        private void comboBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this._ServerConnection = this._ServerConnections[this.comboBoxDatabase.Text];
        }

        private void comboBoxDatabase_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                this._ServerConnection = this._ServerConnections[this.comboBoxDatabase.SelectedItem.ToString()];
                this._dtProjects = null;
                this.buildProjectTree();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Tree

        private void buildProjectTree()
        {
            try
            {
                this.treeViewProjects.Nodes.Clear();
                this._Hierarchy = new DiversityWorkbench.Hierarchy(this.treeViewProjects, this.DtProjects, this._projectBindingSource, "ProjectID", "ProjectParentID", "Project", "Project");
                if (this.textBoxFilter.Text.Length > 0)
                    this._Hierarchy.buildHierarchy(this.textBoxFilter.Text);
                else
                    this._Hierarchy.buildHierarchy();
                foreach (System.Windows.Forms.TreeNode N in this.treeViewProjects.Nodes)
                {
                    this.MarkTreeNodes(N);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void MarkTreeNodes(System.Windows.Forms.TreeNode N)
        {
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                System.Data.DataRow[] rr = this._dtProjectProxy.Select("ProjectURI = '" + R["ProjectURI"] + "'");
                if (rr.Length > 0)
                {
                    N.ForeColor = System.Drawing.Color.Green;
                    N.Checked = true;
                    if (rr[0]["Project"].ToString() != R["Project"].ToString())
                    {
                        N.BackColor = System.Drawing.Color.Yellow;
                        N.Text += " [<> " + rr[0]["Project"].ToString() + "]";
                        this.DifferingProjectRows.Add(R);
                    }
                }
                else
                {
                    N.ForeColor = System.Drawing.Color.Red;
                    N.Checked = false;
                }
                foreach (System.Windows.Forms.TreeNode Node in N.Nodes)
                    this.MarkTreeNodes(Node);
                this.CheckProjectType(N);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void treeViewProjects_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Action != TreeViewAction.Unknown)
                {
                    System.Data.DataRow R = (System.Data.DataRow)e.Node.Tag;
                    if (ProjectTypeIsSet)
                    {
                        string Type = R["ProjectType"].ToString();
                        this.SetNodeAccordingToProjectType(e.Node, Type);
                        if (this.ProjectTypeFits(Type))
                        {
                            if (!this.AddedProjectRows.Contains(R))
                                this.AddedProjectRows.Add(R);
                        }
                        else
                        {
                            string Message = "";
                            if (Type.Length > 0) Message = "The selected project is of type " + Type + ".\r\nThe allowed projects are restricted to type " + ProjectType;
                            else Message = "The selected project misses the correct type (= " + this.ProjectType + ")";
                            Message += "\r\n\r\nYou may deselect the restriction to enable adding differing projects";
                            System.Windows.Forms.MessageBox.Show(Message, "No " + ProjectType, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (this.AddedProjectRows.Contains(R))
                                this.AddedProjectRows.Remove(R);
                            if (e.Node.Checked)
                                e.Node.Checked = false;
                        }
                    }
                    else
                    {
                        if (e.Node.ForeColor == System.Drawing.Color.Red)
                        {
                            if (e.Node.Checked)
                            {
                                e.Node.BackColor = System.Drawing.Color.LightGreen;
                                if (!this.AddedProjectRows.Contains(R))
                                    this.AddedProjectRows.Add(R);
                            }
                            else
                            {
                                e.Node.BackColor = System.Drawing.Color.White;
                                if (this.AddedProjectRows.Contains(R))
                                    this.AddedProjectRows.Remove(R);
                            }
                        }
                        else
                        {
                            e.Node.Checked = true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void treeViewProjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            this.buildProjectTree();
        }

        #endregion

        #region Download

        private void buttonStartDownload_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        #endregion

        #region ProjectProxy

        private void initProjectProxy()
        {
            try
            {
                string SQL = "select c.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'ProjectProxy' " +
                    "and c.DATA_TYPE <> 'uniqueidentifier' " +
                    "and c.DATA_TYPE <> 'xml' " +
                    "order by c.ORDINAL_POSITION";
                System.Data.DataTable dtColumns = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtColumns);

                SQL = "SELECT Project, ProjectID AS ID, ProjectURI ";
                foreach (System.Data.DataRow R in dtColumns.Rows)
                {
                    if (R[0].ToString() != "ProjectID"
                        && R[0].ToString() != "Project"
                        && R[0].ToString() != "ProjectURI")
                        SQL += ", " + R[0].ToString();
                }
                SQL += " FROM ProjectProxy ORDER BY Project";

                System.Data.DataTable dtProject = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtProject);
                this.dataGridViewProjectProxy.DataSource = dtProject;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonProjectProxyRefresh_Click(object sender, EventArgs e)
        {
            this.initProjectProxy();
        }

        #endregion

        #region Restriction to type

        private bool _IncludeAllProjectTypes = true;

        private string _ProjectType = "";
        public string ProjectType
        {
            set
            {
                try
                {
                    this._ProjectType = value;
                    this.checkBoxIncludeAll.Visible = true;
                    this.checkBoxIncludeAll.Checked = true;
                    this.checkBoxIncludeAll.Text = "Restrict to\r\n" + value;
                    this._IncludeAllProjectTypes = false;
                    switch (value.ToLower())
                    {
                        case "agents":
                            this.checkBoxIncludeAll.Image = this.imageListTreeView.Images[2];
                            break;
                        case "checklist":
                            this.checkBoxIncludeAll.Image = this.imageListTreeView.Images[1];
                            break;
                        case "collection":
                            this.checkBoxIncludeAll.Image = this.imageListTreeView.Images[3];
                            break;
                        case "descriptions":
                            this.checkBoxIncludeAll.Image = this.imageListTreeView.Images[12];
                            break;
                        case "gazetteer":
                            this.checkBoxIncludeAll.Image = this.imageListTreeView.Images[4];
                            break;
                        case "references":
                            this.checkBoxIncludeAll.Image = this.imageListTreeView.Images[6];
                            break;
                        case "regulation":
                            this.checkBoxIncludeAll.Image = this.imageListTreeView.Images[10];
                            break;
                        case "samplingplots":
                            this.checkBoxIncludeAll.Image = this.imageListTreeView.Images[7];
                            break;
                        case "scientificterms":
                            this.checkBoxIncludeAll.Image = this.imageListTreeView.Images[8];
                            break;
                        case "taxonnames":
                            this.checkBoxIncludeAll.Image = this.imageListTreeView.Images[9];
                            break;
                        default:
                            this.checkBoxIncludeAll.Image = this.imageListTreeView.Images[0];
                            this._ProjectType = "";
                            this._IncludeAllProjectTypes = true;
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    this._ProjectType = "";
                    this._IncludeAllProjectTypes = true;
                    this.checkBoxIncludeAll.Visible = false;
                    this.checkBoxIncludeAll.Checked = false;
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            get { return _ProjectType; }
        }

        private bool ProjectTypeIsSet { get { return (_ProjectType.Length > 0); } }

        private void checkBoxIncludeAll_Click(object sender, EventArgs e)
        {
            _IncludeAllProjectTypes = !checkBoxIncludeAll.Checked;
            this.buildProjectTree();
        }

        private bool ProjectTypeFits(string Type)
        {
            bool TypeFits = true;
            try
            {
                if (!_IncludeAllProjectTypes && _ProjectType.Length > 0)
                {
                    if (Type != _ProjectType)
                    {
                        TypeFits = false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return TypeFits;
        }

        private void SetNodeAccordingToProjectType(System.Windows.Forms.TreeNode N, string Type)
        {
            try
            {
                this.setNodeImage(N, Type);
                if (!_IncludeAllProjectTypes && _ProjectType.Length > 0)
                {
                    if (Type != _ProjectType)
                    {
                        N.ForeColor = System.Drawing.Color.Gray;
                        if (N.Checked)
                            N.Checked = false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private bool CheckProjectType(System.Windows.Forms.TreeNode N)
        {
            bool TypeFits = false;
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                string Type = R["ProjectType"].ToString();
                this.setNodeImage(N, Type);
                if (!_IncludeAllProjectTypes && _ProjectType.Length > 0)
                {
                    if (Type != _ProjectType)
                    {
                        N.ForeColor = System.Drawing.Color.Gray;
                        N.Checked = false;
                    }
                    else
                        TypeFits = true;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return TypeFits;
        }

        private void setNodeImage(System.Windows.Forms.TreeNode N, string Type)
        {
            /*
            Agents
            Checklist
            Collection
            Descriptions
            Gazetteer
            References
            Regulation
            SamplingPlots
            ScientificTerms
            TaxonNames
            */
            try
            {
                if (_IncludeAllProjectTypes || Type == _ProjectType)
                {
                    switch (Type.ToLower())
                    {
                        case "agents":
                            N.ImageIndex = 2;
                            N.SelectedImageIndex = 2;
                            break;
                        case "checklist":
                            N.ImageIndex = 1;
                            N.SelectedImageIndex = 1;
                            break;
                        case "collection":
                            N.ImageIndex = 3;
                            N.SelectedImageIndex = 3;
                            break;
                        case "gazetteer":
                            N.ImageIndex = 4;
                            N.SelectedImageIndex = 4;
                            break;
                        case "references":
                            N.ImageIndex = 6;
                            N.SelectedImageIndex = 6;
                            break;
                        case "regulation":
                            N.ImageIndex = 10;
                            N.SelectedImageIndex = 10;
                            break;
                        case "samplingplots":
                            N.ImageIndex = 7;
                            N.SelectedImageIndex = 7;
                            break;
                        case "scientificterms":
                            N.ImageIndex = 8;
                            N.SelectedImageIndex = 8;
                            break;
                        case "taxonnames":
                            N.ImageIndex = 9;
                            N.SelectedImageIndex = 9;
                            break;
                        default:
                            N.SelectedImageIndex = 0;
                            N.ImageIndex = 0;
                            break;
                    }
                }
                else
                {
                    N.SelectedImageIndex = 11;
                    N.ImageIndex = 11;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

    }
}
