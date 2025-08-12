using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.LogDatabase
{
    public class HistoryTable
    {

        #region Parameter

        private int _ID;
        private string _PkColumn;
        private string _DataTable;
        private string _VersionTable;
        private bool _ShowAll;
        private bool _LinkToUserProxy;
        private string _Restriction;
        private System.Data.DataTable _DtHistory;
        private System.Data.DataTable _DtColumns;
        private System.Data.DataTable _DtColumnsLog;

        
        #endregion

        #region Construction

        /// <summary>
        /// A table containing the history of the datasets based on the main table, the log table and potential saved log tables
        /// </summary>
        /// <param name="ID">The value of a unique PK in the current dataset or of a superior table</param>
        /// <param name="PkColumn">The name of the column of the PK containing the ID</param>
        /// <param name="DataTable">The name of the main table</param>
        /// <param name="VersionTable">The table containing the version</param>
        /// <param name="ShowAll">'true' if all columns should be inserted</param>
        /// <param name="LinkToUserProxy">If the content of columns, e.g. LogUpdatedBy should be filled with the names of the user retrieved from the table UserProxy</param>
        /// <param name="Restriction">A restriction for the selection of the data</param>
        /// <returns>A table containing the current state and the history of the datasets related to an ID</returns>
        public HistoryTable(
            int ID,
            string PkColumn,
            string DataTable,
            string VersionTable = "",
            bool ShowAll = true,
            bool LinkToUserProxy = false,
            string Restriction = "")
        {
            this._ID = ID;
            this._PkColumn = PkColumn;
            this._DataTable = DataTable;
            this._VersionTable = VersionTable;
            this._ShowAll = ShowAll;
            this._LinkToUserProxy = LinkToUserProxy;
            this._Restriction = Restriction;

            this._DtHistory = new System.Data.DataTable(DataTable);

            //string Message = "";
            //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(this.SQL(Targets.Current), ref this._DtHistory, ref Message);
            //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(this.SQL(Targets.Log), ref this._DtHistory, ref Message);

            //System.Data.DataTable dtColumns = new System.Data.DataTable();
            //string SQL = "select C.COLUMN_NAME, C.DATA_TYPE " +
            //    "from INFORMATION_SCHEMA.COLUMNS C, INFORMATION_SCHEMA.COLUMNS CL " +
            //    "where C.TABLE_NAME = '" + DataTable + "' " +
            //    "and CL.TABLE_NAME = '" + DataTable + "_log' " +
            //    "and C.COLUMN_NAME = CL.COLUMN_NAME " +
            //    "ORDER BY C.ORDINAL_POSITION";
            //try
            //{
            //    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //    a.Fill(dtColumns);
            //}
            //catch (Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            //}
            //System.Data.DataTable dtLogColumns = new System.Data.DataTable();
            //SQL = "select C.COLUMN_NAME, C.DATA_TYPE " +
            //    "from INFORMATION_SCHEMA.COLUMNS C " +
            //    "where C.TABLE_NAME = '" + DataTable + "_log' " +
            //    "ORDER BY C.ORDINAL_POSITION";
            //try
            //{
            //    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //    a.Fill(dtLogColumns);
            //}
            //catch (Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            //}
            //SQL = "SELECT ";
            //string SqlCurrent = "SELECT ";
            //if (VersionTable != DataTable
            //    && VersionTable.Length > 0)
            //{
            //    System.Data.DataRow[] rr = dtLogColumns.Select("COLUMN_NAME = 'LogVersion'", "");
            //    if (rr.Length > 0)
            //    {
            //        SqlCurrent += VersionTable + ".Version, ";
            //        SQL += DataTable + "_log.LogVersion AS Version, ";
            //    }
            //}
            //else if (VersionTable.Length > 0)
            //{
            //    SqlCurrent += " Version, ";
            //    SQL += " Version, ";
            //}
            //foreach (System.Data.DataRow R in dtColumns.Rows)
            //{
            //    try
            //    {
            //        if (ShowAll ||
            //            (!R["COLUMN_NAME"].ToString().StartsWith("Log")
            //            && !R["COLUMN_NAME"].ToString().StartsWith("xx_")
            //            && R["COLUMN_NAME"].ToString() != "RowGUID"))
            //        {
            //            if (R["DATA_TYPE"].ToString() == "date" || R["DATA_TYPE"].ToString() == "datetime" || R["DATA_TYPE"].ToString() == "datetime2" || R["DATA_TYPE"].ToString() == "smalldatetime")
            //            {
            //                SqlCurrent += "convert(varchar(20), [" + DataTable + "]." + R["COLUMN_NAME"].ToString() + ", 120) AS " + R["COLUMN_NAME"].ToString() + ", ";
            //                SQL += "convert(varchar(20), [" + DataTable + "_log]." + R["COLUMN_NAME"].ToString() + ", 120) AS " + R["COLUMN_NAME"].ToString() + ", ";
            //            }
            //            else
            //            {
            //                if (VersionTable != DataTable)
            //                {
            //                    if (R["DATA_TYPE"].ToString().ToLower() == "geography" || R["DATA_TYPE"].ToString().ToLower() == "geometry")
            //                    {
            //                        SqlCurrent += "[" + DataTable + "]." + R["COLUMN_NAME"].ToString() + ".ToString() AS " + R["COLUMN_NAME"].ToString() + ", ";
            //                        SQL += DataTable + "_log." + R["COLUMN_NAME"].ToString() + ".ToString() AS " + R["COLUMN_NAME"].ToString() + ", ";
            //                    }
            //                    else
            //                    {
            //                        if (LinkToUserProxy &&
            //                            (R["COLUMN_NAME"].ToString() == "LogUpdatedBy" ||
            //                            R["COLUMN_NAME"].ToString() == "LogCreatedBy" ||
            //                            R["COLUMN_NAME"].ToString() == "LogInsertedBy"))
            //                        {
            //                            SqlCurrent += "dbo.UserName([" + DataTable + "]." + R["COLUMN_NAME"].ToString() + ") AS " + R["COLUMN_NAME"].ToString() + ", ";
            //                            SQL += "dbo.UserName(" + DataTable + "_log." + R["COLUMN_NAME"].ToString() + " ) AS " + R["COLUMN_NAME"].ToString() + ", ";
            //                        }
            //                        else if (R["DATA_TYPE"].ToString() == "datetime")
            //                        {
            //                            SqlCurrent += "convert(varchar(20), [" + DataTable + "]." + R["COLUMN_NAME"].ToString() + ", 120) AS " + R["COLUMN_NAME"].ToString() + ", ";
            //                            SQL += "convert(varchar(20), [" + DataTable + "_log]." + R["COLUMN_NAME"].ToString() + ", 120) AS " + R["COLUMN_NAME"].ToString() + ", ";
            //                        }
            //                        else
            //                        {
            //                            SqlCurrent += "[" + DataTable + "]." + R["COLUMN_NAME"].ToString() + ", ";
            //                            SQL += DataTable + "_log." + R["COLUMN_NAME"].ToString() + ", ";
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    if (LinkToUserProxy &&
            //                        (R["COLUMN_NAME"].ToString() == "LogUpdatedBy" ||
            //                        R["COLUMN_NAME"].ToString() == "LogCreatedBy" ||
            //                        R["COLUMN_NAME"].ToString() == "LogInsertedBy"))
            //                    {
            //                        SqlCurrent += "dbo.UserName(" + R["COLUMN_NAME"].ToString() + ") AS " + R["COLUMN_NAME"].ToString() + " , ";
            //                        SQL += "dbo.UserName(" + R["COLUMN_NAME"].ToString() + ") AS " + R["COLUMN_NAME"].ToString() + " , ";
            //                    }
            //                    else
            //                    {
            //                        SqlCurrent += R["COLUMN_NAME"].ToString() + ", ";
            //                        SQL += R["COLUMN_NAME"].ToString() + ", ";
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {

            //        }
            //    }
            //    catch (System.Exception ex)
            //    {
            //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            //    }
            //}
            //if (VersionTable != DataTable)
            //{
            //    SqlCurrent += "'current version' AS [Kind of change], convert(varchar(20), [" + DataTable + "].LogUpdatedWhen, 120) AS [Date of change], ";
            //    if (LinkToUserProxy)
            //        SqlCurrent += "dbo.UserName([" + DataTable + "].LogUpdatedBy)";
            //    else
            //        SqlCurrent += "[" + DataTable + "].LogUpdatedBy";
            //    SqlCurrent += " AS [Responsible user], NULL AS LogID " +
            //        "FROM [" + DataTable + "]";
            //    if (VersionTable.Length > 0)
            //    {
            //        SqlCurrent += " INNER JOIN " +
            //        VersionTable + " ON [" + DataTable + "]." + PkColumn + " = " + VersionTable + "." + PkColumn;
            //    }
            //    SqlCurrent += " WHERE [" + DataTable + "]." + PkColumn + " = " + ID.ToString();
            //    SQL += "CASE WHEN " + DataTable + "_log.LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], " +
            //        "convert(varchar(20), " + DataTable + "_log.LogDate, 120) AS [Date of change], ";
            //    if (LinkToUserProxy)
            //        SQL += "dbo.UserName(" + DataTable + "_log.LogUser)";
            //    else
            //        SQL += DataTable + "_log.LogUser ";
            //    SQL += " AS [Responsible user], " + DataTable + "_log.LogID  " +
            //        "FROM  " + DataTable + "_log " +
            //        "WHERE " + PkColumn + " = " + ID.ToString() +
            //        " ORDER BY " + DataTable + "_log.LogID DESC ";
            //}
            //else
            //{
            //    SqlCurrent += "'current version' AS [Kind of change], convert(varchar(20), LogUpdatedWhen, 120) AS [Date of change], ";
            //    if (LinkToUserProxy)
            //        SqlCurrent += "dbo.UserName(" + DataTable + ".LogUpdatedBy)";
            //    else
            //        SqlCurrent += DataTable + ".LogUpdatedBy ";
            //    SqlCurrent += " AS [Responsible user], NULL AS LogID " +
            //        "FROM " + DataTable +
            //        " WHERE " + PkColumn + " = " + ID.ToString();
            //    SQL += "CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], " +
            //        "convert(varchar(20), LogDate, 120) AS [Date of change], ";
            //    if (LinkToUserProxy)
            //        SQL += "dbo.UserName(LogUser)";
            //    else
            //        SQL += "LogUser";
            //    SQL += " AS [Responsible user], LogID  " +
            //        "FROM  " + DataTable + "_log " +
            //        "WHERE " + PkColumn + " = " + ID.ToString() +
            //        " ORDER BY LogID DESC ";
            //}
            //try
            //{
            //    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
            //    a.Fill(this._DtHistory);
            //    a.SelectCommand.CommandText = SQL;
            //    a.Fill(this._DtHistory);
            //}
            //catch (Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            //}
        }

        /// <summary>
        /// A table containing the history of the datasets based on the main table in the current database for transfer into the log database
        /// </summary>
        /// <param name="DataTable">The name of the main table</param>
        public HistoryTable(string DataTable)
        {
            this._DataTable = DataTable;
        }
        
        #endregion

        #region Interface

        public bool FillTable()
        {
            bool OK = true;
            string Message = "";
            this._DtHistory.Clear();
            OK = DiversityWorkbench.Forms.FormFunctions.SqlFillTable(this.SQL(Targets.Current), ref this._DtHistory, ref Message);
            if (OK)
                OK = DiversityWorkbench.Forms.FormFunctions.SqlFillTable(this.SQL(Targets.Log), ref this._DtHistory, ref Message);
            return OK;
        }

        public System.Data.DataTable DataTable() { return this._DtHistory; }

        public void setVersionTable(string VersionTable)
        {
            this._VersionTable = VersionTable;
        }

        public void setID(int ID) { this._ID = ID; }

        public string Name() { return this._DataTable; }

        public bool SavedLogDataPresent()
        {
            bool Present = false;
            if (DiversityWorkbench.LogDatabase.Database.Exists())
            {
                if (DiversityWorkbench.LogDatabase.Database.Schemata().Count > 0)
                {
                    foreach(System.Collections.Generic.KeyValuePair<string, System.DateTime> KV in DiversityWorkbench.LogDatabase.Database.Schemata())
                    {
                        string SQL = "select count(*) from [" + DiversityWorkbench.Settings.DatabaseName + "_log].[" + KV.Key + "].[" + _DataTable + "_log] where [" + _PkColumn + "] = '" + _ID.ToString() + "'";
                        string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        int i;
                        if (int.TryParse(Result, out i) && i > 0)
                        {
                            Present = true;
                            break;
                        }
                    }
                }
            }
            return Present;
        }

        public void GetSavedLogData()
        {
            string Message = "";
            foreach (System.Collections.Generic.KeyValuePair<string, System.DateTime> KV in DiversityWorkbench.LogDatabase.Database.Schemata())
            {
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(this.SQL(Targets.SavedLog, KV.Key), ref this._DtHistory, ref Message);
            }
        }

        public bool TransferDataToLog(ref string Message)
        {
            bool OK = true;
            string SqlColumns = "";
            foreach (System.Data.DataRow R in this.DtColumnsLog.Rows)
            {
                if (SqlColumns.Length > 0)
                    SqlColumns += ", ";
                SqlColumns += R[0].ToString();
            }
            //string Schema = Database.LogSchema(StartDate);
            string SQL =
                "BEGIN TRY " +
                " BEGIN TRANSACTION " +
                " SELECT " + SqlColumns + " INTO [" + DiversityWorkbench.Settings.DatabaseName + "_log].[" + Database.LogSchema() + "].[" + this._DataTable + "_log] " +
                " FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[" + _DataTable + "_log] WHERE LogDate <= " + Database.DateComparer + ";" +
                " DELETE FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[" + this._DataTable + "_log] WHERE LogDate <= " + Database.DateComparer + 
                " COMMIT " +
                "END TRY " +
                "BEGIN CATCH " +
                " ROLLBACK " +
                "END CATCH ";
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
            if (OK)
                OK = GrantSelectOnTable(ref Message);
            return OK;
        }
        
        #endregion

        #region Auxillary

        private System.Data.DataTable DtColumnsLog
        {
            get
            {
                if (this._DtColumnsLog == null)
                {
                    this._DtColumnsLog = new System.Data.DataTable();
                    string SQL = "select C.COLUMN_NAME, C.DATA_TYPE " +
                        "from INFORMATION_SCHEMA.COLUMNS C " +
                        "where C.TABLE_NAME = '" + _DataTable + "_log' " +
                        "ORDER BY C.ORDINAL_POSITION";
                    try
                    {
                        string Message = "";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtColumnsLog, ref Message);
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                    }
                }
                return _DtColumnsLog;
            }
        }

        private bool GrantSelectOnTable(ref string Message)
        {
            bool OK = true;
            string SQL = "USE [" + DiversityWorkbench.Settings.DatabaseName + "_log]; GRANT SELECT ON [" + Database.LogSchema() + "].[" + this._DataTable + "_log] TO LogUser;";
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
            return OK;
        }

        private enum Targets { Current, Log, SavedLog }
        private string SQL(Targets Target = Targets.Log, string Schema = "dbo")
        {
            string Prefix = "";
            switch (Target)
            {
                case Targets.SavedLog:
                    Prefix = "[" + DiversityWorkbench.Settings.DatabaseName + "_Log" + "].[" + Schema + "].";
                    break;
            }
            string SQL = "";
            string Message = "";
            if (this._DtColumns == null)
            {
                this._DtColumns = new System.Data.DataTable();
                SQL = "select C.COLUMN_NAME, C.DATA_TYPE " +
                    "from INFORMATION_SCHEMA.COLUMNS C, INFORMATION_SCHEMA.COLUMNS CL " +
                    "where C.TABLE_NAME = '" + _DataTable + "' " +
                    "and CL.TABLE_NAME = '" + _DataTable + "_log' " +
                    "and C.COLUMN_NAME = CL.COLUMN_NAME " +
                    "ORDER BY C.ORDINAL_POSITION";
                try
                {
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtColumns, ref Message);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                }
            }

            SQL = "SELECT ";

            if (_VersionTable != _DataTable
                && _VersionTable.Length > 0)
            {
                System.Data.DataRow[] rr = this.DtColumnsLog.Select("COLUMN_NAME = 'LogVersion'", "");
                if (rr.Length > 0)
                {
                    if (Target == Targets.Current)
                        SQL += " V.Version, ";
                    else
                        SQL += " T.LogVersion AS Version, ";
                }
            }
            else if (_VersionTable.Length > 0)
            {
                SQL += " V.Version, ";
            }
            foreach (System.Data.DataRow R in this._DtColumns.Rows)
            {
                try
                {
                    if (_ShowAll ||
                        (!R["COLUMN_NAME"].ToString().StartsWith("Log")
                        && !R["COLUMN_NAME"].ToString().StartsWith("xx_")
                        && R["COLUMN_NAME"].ToString() != "RowGUID"))
                    {
                        if (R["DATA_TYPE"].ToString() == "date" || R["DATA_TYPE"].ToString() == "datetime" || R["DATA_TYPE"].ToString() == "datetime2" || R["DATA_TYPE"].ToString() == "smalldatetime")
                        {
                            SQL += "convert(varchar(20), T." + R["COLUMN_NAME"].ToString() + ", 120) AS " + R["COLUMN_NAME"].ToString() + ", ";
                        }
                        else
                        {
                            if (_VersionTable != _DataTable)
                            {
                                if (R["DATA_TYPE"].ToString().ToLower() == "geography" || R["DATA_TYPE"].ToString().ToLower() == "geometry")
                                {
                                    SQL += "T." + R["COLUMN_NAME"].ToString() + ".ToString() AS " + R["COLUMN_NAME"].ToString() + ", ";
                                }
                                else
                                {
                                    if (_LinkToUserProxy &&
                                        (R["COLUMN_NAME"].ToString() == "LogUpdatedBy" ||
                                        R["COLUMN_NAME"].ToString() == "LogCreatedBy" ||
                                        R["COLUMN_NAME"].ToString() == "LogInsertedBy"))
                                    {
                                        SQL += "dbo.UserName(T." + R["COLUMN_NAME"].ToString() + ") AS " + R["COLUMN_NAME"].ToString() + ", ";
                                    }
                                    else if (R["DATA_TYPE"].ToString() == "datetime")
                                    {
                                        SQL += "convert(varchar(20), T." + R["COLUMN_NAME"].ToString() + ", 120) AS " + R["COLUMN_NAME"].ToString() + ", ";
                                    }
                                    else
                                    {
                                        SQL += "T." + R["COLUMN_NAME"].ToString() + ", ";
                                    }
                                }
                            }
                            else
                            {
                                if (_LinkToUserProxy &&
                                    (R["COLUMN_NAME"].ToString() == "LogUpdatedBy" ||
                                    R["COLUMN_NAME"].ToString() == "LogCreatedBy" ||
                                    R["COLUMN_NAME"].ToString() == "LogInsertedBy"))
                                {
                                    SQL += "dbo.UserName(T." + R["COLUMN_NAME"].ToString() + ") AS " + R["COLUMN_NAME"].ToString() + " , ";
                                }
                                else
                                {
                                    SQL += "T." + R["COLUMN_NAME"].ToString() + ", ";
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
            if (_VersionTable != _DataTable)
            {
                switch (Target)
                {
                    case Targets.Current:
                        SQL += "'current version'";
                        break;
                    default:
                        SQL += "CASE WHEN T.LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END";
                        break;
                }
                SQL += " AS [Kind of change], convert(varchar(20), T.LogUpdatedWhen, 120) AS [Date of change], "; ;

                if (_LinkToUserProxy)
                {
                    switch (Target)
                    {
                        case Targets.Current:
                            SQL += "dbo.UserName(T.LogUpdatedBy)";
                            break;
                        default:
                            SQL += "dbo.UserName(T.LogUser)";
                            break;
                    }
                }
                else
                {
                    switch (Target)
                    {
                        case Targets.Current:
                            SQL += "T.LogUpdatedBy";
                            break;
                        default:
                            SQL += "T.LogUser";
                            break;
                    }
                }

                SQL += " AS [Responsible user], ";
                switch (Target)
                {
                    case Targets.Current:
                        SQL += " NULL";
                        break;
                    default:
                        SQL += "T.LogID";
                        break;
                }
                SQL += " AS LogID FROM " + Prefix + "[" + _DataTable;
                if (Target != Targets.Current)
                    SQL += "_log";
                SQL += "] AS T ";

                if (_VersionTable.Length > 0)
                {
                    SQL += " INNER JOIN " + _VersionTable + " AS V ON T." + _PkColumn + " = V." + _PkColumn;
                }
                SQL += " WHERE T." + _PkColumn + " = " + _ID.ToString() +
                    " ORDER BY LogID DESC ";
            }
            else
            {
                switch (Target)
                {
                    case Targets.Current:
                        SQL += "'current version' AS [Kind of change], convert(varchar(20), LogUpdatedWhen, 120) AS [Date of change], ";
                        break;
                    default:
                        SQL += ".CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], " +
                            "convert(varchar(20), LogDate, 120) AS [Date of change],";
                        break;
                }

                switch (Target)
                {
                    case Targets.Current:
                        SQL += " NULL AS [Responsible user], NULL AS LogID";
                        break;
                    default:
                        if (_LinkToUserProxy)
                        {
                            SQL += " dbo.UserName(T.LogUser) AS [Responsible user], LogID";
                        }
                        else
                        {
                            SQL += " T.LogUser AS [Responsible user], LogID";
                        }
                        break;
                }
                SQL += " FROM  " + Prefix + "[" + _DataTable;
                if (Target != Targets.Current)
                    SQL += "_log";
                SQL += "] AS T WHERE T." + _PkColumn + " = " + _ID.ToString() +
                    " ORDER BY LogID DESC ";
            }
            return SQL;
        }
        
        #endregion

    }
}
