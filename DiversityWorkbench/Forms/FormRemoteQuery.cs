using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Windows.Forms;
using DiversityWorkbench.UserControls;
using DWBServices;
using DWBServices.WebServices;
using DWBServices.WebServices.GeoServices;
using DWBServices.WebServices.GeoServices.Geonames;
using DWBServices.WebServices.GeoServices.GFBioTermGeonames;
using DWBServices.WebServices.GeoServices.IHOWorldSeas;
using DWBServices.WebServices.GeoServices.ISOCountries;
using DWBServices.WebServices.TaxonomicServices;
using DWBServices.WebServices.TaxonomicServices.CatalogueOfLife;
using DWBServices.WebServices.TaxonomicServices.GbifSpecies;
using DWBServices.WebServices.TaxonomicServices.GfbioTerminology;
using DWBServices.WebServices.TaxonomicServices.IndexFungorum;
using DWBServices.WebServices.TaxonomicServices.Mycobank;
using DWBServices.WebServices.TaxonomicServices.PESI;
using DWBServices.WebServices.TaxonomicServices.WoRMS;
using Microsoft.Extensions.DependencyInjection;

namespace DiversityWorkbench.Forms
{
    public partial class FormRemoteQuery : Form
    {

        #region Parameter
        private string _URI;
        private string _DisplayText;
        private string _BaseURI;
        private string _BaseUriSuffix = "";
        private string _PartialService;
        private Dictionary<string, string> _UnitCollection = null;
        private Dictionary<string, string> _ValueDictionary = null;
        private DiversityWorkbench.ServerConnection _ServerConnection;
        private DiversityWorkbench.IWorkbenchUnit _IWorkbenchUnit;

        private bool _ShowModuleConnection = true;
        private bool ShowModuleConnection
        {
            get { return _ShowModuleConnection; }
            set
            {
                _ShowModuleConnection = value;
                this.panelOpenModule.Visible = _ShowModuleConnection;
            }
        }

        private bool _IsCurrentDatabase = false;
        private bool _HtmlAvailable = false;
        private bool _AdditionalAvailable = false;
        private bool _ShowAdditional = false;
        private int _SavedUnitID = 0;
        private DiversityWorkbench.DatabaseService _DatabaseService;
        private DiversityWorkbench.WorkbenchUnit.ServiceType _ServiceType;
        private bool _AdaptFormSize = false;
        /// <summary>
        /// if the link can not be changed, only checking the data of the original source
        /// </summary>
        private bool _ReadOnly = false;

        /// <summary>
        /// if the main interface is addressed,empty, otherwise the name of domain of the Workbench unit
        /// e.g. Terminology in the module DiversityScientificTerms when the table Terminology is addressed
        /// Domains need a unique ID to be addressed
        /// </summary>
        private string _Domain = "";

        private int? _ID = null;

        private DwbServiceEnums.DwbService currentDwbService = DwbServiceEnums.DwbService.None;

        #endregion

        #region Construction

        public FormRemoteQuery(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, bool IsCurrentDatabase = false, bool RestrictToModuleDatabases = false, int? ID = null)
        {
            InitializeComponent();
            this._IsCurrentDatabase = IsCurrentDatabase;
            this._ID = ID;
            this.InitForm();
            bool ConnectToLocalServer = true;
            this.ServerConnection = IWorkbenchUnit.getServerConnection();
            if (IsCurrentDatabase)
            {
                this.userControlQueryList.toolStripButtonConnection.Visible = false;
                this._IWorkbenchUnit = IWorkbenchUnit;
                if (_IWorkbenchUnit is WorkbenchUnit)
                {
                    _HtmlAvailable = true; //(_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.HtmlUnitValues);
                    _AdditionalAvailable = (_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.AdditionalUnitValues);
                }
                this.comboBoxDatabase.Enabled = false;
                this.SetLastSelectedDatabase(DiversityWorkbench.Settings.ModuleName, DiversityWorkbench.Settings.DatabaseName);// this._IWorkbenchUnit.getServerConnection().ModuleName, this.comboBoxDatabase.Text);
                this.ServerConnection = DiversityWorkbench.Settings.ServerConnection;
                this.panelDatabase.Visible = true;
                this.comboBoxDatabase.Items.Add(DiversityWorkbench.Settings.DatabaseName);
                this.comboBoxDatabase.SelectedIndex = 0;
            }
            else
            {
                // MW 31.3.2015: Avoid change of connection to current module
                if (this._ServerConnection.ModuleName == DiversityWorkbench.Settings.ModuleName)
                    this.userControlQueryList.toolStripButtonConnection.Visible = false;
                this._IWorkbenchUnit = IWorkbenchUnit;
                if (_IWorkbenchUnit is WorkbenchUnit)
                {
                    _HtmlAvailable = true; //(_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.HtmlUnitValues);
                    _AdditionalAvailable = (_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.AdditionalUnitValues);
                }
                DiversityWorkbench.DatabaseService _DS = new DatabaseService();
                if (this._IWorkbenchUnit.DatabaseServices().Count > 0)
                {
                    // for selection of the database
                    this.panelDatabase.Visible = true;
                    if (RestrictToModuleDatabases)
                    {
                        System.Collections.Generic.List<DatabaseService> L = this._IWorkbenchUnit.DatabaseServices();
                        foreach (DatabaseService DS in L)
                        {
                            if (DS.IsWebservice)
                                continue;
                            if (comboBoxDatabase.Items.Contains(DS.DatabaseName))
                                continue;
                            this.comboBoxDatabase.Items.Add(DS.DatabaseName);
                        }
                    }
                    else
                    {
                        //foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._IWorkbenchUnit.AccessibleDatabasesAndServicesOfModule())
                        //{
                        //    System.Windows.Forms.ListViewItem I = new ListViewItem(KV.Key);
                        //    this.comboBoxDatabase.Items.Add(KV.Key);
                        //}
                        System.Collections.Generic.List<DatabaseService> L = this._IWorkbenchUnit.DatabaseServices();
                        foreach (DatabaseService DS in L)
                        {
                            //if (DS.IsWebservice)
                            //    continue;
                            if (comboBoxDatabase.Items.Contains(DS.DatabaseName))
                                continue;
                            this.comboBoxDatabase.Items.Add(DS.DatabaseName);
                        }
                    }
                    if (this._IWorkbenchUnit.ServerConnections() != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> SC in this._IWorkbenchUnit.ServerConnections())
                        {
                            if (!this.comboBoxDatabase.Items.Contains(SC.Key))
                            {
                                this.comboBoxDatabase.Items.Add(SC.Key);
                            }
                        }
                    }
                    if (this._IWorkbenchUnit.getServerConnection().LinkedServer.Length > 0 && !this.comboBoxDatabase.Items.Contains(this._IWorkbenchUnit.getServerConnection().DatabaseName))
                    {
                        this.comboBoxDatabase.Items.Add(this._IWorkbenchUnit.getServerConnection().DatabaseName);
                    }
                    string Module = this._IWorkbenchUnit.getServerConnection().ModuleName;
                    if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey(Module))
                    {
                        if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._IWorkbenchUnit.getServerConnection().ModuleName].ServerConnectionList().Count > 0)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._IWorkbenchUnit.getServerConnection().ModuleName].ServerConnectionList())
                            {
                                if (!this.comboBoxDatabase.Items.Contains(KV.Key))
                                {
                                    this.comboBoxDatabase.Items.Add(KV.Key);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.splitContainerQueries.Enabled = false;
                        string Database = "";
                        if (this.Databases.ContainsKey(IWorkbenchUnit.getServerConnection().ModuleName))
                            Database = this.Databases[IWorkbenchUnit.getServerConnection().ModuleName];
                        if (Database.Length > 0)
                        {
                            for (int i = 0; i < this.comboBoxDatabase.Items.Count; i++)
                            {
                                if (this.comboBoxDatabase.Items[i].ToString() == Database)
                                {
                                    this.comboBoxDatabase.SelectedIndex = i;
                                    if (this._IWorkbenchUnit.DatabaseServices().Count > i)
                                        _DS = this._IWorkbenchUnit.DatabaseServices()[i];
                                    break;
                                }
                            }
                        }
                        else
                            _DS = new DatabaseService("");
                        if (_DS.DatabaseName == null)
                            _DS.DatabaseName = IWorkbenchUnit.getServerConnection().DatabaseName;
                        if (_DS.DatabaseName.Length == 0) // || _DS.IsWebservice)
                        {
                            this.splitContainerQueries.Visible = false;
                        }
                        if (_DS.IsWebservice)
                        {
                            ConnectToLocalServer = false;
                        }
                    }
                    this.InitDatabaseMenu("");
                }
                else
                {
                    this.panelDatabase.Visible = false;
                    this.splitContainerQueries.Panel2Collapsed = true;
                }
                if (this._ServerConnection != null && ConnectToLocalServer && !_DS.IsForeignSource)
                {
                    try
                    {
                        this.userControlQueryList.setConnection(this._ServerConnection.ConnectionString, IWorkbenchUnit.MainTable());
                        this.userControlQueryList.setQueryConditions(IWorkbenchUnit.QueryConditions());
                        this.userControlQueryList.QueryDisplayColumns = IWorkbenchUnit.QueryDisplayColumns();
                        // MW 8.2.2016 - wurde nicht gesetzt wenn von vorheriger Auswahl noch verfuegbar
                        if (this._ServerConnection.LinkedServer.Length > 0)
                        {
                            this.userControlQueryList.LinkedServer = this._ServerConnection.LinkedServer;
                            this.userControlQueryList.LinkedServerDatabase = this._ServerConnection.DatabaseName;
                        }
                        this.checkModule();
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }

                string Service = this._IWorkbenchUnit.ServiceName().ToLower();
                switch (Service)
                {
                    case "diversitygazetteer":
                    case "diversitysamplingplots":
                        this.splitContainerUnit.Panel1Collapsed = false;
                        this.splitContainerUnit.Panel2Collapsed = true;
                        break;
                    default:
                        this.splitContainerUnit.Panel1Collapsed = false;
                        this.splitContainerUnit.Panel2Collapsed = true;
                        break;
                }
            }
            if (this.Height > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - 40)
                this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - 40;
            // Markus 2020-03-05: Set database if available
            if (_LastSelectedSource != null && _LastSelectedSource.ContainsKey(this.ServerConnection.ModuleName))
            {
                for (int i = 0; i < this.comboBoxDatabase.Items.Count; i++)
                {
                    if (this.comboBoxDatabase.Items[i].ToString() == _LastSelectedSource[this.ServerConnection.ModuleName])
                    {
                        this.comboBoxDatabase.SelectedIndex = i; /// [A]
                        break;
                    }
                }
            }
            else if (this.ServerConnection != null && this.comboBoxDatabase.Items.Count > 0)
            {
                for (int i = 0; i < this.comboBoxDatabase.Items.Count; i++)
                {
                    if (this.comboBoxDatabase.Items[i].ToString() == this.ServerConnection.DisplayText)
                    {
                        this.comboBoxDatabase.SelectedIndex = i;
                        break;
                    }
                }
            }
            // Toni 20200921: Select at least any available entry
            if (this.comboBoxDatabase.SelectedIndex < 0 && this.comboBoxDatabase.Items.Count > 0)
                this.comboBoxDatabase.SelectedIndex = 0;

            // #128
            this.initGetAllServices();
        }

        public FormRemoteQuery(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, Dictionary<string, string> UnitCollection)
            : this(IWorkbenchUnit)
        {
            _UnitCollection = UnitCollection;

            if (_UnitCollection != null)
            {
                this.splitContainerUnitCollection.Panel2Collapsed = false;

                foreach (KeyValuePair<string, string> item in _UnitCollection)
                {
                    if (!this.listViewUnitCollection.Items.ContainsKey(item.Key))
                    {
                        ListViewItem listItem = new ListViewItem(item.Value);
                        listItem.Name = item.Key;
                        listItem.ToolTipText = item.Key;
                        this.listViewUnitCollection.Items.Add(listItem);
                    }
                }
                this.toolStripButtonRemoveUnit.Visible = this.listViewUnitCollection.SelectedItems.Count > 0;
                this.toolStripButtonShowUnit.Visible = this.listViewUnitCollection.SelectedItems.Count > 0;
            }
        }

        public FormRemoteQuery(int ID, DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit)
            : this(IWorkbenchUnit) /// [1]
        {
            this.splitContainerMain.Panel1Collapsed = true;
            this.userControlDialogPanel.Visible = false;
            this.InitForm(ID);
        }

        public FormRemoteQuery(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, int ID, int Height)
            : this(IWorkbenchUnit, false, false, ID) /// [1]
        {
            this.splitContainerMain.Panel1Collapsed = true;
            this.userControlDialogPanel.Visible = false;
            this.InitForm(ID);
            this.Height = Height;
        }

        public FormRemoteQuery(string URI, DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit)
        {
            InitializeComponent();
            this._ReadOnly = true;
            this.ShowInTaskbar = true;
            this.splitContainerUriResources.Panel2Collapsed = true;
            this.splitContainerUnitCollection.Panel2Collapsed = true;
            this.splitContainerMain.Panel1Collapsed = true;
            this.userControlDialogPanel.Visible = false;
            this.ShowModuleConnection = false;
            try
            {
                this.ServerConnection = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URI);
                if (this.ServerConnection != null)
                    IWorkbenchUnit.setServerConnection(this.ServerConnection);
            }
            catch (System.Exception ex) { }
            if (this.ServerConnection == null)
                this.ServerConnection = IWorkbenchUnit.getServerConnection();
            this._IWorkbenchUnit = IWorkbenchUnit;
            if (_IWorkbenchUnit is WorkbenchUnit)
            {
                _HtmlAvailable = true; //(_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.HtmlUnitValues);
                _AdditionalAvailable = (_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.AdditionalUnitValues);
            }
            try
            {
                string IDinURI = "";
                int ID;
                // get the type of the Service
                this._ServiceType = DiversityWorkbench.WorkbenchUnit.getServiceType(URI);

                if (!int.TryParse(URI, out ID))
                {

                    switch (this._ServiceType)
                    {
                        case WorkbenchUnit.ServiceType.WorkbenchModule:
                            if (this.ServerConnection != null)
                                this._BaseURI = this.ServerConnection.BaseURL;
                            else
                                this._BaseURI = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[DiversityWorkbench.WorkbenchUnit.getServiceNameFromURI(URI)].DataBaseURIs()[DiversityWorkbench.WorkbenchUnit.getDatabaseNameFromURI(URI)];
                            IDinURI = URI.Substring(this._BaseURI.Length);
                            if (int.TryParse(IDinURI, out ID))
                            {
                                this._ServerConnection.DatabaseName = DiversityWorkbench.WorkbenchUnit.getDatabaseNameFromURI(URI);
                                this.setUnitPanel(IWorkbenchUnit);
                                this.InitForm(ID);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("Could not parse URI " + URI);
                                this.Close();
                            }
                            this.splitContainerUnit.Panel1Collapsed = false;
                            this.splitContainerUnit.Panel2Collapsed = true;
                            break;

                        case WorkbenchUnit.ServiceType.WebService:
                            if (DwbServiceProviderAccessor.Instance == null)
                            {
                                MessageBox.Show("The webservice is not available -- FormRemoteQuery 379");
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("The webservice is not available --FormRemoteQuery 379.. dwbServiceProvider is null ");
                                this.Close();
                            }
                            string ServiceName = DiversityWorkbench.WorkbenchUnit.getDatabaseNameFromURI(URI);
                            DwbServiceEnums.DwbService service = DwbServiceEnums.DwbService.None;
                            if (Enum.TryParse(ServiceName, true, out DwbServiceEnums.DwbService result))
                            {
                                service = result;
                            }

                            currentDwbService = service;
                            try
                            {

                                this.splitContainerUnit.Panel1Collapsed = true;
                                this.splitContainerUnit.Panel2Collapsed = false;
                                this._URI = URI;
                                this.checkBoxDisplayWebsite.Visible = true;
                                this.checkBoxDisplayWebsite.Checked = true;
                                this.Load += FormRemoteQuery_Load;
                            }
                            catch (NotSupportedException notsupported)
                            {
                                MessageBox.Show("The webservice is not available. Message: " + notsupported.Message);
                                ExceptionHandling.WriteToErrorLogFile("FormRemoteQuery, notsupported exception: " + notsupported);
                            }

                            break;
                        case WorkbenchUnit.ServiceType.Unknown:
                            break;
                        default:
                            break;
                    }

                    if (this._ServiceType != WorkbenchUnit.ServiceType.WorkbenchModule)
                        this.setTitle(URI);

                    return;

                    foreach (System.Collections.Generic.KeyValuePair<string, string> k in this._IWorkbenchUnit.DatabaseAndServiceURIs())
                    {
                        if (URI.ToUpper().StartsWith(k.Value.ToUpper()))
                        {
                            this._BaseURI = k.Value;
                            this._ServerConnection.DatabaseName = k.Key;
                            this.setTitle();
                            break;
                        }
                    }
                    if (this._BaseURI != null
                        && this._BaseURI.Length > 0
                        && DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._IWorkbenchUnit.ServiceName()].DataBaseURIs().ContainsValue(this._BaseURI))
                    {
                        IDinURI = URI.Substring(this._BaseURI.Length);
                        if (int.TryParse(IDinURI, out ID))
                        {
                            this.setUnitPanel(IWorkbenchUnit);
                            this.InitForm(ID);
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Could not parse URI " + URI);
                            this.Close();
                        }
                    }
                }

                else
                {
                    this.setUnitPanel(IWorkbenchUnit);
                    this.InitForm(ID);
                }
            }
            catch (Exception ex)
            {
                this.TopMost = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private async void FormRemoteQuery_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api =
                    DwbServiceProviderAccessor.GetDwbWebservice(currentDwbService);

                if (_api == null || string.IsNullOrEmpty(URI))
                {
                    MessageBox.Show("No webservice defined");
                    return;
                }

                this._BaseURI = _api.GetBaseAddress();
                if (this._DatabaseService == null)
                    this._DatabaseService = new DatabaseService();
                this._DatabaseService.WebService = currentDwbService;
                var tt = await _api.CallWebServiceAsync<object>(URI,
                    DwbServiceEnums.HttpAction.GET);
                if (tt != null)
                {
                    var entityModel = _api.GetDwbApiDetailModel(tt);

                    DwbEntity clientEntity = _api.GetDwbApiDetailModel(tt);
                    BuildWebServiceUnitTree(URI, clientEntity);
                    this.setSourceURI(clientEntity._URL);
                }

                // this.InitForm(CoL.UriValues(URI));
                this._DatabaseService.IsWebservice = true;
                this.Load -= FormRemoteQuery_Load;
            }
            // here catch Webservice exceptions
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    "An error occurred when trying to connect with the webservice! One possibility is that the link used is outdated and no longer valid.\r\n\r\n" +
                    "\r\n Returned error message:" + ex.Message, "Failed connection to web service",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandling.WriteToErrorLogFile(
                    "FormRemoteQuery - FormRemoteQuery_Load, Exception exception: " +
                    ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public FormRemoteQuery(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, Microsoft.Data.SqlClient.SqlConnection Connection)
            : this(IWorkbenchUnit)
        {
            this.ServerConnection.DatabaseName = Connection.Database;
            this.ServerConnection.DatabaseServer = Connection.DataSource;
            foreach (System.Collections.Generic.KeyValuePair<string, string> D in IWorkbenchUnit.DatabaseAndServiceURIs())
            {
                if (this.ServerConnection.DatabaseName.StartsWith(D.Key))
                {
                    this._BaseURI = D.Value;
                    break;
                }
            }
        }

        public FormRemoteQuery(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, DiversityWorkbench.ServerConnection SC)
            : this(IWorkbenchUnit)
        {
            this.ServerConnection = SC;
            if (this.ServerConnection.ModuleName == "DiversityCollectionCache")
            {
                for (int i = 0; i < this.comboBoxDatabase.Items.Count; i++)//  System.Object o in this.comboBoxCacheDBSource.Items)
                {
                    if (this.comboBoxDatabase.Items[i].ToString() == "CacheDB")
                    {
                        this.comboBoxDatabase.SelectedIndex = i;
                        break;
                    }
                }
                if (SC.Project != null && SC.Project.Length > 0)
                {
                    for (int i = 0; i < this.comboBoxCacheDBSource.Items.Count; i++)
                    {
                        if (this.comboBoxCacheDBSource.Items[i].ToString() == SC.Project)
                        {
                            this.comboBoxCacheDBSource.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        public FormRemoteQuery(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, string ListInDatabase)
        {
            InitializeComponent();
            this._PartialService = ListInDatabase;
            this.labelDatabase.Text = "List:";
            bool ConnectToLocalServer = true;
            this.ServerConnection = IWorkbenchUnit.getServerConnection();
            this._IWorkbenchUnit = IWorkbenchUnit;
            if (_IWorkbenchUnit is WorkbenchUnit)
            {
                _HtmlAvailable = true; //(_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.HtmlUnitValues);
                _AdditionalAvailable = (_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.AdditionalUnitValues);
            }
            //if (ListInDatabase.Trim().Length > 0)
            bool OK = ListInDatabase.Length == 0;
            {
                foreach (DiversityWorkbench.DatabaseService D in this._IWorkbenchUnit.DatabaseServices())
                {
                    if (ListInDatabase.Trim().Length > 0 && D.ListName.Trim() == ListInDatabase.Trim())
                    {
                        //this.userControlQueryList.QueryRestriction = D.RestrictionForListInDatabase;
                        this.userControlQueryList.setQueryRestriction(D.RestrictionForListInDatabase, "#");
                        this._IWorkbenchUnit.setDatabaseServiceRestriction(D.RestrictionForListInDatabase);
                        this.setTitle();
                        this._DatabaseService = new DatabaseService();
                        this._DatabaseService.ListName = ListInDatabase.Trim();
                        DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.DatabaseName = D.DisplayText;
                        DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Save();
                        OK = true;
                        break;
                    }
                    else if (ListInDatabase.Trim().Length == 0)
                    {
                        this.userControlQueryList.setQueryRestriction(D.RestrictionForListInDatabase, "#");
                        this._IWorkbenchUnit.setDatabaseServiceRestriction(D.RestrictionForListInDatabase);
                        this.setTitle();
                        this._DatabaseService = new DatabaseService();
                        this._DatabaseService.ListName = ListInDatabase.Trim();
                        DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.DatabaseName = D.DisplayText;
                        DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Save();
                        OK = true;
                        break;
                    }
                }
            }
            if (!OK)
            {
                foreach (DiversityWorkbench.DatabaseService D in this._IWorkbenchUnit.DatabaseServices())
                {
                    if (ListInDatabase.Trim().Length > 0 && D.ListName.Trim().ToLower() == ListInDatabase.Trim().ToLower())
                    {
                        this.userControlQueryList.setQueryRestriction(D.RestrictionForListInDatabase, "#");
                        this._IWorkbenchUnit.setDatabaseServiceRestriction(D.RestrictionForListInDatabase);
                        this.setTitle();
                        this._DatabaseService = new DatabaseService();
                        this._DatabaseService.ListName = D.ListName.Trim();
                        DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.DatabaseName = D.DisplayText;
                        DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Save();
                        OK = true;
                        break;
                    }
                }
            }
            if (!OK)
            {

            }
            //else
            //{
            //}
            this.InitForm();
            DiversityWorkbench.DatabaseService _DS = new DatabaseService();

            System.Collections.Generic.List<DiversityWorkbench.DatabaseService> Services = this._IWorkbenchUnit.DatabaseServices();
            if (Services.Count > 1)
            {
                foreach (DiversityWorkbench.DatabaseService D in Services)
                {
                    if (ListInDatabase.Trim().Length > 0 && ListInDatabase.Trim().ToLower() == D.ListName.Trim().ToLower())
                    {
                        this.comboBoxDatabase.Items.Add(D.DisplayText);
                    }
                    else if (ListInDatabase.Trim().Length == 0)
                    {
                        this.comboBoxDatabase.Items.Add(D.DisplayText);
                    }
                }
                this.InitDatabaseMenu(ListInDatabase);
                this.splitContainerQueries.Enabled = false;
                if (DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.DatabaseName.Length > 0)
                {
                    for (int i = 0; i < this.comboBoxDatabase.Items.Count; i++)
                    {
                        if (this.comboBoxDatabase.Items[i].ToString() == DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.DatabaseName)
                        {
                            this.comboBoxDatabase.SelectedIndex = i;
                            _DS = this._IWorkbenchUnit.DatabaseServices()[i];
                            break;
                        }
                    }
                }
                else
                    _DS = new DatabaseService("");
                if (_DS.DatabaseName == null) _DS.DatabaseName = IWorkbenchUnit.getServerConnection().DatabaseName;
                if (_DS.DatabaseName.Length == 0 || _DS.IsWebservice) ConnectToLocalServer = false;
            }


            //if (this._IWorkbenchUnit.DatabaseServices().Count > 1)
            //{
            //    foreach (DiversityWorkbench.DatabaseService D in this._IWorkbenchUnit.DatabaseServices())
            //    {
            //        if (ListInDatabase.Trim().Length > 0 && ListInDatabase.Trim() == D.ListName.Trim())
            //        {
            //            this.comboBoxDatabase.Items.Add(D.DisplayText);
            //        }
            //        else if (ListInDatabase.Trim().Length == 0)
            //        {
            //            this.comboBoxDatabase.Items.Add(D.DisplayText);
            //        }
            //    }
            //    this.InitDatabaseMenu(ListInDatabase);
            //    this.splitContainerQueries.Enabled = false;
            //    if (DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.DatabaseName.Length > 0)
            //    {
            //        for (int i = 0; i < this.comboBoxDatabase.Items.Count; i++)
            //        {
            //            if (this.comboBoxDatabase.Items[i].ToString() == DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.DatabaseName)
            //            {
            //                this.comboBoxDatabase.SelectedIndex = i;
            //                _DS = this._IWorkbenchUnit.DatabaseServices()[i];
            //                break;
            //            }
            //        }
            //    }
            //    else
            //        _DS = new DatabaseService("");
            //    if (_DS.DatabaseName == null) _DS.DatabaseName = IWorkbenchUnit.getServerConnection().DatabaseName;
            //    if (_DS.DatabaseName.Length == 0 || _DS.IsWebservice) ConnectToLocalServer = false;
            //}
            else
            {
                this.splitContainerQueries.Panel2Collapsed = true;
            }
            if (this._ServerConnection != null && ConnectToLocalServer && !_DS.IsForeignSource)
            {
                try
                {
                    this.userControlQueryList.setConnection(this._ServerConnection.ConnectionString, IWorkbenchUnit.MainTable());
                    this.userControlQueryList.setQueryConditions(IWorkbenchUnit.QueryConditions());
                    this.userControlQueryList.QueryDisplayColumns = IWorkbenchUnit.QueryDisplayColumns();
                    this.checkModule();
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            //this.panelDatabase.Visible = false;
            this.splitContainerQueries.Panel1Collapsed = false;
            this.splitContainerQueries.Panel2Collapsed = true;
            this.splitContainerQueries.Enabled = true;
        }


        #region For Domains

        /// <summary>
        /// Addressing a domain of the workbench unit
        /// </summary>
        /// <param name="IWorkbenchUnit">The workbench unit</param>
        /// <param name="Domain">The name of the domain</param>
        /// <param name="RestrictToDomain">ignored so far</param>
        public FormRemoteQuery(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, string Domain, bool RestrictToDomain)
        {
            InitializeComponent();
            this._Domain = Domain;

            this.InitForm();
            bool ConnectToLocalServer = true;
            this.ServerConnection = IWorkbenchUnit.getServerConnection();
            this._IWorkbenchUnit = IWorkbenchUnit;
            if (_IWorkbenchUnit is WorkbenchUnit)
            {
                _HtmlAvailable = true; //(_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.HtmlUnitValues);
                _AdditionalAvailable = (_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.AdditionalUnitValues);
            }

            // the services
            DiversityWorkbench.DatabaseService _DS = new DatabaseService();
            if (this._IWorkbenchUnit.DatabaseServices().Count > 0)
            {
                this.panelDatabase.Visible = true;

                System.Collections.Generic.List<DatabaseService> L = this._IWorkbenchUnit.DatabaseServices();
                foreach (DatabaseService DS in L)
                {
                    if (comboBoxDatabase.Items.Contains(DS.DatabaseName))
                        continue;
                    this.comboBoxDatabase.Items.Add(DS.DatabaseName);
                }
                if (this._IWorkbenchUnit.ServerConnections() != null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> SC in this._IWorkbenchUnit.ServerConnections())
                    {
                        if (!this.comboBoxDatabase.Items.Contains(SC.Key))
                        {
                            this.comboBoxDatabase.Items.Add(SC.Key);
                        }
                    }
                }
                this.splitContainerQueries.Enabled = false;
                string Database = this.Databases[IWorkbenchUnit.getServerConnection().ModuleName];
                if (Database.Length > 0)
                {
                    for (int i = 0; i < this.comboBoxDatabase.Items.Count; i++)
                    {
                        if (this.comboBoxDatabase.Items[i].ToString() == Database)
                        {
                            this.comboBoxDatabase.SelectedIndex = i;
                            if (this._IWorkbenchUnit.DatabaseServices().Count > i)
                                _DS = this._IWorkbenchUnit.DatabaseServices()[i];
                            break;
                        }
                    }
                }
                else
                    _DS = new DatabaseService("");
                if (_DS.DatabaseName == null)
                    _DS.DatabaseName = IWorkbenchUnit.getServerConnection().DatabaseName;
                if (_DS.DatabaseName.Length == 0) // || _DS.IsWebservice)
                {
                    this.splitContainerQueries.Visible = false;
                }
                if (_DS.IsWebservice)
                {
                    ConnectToLocalServer = false;
                }
            }
            else
            {
                this.panelDatabase.Visible = false;
                this.splitContainerQueries.Panel2Collapsed = true;
            }

            if (this._ServerConnection != null && ConnectToLocalServer && !_DS.IsForeignSource)
            {
                try
                {
                    this.userControlQueryList.setConnection(this._ServerConnection.ConnectionString, Domain);// IWorkbenchUnit.MainTable());
                    this.userControlQueryList.setQueryConditions(IWorkbenchUnit.QueryConditions(Domain));
                    this.userControlQueryList.QueryDisplayColumns = IWorkbenchUnit.QueryDisplayColumns(Domain);
                    this.checkModule();
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }

            string Service = this._IWorkbenchUnit.ServiceName().ToLower();
            switch (Service)
            {
                case "diversitygazetteer":
                case "diversitysamplingplots":
                    this.splitContainerUnit.Panel1Collapsed = false;
                    this.splitContainerUnit.Panel2Collapsed = true;
                    break;
                default:
                    this.splitContainerUnit.Panel1Collapsed = false;
                    this.splitContainerUnit.Panel2Collapsed = true;
                    break;
            }
        }

        /// <summary>
        /// Addressing a list and a domain of the workbench unit
        /// </summary>
        /// <param name="IWorkbenchUnit">The workbench unit</param>
        /// <param name="ListInDatabase">The name of the list</param>
        /// <param name="Domain">The name of the domain</param>
        /// <param name="RestrictToDomain">ignored so far</param>
        public FormRemoteQuery(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, string ListInDatabase, string Domain, bool RestrictToDomain)
        {
            InitializeComponent();
            this._PartialService = ListInDatabase;
            this.labelDatabase.Text = "List:";
            bool ConnectToLocalServer = true;
            this.ServerConnection = IWorkbenchUnit.getServerConnection();
            this._IWorkbenchUnit = IWorkbenchUnit;
            if (_IWorkbenchUnit is WorkbenchUnit)
            {
                _HtmlAvailable = true; //(_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.HtmlUnitValues);
                _AdditionalAvailable = (_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.AdditionalUnitValues);
            }
            foreach (DiversityWorkbench.DatabaseService D in this._IWorkbenchUnit.DatabaseServices())
            {
                if (D.ListName == ListInDatabase)
                {
                    this.userControlQueryList.setQueryRestriction(D.RestrictionForListInDatabase, "#");
                    this._IWorkbenchUnit.setDatabaseServiceRestriction(D.RestrictionForListInDatabase);
                    this.setTitle();
                    this._DatabaseService = new DatabaseService();
                    this._DatabaseService.ListName = ListInDatabase;
                    DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.DatabaseName = D.DisplayText;
                    DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Save();
                    break;
                }
            }
            this.InitForm();
            DiversityWorkbench.DatabaseService _DS = new DatabaseService();
            if (this._IWorkbenchUnit.DatabaseServices().Count > 1)
            {
                this.panelDatabase.Visible = true;
                foreach (DiversityWorkbench.DatabaseService D in this._IWorkbenchUnit.DatabaseServices())
                {
                    if (ListInDatabase == D.ListName)
                    {
                        this.comboBoxDatabase.Items.Add(D.DisplayText);
                    }
                }
                this.splitContainerQueries.Enabled = false;
                if (DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.DatabaseName.Length > 0)
                {
                    for (int i = 0; i < this.comboBoxDatabase.Items.Count; i++)
                    {
                        if (this.comboBoxDatabase.Items[i].ToString() == DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.DatabaseName)
                        {
                            this.comboBoxDatabase.SelectedIndex = i;
                            _DS = this._IWorkbenchUnit.DatabaseServices()[i];
                            break;
                        }
                    }
                }
                else
                    _DS = new DatabaseService("");
                if (_DS.DatabaseName == null) _DS.DatabaseName = IWorkbenchUnit.getServerConnection().DatabaseName;
                if (_DS.DatabaseName.Length == 0 || _DS.IsWebservice) ConnectToLocalServer = false;
            }
            else
            {
                this.panelDatabase.Visible = false;
                this.splitContainerQueries.Panel2Collapsed = true;
            }
            if (this._ServerConnection != null && ConnectToLocalServer && !_DS.IsForeignSource)
            {
                try
                {
                    this.userControlQueryList.setConnection(this._ServerConnection.ConnectionString, IWorkbenchUnit.MainTable());
                    this.userControlQueryList.setQueryConditions(IWorkbenchUnit.QueryConditions(Domain));
                    this.userControlQueryList.QueryDisplayColumns = IWorkbenchUnit.QueryDisplayColumns(Domain);
                    this.checkModule();
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            this.splitContainerQueries.Panel1Collapsed = false;
            this.splitContainerQueries.Panel2Collapsed = true;
            this.splitContainerQueries.Enabled = true;
        }

        public FormRemoteQuery(string URI, DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, string Domain, bool RestrictToDomain)
        {
            InitializeComponent();
            this._ReadOnly = true;
            this.splitContainerMain.Panel1Collapsed = true;
            this.userControlDialogPanel.Visible = false;
            this.ShowModuleConnection = false;
            try
            {
                this.ServerConnection = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URI);
                if (this.ServerConnection != null)
                    IWorkbenchUnit.setServerConnection(this.ServerConnection);
            }
            catch (System.Exception ex) { }
            if (this.ServerConnection == null)
                this.ServerConnection = IWorkbenchUnit.getServerConnection();
            this._IWorkbenchUnit = IWorkbenchUnit;
            if (_IWorkbenchUnit is WorkbenchUnit)
            {
                _HtmlAvailable = true; //(_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.HtmlUnitValues);
                _AdditionalAvailable = (_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.AdditionalUnitValues);
            }
            try
            {
                string IDinURI = "";
                int ID;
                if (!int.TryParse(URI, out ID))
                {
                    // get the type of the Service
                    this._ServiceType = DiversityWorkbench.WorkbenchUnit.getServiceType(URI);
                    // TODO Ariane implement Webservice here? 
                    switch (this._ServiceType)
                    {
                        case WorkbenchUnit.ServiceType.WorkbenchModule:
                            if (this.ServerConnection != null)
                                this._BaseURI = this.ServerConnection.BaseURL;
                            else
                                this._BaseURI = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[DiversityWorkbench.WorkbenchUnit.getServiceNameFromURI(URI)].DataBaseURIs()[DiversityWorkbench.WorkbenchUnit.getDatabaseNameFromURI(URI)];
                            IDinURI = URI.Substring(this._BaseURI.Length + Domain.Length + 1);
                            if (int.TryParse(IDinURI, out ID))
                            {
                                this._ServerConnection.DatabaseName = DiversityWorkbench.WorkbenchUnit.getDatabaseNameFromURI(URI);
                                this.setUnitPanel(IWorkbenchUnit);
                                this.InitForm(ID);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("Could not parse URI " + URI);
                                this.Close();
                            }
                            this.splitContainerUnit.Panel1Collapsed = false;
                            this.splitContainerUnit.Panel2Collapsed = true;
                            break;
                        //case WorkbenchUnit.ServiceType.IndexFungorum:
                        //    this.splitContainerUnit.Panel1Collapsed = false;
                        //    this.splitContainerUnit.Panel2Collapsed = true;
                        //    this.setUnitPanel(IWorkbenchUnit);
                        //    this.setUnitPanel(IWorkbenchUnit);
                        //    DiversityWorkbench.WebserviceIndexFungorum WIF = new WebserviceIndexFungorum();
                        //    this.InitForm(WIF.UriValues(URI));
                        //    System.Uri u = new Uri(DiversityWorkbench.WebserviceIndexFungorum.UriBaseWeb);
                        //    System.Uri uName = new Uri(DiversityWorkbench.WebserviceIndexFungorum.UriBaseNamesRecord + DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI));
                        //    //this.webBrowserURI.Url = uName;
                        //    this.setSourceURI(uName.ToString());
                        //    //this.TopMost = true;
                        //    break;
                        //case WorkbenchUnit.ServiceType.Tropicos:
                        //    this.splitContainerUnit.Panel1Collapsed = false;
                        //    this.splitContainerUnit.Panel2Collapsed = true;
                        //    this.setUnitPanel(IWorkbenchUnit);
                        //    this.setUnitPanel(IWorkbenchUnit);
                        //    DiversityWorkbench.WebserviceTropicos WTro = new WebserviceTropicos();
                        //    this._BaseURI = WTro.UriXmlRecord();
                        //    this._BaseUriSuffix = WTro.UriXmlSuffix();
                        //    this.InitForm(WTro.UriValues(URI));
                        //    System.Uri uTro = new Uri(DiversityWorkbench.WebserviceTropicos.UriBaseWeb);
                        //    System.Uri uTroName = new Uri(DiversityWorkbench.WebserviceTropicos.UriBaseNamesRecord + DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI));
                        //    this.setSourceURI(uTroName.ToString());
                        //    break;
                        //case WorkbenchUnit.ServiceType.WoRMS:
                        //    this.splitContainerUnit.Panel1Collapsed = false;
                        //    this.splitContainerUnit.Panel2Collapsed = true;
                        //    this.setUnitPanel(IWorkbenchUnit);
                        //    this.setUnitPanel(IWorkbenchUnit);
                        //    DiversityWorkbench.WebserviceWoRMS WWo = new WebserviceWoRMS();
                        //    this.InitForm(WWo.UriValues(URI));
                        //    System.Uri uWo = new Uri(DiversityWorkbench.WebserviceWoRMS.UriBaseWeb);
                        //    System.Uri uWoName = new Uri(DiversityWorkbench.WebserviceWoRMS.UriBaseNamesRecord + DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI));
                        //    this.setSourceURI(uWoName.ToString());
                        //    break;
                        //case WorkbenchUnit.ServiceType.CatalogueOfLife:
                        //    this._URI = URI;
                        //    this.splitContainerURI.Panel1Collapsed = false;
                        //    this.splitContainerURI.Panel2Collapsed = true;
                        //    DiversityWorkbench.CatalogueOfLifeWebservice C = new CatalogueOfLifeWebservice();
                        //    URI += C.UriXmlSuffix();
                        //    this._BaseURI = C.UriXmlRecord();
                        //    if (this._DatabaseService == null)
                        //        this._DatabaseService = new DatabaseService();
                        //    this._DatabaseService.WebService = Webservice.WebServices.CatalogueOfLife;
                        //    this._DatabaseService.IsWebservice = true;
                        //    C.BuildUnitTree(C.CorrectedURI(URI), this.treeViewUnit); // TONI 20140430
                        //    this.splitContainerUnit.Panel1Collapsed = true;
                        //    this.splitContainerUnit.Panel2Collapsed = false;
                        //    if (DiversityWorkbench.Webservice.UriCurrentHtmlRecord.Length > 0)
                        //    {
                        //        try
                        //        {
                        //            System.Uri U = new Uri(C.CorrectedURI(DiversityWorkbench.Webservice.UriCurrentHtmlRecord)); // TONI 20140430
                        //            //this.webBrowserURI.Url = U;
                        //            this.setSourceURI(U.ToString());
                        //            this.splitContainerURI.Panel2Collapsed = false;
                        //        }
                        //        catch
                        //        {
                        //            this.splitContainerURI.Panel2Collapsed = true;
                        //        }
                        //    }
                        //    //this.TopMost = true;
                        //    break;
                        //case WorkbenchUnit.ServiceType.CatalogueOfLife_2:
                        //    this._URI = URI;
                        //    this.splitContainerURI.Panel1Collapsed = false;
                        //    this.splitContainerURI.Panel2Collapsed = true;
                        //    DiversityWorkbench.CatalogueOfLife_2 H = new CatalogueOfLife_2();
                        //    URI += H.UriXmlSuffix();
                        //    this._BaseURI = H.UriXmlRecord();
                        //    if (this._DatabaseService == null)
                        //        this._DatabaseService = new DatabaseService();
                        //    this._DatabaseService.WebService = Webservice.WebServices.CatalogueOfLife_2;
                        //    this._DatabaseService.IsWebservice = true;
                        //    H.BuildUnitTree(H.CorrectedURI(URI), this.treeViewUnit); // TONI 20140430
                        //    this.splitContainerUnit.Panel1Collapsed = true;
                        //    this.splitContainerUnit.Panel2Collapsed = false;
                        //    if (DiversityWorkbench.Webservice.UriCurrentHtmlRecord.Length > 0)
                        //    {
                        //        try
                        //        {
                        //            System.Uri U = new Uri(H.CorrectedURI(DiversityWorkbench.Webservice.UriCurrentHtmlRecord)); // TONI 20140430
                        //            //this.webBrowserURI.Url = U;
                        //            this.setSourceURI(U.ToString());
                        //            this.splitContainerURI.Panel2Collapsed = false;
                        //        }
                        //        catch
                        //        {
                        //            this.splitContainerURI.Panel2Collapsed = true;
                        //        }
                        //    }
                        //    //this.TopMost = true;
                        //    //this.checkBoxDisplayWebsite.Checked = false;
                        //    break;
                        //case WorkbenchUnit.ServiceType.PalaeoDB:
                        //    this.splitContainerURI.Panel1Collapsed = false;
                        //    this.splitContainerURI.Panel2Collapsed = false;
                        //    this.splitContainerUnit.Panel1Collapsed = true;
                        //    this.splitContainerUnit.Panel2Collapsed = false;
                        //    DiversityWorkbench.PalaeoDB PalaeoDB = new PalaeoDB();
                        //    this._BaseURI = PalaeoDB.UriXmlRecord();
                        //    if (this._DatabaseService == null)
                        //        this._DatabaseService = new DatabaseService();
                        //    this._DatabaseService.WebService = Webservice.WebServices.PalaeoDB;
                        //    this._DatabaseService.IsWebservice = true;
                        //    this._URI = URI;
                        //    URI += PalaeoDB.UriXmlSuffix();
                        //    PalaeoDB.BuildUnitTree(URI, this.treeViewUnit);
                        //    System.Uri uP = new Uri(DiversityWorkbench.PalaeoDB.UriBaseWeb);
                        //    System.Uri uNameP = new Uri(DiversityWorkbench.PalaeoDB.UriBaseNamesRecord + DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI));
                        //    //this.webBrowserURI.Url = uNameP;
                        //    this.setSourceURI(uNameP.ToString());
                        //    //this.TopMost = true;
                        //    break;
                        //case WorkbenchUnit.ServiceType.PESI:
                        //    this.splitContainerURI.Panel1Collapsed = false;
                        //    this.splitContainerURI.Panel2Collapsed = false;
                        //    this.splitContainerUnit.Panel1Collapsed = true;
                        //    this.splitContainerUnit.Panel2Collapsed = false;
                        //    DiversityWorkbench.WebservicePESI PESI = new WebservicePESI();
                        //    this._BaseURI = PESI.UriXmlRecord();
                        //    if (this._DatabaseService == null)
                        //        this._DatabaseService = new DatabaseService();
                        //    this._DatabaseService.WebService = Webservice.WebServices.PESI;
                        //    this._DatabaseService.IsWebservice = true;
                        //    this._URI = URI;
                        //    URI += PESI.UriXmlSuffix();
                        //    PESI.BuildUnitTree(URI, this.treeViewUnit);
                        //    System.Uri uPESI = new Uri(DiversityWorkbench.WebservicePESI.UriBaseWeb);
                        //    System.Uri uNamePESI = new Uri(DiversityWorkbench.WebservicePESI.UriBaseNamesRecord + DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI));
                        //    this.setSourceURI(uNamePESI.ToString());
                        //    break;
                        //case WorkbenchUnit.ServiceType.GfbioRecordbasis:
                        //    this.splitContainerURI.Panel1Collapsed = false;
                        //    this.splitContainerURI.Panel2Collapsed = false;
                        //    this.splitContainerUnit.Panel1Collapsed = true;
                        //    this.splitContainerUnit.Panel2Collapsed = false;
                        //    DiversityWorkbench.WebserviceGfbioRecordBasis gT = new WebserviceGfbioRecordBasis();
                        //    this._BaseURI = gT.UriXmlRecord();
                        //    if (this._DatabaseService == null)
                        //        this._DatabaseService = new DatabaseService();
                        //    this._DatabaseService.WebService = Webservice.WebServices.gfbioRecordbasis;
                        //    this._DatabaseService.IsWebservice = true;
                        //    this._URI = URI;
                        //    URI += gT.UriXmlSuffix();
                        //    gT.BuildUnitTree(URI, this.treeViewUnit);
                        //    System.Uri ugT = new Uri(DiversityWorkbench.WebserviceGfbioRecordBasis.UriBaseWeb);
                        //    System.Uri uNamegT = new Uri(DiversityWorkbench.WebserviceGfbioRecordBasis.UriBaseNamesRecord + DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI));
                        //    this.setSourceURI(uNamegT.ToString());
                        //    break;
                        //case WorkbenchUnit.ServiceType.RLL:
                        //    this.splitContainerUnit.Panel1Collapsed = false;
                        //    this.splitContainerUnit.Panel2Collapsed = true;
                        //    this.setUnitPanel(IWorkbenchUnit);
                        //    this.setUnitPanel(IWorkbenchUnit);
                        //    DiversityWorkbench.WebserviceRLL RLL = new WebserviceRLL();
                        //    this._BaseURI = RLL.UriXmlRecord();
                        //    this._DatabaseService.WebService = Webservice.WebServices.RLL;
                        //    this._DatabaseService.IsWebservice = true;
                        //    this._URI = URI;
                        //    this.checkBoxDisplayWebsite.Checked = false;




                        //    this.InitForm(RLL.UriValues(URI));
                        //    System.Uri uRLL = new Uri(DiversityWorkbench.WebserviceRLL.UriBaseWeb);
                        //    System.Uri uRLLName = new Uri(DiversityWorkbench.WebserviceRLL.UriBaseNamesRecord + DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI));
                        //    this.setSourceURI(uRLLName.ToString());
                        //    break;
                        case WorkbenchUnit.ServiceType.Unknown:
                            break;
                        default:
                            break;
                    }

                    if (this._ServiceType != WorkbenchUnit.ServiceType.WorkbenchModule)
                        this.setTitle(URI);

                    return;

                    foreach (System.Collections.Generic.KeyValuePair<string, string> k in this._IWorkbenchUnit.DatabaseAndServiceURIs())
                    {
                        if (URI.ToUpper().StartsWith(k.Value.ToUpper()))
                        {
                            this._BaseURI = k.Value;
                            this._ServerConnection.DatabaseName = k.Key;
                            this.setTitle();
                            break;
                        }
                    }
                    if (this._BaseURI != null
                        && this._BaseURI.Length > 0
                        && DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._IWorkbenchUnit.ServiceName()].DataBaseURIs().ContainsValue(this._BaseURI))
                    {
                        IDinURI = URI.Substring(this._BaseURI.Length);
                        if (int.TryParse(IDinURI, out ID))
                        {
                            this.setUnitPanel(IWorkbenchUnit);
                            this.InitForm(ID);
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Could not parse URI " + URI);
                            this.Close();
                        }
                    }
                }
                else
                {
                    this.setUnitPanel(IWorkbenchUnit);
                    this.InitForm(ID);
                }
            }
            catch (Exception ex)
            {
                this.TopMost = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        #endregion

        // Ariane is this constructor needed?
        //public FormRemoteQuery(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, DiversityWorkbench.Webservice.WebServices Service)
        //{
        //    InitializeComponent();
        //    this.InitForm();
        //    this.splitContainerQueries.Panel1Collapsed = true;
        //    this.splitContainerURI.Panel2Collapsed = true;
        //    this.panelDatabase.Visible = false;
        //    this.ShowModuleConnection = false;
        //    this.splitContainerMain.Panel2Collapsed = false;
        //    this._DatabaseService = new DatabaseService();
        //    this._DatabaseService.IsWebservice = true;
        //    this._IWorkbenchUnit = IWorkbenchUnit;
        //    try
        //    {
        //        this.initWebserviceControls(Service);

        //    }
        //    catch (Exception ex)
        //    {
        //        this.TopMost = false;
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }

        //}

        #endregion

        private void ListMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.comboBoxDatabase.Items.Count; i++)
            {
                string ComboboxItem = this.comboBoxDatabase.Items[i].ToString();
                string MenuItem = sender.ToString();
                if (this.comboBoxDatabase.Items[i].ToString().StartsWith(sender.ToString()) || this.comboBoxDatabase.Items[i].ToString().IndexOf(sender.ToString()) > -1)
                {
                    if (sender.GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
                    {
                        System.Windows.Forms.ToolStripMenuItem t = (System.Windows.Forms.ToolStripMenuItem)sender;
                        if (t.Tag != null)
                        {
                            if (t.Tag.ToString() == this.comboBoxDatabase.Items[i].ToString())
                            {
                                this.comboBoxDatabase.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void InitDatabaseMenu(string ListInDatabase)
        {
            try
            {
                // Markus 22.2.2022: Test auf this.comboBoxDatabase.Items.Count
                if (this._IWorkbenchUnit.DatabaseServices().Count > 1 || this.comboBoxDatabase.Items.Count > 0)
                {
                    this.panelDatabase.Visible = true;
                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, string>>> Lists = new SortedDictionary<string, SortedDictionary<string, SortedDictionary<string, string>>>();
                    foreach (DiversityWorkbench.DatabaseService D in this._IWorkbenchUnit.DatabaseServices())
                    {
                        //if (D.WebService == Webservice.WebServices.TNT)
                        //    continue;

                        if (D.IsWebservice)
                        {
                            if (!Lists.ContainsKey("Webservices"))
                            {
                                System.Collections.Generic.SortedDictionary<string, string> L = new SortedDictionary<string, string>();
                                System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, string>> Dict = new SortedDictionary<string, SortedDictionary<string, string>>();
                                L.Add(D.ListName, D.DisplayText);
                                Dict.Add(D.DatabaseName, L);
                                Lists.Add("Webservices", Dict);
                            }
                            else
                            {
                                if (!Lists["Webservices"].ContainsKey(D.DatabaseName))
                                {
                                    System.Collections.Generic.SortedDictionary<string, string> L = new SortedDictionary<string, string>();
                                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>> Dict = new SortedDictionary<string, List<string>>();
                                    L.Add(D.ListName, D.DisplayText);
                                    Lists["Webservices"].Add(D.DatabaseName, L);
                                }
                                else
                                {
                                    Lists["Webservices"][D.DatabaseName].Add(D.ListName, D.DisplayText);
                                }
                            }
                        }
                        else
                        {
                            if (D.Server.Length > 0)
                            {

                            }
                            // getting the name of the server
                            string LocalMachine = System.Windows.Forms.SystemInformation.ComputerName.ToString();
                            string LocalDbServer = DiversityWorkbench.Settings.DatabaseServer;
                            bool IsIP = false;
                            System.Net.IPAddress Address;
                            if (System.Net.IPAddress.TryParse(LocalDbServer, out Address))
                                IsIP = true;
                            string Server = "";
                            if (D.LinkedServer != null && D.LinkedServer.Length > 0)
                                Server = D.LinkedServer.Substring(0, D.LinkedServer.IndexOf("."));
                            else if (D.DisplayText.IndexOf("[") > -1 && D.DisplayText.IndexOf("]") > -1 && D.DisplayText.IndexOf(".") > -1)
                            {
                                Server = D.DisplayText.Substring(1, D.DisplayText.IndexOf(".") - 1);
                            }
                            else if (DiversityWorkbench.Settings.DatabaseServer == "127.0.0.1")
                            {
                                Server = System.Windows.Forms.SystemInformation.ComputerName.ToString();
                            }
                            else if (IsIP)
                            {
                                Server = LocalDbServer;
                            }
                            else if (D.Server.Length > 0 && !D.IsDatabaseOnLinkedServer && D.Server.ToLower().EndsWith(".diversityworkbench.de"))
                            {
                                Server = D.Server.Substring(0, D.Server.IndexOf(".")).ToUpper();
                            }
                            else if (DiversityWorkbench.Settings.DatabaseServer.IndexOf(".") > -1) // Markus 22.4.22 - Bugfix for local servers
                            {
                                Server = DiversityWorkbench.Settings.DatabaseServer.Substring(0, 1).ToUpper() + DiversityWorkbench.Settings.DatabaseServer.Substring(1, DiversityWorkbench.Settings.DatabaseServer.IndexOf(".") - 1);
                            }
                            else
                            {
                                try
                                {
                                    Server = DiversityWorkbench.Settings.DatabaseServer;
                                    //Server = DiversityWorkbench.Settings.DatabaseServer.Substring(0, 1).ToUpper() + DiversityWorkbench.Settings.DatabaseServer.Substring(1, DiversityWorkbench.Settings.DatabaseServer.IndexOf(".") - 1);
                                }
                                catch (System.Exception ex)
                                {
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                }
                            }

                            // getting the name of the database
                            string Database = D.DatabaseName;
                            if (Database.IndexOf("[") > -1 && Database.IndexOf("]") > -1 && Database.IndexOf(".") > -1)
                                Database = Database.Substring(Database.IndexOf("].") + 2);
                            try
                            {
                                if (!Lists.ContainsKey(Server))
                                {
                                    System.Collections.Generic.SortedDictionary<string, string> L = new SortedDictionary<string, string>();
                                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, string>> Dict = new SortedDictionary<string, SortedDictionary<string, string>>();
                                    if (ListInDatabase.Length > 0)
                                    {
                                        if (!L.ContainsKey(D.ListName))
                                            L.Add(D.ListName, D.DisplayText);
                                        else { }
                                    }
                                    else
                                    {
                                        if (!L.ContainsKey(D.ListName))
                                            L.Add(D.ListName, D.DisplayText);
                                        else
                                        {
                                            if (!L.ContainsKey(""))
                                                L.Add("", D.DisplayText);
                                            else { }
                                        }
                                    }
                                    Dict.Add(Database, L);
                                    Lists.Add(Server, Dict);
                                }
                                else
                                {
                                    if (!Lists[Server].ContainsKey(Database))
                                    {
                                        System.Collections.Generic.SortedDictionary<string, string> L = new SortedDictionary<string, string>();
                                        System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, string>> Dict = new SortedDictionary<string, SortedDictionary<string, string>>();
                                        if (ListInDatabase.Length > 0)
                                        {
                                            L.Add(D.ListName, D.DisplayText);

                                        }
                                        else
                                        {
                                            if (!L.ContainsKey(D.ListName))
                                                L.Add(D.ListName, D.DisplayText);
                                            else
                                                L.Add("", D.DisplayText);

                                        }
                                        Lists[Server].Add(Database, L);
                                    }
                                    else
                                    {
                                        if (ListInDatabase.Length > 0)
                                        {
                                            if (!Lists[Server][Database].ContainsKey(D.ListName))
                                                Lists[Server][Database].Add(D.ListName, D.DisplayText);
                                            else
                                            {

                                            }
                                        }
                                        else
                                        {
                                            if (!Lists[Server][Database].ContainsKey(D.ListName))
                                                Lists[Server][Database].Add(D.ListName, D.DisplayText);
                                            else if (!Lists[Server][Database].ContainsKey(""))
                                                Lists[Server][Database].Add("", D.DisplayText);
                                            else { }
                                        }
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                    if (Lists.Count > 0)
                    {
                        try
                        {
                            this.labelDatabase.Visible = false;
                            if (ListInDatabase.Length == 0)
                            {
                                this.toolStripMenuItemLists.Text = "";
                                this.panelDatabaseMenuStrip.Width = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(40);
                            }
                            this.panelDatabaseMenuStrip.Visible = true;
                            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, string>>> KV in Lists)
                            {
                                System.Drawing.Image I = DiversityWorkbench.Properties.Resources.ServerLinked;
                                if (KV.Key == "Webservices")
                                    I = DiversityWorkbench.ResourceWorkbench.Webservice16;
                                else if (DiversityWorkbench.Settings.DatabaseServer.IndexOf(".") > -1 && (KV.Key.ToLower() == DiversityWorkbench.Settings.DatabaseServer.ToLower().Substring(0, DiversityWorkbench.Settings.DatabaseServer.IndexOf(".")) ||
                                    KV.Key == System.Windows.Forms.SystemInformation.ComputerName.ToString() ||
                                    KV.Key == DiversityWorkbench.Settings.DatabaseServer))
                                    I = DiversityWorkbench.Properties.Resources.ServerIO;
                                else if (KV.Key.ToLower().StartsWith(System.Windows.Forms.SystemInformation.ComputerName.ToString().ToLower()) && KV.Key.ToLower().IndexOf("sqlexpress") > -1) // Markus 22.4.22: Local server
                                    I = DiversityWorkbench.Properties.Resources.Laptop;
                                System.Windows.Forms.ToolStripMenuItem ITEM = new ToolStripMenuItem(KV.Key, I);
                                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.SortedDictionary<string, string>> kkvv in KV.Value)
                                {
                                    System.Windows.Forms.ToolStripMenuItem item = null;
                                    if (KV.Key == "Webservices")
                                    {
                                        item = new ToolStripMenuItem(kkvv.Key, DiversityWorkbench.ResourceWorkbench.Webservice16, new EventHandler(ListMenuItem_Click));
                                        item.Tag = kkvv.Value[""];
                                    }
                                    else
                                    {
                                        item = null;
                                        if (ListInDatabase.Length > 0)
                                        {
                                            item = new ToolStripMenuItem(kkvv.Key, DiversityWorkbench.ResourceWorkbench.Database);
                                            foreach (System.Collections.Generic.KeyValuePair<string, string> L in kkvv.Value)
                                            {
                                                if (KV.Key.Length > 0)
                                                {
                                                    System.Windows.Forms.ToolStripMenuItem iitem = new ToolStripMenuItem(L.Key, DiversityWorkbench.Properties.Resources.NewForm, new EventHandler(ListMenuItem_Click));
                                                    iitem.Tag = L.Value;
                                                    item.DropDownItems.Add(iitem);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            item = new ToolStripMenuItem(kkvv.Key, DiversityWorkbench.ResourceWorkbench.Database, new EventHandler(ListMenuItem_Click));
                                            if (kkvv.Value.ContainsKey(""))
                                                item.Tag = kkvv.Value[""];
                                            else
                                            {
                                                foreach (System.Collections.Generic.KeyValuePair<string, string> L in kkvv.Value)
                                                {
                                                    if (KV.Key.Length > 0)
                                                    {
                                                        System.Windows.Forms.ToolStripMenuItem iitem = new ToolStripMenuItem(L.Key, DiversityWorkbench.Properties.Resources.NewForm, new EventHandler(ListMenuItem_Click));
                                                        iitem.Tag = L.Value;
                                                        item.DropDownItems.Add(iitem);
                                                    }
                                                }
                                            }
                                        }
                                        item.ImageScaling = ToolStripItemImageScaling.None;
                                    }
                                    ITEM.DropDownItems.Add(item);
                                }
                                this.toolStripMenuItemLists.DropDownItems.Add(ITEM);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            this.panelDatabaseMenuStrip.Visible = false;
                            this.labelDatabase.Visible = true;
                        }
                    }

                }
                else
                {
                    this.panelDatabase.Visible = false;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region Form

        /// <summary>
        /// Keeping the setting for the optimizing in the main form, so it can be restored
        /// </summary>
        private bool _UseOptimizingInMainForm = false;
        private void InitStorageForOptimizingSetting() { this._UseOptimizingInMainForm = DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing; }

        private void InitForm()
        {
            // container for showing the details of a selected unit
            this.splitContainerUnitCollection.Panel2Collapsed = true;
            this.setQueryControlEvents();
            this.userControlQueryList.splitContainerMain.Orientation = Orientation.Vertical;
            this.userControlQueryList.toolStripButtonConnection.Visible = true;
            this.userControlQueryList.toolStripButtonCopy.Visible = false;
            this.userControlQueryList.toolStripButtonDelete.Visible = false;
            this.userControlQueryList.toolStripButtonNew.Visible = false;
            this.userControlQueryList.toolStripButtonSave.Visible = false;
            this.userControlQueryList.toolStripButtonUndo.Visible = false;
            this.userControlQueryList.toolStripSeparator1.Visible = false;
            // Markus 6.12.2019 - Aufteilung fuer hochaufloesende Panels korrigiert
            this.userControlQueryList.splitContainerMain.SplitterDistance = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected((int)(this.userControlQueryList.splitContainerMain.Width / 2));
            ///TODO: Markus 2018/10/17: hier muss bessere Loesung gefunden werden. Es gibt im Moment nur eine Variable die das Optimizing festlegt - fuer Hauptprogramm und alle RemoteQueries
            ///Anzeige kollidiert mit Zeile unten. Umschalten führt in Hauptprogramm zu anderem Abfrageverhalten wird aber nicht angezeigt, da nicht aktualisiert wird
            this.InitStorageForOptimizingSetting();
            //this.userControlQueryList.AllowOptimizing(false); // 

            this.splitContainerURI.Panel2Collapsed = true;
            if (DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Width > 0 && DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Height > 0)
            {
                this.Width = (int)(DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Width * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                this.Height = (int)(DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Height * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
            }
            this._AdaptFormSize = true;
            // #128
            //this.initGetAllServices();
        }

        private void InitForm(int ID)
        {
            try
            {
                if (this._ID != null && this._ID == -1) return;
                this.setUnit(ID); /// [a]

                int MinHeight = (int)((this.ShowModuleConnection ? 80 : 55) * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                {
                    MinHeight += C.Height;
                }
                if (!this.splitContainerURI.Panel2Collapsed)
                    MinHeight = MinHeight + this.splitContainerURI.Panel2.Height;
                if (MinHeight > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - 40)
                    this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - 40;
                else
                    this.Height = MinHeight;
                int MinWidth = (int)(700 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                int i = (int)(20 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                {
                    if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                    {
                        if (i < (int)(C.Text.Length * 6 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor))
                            i = (int)(C.Text.Length * 6 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    }
                }
                if (i < MinWidth)
                    this.Width = (int)(i + 50 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                if (this.Width < MinWidth) this.Width = MinWidth;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.InitStorageForOptimizingSetting();
        }

        private void InitForm(System.Collections.Generic.Dictionary<string, string> Values)
        {
            try
            {
                this.setUnitPanel(Values);

                int MinHeight = (int)(54 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                int MinWidth = (int)(400 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                int MaxHeight = (int)(700 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                {
                    MinHeight += C.Height;
                }
                if (MinHeight > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - 40)
                    this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - 40;
                else
                    this.Height = MinHeight;
                int i = (int)(20 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                {
                    if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                    {
                        if (i < (int)(C.Text.Length * 6 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor))
                            i = (int)(C.Text.Length * 6 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    }
                }
                if (i < MinWidth)
                    this.Width = i + 50;
                if (this.Width < MinWidth) this.Width = MinWidth;
                if (this.Height > MaxHeight) this.Height = MaxHeight;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.InitStorageForOptimizingSetting();
        }

        private void setTitle()
        {
            try
            {
                string Title = " " + this._ServerConnection.ModuleName;
                if (this._ServerConnection.DatabaseName != this._ServerConnection.ModuleName)
                    Title += "    (" + this._ServerConnection.DatabaseName + ")";
                if (this._ServerConnection.LinkedServer.Length > 0)
                    Title += "    Server: " + this._ServerConnection.LinkedServer;
                else
                    Title += "    Server: " + this._ServerConnection.DatabaseServer;
                if (this._ServerConnection.IsTrustedConnection)
                    Title += "    User: " + System.Environment.UserName.ToString();
                else
                {
                    if (this._ServerConnection.DatabaseUser.Length > 0) Title += "    User: " + this._ServerConnection.DatabaseUser;
                }
                if (this._PartialService != null && this._PartialService.Length > 0) Title = this._PartialService + " - " + Title;
                this.Text = Title;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        // only call this setTitle if the current service is not a WorkbenchModule ServiceType
        private void setTitle(string URI)
        {
            try
            {
                if (string.IsNullOrEmpty(URI))
                {
#if DEBUG
                    Console.WriteLine("Cannot setTitle of FormRemoteQuery, URI is null or empty");
#endif
                    return;
                }
                string Title = " " + this._ServerConnection.ModuleName;
                string Url = URI;

                if (Url.Length > 0)
                {
                    System.Uri U = new Uri(Url);
                    Title += ": " + U.Host;
                }
                this.Text = Title;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void checkModule()
        {
            try
            {
                string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.ApplicationDirectory() + "\\" + this.ServerConnection.ModuleName + ".exe";
                if (System.IO.File.Exists(Path))
                {
                    this.ShowModuleConnection = true;
                    this.labelOpenModule.Text = "open " + this._ServerConnection.ModuleName;
                }
                else
                    this.ShowModuleConnection = false;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setUnitPanel(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit)
        {
            this.tableLayoutPanelUnit.ColumnStyles.Clear();
            foreach (System.Windows.Forms.Control C in this.tableLayoutPanelUnit.Controls)
                C.Dispose();
            this.tableLayoutPanelUnit.Controls.Clear();

            this.tableLayoutPanelUnit.Height = (int)(22 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);

            int r = 0;
            int i = this.tableLayoutPanelUnit.Height;

            System.Windows.Forms.Label labelUnit = new Label();
            labelUnit.Text = IWorkbenchUnit.MainTable();
            labelUnit.Dock = DockStyle.Fill;
            labelUnit.TextAlign = ContentAlignment.BottomLeft;
            this.tableLayoutPanelUnit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelUnit.Controls.Add(labelUnit);
            this.tableLayoutPanelUnit.SetColumn(labelUnit, 0);
            this.tableLayoutPanelUnit.SetRow(labelUnit, r);
            r++;

            string Key = "";
            System.Collections.Generic.Dictionary<string, string> Values = IWorkbenchUnit.UnitValues(-1); /// [4] [C]
            if (_ShowAdditional)
                (this._IWorkbenchUnit as WorkbenchUnit).GetAdditionalUnitValues(Values);
            if (this._Domain != null && this._Domain.Length > 0)
                Values = IWorkbenchUnit.UnitValues(this._Domain, -1);
            foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
            {
                if (!P.Key.StartsWith("_"))
                {
                    if (Key != P.Key)
                    {
                        try
                        {

                            var label = new Label
                            {
                                Text = P.Key,
                                Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Bold),
                                ForeColor = System.Drawing.Color.Gray,
                                Dock = DockStyle.Top,
                                TextAlign = ContentAlignment.BottomLeft
                            };
                            this.panelUnit.Controls.Add(label);
                            label.BringToFront();
                            var textBox = new TextBox
                            {
                                Name = "textBox" + P.Key,
                                Dock = DockStyle.Top,
                                Height = (int)(39 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor),
                                ReadOnly = true,
                                TextAlign = HorizontalAlignment.Center,
                                BorderStyle = BorderStyle.None,
                                Multiline = true,
                                ScrollBars = ScrollBars.Vertical
                            };
                            this.panelUnit.Controls.Add(textBox);
                            textBox.BringToFront();
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    Key = P.Key;
                }
            }
            if (this._ServiceType == WorkbenchUnit.ServiceType.WorkbenchModule)
            {
                WorkbenchUnit wbu = IWorkbenchUnit as WorkbenchUnit;
                if (wbu != null)
                {
                    if (_HtmlAvailable)
                    {
                        System.Windows.Forms.Button H = new Button();
                        H.Text = "Show unit values as HTML ...";
                        H.Dock = DockStyle.Bottom;
                        H.Click += H_Click;
                        this.panelUnit.Controls.Add(H);
                        H.BringToFront();
                    }
                    if (!_ShowAdditional && _AdditionalAvailable)
                    {
                        System.Windows.Forms.Button B = new Button();
                        B.Text = "Include additional unit values ...";
                        B.Dock = DockStyle.Bottom;
                        B.Click += B_Click;
                        this.panelUnit.Controls.Add(B);
                        B.BringToFront();
                    }
                }
            }
        }

        private void B_Click(object sender, EventArgs e)
        {
            _ShowAdditional = true;
            System.Windows.Forms.Cursor lastCusor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            this.SuspendLayout();
            this.panelUnit.Hide();
            try
            {
                int oldHeight = this.Height;
                Point oldLocation = this.Location;
                System.Windows.Forms.Button B = sender as System.Windows.Forms.Button;
                this.panelUnit.Controls.Remove(B);
                B.Dispose();
                foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                    C.Dispose();
                this.panelUnit.Controls.Clear();
                this.setUnitPanel(_IWorkbenchUnit);
                this.InitForm(_SavedUnitID);
                int newY = (oldHeight - this.Height) / 2 + oldLocation.Y;
                if (newY < 0)
                    newY = 0;
                Point newLocation = new Point(oldLocation.X, newY);
                this.Location = newLocation;
            }
            catch (Exception)
            { }
            this.Cursor = lastCusor;
            this.panelUnit.Show();
            this.ResumeLayout();
        }

        private void H_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor lastCusor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            if (_IWorkbenchUnit is WorkbenchUnit)
            {
                string html = (_IWorkbenchUnit as WorkbenchUnit).HtmlUnitValues(_IWorkbenchUnit.UnitValues());
                //System.IO.FileInfo fi = new System.IO.FileInfo(WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\UnitValues.htm");
                System.IO.FileInfo fi = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\DiversityWorkbench\UnitValuesQuery.htm");
                using (System.IO.StreamWriter TxtWriter = new System.IO.StreamWriter(fi.FullName, false, Encoding.Unicode))
                {
                    TxtWriter.Write(html);
                    TxtWriter.Flush();
                    TxtWriter.Close();
                }
                DiversityWorkbench.Forms.FormWebBrowser F = new DiversityWorkbench.Forms.FormWebBrowser(fi.FullName);
                F.ShowExternal = true;
                F.ShowInTaskbar = this.ShowInTaskbar;
                F.ShowDialog();
            }
            this.Cursor = lastCusor;
        }

        private void setUnitPanel(string ID, DwbEntity clientModel)
        {
            BuildWebServiceUnitTree(ID, clientModel);
        }

        private void setUnitPanel(System.Data.DataRowView R)
        {
            foreach (System.Windows.Forms.Control C in this.tableLayoutPanelUnit.Controls)
                C.Dispose();
            this.tableLayoutPanelUnit.Controls.Clear();
            this.tableLayoutPanelUnit.ColumnStyles.Clear();

            foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                C.Dispose();
            this.panelUnit.Controls.Clear();

            this.tableLayoutPanelUnit.Height = (int)(22 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);

            int r = 0;
            int i = this.tableLayoutPanelUnit.Height;

            System.Windows.Forms.Label labelUnit = new Label();
            labelUnit.Text = this._IWorkbenchUnit.MainTable();
            labelUnit.Dock = DockStyle.Fill;
            labelUnit.TextAlign = ContentAlignment.BottomLeft;
            this.tableLayoutPanelUnit.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelUnit.Controls.Add(labelUnit);
            this.tableLayoutPanelUnit.SetColumn(labelUnit, 0);
            this.tableLayoutPanelUnit.SetRow(labelUnit, r);
            r++;

            string Column = "";
            //System.Collections.Generic.Dictionary<string, string> Values = IWorkbenchUnit.UnitValues(-1);
            if (R != null)
            {
                foreach (System.Data.DataColumn C in R.DataView.Table.Columns)
                {
                    if (!C.ColumnName.StartsWith("_")) //|| this._ServiceType == WorkbenchUnit.ServiceType.Mycobank)
                    {
                        if (Column != C.ColumnName && R[C.ColumnName].ToString() != "")
                        {
                            try
                            {
                                System.Windows.Forms.Label L = new Label();
                                L.Text = C.ColumnName;
                                L.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Bold);
                                L.ForeColor = System.Drawing.Color.Gray;
                                L.Dock = DockStyle.Top;
                                L.TextAlign = ContentAlignment.BottomLeft;
                                this.panelUnit.Controls.Add(L);
                                L.BringToFront();

                                System.Windows.Forms.TextBox T = new TextBox();
                                T.Name = "textBox" + C.ColumnName;
                                T.Dock = DockStyle.Top;
                                T.Height = (int)(39 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                                T.ReadOnly = true;
                                T.TextAlign = HorizontalAlignment.Center;
                                T.BorderStyle = BorderStyle.None;
                                T.Multiline = true;
                                T.ScrollBars = ScrollBars.Vertical;
                                this.panelUnit.Controls.Add(T);
                                T.BringToFront();
                                //T.ScrollBars = ScrollBars.None;

                                System.Windows.Forms.Control Cont;
                                // Ariane Webservices
                                //if (this.userControlWebservice.IWebservice.AdditionalControls(R)
                                //    .TryGetValue(C.ColumnName, out Cont))
                                //{
                                //    Cont.Dock = DockStyle.Top;
                                //    //Cont.Height = (int)(20 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                                //    this.panelUnit.Controls.Add(Cont);
                                //    Cont.BringToFront();
                                //    Cont.Click += new EventHandler(getRelatedData);
                                //}
                            }
                            catch (Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }

                        Column = C.ColumnName;
                    }
                }
            }
        }

        private void BuildWebServiceUnitTree<T>(string id, T clientModel) where T : DwbEntity
        {
            if (clientModel == null)
                throw new ArgumentNullException(nameof(clientModel));
            // Map the client model to its API entity model
            dynamic mappedClientModel = (clientModel as dynamic).GetMappedApiEntityModel();
            // Clear existing nodes
            treeViewUnit.Nodes.Clear();
            // Create the root node with the URL ID
            TreeNode rootNode = new TreeNode($"URL ID: {id}");
            treeViewUnit.Nodes.Add(rootNode);
            // Add child nodes using properties from clientModel
            Type parentType = mappedClientModel is GeoEntity ? typeof(GeoEntity) :
                mappedClientModel is TaxonomicEntity ? typeof(TaxonomicEntity) :
                throw new InvalidOperationException("Unsupported type for mappedClientModel");
            // Reflect on the determined parent type
            foreach (var property in parentType.GetProperties())
            {
                if (property.Name == nameof(DwbEntity.ApiJsonResponse))
                {
                    string json = property.GetValue(mappedClientModel) as string;
                    // Handle JSON or XML response
                    if (string.IsNullOrEmpty(json))
                    {
                        buttonShowJsonTreeView.Visible = false;
                        userControlJsonTreeView.ClearJson();
                        userControlJsonTreeView.Visible = false;
                        continue;
                    }
                    userControlJsonTreeView.LoadJson(json);
                    userControlJsonTreeView.BringToFront();
                    buttonShowJsonTreeView.Visible = true;
                    continue;
                }
                var value = property.GetValue(mappedClientModel) as string;
                if (!string.IsNullOrEmpty(value))
                {
                    var propertyNode = new TreeNode(property.Name);
                    propertyNode.Nodes.Add(new TreeNode(value));
                    rootNode.Nodes.Add(propertyNode);
                }
            }
            // Expand the tree
            treeViewUnit.ExpandAll();
        }

        //private void BuildWebServiceUnitTree(string id, DwbEntity clientModel)
        //{
        //    if (clientModel is TaxonomicEntity model)
        //    {
        //        BuildWebServiceUnitTree(id, model);
        //    }

        //    if (clientModel is GeoEntity model)
        //    {
        //        BuildWebServiceUnitTree(id, model);
        //    }
        //}

        //private void BuildWebServiceUnitTree(string id, TaxonomicEntity clientModel)
        //{
        //    TaxonomicEntity mappedClientModel = clientModel.GetMappedApiEntityModel();
        //    // Clear existing nodes
        //    treeViewUnit.Nodes.Clear();
        //    // Create the root node with the URL ID
        //    TreeNode rootNode = new TreeNode($"URL ID: {id}");
        //    treeViewUnit.Nodes.Add(rootNode);
        //    // Add child nodes using properties from clientModel
        //    foreach (var property in typeof(TaxonomicEntity).GetProperties())
        //    {
        //        if (property.Name == nameof(TaxonomicEntity.ApiJsonResponse))
        //        {
        //            string json = property.GetValue(mappedClientModel) as string;
        //            // fro xml webservices there is no json, maybe we could return the whole xml?
        //            if (string.IsNullOrEmpty(json))
        //            {
        //                buttonShowJsonTreeView.Visible = false;
        //                userControlJsonTreeView.ClearJson();
        //                userControlJsonTreeView.Visible = false;
        //                continue;
        //            }

        //            userControlJsonTreeView.LoadJson(json);
        //            userControlJsonTreeView.BringToFront();
        //            buttonShowJsonTreeView.Visible = true;
        //            continue;
        //        }

        //        var value = property.GetValue(mappedClientModel) as string;
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            var propertyNode = new TreeNode(property.Name);
        //            propertyNode.Nodes.Add(new TreeNode(value));
        //            rootNode.Nodes.Add(propertyNode);
        //        }
        //    }
        //    // Expand the tree
        //    treeViewUnit.ExpandAll();
        //}

        //private void BuildWebServiceUnitTree(string id, GeoEntity clientModel)
        //{
        //    GeoEntity mappedClientModel = clientModel.GetMappedApiEntityModel();
        //    // Clear existing nodes
        //    treeViewUnit.Nodes.Clear();
        //    // Create the root node with the URL ID
        //    TreeNode rootNode = new TreeNode($"URL ID: {id}");
        //    treeViewUnit.Nodes.Add(rootNode);
        //    // Add child nodes using properties from clientModel
        //    foreach (var property in typeof(GeoEntity).GetProperties())
        //    {
        //        if (property.Name == nameof(GeoEntity.ApiJsonResponse))
        //        {
        //            string json = property.GetValue(mappedClientModel) as string;
        //            // fro xml webservices there is no json, maybe we could return the whole xml?
        //            if (string.IsNullOrEmpty(json))
        //            {
        //                buttonShowJsonTreeView.Visible = false;
        //                userControlJsonTreeView.ClearJson();
        //                userControlJsonTreeView.Visible = false;
        //                continue;
        //            }

        //            userControlJsonTreeView.LoadJson(json);
        //            userControlJsonTreeView.BringToFront();
        //            buttonShowJsonTreeView.Visible = true;
        //            continue;
        //        }

        //        var value = property.GetValue(mappedClientModel) as string;
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            var propertyNode = new TreeNode(property.Name);
        //            propertyNode.Nodes.Add(new TreeNode(value));
        //            rootNode.Nodes.Add(propertyNode);
        //        }
        //    }
        //    // Expand the tree
        //    treeViewUnit.ExpandAll();
        //}

        private void getRelatedData(object sender, System.EventArgs e)
        {
            System.Windows.Forms.Control C = (System.Windows.Forms.Control)sender;
            System.Collections.Generic.Dictionary<string, string> Dict = (System.Collections.Generic.Dictionary<string, string>)C.Tag;
            string URI;
            if (Dict.TryGetValue("_URI", out URI))
            {
                bool DataPresent = false;
                int i = 0;
                try
                {
                    foreach (System.Data.DataRow R in this.userControlWebservice.DtQuery.Rows)
                    {
                        if (R["_URI"].ToString() == URI)
                        {
                            DataPresent = true;
                            break;
                        }
                        i++;
                    }
                    if (DataPresent)
                    {
                        this.userControlWebservice.listBoxQueryResult.SelectedIndex = -1;
                        this.userControlWebservice.listBoxQueryResult.SelectedIndex = i;
                    }
                    else
                    {
                        System.Data.DataRow RN = this.userControlWebservice.DtQuery.NewRow();
                        foreach (System.Data.DataColumn Col in this.userControlWebservice.DtQuery.Columns)
                        {
                            string Value;
                            if (Dict.TryGetValue(Col.ColumnName, out Value))
                                RN[Col.ColumnName] = Value;
                        }
                        this.userControlWebservice.DtQuery.Rows.Add(RN);
                        i = this.userControlWebservice.DtQuery.Rows.Count;
                        this.userControlWebservice.listBoxQueryResult.SelectedIndex = -1;
                        this.userControlWebservice.listBoxQueryResult.SelectedIndex = i - 1;
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void setUnitPanel(System.Collections.Generic.Dictionary<string, string> Values)
        {
            if (Values == null) return;
            foreach (System.Windows.Forms.Control C in this.tableLayoutPanelUnit.Controls)
                C.Dispose();
            this.tableLayoutPanelUnit.Controls.Clear();
            this.tableLayoutPanelUnit.ColumnStyles.Clear();
            foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                C.Dispose();
            this.panelUnit.Controls.Clear();

            this.tableLayoutPanelUnit.Height = (int)(22 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);

            int r = 0;
            int i = this.tableLayoutPanelUnit.Height;

            System.Windows.Forms.Label labelUnit = new Label();
            labelUnit.Text = this._IWorkbenchUnit.MainTable();
            labelUnit.Dock = DockStyle.Fill;
            labelUnit.TextAlign = ContentAlignment.BottomLeft;
            this.tableLayoutPanelUnit.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelUnit.Controls.Add(labelUnit);
            this.tableLayoutPanelUnit.SetColumn(labelUnit, 0);
            this.tableLayoutPanelUnit.SetRow(labelUnit, r);
            r++;

            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Values)
            {
                if (KV.Value.Length > 0 && !KV.Key.StartsWith("_"))
                {
                    try
                    {
                        System.Windows.Forms.Label L = new Label();
                        L.Text = KV.Key;
                        L.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Bold);
                        L.ForeColor = System.Drawing.Color.Gray;
                        L.Dock = DockStyle.Top;
                        L.TextAlign = ContentAlignment.BottomLeft;
                        this.panelUnit.Controls.Add(L);
                        L.BringToFront();

                        System.Windows.Forms.TextBox T = new TextBox();
                        T.Name = "textBox" + KV.Value;
                        T.Text = KV.Value;
                        T.Dock = DockStyle.Top;
                        T.Height = (int)(22 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                        T.ReadOnly = true;
                        T.TextAlign = HorizontalAlignment.Center;
                        T.BorderStyle = BorderStyle.None;
                        T.ScrollBars = ScrollBars.Vertical;
                        T.Multiline = true;
                        this.panelUnit.Controls.Add(T);
                        T.BringToFront();
                        //T.ScrollBars = ScrollBars.None;
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
        }

        private void comboBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool ValidConnectionEstablished = false;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            // Ensure correct handling of unit collection
            this._URI = null;
            this._DisplayText = null;

            try
            {
                this.panelUnit.Controls.Clear();
                this.userControlWebservice.resetQueryResults();
                resetWebserviceDetailContent();
                if (this.comboBoxDatabase.SelectedIndex > -1)
                {
                    this.splitContainerQueries.Enabled = true;
                    this.splitContainerQueries.Visible = true;
                    this.SetLastSelectedDatabase(this._IWorkbenchUnit.getServerConnection().ModuleName, this.comboBoxDatabase.Text);
                }
                this._DatabaseService = new DatabaseService();
                foreach (DiversityWorkbench.DatabaseService d in this._IWorkbenchUnit.DatabaseServices())
                {
                    if (d.DisplayText == this.comboBoxDatabase.Text)
                    {
                        this._DatabaseService = d;
                        ValidConnectionEstablished = true;
                        break;
                    }
                }

                if (this._DatabaseService.DatabaseName == null || this._DatabaseService.DatabaseName.Length == 0)
                {
                    if (this._IWorkbenchUnit.ServerConnections() != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in this._IWorkbenchUnit.ServerConnections())
                        {
                            if (KV.Value.DisplayText == this.comboBoxDatabase.Text)
                            {
                                this._DatabaseService.DatabaseName = KV.Value.DatabaseName;
                                this._DatabaseService.Server = KV.Value.DatabaseServer;

                                this.userControlQueryList.LinkedServer = KV.Value.LinkedServer;
                                this.userControlQueryList.LinkedServerDatabase = KV.Value.DatabaseName;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (this.ServerConnection.DatabaseName == this.comboBoxDatabase.Text)
                            this._IWorkbenchUnit.setServerConnection(this.ServerConnection);
                    }
                }
                else if (this._DatabaseService.LinkedServer == null)
                {
                    this.userControlQueryList.LinkedServer = "";
                    this.userControlQueryList.LinkedServerDatabase = "";
                }
                this.splitContainerMain.Panel1Collapsed = false;
                this.splitContainerMain.Panel2Collapsed = false;
                this.splitContainerQueries.Panel1Collapsed = this._DatabaseService.IsWebservice;
                this.splitContainerQueries.Panel2Collapsed = !this._DatabaseService.IsWebservice;
                this.splitContainerForeignSources.Panel2Collapsed = this._DatabaseService.IsWebservice;
                this.splitContainerUnit.Panel1Collapsed = this._DatabaseService.DisplayUnitdataInTreeview;
                this.splitContainerUnit.Panel2Collapsed = !this._DatabaseService.DisplayUnitdataInTreeview;
                this.splitContainerURI.Panel2Collapsed = true;
                this.splitContainerQueryLists.Panel1Collapsed = false;
                this.splitContainerQueryLists.Panel2Collapsed = true;
                if (this._DatabaseService.IsWebservice)
                {
                    this.initWebserviceControls(this._DatabaseService.WebService);
                }
                else if (this._DatabaseService.IsForeignSource)
                {
                    this.splitContainerMain.Panel1Collapsed = false;
                    this.splitContainerQueries.Panel1Collapsed = true;
                    this.splitContainerQueries.Panel2Collapsed = false;
                    this.splitContainerForeignSources.Panel2Collapsed = false;
                    this.splitContainerForeignSources.Panel1Collapsed = true;
                    this.splitContainerUnit.Panel1Collapsed = false;
                    this.splitContainerUnit.Panel2Collapsed = true;
                }
                else if (this.comboBoxDatabase.Text == "CacheDB")
                {
                    if (!this.setCacheDbAsSource())
                    {
                        this.comboBoxDatabase.Text = "";
                        this.comboBoxDatabase.SelectedItem = null;
                    }
                }
                else
                {
                    string Service = this.comboBoxDatabase.Text.ToString();
                    if (this._DatabaseService.IsListInDatabase)
                    {
                        if (this._DatabaseService.IsDatabaseOnLinkedServer)
                            Service = "[" + this._DatabaseService.LinkedServer + "]." + this._DatabaseService.DatabaseOnServer;
                        else
                            Service = this._DatabaseService.DatabaseOnServer;
                    }
                    if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._IWorkbenchUnit.ServiceName()].ServerConnectionList().ContainsKey(Service))
                    {
                        DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._IWorkbenchUnit.ServiceName()].ServerConnectionList()[Service];
                        this._ServerConnection = SC;
                        this._IWorkbenchUnit.setServerConnection(SC);
                    }
                    else
                    {
                        if (Service.IndexOf(" ") > -1)
                        {
                            Service = Service.Substring(0, Service.IndexOf(" "));
                            if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._IWorkbenchUnit.ServiceName()].ServerConnectionList().ContainsKey(Service))
                            {
                                DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._IWorkbenchUnit.ServiceName()].ServerConnectionList()[Service];
                                this._ServerConnection = SC;
                                this._IWorkbenchUnit.setServerConnection(SC);
                            }
                        }
                    }

                    this.splitContainerUnit.Panel1Collapsed = false;
                    this.splitContainerUnit.Panel2Collapsed = true;

                    this._BaseURI = this._ServerConnection.BaseURL;
                    this.labelOpenModule.Visible = true;
                    this.buttonOpenModule.Visible = true;
                    if (!this._DatabaseService.IsListInDatabase)
                    {
                        if (this._ServerConnection.DisplayText != this.comboBoxDatabase.Text)
                            this._ServerConnection.DatabaseName = this.comboBoxDatabase.Text;
                    }

                    this.userControlQueryList.Dock = DockStyle.Fill;
                    if (!this._DatabaseService.IsListInDatabase)
                    {
                        if (this._ServerConnection.DisplayText != this.comboBoxDatabase.Text)
                            this._ServerConnection.DatabaseName = this.comboBoxDatabase.Text;
                    }
                    if (this.userControlQueryList.Connection == null)
                        this.userControlQueryList.Connection = new Microsoft.Data.SqlClient.SqlConnection();

                    if (this.userControlQueryList.Connection.ConnectionString != this.ServerConnection.ConnectionString)
                    {
                        this.userControlQueryList.setConnection(this.ServerConnection.ConnectionString, this._IWorkbenchUnit.MainTable());
                        if (this.ServerConnection.LinkedServer.Length > 0)
                        {
                            this.userControlQueryList.LinkedServer = this.ServerConnection.LinkedServer;
                            this.userControlQueryList.LinkedServerDatabase = this.ServerConnection.DatabaseName;
                        }
                        this.userControlQueryList.setQueryConditions(this._IWorkbenchUnit.QueryConditions());
                        this.userControlQueryList.QueryDisplayColumns = this._IWorkbenchUnit.QueryDisplayColumns();
                    }
                    else if (this._ServerConnection.LinkedServer.Length > 0 &&
                        this.userControlQueryList.Connection.ConnectionString == this.ServerConnection.ConnectionString &&
                        (this.userControlQueryList.LinkedServerDatabase != this.ServerConnection.DatabaseName ||
                        this.userControlQueryList.LinkedServer != this.ServerConnection.LinkedServer))
                    {
                        this.userControlQueryList.LinkedServer = this.ServerConnection.LinkedServer;
                        this.userControlQueryList.LinkedServerDatabase = this.ServerConnection.DatabaseName;
                        this.userControlQueryList.setQueryConditions(this._IWorkbenchUnit.QueryConditions());
                    }
                    // Markus 20230309: Bugfix - Query button wurde nicht aktiviert
                    else if (this.userControlQueryList.Connection.ConnectionString.Length > 0 && this.userControlQueryList.Connection.ConnectionString == this.ServerConnection.ConnectionString && !this.userControlQueryList.buttonQuery.Enabled)
                    {
                        this.userControlQueryList.setConnection(this.ServerConnection.ConnectionString, this._IWorkbenchUnit.MainTable());
                        if (this.ServerConnection.LinkedServer.Length > 0)
                        {
                            this.userControlQueryList.LinkedServer = this.ServerConnection.LinkedServer;
                            this.userControlQueryList.LinkedServerDatabase = this.ServerConnection.DatabaseName;
                        }
                        this.userControlQueryList.setQueryConditions(this._IWorkbenchUnit.QueryConditions());
                        this.userControlQueryList.QueryDisplayColumns = this._IWorkbenchUnit.QueryDisplayColumns();
                    }

                    if (this.ServerConnection.ConnectionString.Length == 0 &&
                        this.ServerConnection.DatabaseName.Length > 0 &&
                        this.ServerConnection.DatabaseServer.Length > 0 &&
                        this.ServerConnection.DatabaseUser != DiversityWorkbench.Settings.DatabaseUser)
                    {
                        this.buttonConnectToDatabase.Visible = true;
                        this.userControlQueryList.toolStripButtonConnection.Enabled = false;
                    }
                    else
                    {
                        this.buttonConnectToDatabase.Visible = false;
                        this.userControlQueryList.toolStripButtonConnection.Enabled = true;
                    }
                    this.setTitle();
                    if (this._DatabaseService.IsListInDatabase)
                    {
                        this.userControlQueryList.setQueryRestriction(this._DatabaseService.RestrictionForListInDatabase, "#");
                    }
                }
                DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.DatabaseName = this.comboBoxDatabase.Text;
                this.DatabasesFromSetting = this.Databases;
                DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Save();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void initWebserviceControls(DwbServiceEnums.DwbService Service)
        {
            if (DwbServiceProviderAccessor.Instance == null)
            {
                MessageBox.Show("The webservice is not available");
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("The webservice is not available. ServiceProvider is null in FormRemoteQuery.initWebServiceControls()");
                return;
            }

            this.labelOpenModule.Visible = false;
            this.buttonOpenModule.Visible = false;
            this.Text = this._DatabaseService.DatabaseName;
            this.tableLayoutPanelUnit.ColumnStyles.Clear();
            foreach (System.Windows.Forms.Control C in this.tableLayoutPanelUnit.Controls)
                C.Dispose();
            this.tableLayoutPanelUnit.Controls.Clear();
            try
            {
                //// TODO ARiane 
                //var serviceMap = new Dictionary<DwbServiceEnums.DwbService, Func<object>>
                //{
                //    { DwbServiceEnums.DwbService.CatalogueOfLife, () => DwbServiceProviderAccessor.Instance.GetRequiredService<CoLWebservice>() },
                //    { DwbServiceEnums.DwbService.IndexFungorum, () => DwbServiceProviderAccessor.Instance.GetRequiredService<IndexFungorumWebservice>() },
                //    { DwbServiceEnums.DwbService.PESI, () => DwbServiceProviderAccessor.Instance.GetRequiredService<PESIWebservice>() },
                //    { DwbServiceEnums.DwbService.Mycobank, () => DwbServiceProviderAccessor.Instance.GetRequiredService<MycobankWebservice>() },
                //    { DwbServiceEnums.DwbService.Geonames, () => DwbServiceProviderAccessor.Instance.GetRequiredService<GeonamesWebservice>() },
                //    { DwbServiceEnums.DwbService.IsoCountries, () => DwbServiceProviderAccessor.Instance.GetRequiredService<IsoCountriesWebservice>() },
                //    { DwbServiceEnums.DwbService.IHOWorldSeas, () => DwbServiceProviderAccessor.Instance.GetRequiredService<IHOWorldSeasWebservice>() }
                //};
                //if (serviceMap.TryGetValue(Service, out var serviceFactory))
                //{
                switch (Service)
                {
                    case DwbServiceEnums.DwbService.CatalogueOfLife:
                        this.userControlWebservice.SetDwbWebservice(DwbServiceProviderAccessor.Instance
                            .GetRequiredService<CoLWebservice>(), DwbServiceEnums.DwbService.CatalogueOfLife);
                        this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                        // Markus 31.1.2024: Anzeige um MaxResults einzustellen
                        this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                        break;
                    case DwbServiceEnums.DwbService.PoWo_WCVP:
                        this.userControlWebservice.SetDwbWebservice(DwbServiceProviderAccessor.Instance
                            .GetRequiredService<CoLWebservice>(), DwbServiceEnums.DwbService.PoWo_WCVP);
                        this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                        this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                        break;
                    case DwbServiceEnums.DwbService.IndexFungorum:
                        this.userControlWebservice.SetDwbWebservice(DwbServiceProviderAccessor.Instance
                            .GetRequiredService<IndexFungorumWebservice>(), DwbServiceEnums.DwbService.IndexFungorum);
                        this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                        this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                        break;
                    case DwbServiceEnums.DwbService.PESI:
                        this.userControlWebservice.SetDwbWebservice(DwbServiceProviderAccessor.Instance
                            .GetRequiredService<PESIWebservice>(), DwbServiceEnums.DwbService.PESI);
                        this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                        this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                        break;
                    case DwbServiceEnums.DwbService.Mycobank:
                        this.userControlWebservice.SetDwbWebservice(DwbServiceProviderAccessor.Instance
                            .GetRequiredService<MycobankWebservice>(), DwbServiceEnums.DwbService.Mycobank);
                        this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                        this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                        break;
                    case DwbServiceEnums.DwbService.GBIFSpecies:
                        this.userControlWebservice.SetDwbWebservice(DwbServiceProviderAccessor.Instance
                            .GetRequiredService<GbifSpeciesWebservice>(), DwbServiceEnums.DwbService.GBIFSpecies);
                        this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                        this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                        break;
                    case DwbServiceEnums.DwbService.WoRMS:
                        this.userControlWebservice.SetDwbWebservice(DwbServiceProviderAccessor.Instance
                            .GetRequiredService<WoRMSWebservice>(), DwbServiceEnums.DwbService.WoRMS);
                        this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                        this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                        break;
                    case DwbServiceEnums.DwbService.GFBioTerminologyService:
                        this.userControlWebservice.SetDwbWebservice(DwbServiceProviderAccessor.Instance
                            .GetRequiredService<GfbioTerminologyWebservice>(), DwbServiceEnums.DwbService.GFBioTerminologyService);
                        this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                        this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                        break;
                    case DwbServiceEnums.DwbService.GFBioTermGeonames:
                        this.userControlWebservice.SetDwbWebservice(DwbServiceProviderAccessor.Instance
                            .GetRequiredService<GFBioTermGeonamesWebservice>(), DwbServiceEnums.DwbService.GFBioTermGeonames);
                        this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                        this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                        break;
                    case DwbServiceEnums.DwbService.IHOWorldSeas:
                        this.userControlWebservice.SetDwbWebservice(DwbServiceProviderAccessor.Instance
                            .GetRequiredService<IHOWorldSeasWebservice>(), DwbServiceEnums.DwbService.IHOWorldSeas);
                        this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                        this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                        break;
                    case DwbServiceEnums.DwbService.IsoCountries:
                        this.userControlWebservice.SetDwbWebservice(DwbServiceProviderAccessor.Instance
                            .GetRequiredService<IsoCountriesWebservice>(), DwbServiceEnums.DwbService.IsoCountries);
                        this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                        this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                        break;
                }
                //this.userControlWebservice.SetDwbWebservice((IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity>)serviceFactory());
                //this._BaseURI = this.userControlWebservice.GetDwbWebservice().GetBaseAddress();
                //this.userControlWebservice.setVisibilityOfMaxResultsForQuery(true);
                //}
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("The webservice is not available.", "Invalid webservice",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("FormRemoteQuery: Webservice initialization failed.:" + ex);
            }

            // here try/catch NullReference
            this.setTitle(this._BaseURI);
        }

        private void setTextHeights()
        {
            foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
            {
                if (C.Text.Length == 0)
                    C.Height = 1;
                else
                {
                    if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                    {
                        if (C.Text.Contains("\r\n"))
                        {
                            int l = 1;
                            string text = C.Text;
                            if (text.EndsWith("\r\n")) text = text.Substring(0, text.Length - 2);
                            while (text.Length > 0)
                            {
                                l += 1;
                                if (text.Contains("\r\n")) text = text.Substring(text.IndexOf("\r\n") + 2);
                                if (!text.Contains("\r\n")) text = "";
                            }
                            C.Height = (int)(l * 14 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                        }
                        else
                        {
                            try
                            {
                                int i = C.Text.Length;
                                float f = (float)i;
                                f = f / 160F; // Old value was 80F - output too long
                                i = (int)f * 12; // Why 12, not 14?
                                if (i < 14) i = 14; // Take at least 14 
                                C.Height = (int)(i * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                                //C.Visible = true;
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private void FormRemoteQuery_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_UnitCollection != null && this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                // Remove deleted entries from unit collection
                List<string> deleted = new List<string>();
                foreach (string item in _UnitCollection.Keys)
                {
                    if (!this.listViewUnitCollection.Items.ContainsKey(item))
                        deleted.Add(item);
                }
                foreach (string item in deleted)
                    _UnitCollection.Remove(item);

                // Save new entries in unit list in unit collection
                for (int idx = 0; idx < this.listViewUnitCollection.Items.Count; idx++)
                {
                    string key = this.listViewUnitCollection.Items[idx].Name;
                    string value = this.listViewUnitCollection.Items[idx].Text;
                    if (_UnitCollection.ContainsKey(key))
                    {
                        if (_UnitCollection[key] != value)
                            _UnitCollection[key] = value;
                    }
                    else
                    {
                        _UnitCollection.Add(key, value);
                    }
                }
            }
            if (this._AdaptFormSize)
            {
                DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Height = (int)(this.Height / DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Width = (int)(this.Width / DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Save();
            }
            if (this.userControlQueryList.QueryConditionVisiblity.Length > 0)
            {
                //TODO: Find a way to save these settings
            }
            foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                C.Dispose();
            foreach (System.Windows.Forms.Control C in this.tableLayoutPanelUnit.Controls)
                C.Dispose();
            DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing = this._UseOptimizingInMainForm;
            if (_LastSelectedSource == null)
                _LastSelectedSource = new Dictionary<string, string>();
            if (this._IWorkbenchUnit != null
                && !this._IsCurrentDatabase
                && this.comboBoxDatabase.Items.Count > 0
                && this.comboBoxDatabase.SelectedIndex > -1)
            {
                string SelectedItem = this.comboBoxDatabase.SelectedItem.ToString();
                string Modul = this.ServerConnection.ModuleName;
                if (_LastSelectedSource.ContainsKey(Modul))
                {
                    _LastSelectedSource[Modul] = SelectedItem;
                }
                else
                {
                    _LastSelectedSource.Add(Modul, SelectedItem);
                }
            }
        }

        private static System.Collections.Generic.Dictionary<string, string> _LastSelectedSource;

        private void checkBoxDisplayWebsite_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //this.setSourceURI(this.webBrowserURI.Url.ToString());
                if (this.checkBoxDisplayWebsite.Checked && this.userControlWebViewURI.Url != null)
                    this.setSourceURI(this.userControlWebViewURI.Url.ToString());
                else
                    this.setSourceURI("about:blank");
            }
            catch (System.Exception ex)
            {
            }
            //    this.setSourceURI("about:blank");
        }

        private void userControlWebViewURI_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                //if (this._ServiceType == WorkbenchUnit.ServiceType.CatalogueOfLife_2)
                //    return;
                this.userControlWebViewURI.Refresh();
                if (this.userControlWebViewURI.Url != null)
                {
                    string URL = this.userControlWebViewURI.Url.ToString();
                    int W = this.userControlWebViewURI.Width;
                    int H = this.userControlWebViewURI.Height;
                    if (URL.IndexOf("&Height=") > -1 &&
                        URL.IndexOf("&Width=") > -1)
                    {
                        string EndOfUrl = URL.Substring(URL.IndexOf("&Width=") + 7);
                        int TestWidth;
                        if (!int.TryParse(EndOfUrl, out TestWidth))
                            return;
                        int Hstart = URL.IndexOf("&Height=") + 8;
                        string UrlNew = URL.Substring(0, Hstart) + H.ToString() + "&Width=" + W.ToString();
                        this.setSourceURI(UrlNew);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        private void buttonTimeoutWebrequest_Click(object sender, EventArgs e)
        {
            int TimeOut = DiversityWorkbench.Settings.TimeoutWeb;
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(TimeOut, "Timeout", "Please enter the milliseconds to wait for the result web site");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null)
            {
                DiversityWorkbench.Settings.TimeoutWeb = (int)f.Integer;
            }
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

        public Dictionary<string, string> ValueDictionary()
        {
            if (this._ValueDictionary == null)
                this._ValueDictionary = new Dictionary<string, string>();

            System.Data.DataRowView R = (System.Data.DataRowView)this.userControlWebservice.listBoxQueryResult.SelectedItem;
            foreach (System.Data.DataColumn C in R.Row.Table.Columns)
            {
                if (!this._ValueDictionary.ContainsKey(C.ColumnName) && !R[C.ColumnName].Equals(System.DBNull.Value) && R[C.ColumnName].ToString().Length > 0)
                    this._ValueDictionary.Add(C.ColumnName, R[C.ColumnName].ToString());
            }

            return this._ValueDictionary;
        }

        #endregion

        #region Toolstrip

        private void setQueryControlEvents()
        {
            try
            {
                this.userControlWebservice.listBoxQueryResult.Click += new System.EventHandler(this.listBoxQueryResultWebservice_Click);
                this.userControlWebservice.listBoxQueryResult.SelectedIndexChanged += new System.EventHandler(this.listBoxQueryResultWebservice_SelectedIndexChanged);

                this.userControlQueryList.listBoxQueryResult.SelectedIndexChanged += new System.EventHandler(this.listBoxQueryResult_SelectedIndexChanged);
                this.userControlQueryList.toolStripButtonConnection.Click += new System.EventHandler(this.ConnectToDatabase);
                this.userControlQueryList.toolStripButtonSwitchOrientation.Click += new System.EventHandler(this.switchQueryOrientation);

                this.userControlQueryListCacheDB.listBoxQueryResult.SelectedIndexChanged += new System.EventHandler(this.listBoxQueryResultCacheDB_SelectedIndexChanged);
                //this.userControlQueryListCacheDB.toolStripButtonConnection.Click += new System.EventHandler(this.ConnectToDatabase);
                //this.userControlQueryListCacheDB.toolStripButtonSwitchOrientation.Click += new System.EventHandler(this.switchQueryOrientation);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void ConnectToDatabase(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormConnectToDatabase f = new Forms.FormConnectToDatabase();
                //DiversityWorkbench.FormDatabaseConnection f = new DiversityWorkbench.FormDatabaseConnection();
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                    this.userControlQueryList.setConnection(this._ServerConnection.ConnectionString, this._IWorkbenchUnit.MainTable());
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void switchQueryOrientation(object sender, System.EventArgs e)
        {
            try
            {
                if (this.userControlQueryList.splitContainerMain.Orientation == Orientation.Vertical)
                    this.splitContainerMain.SplitterDistance = (int)this.splitContainerMain.SplitterDistance / 2;
                else
                    this.splitContainerMain.SplitterDistance = (int)this.splitContainerMain.SplitterDistance * 2;
                this.userControlQueryList.switchOrientation();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonConnectToDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormConnectToDatabase f = new Forms.FormConnectToDatabase(this.ServerConnection, true);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    this.ServerConnection.DatabasePassword = f.LocalServerConnection.DatabasePassword;
                    DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[f.ServerConnection.ModuleName].ServerConnectionList()[f.ServerConnection.DisplayText].DatabasePassword = f.ServerConnection.DatabasePassword;
                    this.comboBoxDatabase_SelectedIndexChanged(null, null);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Treeview

        // TODO ARiane do we need this?
        private void treeViewUnit_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                // uncommentend Ariane
                //string URI = "";
                ////string UriBase = "";
                //if (this._DatabaseService.WebService == DiversityWorkbench.Webservice.WebServices.None
                //    && this.treeViewUnit.SelectedNode.Tag != null
                //    && this._URI.Length > 0)
                //{
                //    DiversityWorkbench.WorkbenchUnit.ServiceType ServiceType = DiversityWorkbench.WorkbenchUnit.getServiceType(this._URI);
                //    switch (ServiceType)
                //    {
                //        case WorkbenchUnit.ServiceType.CatalogueOfLife:
                //            this._BaseURI = DiversityWorkbench.WebserviceCatalogueOfLife.UriBaseWeb;
                //            this._DatabaseService.WebService = Webservice.WebServices.CatalogueOfLife;
                //            break;
                //    //    //case WorkbenchUnit.ServiceType.CatalogueOfLife_2:
                //    //    //    this._BaseURI = DiversityWorkbench.CatalogueOfLife_2.UriBaseWeb;
                //    //    //    this._DatabaseService.WebService = Webservice.WebServices.CatalogueOfLife_2;
                //    //    //    break;
                //    //    case WorkbenchUnit.ServiceType.PalaeoDB:
                //    //        this._BaseURI = DiversityWorkbench.PalaeoDB.UriBaseWeb;
                //    //        this._DatabaseService.WebService = Webservice.WebServices.PalaeoDB;
                //    //        break;
                //    //    case WorkbenchUnit.ServiceType.Mycobank:
                //    //        this._BaseURI = DiversityWorkbench.WebserviceMycobank.UriBaseWeb;
                //    //        this._DatabaseService.WebService = Webservice.WebServices.Mycobank;
                //    //        break;
                //    }

                //}
                //if (this.treeViewUnit.SelectedNode.Tag != null)
                //{
                //    {
                //        int ID = 0;
                //        switch (this._DatabaseService.WebService)
                //        {
                //            //case Webservice.WebServices.PalaeoDB:
                //            //    URI = this.treeViewUnit.SelectedNode.Tag.ToString();
                //            //    if (int.TryParse(URI, out ID))
                //            //    {
                //            //        DiversityWorkbench.PalaeoDB P = new PalaeoDB();
                //            //        if (this._ReadOnly && this._BaseURI.Length > 0)
                //            //        {
                //            //            string UriFull = this._BaseURI + ID.ToString() + P.UriXmlSuffix();
                //            //            P.BuildUnitTree(UriFull, this.treeViewUnit);
                //            //        }
                //            //        else
                //            //            P.AddAndBuildUnitTree(ID, this.treeViewUnit, this.userControlWebservice.listBoxQueryResult, this.userControlWebservice.DtQuery);
                //            //        //DiversityWorkbench.PalaeoDB.AddAndBuildUnitTree(ID, this.treeViewUnit, this.userControlWebservice.listBoxQueryResult, this.userControlWebservice.DtQuery);
                //            //    }
                //            //    else
                //            //    {
                //            //        DiversityWorkbench.PalaeoDB P = new PalaeoDB();
                //            //        P.BuildUnitTree(URI, this.treeViewUnit);
                //            //        //DiversityWorkbench.PalaeoDB.BuildUnitTree(URI, this.treeViewUnit);
                //            //    }
                //            //    break;
                //            case Webservice.WebServices.CatalogueOfLife:
                //                URI = this.treeViewUnit.SelectedNode.Tag.ToString();
                //                if (int.TryParse(URI, out ID))
                //                {
                //                    DiversityWorkbench.CatalogueOfLifeWebservice C = new CatalogueOfLifeWebservice();
                //                    if (this._ReadOnly && this._BaseURI.Length > 0)
                //                    {
                //                        string UriFull = this._BaseURI + ID.ToString() + C.UriXmlSuffix();
                //                        C.BuildUnitTree(UriFull, this.treeViewUnit);
                //                    }
                //                    else
                //                        C.AddAndBuildUnitTree(ID, this.treeViewUnit, this.userControlWebservice.listBoxQueryResult, this.userControlWebservice.DtQuery);
                //                    //DiversityWorkbench.CatalogueOfLifeWebservice.AddAndBuildUnitTree(ID, this.treeViewUnit, this.userControlWebservice.listBoxQueryResult, this.userControlWebservice.DtQuery);
                //                }
                //                else
                //                {
                //                    DiversityWorkbench.CatalogueOfLifeWebservice C = new CatalogueOfLifeWebservice();
                //                    C.BuildUnitTree(URI, this.treeViewUnit);
                //                    //DiversityWorkbench.CatalogueOfLifeWebservice.BuildUnitTree(URI, this.treeViewUnit);
                //                }
                //                break;
                //            ////case Webservice.WebServices.CatalogueOfLife:
                //            ////    URI = this.treeViewUnit.SelectedNode.Tag.ToString();
                //            ////    if (int.TryParse(URI, out ID))
                //            ////    {
                //            ////        DiversityWorkbench.CatalogueOfLife_2 H = new CatalogueOfLife_2();
                //            ////        if (this._ReadOnly && this._BaseURI.Length > 0)
                //            ////        {
                //            ////            string UriFull = this._BaseURI + ID.ToString() + H.UriXmlSuffix();
                //            ////            H.BuildUnitTree(UriFull, this.treeViewUnit);
                //            ////        }
                //            ////        else
                //            ////            H.AddAndBuildUnitTree(ID, this.treeViewUnit, this.userControlWebservice.listBoxQueryResult, this.userControlWebservice.DtQuery);
                //            ////    }
                //            ////    else
                //            ////    {
                //            ////        DiversityWorkbench.CatalogueOfLife_2 C = new CatalogueOfLife_2();
                //            ////        C.BuildUnitTree(URI, this.treeViewUnit);
                //            ////    }
                //            ////    break;
                //            //case Webservice.WebServices.Mycobank:
                //            //    URI = this.treeViewUnit.SelectedNode.Tag.ToString();
                //            //    if (int.TryParse(URI, out ID))
                //            //    {
                //            //        DiversityWorkbench.WebserviceMycobank M = new WebserviceMycobank();
                //            //        if (this._ReadOnly && this._BaseURI.Length > 0)
                //            //        {
                //            //            string UriFull = this._BaseURI + ID.ToString() + M.UriXmlSuffix();
                //            //            M.BuildUnitTree(UriFull, this.treeViewUnit);
                //            //        }
                //            //        else
                //            //            M.AddAndBuildUnitTree(ID, this.treeViewUnit, this.userControlWebservice.listBoxQueryResult, this.userControlWebservice.DtQuery);
                //            //    }
                //            //    else
                //            //    {
                //            //        DiversityWorkbench.WebserviceMycobank C = new WebserviceMycobank();
                //            //        C.BuildUnitTree(URI, this.treeViewUnit);
                //            //    }
                //            //    break;
                //            default:
                //                this._IWorkbenchUnit.BuildUnitTree(ID, this.treeViewUnit);
                //                break;
                //        }
                //    }
                //    this._DisplayText = this.treeViewUnit.Nodes[0].Text;
                //}
            }
            catch { }
            finally { this.Cursor = System.Windows.Forms.Cursors.Default; }
        }


        #endregion

        #region Properties

        public string QueryRestriction
        {
            get { return this.userControlQueryList.QueryRestriction; }
            set
            {
                //this.userControlQueryList.QueryRestriction = value; 
                this.userControlQueryList.setQueryRestriction(value, "#");
            }
        }

        public string URI
        {
            get
            {
                if (this._URI == null) this._URI = "";
                if (this._BaseURI == null || this._BaseURI.Length == 0)
                    this._BaseURI = DiversityWorkbench.WorkbenchUnit.getBaseURIfromURI(this._URI);
                if (this._BaseURI.Length == 0)
                    this._BaseURI = this._ServerConnection.BaseURL;
                if (this._URI.StartsWith(this._BaseURI) || this._URI.StartsWith("http://") || this._URI.StartsWith("https://")) // Markus 31.1.2024: Including https
                {
                    return this._URI;
                }
                else if (this._Domain != null && this._Domain.Length > 0)
                {
                    return this._BaseURI + this._Domain + "/" + this.userControlQueryList.ID;
                }
                else if (this._BaseURI.Length > 0 && this._URI.Length == 0)
                {
                    // Toni 20180821: Read query list only if database was called
                    if (!this._DatabaseService.IsWebservice && this.userControlQueryList.ID != 0)
                        return this._BaseURI + this.userControlQueryList.ID.ToString();
                }
                else if (this._URI.Length > 0) // Toni 20180821: Empty URI if nothing was selected
                {
                    // Markus 20200129 - Suffix if needed
                    if (this._BaseUriSuffix.Length > 0)
                        return this._BaseURI + this._URI + this._BaseUriSuffix;
                    else
                    {
                        // Markus 31.1.2024 - Bugfix fuer CoL
                        if (this._URI.StartsWith("https://api.checklistbank.org/dataset/"))
                            return _URI;
                        return this._BaseURI + this._URI;
                    }
                }
                // Toni 20180821: Provide empty URI if nothing was selected
                return this._URI;
            }
        }

        public int ID
        {
            get
            {
                if (this._URI == null)
                {
                    this.DialogResult = DialogResult.Cancel;
                    return -1;
                }
                else
                {
                    // #218
                    int i;
                    if (int.TryParse(this._URI, out i))
                        return i;
                    return -1;
                }
            }
        }

        public string DisplayText
        {
            get
            {
                // Toni 20180821: Supply empty string, not null (-> Return might cause exception at caller site)
                if (this._DisplayText == null)
                    this._DisplayText = "";
                if (this._DatabaseService.IsWebservice && this.treeViewUnit.Nodes.Count > 0)
                {
                    string Display = this.treeViewUnit.Nodes[0].Text.Trim();
                    if (Display.Length > 0 && (this._DisplayText == null || Display.Length > this._DisplayText.Trim().Length && Display.StartsWith(this._DisplayText)))
                        this._DisplayText = Display;
                }
                else if (this._DisplayText.Length == 0 && !this._DatabaseService.IsWebservice) // Toni 20180821: Read query list only if database was called
                {
                    try
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.userControlQueryList.listBoxQueryResult.SelectedItem;
                        // #218
                        if (R != null)
                            this._DisplayText = R["Display"].ToString();
                    }
                    catch (System.Exception ex) { }
                }
                return this._DisplayText.Trim();
            }
        }

        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get { return _ServerConnection; }
            set
            {
                _ServerConnection = value;
                if (_ServerConnection != null)
                    this.setTitle();
            }
        }

        public System.Windows.Forms.HelpProvider HelpProvider
        {
            get { return this.helpProvider; }
            set { this.helpProvider = value; }
        }

        public System.Windows.Forms.TreeView TreeViewUnit
        {
            get { return this.treeViewUnit; }
        }

        public System.Collections.Generic.Dictionary<string, string> SelectedItems()
        {
            System.Collections.Generic.Dictionary<string, string> Items = new Dictionary<string, string>();
            try
            {
                string BaseURL = this.ServerConnection.BaseURL;//this._IWorkbenchUnit.getServerConnection().BaseURL;
                System.Data.DataTable dt = (System.Data.DataTable)this.userControlQueryList.listBoxQueryResult.DataSource;
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    if (R.RowState == DataRowState.Deleted || R.RowState == DataRowState.Detached)
                        continue;
                    string Text = R[1].ToString();
                    if (dt.Columns.Count > 2 && !R[2].Equals(System.DBNull.Value) && R[2].ToString().Length > 0)
                        Text = R[2].ToString();
                    if (!Items.ContainsKey(BaseURL + R[0].ToString()))
                        Items.Add(BaseURL + R[0].ToString(), Text);
                    else
                    {
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Items;
        }

        public System.Collections.Generic.Dictionary<string, string> SelectedItem()
        {
            System.Collections.Generic.Dictionary<string, string> Items = new Dictionary<string, string>();
            try
            {

                string BaseURL = this.ServerConnection.BaseURL;// this._IWorkbenchUnit.getServerConnection().BaseURL;
                if (this.userControlQueryList.listBoxQueryResult.SelectedItem != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.userControlQueryList.listBoxQueryResult.SelectedItem;
                    Items.Add(BaseURL + R[0].ToString(), R[1].ToString());
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Items;
        }

        #endregion

        #region Query

        private void listBoxQueryResult_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.setUnit(this.userControlQueryList.ID);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxQueryResultCacheDB_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.setUnitCacheDB(this.userControlQueryListCacheDB.ID);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void listBoxQueryResultWebservice_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.userControlWebservice.listBoxQueryResult.SelectedIndex == -1)
            {
                this.panelUnit.Controls.Clear();
            }
        }

        private async void listBoxQueryResultWebservice_Click(object sender, System.EventArgs e)
        {
            if (this.userControlWebservice.listBoxQueryResult.SelectedIndex > -1)
            {
                this.SuspendLayout();
                // DiversityWorkbench.Webservice.UriCurrentHtmlRecord = "";
                // this.userControlWebservice.QueryItem();
                string SelectedValue = this.userControlWebservice.listBoxQueryResult.SelectedValue.ToString();
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api =
                        _api = userControlWebservice.GetDwbWebservice();
                    if (_api != null)
                    {
                        if (!_api.IsValidUrl(SelectedValue))
                        {
                            MessageBox.Show("No detailed information is available for the selected item.",
                                "Information",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            return;
                        }

                        var tt = await _api.CallWebServiceAsync<object>(
                            SelectedValue,
                            DwbServiceEnums.HttpAction.GET);
                        DwbEntity clientEntity = _api.GetDwbApiDetailModel(tt);

                        int UnitID;
                        if (int.TryParse(SelectedValue, out UnitID))
                            this.setUnit(UnitID);
                        else
                        {
                            this.setUnit(this.userControlWebservice.listBoxQueryResult.SelectedValue.ToString(),
                                clientEntity);
                        }
                    }
                }
                catch (ArgumentException ae)
                {
                    MessageBox.Show(
                        "The record details cannot be displayed because the web service response is invalid.\r\n\r\n  " +
                        "For more details on the error, see the error log file.\r\n\r\n",
                        "Data Mapping Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    ExceptionHandling.WriteToErrorLogFile(
                        "FormRemoteQuery - listBoxQueryResultWebservice_Click, InvalidOprationException exception: " +
                        ae);
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show(
                        "The record details cannot be displayed because the web service response is invalid.\r\n\r\n  " +
                        "For more details on the error, see the error log file.\r\n\r\n",
                        "Data Mapping Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    ExceptionHandling.WriteToErrorLogFile(
                        "FormRemoteQuery - listBoxQueryResultWebservice_Click, InvalidOprationException exception: " +
                        ioe);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "The record details cannot be displayed because the web service response is invalid.\r\n\r\n  " +
                        "For more details on the error, see the error log file.\r\n\r\n",
                        "Data Mapping Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    ExceptionHandling.WriteToErrorLogFile(
                        "FormRemoteQuery - listBoxQueryResultWebservice_Click, Exception exception: " + ex);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }

                this.ResumeLayout();
            }
        }

        private void AdjustPanelUnitSize(UserControlJsonTreeView jsonTree)
        {
            // Set panelUnit size to match the preferred size of the jsonTree
            panelUnit.Size = jsonTree.PreferredSize;
        }

        private void setUnit(int ID)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (this._DatabaseService == null)
                {
                    this._DatabaseService = new DatabaseService();
                }
                if (this.panelUnit.Controls.Count == 0 &&
                    this._DatabaseService != null &&
                    !this._DatabaseService.IsWebservice)
                    this.setUnitPanel(this._IWorkbenchUnit);
                foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                {
                    if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                        C.Text = "";
                }
                System.Collections.Generic.Dictionary<string, string> Values;
                _Resources = new Dictionary<string, string>();
                if (this._DatabaseService.IsWebservice)
                {
                    Values = new Dictionary<string, string>();
                    this.getWebserviceValues(ref Values);
                    this.setUnitPanel((System.Data.DataRowView)this.userControlWebservice.listBoxQueryResult.SelectedItem);
                    this._IWorkbenchUnit.SetUnitValues(Values);
                }
                else
                {
                    if (this._Domain != null && this._Domain.Length > 0)
                        Values = this._IWorkbenchUnit.UnitValues(this._Domain, ID);
                    else
                    {
                        Values = this._IWorkbenchUnit.UnitValues(ID); /// [b]
                        if (Values != null)
                        {
                            if (_ShowAdditional)
                                (this._IWorkbenchUnit as WorkbenchUnit).GetAdditionalUnitValues(Values);
                        }
                        else
                        {
                            this.setUnitPanel(this._IWorkbenchUnit);
                        }
                    }
                    try
                    {
                        _Resources = this._IWorkbenchUnit.UnitResources(ID);
                        _ResourcesSorted = new SortedDictionary<int, string>();
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _Resources)
                        {
                            _ResourcesSorted.Add(_ResourcesSorted.Count, KV.Key);
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                    _SavedUnitID = ID;
                }

                foreach (var p in Values)
                {
                    foreach (System.Windows.Forms.Control c in this.panelUnit.Controls)
                    {
                        if (c.GetType() != typeof(System.Windows.Forms.TextBox) ||
                            c.Name != "textBox" + p.Key) continue;
                        if (c.Text.Length > 0) c.Text += ", ";
                        c.Text += p.Value;
                    }
                    if (p.Key == "_URI")
                        this._URI = p.Value;
                    if (p.Key == "_DisplayText" && p.Value.Length > 0)
                        this._DisplayText = p.Value;
                }
                //string Url = this._IWorkbenchUnit.SourceURI(this.webBrowserURI);
                this.setSourceURI(this._IWorkbenchUnit.SourceURI(this.userControlWebViewURI));
                if (this._DatabaseService.IsWebservice)
                {
                    this.splitContainerUriResources.Panel2Collapsed = true;
                    this.splitContainerURI.Panel2Collapsed = true;
                }
                else if (_Resources.Count > 0)
                {
                    this.setResourcesControls();
                }
                else
                {
                    //this.splitContainerUriResources.Panel1Collapsed = false;
                    this.splitContainerUriResources.Panel2Collapsed = true;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            this.setTextHeights();
        }

        private void setUnit(string ID)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (this.panelUnit.Controls.Count == 0 && !this._DatabaseService.IsWebservice)
                    this.setUnitPanel(this._IWorkbenchUnit);
                foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                {
                    if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                        C.Text = "";
                }
                System.Collections.Generic.Dictionary<string, string> Values;
                if (this._DatabaseService.IsWebservice)
                {
                    Values = new Dictionary<string, string>();
                    // this.getWebserviceValues(ref Values);
                    this.setUnitPanel((System.Data.DataRowView)this.userControlWebservice.listBoxQueryResult.SelectedItem);
                    this._IWorkbenchUnit.SetUnitValues(Values);
                    this.splitContainerUriResources.Panel2Collapsed = true;
                }
                else
                {
                    Values = this._IWorkbenchUnit.UnitValues();
                }

                foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
                {
                    foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                    {
                        if (C.GetType() == typeof(System.Windows.Forms.TextBox) &&
                            C.Name == "textBox" + P.Key)
                        {
                            if (C.Text.Length > 0) C.Text += ", ";
                            C.Text += P.Value;
                        }
                    }
                    if (P.Key == "_URI")
                        this._URI = P.Value;
                    if (P.Key == "_DisplayText")
                        this._DisplayText = P.Value;
                }

                this.setSourceURI(this._IWorkbenchUnit.SourceURI(this.userControlWebViewURI));
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            this.setTextHeights();
        }

        private void setUnit(string ID, DwbEntity clientEntity)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                {
                    if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                        C.Text = "";
                }
                System.Collections.Generic.Dictionary<string, string> Values;
                if (this._DatabaseService.IsWebservice)
                {
                    Values = new Dictionary<string, string>();
                    this.getWebserviceValues(ref Values);
                    this.setUnitPanel(ID, clientEntity);
                    this._IWorkbenchUnit.SetUnitValues(Values);
                    this.splitContainerUriResources.Panel2Collapsed = true;
                }
                else
                {
                    Values = this._IWorkbenchUnit.UnitValues();
                }

                foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
                {
                    foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                    {
                        if (C.GetType() == typeof(System.Windows.Forms.TextBox) &&
                            C.Name == "textBox" + P.Key)
                        {
                            if (C.Text.Length > 0) C.Text += ", ";
                            C.Text += P.Value;
                        }
                    }
                    if (P.Key == "_URI")
                        this._URI = P.Value;
                    if (P.Key == "_DisplayText")
                        this._DisplayText = P.Value;
                }

                this.setSourceURI(clientEntity._URL);
                //if (this._DatabaseService.IsWebservice)
                //{
                //    if (DiversityWorkbench.Webservice.UriCurrentHtmlRecord.Length > 0)
                //    {
                //        try
                //        {
                //            System.Uri U = new Uri(DiversityWorkbench.Webservice.UriCurrentHtmlRecord);
                //            this.splitContainerURI.Panel2Collapsed = false;
                //            this.setSourceURI(U.ToString());
                //        }
                //        catch
                //        {
                //            this.splitContainerURI.Panel2Collapsed = true;
                //        }
                //    }
                //    else this.splitContainerURI.Panel2Collapsed = true;
                //}
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            //this.setTextHeights();
        }

        private void getWebserviceValues(ref System.Collections.Generic.Dictionary<string, string> Values)
        {
            if (this.userControlWebservice.listBoxQueryResult.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.userControlWebservice.listBoxQueryResult.SelectedItem;
                foreach (System.Data.DataColumn C in R.DataView.Table.Columns)
                {
                    if (!R[C.ColumnName].Equals(System.DBNull.Value))
                    {
                        if (!Values.ContainsKey(C.ColumnName))
                            Values.Add(C.ColumnName, R[C.ColumnName].ToString());
                        else
                        { }
                    }
                }
            }
        }

        private void setSourceURI(string URI)
        {
            try
            {
                // try to find out if the URI returns text and if so avoid display
                if (URI.Length > 0)
                {
                    this.panelWebControls.Visible = true;
                    // Markus 22.8.2016 - Google seems to slow down request - may be for header information only
                    if (URI == "about:blank")
                    {
                        this.splitContainerURI.Panel2Collapsed = true;
                        return;
                    }
                    // Markus 22.8.2016 - try to get the header information only for unknown websites
                    if (URI.IndexOf("/GoogleMaps/") == -1)
                    {
                        System.Net.WebRequest request = System.Net.WebRequest.Create(URI);
                        // Toni 6.9.2017 - 100 ms for timeout is too short for some requests
                        request.Timeout = 10000;
                        request.Method = "HEAD";
                        // Toni 6.9.2017 - Handle time out (-> WebException)
                        try
                        {
                            System.Net.WebResponse response = request.GetResponse();
                            if (!response.ContentType.StartsWith("text/"))
                            {
                                this.splitContainerURI.Panel2Collapsed = true;
                                this.panelWebControls.Visible = false;
                                return;
                            }
                        }
                        catch (System.Net.WebException)
                        {
                            this.splitContainerURI.Panel2Collapsed = true;
                            return;
                        }
                    }
                }
                else
                {
                    this.panelWebControls.Visible = false; //.checkBoxDisplayWebsite.Visible = false;
                }

                if (URI.Length > 0 && this.checkBoxDisplayWebsite.Checked)
                {
                    try
                    {
                        System.Uri U = new Uri(URI);
                        this.userControlWebViewURI.Url = U;
                        this.splitContainerURI.Panel2Collapsed = false;
                    }
                    catch
                    {
                        this.splitContainerURI.Panel2Collapsed = true;
                    }
                }
                else
                {
                    this.splitContainerURI.Panel2Collapsed = true;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Module

        private void initModuleConnection()
        {
            //if (this._RemoteModule != null)
            //{
            //    //this.userControlQueryList.setQueryConditions(this._RemoteModule.QueryConditions());
            //    //this.userControlQueryList.QueryDisplayColumns = this._RemoteModule.QueryDisplayColumns();
            //}
        }

        private void buttonOpenModule_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo StartInfo = new System.Diagnostics.ProcessStartInfo();
                DiversityWorkbench.ServerConnection S = this._IWorkbenchUnit.getServerConnection();
                string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.ApplicationDirectory() + "\\" + S.ModuleName + ".exe";
                StartInfo.FileName = Path;
                StartInfo.CreateNoWindow = false;
                string Arguments = "";
                Arguments = this.userControlQueryList.ID.ToString() + " " + S.IsTrustedConnection.ToString() + " " + S.DatabaseServer + " " + S.DatabaseName;
                if (!S.IsTrustedConnection)
                    Arguments += " " + S.DatabaseUser + " " + S.DatabasePassword;
                StartInfo.Arguments = Arguments;
                if (StartInfo.FileName.Length == 0 || StartInfo.Arguments.Length == 0)
                    return;
                System.Diagnostics.Process P = new System.Diagnostics.Process();
                P.StartInfo = StartInfo;
                P.Start();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Database for picklist

        private System.Collections.Generic.Dictionary<string, string> _Databases;
        private System.Collections.Generic.Dictionary<string, string> Databases
        {
            get
            {
                if (this._Databases == null)
                {
                    this._Databases = new Dictionary<string, string>();

                    // testweise eingefügt - war aber nicht ursache von Fehler - fuer DST wurden die Listen nicht angezeigt
                    //try
                    //{
                    //    if (this._IWorkbenchUnit != null && this._IWorkbenchUnit.ServerConnections().Count > 0)
                    //    {
                    //        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> SC in this._IWorkbenchUnit.ServerConnections())
                    //        {
                    //            if (!this._Databases.ContainsKey(SC.Key))
                    //            {
                    //                string Database = SC.Value.DatabaseName;
                    //                this._Databases.Add(SC.Key, Database);
                    //            }
                    //        }
                    //    }
                    //}
                    //catch(System.Exception ex)
                    //{
                    //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    //}

                    try
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                        {
                            if (!this._Databases.ContainsKey(KV.Key))
                            {
                                string Database = KV.Value.getServerConnection().DatabaseName;
                                this._Databases.Add(KV.Key, Database);
                            }
                        }
                        if (this.DatabasesFromSetting != null && this.DatabasesFromSetting.Count > 0)
                        {
                            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
                            foreach (System.Collections.Generic.KeyValuePair<string, string> DDkv in this.DatabasesFromSetting)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Databases)
                                {
                                    if (DDkv.Key == KV.Key && KV.Value != DDkv.Value)
                                        DD.Add(KV.Key, DDkv.Value);
                                }
                            }
                            if (DD.Count > 0)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DD)
                                {
                                    if (this._Databases.ContainsKey(KV.Key))
                                        this._Databases[KV.Key] = KV.Value;
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                return this._Databases;
            }
        }

        /// <summary>
        /// The selected databases or servcies of the modules as defined in the settings
        /// </summary>
        private System.Collections.Generic.Dictionary<string, string> DatabasesFromSetting
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
                if (DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Modules != null && DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Databases != null)
                {
                    for (int i = 0; i < DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Databases.Count; i++)
                    {
                        DD.Add(DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Modules[i], DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Databases[i]);
                    }
                }
                return DD;
            }
            set
            {
                System.Collections.Generic.Dictionary<string, string> DD = value;
                try
                {
                    if (DD == null || DD.Count == 0)
                        return;
                    System.Collections.Specialized.StringCollection scD = new System.Collections.Specialized.StringCollection();
                    System.Collections.Specialized.StringCollection scM = new System.Collections.Specialized.StringCollection();
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DD)
                    {
                        scD.Add(KV.Value);
                        scM.Add(KV.Key);
                    }
                    DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Modules = scM;
                    DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Databases = scD;
                    DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Save();
                }
                catch (System.Exception ex) { }
            }
        }

        private void SetLastSelectedDatabase(string Module, string Database)
        {
            if (!this.Databases.ContainsKey(Module))
                this.Databases.Add(Module, Database);
            else this.Databases[Module] = Database;
        }

        private string LastSelectedDatabase(string Module)
        {
            if (this.Databases.ContainsKey(Module))
                return this.Databases[Module];
            else return "";
        }

        #endregion

        #region Resources

        private System.Collections.Generic.Dictionary<string, string> _Resources = new Dictionary<string, string>();
        private System.Collections.Generic.SortedDictionary<int, string> _ResourcesSorted;
        private int _ResourcesPosition = 0;

        // unklarer Bug. Der Aufruf dieser Funktion führt zum Schliessen des Formulars
        //private void setUnitResources()
        //{
        //    try
        //    {
        //        _Resources = this._IWorkbenchUnit.UnitResources(ID);
        //        _ResourcesSorted = new SortedDictionary<int, string>();
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _Resources)
        //        {
        //            _ResourcesSorted.Add(_ResourcesSorted.Count, KV.Key);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //}

        private void setResourcesControls()
        {
            this.splitContainerURI.Panel2Collapsed = false;
            this.splitContainerUriResources.Panel1Collapsed = true;
            this.splitContainerUriResources.Panel2Collapsed = false;
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _Resources)
            {
                try
                {
                    this.linkLabelResource.Text = KV.Key;
                    System.Uri U = new Uri(KV.Key);
                    this.userControlWebViewResource.Url = U;
                    this.userControlWebViewResource.Navigate(KV.Key);
                    if (this.userControlWebViewResource.Url.ToString() != "about:blank" && this.userControlWebViewResource.Url.ToString() != U.ToString())
                    {
                        this.userControlWebViewResource.Navigate("about:blank");
                        this.userControlWebViewResource.Navigate(KV.Key);
                    }
                    this.toolStripLabelResourceDescription.Text = KV.Value;
                    break;
                }
                catch (System.Exception ex)
                {
                }
            }
            if (this._Resources.Count > 1)
                this.toolStripButtonResourceNext.Visible = true;
            else
                this.toolStripButtonResourceNext.Visible = false;
        }

        private void toolStripButtonResourceInSeparateForm_Click(object sender, EventArgs e)
        {
            try
            {
                System.Uri U = this.userControlWebViewResource.Url;
                DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(U.ToString());
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonResourceNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._Resources.Count > 0)
                {
                    if (this._Resources.Count > this._ResourcesPosition + 1)
                    {
                        this._ResourcesPosition++;
                    }
                    else
                        this._ResourcesPosition = 0;
                    System.Uri U = new Uri(this._ResourcesSorted[this._ResourcesPosition]);
                    this.userControlWebViewResource.Url = U;
                    this.toolStripLabelResourceDescription.Text = this._Resources[this._ResourcesSorted[this._ResourcesPosition]];
                    this.linkLabelResource.Text = U.ToString();
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void linkLabelResource_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(this.linkLabelResource.Text);
        }

        #endregion

        #region Unit Collection

        private void toolStripButtonAddUnit_Click(object sender, EventArgs e)
        {
            if (_UnitCollection != null && _DisplayText != null && _URI != null)
            {
                if (this.listViewUnitCollection.Items.ContainsKey(URI))
                {
                    if (this.listViewUnitCollection.Items[URI].Text != DisplayText)
                        this.listViewUnitCollection.Items[URI].Text = DisplayText;
                }
                else
                {
                    ListViewItem listItem = new ListViewItem(DisplayText);
                    listItem.Name = URI;
                    listItem.ToolTipText = URI;
                    this.listViewUnitCollection.Items.Add(listItem);
                }
                this.toolStripButtonRemoveUnit.Visible = this.listViewUnitCollection.SelectedItems.Count > 0;
                this.toolStripButtonShowUnit.Visible = this.listViewUnitCollection.SelectedItems.Count > 0;
            }
        }

        private void toolStripButtonRemoveUnit_Click(object sender, EventArgs e)
        {
            if (_UnitCollection != null)
            {
                if (this.listViewUnitCollection.SelectedItems.Count > 0)
                {
                    string key = this.listViewUnitCollection.SelectedItems[0].Name;
                    this.listViewUnitCollection.Items.Remove(this.listViewUnitCollection.SelectedItems[0]);
                }
                this.toolStripButtonRemoveUnit.Visible = this.listViewUnitCollection.SelectedItems.Count > 0;
                this.toolStripButtonShowUnit.Visible = this.listViewUnitCollection.SelectedItems.Count > 0;
            }
        }

        private void listViewUnitCollection_DoubleClick(object sender, EventArgs e)
        {
            if (this.listViewUnitCollection.SelectedItems.Count > 0)
            {
                this.Cursor = Cursors.WaitCursor;
                FormRemoteQuery fr = new FormRemoteQuery(this.listViewUnitCollection.SelectedItems[0].Name, _IWorkbenchUnit);
                fr.Show();
                this.Cursor = Cursors.Default;
            }
        }

        private void listViewUnitCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.toolStripButtonRemoveUnit.Visible = this.listViewUnitCollection.SelectedItems.Count > 0;
            this.toolStripButtonShowUnit.Visible = this.listViewUnitCollection.SelectedItems.Count > 0;
        }


        #endregion

        #region CacheDB

        private DiversityWorkbench.CacheDatabase _CacheDB;

        private void comboBoxCacheDBSource_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }

        private void comboBoxCacheDBSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._CacheDB.SourceViewDisplayText = this.comboBoxCacheDBSource.SelectedItem.ToString();
            this.userControlQueryListCacheDB.setQueryRestriction("Sourceview = '" + this._CacheDB.SourceView + "'", "#");
            this.ServerConnection.Project = this._CacheDB.SourceView;
        }

        private bool setCacheDbAsSource()
        {
            this._DatabaseService.IsCacheDB = true;
            string Service = this.comboBoxDatabase.Text.ToString();
            this._ServerConnection = DiversityWorkbench.Settings.ServerConnection;
            try
            {
                string SQL = ""; // SELECT TOP 1 DatabaseName FROM CacheDatabase 2";
                string Message = "";
                //string CacheDatabase = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                string CacheDatabase = DiversityWorkbench.CacheDatabase.CollectionCacheDB;
                if (Message.Length > 0)
                {
                    System.Windows.Forms.MessageBox.Show(Message);
                    return false;
                }
                SQL = "SELECT [" + CacheDatabase + "].[dbo].[DiversityWorkbenchModule] ()";
                string Module = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                if (Message.Length > 0)
                {
                    System.Windows.Forms.MessageBox.Show(Message);
                    return false;
                }
                this._ServerConnection.DatabaseName = CacheDatabase;
                this._ServerConnection.ModuleName = Module;
                this._ServerConnection.ConnectionIsValid = true;
                this._ServerConnection.BaseURL = "";
                this.labelOpenModule.Visible = false;
                this.buttonOpenModule.Visible = false;
                this._CacheDB = new CacheDatabase(this._ServerConnection);
                switch (this._IWorkbenchUnit.getServerConnection().ModuleName)
                {
                    case "DiversityTaxonNames":
                        _CacheDB.SetCacheDomain(DiversityWorkbench.CacheDatabase.CacheDomain.Names);
                        break;
                    case "":
                        break;
                    default:
                        break;
                }
                this.comboBoxCacheDBSource.Items.Clear();
                foreach (DiversityWorkbench.DatabaseService S in this._CacheDB.DatabaseServices())
                    this.comboBoxCacheDBSource.Items.Add(S.DisplayText);
                if (this.comboBoxCacheDBSource.Items.Count > 0)
                {
                    this.comboBoxCacheDBSource.SelectedIndex = 0;

                    this.userControlQueryListCacheDB.setConnection(this._CacheDB.ServerConnection.ConnectionString, "Sources");
                    this.userControlQueryListCacheDB.setQueryConditions(this._CacheDB.QueryConditions(this._CacheDB.DomainInCacheDB()));
                    this.userControlQueryListCacheDB.QueryDisplayColumns = this._CacheDB.QueryDisplayColumns();
                    this.userControlQueryListCacheDB.toolStripButtonConnection.Enabled = false;
                    //if (this.userControlQueryListCacheDB.DefaultControl() != null)
                    //    this.userControlQueryListCacheDB.DefaultControl().Focus();
                    this.splitContainerQueryLists.Panel1Collapsed = true;
                    this.splitContainerQueryLists.Panel2Collapsed = false;
                    this.splitContainerUnit.Panel1Collapsed = false;
                    this.splitContainerUnit.Panel2Collapsed = true;
                    this.checkBoxDisplayWebsite.Visible = false;
                    this.checkBoxDisplayWebsite.Checked = false;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }

        private void setUnitCacheDB(int ID)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.setUnitPanelCacheDB(ID);
                this.splitContainerUriResources.Panel2Collapsed = true;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            this.setTextHeights();
        }


        private void setUnitPanelCacheDB(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = this._CacheDB.UnitValues(ID);
            foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                C.Dispose();
            this.panelUnit.Controls.Clear();

            foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
            {
                if (!P.Key.StartsWith("_"))
                {
                    try
                    {
                        System.Windows.Forms.Label L = new Label();
                        L.Text = P.Key;
                        L.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Bold);
                        L.ForeColor = System.Drawing.Color.Gray;
                        L.Dock = DockStyle.Top;
                        L.TextAlign = ContentAlignment.BottomLeft;
                        this.panelUnit.Controls.Add(L);
                        L.BringToFront();

                        System.Windows.Forms.TextBox T = new TextBox();
                        T.Name = "textBox" + P.Key;
                        T.Dock = DockStyle.Top;
                        T.Height = (int)(39 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                        T.ReadOnly = true;
                        T.TextAlign = HorizontalAlignment.Center;
                        T.BorderStyle = BorderStyle.None;
                        T.Multiline = true;
                        T.ScrollBars = ScrollBars.Vertical;
                        T.Text = P.Value;
                        this.panelUnit.Controls.Add(T);
                        T.BringToFront();
                        //T.ScrollBars = ScrollBars.None;


                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                else
                {
                    if (P.Key == "_URI")
                        this._URI = P.Value;
                    if (P.Key == "_DisplayText" && P.Value.Length > 0)
                        this._DisplayText = P.Value;
                }
            }
            //foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
            //{
            //    foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
            //    {
            //        if (C.GetType() == typeof(System.Windows.Forms.TextBox) &&
            //            C.Name == "textBox" + P.Key)
            //        {
            //            if (C.Text.Length > 0) C.Text += ", ";
            //            C.Text += P.Value;
            //        }
            //    }
            //    if (P.Key == "_URI")
            //        this._URI = P.Value;
            //    if (P.Key == "_DisplayText" && P.Value.Length > 0)
            //        this._DisplayText = P.Value;
            //}

        }

        #endregion

        private void resetWebserviceDetailContent()
        {
            treeViewUnit.Nodes.Clear();
            userControlJsonTreeView.ClearJson();
            userControlJsonTreeView.Visible = false;
            buttonShowJsonTreeView.Text = "Show whole information as JSON treeview";
            buttonShowJsonTreeView.Visible = false;
        }

        private void buttonShowJsonTreeView_Click(object sender, EventArgs e)
        {
            buttonShowJsonTreeView.Visible = true;
            if (userControlJsonTreeView.Visible)
            {
                buttonShowJsonTreeView.Text = "Show whole information as JSON treeview";
                userControlJsonTreeView.Visible = false;
                treeViewUnit.Visible = true;
            }
            else
            {
                buttonShowJsonTreeView.Text = "Show generic summary";
                userControlJsonTreeView.Visible = true;
                treeViewUnit.Visible = true;
            }
        }

        #region getting all services

        /// <summary>
        /// # 128
        /// </summary>
        private void initGetAllServices()
        {
            if (this.ServerConnection != null && DiversityWorkbench.Settings.ModuleName == this.ServerConnection.ModuleName)
            {
                this.toolStripMenuItemGetAll.Visible = true;
                this.toolStripMenuItemGetAll.Enabled = true;
            }
            else 
            { 
                this.toolStripMenuItemGetAll.Visible = false;
                this.toolStripMenuItemGetAll.Enabled = false;
            }
        }
        private void toolStripMenuItemGetAll_Click(object sender, EventArgs e)
        {
            try
            {
                //System.Collections.Generic.List<DatabaseService> L = this._IWorkbenchUnit.DatabaseServices();
                //foreach (DatabaseService DS in L)
                //{
                //    if (comboBoxDatabase.Items.Contains(DS.DatabaseName))
                //        continue;
                //    this.comboBoxDatabase.Items.Add(DS.DatabaseName);
                //}
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.WorkbenchUnit> D = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList();
                foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in D)
                {
                    if (KV.Value.ServerConnection.ModuleName == DiversityWorkbench.Settings.ModuleName)
                    {
                        //foreach (DatabaseService DS in KV.Value.DatabaseServices())
                        //{
                        //    if (comboBoxDatabase.Items.Contains(DS.DatabaseName))
                        //        continue;
                        //    this.comboBoxDatabase.Items.Add(DS.DatabaseName);
                        //}
                        //foreach (System.Collections.Generic.KeyValuePair<string, string> kv in KV.Value.AccessibleDatabasesAndServicesOfModule())
                        //{
                        //    if (comboBoxDatabase.Items.Contains(kv.Key))
                        //        continue;
                        //    this.comboBoxDatabase.Items.Add(kv.Key);
                        //}
                        DiversityWorkbench.ServerConnection SC = new ServerConnection(KV.Value.ServerConnection.ConnectionString);
                        DiversityWorkbench.WorkbenchUnit unit = new WorkbenchUnit(SC);
                        foreach(string DS in unit.DatabaseList())
                        {
                            if (comboBoxDatabase.Items.Contains(DS))
                                continue;
                            this.comboBoxDatabase.Items.Add(DS);
                        }
                    }
                }

                this.toolStripMenuItemGetAll.Visible = false;
                this.toolStripMenuItemGetAll.Enabled = false;
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion
    }
}