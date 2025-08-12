using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    [Serializable]
    public class ServerConnection
    {
        #region Parameter and Properties

        private string _ModuleName;
        public string ModuleName
        {
            get { return _ModuleName; }
            set
            {
                if (_ModuleName == value)
                    return;
                _ModuleName = value;
                this._ConnectionIsValid = false;
            }
        }

        private string _DatabaseName;
        public string DatabaseName
        {
            get { return _DatabaseName; }
            set
            {
                string dbName = value;
                if (this.LinkedServer.Length > 0 && value.StartsWith("[" + this.LinkedServer + "]."))
                {
                    dbName = value.Substring(value.LastIndexOf(".") + 1);
                }
                if (_DatabaseName == dbName)
                    return;
                _DatabaseName = dbName;
                this._ConnectionIsValid = false;
            }
        }

        private string _DatabaseServer;
        public string DatabaseServer
        {
            get { return _DatabaseServer; }
            set
            {
                if (_DatabaseServer == value)
                    return;
                if (value == "127.0.0.1" || value == "localhost")
                    _DatabaseServer = "localhost";
                else
                    _DatabaseServer = value;
                this._ConnectionIsValid = false;
            }
        }

        private int _DatabaseServerPort;
        public int DatabaseServerPort
        {
            get { return _DatabaseServerPort; }
            set
            {
                if (_DatabaseServerPort == value)
                    return;
                _DatabaseServerPort = value;
                this._ConnectionIsValid = false;
            }
        }

        private bool _IsTrustedConnection;
        public bool IsTrustedConnection
        {
            get { return _IsTrustedConnection; }
            set
            {
                if (_IsTrustedConnection == value)
                    return;
                _IsTrustedConnection = value;
                this._ConnectionIsValid = false;
            }
        }

        private string _DatabaseUser;
        public string DatabaseUser
        {
            get { return _DatabaseUser; }
            set
            {
                if (_DatabaseUser == value)
                    return;
                _DatabaseUser = value;
                this._ConnectionIsValid = false;
            }
        }

        private string _DatabasePassword;
        public string DatabasePassword
        {
            get { return _DatabasePassword; }
            set
            {
                if (_DatabasePassword == value)
                    return;
                _DatabasePassword = value;
                this._ConnectionIsValid = false;
            }
        }

        private bool _IsLocalExpressDatabase;
        public bool IsLocalExpressDatabase
        {
            get { return _IsLocalExpressDatabase; }
            set
            {
                if (_IsLocalExpressDatabase == value)
                    return;
                _IsLocalExpressDatabase = value;
                this._ConnectionIsValid = false;
            }
        }

        private string _SqlExpressDbFileName;
        public string SqlExpressDbFileName
        {
            get { return _SqlExpressDbFileName; }
            set
            {
                if (_SqlExpressDbFileName == value)
                    return;
                _SqlExpressDbFileName = value;
                this._ConnectionIsValid = false;
            }
        }

        private string _BaseURL;
        public string BaseURL
        {
            get
            {
                if (_BaseURL == null &&
                    this.ConnectionString != null &&
                    this.ConnectionString.Length > 0)
                {
                }
                return _BaseURL;
            }
            set
            {
                _BaseURL = value;
            }
        }

        private string _DisplayText;
        public string DisplayText
        {
            get
            {
                if (this._DisplayText == null || this._DisplayText.Length == 0)
                {
                    string D = this._DatabaseName;
                    if (this._DatabaseServer != DiversityWorkbench.Settings.DatabaseServer && this._LinkedServer.Length == 0)
                    {
                        if (this._DatabaseName.EndsWith(" on " + this._DatabaseServer))
                        {
                            this._DatabaseName = this._DatabaseName.Substring(0, this._DatabaseName.Length - (this._DatabaseServer.Length + 4));
                            D = this._DatabaseName;
                        }
                        D += " on " + this._DatabaseServer;
                    }
                    else if (this._LinkedServer.Length > 0)
                    {
                        D = "[" + this._LinkedServer + "]." + this._DatabaseName;
                    }
                    return D;
                }
                else
                    return _DisplayText;
            }
            set { _DisplayText = value; }
        }

        public string DatabaseKey
        {
            get
            {
                return this.DatabaseServer + ":" + this.DatabaseServerPort.ToString() + " " + this.DatabaseName;
            }
        }

        public string DisplayTextExtended()
        {
            string Display = this.DisplayText;
            if (this.ProjectID != null && this.Project != null && this.Project.Length > 0)
            {
                Display += " (" + this.Project;
                if (this.SectionID != null && this.Section != null && this.Section.Length > 0)
                    Display += " - " + this.Section;
                Display += ")";
            }
            return Display;
        }

        private bool _ConnectionIsValid;
        public bool ConnectionIsValid
        {
            get
            {
                if (this._IsLocalExpressDatabase)
                {
                    System.IO.FileInfo F = new System.IO.FileInfo(_SqlExpressDbFileName);
                    if (!F.Exists)
                    {
                        this._ConnectionIsValid = false;
                        return false;
                    }
                }
                if (ConnectionString.Length == 0)
                {
                    this._ConnectionIsValid = false;
                    return false;
                }
                if (CacheDB.Length > 0 && ConnectionStringCacheDB.Length > 0)
                {
                    this._ConnectionIsValid = true;
                    return true;
                }
                if (CacheDB.Length > 0 && ModuleName != DiversityWorkbench.Settings.ModuleName && ConnectionStringModule.Length > 0)
                {
                    this._ConnectionIsValid = true;
                    return true;
                }
                if (ConnectionStringForDB(this._DatabaseName, this._ModuleName).Length == 0)
                {
                    this._ConnectionIsValid = false;
                    return false;
                }
                //if (BaseURL == null)
                return this._ConnectionIsValid;
            }
            set { _ConnectionIsValid = value; }
        }

        private bool _IsAddedRemoteConnection;
        public bool IsAddedRemoteConnection
        {
            get { return _IsAddedRemoteConnection; }
            set { _IsAddedRemoteConnection = value; }
        }

        private string _LinkedServer = "";
        public string LinkedServer
        {
            get { return _LinkedServer; }
            set { _LinkedServer = value; }
        }

        public string Prefix()
        {
            string Prefix = "dbo.";
            if (this.LinkedServer.Length > 0)
                Prefix = "[" + this.LinkedServer + "].[" + this.DatabaseName + "].dbo.";
#if DEBUG
            else if (ConnectionStringModule.Length > 0 && CacheDB.Length > 0 && ModuleName != DiversityWorkbench.Settings.ModuleName)
            {
                if (this.LinkedServer.Length > 0)
                    Prefix = "[" + this.LinkedServer + "].";
                else Prefix = "";
                Prefix += "[" + this.CacheDB + "].dbo.";
            }
            else Prefix = "[" + this.DatabaseName + "].dbo.";
#else
            else Prefix = "[" + this.DatabaseName + "].dbo.";
#endif
            return Prefix;
        }

        private int? _CurrentProjectID;
        public int? CurrentProjectID
        {
            get
            {
                int ID = 0;
                if (this._CurrentProjectID == null)
                {
                    string SQL = "select dbo.DefaultProjectID()";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    if (int.TryParse(C.ExecuteScalar()?.ToString(), out ID))
                        this._CurrentProjectID = ID;
                    con.Close();
                }
                return this._CurrentProjectID;
            }
            set
            {
                if (this._CurrentProjectID == null || this._CurrentProjectID != value)
                {
                    try
                    {
                        string SQL = "UPDATE UserProxy SET CurrentProjectID = " + value.ToString() + " WHERE LoginName = USER_NAME() ";
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        con.Open();
                        C.ExecuteNonQuery();
                        con.Close();
                        this._CurrentProjectID = value;
                    }
                    catch (System.Exception ex) { }
                }
            }
        }

#region Project to enable direct queries restricted to a project

        private int? _ProjectID;
        public int? ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }

        private string _Project;
        public string Project
        {
            get { return _Project; }
            set { _Project = value; }
        }

#endregion

#region Section to enable direct queries restricted to a section

        private int? _SectionID;
        public int? SectionID
        {
            get { return _SectionID; }
            set
            {
                _SectionID = value;
            }
        }

        private string _Section;
        public string Section
        {
            get { return _Section; }
            set
            {
                _Section = value;
            }
        }

#endregion

#region CacheDB

        private string _CacheDBSourceView;
        public string CacheDBSourceView
        {
            get
            {
                if (_CacheDBSourceView == null) _CacheDBSourceView = "";
                return _CacheDBSourceView;
            }
            set
            {
                _CacheDBSourceView = value;
                if (value.Length > 0)
                {
                    _DisplayText = "CacheDB: " + value.Replace("_", " ");
                }
            }
        }

        private string _CacheDB;
        public string CacheDB
        {
            get
            {
                if (_CacheDB == null) _CacheDB = "";
                return _CacheDB;
            }
            set
            {
                _CacheDB = value;
                if (_CacheDB.Length == 0) CacheDBSourceView = "";
            }
        }

        //public bool CacheDbIsAccessible
        //{
        //    get
        //    {
        //        if (_CacheDbIsAccessible != null)
        //            return (bool)_CacheDbIsAccessible;
        //        return false;
        //    }
        //}

        //private bool? _CacheDbIsAccessible;

        #endregion

        #region ModuleConnection

        private string _ModuleDBSourceView;
        public string ModuleDBSourceView
        {
            get
            {
                if (_ModuleDBSourceView == null) _ModuleDBSourceView = "";
                return _ModuleDBSourceView;
            }
            set
            {
                _ModuleDBSourceView = value;
                if (value.Length > 0)
                {
                    _DisplayText = "ModuleDB: " + value.Replace("_", " ");
                }
            }
        }

        private string _ModuleDB;
        public string ModuleDB
        {
            get
            {
                if (_ModuleDB == null) _ModuleDB = "";
                return _ModuleDB;
            }
            set
            {
                _ModuleDB = value;
                if (_ModuleDB.Length == 0) ModuleDBSourceView = "";
            }
        }


        #endregion

        #region Update of backlink modules

        private bool? _BackLinkIsUpdated = null;

        public bool? BackLinkIsUpdated
        {
            get
            {
                if (this.LinkedServer == null || this.LinkedServer.Length == 0)
                {
                    if (_BackLinkIsUpdated == null)
                        _BackLinkIsUpdated = false;
                    else
                    {
                        //string Server = this.DatabaseServer = ":" + this.DatabaseServerPort.ToString();
                        _BackLinkIsUpdated = DiversityWorkbench.Settings.BacklinkUpdatedDatabases().ContainsKey(this.DatabaseKey) 
                            && DiversityWorkbench.Settings.BacklinkUpdatedDatabases()[this.DatabaseKey].ContainsKey(this.ModuleName)
                            && DiversityWorkbench.Settings.BacklinkUpdatedDatabases()[this.DatabaseKey][this.ModuleName].Contains(this.DisplayText);
                    }
                }
                return _BackLinkIsUpdated;
            }
            set
            {
                if (this.LinkedServer.Length == 0)
                {
                    _BackLinkIsUpdated = value;
                }
            }
        }

        public bool BackLinkDoUpdate(string DatabaseKey, string Module)
        {
            bool DoUpdate = false;
            if (this.LinkedServer == null || this.LinkedServer.Length == 0)
            {
                DoUpdate = DiversityWorkbench.Settings.BacklinkUpdatedDatabases().ContainsKey(DatabaseKey)
                            && DiversityWorkbench.Settings.BacklinkUpdatedDatabases()[DatabaseKey].ContainsKey(Module)
                            && DiversityWorkbench.Settings.BacklinkUpdatedDatabases()[DatabaseKey][Module].Contains(this.DisplayText);
            }
            return DoUpdate;
        }

        //private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _BacklinkModules;

        //public Dictionary<string, List<string>> BacklinkModules
        //{
        //    get
        //    {
        //        if (_BacklinkModules == null)
        //        {
        //            _BacklinkModules = new Dictionary<string, List<string>>();
        //            {
        //                System.Collections.Generic.List<string> A = new List<string>();
        //                A.Add("DiversityCollection");
        //                A.Add("DiversityDescriptions");
        //                A.Add("DiversityProjects");
        //                _BacklinkModules.Add("DiversityAgents", A);

        //                System.Collections.Generic.List<string> C = new List<string>();
        //                C.Add("DiversityDescriptions");
        //                _BacklinkModules.Add("DiversityCollection", C);

        //                System.Collections.Generic.List<string> T = new List<string>();
        //                T.Add("DiversityCollection");
        //                T.Add("DiversityDescriptions");
        //                _BacklinkModules.Add("DiversityTaxonNames", T);
        //            }
        //        }
        //        return _BacklinkModules;
        //    }
        //}

        //private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ServerConnection>> _BacklinkModuleDatabases;

        //public void BacklinkModuleDatabase_Add(string Module, ServerConnection serverConnection)
        //{
        //    if (this._BacklinkModuleDatabases == null) this._BacklinkModuleDatabases = new Dictionary<string, List<ServerConnection>>();
        //    if (this._BacklinkModuleDatabases.ContainsKey(Module))
        //    {
        //        this._BacklinkModuleDatabases[Module].Add(serverConnection);
        //    }
        //    else
        //    {
        //        System.Collections.Generic.List<ServerConnection> L = new List<ServerConnection>();
        //        L.Add(serverConnection);
        //        this._BacklinkModuleDatabases.Add(Module, L);
        //    }
        //}

        //public void BacklinkModuleDatabase_Remove(string Module, ServerConnection serverConnection)
        //{
        //    if (this._BacklinkModuleDatabases != null && this._BacklinkModuleDatabases.ContainsKey(Module) && this._BacklinkModuleDatabases[Module].Contains(serverConnection))
        //    {
        //        this._BacklinkModuleDatabases[Module].Remove(serverConnection);
        //    }
        //}

        //public bool BacklinkModuleDatabaseIsUpdated(string Module, ServerConnection serverConnection)
        //{
        //    return (this._BacklinkModuleDatabases != null && this._BacklinkModuleDatabases.ContainsKey(Module) && this._BacklinkModuleDatabases[Module].Contains(serverConnection));
        //    //bool IsUpdated = false;
        //    //return IsUpdated;
        //}

        #endregion

        #region Ignored connections

        private static System.Collections.Generic.Dictionary<string, int> _IgnoredConnections;
        private static System.Collections.Generic.Dictionary<string, int> IgnoredConnections
        {
            get
            {
                if (_IgnoredConnections == null) _IgnoredConnections = new Dictionary<string, int>();
                return _IgnoredConnections;
            }
        }

        #endregion


        #endregion


        #region Construction
        public ServerConnection() { }

        public ServerConnection(string ConnectionString)
        {
            // Markus 3.12.2021 - sicherung in static Dict - wurde mehrfach aufgerufen
#if xxDEBUG
            if (_ServerConnections == null)
                _ServerConnections = new Dictionary<string, ServerConnection>();
#endif
            if (_ServerConnections != null && _ServerConnections.ContainsKey(ConnectionString))
            {
                this._DatabaseName = _ServerConnections[ConnectionString].DatabaseName;
                this._DatabaseServer = _ServerConnections[ConnectionString].DatabaseServer;
                this._DatabaseServerPort = _ServerConnections[ConnectionString].DatabaseServerPort;
                this._DatabaseUser = _ServerConnections[ConnectionString].DatabaseUser;
                this._IsTrustedConnection = _ServerConnections[ConnectionString].IsTrustedConnection;
                this._DatabasePassword = _ServerConnections[ConnectionString].DatabasePassword;
                this._BaseURL = _ServerConnections[ConnectionString].BaseURL;
                this._ModuleName = _ServerConnections[ConnectionString].ModuleName;
                this._CacheDB = _ServerConnections[ConnectionString].CacheDB;
                this._CacheDBSourceView = _ServerConnections[ConnectionString].CacheDBSourceView;
            }
            else
            {

                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                this._DatabaseName = con.Database;
                this._DatabaseServer = con.DataSource;
                if (this._DatabaseServer.IndexOf(',') > -1)
                {
                    string Port = this._DatabaseServer.Substring(this._DatabaseServer.IndexOf(',') + 1);
                    int iPort;
                    if (int.TryParse(Port, out iPort))
                    {
                        if (iPort > 0 && iPort < 65536)
                        {
                            this._DatabaseServerPort = iPort;
                            this._DatabaseServer = this._DatabaseServer.Substring(0, this._DatabaseServer.IndexOf(','));
                        }
                    }
                }

                string User = "";
                string[] ConnectionStringParts = ConnectionString.Split(new char[] { ';' });
                System.Collections.Generic.Dictionary<string, string> ConnDict = new Dictionary<string, string>();
                foreach (string s in ConnectionStringParts)
                {
                    string[] CC = s.Split(new char[] { '=' });
                    if (CC.Length == 2)
                        ConnDict.Add(CC[0], CC[1]);
                }
                if (ConnDict.ContainsKey("User ID"))
                {
                    this._DatabaseUser = ConnDict["User ID"];
                    this._IsTrustedConnection = false;
                    if (ConnDict.ContainsKey("Password") && ConnDict["Password"] != "######")
                    {
                        this._DatabasePassword = ConnDict["Password"];
                    }
                    else if (DiversityWorkbench.Settings.Password.Length > 0 && ConnDict["User ID"] == DiversityWorkbench.Settings.DatabaseUser)
                        this._DatabasePassword = DiversityWorkbench.Settings.Password;
                    else
                        this._DatabasePassword = "";
                }
                else
                    this._IsTrustedConnection = true;

                try
                {
                    if (this.ConnectionString.Length > 0)
                    {
                        string SQL = "SELECT dbo.BaseURL()";
                        Microsoft.Data.SqlClient.SqlConnection c = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, c);
                        c.Open();
                        this._BaseURL = C.ExecuteScalar()?.ToString();
                        C.CommandText = "SELECT dbo.DiversityWorkbenchModule()";
                        this._ModuleName = C.ExecuteScalar()?.ToString() ?? string.Empty;
                        c.Close();
                        c.Dispose();
                    }
                }
                catch (System.Exception ex) { }

                if (this._ModuleName == null || this._ModuleName.Length == 0)
                {
                    if (this._DatabaseName.IndexOf("_") > -1)
                        this._ModuleName = this._DatabaseName.Substring(0, this._DatabaseName.IndexOf('_'));
                    else
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                        {
                            if (this._DatabaseName.StartsWith(KV.Key))
                            {
                                this._ModuleName = KV.Key;
                                break;
                            }
                        }
                        if (this._ModuleName == null || this._ModuleName.Length == 0)
                            this._ModuleName = this._DatabaseName;
                    }
                }
                // Markus 3.12.2021 - sicherung in static Dict - wurde mehrfach aufgerufen
#if xxDEBUG
                if (_ServerConnections != null && !_ServerConnections.ContainsKey(ConnectionString))
                    _ServerConnections.Add(ConnectionString, this);

#endif
            }
        }

#endregion

        // Markus 3.12.2021 - sicherung in static Dict - wurde mehrfach aufgerufen
        private static System.Collections.Generic.Dictionary<string, ServerConnection> _ServerConnections;

#region Connection

        private string _ConnectionString;
        public string ConnectionString
        {
            get
            {
#if xxDEBUG
                // Markus 3.12.2021 - Mehrfach aufruf
                if (this._ConnectionString != null && this._ConnectionString.Length > 0)
                    return _ConnectionString;
#endif
                string ConStr = "";
                if (DatabaseServer == null && !IsLocalExpressDatabase) return "";
                if (!IsLocalExpressDatabase
                    && (string.IsNullOrEmpty(DatabaseServer) ||
                        (DatabaseName.Length == 0 && string.IsNullOrEmpty(ModuleName))))
                    return "";
                if (IsLocalExpressDatabase)
                {
                    if (!DatabaseName.Contains(ModuleName))
                        return "";
                    if (!SqlExpressDbFileName.Contains(ModuleName))
                        return "";
                    ConStr = @"Data Source=SQLNCLI;Server=.\SQLExpress;";
                    if (SqlExpressDbFileName.EndsWith(DatabaseName + "_Data.MDF"))
                        ConStr += "AttachDbFilename=" + SqlExpressDbFileName;
                    else
                    {
                        if (!DatabaseName.Contains(ModuleName))
                            return "";
                        System.IO.FileInfo FI = new System.IO.FileInfo(SqlExpressDbFileName);
                        string File = FI.DirectoryName + "\\" + DatabaseName + "_Data.MDF";
                        System.IO.FileInfo Fi1 = new System.IO.FileInfo(File);
                        if (Fi1.Exists)
                            ConStr += "AttachDbFilename=" + Fi1.FullName;
                        else
                        {
                            string CallingModule = SqlExpressDbFileName.Substring(SqlExpressDbFileName.IndexOf("\\"));
                            while (CallingModule.Contains("\\") && CallingModule.Length > 0)
                            {
                                CallingModule = CallingModule.Substring(CallingModule.IndexOf("\\") + 1);
                            }
                            CallingModule = CallingModule.Substring(0, CallingModule.IndexOf("_Data.MDF"));
                            System.IO.FileInfo Fi2 = new System.IO.FileInfo(SqlExpressDbFileName.Replace(CallingModule, ModuleName));
                            if (Fi2.Exists)
                                ConStr += "AttachDbFilename=" + Fi2.FullName;
                            else
                                return "";
                        }
                    }
                    ConStr += ";Database=" + DatabaseName;
                    ConStr += ";Trusted_Connection=Yes;";
                }
                else
                {
                    ConStr += "Data Source=";
                    ConStr += DatabaseServer;
                    if (DatabaseServerPort != 1433) ConStr += "," + DatabaseServerPort.ToString();
                    ConStr += ";";
                    if (DatabaseName.Length > 0)
                    {
                        if (LinkedServer.Length > 0)
                        {
                            ConStr += "Initial Catalog=" + DiversityWorkbench.Settings.DatabaseName + ";";
                        }
                        else
                        {
                            ConStr += "Initial Catalog=" + DatabaseName + ";";
                        }
                    }
                    else return "";
                    if (IsTrustedConnection)
                        ConStr += "Integrated Security=True; TrustServerCertificate=true"; 
                    else
                    {
                        ConStr += "User ID=" + DatabaseUser + ";";
                        if (DatabaseUser == null
                            || DatabaseUser.Length == 0
                            || DatabasePassword == null
                            || DatabasePassword.Length == 0)
                            return "";
                        ConStr += "Password=" + DatabasePassword;
                        ConStr += ";TrustServerCertificate=true"; 
                    }
                }
                if (this._ConnectionIsValid)
                {

                    // MW 2018/10/01: Encrypted connection
                    if (ConStr.Length > 0 && DiversityWorkbench.Settings.IsEncryptedConnection)
                        ConStr += ";Encrypt=true;TrustServerCertificate=true";

#if xxDEBUG
                    // Markus 3.12.2021 - Mehrfach aufruf
                    _ConnectionString = ConStr;
#endif
                    return ConStr;
                }
                else
                {
                    // Markus 2020-07-09: Restrict timeout to speed up queries that only test the compatiblity
                    string ConStrWithTimeout = ConStr;
                    if (ConStr.ToLower().IndexOf("timeout") == -1)
                    {
                        if (!ConStrWithTimeout.EndsWith(";"))
                            ConStrWithTimeout += ";";
                        ConStrWithTimeout += "Connect Timeout=" + DiversityWorkbench.Settings.TimeoutConnection.ToString() + ";";
                    }
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConStrWithTimeout);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand("SELECT dbo.BaseURL()", con);
                    if (this._LinkedServer.Length == 0)
                    {
                        try
                        {
                            con.Open();
                            this._BaseURL = C.ExecuteScalar()?.ToString() ?? string.Empty;
                            con.Close();
                            if (this._BaseURL.Length > 0)
                                this._ConnectionIsValid = true;
                            else
                                this._ConnectionIsValid = false;
#if xxDEBUG
                            // Markus 3.12.2021 - Mehrfach aufruf
                            _ConnectionString = ConStr;
#endif
                            return ConStr;
                        }
                        catch (System.Exception ex)
                        {
                            // Markus 14.6.24: Some servers seem to respond very slow
                            // it may be an option to store the timeout for every source - ToDo
                            // The connection to the current DB must be valid, otherwise the query will not be initialized
                            //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            if (!IgnoredConnections.ContainsKey(ConStr) && ex.Message.IndexOf(" Timeout ") > -1 && DiversityWorkbench.Settings.TimeoutConnection < 30 && DiversityWorkbench.Settings.TimeoutConnection > 0)
                            {
                                DiversityWorkbench.Forms.FormGetInteger f = new Forms.FormGetInteger(DiversityWorkbench.Settings.TimeoutConnection, "Connection timeout", 
                                    "The connection to the database\r\n   " +  this.DatabaseName + "\r\n on server\r\n   " + this.DatabaseServer + "\r\nfailed due to a timeout. \r\nDo you want to increase the timeout for the connection?\r\n(0 = no restriction)");
                                f.setOption(System.Windows.Forms.DialogResult.Ignore, "Ignore connection", DiversityWorkbench.Properties.Resources.Delete);
                                f.ShowDialog();
                                if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null)
                                    DiversityWorkbench.Settings.TimeoutConnection = (int)f.Integer;
                                else if (f.DialogResult == System.Windows.Forms.DialogResult.Ignore && f.Integer != null)
                                {
                                    if (!IgnoredConnections.ContainsKey(ConStr)) IgnoredConnections.Add(ConStr, (int)f.Integer);
                                }
                            }
                        }
                        C.CommandText = "SELECT dbo.DiversityWorkbenchModule()";
                        if (LinkedServer.Length > 0)
                            C.CommandText = "SELECT TOP 1 DiversityWorkbenchModule FROM ViewDiversityWorkbenchModule";
                        try
                        {
                            if (con.State != System.Data.ConnectionState.Open)
                                con.Open();
                            string Module = C.ExecuteScalar()?.ToString() ?? string.Empty;
                            con.Close();
							if (Module == string.Empty)
								return "";
#if xxDEBUG
                            // Markus 3.12.2021 - Mehrfach aufruf
                            _ConnectionString = ConStr;
#endif
                            return ConStr;
                        }
                        catch (System.Exception ex)
                        {
                            return "";
                        }
                    }
                    else
                    {
#if xxDEBUG
                        // Markus 3.12.2021 - Mehrfach aufruf
                        _ConnectionString = ConStr;
#endif
                        return ConStr;
                    }
                    return "";
                }
            }
        }

        public string ConnectionStringCacheDB
        {
            get
            {
                string ConStr = "";
                if (DatabaseServer == null && !IsLocalExpressDatabase) return "";
                if (!IsLocalExpressDatabase
                    && ((DatabaseServer == null || DatabaseServer.Length == 0) ||
                    CacheDB.Length == 0 || ModuleName.Length == 0))
                    return "";

                ConStr += "Data Source=";
                ConStr += DatabaseServer;
                if (DatabaseServerPort != 1433) ConStr += "," + DatabaseServerPort.ToString();
                ConStr += ";";
                ConStr += "Initial Catalog=";
                if (DiversityWorkbench.Settings.ModuleName == "DiversityCollection")
                {
                    if (this.CollectionCacheDB.Length == 0)
                        return "";
                    else
                        ConStr += CollectionCacheDB;
                }
                else
                    ConStr += CacheDB + ";";
                if (IsTrustedConnection)
                    ConStr += "Integrated Security=True; TrustServerCertificate=true"; // added
                else
                {
                    ConStr += "User ID=" + DatabaseUser + ";";
                    if (DatabaseUser == null
                        || DatabaseUser.Length == 0
                        || DatabasePassword == null
                        || DatabasePassword.Length == 0)
                        return "";
                    ConStr += "Password=" + DatabasePassword;
                    ConStr += "; TrustServerCertificate=true";
                }
                try
                {
                    DiversityWorkbench.ServerConnection S = new ServerConnection();
                    S.DatabaseName = CacheDB;
                    S.LinkedServer = "";
                    S.DatabasePassword = DatabasePassword;
                    S.DatabaseServer = DatabaseServer;
                    S.DatabaseServerPort = DatabaseServerPort;
                    S.DatabaseUser = DatabaseUser;
                    S.IsLocalExpressDatabase = false;
                    S.SqlExpressDbFileName = "";
                    S.IsTrustedConnection = IsTrustedConnection;
                    S.ModuleName = ModuleName;
                    if (S.ConnectionString.Length == 0)
                        ConStr = "";
                }
                catch { }

                return ConStr;
            }
        }

        public string ConnectionStringModule
        {
            get
            {
                string ConStr = "";
                if (DatabaseServer == null && !IsLocalExpressDatabase) return "";
                if (!IsLocalExpressDatabase
                    && ((DatabaseServer == null || DatabaseServer.Length == 0) ||
                    CacheDB.Length == 0 || ModuleName.Length == 0))
                    return "";

                ConStr += "Data Source=";
                ConStr += DatabaseServer;
                if (DatabaseServerPort != 1433) ConStr += "," + DatabaseServerPort.ToString();
                ConStr += ";";
                ConStr += "Initial Catalog=";
#if DEBUG
                if (DiversityWorkbench.Settings.ModuleName == "DiversityCollection" && ModuleName == "DiversityCollection")
                {
                    if (this.CollectionCacheDB.Length == 0)
                        return "";
                    else
                        ConStr += CollectionCacheDB;
                }
                else if (DiversityWorkbench.Settings.ModuleName != ModuleName && CacheDB.Length > 0)
                {
                    ConStr += CacheDB + ";";
                }
                else
                    ConStr += CacheDB + ";";
#else
                if (DiversityWorkbench.Settings.ModuleName == "DiversityCollection")
                {
                    if (this.CollectionCacheDB.Length == 0)
                        return "";
                    else
                        ConStr += CollectionCacheDB;
                }
                else
                    ConStr += CacheDB + ";";
#endif
                if (IsTrustedConnection)
                    ConStr += "Integrated Security=True; TrustServerCertificate=true";
                else
                {
                    ConStr += "User ID=" + DatabaseUser + ";";
                    if (DatabaseUser == null
                        || DatabaseUser.Length == 0
                        || DatabasePassword == null
                        || DatabasePassword.Length == 0)
                        return "";
                    ConStr += "Password=" + DatabasePassword;
                }
                try
                {
                    DiversityWorkbench.ServerConnection S = new ServerConnection();
                    S.DatabaseName = CacheDB;
                    S.LinkedServer = "";
                    S.DatabasePassword = DatabasePassword;
                    S.DatabaseServer = DatabaseServer;
                    S.DatabaseServerPort = DatabaseServerPort;
                    S.DatabaseUser = DatabaseUser;
                    S.IsLocalExpressDatabase = false;
                    S.SqlExpressDbFileName = "";
                    S.IsTrustedConnection = IsTrustedConnection;
                    S.ModuleName = ModuleName;
                    if (S.ConnectionString.Length == 0)
                        ConStr = "";
                }
                catch { }

                return ConStr;
            }
        }

        public string ConnectionStringForDB(string Database, string Module)
        {
            if (Database.Length == 0 || Module == null || Module.Length == 0)
                return "";
            else if (!Database.StartsWith(Module))
                return "";
            else
            {
                string ConStr = "";
                if (!IsLocalExpressDatabase && (DatabaseServer == null || DatabaseServer.Length == 0)) return "";
                if (IsLocalExpressDatabase && (SqlExpressDbFileName == null || SqlExpressDbFileName.Length == 0)) return "";
                if (DatabaseName == null || DatabaseName.Length == 0) return "";
                if (ModuleName == null || ModuleName.Length == 0) return "";
                if (IsLocalExpressDatabase)
                {
                    ConStr = @"Data Source=SQLNCLI;Server=.\SQLExpress;";
                    if (SqlExpressDbFileName.EndsWith(Database + "_Data.MDF")
                        && SqlExpressDbFileName.Contains(ModuleName))
                        ConStr += "AttachDbFilename=" + SqlExpressDbFileName;
                    else
                    {
                        System.IO.FileInfo FI = new System.IO.FileInfo(SqlExpressDbFileName);
                        string File = FI.DirectoryName + "\\" + Database + "_Data.MDF";
                        System.IO.FileInfo Fi1 = new System.IO.FileInfo(File);
                        if (Fi1.Exists && Fi1.FullName.Contains(ModuleName))
                            ConStr += "AttachDbFilename=" + Fi1.FullName;
                        else
                        {
                            string CallingModule = SqlExpressDbFileName.Substring(SqlExpressDbFileName.IndexOf("\\"));
                            while (CallingModule.Contains("\\") && CallingModule.Length > 0)
                            {
                                CallingModule = CallingModule.Substring(CallingModule.IndexOf("\\") + 1);
                            }
                            CallingModule = CallingModule.Substring(0, CallingModule.IndexOf("_Data.MDF"));
                            System.IO.FileInfo Fi2 = new System.IO.FileInfo(SqlExpressDbFileName.Replace(CallingModule, Module));
                            if (Fi2.Exists)
                                ConStr += "AttachDbFilename=" + Fi2.FullName;
                            else
                            {
                                System.IO.FileInfo Fi3 = new System.IO.FileInfo(SqlExpressDbFileName.Replace(CallingModule, Database));
                                if (Fi3.Exists)
                                    ConStr += "AttachDbFilename=" + Fi3.FullName;
                                return "";
                            }
                        }
                    }
                    ConStr += ";Database=" + Database;
                    ConStr += ";Trusted_Connection=Yes;";
                }
                else
                {
                    ConStr += "Data Source=" + DatabaseServer;
                    if (DatabaseServerPort != 1433) ConStr += "," + DatabaseServerPort.ToString();
                    ConStr += ";";
                    if (LinkedServer.Length > 0) ConStr += "Initial Catalog=" + DiversityWorkbench.Settings.DatabaseName + ";";
                    else if (Database.Length > 0) ConStr += "Initial Catalog=" + Database + ";";
                    else return "";
                    if (IsTrustedConnection)
                        ConStr += "Integrated Security=True";
                    else
                    {
                        ConStr += "User ID=" + DatabaseUser + ";";
                        ConStr += "Password=" + DatabasePassword;
                        if (DatabaseUser.Length == 0 ||
                            DatabasePassword.Length == 0)
                            return "";
                    }
                }
                try
                {
                    DiversityWorkbench.ServerConnection S = new ServerConnection();
                    S.DatabaseName = Database;
                    S.LinkedServer = LinkedServer;
                    S.DatabasePassword = DatabasePassword;
                    S.DatabaseServer = DatabaseServer;
                    S.DatabaseServerPort = DatabaseServerPort;
                    S.DatabaseUser = DatabaseUser;
                    S.IsLocalExpressDatabase = IsLocalExpressDatabase;
                    S.SqlExpressDbFileName = SqlExpressDbFileName;
                    S.IsTrustedConnection = IsTrustedConnection;
                    S.ModuleName = ModuleName;
                    return S.ConnectionString;
                }
                catch { }

                // MW 2018/10/01: Encrypted connection
                if (ConStr.Length > 0 && DiversityWorkbench.Settings.IsEncryptedConnection)
                    ConStr += ";Encrypt=true;TrustServerCertificate=true";

                return ConStr;
            }
        }

        public void setConnection(string ConnectionString)
        {
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            this.DatabaseName = con.Database;
            string Server = con.DataSource;
            int Port = 1433;
            if (Server.IndexOf(",") > -1)
            {
                Port = int.Parse(Server.Substring(Server.IndexOf(",")+1));
                Server = Server.Substring(0, Server.IndexOf(","));
            }
            this.DatabaseServer = Server;
            this.DatabaseServerPort = Port;
            if (ConnectionString.IndexOf("Integrated Security=True") > -1)
                this.IsTrustedConnection = true;
            else
            {
                if (ConnectionString.IndexOf("PW=") > -1)
                    this.DatabasePassword = "";
                if (ConnectionString.IndexOf("USER=") > -1)
                    this.DatabaseUser = "";
            }
        }

#endregion

#region CollectionCacheDB

        private void CheckConnectionChange()
        {
            if (this._CollectionConnStrCurrent == null)
            {
                _CollectionCacheDB = null;
                _CollectionCacheDbAccessible = null;
                _CollectionConnStrCurrent = DiversityWorkbench.Settings.ConnectionString;
            }
            else
            {
                if (_CollectionConnStrCurrent != DiversityWorkbench.Settings.ConnectionString)
                {
                    _CollectionCacheDB = null;
                    _CollectionCacheDbAccessible = null;
                    _CollectionConnStrCurrent = DiversityWorkbench.Settings.ConnectionString;
                }
            }
        }

        private string _CollectionConnStrCurrent = null;

        private bool? _CollectionCacheDbAccessible = null;
        public bool CollectionCacheDbAccessible
        {
            get
            {
                if (DiversityWorkbench.Settings.ModuleName != "DiversityCollection")
                    return false;

                this.CheckConnectionChange();

                if (_CollectionCacheDbAccessible != null) return (bool)_CollectionCacheDbAccessible;
                try
                {
                    if (this.CacheDB.Length > 0 && this.CacheDBSourceView.Length > 0)
                    {
                        //if (this.CacheDB.Length == 0)
                        //{
                        //    _CollectionCacheDbAccessible = false;
                        //    return (bool)_CollectionCacheDbAccessible;
                        //}
                        string CollCache = this.CollectionCacheDB;
                        if (CollCache.Length > 0)
                        {
                            string[] ConStr = DiversityWorkbench.Settings.ConnectionString.Split(new char[] { ';' });
                            string conStr = "";
                            foreach (string S in ConStr)
                            {
                                if (conStr.Length > 0) conStr += ";";
                                if (S.StartsWith("Initial"))
                                {
                                    conStr += S.Substring(0, S.IndexOf("="));
                                    conStr += "=" + CollCache; // this._CacheDB;
                                }
                                else conStr += S;
                            }
                            using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(conStr))
                            {
                                string SQL = "SELECT COUNT(*) FROM " + this.CacheDB.Replace("Diversity", "") + "_" + this.CacheDBSourceView;
                                con.Open();
                                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                                try
                                {
                                    string Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                                    if (Result == string.Empty)
                                        _CollectionCacheDbAccessible = false;
                                    _CollectionCacheDbAccessible = true;
                                }
                                catch (System.Exception ex) { _CollectionCacheDbAccessible = false; }
                                con.Close();
                            }
                            if (_CollectionCacheDbAccessible != null)
                                return (bool)_CollectionCacheDbAccessible;
                            else return false;
                        }
                        else
                        {
                            _CollectionCacheDbAccessible = false;
                            return (bool)_CollectionCacheDbAccessible;
                        }
                    }
                    else
                        return false;
                }
                catch { return false; }
            }
        }

        private string _CollectionCacheDB = null;
        public string CollectionCacheDB
        {
            get
            {
                if (DiversityWorkbench.Settings.ModuleName != "DiversityCollection")
                    return "";

                this.CheckConnectionChange();

                if (_CollectionCacheDB != null) return _CollectionCacheDB;

                _CollectionCacheDB = CacheDatabase.CollectionCacheDB;

                //string SQL = "SELECT TOP (1) [DatabaseName] FROM [CacheDatabase 2]";
                //_CollectionCacheDB = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                return _CollectionCacheDB;
            }
        }

#endregion



#region Keys for autocomplete collections

        public string Key()
        {
            string key = this.BaseURL;
            key += "|";
            if (this.ProjectID != null) key += this.ProjectID.ToString();
            key += "|";
            if (this.SectionID != null) key += this.SectionID.ToString();
            return key;
        }

        public string KeyCacheDB()
        {
            string key = "";
            if (this.CacheDB.Length > 0 && this.CacheDBSourceView.Length > 0)
                key = this.ModuleName + "|" + this.CacheDBSourceView;
            return key;
        }

        public string KeyModule()
        {
            string key = "";
            if (this.CacheDB.Length > 0 && this.CacheDBSourceView.Length > 0)
                key = this.ModuleName + "|" + this.CacheDBSourceView;
            return key;
        }

#endregion

    }


}
