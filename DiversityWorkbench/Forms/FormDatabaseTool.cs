using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace DiversityWorkbench.Forms
{
    public partial class FormDatabaseTool : Form
    {

        #region Parameter

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private System.Data.DataTable _dtSynColTax;
        private System.Data.DataTable _dtSynColExs;
        private System.Data.DataTable _dtSynColGaz;
        private System.Data.DataTable _dtSynColRef;
        private System.Data.DataTable _dtCollectionProjects;
        private DiversityWorkbench.ServerConnection _ServerConnection_Collection;
        private DiversityWorkbench.ServerConnection _ServerConnection_Exsiccatae;
        private DiversityWorkbench.ServerConnection _ServerConnection_References;
        private DiversityWorkbench.ServerConnection _ServerConnection_Gazetteer;
        private DiversityWorkbench.ServerConnection _ServerConnection_Habitats;
        private DiversityWorkbench.ServerConnection _ServerConnection_TaxonNames;

        private string _ConnectionString = "";

        //private bool _useContext = false;
        private string _Language;

        Microsoft.SqlServer.Management.Smo.Database _Database;
        Microsoft.SqlServer.Management.Common.ServerConnection _Connection;

        private string _ServerVersion = "";

        System.Data.DataTable _dtCurrentTable;
        System.Data.DataTable _dtCurrentPK;
        //System.Data.DataTable _dtCurrentIndex;
        System.Data.DataTable _dtCurrentRelation;
        System.Data.DataTable _dtCurrentConstraint;

        //System.Data.DataTable _dtCurrentTableContent;
        //System.Data.DataTable _dtTableColumnsForContent;

        string _CurrentTable;
        string _CurrentPK;
        DiversityWorkbench.TaxonName _TaxonName;
        System.Collections.Generic.List<DiversityWorkbench.ServerConnection> _ServerConnections;
        private string[] _SynColGazTypes = new string[3] { "Place name", "Country", "Coordinates" };
        private string _ProcedureType = "";

        private System.Collections.Generic.List<System.Windows.Forms.TextBox> ProcedureParameterTextBoxes = new List<TextBox>();

        private System.Data.DataTable _dtEntityList;
        private System.Data.DataTable _dtEntityColumnList;
        private System.Data.DataTable _dtEntityColumnContentList;

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterWorkbenchEntity;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterWorkbenchEntityRepresentation;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterWorkbenchEntityUsage;

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterWorkbenchContext_Enum;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterWorkbenchEntityUsage_Enum;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterWorkbenchISO_Language_Enum;

        private string _SqlTrgUpd;
        private string _SqlTrgDel;

        public enum ObjectTypes { Function, Table, View, Column }

        private bool _SplitBySchema = false;

        //private enum RepPrepSteps { CreateRepPubTable, AddLogColumns, AddRowGUID, CreateDefault, CreateTempTab, ReadData, DeactivateTrigger, WriteGUID, WriteData, ActivateTrigger, DeleteTempTab};
        //private enum RepPrepStepState { NotNecessary, ToDo, Done };

        #endregion

        #region Construction

        public FormDatabaseTool()
        {
            InitializeComponent();
            this.fillDescriptionTree();
            this.setProcedureList();
            ///TODO: funktioniert nicht mit funktionen!
            this.tabControlMain.TabPages.Remove(this.tabPageProcedures);
            // Durch Skript ersetzt
            this.tabControlReplication.TabPages.Remove(this.tabPageRepPrep);
            ///TODO: in neuer Version wieder einblenden
            //this.tabControlMain.TabPages.Remove(this.tabPageSaveLog);
            //this.tabControlMain.TabPages.Remove(this.tabPageEuDsgvo);
            //this.tabControlMain.TabPages.Add(this.tabPageSaveLog);
            //this.tabControlMain.TabPages.Add(this.tabPageEuDsgvo);
            this.checkBoxDeleteTriggerAddDsgvo.Visible = true;
            this.checkBoxUpdateTriggerAddDsgvo.Visible = true;
            this.checkBoxRepPrepDsgvo.Visible = true;
            this.checkBoxProcSetVersionDsgvo.Visible = true;
            this.initEuDsgvo();
            this.initSaveLog();
            this.initDescriptionCache();
#if DEBUG
#endif
        }

        public FormDatabaseTool(bool ForSavingLog)
        {
            InitializeComponent();
            this.tabControlMain.TabPages.Remove(this.tabPageDescription);
            this.tabControlMain.TabPages.Remove(this.tabPageLogging);
            this.tabControlMain.TabPages.Remove(this.tabPageClearLog);
            this.tabControlMain.TabPages.Remove(this.tabPageProcedures);
            this.tabControlMain.TabPages.Remove(this.tabPageRowGUID);
            this.tabControlMain.TabPages.Remove(this.tabPageEuDsgvo);
            this.Text = "Transfer content of logging tables";
            this.initSaveLog();
        }

        public FormDatabaseTool(string ConnectionString, bool SplitBySchema = false)
        {
            InitializeComponent();
            this._SplitBySchema = SplitBySchema;
            this._ConnectionString = ConnectionString;
            this.fillDescriptionTree(ConnectionString);
            //this.setProcedureList(ConnectionString);
            this.tabControlMain.TabPages.Remove(this.tabPageProcedures);
            this.tabControlMain.TabPages.Remove(this.tabPageLogging);
            this.tabControlMain.TabPages.Remove(this.tabPageClearLog);
            this.tabControlMain.TabPages.Remove(this.tabPageProcedures);
            this.tabControlMain.TabPages.Remove(this.tabPageRowGUID);
            this.tabControlMain.TabPages.Remove(this.tabPageEuDsgvo);
            this.tabControlMain.TabPages.Remove(this.tabPageSaveLog);
            //this.checkBoxDeleteTriggerAddDsgvo.Visible = true;
            //this.checkBoxUpdateTriggerAddDsgvo.Visible = true;
            //this.checkBoxRepPrepDsgvo.Visible = true;
            //this.checkBoxProcSetVersionDsgvo.Visible = true;
            //this.initEuDsgvo();
            //this.initSaveLog();
            this.initDescriptionCache();
        }

        public void setHelpKeyWord(string KeyWord)
        {
            this.helpProvider.SetHelpKeyword(this.tabControlMain, KeyWord);
        }

        public void setHelpProvider(string HelpNamespace, string Keyword)
        {
            this.helpProvider.HelpNamespace = HelpNamespace;
            this.helpProvider.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpKeyword(this, Keyword);
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

        #region Form

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
        }

        public void setTitle(string Title, bool Attach = true)
        {
            if (Attach) this.Text += " " + Title;
            else this.Text = Title;
        }

        #endregion

        #region Logging

        #region Form

        public void setHelpProvider(string Path)
        {
            this.helpProvider.HelpNamespace = Path;
        }

        private void fillTableList()
        {
            System.Data.DataTable dt = this.TableList();
            if (dt.Columns.Count > 0)
            {
                this.listBoxLoggingTables.DataSource = dt;
                this.listBoxLoggingTables.DisplayMember = "TableName";
                this.listBoxLoggingTables.ValueMember = "TableName";

                System.Data.DataTable dtM = dt.Copy();
                this.comboBoxLoggingVersionMasterTable.DataSource = dtM;
                this.comboBoxLoggingVersionMasterTable.DisplayMember = "TableName";
                this.comboBoxLoggingVersionMasterTable.ValueMember = "TableName";
                this.comboBoxLoggingVersionMasterTable.SelectedIndex = -1;
            }
            else
            {
                this.listBoxLoggingTables.DataSource = null;
                this.comboBoxLoggingVersionMasterTable.DataSource = null;
            }
        }

        private System.Data.DataTable TableList()
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "";
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                if (this.ServerVersion == "2000")
                {
                    SQL = "SELECT TABLE_NAME as TableName " +
                        "FROM Information_Schema.Tables " +
                        "WHERE TABLE_NAME <> 'dtproperties' AND TABLE_TYPE = 'BASE TABLE' ORDER BY Table_Name";
                }
                else if (this.ServerVersion == "2005")
                {
                    SQL = "SELECT sys.tables.name as TableName " +
                        "FROM sys.tables " +
                        "WHERE sys.tables.name <> 'sysdiagrams' ORDER BY sys.tables.name";
                }
                else if (this.ServerVersion.Contains("2008") || this.ServerVersion.Contains("2012") || this.ServerVersion.Contains("2014"))
                {
                    SQL = "SELECT TABLE_NAME as TableName " +
                        "FROM Information_Schema.Tables " +
                        "WHERE TABLE_NAME <> 'dtproperties' AND TABLE_TYPE = 'BASE TABLE' ORDER BY Table_Name";
                }
                else
                {
                    SQL = "SELECT TABLE_NAME as TableName " +
                        "FROM Information_Schema.Tables " +
                        "WHERE TABLE_NAME <> 'dtproperties' AND TABLE_TYPE = 'BASE TABLE' ORDER BY Table_Name";
                }
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                try
                {
                    a.Fill(dt);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Please connect to database");
            return dt;
        }

        private string ServerVersion
        {
            get
            {
                if (this._ServerVersion != "") return this._ServerVersion;
                string SQL = "SELECT @@VERSION AS 'SQL Server Version'";
                if (DiversityWorkbench.Settings.DatabaseName == "") DiversityWorkbench.Settings.DatabaseName = "master";
                if (DiversityWorkbench.Settings.ConnectionString != "")
                {
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        this._ServerVersion = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    }
                    finally { con.Close(); }
                }
                if (this._ServerVersion.Length > 0)
                {
                    if (this._ServerVersion.IndexOf("Microsoft SQL Server 2005") == 0) this._ServerVersion = "2005";
                    else if (this._ServerVersion.IndexOf("Microsoft SQL Server  2000") == 0) this._ServerVersion = "2000";
                }
                return this._ServerVersion;
            }
            set
            {
                this._ServerVersion = value;
            }
        }

        private System.Data.DataTable dtTable(string TableName)
        {
            if (this._dtCurrentTable == null) this._dtCurrentTable = new DataTable();
            if (this._dtCurrentTable.Rows.Count > 0 && TableName == this._CurrentTable)
                return this._dtCurrentTable;
            else
                this._dtCurrentTable.Clear();
            this._CurrentTable = TableName;
            string SQL = "";
            SQL = "SELECT COLUMN_NAME as ColumnName, DATA_TYPE as Datatype, CHARACTER_MAXIMUM_LENGTH as Length, " +
                "COLLATION_NAME  AS Collation, COLUMN_DEFAULT AS DefaultValue, '' AS Description " +
                "FROM Information_Schema.COLUMNS  " +
                "WHERE Information_Schema.COLUMNS.TABLE_NAME = '" + TableName + "'";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                a.Fill(this._dtCurrentTable);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return this._dtCurrentTable;
        }

        private System.Data.DataTable _dtCurrentTableColumns;
        private System.Data.DataTable dtTableColumns(string TableName)
        {
            if (this._dtCurrentTableColumns == null) this._dtCurrentTableColumns = new DataTable();
            if (this._dtCurrentTableColumns.Rows.Count > 0 && TableName == this._CurrentTable)
                return this._dtCurrentTableColumns;
            else
                this._dtCurrentTableColumns.Clear();
            this._CurrentTable = TableName;
            string SQL = "";
            SQL = "SELECT C.COLUMN_NAME as ColumnName, C.DATA_TYPE as Datatype, C.CHARACTER_MAXIMUM_LENGTH as Length, " +
                "C.COLLATION_NAME  AS Collation, C.COLUMN_DEFAULT AS DefaultValue, '' AS Description  " +
                "FROM Information_Schema.COLUMNS C, Information_Schema.COLUMNS L " +
                "WHERE C.TABLE_NAME = '" + TableName + "' " +
                "AND L.TABLE_NAME = '" + TableName + "_log' " +
                "AND C.COLUMN_NAME = L.COLUMN_NAME";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                a.Fill(this._dtCurrentTableColumns);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return this._dtCurrentTableColumns;
        }

        private void comboBoxLoggingVersionMasterTable_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxLoggingVersionMasterTable.DataSource == null || this.comboBoxLoggingVersionMasterTable.Items.Count == 0)
            {
                System.Data.DataTable dt = this.TableList();
                this.comboBoxLoggingVersionMasterTable.DataSource = dt;
                this.comboBoxLoggingVersionMasterTable.DisplayMember = "TableName";
                this.comboBoxLoggingVersionMasterTable.ValueMember = "TableName";
            }
        }

        private void listBoxLoggingTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
            string Table = rv[0].ToString();
            System.Data.DataTable dt = this.dtTable(Table);
            this.dataGridViewTable.DataSource = dt;
            int LogUpdatedColumns = 0;
            foreach (System.Windows.Forms.DataGridViewRow r in this.dataGridViewTable.Rows)
            {
                if (r.DataBoundItem != null)
                {
                    System.Data.DataRowView drv = (System.Data.DataRowView)r.DataBoundItem;
                    string Column = drv["ColumnName"].ToString();
                    if (Column.IndexOf("Log") == 0 || Column == "Version" || Column == "RevisionLevel" || Column == "RowGUID")
                    {
                        r.Cells[0].Value = 1;
                    }
                    if (Column.IndexOf("LogUpdated") == 0)
                        LogUpdatedColumns++;
                }
            }
            if (LogUpdatedColumns == 2)
                this.buttonAttachLogColumns.Enabled = false;
            else
                this.buttonAttachLogColumns.Enabled = true;
            this.textBoxInsertTrigger.Text = this.OriginalSQL("trgIns" + Table);
            this.textBoxDeleteTrigger.Text = this.OriginalSQL("trgDel" + Table);
            this.textBoxUpdateTrigger.Text = this.OriginalSQL("trgUpd" + Table);
            this.textBoxInsertTriggerNew.Text = "";
            this.textBoxDeleteTriggerNew.Text = "";
            this.textBoxUpdateTriggerNew.Text = "";
            this.textBoxLogTable.Text = "";
            this._dtCurrentTableColumns = null;
        }

        #endregion

        #region Log table

        private void buttonLogTableShowSQL_Click(object sender, EventArgs e)
        {
            if (this.listBoxLoggingTables.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a table");
                return;
            }
            System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
            string Table = rv[0].ToString();
            this.textBoxLogTable.Text = this.SqlLogTable(Table, false);
        }

        private void buttonLogTableCreate_Click(object sender, EventArgs e)
        {
            string SQL = "";
            try
            {
                // dropping old table
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
                string Table = rv[0].ToString();
                if (this.checkBoxKeepOldLogTable.Checked)
                    this.saveOldLogTable(Table);
                System.Data.DataTable dt = this.dtTable(Table);
                SQL = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + Table + "_log]') AND type in (N'U')) " +
                    "DROP TABLE [dbo].[" + Table + "_log]";
                this.ExecuteSQL(SQL);
                // creating new table
                SQL = this.textBoxLogTable.Text;
                if (this.ExecuteSQL(SQL))
                    System.Windows.Forms.MessageBox.Show("Log table created");
                else
                    System.Windows.Forms.MessageBox.Show("SQL contains syntax error\r\n\r\n" + SQL);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
        }

        private void buttonCreateLogTable_Click(object sender, EventArgs e)
        {
            if (this.listBoxLoggingTables.SelectedIndex > -1)
            {
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
                string Table = rv[0].ToString();
                System.Data.DataTable dt = this.dtTable(Table);
                if (dt.Rows.Count > 0)
                    this.CreateLogTable(Table);
            }
        }

        private void CreateLogTable(string Table)
        {
            if (Table.EndsWith("_log") || Table.IndexOf("_log_") > -1)
            {
                System.Windows.Forms.MessageBox.Show("You can not create a log table for the log table " + Table);
                return;
            }
            if (this.checkBoxKeepOldLogTable.Checked)
                this.saveOldLogTable(Table);
            System.Data.DataTable dt = this.dtTable(Table);
            string SQL = "DROP TABLE [dbo].[" + Table + "_log]";
            this.ExecuteSQL(SQL);
            string User = "user_name()";
            if (this.checkBoxLogTableDSGVO.Checked)
                User = "cast(dbo.UserID() as varchar)";
            SQL = "CREATE TABLE [dbo].[" + Table + "_log] (";
            int i = 0;
            foreach (System.Data.DataRow r in dt.Rows)
            {
                i++;
                SQL += "[" + r["ColumnName"].ToString() + "] [" + r["Datatype"].ToString() + "]";
                if (!r["Length"].Equals(System.DBNull.Value) && r["Datatype"].ToString() != "timestamp")
                    SQL += " (" + r["Length"].ToString() + ") COLLATE " + r["Collation"].ToString();
                SQL += " NULL,\r\n ";
            }
            SQL += "[LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_" + Table + "_Log_LogState]  DEFAULT ('U'), \r\n" +
                "[LogDate] [datetime] NOT NULL CONSTRAINT [DF_" + Table + "_Log_LogDate]  DEFAULT (getdate()), \r\n" +
                "[LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_" + Table + "_Log_LogUser]  DEFAULT (" + User + "), \r\n";
            if (Table != this.comboBoxLoggingVersionMasterTable.Text)
                SQL += "[LogVersion] [int] NULL, \r\n";
            SQL += "[LogID] [int] IDENTITY(1,1) NOT NULL, \r\n";
            if (this.ServerVersion == "2000")
            {
                SQL += "CONSTRAINT [PK_" + Table + "_Log] PRIMARY KEY  CLUSTERED " +
                    "( " +
                    "    [LogID] " +
                    ") WITH  FILLFACTOR = 90  ON [PRIMARY]  " +
                    ") ON [PRIMARY] ";
            }
            else
            {
                SQL += " CONSTRAINT [PK_" + Table + "_Log] PRIMARY KEY CLUSTERED \r\n" +
                "([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] " +
                ") ON [PRIMARY] ";
            }
            this.ExecuteSQL(SQL);
            this.fillTableList();
        }

        private string SqlLogTable(string Table, bool AttachReplicationColumns)
        {
            Replication.AttachReplicationColumns = AttachReplicationColumns;
            if (Table.EndsWith("_log") || Table.IndexOf("_log_") > -1)
            {
                System.Windows.Forms.MessageBox.Show("You can not create a log table for the log table " + Table);
                return "";
            }
            string User = "user_name()";
            if (this.checkBoxLogTableDSGVO.Checked)
                User = "cast(dbo.UserID() as varchar)";
            string SQL = "CREATE TABLE [dbo].[" + Table + "_log] \r\n(";
            int i = 0;
            System.Data.DataTable dt = this.dtTable(Table);
            foreach (System.Data.DataRow r in dt.Rows)
            {
                i++;
                SQL += "[" + r["ColumnName"].ToString() + "] [" + r["Datatype"].ToString() + "]";
                if (r["Datatype"].ToString() == "geography"
                    || r["Datatype"].ToString() == "geometry"
                    || r["Datatype"].ToString() == "xml")
                {
                }
                else if (!r["Length"].Equals(System.DBNull.Value) && r["Datatype"].ToString() != "timestamp")
                {
                    SQL += " (";
                    if (r["Length"].ToString() == "0" || r["Length"].ToString() == "-1") SQL += " MAX ";
                    else SQL += r["Length"].ToString();
                    SQL += ") COLLATE " + r["Collation"].ToString();
                }
                SQL += " NULL,\r\n ";
            }
            if (AttachReplicationColumns)
            {
                foreach (System.Data.DataRow R in Replication.DtReplicationColumns().Rows)
                {
                    System.Data.DataRow[] rr = dt.Select("ColumnName = '" + R["ColumnName"].ToString() + "'");
                    if (rr.Length == 0)
                    {
                        SQL += "[" + R["ColumnName"].ToString() + "] [" + R["Datatype"].ToString() + "]";
                        if (!R["Length"].Equals(System.DBNull.Value) && R["Datatype"].ToString() != "timestamp")
                        {
                            SQL += " (";
                            if (R["Length"].ToString() == "0" || R["Length"].ToString() == "-1") SQL += " MAX ";
                            else SQL += R["Length"].ToString();
                            SQL += ") COLLATE " + R["Collation"].ToString();
                        }
                        SQL += ",\r\n";
                    }
                }
            }
            SQL += " [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_" + Table + "_Log_LogState]  DEFAULT ('U'), \r\n" +
                " [LogDate] [datetime] NOT NULL CONSTRAINT [DF_" + Table + "_Log_LogDate]  DEFAULT (getdate()), \r\n" +
                " [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_" + Table + "_Log_LogUser]  DEFAULT (" + User + "), \r\n";
            if (Table != this.comboBoxLoggingVersionMasterTable.Text && this.checkBoxAddVersion.Checked)
                SQL += " [LogVersion] [int] NULL, \r\n";
            SQL += " [LogID] [int] IDENTITY(1,1) NOT NULL, \r\n";
            if (this.ServerVersion == "2000")
            {
                SQL += "CONSTRAINT [PK_" + Table + "_Log] PRIMARY KEY  CLUSTERED \r\n" +
                    "( " +
                    "    [LogID] " +
                    ") WITH  FILLFACTOR = 90  ON [PRIMARY]  " +
                    ") \r\nON [PRIMARY] ";
            }
            else
            {
                SQL += " CONSTRAINT [PK_" + Table + "_Log] PRIMARY KEY CLUSTERED \r\n" +
                " ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] " +
                ") \r\nON [PRIMARY] ";
            }
            return SQL;
        }

        private void saveOldLogTable(string Table)
        {
            if (this.checkBoxKeepOldLogTable.Checked)
            {
                string SQL = "SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + Table + "_log]') AND type in (N'U')";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    int ilog = System.Int32.Parse(cmd.ExecuteScalar().ToString());
                    if (ilog > 0)
                    {
                        SQL = "ALTER TABLE [dbo].[" + Table + "_Log] DROP CONSTRAINT [PK_" + Table + "_Log]";
                        this.ExecuteSQL(SQL);
                        SQL = "ALTER TABLE [dbo].[" + Table + "_Log] DROP CONSTRAINT [DF_" + Table + "_Log_LogState]";
                        this.ExecuteSQL(SQL);
                        SQL = "ALTER TABLE [dbo].[" + Table + "_Log] DROP CONSTRAINT [DF_" + Table + "_Log_LogUser]";
                        this.ExecuteSQL(SQL);
                        SQL = "ALTER TABLE [dbo].[" + Table + "_Log] DROP CONSTRAINT [DF_" + Table + "_Log_LogDate]";
                        this.ExecuteSQL(SQL);
                        SQL = "EXEC sp_rename '" + Table + "_log', '" + Table + "_log_" + System.DateTime.Now.ToString() + "'";
                        this.ExecuteSQL(SQL);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    if (con.State.ToString() == "Open")
                        con.Close();
                }
            }
        }
        #endregion

        #region Trigger

        #region Insert trigger

        private void buttonInsertTriggerShowSQL_Click(object sender, EventArgs e)
        {
            if (this.listBoxLoggingTables.SelectedIndex > -1)
            {
                string SqlIns = "";
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
                string Table = rv[0].ToString();
                if (this.comboBoxLoggingVersionMasterTable.Text == Table)
                {
                    System.Windows.Forms.MessageBox.Show("No insert trigger for main table");
                    return;
                }
                if (this.textBoxInsertTrigger.Text.Length == 0)
                {
                    this.textBoxInsertTrigger.Text = this.OriginalSQL("trgIns" + Table);
                }
                System.Data.DataTable dt = this.dtTable(Table);
                if (dt.Rows.Count > 0)
                {
                    // creating new triggers
                    SqlIns = "CREATE TRIGGER [trgIns" + Table + "] ON [dbo].[" + Table + "] \r\n" +
                        "FOR INSERT AS\r\n\r\n" +
                        "/*  Created by DiversityWorkbench Administration.  */ \r\n" +
                        "/*  " + System.Windows.Forms.Application.ProductName.ToString() + " " +
                        " " + System.Windows.Forms.Application.ProductVersion.ToString() + " */ \r\n" +
                        "/*  Date: " + System.DateTime.Now.ToShortDateString() + "  */ \r\n";
                    if (this.checkBoxInsertTriggerAddVersion.Checked && this.comboBoxLoggingVersionMasterTable.Text.Length > 0)
                    {
                        SqlIns += this.SqlVersion(true);
                    }
                    this.textBoxInsertTriggerNew.Text = SqlIns;
                }
            }
        }

        private void buttonInsertTriggerCreate_Click(object sender, EventArgs e)
        {
            string SQL = "";
            try
            {
                // dropping old trigger
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
                string Table = rv[0].ToString();
                string Trigger = "trgIns" + Table;
                this.dropTrigger(Trigger);
                // creating new trigger
                SQL = this.textBoxInsertTriggerNew.Text;
                if (SQL.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No trigger defined");
                    return;
                }
                if (this.ExecuteSQL(SQL))
                    System.Windows.Forms.MessageBox.Show("Trigger created");
                else
                    System.Windows.Forms.MessageBox.Show("SQL contains syntax error\r\n\r\n" + SQL);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
        }

        #endregion

        #region Update trigger

        private void buttonUpdateTriggerShowSQL_Click(object sender, EventArgs e)
        {
            if (this.listBoxLoggingTables.SelectedIndex > -1)
            {
                string SqlUpd = "";
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
                string Table = rv[0].ToString();
                if (this.textBoxUpdateTrigger.Text.Length == 0)
                {
                    this.textBoxUpdateTrigger.Text = this.OriginalSQL("trgUpd" + Table);
                }
                System.Data.DataTable dt = this.dtTable(Table);
                if (dt.Rows.Count > 0)
                {
                    // creating new triggers
                    string VersionTable = "";
                    if (this.checkBoxUpdateTriggerAddVersion.Checked && this.comboBoxLoggingVersionMasterTable.Text.Length > 0)
                        VersionTable = this.comboBoxLoggingVersionMasterTable.Text;
                    SqlUpd = this.SqlUpdateTrigger(Table, this.checkBoxUpdateTriggerAddVersion.Checked, VersionTable, this.checkBoxUpdateTriggerAddDsgvo.Checked);
                    //SqlUpd = "CREATE TRIGGER [trgUpd" + Table + "] ON [dbo].[" + Table + "] \r\n" +
                    //    "FOR UPDATE AS\r\n\r\n" +
                    //    "/*  Created by DiversityWorkbench Administration.  */ \r\n" +
                    //    "/*  " + System.Windows.Forms.Application.ProductName.ToString() + " " +
                    //    " " + System.Windows.Forms.Application.ProductVersion.ToString() + " */ \r\n" +
                    //    "/*  Date: " + System.DateTime.Now.ToShortDateString() + "  */ \r\n";
                    //if (this.checkBoxUpdateTriggerAddVersion.Checked && this.comboBoxLoggingVersionMasterTable.Text.Length > 0)
                    //{
                    //    if (this.comboBoxLoggingVersionMasterTable.Text == Table)
                    //        SqlUpd += "\r\nif not update(Version) \r\nBEGIN\r\n";
                    //    SqlUpd += this.SqlVersion(false);
                    //}
                    //string SqlInsert = this.SqlInsertLogTable(Table, 'U', this.checkBoxUpdateTriggerAddVersion.Checked, false, this.checkBoxUpdateTriggerAddDsgvo.Checked);
                    //SqlUpd += "\r\n\r\n" + SqlInsert;
                    //if (this.checkBoxUpdateTriggerAddVersion.Checked && this.comboBoxLoggingVersionMasterTable.Text.Length > 0)
                    //{
                    //    if (this.comboBoxLoggingVersionMasterTable.Text == Table)
                    //        SqlUpd += "\r\nEND";
                    //}
                    //SqlUpd += "\r\n\r\n/* updating the logging columns */\r\n" +
                    //    "Update T \r\n" +
                    //    "set T.LogUpdatedWhen = getdate(), ";
                    //if (this.checkBoxUpdateTriggerAddDsgvo.Checked)
                    //    SqlUpd += " T.LogUpdatedBy = U.ID \r\n" + // current_user\r\n" +
                    //    "FROM " + Table + " T, deleted D, UserProxy U \r\n" +
                    //    "where U.LoginName = SUSER_NAME() ";
                    //else
                    //    SqlUpd += " LogUpdatedBy = SUSER_NAME()\r\n" + // current_user\r\n" +
                    //    "FROM " + Table + " T, deleted D \r\n" +
                    //    "where 1 = 1 ";
                    //System.Data.DataTable dtPK = this.dtPrimaryKey(Table);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    foreach (System.Data.DataRow r in dtPK.Rows)
                    //    {
                    //        SqlUpd += "\r\nAND T." + r["ColumnName"].ToString() + " = D." + r["ColumnName"].ToString();
                    //    }
                    //}
                    this.textBoxUpdateTriggerNew.Text = SqlUpd;
                }
            }
        }

        private string SqlUpdateTrigger(string Table, bool AddVersion, string VersionTable, bool AddDsgvo)
        {
            System.Data.DataTable dt = this.dtTable(Table);
            string User = "user_name()";
            if (AddDsgvo)
                User = "cast(dbo.UserID() as varchar)";
            string SqlUpd = "CREATE TRIGGER [trgUpd" + Table + "] ON [dbo].[" + Table + "] \r\n" +
                "FOR UPDATE AS\r\n\r\n" +
                "/*  Created by DiversityWorkbench Administration.  */ \r\n" +
                "/*  " + System.Windows.Forms.Application.ProductName.ToString() + " " +
                " " + System.Windows.Forms.Application.ProductVersion.ToString() + " */ \r\n" +
                "/*  Date: " + System.DateTime.Now.ToShortDateString() + "  */ \r\n";
            if (VersionTable.Length > 0)
            {
                if (VersionTable == Table)
                    SqlUpd += "\r\nif not update(Version) \r\nBEGIN\r\n";
                SqlUpd += this.SqlVersion(false, Table, VersionTable);
            }
            string SqlInsert = this.SqlInsertLogTable(Table, 'U', AddVersion, VersionTable, false, AddDsgvo);
            SqlUpd += "\r\n\r\n" + SqlInsert;
            if (VersionTable.Length > 0)
            {
                if (VersionTable == Table)
                    SqlUpd += "\r\nEND";
            }
            SqlUpd += "\r\n\r\n/* updating the logging columns */\r\n" +
                "Update T \r\n" +
                "set T.LogUpdatedWhen = getdate(), ";
            if (AddDsgvo)
                SqlUpd += " T.LogUpdatedBy = " + User + " \r\n" + // current_user\r\n" +
                "FROM " + Table + " T, deleted D " + //, UserProxy U \r\n" +
                "where 1 = 1 ";// U.LoginName = SUSER_NAME() ";
            //if (AddDsgvo)
            //    SqlUpd += " T.LogUpdatedBy = U.ID \r\n" + // current_user\r\n" +
            //    "FROM " + Table + " T, deleted D, UserProxy U \r\n" +
            //    "where U.LoginName = SUSER_NAME() ";
            else
                SqlUpd += " LogUpdatedBy = SUSER_NAME()\r\n" + // current_user\r\n" +
                "FROM " + Table + " T, deleted D \r\n" +
                "where 1 = 1 ";
            System.Data.DataTable dtPK = this.dtPrimaryKey(Table);
            if (dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow r in dtPK.Rows)
                {
                    SqlUpd += "\r\nAND T." + r["ColumnName"].ToString() + " = D." + r["ColumnName"].ToString();
                }
            }
            return SqlUpd;
        }

        private string SqlUpdateTrigger(string Table, string VersionTable, bool AttachReplicationColumns, bool IncludeDsgvo = false)
        {
            string SqlUpd = "";
            string User = "SUSER_NAME()";
            if (IncludeDsgvo)
                User = "cast(dbo.UserID() as varchar)";
            System.Data.DataTable dt = this.dtTable(Table);
            if (dt.Rows.Count > 0)
            {
                // creating new triggers
                SqlUpd = "CREATE TRIGGER [trgUpd" + Table + "] ON [dbo].[" + Table + "] \r\n" +
                    "FOR UPDATE AS\r\n\r\n" +
                    "/*  Created by DiversityWorkbench Administration.  */ \r\n" +
                    "/*  " + System.Windows.Forms.Application.ProductName.ToString() + " " +
                    " " + System.Windows.Forms.Application.ProductVersion.ToString() + " */ \r\n" +
                    "/*  Date: " + System.DateTime.Now.ToShortDateString() + "  */ \r\n";
                if (VersionTable.Length > 0)
                {
                    if (VersionTable == Table)
                        SqlUpd += "\r\nif not update(Version) \r\nBEGIN\r\n";
                    SqlUpd += this.SqlVersion(false, Table, VersionTable);
                }
                bool UseVersion = false;
                if (VersionTable.Length > 0)
                    UseVersion = true;
                string SqlInsert = this.SqlInsertLogTable(Table, 'U', UseVersion, this.comboBoxLoggingVersionMasterTable.Text, AttachReplicationColumns);
                SqlUpd += "\r\n\r\n" + SqlInsert;
                if (VersionTable.Length > 0)
                {
                    if (VersionTable == Table)
                        SqlUpd += "\r\nEND";
                }
                SqlUpd += "\r\n\r\n/* updating the logging columns */\r\n" +
                    "Update " + Table + "\r\n" +
                    "set LogUpdatedWhen = getdate(), LogUpdatedBy = " + User + " \r\n" + //current_user
                    "FROM " + Table + ", deleted \r\n" +
                    "where 1 = 1 ";
                System.Data.DataTable dtPK = this.dtPrimaryKey(Table);
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow r in dtPK.Rows)
                    {
                        SqlUpd += "\r\nAND " + Table + "." + r["ColumnName"].ToString() + " = deleted." + r["ColumnName"].ToString();
                    }
                }
            }
            return SqlUpd;
        }

        private void buttonUpdateTriggerCreate_Click(object sender, EventArgs e)
        {
            string SQL = "";
            try
            {
                // dropping old trigger
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
                string Table = rv[0].ToString();
                string Trigger = "trgUpd" + Table;
                this.dropTrigger(Trigger);
                // creating new trigger
                SQL = this.textBoxUpdateTriggerNew.Text;
                if (SQL.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No trigger defined");
                    return;
                }
                if (this.ExecuteSQL(SQL))
                    System.Windows.Forms.MessageBox.Show("Trigger created");
                else
                    System.Windows.Forms.MessageBox.Show("SQL contains syntax error\r\n\r\n" + SQL);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
        }

        #endregion

        #region Delete trigger

        private void buttonDeleteTriggerShowSql_Click(object sender, EventArgs e)
        {
            if (this.listBoxLoggingTables.SelectedIndex > -1)
            {
                string SqlDel = "";
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
                string Table = rv[0].ToString();
                if (this.textBoxUpdateTrigger.Text.Length == 0)
                {
                    this.textBoxUpdateTrigger.Text = this.OriginalSQL("trgDel" + Table);
                }
                System.Data.DataTable dt = this.dtTable(Table);
                if (dt.Rows.Count > 0)
                {
                    // creating new triggers
                    //string ColumnList = this.ColumnList(Table);
                    if (this.checkBoxDeleteTriggerAddVersion.Checked && !this.LogTableContainsColumnLogVersion(Table))
                    {
                        System.Windows.Forms.MessageBox.Show("Logtable " + Table + "_log does not contain the column LogVersion for inserting the version");
                        return;
                    }
                    SqlDel = this.SqlDeleteTrigger(Table, this.comboBoxLoggingVersionMasterTable.Text, this.checkBoxDeleteTriggerAddDsgvo.Checked);
                    //string SqlInsert = this.SqlInsertLogTable(Table, 'D', this.checkBoxDeleteTriggerAddVersion.Checked, this.comboBoxLoggingVersionMasterTable.Text, false, this.checkBoxDeleteTriggerAddDsgvo.Checked);

                    //SqlDel += "CREATE TRIGGER [trgDel" + Table + "] ON [dbo].[" + Table + "] \r\n" +
                    //    "FOR DELETE AS \r\n\r\n" +
                    //    "/*  Created by DiversityWorkbench Administration.  */ \r\n" +
                    //    "/*  " + System.Windows.Forms.Application.ProductName.ToString() + " " +
                    //    " " + System.Windows.Forms.Application.ProductVersion.ToString() + " */ \r\n" +
                    //    "/*  Date: " + System.DateTime.Now.ToShortDateString() + "  */ \r\n\r\n";

                    //if (this.checkBoxDeleteTriggerAddVersion.Checked && this.comboBoxLoggingVersionMasterTable.Text.Length > 0 && this.comboBoxLoggingVersionMasterTable.Text != Table)
                    //{
                    //    SqlDel += this.SqlVersion(false);
                    //}

                    //SqlDel += "\r\n" + SqlInsert;

                    this.textBoxDeleteTriggerNew.Text = SqlDel;
                }
            }
        }

        private string SqlDeleteTrigger(string Table, string VersionTable, bool AddDsgvo)
        {
            bool UseVersion = false;
            if (VersionTable.Length > 0)
                UseVersion = true;
            string SqlInsert = this.SqlInsertLogTable(Table, 'D', UseVersion, VersionTable, false, AddDsgvo);

            string SqlDel = "CREATE TRIGGER [trgDel" + Table + "] ON [dbo].[" + Table + "] \r\n" +
                "FOR DELETE AS \r\n\r\n" +
                "/*  Created by DiversityWorkbench Administration.  */ \r\n" +
                "/*  " + System.Windows.Forms.Application.ProductName.ToString() + " " +
                " " + System.Windows.Forms.Application.ProductVersion.ToString() + " */ \r\n" +
                "/*  Date: " + System.DateTime.Now.ToShortDateString() + "  */ \r\n\r\n";

            if (VersionTable.Length > 0 && VersionTable != Table)
            {
                SqlDel += this.SqlVersion(false, Table, VersionTable);
            }

            SqlDel += "\r\n" + SqlInsert;
            return SqlDel;
        }

        private string SqlDeleteTrigger(string Table, string VersionTable, bool AttachReplicationColumns, bool IncludeDsgvo = false)
        {
            string SqlDel = "";
            System.Data.DataTable dt = this.dtTable(Table);
            if (dt.Rows.Count > 0)
            {
                // creating new triggers
                string ColumnList = this.ColumnList(Table);

                bool UseVersion = false;
                if (VersionTable.Length > 0)
                    UseVersion = true;
                string SqlInsert = this.SqlInsertLogTable(Table, 'D', UseVersion, this.comboBoxLoggingVersionMasterTable.Text, AttachReplicationColumns, IncludeDsgvo);

                SqlDel += "CREATE TRIGGER [trgDel" + Table + "] ON [dbo].[" + Table + "] \r\n" +
                    "FOR DELETE AS \r\n\r\n" +
                    "/*  Created by DiversityWorkbench Administration.  */ \r\n" +
                    "/*  " + System.Windows.Forms.Application.ProductName.ToString() + " " +
                    " " + System.Windows.Forms.Application.ProductVersion.ToString() + " */ \r\n" +
                    "/*  Date: " + System.DateTime.Now.ToShortDateString() + "  */ \r\n\r\n";

                if (UseVersion && VersionTable != Table)
                {
                    SqlDel += this.SqlVersion(false, Table, VersionTable);
                }

                SqlDel += "\r\n" + SqlInsert;

            }
            return SqlDel;
        }

        private void buttonDeleteTriggerCreate_Click(object sender, EventArgs e)
        {
            string SQL = "";
            try
            {
                // dropping old trigger
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
                string Table = rv[0].ToString();
                string Trigger = "trgDel" + Table;
                this.dropTrigger(Trigger);
                // creating new trigger
                SQL = this.textBoxDeleteTriggerNew.Text;
                if (SQL.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No trigger defined");
                    return;
                }
                if (this.ExecuteSQL(SQL))
                    System.Windows.Forms.MessageBox.Show("Trigger created");
                else
                    System.Windows.Forms.MessageBox.Show("SQL contains syntax error\r\n\r\n" + SQL);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
        }

        #endregion

        #region Common

        private string SqlInsertLogTable(string Table, char TriggerType, bool IncludeVersion, string VersionTable, bool AttachReplicationColumns, bool IncludeDsgvo = false)
        {
            Replication.AttachReplicationColumns = AttachReplicationColumns;
            string PkColumn = "";
            if (IncludeVersion)
            {
                if (VersionTable.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a master table for the version setting");
                    return "";
                }
                PkColumn = this.dtPrimaryKey(VersionTable).Rows[0][0].ToString();
            }
            bool ForeignKeyInTable = this.ColumnInTable(PkColumn, Table);
            string ColumnListInsert = this.ColumnList(Table, IncludeDsgvo);
            string Prefix = "deleted";
            string User = "SUSER_NAME()";
            if (IncludeDsgvo)
            {
                User = "cast(dbo.UserID() as varchar)";
                Prefix = "D";
            }
            string ColumnListSelect = this.ColumnList(Table, Prefix, IncludeDsgvo);
            string SqlInsert = "/* saving the original dataset in the logging table */ \r\n";
            if (Table != VersionTable && ForeignKeyInTable && IncludeVersion) SqlInsert += "if (not @Version is null) \r\nbegin\r\n";
            SqlInsert += "INSERT INTO " + Table.ToString() + "_Log (" + ColumnListInsert;
            if (Table != VersionTable && ForeignKeyInTable && IncludeVersion) SqlInsert += " LogVersion, ";
            SqlInsert += " LogState) \r\nSELECT " + ColumnListSelect;
            if (Table != VersionTable && ForeignKeyInTable && IncludeVersion)
            {
                if (!SqlInsert.Trim().EndsWith(",")) SqlInsert += ", ";
                SqlInsert += " @Version, ";
            }
            if (!SqlInsert.Trim().EndsWith(",")) SqlInsert += ", ";
            if (IncludeDsgvo)
            {
                if (true)
                {
                    SqlInsert += " '" + TriggerType + "'\r\nFROM DELETED D \r\n";
                    if (Table != VersionTable && ForeignKeyInTable && IncludeVersion)
                    {
                        SqlInsert += "end\r\nelse\r\nbegin\r\n" +
                        "if (select count(*) FROM DELETED D, " + VersionTable + " V WHERE D." + PkColumn + " = V." + PkColumn + ") > 0 " +
                        "\r\nbegin\r\n" +
                        "INSERT INTO " + Table.ToString() + "_Log (" + ColumnListInsert +
                        " LogVersion, LogState) \r\nSELECT " + ColumnListSelect + ", " +
                        "V.Version, '" + TriggerType + "' \r\nFROM DELETED D, " + VersionTable + " V\r\n" +
                        "WHERE D." + PkColumn + " = V." + PkColumn + "\r\nend" +
                        "\r\nelse" +
                        "\r\nbegin\r\n" +
                        "INSERT INTO " + Table.ToString() + "_Log (" + ColumnListInsert +
                        " LogVersion, LogState) \r\nSELECT " + ColumnListSelect + ", -1, '" + TriggerType + "' \r\n" +
                        "FROM DELETED D " +
                        "\r\nend" +
                        "\r\nend";
                    }
                }
                else // Old version
                {
                    SqlInsert += " '" + TriggerType + "'\r\nFROM DELETED D, UserProxy U \r\n";
                    SqlInsert += " WHERE U.LoginName = suser_sname()\r\n";
                    if (Table != VersionTable && ForeignKeyInTable && IncludeVersion)
                    {
                        SqlInsert += "end\r\nelse\r\nbegin\r\n" +
                        "if (select count(*) FROM DELETED D, " + VersionTable + " V WHERE D." + PkColumn + " = V." + PkColumn + ") > 0 " +
                        "\r\nbegin\r\n" +
                        "INSERT INTO " + Table.ToString() + "_Log (" + ColumnListInsert +
                        " LogVersion, LogState) \r\nSELECT " + ColumnListSelect + ", " +
                        "V.Version, '" + TriggerType + "' \r\nFROM DELETED D, UserProxy U, " + VersionTable + " V\r\n" +
                        "WHERE U.LoginName = suser_sname() AND D." + PkColumn + " = V." + PkColumn + "\r\nend" +
                        "\r\nelse" +
                        "\r\nbegin\r\n" +
                        "INSERT INTO " + Table.ToString() + "_Log (" + ColumnListInsert +
                        " LogVersion, LogState) \r\nSELECT " + ColumnListSelect + ", -1, '" + TriggerType + "' \r\nFROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() " +
                        "\r\nend" +
                        "\r\nend";
                    }
                }
            }
            else
            {
                SqlInsert += " '" + TriggerType + "'\r\nFROM DELETED\r\n";
                if (Table != VersionTable && ForeignKeyInTable && IncludeVersion)
                {
                    SqlInsert += "end\r\nelse\r\nbegin\r\n" +
                    "if (select count(*) FROM DELETED, " + VersionTable + " WHERE deleted." + PkColumn + " = " + VersionTable + "." + PkColumn + ") > 0 " +
                    "\r\nbegin\r\n" +
                    "INSERT INTO " + Table.ToString() + "_Log (" + ColumnListInsert +
                    " LogVersion, LogState) \r\nSELECT " + ColumnListSelect + ", " +
                    VersionTable + ".Version, '" + TriggerType + "' \r\nFROM DELETED, " + VersionTable + "\r\n" +
                    "WHERE deleted." + PkColumn + " = " + VersionTable + "." + PkColumn + "\r\nend" +
                    "\r\nelse" +
                    "\r\nbegin\r\n" +
                    "INSERT INTO " + Table.ToString() + "_Log (" + ColumnListInsert +
                    " LogVersion, LogState) \r\nSELECT " + ColumnListSelect + ", -1, '" + TriggerType + "' \r\nFROM DELETED" +
                    "\r\nend" +
                    "\r\nend";
                }
            }
            return SqlInsert;
        }

        private bool LogTableContainsColumnLogVersion(string Table)
        {
            bool ContainsColumnLogVersion = false;
            string SQL = "SELECT count(*) FROM Information_Schema.COLUMNS L WHERE L.TABLE_NAME = '" + Table + "_log' AND L.COLUMN_NAME = 'LogVersion'";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Result == "1")
                ContainsColumnLogVersion = true;
            return ContainsColumnLogVersion;
        }

        private void dropTrigger(string Trigger)
        {
            this.ExecuteSQL(this.SqlDropTrigger(Trigger));
        }

        private string SqlDropTrigger(string Trigger)
        {
            string SQL = "";
            if (this.ServerVersion == "2000")
            {
                SQL = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + Trigger + "]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)" +
                "drop trigger [dbo].[" + Trigger + "]";
            }
            else
            {
                SQL = "IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[" + Trigger + "]')) " +
                "DROP TRIGGER [dbo].[" + Trigger + "]";
            }
            return SQL;
        }

        #endregion

        #endregion

        #region procSetVersion

        public void HideProcSetVersion()
        {
            this.tabControlLoggingDefinitions.TabPages.Remove(this.tabPageProcSetVersion);
        }

        private void buttonProcSetVersionShowSql_Click(object sender, EventArgs e)
        {
            if (this.textBoxProcSetVersion.Text.Length == 0)
            {
                this.textBoxProcSetVersion.Text = this.OriginalSQL("procSetVersion");
            }
            if (this.comboBoxLoggingVersionMasterTable.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a master table for the version");
                return;
            }
            if (this.dataGridViewTable.DataSource != null)
            {
                string PK = this.PkColumn(this.comboBoxLoggingVersionMasterTable.Text);
                string SQL = "CREATE PROCEDURE [dbo].[procSetVersion" + this.comboBoxLoggingVersionMasterTable.Text + "]  (@ID int) \r\n" +
                "AS \r\n" +
                "/*  Setting the version of a dataset.  */ \r\n" +
                "/*  Created by DiversityWorkbench Administration.  */ \r\n" +
                "/*  " + System.Windows.Forms.Application.ProductName.ToString() + " " +
                " " + System.Windows.Forms.Application.ProductVersion.ToString() + " */ \r\n" +
                "/*  Date: " + System.DateTime.Now.ToShortDateString() + "  */ \r\n\r\n" +

                "DECLARE @NextVersion int \r\n" +
                "DECLARE @CurrentVersion int \r\n" +
                "DECLARE @LastUser nvarchar(500) \r\n" +
                "DECLARE @LastUpdate datetime \r\n" +
                "DECLARE @UpdatePeriod int \r\n\r\n" +

                "set @LastUpdate = (SELECT LogUpdatedWhen FROM " + this.comboBoxLoggingVersionMasterTable.Text + " WHERE " + PK + " = @ID) \r\n" +
                "set @UpdatePeriod = (SELECT DateDiff(hour, @LastUpdate, getdate())) \r\n";
                if (this.checkBoxProcSetVersionDsgvo.Checked)
                    SQL += "set @LastUser = (SELECT U.LoginName FROM " + this.comboBoxLoggingVersionMasterTable.Text + " T, UserProxy U WHERE U.ID =  T.LogUpdatedBy AND " + PK + " = @ID) \r\n\r\n";
                else
                    SQL += "set @LastUser = (SELECT LogUpdatedBy FROM " + this.comboBoxLoggingVersionMasterTable.Text + " WHERE " + PK + " = @ID) \r\n\r\n";
                SQL += "set @CurrentVersion = (select Version from " + this.comboBoxLoggingVersionMasterTable.Text + " where " + PK + " = @ID) \r\n" +
                "if @CurrentVersion is null begin set @CurrentVersion = 0 end \r\n" +
                "set @NextVersion = @CurrentVersion \r\n" +

                "if not @ID is null and (@LastUser <> sUser_sname() or @UpdatePeriod > 24) \r\n" +
                "begin  \r\n" +
                "    set @NextVersion = @CurrentVersion + 1 \r\n" +
                "    update " + this.comboBoxLoggingVersionMasterTable.Text + " set Version = @NextVersion \r\n" +
                "    where " + PK + " = @ID \r\n" +
                "select @NextVersion  \r\n" +
                "END";
                this.textBoxProcSetVersionNew.Text = SQL;
            }
        }

        private void buttonProcSetVersionCreate_Click(object sender, EventArgs e)
        {
            string SQL = "";
            try
            {
                // dropping old procedure
                if (this.ServerVersion == "2000")
                {
                    SQL = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[procSetVersion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) " +
                        "drop procedure [dbo].[procSetVersion]";
                }
                else
                {
                    SQL = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[procSetVersion]') AND type in (N'P', N'PC')) " +
                        "DROP PROCEDURE [dbo].[procSetVersion]";
                }
                this.ExecuteSQL(SQL);
                // creating new procedure
                SQL = this.textBoxProcSetVersionNew.Text;
                if (this.ExecuteSQL(SQL))
                    System.Windows.Forms.MessageBox.Show("Procedure created");
                else
                    System.Windows.Forms.MessageBox.Show("SQL contains syntax error\r\n\r\n" + SQL);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
        }

        #endregion

        #region LogColumns

        private void buttonAttachLogColumns_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
                string Table = rv[0].ToString();
                string SQL = "SET QUOTED_IDENTIFIER ON " +
                    "SET ARITHABORT ON " +
                    "SET NUMERIC_ROUNDABORT OFF " +
                    "SET CONCAT_NULL_YIELDS_NULL ON " +
                    "SET ANSI_NULLS ON " +
                    "SET ANSI_PADDING ON " +
                    "SET ANSI_WARNINGS ON ";
                this.ExecuteSQL(SQL);
                SQL = "ALTER TABLE " + Table + " ADD " +
                    "    LogUpdatedBy nvarchar(50) NULL, " +
                    "    LogUpdatedWhen smalldatetime NULL ";
                if (this.checkBoxAddInsertColumns.Checked)
                {
                    SQL = "ALTER TABLE " + Table + " ADD " +
                        "    LogInsertedBy nvarchar(50) NULL, " +
                        "    LogInsertedWhen smalldatetime NULL, " +
                        "    LogUpdatedBy nvarchar(50) NULL, " +
                        "    LogUpdatedWhen smalldatetime NULL ";
                }
                if (this.checkBoxAddRowGUID.Checked)
                {
                    SQL += " , [RowGUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_" + Table + "_RowGUID]  DEFAULT (newid()) ";
                }
                this.ExecuteSQL(SQL);
                if (this.checkBoxAddInsertColumns.Checked)
                {
                    string Name = "Name";
                    if (this.checkBoxLogcolumnsDSGVO.Checked)
                        Name = "ID";
                    if (this.ServerVersion == "2000")
                    {
                        SQL = "exec sp_addextendedproperty N'MS_Description', N'" + Name + " of user who first entered (typed or imported) the data.', N'user', N'dbo', N'table', N'" + Table + "', N'column', N'LogInsertedBy'";
                    }
                    else
                    {
                        SQL = "DECLARE @v sql_variant  " +
                            "SET @v = N'" + Name + " of user who first entered (typed or imported) the data.' " +
                            "EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Table + "', N'COLUMN', N'LogInsertedBy' ";
                    }

                    this.ExecuteSQL(SQL);
                    if (this.ServerVersion == "2000")
                    {
                        SQL = "exec sp_addextendedproperty N'MS_Description', N'Date and time when the data were first entered (typed or imported) into this database.', N'user', N'dbo', N'table', N'" + Table + "', N'column', N'LogInsertedWhen'";
                    }
                    else
                    {
                        SQL = "DECLARE @v sql_variant  " +
                            "SET @v = N'Date and time when the data were first entered (typed or imported) into this database.' " +
                            "EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Table + "', N'COLUMN', N'LogInsertedWhen' ";
                    }
                    this.ExecuteSQL(SQL);
                }
                if (this.ServerVersion == "2000")
                {
                    SQL = "exec sp_addextendedproperty N'MS_Description', N'" + Name + " of user who last updated the data.', N'user', N'dbo', N'table', N'" + Table + "', N'column', N'LogUpdatedBy'";
                }
                else
                {
                    SQL = "DECLARE @v sql_variant  " +
                        "SET @v = N'" + Name + " of user who last updated the data.' " +
                        "EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Table + "', N'COLUMN', N'LogUpdatedBy' ";
                }
                this.ExecuteSQL(SQL);
                if (this.ServerVersion == "2000")
                {
                    SQL = "exec sp_addextendedproperty N'MS_Description', N'Date and time when the data were last updated.', N'user', N'dbo', N'table', N'" + Table + "', N'column', N'LogUpdatedWhen'";
                }
                else
                {
                    SQL = "DECLARE @v sql_variant  " +
                        "SET @v = N'Date and time when the data were last updated.' " +
                        "EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Table + "', N'COLUMN', N'LogUpdatedWhen' ";
                }
                this.ExecuteSQL(SQL);
                if (this.checkBoxAddInsertColumns.Checked)
                {
                    SQL = "ALTER TABLE " + Table + " ADD CONSTRAINT " +
                        "    DF_" + Table + "_LogInsertedBy DEFAULT (";
                    if (this.checkBoxLogcolumnsDSGVO.Checked)
                        SQL += "cast(dbo.UserID() as varchar)";
                    else
                        SQL += "user_name()";
                    SQL += ") FOR LogInsertedBy ";
                    this.ExecuteSQL(SQL);
                    SQL = "ALTER TABLE " + Table + " ADD CONSTRAINT " +
                        "    DF_" + Table + "_LogInsertedWhen DEFAULT (getdate()) FOR LogInsertedWhen ";
                    this.ExecuteSQL(SQL);
                }
                SQL = "ALTER TABLE " + Table + " ADD CONSTRAINT " +
                    "    DF_" + Table + "_LogUpdatedBy DEFAULT (";
                if (this.checkBoxLogcolumnsDSGVO.Checked)
                    SQL += "cast(dbo.UserID() as varchar)";
                else
                    SQL += "user_name()";
                SQL += ") FOR LogUpdatedBy ";
                this.ExecuteSQL(SQL);
                SQL = "ALTER TABLE " + Table + " ADD CONSTRAINT " +
                    "    DF_" + Table + "_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen ";
                this.ExecuteSQL(SQL);
                System.Windows.Forms.MessageBox.Show("Log columns attached created");
                this.listBoxLoggingTables_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private string SqlAttachLogColumns(string Table)
        //{
        //    string SqlGo = "\r\nGO\r\n";
        //    string SQL = "SET QUOTED_IDENTIFIER ON " +
        //        "SET ARITHABORT ON " +
        //        "SET NUMERIC_ROUNDABORT OFF " +
        //        "SET CONCAT_NULL_YIELDS_NULL ON " +
        //        "SET ANSI_NULLS ON " +
        //        "SET ANSI_PADDING ON " +
        //        "SET ANSI_WARNINGS ON " +
        //        "\r\nGO\r\n" +
        //        "ALTER TABLE " + Table + " ADD " +
        //        "    LogCreatedBy nvarchar(50) NULL, " +
        //        "    LogCreatedWhen smalldatetime NULL, " +
        //        "    LogUpdatedBy nvarchar(50) NULL, " +
        //        "    LogUpdatedWhen smalldatetime NULL " +
        //        SqlGo;
        //    if (this.checkBoxAddInsertColumns.Checked)
        //    {
        //        if (this.ServerVersion == "2000")
        //        {
        //            SQL = "exec sp_addextendedproperty N'MS_Description', N'Name of user who first entered (typed or imported) the data.', N'user', N'dbo', N'table', N'" + Table + "', N'column', N'LogInsertedBy'";
        //        }
        //        else
        //        {
        //            SQL = "DECLARE @v sql_variant  " +
        //                "SET @v = N'Name of user who first entered (typed or imported) the data.' " +
        //                "EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Table + "', N'COLUMN', N'LogInsertedBy' ";
        //        }

        //        SQL += SqlGo;
        //        if (this.ServerVersion == "2000")
        //        {
        //            SQL = "exec sp_addextendedproperty N'MS_Description', N'Date and time when the data were first entered (typed or imported) into this database.', N'user', N'dbo', N'table', N'" + Table + "', N'column', N'LogInsertedWhen'";
        //        }
        //        else
        //        {
        //            SQL = "DECLARE @v sql_variant  " +
        //                "SET @v = N'Date and time when the data were first entered (typed or imported) into this database.' " +
        //                "EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Table + "', N'COLUMN', N'LogInsertedWhen' ";
        //        }
        //        SQL += SqlGo;
        //    }
        //    if (this.ServerVersion == "2000")
        //    {
        //        SQL = "exec sp_addextendedproperty N'MS_Description', N'Name of user who last updated the data.', N'user', N'dbo', N'table', N'" + Table + "', N'column', N'LogUpdatedBy'";
        //    }
        //    else
        //    {
        //        SQL = "DECLARE @v sql_variant  " +
        //            "SET @v = N'Name of user who last updated the data.' " +
        //            "EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Table + "', N'COLUMN', N'LogUpdatedBy' ";
        //    }
        //    SQL += SqlGo;
        //    if (this.ServerVersion == "2000")
        //    {
        //        SQL = "exec sp_addextendedproperty N'MS_Description', N'Date and time when the data were last updated.', N'user', N'dbo', N'table', N'" + Table + "', N'column', N'LogUpdatedWhen'";
        //    }
        //    else
        //    {
        //        SQL = "DECLARE @v sql_variant  " +
        //            "SET @v = N'Date and time when the data were last updated.' " +
        //            "EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Table + "', N'COLUMN', N'LogUpdatedWhen' ";
        //    }
        //    SQL += SqlGo;
        //    SQL = "ALTER TABLE " + Table + " ADD CONSTRAINT " +
        //        "    DF_" + Table + "_LogCreatedBy DEFAULT (user_name()) FOR LogCreatedBy ";
        //    SQL += SqlGo;
        //    SQL = "ALTER TABLE " + Table + " ADD CONSTRAINT " +
        //        "    DF_" + Table + "_LogCreatedWhen DEFAULT (getdate()) FOR LogCreatedWhen ";
        //    SQL += SqlGo;
        //    SQL = "ALTER TABLE " + Table + " ADD CONSTRAINT " +
        //        "    DF_" + Table + "_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy ";
        //    SQL += SqlGo;
        //    SQL = "ALTER TABLE " + Table + " ADD CONSTRAINT " +
        //        "    DF_" + Table + "_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen ";
        //    SQL += SqlGo;
        //    return SQL;
        //}

        #endregion

        #region Table list

        private void buttonListTablesForTrigger_Click(object sender, EventArgs e)
        {
            this.fillTableListTrigger();
        }

        private void fillTableListTrigger()
        {
            System.Data.DataTable dt = this.TableList();
            if (dt.Columns.Count > 0)
            {
                this.listBoxLoggingTables.DataSource = dt;
                this.listBoxLoggingTables.DisplayMember = "TableName";
                this.listBoxLoggingTables.ValueMember = "TableName";
            }
            else
            {
                this.listBoxLoggingTables.DataSource = null;
            }
        }

        #endregion

        #region Auxilliary functions

        private string ColumnList(string Table, bool IncludeDsgvo = false)
        {
            System.Data.DataTable dt = this.dtTableColumns(Table);
            string strColumns = "";
            foreach (System.Data.DataRow r in dt.Rows)
            {
                if (r["Datatype"].ToString() != "timestamp")
                    strColumns += r["ColumnName"].ToString() + ", ";
            }
            if (Replication.AttachReplicationColumns)
            {
                foreach (System.Data.DataRow R in Replication.DtReplicationColumns().Rows)
                {
                    System.Data.DataRow[] rr = dt.Select("ColumnName = '" + R["ColumnName"].ToString() + "'");
                    if (rr.Length == 0)
                    {
                        strColumns += R["ColumnName"].ToString() + ", ";
                    }
                }
            }
            if (IncludeDsgvo)
                strColumns += "LogUser, ";
            return strColumns;
        }

        private string ColumnList(string Table, string Prefix, bool IncludeDsgvo = false)
        {
            System.Data.DataTable dt = this.dtTableColumns(Table);
            string strColumns = "";
            foreach (System.Data.DataRow r in dt.Rows)
            {
                if (r["Datatype"].ToString() != "timestamp")
                {
                    if (strColumns.Length > 0)
                        strColumns += ", ";
                    if (Prefix.Length > 0)
                        strColumns += Prefix + ".";
                    strColumns += r["ColumnName"].ToString();
                }
            }
            if (Replication.AttachReplicationColumns)
            {
                foreach (System.Data.DataRow R in Replication.DtReplicationColumns().Rows)
                {
                    System.Data.DataRow[] rr = dt.Select("ColumnName = '" + R["ColumnName"].ToString() + "'");
                    if (rr.Length == 0)
                    {
                        if (strColumns.Length > 0)
                            strColumns += ", ";
                        if (Prefix.Length > 0)
                            strColumns += Prefix + ".";
                        strColumns += R["ColumnName"].ToString();
                    }
                }
            }
            if (IncludeDsgvo)
            {
                //strColumns += ", cast(U.ID as varchar) ";
                strColumns += ", cast(dbo.UserID() as varchar) ";
            }
            return strColumns;
        }

        private System.Collections.Generic.List<string> TableColumns(string Table)
        {
            System.Collections.Generic.List<string> CC = new List<string>();
            foreach (System.Data.DataRow R in this.dtTable(Table).Rows)
                CC.Add(R[0].ToString());
            return CC;
        }

        private string SqlVersion(bool ForInsert)
        {
            string SQL = "";
            if (this.comboBoxLoggingVersionMasterTable.Text.Length > 0)
            {
                System.Data.DataTable dt = this.dtPrimaryKey(this.comboBoxLoggingVersionMasterTable.Text);
                if (dt.Rows.Count == 1)
                {

                    System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxLoggingTables.SelectedItem;
                    string Table = rv[0].ToString();
                    SQL = this.SqlVersion(ForInsert, Table, this.comboBoxLoggingVersionMasterTable.Text);
                }
            }
            return SQL;
        }

        private string SqlVersion(bool ForInsert, string Table, string VersionTable)
        {
            string SQL = "";
            System.Data.DataTable dt = this.dtPrimaryKey(VersionTable);
            if (dt.Rows.Count == 1)
            {

                System.Data.DataTable dtColumns = this.dtTable(Table);
                string PkColumn = dt.Rows[0]["ColumnName"].ToString();
                bool OK = false;
                foreach (System.Data.DataRow r in dtColumns.Rows)
                {
                    if (r["ColumnName"].ToString() == PkColumn)
                    {
                        OK = true;
                        break;
                    }
                }
                if (OK)
                {
                    SQL = "\r\n/* setting the version in the main table */ \r\n" +
                        "DECLARE @i int \r\n" +
                        "DECLARE @ID int\r\n" +
                        "DECLARE @Version int\r\n\r\n" +
                        "set @i = (select count(*) from ";
                    if (ForInsert) SQL += "inserted";
                    else SQL += "deleted";
                    SQL += ") \r\n\r\n" +
                         "if @i = 1 \r\n" +
                         "BEGIN \r\n" +
                         "   SET  @ID = (SELECT " + PkColumn + " FROM ";
                    if (ForInsert) SQL += "inserted";
                    else SQL += "deleted";
                    SQL += ")\r\n   EXECUTE procSetVersion" + VersionTable + " @ID";
                    if (!ForInsert && Table != VersionTable) SQL += "\r\n" +
                         "   SET @Version = (SELECT Version FROM " + VersionTable + " WHERE " + PkColumn + " = @ID)";
                    SQL += "\r\nEND \r\n";
                }
            }
            return SQL;
        }

        private string OriginalSQL(string ItemName)
        {
            string SqlItem = "";
            if (ItemName.StartsWith("[")) ItemName = ItemName.Substring(1);
            if (ItemName.EndsWith("]")) ItemName = ItemName.Substring(0, ItemName.Length - 1);
            string SQL = "sp_helptext [" + ItemName + "]";
            string SqlTest = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + ItemName + "]') AND type in (N'U', N'TR')) SELECT 1 ELSE SELECT 0";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlTest, con);
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
            try
            {
                con.Open();
                var testF = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (testF == "1")
                    a.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State.ToString() == "Open")
                    con.Close();
            }
            foreach (System.Data.DataRow r in dt.Rows)
            {
                SqlItem += r[0].ToString();
            }
            SqlItem = SqlItem.Replace("\n", "\r\n");
            SqlItem = SqlItem.Replace("\r", "\r\n");
            SqlItem = SqlItem.Replace("\r\n\r", "\r");
            SqlItem = SqlItem.Replace("\n\n", "\n");
            return SqlItem;
        }

        private System.Data.DataTable dtPrimaryKey(string Table)
        {
            if (this._dtCurrentPK == null) this._dtCurrentPK = new DataTable();
            if (this._dtCurrentPK.Rows.Count > 0 && this._CurrentPK == Table)
                return this._dtCurrentPK;
            else
            {
                this._dtCurrentPK.Clear();
                this._CurrentPK = Table;
            }
            //System.Data.DataTable dt = new DataTable();
            string SQL = "";
            if (this.ServerVersion == "2000")
            {
                SQL = "SELECT COLUMN_NAME AS ColumnName  FROM Information_Schema.KEY_COLUMN_USAGE " +
                    "WHERE Information_Schema.KEY_COLUMN_USAGE.TABLE_NAME = '" + Table + "' " +
                    "AND CONSTRAINT_NAME LIKE 'PK_%'";
            }
            else
            {
                SQL = "select c.Column_Name AS ColumnName from INFORMATION_SCHEMA.KEY_COLUMN_USAGE c " +
                ", INFORMATION_SCHEMA.TABLE_CONSTRAINTS t " +
                "where c.Table_name = '" + Table + "' " +
                "and c.Table_name = t.Table_name " +
                "and c.Constraint_name = t.Constraint_name " +
                "and T.Constraint_type = 'primary key'";
            }
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            a.Fill(this._dtCurrentPK);
            return this._dtCurrentPK;
        }

        private System.Collections.Generic.List<string> PKColumns(string Table)
        {
            System.Collections.Generic.List<string> PK = new List<string>();
            foreach (System.Data.DataRow R in this.dtPrimaryKey(Table).Rows)
                PK.Add(R[0].ToString());
            return PK;
        }

        private string PkColumn(string Table)
        {
            try
            {
                System.Data.DataTable dt = this.dtPrimaryKey(Table);
                string PK = dt.Rows[0][0].ToString();
                return PK;
            }
            catch (System.Exception ex) { }
            return "";
        }

        private bool ColumnInTable(string ColumnName, string TableName)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "";
            if (this.ServerVersion == "2000")
            {
                SQL = "SELECT COLUMN_NAME AS ColumnName  FROM Information_Schema.KEY_COLUMN_USAGE " +
                    "WHERE Information_Schema.KEY_COLUMN_USAGE.TABLE_NAME = '" + TableName + "'";
            }
            else
            {
                SQL = "select sys.columns.name AS ColumnName " +
                "from sys.columns, sys.tables " +
                "where sys.tables.object_id =  sys.columns.object_id " +
                "AND sys.tables.name = '" + TableName + "'";
            }
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            a.Fill(dt);
            bool OK = false;
            foreach (System.Data.DataRow R in dt.Rows)
            {
                if (R[0].ToString() == ColumnName)
                {
                    OK = true;
                    break;
                }
            }
            return OK;
        }

        private bool ExecuteSQL(string SQL)
        {
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            bool OK = false;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                OK = true;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State.ToString() == "Open")
                    con.Close();
            }
            return OK;
        }

        #endregion

        #endregion

        #region Run Procedure

        private void setProcedureList(string ConnectionString = "")
        {
            string SQL = "SELECT ROUTINE_NAME AS Name, ROUTINE_TYPE AS ObjectType, DATA_TYPE AS DataType, ROUTINE_DEFINITION AS Definition " +
                "FROM INFORMATION_SCHEMA.ROUTINES " +
                "WHERE ROUTINE_NAME NOT LIKE 'dt_%' " +
                "AND ROUTINE_NAME NOT LIKE 'sp_%' " +
                "ORDER BY ROUTINE_NAME";
            if (ConnectionString.Length == 0)
                ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
            System.Data.DataTable dt = new DataTable();
            ad.Fill(dt);
            this.comboBoxProcedureList.DataSource = dt;
            this.comboBoxProcedureList.DisplayMember = "Name";
            this.comboBoxProcedureList.ValueMember = "Name";
        }

        private void buttonStartDataTransfer_Click(object sender, EventArgs e)
        {
            if (this.comboBoxProcedureList.Text.Length > 0)
            {
                string Error = "";
                string SQL = "";
                if (this.textBoxProcedureType.Text == "PROCEDURE")
                    SQL = "EXECUTE dbo." + this.comboBoxProcedureList.Text;
                else
                {
                    if (this.textBoxProcedureReturns.Text == "TABLE")
                        SQL = "SELECT * FROM dbo." + this.comboBoxProcedureList.Text;
                    else
                        SQL = "SELECT dbo." + this.comboBoxProcedureList.Text;
                }
                if (this.textBoxProcedureParameter1.Visible && this.textBoxProcedureType.Text != "PROCEDURE")
                    SQL += "(";
                foreach (System.Windows.Forms.TextBox T in this.ProcedureParameterTextBoxes)
                {
                    this.addParameter(ref SQL, T);
                }
                if (this.textBoxProcedureParameter1.Visible && this.textBoxProcedureType.Text != "PROCEDURE")
                    SQL += ")";
                this.textBoxProcedureSQL.Text = SQL;
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = (int)this.numericUpDownTimeout.Value;
                System.DateTime Start = System.DateTime.Now;
                this.textBoxTimeElapsed.Text = "";
                if (this.textBoxProcedureReturns.Text == "TABLE")
                {
                    try
                    {
                        con.Open();
                        System.Data.DataTable dt = new DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(C);
                        ad.Fill(dt);
                        this.dataGridViewProcedureResult.DataSource = dt;
                    }
                    catch (Microsoft.Data.SqlClient.SqlException ex)
                    {
                        Error = ex.Message;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                else
                {
                    try
                    {
                        con.Open();
                        this.textBoxProcedureResult.Text = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    }
                    catch (Microsoft.Data.SqlClient.SqlException ex)
                    {
                        Error = ex.Message;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                System.DateTime End = System.DateTime.Now;
                int Diff = End.Second - Start.Second;
                Diff += 60 * (End.Minute - Start.Minute);
                this.textBoxTimeElapsed.Text = Diff.ToString();
                if (Error.Length > 0)
                    System.Windows.Forms.MessageBox.Show(Error);
            }
            else
                System.Windows.Forms.MessageBox.Show("Please select a procedure from the list");
        }

        private void addParameter(ref string SQL, System.Windows.Forms.TextBox T)
        {
            if (T.Visible)
            {
                if (T.Text.Length == 0 && T.ReadOnly)
                {
                    if (this.textBoxProcedureType.Text == "PROCEDURE")
                    {
                        if (!SQL.EndsWith("(")) SQL += ", ";
                        SQL += "NULL";
                    }
                }
                else
                {
                    if (!SQL.EndsWith("(")) SQL += ", ";
                    SQL += "'" + T.Text + "'";
                }
            }
        }

        private void comboBoxProcedureList_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxProcedureList.SelectedItem;
            this.textBoxProcedureDescription.Text = this.getDescription(R["Name"].ToString(), R["ObjectType"].ToString(), "");
            this.textBoxProcedureDefinition.Text = R["Definition"].ToString();
            this.textBoxProcedureReturns.Text = R["DataType"].ToString();
            this.textBoxProcedureType.Text = R["ObjectType"].ToString();
            this.setParameterList(R["Name"].ToString());
            this.textBoxProcedureSQL.Text = "";
            this.textBoxProcedureResult.Text = "";
            this.textBoxTimeElapsed.Text = "";
            this.dataGridViewProcedureResult.DataSource = null;
            if (R["DataType"].Equals(System.DBNull.Value)) this.tableLayoutPanelProcedureResult.Visible = false;
            else
            {
                this.tableLayoutPanelProcedureResult.Visible = true;
                bool ShowTable = false;
                if (R["DataType"].ToString() == "TABLE") ShowTable = true;
                this.splitContainerProcedureResult.Panel1Collapsed = !ShowTable;
                this.splitContainerProcedureResult.Panel2Collapsed = ShowTable;
            }
        }

        private void setParameterList(string Procedure)
        {
            foreach (System.Windows.Forms.TextBox T in this.ProcedureParameterTextBoxes)
                T.Text = "";
            string SQL = "SELECT sys.all_objects.name AS ProcedureName,  " +
                "rtrim(substring(sys.all_parameters.name, 2, 500)) AS ParameterName,  " +
                "sys.types.name AS DataType, sys.all_objects.type AS ProcedureType, sys.all_parameters.is_output " +
                "FROM sys.types INNER JOIN sys.all_objects  " +
                "INNER JOIN sys.all_parameters ON sys.all_objects.object_id = sys.all_parameters.object_id  " +
                "ON  sys.types.user_type_id = sys.all_parameters.user_type_id  " +
                "WHERE (sys.all_objects.is_ms_shipped = 0)  " +
                "AND (sys.all_objects.name NOT LIKE 'dt_%')   " +
                "AND (sys.all_objects.name NOT LIKE 'sp_%')   " +
                "AND sys.all_objects.name = '" + Procedure + "'";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            System.Data.DataTable dt = new DataTable();
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                this.tableLayoutPanelProcedureParameter.Visible = true;
                bool OK = true;
                this.labelProcedureParameter1.Text = dt.Rows[0]["ParameterName"].ToString() + " (" + dt.Rows[0]["DataType"].ToString() + ")";
                this.labelProcedureParameter1.Visible = OK;
                this.textBoxProcedureParameter1.Visible = OK;
                if (dt.Rows[0]["is_output"].ToString() == "True") this.textBoxProcedureParameter1.ReadOnly = true;
                else this.textBoxProcedureParameter1.ReadOnly = false;

                if (dt.Rows.Count < 2) OK = false;
                else this.labelProcedureParameter2.Text = dt.Rows[1]["ParameterName"].ToString() + " (" + dt.Rows[1]["DataType"].ToString() + ")";
                this.labelProcedureParameter2.Visible = OK;
                this.textBoxProcedureParameter2.Visible = OK;
                if (dt.Rows.Count > 1 && dt.Rows[1]["is_output"].ToString() == "True") this.textBoxProcedureParameter2.ReadOnly = true;
                else this.textBoxProcedureParameter2.ReadOnly = false;

                if (dt.Rows.Count < 3) OK = false;
                else this.labelProcedureParameter3.Text = dt.Rows[2]["ParameterName"].ToString() + " (" + dt.Rows[2]["DataType"].ToString() + ")";
                this.labelProcedureParameter3.Visible = OK;
                this.textBoxProcedureParameter3.Visible = OK;
                if (dt.Rows.Count > 2 && dt.Rows[2]["is_output"].ToString() == "True") this.textBoxProcedureParameter3.ReadOnly = true;
                else this.textBoxProcedureParameter3.ReadOnly = false;

                if (dt.Rows.Count < 4) OK = false;
                else this.labelProcedureParameter4.Text = dt.Rows[3]["ParameterName"].ToString() + " (" + dt.Rows[3]["DataType"].ToString() + ")";
                this.labelProcedureParameter4.Visible = OK;
                this.textBoxProcedureParameter4.Visible = OK;
                if (dt.Rows.Count > 3 && dt.Rows[3]["is_output"].ToString() == "True") this.textBoxProcedureParameter4.ReadOnly = true;
                else this.textBoxProcedureParameter4.ReadOnly = false;

                if (dt.Rows.Count < 5) OK = false;
                else this.labelProcedureParameter5.Text = dt.Rows[4]["ParameterName"].ToString() + " (" + dt.Rows[4]["DataType"].ToString() + ")";
                this.labelProcedureParameter5.Visible = OK;
                this.textBoxProcedureParameter5.Visible = OK;
                if (dt.Rows.Count > 4 && dt.Rows[4]["is_output"].ToString() == "True") this.textBoxProcedureParameter5.ReadOnly = true;
                else this.textBoxProcedureParameter5.ReadOnly = false;

                if (dt.Rows.Count < 6) OK = false;
                else this.labelProcedureParameter6.Text = dt.Rows[5]["ParameterName"].ToString() + " (" + dt.Rows[5]["DataType"].ToString() + ")";
                this.labelProcedureParameter6.Visible = OK;
                this.textBoxProcedureParameter6.Visible = OK;
                if (dt.Rows.Count > 5 && dt.Rows[5]["is_output"].ToString() == "True") this.textBoxProcedureParameter6.ReadOnly = true;
                else this.textBoxProcedureParameter6.ReadOnly = false;
            }
            else
                this.tableLayoutPanelProcedureParameter.Visible = false;
        }

        #endregion

        #region Description

        private void fillDescriptionTree(string ConnectionString = "")
        {
            this.treeViewDescription.Nodes.Clear();
            string SQL = "SELECT ROUTINE_NAME AS ObjectName, ROUTINE_TYPE AS ObjectType, DATA_TYPE AS DataType, " +
                "ROUTINE_DEFINITION AS Definition, SQL_DATA_ACCESS AS Action, ROUTINE_SCHEMA AS SchemaName FROM INFORMATION_SCHEMA.ROUTINES  " +
                "WHERE ROUTINE_NAME NOT LIKE 'dt_%'  " +
                "AND ROUTINE_NAME NOT LIKE 'sp_%' " +
                "UNION " +
                "SELECT TABLE_NAME, 'TABLE', '', '', '', TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES " +
                "WHERE TABLE_TYPE = 'BASE TABLE' " +
                "UNION " +
                "SELECT TABLE_NAME, 'VIEW', '', VIEW_DEFINITION, '', TABLE_SCHEMA FROM INFORMATION_SCHEMA.VIEWS  " +
                "UNION " +
                "SELECT name, 'ROLE', '', '', '', 'dbo' FROM sysusers WHERE (issqlrole = 1) AND (name <> N'public') AND (name NOT LIKE 'db_%')  " +
                "ORDER BY ObjectType, ObjectName";
            if (ConnectionString.Length == 0)
                ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
            System.Data.DataTable dt = new DataTable();
            ad.Fill(dt);
            System.Collections.Generic.List<string> Schemas = new List<string>();
            Schemas.Add("dbo");
            if (this._SplitBySchema)
            {
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Schema = R["SchemaName"].ToString();
                    if (!Schemas.Contains(Schema))
                    {
                        Schemas.Add(Schema);
                    }
                }
            }
            string ObjectType = "";
            foreach (System.Data.DataRow R in dt.Rows)
            {
                if (R["ObjectType"].ToString() != ObjectType)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R["ObjectType"].ToString());
                    ObjectType = R["ObjectType"].ToString();
                    this.treeViewDescription.Nodes.Add(N);
                    if (_SplitBySchema && Schemas.Count > 1)
                    {
                        foreach (string Schema in Schemas)
                        {
                            System.Windows.Forms.TreeNode NS = new TreeNode(Schema);
                            N.Nodes.Add(NS);
                            this.addChildNodes(NS, ObjectType, dt, Schema);
                        }
                    }
                    else
                    {
                        this.addChildNodes(N, ObjectType, dt);
                    }
                }
            }
            this.treeViewDescription.ExpandAll();
        }

        private void addChildNodes(System.Windows.Forms.TreeNode N, string ObjectType, System.Data.DataTable T, string Schema = "dbo")
        {
            foreach (System.Data.DataRow R in T.Rows)
            {
                if (R["ObjectType"].ToString() == ObjectType && R["SchemaName"].ToString() == Schema)
                {
                    System.Windows.Forms.TreeNode n = new TreeNode(R["ObjectName"].ToString());
                    n.Tag = R;
                    N.Nodes.Add(n);
                }
            }
        }

        private void setDescription(string DatabaseObject, string ObjectType, string Column, string Description)
        {
            string DescriptionOld = this.getDescription(DatabaseObject, ObjectType, Column);
            if (DescriptionOld != Description
                && Description.Length > 0
                && (DatabaseObject == this._LastSelectedColumnTable || ObjectType == "FUNCTION" || ObjectType == "PROCEDURE" || ObjectType == "TRIGGER" || ObjectType == "USER" || ObjectType == "ROLE" || (ObjectType == "VIEW" && this._LastSelectedColumnTable.Length == 0)))
            {
                string SQL = this.SqlDescription(DatabaseObject, ObjectType, Column, Description, DescriptionGenerationType.Both);// Prefix + "EXEC " + Function + Parameters;
                //if (DescriptionOld.Length > 0)
                //    SQL = this.SqlDescription(DatabaseObject, ObjectType, Column, Description, DescriptionGenerationType.Update);

                // Markus 10.1.2023 - adaption for e.g. CacheDB
                string Conn = this._ConnectionString;
                if (Conn.Length == 0) Conn = DiversityWorkbench.Settings.ConnectionString;
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(Conn);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    C.ExecuteNonQuery();
                }
                catch (Microsoft.Data.SqlClient.SqlException ex)
                {
                    //Function = "sys.sp_updateextendedproperty";
                    SQL = this.SqlDescription(DatabaseObject, ObjectType, Column, Description, DescriptionGenerationType.Update);// Prefix + "EXEC " + Function + Parameters;
                    C.CommandText = SQL;
                    try
                    {
                        C.ExecuteNonQuery();
                    }
                    catch (System.Exception ex2) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex2); }
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private enum DescriptionGenerationType { Add, Update, Both }
        private string SqlDescription(string DatabaseObject, string ObjectType, string Column, string Description, DescriptionGenerationType Type, bool AddExistenceCheck = false)
        {
            // Markus 16.3.2023: Removing char operation by simple replacment
            Description = "'" + Description.Replace("'", "''") + "'";// DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(Description);
            //if (Description.EndsWith(" + '")) Description = Description.Substring(0, Description.Length - 4);
            //if (Description.StartsWith("' + ")) Description = Description.Substring(4);
            //if (!Description.EndsWith("'") && !Description.EndsWith("CHAR(39)")) Description += "'";
            //if (!Description.StartsWith("'") && !Description.StartsWith("CHAR(39)")) Description = "'" + Description;
            //string Prefix = "declare @D nvarchar(500);\r\nset @D = N" + Description + ";\r\n";
            string Function = "sys.sp_addextendedproperty";
            string Parameters = " \r\n@name=N'MS_Description', @value=N" + Description + ",\r\n" +
                 "@level0type=N'SCHEMA', @level0name=N'dbo',\r\n" +
                 "@level1type=N'";
            if (ObjectType == "TRIGGER")
                Parameters += "TABLE";
            else
                Parameters += ObjectType;
            Parameters += "', @level1name=N'" + DatabaseObject + "'";
            if (Column.Length > 0)
            {
                Parameters += ",\r\n@level2type=N'";
                if (Column.StartsWith("@"))
                    Parameters += "PARAMETER";
                else if (ObjectType == "TRIGGER")
                    Parameters += ObjectType;
                else
                    Parameters += "COLUMN";
                Parameters += "', @level2name=N'" + Column + "'";
            }
            if (ObjectType == "USER" || ObjectType == "ROLE")
            {
                Parameters = " @name = N'MS_Description', @value = N" + Description + ",\r\n@level0type = N'USER', @level0name = '" + DatabaseObject + "';\r\n";
            }
            string SqlAdd = /*Prefix +*/ "EXEC " + Function + Parameters;
            Function = "sys.sp_updateextendedproperty";
            string SqlUpdate = /*Prefix +*/ "EXEC " + Function + Parameters;
            string SQL = "";
            switch (Type)
            {
                case DescriptionGenerationType.Add:
                    if (ObjectType == "TABLE")
                    {
                        SQL = "if (SELECT count(*) \r\n" +
                            "FROM sys.extended_properties AS ep \r\n" +
                            "INNER JOIN  sys.tables AS t ON ep.major_id = t.object_id  \r\n" +
                            "left outer JOIN  sys.columns AS c ON ep.major_id = c.object_id  AND ep.minor_id = c.column_id \r\n" +
                            "WHERE class = 1 AND ep.name = 'MS_Description' AND t.name = '" + DatabaseObject + "' AND";
                        if (Column.Length == 0)
                            SQL += " c.column_id IS NULL ";
                        else
                            SQL += " c.name = '" + Column + "' ";
                        SQL += ") = 0 \r\nbegin \r\n   " + SqlAdd +
                            " \r\nend";
                    }
                    else if (ObjectType == "VIEW")
                    {
                        SQL = "if (SELECT count(*) \r\n" +
                            "FROM sys.extended_properties AS ep \r\n" +
                            "INNER JOIN  sys.views AS t ON ep.major_id = t.object_id  \r\n" +
                            "left outer JOIN  sys.columns AS c ON ep.major_id = c.object_id  AND ep.minor_id = c.column_id \r\n" +
                            "WHERE class = 1 AND ep.name = 'MS_Description' AND t.name = '" + DatabaseObject + "' AND";
                        if (Column.Length == 0)
                            SQL += " c.column_id IS NULL ";
                        else
                            SQL += " c.name = '" + Column + "' ";
                        SQL += ") = 0 \r\nbegin \r\n   " + SqlAdd +
                            " \r\nend";
                    }
                    else
                    {
                        SQL = SqlAdd;
                    }
                    break;
                case DescriptionGenerationType.Update:
                    SQL = SqlUpdate;
                    break;
                case DescriptionGenerationType.Both:
                    SQL = "BEGIN TRY\r\n" + SqlAdd;
                    if (!SqlAdd.EndsWith("\r\n"))
                        SQL += "\r\n";
                    //SQL += "END TRY\r\nBEGIN CATCH\r\n" + SqlUpdate/*.Replace("@D ", "@U ").Replace("@D,", "@U,")*/ + " \r\nEND CATCH";
                    SQL += "END TRY\r\nBEGIN CATCH\r\n" + SqlUpdate;
                    if (!SqlUpdate.EndsWith("\r\n"))
                        SQL += "\r\n";
                    SQL += "END CATCH";
                    break;
            }
            if (!SQL.EndsWith(";"))
                SQL += ";";

            if (AddExistenceCheck)
            {
                string SqlCheck = "";
                switch (ObjectType.ToUpper())
                {
                    case "TABLE":
                        SqlCheck = "if (select count(*) from INFORMATION_SCHEMA.TABLES r where r.TABLE_NAME = '" + DatabaseObject + "') > 0";
                        if (Column.Length > 0)
                            SqlCheck += "\r\n and (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = '" + DatabaseObject + "' and c.COLUMN_NAME = '" + Column + "') > 0";
                        break;
                    case "VIEW":
                        SqlCheck = "if (select count(*) from INFORMATION_SCHEMA.VIEWS r where r.TABLE_NAME = '" + DatabaseObject + "') > 0";
                        if (Column.Length > 0)
                            SqlCheck += "\r\n and (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = '" + DatabaseObject + "' and c.COLUMN_NAME = '" + Column + "') > 0";
                        break;
                    case "ROUTINE":
                        break;
                    case "PROCEDURE":
                    case "FUNCTION":
                        SqlCheck = "if (select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = '" + DatabaseObject + "') > 0";
                        if (Column.Length > 0)
                        {
                            if (Column.IndexOf("@") == 0)
                                SqlCheck += "\r\n and (SELECT count(*) " +
                                    "\r\n\tFROM sys.types INNER JOIN sys.all_objects " +
                                    "\r\n\tINNER JOIN sys.all_parameters ON sys.all_objects.object_id = sys.all_parameters.object_id " +
                                    "\r\n\tON sys.types.user_type_id = sys.all_parameters.user_type_id " +
                                    "\r\n\tWHERE(sys.all_objects.is_ms_shipped = 0) " +
                                    "\r\n\tAND sys.all_objects.name = '" + DatabaseObject + "' " +
                                    "\r\n\tAND sys.all_parameters.is_output = 0 " +
                                    "\r\n\tAND sys.all_parameters.name = '" + Column + "') > 0 ";
                            else
                                SqlCheck += "\r\n and (select count(*) from INFORMATION_SCHEMA.ROUTINE_COLUMNS c where c.TABLE_NAME = '" + DatabaseObject + "' and c.COLUMN_NAME = '" + Column + "') > 0";
                        }
                        break;
                    case "COLUMN":
                        break;
                    case "PARAMETER":
                        break;
                    case "TRIGGER":
                        break;
                }
                if (SqlCheck.Length > 0)
                    SQL = SqlCheck + "\r\n" + SQL;
            }

            //SQL += "\r\nGO";
            return SQL;
        }

        private string getDescription(string DatabaseObject, string ObjectType, string Column)
        {
            string Description = "";
            string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', '";
            if (ObjectType == "TRIGGER")
                SQL += "TABLE";
            else
                SQL += ObjectType;
            SQL += "', '" + DatabaseObject + "', ";
            if (Column.Length > 0)
            {
                if (Column.StartsWith("@"))
                    SQL += "'PARAMETER', '" + Column + "'";
                else if (ObjectType == "TRIGGER")
                    SQL += "'" + ObjectType + "', '" + Column + "'";
                else
                    SQL += "'COLUMN', '" + Column + "'";
            }
            else
                SQL += " default, NULL";
            SQL += ") WHERE name =  'MS_Description'";
            string Conn = this._ConnectionString;
            if (Conn.Length == 0) Conn = DiversityWorkbench.Settings.ConnectionString;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(Conn);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                Description = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (Description == string.Empty)
                    return null;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
            return Description;
        }

        private string _LastSelectedColumnTable = "";

        private void treeViewDescription_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeViewDescription.SelectedNode.Tag == null)
            {
                this.textBoxObjectName.Text = "";
                this.textBoxObjectType.Text = "";
                this.textBoxObjectDatatype.Text = "";
                this.textBoxObjectDefinition.Text = "";
                this.textBoxObjectDescription.Text = "";
                this.textBoxObjectAction.Text = "";
                this.splitContainerDescription.Panel2.Enabled = false;
            }
            else
            {
                try
                {
                    this.splitContainerDescription.Panel2.Enabled = true;
                    System.Data.DataRow R = (System.Data.DataRow)this.treeViewDescription.SelectedNode.Tag;
                    if (R.Table.Columns.Contains("ObjectName"))
                    {
                        this.textBoxObjectName.Text = R["ObjectName"].ToString();
                        this.textBoxObjectType.Text = R["ObjectType"].ToString();
                        this.textBoxObjectDatatype.Text = R["DataType"].ToString();
                        this.textBoxObjectDefinition.Text = R["Definition"].ToString();
                        this.textBoxObjectAction.Text = R["Action"].ToString();
                    }

                    // Setting the description
                    if (R["ObjectType"].ToString() == "COLUMN")
                    {
                        this.textBoxObjectDescription.Text = this.getDescription(this.treeViewDescription.SelectedNode.Parent.Text, this.treeViewDescription.SelectedNode.Parent.Parent.Text, R["ObjectName"].ToString());
                        this._LastSelectedColumnTable = this.treeViewDescription.SelectedNode.Parent.Text;
                    }
                    else if (R["ObjectType"].ToString() == "PARAMETER")
                    {
                        this.textBoxObjectDescription.Text = this.getDescription(this.treeViewDescription.SelectedNode.Parent.Text, this.treeViewDescription.SelectedNode.Parent.Parent.Text, R["ObjectName"].ToString());
                    }
                    else if (R["ObjectType"].ToString() == "USER" || R["ObjectType"].ToString() == "ROLE")
                    {
                        this.textBoxObjectDescription.Text = DiversityWorkbench.Forms.FormFunctions.getDescription(FormFunctions.DatabaseObjectType.USER, R["ObjectName"].ToString(), "");// this.getDescription(R["ObjectName"].ToString(), R["ObjectType"].ToString(), "");
                    }
                    else
                        this.textBoxObjectDescription.Text = this.getDescription(R["ObjectName"].ToString(), R["ObjectType"].ToString(), "");

                    // Adding missing nodes
                    if (this.treeViewDescription.SelectedNode.Nodes.Count == 0)
                    {
                        if (R["ObjectType"].ToString() == "TABLE" || R["ObjectType"].ToString() == "VIEW")
                        {
                            string SQL = "SELECT C.COLUMN_NAME AS ObjectName, 'COLUMN' AS ObjectType, " +
                                "DATA_TYPE + CASE WHEN C.CHARACTER_MAXIMUM_LENGTH IS NULL THEN '' ELSE ' (' + CAST(C.CHARACTER_MAXIMUM_LENGTH as varchar) + ')' END AS DataType, " +
                                "'' AS Definition, '' AS Action " +
                                "FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + R["ObjectName"].ToString() + "'";
                            System.Data.DataTable dt = new DataTable();
                            string Conn = this._ConnectionString;
                            if (Conn.Length == 0) Conn = DiversityWorkbench.Settings.ConnectionString;
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, Conn);
                            ad.Fill(dt);
                            foreach (System.Data.DataRow RCol in dt.Rows)
                            {
                                System.Windows.Forms.TreeNode N = new TreeNode(RCol["ObjectName"].ToString());
                                N.Tag = RCol;
                                this.treeViewDescription.SelectedNode.Nodes.Add(N);
                            }
                            if (R["ObjectType"].ToString() == "TABLE")
                            {
                                SQL = "select R.name AS ObjectName, 'TRIGGER' AS ObjectType, '' AS DataType, '' AS Definition, '' AS Action, A.name AS TableName from sys.triggers R, sys.tables A " +
                                    "where R.parent_id = A.object_id " +
                                    "and A.name = '" + R[0].ToString() + "' " +
                                    "and R.type = 'TR'";
                                System.Data.DataTable dtTrigger = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                                if (dtTrigger.Rows.Count > 0)
                                {
                                    System.Windows.Forms.TreeNode Ntrigger = new TreeNode("TRIGGER");
                                    this.treeViewDescription.SelectedNode.Nodes.Add(Ntrigger);
                                    foreach (System.Data.DataRow rTrigger in dtTrigger.Rows)
                                    {
                                        System.Windows.Forms.TreeNode nTrigger = new TreeNode(rTrigger[0].ToString());
                                        nTrigger.Tag = rTrigger;
                                        Ntrigger.Nodes.Add(nTrigger);
                                    }
                                    Ntrigger.Expand();
                                }
                            }
                        }
                        else if (R["ObjectType"].ToString() == "FUNCTION")
                        {
                            if (R["DataType"].ToString() == "TABLE")
                            {
                                // Parameter
                                string SQL = "SELECT P.PARAMETER_NAME AS ObjectName, 'PARAMETER' AS ObjectType " +
                                ", P.DATA_TYPE  + CASE WHEN P.CHARACTER_MAXIMUM_LENGTH IS NULL THEN '' ELSE ' (' + CAST(P.CHARACTER_MAXIMUM_LENGTH as varchar) + ')' END AS DataType " +
                                ", '' AS Definition, '' AS Action " +
                                "FROM    INFORMATION_SCHEMA.PARAMETERS P " +
                                "WHERE P.SPECIFIC_NAME = '" + R["ObjectName"].ToString() + "' and P.PARAMETER_NAME <> ''";
                                System.Data.DataTable dtParameter = new DataTable();
                                string Conn = this._ConnectionString;
                                if (Conn.Length == 0) Conn = DiversityWorkbench.Settings.ConnectionString;
                                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, Conn);
                                ad.Fill(dtParameter);
                                foreach (System.Data.DataRow RCol in dtParameter.Rows)
                                {
                                    System.Windows.Forms.TreeNode N = new TreeNode(RCol["ObjectName"].ToString());
                                    N.Tag = RCol;
                                    this.treeViewDescription.SelectedNode.Nodes.Add(N);
                                }
                                // Columns
                                SQL = "SELECT C.COLUMN_NAME AS ObjectName, 'COLUMN' AS ObjectType, " +
                                    "DATA_TYPE + CASE WHEN C.CHARACTER_MAXIMUM_LENGTH IS NULL THEN '' ELSE ' (' + CAST(C.CHARACTER_MAXIMUM_LENGTH as varchar) + ')' END AS DataType, " +
                                    "'' AS Definition, '' AS Action " +
                                    "FROM INFORMATION_SCHEMA.ROUTINE_COLUMNS C WHERE C.TABLE_NAME = '" + R["ObjectName"].ToString() + "'";
                                System.Data.DataTable dtColumns = new DataTable();
                                ad.SelectCommand.CommandText = SQL; ;
                                ad.Fill(dtColumns);
                                foreach (System.Data.DataRow RCol in dtColumns.Rows)
                                {
                                    System.Windows.Forms.TreeNode N = new TreeNode(RCol["ObjectName"].ToString());
                                    N.Tag = RCol;
                                    this.treeViewDescription.SelectedNode.Nodes.Add(N);
                                }
                            }
                            else
                            {
                                string SQL = "SELECT P.PARAMETER_NAME AS ObjectName, 'PARAMETER' AS ObjectType " +
                                ", P.DATA_TYPE  + CASE WHEN P.CHARACTER_MAXIMUM_LENGTH IS NULL THEN '' ELSE ' (' + CAST(P.CHARACTER_MAXIMUM_LENGTH as varchar) + ')' END AS DataType " +
                                ", '' AS Definition, '' AS Action " +
                                "FROM    INFORMATION_SCHEMA.PARAMETERS P " +
                                "WHERE P.SPECIFIC_NAME = '" + R["ObjectName"].ToString() + "' and P.PARAMETER_NAME <> ''";
                                System.Data.DataTable dt = new DataTable();
                                // Markus 10.1.2023 - adaption for e.g. CacheDB
                                string Conn = this._ConnectionString;
                                if (Conn.Length == 0) Conn = DiversityWorkbench.Settings.ConnectionString;
                                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, Conn);
                                ad.Fill(dt);
                                foreach (System.Data.DataRow RCol in dt.Rows)
                                {
                                    System.Windows.Forms.TreeNode N = new TreeNode(RCol["ObjectName"].ToString());
                                    N.Tag = RCol;
                                    this.treeViewDescription.SelectedNode.Nodes.Add(N);
                                }
                            }
                        }
                        else if (R["ObjectType"].ToString() == "PROCEDURE")
                        {
                            string SQL = "SELECT P.PARAMETER_NAME AS ObjectName, 'PARAMETER' AS ObjectType " +
                            ", P.DATA_TYPE  + CASE WHEN P.CHARACTER_MAXIMUM_LENGTH IS NULL THEN '' ELSE ' (' + CAST(P.CHARACTER_MAXIMUM_LENGTH as varchar) + ')' END AS DataType " +
                            ", '' AS Definition, '' AS Action " +
                            "FROM    INFORMATION_SCHEMA.PARAMETERS P " +
                            "WHERE P.SPECIFIC_NAME = '" + R["ObjectName"].ToString() + "' and P.PARAMETER_NAME <> ''";
                            System.Data.DataTable dtParameter = new DataTable();
                            // Markus 10.1.2023 - adaption for e.g. CacheDB
                            string Conn = this._ConnectionString;
                            if (Conn.Length == 0) Conn = DiversityWorkbench.Settings.ConnectionString;
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, Conn);
                            ad.Fill(dtParameter);
                            foreach (System.Data.DataRow RCol in dtParameter.Rows)
                            {
                                System.Windows.Forms.TreeNode N = new TreeNode(RCol["ObjectName"].ToString());
                                N.Tag = RCol;
                                this.treeViewDescription.SelectedNode.Nodes.Add(N);
                            }
                        }
                        else if (R["ObjectType"].ToString() == "TRIGGER")
                        {
                            string DefinitionTrigger = this.OriginalSQL(R["ObjectName"].ToString());
                            this.textBoxObjectDefinition.Text = DefinitionTrigger;
                            string DescriptionTrigger = this.getDescription(R["TableName"].ToString(), R["ObjectType"].ToString(), R["ObjectName"].ToString());
                            this.textBoxObjectDescription.Text = DescriptionTrigger;
                        }
                    }
                }
                catch (System.Exception ex)
                {

                }
            }
        }

        private void textBoxObjectDescription_Leave(object sender, EventArgs e)
        {
            if (this.textBoxObjectName.Text.Length > 0 && this.textBoxObjectDescription.Text.Length > 0 && this.textBoxObjectType.Text.Length > 0)
            {
                if (this.textBoxObjectType.Text == "COLUMN" || this.textBoxObjectType.Text == "PARAMETER")
                {
                    if (this.treeViewDescription.SelectedNode.Parent.Parent != null)
                        this.setDescription(this.treeViewDescription.SelectedNode.Parent.Text, this.treeViewDescription.SelectedNode.Parent.Parent.Text, this.textBoxObjectName.Text, this.textBoxObjectDescription.Text);
                }
                else if (this.textBoxObjectType.Text == "TRIGGER")
                {
                    if (this.treeViewDescription.SelectedNode.Parent.Parent.Parent != null)
                        this.setDescription(this.treeViewDescription.SelectedNode.Parent.Parent.Text, this.treeViewDescription.SelectedNode.Parent.Text, this.textBoxObjectName.Text, this.textBoxObjectDescription.Text);
                }
                else
                    this.setDescription(this.textBoxObjectName.Text, this.textBoxObjectType.Text, "", this.textBoxObjectDescription.Text);
            }
            else if (this.textBoxObjectName.Text.Length > 0 && this.textBoxObjectDescription.Text.Length == 0 && this.textBoxObjectType.Text.Length > 0)
                System.Windows.Forms.MessageBox.Show("An empty description is not possible here");
        }

        private void buttonObjectDescriptionSQLadd_Click(object sender, EventArgs e)
        {
            this.ShowSqlDescription(DescriptionGenerationType.Add);
        }

        private void buttonObjectDescriptionSQLupdate_Click(object sender, EventArgs e)
        {
            this.ShowSqlDescription(DescriptionGenerationType.Update);
        }

        private void buttonObjectDescriptionSQLany_Click(object sender, EventArgs e)
        {
            this.ShowSqlDescription(DescriptionGenerationType.Both);
        }

        private void ShowSqlDescription(DescriptionGenerationType Type)
        {
            string SQL = "";
            if (this.textBoxObjectName.Text.Length > 0 && this.textBoxObjectDescription.Text.Length > 0 && this.textBoxObjectType.Text.Length > 0)
            {
                if (this.textBoxObjectType.Text == "COLUMN" || this.textBoxObjectType.Text == "PARAMETER")
                {
                    if (this.treeViewDescription.SelectedNode.Parent.Parent != null)
                        SQL = this.SqlDescription(this.treeViewDescription.SelectedNode.Parent.Text, this.treeViewDescription.SelectedNode.Parent.Parent.Text, this.textBoxObjectName.Text, this.textBoxObjectDescription.Text, Type, this.checkBoxDescriptionAddExistenceCheck.Checked);
                }
                else if (this.textBoxObjectType.Text == "TRIGGER")
                {
                    SQL = this.SqlDescription(this.treeViewDescription.SelectedNode.Parent.Parent.Text, this.treeViewDescription.SelectedNode.Parent.Text, this.textBoxObjectName.Text, this.textBoxObjectDescription.Text, Type, this.checkBoxDescriptionAddExistenceCheck.Checked);
                }
                else
                    SQL = this.SqlDescription(this.textBoxObjectName.Text, this.textBoxObjectType.Text, "", this.textBoxObjectDescription.Text, Type, this.checkBoxDescriptionAddExistenceCheck.Checked);
            }
            if (SQL.Length > 0)
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Description for " + this.textBoxObjectType.Text + " " + this.textBoxObjectName.Text, SQL + "\r\nGO", true);
                f.ShowDialog();
            }
            else
                System.Windows.MessageBox.Show("No description available");
        }

        private void buttonDescriptionAddDeprecated_Click(object sender, EventArgs e)
        {
            if (!this.textBoxObjectDescription.Text.ToLower().StartsWith("deprecated"))
            {
                string NewDescription = "Deprecated";
                if (this.textBoxObjectDescription.Text.Length > 0)
                    NewDescription += ". " + this.textBoxObjectDescription.Text;
                this.textBoxObjectDescription.Text = NewDescription;

            }
        }

        #endregion

        #region Replication preparation

        public void HideReplicationPreparation()
        {
            this.tabControlMain.TabPages.Remove(this.tabPageRepPrep);
            this.tabControlMain.TabPages.Remove(this.tabPageRowGUID);
        }

        #region Common 

        private void buttonReplicationListTables_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = this.TableList();
            this._DtRepPrepTables = dt.Copy();
            foreach (System.Data.DataRow R in this._DtRepPrepTables.Rows)
            {
                if (R["TableName"].ToString().EndsWith("_log")
                    || R["TableName"].ToString() == "sysdiagrams"
                    || R["TableName"].ToString() == "ApplicationSearchSelectionStrings"
                    || R["TableName"].ToString() == "dtproperties"
                    || R["TableName"].ToString() == "ReplicationPublisher")
                    R.Delete();
            }
            this._DtRepPrepTables.AcceptChanges();
            if (this._DtRepPrepTables.Columns.Count > 0)
            {
                this.checkedListBoxRepPrep.DataSource = this._DtRepPrepTables;
                this.checkedListBoxRepPrep.DisplayMember = "TableName";
                this.checkedListBoxRepPrep.ValueMember = "TableName";
            }
            else
            {
                this.checkedListBoxRepPrep.DataSource = null;
            }
        }

        #endregion

        #region Manual preparation

        private void checkedListBoxRepPrep_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.checkedListBoxRepPrep.SelectedItem;
            this._RepPrepTable = R[0].ToString();
            this.ReplicationPreparation();
            foreach (DiversityWorkbench.UserControls.UserControlReplicationPreparation U in this.RepPrepStepControls)
            {
                U.ErrorMessage = "";
            }
        }

        private System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlReplicationPreparation> _RepPrepStepControls;

        public System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlReplicationPreparation> RepPrepStepControls
        {
            get
            {
                if (this._RepPrepStepControls == null)
                {
                    this._RepPrepStepControls = new List<UserControls.UserControlReplicationPreparation>();

                    this.userControlReplicationPreparation01CreateReplPublTable.InitControl(UserControls.UserControlReplicationPreparation.RepPrepSteps.CreateRepPubTable);
                    this._RepPrepStepControls.Add(this.userControlReplicationPreparation01CreateReplPublTable);

                    this.userControlReplicationPreparation02AddRowGUID.InitControl(UserControls.UserControlReplicationPreparation.RepPrepSteps.AddRowGUID);
                    this._RepPrepStepControls.Add(this.userControlReplicationPreparation02AddRowGUID);

                    this.userControlReplicationPreparation03CreateDefaults.InitControl(UserControls.UserControlReplicationPreparation.RepPrepSteps.CreateDefaultRowGUID);
                    this._RepPrepStepControls.Add(this.userControlReplicationPreparation03CreateDefaults);

                    this.userControlReplicationPreparation04CreateTemporaryTable.InitControl(UserControls.UserControlReplicationPreparation.RepPrepSteps.CreateTempTab);
                    this._RepPrepStepControls.Add(this.userControlReplicationPreparation04CreateTemporaryTable);

                    this.userControlReplicationPreparation05ReadData.InitControl(UserControls.UserControlReplicationPreparation.RepPrepSteps.ReadData);
                    this._RepPrepStepControls.Add(this.userControlReplicationPreparation05ReadData);

                    this.userControlReplicationPreparation06DeactivateUpdateTrigger.InitControl(UserControls.UserControlReplicationPreparation.RepPrepSteps.DeactivateTrigger);
                    this._RepPrepStepControls.Add(this.userControlReplicationPreparation06DeactivateUpdateTrigger);

                    this.userControlReplicationPreparation07WriteRowGUID.InitControl(UserControls.UserControlReplicationPreparation.RepPrepSteps.WriteGUID);
                    this._RepPrepStepControls.Add(this.userControlReplicationPreparation07WriteRowGUID);

                    this.userControlReplicationPreparation08WriteDate.InitControl(UserControls.UserControlReplicationPreparation.RepPrepSteps.WriteLogDate);
                    this._RepPrepStepControls.Add(this.userControlReplicationPreparation08WriteDate);

                    this.userControlReplicationPreparation09ActivateUpdateTrigger.InitControl(UserControls.UserControlReplicationPreparation.RepPrepSteps.ActivateTrigger);
                    this._RepPrepStepControls.Add(this.userControlReplicationPreparation09ActivateUpdateTrigger);

                    this.userControlReplicationPreparation10DeleteTempTable.InitControl(UserControls.UserControlReplicationPreparation.RepPrepSteps.DeleteTempTab);
                    this._RepPrepStepControls.Add(this.userControlReplicationPreparation10DeleteTempTable);
                }
                return _RepPrepStepControls;
            }
        }

        private string _RepPrepTable = "";
        private string RepPrepTable() { return this._RepPrepTable; }

        private void buttonStartReplicationPreparation_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            foreach (DiversityWorkbench.UserControls.UserControlReplicationPreparation U in this.RepPrepStepControls)
            {
                if (U.getState() == UserControls.UserControlReplicationPreparation.RepPrepStepState.ToDo)
                {
                    U.PrepareReplication();
                }
            }
            this.ReplicationPreparation();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void ReplicationPreparation()
        {
            this.userControlReplicationPreparation02AddRowGUID.SetTableName(this.RepPrepTable());
            string Message = "";
            bool DoAnything = false;
            if (this.userControlReplicationPreparation02AddRowGUID.PreconditionFulfilled(ref Message))
            {
                foreach (DiversityWorkbench.UserControls.UserControlReplicationPreparation U in this.RepPrepStepControls)
                {
                    U.SetTableName(this.RepPrepTable());
                }
                if (this.userControlReplicationPreparation02AddRowGUID.getState() == UserControls.UserControlReplicationPreparation.RepPrepStepState.Done)
                {
                    if (this.userControlReplicationPreparation05ReadData.getState() == UserControls.UserControlReplicationPreparation.RepPrepStepState.NotNecessary)
                    {
                        this.userControlReplicationPreparation04CreateTemporaryTable.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.NotNecessary);
                        this.userControlReplicationPreparation07WriteRowGUID.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.NotNecessary);
                        this.userControlReplicationPreparation10DeleteTempTable.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.NotNecessary);
                    }
                    else
                    {
                        this.userControlReplicationPreparation04CreateTemporaryTable.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.ToDo);
                        this.userControlReplicationPreparation07WriteRowGUID.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.ToDo);
                        this.userControlReplicationPreparation10DeleteTempTable.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.ToDo);
                    }
                }
                else
                {
                    this.userControlReplicationPreparation04CreateTemporaryTable.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.ToDo);
                    this.userControlReplicationPreparation07WriteRowGUID.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.ToDo);
                    this.userControlReplicationPreparation10DeleteTempTable.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.ToDo);
                    DoAnything = true;
                    Message = "Please adapt the triggers for update and delete after the preparation is done";
                }
                if (this.userControlReplicationPreparation08WriteDate.getState() == UserControls.UserControlReplicationPreparation.RepPrepStepState.NotNecessary &&
                    this.userControlReplicationPreparation02AddRowGUID.getState() == UserControls.UserControlReplicationPreparation.RepPrepStepState.Done &&
                    this.userControlReplicationPreparation05ReadData.getState() == UserControls.UserControlReplicationPreparation.RepPrepStepState.NotNecessary)
                {
                    this.userControlReplicationPreparation06DeactivateUpdateTrigger.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.NotNecessary);
                    this.userControlReplicationPreparation09ActivateUpdateTrigger.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.NotNecessary);
                }
                else
                {
                    this.userControlReplicationPreparation06DeactivateUpdateTrigger.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.ToDo);
                    this.userControlReplicationPreparation09ActivateUpdateTrigger.setState(UserControls.UserControlReplicationPreparation.RepPrepStepState.ToDo);
                    DoAnything = true;
                }
                if (this.userControlReplicationPreparation08WriteDate.getState() == UserControls.UserControlReplicationPreparation.RepPrepStepState.ToDo)
                    Message = "Please check the triggers for update and delete and the defaults for the logging columns after the preparation is done";
                this.buttonStartReplicationPreparation.Enabled = DoAnything;
            }
            else
            {
                foreach (DiversityWorkbench.UserControls.UserControlReplicationPreparation U in this.RepPrepStepControls)
                {
                    U.Reset();
                }
                this.buttonStartReplicationPreparation.Enabled = false;
            }
            if (Message.Length == 0)
            {

            }
            this.labelRepPrepMessage.Text = Message;
            //string SQL = "select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C " +
            //    "where C.TABLE_NAME = '" + this.RepPrepTable() + "' " +
            //    "and C.COLUMN_NAME like 'Log%tedWhen'";
            //string Count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //if (Count == "2")
            //{
            //    this.buttonRepPrep01AddLogColumns.Tag = RepPrepStepState.NotNecessary;
            //    this.buttonRepPrep01AddLogColumns.Text = RepPrepStepState.NotNecessary.ToString();
            //}
            //else
            //{
            //    this.buttonRepPrep01AddLogColumns.Tag = RepPrepStepState.ToDo;
            //    this.buttonRepPrep01AddLogColumns.Text = RepPrepStepState.ToDo.ToString();
            //}
            //SQL = "select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C " +
            //    "where C.TABLE_NAME = '" + this.RepPrepTable() + "' " +
            //    "and C.COLUMN_NAME = 'RowGUID'";
            //Count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //if (Count == "1")
            //{
            //    this.buttonRepPrep02AddRowGUID.Tag = RepPrepStepState.NotNecessary;
            //    this.buttonRepPrep02AddRowGUID.Text = RepPrepStepState.NotNecessary.ToString();
            //}
            //else
            //{
            //    this.buttonRepPrep02AddRowGUID.Tag = RepPrepStepState.ToDo;
            //    this.buttonRepPrep02AddRowGUID.Text = RepPrepStepState.ToDo.ToString();
            //}

        }

        private bool RepPrepLogColumsExist()
        {
            return true;
        }

        private bool RepPrepRowGUIDExists()
        {
            return true;
        }

        private bool RepPrepLogColumnDefaultExists()
        {
            return true;
        }

        private bool RepPrepRowGUIDDefaultExists()
        {
            return true;
        }

        private string RepPrepTemporaryTable()
        {
            return "";
        }

        private System.Data.DataTable _DtRepPrepTables;

        #endregion

        #region Script

        private bool RepPrepScriptPreconditionsAreFulfilled(ref string Message)
        {
            bool OK = true;
            foreach (System.Data.DataRowView R in this.checkedListBoxRepPrep.CheckedItems)
            {
                string Table = R["TableName"].ToString();
                //Replication.TableName = Table;
                if (!Table.EndsWith("_Enum"))
                {
                    if (!Replication.logTableExists())
                    {
                        OK = false;
                        Message += "Log table is missing.\r\n";
                    }
                    if (!Replication.TableTriggerList().Contains("trgUpd" + Table))
                    {
                        OK = false;
                        Message += "Update trigger is missing.\r\n";
                    }
                    if (!Replication.TableTriggerList().Contains("trgDel" + Table))
                    {
                        OK = false;
                        Message += "Delete trigger is missing.\r\n";
                    }
                }
                if (Message.Length > 0)
                    Message = "Table " + Table + ":\r\n" + Message;
            }
            return OK;
        }

        private void buttonRepPrepAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxRepPrep.Items.Count; i++)
                this.checkedListBoxRepPrep.SetItemChecked(i, true);
        }

        private void buttonRepPrepNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxRepPrep.Items.Count; i++)
                this.checkedListBoxRepPrep.SetItemChecked(i, false);
        }

        private void buttonRepPrepScript_Click(object sender, EventArgs e)
        {
            this.textBoxRepPrepScript.Text = "";
            //string Message = "";
            //if (!this.RepPrepScriptPreconditionsAreFulfilled(ref Message))
            //{
            //    this.textBoxRepPrepScript.Text = Message;
            //    this.textBoxRepPrepScript.ForeColor = System.Drawing.Color.Red;
            //}
            //else
            {
                string SQL = "USE " + DiversityWorkbench.Settings.Connection.Database + ";\r\n" +
                "GO\r\n";
                SQL += Replication.SqlReplicationPublisherTableDefinition() + "\r\n";
                this.textBoxRepPrepScript.ForeColor = System.Drawing.Color.Black;
                foreach (System.Data.DataRowView R in this.checkedListBoxRepPrep.CheckedItems)
                {
                    //System.Data.DataRow R = this._DtRepPrepTables.Rows[i];
                    Replication.TableName = R["TableName"].ToString();
                    string Spacer = "";
                    foreach (char C in Replication.TableName) Spacer += "#";
                    SQL += "--##" + Spacer + "############################################################################\r\n" +
                        "--  " + Replication.TableName + "  ##########################################################################\r\n" +
                        "--##" + Spacer + "############################################################################\r\n" +
                        this.RepPrepTableScript();
                }
                this.textBoxRepPrepScript.Text = SQL;
            }
        }

        private string RepPrepTableScript()
        {
            bool IsStandardEnumerationTable = false;
            bool IsLoggedTable = true;

            if (Replication.TableName.EndsWith("_Enum"))
            {
                if (Replication.DtColumns.Rows.Count > 5
                    && Replication.DtColumns.Rows[0][0].ToString() == "Code"
                    && Replication.DtColumns.Rows[1][0].ToString() == "Description"
                    && Replication.DtColumns.Rows[2][0].ToString() == "DisplayText"
                    && Replication.DtColumns.Rows[3][0].ToString() == "DisplayOrder"
                    && Replication.DtColumns.Rows[4][0].ToString() == "DisplayEnable"
                    && Replication.DtColumns.Rows[5][0].ToString() == "InternalNotes")
                {
                    if (Replication.DtColumns.Rows.Count == 6)
                        IsStandardEnumerationTable = true;
                    else if (Replication.DtColumns.Rows.Count == 7
                        && (Replication.DtColumns.Rows[6][0].ToString() == "ParentCode"
                        || Replication.DtColumns.Rows[6][0].ToString() == "RowGUID"))
                        IsStandardEnumerationTable = true;
                }
            }
            if (IsStandardEnumerationTable)
                IsLoggedTable = false;
            else
            {
                if (Replication.TableName.IndexOf("Proxy") > -1
                    || Replication.TableName.StartsWith("Project"))
                    IsLoggedTable = false;
            }
            string SQL = "";
            this._SqlTrgDel = "";
            this._SqlTrgUpd = "";
            //bool IsStandardUpdateTrigger = true;
            //bool IsStandardDeleteTrigger = true;
            //string trgUpd = "";
            //string trgDel = "";
            //string Message = "";
            string Ori = this.OriginalSQL("trgUpd" + Replication.TableName);
            string New = "";
            Ori = this.SqlRemoveComment(Ori);
            Ori = Ori.Replace("\r\n", "");
            if (Ori.Trim().Length > 0)
            {
                New = this.SqlUpdateTrigger(Replication.TableName, "", true, this.checkBoxRepPrepDsgvo.Checked);
                New = this.SqlRemoveComment(New);
                New = New.Replace("\r\n", "");
                if (Ori != New)
                {
                    //IsStandardUpdateTrigger = false;
                    //SQL += "\r\n--The update trigger is no standard trigger:\r\n";
                    //SQL += "--" + this.OriginalSQL("trgUpd" + Replication.TableName).Replace("\r\n", "\r\n--") + "\r\n\r\n";
                    this._SqlTrgUpd = "--" + this.OriginalSQL("trgUpd" + Replication.TableName).Replace("\r\n", "\r\n--") + "\r\n\r\n";
                }
            }
            Ori = this.OriginalSQL("trgDel" + Replication.TableName);
            New = "";
            Ori = this.SqlRemoveComment(Ori);
            Ori = Ori.Replace("\r\n", "");
            if (Ori.Trim().Length > 0)
            {
                New = this.SqlDeleteTrigger(Replication.TableName, "", true, this.checkBoxRepPrepDsgvo.Checked);
                New = this.SqlRemoveComment(New);
                New = New.Replace("\r\n", "");
                if (Ori != New)
                {
                    //SQL += "\r\n--The delete trigger is no standard trigger:\r\n";
                    //SQL += "--" + this.OriginalSQL("trgDel" + Replication.TableName).Replace("\r\n", "\r\n--") + "\r\n\r\n";
                    this._SqlTrgDel = "--" + this.OriginalSQL("trgDel" + Replication.TableName).Replace("\r\n", "\r\n--") + "\r\n\r\n";
                    //IsStandardUpdateTrigger = false;
                    //Message += "the delete trigger is no standard trigger\r\n";
                    //trgDel = "--" + this.OriginalSQL("trgDel" + Replication.TableName).Replace("\r\n", "\r\n--");
                }
            }
            //if (!IsStandardUpdateTrigger ||
            //    !IsStandardDeleteTrigger)
            //{
            //    System.Windows.Forms.MessageBox.Show("The Script can not be generated automatically because\r\n" + Message);
            //    return "";
            //}
            //AddLogColumns, CreateLogDefaults, AddRowGUID, CreateRowGUIDDefault, CreateTempTab, ReadData, DeleteUpdateTrigger, WriteGUID, WriteLogDate, CreateUpdateTrigger, ChangeDeleteTrigger, DeleteTempTab
            //SQL += "USE " + DiversityWorkbench.Settings.Connection.Database + ";\r\n" +
            //    "GO\r\n";
            //SQL += Replication.SqlReplicationPublisherTableDefinition();
            if (IsLoggedTable)
            {
                SQL += Replication.SqlAttachLogColumns();
                SQL += Replication.SqlAddLogColumnDefault();
                SQL += "\r\n-- Deleting the update trigger\r\n\r\n";
                SQL += this.SqlDropTrigger("trgUpd" + Replication.TableName);
                SQL += "\r\nGO\r\n\r\n-- Deleting the delete trigger\r\n\r\n";
                SQL += this.SqlDropTrigger("trgDel" + Replication.TableName);
                SQL += "\r\nGO\r\n";
                SQL += Replication.SqlFillLogColumns();
            }
            SQL += Replication.SqlAttachRowGUIDColumn();
            SQL += Replication.SqlAddRowGUIDDefault();
            if (IsLoggedTable)
            {
                SQL += "\r\n-- Add log table if missing\r\n\r\nIF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS T " +
                    "where T.TABLE_NAME = '" + Replication.TableName + "_log') = 0\r\n" +
                    "BEGIN\r\n";
                SQL += this.SqlLogTable(Replication.TableName, true);
                SQL += "\r\nEND;\r\n";
                SQL += Replication.SqlAttachLogColumnsToLogTable();
                SQL += Replication.SqlAttachRowGUIDColumnToLogTable();
            }
            SQL += Replication.SqlTempRowGUIDTable();
            SQL += "\r\n\r\n-- Transfer data to temorary table for RowGUID\r\n\r\n";
            SQL += Replication.SqlTransferDataInTempRowGUIDTable();
            SQL += "\r\n\r\n-- Write RowGUID in data\r\n\r\n";
            SQL += Replication.SqlWriteRowGUID();
            if (IsLoggedTable)
                SQL += Replication.SqlWriteRowGUIDToLogTable();
            SQL += "\r\n\r\n-- Deleting temorary table for RowGUID\r\n\r\n";
            SQL += Replication.SqlDropTempRowGUIDTable();
            SQL += "GO\r\n\r\n";
            if (IsLoggedTable)
            {
                SQL += "\r\n\r\n-- Create update trigger\r\n\r\n";
                if (this._SqlTrgUpd.Length > 0)
                    SQL += "-- Update trigger is NO STANDARD trigger ----------------------------------------------------------------\r\n\r\n" + this._SqlTrgUpd +
                        "\r\n----------------------------------------------------------------------------------------\r\n\r\n";
                SQL += this.SqlUpdateTrigger(Replication.TableName, "", true, this.checkBoxRepPrepDsgvo.Checked);
                SQL += "\r\n\r\nGO\r\n\r\n";
                SQL += "\r\n\r\n-- Create delete trigger\r\n\r\n";
                if (this._SqlTrgDel.Length > 0)
                    SQL += "-- Delete trigger is NO STANDARD trigger ----------------------------------------------------------------\r\n\r\n" + this._SqlTrgDel +
                        "\r\n----------------------------------------------------------------------------------------\r\n\r\n";
                SQL += this.SqlDeleteTrigger(Replication.TableName, "", true, this.checkBoxRepPrepDsgvo.Checked);
                SQL += "\r\n\r\nGO\r\n\r\n";
            }

            return SQL;
        }

        private string SqlRemoveComment(string SQL)
        {
            while (SQL.IndexOf("--") > -1)
            {
                string Start = SQL.Substring(0, SQL.IndexOf("--"));
                string AfterComment = SQL.Substring(SQL.IndexOf("--"));
                string End = AfterComment.Substring(AfterComment.IndexOf("\r\n"));
                if (AfterComment.IndexOf("\r\n") == -1)
                    End = "";
                SQL = Start + End;
            }
            while (SQL.IndexOf("/*") > -1)
            {
                string Start = SQL.Substring(0, SQL.IndexOf("/*"));
                string End = SQL.Substring(SQL.IndexOf("*/") + 2);
                SQL = Start + End;
            }
            return SQL;
        }

        private System.Collections.Generic.Dictionary<RepPrepScriptTableStep, bool> _RepPrepScriptTableSteps;

        private enum RepPrepScriptTableStep { CreateLogTable, AddLogColumns, CreateLogDefaults, AddRowGUID, CreateRowGUIDDefault, CreateTempTab, ReadData, DeleteTrigger, WriteGUID, WriteLogDate, CreateTrigger, DeleteTempTab };

        //public System.Collections.Generic.Dictionary<RepPrepScriptTableStep, bool> RepPrepScriptTableSteps
        //{
        //    get 
        //    {
        //        if (this._RepPrepScriptTableSteps == null)
        //        {
        //            this._RepPrepScriptTableSteps = new Dictionary<RepPrepScriptTableStep, bool>();
        //            this._RepPrepScriptTableSteps.Add(RepPrepScriptTableStep.CreateLogTable, false);
        //            this._RepPrepScriptTableSteps.Add(RepPrepScriptTableStep.AddLogColumns, false);
        //            this._RepPrepScriptTableSteps.Add(RepPrepScriptTableStep.CreateLogDefaults, false);
        //            this._RepPrepScriptTableSteps.Add(RepPrepScriptTableStep.AddRowGUID, false);
        //            this._RepPrepScriptTableSteps.Add(RepPrepScriptTableStep.CreateRowGUIDDefault, false);
        //            this._RepPrepScriptTableSteps.Add(RepPrepScriptTableStep.ReadData, false);
        //            this._RepPrepScriptTableSteps.Add(RepPrepScriptTableStep.DeleteTrigger, false);
        //            this._RepPrepScriptTableSteps.Add(RepPrepScriptTableStep.WriteGUID, false);
        //            this._RepPrepScriptTableSteps.Add(RepPrepScriptTableStep.WriteLogDate, false);
        //            this._RepPrepScriptTableSteps.Add(RepPrepScriptTableStep.CreateTrigger, false);
        //            this._RepPrepScriptTableSteps.Add(RepPrepScriptTableStep.DeleteTempTab, false);
        //        }
        //        return _RepPrepScriptTableSteps; 
        //    }
        //}

        private System.Collections.Generic.List<string> TriggerList(string Tablename)
        {
            System.Collections.Generic.List<string> T = new List<string>();
            string SQL = "select R.name from sys.triggers R, sys.tables A " +
                "where R.parent_id = A.object_id " +
                "and A.name = '" + Tablename + "' " +
                "and R.type = 'TR'";
            return T;
        }

        private void buttonRepPrepScriptSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.StreamWriter sw;
                sw = new System.IO.StreamWriter(this.textBoxRepPrepScriptFile.Text, false, System.Text.Encoding.UTF8);
                sw.Write(this.textBoxRepPrepScript.Text);
                sw.Flush();
                sw.Close();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonRepPrepScriptFile_Click(object sender, EventArgs e)
        {
            this.saveFileDialog = new SaveFileDialog();
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.InitialDirectory = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Tools);// ...Windows.Forms.Application.StartupPath;
            this.saveFileDialog.Filter = "SQL Files|*.sql";
            try
            {
                this.saveFileDialog.ShowDialog();
                if (this.saveFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.saveFileDialog.FileName);
                    this.textBoxRepPrepScriptFile.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion        

        #endregion

        #region Clear log

        private System.Data.DataTable _DtClearLog;

        private void buttonClearLogListTables_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = this.TableList();
            this._DtClearLog = dt.Copy();
            foreach (System.Data.DataRow R in this._DtClearLog.Rows)
            {
                if (!R["TableName"].ToString().EndsWith("_log"))
                    R.Delete();
            }
            this._DtClearLog.AcceptChanges();
            if (this._DtClearLog.Columns.Count > 0)
            {
                this.checkedListBoxClearLog.DataSource = this._DtClearLog;
                this.checkedListBoxClearLog.DisplayMember = "TableName";
                this.checkedListBoxClearLog.ValueMember = "TableName";
            }
            else
            {
                this.checkedListBoxClearLog.DataSource = null;
            }
        }

        private void buttonClearLogSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxClearLog.Items.Count; i++)
                this.checkedListBoxClearLog.SetItemChecked(i, true);
        }

        private void buttonClearLogSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxClearLog.Items.Count; i++)
                this.checkedListBoxClearLog.SetItemChecked(i, false);
        }

        private void buttonClearLogStart_Click(object sender, EventArgs e)
        {
            string SQL = "";
            bool OK = true;
            string Message = "";
            if (this.checkedListBoxClearLog.CheckedItems.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            foreach (System.Data.DataRowView R in this.checkedListBoxClearLog.CheckedItems)
            {
                //System.Data.DataRow R = this._DtRepPrepTables.Rows[i];
                SQL = "TRUNCATE TABLE " + R["TableName"].ToString();
                if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                    OK = false;
            }
            if (!OK)
                System.Windows.Forms.MessageBox.Show("Clearing the log failed:\r\n" + Message);
            else
            {
                System.Windows.Forms.MessageBox.Show("Log cleared");
                this.checkedListBoxClearLog_SelectedIndexChanged(null, null);
            }
        }

        private void checkedListBoxClearLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable DtLog = new DataTable();
                System.Data.DataRowView R = (System.Data.DataRowView)this.checkedListBoxClearLog.SelectedItem;
                string SQL = "SELECT * FROM " + R[0].ToString();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(DtLog);
                this.dataGridViewClearLog.DataSource = DtLog;
            }
            catch (System.Exception ex)
            {

            }
        }

        #endregion

        #region EU-DSGVO

        #region Common

        private System.Data.DataTable _DtEuDsgvo;

        private void initEuDsgvo()
        {
            string SQL = "SELECT dbo." + DiversityWorkbench.Database.PrivacyConsentInfoRoutine + "()";
            string Site = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
            if (Site.Length > 0)
            {
                this.linkLabelEuDsgvoInfoURL.Text = Site;
            }
            else
            {
                this.linkLabelEuDsgvoInfoURL.Text = "http://diversityworkbench.net/Portal/General_Data_Protection_Regulation_(GDPR)_(EU)";
            }
            this.textBoxEuDsgvoScriptFile.Text = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Tools) + "SqlEuDsgvo.sql";
            this.checkBoxUpdateTriggerAddDsgvo.Visible = true;
            this.checkBoxUpdateTriggerAddDsgvo.Checked = true;
            // Obsolete - will be done using defaults
            //this.checkBoxDeleteTriggerAddDsgvo.Visible = true;
            //this.checkBoxDeleteTriggerAddDsgvo.Checked = true;
        }

        private void buttonEuDsgvoListTables_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "select T.TABLE_NAME from INFORMATION_SCHEMA.TABLES T, INFORMATION_SCHEMA.TABLES L " +
                    "WHERE T.TABLE_NAME + '_Log' = L.TABLE_NAME AND T.TABLE_TYPE = 'BASE TABLE' AND L.TABLE_TYPE = 'BASE TABLE' " +
                    "UNION " +
                    "SELECT C.TABLE_NAME from INFORMATION_SCHEMA.COLUMNS C " +
                    "WHERE C.COLUMN_NAME = 'LoginName' " +
                    "ORDER BY T.Table_Name";
                string Message = "";
                this._DtEuDsgvo = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtEuDsgvo, ref Message);
                if (this._DtEuDsgvo.Rows.Count == 0)
                {
                    SQL = "SELECT DISTINCT T.TABLE_NAME from INFORMATION_SCHEMA.COLUMNS C, INFORMATION_SCHEMA.TABLES T " +
                        "WHERE T.TABLE_TYPE = 'BASE TABLE' AND C.TABLE_NAME = T.TABLE_NAME  " +
                        "AND C.COLUMN_NAME LIKE 'Log%By' " +
                        "ORDER BY T.Table_Name";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtEuDsgvo, ref Message);
                }
                if (this._DtEuDsgvo.Columns.Count > 0 && this._DtEuDsgvo.Rows.Count > 0)
                {
                    this.checkedListBoxEuDsgvo.DataSource = this._DtEuDsgvo;
                    this.checkedListBoxEuDsgvo.DisplayMember = "TABLE_NAME";
                    this.checkedListBoxEuDsgvo.ValueMember = "TABLE_NAME";
                }
                else
                {
                    this.checkedListBoxEuDsgvo.DataSource = null;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonEuDsgvoSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxEuDsgvo.Items.Count; i++)
                this.checkedListBoxEuDsgvo.SetItemChecked(i, true);
        }

        private void buttonEuDsgvoSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxEuDsgvo.Items.Count; i++)
                this.checkedListBoxEuDsgvo.SetItemChecked(i, false);
        }

        #endregion

        #region Script

        #region Button events

        private void buttonEuDsgvoScriptCreate_Click(object sender, EventArgs e)
        {
            if (this.checkedListBoxEuDsgvo.CheckedItems.Count == 0 && !this.checkBoxEuDsgvoScriptIncludeBasics.Checked)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            this.textBoxEuDsgvoScript.Text = "";
            try
            {
                string SQL = "";
                if (this.checkBoxEuDsgvoScriptIncludeBasics.Checked)
                {
                    SQL = this.EuDsgvoScriptUserProxy();
                    SQL += this.EuDsgvoUserID();
                    SQL += this.EuDsgvoUserName();
                    SQL += this.EuDsgvoInfoSite();
                }
                this.textBoxEuDsgvoScript.ForeColor = System.Drawing.Color.Black;
                this._EuDsgvoTableLogColumns = new Dictionary<string, List<string>>();
                foreach (System.Data.DataRowView R in this.checkedListBoxEuDsgvo.CheckedItems)
                {
                    string Table = R["Table_Name"].ToString();
                    System.Collections.Generic.List<string> L = this.EuDsgvoLogUserColumns(Table);
                    this._EuDsgvoTableLogColumns.Add(Table, L);
                }
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> KV in this._EuDsgvoTableLogColumns)
                {
                    SQL += this.EuDsgvoTableScript(KV.Key, KV.Value);
                }
                this.textBoxEuDsgvoScript.Text = SQL;
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonEuDsgvoScriptSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.StreamWriter sw;
                sw = new System.IO.StreamWriter(this.textBoxEuDsgvoScriptFile.Text, false, System.Text.Encoding.UTF8);
                sw.Write(this.textBoxEuDsgvoScript.Text);
                sw.Flush();
                sw.Close();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonEuDsgvoScriptFolder_Click(object sender, EventArgs e)
        {
            this.saveFileDialog = new SaveFileDialog();
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.InitialDirectory = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Tools);// ...Windows.Forms.Application.StartupPath;
            this.saveFileDialog.Filter = "SQL Files|*.sql";
            try
            {
                this.saveFileDialog.ShowDialog();
                if (this.saveFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.saveFileDialog.FileName);
                    this.textBoxEuDsgvoScriptFile.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        private string _RoleUser;
        private string RoleUser()
        {
            if (this._RoleUser == null)
            {
                this._RoleUser = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("Select TOP 1 [name] From sysusers U Where issqlrole = 1 AND U.name like '%user'");
                if (this._RoleUser.Length == 0 || this._RoleUser == null)
                    this._RoleUser = "USER";
            }
            return this._RoleUser;
        }

        private string _RoleAdmin;
        private string RoleAdmin()
        {
            if (this._RoleAdmin == null)
            {
                this._RoleAdmin = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("Select TOP 1 [name] From sysusers U Where issqlrole = 1 AND U.name like '%Administrat%'");
                if (this._RoleAdmin.Length == 0 || this._RoleAdmin == null)
                    this._RoleAdmin = "USER";
            }
            return this._RoleAdmin;
        }

        private string EuDsgvoScriptUserProxy()
        {
            string SQL = "USE " + DiversityWorkbench.Settings.DatabaseName + ";\r\n" +
            "GO\r\n\r\n";
            // Insert new column if missing
            SQL += "--#####################################################################################################################\r\n" +
                "--######   UserProxy - Add ID - according to EU-DSGVO   ###############################################################\r\n" +
                "--#####################################################################################################################\r\n";
            SQL += "if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'UserProxy' and c.COLUMN_NAME = 'ID') = 0 \r\n" +
                 "begin \r\n" +
                 "   ALTER TABLE UserProxy ADD [ID] [int] IDENTITY(1,1) NOT NULL \r\n" +
                 "   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'ID' " +
                 "end \r\nGO\r\n\r\n";
            SQL += "--#####################################################################################################################\r\n" +
                "--######   UserProxy - Add - PrivacyConsent and PrivacyConsentDate   ##################################################\r\n" +
                "--#####################################################################################################################\r\n";
            SQL += "if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'UserProxy' and c.COLUMN_NAME = 'PrivacyConsent') = 0 \r\n" +
                 "begin \r\n" +
                 "   ALTER TABLE UserProxy ADD [PrivacyConsent] [bit] NULL \r\n" +
                 "   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the user consents the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsent' " +
                 "end \r\nGO\r\n\r\n";
            SQL += "GRANT UPDATE ON [dbo].[UserProxy] ([PrivacyConsent]) TO [" + this.RoleUser() + "]\r\n" +
                "GO\r\n\r\n";
            SQL += "if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'UserProxy' and c.COLUMN_NAME = 'PrivacyConsentDate') = 0 \r\n" +
                 "begin \r\n" +
                 "   ALTER TABLE UserProxy ADD [PrivacyConsentDate] [datetime] NULL \r\n" +
                 "   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time and date when the user consented or refused the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsentDate' " +
                 "end \r\nGO\r\n\r\n";
            SQL += "GRANT UPDATE ON [dbo].[UserProxy] ([PrivacyConsentDate]) TO [" + this.RoleUser() + "]\r\n" +
                "GO\r\n\r\n";
            SQL += "--#####################################################################################################################\r\n" +
                "--######   UserProxy - Trigger for setting PrivacyConsentDate   #######################################################\r\n" +
                "--#####################################################################################################################\r\n";
            SQL += "CREATE TRIGGER [dbo].[trgUpdUserProxy] ON [dbo].[UserProxy] \r\n" +
                "FOR UPDATE AS \r\n" +
                "declare @PC bit \r\n" +
                "if (select count(*) from deleted) = 1 \r\n" +
                "begin \r\n" +
                "  set @PC = (select case when I.PrivacyConsent <> D.PrivacyConsent \r\n" +
                "    or (I.PrivacyConsent is null and not D.PrivacyConsent is null) \r\n" +
                "    or (not I.PrivacyConsent is null and D.PrivacyConsent is null) \r\n" +
                "    then 1 else 0 end from inserted I, deleted D) \r\n" +
                "  if (@PC = 1) \r\n" +
                "  begin \r\n" +
                "    UPDATE U SET PrivacyConsentDate = GETDATE()  \r\n" +
                "    FROM UserProxy U, deleted D \r\n" +
                "    WHERE U.ID = D.ID \r\n" +
                "  end \r\n" +
                "end \r\n" +
                "GO\r\n\r\n";
            return SQL;
        }

        private string EuDsgvoUserID()
        {
            string SQL = "--#####################################################################################################################\r\n" +
                "--######   Function providing the ID of the user from UserProxy  ######################################################\r\n" +
                "--#####################################################################################################################\r\n" +
                "\r\n" +
                "declare @SQL nvarchar(max) " +
                "set @SQL = (select ' " +
                "CREATE FUNCTION [dbo].[UserID] () RETURNS int AS \r\n" +
                "BEGIN  \r\n" +
                "declare @ID int;  \r\n" +
                "SET @ID = (SELECT MIN(ID) FROM UserProxy U WHERE U.LoginName = SUSER_SNAME()) \r\n" +
                "if (@ID is null) \r\n" +
                "begin \r\n" +
                "	SET @ID = (SELECT MIN(ID) FROM UserProxy U WHERE U.LoginName = USER_NAME()) \r\n" +
                "end \r\n" +
                "RETURN @ID  \r\n" +
                "END ') \r\n" +
                "begin try \r\n" +
                "   exec sp_executesql @SQL \r\n" +
                "   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the User as stored in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'UserID' \r\n" +
                "end try \r\n" +
                "begin catch \r\n" +
                "end catch \r\n" +
                "GO \r\n" +
                "GRANT EXEC ON [dbo].[UserID] TO [" + this.RoleUser() + "] \r\n" +
                "GO \r\n\r\n" +
                "BEGIN TRY \r\n" +
                "  EXEC sys.sp_addextendedproperty  \r\n" +
                "    @name=N'MS_Description', @value=N'Providing the ID of the current user from UserProxy', \r\n" +
                "    @level0type=N'SCHEMA', @level0name=N'dbo', \r\n" +
                "    @level1type=N'FUNCTION', @level1name=N'UserID'; \r\n" +
                "END TRY \r\n" +
                "BEGIN CATCH \r\n" +
                "  EXEC sys.sp_updateextendedproperty  \r\n" +
                "    @name=N'MS_Description', @value=N'Providing the ID of the current user from UserProxy', \r\n" +
                "    @level0type=N'SCHEMA', @level0name=N'dbo', \r\n" +
                "    @level1type=N'FUNCTION', @level1name=N'UserID'; \r\n" +
                "END CATCH \r\n" +
                "GO \r\n\r\n";

            return SQL;
        }

        private string EuDsgvoUserName()
        {
            string SQL = "--#####################################################################################################################\r\n" +
                "--######   Function providing the name of the user from UserProxy  ####################################################\r\n" +
                "--#####################################################################################################################\r\n" +
                "\r\n" +
                "declare @SQL nvarchar(max) " +
                "set @SQL = (select ' " +
                "CREATE FUNCTION [dbo].[UserName] (@ID varchar(10)) RETURNS nvarchar(50) AS \r\n" +
                "BEGIN  \r\n" +
                "declare @User nvarchar(50);  \r\n" +
                "SET @User = (SELECT MIN(CASE WHEN U.CombinedNameCache <> '''' THEN substring(U.CombinedNameCache, 1, 50) ELSE CASE WHEN U.LoginName <> '''' THEN U.LoginName ELSE cast(U.ID as varchar) END END) FROM UserProxy U WHERE cast(U.ID as varchar) = @ID)  \r\n" +
                "RETURN @User  \r\n" +
                "END ') \r\n" +
                "begin try \r\n" +
                "   exec sp_executesql @SQL \r\n" +
                "   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the User as stored in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'UserName' \r\n" +
                "end try \r\n" +
                "begin catch \r\n" +
                "end catch \r\n" +
                "GO \r\n" +
                "GRANT EXEC ON [dbo].[UserName] TO [" + this.RoleUser() + "] \r\n" +
                "GO \r\n\r\n" +
                "BEGIN TRY \r\n" +
                "  EXEC sys.sp_addextendedproperty  \r\n" +
                "    @name=N'MS_Description', @value=N'Providing the name of the user from UserProxy', \r\n" +
                "    @level0type=N'SCHEMA', @level0name=N'dbo', \r\n" +
                "    @level1type=N'FUNCTION', @level1name=N'UserName'; \r\n" +
                "END TRY \r\n" +
                "BEGIN CATCH \r\n" +
                "  EXEC sys.sp_updateextendedproperty  \r\n" +
                "    @name=N'MS_Description', @value=N'Providing the name of the user from UserProxy', \r\n" +
                "    @level0type=N'SCHEMA', @level0name=N'dbo', \r\n" +
                "    @level1type=N'FUNCTION', @level1name=N'UserName'; \r\n" +
                "END CATCH \r\n" +
                "BEGIN TRY \r\n; " +
                "  EXEC sys.sp_addextendedproperty  \r\n" +
                "    @name=N'MS_Description', @value=N'ID of the user according to table UserProxy', \r\n" +
                "    @level0type=N'SCHEMA', @level0name=N'dbo', \r\n" +
                "    @level1type=N'FUNCTION', @level1name=N'UserName', \r\n" +
                "    @level2type=N'PARAMETER', @level2name=N'@ID'; \r\n" +
                "END TRY \r\n" +
                "BEGIN CATCH \r\n" +
                "  EXEC sys.sp_updateextendedproperty  \r\n" +
                "    @name=N'MS_Description', @value=N'ID of the user according to table UserProxy', \r\n" +
                "    @level0type=N'SCHEMA', @level0name=N'dbo', \r\n" +
                "    @level1type=N'FUNCTION', @level1name=N'UserName', \r\n" +
                "    @level2type=N'PARAMETER', @level2name=N'@ID';\r\n " +
                "END CATCH\r\n" +
                "GO \r\n\r\n";
            return SQL;
        }

        private string EuDsgvoInfoSite()
        {
            string SQL = "--#####################################################################################################################\r\n" +
                "--######   Function PrivacyConsentInfo providing common information within the DiversityWorkbench  ####################\r\n" +
                "--#####################################################################################################################\r\n" +
                "\r\n" +
                "CREATE FUNCTION [dbo].[PrivacyConsentInfo] () \r\n" +
                "RETURNS varchar (900) \r\n" +
                "AS  \r\n" +
                "BEGIN return 'http://diversityworkbench.net/Portal/Default_Agreement_on_Processing_of_Personal_Data_in_DWB_Software'  \r\n" +
                "END; \r\n" +
                "GO \r\n" +
                "GRANT EXEC ON [dbo].[PrivacyConsentInfo] TO [" + this.RoleUser() + "] \r\n" +
                "GO \r\n" +
                "GRANT ALTER ON [dbo].[PrivacyConsentInfo] TO [" + this.RoleAdmin() + "] \r\n" +
                "GO \r\n\r\n" +
                "BEGIN TRY \r\n" +
                "  EXEC sys.sp_addextendedproperty  \r\n" +
                "    @name=N'MS_Description', @value=N'Providing common information about the storage and processing of personal data within the DiversityWorkbench', \r\n" +
                "    @level0type=N'SCHEMA', @level0name=N'dbo', \r\n" +
                "    @level1type=N'FUNCTION', @level1name=N'PrivacyConsentInfo'; \r\n" +
                "END TRY \r\n" +
                "BEGIN CATCH \r\n" +
                "  EXEC sys.sp_updateextendedproperty  \r\n" +
                "    @name=N'MS_Description', @value=N'Providing common information about the storage and processing of personal data within the DiversityWorkbench', \r\n" +
                "    @level0type=N'SCHEMA', @level0name=N'dbo', \r\n" +
                "    @level1type=N'FUNCTION', @level1name=N'PrivacyConsentInfo'; \r\n" +
                "END CATCH \r\n" +
                "GO \r\n\r\n";
            return SQL;
        }

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _EuDsgvoTableLogColumns;

        private string EuDsgvoTableScript(string Table, System.Collections.Generic.List<string> Columns)
        {
            string SQL = "";
            SQL += "--#####################################################################################################################\r\n" +
                "--######  Table " + Table.ToUpper() + "     ########################################################################################\r\n" +
                "--#####################################################################################################################\r\n" +
                "--######   UserProxy - filling with login data from table      ########################################################\r\n" +
                "--#####################################################################################################################\r\n";
            // insert user that may be missing in UserProxy
            foreach (string C in Columns)
            {
                SQL += this.EuDsgvoInsertUser(Table, C) + " \r\nGO\r\n\r\n";
                if (this.HasLogTable(Table))
                    SQL += this.EuDsgvoInsertUser(Table + "_log", C) + " \r\nGO\r\n\r\n";
            }
            if (HasLogTable(Table))
                SQL += this.EuDsgvoInsertUser(Table + "_log", "LogUser") + " \r\nGO\r\n\r\n";

            SQL += this.EuDsgvoLogConstraints(Table);
            if (HasLogTable(Table))
                SQL += this.EuDsgvoLogConstraints(Table + "_log");

            // setting the values to the new ID
            SQL += "--#####################################################################################################################\r\n" +
                "--######   Setting the ID as new value for LogUpdatedBy etc. according to EU-DSGVO  ###################################\r\n" +
                "--#####################################################################################################################\r\n" +
                "\r\n";
            if (this.TriggerDoesExist("trgUpd" + Table))
            {
                SQL += "DISABLE TRIGGER dbo.trgUpd" + Table + " ON dbo.[" + Table + "] \r\n" +
                "GO\r\n\r\n";
            }
            foreach (string C in Columns)
            {
                SQL += "UPDATE E \r\n" +
                    "SET [" + C + "] = cast(U.ID as varchar) \r\n" +
                    "FROM [dbo].[" + Table + "] E, [dbo].[UserProxy] U \r\n" +
                    "WHERE U.LoginName = E." + C + " \r\n" +
                    "GO\r\n\r\n";
                SQL += "UPDATE E \r\n" +
                    "SET [" + C + "] = cast(U.ID as varchar) \r\n" +
                    "FROM [dbo].[" + Table + "_log] E, [dbo].[UserProxy] U \r\n" +
                    "WHERE U.LoginName = E." + C + " \r\n" +
                    "GO\r\n\r\n";
            }
            SQL += "UPDATE E \r\n" +
                "SET [LogUser] = cast(U.ID as varchar) \r\n" +
                "FROM [dbo].[" + Table + "_log] E, [dbo].[UserProxy] U \r\n" +
                "WHERE U.LoginName = E.LogUser \r\n" +
                "GO\r\n";
            if (TriggerDoesExist("trgUpd" + Table))
            {
                SQL += "\r\n" +
                "ENABLE TRIGGER dbo.trgUpd" + Table + " ON dbo.[" + Table + "] \r\n" +
                "GO\r\n\r\n";
            }
            if (HasLogTable(Table))
            {
                SQL += "--#####################################################################################################################\r\n" +
                    "--######   Setting the descriptions for LogUpdatedBy etc. according to EU-DSGVO  ######################################\r\n" +
                    "--#####################################################################################################################\r\n" +
                    "\r\n";
                // setting the description of the log columns
                foreach (string C in Columns)
                {
                    if (C == "LogUpdatedBy")
                        SQL += "EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'ID of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + Table + "', @level2type=N'COLUMN',@level2name=N'" + C + "'\r\n" +
                        "GO\r\n\r\n";
                    else if (C == "LogInsertedBy" || C == "LogCreatedBy")
                        SQL += "EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'ID of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + Table + "', @level2type=N'COLUMN',@level2name=N'" + C + "'\r\n" +
                        "GO\r\n\r\n";
                }
            }
            return SQL;
        }

        private bool HasLogTable(string Table)
        {
            string SQL = "SELECT T.TABLE_NAME FROM INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_NAME = '" + Table + "_log'";
            string LogTable = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (LogTable.Length == 0)
                return false;
            else return true;
        }

        private bool TriggerDoesExist(string Trigger)
        {
            if (this.OriginalSQL(Trigger).Length > 0)
                return true;
            else
                return false;
        }

        private string EuDsgvoLogConstraints(string Table)
        {
            string SQL = "";
            string SqlTable = "Select Col.[Name] " +
                "From SysObjects Inner Join (Select [Name],[ID] From SysObjects) As Tab On Tab.[ID] = Sysobjects.[Parent_Obj]   " +
                "Inner Join sysconstraints On sysconstraints.Constid = Sysobjects.[ID]   " +
                "Inner Join SysColumns Col On Col.[ColID] = sysconstraints.[ColID] And Col.[ID] = Tab.[ID]  " +
                "where [Tab].[Name] = '" + Table + "' AND (Col.[Name] LIKE 'Log%tedBy' OR Col.[Name] = 'LogUser' OR Col.[Name] = 'LoginName')";
            string Message = "";
            System.Data.DataTable dt = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SqlTable, ref dt, ref Message);
            if (dt.Rows.Count > 0)
            {
                SQL = "--#####################################################################################################################\r\n" +
                        "--######   Changing constraints for logging columns  ##################################################################\r\n" +
                        "--#####################################################################################################################\r\n" +
                        "\r\n";
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    SQL += "declare @Command nvarchar(500); \r\n" +
                        "SET @Command = (Select TOP 1 'ALTER TABLE [dbo].[' + [Tab].[Name] + '] DROP CONSTRAINT [' + SysObjects.[Name] + ']' As [DropCommand]  \r\n" +
                        "  From SysObjects Inner Join (Select [Name],[ID] From SysObjects) As Tab On Tab.[ID] = Sysobjects.[Parent_Obj]   \r\n" +
                        "  Inner Join sysconstraints On sysconstraints.Constid = Sysobjects.[ID]  \r\n" +
                        "  Inner Join SysColumns Col On Col.[ColID] = sysconstraints.[ColID] And Col.[ID] = Tab.[ID] \r\n" +
                        "  where [Tab].[Name] = '" + Table + "' AND (Col.[Name] = '" + R[0].ToString() + "')) \r\n" +
                        "begin try \r\n" +
                        "  exec sp_executesql @Command \r\n" +
                        "end try \r\n" +
                        "begin catch \r\n" +
                        "end catch \r\n" +
                        "GO \r\n\r\n";
                }
                SqlTable = "Select  SysObjects.[Name] As [Constraint Name], " +
                    "Col.[Name] As [Column Name] " +
                    "From SysObjects Inner Join " +
                    "(Select [Name],[ID] From SysObjects) As Tab " +
                    "On Tab.[ID] = Sysobjects.[Parent_Obj]  " +
                    "Inner Join sysconstraints On sysconstraints.Constid = Sysobjects.[ID]  " +
                    "Inner Join SysColumns Col On Col.[ColID] = sysconstraints.[ColID] And Col.[ID] = Tab.[ID] " +
                    "where [Tab].[Name] = '" + Table + "' AND (Col.[Name] LIKE 'Log%tedBy' OR Col.[Name] = 'LogUser')";
                dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SqlTable, ref dt, ref Message);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    SQL += "ALTER TABLE [dbo].[" + Table + "] ADD CONSTRAINT [DF_" + Table + "_" + R[1].ToString() + "] DEFAULT ([dbo].[UserID]()) FOR [" + R[1].ToString() + "] \r\n" +
                        "GO\r\n\r\n";
                }
            }
            return SQL;
        }

        private System.Collections.Generic.List<string> EuDsgvoLogUserColumns(string Table)
        {
            string SQL = "select DISTINCT c.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = '" + Table + "' and (c.COLUMN_NAME LIKE 'Log%tedBy' or c.COLUMN_NAME = 'LoginName')";
            System.Data.DataTable dt = new DataTable();
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            System.Collections.Generic.List<string> L = new List<string>();
            foreach (System.Data.DataRow R in dt.Rows)
                L.Add(R[0].ToString());
            return L;
        }

        private string EuDsgvoInsertUser(string Table, string Column)
        {
            string SQL = "INSERT INTO [dbo].[UserProxy] " +
                "([LoginName], CombinedNameCache) \r\n" +
                "SELECT DISTINCT L." + Column + ", case when L." + Column + " like '%\\%' then substring(L." + Column + ", charindex('\\', L." + Column + ") + 1, 255) else L." + Column + " end \r\n" +
                "FROM [" + Table + "] AS L LEFT OUTER JOIN " +
                "UserProxy AS U ON L." + Column + " = U.LoginName \r\n" +
                "WHERE (U.LoginName IS NULL)  AND NOT L." + Column + " IS NULL AND isnumeric(L." + Column + ") = 0";
            return SQL;
        }

        private string EuDsgvoUpdateLogUser(string Table, string Column)
        {
            string SQL = "UPDATE E " +
                "SET [LogUpdatedBy] = cast(U.ID as varchar) \r\n" +
                "FROM [dbo].[" + Table + "] E, [dbo].[UserProxy] U \r\n" +
                "WHERE U.LoginName = E." + Column + "\r\n";
            return SQL;
        }

        #region Trigger - obsolet

        private string EuDsgvoTriggerScript(string Trigger, string Table)
        {
            if (this.radioButtonEuDsgvoScriptTriggerOld.Checked)
                return this.EuDsgvoTriggerScriptOld(Trigger, Table);
            else
            {
                string SQL = "";
                if (Trigger.StartsWith("trgDel"))
                    SQL = this.SqlDeleteTrigger(Table, this.comboBoxEuDsgvoScriptTriggerNewVersion.Text, true).Replace("CREATE ", "ALTER ");
                else if (Trigger.StartsWith("trgUpd"))
                    SQL = this.SqlUpdateTrigger(Table, this.checkBoxEuDsgvoScriptTriggerNewVersion.Checked, this.comboBoxEuDsgvoScriptTriggerNewVersion.Text, true).Replace("CREATE ", "ALTER ");
                return SQL;
            }
        }

        private string EuDsgvoTriggerScriptOld(string Trigger, string Table)
        {
            string SqlTrg = this.OriginalSQL(Trigger).Replace("CREATE ", "ALTER ");
            System.Collections.Generic.List<string> L = this.EuDsgvoLogUserColumns(Table);
            System.Collections.Generic.List<string> LDel = new List<string>();
            LDel.Add("Deleted");
            LDel.Add("deleted");
            LDel.Add("DELETED");
            foreach (string Del in LDel)
            {
                foreach (string S in L)
                    SqlTrg = SqlTrg.Replace(" " + Del + "." + S, " cast(U.ID as varchar)");
                //SqlTrg = SqlTrg.Replace(" " + Del + " ", ", UserProxy U");
            }
            SqlTrg = SqlTrg.Replace("FROM ", "FROM UserProxy U, ");
            foreach (string S in L)
                SqlTrg = SqlTrg.Replace(" " + L + " = SYSTEM_USER ", " " + L + " = CAST(U.ID as varchar) ");
            if (SqlTrg.ToUpper().IndexOf(" WHERE ") > -1)
                SqlTrg = SqlTrg.Replace(" WHERE ", " WHERE U.LoginName = SUSER_NAME() AND ");
            else
                SqlTrg += " WHERE U.LoginName = SUSER_NAME()";
            return SqlTrg;
        }

        private void radioButtonEuDsgvoScriptTriggerNew_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonEuDsgvoScriptTriggerNew.Checked)
            {
                this.checkBoxEuDsgvoScriptTriggerNewVersion.Enabled = true;
            }
            else
                this.checkBoxEuDsgvoScriptTriggerNewVersion.Enabled = false;
        }

        private void checkBoxEuDsgvoScriptTriggerNewVersion_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxEuDsgvoScriptTriggerNewVersion.Checked)
                this.comboBoxEuDsgvoScriptTriggerNewVersion.Enabled = true;
            else
                this.comboBoxEuDsgvoScriptTriggerNewVersion.Enabled = false;
        }

        private void comboBoxEuDsgvoScriptTriggerNewVersion_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxEuDsgvoScriptTriggerNewVersion.DataSource == null || this.comboBoxEuDsgvoScriptTriggerNewVersion.Items.Count == 0)
            {
                string SQL = "SELECT DISTINCT T.TABLE_NAME FROM INFORMATION_SCHEMA.TABLES T, INFORMATION_SCHEMA.COLUMNS C " +
                    "WHERE T.TABLE_NAME NOT LIKE '%_log' " +
                    "AND T.TABLE_NAME NOT LIKE '%_log_%' " +
                    "AND T.TABLE_TYPE = 'BASE TABLE' " +
                    "AND T.TABLE_NAME NOT LIKE '%_Enum' " +
                    "AND T.TABLE_NAME = C.TABLE_NAME " +
                    "AND C.DATA_TYPE = 'int' " +
                    "AND C.COLUMN_NAME LIKE '%ID' " +
                    "AND C.IS_NULLABLE = 'NO' " +
                    "ORDER BY T.TABLE_NAME";
                string Message = "";
                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                this.comboBoxEuDsgvoScriptTriggerNewVersion.DataSource = dt;
                this.comboBoxEuDsgvoScriptTriggerNewVersion.DisplayMember = "TABLE_NAME";
                this.comboBoxEuDsgvoScriptTriggerNewVersion.ValueMember = "TABLE_NAME";
            }
        }

        #endregion

        #endregion

        #region Script for old objects

        private void buttonEuDsgvoScriptForTriggerEtcCreateScript_Click(object sender, EventArgs e)
        {
            this.textBoxEuDsgvoScriptForTriggerEtcScript.Text = "";
            if (this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions.Checked)
                this.textBoxEuDsgvoScriptForTriggerEtcScript.Text += this.EuDsgvoScriptForFunctions();
            if (this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures.Checked)
                this.textBoxEuDsgvoScriptForTriggerEtcScript.Text += this.EuDsgvoScriptForProcedures();
            if (this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger.Checked)
                this.textBoxEuDsgvoScriptForTriggerEtcScript.Text += this.EuDsgvoScriptForTrigger();
            if (this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews.Checked)
                this.textBoxEuDsgvoScriptForTriggerEtcScript.Text += this.EuDsgvoScriptForViews();
        }

        private void buttonEuDsgvoScriptForTriggerEtcFolder_Click(object sender, EventArgs e)
        {
            this.saveFileDialog = new SaveFileDialog();
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.InitialDirectory = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Tools);// ...Windows.Forms.Application.StartupPath;
            this.saveFileDialog.Filter = "SQL Files|*.sql";
            try
            {
                this.saveFileDialog.ShowDialog();
                if (this.saveFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.saveFileDialog.FileName);
                    this.textBoxEuDsgvoScriptForTriggerEtcFile.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonEuDsgvoScriptForTriggerEtcSaveScript_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.StreamWriter sw;
                sw = new System.IO.StreamWriter(this.textBoxEuDsgvoScriptForTriggerEtcFile.Text, false, System.Text.Encoding.UTF8);
                sw.Write(this.textBoxEuDsgvoScriptForTriggerEtcScript.Text);
                sw.Flush();
                sw.Close();
            }
            catch (System.Exception ex)
            {
            }
        }

        private System.Collections.Generic.Dictionary<string, string> _EuDsgvoScriptForObjectReplacements;

        public System.Collections.Generic.Dictionary<string, string> EuDsgvoScriptForObjectReplacements
        {
            get
            {
                if (this._EuDsgvoScriptForObjectReplacements == null)
                {
                    this._EuDsgvoScriptForObjectReplacements = new Dictionary<string, string>();
                    this._EuDsgvoScriptForObjectReplacements.Add(" = current_user", " = cast(dbo.UserID() as varchar)");
                    this._EuDsgvoScriptForObjectReplacements.Add(" = user_name()", " = cast(dbo.UserID() as varchar)");
                    this._EuDsgvoScriptForObjectReplacements.Add(" = suser_sname()", " = cast(dbo.UserID() as varchar)");
                }
                return _EuDsgvoScriptForObjectReplacements;
            }
        }

        private string EuDsgvoScriptForObject(string Object, string Definition)
        {
            string SQL = "";
            bool TextReplaced = false;
            if (Definition.Length > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.EuDsgvoScriptForObjectReplacements)
                {
                    while (Definition.ToLower().IndexOf(KV.Key) > -1)
                    {
                        int Position = Definition.ToLower().IndexOf(KV.Key);
                        int Length = KV.Key.Length;
                        Definition = Definition.Substring(0, Position) + KV.Value + Definition.Substring(Position + Length);
                        TextReplaced = true;
                    }
                }
            }
            if (TextReplaced)
            {
                SQL = "--#####################################################################################################################\r\n" +
                    "--####  " + Object + " according to DSGVO  ###########################################################################\r\n" +
                    "--#####################################################################################################################\r\n" +
                    Definition + "\r\nGO";
            }
            return SQL;
        }

        private string EuDsgvoScriptForTrigger()
        {
            string Script = "";
            foreach (System.Data.DataRowView R in this.checkedListBoxEuDsgvo.CheckedItems)
            {
                string Table = R["Table_Name"].ToString();
                string Trigger = "trgUpd" + Table;
                string SqlTrg = this.OriginalSQL(Trigger).Replace("CREATE ", "ALTER ");
                SqlTrg = this.EuDsgvoScriptForObject(Trigger, SqlTrg);
                if (SqlTrg.Length > 0)
                {
                    if (Script.Length > 0)
                        Script += "\r\n\r\n";
                    Script += SqlTrg;
                }
            }
            if (Script.Length > 0)
            {
                Script = "--#####################################################################################################################\r\n" +
                    "--####  TRIGGER  ######################################################################################################\r\n" +
                    "--#####################################################################################################################\r\n" +
                    "\r\n" + Script + "\r\n\r\n";
            }
            return Script;
        }

        private string EuDsgvoScriptForViews()
        {
            string Script = "";
            string SQL = "select V.TABLE_NAME, V.VIEW_DEFINITION from INFORMATION_SCHEMA.VIEWS V";
            System.Data.DataTable dtViews = new DataTable();
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtViews, ref Message);
            foreach (System.Data.DataRow R in dtViews.Rows)
            {
                try
                {
                    string View = R["VIEW_DEFINITION"].ToString();
                    string SqlView = View.Substring(View.IndexOf("CREATE VIEW ")).Replace("CREATE VIEW ", "ALTER VIEW ");
                    SqlView = this.EuDsgvoScriptForObject(R["TABLE_NAME"].ToString(), SqlView);// SqlView.Replace(" = current_user", " = dbo.UserID()");
                    if (SqlView.Length > 0)
                    {
                        if (Script.Length > 0)
                            Script += "\r\n\r\n";
                        Script += SqlView;
                    }
                }
                catch (System.Exception ex) { }
            }
            if (Script.Length > 0)
            {
                Script = "--#####################################################################################################################\r\n" +
                    "--####  VIEWS  ########################################################################################################\r\n" +
                    "--#####################################################################################################################\r\n" +
                    "\r\n" + Script + "\r\n\r\n";
            }
            return Script;
        }


        private string EuDsgvoScriptForFunctions()
        {
            string Script = "";
            string SQL = "select R.ROUTINE_NAME, R.ROUTINE_DEFINITION, * from INFORMATION_SCHEMA.ROUTINES R where R.ROUTINE_TYPE = 'FUNCTION'";
            System.Data.DataTable dt = new DataTable();
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                string RoutineName = R["ROUTINE_NAME"].ToString();
                if (RoutineName == "UserID")
                    continue;
                string Routine = R["ROUTINE_DEFINITION"].ToString().Replace("  ", " ");
                int Start = Routine.ToUpper().IndexOf("CREATE FUNCTION ");
                string SqlRoutine = Routine.Substring(Start);
                SqlRoutine = "ALTER " + SqlRoutine.Substring(7);// .Replace("CREATE FUNCTION ", "ALTER FUNCTION ");
                SqlRoutine = this.EuDsgvoScriptForObject(RoutineName, SqlRoutine);
                if (SqlRoutine.Length > 0)
                {
                    if (Script.Length > 0)
                        Script += "\r\n\r\n";
                    Script += SqlRoutine;
                }
            }
            if (Script.Length > 0)
            {
                Script = "--#####################################################################################################################\r\n" +
                    "--####  FUNCTIONS  ####################################################################################################\r\n" +
                    "--#####################################################################################################################\r\n" +
                    "\r\n" + Script + "\r\n\r\n";
            }
            return Script;
        }

        private string EuDsgvoScriptForProcedures()
        {
            string Script = "";
            string SQL = "select R.ROUTINE_NAME, R.ROUTINE_DEFINITION, * from INFORMATION_SCHEMA.ROUTINES R where R.ROUTINE_TYPE = 'PROCEDURE'";
            System.Data.DataTable dt = new DataTable();
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                try
                {
                    string RoutineName = R["ROUTINE_NAME"].ToString();
                    if (RoutineName.StartsWith("sp_"))
                        continue;
                    string Routine = R["ROUTINE_DEFINITION"].ToString().Replace("  ", " ");
                    int Start = Routine.ToLower().IndexOf("create procedure ");
                    if (Start == -1)
                        Start = Routine.ToLower().IndexOf("create proc ");
                    string SqlRoutine = Routine.Substring(Start);
                    SqlRoutine = "ALTER " + SqlRoutine.Substring(7);
                    SqlRoutine = this.EuDsgvoScriptForObject(RoutineName, SqlRoutine);
                    if (SqlRoutine.Length > 0)
                    {
                        if (Script.Length > 0)
                            Script += "\r\n\r\n";
                        Script += SqlRoutine;
                    }
                }
                catch (System.Exception ex) { }
            }
            if (Script.Length > 0)
            {
                Script = "--#####################################################################################################################\r\n" +
                    "--####  PROCEDURES  ###################################################################################################\r\n" +
                    "--#####################################################################################################################\r\n" +
                    "\r\n" + Script + "\r\n\r\n";
            }
            return Script;
        }
        #endregion

        #region Remove user name

        private string _EuDsgvoRemoveUser = "";
        private System.Data.DataTable _dtEuDsgvoRemoveUser;
        private DiversityWorkbench.Data.Table _TableEuDsgvoUserProxy;
        private DiversityWorkbench.Data.Table TableEuDsgvoUserProxy()
        {
            if (this._TableEuDsgvoUserProxy == null)
            {
                this._TableEuDsgvoUserProxy = new Data.Table("UserProxy", DiversityWorkbench.Settings.ConnectionString);
            }
            return this._TableEuDsgvoUserProxy;
        }

        private void comboBoxEuDsgvoRemoveUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Data.DataRow R = this._dtEuDsgvoRemoveUser.Rows[this.comboBoxEuDsgvoRemoveUser.SelectedIndex];
            this.textBoxEuDsgvoRemoveUserInfo.Text = "Information about the user that will be removed:\r\n\r\n";
            string NotRemoved = "";
            foreach (System.Data.DataColumn C in this._dtEuDsgvoRemoveUser.Columns)
            {
                if (C.ColumnName == "LoginName")
                {
                    this._EuDsgvoRemoveUser = R[C.ColumnName].ToString();
                    this.buttonEuDsgvoRemoveUser.Text = "Remove all informations about the user " + this._EuDsgvoRemoveUser + " from the database";
                }
                if (R[C.ColumnName].Equals(System.DBNull.Value) || R[C.ColumnName].ToString().Length == 0)
                    continue;
                if (C.ColumnName == "ID" || C.ColumnName == "RowGUID")
                {
                    if (NotRemoved.Length > 0) NotRemoved += "\r\n";
                    NotRemoved += "\t" + C.ColumnName + ": " + R[C.ColumnName].ToString();
                }
                else
                {
                    this.textBoxEuDsgvoRemoveUserInfo.Text += C.ColumnName + ":\r\n";
                    this.textBoxEuDsgvoRemoveUserInfo.Text += R[C.ColumnName].ToString() + "\r\n\r\n";
                }
            }
            this.textBoxEuDsgvoRemoveUserInfo.Text = "Not removed informations:\r\n" + NotRemoved + "\r\n\r\n" + this.textBoxEuDsgvoRemoveUserInfo.Text;
        }

        private void comboBoxEuDsgvoRemoveUser_DropDown(object sender, EventArgs e)
        {
            if (this._dtEuDsgvoRemoveUser == null)
            {
                this._dtEuDsgvoRemoveUser = new DataTable();
                string SQL = "SELECT [LoginName] + case when [CombinedNameCache] <> '' and [CombinedNameCache] <> [LoginName]  " +
                    "then ' (= ' + [CombinedNameCache] + ')' else '' end AS [User], * " +
                    "FROM [dbo].[UserProxy] " +
                    "WHERE [LoginName] <> '' " +
                    "ORDER BY [LoginName]";
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._dtEuDsgvoRemoveUser, ref Message);
                this.comboBoxEuDsgvoRemoveUser.DataSource = this._dtEuDsgvoRemoveUser;
                this.comboBoxEuDsgvoRemoveUser.DisplayMember = "User";
                this.comboBoxEuDsgvoRemoveUser.ValueMember = "User";
            }
        }

        private void buttonEuDsgvoRemoveUser_Click(object sender, EventArgs e)
        {
            if (this._EuDsgvoRemoveUser.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            try
            {
                string SQL = "";
                System.Data.DataRow R = this._dtEuDsgvoRemoveUser.Rows[this.comboBoxEuDsgvoRemoveUser.SelectedIndex];
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Column> C in this.TableEuDsgvoUserProxy().Columns)
                {
                    if (C.Key == "ID")
                        continue;
                    if (C.Key == "RowGUID")
                        continue;
                    if (SQL.Length > 0)
                        SQL += ", ";
                    if (C.Value.IsNullable)
                        SQL += C.Key + " = NULL ";
                    else
                        SQL += C.Key + " = '' ";
                }
                SQL = "UPDATE U SET " + SQL + " FROM UserProxy AS U " +
                    "WHERE LoginName = '" + this._EuDsgvoRemoveUser + "'";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    System.Windows.Forms.MessageBox.Show("Informations removed");
                else
                    System.Windows.Forms.MessageBox.Show("Removal failed");
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Info site

        //private string _PrivacyConsentInfoRoutine = "PrivacyConsentInfo";

        private void buttonEuDsgvoInfoURLSetValue_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new FormWebBrowser(this.linkLabelEuDsgvoInfoURL.Text);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (DiversityWorkbench.Database.CreateOrUpdatePrivacyConsentProcedure(f.URL))
                    this.linkLabelEuDsgvoInfoURL.Text = f.URL;
            }
        }

        private void linkLabelEuDsgvoInfoURL_TextChanged(object sender, EventArgs e)
        {
            this.userControlWebViewEuDsgvoInfoURL.Url = null;
            this.userControlWebViewEuDsgvoInfoURL.Navigate(this.linkLabelEuDsgvoInfoURL.Text);
        }

        #endregion

        #endregion

        #region Save log

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterLogDatabase;

        private void buttonSaveLogListTables_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.LogDatabase.Database.InitLogging();
            this.dataGridViewSaveLog.DataSource = DiversityWorkbench.LogDatabase.Database.dtTableCounts(this.dateTimePickerSaveLogStartDate.Value);
            this.dataGridViewSaveLog.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            System.Data.DataRow[] RR = ((System.Data.DataTable)this.dataGridViewSaveLog.DataSource).Select("Transferred > 0");
            if (RR.Length > 0)
                this.buttonSaveLog.Enabled = true;
            else
                this.buttonSaveLog.Enabled = false;
        }

        private void buttonSaveLog_Click(object sender, EventArgs e)
        {
            // Create Schema for the current logs
            // Write log tables into Schema
            // Cut log from productive database
            string Message = "";
            bool OK = DiversityWorkbench.LogDatabase.Database.TransferData(ref Message);//this.dateTimePickerSaveLogStartDate.Value);
            if (!OK)
            {
                string Error = "Transfer failed";
                if (Message.Length > 0)
                    Error += ":\r\n" + Message;
                System.Windows.Forms.MessageBox.Show(Error);
            }
        }

        private void buttonSaveLogUserAdministration_Click(object sender, EventArgs e)
        {
            if (!DiversityWorkbench.LogDatabase.Database.Exists())
            {
                DiversityWorkbench.LogDatabase.Database.InitLogging();
                return;
            }
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.LogDatabase.Database.ConnectionStringLogDB);
            DiversityWorkbench.Forms.FormLoginAdministration f = new FormLoginAdministration(con);
            f.ShowDialog();
        }

        private void dateTimePickerSaveLogStartDate_ValueChanged(object sender, EventArgs e)
        {
            this.buttonSaveLog.Enabled = false;
            this.dataGridViewSaveLog.DataSource = null;
        }

        private void buttonSaveLogCopyUser_Click(object sender, EventArgs e)
        {
            if (!DiversityWorkbench.LogDatabase.Database.Exists())
            {
                DiversityWorkbench.LogDatabase.Database.InitLogging();
                return;
            }
            string Message = "";
            bool OK = DiversityWorkbench.LogDatabase.Database.CopyLogins(ref Message);
            if (!OK && Message.Length > 0)
                System.Windows.Forms.MessageBox.Show("Failed:\r\n" + Message);
            else
                System.Windows.Forms.MessageBox.Show("User copied into log database");
        }

        private void initSaveLog()
        {
            try
            {
                if (DiversityWorkbench.LogDatabase.Database.Exists())
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DateTime> KV in DiversityWorkbench.LogDatabase.Database.Schemata())
                    {

                        // the tabpage for the schema
                        System.Windows.Forms.TabPage TP = new TabPage(KV.Key);
                        this.tabControlSaveLog.TabPages.Add(TP);

                        // the split container
                        System.Windows.Forms.SplitContainer SC = new SplitContainer();
                        SC.Dock = DockStyle.Fill;
                        TP.Controls.Add(SC);

                        // the table for the table names
                        System.Data.DataTable dtTables = new DataTable(KV.Key);

                        // the button for listing the tables
                        System.Windows.Forms.Button B = new Button();
                        B.Text = "List tables";
                        B.Dock = DockStyle.Top;
                        B.Tag = dtTables;
                        B.Click += new System.EventHandler(this.SaveLogButton_Click);
                        SC.Panel1.Controls.Add(B);

                        // The datagrid for the content of a table
                        System.Windows.Forms.DataGridView DGV = new DataGridView();
                        DGV.Dock = DockStyle.Fill;
                        DGV.ReadOnly = true;
                        DGV.AllowUserToAddRows = false;
                        DGV.AllowUserToDeleteRows = false;
                        SC.Panel2.Controls.Add(DGV);

                        // the list containing  the tables
                        System.Windows.Forms.ListBox LB = new ListBox();
                        LB.DataSource = dtTables;
                        LB.DisplayMember = "TABLE_NAME";
                        LB.ValueMember = "TABLE_NAME";
                        LB.SelectedIndexChanged += new System.EventHandler(SaveLogListBox_SelectedIndexChanged);
                        SC.Panel1.Controls.Add(LB);
                        LB.Dock = DockStyle.Fill;
                        LB.BringToFront();
                        LB.Tag = DGV;

                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SaveLogButton_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                System.Data.DataTable dt = (System.Data.DataTable)B.Tag;
                string SQL = "select t.TABLE_NAME from [" + DiversityWorkbench.Settings.DatabaseName + "_log].INFORMATION_SCHEMA.TABLES t " +
                    "where t.TABLE_SCHEMA = '" + dt.TableName + "'";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);

            }
            catch (System.Exception ex)
            {
            }
        }

        private void SaveLogListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ListBox LB = (System.Windows.Forms.ListBox)sender;
                System.Windows.Forms.DataGridView DGV = (System.Windows.Forms.DataGridView)LB.Tag;
                System.Data.DataTable dt = (System.Data.DataTable)LB.DataSource;
                System.Data.DataRowView R = (System.Data.DataRowView)LB.SelectedItem;

                string SQL = "select * from [" + DiversityWorkbench.Settings.DatabaseName + "_log].[" + dt.TableName + "].[" + R[0].ToString() + "]";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                System.Data.DataTable dtContent = new DataTable();
                ad.Fill(dtContent);
                DGV.DataSource = dtContent;
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Description Cache

        private void initDescriptionCache(string ConnectionString = "")
        {
            if (ConnectionString.Length == 0)
                ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString))
            {
                con.Open();
                string SQL = "select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'procFillCacheDescription'";
                int i;
                this.buttonFillDescriptionCache.Enabled = int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, con), out i) && i == 1;
                this.buttonViewDescriptionCache.Enabled = int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, con), out i) && i == 1;
                con.Close();
            }
        }

        private void buttonFillDescriptionCache_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    con.Open();
                    string SQL = "exec dbo.procFillCacheDescription";
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, con);
                    con.Close();
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonViewDescriptionCache_Click(object sender, EventArgs e)
        {
            try
            {
                using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    con.Open();
                    string SQL = "SELECT Type, TableName, ColumnName, LanguageCode, Context, DisplayText, Abbreviation, Description " +
                        "FROM CacheDescription " +
                        "WHERE Type IN ('Column', 'Table') " +
                        "ORDER BY TableName, ColumnName, Type";
                    System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL, _ConnectionString, "CacheDescription");
                    SQL = "SELECT Type, TableName, ColumnName, LanguageCode, Context, DisplayText, Abbreviation, Description " +
                        "FROM CacheDescription " +
                        "WHERE Type IN ('VIEW') " +
                        "ORDER BY TableName, ColumnName, Type";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, con);
                    SQL = "SELECT Type, TableName, ColumnName, LanguageCode, Context, DisplayText, Abbreviation, Description " +
                        "FROM CacheDescription " +
                        "WHERE Type IN ('FUNCTION') " +
                        "ORDER BY TableName, ColumnName, Type";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, con);
                    SQL = "SELECT Type, TableName, ColumnName, LanguageCode, Context, DisplayText, Abbreviation, Description " +
                        "FROM CacheDescription " +
                        "WHERE Type IN ('PROCEDURE') " +
                        "ORDER BY TableName, ColumnName, Type";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, con);

                    DiversityWorkbench.Forms.FormTableContent f = new FormTableContent("CacheDescription", "Content of table CacheDescription", dt);
                    f.Width = this.Width - 10;
                    f.Height = this.Height - 10;
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.ShowDialog();
                    con.Close();
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

    }
}
