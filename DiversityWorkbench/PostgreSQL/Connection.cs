using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace DiversityWorkbench.PostgreSQL
{
    public class Connection
    {
        #region Parameter

        private static System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Server> _PreviousConnections;
        public static string Password;

        #endregion

        #region Previous connections

        public static void UpgradeSettings()
        {
            DiversityWorkbench.PostgreSQL.Settings.Default.Upgrade();
            DiversityWorkbench.PostgreSQL.Settings.Default.Reload();
            DiversityWorkbench.PostgreSQL.Settings.Default.Save();
        }

        public static System.Collections.Specialized.StringCollection PreviousConnections()
        {
            if (DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList == null)
                DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList = new System.Collections.Specialized.StringCollection();
            return DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList;
        }

        public static void AddConnection(string Server, int Port, string Database, string User)
        {
            string NewConnection = Server + "|" + Port.ToString() + "|" + Database + "|" + User;
            if (DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList == null)
                DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList = new System.Collections.Specialized.StringCollection();
            if (!DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Contains(NewConnection))
                DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Add(NewConnection);
            DiversityWorkbench.PostgreSQL.Connection._PreviousConnections = null;
        }

        public static void AddPreviousConnection(string Server, int Port, string User)
        {
            string NewConnection = Server + "|" + Port.ToString() + "|" + User;
            if (DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList == null)
                DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList = new System.Collections.Specialized.StringCollection();
            if (!DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Contains(NewConnection))
            {
                DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Add(NewConnection);
                DiversityWorkbench.PostgreSQL.Settings.Default.Save();
            }
            DiversityWorkbench.PostgreSQL.Connection._PreviousConnections = null;
        }

        public static void AddPreviousConnection(string Server, int Port)
        {
            string NewConnection = Server + "|" + Port.ToString();
            if (DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList == null)
                DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList = new System.Collections.Specialized.StringCollection();
            if (!DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Contains(NewConnection))
                DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Add(NewConnection);
            DiversityWorkbench.PostgreSQL.Connection._PreviousConnections = null;
        }

        public static void RemoveConnection(string Server, int Port, string Database, string User)
        {
            string Connection = Server + "|" + Port.ToString() + "|" + Database + "|" + User;
            if (DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Contains(Connection))
                DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Remove(Connection);
            DiversityWorkbench.PostgreSQL.Connection._PreviousConnections = null;
        }

        #endregion

        #region Current connection

        private static string _DefaultConnectionString = "";

        public static string DefaultConnectionString(bool OptimizeForBulkOperations = false)
        {
            if (!DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted && (DiversityWorkbench.PostgreSQL.Connection.Password == null || DiversityWorkbench.PostgreSQL.Connection.Password.Length == 0))
                return "";
            Npgsql.NpgsqlConnection con = null;
            if (_DefaultConnectionString.Length == 0)
            {
                try
                {
                    _DefaultConnectionString = "";
                    //if (DiversityWorkbench.Settings.TimeoutDatabase < 1024)
                    ////    _DefaultConnectionString = "TIMEOUT=0;";
                    ////else
                    //    _DefaultConnectionString = "TIMEOUT=" + DiversityWorkbench.Settings.TimeoutDatabase.ToString() + ";";
                    if (!DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted)
                        _DefaultConnectionString += "User ID=" + DiversityWorkbench.PostgreSQL.Settings.Default.Role + ";Password=" + DiversityWorkbench.PostgreSQL.Connection.Password + ";";
                    _DefaultConnectionString += "Host=" + DiversityWorkbench.PostgreSQL.Settings.Default.Server + ";Port=" + DiversityWorkbench.PostgreSQL.Settings.Default.Port.ToString() + ";";
                    if (DiversityWorkbench.PostgreSQL.Settings.Default.Database.Length > 0)
                        _DefaultConnectionString += "Database=" + DiversityWorkbench.PostgreSQL.Settings.Default.Database + ";";
                    else
                        _DefaultConnectionString += "Database=postgres;";
                    if (DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted)
                        _DefaultConnectionString += "Integrated Security=true;";
                    /// TODO: Pooling seems not to work
                    _DefaultConnectionString += "Pooling=false;";
                    _DefaultConnectionString += "CommandTimeout=" + DiversityWorkbench.Settings.TimeoutDatabase.ToString() + ";";// 0;";
                    // Markus 25.5.2020: Test - leave without timeout
                    // Markus 2024-05-06: use 0 for infinity and using TimeoutConnection
                    if (DiversityWorkbench.Settings.TimeoutConnection == 0)// && false)
                        _DefaultConnectionString += "Timeout=" + DiversityWorkbench.Settings.TimeoutConnection.ToString(); // 1000;";
                    // End of Test
                    if (OptimizeForBulkOperations)
                        _DefaultConnectionString += "Protocol=-1;UseServerSidePrepare=1;";

                    con = new Npgsql.NpgsqlConnection(_DefaultConnectionString);
                    bool OK = false;
                    try
                    {
                        con.Open();
                        OK = true;
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.Message.ToLower() == "timeout while getting a connection from pool.")
                            DiversityWorkbench.PostgreSQL.Connection._ConnectionPoolingFailed = true;
                        _DefaultConnectionString = _DefaultConnectionString.Replace(";Pooling=true;", ";Pooling=false;");
                        con.ConnectionString = _DefaultConnectionString;
                    }
                    if (!OK)
                        con.Open();
                    DiversityWorkbench.PostgreSQL.Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLower() == "timeout while getting a connection from pool.")
                        DiversityWorkbench.PostgreSQL.Connection._ConnectionPoolingFailed = true;
                    if (con.State != ConnectionState.Open)
                        _DefaultConnectionString = "";
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return _DefaultConnectionString;
        }

        public static string DefaultConnectionString(int Timeout)
        {
            if (!DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted && (DiversityWorkbench.PostgreSQL.Connection.Password == null || DiversityWorkbench.PostgreSQL.Connection.Password.Length == 0))
                return "";
            Npgsql.NpgsqlConnection con = null;
            if (_DefaultConnectionString.Length == 0)
            {
                try
                {
                    _DefaultConnectionString = "";
                    //if (DiversityWorkbench.Settings.TimeoutDatabase < 1024)
                    ////    _DefaultConnectionString = "TIMEOUT=0";
                    ////else
                    //    _DefaultConnectionString = "TIMEOUT=" + DiversityWorkbench.Settings.TimeoutDatabase.ToString();
                    if (!DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted)
                        _DefaultConnectionString += "User ID=" + DiversityWorkbench.PostgreSQL.Settings.Default.Role + ";Password=" + DiversityWorkbench.PostgreSQL.Connection.Password + ";";
                    _DefaultConnectionString += "Host=" + DiversityWorkbench.PostgreSQL.Settings.Default.Server + ";Port=" + DiversityWorkbench.PostgreSQL.Settings.Default.Port.ToString() + ";";
                    if (DiversityWorkbench.PostgreSQL.Settings.Default.Database.Length > 0)
                        _DefaultConnectionString += "Database=" + DiversityWorkbench.PostgreSQL.Settings.Default.Database + ";";
                    else
                        _DefaultConnectionString += "Database=postgres;";
                    if (DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted)
                        _DefaultConnectionString += "Integrated Security=true;";
                    _DefaultConnectionString += "Pooling=false;";
                    if (Timeout > 1024)
                        _DefaultConnectionString += "CommandTimeout=0;";
                    else
                        _DefaultConnectionString += "CommandTimeout=" + Timeout.ToString() + ";";
                    con = new Npgsql.NpgsqlConnection(_DefaultConnectionString);
                    bool OK = false;
                    try
                    {
                        con.Open();
                        OK = true;
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.Message.ToLower() == "timeout while getting a connection from pool.")
                            DiversityWorkbench.PostgreSQL.Connection._ConnectionPoolingFailed = true;
                        _DefaultConnectionString = _DefaultConnectionString.Replace(";Pooling=true;", ";Pooling=false;");
                        con.ConnectionString = _DefaultConnectionString;
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, _DefaultConnectionString);
                    }
                    if (!OK)
                        con.Open();
                    DiversityWorkbench.PostgreSQL.Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLower() == "timeout while getting a connection from pool.")
                        DiversityWorkbench.PostgreSQL.Connection._ConnectionPoolingFailed = true;
                    if (con.State != ConnectionState.Open)
                        _DefaultConnectionString = "";
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return _DefaultConnectionString;
        }

        public static string DatabaseConnectionString(string Database)
        {
            if (!DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted && (DiversityWorkbench.PostgreSQL.Connection.Password == null || DiversityWorkbench.PostgreSQL.Connection.Password.Length == 0))
                return "";
            Npgsql.NpgsqlConnection con = null;
            string DatabaseConnectionString = "";
            try
            {
                if (!DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted)
                    DatabaseConnectionString = "User ID=" + DiversityWorkbench.PostgreSQL.Settings.Default.Role + ";Password=" + DiversityWorkbench.PostgreSQL.Connection.Password + ";";
                DatabaseConnectionString += "Host=" + DiversityWorkbench.PostgreSQL.Settings.Default.Server + ";Port=" + DiversityWorkbench.PostgreSQL.Settings.Default.Port.ToString() + ";";
                if (DiversityWorkbench.PostgreSQL.Settings.Default.Database.Length > 0)
                    DatabaseConnectionString += "Database=" + Database + ";";
                else
                    DatabaseConnectionString += "Database=postgres;";
                if (DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted)
                    DatabaseConnectionString += "Integrated Security=true;";
                DatabaseConnectionString += "Pooling=false;";
                if (DiversityWorkbench.Settings.TimeoutConnection > -1)
                {
                    DatabaseConnectionString += "Timeout=" + DiversityWorkbench.Settings.TimeoutConnection.ToString() + ";";
                }
                if (DiversityWorkbench.Settings.TimeoutDatabase > -1)
                {
                    DatabaseConnectionString += "CommandTimeout=" + DiversityWorkbench.Settings.TimeoutConnection.ToString() + ";";
                }
                con = new Npgsql.NpgsqlConnection(DatabaseConnectionString);
                bool OK = false;
                try
                {
                    con.Open();
                    OK = true;
                }
                catch (System.Exception ex)
                {
                    if (ex.Message.ToLower() == "timeout while getting a connection from pool.")
                        DiversityWorkbench.PostgreSQL.Connection._ConnectionPoolingFailed = true;
                    DatabaseConnectionString = DatabaseConnectionString.Replace(";Pooling=true;", ";Pooling=false;");
                    con.ConnectionString = DatabaseConnectionString;
                }
                if (!OK)
                    con.Open();
            }
            catch (Exception ex)
            {
                if (con.State != ConnectionState.Open)
                    DatabaseConnectionString = "";
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return DatabaseConnectionString;
        }

        private static bool _ConnectionPoolingFailed = false;

        public static void ResetDefaultConnectionString()
        {
            _DefaultConnectionString = "";
        }

        public static string CurrentServerKey()
        {
            string Key = DiversityWorkbench.PostgreSQL.Settings.Default.Server + "|" + DiversityWorkbench.PostgreSQL.Settings.Default.Port.ToString();
            return Key;
        }

        public static DiversityWorkbench.PostgreSQL.Server _CurrentServer;
        public static DiversityWorkbench.PostgreSQL.Server CurrentServer()
        {
            if (DiversityWorkbench.PostgreSQL.Connection._CurrentServer == null)
            {
                DiversityWorkbench.PostgreSQL.Connection._CurrentServer = new Server(DiversityWorkbench.PostgreSQL.Settings.Default.Server, DiversityWorkbench.PostgreSQL.Settings.Default.Port);
            }
            return DiversityWorkbench.PostgreSQL.Connection._CurrentServer;
        }

        public static int CurrentPort()
        {
            return DiversityWorkbench.PostgreSQL.Settings.Default.Port;
        }

        public static string CurrentRole()
        {
            return DiversityWorkbench.PostgreSQL.Settings.Default.Role;
        }

        public static string CurrentConnectionDisplayText()
        {
            string Connection = DiversityWorkbench.PostgreSQL.Settings.Default.Database + " on " + DiversityWorkbench.PostgreSQL.Settings.Default.Server + ", " + DiversityWorkbench.PostgreSQL.Settings.Default.Port.ToString() + " as " + DiversityWorkbench.PostgreSQL.Settings.Default.Role;
            return Connection;
        }

        public static DiversityWorkbench.PostgreSQL.Database CurrentDatabase()
        {
            if (DiversityWorkbench.PostgreSQL.Connection.Databases().ContainsKey(DiversityWorkbench.PostgreSQL.Settings.Default.Database))
                return DiversityWorkbench.PostgreSQL.Connection.Databases()[DiversityWorkbench.PostgreSQL.Settings.Default.Database];
            return null;
        }

        public static bool SetCurrentDatabase(string Database)
        {
            bool OK = true;
            try
            {
                DiversityWorkbench.PostgreSQL.Connection.ResetDefaultConnectionString();
                DiversityWorkbench.PostgreSQL.Settings.Default.Database = Database;
                DiversityWorkbench.PostgreSQL.Settings.Default.Save();
                _PostgresConnection = null;
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        public static bool SetCurrentServer(string Server)
        {
            bool OK = true;
            try
            {
                /// TODO - Test resetting Server, 29.8.2017
                if (DiversityWorkbench.PostgreSQL.Connection._CurrentServer != null &&
                    Server != DiversityWorkbench.PostgreSQL.Connection._CurrentServer.Name)
                {
                    DiversityWorkbench.PostgreSQL.Connection.ResetDefaultConnectionString();
                    DiversityWorkbench.PostgreSQL.Connection._CurrentServer = null;
                }

                DiversityWorkbench.PostgreSQL.Settings.Default.Server = Server;
                DiversityWorkbench.PostgreSQL.Settings.Default.Save();
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        public static bool SetCurrentPort(int Port)
        {
            bool OK = true;
            try
            {
                DiversityWorkbench.PostgreSQL.Settings.Default.Port = Port;
                DiversityWorkbench.PostgreSQL.Settings.Default.Save();
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentServer() != null &&
                    DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port != Port)
                    DiversityWorkbench.PostgreSQL.Connection._CurrentServer = null;
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        public static bool SetCurrentRole(string Role)
        {
            bool OK = true;
            try
            {
                DiversityWorkbench.PostgreSQL.Settings.Default.Role = Role;
                DiversityWorkbench.PostgreSQL.Settings.Default.Save();
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        public static string ConnectionString(string Database)
        {
            string ConnStr = "";
            if (ConnStr.Length == 0)
            {
                try
                {
                    ConnStr = "User ID=" + DiversityWorkbench.PostgreSQL.Settings.Default.Role +
                        ";Password=" + DiversityWorkbench.PostgreSQL.Connection.Password +
                        ";Host=" + DiversityWorkbench.PostgreSQL.Settings.Default.Server +
                        ";Port=" + DiversityWorkbench.PostgreSQL.Settings.Default.Port.ToString() +
                        ";Database=" + Database + ";" +
                        "Pooling=false";
                    Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnStr);
                    con.Open();
                    con.Close();
                    con.Dispose();
                }
                catch (Exception ex)
                {
                    ConnStr = "";
                }
            }
            return ConnStr;
        }

        public static string ConnectionString(string Database, ref string ExceptionMessage)
        {
            string ConnStr = "";
            if (ConnStr.Length == 0)
            {
                try
                {
                    ConnStr = "User ID=" + DiversityWorkbench.PostgreSQL.Settings.Default.Role +
                        ";Password=" + DiversityWorkbench.PostgreSQL.Connection.Password +
                        ";Host=" + DiversityWorkbench.PostgreSQL.Settings.Default.Server +
                        ";Port=" + DiversityWorkbench.PostgreSQL.Settings.Default.Port.ToString() +
                        ";Database=" + Database + ";" +
                        "Pooling=false";
                    Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnStr);
                    con.Open();
                    con.Close();
                    con.Dispose();
                }
                catch (Exception ex)
                {
                    ConnStr = "";
                    ExceptionMessage = ex.Message;
                    if (ExceptionMessage.IndexOf("f�r") > -1)
                    {
                        ExceptionMessage = ExceptionMessage.Replace("f�r", "für");
                    }
                    if (ExceptionMessage.IndexOf("�") > -1)
                    {
                        ExceptionMessage = ExceptionMessage.Replace("�", "'");
                    }

                }
            }
            return ConnStr;
        }


        public static string TimeoutTest(int Seconds = 60)
        {
            string Message = "OK";
            {
                string SQL = "SELECT SLEEP(" + Seconds.ToString() + ");";
            }
            return Message;
        }

        #endregion

        #region Roles

        private static System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Role> _Roles;

        public static System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Role> Roles()
        {
            if (DiversityWorkbench.PostgreSQL.Connection._Roles == null || DiversityWorkbench.PostgreSQL.Connection._Roles.Count == 0)
            {
                _Roles = new Dictionary<string, Role>();
                try
                {
                    string SQL = "SELECT rolname, rolsuper, rolcanlogin, rolcreaterole, rolcreatedb, rolinherit FROM pg_roles order by rolname;";
                    System.Data.DataTable dtRoles = new DataTable();
                    string Message = "";
                    if (!DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtRoles, ref Message))
                        System.Windows.Forms.MessageBox.Show(Message);
                    foreach (System.Data.DataRow R in dtRoles.Rows)
                    {
                        DiversityWorkbench.PostgreSQL.Role Role = new Role(R[0].ToString());
                        Role.IsSuperuser = bool.Parse(R[1].ToString());
                        Role.CanLogin = bool.Parse(R[2].ToString());
                        Role.CanCreateRoles = bool.Parse(R[3].ToString());
                        Role.CanCreateDatabase = bool.Parse(R[4].ToString());
                        Role.Inherit = bool.Parse(R[5].ToString());
                        DiversityWorkbench.PostgreSQL.Connection._Roles.Add(Role.Name, Role);
                    }
                }
                catch (System.Exception ex)
                { }
            }
            return DiversityWorkbench.PostgreSQL.Connection._Roles;
        }

        public static System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Role> Groups()
        {
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Role> GG = new Dictionary<string, Role>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Role> KV in DiversityWorkbench.PostgreSQL.Connection.Roles())
            {
                if (!KV.Value.CanLogin)
                    GG.Add(KV.Key, KV.Value);
            }
            return GG;
        }

        public static System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Role> Logins()
        {
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Role> LL = new Dictionary<string, Role>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Role> KV in DiversityWorkbench.PostgreSQL.Connection.Roles())
            {
                if (KV.Value.CanLogin)
                    LL.Add(KV.Key, KV.Value);
            }
            return LL;
        }


        public static void ResetRoles()
        {
            DiversityWorkbench.PostgreSQL.Connection._Roles = null;
        }

        public static bool AddGroup(string Name)
        {
            bool OK = true;
            string SQL = "CREATE ROLE \"" + Name + "\" NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION;";
            string Message = "";
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            if (!OK)
                System.Windows.Forms.MessageBox.Show(Message);
            return OK;
        }

        public static bool AddLogin(string Name)
        {
            bool OK = true;
            DiversityWorkbench.Forms.FormGetPassword f = new Forms.FormGetPassword(Name);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string SQL = "CREATE ROLE \"" + Name + "\" LOGIN NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION PASSWORD '" + f.Password() + "' ;";
                string Message = "";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
                if (!OK)
                    System.Windows.Forms.MessageBox.Show(Message);
            }
            return OK;
        }

        public static bool DeleteRole(string Name)
        {
            bool OK = true;
            string SQL = "DROP ROLE \"" + Name + "\";";
            string Message = "";
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            if (!OK)
                System.Windows.Forms.MessageBox.Show(Message);
            return OK;
        }

        /// <summary>
        /// Try to set the default role for the collection to either CacheAdmin or CacheUser according to the availble permissions
        /// </summary>
        /// <returns></returns>
        public static bool SetRoleWithMaxPermission()
        {
            bool OK = true;
            try
            {
                List<string> pgRoles = new List<string>();
                string msg = "";
                string SQL = "select role_name " +
                             "from information_schema.applicable_roles";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref msg);
                foreach (System.Data.DataRow R in dt.Rows)
                    pgRoles.Add(R[0].ToString());

                if (pgRoles.Contains("CacheAdmin"))
                    SQL = "SET ROLE \"CacheAdmin\";";
                else if (pgRoles.Contains("CacheUser"))
                    SQL = "SET ROLE \"CacheUser\";";
                else
                    SQL = "";

                if (SQL != "")
                {
                    if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref msg, true, false))
                    {
                        System.Windows.Forms.MessageBox.Show(msg, "Failed to set role", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        OK = false;
                    }
                }
                else
                    OK = false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }

            return OK;
        }

        #endregion

        #region Databases

        private static System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Database> _Databases;

        public static System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Database> Databases(string Module = "")
        {
            try
            {
                if (DiversityWorkbench.PostgreSQL.Connection._Databases == null)
                {
                    _Databases = new Dictionary<string, Database>();
                }
                if (DiversityWorkbench.PostgreSQL.Connection._Databases.Count == 0)
                {
                    if (DiversityWorkbench.PostgreSQL.Connection.CurrentServer() != null ||
                        (DiversityWorkbench.PostgreSQL.Connection.Password != null &&
                        DiversityWorkbench.PostgreSQL.Connection.Password.Length > 0))
                    {
                        try
                        {
                            string SQL = "SELECT datname FROM pg_database WHERE datname not like 'template%' AND datname <> 'postgres' ORDER BY datname";
                            System.Data.DataTable dtDatabases = new DataTable();
                            string Message = "";
                            if (!DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtDatabases, ref Message))
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("DiversityWorkbench.PostgreSQL.Databases()", SQL, Message);
                                //System.Windows.Forms.MessageBox.Show(Message);
                            }
                            if (dtDatabases.Rows.Count == 0)
                            {
                                SQL = "SELECT datname FROM pg_database WHERE datname not like 'template%' ORDER BY datname";
                                if (!DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtDatabases, ref Message))
                                {
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("DiversityWorkbench.PostgreSQL.Databases()", SQL, Message);
                                    //System.Windows.Forms.MessageBox.Show(Message);
                                }
                            }
                            foreach (System.Data.DataRow R in dtDatabases.Rows)
                            {
                                string ConnectionString = DiversityWorkbench.PostgreSQL.Connection.DatabaseConnectionString(R[0].ToString());
                                if (ConnectionString.IndexOf("Database=" + R[0].ToString()) == -1 && ConnectionString.IndexOf("Database=postgres") > -1)
                                    ConnectionString = ConnectionString.Replace("Database=postgres", "Database=" + R[0].ToString());
                                if (ConnectionString.Length == 0)
                                    continue;
                                // testing existence of function diversityworkbenchmodule
                                SQL = "SELECT count(*) from information_schema.routines " +
                                    "where routine_type = 'FUNCTION' " +
                                    "and routine_name = 'diversityworkbenchmodule' " +
                                    "and specific_schema = 'public';";
                                string Result = "";
                                try
                                {
                                    Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ConnectionString);
                                }
                                catch (Exception) { }
                                if (Result.Length == 0 || Result == "0")
                                    continue;
                                if (!R[0].Equals(System.DBNull.Value))
                                {
                                    if (Module != "")
                                    {
                                        // Check module value
                                        SQL = "SELECT public.diversityworkbenchmodule();";
                                        Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ConnectionString);
                                        if (Result == Module)
                                        {
                                            DiversityWorkbench.PostgreSQL.Database D = new Database(R[0].ToString());
                                            DiversityWorkbench.PostgreSQL.Connection._Databases.Add(R[0].ToString(), D);
                                        }
                                    }
                                    else
                                    {
                                        DiversityWorkbench.PostgreSQL.Database D = new Database(R[0].ToString());
                                        DiversityWorkbench.PostgreSQL.Connection._Databases.Add(R[0].ToString(), D);
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
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return DiversityWorkbench.PostgreSQL.Connection._Databases;
        }

        public static void ResetDatabases()
        {
            DiversityWorkbench.PostgreSQL.Connection._Databases = null;
        }

        public static void RestrictDatabasesToDiversityWorkbenchModule(string ModuleName)
        {
            System.Collections.Generic.List<string> DbToRemove = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Database> KV in DiversityWorkbench.PostgreSQL.Connection._Databases)
            {
                DbToRemove.Add(KV.Key);
            }
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Database> KV in DiversityWorkbench.PostgreSQL.Connection._Databases)
            {
                string SQL = "select public.diversityworkbenchmodule()";
                string Module = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, DiversityWorkbench.PostgreSQL.Connection.ConnectionString(KV.Key));
                if (Module == ModuleName)
                {
                    SQL = "select public.version()";
                    string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, DiversityWorkbench.PostgreSQL.Connection.ConnectionString(KV.Key));
                    if (Result != "")
                        DbToRemove.Remove(KV.Key);
                }
            }
            foreach (string DB in DbToRemove)
                DiversityWorkbench.PostgreSQL.Connection._Databases.Remove(DB);
        }

        #endregion

        #region SQL

        private static Npgsql.NpgsqlConnection _NpgsqlConnection;
        private static Npgsql.NpgsqlCommand _NpgsqlCommand;
        public static bool SqlExecuteNonQueryWithSameConnection(string SQL, ref string Message, bool Retry = false)
        {
            bool OK = false;
            try
            {
                if (_NpgsqlConnection == null)
                {
                    if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)// .Postgres.PostgresConnection() != null)
                    {
                        _NpgsqlConnection = new NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                        if (_NpgsqlConnection.State == ConnectionState.Closed)
                            _NpgsqlConnection.Open();
                    }
                }
                if (_NpgsqlCommand == null)
                {
                    _NpgsqlCommand = new NpgsqlCommand(SQL, _NpgsqlConnection);// .Postgres.PostgresConnection());
                    _NpgsqlCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                }
                else
                    _NpgsqlCommand.CommandText = SQL;
                _NpgsqlCommand.ExecuteNonQuery();
                OK = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.Message.ToLower() == "timeout while getting a connection from pool.")
                {
                    DiversityWorkbench.PostgreSQL.Connection._ConnectionPoolingFailed = true;
                    DiversityWorkbench.PostgreSQL.Connection._DefaultConnectionString = "";
                }
                else if (ConnectionFailure(ex) && !Retry)
                {
                    Message = "";
                    SqlExecuteNonQueryCloseConnection();
                    SqlExecuteNonQueryWithSameConnection(SQL, ref Message, true);
                }
                else
                {
                    if (Message.Length > 0)
                        Message += ": ";
                    Message += SQL;
                    if (ex is PostgresException)
                    {
                        PostgresException npgsqlException = (PostgresException)ex;
                        if (npgsqlException.StackTrace != null && npgsqlException.StackTrace.Length > 0)
                        {
                            Message += "\r\n" + npgsqlException.StackTrace;
                        }
                    }
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, Message);
                }
            }
            return OK;
        }

        public static void SqlExecuteNonQueryCloseConnection()
        {
            if (_NpgsqlConnection != null)
            {
                try
                {
                    if (_NpgsqlConnection.State == ConnectionState.Open)
                        _NpgsqlConnection.Close();
                    _NpgsqlConnection.Dispose();
                    _NpgsqlConnection = null;
                }
                catch { }
            }
            if (_NpgsqlCommand != null)
            {
                try
                {
                    _NpgsqlCommand.Dispose();
                    _NpgsqlCommand = null;
                }
                catch { }
            }
        }

        public static bool SqlExecuteNonQuery(string SQL, bool Retry = false)
        {
            bool OK = false;
            try
            {
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)// .Postgres.PostgresConnection() != null)
                {
                    Npgsql.NpgsqlConnection con = new NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    Npgsql.NpgsqlCommand C = new NpgsqlCommand(SQL, con);// .Postgres.PostgresConnection());
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    C.ExecuteNonQuery();
                    C.Dispose();
                }
                OK = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower() == "timeout while getting a connection from pool.")
                {
                    DiversityWorkbench.PostgreSQL.Connection._ConnectionPoolingFailed = true;
                    DiversityWorkbench.PostgreSQL.Connection._DefaultConnectionString = "";
                }
                else if (ConnectionFailure(ex) && !Retry)
                {
                    SqlExecuteNonQuery(SQL, true);
                }
                else
                {
                    HandleException(ex, SQL);
                    //string Message = SQL;
                    //if (ex.GetType() == typeof(NpgsqlException))
                    //{
                    //    NpgsqlException npgsqlException = (NpgsqlException)ex;
                    //    if (npgsqlException.Detail != null && npgsqlException.Detail.Length > 0)
                    //        Message += "\r\n" + npgsqlException.Detail;
                    //}
                    //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, Message);
                }
            }
            return OK;
        }

        public static bool SqlExecuteNonQuery(
            string SQL,
            ref string Message,
            int Timeout)
        {
            return SqlExecuteNonQuery(SQL, ref Message, true, true, false, false, "", Timeout);
        }

        public static bool SqlExecuteNonQuery(
        string SQL,
        ref string Message,
        bool UseDefaultConnection = true,
        bool IncludeInTransaction = true,
        bool OptimizeForBulkOperations = false,
        bool Retry = false,
        string Role = "",
        int Timeout = -1)
        {
            //Microsoft.Data.SqlClient.SqlTransaction t;

            bool OK = false;
            NpgsqlConnection con = null;
            Npgsql.NpgsqlCommand C = null;
            NpgsqlTransaction T = null;
            try
            {
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)// .Postgres.PostgresConnection() != null)
                {
                    // Markus 20.5.2019 - über zentrale Verbindung
                    if (UseDefaultConnection)
                    {
                        if (IncludeInTransaction) // Toni 20200226
                        {
                            T = PostgresConnection().BeginTransaction();
                        }
                        C = new NpgsqlCommand(SQL, PostgresConnection(), T);
                        C.CommandTimeout = Timeout >= 0 ? Timeout : DiversityWorkbench.Settings.TimeoutDatabase;
                        C.ExecuteNonQuery();
                    }
                    else
                    {
                        con = new NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                        if (IncludeInTransaction) // Toni 20200226
                            T = con.BeginTransaction();
                        C = new NpgsqlCommand(SQL, con, T);// .Postgres.PostgresConnection());
                        C.CommandTimeout = Timeout >= 0 ? Timeout : DiversityWorkbench.Settings.TimeoutDatabase;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        C.ExecuteNonQuery();
                    }
                }
                if (T != null && !SQL.EndsWith("COMMIT;")) // Markus 11.2.2021 - Wenn SQL bereits Commit enthaelt kein weiteres Commit mehr möglich
                {
                    T.Commit();
                }
                OK = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (T != null)
                    T.Rollback();
                if (ex.Message.ToLower() == "timeout while getting a connection from pool.")
                {
                    DiversityWorkbench.PostgreSQL.Connection._ConnectionPoolingFailed = true;
                    DiversityWorkbench.PostgreSQL.Connection._DefaultConnectionString = "";
                }
                else if (ConnectionFailure(ex) && !Retry)
                {
                    Message = "";
                    SqlExecuteNonQuery(SQL, ref Message, UseDefaultConnection, IncludeInTransaction, OptimizeForBulkOperations, true);
                }
                else if (ex.Message.ToLower().IndexOf("must be member of role") > -1 || ex.Message.ToLower().IndexOf("must be superuser or a member of") > -1 && !Retry && Role.Length > 0)
                {
                    SQL = "SET ROLE \"" + Role + "\"; " + SQL;
                    SqlExecuteNonQuery(SQL, ref Message, UseDefaultConnection, IncludeInTransaction, OptimizeForBulkOperations, true);
                }
                else
                {
                    HandleException(ex, SQL);
                }
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con.Dispose();
                    con = null;
                }
                if (C != null)
                {
                    C.Dispose();
                    C = null;
                }
            }

            return OK;
        }

        public enum ObjectType { Table, View, Function, Schema }
        public static string ObjectOwner(
            string Name,
            string Schema,
            ObjectType Type)
        {
            string Owner = "";
            string SQL = "";
            switch (Type)
            {
                case ObjectType.Function:
                    SQL = "select proowner from pg_proc where proname = '" + Name + "' and schemaname = '" + Schema + "';";
                    break;
                case ObjectType.Schema:
                    SQL = "select schema_owner from information_schema.schemata where schema_name = '" + Schema + "'; ";
                    break;
                case ObjectType.Table:
                    SQL = "select tableowner from pg_tables where tablename = '" + Name + "' and schemaname = '" + Schema + "';";
                    break;
                case ObjectType.View:
                    SQL = "select viewowner from pg_views where viewname = '" + Name + "' and schemaname = '" + Schema + "';";
                    break;
            }
            Owner = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
            return Owner;
        }

        private static NpgsqlConnection _PostgresConnection;
        private static NpgsqlConnection PostgresConnection()
        {
            if (_PostgresConnection == null)
            {
                try
                {
                    _PostgresConnection = new NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    //if (_PostgresConnection.State == ConnectionState.Closed)
                    //    _PostgresConnection.Open();
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            try // Markus 12.2.2021
            {
                if (_PostgresConnection.State == ConnectionState.Closed)
                    _PostgresConnection.Open();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return _PostgresConnection;
        }

        public static void PostgresConnectionClose()
        {
            PostgresConnection().Close();
            _PostgresConnection.Dispose();
        }

        public static string SqlExecuteSkalar(string SQL, bool Retry = false, bool IgnoreException = false)
        {
            string Result = "";
            Npgsql.NpgsqlConnection con = null;
            Npgsql.NpgsqlCommand C = null;
            try
            {
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                {
                    con = new NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    C = new NpgsqlCommand(SQL, con);
                    con.Open();
                    Result = C.ExecuteScalar()?.ToString();
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (!IgnoreException)
                    HandleException(ex, SQL);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            catch (Exception ex)
            {
                if (ConnectionFailure(ex) && !Retry)
                {
                    SqlExecuteSkalar(SQL, true, IgnoreException);
                }
                else if (!IgnoreException)
                    HandleException(ex, SQL);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            finally
            {
                if (C != null)
                    C.Dispose();
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                if (con != null)
                    con.Dispose();
            }
            return Result;
        }

        // Markus 2.4.25: Option to check count added
        public static string SqlExecuteSkalar(string SQL, ref string Message, bool Retry = false, bool CheckCount = false)
        {
            string Result = "";
            Npgsql.NpgsqlConnection con = null;
            Npgsql.NpgsqlCommand C = null;
            try
            {
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                {
                    con = new NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    con.Open();
                    bool AnyData = true;
                    if (CheckCount)
                    {
                        string SqlCount = "SELECT COUNT(*) " + SQL.Substring(SQL.IndexOf(" FROM "));
                        Npgsql.NpgsqlCommand Count = new Npgsql.NpgsqlCommand(SqlCount, con);
                        string count = Count.ExecuteScalar().ToString();
                        int i = 0;
                        if (int.TryParse(count, out i) && i == 0)
                            AnyData = false;
                    }
                    if (AnyData)
                    {
                        C = new NpgsqlCommand(SQL, con);
                        Result = C.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Message = ex.Message;
            }
            catch (Exception ex)
            {
                if (ConnectionFailure(ex) && !Retry)
                {
                    Message = "";
                    SqlExecuteSkalar(SQL, ref Message, true);
                }
                else
                {
                    Message = ex.Message;
                    if (ex.GetType() == typeof(NpgsqlException))
                    {
                        NpgsqlException npgsqlException = (NpgsqlException)ex;
                        if (npgsqlException.StackTrace != null && npgsqlException.StackTrace.Length > 0)
                            Message += " - " + npgsqlException.StackTrace;
                    }
                }
            }
            finally
            {
                if (C != null)
                    C.Dispose();
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                if (con != null)
                    con.Dispose();
            }
            return Result;
        }
        
        public static string SqlExecuteSkalar(string SQL, string ConnectionString, bool Retry = false)
        {
            string Result = "";
            NpgsqlConnection con = null;
            Npgsql.NpgsqlCommand C = null;
            try
            {
                con = new NpgsqlConnection(ConnectionString);
                C = new NpgsqlCommand(SQL, con);
                con.Open();
                Result = C.ExecuteScalar()?.ToString() ?? "";
                C.Dispose();
                con.Close();
                con.Dispose();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            catch (Exception ex)
            {
                if (ConnectionFailure(ex) && !Retry)
                {
                    SqlExecuteSkalar(SQL, ConnectionString, true);
                }
                else
                    HandleException(ex, SQL);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            finally
            {
                C.Dispose();
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Dispose();
            }
            return Result;
        }

        public static string SqlExecuteSkalar(string SQL, string ConnectionString, ref string Message, bool Retry = false)
        {
            string Result = "";
            NpgsqlConnection con = null;
            Npgsql.NpgsqlCommand C = null;
            try
            {
                con = new NpgsqlConnection(ConnectionString);
                C = new NpgsqlCommand(SQL, con);
                con.Open();
                Result = C.ExecuteScalar()?.ToString();
                C.Dispose();
                con.Close();
                con.Dispose();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Message = ex.Message;
            }
            catch (Exception ex)
            {
                if (ConnectionFailure(ex) && !Retry)
                {
                    SqlExecuteSkalar(SQL, ConnectionString, ref Message, true);
                }
                else
                {
                    Message = ex.Message;
                    if (ex.GetType() == typeof(NpgsqlException))
                    {
                        NpgsqlException npgsqlException = (NpgsqlException)ex;
                        if (npgsqlException.StackTrace != null && npgsqlException.StackTrace.Length > 0)
                            Message += " - " + npgsqlException.StackTrace;
                    }
                }
            }
            finally
            {
                C.Dispose();
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Dispose();
            }
            return Result;
        }

        public static string SqlExecuteSkalar(string SQL, int TimeOutSeconds, bool Retry = false)
        {
            string Result = "";
            Npgsql.NpgsqlConnection con = null;
            Npgsql.NpgsqlCommand C = null;
            try
            {
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                {
                    con = new NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    C = new NpgsqlCommand(SQL, con);
                    C.CommandTimeout = TimeOutSeconds;
                    con.Open();
                    Result = C.ExecuteScalar()?.ToString();
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            catch (Exception ex)
            {
                if (ConnectionFailure(ex) && !Retry)
                {
                    SqlExecuteSkalar(SQL, TimeOutSeconds, true);
                }
                else
                    HandleException(ex, SQL);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            finally
            {
                if (C != null)
                    C.Dispose();
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                if (con != null)
                    con.Dispose();
            }
            return Result;
        }

        public static bool SqlFillTable(string SQL, ref System.Data.DataTable Table, ref string Message, bool Retry = false)
        {
            bool OK = true;
            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length == 0)
                return OK;
            Npgsql.NpgsqlDataAdapter ad = null;
            try
            {
                ad = new Npgsql.NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                ad.Fill(Table);
            }
            catch (Exception ex)
            {
                OK = false;
                Message = ex.Message;
                if (ex.Message.ToLower() == "timeout while getting a connection from pool.")
                {
                    DiversityWorkbench.PostgreSQL.Connection._ConnectionPoolingFailed = true;
                    DiversityWorkbench.PostgreSQL.Connection._DefaultConnectionString = "";
                }
                else if (ConnectionFailure(ex) && !Retry)
                {
                    Message = "";
                    SqlFillTable(SQL, ref Table, ref Message, true);
                }
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                ad.Dispose();
            }
            return OK;
        }

        public static bool SqlFillTable(string SQL, ref System.Data.DataTable Table, ref string Message, string Database, bool Retry = false)
        {
            bool OK = true;
            Npgsql.NpgsqlDataAdapter ad = null;
            try
            {
                ad = new Npgsql.NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DatabaseConnectionString(Database));
                ad.Fill(Table);
            }
            catch (Exception ex)
            {
                if (ConnectionFailure(ex) && !Retry)
                {
                    Message = "";
                    SqlFillTable(SQL, ref Table, ref Message, Database, true);
                }
                else
                {
                    OK = false;
                    Message = ex.Message;
                    if (ex.GetType() == typeof(NpgsqlException))
                    {
                        NpgsqlException npgsqlException = (NpgsqlException)ex;
                        if (npgsqlException.StackTrace != null && npgsqlException.StackTrace.Length > 0)
                            Message += " - " + npgsqlException.StackTrace;
                    }

                }
            }
            finally
            {
                ad.Dispose();
            }
            return OK;
        }

        /// <summary>
        /// If an object exists in the current database
        /// </summary>
        /// <param name="Name">Name of the object</param>
        /// <param name="Type">Type of the object (default = Table)</param>
        /// <param name="Schema">Schema of the object (default = public)</param>
        /// <returns></returns>
        public static bool ObjectExists(string Name, ObjectType Type = ObjectType.Table, string Schema = "")
        {
            bool Exists = false;
            if (Schema.Length == 0)
                Schema = "public";
            string SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.";
            switch (Type)
            {
                case ObjectType.Schema:
                    SQL += "schemata s where s.schema_name = '" + Name + "'";
                    break;
                case ObjectType.Table:
                    SQL += "Tables T where t.table_name = '" + Name + "' " +
                        "and t.table_type = 'BASE TABLE' " +
                        "and t.table_schema = '" + Schema + "'";
                    break;
                case ObjectType.View:
                    SQL += "Tables T where t.table_name = '" + Name + "' " +
                        "and t.table_type = 'VIEW' " +
                        "and t.table_schema = '" + Schema + "'";
                    break;
                case ObjectType.Function:
                    SQL += "routines r where r.routine_name = '" + Name + "' " +
                        "and r.routine_schema = '" + Schema + "'";
                    break;
            }
            string Result = SqlExecuteSkalar(SQL);
            if (Result.Length > 0 && Result != "0")
                Exists = true;
            return Exists;
        }

        private static bool ConnectionFailure(Exception ex)
        {
            if (ex.Message.ToLower().IndexOf(ErrorMessageTransportConnection) > -1)
                return true;
            else
                return false;
        }

        private static readonly string ErrorMessageTransportConnection = "unable to write data to the transport connection";

        /// <summary>
        /// Markus 17.11.2020 - Handling for NpgsqlException to extract details
        /// </summary>
        /// <param name="ex">The exception that occured</param>
        /// <param name="Message">The message e.g. the SQL that caused the problem</param>
        private static void HandleException(Exception ex, string Message)
        {
            try
            {
                if (ex.GetType() == typeof(NpgsqlException))
                {
                    NpgsqlException npgsqlException = (NpgsqlException)ex;
                    if (npgsqlException.StackTrace != null && npgsqlException.StackTrace.Length > 0)
                        Message += " - " + npgsqlException.StackTrace;
                    //if (npgsqlException.s.Count > 0)
                    //{
                    //    foreach (Npgsql.NpgsqlError E in npgsqlException.Errors)
                    //    {
                    //        Message += "\r\n" + E.Message;
                    //    }
                    //}
                }
                else if (ex.GetType() == typeof(Npgsql.NpgsqlException))
                {
                    Npgsql.NpgsqlException npgsqlException = (Npgsql.NpgsqlException)ex;
                    if (npgsqlException.StackTrace != null && npgsqlException.StackTrace.Length > 0)
                        Message += " - " + npgsqlException.StackTrace;
                    //if (npgsqlException.Errors.Count > 0)
                    //{
                    //    foreach(Npgsql.NpgsqlError E in npgsqlException.Errors)
                    //    {
                    //        Message += "\r\n" + E.Message;
                    //    }
                    //}
                }
                string Test = ex.GetType().ToString();
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, Message);
            }
            catch (Exception EX)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(EX);
            }
        }

        #endregion

        #region OpenRowSet commands

        public static bool SqlTransferTableDataViaOpenRowset(string Table, DiversityWorkbench.PostgreSQL.Schema Schema, string ConnectionString)
        {
            bool OK = true;
            DiversityWorkbench.PostgreSQL.Table T = new PostgreSQL.Table(Table, Schema);
            string SQL = "INSERT INTO OPENROWSET('MSDASQL', " +
                "'Driver=PostgreSQL AMD64A;" +
                "uid=" + DiversityWorkbench.PostgreSQL.Settings.Default.Role + ";" +
                "Server=" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer() + ";" +
                "database=" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() + ";" +
                "pwd=" + DiversityWorkbench.PostgreSQL.Connection.Password + "', " +
                "'SELECT " + T.ColumnsAsString().Replace("\"", "") + " " +
                "FROM " + Table + " ') " +
                "SELECT " + T.ColumnsAsString().Replace("\"", "") + " " +
                "FROM " + Table;
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                con.Open();
                C.ExecuteNonQuery();
                con.Close();
                con.Dispose();
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        public static bool SqlClearTableViaOpenRowset(string Table, DiversityWorkbench.PostgreSQL.Schema Schema, string ConnectionString)
        {
            bool OK = true;
            DiversityWorkbench.PostgreSQL.Table T = new PostgreSQL.Table(Table, Schema);
            string SQL = "DELETE FROM " +
                "OPENROWSET('MSDASQL', " +
                "'Driver=PostgreSQL AMD64A;" +
                "uid=" + DiversityWorkbench.PostgreSQL.Settings.Default.Role + ";" +
                "Server=" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer() + ";" +
                "database=" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() + ";" +
                "pwd=" + DiversityWorkbench.PostgreSQL.Connection.Password + "', " +
                "'SELECT " + T.ColumnsAsString().Replace("\"", "") + " " +
                "FROM " + Table + "')";
            return OK;
        }

        #endregion

    }
}
