using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.LogDatabase
{
    /*
     * Benoetigte Funktionalitaet:
     * Anlegen der lokalen Tabellen (LogDatabaseSchema, LogDatabaseTable) um Ziele abzulegen
     * Anlegen der Datenbanke
     * Abrufen der Datenbanke
     * evtl. Anlegen eines linkedServers fuer Zugriff
     * Bereitstellung der Daten 
     * Test ob Daten aus Tabelle abgelegt wurden
     * 
     * es wird eine zentrale Log DB angelegt und darin Schemata mit Namen Log_ + Datum, e.g. Log_20181127
     * beim Sichern des Log werden die Daten ab dem gewaehlten Zeitpunkt fuer jede Tabelle die gewaehlt wurde in dieses Schema geschrieben
     * */
    public class Database
    {
        #region Start date

        private static DateTime _StartDate;
        private static string _DateComparer;
        private static void setDateComparer(System.DateTime StartDate)
        {
            _StartDate = StartDate;
            string DateCompare = "CONVERT(DATETIME, '" + StartDate.Year.ToString() + "-";
            if (StartDate.Month < 10)
                DateCompare += "0";
            DateCompare += StartDate.Month.ToString() + "-";
            if (StartDate.Day < 10)
                DateCompare += "0";
            DateCompare += StartDate.Day.ToString() + " 00:00:00', 102)";
            _DateComparer = DateCompare;
        }

        public static string DateComparer
        {
            get
            {
                if (_DateComparer == null)
                    setDateComparer(System.DateTime.Now);
                return _DateComparer;
            }
        }
        
        #endregion

        #region Tables

        public static System.Collections.Generic.Dictionary<string, HistoryTable> HistoryTables(System.Collections.Generic.Dictionary<string, int> Keys)
        {
            System.Collections.Generic.Dictionary<string, HistoryTable> Dict = new Dictionary<string, HistoryTable>();
            string SQL = "select T.TABLE_NAME from INFORMATION_SCHEMA.TABLES T, INFORMATION_SCHEMA.TABLES L " +
                "where T.TABLE_NAME + '_log' = L.TABLE_NAME " +
                "order by T.TABLE_NAME";
            System.Data.DataTable DtPotentialLogTables = new System.Data.DataTable();
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref DtPotentialLogTables, ref Message);
            if (Keys.Count > 0)
            {
                foreach (System.Data.DataRow R in DtPotentialLogTables.Rows)
                {
                    DiversityWorkbench.Data.Table T = new Data.Table(R[0].ToString(), DiversityWorkbench.Settings.ConnectionString);
                    foreach (System.Collections.Generic.KeyValuePair<string, int> KV in Keys)
                    {
                        if (T.PrimaryKeyColumnList.Contains(KV.Key))
                        {
                            HistoryTable HT = new HistoryTable(KV.Value, KV.Key, T.Name);
                            HT.FillTable();
                            if (HT.DataTable().Rows.Count > 0)
                                Dict.Add(HT.Name(), HT);
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (System.Data.DataRow R in DtPotentialLogTables.Rows)
                {
                }
            }
            return Dict;
        }

        private static System.Data.DataTable _dtTableCounts;
        /// <summary>
        /// The tables and the counts of line that are transferred resp. kept with a certain date
        /// </summary>
        /// <param name="StartDate">The latest date of the logged data that should be transferred into the log database</param>
        /// <returns>a table containing the table names and the numbers</returns>
        public static System.Data.DataTable dtTableCounts(System.DateTime StartDate)
        {
            _dtTableCounts = new System.Data.DataTable();
            _StartDate = StartDate;
            string SQL = "select L.TABLE_NAME AS [Tablename], 0 AS [Transferred], '' AS [Start date], '' AS [End date], 0 AS [Kept] " +
                "from INFORMATION_SCHEMA.TABLES T, INFORMATION_SCHEMA.TABLES L, INFORMATION_SCHEMA.COLUMNS C " +
                "where T.TABLE_NAME + '_Log' = L.TABLE_NAME " +
                "and L.TABLE_NAME = C.TABLE_NAME " +
                "and C.COLUMN_NAME = 'LogDate' " +
                "order by L.TABLE_NAME";
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _dtTableCounts, ref Message);
            setDateComparer(StartDate);
            foreach (System.Data.DataRow R in _dtTableCounts.Rows)
            {
                SQL = "SELECT COUNT(*) AS Anzahlrt " +
                    "FROM " + R[0].ToString() + " AS L " +
                    "WHERE LogDate > " + DateComparer;
                int i = 0;
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i))
                    R[4] = i;
                SQL = "SELECT COUNT(*) AS Anzahl, convert(varchar(10), Min(LogDate), 120) as [Start date], convert(varchar(10), Max(LogDate), 120) AS [End date] " +
                    "FROM " + R[0].ToString() + " AS L " +
                    "WHERE LogDate <= " + DateComparer;
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                if (int.TryParse(dt.Rows[0][0].ToString(), out i))
                {
                    R[1] = i;
                    R[2] = dt.Rows[0][1].ToString();
                    R[3] = dt.Rows[0][2].ToString();
                }
                if (int.Parse(R[1].ToString()) == 0 && int.Parse(R[4].ToString()) == 0)
                    R.Delete();
            }
            _dtTableCounts.AcceptChanges();
            return _dtTableCounts;
        }

        public static bool TransferTables(ref string Message)
        {
            bool OK = true;
            System.Collections.Generic.Dictionary<string, int> Keys = new Dictionary<string,int>();
            foreach (System.Collections.Generic.KeyValuePair<string, HistoryTable> HT in HistoryTables(Keys))
            {
                HT.Value.TransferDataToLog(ref Message);
            }

            //foreach (System.Data.DataRow R in _dtTableCounts.Rows)
            //{
            //    bool TT = TransferTableContent(R[0].ToString());
            //    if (!TT)
            //    {
            //        OK = false;
            //        if (Message.Length == 0)
            //            Message = "Transfer failed for the following tables:\r\n";
            //        Message += R[0].ToString() + "\r\n";
            //    }
            //    else
            //    {
            //        OK = GrantSelectOnTable(R[0].ToString());
            //    }
            //}
            return OK;
        }

        //private static bool GrantSelectOnTable(string Table)
        //{
        //    bool OK = true;
        //    string SQL = "GRANT SELECT ON " + _LogSchema + "." + Table + " TO LogUser;";
        //    OK = SqlExecuteNonQuery(SQL);
        //    return OK;
        //}

        //private static bool TransferTableContent(string Table)
        //{
        //    bool OK = true;
        //    string SQL =
        //        "BEGIN TRY " +
        //        " BEGIN TRANSACTION " +
        //        SqlTransferTableContent(Table) +
        //        " COMMIT " +
        //        "END TRY " +
        //        "BEGIN CATCH " +
        //        " ROLLBACK " +
        //        "END CATCH ";
        //    OK = SqlExecuteNonQuery(SQL);
        //    return OK;
        //}

        //private static string SqlTransferTableContent(string Table)
        //{
        //    string SQL = "SELECT C.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS C " +
        //        "WHERE C.TABLE_NAME = '" + Table + "' " +
        //        "ORDER BY C.ORDINAL_POSITION ";
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    string Message = "";
        //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
        //    string SqlColumns = "";
        //    foreach (System.Data.DataRow R in dt.Rows)
        //    {
        //        if (SqlColumns.Length > 0)
        //            SqlColumns += ", ";
        //        SqlColumns += R[0].ToString();
        //    }
        //    SQL = "SELECT " + SqlColumns + " INTO " + _LogSchema + "." + Table + " FROM [" + DiversityWorkbench.Settings.DatabaseName + "].dbo." + Table + " WHERE LogDate <= " + DateComparer + ";";
        //    SQL += "DELETE FROM [" + DiversityWorkbench.Settings.DatabaseName + "].dbo." + Table + " WHERE LogDate <= " + DateComparer + ";";
        //    return SQL;
        //}

        #endregion

        #region Database

        /// <summary>
        /// Create the tables in the local DB 
        /// </summary>
        /// <returns></returns>
        public static bool InitLogging()
        {
            bool OK = initDatabase();
            return OK;
        }

        private static bool initDatabase()
        {
            bool OK = true;
            try
            {
                // Check if DB exists
                string DBname = DiversityWorkbench.Settings.DatabaseName + "_Log";
                string SQL = "select [" + DBname + "].dbo.DiversityWorkbenchModule()";
                bool DbExists = false;
                string Module = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Module.ToLower() == "diversityworkbenchlog")
                    DbExists = true;
                if (!DbExists)
                {
                    // Create DB
                    if (System.Windows.Forms.MessageBox.Show("So far no log database has been defined.\r\nDo you want to create a log database?", "Create log database?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Data.DataTable dtFiles = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter("exec sp_helpfile", DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dtFiles);
                        string DataFile = dtFiles.Rows[0]["filename"].ToString();
                        string LogFile = "";
                        System.IO.FileInfo FI = new System.IO.FileInfo(DataFile);
                        string Directory = FI.DirectoryName;
                        DiversityWorkbench.Forms.FormGetString fDir = new DiversityWorkbench.Forms.FormGetString("Directory", "If you change the default directory\r\n" + Directory + " \r\nPlease make sure this directory does exist", Directory);
                        fDir.ShowDialog();
                        if (fDir.DialogResult == System.Windows.Forms.DialogResult.OK && fDir.String.Length > 0)
                        {
                            DataFile = fDir.String + "\\" + DBname + ".mdf";
                            LogFile = fDir.String + "\\" + DBname + "_log.ldf";
                        }
                        SQL = "CREATE DATABASE [" + DBname + "] " +
                            "CONTAINMENT = NONE " +
                            "ON  PRIMARY  " +
                            "( NAME = N'" + DBname + "', FILENAME = N'" + DataFile + "' , SIZE = 5032KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%) " +
                            "LOG ON  " +
                            "( NAME = N'" + DBname + "_Log', FILENAME = N'" + LogFile + "' , SIZE = 504KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%) ";
                        string Message = "";
                        try
                        {
                            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                            {
                                SQL = "ALTER DATABASE " + DBname + " SET RECOVERY SIMPLE;";
                                OK = SqlExecuteNonQuery(SQL);
                                if (OK)
                                {
                                    OK = CreateUserRole();
                                    if (OK)
                                    {
                                        OK = CreateFunctionDiversityworkbenchLog();
                                        if (OK)
                                        {
                                            OK = CopyLogins(ref Message);
                                            if (!OK && Message.Length > 0)
                                            {
                                                System.Windows.Forms.MessageBox.Show("Transfer of users failed:\r\n" + Message);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show("Creation of cache database failed: " + Message + "\r\nYour database server may be of an old version.\r\nPlease update to e.g. SQL-Server 2014");
                            OK = false;
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    else
                        OK = false;
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("No log databases available");
                OK = false;
            }
            return OK;
        }

        private static bool CreateFunctionDiversityworkbenchLog()
        {
            bool OK = true;
            string SQL = "CREATE FUNCTION [dbo].[DiversityWorkbenchModule] () RETURNS nvarchar(50) AS BEGIN RETURN 'DiversityWorkbenchLog' END";
            OK = SqlExecuteNonQuery(SQL);
            if (OK)
            {
                SQL = "GRANT EXEC ON [dbo].[DiversityWorkbenchModule] TO [LogUser]";
                OK = SqlExecuteNonQuery(SQL);
            }
            return OK;
        }

        public static bool Exists()
        {
            bool OK = false;
            string SQL = "SELECT [" + DiversityWorkbench.Settings.DatabaseName + "_Log].[dbo].[DiversityWorkbenchModule] ();";
            string Message = "";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
            if (Result == "DiversityWorkbenchLog")
            {
                if (Schemata().Count > 0)
                    OK = true;
            }
            return OK;
        }
        
        #endregion

        public static bool TransferData(ref string Message) //System.DateTime StartDate)
        {
            if (_StartDate == null)
                return false;
            bool OK = true;
            //_StartDate = StartDate;
            foreach (System.Data.DataRow R in _dtTableCounts.Rows)
            {
                string Table = R[0].ToString();
                int i;
                if (int.TryParse(R[1].ToString(), out i) && i == 0)
                    continue;
                if (Table.ToLower().EndsWith("_log"))
                    Table = Table.Substring(0, Table.Length - 4);
                HistoryTable HT = new HistoryTable(Table);
                if (!HT.TransferDataToLog(ref Message))
                    OK = false;
            }
            return OK;
        }

        #region Logins and roles

        private static bool CreateUserRole()
        {
            bool OK = true;
            string SQL = "CREATE ROLE [LogUser];";
            OK = SqlExecuteNonQuery(SQL);
            return OK;
        }

        public static bool CopyLogins(ref string Message)
        {
            bool OK = true;
            string SQL = "SELECT distinct " +
            "[UserName] = CASE memberprinc.[type]  " +
            "WHEN 'S' THEN memberprinc.[name] " +
            "WHEN 'U' THEN case when ulogin.[name] is null then memberprinc.[name] end COLLATE Latin1_General_CI_AI " +
            "END " +
            "FROM     " +
            "sys.database_role_members members " +
            "JOIN " +
            "sys.database_principals roleprinc ON roleprinc.[principal_id] = members.[role_principal_id] " +
            "JOIN " +
            "sys.database_principals memberprinc ON memberprinc.[principal_id] = members.[member_principal_id] " +
            "LEFT JOIN " +
            "sys.login_token ulogin on memberprinc.[sid] = ulogin.[sid] " +
            "LEFT JOIN         " +
            "sys.database_permissions perm ON perm.[grantee_principal_id] = roleprinc.[principal_id] " +
            "JOIN " +
            "sys.objects obj ON perm.[major_id] = obj.[object_id] " +
            "where perm.[state_desc] =  'GRANT' " +
            "and perm.[permission_name] = 'SELECT' " +
            "and obj.type_desc = 'USER_TABLE' " +
            "and OBJECT_NAME(perm.major_id) like '%_log' " +
            "and memberprinc.[type] in ('S', 'U')";
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            Message = "";
            foreach (System.Data.DataRow R in dt.Rows)
            {
                SQL = "use " + DiversityWorkbench.Settings.DatabaseName + "_Log; " +
                    "IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + R[0].ToString() + "') " +
                    "DROP USER [" + R[0].ToString() + "] " +
                    "IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + R[0].ToString() + "') " +
                    "CREATE USER [" + R[0].ToString() + "] FOR LOGIN [" + R[0].ToString() + "] WITH DEFAULT_SCHEMA=[dbo];";
                OK = SqlExecuteNonQuery(SQL);
                if (OK)
                {
                    SQL = "sp_addrolemember 'LogUser', '" + R[0].ToString() + "';";
                    OK = SqlExecuteNonQuery(SQL);
                    if (!OK)
                        Message += "Adding of " + R[0].ToString() + " to role LogUser failed\r\n";
                }
                else
                {
                    Message += "Could not create the database user " + R[0].ToString() + "\r\n";
                }
            }
            return OK;
        }

       
        #endregion

        #region Schema

        private static string _LogSchema;

        private static string CreateLogSchema()//System.DateTime StartDate)
        {
            string Schema = "Log_" + _StartDate.Year.ToString();
            if (_StartDate.Month < 10)
                Schema += "0";
            Schema += _StartDate.Month.ToString();
            if (_StartDate.Day < 10)
                Schema += "0";
            Schema += _StartDate.Day.ToString();
            string SQL = "CREATE SCHEMA [" + Schema + "]";
            if (!SqlExecuteNonQuery(SQL))
                Schema = "";
            else
            {
                SQL = "GRANT SELECT ON SCHEMA :: [" + Schema + "] TO [LogUser]";
                if (!SqlExecuteNonQuery(SQL))
                    Schema = "";
            }
            _LogSchema = Schema;
            return _LogSchema;
        }

        public static string LogSchema() //System.DateTime StartDate)
        {
            //_StartDate = StartDate;
            if (_LogSchema == null || _LogSchema.Length == 0)
                CreateLogSchema();//_StartDate);
            return _LogSchema;
        }

        private static System.Collections.Generic.Dictionary<string, System.DateTime> _Schemata;
        public static System.Collections.Generic.Dictionary<string, System.DateTime> Schemata()
        {
            if (_Schemata == null || _Schemata.Count == 0)
            {
                try
                {
                    _Schemata = new Dictionary<string, DateTime>();
                    string SQL = "select S.SCHEMA_NAME from [" + DiversityWorkbench.Settings.DatabaseName + "_Log].INFORMATION_SCHEMA.SCHEMATA S " +
                        "where S.SCHEMA_NAME like 'Log_%' ORDER BY S.SCHEMA_NAME";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    string Message = "";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string Schema = R[0].ToString();
                        int Year;
                        if (int.TryParse(Schema.Substring(4, 4), out Year))
                        {
                            int Month;
                            if (int.TryParse(Schema.Substring(8, 2), out Month))
                            {
                                int Day;
                                if (int.TryParse(Schema.Substring(10, 2), out Day))
                                {
                                    System.DateTime D = new DateTime(Year, Month, Day);
                                    _Schemata.Add(Schema, D);
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
            return _Schemata;
        }

        public static void ResetSchemata()
        {
            _Schemata = null;
        }

        #endregion

        #region Auxillary
        
        private static bool SqlExecuteNonQuery(string SQL)
        {
            bool OK = true;
            Microsoft.Data.SqlClient.SqlConnection Con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionStringLogDB);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, Con);
            try
            {
                Con.Open();
                C.ExecuteNonQuery();
                Con.Close();
            }
            catch (System.Exception ex)
            {
                OK = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private static string _ConnectionStringLogDB;
        
        public static string ConnectionStringLogDB
        {
            get
            {
                if (_ConnectionStringLogDB == null || _ConnectionStringLogDB.Length == 0)
                {
                    if (DiversityWorkbench.Settings.DatabaseServer == null
                        || DiversityWorkbench.Settings.DatabaseServer.Length == 0
                        || DiversityWorkbench.Settings.DatabaseName == null
                        || DiversityWorkbench.Settings.DatabaseName.Length == 0)
                        return "";
                }
                _ConnectionStringLogDB = "Data Source=" + DiversityWorkbench.Settings.DatabaseServer + "," + DiversityWorkbench.Settings.DatabasePort.ToString();
                _ConnectionStringLogDB += ";initial catalog=" + DiversityWorkbench.Settings.DatabaseName + "_Log;";
                if (DiversityWorkbench.Settings.IsTrustedConnection)
                    _ConnectionStringLogDB += "Integrated Security=True";
                else
                {
                    if (DiversityWorkbench.Settings.DatabaseUser.Length > 0 && DiversityWorkbench.Settings.Password.Length > 0)
                        _ConnectionStringLogDB += "user id=" + DiversityWorkbench.Settings.DatabaseUser + ";password=" + DiversityWorkbench.Settings.Password;
                    else _ConnectionStringLogDB = "";
                }
                _ConnectionStringLogDB += ";Connection Timeout=" + DiversityWorkbench.Settings.TimeoutDatabase.ToString() + ";";

                // MW 2018/10/01: Encripted connection
                if (_ConnectionStringLogDB.Length > 0 && DiversityWorkbench.Settings.IsEncryptedConnection)
                    _ConnectionStringLogDB += ";Encrypt=true;TrustServerCertificate=true";

                return _ConnectionStringLogDB;
            }
        }
        
        #endregion

    }
}
