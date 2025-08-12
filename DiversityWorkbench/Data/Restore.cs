using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Data
{
    public class Restore
    {
        #region Restore deleted
        private static System.DateTime _RestoreStartDate;
        private static bool _UseRestoreStartDate = false;

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _RestoreRootTables;

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _RestoreParentTables;

        private static bool RestoreDeletedRow(string Table, int LogID, ref string Error, ref string Message, bool AskForConfirmation)
        {
            bool OK = true;
            try
            {
                // Getting the columns
                System.Data.DataTable dtColumns = new System.Data.DataTable();
                string SqlColumnList = "SELECT T.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS T, INFORMATION_SCHEMA.COLUMNS L " +
                    "WHERE T.TABLE_NAME = '" + Table + "' " +
                    "AND L.TABLE_NAME = '" + Table + "_Log' " +
                    "AND T.COLUMN_NAME = L.COLUMN_NAME";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlColumnList, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtColumns);

                // Getting the row
                string SQL = "SELECT * FROM [" + Table + "_log] WHERE [LogID] = " + LogID.ToString();
                if (_UseRestoreStartDate)
                {
                    SQL += " and [LogDate] >= CONVERT(DATETIME, '" + _RestoreStartDate.Year.ToString() + "-" + _RestoreStartDate.Month.ToString() + "-" + _RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                }
                System.Data.DataTable dtSource = new System.Data.DataTable();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(dtSource);
                if (dtSource.Rows.Count != 1)
                    return false;

                // Checking for Identity
                DiversityWorkbench.Data.Table T = new Data.Table(Table, DiversityWorkbench.Settings.ConnectionString);
                if (T.IdentityColumn != null && T.IdentityColumn.Length > 0)
                    SQL = "SET IDENTITY_INSERT [" + Table + "] ON; ";
                else SQL = "";

                T.FindChildParentColumns();
                T.FindColumnsWithForeignRelations();

                // Insert the data
                SQL += "INSERT INTO [" + Table + "]";
                string SqlColumns = "";
                string Desciption = "";
                foreach (System.Data.DataColumn C in dtSource.Columns)
                {
                    if (dtSource.Rows[0][C].Equals(System.DBNull.Value))
                        continue;

                    System.Data.DataRow[] rr = dtColumns.Select("COLUMN_NAME = '" + C.ColumnName + "'");

                    if (rr.Length == 0)
                        continue;
                    if (SqlColumns.Length > 0)
                        SqlColumns += ", ";
                    SqlColumns += "[" + C.ColumnName + "]";
                    if (!dtSource.Rows[0][C.ColumnName].Equals(System.DBNull.Value) && dtSource.Rows[0][C.ColumnName].ToString().Length > 0)
                    {
                        if (Desciption.Length > 0) Desciption += "; ";
                        Desciption += C.ColumnName + " = " + dtSource.Rows[0][C.ColumnName].ToString();
                    }
                }
                SQL += "(" + SqlColumns + ") SELECT " + SqlColumns + " FROM [" + Table + "_Log] WHERE [LogID] = " + LogID.ToString();
                if (!AskForConfirmation || System.Windows.Forms.MessageBox.Show("Do you want to insert the values\r\n\r\n" + Desciption + "\r\n\r\ninto the table " + Table + "?", "Insert", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    {
                        Message = "Dataset in table " + Table + " inserted\r\n";
                        //_DataRestored = true;
                    }
                    else
                    {
                        Message = "Insert in table " + Table + " failed\r\n";
                        Error = Message;
                        OK = false;
                        if (!AskForConfirmation)
                            RestoreAddFailedRow(Table, LogID);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Message += ex.Message + "\r\n";
                OK = false;
            }
            return OK;
        }

        private static string _RestoreRootTable;

        public static void RestoreFromLog(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> RestoreRootTables,
                                          System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> RestoreParentTables = null)
        {
            try
            {
                RestoreFailedRowsDictionary.Clear();
                Restore._RestoreRootTables = RestoreRootTables;
                Restore._RestoreParentTables = RestoreParentTables;
                System.Collections.Generic.List<string> RootTables = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> KV in _RestoreRootTables)
                    RootTables.Add(KV.Key);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(RootTables, "Root table", "Please select the table where the restoration should start", true);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.SelectedString.Length > 0)
                {
                    _RestoreRootTable = f.SelectedString;
                    DiversityWorkbench.Forms.FormGetDate fD = new Forms.FormGetDate(false);
                    fD.SetTitle("Set start date?");
                    fD.ShowDialog();
                    if (fD.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        _UseRestoreStartDate = true;
                        _RestoreStartDate = fD.DateTime;
                    }
                    DiversityWorkbench.Forms.FormGetInteger fint = new DiversityWorkbench.Forms.FormGetInteger(100, "Number of lines", "Please enter the number of lines of the table " + f.SelectedString + " that should be listed");
                    fint.ShowDialog();
                    if (fint.DialogResult == System.Windows.Forms.DialogResult.OK && fint.Integer != null && (int)fint.Integer > 0)
                    {
                        DiversityWorkbench.Data.Table T = new Data.Table(_RestoreRootTable, DiversityWorkbench.Settings.ConnectionString);
                        System.Collections.Generic.List<string> PK = T.PrimaryKeyColumnList;
                        if (PK.Count == 0)
                            return;
                        string SQL = "Select TOP " + fint.Integer.ToString() + " L.* from [" + _RestoreRootTable + "_log] L LEFT OUTER JOIN [" + _RestoreRootTable + "] T ON ";
                        string OnClause = "";
                        foreach (string K in PK)
                        {
                            if (OnClause.Length > 0)
                                OnClause += " AND ";
                            OnClause += "T.[" + K + "] = L.[" + K + "] ";
                        }
                        SQL += OnClause + " WHERE T.[" + PK[0] + "] IS NULL AND L.LogState = 'D' ";
                        if (_UseRestoreStartDate)
                            SQL += " and L.LogDate >= CONVERT(DATETIME, '" + _RestoreStartDate.Year.ToString() + "-" + _RestoreStartDate.Month.ToString() + "-" + _RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                        SQL += " order by L.Logdate desc";
                        System.Data.DataTable dt = new System.Data.DataTable();
                        string Message = "";
                        string Error = "";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                        if (dt.Rows.Count > 0)
                        {
                            DiversityWorkbench.Forms.FormGetRowFromTable fr = new Forms.FormGetRowFromTable("Restore " + _RestoreRootTable, "Please select the row that should be restored", dt);
                            fr.ShowDialog();
                            if (fr.DialogResult == System.Windows.Forms.DialogResult.OK && fr.SeletedRows().Count > 0)
                            {
                                int iSuccess = 0;
                                int itemCnt = fr.SeletedRows().Count;
                                using (Forms.FormProgress fp = new Forms.FormProgress("Restoring " + _RestoreRootTable + " datasets", itemCnt, false))
                                {
                                    fp.Show();
                                    foreach (System.Data.DataRow R in fr.SeletedRows())
                                    {
                                        fp.PerformStep();
                                        if (RestoreDeletedData(_RestoreRootTable, int.Parse(R["LogID"].ToString()), ref Error, ref Message))
                                        {
                                            if (RestoreFailedRowsNumber() > 0)
                                                RestoreFailedRows();
                                            int i = RestoreFailedRowsNumber();
                                            if (i == 0)
                                                iSuccess++;
                                            else
                                            {
                                                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in RestoreFailedRowsDictionary)
                                                {
                                                    if (KV.Value.Count > 0)
                                                        Message += KV.Key + ": " + KV.Value.Count + " dataset(s) not restored\r\n";
                                                }
                                                Message += "Restoring failed for those tables:\r\n" + Message;
                                            }
                                        }
                                    }
                                    fp.Close();
                                }
                                if (iSuccess > 0)
                                    Message = iSuccess.ToString() + " dataset(s) have been restored\r\n" + Message;
                                System.Windows.Forms.MessageBox.Show(Message);
                            }
                        }
                        else
                            System.Windows.Forms.MessageBox.Show("No deleted data detected");
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private static bool RestoreDeletedData(string Table, int LogID, ref string Error, ref string Message)
        {
            bool OK = true;
            if (!_RestoreRootTables[_RestoreRootTable].Contains(Table))
                return OK;
            string SQL = "";
            string msg = "";
            try
            {
                DiversityWorkbench.Data.Table T = new Data.Table(Table, DiversityWorkbench.Settings.ConnectionString);
                T.FindChildParentColumns();
                T.FindColumnsWithForeignRelations();

                // Getting the row of the table
                SQL = "SELECT * FROM " + Table + "_log WHERE LogID = " + LogID.ToString();
                if (_UseRestoreStartDate)
                    SQL += " and LogDate >= CONVERT(DATETIME, '" + _RestoreStartDate.Year.ToString() + "-" + _RestoreStartDate.Month.ToString() + "-" + _RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                System.Data.DataTable dtSource = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtSource);
                if (dtSource.Rows.Count != 1)
                    return false;

                // Check if PK is present in Table
                SQL = "";
                foreach (string PK in T.PrimaryKeyColumnList)
                {
                    if (SQL.Length > 0)
                        SQL += " AND ";
                    SQL += PK + " = '" + dtSource.Rows[0][PK].ToString() + "'";
                }
                SQL = "SELECT COUNT(*) FROM [" + Table + "] WHERE " + SQL;
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
                if (Result != "0")
                {
                    Error += "The dataset in the table " + Table + " with the PK ";
                    foreach (string PK in T.PrimaryKeyColumnList)
                    {
                        Error += "\r\n" + PK + " = " + dtSource.Rows[0][PK].ToString();
                    }
                    Error += "\r\nis allready present\r\n";
                    Message = Error;
                    return false;
                }

                // Check for parents and self related content - must be present, otherwise these must be restored first
                try
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Table.TableRelation> KV in T.RelatedTables())
                    {
                        if (KV.Value == Data.Table.TableRelation.Parent)// || KV.Value == Data.Table.TableRelation.Self)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, Data.Column> DC in T.Columns)
                            {
                                if (DC.Value.ForeignRelationTable == KV.Key)
                                {
                                    if (!dtSource.Rows[0][DC.Key].Equals(System.DBNull.Value))
                                    {
                                        SQL = "SELECT COUNT(*) FROM " + DC.Value.ForeignRelationTable + " WHERE " + DC.Value.ForeignRelationColumn + " = '" + dtSource.Rows[0][DC.Key].ToString() + "'";
                                        string Exception = "";
                                        try
                                        {
                                            Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Exception);
                                            if (Result == "0" || Exception.Length > 0)
                                            {
                                                bool parentMissing = true;
                                                if (_RestoreParentTables != null && _RestoreParentTables.ContainsKey(Table) && _RestoreParentTables[Table].Contains(KV.Key))
                                                {
                                                    // Try to restore parent from log
                                                    SQL = "SELECT LogId FROM " + KV.Key + "_log WHERE LogState = 'D' AND " + DC.Value.ForeignRelationColumn + "=";
                                                    if (DC.Value.DataTypeBasicType != DiversityWorkbench.Data.Column.DataTypeBase.numeric)
                                                        SQL += "'" + dtSource.Rows[0][DC.Key].ToString() + "'";
                                                    else
                                                        SQL += dtSource.Rows[0][DC.Key].ToString();
                                                    DateTime dt = (DateTime)dtSource.Rows[0]["LogDate"];
                                                    SQL += " AND LogDate >= CONVERT(DATETIME, '" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "', 102) ";
                                                    SQL += "ORDER BY LogDate";
                                                    Exception = "";
                                                    System.Data.DataTable dtParent = new System.Data.DataTable();
                                                    ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                                                    ad.Fill(dtParent);
                                                    if (dtParent.Rows.Count == 1)
                                                    {
                                                        parentMissing = !RestoreDeletedRow(KV.Key, (int)dtParent.Rows[0][0], ref Error, ref msg, false);
                                                        if (!Message.Contains(msg))
                                                            Message += msg + (msg.EndsWith("\r\n") ? "" : "\r\n");
                                                    }
                                                }
                                                if (parentMissing)
                                                {
                                                    Error += "A dataset in the table " + KV.Key + " must be restored first: " + DC.Value.ForeignRelationColumn + " = " + dtSource.Rows[0][DC.Key].ToString() + "\r\n";
                                                    Message = Error;
                                                    return false;
                                                }
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            Message += ex.Message + "\r\n";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Message += ex.Message + "\r\n";
                }

                // Restore the data for this table
                RestoreDeletedRow(Table, LogID, ref Error, ref msg, false);
                if (!Message.Contains(msg))
                    Message += msg + (msg.EndsWith("\r\n") ? "" : "\r\n");

                // Restore the depending data
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Table.TableRelation> KV in T.RelatedTables())
                {
                    if (KV.Value == Data.Table.TableRelation.Child)
                    {
                        DiversityWorkbench.Data.Table C = new Data.Table(KV.Key, DiversityWorkbench.Settings.ConnectionString);
                        C.FindColumnsWithForeignRelations();
                        System.Collections.Generic.List<string> SelfRelationColumns = new List<string>();
                        foreach (System.Collections.Generic.KeyValuePair<string, Data.Column> DC in C.Columns)
                        {
                            if (DC.Value.ForeignRelationTable == C.Name && !C.PrimaryKeyColumnList.Contains(DC.Key))
                                SelfRelationColumns.Add(DC.Key);
                        }
                        SQL = "";
                        System.Collections.Generic.Dictionary<string, string> FR = C.ForeignRelationColumns(Table);
                        string ColumnClause = "C.LogID";
                        string WhereClause = "C.LogState = 'D' AND T.LogID = " + LogID.ToString();
                        foreach (System.Collections.Generic.KeyValuePair<string, string> FC in FR)
                            WhereClause += " AND C." + FC.Key + " = T." + FC.Value;
                        foreach (string PK in C.PrimaryKeyColumnList)
                            ColumnClause += ", C." + PK;
                        foreach (string SR in SelfRelationColumns)
                            ColumnClause += ", C." + SR;
                        SQL = "SELECT " + ColumnClause + " FROM [" + KV.Key + "_log] C, [" + Table + "_log] T WHERE " + WhereClause;
                        if (_UseRestoreStartDate)
                        {
                            SQL += " and C.LogDate >= CONVERT(DATETIME, '" + _RestoreStartDate.Year.ToString() + "-" + _RestoreStartDate.Month.ToString() + "-" + _RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                            SQL += " and T.LogDate >= CONVERT(DATETIME, '" + _RestoreStartDate.Year.ToString() + "-" + _RestoreStartDate.Month.ToString() + "-" + _RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                        }
                        SQL += " ORDER BY LogID DESC";

                        ad.SelectCommand.CommandText = SQL;
                        System.Data.DataTable dtDependent = new System.Data.DataTable();
                        try
                        {
                            ad.Fill(dtDependent);
                        }
                        catch (System.Exception ex)
                        {
                            if (_RestoreRootTables[_RestoreRootTable].Contains(KV.Key))
                                Message += ex.Message + "\r\n";
                        }

                        if (dtDependent.Rows.Count > 0)
                        {
                            // getting only the last deleted rows
                            System.Collections.Generic.Dictionary<string, int> DependentData = new Dictionary<string, int>();
                            foreach (System.Data.DataRow Rdep in dtDependent.Rows)
                            {
                                string KeyFilter = "";
                                foreach (string PK in C.PrimaryKeyColumnList)
                                {
                                    if (KeyFilter.Length > 0)
                                        KeyFilter += " AND ";
                                    KeyFilter += PK + " = '" + Rdep[PK].ToString() + "'";
                                }
                                if (!DependentData.ContainsKey(KeyFilter))
                                    DependentData.Add(KeyFilter, int.Parse(Rdep[0].ToString()));
                            }

                            // Sorting the dependent rows after selfrelated content
                            SQL = "";
                            foreach (System.Collections.Generic.KeyValuePair<string, int> DD in DependentData)
                            {
                                if (SQL.Length > 0)
                                    SQL += ", ";
                                SQL += DD.Value.ToString();
                            }
                            string OrderByClause = "";
                            foreach (string SRC in SelfRelationColumns)
                            {
                                if (OrderByClause.Length > 0)
                                    OrderByClause += ", ";
                                OrderByClause += SRC;
                            }
                            SQL = "SELECT * FROM " + KV.Key + "_log WHERE LogID IN (" + SQL + ")";
                            if (_UseRestoreStartDate)
                                SQL += " and LogDate >= CONVERT(DATETIME, '" + _RestoreStartDate.Year.ToString() + "-" + _RestoreStartDate.Month.ToString() + "-" + _RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                            if (OrderByClause.Length > 0)
                                SQL += " ORDER BY " + OrderByClause;
                            dtDependent.Clear();
                            ad.SelectCommand.CommandText = SQL;
                            try
                            {
                                ad.Fill(dtDependent);
                            }
                            catch (System.Exception ex)
                            {
                                Message += ex.Message + "\r\n";
                            }
                            foreach (System.Data.DataRow R in dtDependent.Rows)
                            {
                                OK = RestoreDeletedData(KV.Key, int.Parse(R["LogID"].ToString()), ref Error, ref Message);
                            }
                        }
                        //foreach (System.Collections.Generic.KeyValuePair<string, int> DD in DependentData)
                        //{
                        //    OK = RestoreDeletedData(KV.Key, DD.Value, ref Error, ref Message);
                        //}
                    }
                }

                // Restore data in referencing tables (DiversityCollection only)
                if (DiversityWorkbench.Settings.ModuleName == "DiversityCollection" && T.IdentityColumn.Length > 0)
                {
                    System.Collections.Generic.List<string> ReferencingTables = new List<string>();
                    ReferencingTables.Add("ExternalIdentifier");
                    ReferencingTables.Add("Annotation");
                    foreach (string RT in ReferencingTables)
                    {
                        SQL = "SELECT C.LogID FROM " + RT + "_log C, [" + Table + "_log] T " +
                            " WHERE C.LogState = 'D' AND T.LogID = " + LogID.ToString() +
                            " AND C.ReferencedTable = '" + Table + "' " +
                            " AND C.ReferencedID = T." + T.IdentityColumn;
                        if (_UseRestoreStartDate)
                            SQL += " and C.LogDate >= CONVERT(DATETIME, '" + _RestoreStartDate.Year.ToString() + "-" + _RestoreStartDate.Month.ToString() + "-" + _RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                        SQL += " ORDER BY LogID DESC";
                        ad.SelectCommand.CommandText = SQL;
                        System.Data.DataTable dtDependent = new System.Data.DataTable();
                        try
                        {
                            ad.Fill(dtDependent);
                        }
                        catch (System.Exception ex)
                        {
                            Message += ex.Message + "\r\n";
                        }
                        foreach (System.Data.DataRow Rdep in dtDependent.Rows)
                        {
                            OK = RestoreDeletedData(RT, int.Parse(Rdep[0].ToString()), ref Error, ref Message);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Message += ex.Message + "\r\n";
            }
            return OK;
        }

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> _RestoreFailedRowsDictionary;

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> RestoreFailedRowsDictionary
        {
            get
            {
                if (_RestoreFailedRowsDictionary == null)
                    _RestoreFailedRowsDictionary = new Dictionary<string, List<int>>();
                return _RestoreFailedRowsDictionary;
            }
            //set { _RestoreFailedRows = value; }
        }

        private static void RestoreAddFailedRow(string Table, int LogID)
        {
            if (RestoreFailedRowsDictionary.ContainsKey(Table))
            {
                if (!RestoreFailedRowsDictionary[Table].Contains(LogID))
                    RestoreFailedRowsDictionary[Table].Add(LogID);
            }
            else
            {
                System.Collections.Generic.List<int> L = new List<int>();
                L.Add(LogID);
                RestoreFailedRowsDictionary.Add(Table, L);
            }
        }

        private static int RestoreFailedRowsNumber()
        {
            int i = 0;
            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in RestoreFailedRowsDictionary)
            {
                foreach (int r in KV.Value)
                    i++;
            }
            return i;
        }

        private static bool RestoreFailedRows()
        {
            bool OK = true;
            int iBefore = RestoreFailedRowsNumber();
            int iAfter = 0;
            string Message = "";
            string Error = "";
            // as long as number gets lower try to restore rows
            while (iAfter < iBefore)
            {
                // the dictionary keeping the restored rows
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> RestoredRowsDictionary = new Dictionary<string, List<int>>();
                // try to restore the rows
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in RestoreFailedRowsDictionary)
                {
                    foreach (int LogID in KV.Value)
                    {
                        if (RestoreDeletedRow(KV.Key, LogID, ref Error, ref Message, false))
                        {
                            if (RestoredRowsDictionary.ContainsKey(KV.Key))
                            {
                                if (!KV.Value.Contains(LogID))
                                    RestoredRowsDictionary[KV.Key].Add(LogID);
                            }
                            else
                            {
                                System.Collections.Generic.List<int> L = new List<int>();
                                L.Add(LogID);
                                RestoredRowsDictionary.Add(KV.Key, L);
                            }
                        }
                    }
                }
                // removing the successful restored rows from the dictionary
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in RestoredRowsDictionary)
                {
                    foreach (int LogID in KV.Value)
                    {
                        if (RestoreFailedRowsDictionary[KV.Key].Contains(LogID))
                            RestoreFailedRowsDictionary[KV.Key].Remove(LogID);
                    }
                }
                iAfter = RestoreFailedRowsNumber();
                if (iAfter == 0)
                    break;
            }
            if (iAfter > 0)
                OK = false;
            return OK;
        }

        #endregion


    }
}
