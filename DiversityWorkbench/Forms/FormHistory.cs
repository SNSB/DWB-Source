using DiversityWorkbench.Archive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormHistory : Form
    {
        #region Parameter and properties

        private bool _DataRestored = false;

        public bool DataRestored
        {
            get { return _DataRestored; }
            //set { _DataRestored = value; }
        }

        private bool _ReadOnly = false;

        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                this.buttonRestoreDeleted.Visible = !_ReadOnly && _RestoreRootTable != null && _RestoreRootTable.Length > 0;
                this.buttonRollBack.Visible = !ReadOnly;
            }
        }

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.LogDatabase.HistoryTable> _HistoryTables;

        #endregion

        #region Construction

        private FormHistory(string Title, string PathHelpProvider)
        {
            InitializeComponent();
            this.Text = Title;
            this.helpProvider.HelpNamespace = PathHelpProvider;
            this.InitForm();
        }

        public FormHistory(string Title, System.Data.DataTable[] DataTables, string PathHelpProvider)
            : this(Title, PathHelpProvider)
        {
            this.InitForm(DataTables);
        }

        public FormHistory(string Title, System.Collections.Generic.List<System.Data.DataTable> DataTables, string PathHelpProvider)
            : this(Title, PathHelpProvider)
        {
            foreach (System.Data.DataTable DT in DataTables)
            {
                if (DT.Rows.Count > 0)
                    this.CreateTabPage(DT);
            }
        }

        public FormHistory(string Title, System.Collections.Generic.Dictionary<string, DiversityWorkbench.LogDatabase.HistoryTable> HistoryTables, string PathHelpProvider)
            : this(Title, PathHelpProvider)
        {
            this.InitForm(HistoryTables);
        }

        #endregion

        #region Form and Tables

        private void InitForm()
        {
            if (!DiversityWorkbench.LogDatabase.Database.Exists())
                this.buttonIncludeSavedLog.Visible = false;
        }

        private void InitForm(System.Data.DataTable[] DataTables)
        {
#if !DEBUG
            this.buttonIncludeSavedLog.Visible = false;
#else
            this.SavedLogIncluded = new Dictionary<string, SavedLogState>();
#endif
            foreach (System.Data.DataTable dt in DataTables)
            {
                System.Windows.Forms.TabPage p = new TabPage(dt.TableName);
                this.tabControlHistory.TabPages.Add(p);
                System.Windows.Forms.DataGridView g = new DataGridView();
                p.Controls.Add(g);
                g.Dock = DockStyle.Fill;
                g.DataSource = dt;
                g.ReadOnly = true;
#if DEBUG
                if (DiversityWorkbench.LogDatabase.Database.Exists())
                    this.SavedLogIncluded.Add(dt.TableName, SavedLogState.Unchecked);
                else
                    this.SavedLogIncluded.Add(dt.TableName, SavedLogState.Missing);
#endif
            }
        }

        private void InitForm(System.Collections.Generic.Dictionary<string, DiversityWorkbench.LogDatabase.HistoryTable> HistoryTables)
        {
            this.SavedLogIncluded = new Dictionary<string, SavedLogState>();
            this._HistoryTables = HistoryTables;
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LogDatabase.HistoryTable> KV in _HistoryTables)
            {
                System.Windows.Forms.TabPage p = new TabPage(KV.Key);
                this.tabControlHistory.TabPages.Add(p);
                System.Windows.Forms.DataGridView g = new DataGridView();
                p.Controls.Add(g);
                g.Dock = DockStyle.Fill;
                g.DataSource = KV.Value.DataTable();
                g.ReadOnly = true;
                if (DiversityWorkbench.LogDatabase.Database.Exists())
                    this.SavedLogIncluded.Add(KV.Key, SavedLogState.Unchecked);
                else
                    this.SavedLogIncluded.Add(KV.Key, SavedLogState.Missing);
            }
        }

        private void CreateTabPage(System.Data.DataTable DataTable)
        {
            System.Windows.Forms.TabPage p = new TabPage(DataTable.TableName);
            this.tabControlHistory.TabPages.Add(p);
            System.Windows.Forms.DataGridView g = new DataGridView();
            p.Controls.Add(g);
            g.Dock = DockStyle.Fill;
            g.DataSource = DataTable;
            g.ReadOnly = true;
        }

        #endregion

        #region Help

        public void setHelpProviderNameSpace(string HelpNameSpace, string Keyword)
        {
            try
            {
                this.helpProvider.HelpNamespace = HelpNameSpace;
                this.helpProvider.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
                this.helpProvider.SetHelpKeyword(this, Keyword);
            }
            catch { }
        }
        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Button events

        private void buttonRollBack_Click(object sender, EventArgs e)
        {
            bool UseRowGUID = false;
            try
            {
                System.Windows.Forms.DataGridView G = (System.Windows.Forms.DataGridView)this.tabControlHistory.SelectedTab.Controls[0];
                if (G.CurrentRow.Index == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Please select the line that should be restored");
                    return;
                }

                int i = G.CurrentRow.Index;
                System.Data.DataTable dt = (System.Data.DataTable)G.DataSource;
                System.Data.DataRow R = dt.Rows[i];
                int LogID;
                if (!int.TryParse(R["LogID"].ToString(), out LogID))
                {
                    System.Windows.Forms.MessageBox.Show("This line can not be restored");
                    return;
                }
                string Table = dt.TableName.Replace(" ", "");
                string TableLog = Table + "_log";

                // Check for Identity
                string Message = "";
                if (this.TableHasIdentityAndUserMissProperPermissions(Table, ref Message))
                {
                    System.Windows.Forms.MessageBox.Show("This line can not be restored\r\n" + Message);
                    return;
                }

                System.Data.DataTable dtColumns = new DataTable();
                string SqlColumnList = "SELECT T.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS T, INFORMATION_SCHEMA.COLUMNS L " +
                    "WHERE T.TABLE_NAME = '" + Table + "' " +
                    "AND L.TABLE_NAME = '" + TableLog + "' " +
                    "AND T.COLUMN_NAME = L.COLUMN_NAME";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlColumnList, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtColumns);

                string Prefix = "";

                System.Data.DataRow[] RR = dtColumns.Select("COLUMN_NAME = 'RowGUID'");
                if (RR.Length > 0)
                {
                    string SqlCheckRowGUID = "SELECT RowGUID FROM [" + TableLog + "] WHERE LogID = " + LogID.ToString();
                    string RG = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlCheckRowGUID);
                    if (RG.Length > 0)
                        UseRowGUID = true;
#if DEBUG
                    else if (DiversityWorkbench.LogDatabase.Database.Exists())
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DateTime> KV in DiversityWorkbench.LogDatabase.Database.Schemata())
                        {
                            SqlCheckRowGUID = "SELECT RowGUID FROM [" + DiversityWorkbench.Settings.DatabaseName + "_log].[" + KV.Key + "].[" + TableLog + "] WHERE LogID = " + LogID.ToString();
                            RG = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlCheckRowGUID);
                            if (RG.Length > 0)
                            {
                                UseRowGUID = true;
                                Prefix = "[" + DiversityWorkbench.Settings.DatabaseName + "_log].[" + KV.Key + "].";
                                break;
                            }
                        }
                    }
#endif
                }

                string SQL = "SELECT LogState FROM " + Prefix + TableLog + " WHERE LogID = " + LogID.ToString();
                string LogState = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);

                string SqlPK = "SELECT K.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS T, INFORMATION_SCHEMA.KEY_COLUMN_USAGE K " +
                    "WHERE T.TABLE_NAME = '" + Table + "' " +
                    "AND T.TABLE_NAME = K.TABLE_NAME " +
                    "AND T.CONSTRAINT_NAME = K.CONSTRAINT_NAME " +
                    "AND T.CONSTRAINT_TYPE = 'PRIMARY KEY'";
                System.Data.DataTable dtPK = new DataTable();
                ad.SelectCommand.CommandText = SqlPK;
                ad.Fill(dtPK);

                string SqlCheckExistence = "SELECT COUNT(*) FROM [" + Table + "] AS T, " + Prefix + "[" + TableLog + "] AS L WHERE L.LogID = " + LogID.ToString();
                if (UseRowGUID)
                {
                    SqlCheckExistence += " AND T.RowGUID = L.RowGUID";
                }
                else
                {
                    foreach (System.Data.DataRow RPK in dtPK.Rows)
                    {
                        SqlCheckExistence += " AND T." + RPK[0].ToString() + " = L." + RPK[0].ToString();
                    }
                }
                int iCount = 0;
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlCheckExistence), out iCount) && iCount > 0)
                    LogState = "U";
                else LogState = "D";

                if (LogState == "D")
                {
                    Message = "";
                    string Error = "";
                    this.RestoreDeletedRow(Table, LogID, ref Error, ref Message, true);
                    System.Windows.Forms.MessageBox.Show(Message + Error);
                }
                else if (LogState == "U")
                {

                    SQL = "";
                    string SqlUpdate = "";
                    string SqlWhereClause = " WHERE L.LogID = " + R["LogID"].ToString();
                    //string DesciptionValues = "";
                    string DesciptionKey = "";
                    string Tcolumns = "'T'";
                    string Lcolumns = "'L'";

                    string TcolumnsCompare = "'T'";
                    string LcolumnsCompare = "'L'";

                    SQL = "SELECT * FROM " + Prefix + TableLog + " WHERE LogID = " + LogID.ToString();
                    System.Data.DataTable dtSource = new DataTable();
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dtSource);
                    if (dtSource.Rows.Count != 1)
                        return;

                    foreach (System.Data.DataColumn C in dtSource.Columns)
                    {
                        // unklar warum das nicht uebernommen werden sollte - besser fuer Replikation wenn RowGUID mitkommt
                        //if (!UseRowGUID && C.ColumnName.ToLower() == "rowguid")
                        //    continue;
                        System.Data.DataRow[] rr = dtColumns.Select("COLUMN_NAME = '" + C.ColumnName + "'");
                        if (rr.Length == 0)
                            continue;
                        System.Data.DataRow[] rrPK = dtPK.Select("COLUMN_NAME = '" + C.ColumnName + "'");
                        if (rrPK.Length > 0)
                        {
                            if (UseRowGUID)
                            {
                                SQL = "select count(*) from sys.columns c, sys.tables t where c.object_id = t.object_id " +
                                    "and t.name = '" + Table + "' and c.name = '" + C.ColumnName + "' " +
                                    "and c.is_identity = 1 ";
                                string Check = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                                if (Check != "1")
                                {
                                    if (SqlUpdate.Length > 0)
                                        SqlUpdate += ", ";
                                    SqlUpdate += " T." + C.ColumnName + " = L." + C.ColumnName;
                                    Tcolumns += ", T." + C.ColumnName;
                                    Lcolumns += ", L." + C.ColumnName;
                                    TcolumnsCompare += ", T." + C.ColumnName;
                                    LcolumnsCompare += ", L." + C.ColumnName;
                                }
                            }
                            else
                            {
                                SqlWhereClause += " AND T." + C.ColumnName + " = '" + dtSource.Rows[0][C.ColumnName].ToString() + "' " +
                                    "AND T." + C.ColumnName + " = L." + C.ColumnName;
                            }
                            if (DesciptionKey.Length > 0)
                                DesciptionKey += "\r\n";
                            DesciptionKey += C.ColumnName + " = " + dtSource.Rows[0][C.ColumnName].ToString();
                        }
                        else if (UseRowGUID && C.ColumnName.ToLower() == "rowguid")
                        {
                            SqlWhereClause += " AND T." + C.ColumnName + " = '" + dtSource.Rows[0][C.ColumnName].ToString() + "' " +
                                "AND T." + C.ColumnName + " = L." + C.ColumnName;
                        }
                        else
                        {
                            if (SqlUpdate.Length > 0)
                                SqlUpdate += ", ";
                            SqlUpdate += " T." + C.ColumnName + " = L." + C.ColumnName;
                            Tcolumns += ", T." + C.ColumnName;
                            Lcolumns += ", L." + C.ColumnName;
                            if (C.DataType.Name == "SqlGeography")
                            {
                                TcolumnsCompare += ", CAST(T." + C.ColumnName + " AS VARCHAR(MAX))";
                                LcolumnsCompare += ", CAST(L." + C.ColumnName + " AS VARCHAR(MAX))";
                            }
                            else
                            {
                                TcolumnsCompare += ", T." + C.ColumnName;
                                LcolumnsCompare += ", L." + C.ColumnName;
                            }
                        }
                    }
                    string SqlCompare = "SELECT " + TcolumnsCompare + " FROM " + Table + " AS T, " + Prefix + TableLog + " AS L " + SqlWhereClause +
                        " UNION " +
                        "SELECT " + LcolumnsCompare + " FROM " + Table + " AS T, " + Prefix + TableLog + " AS L " + SqlWhereClause;
                    System.Data.DataTable dtCompare = new DataTable();
                    ad.SelectCommand.CommandText = SqlCompare;
                    ad.Fill(dtCompare);
                    if (dtCompare.Rows.Count == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("This dataset has been deleted from the table and can not be updated.\r\nPlease restore the deleted version of the dataset");
                        return;
                    }
                    string CompareColumns = "";
                    foreach (System.Data.DataColumn C in dtCompare.Columns)
                    {
                        if (C.ColumnName == "Column1") continue;
                        if (dtCompare.Rows[0][C.ColumnName].ToString() != dtCompare.Rows[1][C.ColumnName].ToString())
                            CompareColumns += "\r\n" + C.ColumnName + ": " + dtCompare.Rows[1][C.ColumnName].ToString() + "\t->\t" + dtCompare.Rows[0][C.ColumnName].ToString();
                    }
                    if (CompareColumns.Length == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("No differences found");
                        return;
                    }
                    else
                    {
                        Message = "Do you want to update the following values in table " + Table +
                            "\r\nof the dataset\r\n" + DesciptionKey + ":\r\n" + CompareColumns;
                        if (System.Windows.Forms.MessageBox.Show(Message + "?", "Update ?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            SQL = "UPDATE T SET " + SqlUpdate + " FROM " + Table + " AS T, " + Prefix + TableLog + " AS L " + SqlWhereClause;
                            Message = "";
                            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                            {
                                System.Windows.Forms.MessageBox.Show("Dataset updated");
                                this._DataRestored = true;
                            }
                            else
                                System.Windows.Forms.MessageBox.Show("Update failed");
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        private bool TableHasIdentityAndUserMissProperPermissions(string TableName, ref string Message)
        {
            bool OK = false;
            try
            {
                DiversityWorkbench.Data.Table T = new Data.Table(TableName);
                System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool> Permissions = new Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool>();
                if (T.IdentityColumn.Length > 0)
                {
                    if (!DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
                    {
                        OK = true;
                        Message = "The table " + TableName + " has an identiy column (" + T.IdentityColumn + ") and you miss the proper rights to restore the selected line. Please turn to your administrator";
                    }
                }
            }
            catch(System.Exception ex)
            {
                OK = true;
                Message = ex.Message;
            }
            return OK;
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

        #region Restore deleted

        private System.DateTime _RestoreStartDate;
        private bool _UseRestoreStartDate = false;

        private System.DateTime _RestoreEndDate;
        private bool _UseRestoreEndDate = false;

        private string _RestoreFilter = "";
        private bool _UseRestoreFilter = false;

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _RestoreRootTables;
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _RestoreParentTables;

        private bool RestoreDeletedRow(string Table, int LogID, ref string Error, ref string Message, bool AskForConfirmation)
        {
            bool OK = true;
            try
            {
                // Getting the columns
                System.Data.DataTable dtColumns = new DataTable();
                string SqlColumnList = "SELECT T.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS T, INFORMATION_SCHEMA.COLUMNS L " +
                    "WHERE T.TABLE_NAME = '" + Table + "' " +
                    "AND L.TABLE_NAME = '" + Table + "_Log' " +
                    "AND T.COLUMN_NAME = L.COLUMN_NAME";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlColumnList, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtColumns);
                string Prefix = "";
#if DEBUG
                if (DiversityWorkbench.LogDatabase.Database.Exists())
                {
                    string SqlLog = "SELECT count(*) FROM " + Table + "_log WHERE LogID = " + LogID.ToString();
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlLog);
                    int i = 0;
                    if (!int.TryParse(Result, out i) || i <= 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DateTime> KV in DiversityWorkbench.LogDatabase.Database.Schemata())
                        {
                            Prefix = "[" + DiversityWorkbench.Settings.DatabaseName + "_log].[" + KV.Key + "].";
                            SqlLog = "SELECT count(*) FROM " + Prefix + Table + "_log WHERE LogID = " + LogID.ToString();
                            Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlLog);
                            if (int.TryParse(Result, out i) && i > 0)
                            {
                                break;
                            }
                            else
                                Prefix = "";
                        }
                    }
                }
#endif

                // Getting the row
                string SQL = "SELECT * FROM " + Prefix + Table + "_log WHERE LogID = " + LogID.ToString();
                if (this._UseRestoreStartDate)
                {
                    SQL += " and LogDate >= CONVERT(DATETIME, '" + this._RestoreStartDate.Year.ToString() + "-" + this._RestoreStartDate.Month.ToString() + "-" + this._RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                }
                System.Data.DataTable dtSource = new DataTable();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(dtSource);
                if (dtSource.Rows.Count != 1)
                    return false;

                // Checking for Identity
                DiversityWorkbench.Data.Table T = new Data.Table(Table, DiversityWorkbench.Settings.ConnectionString);
                if (T.IdentityColumn != null && T.IdentityColumn.Length > 0)
                    SQL = "SET IDENTITY_INSERT " + Table + " ON; ";
                else SQL = "";

                T.FindChildParentColumns();
                T.FindColumnsWithForeignRelations();

                // Insert the data
                SQL += "INSERT INTO " + Table;
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
                SQL += "(" + SqlColumns + ") SELECT " + SqlColumns + " FROM " + Prefix + Table + "_Log WHERE LogID = " + LogID.ToString();
                if (!AskForConfirmation || System.Windows.Forms.MessageBox.Show("Do you want to insert the values\r\n\r\n" + Desciption + "\r\n\r\ninto the table " + Table + "?", "Insert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    {
                        Message = "Dataset in table " + Table + " inserted\r\n";
                        this._DataRestored = true;
                    }
                    else
                    {
                        Message = "Insert in table " + Table + " failed\r\n";
                        Error = Message;
                        OK = false;
                        if (!AskForConfirmation)
                            this.RestoreAddFailedRow(Table, LogID);
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

        public void SetRestoreRootTables(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> RootTables,
                                         System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> ParentTables = null)
        {
            this._RestoreRootTables = RootTables;
            this._RestoreParentTables = ParentTables;
            this.buttonRestoreDeleted.Visible = !_ReadOnly;
        }

        private string _RestoreRootTable;

        private System.Collections.Generic.List<string> _LogSavedSchemata = new List<string>();

        private void buttonRestoreDeleted_Click(object sender, EventArgs e)
        {
            try
            {
                this.RestoreFailedRowsDictionary.Clear();
                System.Collections.Generic.List<string> RootTables = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> KV in this._RestoreRootTables)
                    RootTables.Add(KV.Key);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(RootTables, "Root table", "Please select the table where the restoration should start", true);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.SelectedString.Length > 0)
                {
                    this._RestoreRootTable = f.SelectedString;
                    DiversityWorkbench.Forms.FormGetDate fD = new Forms.FormGetDate(false);
                    fD.SetTitle("Set start date?");
                    fD.ShowDialog();
                    if (fD.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        this._UseRestoreStartDate = true;
                        this._RestoreStartDate = fD.DateTime;
                    }
                    fD.SetTitle("Set end date?");
                    fD.ShowDialog();
                    if (fD.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        this._UseRestoreEndDate = true;
                        this._RestoreEndDate = fD.DateTime;
                    }
                    if (System.Windows.Forms.MessageBox.Show("Do you want to set a filter for the query?", "Filter?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.setRestoreFilter();
                    }
                    DiversityWorkbench.Forms.FormGetInteger fint = new FormGetInteger(100, "Number of lines", "Please enter the number of lines of the table " + f.SelectedString + " that should be listed");
                    fint.ShowDialog();
                    if (fint.DialogResult == System.Windows.Forms.DialogResult.OK && fint.Integer != null && (int)fint.Integer > 0)
                    {
#if DEBUG
#endif
                        if (DiversityWorkbench.LogDatabase.Database.Exists())
                        {
                            // macht evtl. keinen Sinn so - besser es wird automatisch mitgenommen als den Benutzer zu verwirren
                            //System.Collections.Generic.Dictionary<string, bool> LogSavedSchemata = new Dictionary<string,bool>();
                            //foreach(System.Collections.Generic.KeyValuePair<string, DateTime> KV in DiversityWorkbench.LogDatabase.Database.Schemata())
                            //{
                            //    LogSavedSchemata.Add(KV.Key, false);
                            //}
                            //DiversityWorkbench.Forms.FormGetMultiFromList fgm = new Forms.FormGetMultiFromList("Saved log data", "Data have been transferred into the log database. Please select those that should be included in the search", LogSavedSchemata);
                            //fgm.ShowDialog();
                            //if (fgm.DialogResult == System.Windows.Forms.DialogResult.OK)
                            //{
                            //    foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in fgm.Items)
                            //    {
                            //        if (KV.Value)
                            //            SavedSchemata.Add(KV.Key);
                            //    }
                            //}

                            // Loesung mit List - es wird alles mitgenommen
                            this._LogSavedSchemata = new List<string>();
                            foreach (System.Collections.Generic.KeyValuePair<string, DateTime> KV in DiversityWorkbench.LogDatabase.Database.Schemata())
                            {
                                _LogSavedSchemata.Add(KV.Key);
                            }
                        }
                        DiversityWorkbench.Data.Table T = new Data.Table(this._RestoreRootTable, DiversityWorkbench.Settings.ConnectionString);
                        System.Collections.Generic.List<string> PK = T.PrimaryKeyColumnList;
                        if (PK.Count == 0)
                            return;
                        System.Data.DataTable dt = this.getRowsThatCanBeRestored((int)fint.Integer);// new DataTable();
                        string Message = "";
                        string Error = "";
                        if (dt.Rows.Count > 0)
                        {
                            DiversityWorkbench.Forms.FormGetRowFromTable fr = new Forms.FormGetRowFromTable("Restore " + this._RestoreRootTable, "Please select the rows that should be restored", dt);
                            fr.ShowDialog();
                            if (fr.DialogResult == System.Windows.Forms.DialogResult.OK && fr.SeletedRows().Count > 0)
                            {
                                this.Enabled = false;
                                int iSuccess = 0;
                                int itemCnt = fr.SeletedRows().Count;
                                using (Forms.FormProgress fp = new Forms.FormProgress("Restoring datasets", itemCnt, false))
                                {
                                    fp.Show();
                                    foreach (System.Data.DataRow R in fr.SeletedRows())
                                    {
                                        fp.PerformStep();
                                        if (this.RestoreDeletedData(this._RestoreRootTable, int.Parse(R["LogID"].ToString()), ref Error, ref Message))
                                        {
                                            if (this.RestoreFailedRowsNumber() > 0)
                                                this.RestoreFailedRows();
                                            int i = this.RestoreFailedRowsNumber();
                                            if (i == 0)
                                                iSuccess++;
                                            else
                                            {
                                                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in this.RestoreFailedRowsDictionary)
                                                {
                                                    if (KV.Value.Count > 0)
                                                        Message += KV.Key + ": " + KV.Value.Count + " datasets not restored\r\n";
                                                }
                                                Message += "Restoring failed for those tables:\r\n" + Message;
                                            }
                                        }
                                    }
                                    fp.Close();
                                    this.Enabled = true;
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
            finally
            {
            }
            this.Enabled = true;
        }

        private bool setRestoreFilter()
        {
            bool OK = true;
            try
            {
                string SqlFilter = "SELECT C.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + this._RestoreRootTable + "' ORDER BY C.COLUMN_NAME ";
                System.Data.DataTable dtCol = new DataTable();
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SqlFilter, ref dtCol, ref Message);
                DiversityWorkbench.Forms.FormGetStringFromList fColumn = new DiversityWorkbench.Forms.FormGetStringFromList(dtCol, "Please select the column for the filter");
                fColumn.ShowDialog();
                if (fColumn.DialogResult == System.Windows.Forms.DialogResult.OK && fColumn.String.Length > 0)
                {
                    DiversityWorkbench.Forms.FormGetString fFilter = new FormGetString("Filter", "Please enter the filter (query with LIKE)", "%");
                    fFilter.ShowDialog();
                    if (fFilter.DialogResult == System.Windows.Forms.DialogResult.OK && fFilter.String.Length > 0)
                    {
                        this._UseRestoreFilter = true;
                        this._RestoreFilter = "L.[" + fColumn.String + "] LIKE '" + fFilter.String.Replace("'", "''") + "' ";
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        /// <summary>
        /// get the rows from the log table(s) that may be restored
        /// </summary>
        /// <param name="MaxRows">the maximal number of rows</param>
        /// <returns>a table containinig the rows from the log table that have been deleted and may be restored</returns>
        private System.Data.DataTable getRowsThatCanBeRestored(int MaxRows)
        {
            System.Data.DataTable dt = new DataTable();
            try
            {
                DiversityWorkbench.Data.Table T = new Data.Table(this._RestoreRootTable, DiversityWorkbench.Settings.ConnectionString);
                System.Collections.Generic.List<string> PK = T.PrimaryKeyColumnList;
                if (PK.Count > 0)
                {
                    string SQL = this.SqlRestoreDeletedRows(MaxRows, "", PK);// "Select TOP " + MaxRows.ToString() + " L.* from [" + this._RestoreRootTable + "_log] L LEFT OUTER JOIN [" + this._RestoreRootTable + "] T ON ";
                    string Message = "";
                    //string Error = "";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                    if (DiversityWorkbench.LogDatabase.Database.Exists() && this._LogSavedSchemata != null && this._LogSavedSchemata.Count > 0)
                    {
                        foreach (string Schema in this._LogSavedSchemata)
                        {
                            MaxRows -= dt.Rows.Count;
                            if (MaxRows <= 0)
                                break;
                            SQL = this.SqlRestoreDeletedRows(MaxRows, "[" + DiversityWorkbench.Settings.DatabaseName + "_log].[" + Schema + "].", PK);
                            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dt;
        }

        /// <summary>
        /// The SQL-Command for the retrieval of the deleted rows
        /// </summary>
        /// <param name="MaxRows">the maximal number of rows</param>
        /// <param name="Prefix">The prefix esp. for log tables in the log database</param>
        /// <param name="PK">The primary key columns of the table</param>
        /// <returns></returns>
        private string SqlRestoreDeletedRows(int MaxRows, string Prefix, System.Collections.Generic.List<string> PK)
        {
            string SQL = "Select TOP " + MaxRows.ToString() + " L.* from " + Prefix + "[" + this._RestoreRootTable + "_log] L LEFT OUTER JOIN [" + this._RestoreRootTable + "] T ON ";
            string OnClause = "";
            foreach (string K in PK)
            {
                if (OnClause.Length > 0)
                    OnClause += " AND ";
                OnClause += "T.[" + K + "] = L.[" + K + "] ";
            }
            SQL += OnClause + " WHERE T.[" + PK[0] + "] IS NULL AND L.LogState = 'D' ";
            if (this._UseRestoreStartDate)
                SQL += " and L.LogDate >= CONVERT(DATETIME, '" + this._RestoreStartDate.Year.ToString() + "-" + this._RestoreStartDate.Month.ToString() + "-" + this._RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
            if (this._UseRestoreEndDate)
                SQL += " and L.LogDate <= CONVERT(DATETIME, '" + this._RestoreEndDate.Year.ToString() + "-" + this._RestoreEndDate.Month.ToString() + "-" + this._RestoreEndDate.Day.ToString() + " 23:59:59', 102) ";
            if (this._UseRestoreFilter)
                SQL += " and " + this._RestoreFilter;
            SQL += " order by L.Logdate desc";
            return SQL;
        }

        private bool RestoreDeletedData(string Table, int LogID, ref string Error, ref string Message)
        {
            bool OK = true;
            if (!this._RestoreRootTables[this._RestoreRootTable].Contains(Table))
                return OK;
            string SQL = "";
            string msg = "";
            try
            {
                DiversityWorkbench.Data.Table T = new Data.Table(Table, DiversityWorkbench.Settings.ConnectionString);
                T.FindChildParentColumns();
                T.FindColumnsWithForeignRelations();

                // Getting the row of the table
                SQL = "SELECT * FROM [" + Table + "_log] WHERE [LogID] = " + LogID.ToString();
                if (this._UseRestoreStartDate)
                    SQL += " and [LogDate] >= CONVERT(DATETIME, '" + this._RestoreStartDate.Year.ToString() + "-" + this._RestoreStartDate.Month.ToString() + "-" + this._RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                System.Data.DataTable dtSource = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtSource);
                if (dtSource.Rows.Count != 1)
                {
#if DEBUG
                    if (DiversityWorkbench.LogDatabase.Database.Exists())
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DateTime> KV in DiversityWorkbench.LogDatabase.Database.Schemata())
                        {
                            SQL = "SELECT * FROM [" + DiversityWorkbench.Settings.DatabaseName + "_log].[" + KV.Key + "].[" + Table + "_log] WHERE [LogID] = " + LogID.ToString();
                            if (this._UseRestoreStartDate)
                                SQL += " and [LogDate] >= CONVERT(DATETIME, '" + this._RestoreStartDate.Year.ToString() + "-" + this._RestoreStartDate.Month.ToString() + "-" + this._RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                            ad.SelectCommand.CommandText = SQL;
                            ad.Fill(dtSource);
                            if (dtSource.Rows.Count > 0)
                                break;
                        }
                    }
                    if (dtSource.Rows.Count != 1)
                        return false;
#else
                    return false;
#endif
                }

                // Check if PK is present in Table
                SQL = "";
                foreach (string PK in T.PrimaryKeyColumnList)
                {
                    if (SQL.Length > 0)
                        SQL += " AND ";
                    SQL += "[" + PK + "] = '" + dtSource.Rows[0][PK].ToString() + "'";
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
                                        SQL = "SELECT COUNT(*) FROM [" + DC.Value.ForeignRelationTable + "] WHERE [" + DC.Value.ForeignRelationColumn + "] = '" + dtSource.Rows[0][DC.Key].ToString() + "'";
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
                                                    SQL = "SELECT [LogId] FROM [" + KV.Key + "_log] WHERE [" + DC.Value.ForeignRelationColumn + "]=";
                                                    if (DC.Value.DataTypeBasicType != DiversityWorkbench.Data.Column.DataTypeBase.numeric)
                                                        SQL += "'" + dtSource.Rows[0][DC.Key].ToString() + "'";
                                                    else
                                                        SQL += dtSource.Rows[0][DC.Key].ToString();
                                                    SQL += " AND [LogDate] >= CONVERT(DATETIME, '" + ((DateTime)dtSource.Rows[0]["LogDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "', 102) ";
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
                this.RestoreDeletedRow(Table, LogID, ref Error, ref msg, false);
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
                        if (this._UseRestoreStartDate)
                        {
                            SQL += " and C.LogDate >= CONVERT(DATETIME, '" + this._RestoreStartDate.Year.ToString() + "-" + this._RestoreStartDate.Month.ToString() + "-" + this._RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                            SQL += " and T.LogDate >= CONVERT(DATETIME, '" + this._RestoreStartDate.Year.ToString() + "-" + this._RestoreStartDate.Month.ToString() + "-" + this._RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                        }
                        SQL += " ORDER BY LogID DESC";
                        //SQL += " FROM [" + KV.Key + "_log] C, [" + Table + "_log] T WHERE C.LogState = 'D' AND T.LogID = " + LogID.ToString() + SQL + " ORDER BY LogID DESC";
                        ad.SelectCommand.CommandText = SQL;
                        System.Data.DataTable dtDependent = new DataTable();
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
                            SQL = "SELECT * FROM [" + KV.Key + "_log] WHERE [LogID] IN (" + SQL + ")";
                            if (this._UseRestoreStartDate)
                                SQL += " and [LogDate] >= CONVERT(DATETIME, '" + this._RestoreStartDate.Year.ToString() + "-" + this._RestoreStartDate.Month.ToString() + "-" + this._RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
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
                                OK = this.RestoreDeletedData(KV.Key, int.Parse(R["LogID"].ToString()), ref Error, ref Message);
                            }
                        }
                        //foreach (System.Collections.Generic.KeyValuePair<string, int> DD in DependentData)
                        //{
                        //    OK = this.RestoreDeletedData(KV.Key, DD.Value, ref Error, ref Message);
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
                        if (this._UseRestoreStartDate)
                            SQL += " and C.LogDate >= CONVERT(DATETIME, '" + this._RestoreStartDate.Year.ToString() + "-" + this._RestoreStartDate.Month.ToString() + "-" + this._RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
                        SQL += " ORDER BY LogID DESC";
                        ad.SelectCommand.CommandText = SQL;
                        System.Data.DataTable dtDependent = new DataTable();
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
                            OK = this.RestoreDeletedData(RT, int.Parse(Rdep[0].ToString()), ref Error, ref Message);
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

        private bool RestoreGetDeletedRow(string Table, int LogID, ref string Error, ref string Message)
        {
            bool OK = true;

            string WhereClause = " WHERE [LogID] = " + LogID.ToString();
            if (this._UseRestoreStartDate)
                WhereClause += " and [LogDate] >= CONVERT(DATETIME, '" + this._RestoreStartDate.Year.ToString() + "-" + this._RestoreStartDate.Month.ToString() + "-" + this._RestoreStartDate.Day.ToString() + " 00:00:00', 102) ";
            string SQL = "SELECT * FROM [" + Table + "_log] " + WhereClause;
            System.Data.DataTable dtSource = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtSource);
            if (DiversityWorkbench.LogDatabase.Database.Exists() && dtSource.Rows.Count == 0 && this._LogSavedSchemata != null && this._LogSavedSchemata.Count > 0)
            {
                foreach (string Schema in this._LogSavedSchemata)
                {
                    SQL = "SELECT * FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[" + Schema + "].[" + Table + "_log] " + WhereClause;
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dtSource);
                    if (dtSource.Rows.Count > 0)
                        break;
                }
            }

            return OK;
        }

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> _RestoreFailedRowsDictionary;

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> RestoreFailedRowsDictionary
        {
            get
            {
                if (this._RestoreFailedRowsDictionary == null)
                    this._RestoreFailedRowsDictionary = new Dictionary<string, List<int>>();
                return _RestoreFailedRowsDictionary;
            }
            //set { _RestoreFailedRows = value; }
        }

        private void RestoreAddFailedRow(string Table, int LogID)
        {
            if (this.RestoreFailedRowsDictionary.ContainsKey(Table))
            {
                if (!this.RestoreFailedRowsDictionary[Table].Contains(LogID))
                    this.RestoreFailedRowsDictionary[Table].Add(LogID);
            }
            else
            {
                System.Collections.Generic.List<int> L = new List<int>();
                L.Add(LogID);
                this.RestoreFailedRowsDictionary.Add(Table, L);
            }
        }

        private int RestoreFailedRowsNumber()
        {
            int i = 0;
            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in this.RestoreFailedRowsDictionary)
            {
                foreach (int r in KV.Value)
                    i++;
            }
            return i;
        }

        private bool RestoreFailedRows()
        {
            bool OK = true;
            int iBefore = this.RestoreFailedRowsNumber();
            int iAfter = 0;
            string Message = "";
            string Error = "";
            // as long as number gets lower try to restore rows
            while (iAfter < iBefore)
            {
                // the dictionary keeping the restored rows
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> RestoredRowsDictionary = new Dictionary<string, List<int>>();
                // try to restore the rows
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in this.RestoreFailedRowsDictionary)
                {
                    foreach (int LogID in KV.Value)
                    {
                        if (this.RestoreDeletedRow(KV.Key, LogID, ref Error, ref Message, false))
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
                        if (this.RestoreFailedRowsDictionary[KV.Key].Contains(LogID))
                            this.RestoreFailedRowsDictionary[KV.Key].Remove(LogID);
                    }
                }
                iAfter = this.RestoreFailedRowsNumber();
                if (iAfter == 0)
                    break;
            }
            if (iAfter > 0)
                OK = false;
            return OK;
        }

        #endregion

        #region Include Saved Log

        private enum SavedLogState { Unchecked, Included, Missing }
        private System.Collections.Generic.Dictionary<string, SavedLogState> SavedLogIncluded;

        private void buttonIncludeSavedLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tabControlHistory.SelectedTab != null)
                {
                    string Table = this.tabControlHistory.SelectedTab.Text;
                    if (this._HistoryTables != null && this._HistoryTables.ContainsKey(Table))
                    {
                        this._HistoryTables[Table].GetSavedLogData();
                        if (this.SavedLogIncluded.ContainsKey(Table))
                            this.SavedLogIncluded[Table] = SavedLogState.Included;
                    }
                    else if (this.SavedLogIncluded != null && this.SavedLogIncluded.ContainsKey(Table))
                        this.SavedLogIncluded[Table] = SavedLogState.Missing;
                    this.buttonIncludeSavedLog.Visible = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void tabControlHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
#if DEBUG
            try
            {
                if (DiversityWorkbench.LogDatabase.Database.Exists())
                {
                    string Table = this.tabControlHistory.SelectedTab.Text;
                    if (this.SavedLogIncluded != null && this.SavedLogIncluded.ContainsKey(Table))
                    {
                        if (this.SavedLogIncluded[Table] == SavedLogState.Unchecked)
                            this.buttonIncludeSavedLog.Visible = true;
                        else
                            this.buttonIncludeSavedLog.Visible = false;
                    }
                }
                else
                    this.buttonIncludeSavedLog.Visible = false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
#endif
        }

        #endregion

    }
}