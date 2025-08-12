using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormConnectionAdministration : Form
    {
        #region Parameter
        private bool _IsSecurityAdmin = false;
        private string _SystemUser = "";
        private string SystemUser
        {
            get
            {
                if (_SystemUser == "")
                {
                    string SQL = "SELECT SYSTEM_USER";
                    try
                    {
                        _SystemUser = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    }
                    catch { }
                }
                return _SystemUser;
            }
        }

        private System.Data.DataTable _dtSecurityAdmin;
        private System.Data.DataTable DtSecurityAdmin
        {
            get
            {
                if (this._dtSecurityAdmin == null)
                {
                    this._dtSecurityAdmin = new DataTable();
                    string SQL = "sp_helpsrvrolemember 'sysadmin'";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._dtSecurityAdmin);
                }
                return _dtSecurityAdmin;
            }
        }
        #endregion

        #region Construction and form

        public FormConnectionAdministration(string PathHelpProvider, bool BacklinkUpdateEnabled = false)
        {
            InitializeComponent();
            this.initForm();
            this.initTree();
            this.buttonUpdateBacklinkModule.Visible = BacklinkUpdateEnabled;
            // online manual
            this.helpProvider.HelpNamespace = PathHelpProvider;
        }

        private void FormConnectionAdministration_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
            catch (System.Exception ex) { }
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Form
        private void initForm()
        {
            try
            {
                System.Data.DataRow[] rr = this.DtSecurityAdmin.Select("MemberName = '" + this.SystemUser + "'");
                if (rr.Length > 0)
                {
                    _IsSecurityAdmin = true;
                }
                this.buttonLinkedServer.Enabled = _IsSecurityAdmin;

                /// ToDo - das klappt noch nicht. Datenbanken werden zwar angezeigt aber die Linked Server nicht dauerhaft aktualisiert. Ist nach Schliessen den Formulars wieder weg
#if !DEBUG
                this.buttonRequery.Visible = false;
#endif
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        #endregion

        #region Tree

        private void initTree()
        {
            this.buttonLoadAdded.Enabled = (DiversityWorkbench.Settings.AddedRemoteConnections != null && DiversityWorkbench.Settings.AddedRemoteConnections.Count > 0);
            this.treeViewConnections.Nodes.Clear();
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                {
                    // the modules
                    System.Drawing.Font Font = new Font(this.treeViewConnections.Font, FontStyle.Bold);
                    System.Drawing.Font FontDB = new Font(this.treeViewConnections.Font, FontStyle.Underline);
                    System.Windows.Forms.TreeNode N = new TreeNode(KV.Key + "         ");
                    N.Tag = KV.Value;
                    N.ImageIndex = 0;
                    N.SelectedImageIndex = 0;
                    N.NodeFont = Font;

                    if (KV.Key == DiversityWorkbench.Settings.ModuleName)
                        N.BackColor = System.Drawing.Color.Yellow;

                    this.treeViewConnections.Nodes.Add(N);
                    string x = KV.Value.ServerConnection.ModuleName;

                    this.treeViewAddServerConnections(N, KV.Value, FontDB);
                    this.treeViewAddAdditionalServices(N, KV.Value, FontDB);

                    N.Expand();
                }
                if (DiversityWorkbench.Settings.LoadConnections || DiversityWorkbench.Settings.LoadConnectionsPassed())
                {
                    if (DiversityWorkbench.LinkedServer.LinkedServers().Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServer> KV in DiversityWorkbench.LinkedServer.LinkedServers())
                        {
                            System.Windows.Forms.TreeNode NLink = new TreeNode(KV.Key);
                            NLink.SelectedImageIndex = 3;
                            NLink.ImageIndex = 3;
                            NLink.ForeColor = System.Drawing.Color.Gray;
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServerDatabase> KVdb in KV.Value.Databases())
                            {
                                System.Windows.Forms.TreeNode Ndb = new TreeNode(KVdb.Key);
                                Ndb.ImageIndex = 4;
                                Ndb.SelectedImageIndex = 4;
                                Ndb.ForeColor = System.Drawing.Color.Gray;
                                NLink.Nodes.Add(Ndb);
                            }
                            this.treeViewConnections.Nodes.Add(NLink);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void treeViewAddServerConnections(System.Windows.Forms.TreeNode N, DiversityWorkbench.WorkbenchUnit WBUnit, System.Drawing.Font FontDB, bool Requery = false)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in WBUnit.ServerConnectionList(Requery))
            {
                // the databases
                if (WBUnit.ServerConnection.ModuleName != KVconn.Value.ModuleName)
                    continue;
                else if (WBUnit.ServerConnection.ModuleName == DiversityWorkbench.Settings.ServerConnection.ModuleName)
                {
                    if (KVconn.Value.LinkedServer.Length > 0)
                        continue;
                }

                System.Windows.Forms.TreeNode NC = new TreeNode(KVconn.Key);
                NC.NodeFont = FontDB;
                NC.Tag = KVconn.Value;
                NC.SelectedImageIndex = 1;
                NC.ImageIndex = 1;
                if (KVconn.Value.LinkedServer.Length > 0)
                {
                    NC.SelectedImageIndex = 4;
                    NC.ImageIndex = 4;
                    NC.ForeColor = System.Drawing.Color.Gray;
                }
                else
                {
                    if (KVconn.Value.ModuleName != DiversityWorkbench.Settings.ModuleName)
                    {
                        if (KVconn.Value.BackLinkDoUpdate(DiversityWorkbench.Settings.ServerConnection.DatabaseKey, KVconn.Value.ModuleName))// BackLinkIsUpdated != null && (bool)KVconn.Value.BackLinkIsUpdated)
                            NC.BackColor = System.Drawing.Color.LightGreen;
                    }
                }
                bool IsAddedRemoteConnection = false;
                if (WBUnit.ServerConnectionList()[KVconn.Key].IsAddedRemoteConnection)
                    IsAddedRemoteConnection = true;
                if (IsAddedRemoteConnection)
                {
                    NC.ForeColor = System.Drawing.Color.Green;
                    if (!WBUnit.ServerConnectionList()[KVconn.Key].ConnectionIsValid)
                        NC.BackColor = System.Drawing.Color.Pink;
                }
                else if (WBUnit.ServiceName() == DiversityWorkbench.Settings.ModuleName)
                    NC.BackColor = System.Drawing.Color.Yellow;
                N.Nodes.Add(NC);
                if (KVconn.Value.IsLocalExpressDatabase)
                {
                    System.Windows.Forms.TreeNode NCLocal = new TreeNode("Local DB: " + KVconn.Value.SqlExpressDbFileName);
                    NC.Nodes.Add(NCLocal);
                }
                else if (KVconn.Value.DatabaseServer != null)
                {
                    if (KVconn.Value.LinkedServer.Length > 0)
                    {
                        string Server = "Server: " + KVconn.Value.LinkedServer;
                        System.Windows.Forms.TreeNode NCServer = new TreeNode(Server);
                        NCServer.ForeColor = System.Drawing.Color.Gray;
                        NCServer.ImageIndex = 5;
                        NCServer.SelectedImageIndex = 5;
                        NC.Nodes.Add(NCServer);
                    }
                    else
                    {
                        string Server = "Server: " + KVconn.Value.DatabaseServer;
                        if (KVconn.Value.DatabaseServerPort != 0 && KVconn.Value.DatabaseServerPort != 1433)
                            Server += ". Port: " + KVconn.Value.DatabaseServerPort.ToString();
                        System.Windows.Forms.TreeNode NCServer = new TreeNode(Server);
                        if (IsAddedRemoteConnection)
                            NCServer.ForeColor = System.Drawing.Color.Green;
                        else if (WBUnit.ServiceName() == DiversityWorkbench.Settings.ModuleName)
                            NCServer.BackColor = System.Drawing.Color.Yellow;
                        NCServer.ImageIndex = 5;
                        NCServer.SelectedImageIndex = 5;
                        NC.Nodes.Add(NCServer);
                    }
                }
                if (KVconn.Value.IsTrustedConnection || KVconn.Value.DatabaseUser != null)
                {
                    string Login = "SQL-server authentication. User: " + KVconn.Value.DatabaseUser;
                    if (KVconn.Value.IsTrustedConnection) Login = "Windows authentication";
                    if (KVconn.Value.LinkedServer.Length > 0) Login = "Linked server account";
                    System.Windows.Forms.TreeNode NCLogin = new TreeNode(Login);
                    if (IsAddedRemoteConnection)
                        NCLogin.ForeColor = System.Drawing.Color.Green;
                    else if (WBUnit.ServiceName() == DiversityWorkbench.Settings.ModuleName)
                        NCLogin.BackColor = System.Drawing.Color.Yellow;
                    else if (KVconn.Value.LinkedServer.Length > 0)
                        NCLogin.ForeColor = System.Drawing.Color.Gray;
                    NCLogin.SelectedImageIndex = 6;
                    NCLogin.ImageIndex = 6;
                    NC.Nodes.Add(NCLogin);
                }
                if (KVconn.Value.LinkedServer.Length > 0)
                {
                    string BaseURL = "";
                    if (DiversityWorkbench.LinkedServer.LinkedServers().ContainsKey(KVconn.Value.LinkedServer) &&
                        DiversityWorkbench.LinkedServer.LinkedServers()[KVconn.Value.LinkedServer].Databases().ContainsKey(KVconn.Value.DatabaseName))
                    {
                        BaseURL = DiversityWorkbench.LinkedServer.LinkedServers()[KVconn.Value.LinkedServer].Databases()[KVconn.Value.DatabaseName].BaseURL;
                    }
                    else if (KVconn.Value.DatabaseName.StartsWith("[" + KVconn.Value.LinkedServer + "]."))
                    {
                        string DB = KVconn.Value.DatabaseName.Substring(KVconn.Value.DatabaseName.LastIndexOf(".") + 1);
                        if (DiversityWorkbench.LinkedServer.LinkedServers()[KVconn.Value.LinkedServer].Databases().ContainsKey(DB))
                            BaseURL = DiversityWorkbench.LinkedServer.LinkedServers()[KVconn.Value.LinkedServer].Databases()[DB].BaseURL;
                    }
                    System.Windows.Forms.TreeNode NCBaseURL = new TreeNode("Base URI = " + BaseURL);
                    NCBaseURL.ForeColor = System.Drawing.Color.Gray;
                    NCBaseURL.ImageIndex = 7;
                    NCBaseURL.SelectedImageIndex = 7;
                    NC.Nodes.Add(NCBaseURL);
                }
                else if (KVconn.Value.BaseURL != null && KVconn.Value.BaseURL.Length > 0)
                {
                    System.Windows.Forms.TreeNode NCBaseURL = new TreeNode("Base URI = " + KVconn.Value.BaseURL);
                    if (IsAddedRemoteConnection)
                    {
                        NCBaseURL.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (WBUnit.ServiceName() == DiversityWorkbench.Settings.ModuleName)
                        NCBaseURL.BackColor = System.Drawing.Color.Yellow;
                    NCBaseURL.ImageIndex = 7;
                    NCBaseURL.SelectedImageIndex = 7;
                    NC.Nodes.Add(NCBaseURL);
                }
            }
        }

        private void treeViewAddAdditionalServices(System.Windows.Forms.TreeNode N, DiversityWorkbench.WorkbenchUnit WBUnit, System.Drawing.Font FontDB)
        {
            System.Collections.Generic.SortedDictionary<string, string> AdditionalServices = new SortedDictionary<string, string>();

            foreach (System.Collections.Generic.KeyValuePair<string, string> KVservice in WBUnit.AccessibleDatabasesAndServicesOfModule())
            {
                // CacheDB
                if (WBUnit.ServiceName() != DiversityWorkbench.Settings.ServerConnection.ModuleName)
                {
                    if (!WBUnit.ServerConnectionList().ContainsKey(KVservice.Key))
                    {
                        if (KVservice.Key == "CacheDB")
                        {
                            // Check if CacheDB exists
                            string SQL = "SELECT TOP (1) DatabaseName FROM CacheDatabase2";
                            string Database = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                            if (Database.Length > 0)
                            {
                                System.Windows.Forms.TreeNode NService = new TreeNode(KVservice.Value);
                                NService.NodeFont = FontDB;
                                NService.ForeColor = System.Drawing.Color.DarkViolet;
                                NService.ImageIndex = 8;
                                NService.SelectedImageIndex = 8;
                                if (/*KV.Key == "DiversityTaxonNames" &&*/ DiversityWorkbench.Settings.ModuleName == "DiversityCollection")
                                {
                                    try
                                    {
                                        SQL = "SELECT Source, LinkedServerName, DatabaseName FROM " + Database + ".dbo.";
                                        switch (WBUnit.ServiceName())
                                        {
                                            case "DiversityAgents":
                                                SQL += "AgentSource";
                                                break;
                                            case "DiversityTaxonNames":
                                                SQL += "TaxonSynonymySource";
                                                break;
                                            case "DiversityReferences":
                                                SQL += "ReferenceTitleSource";
                                                break;
                                            case "DiversitySamplingPlots":
                                                SQL += "SamplingPlotSource";
                                                break;
                                            case "DiversityScientificTerms":
                                                SQL += "ScientificTermSource";
                                                break;
                                            case "DiversityGazetteer":
                                                SQL += "GazetteerSource";
                                                break;
                                        }
                                        SQL += " ORDER BY LinkedServerName desc, DatabaseName, Source";
                                        System.Data.DataTable dt = new DataTable();
                                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                                        foreach (System.Data.DataRow R in dt.Rows)
                                        {
                                            string DisplayText = R["Source"].ToString();
                                            if (!R["DatabaseName"].Equals(DBNull.Value) && R["DatabaseName"].ToString().Length > 0)
                                                DisplayText += " in " + R["DatabaseName"].ToString();
                                            if (!R["LinkedServerName"].Equals(DBNull.Value) && R["LinkedServerName"].ToString().Length > 0)
                                                DisplayText += " on " + R["LinkedServerName"].ToString();
                                            System.Windows.Forms.TreeNode Nsoure = new TreeNode(DisplayText);
                                            if (!R["LinkedServerName"].Equals(DBNull.Value) && R["LinkedServerName"].ToString().Length > 0)
                                            {
                                                Nsoure.ForeColor = System.Drawing.Color.Gray;
                                                Nsoure.SelectedImageIndex = 4;
                                                Nsoure.ImageIndex = 4;
                                            }
                                            else if (!R["DatabaseName"].Equals(DBNull.Value) && R["DatabaseName"].ToString().Length > 0)
                                            {
                                                Nsoure.ForeColor = System.Drawing.Color.Black;
                                                Nsoure.SelectedImageIndex = 1;
                                                Nsoure.ImageIndex = 1;
                                            }
                                            else
                                            {
                                                Nsoure.ForeColor = System.Drawing.Color.Blue;
                                                Nsoure.SelectedImageIndex = 2;
                                                Nsoure.ImageIndex = 2;
                                            }
                                            NService.Nodes.Add(Nsoure);
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                    }
                                }
                                N.Nodes.Add(NService);
                            }
                        }
                        else
                        {
                            AdditionalServices.Add(KVservice.Key, KVservice.Value);
                        }
                    }
                }
            }

            foreach (System.Collections.Generic.KeyValuePair<string, string> AS in AdditionalServices)
            {
                System.Windows.Forms.TreeNode NService = new TreeNode(AS.Value);
                NService.NodeFont = FontDB;
                NService.ForeColor = System.Drawing.Color.Blue;
                NService.ImageIndex = 2;
                NService.SelectedImageIndex = 2;
                N.Nodes.Add(NService);
                string value = "";
                if (!WBUnit.DatabaseAndServiceURIs().TryGetValue(AS.Key, out value))
                    continue;
                System.Windows.Forms.TreeNode NURI = new TreeNode("Base URI = " + value);
                NURI.ForeColor = System.Drawing.Color.Blue;
                NURI.SelectedImageIndex = 7;
                NURI.ImageIndex = 7;
                NService.Nodes.Add(NURI);
            }
        }

        private void treeViewConnections_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.buttonRemove.Enabled = false;
            this.buttonConnect.Enabled = false;
            this.buttonAdd.Enabled = false;
            this.buttonRequery.Enabled = false;
            this.buttonUpdateBacklinkModule.Enabled = false;
            if (this.treeViewConnections.SelectedNode != null && this.treeViewConnections.SelectedNode.Tag != null)
            {
                if (this.treeViewConnections.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.ServerConnection))
                {
                    DiversityWorkbench.ServerConnection S = (DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag;
                    if (S.ModuleName != DiversityWorkbench.Settings.ModuleName)
                    {
                        this.buttonRemove.Enabled = true;
                        if (S.IsAddedRemoteConnection && !S.IsTrustedConnection && (S.DatabasePassword == null || S.DatabasePassword.Length == 0))
                            this.buttonConnect.Enabled = true;
                        else
                            this.buttonConnect.Enabled = false;
                        // vorerst abgeschaltet
                        // this.buttonConnect.Enabled = true;
                        // Markus 18.11.22: Check for dependent mocules
                        this.buttonUpdateBacklinkModule.Enabled = S.BackLinkIsUpdated != null;// && (bool)S.BackLinkIsUpdated.BacklinkModules[DiversityWorkbench.Settings.ModuleName].Contains(S.ModuleName);
                        if (S.BackLinkIsUpdated != null)
                        {
                            bool DbIsUpdated = DiversityWorkbench.Settings.BacklinkUpdatedDatabases().ContainsKey(DiversityWorkbench.Settings.ServerConnection.DatabaseKey)
                                && DiversityWorkbench.Settings.BacklinkUpdatedDatabases()[DiversityWorkbench.Settings.ServerConnection.DatabaseKey].ContainsKey(S.ModuleName)
                                && DiversityWorkbench.Settings.BacklinkUpdatedDatabases()[DiversityWorkbench.Settings.ServerConnection.DatabaseKey][S.ModuleName].Contains(S.DisplayText);
                            if (DbIsUpdated) this.buttonUpdateBacklinkModule.BackColor = System.Drawing.Color.Pink;
                            else this.buttonUpdateBacklinkModule.BackColor = System.Drawing.SystemColors.Control;
                        }
                    }
                }
                else if (this.treeViewConnections.SelectedNode.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                {
                    DiversityWorkbench.WorkbenchUnit WU = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Tag;
                    if (WU.ServiceName() != DiversityWorkbench.Settings.ModuleName)
                    {
                        this.buttonConnect.Enabled = false;
                        // vorerst abgeschaltet
                        // this.buttonConnect.Enabled = true;
                        this.buttonAdd.Enabled = true;
                        this.buttonRequery.Enabled = true;
                        this.buttonUpdateBacklinkModule.Enabled = true;
                        this.buttonUpdateBacklinkModule.BackColor = System.Drawing.SystemColors.Control;
                    }
                    else
                        this.buttonConnect.Enabled = false;
                }
                else this.buttonConnect.Enabled = false;
            }
            else this.buttonConnect.Enabled = false;
        }

        private void RequeryNode(System.Windows.Forms.TreeNode N)
        {
            try
            {
                if (N.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                {
                    DiversityWorkbench.WorkbenchUnit workbenchUnit = (DiversityWorkbench.WorkbenchUnit)N.Tag;
                    N.Nodes.Clear();
                    System.Drawing.Font FontDB = new Font(this.treeViewConnections.Font, FontStyle.Underline);
                    this.treeViewAddServerConnections(N, workbenchUnit, FontDB, true);
                    this.treeViewAddAdditionalServices(N, workbenchUnit, FontDB);
                    //if (DiversityWorkbench.LinkedServer.LinkedServers().Count > 0)
                    //{
                    //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServer> KV in DiversityWorkbench.LinkedServer.LinkedServers())
                    //    {
                    //        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServerDatabase> KVdb in KV.Value.Databases(workbenchUnit.ServiceName(), true))
                    //        {
                    //            if (KVdb.Value.DiversityWorkbenchModule == workbenchUnit.ServiceName())
                    //            {
                    //                System.Windows.Forms.TreeNode Ndb = new TreeNode(KVdb.Key);
                    //                Ndb.ImageIndex = 4;
                    //                Ndb.SelectedImageIndex = 4;
                    //                Ndb.ForeColor = System.Drawing.Color.Gray;
                    //                N.Nodes.Add(Ndb);
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #region Button events

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (this.treeViewConnections.SelectedNode != null && this.treeViewConnections.SelectedNode.Tag != null)
            {
                if (this.treeViewConnections.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.ServerConnection))
                {
                    DiversityWorkbench.ServerConnection S = (DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag;
                    if (S.ModuleName != DiversityWorkbench.Settings.ModuleName)
                    {
                        if (S.IsAddedRemoteConnection && !S.IsTrustedConnection && (S.DatabasePassword == null || S.DatabasePassword.Length == 0))
                        {
                            System.Windows.Forms.DialogResult Res = System.Windows.Forms.DialogResult.OK;
                            DiversityWorkbench.Forms.FormGetPassword fP = new Forms.FormGetPassword(S.DatabaseUser);
                            //DiversityWorkbench.Forms.FormGetString fP = new FormGetString("Password", "Please enter the password for the connection\r\n" + S.DisplayText, "");
                            int i = 5;
                            while (Res == System.Windows.Forms.DialogResult.OK && i > 0)
                            {
                                fP.ShowDialog();
                                if (fP.DialogResult == System.Windows.Forms.DialogResult.OK && fP.Password().Length > 0)
                                {
                                    S.DatabasePassword = fP.Password();
                                    if (S.ConnectionIsValid)
                                    {
                                        System.Windows.Forms.MessageBox.Show("Connection established");
                                        this.buttonConnect.Enabled = false;
                                        this.treeViewConnections.SelectedNode.BackColor = System.Drawing.SystemColors.Window;
                                        break;
                                    }
                                    else
                                    {
                                        Res = System.Windows.Forms.MessageBox.Show("Wrong password. " + i.ToString() + " trials left. Try again?", "Wrong password", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                                        S.DatabasePassword = "";
                                    }
                                }
                                else if (fP.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                                    break;
                                i--;
                            }
                        }
                    }
                }
            }
            return;

            //return;
            // vorerst wird diese Moeglichkeit entfernt - zusaetzliche Fehlerquelle wenn die DB bereits in der Liste vorhanden ist
            // nur loeschen und neue anlegen zulassen. Beim neu anlegen abfragen ob die Verbindung bereits vorhanden ist

            if (this.treeViewConnections.SelectedNode != null && this.treeViewConnections.SelectedNode.Tag != null)
            {
                DiversityWorkbench.Forms.FormConnectToDatabase f;
                if (this.treeViewConnections.SelectedNode.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                    f = new DiversityWorkbench.Forms.FormConnectToDatabase((DiversityWorkbench.IWorkbenchUnit)this.treeViewConnections.SelectedNode.Tag);
                else
                    f = new DiversityWorkbench.Forms.FormConnectToDatabase((DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag);

                //DiversityWorkbench.FormDatabaseConnection f;
                //if (this.treeViewConnections.SelectedNode.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                //    f = new FormDatabaseConnection((DiversityWorkbench.IWorkbenchUnit)this.treeViewConnections.SelectedNode.Tag);
                //else
                //    f = new FormDatabaseConnection((DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    if (f.ServerConnection.ConnectionString.Length > 0)
                    {
                        DiversityWorkbench.WorkbenchUnit WU;
                        if (this.treeViewConnections.SelectedNode.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                            WU = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Tag;
                        else if (this.treeViewConnections.SelectedNode.Parent.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                            WU = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Parent.Tag;
                        else
                        {
                            WU = new WorkbenchUnit(null);
                            return;
                        }
                        if (!DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[WU.ServiceName()].ServerConnectionList().ContainsKey(f.ServerConnection.DatabaseName))
                        {
                            //DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[WU.ServiceName()].ServerConnectionList().Add(f.ServerConnection.DatabaseName, f.ServerConnection);
                            DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitListAddDatabase(WU.ServiceName(), f.ServerConnection.DatabaseName, f.ServerConnection);
                        }
                        else
                        {
                            DiversityWorkbench.ServerConnection S = (DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag;
                            if (f.ServerConnection.ConnectionString.Length > 0
                                && S.DatabaseName == f.ServerConnection.DatabaseName)
                            {
                                S.DatabaseName = f.ServerConnection.DatabaseName;
                                S.DatabasePassword = f.ServerConnection.DatabasePassword;
                                S.DatabaseServer = f.ServerConnection.DatabaseServer;
                                S.DatabaseServerPort = f.ServerConnection.DatabaseServerPort;
                                S.DatabaseUser = f.ServerConnection.DatabaseUser;
                                S.IsLocalExpressDatabase = f.ServerConnection.IsLocalExpressDatabase;
                                S.IsTrustedConnection = f.ServerConnection.IsTrustedConnection;
                                S.BaseURL = f.ServerConnection.BaseURL;
                                //if (f.ServerConnection.IsTrustedConnection != S.IsTrustedConnection
                                //    || f.ServerConnection.DatabaseServer != S.DatabaseServer
                                //    || f.ServerConnection.DatabaseUser != S.DatabaseUser
                                //    || f.ServerConnection.DatabaseServer != DiversityWorkbench.Settings.DatabaseServer
                                //    || f.ServerConnection.DatabaseUser != DiversityWorkbench.Settings.DatabaseUser)
                                //    S.IsAddedRemoteConnection = true;
                                if (S.IsLocalExpressDatabase)
                                    S.SqlExpressDbFileName = f.ServerConnection.SqlExpressDbFileName;
                            }
                        }
                        this.initTree();
                    }
                }
                //else
                //{
                //    DiversityWorkbench.ServerConnection S = (DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag;
                //    DiversityWorkbench.FormDatabaseConnection f = new FormDatabaseConnection();
                //    f.ShowDialog();
                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        if (f.ServerConnection.ConnectionString.Length > 0 
                //            //&& S.ModuleName == f.ServerConnection.ModuleName
                //            && S.DatabaseName == f.ServerConnection.DatabaseName)
                //        {
                //            S.DatabasePassword = f.ServerConnection.DatabasePassword;
                //            S.DatabaseServer = f.ServerConnection.DatabaseServer;
                //            S.DatabaseServerPort = f.ServerConnection.DatabaseServerPort;
                //            S.DatabaseUser = f.ServerConnection.DatabaseUser;
                //            S.IsLocalExpressDatabase = f.ServerConnection.IsLocalExpressDatabase;
                //            S.IsTrustedConnection = f.ServerConnection.IsTrustedConnection;
                //            if (S.IsLocalExpressDatabase)
                //                S.SqlExpressDbFileName = f.ServerConnection.SqlExpressDbFileName;
                //            this.initTree();
                //        }
                //    }
                //}
            }
        }

        /// <summary>
        /// Removing an entry from the tree that list all connections to the module relalted databases and from the global list. 
        /// For the ServerConnection for the module, try to set the database to the next name in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeViewConnections.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.ServerConnection))
                {
                    DiversityWorkbench.ServerConnection S = (DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag;
                    if (S.IsAddedRemoteConnection)
                    {
                        try
                        {
                            System.Collections.Specialized.StringCollection CC = DiversityWorkbench.Settings.AddedRemoteConnections;
                            System.Collections.Specialized.StringCollection CCRemove = new System.Collections.Specialized.StringCollection();
                            foreach (string C in CC)
                            {
                                DiversityWorkbench.ServerConnection Sadded = new ServerConnection(C);
                                if (S.ModuleName == Sadded.ModuleName &&
                                    S.DatabaseName == Sadded.DatabaseName &&
                                    S.DatabaseServer == Sadded.DatabaseServer &&
                                    S.DatabaseServerPort == Sadded.DatabaseServerPort &&
                                    S.DatabaseUser == Sadded.DatabaseUser)
                                    CCRemove.Add(C);
                            }
                            foreach (string s in CCRemove)
                                CC.Remove(s);
                            DiversityWorkbench.Settings.AddedRemoteConnections = CC;
                        }
                        catch (System.Exception ex) { }
                    }
                    //DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[S.ModuleName].ServerConnectionList().Remove(S.DatabaseName);
                    DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[S.ModuleName].ServerConnectionList().Remove(S.DisplayText);
                    string Database = S.DatabaseName;
                    string CurrentDatabaseOfModule = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[S.ModuleName].ServerConnection.DatabaseName;
                    if (CurrentDatabaseOfModule == Database)
                    {
                        if (!DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[S.ModuleName].ServerConnectionList().ContainsKey(Database)) // Toni 20150205: Handle unambigious database names
                        {
                            System.Collections.Generic.List<string> Databases = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[S.ModuleName].DatabaseList();
                            Databases.Remove(Database);
                            if (Databases.Count > 0)
                                DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[S.ModuleName].ServerConnection.DatabaseName = Databases[0];
                        }
                    }
                    this.initTree();
                }
            }
            catch { }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DiversityWorkbench.Settings.LoadConnectionsPassed(true);
            DiversityWorkbench.WorkbenchUnit.RequeryWorkbenchUnitConnections();
            this.initTree();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonRequery_Click(object sender, EventArgs e)
        {
            if (this.treeViewConnections.SelectedNode != null)
            {
                this.RequeryNode(this.treeViewConnections.SelectedNode);
            }
        }


        private void buttonLoadAdded_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.WorkbenchUnit.LoadAddedRemoteConnections();
            this.initTree();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string CurrentConnectionString = DiversityWorkbench.Settings.ConnectionString;
            try
            {
                if (this.treeViewConnections.SelectedNode != null && this.treeViewConnections.SelectedNode.Tag != null)
                {
                    DiversityWorkbench.Forms.FormConnectToDatabase fConn;
                    if (this.treeViewConnections.SelectedNode.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                    {
                        DiversityWorkbench.IWorkbenchUnit WU = (DiversityWorkbench.IWorkbenchUnit)this.treeViewConnections.SelectedNode.Tag;
                        DiversityWorkbench.ServerConnection SC = WU.getServerConnection();
                        fConn = new Forms.FormConnectToDatabase(SC, true);
                        //fConn = new Forms.FormConnectToDatabase(SC);
                    }
                    else if (this.treeViewConnections.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.WorkbenchUnit))
                    {
                        DiversityWorkbench.WorkbenchUnit U = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Tag;
                        fConn = new Forms.FormConnectToDatabase(U.ServerConnection, true);
                        //fConn = new Forms.FormConnectToDatabase(U.ServerConnection);
                    }
                    else
                    {
                        fConn = new Forms.FormConnectToDatabase((DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag, true);
                        //fConn = new Forms.FormConnectToDatabase((DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag);
                    }
                    fConn.ShowDialog();
                    if (fConn.DialogResult == DialogResult.OK)
                    {
                        string ConnectionStringForNewConnection = fConn.LocalServerConnection.ConnectionString;
                        if (ConnectionStringForNewConnection.Length == 0 && fConn.ServerConnection.ConnectionString.Length > 0)
                            ConnectionStringForNewConnection = fConn.ServerConnection.ConnectionString;
                        if (ConnectionStringForNewConnection.Length > 0)
                        {
                            DiversityWorkbench.WorkbenchUnit WU;
                            if (this.treeViewConnections.SelectedNode.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                                WU = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Tag;
                            else if (this.treeViewConnections.SelectedNode.Parent != null && this.treeViewConnections.SelectedNode.Parent.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                                WU = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Parent.Tag;
                            else if (this.treeViewConnections.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.WorkbenchUnit))
                                WU = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Tag;
                            else
                            {
                                WU = new WorkbenchUnit(null);
                                return;
                            }
                            if (!DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[WU.ServiceName()].ServerConnectionList().ContainsKey(fConn.LocalServerConnection.DisplayText))
                            {
                                DiversityWorkbench.ServerConnection S = fConn.ServerConnection;
                                S.IsAddedRemoteConnection = true;
                                DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitListAddDatabase(WU.ServiceName(), S.DisplayText, S);
                                DiversityWorkbench.Settings.AddRemoteConnection(S);
                            }
                            else if (this.treeViewConnections.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.ServerConnection))
                            {
                                DiversityWorkbench.ServerConnection S = (DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag;
                                if (fConn.LocalServerConnection.ConnectionString.Length > 0
                                    && S.DatabaseName == fConn.LocalServerConnection.DatabaseName)
                                {
                                    S.DatabaseName = fConn.LocalServerConnection.DatabaseName;
                                    S.DatabasePassword = fConn.LocalServerConnection.DatabasePassword;
                                    S.DatabaseServer = fConn.LocalServerConnection.DatabaseServer;
                                    S.DatabaseServerPort = fConn.LocalServerConnection.DatabaseServerPort;
                                    S.DatabaseUser = fConn.LocalServerConnection.DatabaseUser;
                                    S.IsLocalExpressDatabase = fConn.LocalServerConnection.IsLocalExpressDatabase;
                                    S.IsTrustedConnection = fConn.LocalServerConnection.IsTrustedConnection;
                                    if (S.IsLocalExpressDatabase)
                                        S.SqlExpressDbFileName = fConn.LocalServerConnection.SqlExpressDbFileName;
                                }
                            }
                            else if (this.treeViewConnections.SelectedNode.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                            {
                                DiversityWorkbench.WorkbenchUnit U = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Tag;
                                DiversityWorkbench.ServerConnection S = U.ServerConnection;
                                if (fConn.LocalServerConnection.ConnectionString.Length > 0
                                    && S.DatabaseName == fConn.LocalServerConnection.DatabaseName)
                                {
                                    if (S.DatabaseServer == fConn.LocalServerConnection.DatabaseServer)
                                    {
                                        string Message = "The database\r\n" + S.DatabaseName + " on " + S.DatabaseServer + "\r\nis allready listed.\r\nPlease remove this entry before adding a new connection for this database";
                                        System.Windows.Forms.MessageBox.Show(Message);
                                        return;
                                    }
                                    S.DatabaseName = fConn.LocalServerConnection.DatabaseName;
                                    S.DatabasePassword = fConn.LocalServerConnection.DatabasePassword;
                                    S.DatabaseServer = fConn.LocalServerConnection.DatabaseServer;
                                    S.DatabaseServerPort = fConn.LocalServerConnection.DatabaseServerPort;
                                    S.DatabaseUser = fConn.LocalServerConnection.DatabaseUser;
                                    S.IsLocalExpressDatabase = fConn.LocalServerConnection.IsLocalExpressDatabase;
                                    S.IsTrustedConnection = fConn.LocalServerConnection.IsTrustedConnection;
                                    if (S.IsLocalExpressDatabase)
                                        S.SqlExpressDbFileName = fConn.LocalServerConnection.SqlExpressDbFileName;
                                    //string x = fConn.ServerConnection.ConnectionString;
                                }
                                else
                                {
                                }
                            }
                            else
                            {
                                DiversityWorkbench.ServerConnection S = (DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag;
                                if (fConn.LocalServerConnection.ConnectionString.Length > 0
                                    && S.DatabaseName == fConn.LocalServerConnection.DatabaseName)
                                {
                                    S.DatabaseName = fConn.LocalServerConnection.DatabaseName;
                                    S.DatabasePassword = fConn.LocalServerConnection.DatabasePassword;
                                    S.DatabaseServer = fConn.LocalServerConnection.DatabaseServer;
                                    S.DatabaseServerPort = fConn.LocalServerConnection.DatabaseServerPort;
                                    S.DatabaseUser = fConn.LocalServerConnection.DatabaseUser;
                                    S.IsLocalExpressDatabase = fConn.LocalServerConnection.IsLocalExpressDatabase;
                                    S.IsTrustedConnection = fConn.LocalServerConnection.IsTrustedConnection;
                                    if (S.IsLocalExpressDatabase)
                                        S.SqlExpressDbFileName = fConn.LocalServerConnection.SqlExpressDbFileName;
                                }
                            }
                            this.initTree();
                        }


                        //if (fConn.ServerConnection.ConnectionString.Length > 0)
                        //{
                        //    DiversityWorkbench.WorkbenchUnit WU;
                        //    if (this.treeViewConnections.SelectedNode.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                        //        WU = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Tag;
                        //    else if (this.treeViewConnections.SelectedNode.Parent != null && this.treeViewConnections.SelectedNode.Parent.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                        //        WU = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Parent.Tag;
                        //    else if (this.treeViewConnections.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.WorkbenchUnit))
                        //        WU = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Tag;
                        //    else
                        //    {
                        //        WU = new WorkbenchUnit(null);
                        //        return;
                        //    }
                        //    if (!DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[WU.ServiceName()].ServerConnectionList().ContainsKey(fConn.ServerConnection.DisplayText))
                        //    {
                        //        DiversityWorkbench.ServerConnection S = fConn.ServerConnection;
                        //        S.IsAddedRemoteConnection = true;
                        //        DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitListAddDatabase(WU.ServiceName(), S.DisplayText, S);
                        //        DiversityWorkbench.Settings.AddRemoteConnection(S);
                        //    }
                        //    else if (this.treeViewConnections.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.ServerConnection))
                        //    {
                        //        DiversityWorkbench.ServerConnection S = (DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag;
                        //        if (fConn.ServerConnection.ConnectionString.Length > 0
                        //            && S.DatabaseName == fConn.ServerConnection.DatabaseName)
                        //        {
                        //            S.DatabaseName = fConn.ServerConnection.DatabaseName;
                        //            S.DatabasePassword = fConn.ServerConnection.DatabasePassword;
                        //            S.DatabaseServer = fConn.ServerConnection.DatabaseServer;
                        //            S.DatabaseServerPort = fConn.ServerConnection.DatabaseServerPort;
                        //            S.DatabaseUser = fConn.ServerConnection.DatabaseUser;
                        //            S.IsLocalExpressDatabase = fConn.ServerConnection.IsLocalExpressDatabase;
                        //            S.IsTrustedConnection = fConn.ServerConnection.IsTrustedConnection;
                        //            if (S.IsLocalExpressDatabase)
                        //                S.SqlExpressDbFileName = fConn.ServerConnection.SqlExpressDbFileName;
                        //        }
                        //    }
                        //    else if (this.treeViewConnections.SelectedNode.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                        //    {
                        //        DiversityWorkbench.WorkbenchUnit U = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Tag;
                        //        DiversityWorkbench.ServerConnection S = U.ServerConnection;
                        //        if (fConn.ServerConnection.ConnectionString.Length > 0
                        //            && S.DatabaseName == fConn.ServerConnection.DatabaseName)
                        //        {
                        //            if (S.DatabaseServer == fConn.ServerConnection.DatabaseServer)
                        //            {
                        //                string Message = "The database\r\n" + S.DatabaseName + " on " + S.DatabaseServer + "\r\nis allready listed.\r\nPlease remove this entry before adding a new connection for this database";
                        //                System.Windows.Forms.MessageBox.Show(Message);
                        //                return;
                        //            }
                        //            S.DatabaseName = fConn.ServerConnection.DatabaseName;
                        //            S.DatabasePassword = fConn.ServerConnection.DatabasePassword;
                        //            S.DatabaseServer = fConn.ServerConnection.DatabaseServer;
                        //            S.DatabaseServerPort = fConn.ServerConnection.DatabaseServerPort;
                        //            S.DatabaseUser = fConn.ServerConnection.DatabaseUser;
                        //            S.IsLocalExpressDatabase = fConn.ServerConnection.IsLocalExpressDatabase;
                        //            S.IsTrustedConnection = fConn.ServerConnection.IsTrustedConnection;
                        //            if (S.IsLocalExpressDatabase)
                        //                S.SqlExpressDbFileName = fConn.ServerConnection.SqlExpressDbFileName;
                        //        }
                        //        else
                        //        {
                        //        }
                        //    }
                        //    else
                        //    {
                        //        DiversityWorkbench.ServerConnection S = (DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag;
                        //        if (fConn.ServerConnection.ConnectionString.Length > 0
                        //            && S.DatabaseName == fConn.ServerConnection.DatabaseName)
                        //        {
                        //            S.DatabaseName = fConn.ServerConnection.DatabaseName;
                        //            S.DatabasePassword = fConn.ServerConnection.DatabasePassword;
                        //            S.DatabaseServer = fConn.ServerConnection.DatabaseServer;
                        //            S.DatabaseServerPort = fConn.ServerConnection.DatabaseServerPort;
                        //            S.DatabaseUser = fConn.ServerConnection.DatabaseUser;
                        //            S.IsLocalExpressDatabase = fConn.ServerConnection.IsLocalExpressDatabase;
                        //            S.IsTrustedConnection = fConn.ServerConnection.IsTrustedConnection;
                        //            if (S.IsLocalExpressDatabase)
                        //                S.SqlExpressDbFileName = fConn.ServerConnection.SqlExpressDbFileName;
                        //        }
                        //    }
                        //    this.initTree();
                        //}
                    }

                }
                string Test = DiversityWorkbench.Settings.ConnectionString;
                if (Test != CurrentConnectionString)
                {
                    System.Windows.Forms.MessageBox.Show("Bug:\r\n" + Test + "\r\n<>\r\n" + CurrentConnectionString);
                }
            }
            catch (System.Exception ex) { }
        }

        private bool ConnectionAllreadyPresent(DiversityWorkbench.ServerConnection S)
        {
            bool IsPresent = false;
            return IsPresent;
        }

        private void buttonLinkedServer_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormLinkedServer f = new DiversityWorkbench.Forms.FormLinkedServer();
            f.setHelp("Linked server");
            f.ShowDialog();
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
        }

        #region BacklinkModuleUpdate

        private void buttonUpdateBacklinkModule_Click(object sender, EventArgs e)
        {
            if (this.treeViewConnections.SelectedNode.Tag != null)
            {
                if (this.treeViewConnections.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.ServerConnection))
                {
                    DiversityWorkbench.ServerConnection S = (DiversityWorkbench.ServerConnection)this.treeViewConnections.SelectedNode.Tag;
                    //string Server = S.DatabaseServer + ":" + S.DatabaseServerPort.ToString();
                    bool DbIsUpdated = DiversityWorkbench.Settings.BacklinkUpdatedDatabases().ContainsKey(DiversityWorkbench.Settings.ServerConnection.DatabaseKey)
                        && DiversityWorkbench.Settings.BacklinkUpdatedDatabases()[DiversityWorkbench.Settings.ServerConnection.DatabaseKey].ContainsKey(S.ModuleName)
                        && DiversityWorkbench.Settings.BacklinkUpdatedDatabases()[DiversityWorkbench.Settings.ServerConnection.DatabaseKey][S.ModuleName].Contains(S.DisplayText);
                    if (DbIsUpdated)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Should changes in the current database leave the database " + S.DatabaseName + " unchanged?", "No forwarding of Updates?", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                        {
                            DiversityWorkbench.Settings.BacklinkDatabaseRemove(DiversityWorkbench.Settings.ServerConnection.DatabaseKey, S.ModuleName, S.DisplayText);
                            DiversityWorkbench.Settings.BacklinksSave();
                            S.BackLinkIsUpdated = false;
                            this.treeViewConnections.SelectedNode.BackColor = System.Drawing.Color.White;
                        }
                    }
                    else
                    {
                        if (System.Windows.Forms.MessageBox.Show("Should changes in the current database be forwarded to the database " + S.DatabaseName, "Forward Updates?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            DiversityWorkbench.Settings.BacklinkDatabaseAdd(DiversityWorkbench.Settings.ServerConnection.DatabaseKey, S.ModuleName, S.DisplayText);
                            DiversityWorkbench.Settings.BacklinksSave();
                            S.BackLinkIsUpdated = true;
                            this.treeViewConnections.SelectedNode.BackColor = System.Drawing.Color.LightGreen;
                        }
                    }
                }
                else if (this.treeViewConnections.SelectedNode.Tag.GetType().BaseType == typeof(DiversityWorkbench.WorkbenchUnit))
                {
                    DiversityWorkbench.WorkbenchUnit workbenchUnit = (DiversityWorkbench.WorkbenchUnit)this.treeViewConnections.SelectedNode.Tag;
                    if (System.Windows.Forms.MessageBox.Show("Should changes in the current database be forwarded to all available " + workbenchUnit.ServerConnection.ModuleName + " databases", "Forward updates?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        foreach (System.Windows.Forms.TreeNode treeNode in this.treeViewConnections.SelectedNode.Nodes)
                        {
                            if (treeNode.Tag != null && treeNode.Tag.GetType() == typeof(DiversityWorkbench.ServerConnection))
                            {
                                DiversityWorkbench.ServerConnection S = (DiversityWorkbench.ServerConnection)treeNode.Tag;
                                if (S.LinkedServer.Length > 0)
                                    continue;
                                DiversityWorkbench.Settings.BacklinkDatabaseAdd(DiversityWorkbench.Settings.ServerConnection.DatabaseKey, S.ModuleName, S.DisplayText);
                                S.BackLinkIsUpdated = true;
                                treeNode.BackColor = System.Drawing.Color.LightGreen;
                            }
                            DiversityWorkbench.Settings.BacklinksSave();
                        }
                    }
                }
            }
        }

        #region Context menu
        private void showSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Display = "Updated Databases:\r\n";
            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> KV in DiversityWorkbench.Settings.BacklinkUpdatedDatabases())
            {
                Display += "\r\nSource: " + KV.Key + "\r\n";
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> kv in KV.Value)
                {
                    Display += "\r\n\tModule " + kv.Key + "\r\n";
                    foreach (string DB in kv.Value)
                        Display += "\t\t" + DB + "\r\n";
                }
            }
            System.Windows.Forms.MessageBox.Show(Display, "Update settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void resetSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.BacklinkUpdatedDatabases_Reset();
            this.initTree();
        }

        #endregion

        #endregion

        #endregion

    }

}
