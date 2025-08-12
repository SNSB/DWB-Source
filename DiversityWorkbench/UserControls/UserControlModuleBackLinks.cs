using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlModuleBackLinks : UserControl
    {

        #region Parameter

        private DiversityWorkbench.WorkbenchUnit _WU;
        private DiversityWorkbench.WorkbenchUnit.ModuleType _BackLinkModuleType;
        private DiversityWorkbench.WorkbenchUnit.ModuleType _CallingModuleType;
        private int _MaxHeight = 160;
        private string _ID = "";
        private bool _EnableCreate = false;
        private bool _EnableHtml = false;

        public DiversityWorkbench.WorkbenchUnitUserInterface _I;

        private bool _CheckboxScanModuleVisible = false;

        private List<TreeNode> _UnaccessableNodes;
        #endregion

        #region Construction

        public UserControlModuleBackLinks()
        {
            InitializeComponent();
            this.panelModulePath.Visible = false;
            this.splitContainerData.Panel2Collapsed = true;
            _UnaccessableNodes = new List<TreeNode>();
        }

        #endregion

        #region Interface

        // Markus 8.5.23: Ermöglichen der Kontrolle ob richtiger typ gesetzt ist
        public DiversityWorkbench.WorkbenchUnit.ModuleType BackLinkModuleType { get { return _BackLinkModuleType; } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="BackLinkType">The type of the module that should be scanned</param>
        /// <param name="I">The calling form providing the information of the calling module</param>
        /// <param name="EnableCreate">If the creation of datasets in the backlinked module should be enabled</param>
        public void SetModuleParameter(DiversityWorkbench.WorkbenchUnit.ModuleType BackLinkType, DiversityWorkbench.WorkbenchUnitUserInterface I, bool EnableCreate = false)
        {
            this._BackLinkModuleType = BackLinkType;
            this._I = I;
            this._CallingModuleType = this._I.GetModuleType();
            if (EnableCreate)
            {
                this._EnableCreate = EnableCreate;
            }
            this.groupBoxData.Visible = this._EnableCreate;
            //if (this._EnableCreate)
            //    this.buttonCreateEntry.Visible = true;

            this.textBoxModulePath.Text = DiversityWorkbench.Settings.ScannedModulePath(this._BackLinkModuleType);

            switch (BackLinkType)
            {
                case WorkbenchUnit.ModuleType.Agents:
                    DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = A;
                    break;
                case WorkbenchUnit.ModuleType.Collection:
                    DiversityWorkbench.CollectionSpecimen C = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = C;
                    break;
                case WorkbenchUnit.ModuleType.Descriptions:
                    DiversityWorkbench.Description D = new DiversityWorkbench.Description(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = D;
                    break;
                case WorkbenchUnit.ModuleType.Exsiccatae:
                    DiversityWorkbench.Exsiccate E = new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = E;
                    break;
                case WorkbenchUnit.ModuleType.Gazetteer:
                    DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = G;
                    break;
                case WorkbenchUnit.ModuleType.Projects:
                    DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = P;
                    break;
                case WorkbenchUnit.ModuleType.References:
                    DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = R;
                    break;
                case WorkbenchUnit.ModuleType.SamplingPlots:
                    DiversityWorkbench.SamplingPlot SP = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = SP;
                    break;
                case WorkbenchUnit.ModuleType.ScientificTerms:
                    DiversityWorkbench.ScientificTerm ST = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = ST;
                    break;
                case WorkbenchUnit.ModuleType.TaxonNames:
                    DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = T;
                    break;
            }

            // Toni 20201112: Set HTML enable
            this._EnableHtml = true; // this._WU.FeatureList.Contains(WorkbenchUnit.ClientFeature.HtmlUnitValues);
            this.buttonShowHtml.Visible = this._EnableHtml;
            // Toni 20201112: Unit values panel must be closed if control is re-initialized
            this.splitContainerData.Panel2Collapsed = true;

            //this.checkBoxScanModule.Text += this._WU.ServerConnection.ModuleName;
            this.labelNothingFound.Text = string.Format("No links as defined in the module Diversity{0} could be found", BackLinkType); // this.labelNothingFound.Text.Replace("module", "module Diversity" + BackLinkType.ToString());
            this.groupBoxData.Text = "Links from Diversity" + BackLinkType.ToString();
            //this.setHeight();
        }

        public void setHeight()
        {
            if (this._CheckboxScanModuleVisible)
            {
                //this.splitContainerMain.Panel1Collapsed = false;
                //this.checkBoxScanModule.Dock = DockStyle.Top;
                //if (this.checkBoxScanModule.Checked)
                //{
                //    this.splitContainerMain.Panel2Collapsed = false;
                //    this.Height = this._MaxHeight;
                //    this._I.SetHeight(this._MaxHeight);
                //}
                //else
                //{
                //    this.splitContainerMain.Panel2Collapsed = true;
                //    this.Height = 26;
                //    this._I.SetHeight(26);
                //}
                //this.splitContainerMain.SplitterDistance = 30;
            }
            else
            {

                if (this.treeViewLinks.Nodes.Count > 0)
                {
                    this.groupBoxData.Visible = true;
                    this.Height = (int)((float)100 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                }
                else
                {
                    this.groupBoxData.Visible = false;
                    this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(26);// (int)((float)26 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                }
            }
        }

        public void SearchForBackLinks(int ID, System.Collections.Generic.List<string> Restrictions = null)
        {
            string URI = DiversityWorkbench.Settings.ServerConnection.BaseURL + ID.ToString();
            this._ID = ID.ToString();
            this.SearchForBackLinks(URI, Restrictions);
        }

        /// <summary>
        /// Searching for back links in a module
        /// </summary>
        /// <param name="URI">The URL for the current dataset in the current module</param>
        /// <param name="Restrictions">An optional list of restrictions where T is the alias for the table containing the URL</param>
        public void SearchForBackLinks(string URI, System.Collections.Generic.List<string> Restrictions = null)
        {
            try
            {
                _UnaccessableNodes.Clear();
                this.treeViewLinks.Nodes.Clear();
                System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> L = null;
                this.panelUnitValues.Controls.Clear();
                DiversityWorkbench.WorkbenchUnit WBU = null;
                switch (this._BackLinkModuleType)
                {
                    case WorkbenchUnit.ModuleType.Agents:
                        WBU = new Agent(DiversityWorkbench.Settings.ServerConnection);
                        break;
                    case WorkbenchUnit.ModuleType.Collection:
                        WBU = new CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                        break;
                    case WorkbenchUnit.ModuleType.Descriptions:
                        WBU = new Description(DiversityWorkbench.Settings.ServerConnection);
                        break;
                    case WorkbenchUnit.ModuleType.Exsiccatae:
                        WBU = new Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                        break;
                    case WorkbenchUnit.ModuleType.Gazetteer:
                        WBU = new Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                        break;
                    case WorkbenchUnit.ModuleType.Projects:
                        WBU = new Project(DiversityWorkbench.Settings.ServerConnection);
                        break;
                    case WorkbenchUnit.ModuleType.References:
                        WBU = new Reference(DiversityWorkbench.Settings.ServerConnection);
                        break;
                    case WorkbenchUnit.ModuleType.SamplingPlots:
                        WBU = new SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                        break;
                    case WorkbenchUnit.ModuleType.ScientificTerms:
                        WBU = new ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                        break;
                    case WorkbenchUnit.ModuleType.TaxonNames:
                        WBU = new TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        break;
                    default:
                        break;
                }
                if (WBU != null)
                {
                    this.treeViewLinks.ImageList = WBU.BackLinkImages(this._CallingModuleType);
                    L = WBU.BackLinkServerConnectionDomains(URI, this._CallingModuleType, this._EnableCreate, Restrictions);
                }
                if (L != null)
                {
                    if (L.Count == 0)
                    {
                        this.labelNothingFound.Visible = true;
                        if (!this._EnableCreate)
                            this.groupBoxData.Visible = false;
                    }
                    else
                    {
                        this.groupBoxData.Visible = true;
                        this.labelNothingFound.Visible = false;
                        foreach (System.Collections.Generic.KeyValuePair<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> KV in L)
                        {
                            System.Windows.Forms.TreeNode TDB = new TreeNode(KV.Key.DisplayText, 0, 0);
                            TDB.Tag = KV.Key;
                            this.treeViewLinks.Nodes.Add(TDB);
                            foreach (BackLinkDomain D in KV.Value)
                            {
                                if (D.DtItems.Rows.Count > 0)
                                {
                                    bool evalAccess = D.DtItems.Columns.Contains("AccessCount");
                                    System.Windows.Forms.TreeNode Tdomain = new TreeNode(D.DisplayText, D.ImageKey, D.ImageKey);
                                    TDB.Nodes.Add(Tdomain);
                                    foreach (System.Data.DataRow R in D.DtItems.Rows)
                                    {
                                        System.Windows.Forms.TreeNode Titem = new TreeNode(R["DisplayText"].ToString(), 1, 1);
                                        Titem.Tag = int.Parse(R["ID"].ToString());
                                        Tdomain.Nodes.Add(Titem);
                                        if (evalAccess && R["AccessCount"].ToString() == "0")
                                            _UnaccessableNodes.Add(Titem);
                                    }
                                }
                            }
                        }
                    }
                }

                this.treeViewLinks.ExpandAll();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region private events

        private void buttonSetModulePath_Click(object sender, EventArgs e)
        {
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            this.openFileDialog.Filter = "Program Files|*.exe";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxModulePath.Text = f.FullName;
                    DiversityWorkbench.Settings.ScannedModuleDoScan(this._BackLinkModuleType, true, f.FullName);
                    DiversityWorkbench.Settings.ScannedModuleSave();
                    //this.buttonSetModulePath.Visible = false;
                    //this.textBoxModulePath.Visible = false;
                    //switch (this._ModuleType)
                    //{
                    //    case WorkbenchUnit.ModuleType.Agents:
                    //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityAgentsPath = f.FullName;
                    //        break;
                    //    case WorkbenchUnit.ModuleType.Collection:
                    //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityCollectionPath = f.FullName;
                    //        break;
                    //    case WorkbenchUnit.ModuleType.Descriptions:
                    //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityDescriptionsPath = f.FullName;
                    //        break;
                    //    case WorkbenchUnit.ModuleType.Exsiccatae:
                    //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityExsiccataePath = f.FullName;
                    //        break;
                    //    case WorkbenchUnit.ModuleType.Gazetteer:
                    //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityGazetteerPath = f.FullName;
                    //        break;
                    //    case WorkbenchUnit.ModuleType.Projects:
                    //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityProjectsPath = f.FullName;
                    //        break;
                    //    case WorkbenchUnit.ModuleType.References:
                    //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityReferencesPath = f.FullName;
                    //        break;
                    //    case WorkbenchUnit.ModuleType.SamplingPlots:
                    //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversitySamplingPlotsPath = f.FullName;
                    //        break;
                    //    case WorkbenchUnit.ModuleType.ScientificTerms:
                    //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityScientificTermsPath = f.FullName;
                    //        break;
                    //    case WorkbenchUnit.ModuleType.TaxonNames:
                    //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityTaxonNamesPath = f.FullName;
                    //        break;
                    //}
                    //DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonOpenModule_Click(object sender, EventArgs e)
        {
            try
            {
                string Path = DiversityWorkbench.Settings.ScannedModulePath(this._BackLinkModuleType);// "";


                if (Path.Length > 0 && Path.EndsWith(".exe"))
                {
                    DiversityWorkbench.ServerConnection SC = (DiversityWorkbench.ServerConnection)this.treeViewLinks.SelectedNode.Parent.Parent.Tag;
                    string URI = SC.BaseURL + this.treeViewLinks.SelectedNode.Tag.ToString();// this.listBoxModuleLinks.SelectedValue.ToString();
                    string Database = SC.DatabaseName;
                    string Server = SC.DatabaseServer;
                    int Port = SC.DatabaseServerPort;
                    string Arguments = "singleitem " + Server + " " + Port.ToString() + " " + Database + " " + URI;
                    if (!SC.IsTrustedConnection)
                        Arguments += " " + DiversityWorkbench.Settings.DatabaseUser + " " + DiversityWorkbench.Settings.Password;
                    System.Diagnostics.Process.Start(Path, Arguments);
                    this.panelModulePath.Visible = false;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please set the path to the application");
                    this.panelModulePath.Visible = true;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonShowHtml_Click(object sender, EventArgs e)
        {
            if (this._EnableHtml)
            {
                try
                {
                    DiversityWorkbench.ServerConnection SC = (DiversityWorkbench.ServerConnection)this.treeViewLinks.SelectedNode.Parent.Parent.Tag;
                    string URI = SC.BaseURL + this.treeViewLinks.SelectedNode.Tag.ToString();
                    string html = _WU.HtmlUnitValues(_WU.UnitValues(URI));
                    //System.IO.FileInfo fi = new System.IO.FileInfo(WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\UnitValues.htm");
                    System.IO.FileInfo fi = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\DiversityWorkbench\UnitValuesBack.htm");
                    using (System.IO.StreamWriter TxtWriter = new System.IO.StreamWriter(fi.FullName, false, Encoding.Unicode))
                    {
                        TxtWriter.Write(html);
                        TxtWriter.Flush();
                        TxtWriter.Close();
                    }
                    DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(fi.FullName);
                    f.ShowExternal = true;
                    f.ShowDialog();
                    // System.Diagnostics.Process.Start(fi.FullName);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private DiversityWorkbench.ServerConnection _SC_createEntry;
        private void buttonCreateEntry_Click(object sender, EventArgs e)
        {
            string Path = DiversityWorkbench.Settings.ScannedModulePath(this._BackLinkModuleType);// "";
            try
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                if (!FI.Exists)
                    Path = "";
            }
            catch
            {
                Path = "";
                this.panelModulePath.Visible = true;
            }
            if (Path.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please set the path to the application");
                this.panelModulePath.Visible = true;
                return;
            }
            if (this._SC_createEntry == null)
            {
                System.Collections.Generic.List<string> L = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in this._WU.ServerConnections())
                    L.Add(KV.Key);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(L, "Select database", "Please select the database where the description should be created", true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this._SC_createEntry = this._WU.ServerConnections()[f.SelectedString];
                }
                else
                    return;
            }
            string Database = this._SC_createEntry.DatabaseName;
            string Server = this._SC_createEntry.DatabaseServer;
            string Port = this._SC_createEntry.DatabaseServerPort.ToString();
            string URI = DiversityWorkbench.Settings.ServerConnection.BaseURL + this._ID;
            if (Path.Length > 0)
            {
                string Arguments = "createitem " + Server + " " + Port + " " + Database + " " + URI;
                if (!DiversityWorkbench.Settings.IsTrustedConnection)
                    Arguments += " " + DiversityWorkbench.Settings.DatabaseUser + " " + DiversityWorkbench.Settings.Password;
                System.Diagnostics.Process.Start(Path, Arguments);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please set the path to the application");
            }
        }

        private void treeViewLinks_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.buttonCreateEntry.Visible = false;
            this.splitContainerData.Panel2Collapsed = true;
            if (this.treeViewLinks.SelectedNode.Tag != null)
            {
                if (this.treeViewLinks.SelectedNode.Tag.GetType() == typeof(int))
                {
                    ServerConnection SC = (ServerConnection)this.treeViewLinks.SelectedNode.Parent.Parent.Tag;
                    string URI = SC.BaseURL + this.treeViewLinks.SelectedNode.Tag.ToString();
                    this.setValuePanel(URI);
                    this.splitContainerData.Panel2Collapsed = false;
                    this.buttonOpenModule.Visible = !_UnaccessableNodes.Contains(this.treeViewLinks.SelectedNode);
                }
                else if (this.treeViewLinks.SelectedNode.Parent == null && this._EnableCreate)
                {
                    ServerConnection SC = (ServerConnection)this.treeViewLinks.SelectedNode.Tag;
                    this._SC_createEntry = SC;
                    this.buttonCreateEntry.Text = "Create a new entry in the database " + SC.DatabaseName + " on " + SC.DatabaseServer;
                    this.buttonCreateEntry.Visible = true;
                }
            }
            else
                this.splitContainerData.Panel2Collapsed = true;
        }

        private void setValuePanel(string URI)
        {
            this.panelUnitValues.SuspendLayout();
            this.panelUnitValues.Visible = false;
            this.splitContainerData.Panel2Collapsed = false;
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            Values = this._WU.UnitValues(URI);
            string Key = "";
            this.panelUnitValues.Controls.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
            {
                if (!P.Key.StartsWith("_"))
                {
                    if (Key != P.Key)
                    {
                        try
                        {
                            System.Windows.Forms.Label L = new Label();
                            L.Text = P.Key;
                            L.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Bold);
                            L.ForeColor = System.Drawing.Color.Gray;
                            L.Dock = DockStyle.Top;
                            L.TextAlign = ContentAlignment.BottomLeft;
                            L.Height = 13;
                            this.panelUnitValues.Controls.Add(L);
                            L.BringToFront();

                            System.Windows.Forms.TextBox T = new TextBox();
                            T.Name = "textBox" + P.Key;
                            T.Dock = DockStyle.Top;
                            T.Height = (int)(18 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                            T.ReadOnly = true;
                            T.TextAlign = HorizontalAlignment.Center;
                            T.BorderStyle = BorderStyle.None;
                            T.Multiline = true;
                            T.ScrollBars = ScrollBars.Vertical;
                            T.Text = P.Value;
                            this.panelUnitValues.Controls.Add(T);
                            T.BringToFront();
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    Key = P.Key;
                }
            }
            this.panelUnitValues.ResumeLayout();
            this.panelUnitValues.Visible = true;
        }

        #endregion

    }
}
