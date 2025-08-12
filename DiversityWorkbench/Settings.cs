using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    public class Settings
    {

        #region Directory, saving etc.

        public static string SettingsDirectory()
        {
            System.IO.DirectoryInfo Directory = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Settings));// DiversityWorkbench.Settings.ResourcesDirectoryModule() + "\\Settings");
            if (!Directory.Exists)
            {
                Directory.Create();
                Directory.Attributes = System.IO.FileAttributes.Directory | System.IO.FileAttributes.Hidden;

            }
            return Directory.FullName;
        }

        public static string SettingsFile()
        {
            string FileName = Settings.SettingsDirectory() + "\\Settings.xml";
            return FileName;
        }

        public static void SaveWorkbenchSettings()
        {
            string FileName = Settings.SettingsFile();
            System.Xml.XmlWriter W = null;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(FileName, settings);
            W.WriteStartDocument();
            W.WriteStartElement("Settings");
            W.WriteElementString("DatabaseName", DiversityWorkbench.Settings.DatabaseName);
            W.WriteElementString("DatabasePort", DiversityWorkbench.Settings.DatabasePort.ToString());
            W.WriteElementString("DatabaseServer", DiversityWorkbench.Settings.DatabaseServer);
            W.WriteElementString("DatabaseUser", DiversityWorkbench.Settings.DatabaseUser);
            W.WriteElementString("IsTrustedConnection", DiversityWorkbench.Settings.IsTrustedConnection.ToString());
            W.WriteElementString("QueryMaxResults", DiversityWorkbench.Settings.QueryMaxResults.ToString());
            W.WriteEndElement();//Settings
            W.WriteEndDocument();
            W.Flush();
            W.Close();
        }

        private static void ReadSettingsFromResources()
        {
            System.Xml.XmlReaderSettings xSettings = new System.Xml.XmlReaderSettings();
            System.Xml.Linq.XElement SettingsDocument = null;
            System.IO.FileInfo _SettingsFile = new System.IO.FileInfo(Settings.SettingsFile());
            if (_SettingsFile.Exists)
            {
                SettingsDocument = System.Xml.Linq.XElement.Load(System.Xml.XmlReader.Create(_SettingsFile.FullName, xSettings));
                IEnumerable<System.Xml.Linq.XElement> Sets = SettingsDocument.Elements();
                // Read the entire XML
                foreach (var Set in Sets)
                {
                    try
                    {
                        switch (Set.Name.LocalName)
                        {
                            case "DatabaseName":
                                DiversityWorkbench.WorkbenchSettings.Default.DatabaseName = Set.Value;
                                break;
                            case "DatabasePort":
                                DiversityWorkbench.WorkbenchSettings.Default.DatabasePort = int.Parse(Set.Value);
                                break;
                            case "DatabaseServer":
                                DiversityWorkbench.WorkbenchSettings.Default.DatabaseServer = Set.Value;
                                break;
                            case "DatabaseUser":
                                DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser = Set.Value;
                                break;
                            case "IsTrustedConnection":
                                DiversityWorkbench.WorkbenchSettings.Default.IsTrustedConnection = bool.Parse(Set.Value);
                                break;
                            case "QueryMaxResults":
                                DiversityWorkbench.WorkbenchSettings.Default.QueryMaxResults = int.Parse(Set.Value);
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
        }

        public static void TransferPreviousSettings()
        {
            try
            {
                DiversityWorkbench.WorkbenchSettings.Default.Upgrade();
                DiversityWorkbench.WorkbenchSettings.Default.Reload();
                DiversityWorkbench.WorkbenchSettings.Default.Save();

                DiversityWorkbench.PostgreSQL.Connection.UpgradeSettings();

                DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Upgrade();
                DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Reload();
                DiversityWorkbench.Forms.FormRemoteQuerySettings.Default.Save();

                DiversityWorkbench.Import.SettingsImport.Default.Upgrade();
                DiversityWorkbench.Import.SettingsImport.Default.Reload();
                DiversityWorkbench.Import.SettingsImport.Default.Save();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            if (DiversityWorkbench.WorkbenchSettings.Default.DatabasePort == 0 ||
                DiversityWorkbench.WorkbenchSettings.Default.DatabasePort == 1433)
            {
                ReadSettingsFromResources();
            }

            return;

            //if (DiversityWorkbench.WorkbenchSettings.Default.DatabasePort != 1433)
            //    return;


            #region Old version using GetPreviousVersion

            bool ErrorOccured = false;
            try
            {
                string Setting = "";
                try { Setting = DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("DatabaseName").ToString(); }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    ErrorOccured = true;
                }
                if (Setting.Length > 0)
                    DiversityWorkbench.WorkbenchSettings.Default.DatabaseName = Setting;

                Setting = "";
                try { Setting = DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("DatabaseServer").ToString(); }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    ErrorOccured = true;
                }
                if (Setting.Length > 0)
                    DiversityWorkbench.WorkbenchSettings.Default.DatabaseServer = Setting;

                bool IsTrusted = true;
                try
                {
                    if (bool.TryParse(DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("IsTrustedConnection").ToString(), out IsTrusted))
                        DiversityWorkbench.WorkbenchSettings.Default.IsTrustedConnection = IsTrusted;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    ErrorOccured = true;
                }

                bool IsLocal = false;
                try
                {
                    if (bool.TryParse(DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("IsLocalExpressDB").ToString(), out IsLocal))
                        DiversityWorkbench.WorkbenchSettings.Default.IsLocalExpressDB = IsLocal;
                }
                catch (System.Exception ex)
                {
                    ErrorOccured = true;
                }

                Setting = "";
                try { Setting = DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("SqlExpressDbFileName").ToString(); }
                catch (System.Exception ex)
                {
                    ErrorOccured = true;
                }
                if (Setting.Length > 0)
                    DiversityWorkbench.WorkbenchSettings.Default.SqlExpressDbFileName = Setting;

                Setting = "";
                try { Setting = DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("DatabaseUser").ToString(); }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    ErrorOccured = true;

                }
                if (Setting.Length > 0)
                    DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser = Setting;

                int Port = 5432;
                try
                {
                    if (int.TryParse(DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("DatabasePort").ToString(), out Port))
                        DiversityWorkbench.WorkbenchSettings.Default.DatabasePort = Port;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    ErrorOccured = true;

                }

                Setting = "";
                try { Setting = DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("QueryMaxResults").ToString(); }
                catch (System.Exception ex) { }
                if (Setting.Length > 0)
                {
                    int i = 0;
                    if (int.TryParse(Setting, out i))
                        DiversityWorkbench.WorkbenchSettings.Default.QueryMaxResults = i;
                }

                int MaximalImageSizeInKb = 2000;
                try
                {
                    if (int.TryParse(DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("MaximalImageSizeInKb").ToString(), out MaximalImageSizeInKb))
                        DiversityWorkbench.WorkbenchSettings.Default.MaximalImageSizeInKb = MaximalImageSizeInKb;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    ErrorOccured = true;

                }

                int QueryLimitDropdownList = 0;
                try
                {
                    if (int.TryParse(DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("QueryLimitDropdownList").ToString(), out QueryLimitDropdownList))
                        DiversityWorkbench.WorkbenchSettings.Default.QueryLimitDropdownList = QueryLimitDropdownList;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    ErrorOccured = true;

                }

                System.Collections.Specialized.StringCollection SS = new System.Collections.Specialized.StringCollection();
                try
                {
                    if (DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("AddedRemoteConnections") != null)
                    {
                        SS = (System.Collections.Specialized.StringCollection)DiversityWorkbench.WorkbenchSettings.Default.GetPreviousVersion("AddedRemoteConnections");
                        DiversityWorkbench.WorkbenchSettings.Default.AddedRemoteConnections = SS;
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    ErrorOccured = true;

                }


                DiversityWorkbench.Settings.Password = "";

                DiversityWorkbench.WorkbenchSettings.Default.Save();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                ErrorOccured = true;

                //System.Windows.Forms.MessageBox.Show("To transfer the settings of a previous version this version should have been located in the same directory");
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            //if (ErrorOccured)
            //    DiversityWorkbench.WorkbenchSettings.Default.Upgrade();

            #endregion

        }

        #endregion

        #region Connection

        #region Default connection

        public static bool FirstLogin
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.FirstLogin;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.FirstLogin = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static string DefaultConnectionFile
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.DefaultConnectionFile;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.DefaultConnectionFile = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static string UpdateExcludeFile
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.UpdateExcludeFile;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.UpdateExcludeFile = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        /// <summary>
        /// Get connection string for a windows connection to the master database
        /// This function is exclusively used for the silent database upgrade using the command line interface!
        /// It is checked that the resulting windows connection has sysadmin rights. If not an empty string is returned!
        /// </summary>
        /// <param name="Server">Server</param>
        /// <param name="Port">Port</param>
        /// <returns>Connection string</returns>
        public static string MasterWindowsConnectionString(string Server, int Port)
        {
            // Build connection string
            string connectionString = "Data Source=" + Server + "," + Port.ToString();
            connectionString += ";initial catalog=master;Integrated Security=True;Encrypt=true;TrustServerCertificate=true";

            // Check that login has sysadmin rights
            string SQL = "SELECT USER_NAME()";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            string userName = "";
            try
            {
                con.Open();
                userName = C.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
            }
            catch (Exception ex)
            {
                connectionString = "";
            }
            if (userName != "dbo")
            {
                connectionString = "";
            }
            return connectionString;
        }

        /// <summary>
        /// Normalize version of Format "1.2.3" to "0001.0002.0003"
        /// </summary>
        /// <param name="Version">Version string</param>
        /// <param name="Count">Count per block (Default:4)</param>
        /// <returns>Normalized version string</returns>
        public static string NormalizeVersion(string Version, int Count = 4)
        {
            string result = "";
            Version = Version.Replace('/', '.');
            if (Version != null && Version.Contains("."))
            {
                string[] splitVersion = Version.Split('.');
                for (int i = 0; i < splitVersion.Length; i++)
                {
                    result += (result == "" ? "" : ".") + (splitVersion[i].Length < Count ? new string('0', Count - splitVersion[i].Length) : "") + splitVersion[i];
                }
            }
            return result;
        }

        /// <summary>
        /// Set parameter for a default Windows connection
        /// Gets a Server, Port and a matching, that can be used to access with the Windows connection 
        /// for the first start of the application. If in the workbench directory is a file (file name  
        /// DefaultConnectionFile of the settings), the Server name and Port are read from the file. 
        /// A database matching the specified client and database version and the connection properties
        /// are stored in the settings data.
        /// Expected file format: [server], [port]
        /// </summary>
        /// <param name="ClientVersion">Version of the client</param>
        /// <param name="DatabaseVersion">Minimal required database version</param>
        /// <returns>'true' if parameter are available</returns>
        public static bool SetDefaultConnection(string ClientVersion, string DatabaseVersion)
        {
            string Server = "";
            int Port = 0;
            string DatabaseName = "";
            try
            {
                // Check for default connection only for first login
                if (DiversityWorkbench.WorkbenchSettings.Default.FirstLogin)
                {
                    // Normalize versions
                    ClientVersion = NormalizeVersion(ClientVersion);
                    DatabaseVersion = NormalizeVersion(DatabaseVersion, 2);

                    // Reset first login flag
                    DiversityWorkbench.WorkbenchSettings.Default.FirstLogin = false;
                    DiversityWorkbench.WorkbenchSettings.Default.Save();

                    // Check if default connection file is available
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryPath());
                    System.IO.FileInfo fi = new System.IO.FileInfo(di.FullName + System.IO.Path.DirectorySeparatorChar + DiversityWorkbench.WorkbenchSettings.Default.DefaultConnectionFile);
                    if (fi.Exists)
                    {
                        string dataLine = null;
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(fi.FullName))
                        {
                            while (!reader.EndOfStream)
                            {
                                dataLine = reader.ReadLine().Trim();
                                if (dataLine.StartsWith("--") || dataLine.StartsWith("//"))
                                    continue;
                                if (dataLine.Length > 0)
                                    break;
                            }
                            reader.Close();
                        }
                        if (dataLine != null)
                        {
                            // File must contain connection parameter in format <server>, <port>
                            string[] fields = dataLine.Split(',');
                            if (fields.Length == 2 && fields[0].Trim().Length > 0 && fields[1].Trim().Length > 0)
                            {
                                if (int.TryParse(fields[1], out Port))
                                {
                                    Server = fields[0].Trim();

                                    // Get database list
                                    Dictionary<string, string> databases = GetDatabaseList(Server, Port, ClientVersion);
                                    if (databases == null || databases.Count == 0)
                                        return false;

                                    // Check for best fitting database
                                    string compVersion = "0000.0000.0000";
                                    foreach (KeyValuePair<string, string> item in databases)
                                    {
                                        if (compVersion.CompareTo(item.Value) <= 0 && DatabaseVersion.CompareTo(item.Value) <= 0)
                                        {
                                            compVersion = item.Value;
                                            DatabaseName = item.Key;
                                        }
                                    }
                                    if (DatabaseName.Length > 0)
                                    {
                                        DiversityWorkbench.Settings.DatabaseServer = Server;
                                        DiversityWorkbench.Settings.DatabasePort = Port;
                                        DiversityWorkbench.Settings.DatabaseName = DatabaseName;
                                        DiversityWorkbench.Settings.IsTrustedConnection = true;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return false;
        }

        /// <summary>
        /// Get list of databases that shall be updated
        /// Provides a list of databases for the current workbench module, that can be accessed with the 
        /// Windows connection specified by Server and Port. If in the application directory is a file
        /// (file name in UpdateExcludeFile of the settings), the database names in this file are 
        /// excluded from the result list.
        /// </summary>
        /// <param name="ConnectionString">Connection string</param>
        /// <returns>List of database names</returns>
        public static Dictionary<string, string> GetUpdateDatabases(string ConnectionString)
        {
            try
            {
                // Check if datasbase exclusion file is available
                List<string> ignoreList = new List<string>();
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule());
                System.IO.FileInfo fi = new System.IO.FileInfo(di.FullName + System.IO.Path.DirectorySeparatorChar + DiversityWorkbench.WorkbenchSettings.Default.UpdateExcludeFile);
                if (fi.Exists)
                {
                    string dataLine = null;
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(fi.FullName))
                    {
                        while (!reader.EndOfStream)
                        {
                            dataLine = reader.ReadLine().Trim();
                            if (dataLine.StartsWith("--") || dataLine.StartsWith("//"))
                                continue;
                            if (dataLine.Length > 0)
                                ignoreList.Add(dataLine);
                        }
                        reader.Close();
                    }
                }
                // Return database list
                return GetDatabaseList(ConnectionString, "", ignoreList);
            }
            catch (Exception ex)
            { }
            Dictionary<string, string> result = new Dictionary<string, string>();
            return result;
        }

        /// <summary>
        /// Get list of databases that can be accessed
        /// Provides a list of databases for the current workbench module, that can be accessed with the 
        /// Windows connection specified by Server and Port. If an IgnoreList is specified, the databases
        /// from the ignoreList will not be included in the result.
        /// </summary>
        /// <param name="ConnectionString">Connection string</param>
        /// <param name="Server">Server</param>
        /// <param name="Port">Port</param>
        /// <param name="ClientVersion">Client version</param>
        /// <param name="IgnoreList">List of databases that shall be ignored</param>
        /// <returns>List of database names and versions</returns>
        public static Dictionary<string, string> GetDatabaseList(string Server, int Port, string ClientVersion, List<string> IgnoreList = null)
        {
            string connString = MasterWindowsConnectionString(Server, Port);
            return GetDatabaseList(connString, ClientVersion, IgnoreList);
        }

        /// <summary>
        /// Get list of databases that can be accessed
        /// Provides a list of databases for the current workbench module, that can be accessed with the 
        /// Windows connection specified by Server and Port. If an IgnoreList is specified, the databases
        /// from the ignoreList will not be included in the result.
        /// </summary>
        /// <param name="ConnectionString">Connection string</param>
        /// <param name="ClientVersion">Client version</param>
        /// <param name="IgnoreList">List of databases that shall be ignored</param>
        /// <returns>List of database names and versions</returns>
        public static Dictionary<string, string> GetDatabaseList(string ConnectionString, string ClientVersion, List<string> IgnoreList = null)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                if (ConnectionString != null && ConnectionString.Length > 0)
                {
                    // Get database list
                    string moduleName = DiversityWorkbench.WorkbenchSettings.Default.ModuleName;
                    if (moduleName.Trim().Length <= 0)
                        return result;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    string SQL = string.Format("SELECT name as DatabaseName FROM sys.databases where name not in ( 'master', 'model', 'tempdb', 'msdb')"
                                 + " AND name LIKE '{0}%' AND NOT name LIKE '{0}Cache%'", moduleName);
                    SQL += " ORDER BY name";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                    try
                    {
                        ad.Fill(dt);
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                        return result;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ClientVersion = NormalizeVersion(ClientVersion);
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                        con.Open();
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand("", con);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (IgnoreList != null && IgnoreList.Contains((string)R[0]))
                                continue;

                            // Build SQL requests for checks
                            SQL = "use " + R[0].ToString() + "; IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Version]') AND " +
                                  "type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                                  "BEGIN SELECT dbo.Version() END " +
                                  "ELSE BEGIN SELECT NULL END";
                            C.CommandText = SQL;
                            try
                            {
                                string databaseVersion = NormalizeVersion(C.ExecuteScalar()?.ToString(), 2);
                                if (databaseVersion.Length > 0)
                                {
                                    string SqlModule = "use " + R[0].ToString() + "; IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityWorkbenchModule]') AND " +
                                                       "type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                                                       "BEGIN SELECT dbo.DiversityWorkbenchModule() END " +
                                                       "ELSE BEGIN SELECT NULL END";
                                    C.CommandText = SqlModule;
                                    string module = C.ExecuteScalar()?.ToString() ?? string.Empty;
                                    if (module.Trim().Length <= 0)
                                        continue;
                                    if (moduleName == module)
                                    {
                                        if (ClientVersion == null || ClientVersion.Length == 0)
                                            result.Add(R[0].ToString(), databaseVersion);
                                        else
                                        {
                                            string SqlClient = "use " + R[0].ToString() + "; IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityWorkbenchModule]') AND " +
                                                               "type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                                                               "BEGIN SELECT dbo.VersionClient() END " +
                                                               "ELSE BEGIN SELECT NULL END";
                                            C.CommandText = SqlClient;
                                            string client = NormalizeVersion(C.ExecuteScalar()?.ToString());
                                            if (ClientVersion.CompareTo(client) >= 0)
                                                result.Add(R[0].ToString(), databaseVersion);
                                        }
                                    }
                                }
                            }
                            catch { }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            { }
            return result;
        }
        #endregion

        #region Connection parameters

        private static string myPassword = "";

        public static string Password
        {
            get
            {
                if (DiversityWorkbench.WorkbenchSettings.Default.Password.Length > 0)
                    return DiversityWorkbench.WorkbenchSettings.Default.Password;
                else
                    return Settings.myPassword;
                //if (Settings.myPassword.Length == 0 && !Settings.IsTrustedConnection)
                //{
                //    DiversityWorkbench.FormDatabaseConnection f = new FormDatabaseConnection();
                //    f.ShowDialog();
                //    //if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                //    //{
                //    //    Settings.myPassword = f.Password;
                //    //    if (f.UserName != DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser)
                //    //        DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser = f.UserName;
                //    //}
                //    //else Settings.myPassword = "";
                //}
                //return Settings.myPassword; 
            }
            set
            {
                myPassword = value;
            }
        }

        public static string ModuleName
        {
            get
            {
                if (DiversityWorkbench.WorkbenchSettings.Default.ModuleName.Length == 0 && DiversityWorkbench.Settings.ConnectionString != null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT [dbo].[DiversityWorkbenchModule] ()";
                    string Module = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Module.Length > 0)
                        DiversityWorkbench.WorkbenchSettings.Default.ModuleName = Module;
                }
                return DiversityWorkbench.WorkbenchSettings.Default.ModuleName;
            }
            set
            {
                try
                {
                    DiversityWorkbench.WorkbenchSettings.Default.ModuleName = value;
                    DiversityWorkbench.WorkbenchSettings.Default.Save();
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        public static string DatabaseUser
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
                DefaultDatabaseAvailable = null;
            }
        }

        public static string DatabaseName
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.DatabaseName;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.DatabaseName = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
                DefaultDatabaseAvailable = null;
            }
        }

        public static string DatabaseServer
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.DatabaseServer;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.DatabaseServer = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
                DefaultDatabaseAvailable = null;
            }
        }

        public static int DatabasePort
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.DatabasePort;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.DatabasePort = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
                DefaultDatabaseAvailable = null;
            }
        }

        public static bool IsTrustedConnection
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.IsTrustedConnection;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.IsTrustedConnection = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
                DefaultDatabaseAvailable = null;
            }
        }

        public static bool IsEncryptedConnection
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.IsEncryptedConnection;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.IsEncryptedConnection = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static bool IsLocalExpressDatabase
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.IsLocalExpressDB;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.IsLocalExpressDB = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
                DefaultDatabaseAvailable = null;
            }
        }

        public static string SqlExpressDbFileName
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.SqlExpressDbFileName;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.SqlExpressDbFileName = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
                DefaultDatabaseAvailable = null;
            }
        }

        public static string SqlExpressDbFileNameForDatabase(string DatabaseName)
        {
            string File = "";
            if (DiversityWorkbench.Settings.SqlExpressDbFileName.Length > 0)
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(DiversityWorkbench.Settings.SqlExpressDbFileName);
                File = FI.DirectoryName + "\\" + DatabaseName + "_Data.MDF";
                System.IO.FileInfo Fi1 = new System.IO.FileInfo(File);
                if (Fi1.Exists)
                    return File;
                else
                {
                    string CallingModule = SqlExpressDbFileName.Substring(SqlExpressDbFileName.IndexOf("\\"));
                    while (CallingModule.Contains("\\") && CallingModule.Length > 0)
                    {
                        CallingModule = CallingModule.Substring(CallingModule.IndexOf("\\") + 1);
                    }
                    if (CallingModule.IndexOf("_Data.MDF") > -1)
                        CallingModule = CallingModule.Substring(0, CallingModule.IndexOf("_Data.MDF"));
                    File = SqlExpressDbFileName;
                    if (DatabaseName.IndexOf("_") > -1) File = File.Replace(CallingModule, DatabaseName.Substring(0, DatabaseName.IndexOf("_")));
                    else File = File.Replace(CallingModule, DatabaseName);
                    System.IO.FileInfo Fi2 = new System.IO.FileInfo(File);
                    if (Fi2.Exists)
                        return Fi2.FullName;
                    else
                    {
                        System.IO.FileInfo Fi3 = new System.IO.FileInfo(SqlExpressDbFileName.Replace(CallingModule, DatabaseName));
                        if (Fi3.Exists)
                            return Fi3.FullName;
                        else
                        {
                            System.IO.FileInfo Fi4 = new System.IO.FileInfo(Fi2.DirectoryName + "\\" + DatabaseName + "_Data.MDF");
                            if (Fi4.Exists)
                                return Fi4.FullName;
                            else
                            {
                                System.IO.FileInfo Fi5 = new System.IO.FileInfo(Fi3.DirectoryName + "\\" + DatabaseName + "_Data.MDF");
                                if (Fi5.Exists)
                                    return Fi5.FullName;
                            }
                        }
                        return "";
                    }
                }
            }
            return "";
        }

        #endregion

        public static string ConnectionString
        {
            get
            {
                string ConStr = "";
                try
                {
                    if (DiversityWorkbench.WorkbenchSettings.Default.DatabaseServer.Length == 0 ||
                        DiversityWorkbench.WorkbenchSettings.Default.DatabaseName.Length == 0)
                        return "";
                    if (!DiversityWorkbench.WorkbenchSettings.Default.IsLocalExpressDB)
                    {
                        ConStr += "Data Source=" + DiversityWorkbench.WorkbenchSettings.Default.DatabaseServer;
                        if (DiversityWorkbench.WorkbenchSettings.Default.DatabasePort != 1433) ConStr += "," + DiversityWorkbench.WorkbenchSettings.Default.DatabasePort.ToString();
                        ConStr += ";";
                        ConStr += "Initial Catalog=" + DiversityWorkbench.WorkbenchSettings.Default.DatabaseName + ";";
                        if (DiversityWorkbench.WorkbenchSettings.Default.IsTrustedConnection)
                        {
                            ConStr += "Integrated Security=True;";
                        }
                        else
                        {
                            ConStr += "User ID=" + DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser + ";";
                            ConStr += "Password=" + DiversityWorkbench.Settings.Password;
                            if (DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser.Length == 0 ||
                                DiversityWorkbench.Settings.Password.Length == 0)
                                return "";
                        }
                    }
                    else
                    {
                        if (!DiversityWorkbench.WorkbenchSettings.Default.DatabaseName.Contains(DiversityWorkbench.WorkbenchSettings.Default.ModuleName))
                            return "";
                        ConStr = @"Data Source=SQLNCLI;Server=.\SQLExpress;";
                        ConStr += "AttachDbFilename=" + DiversityWorkbench.WorkbenchSettings.Default.SqlExpressDbFileName;
                        ConStr += ";Database=" + DiversityWorkbench.WorkbenchSettings.Default.DatabaseName;
                        ConStr += ";Trusted_Connection=Yes;";
                    }

                    // MW 2018/10/01: Encrypted connection
                    if (ConStr.Length > 0 && DiversityWorkbench.Settings.IsEncryptedConnection)
                        ConStr += ";Encrypt=true;TrustServerCertificate=true";
                    // MW 2018/11/14: Do not connection string for not available database
                    if (DefaultDatabaseAvailable == null || DefaultDatabaseAvailable == false)
                    {
                        try
                        {
                            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConStr);
                            Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand("SELECT 1", con);
                            con.Open();
                            c.ExecuteNonQuery();
                            con.Close();
                            DefaultDatabaseAvailable = true;
                        }
                        catch (Exception ex)
                        {
                            DefaultDatabaseAvailable = false;
                            ConStr = "";
                        }
                    }
                    else if (DefaultDatabaseAvailable == false)
                        ConStr = "";
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    DefaultDatabaseAvailable = false;
                    ConStr = "";
                }
                return ConStr;
            }
        }

        /// <summary>
        /// Connection string to master database
        /// </summary>
        public static string ConnectionStringToMaster
        {
            get
            {
                string ConStr = "";
                try
                {
                    if (DiversityWorkbench.WorkbenchSettings.Default.DatabaseServer.Length == 0 ||
                        DiversityWorkbench.WorkbenchSettings.Default.DatabaseName.Length == 0)
                        return "";
                    ConStr += "Data Source=" + DiversityWorkbench.WorkbenchSettings.Default.DatabaseServer;
                    if (DiversityWorkbench.WorkbenchSettings.Default.DatabasePort != 1433) ConStr += "," + DiversityWorkbench.WorkbenchSettings.Default.DatabasePort.ToString();
                    ConStr += ";";
                    ConStr += "Initial Catalog=master;";
                    if (DiversityWorkbench.WorkbenchSettings.Default.IsTrustedConnection)
                    {
                        ConStr += "Integrated Security=True;";
                        ConStr += "TrustServerCertificate=True";
                    }
                    else
                    {
                        ConStr += "User ID=" + DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser + ";";
                        ConStr += "Password=" + DiversityWorkbench.Settings.Password;
                        ConStr += ";TrustServerCertificate=True";
                        if (DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser.Length == 0 ||
                            DiversityWorkbench.Settings.Password.Length == 0)
                            return "";
                    }

                    // Encrypted connection
                    if (ConStr.Length > 0 && DiversityWorkbench.Settings.IsEncryptedConnection)
                        ConStr += ";Encrypt=true;TrustServerCertificate=true";
                    try
                    {
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConStr);
                        Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand("SELECT 1", con);
                        con.Open();
                        c.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        ConStr = "";
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    ConStr = "";
                }
                return ConStr;
            }
        }

        // #137
        public static void ResetDefaultDatabaseAvailability() { DefaultDatabaseAvailable = null; }
        public static void ResetDefaultDatabase() { ResetDefaultDatabaseAvailability(); }

        private static bool? DefaultDatabaseAvailable = null;

        private static Microsoft.Data.SqlClient.SqlConnection _Connection;
        public static Microsoft.Data.SqlClient.SqlConnection Connection
        {
            get
            {
                if (DiversityWorkbench.Settings._Connection == null)
                    DiversityWorkbench.Settings._Connection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                return DiversityWorkbench.Settings._Connection;
            }
            set
            {
                DiversityWorkbench.Settings._Connection = value;
            }
        }

        public static DiversityWorkbench.ServerConnection ServerConnection
        {
            get
            {
                DiversityWorkbench.ServerConnection S = new DiversityWorkbench.ServerConnection();
                S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
                S.DatabaseName = DiversityWorkbench.Settings.DatabaseName;
                S.DatabasePassword = DiversityWorkbench.Settings.Password;
                S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
                S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
                S.ModuleName = DiversityWorkbench.Settings.ModuleName;
                S.IsLocalExpressDatabase = DiversityWorkbench.Settings.IsLocalExpressDatabase;
                S.SqlExpressDbFileName = DiversityWorkbench.Settings.SqlExpressDbFileName;
                return S;
            }
        }


        public static string ConnectionStringWithTimeout(int Timeout)
        {
            string ConStr = "";
            try
            {
                if (DiversityWorkbench.WorkbenchSettings.Default.DatabaseServer.Length == 0 ||
                    DiversityWorkbench.WorkbenchSettings.Default.DatabaseName.Length == 0)
                    return "";
                ConStr += "Data Source=" + DiversityWorkbench.WorkbenchSettings.Default.DatabaseServer;
                if (DiversityWorkbench.WorkbenchSettings.Default.DatabasePort != 1433) ConStr += "," + DiversityWorkbench.WorkbenchSettings.Default.DatabasePort.ToString();
                ConStr += ";";
                ConStr += "Initial Catalog=" + DiversityWorkbench.WorkbenchSettings.Default.DatabaseName + ";";
                if (DiversityWorkbench.WorkbenchSettings.Default.IsTrustedConnection)
                {
                    ConStr += "Integrated Security=True;";
                    ConStr += "TrustServerCertificate=True";
                    
                }
                else
                {
                    ConStr += "User ID=" + DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser + ";";
                    ConStr += "Password=" + DiversityWorkbench.Settings.Password;
                    if (DiversityWorkbench.WorkbenchSettings.Default.DatabaseUser.Length == 0 ||
                        DiversityWorkbench.Settings.Password.Length == 0)
                        return "";
                    ConStr += "; TrustServerCertificate=True";
                }
                ConStr += ";Connection Timeout=" + Timeout.ToString() + ";";

                // MW 2018/11/14: Do not connection string for not available database
                if (DefaultDatabaseAvailable == null)
                {
                    try
                    {
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConStr);
                        Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand("SELECT 1", con);
                        con.Open();
                        c.ExecuteNonQuery();
                        con.Close();
                        DefaultDatabaseAvailable = true;
                    }
                    catch (Exception ex)
                    {
                        DefaultDatabaseAvailable = false;
                        ConStr = "";
                    }
                }
                else if (DefaultDatabaseAvailable == false)
                    ConStr = "";

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return ConStr;
        }

        public static System.Collections.Specialized.StringCollection AddedRemoteConnections
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.AddedRemoteConnections;
            }
            set
            {
                System.Collections.Specialized.StringCollection SS = new System.Collections.Specialized.StringCollection();
                foreach (string s in value)
                    if (!SS.Contains(s))
                        SS.Add(s);
                DiversityWorkbench.WorkbenchSettings.Default.AddedRemoteConnections = SS;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static void AddRemoteConnection(DiversityWorkbench.ServerConnection SC)
        {
            string Connection = SC.ConnectionString;
            if (Connection.IndexOf("Password=") > -1)
            {
                string PW = "Password=" + SC.DatabasePassword;
                Connection = Connection.Replace(PW, "Password=######");
            }
            System.Collections.Specialized.StringCollection CC = new System.Collections.Specialized.StringCollection();
            if (Settings.AddedRemoteConnections != null)
                CC = Settings.AddedRemoteConnections;
            CC.Add(Connection);
            Settings.AddedRemoteConnections = CC;
            DiversityWorkbench.WorkbenchSettings.Default.Save();
        }

        public static void AddPreviousConnection(string Server, int Port, string User)
        {
            string NewConnection = Server + "|" + Port.ToString() + "|" + User;
            if (DiversityWorkbench.WorkbenchSettings.Default.PreviousConnections == null)
                DiversityWorkbench.WorkbenchSettings.Default.PreviousConnections = new System.Collections.Specialized.StringCollection();
            if (!DiversityWorkbench.WorkbenchSettings.Default.PreviousConnections.Contains(NewConnection))
            {
                DiversityWorkbench.WorkbenchSettings.Default.PreviousConnections.Add(NewConnection);
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static void AddPreviousConnection(string Server, int Port)
        {
            string NewConnection = Server + "|" + Port.ToString();
            if (DiversityWorkbench.WorkbenchSettings.Default.PreviousConnections == null)
                DiversityWorkbench.WorkbenchSettings.Default.PreviousConnections = new System.Collections.Specialized.StringCollection();
            if (!DiversityWorkbench.WorkbenchSettings.Default.PreviousConnections.Contains(NewConnection))
            {
                DiversityWorkbench.WorkbenchSettings.Default.PreviousConnections.Add(NewConnection);
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }
        #endregion

        #region Timeout

        public static int TimeoutWeb
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.TimeoutWeb;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.TimeoutWeb = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static int TimeoutWebInSeconds
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.TimeoutWeb / 1000;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.TimeoutWeb = value * 1000;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static int TimeoutDatabase
        {
            get
            {
                // fixing bug milisecond <> seconds if untouched
                if (DiversityWorkbench.WorkbenchSettings.Default.TimeoutDatabase == 30000)
                    DiversityWorkbench.WorkbenchSettings.Default.TimeoutDatabase = 30;
                return DiversityWorkbench.WorkbenchSettings.Default.TimeoutDatabase;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.TimeoutDatabase = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static int TimeoutConnection
        {
            get
            {
                // fixing bug milisecond <> seconds if untouched
                if (DiversityWorkbench.WorkbenchSettings.Default.TimeoutConnection == 3000)
                    DiversityWorkbench.WorkbenchSettings.Default.TimeoutConnection = 3;
                return DiversityWorkbench.WorkbenchSettings.Default.TimeoutConnection;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.TimeoutConnection = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static int TimeoutStart
        {
            get
            {
                // fixing bug milisecond <> seconds if untouched
                if (DiversityWorkbench.WorkbenchSettings.Default.TimeoutStart == 1000)
                    DiversityWorkbench.WorkbenchSettings.Default.TimeoutStart = 1;
                return DiversityWorkbench.WorkbenchSettings.Default.TimeoutStart;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.TimeoutStart = value;
                if (DiversityWorkbench.WorkbenchSettings.Default.TimeoutStart < 0)
                    DiversityWorkbench.WorkbenchSettings.Default.TimeoutStart = System.Math.Abs(DiversityWorkbench.WorkbenchSettings.Default.TimeoutStart);
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }


        #endregion

        #region Geographic infos

        public static bool GazetteerHierarchyPlaceToCountry
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.GazetteerHierarchyPlaceToCountry;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.GazetteerHierarchyPlaceToCountry = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static string GazetteerHierarchySeparator
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.GazetteerHierarchySeparator;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.GazetteerHierarchySeparator = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static string CountryListSource
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.CountryListSource;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.CountryListSource = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static double GeoLatitude
        {
            get { return DiversityWorkbench.Forms.FormGoogleMapsCoordinatesSettings.Default.Latitude; }
        }

        public static double GeoLongitude
        {
            get { return DiversityWorkbench.Forms.FormGoogleMapsCoordinatesSettings.Default.Longitude; }
        }

        #endregion

        #region Diverses

        public static string QRcodeService
        {
            get { return DiversityWorkbench.WorkbenchSettings.Default.QRcodeService; }
            set { DiversityWorkbench.WorkbenchSettings.Default.QRcodeService = value; DiversityWorkbench.WorkbenchSettings.Default.Save(); }
        }

        public static int QRcodeSize
        {
            get { return DiversityWorkbench.WorkbenchSettings.Default.QRcodeSize; }
            set { DiversityWorkbench.WorkbenchSettings.Default.QRcodeSize = value; DiversityWorkbench.WorkbenchSettings.Default.Save(); }
        }

        public static int MaximalImageSizeInKb
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.MaximalImageSizeInKb;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.MaximalImageSizeInKb = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static int FormLimitCharaterCount
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.FormLimitCharaterCount;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.FormLimitCharaterCount = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static string Language
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.Language;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.Language = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
                DiversityWorkbench.Entity.ResetContextTable();
            }
        }

        public static string Context
        {
            get
            {
                if (DiversityWorkbench.WorkbenchSettings.Default.Context != "System.Data.DataRowView")
                    return DiversityWorkbench.WorkbenchSettings.Default.Context;
                else
                    return "";
            }
            set
            {
                if (value != "System.Data.DataRowView")
                {
                    DiversityWorkbench.WorkbenchSettings.Default.Context = value;
                    DiversityWorkbench.WorkbenchSettings.Default.Save();
                }
            }
        }

        public static void SetCurrentProjectID(int ProjectID, DiversityWorkbench.ServerConnection SC)
        {

        }

        public static bool UseEntity
        {
            get { return DiversityWorkbench.WorkbenchSettings.Default.UseEntity; }
            set { DiversityWorkbench.WorkbenchSettings.Default.UseEntity = value; }
        }

        public static bool UseAutocompletion
        {
            get { return DiversityWorkbench.WorkbenchSettings.Default.UseAutocompletion; }
            set { DiversityWorkbench.WorkbenchSettings.Default.UseAutocompletion = value; DiversityWorkbench.WorkbenchSettings.Default.Save(); }
        }

        public static bool UseQueryCharts
        {
            get { return DiversityWorkbench.WorkbenchSettings.Default.UseQueryCharts; }
            set { DiversityWorkbench.WorkbenchSettings.Default.UseQueryCharts = value; DiversityWorkbench.WorkbenchSettings.Default.Save(); }
        }


        public static bool LoadConnections
        {
            get { return DiversityWorkbench.WorkbenchSettings.Default.LoadConnections; }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.LoadConnections = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        private static bool _LoadConnectionsPassed = false;
        public static bool LoadConnectionsPassed(bool Passed = false)
        {
            if (_LoadConnectionsPassed) return _LoadConnectionsPassed;
            _LoadConnectionsPassed = Passed; return _LoadConnectionsPassed;
        }

        //public static string ScientificTermsRemoteParameter
        //{
        //    get { return DiversityWorkbench.WorkbenchSettings.Default.ScientificTermsRemoteParameter; }
        //    set
        //    {
        //        DiversityWorkbench.WorkbenchSettings.Default.ScientificTermsRemoteParameter = value;
        //        DiversityWorkbench.WorkbenchSettings.Default.Save();
        //    }
        //}

        #endregion

        #region Query

        public static int QueryMaxResults
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.QueryMaxResults;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.QueryMaxResults = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static int QueryLimitDropdownList
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.QueryLimitDropdownList;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.QueryLimitDropdownList = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static int QueryLimitHierarchy
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.QueryLimitHierarchy;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.QueryLimitHierarchy = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static int QueryLimitCharaterCount
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.QueryLimitCharacterCount;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.QueryLimitCharacterCount = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static bool QueryOptimizedByDefault
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.QueryOptimizedByDefault;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.QueryOptimizedByDefault = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static bool QueryRememberQuerySettingsByDefault
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.QueryRememberQuerySettingsByDefault;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.QueryRememberQuerySettingsByDefault = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        #endregion


        #region Many order columns

        public static readonly int ManyOrderColumnDefaultWidth = 10;

        public static void ManyOrderColumnSave(string Module, string Form, System.Collections.Generic.Dictionary<string, int> OrderColumns)
        {
            if (DiversityWorkbench.WorkbenchSettings.Default.ManyOrderColumns == null && OrderColumns.Count > 0 && Module.Length > 0)
            {
                DiversityWorkbench.WorkbenchSettings.Default.ManyOrderColumns = new System.Collections.Specialized.StringCollection();
            }
            if (DiversityWorkbench.WorkbenchSettings.Default.ManyOrderColumns != null)
            {
                System.Collections.Generic.List<string> Content = new List<string>();
                foreach (string C in DiversityWorkbench.WorkbenchSettings.Default.ManyOrderColumns)
                    Content.Add(C);
                DiversityWorkbench.WorkbenchSettings.Default.ManyOrderColumns.Clear();
                foreach (string C in Content)
                {
                    string[] cc = C.Split(new char[] { '|' });
                    if (cc[0].ToUpper() != Module.ToUpper() || cc[1].ToUpper() != Form.ToUpper())
                        DiversityWorkbench.WorkbenchSettings.Default.ManyOrderColumns.Add(C);
                }
                foreach (System.Collections.Generic.KeyValuePair<string, int> KV in OrderColumns)
                {
                    DiversityWorkbench.WorkbenchSettings.Default.ManyOrderColumns.Add(Module + "|" + Form + "|" + KV.Key + "|" + KV.Value.ToString());
                }
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static System.Collections.Generic.Dictionary<string, int> ManyOrderColumns(string Module, string Form)
        {
            System.Collections.Generic.Dictionary<string, int> Columns = new Dictionary<string, int>();
            if (Form.Length > 0 && DiversityWorkbench.WorkbenchSettings.Default.ManyOrderColumns != null)
            {
                foreach (string C in DiversityWorkbench.WorkbenchSettings.Default.ManyOrderColumns)
                {
                    string[] cc = C.Split(new char[] { '|' });
                    if (cc[0].ToUpper() == Module.ToUpper() && cc[1].ToUpper() == Form.ToUpper())
                        Columns.Add(cc[2], int.Parse(cc[3]));
                }
            }
            //DiversityWorkbench.WorkbenchSettings.Default.ManyOrderColumns.Clear(); - da stand alter Code drin
            return Columns;
        }

        public static void ManyOrderColumnSaveWidth(string Module, string Column, int Width)
        {

        }

        #endregion

        #region Backlinks

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> _BacklinkUpdatedDatabases;

        /// <summary>
        /// Parse Backlink databases from setting stored as Source{Module:Database|Database|}Module:Database|}
        /// </summary>
        /// <returns>Dictionary containing servers with modules and databases</returns>
        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> BacklinkUpdatedDatabases()
        {
            try
            {
                if (_BacklinkUpdatedDatabases == null || _BacklinkUpdatedDatabases.Count == 0)
                {
                    _BacklinkUpdatedDatabases = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>>();
                    if (DiversityWorkbench.WorkbenchSettings.Default.BacklinkUpdate == null)
                        DiversityWorkbench.WorkbenchSettings.Default.BacklinkUpdate = new System.Collections.Specialized.StringCollection();
                    else
                    {
                        foreach (string B in DiversityWorkbench.WorkbenchSettings.Default.BacklinkUpdate)
                        {
                            // separating the server from modules
                            string Source = "";
                            string Backlink = "";
                            if (B.IndexOf('{') > -1)
                            {
                                string[] ss = B.Split(new char[] { '{' });
                                Source = ss[0];
                                Backlink = ss[1];
                            }
                            else
                            {
                                Source = DiversityWorkbench.Settings.ServerConnection.DatabaseKey;
                                Backlink = B;
                            }
                            // separating the modules
                            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> Modules = new Dictionary<string, List<string>>();
                            if (Backlink.Length > 1)
                            {
                                string[] mm = Backlink.Split(new char[] { '}' });
                                foreach (string m in mm)
                                {
                                    if (m.Length > 0)
                                    {
                                        // separating module from databases
                                        string[] MM = m.Split(new char[] { ':' });
                                        string Module = MM[0];
                                        // separating the databases
                                        string[] dd = MM[1].Split(new char[] { '|' });
                                        System.Collections.Generic.List<string> dbs = new List<string>();
                                        foreach (string d in dd)
                                        {
                                            if (d.Length > 0) dbs.Add(d);
                                        }
                                        Modules.Add(Module, dbs);
                                    }
                                }
                            }
                            _BacklinkUpdatedDatabases.Add(Source, Modules);
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return _BacklinkUpdatedDatabases;
        }

        /// <summary>
        /// Adding a database
        /// </summary>
        /// <param name="Source">The Source to which the database should be added - written as ServerName:Port</param>
        /// <param name="Module">The module to which the database should be added</param>
        /// <param name="Database">The database that should be added encoded with the display text from the ServerConnection to ensure unique names</param>
        public static void BacklinkDatabaseAdd(string Source, string Module, string Database)
        {
            if (BacklinkUpdatedDatabases().ContainsKey(Source))
            {
                if (BacklinkUpdatedDatabases()[Source].ContainsKey(Module))
                {
                    if (!BacklinkUpdatedDatabases()[Source][Module].Contains(Database))
                        BacklinkUpdatedDatabases()[Source][Module].Add(Database);
                }
                else
                {
                    System.Collections.Generic.List<string> DD = new List<string>();
                    DD.Add(Database);
                    BacklinkUpdatedDatabases()[Source].Add(Module, DD);
                }
            }
            else
            {
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> Modules = new Dictionary<string, List<string>>();
                System.Collections.Generic.List<string> l = new List<string>();
                l.Add(Database);
                Modules.Add(Module, l);
                BacklinkUpdatedDatabases().Add(Source, Modules);
            }
        }

        /// <summary>
        /// Removing a database
        /// </summary>
        /// <param name="Source">The Source from which the database should be removed - written as ServerName:Port</param>
        /// <param name="Module">The module from which the database should be removed</param>
        /// <param name="Database">The database that should be removed encoded with the display text from the ServerConnection to ensure unique names</param>
        public static void BacklinkDatabaseRemove(string Source, string Module, string Database)
        {
            if (BacklinkUpdatedDatabases().ContainsKey(Source))
            {
                if (BacklinkUpdatedDatabases()[Source].ContainsKey(Module))
                {
                    if (BacklinkUpdatedDatabases()[Source][Module].Contains(Database))
                        BacklinkUpdatedDatabases()[Source][Module].Remove(Database);
                }
            }
        }

        /// <summary>
        /// Writing the Backlinks into the settings: Server{Module:Database|Database|}Module:Database|} 
        /// for every server separate to ensure correct selection when server is changed
        /// </summary>
        public static void BacklinksSave()
        {
            DiversityWorkbench.WorkbenchSettings.Default.BacklinkUpdate.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> KV in BacklinkUpdatedDatabases())
            {
                string Backlink = KV.Key + "{";
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> kv in KV.Value)
                {
                    Backlink += kv.Key + ":";
                    foreach (string DB in kv.Value)
                        Backlink += DB + "|";
                    Backlink += "}";
                }
                DiversityWorkbench.WorkbenchSettings.Default.BacklinkUpdate.Add(Backlink);
            }
            DiversityWorkbench.WorkbenchSettings.Default.Save();
        }

        public static void BacklinkUpdatedDatabases_Reset()
        {
            _BacklinkUpdatedDatabases = null;
            DiversityWorkbench.WorkbenchSettings.Default.BacklinkUpdate = null;
            DiversityWorkbench.WorkbenchSettings.Default.Save();
        }

        #endregion

        #region WebView

        public static bool UseWebView
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.UseWebView;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.UseWebView = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        /// <summary>
        /// Set Display of images to new WebView2
        /// </summary>
        /// <param name="UseWebView">If WebView2 should be used for all images</param>
        /// <param name="toolStripButton">an optional toolstripbutton that will be adapted concerning icon and tooltip text</param>
        public static void WebViewUsage(bool UseWebView, System.Windows.Forms.ToolStripButton toolStripButton = null)
        {
            try
            {
                DiversityWorkbench.WorkbenchSettings.Default.UseWebView = UseWebView;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
                if (toolStripButton != null)
                {
                    WebViewUsage(toolStripButton);
                }
            }
            catch (System.Exception ex) { ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// Adapt toolstripbutton concerning icon and tooltip text
        /// </summary>
        /// <param name="toolStripButton">The toolstripbutton that should be adapted to DiversityWorkbench.WorkbenchSettings.Default.UseWebView</param>
        public static void WebViewUsage(System.Windows.Forms.ToolStripButton toolStripButton)
        {
            try
            {
                if (UseWebView)
                {
                    toolStripButton.Image = DiversityWorkbench.Properties.Resources.ExternerBrowserSmall;
                    toolStripButton.ToolTipText = "Brower display. Click to change to image display";
                }
                else
                {
                    toolStripButton.Image = DiversityWorkbench.Properties.Resources.Icones;
                    toolStripButton.ToolTipText = "Image display. Click to change to browser display";
                }
            }
            catch (System.Exception ex) { ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #region Resources - functions Transferred to class WorkbenchResources.WorkbenchDirectory

        public static string ResourcesDirectory
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.ResourcesDirectory;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.ResourcesDirectory = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        //public static string ResourcesDirectoryModule()
        //{
        //    System.IO.DirectoryInfo SpreadsheetDirectory = new System.IO.DirectoryInfo(DiversityWorkbench.Settings.ResourcesDirectoryWorkbench() + "\\" + DiversityWorkbench.WorkbenchSettings.Default.ModuleName);
        //    if (!SpreadsheetDirectory.Exists)
        //    {
        //        SpreadsheetDirectory.Create();
        //    }
        //    return SpreadsheetDirectory.FullName;
        //}

        //public enum ResoucesDirectoryTypes { Home, MyDocuments, Application }


        //public static string ResourcesDirectoryWorkbench()
        //{
        //    string FolderPath = ...Windows.Forms.Application.StartupPath + "\\DiversityWorkbench";
        //    // The application directory for the resources
        //    if (DiversityWorkbench.Settings.ResourcesDirectory == DiversityWorkbench.Settings.ResoucesDirectoryTypes.Application.ToString())
        //    {
        //        System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(...Windows.Forms.Application.StartupPath);
        //        FolderPath = DI.Parent.FullName + "\\DiversityWorkbench";
        //    }
        //    // The home directory for the user
        //    else if (DiversityWorkbench.Settings.ResourcesDirectory.ToUpper() == DiversityWorkbench.Settings.ResoucesDirectoryTypes.Home.ToString().ToUpper())
        //    {
        //        try
        //        {
        //            var homeDrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
        //            if (homeDrive != null)
        //            {
        //                var homePath = Environment.GetEnvironmentVariable("HOMEPATH");
        //                if (homePath != null)
        //                {
        //                    var fullHomePath = homeDrive + System.IO.Path.DirectorySeparatorChar + homePath;
        //                    FolderPath = System.IO.Path.Combine(fullHomePath, "DiversityWorkbench");
        //                }
        //                else
        //                {
        //                    throw new Exception("Environment variable error, there is no 'HOMEPATH'");
        //                }
        //            }
        //            else
        //            {
        //                throw new Exception("Environment variable error, there is no 'HOMEDRIVE'");
        //            }
        //        }
        //        catch (System.Exception ex)
        //        {
        //        }
        //        if (FolderPath.Length > 0)
        //        {
        //            System.IO.DirectoryInfo HomeDirectory = new System.IO.DirectoryInfo(FolderPath);
        //            if (HomeDirectory.Exists)
        //                FolderPath = HomeDirectory.FullName;
        //        }
        //    }
        //    // the myDocuments directory
        //    else if (DiversityWorkbench.Settings.ResourcesDirectory == DiversityWorkbench.Settings.ResoucesDirectoryTypes.MyDocuments.ToString())
        //    {
        //        System.IO.DirectoryInfo HomeDirectory = new System.IO.DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        //        //System.IO.DirectoryInfo HomeDirectory = new System.IO.DirectoryInfo(System.Environment.SpecialFolder.MyDocuments.ToString());
        //        if (HomeDirectory.Exists)
        //            FolderPath = HomeDirectory.FullName + "\\DiversityWorkbench";
        //    }
        //    // a path entered by the user
        //    else
        //    {
        //        System.IO.DirectoryInfo HomeDirectory = new System.IO.DirectoryInfo(DiversityWorkbench.Settings.ResourcesDirectory);
        //        if (HomeDirectory.Exists)
        //        {
        //            FolderPath = HomeDirectory.FullName;
        //            if (!FolderPath.EndsWith("\\DiversityWorkbench"))
        //                FolderPath += "\\DiversityWorkbench";
        //        }
        //    }
        //    System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(FolderPath);
        //    if (!D.Exists)
        //        D.Create();
        //    return FolderPath;
        //}

        //private static string ApplicationDirectory()
        //{
        //    System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(...Windows.Forms.Application.StartupPath);
        //    string FolderPath = DI.Parent.FullName + "\\DiversityWorkbench";
        //    return FolderPath;
        //}

        //private static string HomeDirectory()
        //{
        //    string FolderPath = "";
        //    try
        //    {
        //        var homeDrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
        //        if (homeDrive != null)
        //        {
        //            var homePath = Environment.GetEnvironmentVariable("HOMEPATH");
        //            if (homePath != null)
        //            {
        //                var fullHomePath = homeDrive + System.IO.Path.DirectorySeparatorChar + homePath;
        //                FolderPath = System.IO.Path.Combine(fullHomePath, "DiversityWorkbench");
        //            }
        //            else
        //            {
        //                throw new Exception("Environment variable error, there is no 'HOMEPATH'");
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Environment variable error, there is no 'HOMEDRIVE'");
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //    if (FolderPath.Length > 0)
        //    {
        //        System.IO.DirectoryInfo HomeDirectory = new System.IO.DirectoryInfo(FolderPath);
        //        if (HomeDirectory.Exists)
        //            FolderPath = HomeDirectory.FullName;
        //    }
        //    return FolderPath;
        //}

        //public static void setResourcesDirectory()
        //{
        //    System.Collections.Generic.Dictionary<string, string> D = new Dictionary<string, string>();
        //    D.Add(DiversityWorkbench.Settings.ResoucesDirectoryTypes.Application.ToString(), "Same directory as APPLICATION directory");
        //    D.Add(DiversityWorkbench.Settings.ResoucesDirectoryTypes.Home.ToString(), "HOME directory of the user");
        //    D.Add(DiversityWorkbench.Settings.ResoucesDirectoryTypes.MyDocuments.ToString(), "MY DOCUMENTS directory of the user");
        //    if (DiversityWorkbench.Settings.ResourcesDirectory.Length > 0 && !D.ContainsKey(DiversityWorkbench.Settings.ResourcesDirectory))
        //        D.Add(DiversityWorkbench.Settings.ResourcesDirectory, DiversityWorkbench.Settings.ResourcesDirectory);
        //    System.IO.DirectoryInfo MyDocuments = new System.IO.DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        //    string MyDocumentsPath = MyDocuments.FullName + "\\DiversityWorkbench"; // MyDocumentsDirectory();// MyDocuments.FullName;
        //    string Explanation = "Please select or enter the place for the resources for the application\r\n\r\n" +
        //        "APPLICATION directory: " + DiversityWorkbench.Settings.ApplicationDirectory() + "\r\n" +
        //        "HOME directory: " + DiversityWorkbench.Settings.HomeDirectory() + "\r\n" +
        //        "MY DOCUMENTS: " + MyDocumentsPath;
        //    if (DiversityWorkbench.Settings.ResourcesDirectory != null && DiversityWorkbench.Settings.ResourcesDirectory.Length > 0)
        //        Explanation += "\r\n\r\nCurrent directory: " + DiversityWorkbench.Settings.ResourcesDirectory;

        //    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(D, "Resources directory", Explanation, false);
        //    f.PresetSelection(DiversityWorkbench.Settings.ResourcesDirectory);
        //    f.Width = 600;
        //    f.ShowDialog();
        //    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
        //    {
        //        string Dir = "";
        //        if (f.SelectedValue != null && f.SelectedValue != "NULL")
        //            DiversityWorkbench.Settings.ResourcesDirectory = f.SelectedValue;
        //        else if (f.SelectedString != null)
        //        {
        //            Dir = f.SelectedString;
        //            System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(Dir);
        //            if (DI.Exists)
        //                DiversityWorkbench.Settings.ResourcesDirectory = Dir;
        //            else
        //                System.Windows.Forms.MessageBox.Show(Dir + " is not a valid directory");
        //        }
        //    }
        //}

        //public enum Directory { Archive, Printing, Import, Export, Transaction, Updates }
        //private static System.Collections.Generic.Dictionary<Directory, string> _Directories;
        //public static System.Collections.Generic.Dictionary<Directory, string> Directories()
        //{
        //    if (_Directories == null)
        //    {
        //        _Directories = new Dictionary<Directory, string>();
        //        _Directories.Add(Directory.Archive, DiversityWorkbench.Settings.ResourcesDirectory + "\\Archive");
        //        _Directories.Add(Directory.Printing, DiversityWorkbench.Settings.ResourcesDirectory + "\\LabelPrinting");
        //        _Directories.Add(Directory.Export, DiversityWorkbench.Settings.ResourcesDirectory + "\\Export");
        //        _Directories.Add(Directory.Import, DiversityWorkbench.Settings.ResourcesDirectory + "\\Import");
        //        _Directories.Add(Directory.Transaction, DiversityWorkbench.Settings.ResourcesDirectory + "\\Transaction");
        //    }
        //    return _Directories;
        //}

        //private static string GetDirectory(Directory Dir)
        //{
        //    System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(DiversityWorkbench.Settings.ResourcesDirectory + "\\" + Dir.ToString());
        //    if (!D.Exists)
        //    {
        //        System.IO.DirectoryInfo D_InApplication = new System.IO.DirectoryInfo(...Windows.Forms.Application.StartupPath + "\\" + Dir.ToString());
        //        if (D_InApplication.Exists)
        //        {
        //            string Message = "Should the directory " + D_InApplication.FullName + " be transferred to " + D.FullName + "?";
        //            if (System.Windows.Forms.MessageBox.Show(Message, "Transfer " + Dir.ToString(), System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
        //            {
        //                D_InApplication.MoveTo(D.FullName);
        //            }
        //        }
        //        if (!D.Exists)
        //            D.Create();
        //    }
        //    return D.FullName;
        //}

        //public static string HelpProviderNameSpace()
        //{
        //    string NameSpace = ...Windows.Forms.Application.StartupPath + "\\" + System.Windows.Forms.Application.ProductName.ToString() + ".chm";
        //    return NameSpace;
        //}

        #endregion

        #region Current user

        private static string _CurrentUserName = "";
        public static string CurrentUserName()
        {
            if (_CurrentUserName.Length == 0)
            {
                _CurrentUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                try
                {
                    string SQL = "SELECT [CombinedNameCache] FROM [dbo].[UserProxy] U WHERE U.LoginName = USER_NAME()";
                    string SqlUser = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (SqlUser.Length > 0)
                    {
                        _CurrentUserName = SqlUser;
                        SQL = "SELECT [AgentURI] FROM [dbo].[UserProxy] U WHERE U.LoginName = USER_NAME()";
                        _CurrentUserUri = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    }
                    else
                    {
                        SQL = "SELECT [CombinedNameCache] FROM [dbo].[UserProxy] U WHERE U.LoginName = SUSER_SNAME()";
                        SqlUser = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        _CurrentUserName = SqlUser;
                        SQL = "SELECT [AgentURI] FROM [dbo].[UserProxy] U WHERE U.LoginName = SUSER_SNAME()";
                        _CurrentUserUri = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    }
                }
                catch { }
            }
            return _CurrentUserName;
        }

        private static string _CurrentUserUri = "";
        public static string CurrentUserUri()
        {
            return _CurrentUserUri;
        }

        #endregion

        #region ErrorLogResetAtStartup
        public static bool ErrorLogResetAtStartup
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.ErrorLogResetAtStartup;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.ErrorLogResetAtStartup = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static bool GenerateTraceFile
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.GenerateTraceFile;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.GenerateTraceFile = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        #endregion

        #region Order by prefix
        public static void OrderByPrefixAdd(string Prefix)
        {
            if (DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix == null)
                DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix = new System.Collections.Specialized.StringCollection();
            if (!DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix.Contains(Prefix))
            {
                DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix.Add(Prefix);
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }


        public static void OrderByPrefixRemove(string Prefix)
        {
            if (DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix == null)
                DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix = new System.Collections.Specialized.StringCollection();
            if (DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix.Contains(Prefix))
            {
                DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix.Remove(Prefix);
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        #endregion

        #region Scanned modules

        public static bool ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType Type)
        {
            bool Scan = false;
            if (ScannedModules().ContainsKey(Type))
                Scan = true;
            return Scan;
        }

        public static void ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType Type, bool DoScan, string Path = "")
        {
            if (Path.Length == 0)
            {
                Path = ScannedModuleProgramPath(Type);
                if (Path == null)
                    Path = "";
            }
            if (DoScan)
            {
                if (!ScannedModules().ContainsKey(Type))
                {
                    ScannedModules().Add(Type, Path);
                }
                else
                {
                    ScannedModules()[Type] = Path;
                }
            }
            else
            {
                if (ScannedModules().ContainsKey(Type))
                    ScannedModules().Remove(Type);
            }
            ScannedModuleSave();
        }

        public static string ScannedModulePath(DiversityWorkbench.WorkbenchUnit.ModuleType Type)
        {
            if (ScannedModules().ContainsKey(Type))
                return ScannedModules()[Type];
            else
                return "";
        }

        public static void ScannedModuleSave()
        {
            if (DiversityWorkbench.WorkbenchSettings.Default.ScannedModules == null && ScannedModules().Count > 0)
            {
                DiversityWorkbench.WorkbenchSettings.Default.ScannedModules = new System.Collections.Specialized.StringCollection();
            }
            if (DiversityWorkbench.WorkbenchSettings.Default.ScannedModules != null)
            {
                DiversityWorkbench.WorkbenchSettings.Default.ScannedModules.Clear();
                foreach (System.Collections.Generic.KeyValuePair<DiversityWorkbench.WorkbenchUnit.ModuleType, string> KV in ScannedModules())
                {
                    DiversityWorkbench.WorkbenchSettings.Default.ScannedModules.Add(KV.Key.ToString() + "|" + KV.Value);
                }
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        //public static bool ScannedModuleRestrictToLocalServer
        //{
        //    get
        //    {
        //        return DiversityWorkbench.WorkbenchSettings.Default.ScannedModulesRestrictToLocalServer;
        //    }
        //    set
        //    {
        //        DiversityWorkbench.WorkbenchSettings.Default.ScannedModulesRestrictToLocalServer = value;
        //        DiversityWorkbench.WorkbenchSettings.Default.Save();
        //    }
        //}

        private static System.Collections.Generic.Dictionary<DiversityWorkbench.WorkbenchUnit.ModuleType, string> _ScannedModules;

        private static System.Collections.Generic.Dictionary<DiversityWorkbench.WorkbenchUnit.ModuleType, string> ScannedModules()
        {
            if (_ScannedModules == null)
            {
                _ScannedModules = new Dictionary<WorkbenchUnit.ModuleType, string>();
                if (DiversityWorkbench.WorkbenchSettings.Default.ScannedModules != null)
                {
                    //DiversityWorkbench.WorkbenchSettings.Default.ScannedModules.Clear();
                    foreach (string S in DiversityWorkbench.WorkbenchSettings.Default.ScannedModules)
                    {
                        string[] ss = S.Split(new char[] { '|' });
                        switch (ss[0])
                        {
                            case "Agents":
                                _ScannedModules.Add(WorkbenchUnit.ModuleType.Agents, ss[1]);
                                break;

                            case "Collection":
                                _ScannedModules.Add(WorkbenchUnit.ModuleType.Collection, ss[1]);
                                break;

                            case "Descriptions":
                                _ScannedModules.Add(WorkbenchUnit.ModuleType.Descriptions, ss[1]);
                                break;

                            case "Exsiccatae":
                                _ScannedModules.Add(WorkbenchUnit.ModuleType.Exsiccatae, ss[1]);
                                break;

                            case "Gazetteer":
                                _ScannedModules.Add(WorkbenchUnit.ModuleType.Gazetteer, ss[1]);
                                break;

                            case "Projects":
                                _ScannedModules.Add(WorkbenchUnit.ModuleType.Projects, ss[1]);
                                break;

                            case "References":
                                _ScannedModules.Add(WorkbenchUnit.ModuleType.References, ss[1]);
                                break;

                            case "SamplingPlots":
                                _ScannedModules.Add(WorkbenchUnit.ModuleType.SamplingPlots, ss[1]);
                                break;

                            case "ScientificTerms":
                                _ScannedModules.Add(WorkbenchUnit.ModuleType.ScientificTerms, ss[1]);
                                break;

                            case "TaxonNames":
                                _ScannedModules.Add(WorkbenchUnit.ModuleType.TaxonNames, ss[1]);
                                break;
                        }
                    }
                }
            }
            return _ScannedModules;
        }

        private static string ScannedModuleProgramPath(DiversityWorkbench.WorkbenchUnit.ModuleType Type)
        {
            // funktioniert alles nicht - evtl. Eintrag in Registry noetig (per Installer)
            try
            {
                string fileName = "Diversity" + Type.ToString() + ".exe";
                Microsoft.Win32.RegistryKey localMachine = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey fileKey = localMachine.OpenSubKey(string.Format(@"{0}\{1}", @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths", fileName));
                //Microsoft.Win32.RegistryKey fileKey = localMachine.OpenSubKey(string.Format(@"{0}\{1}", @"SOFTWARE\DiversityWorkbench Paths", fileName));
                if (fileKey == null)
                    fileKey = localMachine.OpenSubKey(string.Format(@"{0}\{1}", @"Software\Microsoft\Installer\Assemblies\C:", fileName));
                if (fileKey == null)
                    fileKey = localMachine.OpenSubKey(string.Format(@"{0}\{1}", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData", fileName));
                if (fileKey == null)
                {
                    string Directory = @"C:\Program Files (x86)\DiversityWorkbench\Diversity" + Type.ToString();
                    Microsoft.Win32.RegistryKey dir = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\Folders");// localMachine.OpenSubKey(string.Format(@"{0}\{1}", @"Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\Folders", Directory));
                    fileKey = localMachine.OpenSubKey(string.Format(@"{0}\{1}", @"Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\Folders", fileName));
                }
                object result = null;
                if (fileKey != null)
                {
                    result = fileKey.GetValue(string.Empty);
                    fileKey.Close();
                }
                else
                {
                    String path = Environment.GetEnvironmentVariable("path");
                    String[] folders = path.Split(';');
                    foreach (String folder in folders)
                    {
                        if (System.IO.File.Exists(folder + fileName))
                        {
                            return folder + fileName;
                        }
                        else if (System.IO.File.Exists(folder + "\\" + fileName))
                        {
                            return folder + "\\" + fileName;
                        }
                    }
                }
                if (result != null)
                    return (string)result;
                else
                    return "";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return "";
        }

        #endregion

        #region ToolTip

        public static int ToolTipAutoPopDelay
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.ToolTipAutoPopDelay;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.ToolTipAutoPopDelay = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        #endregion

    }
}
