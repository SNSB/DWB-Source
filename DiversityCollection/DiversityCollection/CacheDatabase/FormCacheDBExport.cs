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
    /// <summary>
    /// Form providing the interface to transfer data to a cache database
    /// First a database must be selected (if there are more than 1)
    /// 
    /// Collection Data:
    /// For the transfer of the collection data a project must be selected
    ///     For every table that is transferred, there is an option, to compare the current content in the cache database with the data that should be transferred
    ///     
    /// Taxonomy:
    /// For the transfer of the taxonomy a taxon database must be selected
    /// 
    /// Interface:
    /// The final presentation after the conversion for a certain interface
    /// Data are refreshed after user request for a single project
    /// </summary>
    public partial class FormCacheDBExport : Form
    {
        #region Parameter

        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCacheDatabase;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterDiversityTaxonNames;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterDiversityTaxonNamesProjectSequence;
        private string _DiversityTaxonNamesConnectionString;
        private string _DiversityTaxonNamesDataSource;
        private string _DiversityTaxonNamesDataBase;

        private string _DatabaseVersion;

        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, DiversityCollection.CacheDatabase.UserControlTable>> _ProjectExportTables;

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Windows.Forms.DataGridView>>> _InterfaceDatagrids;
        
        #endregion

        #region Construction and Form

        public FormCacheDBExport()
        {
            InitializeComponent();
            this.initForm();
        }
        
        public void initForm()
        {
            this.initDatabases();
            this.helpProvider.HelpNamespace = System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm";
        }

        #endregion

        #region Database selection

        private void initDatabases()
        {
            try
            {
                string SQL = "SELECT DatabaseName, Server, Port, Version /*, ProjectsDatabaseName*/ FROM CacheDatabase";
                this._SqlDataAdapterCacheDatabase = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._SqlDataAdapterCacheDatabase.Fill(this.dataSetCacheDB.CacheDatabase);
                if (this.dataSetCacheDB.CacheDatabase.Rows.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("So far no cache database has been defined");
                    this.Close();
                }
                else if (this.dataSetCacheDB.CacheDatabase.Rows.Count == 1)
                {
                    this.splitContainerStart.Panel1Collapsed = true;
                    this.splitContainerStart.Panel2Collapsed = false;
                    this.setDatabase();
                }
                else
                {
                    this.splitContainerStart.Panel1Collapsed = false;
                    this.splitContainerStart.Panel2Collapsed = true;

                    if (this.listBoxSelectCacheDB.DataSource == null)
                    {
                        this.listBoxSelectCacheDB.DataSource = this.dataSetCacheDB.CacheDatabase;
                        this.listBoxSelectCacheDB.DisplayMember = "DatabaseName";
                        this.listBoxSelectCacheDB.ValueMember = "Server";
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("No cache databases available");
                this.Close();
            }
        }
        
        private void listBoxSelectCacheDB_Click(object sender, EventArgs e)
        {
            if (this.listBoxSelectCacheDB.SelectedIndex > -1)
            {
                this.setDatabase();
            }
        }

        private void setDatabase()
        {
            try
            {
                if (this.listBoxSelectCacheDB.SelectedIndex > -1)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxSelectCacheDB.SelectedItem;
                    System.Data.DataRow R = RV.Row;
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseName = R[0].ToString();
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseServer = R[1].ToString();
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseServerPort = int.Parse(R[2].ToString());
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion = R[4].ToString();
                    //DiversityCollection.CacheDatabase.CacheDB.ProjectsDatabase = R[3].ToString();
                }
                else if (this.dataSetCacheDB.CacheDatabase.Rows.Count == 1)
                {
                    System.Data.DataRow R = this.dataSetCacheDB.CacheDatabase.Rows[0];
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseName = R[0].ToString();
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseServer = R[1].ToString();
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseServerPort = int.Parse(R[2].ToString());
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion = R[4].ToString();
                    //DiversityCollection.CacheDatabase.CacheDB.ProjectsDatabase = R[3].ToString();
                }

                this.DatabaseVersion = DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion;

                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    this.textBoxCacheDB.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseName + " on " + DiversityCollection.CacheDatabase.CacheDB.DatabaseServer;
                    if (!this.DatabaseVersion.StartsWith("01"))
                    {
                        this.initSpecimenProjects();
                        this.initTaxa();
                        this.initInterfaces();
                    }
                    else
                    {
                        this.setV1Messages();
                        this.setV1ProjectLists();
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        public string DatabaseVersion
        {
            get { return _DatabaseVersion; }
            set 
            { 
                _DatabaseVersion = value;
                if (_DatabaseVersion.StartsWith("01"))
                {
                    this.splitContainerVersions.Panel2Collapsed = true;
                    this.splitContainerVersions.Panel1Collapsed = false;
                    this.Height = 500;
                    this.Width = 500;
                }
                else
                {
                    this.splitContainerVersions.Panel2Collapsed = false;
                    this.splitContainerVersions.Panel1Collapsed = true;
                }
            }
        }

        #endregion

        #region Specimen projects

        private void initSpecimenProjects()
        {
            try
            {
                this.tabControlSpecimenProjects.TabPages.Clear();
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    string SQL = "SELECT ProjectID, Project, ProjectURI, DataType " +
                        "FROM ProjectProxy ORDER BY Project";
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    ad.Fill(this.dataSetCacheDB.ProjectProxy);
                    foreach (System.Data.DataRow R in this.dataSetCacheDB.ProjectProxy)
                    {
                        //DiversityCollection.CacheDatabase.CacheDBSpecimenProject SP = new CacheDatabase.CacheDBSpecimenProject(int.Parse(R["ProjectID"].ToString()), R["DataType"].ToString());
                        System.Windows.Forms.TabPage TabPageProject = new TabPage(R["Project"].ToString());
                        //TabPageProject.Tag = SP;
                        this.tabControlSpecimenProjects.TabPages.Add(TabPageProject);

                        System.Windows.Forms.TableLayoutPanel T = new TableLayoutPanel();
                        System.Windows.Forms.ColumnStyle CS1 = new ColumnStyle(SizeType.Percent, 50);
                        T.ColumnStyles.Add(CS1);
                        System.Windows.Forms.ColumnStyle CS2 = new ColumnStyle(SizeType.Percent, 25);
                        T.ColumnStyles.Add(CS2);
                        System.Windows.Forms.ColumnStyle CS3 = new ColumnStyle(SizeType.Percent, 25);
                        T.ColumnStyles.Add(CS3);
                        System.Windows.Forms.RowStyle RS1 = new RowStyle(SizeType.AutoSize);
                        T.RowStyles.Add(RS1);
                        System.Windows.Forms.RowStyle RS2 = new RowStyle(SizeType.Percent, 100);
                        T.RowStyles.Add(RS2);
                        T.Dock = DockStyle.Fill;
                        TabPageProject.Controls.Add(T);

                        System.Windows.Forms.Button ButtonClearCache = new Button();
                        ButtonClearCache.Text = "Clear cache of project " + R["Project"].ToString();
                        ButtonClearCache.TextAlign = ContentAlignment.MiddleLeft;
                        ButtonClearCache.Width = 150;
                        ButtonClearCache.Height = 24;
                        ButtonClearCache.Tag = int.Parse(R["ProjectID"].ToString());
                        ButtonClearCache.Image = this.imageList.Images[2];
                        ButtonClearCache.ImageAlign = ContentAlignment.MiddleRight;
                        ButtonClearCache.Click += this.clearSpecimenProjectButton_Click;
                        ButtonClearCache.Dock = DockStyle.Right;
                        ButtonClearCache.Enabled = false;
                        //ButtonExport.Tag = SP;
                        T.Controls.Add(ButtonClearCache, 1, 0);

                        System.Windows.Forms.Button ButtonExport = new Button();
                        ButtonExport.Text = "Export data for project " + R["Project"].ToString();
                        ButtonExport.TextAlign = ContentAlignment.MiddleLeft;
                        ButtonExport.Width = 150;
                        ButtonExport.Height = 24;
                        ButtonExport.Tag = int.Parse(R["ProjectID"].ToString());
                        ButtonExport.Image = this.imageList.Images[1];
                        ButtonExport.ImageAlign = ContentAlignment.MiddleRight;
                        ButtonExport.Click += this.exportSpecimenProjectButton_Click;
                        ButtonExport.Dock = DockStyle.Right;
                        ButtonExport.Enabled = false;
                        //ButtonExport.Tag = SP;
                        T.Controls.Add(ButtonExport, 2, 0);

                        System.Windows.Forms.Button ButtonRefresh = new Button();
                        ButtonRefresh.Text = "Requery data for project " + R["Project"].ToString();
                        ButtonRefresh.TextAlign = ContentAlignment.MiddleLeft;
                        ButtonRefresh.Tag = int.Parse(R["ProjectID"].ToString());
                        ButtonRefresh.Width = 150;
                        ButtonRefresh.Height = 24;
                        ButtonRefresh.Image = this.imageList.Images[0];
                        ButtonRefresh.ImageAlign = ContentAlignment.MiddleRight;
                        ButtonRefresh.Click += this.refreshSpecimenProjectButton_Click;
                        ButtonRefresh.Dock = DockStyle.Left;
                        //ButtonRefresh.Tag = SP;
                        T.Controls.Add(ButtonRefresh, 0, 0);
                        //TabPageProject.Controls.Add(ButtonRefresh);

                        System.Windows.Forms.TabControl TabControlTables = new TabControl();
                        TabControlTables.Dock = DockStyle.Fill;
                        T.Controls.Add(TabControlTables, 0, 1);
                        T.SetColumnSpan(TabControlTables, 3);
                        System.Collections.Generic.Dictionary<string, DiversityCollection.CacheDatabase.UserControlTable> TableDict = new Dictionary<string, CacheDatabase.UserControlTable>();
                        foreach (string s in DiversityCollection.CacheDatabase.CacheDB.CollectionTables)
                        {
                            System.Windows.Forms.TabPage TpT = new TabPage(s);
                            DiversityCollection.CacheDatabase.UserControlTable U = new CacheDatabase.UserControlTable(s, int.Parse(R["ProjectID"].ToString()));
                            U.Dock = DockStyle.Fill;
                            TpT.Controls.Add(U);
                            TabControlTables.TabPages.Add(TpT);
                            TableDict.Add(s, U);
                            //this._ExportTables.Add(s, U);
                        }
                        if (this._ProjectExportTables == null)
                            this._ProjectExportTables = new Dictionary<int, Dictionary<string, CacheDatabase.UserControlTable>>();
                        this._ProjectExportTables.Add(int.Parse(R["ProjectID"].ToString()), TableDict);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void initSpecimenProjectTables(System.Windows.Forms.TabControl TabControl, int ProjectID)
        {
        }

        private void refreshSpecimenProjectButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
            string sProject = B.Tag.ToString();
            int ProjectID = int.Parse(((System.Windows.Forms.Button)sender).Tag.ToString());
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.CacheDatabase.UserControlTable> KV in this._ProjectExportTables[ProjectID])
            {
                KV.Value.RequeyContent();
            }
            System.Windows.Forms.TableLayoutPanel T = (System.Windows.Forms.TableLayoutPanel)B.Parent;
            foreach (System.Windows.Forms.Control C in T.Controls)
            {
                if (C.Enabled == false)
                    C.Enabled = true;
            }
        }

        private void clearSpecimenProjectButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
            string sProject = B.Tag.ToString();
            int ProjectID = int.Parse(((System.Windows.Forms.Button)sender).Tag.ToString());
            System.Windows.Forms.TableLayoutPanel T = (System.Windows.Forms.TableLayoutPanel)B.Parent;
            System.Windows.Forms.TabControl TC = new TabControl();
            foreach (System.Windows.Forms.Control C in T.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.TabControl))
                {
                    TC = (System.Windows.Forms.TabControl)C;
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.CacheDatabase.UserControlTable> KV in this._ProjectExportTables[ProjectID])
            {
                foreach (System.Windows.Forms.TabPage TP in TC.TabPages)
                {
                    if (TP.Text == KV.Key)
                        TC.SelectedTab = TP;
                }
                KV.Value.ClearCacheData();
            }
        }

        private void exportSpecimenProjectButton_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                string sProject = B.Tag.ToString();
                int ProjectID = int.Parse(((System.Windows.Forms.Button)sender).Tag.ToString());
                System.Windows.Forms.TableLayoutPanel T = (System.Windows.Forms.TableLayoutPanel)B.Parent;
                System.Windows.Forms.TabControl TC = new TabControl();
                foreach (System.Windows.Forms.Control C in T.Controls)
                {
                    if (C.GetType() == typeof(System.Windows.Forms.TabControl))
                    {
                        TC = (System.Windows.Forms.TabControl)C;
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.CacheDatabase.UserControlTable> KV in this._ProjectExportTables[ProjectID])
                {
                    foreach (System.Windows.Forms.TabPage TP in TC.TabPages)
                    {
                        if (TP.Text == KV.Key)
                            TC.SelectedTab = TP;
                    }
                    KV.Value.ExportData();
                    //DiversityCollection.CacheDatabase.CacheDB.ExportTable(KV.Key, ProjectID);
                }
                string SQL = "USE " + DiversityCollection.CacheDatabase.CacheDB.ProjectsDatabase + "; SELECT CAST(ProjectSettings AS nvarchar(max)) AS ProjectSettings " +
                    "FROM Project " +
                    "WHERE ProjectID = " + ProjectID.ToString();
                string ProjectSetting = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
                SQL = "UPDATE ProjectProxy SET ProjectSettings = '" + ProjectSetting + "' WHERE ProjectID = " + ProjectID.ToString();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Taxa
        
        private void initTaxa()
        {
            this.tabControlTaxa.TabPages.Clear();

            if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
            {
                string SQL = "SELECT DataSource, DatabaseName, LastUpdate " +
                    "FROM DiversityTaxonNamesSources ORDER BY DataSource, DatabaseName";
                this._SqlDataAdapterDiversityTaxonNames = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                this._SqlDataAdapterDiversityTaxonNames.Fill(this.dataSetCacheDB.DiversityTaxonNames);

                if (this.dataSetCacheDB.DiversityTaxonNames.Rows.Count > 0)
                {
                    SQL = "SELECT DataSource, DatabaseName, ProjectID, Sequence, Project " +
                        "FROM DiversityTaxonNamesProjectSequence ";
                    this._SqlDataAdapterDiversityTaxonNamesProjectSequence = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    this._SqlDataAdapterDiversityTaxonNamesProjectSequence.Fill(this.dataSetCacheDB.DiversityTaxonNamesProjectSequence);

                    foreach (System.Data.DataRow R in this.dataSetCacheDB.DiversityTaxonNames.Rows)
                    {
                        System.Collections.Generic.Dictionary<int, string> ProjectIDs = new Dictionary<int, string>();
                        System.Data.DataRow[] rr = this.dataSetCacheDB.DiversityTaxonNamesProjectSequence.Select("DataSource = '" + R["DataSource"].ToString() + "' AND DatabaseName = '" + R["DatabaseName"].ToString() + "'", "Sequence");
                        foreach (System.Data.DataRow RR in rr)
                        {
                            ProjectIDs.Add(int.Parse(RR["ProjectID"].ToString()), RR["Project"].ToString());
                        }
                        DiversityCollection.CacheDatabase.CacheDB.CacheDBTaxonSourcesAdd(R["DataSource"].ToString(), R["DatabaseName"].ToString(), ProjectIDs, this.dataSetCacheDB.TaxonSynonymy);
                        DiversityCollection.CacheDatabase.CacheDBTaxonSource CDBT = DiversityCollection.CacheDatabase.CacheDB.CacheDBTaxonSourceGet(R["DataSource"].ToString(), R["DatabaseName"].ToString());
                        if (CDBT.BaseURL != null && CDBT.BaseURL.Length > 0)
                        {
                            string Title = R["DataSource"].ToString() + "." + R["DatabaseName"].ToString();
                            System.Windows.Forms.TabPage T = new TabPage(Title);

                            System.Windows.Forms.TableLayoutPanel TLP = new TableLayoutPanel();
                            System.Windows.Forms.ColumnStyle CS1 = new ColumnStyle(SizeType.Percent, 50);
                            TLP.ColumnStyles.Add(CS1);
                            System.Windows.Forms.ColumnStyle CS2 = new ColumnStyle(SizeType.Percent, 50);
                            TLP.ColumnStyles.Add(CS2);
                            System.Windows.Forms.ColumnStyle CS3 = new ColumnStyle(SizeType.Absolute, 208);
                            TLP.ColumnStyles.Add(CS3);
                            System.Windows.Forms.RowStyle RS1 = new RowStyle(SizeType.AutoSize);
                            TLP.RowStyles.Add(RS1);
                            System.Windows.Forms.RowStyle RS2 = new RowStyle(SizeType.Percent, 100);
                            TLP.RowStyles.Add(RS2);
                            TLP.Dock = DockStyle.Fill;
                            T.Controls.Add(TLP);

                            System.Windows.Forms.DataGridView DGV = new DataGridView();
                            DGV.DataSource = CDBT.DataTable;
                            DGV.ReadOnly = true;
                            DGV.Dock = DockStyle.Fill;
                            DGV.BringToFront();
                            DGV.AllowUserToAddRows = false;
                            TLP.Controls.Add(DGV, 0, 1);
                            TLP.SetColumnSpan(DGV, 3);

                            System.Windows.Forms.Button ButtonShowCurrent = new Button();
                            ButtonShowCurrent.Text = "Show current content";
                            ButtonShowCurrent.Height = 24;
                            ButtonShowCurrent.Width = 140;
                            ButtonShowCurrent.TextAlign = ContentAlignment.MiddleLeft;
                            ButtonShowCurrent.Click += this.showCurrentTaxaButton_Click;
                            ButtonShowCurrent.Tag = CDBT;
                            ButtonShowCurrent.Dock = DockStyle.Left;
                            ButtonShowCurrent.Image = this.imageList.Images[0];
                            ButtonShowCurrent.ImageAlign = ContentAlignment.MiddleRight;
                            TLP.Controls.Add(ButtonShowCurrent, 0, 0);

                            System.Windows.Forms.Button ButtonClear = new Button();
                            ButtonClear.Text = "Delete current content";
                            ButtonClear.TextAlign = ContentAlignment.MiddleLeft;
                            ButtonClear.Click += this.deleteCurrentTaxaButton_Click;
                            ButtonClear.Tag = CDBT;
                            ButtonClear.Dock = DockStyle.Right;
                            ButtonClear.Height = 24;
                            ButtonClear.Width = 140;
                            ButtonClear.Image = this.imageList.Images[2];
                            ButtonClear.ImageAlign = ContentAlignment.MiddleRight;
                            TLP.Controls.Add(ButtonClear, 1, 0);

                            System.Windows.Forms.Button ButtonRefresh = new Button();
                            ButtonRefresh.Text = "Refresh taxon list for current source";
                            ButtonRefresh.TextAlign = ContentAlignment.MiddleLeft;
                            ButtonRefresh.Click += this.refreshTaxonButton_Click;
                            ButtonRefresh.Tag = CDBT;
                            ButtonRefresh.Image = this.imageList.Images[1];
                            ButtonRefresh.ImageAlign = ContentAlignment.MiddleRight;
                            ButtonClear.Height = 24;
                            ButtonClear.Width = 140;
                            ButtonRefresh.Dock = DockStyle.Fill;
                            TLP.Controls.Add(ButtonRefresh, 2, 0);

                            this.tabControlTaxa.TabPages.Add(T);
                        }
                    }
                }
            }
        }

        private void refreshTaxonButton_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to refresh the taxa of this datasource?", "Refresh?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                    DiversityCollection.CacheDatabase.CacheDBTaxonSource CDBT = (DiversityCollection.CacheDatabase.CacheDBTaxonSource)B.Tag;
                    System.Data.DataTable dt = CDBT.RequerySource();
                    if (dt.Rows.Count > 0)
                    {
                        System.Windows.Forms.Form F = new Form();
                        System.Windows.Forms.Label L = new Label();
                        L.Text = "Errors that occurred during the insert of the taxa from " + CDBT.Datasource;
                        L.TextAlign = ContentAlignment.MiddleCenter;
                        F.Controls.Add(L);
                        L.Dock = DockStyle.Top;
                        System.Windows.Forms.DataGridView G = new DataGridView();
                        F.Controls.Add(G);
                        G.DataSource = dt;
                        G.ReadOnly = true;
                        G.Dock = DockStyle.Fill;
                        G.BringToFront();
                        F.Width = this.Width - 10;
                        F.Height = this.Height - 10;
                        F.StartPosition = FormStartPosition.CenterParent;
                        F.Text = dt.Rows.Count.ToString() + " Errors";
                        F.ShowDialog();
                    }
                    CDBT.RefershDataTable();
                }
                catch (System.Exception ex) { }
            }
        }

        private void showCurrentTaxaButton_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to show the current taxa of this datasource?", "Refresh?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                    DiversityCollection.CacheDatabase.CacheDBTaxonSource CDBT = (DiversityCollection.CacheDatabase.CacheDBTaxonSource)B.Tag;
                    //System.Data.DataTable dt = CDBT.RequerySource();
                    //if (dt.Rows.Count > 0)
                    //{
                    //    System.Windows.Forms.Form F = new Form();
                    //    System.Windows.Forms.Label L = new Label();
                    //    L.Text = "Errors that occurred during the insert of the taxa from " + CDBT.Datasource;
                    //    L.TextAlign = ContentAlignment.MiddleCenter;
                    //    F.Controls.Add(L);
                    //    L.Dock = DockStyle.Top;
                    //    System.Windows.Forms.DataGridView G = new DataGridView();
                    //    F.Controls.Add(G);
                    //    G.DataSource = dt;
                    //    G.ReadOnly = true;
                    //    G.Dock = DockStyle.Fill;
                    //    G.BringToFront();
                    //    F.Width = this.Width - 10;
                    //    F.Height = this.Height - 10;
                    //    F.StartPosition = FormStartPosition.CenterParent;
                    //    F.Text = dt.Rows.Count.ToString() + " Errors";
                    //    F.ShowDialog();
                    //}
                    CDBT.RefershDataTable();
                }
                catch (System.Exception ex) { }
            }
        }

        private void deleteCurrentTaxaButton_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to delete current taxa of this datasource?", "Delete?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                    DiversityCollection.CacheDatabase.CacheDBTaxonSource CDBT = (DiversityCollection.CacheDatabase.CacheDBTaxonSource)B.Tag;
                    CDBT.ClearCurrentData();
                    CDBT.RefershDataTable();
                }
                catch (System.Exception ex) { }
            }
        }

        private string BaseURL(string DataSource, string Database)
        {
            string URL = "";
            string SQL = "SELECT dbo.BaseURL()";
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.ConnectionStringSource(DataSource, Database));
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                URL = C.ExecuteScalar().ToString();
                con.Close();
            }
            catch (Exception ex)
            {
            }
            return URL;
        }
        
        #endregion

        #region Interface
        
        private void initInterfaces()
        {
            this._InterfaceDatagrids = new Dictionary<string, Dictionary<int, Dictionary<string, DataGridView>>>();
            try
            {
                System.Data.DataTable dt = DiversityCollection.CacheDatabase.CacheDB.DtInterface();
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string Interface = R[0].ToString();
                        System.Windows.Forms.TabPage TP = new TabPage(Interface);
                        this.tabControlMain.TabPages.Add(TP);

                        System.Windows.Forms.TabControl TCprojects = new TabControl();
                        TCprojects.Dock = DockStyle.Fill;
                        TP.Controls.Add(TCprojects);
                        foreach (System.Data.DataRow Rpp in this.dataSetCacheDB.ProjectProxy)
                        {
                            int ProjectID = int.Parse(Rpp["ProjectID"].ToString());
                            System.Windows.Forms.TabPage TabPageProject = new TabPage(Rpp["Project"].ToString());
                            TCprojects.TabPages.Add(TabPageProject);

                            System.Windows.Forms.TableLayoutPanel T = new TableLayoutPanel();
                            System.Windows.Forms.ColumnStyle CS1 = new ColumnStyle(SizeType.Percent, 50);
                            T.ColumnStyles.Add(CS1);
                            System.Windows.Forms.ColumnStyle CS2 = new ColumnStyle(SizeType.Percent, 25);
                            T.ColumnStyles.Add(CS2);
                            System.Windows.Forms.ColumnStyle CS3 = new ColumnStyle(SizeType.Percent, 25);
                            T.ColumnStyles.Add(CS3);
                            System.Windows.Forms.RowStyle RS1 = new RowStyle(SizeType.AutoSize);
                            T.RowStyles.Add(RS1);
                            System.Windows.Forms.RowStyle RS2 = new RowStyle(SizeType.Percent, 100);
                            T.RowStyles.Add(RS2);
                            T.Dock = DockStyle.Fill;
                            T.Tag = ProjectID;
                            TabPageProject.Controls.Add(T);

                            System.Windows.Forms.Button ButtonRefresh = new Button();
                            ButtonRefresh.Text = "Requery data for project " + Rpp["Project"].ToString();
                            ButtonRefresh.TextAlign = ContentAlignment.MiddleLeft;
                            //ButtonRefresh.Tag = int.Parse(Rpp["ProjectID"].ToString());
                            ButtonRefresh.Width = 150;
                            ButtonRefresh.Height = 24;
                            ButtonRefresh.Image = this.imageList.Images[0];
                            ButtonRefresh.ImageAlign = ContentAlignment.MiddleRight;
                            ButtonRefresh.Click += this.refreshInterfaceProjectButton_Click;
                            ButtonRefresh.Dock = DockStyle.Left;
                            T.Controls.Add(ButtonRefresh, 0, 0);

                            System.Windows.Forms.TabControl TabControlTables = new TabControl();
                            TabControlTables.Dock = DockStyle.Fill;
                            T.Controls.Add(TabControlTables, 0, 1);
                            T.SetColumnSpan(TabControlTables, 3);
                            System.Data.DataTable DtInterfaceTables = DiversityCollection.CacheDatabase.CacheDB.DtInterfaceTables(R[0].ToString());
                            foreach (System.Data.DataRow Rit in DtInterfaceTables.Rows)
                            {
                                string InterfaceTable = Rit[0].ToString();
                                System.Windows.Forms.TabPage TpT = new TabPage(InterfaceTable);
                                System.Windows.Forms.DataGridView DGV = new DataGridView();
                                TpT.Controls.Add(DGV);
                                DGV.Dock = DockStyle.Fill;
                                TabControlTables.TabPages.Add(TpT);
                                if (this._InterfaceDatagrids.ContainsKey(Interface))
                                {
                                    if (this._InterfaceDatagrids[Interface].ContainsKey(ProjectID))
                                        this._InterfaceDatagrids[Interface][ProjectID].Add(InterfaceTable, DGV);
                                    else
                                    {
                                        System.Collections.Generic.Dictionary<string, System.Windows.Forms.DataGridView> D = new Dictionary<string,DataGridView>();
                                        D.Add(InterfaceTable, DGV);
                                        this._InterfaceDatagrids[Interface].Add(ProjectID, D);
                                    }
                                }
                                else
                                {
                                    System.Collections.Generic.Dictionary<string, System.Windows.Forms.DataGridView> D = new Dictionary<string, DataGridView>();
                                    D.Add(InterfaceTable, DGV);
                                    System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Windows.Forms.DataGridView>> DD = new Dictionary<int, Dictionary<string, DataGridView>>();
                                    DD.Add(ProjectID, D);
                                    this._InterfaceDatagrids.Add(Interface, DD);
                                }
                            }
                            ButtonRefresh.Tag = this._InterfaceDatagrids[Interface][ProjectID];
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        private void refreshInterfaceProjectButton_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                System.Collections.Generic.Dictionary<string, System.Windows.Forms.DataGridView> D = (System.Collections.Generic.Dictionary<string, System.Windows.Forms.DataGridView>)B.Tag;
                System.Windows.Forms.TableLayoutPanel T = (System.Windows.Forms.TableLayoutPanel)B.Parent;
                int ProjectID = int.Parse(T.Tag.ToString());
                foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.DataGridView> KV in D)
                {
                    KV.Value.DataSource = DiversityCollection.CacheDatabase.CacheDB.DtInterfaceTable(KV.Key, ProjectID);
                }
            }
            catch (System.Exception ex) { }
        }
        
        #endregion

        #region Auxillary
        
        private string ConnectionStringSource(string DataSource, string Database)
        {
            string Conn = "";
            DiversityWorkbench.ServerConnection S = new DiversityWorkbench.ServerConnection();
            S.DatabaseServer = DataSource;
            S.DatabaseName = Database;
            S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
            if (!S.IsTrustedConnection)
            {
                S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                S.DatabasePassword = DiversityWorkbench.Settings.Password;
            }
            Conn = S.ConnectionString;
            return Conn;
        }
        
        #endregion

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
        }

        #region Version 1

        private void setV1Messages()
        {
            string SQL = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
            {
                this.UpdateCheckForDatabase();
                SQL = "SELECT COUNT(*) AS Anzahl FROM CollectionSpecimenCache";
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                con.Open();
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                this.labelV1CurrentSpecimenNumber.Text = "Current specimens: " + C.ExecuteScalar().ToString();
                C.CommandText = "SELECT COUNT(*) AS Anzahl FROM TaxonSynonymy";
                this.labelV1CurrentTaxa.Text = "Current taxa: " + C.ExecuteScalar().ToString();
                C.CommandText = "SELECT convert(varchar(50), max([LogInsertedWhen]), 102) AS LastTransfer FROM [dbo].[CollectionSpecimenCache]";
                this.labelV1LastSpecimenTransfer.Text = "Last transfer: " + C.ExecuteScalar().ToString();
                C.CommandText = "SELECT convert(varchar(50), max([LogInsertedWhen]), 102) AS LastTransfer FROM [dbo].[TaxonSynonymy]";
                this.labelV1LastTaxonTransfer.Text = "Last transfer: " + C.ExecuteScalar().ToString();
                con.Close();
                con.Dispose();
            }
        }

        #region Projects

        private System.Data.DataTable _DtV1Publihed;
        private System.Data.DataTable _DtV1UnPublihed;

        private void setV1ProjectLists()
        {
            try
            {
                this._DtV1Publihed = new DataTable();

                this._DtV1UnPublihed = new DataTable();

                string SQL = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    SQL = "SELECT ProjectID, Project " +
                    "FROM ProjectPublished " +
                    "ORDER BY Project ";
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    ad.Fill(this._DtV1Publihed);
                    SQL = "";
                    string Projects = "";
                    foreach (System.Data.DataRow R in this._DtV1Publihed.Rows)
                    {
                        if (Projects.Length > 0) Projects += ", ";
                        Projects += R["ProjectID"].ToString();
                    }

                    SQL = "SELECT ProjectID, Project " +
                    "FROM ProjectProxy ";
                    if (Projects.Length > 0)
                        SQL += "WHERE ProjectID NOT IN (" + Projects + ") ";
                    SQL += "ORDER BY Project ";
                    System.Data.SqlClient.SqlDataAdapter adUn = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    adUn.Fill(this._DtV1UnPublihed);
                }
                this.listBoxV1ProjectPublished.DataSource = this._DtV1Publihed;
                this.listBoxV1ProjectPublished.DisplayMember = "Project";
                this.listBoxV1ProjectPublished.ValueMember = "ProjectID";

                this.listBoxV1ProjectsUnpublished.DataSource = this._DtV1UnPublihed;
                this.listBoxV1ProjectsUnpublished.DisplayMember = "Project";
                this.listBoxV1ProjectsUnpublished.ValueMember = "ProjectID";
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonV1PublishProject_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxV1ProjectsUnpublished.SelectedItem;
            string SQL = "INSERT INTO ProjectPublished (ProjectID, Project) VALUES (" + R["ProjectID"].ToString() + ", '" + R["Project"].ToString() + "')";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            con.Open();
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            C.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            this.setV1ProjectLists();
            this.setV1Messages();
        }

        private void buttonV1UnpublishProject_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxV1ProjectPublished.SelectedItem;
            string SQL = "DELETE P FROM ProjectPublished P WHERE ProjectID = " + R["ProjectID"].ToString();
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            con.Open();
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            C.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            this.setV1ProjectLists();
            this.setV1Messages();
        }
        
        #endregion

        #region Transfer

        private void buttonV1StartTaxonTransfer_Click(object sender, EventArgs e)
        {
            string Result = "";
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            try
            {
                string SQL = "DECLARE @RC int; EXECUTE @RC = [dbo].[procRefreshTaxonSynonyms] ";
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                Result = C.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            if (Result.Length == 0)
                Result = "Transfer failed";
            this.Cursor = System.Windows.Forms.Cursors.Default;
            System.Windows.Forms.MessageBox.Show(Result);
        }

        private void buttonV1StartCollectionTransfer_Click(object sender, EventArgs e)
        {
            string Result = "";
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            try
            {
                string SQL = "DECLARE @RC int; EXECUTE @RC = [dbo].[procRefresh] ";
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = 99999;
                con.Open();
                Result = C.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            if (Result.Length == 0)
                Result = "Transfer failed";
            this.Cursor = System.Windows.Forms.Cursors.Default;
            System.Windows.Forms.MessageBox.Show(Result);
            if (Result.StartsWith("Problems with the taxon syn"))
            {
                System.Windows.Forms.Form f = new Form();
                f.Text = "Problems with the taxon synonymy";
                System.Windows.Forms.DataGridView DGV = new DataGridView();
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT CollectionSpecimenID, IdentificationUnitID, AcceptedName AS [Accepted name 1], _AcceptedName AS [Accepted name 2], AcceptedNameURI AS [Link to name 1], " +
                    "_AcceptedNameURI AS [Link to name 2] " +
                    "FROM Test_IdentificationUnit_AcceptedName_Duplicates";
                System.Data.SqlClient.SqlDataAdapter a = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                a.Fill(dt);
                DGV.DataSource = dt;
                f.Controls.Add(DGV);
                f.StartPosition = FormStartPosition.CenterParent;
                f.Height = this.Height - 20;
                f.Width = this.Width - 20;
                DGV.Dock = DockStyle.Fill;
                f.ShowDialog();
            }
        }

        #endregion

        #region Inspection

        private void buttonV1CheckSpecimen_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "";
                string ProjectID;
                string Project;
                if (this.listBoxV1ProjectPublished.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a published project");
                    return;
                }
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxV1ProjectPublished.SelectedItem;
                ProjectID = R["ProjectID"].ToString();
                Project = R["Project"].ToString();
                SQL = "SELECT S.AccessionNumber, S.CollectionDate, S.CollectionDay, S.CollectionMonth, S.CollectionYear, S.CollectionDateSupplement, S.LocalityDescription, S.CountryCache, " +
                    "S.LabelTranscriptionNotes, S.ExsiccataAbbreviation, S.OriginalNotes, S.Latitude, S.Longitude, S.LocationAccuracy, S.DistanceToLocation, S.DirectionToLocation, " +
                    "S.CollectorsEventNumber, S.Chronostratigraphy, S.Lithostratigraphy " +
                    "FROM CollectionSpecimenCache AS S INNER JOIN " +
                    "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE P.ProjectID =  " + ProjectID +
                    " ORDER BY S.AccessionNumber";
                System.Data.DataTable dt = new DataTable();
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(dt);
                DiversityWorkbench.Forms.FormTableContent f = new DiversityWorkbench.Forms.FormTableContent("Specimen - " + Project + " (" + dt.Rows.Count.ToString() + ")", dt.Rows.Count.ToString() + " specimen within the project " + Project + " in the cache database", dt);
                f.ShowDialog();

            }
            catch (Exception ex)
            {
            }
        }

        private void buttonV1CheckTaxa_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT distinct reverse(substring(reverse([NameURI]), charindex('/', reverse([NameURI])) + 1, 255)) AS Source FROM [dbo].[TaxonSynonymy]";
                System.Data.DataTable dtSource = new DataTable();
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(dtSource);
                string WhereClause = "";
                string Source = "";
                if (dtSource.Rows.Count > 1)
                {
                    DiversityWorkbench.FormGetStringFromList f = new DiversityWorkbench.FormGetStringFromList(dtSource, "Please select a source");
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        WhereClause = " WHERE NameURI LIKE '" + f.SelectedString + "%'";
                        Source = f.SelectedString;
                    }
                    else
                        return;
                }
                SQL = "SELECT AcceptedName AS [Accepted name], CASE WHEN SynonymName = AcceptedName then '' else SynonymName end as Synonym FROM TaxonSynonymy " + WhereClause + " ORDER BY AcceptedName";
                System.Data.DataTable dtTaxa = new DataTable();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(dtTaxa);
                string Title ="Taxa (" + dtTaxa.Rows.Count.ToString() + ")";
                string Header = dtTaxa.Rows.Count.ToString() + " taxa in cache database";
                if (Source.Length > 0)
                {
                    Title += ": " + Source;
                    Header += ". Source = " + Source;
                }
                DiversityWorkbench.Forms.FormTableContent fTable = new DiversityWorkbench.Forms.FormTableContent(Title, Header, dtTaxa);
                fTable.ShowDialog();
            }
            catch (System.Exception ex)
            {
            }
        }
        
        #endregion

        #region Update

        private bool UpdateCheckForDatabase()
        {
            return false;

            bool Update = false;
            string SQL = "select dbo.Version()";
            string Version = "";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                Version = C.ExecuteScalar().ToString();
                con.Close();
            }
            catch (System.Exception ex) 
            {
                System.Windows.Forms.MessageBox.Show("The connection " + DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB + " is not valid.");
                 return false;
            }
            if (Version != DiversityCollection.Properties.Settings.Default.CacheDBV1)
            {
                int DBversionMajor;
                int DBversionMinor;
                int DBversionRevision;
                int C_versionMajor;
                int C_versionMinor;
                int C_versionRevision;
                string[] VersionDBParts;
                if (Version.IndexOf('.') > -1) VersionDBParts = Version.Split(new Char[] { '.' });
                else if (Version.IndexOf('/') > -1) VersionDBParts = Version.Split(new Char[] { '/' });
                else VersionDBParts = Version.Split(new Char[] { ' ' });
                if (!int.TryParse(VersionDBParts[0], out DBversionMajor)) return false;
                if (!int.TryParse(VersionDBParts[1], out DBversionMinor)) return false;
                if (!int.TryParse(VersionDBParts[2], out DBversionRevision)) return false;

                string[] VersionC_Parts = DiversityCollection.Properties.Settings.Default.CacheDBV1.Split(new Char[] { '.' });
                if (!int.TryParse(VersionC_Parts[0], out C_versionMajor)) return false;
                if (!int.TryParse(VersionC_Parts[1], out C_versionMinor)) return false;
                if (!int.TryParse(VersionC_Parts[2], out C_versionRevision)) return false;

                if (C_versionMajor > DBversionMajor ||
                    (C_versionMajor == DBversionMajor && C_versionMinor > DBversionMinor) ||
                    (C_versionMajor == DBversionMajor && C_versionMinor == DBversionMinor && C_versionRevision > DBversionRevision))
                    Update = true;
                if (DBversionMajor == 2)
                {
                    Version = "00.00.00";
                    Update = true;
                }
            }
            if (Update) this.labelV1Update.Text = "Your version of the database is " + Version + ". Please update the database to version " + DiversityCollection.Properties.Settings.Default.CacheDBV1;
            else this.labelV1Update.Text = "No updates for database";
            this.panelV1Update.Visible = false;// Update;
            return Update;
        }


        private void UpdateDatabase()
        {
            string DatabaseCurrentVersion = "";
            string SQL = "select dbo.Version()";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                DatabaseCurrentVersion = C.ExecuteScalar().ToString();
                con.Close();
            }
            catch { }
            DatabaseCurrentVersion = DatabaseCurrentVersion.Replace(".", "").Replace("/", "");
            string DatabaseFinalVersion = DiversityCollection.Properties.Settings.Default.CacheDBV1;
            DatabaseFinalVersion = DatabaseFinalVersion.Replace(".", "").Replace("/", "");

            // Ignore former version
            //if (DatabaseCurrentVersion.StartsWith("02"))
            //    DatabaseCurrentVersion = "000000";

            // check resouces for update scripts
            System.Collections.Generic.Dictionary<string, string> Versions = new Dictionary<string, string>();
            System.Resources.ResourceManager rm = Properties.Resources.ResourceManager;
            System.Resources.ResourceSet rs = rm.GetResourceSet(new System.Globalization.CultureInfo("en-US"), true, true);
            if (rs != null)
            {
                System.Collections.IDictionaryEnumerator de = rs.GetEnumerator();
                while (de.MoveNext() == true)
                {
                    if (de.Entry.Value is string)
                    {
                        if (de.Key.ToString().StartsWith("DiversityCollectionCacheUpdateV1_"))
                        {
                            Versions.Add(de.Key.ToString(), de.Value.ToString());
                        }
                    }
                }
            }

            if (Versions.Count > 0)
            {
                DiversityWorkbench.FormUpdateDatabase f = new DiversityWorkbench.FormUpdateDatabase(DiversityCollection.CacheDatabase.CacheDB.DatabaseName, DiversityCollection.Properties.Settings.Default.CacheDBV1, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB, Versions, System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm");
                f.ShowDialog();
                if (f.Reconnect) this.setDatabase();
                this.UpdateCheckForDatabase();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Upgrade resources are missing");
            }
        }

        private void buttonV1Update_Click(object sender, EventArgs e)
        {
            this.UpdateDatabase();
        }
        
        #endregion

        #endregion    


    }
}
