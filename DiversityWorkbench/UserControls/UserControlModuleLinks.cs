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
    public struct OptionalRestriction
    {
        public bool Selected;
        public string DisplayText;
        public string Restriction;
        public OptionalRestriction(bool IsSelected, string Display, string SQL)
        {
            this.Selected = IsSelected;
            this.DisplayText = Display;
            this.Restriction = SQL;
        }
    }

    /// <summary>
    /// User control for the scan of back links in modules on a current dataset
    /// </summary>
    public partial class UserControlModuleLinks : UserControl
    {

        #region Parameter

        private DiversityWorkbench.WorkbenchUnit _WU;
        private string _Domain = "";
        private DiversityWorkbench.WorkbenchUnit.ModuleType _ModuleType;
        private string _Table;
        private string _Column;
        private int _MaxHeight = 160;
        private System.Collections.Generic.Dictionary<string, OptionalRestriction> _Restrictions;
        private string _ID = "";

        public DiversityWorkbench.WorkbenchUnitUserInterface _I;

        private bool _CheckboxScanModuleVisible = false;

        #endregion

        #region Construction

        public UserControlModuleLinks()
        {
            InitializeComponent();
        }

        #endregion

        #region Interface

        // DiversityWorkbench.UserControls.UserControlModuleLinksSettings will be replaced by ScannedModules in WorkbenchSettings.settings
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">The type of the module that should be scanned</param>
        /// <param name="I">The calling form providing the information of the calling module</param>
        /// <param name="Table">The name of the table containing the links to the current module</param>
        /// <param name="Column">The name of the column containing the links to the current module</param>
        /// <param name="Domain">The name of the table of the domain within the module</param>
        public void SetModuleParameter(DiversityWorkbench.WorkbenchUnit.ModuleType Type, DiversityWorkbench.WorkbenchUnitUserInterface I, string Table, string Column, string Domain = "")
        {
            this._ModuleType = Type;
            this._I = I;
            this._Table = Table;
            this._Column = Column;
            this._Domain = Domain;

            this.checkBoxScanModule.Checked = DiversityWorkbench.Settings.ScannedModuleIsScanned(this._ModuleType);
            this.textBoxModulePath.Text = DiversityWorkbench.Settings.ScannedModulePath(this._ModuleType);

            switch (Type)
            {
                case WorkbenchUnit.ModuleType.Agents:
                    DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = A;
                    break;
                case WorkbenchUnit.ModuleType.Collection:
                    DiversityWorkbench.CollectionSpecimen C = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = C;
                    break;
                //case WorkbenchUnit.ModuleType.CollectionProject:
                //    DiversityWorkbench.CollectionProject CP = new DiversityWorkbench.CollectionProject(DiversityWorkbench.Settings.ServerConnection);
                //    this._WU = CP;
                //    break;
                case WorkbenchUnit.ModuleType.Descriptions:
                    DiversityWorkbench.Description D = new DiversityWorkbench.Description(DiversityWorkbench.Settings.ServerConnection);
                    this._WU = D;
                    this.buttonCreateEntry.Text = "Create a description for the current entry";
                    this.buttonCreateEntry.Visible = true;
                    this.panelModulePath.Visible = true;
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

            this.checkBoxScanModule.Text += this._WU.ServerConnection.ModuleName;
            this.labelNothingFound.Text = this.labelNothingFound.Text.Replace("module", "module Diversity" + Type.ToString());
            this.groupBoxData.Text = "Links from Diversity" + Type.ToString();
            //this.setHeight();
        }

        public void SetOptionalRestrictions(System.Collections.Generic.Dictionary<string, OptionalRestriction> Restrictions)
        {
            this._Restrictions = Restrictions;
            foreach (System.Collections.Generic.KeyValuePair<string, OptionalRestriction> KV in Restrictions)
            {
                System.Windows.Forms.CheckBox CB = new CheckBox();
                CB.Checked = KV.Value.Selected;
                CB.Text = KV.Value.DisplayText;
                CB.Tag = KV.Key;
                CB.Click += new EventHandler(this.SetRestriction);
                this.panelOptionalRestrictions.Controls.Add(CB);
                CB.Dock = DockStyle.Top;
                CB.BringToFront();
                this.panelOptionalRestrictions.Height += 20;
            }
        }

        private void SetRestriction(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox CB = (System.Windows.Forms.CheckBox)sender;
            string RK = CB.Tag.ToString();
            OptionalRestriction R = this._Restrictions[RK];
            R.Selected = CB.Checked;
            this._Restrictions[RK] = R;
            if (this._ID.Length > 0 && this.checkBoxScanModule.Checked)
                this.SearchForModuleLinks(this._ID);
        }

        public void setHeight()
        {
            if (this._CheckboxScanModuleVisible)
            {
                this.splitContainerMain.Panel1Collapsed = false;
                this.checkBoxScanModule.Dock = DockStyle.Top;
                if (this.checkBoxScanModule.Checked)
                {
                    this.splitContainerMain.Panel2Collapsed = false;
                    this.Height = this._MaxHeight;
                    this._I.SetHeight(this._MaxHeight);
                }
                else
                {
                    this.splitContainerMain.Panel2Collapsed = true;
                    this.Height = 26;
                    this._I.SetHeight(26);
                }
                this.splitContainerMain.SplitterDistance = 30;
            }
            else
            {
                this.splitContainerMain.Panel1Collapsed = true;
                if (this.listBoxModuleLinks.DataSource != null)
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

        public bool SearchForModuleLinks(string ID)
        {
            if (this._WU == null || this._WU.DatabaseList().Count == 0)
                return false;

            this._ID = ID;
            string SQL = "";
            string Module = "";
            switch (this._ModuleType)
            {
                case WorkbenchUnit.ModuleType.Agents:
                    SQL = "";
                    break;
                case WorkbenchUnit.ModuleType.Collection:
                    switch (this._I.GetModuleType())
                    {
                        case WorkbenchUnit.ModuleType.TaxonNames:
                            SQL = "SELECT DISTINCT B.BaseURL + CAST(T.CollectionSpecimenID AS VARCHAR) AS URI " +
                                "FROM [dbo]." + this._Table + " AS T, [dbo].ViewBaseURL AS B WHERE T." + this._Column + " = '" + DiversityWorkbench.Settings.ServerConnection.BaseURL + ID + "'";
                            break;
                        case WorkbenchUnit.ModuleType.Projects:
                            SQL = "SELECT DISTINCT T.ProjectURI AS URI, '##Connection##' AS ConnectionString, '##Display##' AS DisplayText " +
                                "FROM [dbo].[" + this._Table + "] AS T INNER JOIN CollectionProject AS C ON T.ProjectID = C.ProjectID WHERE T." + this._Column + " = '" + DiversityWorkbench.Settings.ServerConnection.BaseURL + ID + "'";
                            break;
                    }
                    Module = "DiversityCollection";
                    break;
                case WorkbenchUnit.ModuleType.Descriptions:
                    // Test existence of ViewBaseURL
                    SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS v where v.TABLE_NAME = 'ViewBaseURL'";
                    switch (this._I.GetModuleType())
                    {
                        case WorkbenchUnit.ModuleType.Collection:
                            SQL = "SELECT DISTINCT B.BaseURL + CAST(T.description_id AS VARCHAR) AS URI " +
                                "FROM [dbo]." + this._Table + " AS T, [dbo].ViewBaseURL AS B WHERE (T.[type] = 'Specimen' OR T.[type] = 'Observation') " +
                                "AND " + this._Column + " = '" + DiversityWorkbench.Settings.ServerConnection.BaseURL + ID + "'";
                            break;
                        case WorkbenchUnit.ModuleType.TaxonNames:
                            SQL = "SELECT DISTINCT B.BaseURL + CAST(T.description_id AS VARCHAR) AS URI " +
                                "FROM [dbo]." + this._Table + " AS T, [dbo].ViewBaseURL AS B WHERE (T.[type] = 'TaxonName') " +
                                "AND T." + this._Column + " = '" + DiversityWorkbench.Settings.ServerConnection.BaseURL + ID + "'";
                            break;
                    }
                    Module = "DiversityDescriptions";
                    break;
                case WorkbenchUnit.ModuleType.Exsiccatae:
                    SQL = "";
                    break;
                case WorkbenchUnit.ModuleType.Gazetteer:
                    SQL = "";
                    break;
                case WorkbenchUnit.ModuleType.Projects:
                    SQL = "";
                    break;
                case WorkbenchUnit.ModuleType.References:
                    SQL = "";
                    break;
                case WorkbenchUnit.ModuleType.SamplingPlots:
                    SQL = "";
                    break;
                case WorkbenchUnit.ModuleType.ScientificTerms:
                    switch (this._I.GetModuleType())
                    {
                        case WorkbenchUnit.ModuleType.Projects:
                            SQL = "SELECT DISTINCT T.ProjectURI AS URI " +
                                "FROM [dbo].[" + this._Table + "] AS T INNER JOIN [dbo].[Term] AS C ON T.TerminologyID = C.TerminologyID WHERE T." + this._Column + " = '" + DiversityWorkbench.Settings.ServerConnection.BaseURL + ID + "'";
                            break;
                    }
                    Module = "DiversityScientificTerms";

                    break;
                case WorkbenchUnit.ModuleType.TaxonNames:
                    switch (this._I.GetModuleType())
                    {
                        case WorkbenchUnit.ModuleType.Projects:
                            switch (this._Domain)
                            {
                                case "ProjectProxy":
                                    SQL = "SELECT DISTINCT T.ProjectURI AS URI " +
                                        "FROM [dbo].[" + this._Table + "] AS T INNER JOIN TaxonNameProject AS C ON T.ProjectID = C.ProjectID WHERE T." + this._Column + " = '" + DiversityWorkbench.Settings.ServerConnection.BaseURL + ID + "'";
                                    break;
                                case "TaxonNameListProjectProxy":
                                    SQL = "SELECT DISTINCT T.ProjectURI AS URI " +
                                        "FROM [dbo].[" + this._Table + "] AS T INNER JOIN TaxonNameList AS C ON T.ProjectID = C.ProjectID WHERE T." + this._Column + " = '" + DiversityWorkbench.Settings.ServerConnection.BaseURL + ID + "'";
                                    break;
                            }
                            break;
                    }
                    Module = "DiversityTaxonNames";
                    break;
            }
            if (this._Restrictions != null && this._Restrictions.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.UserControls.OptionalRestriction> OR in this._Restrictions)
                {
                    if (OR.Value.Selected)
                    {
                        SQL += " AND " + OR.Value.Restriction;
                    }
                }
            }
            if (SQL.Length > 0)
            {
                System.Data.DataTable dt = new DataTable();

                System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> ServerConnections = new Dictionary<string, DiversityWorkbench.ServerConnection>();
                if (!DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey(Module))
                {
                    switch (Module)
                    {
                        case "DiversityAgents":
                            DiversityWorkbench.Agent A = new Agent(DiversityWorkbench.Settings.ServerConnection);
                            break;
                        case "DiversityCollection":
                            DiversityWorkbench.CollectionSpecimen C = new CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                            break;
                        case "DiversityDescriptions":
                            DiversityWorkbench.Description D = new Description(DiversityWorkbench.Settings.ServerConnection);
                            break;
                        case "DiversityScientificTerms":
                            DiversityWorkbench.ScientificTerm ST = new ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                            break;
                    }
                }

                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[Module].ServerConnections())
                {
                    ServerConnections.Add(KV.Value.DisplayText, KV.Value);
                }
                if (ServerConnections.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> SC in ServerConnections)
                    {
                        // test validity
                        string SqlValid = "SELECT MIN([BaseURL]) FROM [dbo].[ViewBaseURL]";
                        if (SC.Value.LinkedServer.Length > 0)
                        {
                            SQL = SQL.Replace(" [dbo].", " [" + SC.Value.LinkedServer + "].[" + SC.Value.DatabaseName + "].[dbo].");
                            SqlValid = SqlValid.Replace(" [dbo].", " [" + SC.Value.LinkedServer + "].[" + SC.Value.DatabaseName + "].[dbo].");
                        }
                        if (SQL.IndexOf("##Connection##") > -1)
                            SQL = SQL.Replace("##Connection##", SC.Value.ConnectionString);
                        if (SQL.IndexOf("##Display##") > -1)
                            SQL = SQL.Replace("##Display##", SC.Value.DatabaseName + " on " + SC.Value.DatabaseServer);
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(SC.Value.ConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlValid, con);
                        bool Valid = true;
                        try
                        {
                            con.Open();
                            string BaseURL = C.ExecuteScalar()?.ToString() ?? string.Empty;
                            if (BaseURL == string.Empty)
                                Valid = false;
                            con.Close();
                            con.Dispose();
                        }
                        catch (System.Exception ex) { Valid = false; }
                        if (Valid)
                        {
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.Value.ConnectionString);
                            try
                            {
                                ad.Fill(dt);
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "SQL: " + SQL + "; ConnectionString: " + SC.Value.ConnectionString);
                            }
                            ad.Dispose();
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    this.listBoxModuleLinks.DataSource = dt;
                    if (this._ModuleType == WorkbenchUnit.ModuleType.Collection)
                    {
                        this.listBoxModuleLinks.DisplayMember = "DisplayText";
                        this.listBoxModuleLinks.ValueMember = "ConnectionString";
                    }
                    else
                    {
                        this.listBoxModuleLinks.DisplayMember = "URI";
                        this.listBoxModuleLinks.ValueMember = "URI";
                    }
                    this.labelNothingFound.Visible = false;
                    this.splitContainerData.Enabled = true;
                    this.buttonOpenModule.Enabled = true;
                    this.labelCount.Text = dt.Rows.Count.ToString();
                }
                else
                {
                    this.listBoxModuleLinks.DataSource = null;
                    this.listBoxModuleLinks.Visible = true;
                    this.labelNothingFound.Visible = true;
                    this.splitContainerData.Enabled = false;
                    this.buttonOpenModule.Enabled = false;
                    this.labelCount.Text = "";
                }
                this.buttonSetModulePath.Enabled = true;
            }
            return true;
        }

        public void ShowCheckboxForScanning(bool ShowCheckbox)
        {
            this._CheckboxScanModuleVisible = ShowCheckbox;
            if (this._CheckboxScanModuleVisible)
                this.splitContainerMain.Panel1Collapsed = false;
            else
                this.splitContainerMain.Panel1Collapsed = true;
        }

        public void ScanModul(bool DoScan)
        {

            DiversityWorkbench.Settings.ScannedModuleDoScan(this._ModuleType, DoScan);
            DiversityWorkbench.Settings.ScannedModuleSave();

            //switch (this._ModuleType)
            //{
            //    case WorkbenchUnit.ModuleType.Agents:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityAgents = DoScan;
            //        break;
            //    case WorkbenchUnit.ModuleType.Collection:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityCollection = DoScan;
            //        break;
            //    case WorkbenchUnit.ModuleType.Descriptions:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityDescriptions = DoScan;
            //        break;
            //    case WorkbenchUnit.ModuleType.Exsiccatae:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityExsiccatae = DoScan;
            //        break;
            //    case WorkbenchUnit.ModuleType.Gazetteer:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityGazetteer = DoScan;
            //        break;
            //    case WorkbenchUnit.ModuleType.Projects:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityProjects = DoScan;
            //        break;
            //    case WorkbenchUnit.ModuleType.References:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityReferences = DoScan;
            //        break;
            //    case WorkbenchUnit.ModuleType.SamplingPlots:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversitySamplingPlots = DoScan;
            //        break;
            //    case WorkbenchUnit.ModuleType.ScientificTerms:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityScientificTerms = DoScan;
            //        break;
            //    case WorkbenchUnit.ModuleType.TaxonNames:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityTaxonNames = DoScan;
            //        break;
            //}
            //DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.Save();

        }

        #endregion

        #region private events

        private void checkBoxScanModule_Click(object sender, EventArgs e)
        {
            bool IsScanned = false;
            IsScanned = DiversityWorkbench.Settings.ScannedModuleIsScanned(this._ModuleType);
            DiversityWorkbench.Settings.ScannedModuleDoScan(this._ModuleType, !IsScanned);
            this.checkBoxScanModule.Checked = DiversityWorkbench.Settings.ScannedModuleIsScanned(this._ModuleType);
            DiversityWorkbench.Settings.ScannedModuleSave();

            //switch (this._ModuleType)
            //{
            //    case WorkbenchUnit.ModuleType.Agents:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityAgents = !DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityAgents;
            //        this.checkBoxScanModule.Checked = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityAgents;
            //        break;
            //    case WorkbenchUnit.ModuleType.Collection:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityCollection = !DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityCollection;
            //        this.checkBoxScanModule.Checked = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityCollection;
            //        break;
            //    case WorkbenchUnit.ModuleType.Descriptions:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityDescriptions = !DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityDescriptions;
            //        this.checkBoxScanModule.Checked = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityDescriptions;
            //        break;
            //    case WorkbenchUnit.ModuleType.Exsiccatae:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityExsiccatae = !DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityExsiccatae;
            //        this.checkBoxScanModule.Checked = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityExsiccatae;
            //        break;
            //    case WorkbenchUnit.ModuleType.Gazetteer:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityGazetteer = !DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityGazetteer;
            //        this.checkBoxScanModule.Checked = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityGazetteer;
            //        break;
            //    case WorkbenchUnit.ModuleType.Projects:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityProjects = !DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityProjects;
            //        this.checkBoxScanModule.Checked = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityProjects;
            //        break;
            //    case WorkbenchUnit.ModuleType.References:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityReferences = !DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityReferences;
            //        this.checkBoxScanModule.Checked = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityReferences;
            //        break;
            //    case WorkbenchUnit.ModuleType.SamplingPlots:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversitySamplingPlots = !DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversitySamplingPlots;
            //        this.checkBoxScanModule.Checked = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversitySamplingPlots;
            //        break;
            //    case WorkbenchUnit.ModuleType.ScientificTerms:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityScientificTerms = !DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityScientificTerms;
            //        this.checkBoxScanModule.Checked = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityScientificTerms;
            //        break;
            //    case WorkbenchUnit.ModuleType.TaxonNames:
            //        DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityTaxonNames = !DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityTaxonNames;
            //        this.checkBoxScanModule.Checked = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.ScanDiversityTaxonNames;
            //        break;
            //}
            //DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.Save();

            this.setHeight();
        }

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
                    DiversityWorkbench.Settings.ScannedModuleDoScan(this._ModuleType, true, f.FullName);
                    DiversityWorkbench.Settings.ScannedModuleSave();

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
            if (this.listBoxModuleLinks.DataSource == null)
                return;
            if (this.listBoxModuleLinks.SelectedValue == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select an item from the list");
                return;
            }
            string Path = DiversityWorkbench.Settings.ScannedModulePath(this._ModuleType);// "";

            //switch (this._ModuleType)
            //{
            //    case WorkbenchUnit.ModuleType.Agents:
            //        Path = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityAgentsPath;
            //        break;
            //    case WorkbenchUnit.ModuleType.Collection:
            //        Path = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityCollectionPath;
            //        break;
            //    case WorkbenchUnit.ModuleType.Descriptions:
            //        Path = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityDescriptionsPath;
            //        break;
            //    case WorkbenchUnit.ModuleType.Exsiccatae:
            //        Path = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityExsiccataePath;
            //        break;
            //    case WorkbenchUnit.ModuleType.Gazetteer:
            //        Path = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityGazetteerPath;
            //        break;
            //    case WorkbenchUnit.ModuleType.Projects:
            //        Path = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityProjectsPath;
            //        break;
            //    case WorkbenchUnit.ModuleType.References:
            //        Path = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityReferencesPath;
            //        break;
            //    case WorkbenchUnit.ModuleType.SamplingPlots:
            //        Path = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversitySamplingPlotsPath;
            //        break;
            //    case WorkbenchUnit.ModuleType.ScientificTerms:
            //        Path = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityScientificTermsPath;
            //        break;
            //    case WorkbenchUnit.ModuleType.TaxonNames:
            //        Path = DiversityWorkbench.UserControls.UserControlModuleLinksSettings.Default.DiversityTaxonNamesPath;
            //        break;
            //}

            if (Path.Length > 0)
            {
                string URI = this.listBoxModuleLinks.SelectedValue.ToString();
                string Database = DiversityWorkbench.WorkbenchUnit.getDatabaseNameFromURI(URI);
                string Server = DiversityWorkbench.Settings.DatabaseServer;
                string Port = DiversityWorkbench.Settings.DatabasePort.ToString();
                DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URI);
                if (SC != null)
                {
                    if (SC.ConnectionIsValid)
                    {
                        Server = SC.DatabaseServer;
                        Port = SC.DatabaseServerPort.ToString();
                    }
                }
                string Arguments = "singleitem " + Server + " " + Port + " " + Database + " " + URI;
                if (!DiversityWorkbench.Settings.IsTrustedConnection)
                    Arguments += " " + DiversityWorkbench.Settings.DatabaseUser + " " + DiversityWorkbench.Settings.Password;
                System.Diagnostics.Process.Start(Path, Arguments);
            }
            else
                System.Windows.Forms.MessageBox.Show("Please set the path to the application");
        }

        private void listBoxModuleLinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Control C in this.panelUnitValues.Controls)
                C.Dispose();
            this.panelUnitValues.Controls.Clear();
            string Key = "";
            if (this.listBoxModuleLinks.SelectedValue != null)
            {
                System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
                if (this._Domain.Length > 0)
                {
                    string URI = "";
                    string ConnectionString = "";
                    string DisplayText = "";
                    if (this.listBoxModuleLinks.SelectedValue.GetType() == typeof(System.Data.DataRowView))
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxModuleLinks.SelectedValue;
                        if (R.Row.Table.Columns.Contains("DisplayText"))
                            DisplayText = R["DisplayText"].ToString();
                        if (R.Row.Table.Columns.Contains("ConnectionString"))
                            ConnectionString = R["ConnectionString"].ToString();
                        if (R.Row.Table.Columns.Contains("URI"))
                            URI = R["URI"].ToString();
                        else
                            URI = R[0].ToString();
                    }
                    else if (this.listBoxModuleLinks.SelectedItem.GetType() == typeof(System.Data.DataRowView))
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxModuleLinks.SelectedItem;
                        if (R.Row.Table.Columns.Contains("DisplayText"))
                            DisplayText = R["DisplayText"].ToString();
                        if (R.Row.Table.Columns.Contains("ConnectionString"))
                            ConnectionString = R["ConnectionString"].ToString();
                        if (R.Row.Table.Columns.Contains("URI"))
                            URI = R["URI"].ToString();
                        else
                            URI = R[0].ToString();
                    }
                    else
                        URI = this.listBoxModuleLinks.SelectedValue.ToString();
                    Values = this._WU.BackLinkValues(this._Domain, ConnectionString, URI);

                    //string sID = URI.Substring(URI.LastIndexOf("/") + 1);
                    //int ID;
                    //if (int.TryParse(sID, out ID))
                    //    Values = this._WU.UnitValues(this._Domain, ID);
                }
                else
                {
                    if (this.listBoxModuleLinks.SelectedValue.GetType() == typeof(System.Data.DataRowView))
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxModuleLinks.SelectedValue;
                        Values = this._WU.UnitValues(R[0].ToString());
                    }
                    else
                        Values = this._WU.UnitValues(this.listBoxModuleLinks.SelectedValue.ToString());
                }
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
            }
        }

        private DiversityWorkbench.ServerConnection _SC_createEntry;
        private void buttonCreateEntry_Click(object sender, EventArgs e)
        {
            string Path = DiversityWorkbench.Settings.ScannedModulePath(this._ModuleType);// "";
            if (Path.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please set the path to the application");
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

        #endregion

    }
}
