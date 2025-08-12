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
    public partial class FormLoginOverview : Form
    {
        #region Parameter

        private System.Data.DataTable _dtLogins;
        private string _LoginName;
        private DiversityWorkbench.Login _Login;
        private System.Collections.Generic.List<DiversityWorkbench.ServerConnection> _ServerConnetionList;

        #endregion

        #region Construction

        public FormLoginOverview(string LoginName)
        {
            InitializeComponent();
            this._LoginName = LoginName;
            this._Login = new Login(LoginName);
            this._ServerConnetionList = new List<ServerConnection>();
            this.Text = "Overview for " + LoginName;
            this.initDatabaseTree();
            //this.toolStrip.Visible = false;
            foreach (System.Data.DataRow R in this.DtLogins.Rows)
            {
                this.toolStripComboBoxTargetLogin.Items.Add(R[0].ToString());
            }
            this.toolStripButtonTransferAllSettings.Enabled = false;
            this.toolStripButtonTransferDatabaseSettings.Enabled = false;
        }

        #endregion

        #region Init functions etc,

        private System.Data.DataTable DtLogins
        {
            get
            {
                if (this._dtLogins == null)
                {
                    string SQL = "select p.name, p.is_disabled, p.type " +
                        "from sys.server_principals p  " +
                        "where p.type in ('S')  " +
                        "and  p.name not like '##%##'  " +
                        "and p.name <> 'sa'   " +
                        "and p.name <> '" + this._LoginName + "' " +
                        "order by name;";
                    this._dtLogins = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(this._dtLogins);
                    }
                    catch (System.Exception ex) { }
                }
                return _dtLogins;
            }
            //set { _dtLogins = value; }
        }

        private void initDatabaseTree()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
            {
                // the modules
                System.Drawing.Font FontModule = new Font(this.treeView.Font, FontStyle.Bold);
                System.Drawing.Font FontDB = new Font(this.treeView.Font, FontStyle.Underline);
                System.Drawing.Font FontRole = new Font(this.treeView.Font, FontStyle.Italic);
                System.Drawing.Font FontProject = new Font(this.treeView.Font, FontStyle.Italic);
                System.Windows.Forms.TreeNode N = new TreeNode(KV.Key + "         ");
                N.NodeFont = FontModule;
                N.ImageIndex = 0;
                N.SelectedImageIndex = 0;

                this.treeView.Nodes.Add(N);
                string x = KV.Value.ServerConnection.ModuleName;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in KV.Value.ServerConnectionList())
                {
                    if (KV.Value.ServerConnection.ModuleName != KVconn.Value.ModuleName)
                        continue;
                    if (KV.Value.ServerConnectionList()[KVconn.Key].IsAddedRemoteConnection)
                        continue;

                    this._ServerConnetionList.Add(KVconn.Value);
                    System.Windows.Forms.TreeNode NC = new TreeNode(KVconn.Key);
                    NC.NodeFont = FontDB;
                    NC.Tag = KVconn.Value;
                    NC.ImageIndex = 1;
                    NC.SelectedImageIndex = 1;
                    NC.Tag = KVconn.Value;
                    N.Nodes.Add(NC);
                    if (LoginHasAccessToDatabase(KVconn.Value))
                    {
                        this.getAgentInfos(NC, KVconn.Value);
                        this.getUserRoles(NC, KVconn.Value);
                        this.getProjects(NC, KVconn.Value);
                    }
                    else
                        NC.ForeColor = System.Drawing.Color.LightGray;
                }
            }
            this.treeView.ExpandAll();
        }

        private void getAgentInfos(System.Windows.Forms.TreeNode N, ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = this._Login.AgentInfos(SC);
            if (Dict.Count > 0)
            {
                string Title = "Info";
                if (Dict.ContainsKey("PersonName"))
                    Title = Dict["PersonName"];
                System.Windows.Forms.TreeNode Ninfos = new TreeNode(Title);
                System.Drawing.Font FontInfo = new Font(this.treeView.Font, FontStyle.Regular);
                Ninfos.ImageIndex = 6;
                Ninfos.SelectedImageIndex = 6;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Login.AgentInfos(SC))
                {
                    if (KV.Key.StartsWith("_"))
                        continue;
                    if (KV.Value.Length > 0)
                    {
                        System.Windows.Forms.TreeNode Ninfo = new TreeNode(KV.Key + ": " + KV.Value);
                        Ninfo.NodeFont = FontInfo;
                        Ninfo.ForeColor = System.Drawing.Color.Blue;
                        Ninfo.ImageIndex = 5;
                        Ninfo.SelectedImageIndex = 5;
                        Ninfos.Nodes.Add(Ninfo);
                    }
                }
                if (Ninfos.Nodes.Count > 0)
                {
                    N.Nodes.Add(Ninfos);
                }
            }
        }

        private void getUserRoles(System.Windows.Forms.TreeNode N, ServerConnection SC)
        {
            System.Drawing.Font FontRole = new Font(this.treeView.Font, FontStyle.Italic);
            System.Data.DataTable dtUserRoles = this._Login.UserRoles(SC);// new DataTable();
            if (dtUserRoles.Rows.Count > 0)
            {
                System.Windows.Forms.TreeNode Nroles = new TreeNode("Roles");
                Nroles.NodeFont = FontRole;
                Nroles.ImageIndex = 2;
                Nroles.SelectedImageIndex = 2;
                N.Nodes.Add(Nroles);
                foreach (System.Data.DataRow R in dtUserRoles.Rows)
                {
                    System.Windows.Forms.TreeNode Nrole = new TreeNode(R["name"].ToString());
                    Nrole.NodeFont = FontRole;
                    Nrole.ImageIndex = 5;
                    Nrole.SelectedImageIndex = 5;
                    Nroles.Nodes.Add(Nrole);
                }
            }
        }

        private void getProjects(System.Windows.Forms.TreeNode N, ServerConnection SC)
        {
            if (this._Login.ProjectsAndUserProxyDoExist(SC))// this.ProjectsAndUserProxyDoExist(SC))
            {
                try
                {
                    bool ProjectsProvideReadOnly = this._Login.ProjectProvideReadOnly(SC);// false;
                    System.Data.DataTable dtProjectsAccess = this._Login.AccessibleProjects(SC);// new DataTable();
                    if (dtProjectsAccess.Rows.Count > 0)
                    {
                        System.Windows.Forms.TreeNode NP = new TreeNode("Projects");
                        NP.ImageIndex = 3;
                        NP.SelectedImageIndex = 3;
                        N.Nodes.Add(NP);
                        foreach (System.Data.DataRow R in dtProjectsAccess.Rows)
                        {
                            System.Windows.Forms.TreeNode Nproject = new TreeNode(R["Project"].ToString());
                            Nproject.Tag = R;
                            Nproject.ImageIndex = 5;
                            Nproject.SelectedImageIndex = 5;
                            NP.Nodes.Add(Nproject);
                        }
                    }

                    // Read Only Projects
                    if (ProjectsProvideReadOnly)
                    {
                        System.Data.DataTable dtProjectsReadOnly = this._Login.ReadOnlyProjects(SC);// new DataTable();
                        if (dtProjectsReadOnly.Rows.Count > 0)
                        {
                            System.Windows.Forms.TreeNode NR = new TreeNode("Read only projects");
                            NR.ImageIndex = 4;
                            NR.SelectedImageIndex = 4;
                            NR.ForeColor = System.Drawing.Color.LightGray;
                            N.Nodes.Add(NR);
                            foreach (System.Data.DataRow R in dtProjectsReadOnly.Rows)
                            {
                                System.Windows.Forms.TreeNode Nread = new TreeNode(R["Project"].ToString());
                                Nread.Tag = R;
                                Nread.ForeColor = System.Drawing.Color.LightGray;
                                Nread.ImageIndex = 5;
                                Nread.SelectedImageIndex = 5;
                                NR.Nodes.Add(Nread);
                            }
                        }
                    }
                    bool ProjectsHaveListRestriction = this._Login.ProjectHasListRestriction(SC);
                    if (ProjectsHaveListRestriction)
                    {
                        System.Data.DataTable dtListRestrictions = this._Login.ProjectListRestrictions(SC);
                        if (dtListRestrictions.Rows.Count > 0)
                        {
                            System.Windows.Forms.TreeNode NR = new TreeNode("Restriction to lists");
                            NR.ImageIndex = 3;
                            NR.SelectedImageIndex = 3;
                            NR.ForeColor = System.Drawing.Color.Green;
                            N.Nodes.Add(NR);
                            foreach (System.Data.DataRow R in dtListRestrictions.Rows)
                            {
                                System.Windows.Forms.TreeNode Nread = new TreeNode(R["List"].ToString());
                                Nread.Tag = R;
                                Nread.ForeColor = System.Drawing.Color.Green;
                                Nread.ImageIndex = 5;
                                Nread.SelectedImageIndex = 5;
                                NR.Nodes.Add(Nread);
                            }
                        }
                    }

                }
                catch (System.Exception ex) { }
            }
        }

        private bool LoginHasAccessToDatabase(DiversityWorkbench.ServerConnection SC)
        {
            bool LoginHasAccess = this._Login.LoginHasAccessToDatabase(SC);// false;
            //try
            //{
            //    string HasAccess = "0";
            //    string SQL = "use " + SC.DatabaseName + "; " +
            //        "SELECT u.hasdbaccess " +
            //        "FROM sysusers u " +
            //        "WHERE u.name = '" + this._LoginName + "'";
            //    Microsoft.Data.SqlClient.SqlConnection Con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, Con);
            //    try
            //    {
            //        Con.Open();
            //        HasAccess = C.ExecuteScalar().ToString();
            //    }
            //    catch (System.Exception ex) { }
            //    finally
            //    {
            //        Con.Close();
            //        Con.Dispose();
            //    }
            //    System.Data.DataTable dt = new DataTable();
            //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //    ad.Fill(dt);
            //    if (HasAccess == "0")
            //    {
            //        LoginHasAccess = false;
            //    }
            //    else
            //    {
            //        LoginHasAccess = true;
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    LoginHasAccess = false;
            //}
            return LoginHasAccess;
        }

        private bool ProjectsAndUserProxyDoExist(DiversityWorkbench.ServerConnection SC)
        {
            bool ProjectsDoExist = this._Login.ProjectsAndUserProxyDoExist(SC);// false;
            //try
            //{
            //    int i;
            //    //Check if the table UserProxy and the column AgentURI exists
            //    string SQL = "use " + SC.DatabaseName + "; select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'UserProxy' and C.COLUMN_NAME = 'AgentURI'";
            //    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i) && i > 0)
            //    {
            //        SQL = "use " + SC.DatabaseName + "; " +
            //        "SELECT COUNT(*) AS Anzahl " +
            //        "FROM UserProxy AS U INNER JOIN " +
            //        "ProjectUser AS PU ON U.LoginName = PU.LoginName RIGHT OUTER JOIN " +
            //        "ProjectProxy AS P ON PU.ProjectID = P.ProjectID";
            //        if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i))
            //        {
            //            ProjectsDoExist = true;
            //        }
            //        else
            //            ProjectsDoExist = false;
            //    }
            //    else
            //        ProjectsDoExist = false;
            //}
            //catch (System.Exception ex)
            //{
            //    ProjectsDoExist = false;
            //}

            return ProjectsDoExist;
        }

        #endregion

        #region Events

        private void toolStripButtonTransferAllSettings_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Login Target = new Login(this.toolStripComboBoxTargetLogin.SelectedItem.ToString());
            foreach (DiversityWorkbench.ServerConnection SC in this._ServerConnetionList)
            {
                this._Login.CopyDatabaseSettings(SC, Target);
            }
        }

        private void toolStripButtonTransferDatabaseSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeView.SelectedNode.Tag != null)

                {
                    DiversityWorkbench.Login Target = new Login(this.toolStripComboBoxTargetLogin.SelectedText);
                    DiversityWorkbench.ServerConnection SC = (DiversityWorkbench.ServerConnection)this.treeView.SelectedNode.Tag;
                    this._Login.CopyDatabaseSettings(SC, Target);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (this.treeView.SelectedNode.Tag != null)
                {
                    DiversityWorkbench.ServerConnection SC = (DiversityWorkbench.ServerConnection)this.treeView.SelectedNode.Tag;
                    this.toolStripButtonTransferDatabaseSettings.Enabled = true;
                }
                else
                    this.toolStripButtonTransferDatabaseSettings.Enabled = false;
            }
            catch (System.Exception ex)
            {
                this.toolStripButtonTransferDatabaseSettings.Enabled = false;
            }
        }

        private void toolStripComboBoxTargetLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.toolStripButtonTransferAllSettings.Enabled = true;
        }

        #endregion

    }
}
