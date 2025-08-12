using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlReplicationPreparation : UserControl
    {
        public enum RepPrepSteps { CreateRepPubTable, AddRowGUID, CreateDefaultRowGUID, CreateTempTab, ReadData, DeactivateTrigger, WriteGUID, WriteLogDate, ActivateTrigger, DeleteTempTab };
        //public enum RepPrepScriptSteps { CreateRepPubTable, CreateLogTable, AddLogColumns, CreateLogDefaults, AddRowGUID, CreateRowGUIDDefault, CreateTempTab, ReadData, DeleteTrigger, WriteGUID, WriteLogDate, CreateTrigger, DeleteTempTab };

        public enum RepPrepStepState { NotNecessary, ToDo, Done };
        private RepPrepStepState _RepPerpStepState = RepPrepStepState.ToDo;

        private RepPrepSteps _RepPrepStep;

        public UserControlReplicationPreparation()
        {
            InitializeComponent();
        }

        private string _TableName;

        public void Reset()
        {
            this._RepPerpStepState = RepPrepStepState.ToDo;
        }

        private string _ErrorMessage;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
                this.textBoxErrorMessage.Text = value;
            }
        }

        public bool PreconditionFulfilled(ref string Message)
        {
            bool OK = false;
            string SQL = "";
            string Count = "";
            Replication.TableName = this._TableName;
            if (!this._TableName.EndsWith("_Enum"))
            {
                Count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Replication.logColumnsExist())
                    OK = true;
                else
                {
                    OK = false;
                    Message = "Columns for logging the time of creation and update of the dataset are missing";
                }
                if (Message.Length == 0)
                {
                    if (Replication.logTableExists())
                        OK = true;
                    else
                    {
                        OK = false;
                        Message = "Logging table is missing";
                    }
                }
            }
            return OK;
        }

        public void setState(RepPrepStepState State)
        {
            this._RepPerpStepState = State;
            switch (State)
            {
                case RepPrepStepState.NotNecessary:
                    this.button.Text = "-";
                    this.toolTip.SetToolTip(this.button, "Not necessary");
                    this.button.ForeColor = System.Drawing.Color.Black;
                    break;
                case RepPrepStepState.ToDo:
                    this.button.Text = "To Do";
                    this.toolTip.SetToolTip(this.button, "Needs to be done");
                    this.button.ForeColor = System.Drawing.Color.Red;
                    break;
                case RepPrepStepState.Done:
                    this.button.Text = "Done";
                    this.toolTip.SetToolTip(this.button, "Done");
                    this.button.ForeColor = System.Drawing.Color.Green;
                    break;
            }
        }

        public RepPrepStepState getState()
        {
            return this._RepPerpStepState;
        }

        public void SetTableName(string TableName)
        {
            this._TableName = TableName;
            Replication.TableName = TableName;
            string SQL = "";
            string Count = "";
            //this._dtPK = null;
            //this._dtColumns = null;
            switch (this._RepPrepStep)
            {
                case RepPrepSteps.CreateRepPubTable:
                    SQL = "select COUNT(*) from INFORMATION_SCHEMA.TABLES T " +
                        "where T.TABLE_NAME = 'ReplicationPublisher' ";
                    Count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Count == "1") this._RepPerpStepState = RepPrepStepState.Done;
                    else this._RepPerpStepState = RepPrepStepState.ToDo;
                    break;
                case RepPrepSteps.AddRowGUID:
                    System.Data.DataRow[] rrGuid = Replication.DtColumns.Select("COLUMN_NAME = 'RowGUID'");
                    if (rrGuid.Length == 1) this._RepPerpStepState = RepPrepStepState.Done;
                    else this._RepPerpStepState = RepPrepStepState.ToDo;
                    break;
                case RepPrepSteps.CreateDefaultRowGUID:
                    System.Data.DataRow[] rrDef = Replication.DtColumns.Select("COLUMN_NAME = 'RowGUID' AND NOT COLUMN_DEFAULT IS NULL ");
                    if (rrDef.Length == 1) this._RepPerpStepState = RepPrepStepState.Done;
                    else this._RepPerpStepState = RepPrepStepState.ToDo;
                    break;
                case RepPrepSteps.ReadData:
                    SQL = "select COUNT(*) from [" + this._TableName + "] T " +
                        "where T.RowGUID IS NULL";
                    Count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Count == "0") this._RepPerpStepState = RepPrepStepState.NotNecessary;
                    else this._RepPerpStepState = RepPrepStepState.ToDo;
                    break;
                case RepPrepSteps.WriteLogDate:
                    SQL = "select COUNT(*) from [" + this._TableName + "] T " +
                        "where T.logUpdatedWhen IS NULL ";
                    Count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Count == "0") this._RepPerpStepState = RepPrepStepState.NotNecessary;
                    else this._RepPerpStepState = RepPrepStepState.ToDo;
                    System.Data.DataRow[] rrLog = Replication.DtColumns.Select("COLUMN_NAME = 'logUpdatedWhen' AND NOT COLUMN_DEFAULT IS NULL ");
                    if (rrLog.Length == 1) this.ErrorMessage = "Default for column logUpdatedWhen is missing";
                    break;
            }
            this.setState(this._RepPerpStepState);
        }

        public void InitControl(RepPrepSteps Step)
        {
            this._RepPrepStep = Step;
            switch (Step)
            {
                case RepPrepSteps.CreateRepPubTable:
                    this.label.Text = "Create table for replication publishers";
                    break;
                case RepPrepSteps.ActivateTrigger:
                    this.label.Text = "Activate update trigger";
                    break;
                case RepPrepSteps.AddRowGUID:
                    this.label.Text = "Add Row GUID column";
                    break;
                case RepPrepSteps.CreateDefaultRowGUID:
                    this.label.Text = "Create default for Row GUID";
                    break;
                case RepPrepSteps.CreateTempTab:
                    this.label.Text = "Create temporary table";
                    break;
                case RepPrepSteps.DeactivateTrigger:
                    this.label.Text = "Deactivate update trigger";
                    break;
                case RepPrepSteps.DeleteTempTab:
                    this.label.Text = "Delete the temporary table";
                    break;
                case RepPrepSteps.ReadData:
                    this.label.Text = "Read data into temporary table";
                    break;
                case RepPrepSteps.WriteLogDate:
                    this.label.Text = "Write date into logging columns";
                    break;
                case RepPrepSteps.WriteGUID:
                    this.label.Text = "Write Row GUID from temporary table into table and log table";
                    break;
            }
            this.Reset();
        }

        public bool PrepareReplication()
        {
            bool OK = true;
            string SQL = "";
            switch (this._RepPrepStep)
            {
                case RepPrepSteps.CreateRepPubTable:
                    SQL = Replication.SqlReplicationPublisherTableDefinition();
                    break;
                case RepPrepSteps.AddRowGUID:
                    SQL = "ALTER TABLE [" + this._TableName + "] ADD [RowGUID] [uniqueidentifier] NULL; ALTER TABLE [" + this._TableName + "_log] ADD [RowGUID] [uniqueidentifier] NULL;";
                    break;
                case RepPrepSteps.CreateDefaultRowGUID:
                    SQL = "ALTER TABLE [dbo].[" + this._TableName + "] ADD DEFAULT (newsequentialid()) FOR [RowGUID];";
                    break;
                case RepPrepSteps.ActivateTrigger:
                    SQL = "ENABLE TRIGGER dbo.trgUpd" + this._TableName + " ON dbo.[" + this._TableName + "]";
                    break;
                case RepPrepSteps.CreateTempTab:
                    SQL = Replication.SqlTempRowGUIDTable();
                    break;
                case RepPrepSteps.DeactivateTrigger:
                    SQL = "DISABLE TRIGGER dbo.trgUpd" + this._TableName + " ON dbo.[" + this._TableName + "]";
                    break;
                case RepPrepSteps.DeleteTempTab:
                    SQL = "DROP TABLE [dbo].[" + this._TableName + "_RowGUID]";
                    break;
                case RepPrepSteps.ReadData:
                    SQL = Replication.SqlTransferDataInTempRowGUIDTable();
                    break;
                case RepPrepSteps.WriteLogDate:
                    SQL = Replication.SqlWriteLogDate();
                    break;
                case RepPrepSteps.WriteGUID:
                    SQL = Replication.SqlWriteRowGUID();
                    SQL += Replication.SqlWriteRowGUIDToLogTable();
                    break;
            }
            string Message = "";
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
            this.ErrorMessage = Message;
            return OK;
        }

        //private string SqlReplicationPublisherTableDefinition()
        //{
        //    string SQL = "CREATE TABLE [dbo].[ReplicationPublisher](" +
        //    "[DatabaseName] [varchar](255) NOT NULL, " +
        //    "[Server] [varchar](255) NOT NULL, " +
        //    "[Port] [smallint] NULL, " +
        //    "[LogCreatedWhen] [datetime] NULL, " +
        //    "[LogCreatedBy] [nvarchar](50) NULL, " +
        //    "[LogUpdatedWhen] [datetime] NULL, " +
        //    "[LogUpdatedBy] [nvarchar](50) NULL, " +
        //    "CONSTRAINT [PK_ReplicationPublisher] PRIMARY KEY CLUSTERED  " +
        //    "(	[DatabaseName] ASC, " +
        //    "[Server] ASC " +
        //    ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
        //    ") ON [PRIMARY] " +
        //    "GO " +
        //    "SET ANSI_PADDING OFF " +
        //    "GO  " +
        //    "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the publishing database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'DatabaseName' " +
        //    "GO " +
        //    "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name or address of the server where the publishing database is located' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Server' " +
        //    "GO " +
        //    "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The port used by the server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Port' " +
        //    "GO " +
        //    "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'" +
        //    "GO " +
        //    "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'" +
        //    "GO " +
        //    "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'" +
        //    "GO " +
        //    "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'" +
        //    "GO " +
        //    "ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]" +
        //    "GO " +
        //    "ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]" +
        //    "GO " +
        //    "ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]" +
        //    "GO " +
        //    "ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]" +
        //    "GO ";
        //    string SqlAdmin = "select MIN(p.name) from sys.database_principals p " +
        //        "where p.type = 'R' " +
        //        "and p.name not like 'db[_]%' " +
        //        "and p.name like '%admin%'";
        //    string AdminGroup = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlAdmin);
        //    SQL += "GRANT DELETE ON dbo.ReplicationPublisher TO " + AdminGroup +
        //        "GO " +
        //        "GRANT INSERT ON dbo.ReplicationPublisher TO " + AdminGroup +
        //        "GO " +
        //        "GRANT UPDATE ON dbo.ReplicationPublisher TO" + AdminGroup +
        //        "GO " +
        //        "GRANT SELECT ON dbo.ReplicationPublisher TO" + AdminGroup +
        //        "GO ";
        //    return SQL;
        //}

        //private string SqlTempTable()
        //{
        //    string SQL = "";
        //    foreach(System.Data.DataRow R in this.DtPK.Rows)
        //    {
        //        SQL += "[" + R["COLUMN_NAME"].ToString() + "] [" + R["DATA_TYPE"].ToString() + "]";
        //        if (R["DATA_TYPE"].ToString().EndsWith("char"))
        //            SQL += "(" + R["NUMERIC_PRECISION"].ToString() + ") NOT NULL ";
        //        SQL += ", ";
        //    }
        //    SQL = "CREATE TABLE [dbo].[" + this._TableName + "_RowGUID](" + 
        //        SQL +
        //        " [RowGUID] [uniqueidentifier] NULL, " +
        //        "CONSTRAINT [PK_" + this._TableName + "_RowGUID] PRIMARY KEY CLUSTERED " +
        //        "(";
        //    for (int i = 0; i < this.DtPK.Rows.Count; i++)
        //    {
        //        if (i > 0) SQL += ", ";
        //        SQL += "[" + this.DtPK.Rows[i]["COLUMN_NAME"].ToString() + "] ASC ";
        //    }
        //    SQL += ") WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] " +
        //        ") ON [PRIMARY]; " +
        //        "ALTER TABLE [dbo].[" + this._TableName + "_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; ";
        //    return SQL;
        //}

        //private string SqlTransferData()
        //{
        //    string SqlColumns = "";
        //    foreach(System.Data.DataRow R in this.DtPK.Rows)
        //    {
        //        if (SqlColumns.Length > 0) SqlColumns += ", ";
        //        SqlColumns += "[" + R["COLUMN_NAME"].ToString() + "]";
        //    }
        //    string SQL = "INSERT INTO [" + this._TableName + "_RowGUID] (" +
        //        SqlColumns + ") " +
        //        "SELECT " +
        //        SqlColumns +
        //        " FROM [" + this._TableName + "] " +
        //        " WHERE [RowGUID] IS NULL ";
        //    return SQL;
        //}

        //private string SqlWriteLogDate()
        //{
        //    string SQL = "";
        //    System.Data.DataRow[] rrLog = this.DtColumns.Select("COLUMN_NAME = 'logupdatedWhen' OR COLUMN_NAME = 'loginsertedWhen' OR COLUMN_NAME = 'logcreatedWhen'");
        //    foreach (System.Data.DataRow R in rrLog)
        //    {
        //        SQL += "UPDATE P SET P." + R["COLUMN_NAME"].ToString() + " = '1900-01-01' " +
        //            "FROM [" + this._TableName + "] P " +
        //            "WHERE P." + R["COLUMN_NAME"].ToString() + " IS NULL; ";
        //        SQL += "UPDATE P SET P." + R["COLUMN_NAME"].ToString() + " = '1900-01-01' " +
        //            "FROM [" + this._TableName + "_log] P " +
        //            "WHERE P." + R["COLUMN_NAME"].ToString() + " IS NULL; ";
        //    }
        //    return SQL;
        //}

        //private string SqlWriteRowGUID()
        //{
        //    string SQL = "";
        //    foreach (System.Data.DataRow R in this.DtPK.Rows)
        //    {
        //        if (SQL.Length > 0) SQL += " AND ";
        //        SQL +=  "T.[" + R["COLUMN_NAME"].ToString() + "] = R.[" + R["COLUMN_NAME"].ToString() + "]";
        //    }
        //    SQL = "UPDATE T SET [RowGUID] = R.[RowGUID] " +
        //        "FROM [" + this._TableName + "] AS T  INNER JOIN [" + this._TableName + "_RowGUID] AS R ON " +
        //        SQL + ";  UPDATE T SET [RowGUID] = R.[RowGUID] " +
        //        "FROM [" + this._TableName + "_log] AS T INNER JOIN [" + this._TableName + "_RowGUID] AS R ON " +
        //        SQL + "; ";
        //    return SQL;
        //}

        //private System.Data.DataTable _dtPK;

        //public System.Data.DataTable DtPK
        //{
        //    get 
        //    {
        //        if (this._dtPK == null)
        //        {
        //            this._dtPK = new DataTable();
        //            string SqlPK = "SELECT COLUMN_NAME, C.DATA_TYPE, C.NUMERIC_PRECISION, C.DATETIME_PRECISION " +
        //                "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
        //                "WHERE (TABLE_NAME = '" + this._TableName + "') AND (EXISTS " +
        //                "(SELECT * " +
        //                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
        //                "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
        //                "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
        //            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlPK, DiversityWorkbench.Settings.ConnectionString);
        //            ad.Fill(_dtPK);
        //        }
        //        return _dtPK; 
        //    }
        //}

        //private System.Data.DataTable _dtColumns;

        //public System.Data.DataTable DtColumns
        //{
        //    get
        //    {
        //        if (this._dtColumns == null)
        //        {
        //            this._dtColumns = new DataTable();
        //            string SqlPK = "SELECT C.COLUMN_NAME, C.COLUMN_DEFAULT " +
        //                "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
        //                "WHERE TABLE_NAME = '" + this._TableName + "'";
        //            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlPK, DiversityWorkbench.Settings.ConnectionString);
        //            ad.Fill(_dtColumns);
        //        }
        //        return _dtColumns;
        //    }
        //}

    }
}
