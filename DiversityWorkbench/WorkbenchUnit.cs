#define UseAsyncAwait

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using DWBServices.WebServices;
using DWBServices.WebServices.TaxonomicServices;
using DWBServices.WebServices.TaxonomicServices.CatalogueOfLife;

namespace DiversityWorkbench
{

    #region Backlinks

    public class BackLink
    {
        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _Modules;

        public static Dictionary<string, List<string>> Modules
        {
            get
            {
                if (_Modules == null)
                {
                    _Modules = new Dictionary<string, List<string>>();
                    {
                        System.Collections.Generic.List<string> A = new List<string>();
                        A.Add("DiversityCollection");
                        A.Add("DiversityDescriptions");
                        A.Add("DiversityProjects");
                        _Modules.Add("DiversityAgents", A);

                        System.Collections.Generic.List<string> C = new List<string>();
                        C.Add("DiversityDescriptions");
                        _Modules.Add("DiversityCollection", C);

                        System.Collections.Generic.List<string> T = new List<string>();
                        T.Add("DiversityCollection");
                        T.Add("DiversityDescriptions");
                        _Modules.Add("DiversityTaxonNames", T);
                    }
                }
                return _Modules;
            }
        }

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ServerConnection>> _Databases;

        public static void Database_Add(string Module, ServerConnection serverConnection)
        {
            if (_Databases == null) _Databases = new Dictionary<string, List<ServerConnection>>();
            if (_Databases.ContainsKey(Module))
            {
                _Databases[Module].Add(serverConnection);
            }
            else
            {
                System.Collections.Generic.List<ServerConnection> L = new List<ServerConnection>();
                L.Add(serverConnection);
                _Databases.Add(Module, L);
            }
        }

        public static void Database_Remove(string Module, ServerConnection serverConnection)
        {
            if (_Databases != null && _Databases.ContainsKey(Module) && _Databases[Module].Contains(serverConnection))
            {
                _Databases[Module].Remove(serverConnection);
            }
        }

        public static bool DatabaseIsUpdated(string Module, ServerConnection serverConnection)
        {
            return (_Databases != null && _Databases.ContainsKey(Module) && _Databases[Module].Contains(serverConnection));
        }

    }

    /// <summary>
    /// The domains within a module containing backlinks to other modules, e.g. DiversityCollection contains backlinks to Terms in 3 tables (EventProperty, Identification, PartDescription)
    /// </summary>
    public class BackLinkDomain
    {
        /// <summary>
        /// Text shown in the user interface, e.g. List for the table TaxonNameListProjectProxy in DTN
        /// </summary>
        public string DisplayText;
        /// <summary>
        /// The name of the table containing the backlinks
        /// </summary>
        public string TableName;
        /// <summary>
        /// The column within the table containing the back links, e.g. NameURI in table Identification in DiversityCollection linked to DiversityTaxonNames
        /// </summary>
        public string ColumnName;
        /// <summary>
        /// The column within the table the cached value from the source, e.g. TaxonName in table Identification in DiversityCollection linked to DiversityTaxonNames
        /// </summary>
        public string CacheColumnName;
        /// <summary>
        /// An image representing the domain (if available)
        /// </summary>
        public int ImageKey;
        /// <summary>
        /// The dictionary of the objects containing the backlinks, e.g. CollectionSpecimen in DC
        /// </summary>
        //public System.Collections.Generic.Dictionary<string, int> Items;

        public System.Collections.Generic.List<BackLinkColumn> BackLinkColumns;

        public string Key { get { return TableName + "." + ColumnName; } }


        public System.Data.DataTable DtItems;

        public BackLinkDomain(string Display, string TableName, string Column, int ImageKey = 0)
        {
            this.TableName = TableName;
            this.ColumnName = Column;
            this.DisplayText = Display;
            this.ImageKey = ImageKey;
            this.BackLinkColumns = new List<BackLinkColumn>();
            //this.Items = new Dictionary<string, int>();
            this.DtItems = new System.Data.DataTable();
        }

        public BackLinkDomain(string DisplayText, string TableName, string Column, string CacheColumn, System.Collections.Generic.List<BackLinkColumn> backLinkColumns)
        {
            this.DisplayText = DisplayText;
            this.TableName = TableName;
            this.ColumnName = Column;
            this.CacheColumnName = CacheColumn;
            this.BackLinkColumns = backLinkColumns;
        }


        public void AddBacklinkColumn(string TableName, string Column, string BacklinkParameter)
        {
            BackLinkColumn backLinkColumn = new BackLinkColumn();
            backLinkColumn.TableName = TableName;
            backLinkColumn.ColumnName = Column;
            backLinkColumn.BacklinkParameter = BacklinkParameter;
            this.BackLinkColumns.Add(backLinkColumn);
        }

        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> BackLinkColumnTables()
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> dict = new Dictionary<string, System.Collections.Generic.List<string>>();
            System.Collections.Generic.List<string> lMain = new List<string>();
            lMain.Add(this.CacheColumnName);
            dict.Add(this.TableName, lMain);
            foreach (BackLinkColumn c in this.BackLinkColumns)
            {
                if (dict.ContainsKey(c.TableName))
                {
                    if (!dict[c.TableName].Contains(c.ColumnName))
                        dict[c.TableName].Add(c.ColumnName);
                }
                else
                {
                    System.Collections.Generic.List<string> L = new List<string>();
                    L.Add(c.ColumnName);
                    dict.Add(c.TableName, L);
                }
            }
            return dict;
        }

    }

    public struct BackLinkColumn
    {
        /// <summary>
        /// The name of the table containing the backlinks
        /// </summary>
        public string TableName;
        /// <summary>
        /// The column within the table containing the back links
        /// </summary>
        public string ColumnName;
        /// <summary>
        /// The value provided by the back link source
        /// </summary>
        public string BacklinkParameter;

        public string Key { get { return TableName + "." + ColumnName; } }

        public BackLinkColumn(string Table, string Column, string Parmeter)
        {
            TableName = Table;
            ColumnName = Column;
            BacklinkParameter = Parmeter;
        }
    }




    #endregion


    public class DatabaseService
    {
        public string DatabaseName;
        private string _Server;

        public string Server
        {
            get { return _Server; }
            set
            {
                _Server = value;
                if (this.DatabaseName.EndsWith(" on " + value))
                    this.DatabaseName = this.DatabaseName.Substring(0, this.DatabaseName.Length - (value.Length + 4));
            }
        }

        public bool IsWebservice;
        public bool IsForeignSource;
        public bool IsListInDatabase;
        public bool IsCacheDB;
        public string ListName;
        public bool DisplayUnitdataInTreeview;
        public string RestrictionForListInDatabase;
        public DwbServiceEnums.DwbService WebService;

        private bool _IsDatabaseOnLinkedServer;
        private string _LinkedServer;

        public string LinkedServer { get { return this._LinkedServer; } }

        /// <summary>
        /// If the database is located on a linked MS-SQL server
        /// </summary>
        public bool IsDatabaseOnLinkedServer
        {
            get
            {
                if (this.IsWebservice || this.IsForeignSource)
                    this._IsDatabaseOnLinkedServer = false;
                else if (this.DatabaseName.IndexOf("[") == 0)
                    this._IsDatabaseOnLinkedServer = true;
                else if (this._LinkedServer != null && this._LinkedServer.Length > 0)
                    this._IsDatabaseOnLinkedServer = true;
                else
                    this._IsDatabaseOnLinkedServer = false;
                return _IsDatabaseOnLinkedServer;
            }
            set { _IsDatabaseOnLinkedServer = value; }
        }

        public DatabaseService()
        {
            DatabaseName = "";
            _Server = "";
            ListName = "";
            IsWebservice = false;
            IsForeignSource = false;
            IsListInDatabase = false;
            IsCacheDB = false;
            DisplayUnitdataInTreeview = false;
            RestrictionForListInDatabase = "";
            WebService = DwbServiceEnums.DwbService.None;
        }

        public DatabaseService(string Database)
            : this()
        {
            DatabaseName = Database;
        }

        public DatabaseService(string Database, string LinkedServer)
            : this()
        {
            DatabaseName = Database;
            this._LinkedServer = LinkedServer;
            this.IsDatabaseOnLinkedServer = true;
        }

        public string DatabaseOnServer
        {
            get
            {
                string D = this.DatabaseName;
                if (this._Server.Length > 0)
                    D += " on " + _Server;
                return D;
            }
        }

        public string DisplayText
        {
            get
            {
                string D = this.DatabaseName;
                if (this._Server.Length > 0)
                    D += " on " + _Server;
                if (this.IsDatabaseOnLinkedServer && this._LinkedServer != null && this._LinkedServer.Length > 0)
                    D = "[" + this._LinkedServer + "]." + this.DatabaseName;
                if (this.IsListInDatabase)
                    D = this.ListName.Trim() + " [" + D + "]";
                return D;
            }
        }
    }

    public interface IWorkbenchUnit
    {
        bool IsAnyServiceAvailable();
        //bool DisplayUnitdataInTreeview();
        void BuildUnitTree(int ID, System.Windows.Forms.TreeView TreeView);
        void BuildUnitTree(string URI, System.Windows.Forms.TreeView TreeView);
        void setServerConnection(DiversityWorkbench.ServerConnection ServerConnection);
        DiversityWorkbench.ServerConnection getServerConnection();
        string MainTable();
        string ServiceName();
        DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns();
        System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions();
        System.Collections.Generic.Dictionary<string, string> UnitValues(int ID);
        System.Collections.Generic.Dictionary<string, string> UnitResources(int ID);
        bool DoesExist(int ID);
        //System.Collections.Generic.Dictionary<string, string> UnitValues(string sID);
        System.Collections.Generic.Dictionary<string, string> UnitValues();
        string SourceURI(DiversityWorkbench.UserControls.UserControlWebView WebBrowser);
        void SetUnitValues(System.Collections.Generic.Dictionary<string, string> UnitValues);
        void setDatabaseServiceRestriction(string Restriction);
        System.Collections.Generic.List<DiversityWorkbench.QueryCondition> PredefinedQueryPersistentConditionList();
        //bool ShowMaxResultsForQuery();

        // alt
        System.Collections.Generic.List<DiversityWorkbench.DatabaseService> DatabaseServices();

        // neu
        System.Collections.Generic.Dictionary<string, string> AccessibleDatabasesAndServicesOfModule();
        System.Collections.Generic.Dictionary<string, string> DatabaseAndServiceURIs();
        System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> ServerConnections();

        System.Collections.Generic.Dictionary<string, string> AdditionalServicesOfModule();

        // for Domains
        System.Collections.Generic.Dictionary<string, string> UnitValues(string Domain, int ID);
        DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns(string Domain);
        System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions(string Domain);

        // for dependent
        bool DependentQueryAvailable(string URI);
        DiversityWorkbench.UserControls.QueryDisplayColumn[] DependentQueryDisplayColumns(string URI);
        System.Collections.Generic.List<DiversityWorkbench.QueryCondition> DependentQueryConditions(string URI);
    }

    public class WorkbenchUnit
    {

        #region Parameter

        private System.Collections.Generic.List<string> _DatabaseList;

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> _ServerConnectionList;

        protected DiversityWorkbench.ServerConnection _ServerConnection;
        //protected bool? _DatabaseIsAvailable;
        protected System.Collections.Generic.Dictionary<string, string> _UnitValues;
        protected System.Collections.Generic.Dictionary<string, string> _DataBaseURIs;
        protected System.Collections.Generic.Dictionary<string, string> _Services;
        protected string _DatabaseServiceRestriction = "";
        protected string _UriHtmlRecord;
        protected System.Collections.Generic.List<string> _DataDomains;

        public enum ServiceType { WorkbenchModule, WebService, CacheDB, Unknown };

        public enum ModuleType { Agents, Collection, /*CollectionProject,*/ Descriptions, Exsiccatae, Gazetteer, Projects, References, SamplingPlots, ScientificTerms, TaxonNames, None }

        /// <summary>
        /// Features of client application
        /// CreateItem: Client may be opened for creating an item with command arguments: ApplicationName 'CreateItem' DatabaseServer Port DatabaseName URI [user password] 
        ///             URI specifies the target dataset of the caller that shall be referenced in the new item
        /// SingleItem: Client may be opened for editing by a command arguments: ApplicationName 'SingleItem' DatabaseServer Port DatabaseName URI [user password] 
        ///             URI specifies the dataset that shall be opened for editing
        /// AdditionalUnitValues: Unit provides values GetAdditionalUnitValues as text
        /// HtmlUnitValues: Unit provides values in HtmlUnitValues as HTML text !!! removed, generic HtmlUnitValues in WorkbenchUnit available!!!
        /// (to be extended)
        /// </summary>
        public enum ClientFeature { CreateItem, SingleItem, AdditionalUnitValues /*, HtmlUnitValues*/ }

        protected List<ClientFeature> _FeatureList = null;
        public List<ClientFeature> FeatureList
        {
            get
            {
                List<ClientFeature> result = new List<ClientFeature>();
                if (_FeatureList != null)
                {
                    foreach (ClientFeature item in _FeatureList)
                        result.Add(item);
                }
                return result;
            }
            //set { _FeatureList = value; }
        }

        #endregion

        #region Construction

        public WorkbenchUnit(DiversityWorkbench.ServerConnection ServerConnection)
        {
            this.ServerConnection = ServerConnection;
            this.AddUnitToGlobalList();
        }

        public WorkbenchUnit(DiversityWorkbench.ServerConnection ServerConnection, System.Uri URL)
        {
            try
            {
                this.ServerConnection = ServerConnection;
                string Database = URL.OriginalString.Substring(URL.Scheme.Length).Replace("://", "").Substring(URL.Host.Length + 1);
                Database = Database.Substring(0, Database.IndexOf('/'));
                string BaseURL = URL.Scheme + "://" + URL.Host + "/" + Database + "/";
                if (ServerConnection.BaseURL == null || ServerConnection.BaseURL.ToLower() != BaseURL.ToLower())
                    ServerConnection.BaseURL = BaseURL;
                if (!Database.StartsWith("Diversity"))
                    Database = "Diversity" + Database.Substring(0, 1).ToUpper() + Database.Substring(1);
                if (ServerConnection.DatabaseName == null || ServerConnection.DatabaseName.ToLower() != Database.ToLower())
                    ServerConnection.DatabaseName = Database;
                this.AddUnitToGlobalList();
            }
            catch (System.Exception ex) { }
        }

        public WorkbenchUnit(DiversityWorkbench.ServerConnection ServerConnection, bool IsAddedRemoteConnection)
        {
            try
            {
                if (IsAddedRemoteConnection)
                {
                    this._ServerConnection = ServerConnection;
                    this._ServerConnection.IsAddedRemoteConnection = IsAddedRemoteConnection;
                    if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this._ServerConnection.ModuleName))
                    {
                        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.Add(this._ServerConnection.ModuleName, this);
                        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList = new Dictionary<string, ServerConnection>();
                    }
                    if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList.ContainsKey(this._ServerConnection.DatabaseName))
                    {
                        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList.Add(this._ServerConnection.DatabaseName, this._ServerConnection);
                    }
                }
                else
                {
                    this.ServerConnection = ServerConnection;
                    this.AddUnitToGlobalList();
                }
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Properties

        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get
            {
                if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList != null &&
                    DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this._ServerConnection.ModuleName))
                {
                    if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList == null)
                        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList = new Dictionary<string, ServerConnection>();
                    if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList.ContainsKey(this._ServerConnection.DatabaseName))
                    {
                        return DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList[this._ServerConnection.DatabaseName];
                    }
                    else
                    {
                        if (this._ServerConnection.ModuleName == DiversityWorkbench.Settings.ModuleName
                            && this._ServerConnection.DatabaseName != DiversityWorkbench.Settings.DatabaseName)
                            this._ServerConnection.DatabaseName = DiversityWorkbench.Settings.DatabaseName;
                    }
                }
                return this._ServerConnection;
            }

            set
            {
                if (value != null)
                    this._ServerConnection = value;
                else
                {
                    this._ServerConnection = new ServerConnection();
                    this._ServerConnection.DatabaseServer = "127.0.0.1";
                    this._ServerConnection.IsTrustedConnection = true;
                    this._ServerConnection.DatabaseName = this.ServiceName();
                }
                this._ServerConnection.ModuleName = this.ServiceName();
                if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList == null)
                    DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList = new Dictionary<string, WorkbenchUnit>();
                if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName()))
                    DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().Add(this.ServiceName(), this);
                if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList == null)
                    DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList = new Dictionary<string, ServerConnection>();
#if DEBUG
                if (this.DatabaseList().Count > 0 && (this._ServerConnection.DatabaseName == null || !this._ServerConnection.DatabaseName.StartsWith(this._ServerConnection.ModuleName)))
                    this._ServerConnection.DatabaseName = this.DatabaseList()[0];


                // for the same module take only the current database
                if (ServerConnection.ModuleName == DiversityWorkbench.Settings.ModuleName
                    && ServerConnection.DatabaseName != DiversityWorkbench.Settings.DatabaseName)
                    return;

                if (ServerConnection.ConnectionString.Length > 0 && ServerConnection.ConnectionIsValid)
                {
                    if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.ContainsKey(ServerConnection.DisplayText))
                        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(ServerConnection.DisplayText, ServerConnection);
                    else
                    {
                        if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList[ServerConnection.DisplayText].ConnectionString != ServerConnection.ConnectionString)
                        {
                            DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Remove(ServerConnection.DisplayText);
                            if (ServerConnection.ConnectionIsValid)
                                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(ServerConnection.DisplayText, ServerConnection);
                        }
                    }
                }
#else
                if (!DiversityWorkbench.Settings.LoadConnections && !DiversityWorkbench.Settings.LoadConnectionsPassed())
                {
                    if (ServerConnection.ModuleName == DiversityWorkbench.Settings.ModuleName
                        && ServerConnection.DatabaseName == DiversityWorkbench.Settings.DatabaseName
                        && !DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.ContainsKey(ServerConnection.DisplayText))
                        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(ServerConnection.DisplayText, ServerConnection);
                }
                else
                {
                    if (this.DatabaseList().Count > 0 && (this._ServerConnection.DatabaseName == null || !this._ServerConnection.DatabaseName.StartsWith(this._ServerConnection.ModuleName)))
                        this._ServerConnection.DatabaseName = this.DatabaseList()[0];


                    // for the same module take only the current database
                    if (ServerConnection.ModuleName == DiversityWorkbench.Settings.ModuleName
                        && ServerConnection.DatabaseName != DiversityWorkbench.Settings.DatabaseName)
                        return;

                    if (ServerConnection.ConnectionString.Length > 0 && ServerConnection.ConnectionIsValid)
                    {
                        if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.ContainsKey(ServerConnection.DisplayText))
                            DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(ServerConnection.DisplayText, ServerConnection);
                        else
                        {
                            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList[ServerConnection.DisplayText].ConnectionString != ServerConnection.ConnectionString)
                            {
                                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Remove(ServerConnection.DisplayText);
                                if (ServerConnection.ConnectionIsValid)
                                    DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(ServerConnection.DisplayText, ServerConnection);
                            }
                        }
                    }
                }
#endif
            }
        }

        public string Prefix
        {
            get
            {
                string Prefix = "";
                if (this._ServerConnection.LinkedServer.Length > 0)
                    Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
                else Prefix = "dbo.";
                return Prefix;
            }
        }

        public bool IsLinkedServer
        {
            get
            {
                return (this._ServerConnection.LinkedServer.Length > 0);
            }
        }

        #endregion

        #region Interface
        /// <summary>
        /// For DiversityWorkbench modules this responds to the name of the module
        /// </summary>
        /// <returns>The name of the module resp. the service</returns>
        public virtual string ServiceName() { return DiversityWorkbench.Settings.ModuleName; }
        //public virtual System.Collections.Generic.List<string> DatabaseList()
        //{
        //    if (this._DatabaseList == null) this._DatabaseList = new List<string>();
        //    else this._DatabaseList.Clear();
        //    this._DatabaseList.Add(this.ServerConnection.ModuleName);
        //    return this._DatabaseList;
        //}

        /// <summary>
        /// Data domains within a module different from the main data domain
        /// e.g. Terminology in ScientificTerms, IdentificationUnit in Collection etc.
        /// </summary>
        /// <returns></returns>
        public virtual System.Collections.Generic.List<string> DataDomains()
        {
            if (this._DataDomains == null) this._DataDomains = new List<string>();
            return this._DataDomains;
        }

        public virtual bool DoesExist(int ID)
        {
            // Markus 20230308: Empty unit values indicate non existence of dataset
            Dictionary<string, string> unitValues = this.UnitValues(ID);
            bool Exists = false;
            if (unitValues.Count > 1)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in unitValues)
                {
                    if (KV.Value != null && KV.Value.Length > 0)
                    {
                        Exists = true;
                        break;
                    }
                }
            }
            return Exists;

            return unitValues.Count > 1;
        }

        public virtual string UriHtmlRecord() { return this._UriHtmlRecord; }

        public virtual void setUriHtmlRecord(string URL) { this._UriHtmlRecord = URL; }

        public void ResetDatabaseList()
        {
            this._DatabaseList = null;
            _Databases = null;
        }

        // Markus 3.12.2021 - List wurde mehrfach aufgerufen - Speichern in static Dictionary
        private static System.Collections.Generic.Dictionary<string, List<string>> _Databases;
        public System.Collections.Generic.List<string> DatabaseList()
        {
            // Markus 14.6.2019: Unter bestimmten umstaenden war die Liste nicht aktuell - noch nicht ganz klar wann. Die manuell hinzugefügten DBs haben gefehlt
            if (this._DatabaseList != null)
            {
                if (this.ServiceName() != null)
                {
                    if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList != null) // Markus 14.8.2019 - make sure to exist
                    {
                        if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName()))
                        {
                            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList != null)
                            {
                                if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Count > this._DatabaseList.Count)
                                    this._DatabaseList = null;
                            }
                        }
                    }
                }
            }

            if (this._DatabaseList != null)
                return this._DatabaseList;

            // Markus 3.12.2021 - List wurde mehrfach aufgerufen - Speichern in static Dictionary
#if xx_DEBUG
            if (_Databases != null && _Databases.ContainsKey(this.ServerConnection.ModuleName))
                return _Databases[this.ServerConnection.ModuleName];
#endif

            if (this._DatabaseList == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                this._DatabaseList = new List<string>();
                try
                {
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand("", con);
                    string SQL = "use master " +
                            "select name from sys.databases where name like('" + this.ServiceName() + "%') AND state_desc = 'ONLINE'";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    ad.Fill(dt);
                    con.Open();
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        C.CommandText = "IF EXISTS (SELECT * FROM " + R[0].ToString() + ".sys.objects " +
                            "WHERE object_id = OBJECT_ID(N'[" + R[0].ToString() + "].[dbo].[DiversityWorkbenchModule]') " +
                            "AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) SELECT 1 ELSE SELECT 0";
                        try
                        {
                            // Test Access for current login
                            bool DBaccessible = true;
                            try { C.ExecuteScalar(); }
                            catch (System.Exception ex) { DBaccessible = false; }

                            if (DBaccessible && C.ExecuteScalar()?.ToString() == "1")
                            {
                                C.CommandText = "SELECT [" + R[0].ToString() + "].[dbo].[DiversityWorkbenchModule]()";
                                string Module = C.ExecuteScalar()?.ToString();
                                if (!R[0].ToString().StartsWith(Module) && !(R[0].ToString().IndexOf("[") == 0 && R[0].ToString().IndexOf("].[") > -1 && R[0].ToString().IndexOf("].[" + Module) > 0))
                                    continue;
                                if (this.ServerConnection == null)
                                    continue;
                                if (this.ServerConnection.ModuleName != Module) // MW 23.3.2015 - otherwise cache DBs would list as well
                                    continue;
                                this._DatabaseList.Add(R[0].ToString());
                                if (!DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey(this.ServiceName()))
                                {
                                    DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().Add(this.ServiceName(), this);
                                }
                                DiversityWorkbench.ServerConnection S = new ServerConnection();
                                S.DatabaseName = R[0].ToString();
                                S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
                                S.LinkedServer = "";
                                S.DatabasePassword = DiversityWorkbench.Settings.Password;
                                S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
                                S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                                S.IsLocalExpressDatabase = DiversityWorkbench.Settings.IsLocalExpressDatabase;
                                S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
                                S.ModuleName = this.ServiceName();
                                S.SqlExpressDbFileName = DiversityWorkbench.Settings.SqlExpressDbFileNameForDatabase(S.DatabaseName);

                                // Testing the connection
                                string s = S.ConnectionString;
                                if (s.Length > 0)
                                {
                                    // Assure that list exists
                                    DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList();
                                    if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName()))
                                        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.Add(this.ServiceName(), this);

                                    if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList == null)
                                        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList = new Dictionary<string, ServerConnection>();

                                    if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.ContainsKey(S.DisplayText)
                                        && (S.ModuleName != DiversityWorkbench.Settings.ModuleName
                                        || (S.ModuleName == DiversityWorkbench.Settings.ModuleName
                                        && S.DatabaseName == DiversityWorkbench.Settings.DatabaseName)))
                                    {
                                        if (S.ConnectionIsValid)
                                            DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(S.DisplayText, S);
                                        else
                                        {
                                            DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(S.DatabaseName, S);
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }

                    // Adding databases from linked server
                    string Message = this.AddLinkedServerDatabasesSync();
                    if (Message.Length > 0)
                        System.Windows.Forms.MessageBox.Show(Message, "Failed connections", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                    // Adding manually added databases
                    try
                    {
                        if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList != null &&
                            DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName()) &&
                            DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()].ServerConnectionList() != null)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, ServerConnection> SC in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()].ServerConnectionList())
                            {
                                if (SC.Value.ConnectionIsValid && !this._DatabaseList.Contains(SC.Key))
                                    this._DatabaseList.Add(SC.Key);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    con.Close();
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }

                // Markus 3.12.2021 - List wurde mehrfach aufgerufen - Speichern in static Dictionary
#if xxDEBUG
                if (_Databases == null)
                    _Databases = new Dictionary<string, List<string>>();
#endif
                if (_Databases != null && !_Databases.ContainsKey(this.ServerConnection.ModuleName))
                    _Databases.Add(this.ServerConnection.ModuleName, _DatabaseList);

                return this._DatabaseList;
            }
            else
            {
                System.Collections.Generic.List<string> L = new List<string>();
                return L;
            }
        }

        private string AddLinkedServerDatabasesSync()
        {
            if (!DiversityWorkbench.Settings.LoadConnections && !DiversityWorkbench.Settings.LoadConnectionsPassed())
                return "";

            string Message = "";
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServer> KV in DiversityWorkbench.LinkedServer.LinkedServers())
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServerDatabase> KVdb in KV.Value.Databases(this.ServiceName()))
                    {
                        try
                        {
                            DiversityWorkbench.ServerConnection S = new ServerConnection();
                            S.DatabaseName = KVdb.Value.DatabaseName;
                            S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
                            S.LinkedServer = KV.Key;
                            S.DatabasePassword = DiversityWorkbench.Settings.Password;
                            S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
                            S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                            S.IsLocalExpressDatabase = DiversityWorkbench.Settings.IsLocalExpressDatabase;
                            S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
                            S.ModuleName = this.ServiceName();
                            S.BaseURL = DiversityWorkbench.LinkedServer.LinkedServers()[KV.Key].Databases()[KVdb.Key].BaseURL;
                            S.SqlExpressDbFileName = DiversityWorkbench.Settings.SqlExpressDbFileNameForDatabase(S.DatabaseName);
                            // Testing the connection
                            string s = S.ConnectionString;
                            if (s.Length > 0)
                            {
                                // Assure that list exists
                                DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList();
                                if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName()))
                                    DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.Add(this.ServiceName(), this);

                                if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList == null)
                                    DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList = new Dictionary<string, ServerConnection>();

                                // Toni 10.2.2016: Copy validity flag if connection is already in table
                                if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.ContainsKey(S.DisplayText))
                                    S.ConnectionIsValid = DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList[S.DisplayText].ConnectionIsValid;
                                else if (S.ModuleName != DiversityWorkbench.Settings.ModuleName ||
                                         (S.ModuleName == DiversityWorkbench.Settings.ModuleName && S.DatabaseName == DiversityWorkbench.Settings.DatabaseName))
                                {
                                    // Markus 22.11.2019 Testing the connection
                                    Microsoft.Data.SqlClient.SqlConnection cTest = new Microsoft.Data.SqlClient.SqlConnection(S.ConnectionString);
                                    string Command = "SELECT dbo.BaseURL()";
                                    if (S.LinkedServer != null)
                                        Command = "SELECT BaseURL FROM [" + S.LinkedServer + "].[" + S.DatabaseName + "].dbo.ViewBaseURL";
                                    Microsoft.Data.SqlClient.SqlCommand Ctest = new Microsoft.Data.SqlClient.SqlCommand(Command, cTest);
                                    // Markus 11.3.2022 - shorting testtime to new timeout
                                    Ctest.CommandTimeout = DiversityWorkbench.Settings.TimeoutConnection;
                                    cTest.Open();
                                    string BaseURL = "";
                                    try
                                    {
                                        BaseURL = Ctest.ExecuteScalar().ToString();
                                    }
                                    catch (Exception)
                                    { }
                                    cTest.Close();
                                    if (BaseURL == S.BaseURL)
                                    {
                                        S.ConnectionIsValid = true;
                                        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(S.DisplayText, S);
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            if (Message.Length > 0) Message += "\r\n";
                            Message += KV.Key + " - " + KVdb.Key + ":\r\n" + ex.Message;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Message;
        }

        private void AddLinkedServerDatabasesAsync()
        {
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServer> KV in DiversityWorkbench.LinkedServer.LinkedServers())
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServerDatabase> KVdb in KV.Value.Databases(this.ServiceName()))
                    {
                        DiversityWorkbench.ServerConnection S = new ServerConnection();
                        S.DatabaseName = KVdb.Value.DatabaseName;
                        S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
                        S.LinkedServer = KV.Key;
                        S.DatabasePassword = DiversityWorkbench.Settings.Password;
                        S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
                        S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                        S.IsLocalExpressDatabase = DiversityWorkbench.Settings.IsLocalExpressDatabase;
                        S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
                        S.ModuleName = this.ServiceName();
                        S.BaseURL = DiversityWorkbench.LinkedServer.LinkedServers()[KV.Key].Databases()[KVdb.Key].BaseURL;
                        S.SqlExpressDbFileName = DiversityWorkbench.Settings.SqlExpressDbFileNameForDatabase(S.DatabaseName);
                        // Testing the connection
                        string s = S.ConnectionString;
                        if (s.Length > 0)
                        {
                            // Assure that list exists
                            DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList();
                            if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName()))
                                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.Add(this.ServiceName(), this);

                            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList == null)
                                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList = new Dictionary<string, ServerConnection>();

                            // Toni 10.2.2016: Copy validity flag if connection is already in table
                            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.ContainsKey(S.DisplayText))
                                S.ConnectionIsValid = DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList[S.DisplayText].ConnectionIsValid;
                            else if (S.ModuleName != DiversityWorkbench.Settings.ModuleName ||
                                     (S.ModuleName == DiversityWorkbench.Settings.ModuleName && S.DatabaseName == DiversityWorkbench.Settings.DatabaseName))
                            {
                                // Markus 22.11.2019 Testing the connection
                                Microsoft.Data.SqlClient.SqlConnection cTest = new Microsoft.Data.SqlClient.SqlConnection(S.ConnectionString);
                                string Command = "SELECT dbo.BaseURL()";
                                if (S.LinkedServer != null)
                                    Command = "SELECT BaseURL FROM [" + S.LinkedServer + "].[" + S.DatabaseName + "].dbo.ViewBaseURL";
                                Microsoft.Data.SqlClient.SqlCommand Ctest = new Microsoft.Data.SqlClient.SqlCommand(Command, cTest);
                                // Markus 11.3.2022 - shorting testtime to new timeout
                                Ctest.CommandTimeout = DiversityWorkbench.Settings.TimeoutConnection;
                                cTest.Open();
                                string BaseURL = Ctest.ExecuteScalar().ToString();
                                cTest.Close();
                                if (BaseURL == S.BaseURL)
                                {
                                    S.ConnectionIsValid = true;
                                    DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(S.DisplayText, S);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> ServerConnections()
        {
            if ((this._ServerConnectionList == null || (this._ServerConnectionList != null && this._ServerConnectionList.Count == 0)) && DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList != null) // Markus 14.8.2019 - make sure DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList exists
            {
                if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName()))
                {
                    this._ServerConnectionList = DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList;
                }
            }
            return this._ServerConnectionList;
        }

        public virtual System.Collections.Generic.Dictionary<string, string> Services()
        {
            if (this._Services == null) this._Services = new Dictionary<string, string>();
            if (this._DataBaseURIs != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._DataBaseURIs)
                {
                    if (!this._Services.ContainsKey(KV.Key))
                        this._Services.Add(KV.Key, KV.Value);
                }
            }
            return this._Services;
        }

        public virtual void setDatabaseServiceRestriction(string Restriction)
        {
            this._DatabaseServiceRestriction = Restriction;
        }

        public bool IsAnyServiceAvailable()
        {
            try
            {
                if (this.AdditionalServicesOfModule().Count > 0) return true;
                else
                {
                    if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName()))
                    {
                        if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList == null)
                            DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList = new Dictionary<string, ServerConnection>();
                        if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Count > 0)
                            return true;
                        else
                            return false;
                    }
                    else return false;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return false;
        }

        public virtual void BuildUnitTree(string URI, System.Windows.Forms.TreeView TreeView)
        {
            TreeView.Nodes.Clear();
            System.Windows.Forms.TreeNode N = new System.Windows.Forms.TreeNode("URI: " + URI);
            TreeView.Nodes.Add(N);
        }

        public virtual void BuildUnitTree(int ID, System.Windows.Forms.TreeView TreeView)
        {
            TreeView.Nodes.Clear();
            System.Windows.Forms.TreeNode N = new System.Windows.Forms.TreeNode("ID: " + ID.ToString());
            TreeView.Nodes.Add(N);
        }

        public void setServerConnection(DiversityWorkbench.ServerConnection ServerConnection)
        {
            this.ServerConnection = ServerConnection;
            if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName()))
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.Add(this.ServiceName(), this);
            if (ServerConnection.ModuleName != DiversityWorkbench.Settings.ModuleName
                || (ServerConnection.ModuleName == DiversityWorkbench.Settings.ModuleName
                && ServerConnection.DatabaseName == DiversityWorkbench.Settings.DatabaseName))
            {
                // Check if connection is already in list
                bool match = false;
                foreach (KeyValuePair<string, ServerConnection> item in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList)
                {
                    if (ServerConnection.ConnectionString == item.Value.ConnectionString)
                    {
                        match = true;
                        break;
                    }
                }

                if (!match)
                {
                    if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.ContainsKey(ServerConnection.DatabaseName))
                        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(ServerConnection.DatabaseName, ServerConnection);
                    else if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.ContainsKey(ServerConnection.DisplayText))
                    {
                        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(ServerConnection.DisplayText, ServerConnection);
                    }
                    else
                    {
                        //DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Remove(ServerConnection.DatabaseName);
                        //DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList.Add(ServerConnection.DatabaseName, ServerConnection);
                    }
                }
            }
        }

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> ServerConnectionList(bool Requery = false)
        {
            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName()))
            {
                if (Requery)
                {
                    //this.setServerConnection(this.ServerConnection);
                    RequeryWorkbenchUnitConnections();
                    //DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList = null;
                }
                return DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList;
            }
            else return null;
        }

        public virtual string SourceURI(DiversityWorkbench.UserControls.UserControlWebView WebBrowser) { return ""; }

        public DiversityWorkbench.ServerConnection getServerConnection()
        {
            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this._ServerConnection.ModuleName))
            {
                if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList == null)
                    DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList = new Dictionary<string, ServerConnection>();
                if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList.ContainsKey(this._ServerConnection.DatabaseName))
                {
                    return DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList[this._ServerConnection.DatabaseName];
                }
                else if (this._ServerConnection.LinkedServer.Length > 0 &&
                    DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList.ContainsKey("[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName))
                {
                    return DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this._ServerConnection.ModuleName]._ServerConnectionList["[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName];
                }
                else
                {
                    if (this._ServerConnection.ModuleName == DiversityWorkbench.Settings.ModuleName
                        && this._ServerConnection.DatabaseName != DiversityWorkbench.Settings.DatabaseName)
                        this._ServerConnection.DatabaseName = DiversityWorkbench.Settings.DatabaseName;
                    if (this._ServerConnection.ConnectionString.IndexOf(this._ServerConnection.ModuleName) == -1)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                        {
                            if (KV.Value._ServerConnection.ModuleName == this._ServerConnection.ModuleName)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> SC in KV.Value._ServerConnectionList)
                                {
                                    this._ServerConnection.DatabaseName = SC.Value.DatabaseName;
                                    this._ServerConnection.DatabaseUser = SC.Value.DatabaseUser;
                                    this._ServerConnection.IsTrustedConnection = SC.Value.IsTrustedConnection;
                                    this._ServerConnection.DatabasePassword = SC.Value.DatabasePassword;
                                    this._ServerConnection.BaseURL = SC.Value.BaseURL;
                                    this._ServerConnection.DatabaseServer = SC.Value.DatabaseServer;
                                    this._ServerConnection.DatabaseServerPort = SC.Value.DatabaseServerPort;
                                    break;
                                }
                            }
                        }
                    }

                }
            }
            return this._ServerConnection;
        }

        public virtual System.Collections.Generic.List<DiversityWorkbench.QueryCondition> PredefinedQueryPersistentConditionList()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> _PredefinedQueryPersistentConditionList = new List<QueryCondition>();
            return _PredefinedQueryPersistentConditionList;
        }

        #region UnitValues

        public virtual System.Collections.Generic.Dictionary<string, string> UnitValues()
        {
            if (this._UnitValues == null)
                this._UnitValues = new Dictionary<string, string>();
            return this._UnitValues;
        }

        public virtual System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            return Values;
        }

        //public virtual System.Collections.Generic.Dictionary<int, string> UnitValues(System.Collections.Generic.List<int> IDs, int ProjectID)
        //{
        //    System.Collections.Generic.Dictionary<int, string> values = new Dictionary<int, string>();
        //    return values;
        //}

        /// <summary>
        /// Getting JSON for all items of a project
        /// </summary>
        /// <param name="ProjectID">ID of the project</param>
        /// <returns>Dictionary containing the ID of the item and the corresponding data as JSON</returns>
        public virtual System.Collections.Generic.Dictionary<int, System.Object> ModuleProjectValues(int ProjectID)
        {
            System.Collections.Generic.Dictionary<int, System.Object> values = new Dictionary<int, System.Object>();
            return values;
        }

        public virtual void GetAdditionalUnitValues(System.Collections.Generic.Dictionary<string, string> UnitValues)
        {
        }

        public virtual string HtmlUnitValues(Dictionary<string, string> UnitValues)
        {
            // String builder for result
            StringBuilder sb = new StringBuilder();

            try
            {
                // Start XML writer
                using (System.Xml.XmlWriter W = System.Xml.XmlWriter.Create(sb))
                {
                    // Save title
                    string title = this.ServiceName();
                    if (UnitValues["_DisplayText"].Length > 0)
                        title = UnitValues["_DisplayText"];

                    // Start HTML document
                    W.WriteStartElement("html");
                    W.WriteString("\r\n");
                    W.WriteStartElement("head");
                    W.WriteElementString("title", title);
                    W.WriteEndElement();//head
                    W.WriteString("\r\n");
                    W.WriteStartElement("body");
                    W.WriteString("\r\n");

                    // Write reference title
                    W.WriteStartElement("h2");
                    W.WriteStartElement("font");
                    W.WriteAttributeString("face", "Verdana");
                    DiversityWorkbench.Description.WriteXmlString(W, title);
                    W.WriteEndElement();//font
                    W.WriteEndElement();//h2

                    // Start table
                    W.WriteStartElement("table");
                    //W.WriteAttributeString("width", "900");
                    W.WriteAttributeString("border", "0");
                    W.WriteAttributeString("cellpadding", "1");
                    W.WriteAttributeString("cellspacing", "0");
                    W.WriteAttributeString("class", "small");
                    W.WriteAttributeString("style", "margin-left:0px");

                    foreach (KeyValuePair<string, string> item in UnitValues)
                    {
                        // Skip irrelevant entries
                        if (item.Key.StartsWith("_"))
                            continue;
                        if (item.Key.StartsWith("Link to"))
                            continue;

                        // Insert unit value
                        if (item.Value.Trim() != "")
                        {
                            W.WriteStartElement("tr");
                            //W.WriteAttributeString("style", "padding-top:10px; padding-bottom:20px");

                            // Write first column <td width=_ColumnWidth align="right">
                            W.WriteStartElement("td");
                            W.WriteAttributeString("width", "150");
                            W.WriteAttributeString("align", "right");
                            W.WriteAttributeString("valign", "top");
                            W.WriteAttributeString("style", "padding-right:5px");
                            W.WriteStartElement("font");
                            W.WriteAttributeString("face", "Verdana");
                            W.WriteAttributeString("size", "2");
                            DiversityWorkbench.Description.WriteXmlString(W, string.Format("<b>{0}</b>", item.Key));
                            W.WriteEndElement();//font
                            W.WriteEndElement();//td

                            // Write second column <td style="padding-left:5px">
                            W.WriteStartElement("td");
                            W.WriteAttributeString("style", "padding-left:5px");
                            W.WriteStartElement("font");
                            W.WriteAttributeString("face", "Verdana");
                            W.WriteAttributeString("size", "2");
                            DiversityWorkbench.Description.WriteXmlString(W, item.Value);
                            W.WriteEndElement();//font
                            W.WriteEndElement();//td
                            W.WriteEndElement();//tr
                        }
                    }

                    W.WriteEndElement();//body
                    W.WriteEndElement();//html
                    W.WriteEndDocument();
                    W.Flush();
                    W.Close();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return sb.ToString();
        }

        public virtual System.Collections.Generic.Dictionary<string, string> UnitResources(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            return Values;
        }

        public virtual void SetUnitValues(System.Collections.Generic.Dictionary<string, string> UnitValues)
        {
            this._UnitValues = UnitValues;
        }

        public virtual System.Collections.Generic.Dictionary<string, string> UnitValues(string URI)
        {
            int ID;
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV
                    in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.ServiceName()].ServerConnectionList())
                {
                    if (compareUriStart(URI, KV.Value.BaseURL))
                    {
                    //}
                    //if (URI.ToLower().StartsWith(KV.Value.BaseURL.ToLower()))
                    //{
                        string IdInUri = URI.Substring(KV.Value.BaseURL.Length);
                        this._ServerConnection.DatabaseName = KV.Value.DatabaseName;
                        this._ServerConnection.BaseURL = KV.Value.BaseURL;
                        this._ServerConnection.DatabaseServer = KV.Value.DatabaseServer;
                        this._ServerConnection.DatabaseServerPort = KV.Value.DatabaseServerPort;
                        this._ServerConnection.IsTrustedConnection = KV.Value.IsTrustedConnection;
                        this._ServerConnection.LinkedServer = KV.Value.LinkedServer;
                        if (!KV.Value.IsTrustedConnection)
                        {
                            this._ServerConnection.DatabaseUser = KV.Value.DatabaseUser;
                            this._ServerConnection.DatabasePassword = KV.Value.DatabasePassword;
                        }
                        if (int.TryParse(IdInUri, out ID))
                            this._UnitValues = this.UnitValues(ID);
                        else
                        {
                            if (IdInUri.IndexOf("/") > 1)
                            {
                                string Domain = IdInUri.Substring(0, IdInUri.IndexOf("/"));
                                if (int.TryParse(IdInUri.Substring(IdInUri.IndexOf("/")), out ID))
                                    this._UnitValues = this.UnitValues(Domain, ID);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
            if (this._UnitValues == null)
            {
                this._UnitValues = new Dictionary<string, string>();
            }
            
            return this._UnitValues;
        }

        #endregion

        #region Domains

        public virtual System.Collections.Generic.Dictionary<string, string> BackLinkValues(string Domain, string ConnectionString, string URI)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            return Values;
        }

        public virtual System.Collections.Generic.Dictionary<string, string> UnitValues(string Domain, int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            return Values;
        }

        public virtual DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns(string Domain)
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] A = new UserControls.QueryDisplayColumn[0];
            return A;
        }

        public virtual System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions(string Domain)
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> L = new List<QueryCondition>();
            return L;
        }

        #endregion

        #region Dependent

        //public virtual System.Collections.Generic.Dictionary<string, string> BackLinkValues(string Domain, string ConnectionString, string URI)
        //{
        //    System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
        //    return Values;
        //}

        //public virtual System.Collections.Generic.Dictionary<string, string> UnitValues(string Domain, int ID)
        //{
        //    System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
        //    return Values;
        //}

        public virtual bool DependentQueryAvailable(string URI)
        {
            return false;
        }


        public virtual DiversityWorkbench.UserControls.QueryDisplayColumn[] DependentQueryDisplayColumns(string URI)
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] A = new UserControls.QueryDisplayColumn[0];
            return A;
        }

        public virtual System.Collections.Generic.List<DiversityWorkbench.QueryCondition> DependentQueryConditions(string URI)
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> L = new List<QueryCondition>();
            return L;
        }

        #endregion

        #region SQL

        protected string SqlScalar(string SQL)
        {
            string Result = "";
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.Connection());
                Result = C.ExecuteScalar()?.ToString();
                this._Connection.Close();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Result;
        }

        protected System.Data.DataTable SqlTable(string SQL)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                ad.Fill(dt);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dt;
        }

        protected string IDsAsString(System.Collections.Generic.List<int> IDs)
        {
            string SQL = "";
            foreach (int ID in IDs)
            {
                if (SQL.Length > 0) SQL += ", ";
                SQL += ID.ToString();
            }
            return SQL;
        }



        private Microsoft.Data.SqlClient.SqlConnection _Connection;

        private Microsoft.Data.SqlClient.SqlConnection Connection()
        {
            if (this._Connection == null)
                this._Connection = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);
            if (this._Connection.State == System.Data.ConnectionState.Closed)
                this._Connection.Open();
            return this._Connection;
        }

        #endregion

        public virtual System.Collections.Generic.List<DiversityWorkbench.DatabaseService> DatabaseServices()
        {
            System.Collections.Generic.List<DiversityWorkbench.DatabaseService> ds = new List<DatabaseService>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV 
                     in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.ServiceName()].ServerConnectionList())
            {
                if (KV.Value.ConnectionIsValid)
                {
                    DiversityWorkbench.DatabaseService d = new DatabaseService(KV.Key);
                    d.IsWebservice = false;
                    if (KV.Value.DatabaseServer != DiversityWorkbench.Settings.DatabaseServer)
                    {
                        d.Server = KV.Value.DatabaseServer;
                    }
                    ds.Add(d);
                }
            }

            return ds;
        }

        /// <summary>
        /// A list of the accessible databases and services of a module based on the global list
        /// </summary>
        /// <returns>Dictionary listing the names (Key) and display text (Value) for the services</returns>
        public virtual System.Collections.Generic.Dictionary<string, string> AccessibleDatabasesAndServicesOfModule()
        {
            System.Collections.Generic.Dictionary<string, string> _Accessible = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV
                in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.ServiceName()].ServerConnectionList())
            {
                // Markus 31.10.22: Do not take lists within the cache DB into the list
                if (KV.Value.DisplayText.StartsWith("CacheDB: "))
                    continue;
                if (KV.Value.ConnectionIsValid && !_Accessible.ContainsKey(KV.Value.DisplayText))
                {
                    _Accessible.Add(KV.Value.DisplayText, KV.Value.DatabaseName.Replace("_", " - "));
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AdditionalServicesOfModule())
                _Accessible.Add(KV.Key, KV.Value);
            return _Accessible;
        }
        
        /// <summary>
        /// The list of webservices provided by the module
        /// </summary>
        /// <returns>Dictionary providing the List of Webservices</returns>
        public virtual System.Collections.Generic.Dictionary<string, string> AdditionalServicesOfModule()
        {
            System.Collections.Generic.Dictionary<string, string> _Add = new Dictionary<string, string>();
            return _Add;
        }
        protected virtual System.Collections.Generic.Dictionary<string, string> AdditionalServiceURIsOfModule()
        {
            System.Collections.Generic.Dictionary<string, string> _Add = new Dictionary<string, string>();
            return _Add;
        }

        public virtual System.Collections.Generic.Dictionary<string, string> DataBaseURIs()
        {
            System.Collections.Generic.Dictionary<string, string> _DataBaseURIs = new Dictionary<string, string>();
            // Markus 30.4.24: Prüfung auf null
            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList != null 
                && DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName())
                && DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[this.ServiceName()]._ServerConnectionList)
                {
                    if (KV.Value.BaseURL != null && KV.Value.BaseURL.Length > 0 && !_DataBaseURIs.ContainsKey(KV.Value.DisplayText))
                        _DataBaseURIs.Add(KV.Value.DisplayText, KV.Value.BaseURL);
                }
            }
            return _DataBaseURIs;
        }

        public virtual System.Collections.Generic.Dictionary<string, string> DatabaseAndServiceURIs()
        {
            System.Collections.Generic.Dictionary<string, string> _URIs = this.DataBaseURIs();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AdditionalServiceURIsOfModule())
            {
                if (!_URIs.ContainsKey(KV.Key))
                    _URIs.Add(KV.Key, KV.Value);
            }
            return _URIs;
        }

        //public virtual bool ShowMaxResultsForQuery() { return true; }

        #endregion

        #region Backlinks

        protected System.Windows.Forms.ImageList _BackLinkImages;

        protected virtual System.Windows.Forms.ImageList BackLinkImages()
        {
            if (this._BackLinkImages == null)
            {
                this._BackLinkImages = new System.Windows.Forms.ImageList();
                this._BackLinkImages.Images.Add("Database", DiversityWorkbench.ResourceWorkbench.Database);
                this._BackLinkImages.Images.Add("Link", DiversityWorkbench.Properties.Resources.Link);
            }
            return this._BackLinkImages;
        }

        public virtual System.Windows.Forms.ImageList BackLinkImages(ModuleType CallingModule)
        {
            if (this._BackLinkImages == null)
                this._BackLinkImages = new System.Windows.Forms.ImageList();
            return this._BackLinkImages;
        }

        protected System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> _BackLinkDomains;

        public virtual System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> BackLinkServerConnectionDomains(string URI, ModuleType CallingModule, bool IncludeEmpty = false, System.Collections.Generic.List<string> Restrictions = null)
        {
            if (this._BackLinkDomains == null)
                this._BackLinkDomains = new Dictionary<ServerConnection, List<BackLinkDomain>>();
            return this._BackLinkDomains;
        }

        protected System.Collections.Generic.Dictionary<ModuleType, System.Collections.Generic.Dictionary<string, BackLinkDomain>> _backLinkDomains;
        public virtual System.Collections.Generic.Dictionary<string, BackLinkDomain> BackLinkDomains(ModuleType CallingModule)
        {
            if (this._backLinkDomains == null)
            {
                this._backLinkDomains = new Dictionary<ModuleType, System.Collections.Generic.Dictionary<string, BackLinkDomain>>();
            }
            return this._backLinkDomains[CallingModule];
        }

        //public void BackLinkServerConnectionDomains_Reset()
        //{
        //    this._BackLinkDomains = new Dictionary<ServerConnection, List<BackLinkDomain>>();
        //    DiversityWorkbench.Settings.BacklinkUpdatedDatabases()
        //}

        private System.Collections.Generic.Dictionary<ModuleType, System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection>> _BackLinkDictionary;

        protected System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> BackLinkConnections(ModuleType BackLinkModule)
        {
            if (this._BackLinkDictionary == null)
                this._BackLinkDictionary = new Dictionary<ModuleType, Dictionary<string, ServerConnection>>();
            if (!this._BackLinkDictionary.ContainsKey(BackLinkModule))
            {
                string ModuleName = "Diversity" + BackLinkModule.ToString();
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> Dict = new Dictionary<string, ServerConnection>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[ModuleName].ServerConnections())
                {
                    if (KV.Value.ConnectionIsValid)
                        Dict.Add(KV.Value.DisplayText, KV.Value);
                }
                this._BackLinkDictionary.Add(BackLinkModule, Dict);
            }
            return this._BackLinkDictionary[BackLinkModule];
        }

        protected virtual System.Collections.Generic.List<BackLinkDomain> BackLinkDomains(ServerConnection SC, string URI, ModuleType CallingModule)
        {
            System.Collections.Generic.List<BackLinkDomain> List = new List<BackLinkDomain>();
            return List;
        }

        protected virtual string BacklinkUpdateRestriction(string TableAlias, string RestrictionView)
        {
            string SQL = " INNER JOIN " + RestrictionView + " AS RV ON RV." + this.QueryDisplayColumns("")[0].IdentityColumn + " = " + TableAlias + "." + this.QueryDisplayColumns("")[0].IdentityColumn;
            return SQL;
        }

        protected virtual string BacklinkUpdateRestriction(string TableAlias, string RestrictionView, string JoinTable, string ConnectionString)
        {
            string SQL = " INNER JOIN " + RestrictionView + " AS RV ON RV." + this.QueryDisplayColumns("")[0].IdentityColumn + " = " + TableAlias + "." + this.QueryDisplayColumns("")[0].IdentityColumn;
            return SQL;
        }

        protected virtual string BacklinkUpdateRestrictionView() { return ""; }

        public static void initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType Module, DiversityWorkbench.WorkbenchUnitUserInterface Interface, System.Windows.Forms.TabControl Tabcontrol, System.Windows.Forms.TabPage Page, DiversityWorkbench.UserControls.UserControlModuleBackLinks U, System.Windows.Forms.ToolStripMenuItem Menu, bool EnableCreate = false)
        {
            BackLinksInit(Module, Interface, Tabcontrol, Page, U, Menu, EnableCreate);
        }

        public static void BackLinksInit(DiversityWorkbench.WorkbenchUnit.ModuleType Module, DiversityWorkbench.WorkbenchUnitUserInterface Interface, System.Windows.Forms.TabControl Tabcontrol, System.Windows.Forms.TabPage Page, DiversityWorkbench.UserControls.UserControlModuleBackLinks U, System.Windows.Forms.ToolStripMenuItem Menu, bool EnableCreate = false)
        {
            try
            {
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(Module))
                {
                    if (!Tabcontrol.TabPages.Contains(Page))
                        Tabcontrol.TabPages.Add(Page);
                    U.SetModuleParameter(Module, Interface, EnableCreate);
                    Menu.Checked = true;
                }
                else
                {
                    if (Tabcontrol.TabPages.Contains(Page))
                        Tabcontrol.TabPages.Remove(Page);
                    Menu.Checked = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static string _BacklinkBaseURL;
        private static System.Collections.Generic.List<int> _BacklinkIDs;

        public static bool BacklinkSetBaseUri(string URI)
        {
            if (_BacklinkIDs != null && _BacklinkIDs.Count > 0)
                return false;
            _BacklinkBaseURL = URI;
            return true;
        }

        public static void BacklinkAddID(int ID)
        {
            if (_BacklinkIDs == null) _BacklinkIDs = new List<int>();
            if (!_BacklinkIDs.Contains(ID)) _BacklinkIDs.Add(ID);
        }

        public static System.Collections.Generic.List<int> BacklinkIDs { get { return _BacklinkIDs; } }

        public static bool BacklinkReset()
        {
            if (_BacklinkIDs != null && _BacklinkIDs.Count > 0)
                return false;
            _BacklinkIDs = null;
            return true;
        }

        public static void BacklinkClear()
        {
            _BacklinkIDs = new List<int>();
        }

        public static bool BacklinkUpdate(ref string Message, string Module = "")
        {
            if (Module.Length == 0) Module = DiversityWorkbench.Settings.ServerConnection.ModuleName;
            int iFailed = 0;
            int iSuccess = 0;
            int iTargets = 0;
            try
            {
                //if(DiversityWorkbench.Settings.BacklinkUpdatedDatabases().Count == 0)
                //{
                //    Message = "No database for forwarding changes have been selected";
                //    return false;
                //}
                if (DiversityWorkbench.Settings.BacklinkUpdatedDatabases().ContainsKey(DiversityWorkbench.Settings.ServerConnection.DatabaseKey))
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> KV in DiversityWorkbench.Settings.BacklinkUpdatedDatabases()[DiversityWorkbench.Settings.ServerConnection.DatabaseKey])
                    {
                        System.Collections.Generic.Dictionary<string, ServerConnection> ServerConnections = new Dictionary<string, ServerConnection>();
                        string LinkedModule = KV.Key;
                        ModuleType moduleType = ModuleType.None;
                        switch (Module)
                        {
                            case "DiversityAgents":
                                moduleType = ModuleType.Agents;
                                break;
                            case "DiversityCollection":
                                moduleType = ModuleType.Collection;
                                break;
                            case "DiversityDescriptions":
                                moduleType = ModuleType.Descriptions;
                                break;
                            case "DiversityExsiccatae":
                                moduleType = ModuleType.Exsiccatae;
                                break;
                            case "DiversityGazetteer":
                                moduleType = ModuleType.Gazetteer;
                                break;
                            case "DiversityProjects":
                                moduleType = ModuleType.Projects;
                                break;
                            case "DiversityReferences":
                                moduleType = ModuleType.References;
                                break;
                            case "DiversitySamplingPlots":
                                moduleType = ModuleType.SamplingPlots;
                                break;
                            case "DiversityScientificTerms":
                                moduleType = ModuleType.ScientificTerms;
                                break;
                            case "DiversityTaxonNames":
                                moduleType = ModuleType.TaxonNames; // DTN updating depending datasets
                                break;
                        }
                        foreach (string DB in KV.Value)
                        {
                            if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[LinkedModule].ServerConnectionList().ContainsKey(DB))
                            {
                                iTargets++;
                                DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[LinkedModule].ServerConnectionList()[DB];
                                //DiversityWorkbench.CollectionSpecimen collectionSpecimen = new CollectionSpecimen(SC);
                                if (_BacklinkIDs != null)
                                {
                                    DiversityWorkbench.WorkbenchUnit workbenchUnitTarget = new WorkbenchUnit(DiversityWorkbench.Settings.ServerConnection);
                                    switch (SC.ModuleName)
                                    {
                                        case "DiversityCollection":
                                            workbenchUnitTarget = new CollectionSpecimen(SC);
                                            break;
                                    }
                                    DiversityWorkbench.WorkbenchUnit workbenchUnitSource = new WorkbenchUnit(DiversityWorkbench.Settings.ServerConnection);
                                    switch (moduleType)
                                    {
                                        case ModuleType.TaxonNames:
                                            workbenchUnitSource = new TaxonName(DiversityWorkbench.Settings.ServerConnection);
                                            break;
                                        case ModuleType.Agents:
                                            workbenchUnitSource = new Agent(DiversityWorkbench.Settings.ServerConnection);
                                            break;
                                        default:
#if DEBUG
                                            System.Windows.Forms.MessageBox.Show(moduleType.ToString() + " fehlt noch!!!");
#endif
                                            break;
                                    }

                                    foreach (int ID in _BacklinkIDs)
                                    {
                                        System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> _BackLinkServerConnectionDomains =
                                            workbenchUnitTarget.BackLinkServerConnectionDomains(DiversityWorkbench.Settings.ServerConnection.BaseURL + ID.ToString(), moduleType);
                                        foreach (System.Collections.Generic.KeyValuePair<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> backLink in _BackLinkServerConnectionDomains)
                                        {
                                            System.Collections.Generic.Dictionary<string, string> Values = workbenchUnitSource.UnitValues(ID);
                                            foreach (BackLinkDomain BLD in backLink.Value)
                                            {
                                                // Checking the permissions
                                                Microsoft.Data.SqlClient.SqlConnection conPermit = new Microsoft.Data.SqlClient.SqlConnection(SC.ConnectionString);
                                                conPermit.Open();
                                                bool HasPermission = DiversityWorkbench.Forms.FormFunctions.Permissions(BLD.TableName, "UPDATE", conPermit);
                                                conPermit.Close();
                                                conPermit.Dispose();
                                                if (!HasPermission) continue;

                                                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> ColumnTables = BLD.BackLinkColumnTables();
                                                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> ct in ColumnTables)
                                                {
                                                    bool ValueDetected = false;
                                                    string Restriction = "";
                                                    string SQL = "";
                                                    for (int i = 0; i < ct.Value.Count; i++)
                                                    {
                                                        string BacklinkParameter = "";
                                                        foreach (BackLinkColumn linkColumn in BLD.BackLinkColumns)
                                                        {
                                                            if (linkColumn.ColumnName == ct.Value[i]) { BacklinkParameter = linkColumn.BacklinkParameter; break; }
                                                        }
                                                        if (Values.ContainsKey(BacklinkParameter))
                                                        {
                                                            ValueDetected = true;
                                                            if (Restriction.Length > 0) Restriction += " OR ";
                                                            Restriction += ct.Value[i] + " <> '" + Values[BacklinkParameter] + "' ";
                                                            if (SQL.Length > 0) SQL += ", ";
                                                            SQL += ct.Value[i] + " = '" + Values[BacklinkParameter] + "' ";
                                                        }
                                                        //SQL += ct.Value[i] + " = '";
                                                    }
                                                    if (ValueDetected)
                                                    {
                                                        SQL += " FROM [" + ct.Key + "] AS T ";
                                                        if (ct.Key != BLD.TableName)
                                                        {
                                                            DiversityWorkbench.Data.Table t = new Data.Table(ct.Key, "dbo", SC.ConnectionString, SC.ModuleName);
                                                            string Join = t.JoinClause("TT", "T", BLD.TableName);
                                                            SQL += " INNER JOIN [" + BLD.TableName + "] AS TT ON ";
                                                            SQL += t.JoinClause("TT", "T", BLD.TableName);
                                                        }
                                                        SQL += workbenchUnitTarget.BacklinkUpdateRestriction("T", workbenchUnitTarget.BacklinkUpdateRestrictionView(), BLD.TableName, SC.ConnectionString);
                                                        SQL += " WHERE ";
                                                        if (ct.Key != BLD.TableName) SQL += "TT";
                                                        else SQL += "T";
                                                        SQL += "." + BLD.ColumnName + " = '" + Values["_URI"] + "' " + " AND (" + Restriction + ")";
                                                        SQL = "UPDATE T SET " + SQL;
                                                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, SC.ConnectionString)) iSuccess++;
                                                        else
                                                        {
                                                            iFailed++;
                                                            Message += "\r\nfailed:\r\n" + SQL;
                                                        }
                                                    }
                                                }
                                            }
                                            //switch (moduleType)
                                            //{
                                            //    case ModuleType.TaxonNames:
                                            //        DiversityWorkbench.TaxonName TN = new TaxonName(DiversityWorkbench.Settings.ServerConnection);
                                            //        System.Collections.Generic.Dictionary<string, string> Values = TN.UnitValues(ID);
                                            //        break;
                                            //}
                                            //DiversityWorkbench.BackLinkDomain backLinkDomain = new BackLinkDomain()
                                        }
                                    }
                                }
                            }
                        }

                        //switch (KV.Key)
                        //{
                        //    case "DiversityCollection":
                        //        foreach (string DB in KV.Value)
                        //        {
                        //            if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityCollection"].ServerConnectionList().ContainsKey(DB))
                        //            {
                        //                DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityCollection"].ServerConnectionList()[DB];
                        //                DiversityWorkbench.CollectionSpecimen collectionSpecimen = new CollectionSpecimen(SC);
                        //                if (_BacklinkIDs != null)
                        //                {
                        //                    foreach (int ID in _BacklinkIDs)
                        //                    {
                        //                        System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> _BackLinkServerConnectionDomains =
                        //                            collectionSpecimen.BackLinkServerConnectionDomains(DiversityWorkbench.Settings.ServerConnection.BaseURL + ID.ToString(), ModuleType.TaxonNames);
                        //                        foreach(System.Collections.Generic.KeyValuePair<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> backLink in _BackLinkServerConnectionDomains)
                        //                        {
                        //                            //backLink.Value.b
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }
                        //        break;
                        //}
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); return false; }
            if (iSuccess > 0) Message += "\r\n" + iSuccess.ToString() + " Datasets updated";
            if (iFailed > 0) Message += "\r\n" + iFailed.ToString() + " Updates failed";
            if (iTargets == 0) Message += "\r\n\r\nNOTHING TO UPDATE!\r\nNo target databases have been selected";
            if (iSuccess == 0 && iFailed == 0 && iTargets == 0) return false;
            return true;
        }

        private static System.Collections.Generic.List<string> BacklinkUpdateAdditionalTableList(System.Collections.Generic.List<BackLinkDomain> backLink)
        {
            System.Collections.Generic.List<string> Tables = new List<string>();
            //foreach(BackLinkDomain c in backLink.B)
            return Tables;
        }

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, ServerConnection>> _BacklinkUpdate_ServerConnections;
        public static System.Collections.Generic.Dictionary<string, ServerConnection> BacklinkUpdate_ServerConnections(string Module)
        {
            if (_BacklinkUpdate_ServerConnections == null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                {
                    string ModuleName = KV.Key;
                    System.Collections.Generic.Dictionary<string, ServerConnection> dict = new Dictionary<string, ServerConnection>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in KV.Value.ServerConnectionList())
                    {
                        //if (DiversityWorkbench.Settings.)
                        dict.Add(KVconn.Key, KVconn.Value);
                    }
                    _BacklinkUpdate_ServerConnections.Add(ModuleName, dict);
                }
            }
            if (_BacklinkUpdate_ServerConnections.ContainsKey(Module))
                return _BacklinkUpdate_ServerConnections[Module];
            else
            {
                System.Collections.Generic.Dictionary<string, ServerConnection> dict = new Dictionary<string, ServerConnection>();
                return dict;
            }
            return null;
        }

        private static System.Collections.Generic.Dictionary<string, ServerConnection> _BackLinkServerConnections(string Module)
        {
            System.Collections.Generic.Dictionary<string, ServerConnection> SCs = new Dictionary<string, ServerConnection>();
            return SCs;
        }

        public static int BacklinkCount()
        {
            if (_BacklinkIDs == null) return 0;
            else return _BacklinkIDs.Count;
        }

        #endregion

        #region Auxilliary functions

        /// <summary>
        /// fills the data based on a query into a data dictionary
        /// </summary>
        /// <param name="SQL">the query for getting the data</param>
        /// <param name="UnitValues">the data dictionary that should be filled with the data</param>
        /// <param name="CondSemi">true if separator semicolon shall be used for comma in data</param>
        protected bool getDataFromTable(string SQL, ref System.Collections.Generic.Dictionary<string, string> UnitValues, bool CondSemi = false)
        {
            bool OK = true;
            if (this.ServerConnection.ConnectionString.Length > 0)
            {
                System.Data.DataTable dtData = new System.Data.DataTable();
                try
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                    ad.Fill(dtData);
                    if (dtData.Rows.Count > 0)
                    {
                        foreach (System.Data.DataColumn C in dtData.Columns)
                        {
                            string Separator = ", ";
                            string Value = "";
                            if (CondSemi)
                            {
                                // Check if data contin a comma and use semicolon if requested
                                foreach (System.Data.DataRow R in dtData.Rows)
                                {
                                    if (R[C.ColumnName].ToString().Contains(","))
                                    {
                                        Separator = "; ";
                                        break;
                                    }
                                }
                            }
                            foreach (System.Data.DataRow R in dtData.Rows)
                            {
                                string colVal = R[C.ColumnName].ToString().Replace("<br />", "\r\n");
                                if (!colVal.EndsWith("\r\n") && !Value.EndsWith(" | "))
                                {
                                    if (Value.Length > 0) Value += Separator;
                                }
                                Value += colVal;
                            }
                            if (UnitValues.ContainsKey(C.ColumnName))
                            {
                                UnitValues[C.ColumnName] = Value;
                            }
                            else
                            {
                                if (!UnitValues.ContainsKey(C.ColumnName))
                                    UnitValues.Add(C.ColumnName, Value);
                                else { }
                            }
                        }
                    }
                    else
                    {
                        OK = false;
                        foreach (System.Data.DataColumn C in dtData.Columns)
                        {
                            if (UnitValues.ContainsKey(C.ColumnName))
                            { }
                            else
                                UnitValues.Add(C.ColumnName, "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    OK = false;
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                }
            }
            return OK;
        }

        protected bool getResources(string SQL, ref System.Collections.Generic.Dictionary<string, string> UnitResources)
        {
            bool OK = true;
            try
            {
                System.Data.DataTable dtData = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                ad.Fill(dtData);
                if (dtData.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in dtData.Rows)
                    {
                        UnitResources.Add(R[0].ToString(), R[1].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                OK = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return OK;
        }

        protected string getDatabase(string QueryColumn, string QueryValue, string Table, string[] DatabaseList)
        {
            string DB = DatabaseList[0];
            string SQL = "SELECT COUNT(*) FROM " + Table + " WHERE " + QueryColumn + " = " + QueryValue;
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection();
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL);
                for (int i = 0; i < DatabaseList.Length; i++)
                {
                    this._ServerConnection.DatabaseName = DatabaseList[i];
                    con.ConnectionString = this._ServerConnection.ConnectionString;
                    C.Connection = con;
                    con.Open();
                    int Count = int.Parse(C.ExecuteScalar()?.ToString());
                    con.Close();
                    if (Count > 0)
                    {
                        DB = DatabaseList[i];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return DB;
        }

        protected void GetData(ref System.Data.DataTable DT, string SQL)
        {
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                try { a.Fill(DT); }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL); }
            }
        }

        #endregion

        #region Global list of workbench units

        private static System.Collections.Generic.Dictionary<string, DiversityWorkbench.WorkbenchUnit> _GlobalWorkbenchUnitList;

        public static System.Collections.Generic.Dictionary<string, DiversityWorkbench.WorkbenchUnit> GlobalWorkbenchUnitList()
        {
            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList == null)
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList = new Dictionary<string, WorkbenchUnit>();
            return DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList;
        }

        public static System.Collections.Generic.Dictionary<string, DiversityWorkbench.WorkbenchUnit> GlobalWorkbenchUnitList(string ModuleName)
        {
            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList == null)
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList = new Dictionary<string, WorkbenchUnit>();
            if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(ModuleName))
            {

            }
            return DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList;
        }

        private static bool compareUriStart(string baseUri, string kvValue)
        {
            // Parse the URIs using the Uri class
            Uri uriToCompare;
            Uri kvUri;
            if (Uri.TryCreate(baseUri, UriKind.Absolute, out uriToCompare) &&
                Uri.TryCreate(kvValue, UriKind.Absolute, out kvUri))
            {
                // Compare the host and path, ignoring the scheme (http/https)
                if (uriToCompare.Host.Equals(kvUri.Host, StringComparison.OrdinalIgnoreCase) &&
                    uriToCompare.AbsolutePath.StartsWith(kvUri.AbsolutePath, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            } 
            return false;
        }
        
        public static bool IsServiceAvailable(string ModuleName, string BaseURI)
        {
            string Base = DiversityWorkbench.WorkbenchUnit.getBaseURIfromURI(BaseURI);
            if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(ModuleName) || string.IsNullOrEmpty(Base))
                return false;
            else
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ModuleName].DatabaseAndServiceURIs())
                {
                    if (!string.IsNullOrEmpty(KV.Value))
                    {
                        if (compareUriStart(Base, KV.Value))
                            return true;
                        //if (Base.StartsWith(KV.Value))
                        //    return true;
                    }
                }
                if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ModuleName].DatabaseAndServiceURIs().ContainsValue(Base))
                    return false;
                //if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ModuleName]._DataBaseURIs.ContainsValue(BaseURI)) return false;
                else
                    return true;
            }
        }

        public static bool IsAnyServiceAvailable(string ModuleName)
        {
            try
            {
                if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(ModuleName)) return false;
                if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ModuleName].IsAnyServiceAvailable()) return true;
                else return false;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return false;
        }

        protected void AddUnitToGlobalList()
        {
            // do not add connections where module name does not match the Database name
            if (!this.ServerConnection.DatabaseName.StartsWith(this.ServerConnection.ModuleName))
                return;
            // add the service to the global list
            if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(this.ServiceName()))
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.Add(this.ServiceName(), this);
        }

        public static DiversityWorkbench.WorkbenchUnit getWorkbenchUnit(string Name)
        {
            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(Name))
                return DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[Name];
            else return null;
        }

        public static void RequeryWorkbenchUnitConnections()
        {
            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList == null
                || DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.Count == 0)
                return;
            // remove all connections of the current Unit
            if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey(DiversityWorkbench.Settings.ModuleName))
                DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[DiversityWorkbench.Settings.ModuleName]._ServerConnectionList.Clear();
            // add the current connection
            DiversityWorkbench.ServerConnection S = DiversityWorkbench.Settings.ServerConnection;
            if (S.ConnectionIsValid)
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[DiversityWorkbench.Settings.ModuleName]._ServerConnectionList.Add(S.DatabaseName, S);

            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList == null)
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList = new Dictionary<string, WorkbenchUnit>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList)
            {
                System.Collections.Generic.List<string> L = KV.Value.DatabaseList();
                if (KV.Key == DiversityWorkbench.Settings.ModuleName)
                {
                    System.Collections.Generic.List<string> ToDelete = new List<string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVlist in KV.Value._ServerConnectionList)
                    {
                        if (KVlist.Key != DiversityWorkbench.Settings.DatabaseName)
                            ToDelete.Add(KVlist.Key);
                    }
                    if (ToDelete.Count > 0)
                    {
                        foreach (string s in ToDelete)
                            KV.Value._ServerConnectionList.Remove(s);
                    }
                }
                else
                {
                    KV.Value._DatabaseList = null;
                    System.Collections.Generic.List<string> DBlist = KV.Value.DatabaseList();
                    foreach (string s in KV.Value._DatabaseList)
                    {
                        if (!KV.Value._ServerConnectionList.ContainsKey(s))
                        {
                            DiversityWorkbench.ServerConnection SC = new ServerConnection();
                            SC.DatabaseName = s;
                            SC.ModuleName = KV.Key;
                            SC.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
                            SC.DatabasePassword = DiversityWorkbench.Settings.Password;
                            SC.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
                            SC.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
                            SC.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                            DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitListAddDatabase(KV.Key, s, SC);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removing connections pointing to a previous server
        /// </summary>
        public static void RemoveMistakenWorkbenchUnitConnections()
        {
            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList == null
                || DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.Count == 0)
                return;

            try
            {
                // get the server
                string Server = DiversityWorkbench.Settings.DatabaseServer;

                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList)
                {
                    if (KV.Value._DatabaseList != null) // Markus 6.8.24: Prüfung ob vorhanden
                    {
                        foreach (string s in KV.Value._DatabaseList)
                        {
                            if (DiversityWorkbench.Settings.AddedRemoteConnections != null &&
                                DiversityWorkbench.Settings.AddedRemoteConnections.Contains(s))
                                continue;
                            if (KV.Value._ServerConnectionList.ContainsKey(s))
                            {
                                DiversityWorkbench.ServerConnection SC = KV.Value._ServerConnectionList[s];
                                if (SC.DatabaseServer != Server)
                                {
                                    DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[SC.ModuleName]._ServerConnectionList.Remove(s);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        public static void LoadAddedRemoteConnections()
        {
            try
            {
                if (DiversityWorkbench.Settings.AddedRemoteConnections == null)
                    return;
                foreach (string s in DiversityWorkbench.Settings.AddedRemoteConnections)
                {
                    if (DiversityWorkbench.Settings.TimeoutDatabase == 0)
                        break;

                    DiversityWorkbench.ServerConnection S = new ServerConnection(s + ";Connection Timeout=" + DiversityWorkbench.Settings.TimeoutDatabase.ToString() + ";");
                    S.IsAddedRemoteConnection = true;
                    if (!DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey(S.ModuleName))
                    {
                        DiversityWorkbench.WorkbenchUnit W = new WorkbenchUnit(S, true);
                    }
                    else
                    {
                        if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[S.ModuleName]._ServerConnectionList == null)
                        {
                            DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[S.ModuleName]._ServerConnectionList = new Dictionary<string, ServerConnection>();
                        }
                        //if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[S.ModuleName]._ServerConnectionList.ContainsKey(S.DatabaseName))
                        //{
                        //    if (S.DatabaseServer == DiversityWorkbench.Settings.DatabaseServer)
                        //        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[S.ModuleName]._ServerConnectionList.Add(S.DatabaseName, S);
                        //    else
                        //        DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[S.ModuleName]._ServerConnectionList.Add(S.DisplayText, S);
                        //}
                        if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[S.ModuleName]._ServerConnectionList.ContainsKey(S.DisplayText))
                        {
                            DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[S.ModuleName]._ServerConnectionList.Add(S.DisplayText, S);
                        }
                        else if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[S.ModuleName]._ServerConnectionList.ContainsKey(S.DatabaseName)
                            && S.DatabaseServer != DiversityWorkbench.Settings.DatabaseServer)
                        {
                            DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[S.ModuleName]._ServerConnectionList.Add(S.DisplayText, S);
                        }
                        else
                        {
                            //DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[S.ModuleName]._ServerConnectionList.Remove(S.DatabaseName);
                            //DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[S.ModuleName]._ServerConnectionList.Add(S.DatabaseName, S);
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        public static void ResetWorkbenchUnitConnections()
        {
            DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList = new Dictionary<string, WorkbenchUnit>();
        }

        public static string getServiceNameFromURI(string URI)
        {
            string Service = "";
            DiversityWorkbench.WorkbenchUnit.CheckModulePresence(URI);
            // Markus 30.4.24: Prüfung auf Existenz
            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList != null) 
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList)
                {
                    try
                    {
                        Service = DiversityWorkbench.WorkbenchUnit.getServiceNameFromURI(URI, KV.Key);
                        if (Service.Length > 0)
                        {
                            if (Service != KV.Key) Service = KV.Key;
                            break;
                        }
                    }
                    catch (System.Exception ex) { }
                }
            }
            return Service;
        }

        private static void CheckModulePresence(string URI)
        {
            try
            {
                if (string.IsNullOrEmpty(URI))
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("WorkbenchUnit.cs 2344: Module not found: URI is empty or null");
#endif
                    return;
                }
                if (URI.ToLower().IndexOf("agents") > -1)
                {
                    DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                }
                else if (URI.ToLower().IndexOf("collection") > -1)
                {
                    DiversityWorkbench.CollectionSpecimen C = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                }
                else if (URI.ToLower().IndexOf("description") > -1 && !DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey("DiversityDescriptions"))
                {
                    DiversityWorkbench.Description D = new DiversityWorkbench.Description(DiversityWorkbench.Settings.ServerConnection);
                }
                else if (URI.ToLower().IndexOf("exsiccat") > -1 && !DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey("DiversityExsiccatae"))
                {
                    DiversityWorkbench.Exsiccate E = new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                }
                else if (URI.ToLower().IndexOf("gazetteer") > -1 && !DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey("DiversityGazetteer"))
                {
                    DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                }
                else if (URI.ToLower().IndexOf("projects") > -1 && !DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey("DiversityProjects"))
                {
                    DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                }
                else if (URI.ToLower().IndexOf("references") > -1 && !DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey("DiversityReferences"))
                {
                    DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                }
                else if (URI.ToLower().IndexOf("samplingplots") > -1 && !DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey("DiversitySamplingPlots"))
                {
                    DiversityWorkbench.SamplingPlot R = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                }
                else if (URI.ToLower().IndexOf("scientificterms") > -1 && !DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey("DiversityScientificTerms"))
                {
                    DiversityWorkbench.ScientificTerm R = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                }
                else if (URI.ToLower().IndexOf("taxonnames") > -1 && !DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey("DiversityTaxonNames"))
                {
                    DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, URI);
            }
        }

        public static string getDatabaseNameFromURI(string URI)
        {
            string Database = "";
            DiversityWorkbench.WorkbenchUnit.CheckModulePresence(URI);
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList)
            {
                Database = DiversityWorkbench.WorkbenchUnit.getServiceNameFromURI(URI, KV.Key);
                if (Database.Length == 0)
                    Database = DiversityWorkbench.WorkbenchUnit.getDatabaseNameFromURI(URI, KV.Key);
                if (Database.Length > 0)
                    break;
            }
            if (Database.Length == 0)
            {
                try
                {
                    string[] UriParts = URI.Split(new char[] { '/' });
                    if (UriParts.Length > 1) // Markus 8.8.23: Bugfix
                    {
                        string DB = UriParts[UriParts.Length - 2];
                        string SQL = "SELECT TOP 1 name FROM master.dbo.sysdatabases WHERE name like '%" + DB + "%'";
                        string Message = "";
                        DB = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                        if (DB.Length > 0)
                        {
                            SQL = "use " + DB + "; SELECT dbo.BaseURL();";
                            try
                            {
                                string BaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                                if (BaseURL.Length > 0 && URI.StartsWith(BaseURL))
                                    Database = DB;
                            }
                            catch(Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, URI);
                }
            }
            return Database;
        }

        public static string getDatabaseNameFromURI(string URI, string ServiceName)
        {
            string Database = "";
            if (string.IsNullOrEmpty(URI))
            {
                return Database;
            }
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName].DatabaseAndServiceURIs())
            {
               //if (URI.StartsWith(KV.Value))
                if (compareUriStart(URI, KV.Value))
                {
                    Database = KV.Key;
                    break;
                }
            }
            return Database;
        }
        // TODO Ariane neue Methoden nach Umstellen siehe WorkbenchUnit: TODO Ariane
        //public static string GetLastPartOfUri(string uri)
        //{
        //    if (string.IsNullOrWhiteSpace(uri))
        //        throw new ArgumentException("The URI cannot be null or empty", nameof(uri));

        //    try
        //    {
        //        var uriObject = new System.Uri(uri);
        //        var segments = uriObject.Segments;
        //        return segments[^1].TrimEnd('/'); // Get the last segment and trim any trailing slashes
        //    }
        //    catch (UriFormatException ex)
        //    {
        //        throw new ArgumentException("The provided string is not a valid URI.", ex);
        //    }

        //}

        //public static string getIDFromURIAndDatabase(string uri, string databaseName)
        //{
        //    if (string.IsNullOrWhiteSpace(uri))
        //    {
        //        throw new ArgumentException("The URI cannot be null or empty.", nameof(uri));
        //    }
        //    if (string.IsNullOrWhiteSpace(databaseName))
        //    {
        //        throw new ArgumentException("The database name cannot be null or empty.", nameof(databaseName));
        //    }
        //    try
        //    {
        //        // Check if the URI contains the database name
        //        if (uri.Contains(databaseName, StringComparison.OrdinalIgnoreCase))
        //        {
        //            // Extract and return the last part of the URI
        //            return GetLastPartOfUri(uri);
        //        }
        //        // If the database name is not found in the URI, return an empty string or handle as needed
        //        return string.Empty;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw new InvalidOperationException("An error occurred while processing the URI.", ex);
        //    }
        //}
        
        public static string getIDFromURI(string URI, bool IncludeSplittingOption = false)
        {
            string ID = "";
            if (URI.Length > 0)
            {
                try
                {
                    string ServiceName = DiversityWorkbench.WorkbenchUnit.getServiceNameFromURI(URI);
                    if (ServiceName.Length > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV
                            in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[ServiceName].DatabaseAndServiceURIs())
                        {
                            //  if (URI.ToLower().StartsWith(KV.Value.ToLower()))
                            if (compareUriStart(URI, KV.Value))
                            {
                                ID = URI.Substring(KV.Value.Length);
                                break;
                            }
                        }
                        if (ID.Length == 0 && URI.Length > 0 && IncludeSplittingOption)
                        {
                            string[] UriParts = URI.Split(new char[] { '/' });
                            ID = UriParts[UriParts.Length - 1];
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, URI);
                }
            }
            return ID;
        }

        public static string getServiceNameFromURI(string URI, string ServiceName)
        {
            //Markus 28.3.23: Bugfix for missing ServiceName
            string Service = "";
            if (URI == null)
                return "";
            try
            {
                if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(ServiceName))
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName]._ServerConnectionList)
                    {
                        if (KV.Value.BaseURL != null && URI.ToLower().StartsWith(KV.Value.BaseURL.ToLower()))
                        {
                            Service = KV.Value.DatabaseName;
                            break;
                        }
                    }
                }

                // alter Code
                if (Service.Length == 0 && DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(ServiceName))
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName].DatabaseAndServiceURIs())
                    {
                        if (URI == null) continue;
                        if (compareUriStart(URI, KV.Value))
                        {
                            Service = KV.Key;
                            break;
                        }
                        //if (URI.ToLower().StartsWith(KV.Value.ToLower()))
                        //{
                        //    Service = KV.Key;
                        //    break;
                        //}
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, URI); }
            return Service;
        }

        public static string getBaseURIfromURI(string URI)
        {
            string BaseURI = "";
            string Service = DiversityWorkbench.WorkbenchUnit.getServiceNameFromURI(URI);
            // Markus 30.4.24: Prüfung auf Existenz
            if (Service.Length > 0
                && DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey(Service))
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV
                    in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[Service].DatabaseAndServiceURIs())
                {
                    if (URI == null) continue;
                    if (compareUriStart(URI, KV.Value))
                    {
                        BaseURI = KV.Value;
                        break;
                    }
                }
            }
            // Markus 12.6.24: Checking if BaseURL != null in case no DP database is accessible
            // Markus 17.5.23: Getting BaseURI from current connection if fitting to URI
            if (BaseURI.Length == 0 
                && DiversityWorkbench.Settings.ServerConnection.BaseURL != null 
                && URI.ToLower().StartsWith(DiversityWorkbench.Settings.ServerConnection.BaseURL.ToLower()))
            {
                BaseURI = DiversityWorkbench.Settings.ServerConnection.BaseURL;
            }
            return BaseURI;
        }

        public static DiversityWorkbench.WorkbenchUnit.ServiceType getServiceType(string URI)
        {
            string Database = DiversityWorkbench.WorkbenchUnit.getDatabaseNameFromURI(URI);
            string Service = DiversityWorkbench.WorkbenchUnit.getServiceNameFromURI(URI);
            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(Service))
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[Service].ServerConnectionList())
                {
                    if (KV.Value.DatabaseName == Database)
                        return ServiceType.WorkbenchModule;
                }

                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.WorkbenchUnit
                             ._GlobalWorkbenchUnitList[Service].AdditionalServicesOfModule())
                {
                    if (KV.Key == Database)
                        return ServiceType.WebService;
                }
            }
            return ServiceType.Unknown;
        }

        public static DiversityWorkbench.ServerConnection getServerConnectionFromURI(string URI)
        {            

            string Database = DiversityWorkbench.WorkbenchUnit.getDatabaseNameFromURI(URI);
            string Service = DiversityWorkbench.WorkbenchUnit.getServiceNameFromURI(URI);
            if (DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList.ContainsKey(Service))
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[Service].ServerConnectionList())
                {
                    if (KV.Value.BaseURL != null && URI.ToLower().StartsWith(KV.Value.BaseURL.ToLower())
                        && Database == KV.Value.DatabaseName)
                        return KV.Value;
                }
            }
            return null;
        }

        public static void GlobalWorkbenchUnitListAddDatabase(string ServiceName, string Database, DiversityWorkbench.ServerConnection ServerConnection)
        {
            try
            {
                if (!DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName].ServerConnectionList().ContainsKey(Database))
                {
                    DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName].ServerConnectionList().Add(Database, ServerConnection);
                }
                else
                {
                    DiversityWorkbench.ServerConnection S = new ServerConnection();
                    if (ServerConnection.ConnectionString.Length > 0
                        && S.DatabaseName == ServerConnection.DatabaseName)
                    {
                        S.DatabaseName = ServerConnection.DatabaseName;
                        S.DatabasePassword = ServerConnection.DatabasePassword;
                        S.DatabaseServer = ServerConnection.DatabaseServer;
                        S.DatabaseServerPort = ServerConnection.DatabaseServerPort;
                        S.DatabaseUser = ServerConnection.DatabaseUser;
                        S.IsLocalExpressDatabase = ServerConnection.IsLocalExpressDatabase;
                        S.IsTrustedConnection = ServerConnection.IsTrustedConnection;
                        if (S.IsLocalExpressDatabase)
                            S.SqlExpressDbFileName = ServerConnection.SqlExpressDbFileName;
                    }
                }
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName]._ServerConnection.DatabaseName = ServerConnection.DatabaseName;
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName]._ServerConnection.DatabasePassword = ServerConnection.DatabasePassword;
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName]._ServerConnection.DatabaseServer = ServerConnection.DatabaseServer;
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName]._ServerConnection.DatabaseServerPort = ServerConnection.DatabaseServerPort;
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName]._ServerConnection.DatabaseUser = ServerConnection.DatabaseUser;
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName]._ServerConnection.IsTrustedConnection = ServerConnection.IsTrustedConnection;
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName]._ServerConnection.IsLocalExpressDatabase = ServerConnection.IsLocalExpressDatabase;
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName]._ServerConnection.ModuleName = ServerConnection.ModuleName;
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName]._ServerConnection.SqlExpressDbFileName = ServerConnection.SqlExpressDbFileName;
                DiversityWorkbench.WorkbenchUnit._GlobalWorkbenchUnitList[ServiceName]._ServerConnection.DisplayText = ServerConnection.DisplayText;
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Settings for datasources

        public static bool FixedSourceSetParameters(ref DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit
            , ref DiversityWorkbench.ServerConnection ServerConnection
            , ref DwbServiceEnums.DwbService Webservice
            // , ref System.Collections.Generic.List<DiversityWorkbench.WebserviceQueryOption> WebserviceOptions
            , ref System.Windows.Forms.Button buttonFixSource
            , ref System.Windows.Forms.ComboBox comboBoxSourceValues
            , System.Windows.Forms.ToolTip toolTip)
        {
            bool SourceFound = false;
            bool SearchCanceled = false;
            string SQL = "";
            try
            {
                System.Collections.Generic.List<string> Sources = new List<string>();
                string Source = "";

                if (IWorkbenchUnit != null)
                {
                    if (IWorkbenchUnit.ServerConnections() != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> SC in IWorkbenchUnit.ServerConnections())
                        {
                            if (!Sources.Contains(SC.Key))
                            {
                                Sources.Add(SC.Key);
                            }
                        }
                    }
                    if (IWorkbenchUnit.AccessibleDatabasesAndServicesOfModule() != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in IWorkbenchUnit.AccessibleDatabasesAndServicesOfModule())
                        {
                            if (!Sources.Contains(KV.Key))
                            {
                                Sources.Add(KV.Key);
                            }
                        }
                    }
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(Sources, "Source", "Please select a source from the list", true);
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        if (IWorkbenchUnit.ServerConnections() != null && IWorkbenchUnit.ServerConnections().ContainsKey(f.SelectedString))
                        {
                            ServerConnection = IWorkbenchUnit.ServerConnections()[f.SelectedString];
                            Source = ServerConnection.DisplayText;
                            Webservice = DwbServiceEnums.DwbService.None;
                            //WebserviceOptions = null;
                            // getting a project
                            DiversityWorkbench.Forms.FormGetProject fP = new Forms.FormGetProject(ServerConnection, "");
                            if (fP != null && fP.ProjectCount > 0)
                            {
                                fP.ShowDialog();
                                if (fP.DialogResult == System.Windows.Forms.DialogResult.OK && fP.ProjectID != null)
                                {
                                    ServerConnection.ProjectID = fP.ProjectID;
                                    ServerConnection.Project = fP.Project;
                                    Source += "; " + fP.Project;
                                }
                                SourceFound = true;
                                switch (ServerConnection.ModuleName)
                                {
                                    case "ScientificTerms":
                                        DiversityWorkbench.Forms.FormGetProject fS = new Forms.FormGetProject(ServerConnection, "", Forms.FormGetProject.Subgroup.Section, "TerminologyID = " + ServerConnection.ProjectID.ToString());
                                        break;
                                }
                            }
                        }
                        else if (f.SelectedString == "CacheDB" && DiversityWorkbench.Settings.ServerConnection.ModuleName == "DiversityCollection")
                        {
                            ServerConnection = DiversityWorkbench.Settings.ServerConnection;
                            //SQL = "SELECT DatabaseName FROM CacheDatabase 2";
                            string Database = CacheDatabase.CollectionCacheDB;// DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            if (Database.Length > 0)
                            {
                                SQL = "SELECT [" + Database + "].dbo.DiversityWorkbenchModule() ";
                                string AccessionTest = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                                if (AccessionTest == "DiversityCollectionCache")
                                {
                                    Webservice = DwbServiceEnums.DwbService.None;
                                    //WebserviceOptions = null;
                                    Source = f.SelectedString;

                                    ServerConnection.DatabaseName = Database;
                                    ServerConnection.ModuleName = AccessionTest;

                                    // getting a project
                                    SQL = "SELECT DISTINCT ProjectID, SourceView AS Project " +
                                        "FROM " + Database + ".dbo.TaxonSynonymy " +
                                        "WHERE not ProjectID is null " +
                                        "ORDER BY SourceView";
                                    System.Data.DataTable dtP = new System.Data.DataTable();
                                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                                    ad.Fill(dtP);
                                    DiversityWorkbench.Forms.FormGetStringFromList fP = new DiversityWorkbench.Forms.FormGetStringFromList(dtP, "Project", "ProjectID", "Project", "Please select a project from the list");
                                    fP.ShowDialog();
                                    if (fP.DialogResult == System.Windows.Forms.DialogResult.OK && fP.SelectedValue != null && fP.SelectedValue.Length > 0)
                                    {
                                        ServerConnection.ProjectID = int.Parse(fP.SelectedValue);
                                        ServerConnection.Project = fP.SelectedString;
                                        Source += "; " + fP.SelectedString;
                                    }
                                    else if (fP.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                                        SearchCanceled = true;

                                    SourceFound = true;
                                }
                            }
                        }
                        else if (IWorkbenchUnit.AccessibleDatabasesAndServicesOfModule().ContainsKey(f.SelectedString))
                        {
                            Source = f.SelectedString;
                            SourceFound = FixedSourceWebservice(Source, ref ServerConnection, ref Webservice);// true;
                        }
                    }
                    else if (f.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                        SearchCanceled = true;
                }
                if (SourceFound)
                {
                    buttonFixSource.Image = DiversityWorkbench.Properties.Resources.Pin_3; //this.imageList.Images[0];
                    comboBoxSourceValues.BackColor = System.Drawing.Color.SkyBlue;
                    toolTip.SetToolTip(buttonFixSource, "Source: " + Source);
                    toolTip.SetToolTip(comboBoxSourceValues, "Search for values in " + Source);
                }
                else if (!SearchCanceled)
                {
                    ServerConnection = null;
                    Webservice = DwbServiceEnums.DwbService.None;
                    //WebserviceOptions = null;
                    buttonFixSource.Image = DiversityWorkbench.Properties.Resources.Pin_3Gray;// this.imageList.Images[1];
                    comboBoxSourceValues.BackColor = System.Drawing.Color.White;
                    toolTip.SetToolTip(buttonFixSource, "Set the source");
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return SourceFound;
        }

        public static bool FixedSourceWebservice(
            string Service
            , ref DiversityWorkbench.ServerConnection ServerConnection
            , ref DWBServices.WebServices.DwbServiceEnums.DwbService Webservice)
        {
            bool SourceFound = true;
            Webservice = DwbServiceEnums.DwbService.None;
            if (Enum.TryParse(Service, true, out DwbServiceEnums.DwbService result))
            {
                Webservice = result;
            }

            if (Webservice == DwbServiceEnums.DwbService.None)
                SourceFound = false;
            
            if (SourceFound)
                ServerConnection = null;
            return SourceFound;
        }

        public static bool SaveSetting(System.Collections.Generic.List<string> Setting
           , DiversityWorkbench.ServerConnection ServerConnection
           , DWBServices.WebServices.DwbServiceEnums.DwbService Webservice
           )
        {
            bool OK = true;
            try
            {
                DiversityWorkbench.UserSettings U = new DiversityWorkbench.UserSettings();
                if (ServerConnection != null)
                {
                    if (ServerConnection.ModuleName == "DiversityCollectionCache" || ServerConnection.CacheDB.IndexOf("Cache") > -1)
                    {
                        U.DeleteSettingAttribute(Setting, "Webservice");
                        U.SaveSetting(Setting, "Project", ServerConnection.CacheDBSourceView);
                        // repeat to ensure entry
                        U.SaveSetting(Setting, "Project", ServerConnection.CacheDBSourceView);
                        U.SaveSetting(Setting, "ProjectID", ServerConnection.ProjectID.ToString());
                        U.SaveSetting(Setting, "Database", ServerConnection.CacheDB);
                        U.SaveSetting(Setting, "Module", ServerConnection.ModuleName);
                    }
                    else if (ServerConnection.ProjectID != null)
                    {
                        U.DeleteSettingAttribute(Setting, "Webservice");
                        U.SaveSetting(Setting, "Project", ServerConnection.Project);
                        // repeat to ensure entry
                        U.SaveSetting(Setting, "Project", ServerConnection.Project);
                        U.SaveSetting(Setting, "ProjectID", ServerConnection.ProjectID.ToString());
                        U.SaveSetting(Setting, "Database", ServerConnection.DisplayText);
                        if (ServerConnection.SectionID != null)
                        {
                            U.SaveSetting(Setting, "Section", ServerConnection.Section);
                            U.SaveSetting(Setting, "SectionID", ServerConnection.SectionID.ToString());
                        }
                    }
                }
                else if (Webservice != null)
                {
                    U.DeleteSettingAttribute(Setting, "Section");
                    U.DeleteSettingAttribute(Setting, "SectionID");
                    U.DeleteSettingAttribute(Setting, "Project");
                    U.DeleteSettingAttribute(Setting, "ProjectID");
                    U.DeleteSettingAttribute(Setting, "Database");
                    //U.SaveSetting(Setting, "", "");
                    U.SaveSetting(Setting, "Webservice", Webservice.ToString());
                    // U.SaveSetting(Setting, "Webservice", Webservie.ServiceName());
                    //if (WebserviceOptions != null && WebserviceOptions.Count > 0)
                    //{
                    //    foreach (DiversityWorkbench.WebserviceQueryOption O in WebserviceOptions)
                    //    {
                    //        if (O.Value != null && O.Value.Length > 0)
                    //            U.SaveSetting(Setting, O.Name(), O.Value);
                    //    }
                    //}
                }
                else
                    U.SaveSetting(Setting, "", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured when trying to save the settings. Message: " + ex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
                
            }
            return OK;
        }

        #endregion

    }
}
