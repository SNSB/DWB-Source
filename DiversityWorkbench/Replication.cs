using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{

    public struct ReplicationFilter
    {
        public string Operator;
        public string Value;
        public string SQL;
        public ReplicationFilter(string Operator = "", string Value = "", string SQL = "")
        {
            this.Operator = Operator;
            this.Value = Value;
            this.SQL = SQL;
        }
    }

    public interface ReplicationForm
    {
        void PropagateFilter(string TableName, System.Collections.Generic.Dictionary<string, ReplicationFilter> Filters);
    }


    public class Replication
    {
        #region Parameter

        public enum ReplicationDirection { Download, Upload, Clean, Merge }

        #endregion

        #region Construction
        
        public Replication()
        {
        }
        
        #endregion

        #region Menu preparation (Todo - dazu muss ein allgemeines Formular geschaffen werden)

        private System.Data.DataTable _DtReplicationPublisher;

        public void initReplicationMenu(bool IsAdmin,
            System.Windows.Forms.ToolStripMenuItem replicationToolStripMenuItem,
            System.Windows.Forms.ToolStripMenuItem addPublisherToolStripMenuItem,
            System.Windows.Forms.ToolStripMenuItem cleanDatabaseToolStripMenuItem)
        {
            addPublisherToolStripMenuItem.Visible = true;
            cleanDatabaseToolStripMenuItem.Visible = true;

            replicationToolStripMenuItem.DropDownItems.Clear();
            if (IsAdmin)
            {
                replicationToolStripMenuItem.DropDownItems.Add(addPublisherToolStripMenuItem);
                replicationToolStripMenuItem.DropDownItems.Add(cleanDatabaseToolStripMenuItem);
            }

            string SQL = "";
            if (this._DtReplicationPublisher == null)
                this._DtReplicationPublisher = new System.Data.DataTable();
            else
                this._DtReplicationPublisher.Clear();

            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    // Import
                    SQL = "SELECT DatabaseName, Server, Port FROM ReplicationPublisher";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._DtReplicationPublisher);
                    if (this._DtReplicationPublisher.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in this._DtReplicationPublisher.Rows)
                        {
                            //string DisplayText = R["DatabaseName"].ToString() + " on " + R["Server"].ToString();
                            //System.Windows.Forms.ToolStripMenuItem MI = new System.Windows.Forms.ToolStripMenuItem(DisplayText, DiversityWorkbench.Properties.Resources.Synchronize);
                            //replicationToolStripMenuItem.DropDownItems.Add(MI);
                            //System.Windows.Forms.ToolStripMenuItem MiDownload = new System.Windows.Forms.ToolStripMenuItem("Download ...", DiversityWorkbench.Properties.Resources.Download, this.downloadToolStripMenuItem_Click);
                            //MiDownload.Tag = R;
                            //MiDownload.ToolTipText = "Download data from the replication publisher";
                            //MI.DropDownItems.Add(MiDownload);
                            //System.Windows.Forms.ToolStripMenuItem MiMerge = new System.Windows.Forms.ToolStripMenuItem("Merge ...", DiversityWorkbench.Properties.Resources.Merge, this.mergeToolStripMenuItem_Click);
                            //MiMerge.Tag = R;
                            //MiMerge.ToolTipText = "Merge data for the current project between the replication publisher and your local database";
                            //MI.DropDownItems.Add(MiMerge);
                            //System.Windows.Forms.ToolStripMenuItem MiUpload = new System.Windows.Forms.ToolStripMenuItem("Upload ...", DiversityWorkbench.Properties.Resources.Upload, this.uploadToolStripMenuItem_Click);
                            //MiUpload.Tag = R;
                            //MiUpload.ToolTipText = "Upload the data of the current project to the replication publisher";
                            //MI.DropDownItems.Add(MiUpload);
                            //if (IsAdmin)
                            //{
                            //    System.Windows.Forms.ToolStripMenuItem MiDelete = new System.Windows.Forms.ToolStripMenuItem("Remove", DiversityWorkbench.Properties.Resources.NoDatabase, this.deleteReplicationPublisherToolStripMenuItem_Click);
                            //    MiDelete.Tag = R;
                            //    MiDelete.ToolTipText = "Remove the replication publisher from the list";
                            //    MI.DropDownItems.Add(MiDelete);
                            //}
                        }
                        replicationToolStripMenuItem.Visible = true;
                        addPublisherToolStripMenuItem.Visible = true;
                        cleanDatabaseToolStripMenuItem.Visible = true;
                    }
                    replicationToolStripMenuItem.DropDownItems.Add(cleanDatabaseToolStripMenuItem);
                }
            }
            catch (Exception ex)
            {
                replicationToolStripMenuItem.Visible = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        //    //return;
        //    System.Windows.Forms.ToolStripMenuItem T = (System.Windows.Forms.ToolStripMenuItem)sender;
        //    System.Data.DataRow R = (System.Data.DataRow)T.Tag;
        //    try
        //    {
        //        DiversityTaxonNames.Forms.FormReplication f = new DiversityTaxonNames.Forms.FormReplication(DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Download, null, this.ReplicationPublisherConnectionString(R));
        //        f.ShowDialog();
        //    }
        //    catch (System.Exception ex) { }
        //}

        //private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (!this.userControlQueryList.ProjectIsSelected)
        //    {
        //        System.Windows.Forms.MessageBox.Show("Please select a project");
        //        return;
        //    }
        //    System.Windows.Forms.ToolStripMenuItem T = (System.Windows.Forms.ToolStripMenuItem)sender;
        //    System.Data.DataRow R = (System.Data.DataRow)T.Tag;
        //    if (!ProjectForUploadIsAvailableInPublisher(this.ReplicationPublisherConnectionString(R)))
        //    {
        //        System.Windows.Forms.MessageBox.Show("Either you have no access to the publishing database or the current project does not exist there");
        //        return;
        //    }
        //    try
        //    {
        //        DiversityTaxonNames.Forms.FormReplication f = new DiversityTaxonNames.Forms.FormReplication(DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Upload, this.userControlQueryList.ProjectID, this.ReplicationPublisherConnectionString(R));
        //        f.ShowDialog();
        //    }
        //    catch (System.Exception ex) { }
        //}

        //private void cleanDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DiversityTaxonNames.Forms.FormReplication f = new DiversityTaxonNames.Forms.FormReplication(DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Clean, null, "");
        //        f.ShowDialog();
        //    }
        //    catch (System.Exception ex) { }
        //}

        //private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (!this.userControlQueryList.ProjectIsSelected)
        //    {
        //        System.Windows.Forms.MessageBox.Show("Please select a project");
        //        return;
        //    }
        //    System.Windows.Forms.ToolStripMenuItem T = (System.Windows.Forms.ToolStripMenuItem)sender;
        //    System.Data.DataRow R = (System.Data.DataRow)T.Tag;
        //    try
        //    {
        //        DiversityTaxonNames.Forms.FormReplication f = new DiversityTaxonNames.Forms.FormReplication(DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Merge, this.userControlQueryList.ProjectID, this.ReplicationPublisherConnectionString(R));
        //        f.ShowDialog();
        //    }
        //    catch (System.Exception ex) { }
        //}

        //private void deleteReplicationPublisherToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    System.Windows.Forms.ToolStripMenuItem T = (System.Windows.Forms.ToolStripMenuItem)sender;
        //    System.Data.DataRow R = (System.Data.DataRow)T.Tag;
        //    string SQL = "DELETE R " +
        //        "FROM  ReplicationPublisher AS R " +
        //        "WHERE (R.DatabaseName = '" + R["DatabaseName"].ToString() + "') AND (R.Server = '" + R["Server"].ToString() + "')";
        //    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
        //        this.initReplicationMenu(true);
        //    else System.Windows.Forms.MessageBox.Show("Removal of replication publisher failed");
        //}
        
        #endregion

        #region Grants

        public static bool UpdateAndInsertInSubscriberGranted(string TableName)
        {
            bool OK = true;
            if (!DiversityWorkbench.Forms.FormFunctions.Permissions(TableName, "INSERT", UserControls.ReplicationRow.SqlConnectionSubscriber))
                return false;
            if (!DiversityWorkbench.Forms.FormFunctions.Permissions(TableName, "UPDATE", UserControls.ReplicationRow.SqlConnectionSubscriber))
                return false;
            return OK;
        }

        public static bool UpdateAndInsertInPublisherGranted(string TableName)
        {
            bool OK = true;
            if (!DiversityWorkbench.Forms.FormFunctions.Permissions(TableName, "INSERT", UserControls.ReplicationRow.SqlConnectionPublisher))
                return false;
            if (!DiversityWorkbench.Forms.FormFunctions.Permissions(TableName, "UPDATE", UserControls.ReplicationRow.SqlConnectionPublisher))
                return false;
            return OK;
        }
        
        #endregion

        #region Preparation for replication

        private static bool _AttachReplicationColumns;
        public static bool AttachReplicationColumns
        {
            get { return _AttachReplicationColumns; }
            set { _AttachReplicationColumns = value; }
        }

        private static string _ServerVersion = "";
        private static string ServerVersion
        {
            get
            {
                if (Replication._ServerVersion != "") return Replication._ServerVersion;
                string SQL = "SELECT @@VERSION AS 'SQL Server Version'";
                if (DiversityWorkbench.Settings.DatabaseName == "") DiversityWorkbench.Settings.DatabaseName = "master";
                if (DiversityWorkbench.Settings.ConnectionString != "")
                {
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        Replication._ServerVersion = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    }
                    finally { con.Close(); }
                }
                if (Replication._ServerVersion.Length > 0)
                {
                    if (Replication._ServerVersion.IndexOf("Microsoft SQL Server 2005") == 0) Replication._ServerVersion = "2005";
                    else if (Replication._ServerVersion.IndexOf("Microsoft SQL Server  2000") == 0) Replication._ServerVersion = "2000";
                }
                return Replication._ServerVersion;
            }
            set
            {
                Replication._ServerVersion = value;
            }
        }
        
        private static string _TableName;

        public static string TableName
        {
            get { return Replication._TableName; }
            set 
            {
                Replication._TableName = value;
                Replication._TableTriggerList = null;
                Replication._RepPrepScriptTableSteps = null;
                Replication._dtColumns = null;
                Replication._dtPK = null;
            }
        }

        #region alt

        public enum RepPrepScriptTableStep { AddLogColumns, CreateLogDefaults, AddRowGUID, CreateRowGUIDDefault, CreateTempTab, ReadData, DeleteUpdateTrigger, WriteGUID, WriteLogDate, CreateUpdateTrigger, ChangeDeleteTrigger, DeleteTempTab };

        private static System.Collections.Generic.List<RepPrepScriptTableStep> _RepPrepScriptTableTemplateSteps;
        public static System.Collections.Generic.List<RepPrepScriptTableStep> RepPrepScriptTableTemplateSteps
        {
            get
            {
                if (Replication._RepPrepScriptTableTemplateSteps == null)
                {
                    Replication._RepPrepScriptTableTemplateSteps = new List<RepPrepScriptTableStep>();
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.AddLogColumns);
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.CreateLogDefaults);
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.AddRowGUID);
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.CreateRowGUIDDefault);
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.CreateTempTab);
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.ReadData);
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.DeleteUpdateTrigger);
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.WriteGUID);
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.WriteLogDate);
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.CreateUpdateTrigger);
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.ChangeDeleteTrigger);
                    Replication._RepPrepScriptTableTemplateSteps.Add(RepPrepScriptTableStep.DeleteTempTab);
                }
                return _RepPrepScriptTableTemplateSteps;
            }
        }

        private static System.Collections.Generic.List<RepPrepScriptTableStep> _RepPrepScriptTableSteps;

        public static System.Collections.Generic.List<RepPrepScriptTableStep> RepPrepScriptTableSteps
        {
            get
            {
                if (Replication._RepPrepScriptTableSteps == null)
                    Replication._RepPrepScriptTableSteps = new List<RepPrepScriptTableStep>();
                foreach (RepPrepScriptTableStep S in Replication.RepPrepScriptTableTemplateSteps)
                {
                    switch (S)
                    {
                        case RepPrepScriptTableStep.AddLogColumns:
                            break;
                    }
                }
                return Replication._RepPrepScriptTableSteps;
            }
            set { Replication._RepPrepScriptTableSteps = value; }
        }

        public static string RepPrepScript(RepPrepScriptTableStep S)
        {
            string SQL = "";
            switch (S)
            {
                case RepPrepScriptTableStep.AddLogColumns:
                    break;
            }
            return SQL;
        }

        public static bool logTableExists()
        {
            string SQL = "select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = '" + Replication.TableName + "'";
            string Count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Count == "1")
                return true;
            else return false;
        }

        public static bool logColumnsExist()
        {
            string SQL = "select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = '" + Replication.TableName + "' " +
                "and C.COLUMN_NAME like 'Log%tedWhen'";
            string Count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Count == "2")
                return true;
            else return false;
        }

        public static bool logColumnsAreFilled()
        {
            string SQL = "select COUNT(*) from [" + Replication.TableName + "] T " +
                "where T.logUpdatedWhen IS NULL ";
            string Count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Count == "0")
                return true;
            else return false;
        }

        public static bool logColumnsHaveDefault()
        {
            System.Data.DataRow[] rrLog = Replication.DtColumns.Select("COLUMN_NAME = 'logUpdatedWhen' AND NOT COLUMN_DEFAULT IS NULL ");
            if (rrLog.Length == 1)
                return true;
            else return false;
        }

        public static bool ColumnRowGUIDExists()
        {
            System.Data.DataRow[] rrGuid = Replication.DtColumns.Select("COLUMN_NAME = 'RowGUID'");
            if (rrGuid.Length == 1)
                return true;
            else return false;
        }

        public static bool ColumnRowGUIDHasDefault()
        {
            System.Data.DataRow[] rrDef = Replication.DtColumns.Select("COLUMN_NAME = 'RowGUID' AND NOT COLUMN_DEFAULT IS NULL ");
            if (rrDef.Length == 1)
                return true;
            else return false;
        }

        
        #endregion        
        
        public static string SqlReplicationPublisherTableDefinition()
        {
            string InsCre = Replication.InsertedOrCreated();
            string SQL = "\r\n-- Add table ReplicationPublisher if missing\r\n\r\n" +
                "IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T " +
                "where T.TABLE_NAME = 'ReplicationPublisher') = 0\r\n" +
                "BEGIN\r\n" +
            "  CREATE TABLE [dbo].[ReplicationPublisher](\r\n" +
            "    [DatabaseName] [varchar](255) NOT NULL, \r\n" +
            "    [Server] [varchar](255) NOT NULL, \r\n" +
            "    [Port] [smallint] NULL, \r\n" +
            "    [Log" + InsCre + "When] [datetime] NULL, \r\n" +
            "    [Log" + InsCre + "By] [nvarchar](50) NULL, \r\n" +
            "    [LogUpdatedWhen] [datetime] NULL, \r\n" +
            "    [LogUpdatedBy] [nvarchar](50) NULL, \r\n" +
            "    CONSTRAINT [PK_ReplicationPublisher] PRIMARY KEY CLUSTERED  \r\n" +
            "    ([DatabaseName] ASC, \r\n" +
            "    [Server] ASC \r\n" +
            "    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] \r\n" +
            "    ) ON [PRIMARY]; \r\n\r\n" +
            "  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the publishing database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'DatabaseName';\r\n" +
            "  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name or address of the server where the publishing database is located' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Server';\r\n" +
            "  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The port used by the server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Port';\r\n" +
            "  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was " + InsCre.ToLower() + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Log" + InsCre + "When';\r\n" +
            "  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Log" + InsCre + "By';\r\n" +
            "  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen';\r\n" +
            "  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy';\r\n\r\n" +
            "  ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_Log" + InsCre + "When]  DEFAULT (getdate()) FOR [Log" + InsCre + "When];\r\n" +
            "  ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_Log" + InsCre + "By]  DEFAULT (user_name()) FOR [Log" + InsCre + "By];\r\n" +
            "  ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen];\r\n" +
            "  ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy];\r\n" +
            "END \r\n" +
            "GO  \r\n\r\n";
            string SqlAdmin = "select MIN(p.name) from sys.database_principals p \r\n" +
                "where p.type = 'R' \r\n" +
                "and p.name not like 'db[_]%' \r\n" +
                "and p.name like '%admin%'\r\n";
            string AdminGroup = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlAdmin);
            SQL += "GRANT DELETE ON dbo.ReplicationPublisher TO " + AdminGroup + "\r\n" +
                "GO \r\n" +
                "GRANT INSERT ON dbo.ReplicationPublisher TO " + AdminGroup + "\r\n" +
                "GO \r\n" +
                "GRANT UPDATE ON dbo.ReplicationPublisher TO " + AdminGroup + "\r\n" +
                "GO \r\n" +
                "GRANT SELECT ON dbo.ReplicationPublisher TO " + AdminGroup + "\r\n" +
                "GO \r\n";
            return SQL;
        }

        #region Log Columns

        public static string InsertedOrCreated()
        {
            string InsertedCreated = "Created";
            string SQL = "select C.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = '" + Replication.TableName + "' " +
                "and C.COLUMN_NAME LIKE 'Log%tedWhen' " +
                "and C.COLUMN_NAME <> 'LogUpdatedWhen'";
            string Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Test.Length > 0 && Test == "LogInsertedWhen")
                InsertedCreated = "Inserted";
            return InsertedCreated;
        }

        public static string SqlAttachLogColumns()
        {
            string InsCre = Replication.InsertedOrCreated();
            string SQL = "\r\n-- Add log columns if missing\r\n\r\n" +
                "IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T " +
                "where T.TABLE_NAME = '" + Replication.TableName + "' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0\r\n" +
                "BEGIN\r\n" +
                "   ALTER TABLE [" + Replication.TableName + "] ADD\r\n" +
                "      Log" + InsCre + "By nvarchar(50) NULL,\r\n" +
                "      Log" + InsCre + "When datetime NULL,\r\n" +
                "      LogUpdatedBy nvarchar(50) NULL,\r\n" +
                "      LogUpdatedWhen datetime NULL\r\n\r\n";
            if (Replication.ServerVersion == "2000")
            {
                SQL += "   exec sp_addextendedproperty N'MS_Description', N'Name of user who first entered (typed or imported) the data.', N'user', N'dbo', N'table', N'" + Replication.TableName + "', N'column', N'Log" + InsCre + "By';\r\n";
                SQL += "   exec sp_addextendedproperty N'MS_Description', N'Date and time when the data were first entered (typed or imported) into this database.', N'user', N'dbo', N'table', N'" + Replication.TableName + "', N'column', N'Log" + InsCre + "When';\r\n";
                SQL += "   exec sp_addextendedproperty N'MS_Description', N'Name of user who last updated the data.', N'user', N'dbo', N'table', N'" + Replication.TableName + "', N'column', N'LogUpdatedBy';\r\n";
                SQL += "   exec sp_addextendedproperty N'MS_Description', N'Date and time when the data were last updated.', N'user', N'dbo', N'table', N'" + Replication.TableName + "', N'column', N'LogUpdatedWhen';\r\n\r\n";
            }
            else
            {
                SQL += "   DECLARE @v sql_variant; \r\n" +
                    "   SET @v = N'Name of user who first entered (typed or imported) the data.';\r\n" +
                    "   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Replication.TableName + "', N'COLUMN', N'Log" + InsCre + "By';\r\n";
                SQL += "   SET @v = N'Date and time when the data were first entered (typed or imported) into this database.';\r\n " +
                    "   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Replication.TableName + "', N'COLUMN', N'Log" + InsCre + "When';\r\n";
                SQL += "   SET @v = N'Name of user who last updated the data.';\r\n " +
                    "   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Replication.TableName + "', N'COLUMN', N'LogUpdatedBy';\r\n";
                SQL += "   SET @v = N'Date and time when the data were last updated.'; \r\n" +
                    "   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'" + Replication.TableName + "', N'COLUMN', N'LogUpdatedWhen';\r\n\r\n";
            }
            //SQL += "   ALTER TABLE [" + Replication.TableName + "] ADD CONSTRAINT DF_" + Replication.TableName + "_Log" + InsCre + "By DEFAULT (user_name()) FOR Log" + InsCre + "By; \r\n";
            //SQL += "   ALTER TABLE [" + Replication.TableName + "] ADD CONSTRAINT DF_" + Replication.TableName + "_Log" + InsCre + "When DEFAULT (getdate()) FOR Log" + InsCre + "When; \r\n";
            //SQL += "   ALTER TABLE [" + Replication.TableName + "] ADD CONSTRAINT DF_" + Replication.TableName + "_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy; \r\n";
            //SQL += "   ALTER TABLE [" + Replication.TableName + "] ADD CONSTRAINT DF_" + Replication.TableName + "_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen; \r\n";
            SQL += "END\r\n" +
            "GO\r\n";
            return SQL;
        }

        public static string SqlAddLogColumnDefault()
        {
            string InsCre = Replication.InsertedOrCreated();
            string SQL = "\r\n-- Add log columns defaults if missing\r\n\r\n" +
                "IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = '" + Replication.TableName + "' and C.COLUMN_NAME like 'Log%tedWhen' and C.COLUMN_DEFAULT is null) > 0\r\n" +
                "BEGIN\r\n";
            SQL += "   ALTER TABLE [" + Replication.TableName + "] ADD CONSTRAINT DF_" + Replication.TableName + "_Log" + InsCre + "By DEFAULT (user_name()) FOR Log" + InsCre + "By; \r\n";
            SQL += "   ALTER TABLE [" + Replication.TableName + "] ADD CONSTRAINT DF_" + Replication.TableName + "_Log" + InsCre + "When DEFAULT (getdate()) FOR Log" + InsCre + "When; \r\n";
            SQL += "   ALTER TABLE [" + Replication.TableName + "] ADD CONSTRAINT DF_" + Replication.TableName + "_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy; \r\n";
            SQL += "   ALTER TABLE [" + Replication.TableName + "] ADD CONSTRAINT DF_" + Replication.TableName + "_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen; \r\n";
            SQL += "END\r\n" +
            "GO\r\n";
            return SQL;
        }

        public static string SqlFillLogColumns()
        {
            string InsCre = Replication.InsertedOrCreated();
            string SQL = "\r\n-- Fill log columns if empty\r\n\r\n" +
                "IF (select COUNT(*) from [" + Replication.TableName + "] T where T.logUpdatedWhen IS NULL) > 0\r\n" +
                "BEGIN\r\n";
            SQL += "   UPDATE T SET Log" + InsCre + "When = '1900-01-01' FROM [" + Replication.TableName + "] T WHERE T.Log" + InsCre + "When  IS NULL;\r\n";
            SQL += "   UPDATE T SET LogUpdatedWhen = '1900-01-01' FROM [" + Replication.TableName + "] T WHERE T.LogUpdatedWhen  IS NULL;\r\n";
            SQL += "END\r\n" +
            "GO\r\n";
            return SQL;
        }

        public static string SqlWriteLogDate()
        {
            string SQL = "\r\n-- Fill log columns if empty\r\n\r\n";
            System.Data.DataRow[] rrLog = Replication.DtColumns.Select("COLUMN_NAME = 'logupdatedWhen' OR COLUMN_NAME = 'loginsertedWhen' OR COLUMN_NAME = 'logCreatedWhen'");
            foreach (System.Data.DataRow R in rrLog)
            {
                SQL += "UPDATE P SET P." + R["COLUMN_NAME"].ToString() + " = '1900-01-01' " +
                    "FROM [" + Replication.TableName + "] P " +
                    "WHERE P." + R["COLUMN_NAME"].ToString() + " IS NULL; \r\n";
                SQL += "UPDATE P SET P." + R["COLUMN_NAME"].ToString() + " = '1900-01-01' " +
                    "FROM [" + Replication.TableName + "_log] P " +
                    "WHERE P." + R["COLUMN_NAME"].ToString() + " IS NULL; \r\n";
            }
            return SQL;
        }

        public static string SqlAttachLogColumnsToLogTable()
        {
            string InsCre = Replication.InsertedOrCreated();
            string SQL = "\r\n-- Add log columns to log table if missing\r\n\r\n" +
                "IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T " +
                "where T.TABLE_NAME = '" + Replication.TableName + "_log' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0\r\n" +
                "BEGIN\r\n" +
                "   ALTER TABLE [" + Replication.TableName + "_log] ADD\r\n" +
                "      Log" + InsCre + "By nvarchar(50) NULL,\r\n" +
                "      Log" + InsCre + "When datetime NULL,\r\n" +
                "      LogUpdatedBy nvarchar(50) NULL,\r\n" +
                "      LogUpdatedWhen datetime NULL\r\n\r\n";
            SQL += "END\r\n" +
            "GO\r\n";
            return SQL;
        }

        #endregion

        #region Column RowGUID

        public static string SqlAttachRowGUIDColumn()
        {
            string SQL = "\r\n-- Add RowGUID column if missing\r\n\r\n" +
                "IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T " +
                "where T.TABLE_NAME = '" + Replication.TableName + "' AND T.COLUMN_NAME = 'RowGUID') = 0\r\n" +
                "BEGIN\r\n" +
                 "   ALTER TABLE [" + Replication.TableName + "] ADD [RowGUID] [uniqueidentifier] NULL;\r\n" +
                 "   ALTER TABLE [" + Replication.TableName + "] ADD DEFAULT (newsequentialid()) FOR [RowGUID];\r\n" +
                 "END\r\n" +
            "GO\r\n";
            return SQL;
        }

        public static string SqlAttachRowGUIDColumnToLogTable()
        {
            string SQL = "\r\n-- Add RowGUID column to log table if missing\r\n\r\n" +
                "IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T " +
                "where T.TABLE_NAME = '" + Replication.TableName + "_log' AND T.COLUMN_NAME = 'RowGUID') = 0\r\n" +
                "BEGIN\r\n" +
                 "   ALTER TABLE [" + Replication.TableName + "_log] ADD [RowGUID] [uniqueidentifier] NULL;\r\n" +
                 "END\r\n" +
            "GO\r\n";
            return SQL;
        }

        public static string SqlAddRowGUIDDefault()
        {
            string SQL = "\r\n-- Add default for RowGUID if missing\r\n\r\n" +
                "IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = '" + Replication.TableName + "' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0\r\n" +
                "BEGIN\r\n";
            SQL += "   ALTER TABLE [" + Replication.TableName + "] ADD DEFAULT (newsequentialid()) FOR [RowGUID];\r\n";
            SQL += "END\r\n" +
            "GO\r\n";
            return SQL;
        }

        public static string SqlTempRowGUIDTable()
        {
            string InsCre = Replication.InsertedOrCreated();
            string SQL = "";
            foreach (System.Data.DataRow R in Replication.DtPK.Rows)
            {
                SQL += "[" + R["COLUMN_NAME"].ToString() + "] [" + R["DATA_TYPE"].ToString() + "]";
                if (R["DATA_TYPE"].ToString().EndsWith("char") && R["CHARACTER_MAXIMUM_LENGTH"].ToString().Length > 0)
                    SQL += "(" + R["CHARACTER_MAXIMUM_LENGTH"].ToString() + ") NOT NULL ";
                SQL += ",\r\n ";
            }
            SQL = "\r\n-- Create temporary table containing RowGUID if missing\r\n\r\n" +
                "IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = '" + Replication.TableName + "_RowGUID') = 0\r\n" +
                "BEGIN\r\n" +
                "CREATE TABLE [dbo].[" + Replication.TableName + "_RowGUID](" +
                SQL +
                " [RowGUID] [uniqueidentifier] NULL,\r\n " +
                " [Log" + InsCre + "When] [datetime] NULL,\r\n " +
                " [Log" + InsCre + "By] [nvarchar](50) NULL,\r\n " +
                " [LogUpdatedWhen] [datetime] NULL,\r\n " +
                " [LogUpdatedBy] [nvarchar](50) NULL,\r\n " +
                "CONSTRAINT [PK_" + Replication.TableName + "_RowGUID] PRIMARY KEY CLUSTERED\r\n" +
                "(";
            for (int i = 0; i < Replication.DtPK.Rows.Count; i++)
            {
                if (i > 0) SQL += ", ";
                SQL += "[" + Replication.DtPK.Rows[i]["COLUMN_NAME"].ToString() + "] ASC ";
            }
            SQL += ") WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] \r\n" +
                ") ON [PRIMARY]; \r\n" +
                "ALTER TABLE [dbo].[" + Replication.TableName + "_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; \r\n" +
                "END\r\n";
            return SQL;
        }

        private static System.Data.DataTable _DtReplicationColumns;

        public static System.Data.DataTable DtReplicationColumns()
        {
            if (Replication._DtReplicationColumns == null)
            {
                string SQL = Replication.SqlTempRowGUIDTable();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                Replication._DtReplicationColumns = new System.Data.DataTable();
                SQL = "SELECT COLUMN_NAME as ColumnName, DATA_TYPE as Datatype, CHARACTER_MAXIMUM_LENGTH as Length, " +
                    "COLLATION_NAME  AS Collation, COLUMN_DEFAULT AS DefaultValue, '' AS Description " +
                    "FROM Information_Schema.COLUMNS  " +
                    "WHERE Information_Schema.COLUMNS.TABLE_NAME = '" + Replication.TableName + "_RowGUID' " +
                    "AND (COLUMN_NAME LIKE 'log%ted%' OR COLUMN_NAME = 'RowGUID')";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                try
                {
                    a.Fill(Replication._DtReplicationColumns);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                SQL = Replication.SqlDropTempRowGUIDTable();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            return Replication._DtReplicationColumns; 
        }

        public static string SqlTransferDataInTempRowGUIDTable()
        {
            string SqlColumns = "";
            foreach (System.Data.DataRow R in Replication.DtPK.Rows)
            {
                if (SqlColumns.Length > 0) SqlColumns += ", ";
                SqlColumns += "[" + R["COLUMN_NAME"].ToString() + "]";
            }
            string SQL = "INSERT INTO [" + Replication.TableName + "_RowGUID] (" +
                SqlColumns + ") \r\n" +
                "SELECT " +
                SqlColumns +
                "\r\nFROM [" + Replication.TableName + "] \r\n" +
                "WHERE [RowGUID] IS NULL;\r\n\r\n";
            return SQL;
        }

        public static string SqlWriteRowGUID()
        {
            string SQL = "";
            foreach (System.Data.DataRow R in Replication.DtPK.Rows)
            {
                if (SQL.Length > 0) SQL += " AND ";
                SQL += "T.[" + R["COLUMN_NAME"].ToString() + "] = R.[" + R["COLUMN_NAME"].ToString() + "]";
            }
            SQL = "UPDATE T SET [RowGUID] = R.[RowGUID] \r\n" +
                "FROM [" + Replication.TableName + "] AS T  INNER JOIN [" + Replication.TableName + "_RowGUID] AS R ON \r\n" +
                SQL + "; \r\n";
            return SQL;
        }

        public static string SqlWriteRowGUIDToLogTable()
        {
            string SQL = "";
            foreach (System.Data.DataRow R in Replication.DtPK.Rows)
            {
                if (SQL.Length > 0) SQL += " AND ";
                SQL += "T.[" + R["COLUMN_NAME"].ToString() + "] = R.[" + R["COLUMN_NAME"].ToString() + "]";
            }
            SQL = "UPDATE T SET [RowGUID] = R.[RowGUID]\r\n" +
                "FROM [" + Replication.TableName + "_log] AS T INNER JOIN [" + Replication.TableName + "_RowGUID] AS R ON \r\n" +
                SQL + "; \r\n";
            return SQL;
        }

        public static string SqlDropTempRowGUIDTable()
        {
            string SQL = "DROP TABLE [dbo].[" + Replication.TableName + "_RowGUID]\r\n";
            return SQL;
        }

        #endregion

        #region Trigger

        public static string SqlDisableUpdateTrigger()
        {
            string SQL = "DISABLE TRIGGER dbo.trgUpd" + Replication.TableName + " ON dbo.[" + Replication.TableName + "]\r\n" + 
                "GO\r\n";
            return SQL;
        }

        public static string SqlEnableUpdateTrigger()
        {
            string SQL = "ENABLE TRIGGER dbo.trgUpd" + Replication.TableName + " ON dbo.[" + Replication.TableName + "]\r\n" +
                "GO\r\n";
            return SQL;
        }

        private static System.Collections.Generic.List<string> _TableTriggerList;

        public static System.Collections.Generic.List<string> TableTriggerList()
        {
            if (Replication._TableTriggerList == null)
            {
                Replication._TableTriggerList = new List<string>();
                string SQL = "select R.name from sys.triggers R, sys.tables A " +
                    "where R.parent_id = A.object_id " +
                    "and A.name = '" + Replication.TableName + "' " +
                    "and R.type = 'TR'";
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                    Replication._TableTriggerList.Add(R[0].ToString());
            }
            return Replication._TableTriggerList;
        }

        
        #endregion        
        
        private static System.Data.DataTable _dtPK;

        public static System.Data.DataTable DtPK
        {
            get
            {
                if (Replication._dtPK == null)
                {
                    Replication._dtPK = new System.Data.DataTable();
                    string SqlPK = "SELECT COLUMN_NAME, C.DATA_TYPE, C.NUMERIC_PRECISION, C.DATETIME_PRECISION, C.CHARACTER_MAXIMUM_LENGTH " +
                        "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                        "WHERE (TABLE_NAME = '" + Replication.TableName + "') AND (EXISTS " +
                        "(SELECT * " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                        "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                        "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlPK, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(_dtPK);
                }
                return _dtPK;
            }
        }

        private static System.Data.DataTable _dtColumns;

        public static System.Data.DataTable DtColumns
        {
            get
            {
                if (Replication._dtColumns == null)
                {
                    Replication._dtColumns = new System.Data.DataTable();
                    string SqlPK = "SELECT C.COLUMN_NAME, C.COLUMN_DEFAULT " +
                        "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                        "WHERE TABLE_NAME = '" + Replication.TableName + "'";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlPK, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(_dtColumns);
                }
                return _dtColumns;
            }
        }

        #endregion

        #region RowGUID
        
        /// <summary>
        /// Check if the table has an index for the the column RowGUID
        /// </summary>
        /// <param name="TableName">The name of the table</param>
        /// <param name="Connection">The connection to the database</param>
        /// <returns>True if an index containing the column RowGUID exists</returns>
        public static bool IndexForRowGUID(string TableName, Microsoft.Data.SqlClient.SqlConnection Connection)
        {
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(Connection.ConnectionString);
            bool IndexDoesExist = false;
            string SQL = "select count(*) " +
                "from sys.indexes I, sys.index_columns IC, sys.tables T, sys.columns C " +
                "where I.index_id = IC.index_id " +
                "and IC.column_id = C.column_id " +
                "and C.object_id = T.object_id " +
                "and T.object_id = I.object_id " +
                "and IC.object_id = I.object_id " +
                "and T.name = '" + TableName + "' " +
                "and C.name = 'RowGUID'";
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                string Result = C.ExecuteScalar()?.ToString();
                bool.TryParse(Result, out IndexDoesExist);
            }
            catch (System.Exception ex) { }
            con.Close();
            con.Dispose();
            return IndexDoesExist;
        }

        /// <summary>
        /// Creating an index for the column RowGUID
        /// </summary>
        /// <param name="TableName">The name of the table for which the index should be created</param>
        /// <param name="Connection">The connection to the database</param>
        /// <returns>If the creation of the index was successful</returns>
        public static bool CreateIndexForRowGUID(string TableName, Microsoft.Data.SqlClient.SqlConnection Connection)
        {
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(Connection.ConnectionString);
            bool IndexDoesExist = false;
            string SQL = "CREATE UNIQUE NONCLUSTERED INDEX [IX_RowGUID] ON [dbo].[" + TableName + "] " +
                "([RowGUID] ASC) " +
                "WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)";
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                C.ExecuteNonQuery();
                IndexDoesExist = true;
            }
            catch (System.Exception ex) { }
            con.Close();
            con.Dispose();
            return IndexDoesExist;
        }
        
        #endregion

    }
}
