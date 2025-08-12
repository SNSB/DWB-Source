using System;

namespace DiversityWorkbench
{

    /// <summary>
    /// provides central functionality needed for handling databases within the Diversity Workbench
    /// </summary>
    public class Database
    {
        #region Connection

        private System.Data.OleDb.OleDbConnection oleDbConnection;
        public Database(string ConnectionString)
        {
            this.oleDbConnection = new System.Data.OleDb.OleDbConnection(ConnectionString);
        }

        /// <summary>
        /// Checking if a database can be accessed with the current login
        /// </summary>
        /// <param name="Database">Name of the database</param>
        /// <returns>true if database is accessible, else false</returns>
        public bool CheckDatabase()
        {
            bool OK = true;
            string SQL;
            try
            {
                string Database = this.oleDbConnection.Database.ToString();
                SQL = "SELECT * FROM " + Database + ".dbo.sysconstraints"; // Check a system table present in any database
                if (Database.Length == 0)
                {
                    OK = false;
                }
                if (OK)
                {
                    System.Data.OleDb.OleDbCommand com = new System.Data.OleDb.OleDbCommand(SQL, this.oleDbConnection);
                    this.oleDbConnection.Open();
                    com.ExecuteNonQuery();
                    this.oleDbConnection.Close();
                }
            }
            catch (System.Data.OleDb.OleDbException ex)
            {
                OK = false;
            }
            catch (System.Exception ex)
            {
                OK = false;
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error checking the database", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return OK;
        }

        /// <summary>
        /// Check if the database is accessible by selecting data from it
        /// </summary>
        public bool DatabaseIsAccessible(string TestTable)
        {
            bool OK = true;
            try
            {
                string SQL = "SELECT COUNT(*) FROM " + TestTable;
                System.Data.DataTable dt = new System.Data.DataTable(TestTable);
                System.Data.OleDb.OleDbDataAdapter ad = new System.Data.OleDb.OleDbDataAdapter(SQL, this.oleDbConnection);
                ad.Fill(dt);
            }
            catch (System.Data.OleDb.OleDbException)
            {
                OK = false;
            }
            catch (System.Exception ex)
            {
                OK = false;
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error testing the accessibility of a database", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return OK;
        }

        #endregion

        #region Creation

        public static bool CreateDatabase(string Server, int Port, System.Collections.Generic.Dictionary<string, string> Versions)
        {
            bool OK = false;

            // Toni 20210420 Only ask if database shall be created
            string msg = string.Format("Do you really want to create a new database on server {0}, port {1}?", Server, Port);
            if (System.Windows.Forms.MessageBox.Show(msg, "Create database", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                DiversityWorkbench.Settings.DatabaseServer = Server;
                DiversityWorkbench.Settings.DatabasePort = Port;
                string Message = "";
                OK = CreateDatabase(Versions, ref Message);
            }
            return OK;
        }

        public static bool CreateDatabase(System.Collections.Generic.Dictionary<string, string> Versions, ref string Message)
        {
            bool OK = CreateDatabase(Versions);
            Message = _CreateDatabaseMessage;
            return OK;
        }

        private static string _CreateDatabaseMessage = "";

        public static bool CreateDatabase(System.Collections.Generic.Dictionary<string, string> Versions)
        {
            bool OK = false;

            string Server = "";
            int Port = 0;
            if (DiversityWorkbench.Settings.DatabaseServer.Trim().Length == 0 || DiversityWorkbench.Settings.DatabasePort == 0) //DiversityWorkbench.Settings.ConnectionString.Length == 0) Toni 20210420 Connection string is empty since DB name not yet defined!
            {
                Server = CreateDatabaseGetServer();
                Port = CreateDatabaseGetPort();
            }
            else
            {
                Server = DiversityWorkbench.Settings.DatabaseServer;
                Port = DiversityWorkbench.Settings.DatabasePort;
            }
            if (Server.Length == 0 || Port == 0)
                return false;

            try
            {
                // Check database rights Toni 20230120: NICHT das Windows-Login verwenden, das war nur für das Kommandozeilenupdate bestimmt!
                // Toni 20231128: Neuer Connection String zur master db! Andere DB geht schief, falls keine DB vorhanden!
                string ConString = DiversityWorkbench.Settings.ConnectionStringToMaster; // DiversityWorkbench.Settings.MasterWindowsConnectionString(Server, Port);

                // Da ist was faul...
                if (ConString.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("To create a database you need sysadmin permissions");
                    return false;
                }

                // Toni 20230120: Das verwendete Login muss Sysadmin-Rechte haben, dann ist es als dbo unterwegs!
                string SQL = "SELECT USER_NAME();";
                using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConString))
                {
                    using (Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con))
                    {
                        C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                        con.Open();
                        string SysAdmin = C.ExecuteScalar()?.ToString() ?? "";
                        con.Close();
                        if (SysAdmin != "dbo")
                        {
                            System.Windows.Forms.MessageBox.Show("To create a database you need sysadmin permissions");
                            return false;
                        }
                        else
                        {
                            string Database = CreateDatabaseName(ConString);
                            DiversityWorkbench.Forms.FormGetString fDB = new DiversityWorkbench.Forms.FormGetString("Database", "Please enter the name of the Database (Starting with " + DiversityWorkbench.Settings.ModuleName + ")", Database, DiversityWorkbench.Properties.Resources.DatabaseAdd);
                            fDB.ShowDialog();
                            if (fDB.DialogResult == System.Windows.Forms.DialogResult.OK && fDB.String.StartsWith(DiversityWorkbench.Settings.ModuleName))
                            {
                                System.Data.DataTable dtFiles = new System.Data.DataTable();
                                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter("exec sp_helpfile", ConString);
                                ad.Fill(dtFiles);

                                // Name
                                string DBname = fDB.String;

                                // Markus 25.8.2021 - Vorschlag von Toni
                                // Get Default directory for database
                                SQL = "SELECT SERVERPROPERTY('InstanceDefaultDataPath')";
                                C.CommandText = SQL;
                                con.Open();
                                string DefaultDirectory = C.ExecuteScalar()?.ToString() ?? "";
                                con.Close();

                                // setting the file names
                                string DataFile = "";
                                string LogFile = "";
                                if (DefaultDirectory.Length == 0)
                                {
                                    DataFile = dtFiles.Rows[0]["filename"].ToString();
                                    DataFile = DataFile.Substring(0, DataFile.LastIndexOf("\\")) + "\\" + DBname + ".mdf";
                                    LogFile = DataFile.Substring(0, DataFile.LastIndexOf(".")) + "_log.ldf";
                                }
                                else
                                {
                                    DataFile = DefaultDirectory + DBname + ".mdf";
                                    LogFile = DataFile.Substring(0, DataFile.LastIndexOf(".")) + "_log.ldf";
                                }

                                // Collation
                                SQL = string.Format("SELECT [collation_name] FROM [master].[sys].[databases] WHERE [name]='{0}'", DiversityWorkbench.Settings.DatabaseName);
                                C.CommandText = SQL;
                                con.Open();
                                string collation = C.ExecuteScalar()?.ToString() ?? "";
                                con.Close();

                                SQL = "CREATE DATABASE [" + DBname + "] " +
                                    "ON PRIMARY  " +
                                    "( NAME = N'" + DBname + "', FILENAME = N'" + DataFile + "' , SIZE = 5032KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%) " +
                                    "LOG ON  " +
                                    "( NAME = N'" + DBname + "_Log', FILENAME = N'" + LogFile + "' , SIZE = 504KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%) ";
                                // Markus 22.2.2022: collation may be empty
                                if (collation.Length > 0)
                                    SQL += "COLLATE " + collation;

                                // Check if Database exists
                                string SqlCheck = "select count(*) from sys.databases d where d.name = '" + DBname + "'";
                                C.CommandText = SqlCheck;
                                con.Open();
                                string Result = C.ExecuteScalar()?.ToString() ?? "";
                                con.Close();
                                if (Result == "1")
                                {
                                    System.Windows.Forms.MessageBox.Show("A database with then name " + DBname + " already exists");
                                    return false;
                                }
                                else
                                {
                                    C.CommandText = SQL;
                                    con.Open();
                                    C.ExecuteNonQuery();
                                    con.Close();

                                    // setting the recovery model
                                    SQL = "ALTER DATABASE " + DBname + " SET RECOVERY SIMPLE;";
                                    C.CommandText = SQL;
                                    con.Open();
                                    C.ExecuteNonQuery();
                                    con.Close();

                                    DiversityWorkbench.Settings.DatabaseServer = Server;
                                    DiversityWorkbench.Settings.DatabasePort = Port;
                                    DiversityWorkbench.Settings.DatabaseName = DBname;
                                    //DiversityWorkbench.Settings.IsTrustedConnection = true; Toni 20230120: Nicht einfach die Verbindung umstellen!!!
                                    ConString = DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase);

                                    OK = CreateBaseURL(ConString);

                                    if (OK)
                                    {
                                        OK = CreateVersion(ConString);
                                        if (OK)
                                        {
                                            string FinalVersion = "";
                                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Versions)
                                            {
                                                FinalVersion = KV.Key.Substring(KV.Key.IndexOf("_to_") + 4);
                                            }
                                            DiversityWorkbench.Forms.FormUpdateDatabase f = new DiversityWorkbench.Forms.FormUpdateDatabase(DiversityWorkbench.Settings.DatabaseName, FinalVersion, ConString, Versions, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());
                                            f.ShowDialog();
                                            if (!f.Reconnect)
                                            {
                                                OK = false;
                                                _CreateDatabaseMessage += "\r\nUpdate of database failed";
                                            }
                                        }
                                    }
                                }
                            }
                            else if (!fDB.String.StartsWith(DiversityWorkbench.Settings.ModuleName))
                            {
                                System.Windows.Forms.MessageBox.Show("The name of the database should start with " + DiversityWorkbench.Settings.ModuleName);
                                return false;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private static string CreateDatabaseGetServer()
        {
            string Server = "127.0.0.1";
            DiversityWorkbench.Forms.FormGetString fS = new DiversityWorkbench.Forms.FormGetString("Server", "Please enter the name or the IP-Address of the server", Server, DiversityWorkbench.Properties.Resources.ServerIO);
            fS.ShowDialog();
            if (fS.DialogResult == System.Windows.Forms.DialogResult.OK && fS.String.Length > 0)
            {
                Server = fS.String;
            }
            else
                _CreateDatabaseMessage += "\r\nServer has not been specified";
            return Server;
        }

        private static int CreateDatabaseGetPort()
        {
            int Port = 1433;
            int? iPort = Port;
            DiversityWorkbench.Forms.FormGetInteger fPort = new DiversityWorkbench.Forms.FormGetInteger(iPort, "Port", "Please enter the port of the server");
            fPort.ShowDialog();
            if (fPort.DialogResult == System.Windows.Forms.DialogResult.OK && fPort.Integer != null)
            {
                Port = (int)fPort.Integer;
            }
            else
                _CreateDatabaseMessage += "\r\nPort has not been specified";
            return Port;
        }



        private static string CreateDatabaseName(string ConStr)
        {
            string DbName = "";
            if (DiversityWorkbench.Settings.DatabaseName.Length > 0)
                DbName = DiversityWorkbench.Settings.DatabaseName;
            else
                DbName = DiversityWorkbench.Settings.ModuleName;
            try
            {
                string SqlCheck = "select count(*) from sys.databases d where d.name = '" + DbName;
                int i = 0;
                bool OK = false;
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConStr);
                Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SqlCheck, con);
                con.Open();

                while (!OK)
                {
                    string SQL = SqlCheck;
                    if (i > 0)
                        SQL += "_" + i.ToString() + "'";
                    else
                        SQL += "'";
                    c.CommandText = SQL;
                    string result = c.ExecuteScalar()?.ToString() ?? string.Empty;
                    if (result == "0" || i > 8)
                    {
                        OK = true;
                        if (i > 0)
                            DbName += "_" + i.ToString();
                    }
                    i++;
                }
                con.Close();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                _CreateDatabaseMessage += "\r\n" + ex.Message;
            }
            return DbName;
        }

        private static bool CreateBaseURL(string ConStr)
        {
            bool OK = false;
            string SQL = "CREATE FUNCTION [dbo].[BaseURL] () RETURNS  varchar(255) AS BEGIN declare @URL varchar(255) set @URL = 'http://" + DiversityWorkbench.Settings.DatabaseServer + "/" + DiversityWorkbench.Settings.DatabaseName.Replace("Diversity", "") + "/' return @URL END;";
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ConStr, ref _CreateDatabaseMessage);
            return OK;
        }

        private static bool CreateVersion(string ConStr)
        {
            bool OK = false;
            string SQL = "CREATE FUNCTION [dbo].[Version] () RETURNS nvarchar(8) AS BEGIN RETURN '00.00.00' END;";
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ConStr, ref _CreateDatabaseMessage);
            return OK;
        }

        #endregion

        #region History

        /// <summary>
        /// 
        /// A table containing the history of the datasets based on the log table
        /// </summary>
        /// <param name="ID">The value of a unique PK in the current of a superior table</param>
        /// <param name="PkColumn">The name of the column of the PK containing the ID</param>
        /// <param name="DataTable">The name of the table</param>
        /// <param name="VersionTable">The table containing the version</param>
        /// <param name="ShowAll">'true' if all columns shall be inserted</param>
        /// <returns>A table containing the current state and the history of the datasets related to an ID</returns>
        public static System.Data.DataTable DtHistory(
            int ID,
            string PkColumn,
            string DataTable,
            string VersionTable,
            bool ShowAll = true,
            bool LinkToUserProxy = false)
        {
            System.Data.DataTable dtHistory = new System.Data.DataTable(DataTable);
            System.Data.DataTable dtColumns = new System.Data.DataTable();
            string SQL = "select C.COLUMN_NAME, C.DATA_TYPE " +
                "from INFORMATION_SCHEMA.COLUMNS C, INFORMATION_SCHEMA.COLUMNS CL " +
                "where C.TABLE_NAME = '" + DataTable + "' " +
                "and CL.TABLE_NAME = '" + DataTable + "_log' " +
                "and C.COLUMN_NAME = CL.COLUMN_NAME " +
                "ORDER BY C.ORDINAL_POSITION";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                a.Fill(dtColumns);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            System.Data.DataTable dtLogColumns = new System.Data.DataTable();
            SQL = "select C.COLUMN_NAME, C.DATA_TYPE " +
                "from INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = '" + DataTable + "_log' " +
                "ORDER BY C.ORDINAL_POSITION";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                a.Fill(dtLogColumns);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            SQL = "SELECT ";
            string SqlCurrent = "SELECT ";
            if (VersionTable != DataTable
                && VersionTable.Length > 0)
            {
                System.Data.DataRow[] rr = dtLogColumns.Select("COLUMN_NAME = 'LogVersion'", "");
                if (rr.Length > 0)
                {
                    SqlCurrent += VersionTable + ".Version, ";
                    SQL += DataTable + "_log.LogVersion AS Version, ";
                }
            }
            else if (VersionTable.Length > 0)
            {
                SqlCurrent += " Version, ";
                SQL += " Version, ";
            }
            foreach (System.Data.DataRow R in dtColumns.Rows)
            {
                try
                {
                    if (ShowAll ||
                        (!R["COLUMN_NAME"].ToString().StartsWith("Log")
                        && !R["COLUMN_NAME"].ToString().StartsWith("xx_")
                        && R["COLUMN_NAME"].ToString() != "RowGUID"))
                    {
                        if (R["DATA_TYPE"].ToString() == "date" || R["DATA_TYPE"].ToString() == "datetime" || R["DATA_TYPE"].ToString() == "datetime2" || R["DATA_TYPE"].ToString() == "smalldatetime")
                        {
                            SqlCurrent += "convert(varchar(20), [" + DataTable + "]." + R["COLUMN_NAME"].ToString() + ", 120) AS " + R["COLUMN_NAME"].ToString() + ", ";
                            SQL += "convert(varchar(20), [" + DataTable + "_log]." + R["COLUMN_NAME"].ToString() + ", 120) AS " + R["COLUMN_NAME"].ToString() + ", ";
                        }
                        else
                        {
                            if (VersionTable != DataTable)
                            {
                                if (R["DATA_TYPE"].ToString().ToLower() == "geography" || R["DATA_TYPE"].ToString().ToLower() == "geometry")
                                {
                                    SqlCurrent += "[" + DataTable + "]." + R["COLUMN_NAME"].ToString() + ".ToString() AS " + R["COLUMN_NAME"].ToString() + ", ";
                                    SQL += DataTable + "_log." + R["COLUMN_NAME"].ToString() + ".ToString() AS " + R["COLUMN_NAME"].ToString() + ", ";
                                }
                                else
                                {
                                    if (LinkToUserProxy &&
                                        (R["COLUMN_NAME"].ToString() == "LogUpdatedBy" ||
                                        R["COLUMN_NAME"].ToString() == "LogCreatedBy" ||
                                        R["COLUMN_NAME"].ToString() == "LogInsertedBy"))
                                    {
                                        SqlCurrent += "dbo.UserName([" + DataTable + "]." + R["COLUMN_NAME"].ToString() + ") AS " + R["COLUMN_NAME"].ToString() + ", ";
                                        SQL += "dbo.UserName(" + DataTable + "_log." + R["COLUMN_NAME"].ToString() + " ) AS " + R["COLUMN_NAME"].ToString() + ", ";
                                    }
                                    else if (R["DATA_TYPE"].ToString() == "datetime")
                                    {
                                        SqlCurrent += "convert(varchar(20), [" + DataTable + "]." + R["COLUMN_NAME"].ToString() + ", 120) AS " + R["COLUMN_NAME"].ToString() + ", ";
                                        SQL += "convert(varchar(20), [" + DataTable + "_log]." + R["COLUMN_NAME"].ToString() + ", 120) AS " + R["COLUMN_NAME"].ToString() + ", ";
                                    }
                                    else
                                    {
                                        SqlCurrent += "[" + DataTable + "]." + R["COLUMN_NAME"].ToString() + ", ";
                                        SQL += DataTable + "_log." + R["COLUMN_NAME"].ToString() + ", ";
                                    }
                                }
                            }
                            else
                            {
                                if (LinkToUserProxy &&
                                    (R["COLUMN_NAME"].ToString() == "LogUpdatedBy" ||
                                    R["COLUMN_NAME"].ToString() == "LogCreatedBy" ||
                                    R["COLUMN_NAME"].ToString() == "LogInsertedBy"))
                                {
                                    SqlCurrent += "dbo.UserName(" + R["COLUMN_NAME"].ToString() + ") AS " + R["COLUMN_NAME"].ToString() + " , ";
                                    SQL += "dbo.UserName(" + R["COLUMN_NAME"].ToString() + ") AS " + R["COLUMN_NAME"].ToString() + " , ";
                                }
                                else
                                {
                                    SqlCurrent += R["COLUMN_NAME"].ToString() + ", ";
                                    SQL += R["COLUMN_NAME"].ToString() + ", ";
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                }
            }
            if (VersionTable != DataTable)
            {
                SqlCurrent += "'current version' AS [Kind of change], convert(varchar(20), [" + DataTable + "].LogUpdatedWhen, 120) AS [Date of change], ";
                if (LinkToUserProxy)
                    SqlCurrent += "dbo.UserName([" + DataTable + "].LogUpdatedBy)";
                else
                    SqlCurrent += "[" + DataTable + "].LogUpdatedBy";
                SqlCurrent += " AS [Responsible user], NULL AS LogID " +
                    "FROM [" + DataTable + "]";
                if (VersionTable.Length > 0)
                {
                    SqlCurrent += " INNER JOIN " +
                    VersionTable + " ON [" + DataTable + "]." + PkColumn + " = " + VersionTable + "." + PkColumn;
                }
                SqlCurrent += " WHERE [" + DataTable + "]." + PkColumn + " = " + ID.ToString();
                SQL += "CASE WHEN " + DataTable + "_log.LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], " +
                    "convert(varchar(20), " + DataTable + "_log.LogDate, 120) AS [Date of change], ";
                if (LinkToUserProxy)
                    SQL += "dbo.UserName(" + DataTable + "_log.LogUser)";
                else
                    SQL += DataTable + "_log.LogUser ";
                SQL += " AS [Responsible user], " + DataTable + "_log.LogID  " +
                    "FROM  " + DataTable + "_log " +
                    "WHERE " + PkColumn + " = " + ID.ToString() +
                    " ORDER BY " + DataTable + "_log.LogID DESC ";
            }
            else
            {
                SqlCurrent += "'current version' AS [Kind of change], convert(varchar(20), LogUpdatedWhen, 120) AS [Date of change], ";
                if (LinkToUserProxy)
                    SqlCurrent += "dbo.UserName(" + DataTable + ".LogUpdatedBy)";
                else
                    SqlCurrent += DataTable + ".LogUpdatedBy ";
                SqlCurrent += " AS [Responsible user], NULL AS LogID " +
                    "FROM " + DataTable +
                    " WHERE " + PkColumn + " = " + ID.ToString();
                SQL += "CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], " +
                    "convert(varchar(20), LogDate, 120) AS [Date of change], ";
                if (LinkToUserProxy)
                    SQL += "dbo.UserName(LogUser)";
                else
                    SQL += "LogUser";
                SQL += " AS [Responsible user], LogID  " +
                    "FROM  " + DataTable + "_log " +
                    "WHERE " + PkColumn + " = " + ID.ToString() +
                    " ORDER BY LogID DESC ";
            }
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                a.Fill(dtHistory);
                a.SelectCommand.CommandText = SQL;
                a.Fill(dtHistory);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return dtHistory;
        }

        /// <summary>
        /// A table containing the history of the datasets based on the log table
        /// </summary>
        /// <param name="ID">The value of a unique PK in the current of a superior table</param>
        /// <param name="PkColumn">The name of the column of the PK containing the ID</param>
        /// <param name="Restriction">A restriction for the selection of the data</param>
        /// <param name="DataTable">The name of the table</param>
        /// <param name="VersionTable">The table containing the version</param>
        /// <param name="ShowAll">'true' if all columns shall be inserted</param>
        /// <returns>A table containing the current state and the history of the datasets related to an ID</returns>
        public static System.Data.DataTable DtHistory(
            int ID,
            string PkColumn,
            string Restriction,
            string DataTable,
            string VersionTable,
            bool ShowAll = false)
        {
            System.Data.DataTable dtHistory = new System.Data.DataTable(DataTable);
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "SELECT * FROM [" + DataTable + "] WHERE " + PkColumn + " = " + ID.ToString();
            if (Restriction.Length > 0) SQL += " AND " + Restriction;
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                a.Fill(dt);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            SQL = "SELECT ";
            string SqlCurrent = "SELECT ";
            if (VersionTable != DataTable
                && VersionTable.Length > 0)
            {
                SqlCurrent += VersionTable + ".Version, ";
                SQL += DataTable + "_log.LogVersion AS Version, ";
            }
            else if (VersionTable.Length > 0)
            {
                SqlCurrent += " Version, ";
                SQL += " Version, ";
            }
            foreach (System.Data.DataColumn C in dt.Columns)
            {
                if (ShowAll ||
                    (C.ColumnName != "Version"
                    && !C.ColumnName.StartsWith("Log")
                    && !C.ColumnName.StartsWith("xx_")
                    && C.ColumnName != "RowGUID"))
                {
                    if (VersionTable != DataTable)
                    {
                        if (C.DataType.Name == "SqlGeography")
                        {
                            SqlCurrent += "[" + DataTable + "]." + C.ColumnName + ".ToString() AS " + C.ColumnName + ", ";
                            SQL += DataTable + "_log." + C.ColumnName + ".ToString() AS " + C.ColumnName + ", ";
                        }
                        else
                        {
                            SqlCurrent += "[" + DataTable + "]." + C.ColumnName + ", ";
                            SQL += DataTable + "_log." + C.ColumnName + ", ";
                        }
                    }
                    else
                    {
                        SqlCurrent += C.ColumnName + ", ";
                        SQL += C.ColumnName + ", ";
                    }
                }
            }
            if (VersionTable != DataTable)
            {
                SqlCurrent += "'current version' AS [Kind of change], [" + DataTable + "].LogUpdatedWhen AS [Date of change], [" +
                    DataTable + "].LogUpdatedBy AS [Responsible user], NULL AS LogID " +
                    "FROM [" + DataTable + "]";
                if (VersionTable.Length > 0)
                {
                    SqlCurrent += " INNER JOIN " +
                    VersionTable + " ON [" + DataTable + "]." + PkColumn + " = " + VersionTable + "." + PkColumn;
                }
                SqlCurrent += " WHERE [" + DataTable + "]." + PkColumn + " = " + ID.ToString();
                if (Restriction.Length > 0)
                    SqlCurrent += " AND " + Restriction;
                SQL += "CASE WHEN " + DataTable + "_log.LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], " +
                    DataTable + "_log.LogDate AS [Date of change], " +
                    DataTable + "_log.LogUser AS [Responsible user], " +
                    DataTable + "_log.LogID  " +
                    "FROM  " + DataTable + "_log " +
                    "WHERE " + PkColumn + " = " + ID.ToString();
                if (Restriction.Length > 0)
                    SQL += " AND " + Restriction;
                SQL += " ORDER BY " + DataTable + "_log.LogID DESC ";
            }
            else
            {
                SqlCurrent += "'current version' AS [Kind of change], LogUpdatedWhen AS [Date of change], " +
                    "LogUpdatedBy AS [Responsible user], NULL AS LogID " +
                    "FROM " + DataTable +
                    " WHERE " + PkColumn + " = " + ID.ToString();
                if (Restriction.Length > 0)
                    SqlCurrent += " AND " + Restriction;
                SQL += "CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], " +
                    "LogDate AS [Date of change], " +
                    "LogUser AS [Responsible user], LogID  " +
                    "FROM  " + DataTable + "_log " +
                    "WHERE " + PkColumn + " = " + ID.ToString();
                if (Restriction.Length > 0)
                    SQL += " AND " + Restriction;
                SQL += " ORDER BY LogID DESC ";
            }
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                a.Fill(dtHistory);
                a.SelectCommand.CommandText = SQL;
                a.Fill(dtHistory);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtHistory;
        }

        public static System.Data.DataTable DtHistory(
            System.Data.DataTable dtData,
            bool ShowAll = false)
        {
            System.Data.DataTable dtHistory = new System.Data.DataTable(dtData.TableName);
            if (dtData.Rows.Count > 0)
            {
                System.Data.DataTable dtPK = new System.Data.DataTable();
                string SQL = "SELECT COLUMN_NAME " +
                    "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                    "WHERE (TABLE_NAME = '" + dtData.TableName + "') AND (EXISTS " +
                    "(SELECT * " +
                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                    "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                    "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtPK);

                string SqlCurrent = "SELECT ";
                string SqlLog = "SELECT ";

                foreach (System.Data.DataColumn C in dtData.Columns)
                {
                    if (ShowAll ||
                        (!C.ColumnName.StartsWith("Log")
                        && !C.ColumnName.StartsWith("xx_")
                        && C.ColumnName != "RowGUID"))
                    {
                        if (C.DataType.Name == "SqlGeography")
                        {
                            SqlCurrent += "[" + dtData.TableName + "].[" + C.ColumnName + "].ToString() AS " + C.ColumnName + ", ";
                            SqlLog += dtData.TableName + "_log.[" + C.ColumnName + "].ToString() AS [" + C.ColumnName + "], ";
                        }
                        else
                        {
                            SqlCurrent += "[" + dtData.TableName + "].[" + C.ColumnName + "], ";
                            SqlLog += dtData.TableName + "_log.[" + C.ColumnName + "], ";
                        }
                    }
                }
                SqlCurrent += "'current version' AS [Kind of change], [" + dtData.TableName + "].LogUpdatedWhen AS [Date of change], [" +
                    dtData.TableName + "].LogUpdatedBy AS [Responsible user], NULL AS LogID " +
                    "FROM [" + dtData.TableName + "]";
                SqlCurrent += " WHERE ";//[" + dtData.TableName + "]." + PkColumn + " = " + ID.ToString();
                SqlLog += "CASE WHEN " + dtData.TableName + "_log.LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], " +
                    dtData.TableName + "_log.LogDate AS [Date of change], " +
                    dtData.TableName + "_log.LogUser AS [Responsible user], " +
                    dtData.TableName + "_log.LogID  " +
                    "FROM  " + dtData.TableName + "_log " +
                    "WHERE ";// +PkColumn + " = " + ID.ToString() +
                SQL = "";
                foreach (System.Data.DataRow R in dtPK.Rows)
                {
                    if (SQL.Length > 0) SQL += " AND ";
                    SQL += " " + R[0].ToString() + " IN (";
                    for (int i = 0; i < dtData.Rows.Count; i++)
                    {
                        if (i > 0) SQL += ", ";
                        SQL += "'" + dtData.Rows[i][R[0].ToString()].ToString().Replace("'", "''") + "'";
                    }
                    SQL += ")";
                }
                SqlCurrent += SQL;
                SqlLog += SQL;
                SqlLog += " ORDER BY " + dtData.TableName + "_log.LogID DESC ";

                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                a.Fill(dtHistory);
                a.SelectCommand.CommandText = SqlLog;
                a.Fill(dtHistory);
            }
            return dtHistory;
        }

        #endregion

        #region Roles

        private static System.Collections.Generic.List<string> _DatabaseRoles;

        /// <summary>
        /// Roles of the user within the current database
        /// </summary>
        /// <returns>List of the roles</returns>
        public static System.Collections.Generic.List<string> DatabaseRoles()
        {
            if (DiversityWorkbench.Database._DatabaseRoles != null)
                return DiversityWorkbench.Database._DatabaseRoles;
            DiversityWorkbench.Database._DatabaseRoles = new System.Collections.Generic.List<string>();
            string SQL = "select pR.name " +
                "from sys.database_principals pR, sys.database_role_members, sys.database_principals pU  " +
                "where sys.database_role_members.role_principal_id = pR.principal_id  " +
                "and sys.database_role_members.member_principal_id = pU.principal_id " +
                "and pU.type <> 'R' " +
                "and pU.Name = User_Name()";
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                    DiversityWorkbench.Database._DatabaseRoles.Add(R[0].ToString());
            }
            catch (System.Exception ex) { }
            return DiversityWorkbench.Database._DatabaseRoles;
        }

        public static System.Collections.Generic.List<string> DatabaseRoles(string ConnectionString)
        {
            if (DiversityWorkbench.Database._DatabaseRoles != null)
                return DiversityWorkbench.Database._DatabaseRoles;
            DiversityWorkbench.Database._DatabaseRoles = new System.Collections.Generic.List<string>();
            string SQL = "select pR.name " +
                "from sys.database_principals pR, sys.database_role_members, sys.database_principals pU  " +
                "where sys.database_role_members.role_principal_id = pR.principal_id  " +
                "and sys.database_role_members.member_principal_id = pU.principal_id " +
                "and pU.type <> 'R' " +
                "and pU.Name = User_Name()";
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                    DiversityWorkbench.Database._DatabaseRoles.Add(R[0].ToString());
            }
            catch (System.Exception ex) { }
            return DiversityWorkbench.Database._DatabaseRoles;
        }

        public static void DatabaseRolesReset()
        {
            try
            {
                DiversityWorkbench.Database._DatabaseRoles = null;
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Update

        public static bool CheckIfClientNeedsAnUpdate(int versionMajor, int versionMinor, int versionBuild, int versionRevision, ref string clientVersionAsInDb)
        {
            bool clientNeedsUpdate = false;
            string currentClientVersion = versionMajor.ToString() + "." + versionMinor.ToString() + "." + versionBuild.ToString() + "." + versionRevision.ToString();// System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString();
            const string sql = "select dbo.VersionClient()";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(sql, con);
            try
            {
                con.Open();
                clientVersionAsInDb = c.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
                if (clientVersionAsInDb == string.Empty)
                    throw new Exception("Database version is empty.");
            }
            catch
            {
                // ignored
            }

            try
            {
                string[] VersionParts;
                if (clientVersionAsInDb.IndexOf('/') > -1)
                    VersionParts = clientVersionAsInDb.Split(new char[] { '/' });
                else
                    VersionParts = clientVersionAsInDb.Split(new char[] { '.' });

                int LastVersionMajor = int.Parse(VersionParts[0]);
                int LastVersionMinor = int.Parse(VersionParts[1]);
                int LastVersionBuild = int.Parse(VersionParts[2]);
                int LastVersionRevision = int.Parse(VersionParts[3]);
                if (LastVersionMajor != versionMajor
                    || LastVersionMinor != versionMinor
                    || LastVersionBuild != versionBuild
                    || LastVersionRevision != versionRevision)
                {
                    if (LastVersionMajor > versionMajor)
                        clientNeedsUpdate = true;
                    else if (LastVersionMajor == versionMajor)
                    {
                        if (LastVersionMinor > versionMinor)
                            clientNeedsUpdate = true;
                        else if (LastVersionMinor == versionMinor)
                        {
                            if (LastVersionBuild > versionBuild)
                                clientNeedsUpdate = true;
                            else if (LastVersionBuild == versionBuild)
                            {
                                if (LastVersionRevision > versionRevision)
                                    clientNeedsUpdate = true;
                                else clientNeedsUpdate = false;
                            }
                            else clientNeedsUpdate = false;
                        }
                        else clientNeedsUpdate = false;
                    }
                    else clientNeedsUpdate = false;
                }
            }
            catch { }
            return clientNeedsUpdate;
        }

        /// <summary>
        /// Check if an update of the database is necessary
        /// </summary>
        /// <param name="DatabaseVersionForClient">The version of the database expected by the client</param>
        /// <param name="VersionOfDatabase">The actual version of the database</param>
        /// <returns>if an update of the database is necessary</returns>
        public static bool CheckIfDatabaseNeedsAnUpdate(string DatabaseVersionForClient, ref string VersionOfDatabase)
        {
            bool Update = false;
            string SQL = "select dbo.Version()";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                VersionOfDatabase = C.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
            }
            catch { }
            if (VersionOfDatabase != DatabaseVersionForClient)
            {
                int DBversionMajor;
                int DBversionMinor;
                int DBversionRevision;
                int C_versionMajor;
                int C_versionMinor;
                int C_versionRevision;
                string[] VersionDBParts;
                if (VersionOfDatabase.IndexOf('.') > -1) VersionDBParts = VersionOfDatabase.Split(new Char[] { '.' });
                else if (VersionOfDatabase.IndexOf('/') > -1) VersionDBParts = VersionOfDatabase.Split(new Char[] { '/' });
                else VersionDBParts = VersionOfDatabase.Split(new Char[] { ' ' });
                if (!int.TryParse(VersionDBParts[0], out DBversionMajor)) return false;
                if (!int.TryParse(VersionDBParts[1], out DBversionMinor)) return false;
                if (!int.TryParse(VersionDBParts[2], out DBversionRevision)) return false;

                string[] VersionC_Parts = DatabaseVersionForClient.Split(new Char[] { '.' });
                if (!int.TryParse(VersionC_Parts[0], out C_versionMajor)) return false;
                if (!int.TryParse(VersionC_Parts[1], out C_versionMinor)) return false;
                if (!int.TryParse(VersionC_Parts[2], out C_versionRevision)) return false;

                if (C_versionMajor > DBversionMajor ||
                    (C_versionMajor == DBversionMajor && C_versionMinor > DBversionMinor) ||
                    (C_versionMajor == DBversionMajor && C_versionMinor == DBversionMinor && C_versionRevision > DBversionRevision))
                    Update = true;
            }
            return Update;
        }

        public static bool UpdateDatabase(string VersionOfDatabaseExpectedByClient,
            string PrefixForUpdateFiles,
            string PathForHelpFile,
            System.Resources.ResourceManager RM)
        {
            bool Reconnect = true;
            try
            {
                //TODO Ariane not used? string DatabaseCurrentVersion = "";
                //string SQL = "select dbo.Version()";
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                //Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                //try
                //{
                //    con.Open();
                //    DatabaseCurrentVersion = C.ExecuteScalar()?.ToString();
                //    con.Close();
                //}
                //catch { }
                //DatabaseCurrentVersion = DatabaseCurrentVersion.Replace(".", "").Replace("/", "");
                string DatabaseFinalVersion = VersionOfDatabaseExpectedByClient;
                DatabaseFinalVersion = DatabaseFinalVersion.Replace(".", "").Replace("/", "");

                // check resouces for update scripts
                System.Collections.Generic.Dictionary<string, string> Versions = new System.Collections.Generic.Dictionary<string, string>();
                System.Resources.ResourceSet rs = RM.GetResourceSet(new System.Globalization.CultureInfo("en-US"), true, true);
                if (rs != null)
                {
                    System.Collections.IDictionaryEnumerator de = rs.GetEnumerator();
                    while (de.MoveNext() == true)
                    {
                        if (de.Entry.Value is string)
                        {
                            if (de.Key.ToString().StartsWith(PrefixForUpdateFiles))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                            }
                        }
                    }
                }

                if (Versions.Count > 0)
                {
                    DiversityWorkbench.Forms.FormUpdateDatabase f = new DiversityWorkbench.Forms.FormUpdateDatabase(DiversityWorkbench.Settings.DatabaseName,
                        VersionOfDatabaseExpectedByClient,
                        DiversityWorkbench.Settings.ConnectionString,
                        Versions,
                        PathForHelpFile);
                    f.ShowDialog();
                    Reconnect = f.Reconnect;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Upgrade resources are missing");
                }

            }
            catch (Exception ex)
            {
                Reconnect = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Reconnect;
        }

        /// <summary>
        /// Silent database mass update for windows database owner
        /// Updates all databases of the calling application to the specified target version
        /// </summary>
        /// <param name="Server">Server</param>
        /// <param name="Port">Port</param>
        /// <param name="VersionOfDatabaseExpectedByClient">Target version</param>
        /// <param name="PrefixForUpdateFiles">Prefix for update files</param>
        /// <param name="PathForHelpFile">Help path</param>
        /// <param name="RM">Resource manager of application</param>
        /// <returns>'true' if success</returns>
        public static bool SilentDatabaseUpdate(string Server,
            int Port,
            string VersionOfDatabaseExpectedByClient,
            string PrefixForUpdateFiles,
            string PathForHelpFile,
            System.Resources.ResourceManager RM)
        {
            // Save default connection parameter
            string saveServer = DiversityWorkbench.Settings.DatabaseServer;
            int savePort = DiversityWorkbench.Settings.DatabasePort;
            string saveDatabase = DiversityWorkbench.Settings.DatabaseName;
            string saveUser = DiversityWorkbench.Settings.DatabaseUser;
            string savePassword = DiversityWorkbench.Settings.Password;
            bool saveTrusted = DiversityWorkbench.Settings.IsTrustedConnection;

            bool Result = true;
            string Message = "";

            try
            {
                // Check database rights
                string masterConString = Settings.MasterWindowsConnectionString(Server, Port);
                if (masterConString.Length == 0)
                {
                    Message += (Message == "" ? "" : "\r\n") + "Insufficient database rights";
                    Result = false;
                }

                // Get resouces for update scripts
                System.Collections.Generic.Dictionary<string, string> Versions = new System.Collections.Generic.Dictionary<string, string>();
                System.Resources.ResourceSet rs = RM.GetResourceSet(new System.Globalization.CultureInfo("en-US"), true, true);
                if (rs != null)
                {
                    System.Collections.IDictionaryEnumerator de = rs.GetEnumerator();
                    while (de.MoveNext() == true)
                    {
                        if (de.Entry.Value is string)
                        {
                            if (de.Key.ToString().StartsWith(PrefixForUpdateFiles))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                            }
                        }
                    }
                }
                if (Versions.Count < 1)
                {
                    Message = "Update resources are missing";
                    Result = false;
                }

                if (Result)
                {
                    // Nomalize database version
                    string targetVersion = Settings.NormalizeVersion(VersionOfDatabaseExpectedByClient, 2);

                    // Get database list
                    System.Collections.Generic.Dictionary<string, string> databases = Settings.GetUpdateDatabases(masterConString);

                    foreach (System.Collections.Generic.KeyValuePair<string, string> item in databases)
                    {
                        if (item.Value.CompareTo(targetVersion) < 0)
                        {
                            // Set connection
                            DiversityWorkbench.Settings.DatabaseServer = Server;
                            DiversityWorkbench.Settings.DatabasePort = Port;
                            DiversityWorkbench.Settings.IsTrustedConnection = true;
                            DiversityWorkbench.Settings.DatabaseName = item.Key;
                            string connectionString = DiversityWorkbench.Settings.ConnectionString;

                            if (connectionString.Length > 0)
                            {
                                // Update database
                                using (DiversityWorkbench.Forms.FormUpdateDatabase f = new DiversityWorkbench.Forms.FormUpdateDatabase(DiversityWorkbench.Settings.DatabaseName,
                                            VersionOfDatabaseExpectedByClient,
                                            connectionString,
                                            Versions,
                                            PathForHelpFile))
                                {
                                    // Perform update
                                    if (f.UpdateSilent())
                                        Message += (Message == "" ? "" : "\r\n") + item.Key + " update to " + targetVersion;
                                    else
                                        Message += (Message == "" ? "" : "\r\n") + item.Key + " update failed";
                                }
                            }
                            else
                            {
                                // Mark connection problem
                                Message += (Message == "" ? "" : "\r\n") + item.Key + " connection proplem";
                            }
                        }
                        else
                        {
                            Message += (Message == "" ? "" : "\r\n") + item.Key + " no update";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message += (Message == "" ? "" : "\r\n") + ex.Message;
                Result = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            // Restore default connection
            DiversityWorkbench.Settings.DatabaseServer = saveServer;
            DiversityWorkbench.Settings.DatabasePort = savePort;
            DiversityWorkbench.Settings.DatabaseName = saveDatabase;
            DiversityWorkbench.Settings.DatabaseUser = saveUser;
            DiversityWorkbench.Settings.Password = savePassword;
            DiversityWorkbench.Settings.IsTrustedConnection = saveTrusted;

            try
            {
                // Write update Report
                string Logfile = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Updates) + "\\Updatereport.log";
                using (System.IO.StreamWriter sw = System.IO.File.Exists(Logfile) ? new System.IO.StreamWriter(Logfile, true) : new System.IO.StreamWriter(Logfile))
                {
                    sw.WriteLine("User:\t" + System.Environment.UserName);
                    sw.Write("Date:\t");
                    sw.WriteLine(DateTime.Now);
                    sw.WriteLine("Server:\t" + Server + ", " + Port.ToString());
                    sw.WriteLine("Target:\t" + VersionOfDatabaseExpectedByClient);
                    sw.WriteLine(Message);
                    sw.WriteLine("Processing " + (Result ? "ended" : "aborted"));
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Result;
        }

        private static string _ClientVersionAvailable;
        public static string ClientVersionAvailable()
        {
            if (Database._ClientVersionAvailable != null)
                return Database._ClientVersionAvailable;

            string ClientVersionAvailable = "";
            string SQL = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VersionClient]') " +
                "AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                "BEGIN SELECT 1 END " +
                "ELSE BEGIN SELECT 0 END ";
            string FunctionExists = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (FunctionExists != "1")
            {
                SQL = "CREATE FUNCTION [dbo].[VersionClient] () RETURNS nvarchar(11) AS BEGIN RETURN '00.00.00.00' END";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            SQL = "SELECT dbo.VersionClient ()";
            ClientVersionAvailable = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            return ClientVersionAvailable;
        }

        #endregion

        #region Backup & Restore

        /// <summary>
        /// Creates copy backup from a database
        /// </summary>
        /// <param name="Database">Database name</param>
        /// <param name="BackupFile">IN: Backup file name; OUT: Complete backup file name
        /// If in input no path is included, the backup will be stored in the database directory. If an empty
        /// string is specified, the backup file name will be created from database name, data and time and
        /// included in the database directory</param>
        /// <param name="Result">Error message</param>
        /// <returns>'true' if success</returns>
        public static bool BackupCopy(string Database, ref string BackupFile, out string Result)
        {
            string SQL;
            string backupPath = ".";
            string dbVersion = "xxxxxxxx";
            DateTime backupTime = DateTime.Now;
            System.Data.DataTable dt = new System.Data.DataTable();

            // initialize result string
            Result = string.Empty;

            if (!BackupFile.Contains("\\"))
            {
                // retrieve database file names to get path
                SQL = string.Format("SELECT [physical_name]\r\n" +
                                    "FROM [{0}].[sys].[database_files]", Database);

                try
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(dt);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    Result = ex.Message;
                    return false;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    backupPath = dt.Rows[i]["physical_name"].ToString();
                    if (backupPath.EndsWith(".mdf", StringComparison.InvariantCultureIgnoreCase))
                    {
                        int idx = backupPath.LastIndexOf('\\');
                        if (idx > 0)
                        {
                            backupPath = backupPath.Remove(idx);
                            break;
                        }
                    }
                    // reinitialze path for standard backup directory
                    backupPath = ".";
                }
            }

            if (BackupFile == string.Empty)
            {
                Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                try
                {
                    sqlConnection.Open();
                    dbVersion = sqlConnection.ServerVersion.Replace(".", "");
                    sqlConnection.Close();
                    //Microsoft.SqlServer.Management.Common.ServerConnection conn = new Microsoft.SqlServer.Management.Common.ServerConnection(sqlConnection);
                    //Microsoft.SqlServer.Management.Smo.Server srv = new Microsoft.SqlServer.Management.Smo.Server(conn);
                    //dbVersion = srv.Information.Version.ToString().Replace(".","");
                }
                catch { }

                // generate file name from databae, date and time and insert path
                BackupFile = string.Format("{0}\\MSSQL{1}_{2}_{3}{4}{5}_{6}{7}.bak", backupPath, dbVersion, Database,
                    backupTime.Year.ToString("0000"), backupTime.Month.ToString("00"), backupTime.Day.ToString("00"), backupTime.Hour.ToString("00"), backupTime.Minute.ToString("00"));
            }
            else if (!BackupFile.Contains("\\"))
            {
                // insert path
                BackupFile = string.Format("{0}\\{1}", backupPath, BackupFile);
            } // else keep provided file name unchanged

            SQL = string.Format("BACKUP DATABASE [{0}] TO  DISK = N'{1}' " +
                                "WITH COPY_ONLY, NOFORMAT, NOINIT, NAME = N'Backup copy of {0} from {2}'," +
                                "SKIP, NOREWIND, NOUNLOAD, STATS = 10", Database, BackupFile, backupTime.ToString());

            return DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Result);
        }

        /// <summary>
        /// Restores backup copy to a database
        /// </summary>
        /// <param name="Database">Database name</param>
        /// <param name="BackupFile">Complete Backup file name</param>
        /// <param name="Result">Error message</param>
        /// <returns>'true' if success</returns>
        public static bool RestoreCopy(string Database, string BackupFile, out string Result)
        {
            string SQL;

            // initialize result string
            Result = string.Empty;

            // use connection to master database for further steps
            string conStrMaster = DiversityWorkbench.Settings.ConnectionString.Replace(DiversityWorkbench.Settings.Connection.Database, "master");
            Microsoft.Data.SqlClient.SqlConnection conMaster = new Microsoft.Data.SqlClient.SqlConnection(conStrMaster);
            int SQLerror = 0;

            // restore database
            ScriptProcessor sp = new ScriptProcessor(conStrMaster);

            using (sp)
            {
                SQL = string.Format("USE [master]\r\nGO\r\nALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", Database);
                sp.Open(SQL, "ResetCONN");
                SQLerror = sp.Run();
                sp.Close();
                if (SQLerror > 16)
                {
                    Result = sp.Errors;
                    return false;
                }

                SQL = string.Format("USE [master]\r\nGO\r\nRESTORE DATABASE [{0}] FROM  DISK = N'{1}' WITH  FILE = 1, NOUNLOAD, REPLACE, STATS = 10", Database, BackupFile);
                sp.Open(SQL, "RestoreDB");
                SQLerror = sp.Run();
                sp.Close();
                if (SQLerror > 0)
                {
                    Result = sp.Errors;
                    SQL = string.Format("USE [master]\r\nGO\r\nALTER DATABASE [{0}] SET MULTI_USER", Database);
                    sp.Open(SQL, "SetMULTI");
                    SQLerror = sp.Run();
                    sp.Close();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Change the name of the current database
        /// </summary>
        /// <param name="NewName">The new name of the database</param>
        public static bool RenameDatabase()
        {
            bool OK = false;
            try
            {
                /// Checking processes from other hosts
                System.Data.DataTable dt = new System.Data.DataTable();
                string SQL = "sp_who";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                string MessageProcess = "";
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    if (R["dbname"].ToString() == DiversityWorkbench.Settings.DatabaseName && R["hostname"].ToString().Trim() != System.Environment.MachineName)
                    {
                        MessageProcess += "\r\nID: " + R["spid"].ToString() + " User: " + R["loginame"].ToString().Trim() + " Host: " + R["hostname"].ToString().Trim();
                    }
                }

                string Message = "Do you really want to change the name of the current database " + DiversityWorkbench.Settings.DatabaseName + "?";
                if (MessageProcess.Length > 0)
                    Message += "\r\n\r\nRunning processes from other hosts that will be terminated:\r\n" + MessageProcess;
                if (System.Windows.Forms.MessageBox.Show(Message, "Change name of database", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return OK;

                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New name of database", "Please enter the new name of the database", DiversityWorkbench.Settings.DatabaseName);
                f.ShowDialog();
                if (f.DialogResult != System.Windows.Forms.DialogResult.OK || f.String.Length == 0)
                    return OK;
                else
                {
                    OK = true;
                    string NewName = f.String;
                    string Sql_1_SingleMode = "USE master; ALTER DATABASE " + DiversityWorkbench.Settings.DatabaseName +
                        " SET SINGLE_USER " +
                        " WITH ROLLBACK IMMEDIATE;";
                    string Sql_2_Rename = "ALTER DATABASE " + DiversityWorkbench.Settings.DatabaseName + " MODIFY NAME = " + NewName + ";";
                    string Sql_3_MultiMode = "ALTER DATABASE " + NewName + " SET MULTI_USER;";
                    string conStrMaster = DiversityWorkbench.Settings.ConnectionString.Replace(DiversityWorkbench.Settings.Connection.Database, "master");
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(conStrMaster);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(Sql_1_SingleMode, con);
                    try
                    {
                        con.Open();
                        C.ExecuteNonQuery();
                        try
                        {
                            C.CommandText = Sql_2_Rename;
                            C.ExecuteNonQuery();
                            DiversityWorkbench.Settings.DatabaseName = NewName;
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                            OK = false;

                            // Rename failed, set multimode for old database
                            Sql_3_MultiMode = "ALTER DATABASE " + DiversityWorkbench.Settings.DatabaseName + " SET MULTI_USER;";
                        }
                        C.CommandText = Sql_3_MultiMode;
                        C.ExecuteNonQuery();
                        DiversityWorkbench.Settings.Connection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                        OK = false;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }

                    DiversityWorkbench.Settings.Connection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);

                    if (OK)
                    {
                        Message = "The name of the current database has been changed to\r\n" + NewName + ".\r\nDo you want to adapt the address published by the current database for access by other modules?";
                        if (System.Windows.Forms.MessageBox.Show(Message, "Change name of database", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                            return OK;
                        else DiversityWorkbench.Database.SetBaseURL();
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("Renaming failed");
                }
            }
            catch (System.Exception ex)
            {
            }
            return OK;
        }

        /// <summary>
        /// Adapt the value returned of the function BaseURL to the current server and database
        /// </summary>
        public static void SetBaseURL()
        {
            try
            {
                string SQL = "SELECT [dbo].[BaseURL] ()";
                string OldBaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);

                string BaseURL = Database.BaseURLaccordingToDatabaseName();
                if (OldBaseURL == BaseURL)
                {
                    System.Windows.Forms.MessageBox.Show("The published address is already set according to the current connection:\r\n" + BaseURL);
                    return;
                }

                string Message = "Do you really want to change the basic address published by the current database from\r\n" + OldBaseURL + "\r\nto\r\n" + BaseURL + "\r\n?";
                if (System.Windows.Forms.MessageBox.Show(Message, "Change published address", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
                SQL = "ALTER FUNCTION [dbo].[BaseURL] ()  " +
                    "RETURNS  varchar (255) AS  " +
                    "BEGIN " +
                    "declare @URL varchar(255) " +
                    "set @URL =  '" + BaseURL + "' " +
                    "return @URL " +
                    "END;";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    System.Windows.Forms.MessageBox.Show("Published address changed");
                else
                    System.Windows.Forms.MessageBox.Show("Change of address failed");
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Change of address failed");
            }
        }

        public static string BaseURLaccordingToDatabaseName()
        {
            string BaseURL = "http://" + DiversityWorkbench.Settings.DatabaseServer + "/";
            if (DiversityWorkbench.Settings.DatabaseName.StartsWith("Diversity"))
                BaseURL += DiversityWorkbench.Settings.DatabaseName.Substring("Diversity".Length);
            else BaseURL += DiversityWorkbench.Settings.DatabaseName;
            BaseURL += "/";
            return BaseURL;
        }

        #endregion

        #region Admin check

        private static System.Data.DataTable _DtSysAdmin;
        private static System.Data.DataTable DtSysAdmin
        {
            get
            {
                if (DiversityWorkbench.Database._DtSysAdmin == null)
                {
                    DiversityWorkbench.Database._DtSysAdmin = new System.Data.DataTable();
                    string SQL = "sp_helpsrvrolemember 'sysadmin'";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(DiversityWorkbench.Database._DtSysAdmin);
                }
                return DiversityWorkbench.Database._DtSysAdmin;
            }
        }

        public static bool LoginIsSysAdmin(string LoginName)
        {
            bool IsAdmin = false;
            try
            {
                System.Data.DataRow[] rr = DiversityWorkbench.Database.DtSysAdmin.Select("MemberName = '" + LoginName + "'");
                if (rr.Length > 0)
                {
                    IsAdmin = true;
                }
            }
            catch (Exception ex)
            {
            }
            return IsAdmin;
        }

        #endregion

        #region Privacy consent

        public enum PrivacyConsent { undecided, consented, rejected, missingUser, NoAccess }

        public static PrivacyConsent PrivacyConsentState()
        {
            PrivacyConsent Consent = PrivacyConsent.undecided;
            try
            {
                string Login = "";
                string SQL = "select case when PrivacyConsent = 1 then 'consented' else case when PrivacyConsent is null then 'undecided' else 'rejected' end end " +
                    "from UserProxy where LoginName ";
                if (IsWindowsLogin(ref Login))
                    SQL += "LIKE '%' + char(92) + '" + Login + "'";
                else
                    SQL += "= SUSER_SNAME()";
                string Message = "";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                if (Result == PrivacyConsent.consented.ToString())
                    Consent = PrivacyConsent.consented;
                else if (Result == PrivacyConsent.rejected.ToString())
                    Consent = PrivacyConsent.rejected;
                else if (Result == "")
                {
                    if (Message.Length == 0)
                    {
                        if (DatabaseRoles().Contains("dbo"))
                        {

                        }
                        Consent = PrivacyConsent.missingUser;
                    }
                    else
                        Consent = PrivacyConsent.NoAccess;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Consent;
        }

        public static bool ColumnPrivacyConsentDoesExist()
        {
            bool Exists = false;
            string SQL = "select count(*) " +
                "from INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = 'UserProxy' and C.COLUMN_NAME = 'PrivacyConsent'";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Result == "1")
                Exists = true;
            else if (Result == "")
                Exists = true;
            return Exists;
        }

        public static void SetPrivacyConsentState(bool Consented)
        {
            string SQL = "UPDATE U SET PrivacyConsent = ";
            if (Consented) SQL += "1";
            else SQL += "0";
            SQL += " from UserProxy U where LoginName ";
            string Login = "";
            if (IsWindowsLogin(ref Login))
                SQL += "LIKE '%' + char(92) + '" + Login + "'";
            else
                SQL += "= SUSER_SNAME()";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
        }

        private static bool IsWindowsLogin(ref string Login)
        {
            string SQL = "SELECT SUSER_SNAME()";
            Login = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Login.IndexOf("\\") > -1)
            {
                Login = Login.Substring(Login.IndexOf("\\") + 1);
                return true;
            }
            else
                return false;
        }

        public static bool PrivacyConsentOK(ref PrivacyConsent Consent)
        {
            bool OK = true;
            if (DiversityWorkbench.Database.ColumnPrivacyConsentDoesExist())
            {
                switch (DiversityWorkbench.Database.PrivacyConsentState())
                {
                    case PrivacyConsent.rejected:
                    case PrivacyConsent.undecided:
                        DiversityWorkbench.Forms.FormPrivacyConsent f = new DiversityWorkbench.Forms.FormPrivacyConsent();
                        f.ShowDialog();
                        if (f.PrivacyConsent() == PrivacyConsent.consented)
                        {
                            OK = true;
                            Consent = PrivacyConsent.consented;
                        }
                        else
                        {
                            OK = false;
                            Consent = PrivacyConsent.rejected;
                        }
                        break;
                    case PrivacyConsent.consented:
                        OK = true;
                        Consent = PrivacyConsent.consented;
                        break;
                    case PrivacyConsent.missingUser:
                        OK = true;
                        Consent = PrivacyConsent.missingUser;
                        break;
                    case PrivacyConsent.NoAccess:
                        OK = false;
                        Consent = PrivacyConsent.NoAccess;
                        break;
                }
            }
            return OK;
        }

        public static readonly string PrivacyConsentInfoRoutine = "PrivacyConsentInfo";
        public static bool CreateOrUpdatePrivacyConsentProcedure(string URL)
        {
            bool OK = true;
            try
            {
                string SQL = "select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = '" + PrivacyConsentInfoRoutine + "'";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result == "1") SQL = "ALTER";
                else SQL = "CREATE";
                SQL += " FUNCTION [dbo].[" + PrivacyConsentInfoRoutine + "] () RETURNS varchar (900) AS BEGIN return '" + URL + "' END;";
                string Message = "";
                if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                {
                    System.Windows.Forms.MessageBox.Show(Message);
                    OK = false;
                }
                else
                {
                    if (Result != "1")
                    {
                        string RoleUser = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("Select TOP 1 [name] From sysusers U Where issqlrole = 1 AND U.name like '%user'");
                        if (RoleUser.Length == 0 || RoleUser == null)
                            RoleUser = "USER";
                        SQL = "GRANT EXEC ON [dbo].[" + PrivacyConsentInfoRoutine + "] TO [" + RoleUser + "];";
                        if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                        {
                            System.Windows.Forms.MessageBox.Show(Message);
                            OK = false;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }
        #endregion

        //    alternative code with detach and restore 
        //    string SQL;
        //    System.Data.DataTable dt = new System.Data.DataTable();

        //    // initialize result string
        //    Result = string.Empty;

        //    // retrieve database file names
        //    SQL = "SELECT [physical_name]\r\n" +
        //            string.Format("FROM [{0}].[sys].[database_files]", Database);

        //    try
        //    {
        //        Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        a.Fill(dt);
        //    }
        //    catch (Exception)
        //    {
        //        dt = null;
        //    }

        //    // use connection to master database for further steps
        //    string conStrMaster = DiversityWorkbench.Settings.ConnectionString.Replace(DiversityWorkbench.Settings.Connection.Database, "master");
        //    Microsoft.Data.SqlClient.SqlConnection conMaster = new Microsoft.Data.SqlClient.SqlConnection(conStrMaster);
        //    int SQLerror = 0;

        //    if (dt != null)
        //    {
        //        // detach actual database
        //        SQL = string.Format("USE [master]\r\nGO\r\nALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE\r\nGO\r\n", Database) +
        //              string.Format("USE [master]\r\nGO\r\nEXEC master.dbo.sp_detach_db @dbname = N'{0}'\r\nGO\r\n", Database);

        //        ScriptProcessor sp = new ScriptProcessor(conMaster);
        //        using (sp)
        //        {
        //            sp.Open(SQL, "DetachDB");
        //            SQLerror = sp.Run();
        //            sp.Close();
        //        }
        //    }

        //    // restore database
        //    SQL = string.Format("RESTORE DATABASE [{0}] FROM  DISK = N'{1}' WITH  FILE = 1, NOUNLOAD, REPLACE, STATS = 10", Database, BackupFile);
        //    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, conMaster);
        //    try
        //    {
        //        conMaster.Open();
        //        C.ExecuteNonQuery();
        //        SQLerror = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Result = ex.Message + "\r\nSQL-Statement:\r\n" + SQL + "\r\n\r\n";
        //        SQLerror = 22;
        //    }
        //    finally
        //    {
        //        conMaster.Close();
        //        conMaster.Dispose();
        //    }

        //    if (SQLerror > 0)
        //    {
        //        if (dt != null)
        //        {
        //            // re-attach old database
        //            SQL = string.Format("USE [master]\r\nGO\r\nCREATE DATABASE [{0}] ON ", Database);
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //                SQL += (i == 0 ? "\r\n" : ",\r\n") + string.Format("( FILENAME = N'{0}' )", dt.Rows[i]["physical_name"].ToString());
        //            SQL += "\r\nFOR ATTACH\r\nGO";

        //            ScriptProcessor sp = new ScriptProcessor(conMaster);
        //            using (sp)
        //            {
        //                sp.Open(SQL, "AttachDB");
        //                SQLerror = sp.Run();
        //                sp.Close();
        //            }
        //        }
        //        return false;
        //    }
        //    return true;
        //}

    }
}
