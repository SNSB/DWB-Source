using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace DiversityProjects
{
    public partial class FormDiversityProjects : Form, DiversityWorkbench.WorkbenchUnitUserInterface
    {

        #region Parameter

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProject;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectAgent;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectAgentRole;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectResource;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectReference;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectSetting;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectLicense;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectIdentifier;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectDescriptor;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectArchive;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectArchiveIdentifier;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectLabel;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProjectKeyword;
        private DiversityWorkbench.Hierarchy _Hierarchy;

        private System.Data.DataTable _DtProjectIdentifierType;

        // Parameter for command line processing (von Toni übernommen)
        private enum ProcessingMode { RegularInteractive, InteractiveQuery, SetDatabase, UpdateDatabase, HiddenCache, HiddenArchive, IrregularAbort };
        private ProcessingMode _Processing = ProcessingMode.RegularInteractive;
        private int _ProcessingResult = 0;



        #endregion

        #region Construction

        public FormDiversityProjects()
        {
            InitializeComponent();
            //this.Font = new Font("Microsoft Sans Serif", 8.25F); 
            try
            {
                //this.About();
                try { DiversityWorkbench.Settings.ModuleName = "DiversityProjects"; }
                catch { }
                DiversityWorkbench.ExceptionHandling.InitErrorLogFile();
                this.setDatabase();
                if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
                {
                    this.TransferPreviousSettings();
                }
                this.initForm();
                this.initQuery();
                this.setQueryControlEvents();
                this._Hierarchy = new DiversityWorkbench.Hierarchy(this.treeViewProjects, this.dataSetDiversityProjects.Project, this.projectBindingSource, "ProjectID", "ProjectParentID", "Project", "Project");
                if (this._ProjectIDList != null)
                    this._Hierarchy.IDs = this.ProjectIDList;
                this._Hierarchy.ColumnImageSelection = "ProjectType";
                this._Hierarchy.ImageKeys = this.ImageKeys;
                this.askOnExitToolStripMenuItem.Checked = Properties.Settings.Default.AskOnExit;

                // online manual
                this.helpProviderDiversityProjects.HelpNamespace = System.Windows.Forms.Application.StartupPath + "\\DiversityProjects.chm";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public FormDiversityProjects(string[] args)
            : this()
        {
            try
            {
                System.Collections.Generic.List<string> Arguments = new List<string>();
                string ArgumentsString = "";
                foreach (var arg in args)
                {
                    if (ArgumentsString.Length > 0)
                        ArgumentsString += "; ";
                    ArgumentsString += arg;
                    Arguments.Add(arg.ToString());
                }
                string Target = Arguments[0].ToLower().Trim();
                switch (Target)
                {

                    case "updatedatabase": //  (von Toni übernommen)
                        // UpdateDatabase <server> <port> 
                        #region UpdateDatabase
                        // Read server address and update available databases
                        try
                        {
                            // Save data
                            _Processing = ProcessingMode.UpdateDatabase;
                            string Server = Arguments[1];
                            int Port = int.Parse(Arguments[2]);

                            // Update database parameter
                            if (DiversityWorkbench.Database.SilentDatabaseUpdate(Server,
                                                                                 Port,
                                                                                 DiversityProjects.Properties.Settings.Default.DatabaseVersion,
                                                                                 "DiversityProjectsUpdate_",
                                                                                 this.helpProviderDiversityProjects.HelpNamespace,
                                                                                 Properties.Resources.ResourceManager))
                            {
                                // Mark success
                                _ProcessingResult = 1;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            string message = ex.Message + "\r\n\r\n" + ArgumentsString;
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(this.Name, "FormDiversityProjects(string[] args)", message);
                            _Processing = ProcessingMode.IrregularAbort;
                        }
                        finally
                        {
                            this.Close();
                        }
                        #endregion
                        break;


                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        #endregion

        #region Menu

        private void setMenu()
        {
            this.setDataMenu();
            this.setAdminMenu();
            this.setUpdateMenu();
            this.setTableEditorMenu();
            this.setHelpMenu();
            //this.setContextMenu();
            //this.setHelpMenu();
        }

        #region Connection

        private void createDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.Settings.ModuleName.Length == 0)
            {
                string Name = System.Reflection.Assembly.GetExecutingAssembly().FullName;
                Name = Name.Substring(0, Name.IndexOf(","));
                DiversityWorkbench.Settings.ModuleName = Name;
            }
            System.Collections.Generic.Dictionary<string, string> Versions = this.createDatabase_Versions();
            string Message = "";
            if (Versions.Count > 0)
            {
                if (DiversityWorkbench.Database.CreateDatabase(Versions, ref Message))
                {
                    this.setDatabase();
                    this.setUpdateMenu();
                    this.initQuery();
                }
                else
                {
                    if (Message.Length > 0)
                        MessageBox.Show(Message, "Failed to create database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("No database was created");
                }
            }
            else
                MessageBox.Show(Message, "Update script is missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private System.Collections.Generic.Dictionary<string, string> createDatabase_Versions()
        {
            System.Collections.Generic.Dictionary<string, string> Versions = new Dictionary<string, string>();
            try
            {
                System.Resources.ResourceManager rm = Properties.Resources.ResourceManager;
                System.Resources.ResourceSet rs = rm.GetResourceSet(new System.Globalization.CultureInfo("en-US"), true, true);
                if (rs != null)
                {
                    System.Collections.IDictionaryEnumerator de = rs.GetEnumerator();
                    while (de.MoveNext() == true)
                    {
                        if (de.Entry.Value is string)
                        {
                            if (de.Key.ToString().StartsWith("DiversityProjectsUpdate_000000_to_"))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                                break;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Versions;
        }

        private void databaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, string> Versions = this.createDatabase_Versions();

                DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase(DiversityWorkbench.Settings.ModuleName, Versions);
                f.setHelpProviderNameSpace(System.Windows.Forms.Application.StartupPath + "\\DiversityProjects.chm", "Login");
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    DiversityWorkbench.Forms.FormFunctions.ResetPermissionsCache();
                    this.setDatabase();
                    this._DatabaseRoles = DiversityWorkbench.Database.DatabaseRoles();
                    this.initQuery();
#if DEBUG
                    DiversityWorkbench.WorkbenchUnit.RemoveMistakenWorkbenchUnitConnections();
                    DiversityWorkbench.WorkbenchUnit.RequeryWorkbenchUnitConnections();

                    //DiversityWorkbench.WorkbenchUnit.ResetWorkbenchUnitConnections();
                    //DiversityWorkbench.WorkbenchUnit.RemoveMistakenWorkbenchUnitConnections();
                    //DiversityWorkbench.WorkbenchUnit.RequeryWorkbenchUnitConnections();
                    ////this.userControlQueryList.setConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase), "CollectionSpecimen");
                    //DiversityWorkbench.WorkbenchUnit.LoadAddedRemoteConnections();
#else
                    DiversityWorkbench.WorkbenchUnit.RemoveMistakenWorkbenchUnitConnections();
                    DiversityWorkbench.WorkbenchUnit.RequeryWorkbenchUnitConnections();
#endif

                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void moduleConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormConnectionAdministration f = new DiversityWorkbench.Forms.FormConnectionAdministration(System.Windows.Forms.Application.StartupPath + "\\DiversityProjects.chm");
            f.ShowDialog();
        }

        private void transferPreviousSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TransferPreviousSettings();
        }

        private void TransferPreviousSettings()
        {
            try
            {
                DiversityWorkbench.Settings.TransferPreviousSettings();

                GFBio.SettingsGFBio.Default.Upgrade();
                GFBio.SettingsGFBio.Default.Reload();
                GFBio.SettingsGFBio.Default.Save();

                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.Reload();
                Properties.Settings.Default.Save();

                this.askOnExitToolStripMenuItem.Checked = Properties.Settings.Default.AskOnExit;
                this.setDatabase();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void currentServerActivityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFunctions.ShowServerActiviy();
        }

        private void queryTimeoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Timeout = DiversityWorkbench.Settings.TimeoutDatabase;
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Timeout", "Please enter the timeout for database queries in seconds");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null)
            {
                DiversityWorkbench.Settings.TimeoutDatabase = (int)f.Integer;
                queryTimeoutToolStripMenuItem.ToolTipText = DiversityWorkbench.Settings.TimeoutDatabase.ToString() + " sec.";
            }
        }

        private void connectionTimeoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Timeout = DiversityWorkbench.Settings.TimeoutConnection;
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Timeout", "Please enter the timeout for database connection in seconds");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null)
            {
                DiversityWorkbench.Settings.TimeoutConnection = (int)f.Integer;
                connectionTimeoutToolStripMenuItem.ToolTipText = DiversityWorkbench.Settings.TimeoutConnection.ToString() + " sec.";
            }
        }

        #endregion

        #region Help

        private void setHelpMenu()
        {
            try
            {
                bool OK = this.FormFunctions.getObjectPermissions("Project", "UPDATE");
                this.clearErrorlogAtProgramStartToolStripMenuItem.Visible = !DiversityWorkbench.Settings.ErrorLogResetAtStartup;
                this.keepErrorlogToolStripMenuItem.Visible = DiversityWorkbench.Settings.ErrorLogResetAtStartup;
                this.clearErrorlogToolStripMenuItem.Visible = !DiversityWorkbench.Settings.ErrorLogResetAtStartup;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.About();
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string urlManual = global::DiversityProjects.Properties.Settings.Default.ManualURL;
                if (!string.IsNullOrEmpty(urlManual))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = urlManual,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to open manual: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);

            }
        }

        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this.userControlQueryList.QueryString(),
                this.ID.ToString());
        }
        
        private void downloadApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string urlManual = global::DiversityProjects.Properties.Settings.Default.DownloadApplicationURL;
                if (!string.IsNullOrEmpty(urlManual))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = urlManual,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to open download page: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);

            }
        }

        private void informationModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string urlManual = global::DiversityProjects.Properties.Settings.Default.InformationModelManualURL;
                if (!string.IsNullOrEmpty(urlManual))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = urlManual,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to open information model manual page: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);

            }
        }

        private void errorLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.ExceptionHandling.ShowErrorLog();
        }

        private void clearErrorlogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.ExceptionHandling.InitErrorLogFile("");
        }

        private void keepErrorlogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ErrorLogResetAtStartup = false;
            this.setHelpMenu();
        }

        private void clearErrorlogAtProgramStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ErrorLogResetAtStartup = true;
            this.setHelpMenu();
        }

        private void toolStripMenuItemIconTest_Click(object sender, EventArgs e)
        {
            //FormIconTest f = new FormIconTest();
            //f.ShowDialog();
        }

        private void metadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
            System.Collections.Generic.Dictionary<string, string> Metadata = P.Metadata(this.ID);
            System.Data.DataTable dtMeta = new DataTable("Metadata");
            System.Data.DataColumn Key = new DataColumn("Key", typeof(string));
            dtMeta.Columns.Add(Key);
            System.Data.DataColumn Value = new DataColumn("Value", typeof(string));
            dtMeta.Columns.Add(Value);
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Metadata)
            {
                System.Data.DataRow R = dtMeta.NewRow();
                R[0] = KV.Key;
                R[1] = KV.Value;
                dtMeta.Rows.Add(R);
            }
            DiversityWorkbench.Forms.FormTableContent f = new DiversityWorkbench.Forms.FormTableContent("Metadata", "Metadata for the current project", dtMeta);
            f.ShowDialog();
        }

        private void xGA1024X768ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Height = 768;
            this.Width = 1024;
        }

        #endregion

        #region Data

        private void setDataMenu()
        {
            try
            {
                if (DiversityWorkbench.Database.DatabaseRoles().Contains("Administrator")
                    || DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
                {
                    this.backupDatabaseToolStripMenuItem.Visible = true;
                    this.resetDatabaseToolStripMenuItem.Visible = true;
                }
                else
                {
                    this.backupDatabaseToolStripMenuItem.Visible = false;
                    this.resetDatabaseToolStripMenuItem.Visible = false;
                }
            }
            catch (Exception ex)
            {
                this.dataToolStripMenuItem.Visible = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        #region Export
        private void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to create a backup of the whole database?", "Backup", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string BackupPath;
                string Filename = "";
                DiversityWorkbench.Database.BackupCopy(DiversityWorkbench.Settings.DatabaseName, ref Filename, out BackupPath);
                System.Windows.Forms.MessageBox.Show(BackupPath);
            }
        }

        private void exportDatabaseAsCsvbcpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormExportToCSV f = new DiversityWorkbench.Forms.FormExportToCSV();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        private void exportWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show start window
            DiversityWorkbench.Import.FormStartWizard fs = new DiversityWorkbench.Import.FormStartWizard("Starting export wizard...", DiversityWorkbench.Import.FormStartWizard.Direction.Export);
            fs.Show();
            Application.DoEvents();

            if (this.userControlQueryList.ListOfIDs.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                fs.Close();
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DiversityWorkbench.Export.Exporter.ResetAll();

            if (this.userControlQueryList.ListOfIDs.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DiversityWorkbench.Export.Exporter.ResetAll();

            // getting the IDs
            System.Collections.Generic.List<int> L = new List<int>();
            foreach (int ID in this.userControlQueryList.ListOfIDs)
            {
                L.Add(ID);
            }

            // getting the tables
            // getting the template tables
            DiversityWorkbench.Export.Table TProject = DiversityWorkbench.Export.Exporter.AddTemplateTable("Project", "Project", DiversityWorkbench.Export.Table.Parallelity.unique, DiversityProjects.Properties.Resources.Project, null, null, null, "");
            TProject.IsStartPoint = true;

            // Agents
            System.Collections.Generic.Dictionary<string, string> TM = new Dictionary<string, string>();
            TM.Add("AgentURI", "DiversityAgents");
            DiversityWorkbench.Export.Table TAgent = DiversityWorkbench.Export.Exporter.AddTemplateTable("ProjectAgent", "Agent", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityProjects.Properties.Resources.Agent, TProject, null, TM, "");

            // ProjectIdentifier
            DiversityWorkbench.Export.Table TProjectIdentifier = DiversityWorkbench.Export.Exporter.AddTemplateTable("ProjectIdentifier", "Identifier", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityProjects.Properties.Resources.Identifier, TProject, null, null, "");

            // ProjectLicense
            System.Collections.Generic.Dictionary<string, string> TL = new Dictionary<string, string>();
            TL.Add("LicenseHolderAgentURI", "DiversityAgents");
            TL.Add("IPRHolderAgentURI", "DiversityAgents");
            TL.Add("CopyrightHolderAgentURI", "DiversityAgents");
            DiversityWorkbench.Export.Table TProjectLicense = DiversityWorkbench.Export.Exporter.AddTemplateTable("ProjectLicense", "Licenses", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityProjects.Properties.Resources.Document, TProject, null, TL, "");

            // ProjectReference
            DiversityWorkbench.Export.Table TProjectReference = DiversityWorkbench.Export.Exporter.AddTemplateTable("ProjectReference", "References", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityProjects.Properties.Resources.References, TProject, null, null, "");

            // ProjectDescriptor
            DiversityWorkbench.Export.Table TProjectDescriptor = DiversityWorkbench.Export.Exporter.AddTemplateTable("ProjectDescriptor", "Descriptor", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityProjects.Properties.Resources.KeyBlue, TProject, null, null, "");

            // ProjectResource
            DiversityWorkbench.Export.Table TProjectResource = DiversityWorkbench.Export.Exporter.AddTemplateTable("ProjectResource", "Resources", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityProjects.Properties.Resources.OpenFolder, TProject, null, null, "");

            // ProjectSetting
            DiversityWorkbench.Export.Table TProjectSetting = DiversityWorkbench.Export.Exporter.AddTemplateTable("ProjectSetting", "Settings", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityProjects.Properties.Resources.Settings, TProject, null, null, "");

            // Table list
            System.Collections.Generic.List<DiversityWorkbench.Export.Table> TT = new List<DiversityWorkbench.Export.Table>();
            TT.Add(TProject);

            DiversityWorkbench.Export.FormExport f = new DiversityWorkbench.Export.FormExport(TProject, L, TT, this.helpProviderDiversityProjects.HelpNamespace);
            f.Width = this.Width - 10;
            f.Height = this.Height - 10;
            f.StartPosition = FormStartPosition.CenterParent;
            this.Cursor = System.Windows.Forms.Cursors.Default;

            fs.Close();

            f.ShowDialog();
        }

        #region Import
        private void gFBioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GFBio.FormGFBioImport f = new GFBio.FormGFBioImport();
            f.ShowDialog();
            if (f.DataImported())
            {
                this.ResetProjectIDList();
            }
        }

        #endregion
        private void importWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartWizard("Project");
        }

        #endregion

        #region Table editors

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("Project", DiversityProjects.Properties.Resources.Project);
        }

        private void agentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("ProjectAgent", DiversityProjects.Properties.Resources.Agent);
        }

        private void iPRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("ProjectLicense", DiversityProjects.Properties.Resources.Document);
        }

        private void referencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("ProjectReference", DiversityProjects.Properties.Resources.References);
        }

        private void iDsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("ProjectIdentifier", DiversityProjects.Properties.Resources.Identifier);
        }

        private void keywordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("ProjectKeyword", DiversityProjects.Properties.Resources.Identifier);
        }

        private void labelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("ProjectLabel", DiversityProjects.Properties.Resources.Identifier);
        }

        private void resourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("ProjectResource", DiversityProjects.Properties.Resources.Resource);
        }

        private void descriptorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("ProjectDescriptor", DiversityProjects.Properties.Resources.KeyBlue);
        }

        private void projectSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("ProjectSetting", DiversityProjects.Properties.Resources.KeyBlue);
        }

        private void setTableEditorMenu()
        {
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    bool Any = false;
                    bool OK;
                    string TableName = "";

                    // Agent
                    TableName = "ProjectAgent";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    this.agentToolStripMenuItem.Visible = OK;
                    if (OK) Any = true;

                    // Contact
                    TableName = "Project";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    this.projectToolStripMenuItem.Visible = OK;
                    if (OK) Any = true;

                    this.tableEditorToolStripMenuItem.Visible = Any;
                }
                else
                {
                    this.tableEditorToolStripMenuItem.Visible = false;

                    this.agentToolStripMenuItem.Visible = false;
                    this.projectToolStripMenuItem.Visible = false;
                }
            }
            catch (Exception ex)
            {
                this.administrationToolStripMenuItem.Visible = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void TableEditing(string Table, System.Drawing.Image Icon)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (this.userControlQueryList.ListOfIDs.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                this.Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }
            System.Collections.Generic.List<string> ReadOnlyColumns = new List<string>();
            string SqlCol = "SELECT C.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + Table + "'";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter adCol = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCol, DiversityWorkbench.Settings.ConnectionString);
            adCol.Fill(dt);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                if (R[0].ToString().EndsWith("ID") || R[0].ToString().StartsWith("Log"))
                    ReadOnlyColumns.Add(R[0].ToString());
            }

            string IDs = "";
            foreach (int i in this.userControlQueryList.ListOfIDs)
            {
                if (IDs.Length > 0) IDs += ",";
                IDs += i.ToString();
            }
            string SQL = "SELECT * FROM " + Table + " T WHERE T.ProjectID IN (" + IDs + ")";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(Icon, ad, ReadOnlyColumns, 9999);
            f.StartPosition = FormStartPosition.CenterParent;
            f.Width = this.Width - 10;
            f.Height = this.Height - 10;
            f.setHelpProvider(this.helpProviderDiversityProjects.HelpNamespace, "Table editor");
            try
            {
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        #region XML-Archive

        private void administrateArchivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Archive.FormArchiveAdministration f = new DiversityWorkbench.Archive.FormArchiveAdministration(
                this.ArchiveTables(),
                "Administrate the scheduler based creation of archives",
                "Project",
                "Project",
                "ProjectID",
                "CreateArchive",
                "ArchiveProtocol",
                " WHERE ProjectID IN (SELECT ProjectID FROM ProjectUser WHERE LoginName = SUSER_SNAME())");
            f.setHelp("Archive");
            f.ShowDialog();
        }

        private void createArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;

                // Show start window
                DiversityWorkbench.Import.FormStartWizard fs = new DiversityWorkbench.Import.FormStartWizard("Initialise archive ...", "Archive", DiversityProjects.Properties.Resources.ArchivCreate);
                fs.Show();
                Application.DoEvents();

                DiversityWorkbench.Archive.FormCreateArchive f = new DiversityWorkbench.Archive.FormCreateArchive(
                    "Select the project you want to create an archive from",
                    "Project",
                    "Project",
                    "ProjectID",
                    " WHERE ProjectID IN (SELECT ProjectID FROM ProjectUser WHERE LoginName = SUSER_SNAME() OR LoginName = USER_NAME())",
                    "Project",
                    "ProjectID",
                    "ArchiveProtocol",
                    this.ArchiveTables(),
                    "ProjectDataLastChanges");

                // Close start window
                fs.Close();
                f.SetQueryStringTempIDs("SELECT ProjectID FROM [dbo].[ProjectParents] (" + DiversityWorkbench.Archive.DataArchive.PlaceHolderQueryStringTempID + ") union SELECT ProjectID FROM [dbo].[ProjectChildNodes] (" + DiversityWorkbench.Archive.DataArchive.PlaceHolderQueryStringTempID + ")");
                f.setHelp("Archive");
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void resetDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> Tables = new List<string>();
            Tables.Add("ProjectArchiveIdentifier");
            Tables.Add("ProjectArchive");
            Tables.Add("ProjectArchiveFormat_Enum");
            Tables.Add("ProjectArchiveMimeType_Enum");
            Tables.Add("ProjectDescriptor");
            Tables.Add("ProjectDescriptorElement");
            Tables.Add("ProjectSetting");
            Tables.Add("Setting");
            Tables.Add("ProjectIdentifier");
            Tables.Add("ProjectIdentifierType");
            Tables.Add("ProjectReference");
            Tables.Add("ProjectLicense");
            Tables.Add("ProjectAgent");
            Tables.Add("ProjectAgentRole");
            Tables.Add("ProjectResource");
            Tables.Add("ProjectUser");
            Tables.Add("Project");
            Tables.Add("UserProxy");
            DiversityWorkbench.Archive.FormResetDatabase f = new DiversityWorkbench.Archive.FormResetDatabase(Tables);
            f.setHelp("Archive");
            f.ShowDialog();
        }

        private void restoreArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> TableList = new List<string>();
            // Project
            TableList.Add("Project");
            TableList.Add("ProjectResource");
            TableList.Add("ProjectAgent");
            TableList.Add("ProjectAgentRole");
            TableList.Add("ProjectLicense");
            TableList.Add("ProjectReference");
            TableList.Add("ProjectIdentifierType");
            TableList.Add("ProjectIdentifier");
            TableList.Add("Setting");
            TableList.Add("ProjectSetting");
            TableList.Add("ProjectDescriptorElement");
            TableList.Add("ProjectDescriptor");
            TableList.Add("ProjectArchiveMimeType_Enum");
            TableList.Add("ProjectArchiveFormat_Enum");
            TableList.Add("ProjectArchive");
            TableList.Add("ProjectArchiveIdentifier");

            DiversityWorkbench.Archive.FormRestoreArchive f = new DiversityWorkbench.Archive.FormRestoreArchive(
                "Restore an archive of DiversityProjects", TableList);
            f.setHelp("Archive");
            f.ShowDialog();
            if (f.ProjectID() != null)
            {
                // Insert the current user
                // Markus 24.5.23: Bugfix inserting data into UserProxy
                string SQL = "INSERT INTO UserProxy (LoginName, CombinedNameCache) " + //, CurrentProjectID) " +
                    "VALUES (SUSER_SNAME(), SUSER_SNAME()) "; //," + f.ProjectID().ToString() + ")";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                SQL = "INSERT INTO UserProxy (LoginName, CombinedNameCache) " +
                    "SELECT USER_NAME(), SUSER_SNAME() WHERE SUSER_SNAME() != USER_NAME()";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                // Insert the projects for the user
                // Markus 24.5.23: Bugfix inserting data into ProjectUser
                SQL = "INSERT INTO ProjectUser (LoginName, ProjectID) " +
                    "select SUSER_SNAME(), " + f.ProjectID().ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                SQL = "INSERT INTO ProjectUser (LoginName, ProjectID) " +
                    "select USER_NAME(), " + f.ProjectID().ToString() + " WHERE SUSER_SNAME() != USER_NAME()";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                // Requery project list
                this.initQuery();
            }
        }

        private System.Collections.Generic.Dictionary<string, string> ArchiveTables()
        {
            System.Collections.Generic.Dictionary<string, string> DataTables = new Dictionary<string, string>();
            DataTables.Add("Project", "");
            // Agent tables
            DataTables.Add("ProjectAgent", "Project");
            DataTables.Add("ProjectAgentRole", "ProjectAgent");//|Project");
            // Archive tables
            DataTables.Add("ProjectArchiveFormat_Enum", "");
            DataTables.Add("ProjectArchiveMimeType_Enum", "");
            DataTables.Add("ProjectArchive", "Project");//|ProjectArchiveFormat_Enum|ProjectArchiveMimeType_Enum");
            // Identifier tables
            DataTables.Add("ProjectIdentifierType", "");
            DataTables.Add("ProjectIdentifier", "Project");//IdentifierType|Project");
            DataTables.Add("ProjectArchiveIdentifier", "Project");//Identifier|ProjectArchive");
            // Descriptor tables
            DataTables.Add("ProjectDescriptorElement", "");
            DataTables.Add("ProjectDescriptor", "Project");//DescriptorElement|Project");
            // Keyword tables
            DataTables.Add("ProjectKeyword", "Project");
            // License tables
            DataTables.Add("ProjectLicense", "Project");
            // Reference tables
            DataTables.Add("ProjectReference", "Project");
            // ProjectResource
            DataTables.Add("ProjectResource", "Project");
            // ProjectSetting
            DataTables.Add("Setting", "");
            DataTables.Add("ProjectSetting", "Project");//|Setting");

            return DataTables;
        }

        private void createSchemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> Excludes = new List<string>();
            Excludes.Add("*_log");
            Excludes.Add("*_Enum");
            Excludes.Add("dtproperties");
            Excludes.Add("*Proxy");
            Excludes.Add("sysdiagrams");
            DiversityWorkbench.Archive.FormCreateSchema f = new DiversityWorkbench.Archive.FormCreateSchema(Excludes);
            f.setHelp("Archive");
            f.ShowDialog();
        }

        #endregion

        #region Scan modules

        private void diversityAgentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.Agents, !this.diversityAgentsToolStripMenuItem.Checked);
            this.initBackLinksAgent();
        }

        private void diversityCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection, !this.diversityCollectionToolStripMenuItem.Checked);
            this.initBackLinksCollection();
        }

        private void diversityDescriptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.Descriptions, !this.diversityDescriptionsToolStripMenuItem.Checked);
            this.initBackLinksDescriptions();
        }

        private void diversityGazetteerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer, !this.diversityGazetteerToolStripMenuItem.Checked);
            this.initBackLinksGazetteer();
        }

        private void diversityReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.References, !this.diversityReferencesToolStripMenuItem.Checked);
            this.initBackLinksReferences();
        }

        private void diversitySamplingPlotsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots, !this.diversitySamplingPlotsToolStripMenuItem.Checked);
            this.initBackLinksSamplingPlots();
        }

        private void diversityScientificTermsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms, !this.diversityScientificTermsToolStripMenuItem.Checked);
            this.initBackLinksScientificTerms();
        }

        private void diversityTaxonNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames, !this.diversityTaxonNamesToolStripMenuItem.Checked);
            this.initBackLinksTaxonNames();
        }

        #endregion

        #endregion

        #region Update

        private System.Collections.Generic.List<string> _DatabaseRoles;

        public System.Collections.Generic.List<string> DatabaseRoles
        {
            get
            {
                if (this._DatabaseRoles == null)
                    this._DatabaseRoles = DiversityWorkbench.Database.DatabaseRoles();
                return _DatabaseRoles;
            }
            set { _DatabaseRoles = value; }
        }

        private void setUpdateMenu()
        {
            string SQL = "select user_name()";
            string User = "";
            bool UpdateDatabase = false;
            bool UpdateClient = false;
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    User = C.ExecuteScalar().ToString();
                    con.Close();
                    bool IsAdmin = false;
                    if (this.DatabaseRoles.Contains("db_owner"))
                        IsAdmin = true;
                    if (User == "dbo" || IsAdmin)
                    {
                        UpdateDatabase = this.UpdateCheckForDatabase();
                        this.updateDatabaseToolStripMenuItem.Visible = UpdateDatabase;
                    }
                    string ClientVersionAsInDB = "";
                    UpdateClient = DiversityWorkbench.Database.CheckIfClientNeedsAnUpdate(
                        System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Major,
                        System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Minor,
                        System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Build,
                        System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Revision,
                        ref ClientVersionAsInDB);
                    this.updateClientToolStripMenuItem.Visible = UpdateClient;
                    if (UpdateDatabase || UpdateClient)
                        this.updateToolStripMenuItem.Visible = true;
                    else this.updateToolStripMenuItem.Visible = false;
                }
                catch { }
            }
        }

        private bool UpdateCheckForDatabase()
        {
            string DBversion = "";
            bool Update = DiversityWorkbench.Database.CheckIfDatabaseNeedsAnUpdate(DiversityProjects.Properties.Settings.Default.DatabaseVersion, ref DBversion);
            return Update;
        }

        private void updateDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.Database.UpdateDatabase(DiversityProjects.Properties.Settings.Default.DatabaseVersion, "DiversityProjectsUpdate", this.helpProviderDiversityProjects.HelpNamespace, Properties.Resources.ResourceManager))
                this.setDatabase();
            this.setUpdateMenu();
        }

        private void updateClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser("http://www.diversityworkbench.net/Portal/DiversityProjects");
            f.ShowDialog();
        }

        #endregion

        #region Administration

        private void setAdminMenu()
        {
            try
            {
                //#if DEBUG
                //                this.descriptorTypesToolStripMenuItem.Visible = true;
                //#else
                //                this.descriptorTypesToolStripMenuItem.Visible = false;
                //#endif
                this.toolsToolStripMenuItem.Visible = false;
                this.databaseToolStripMenuItem1.Visible = false;
                this.jsonCacheToolStripMenuItem.Visible = false;

                if (DiversityWorkbench.Settings.Connection != null)
                {

                    bool IsAdmin = false;
                    if (DiversityWorkbench.Database.DatabaseRoles().Contains("DiversityWorkbenchAdministrator")
                        || DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
                    {
                        IsAdmin = true;
                    }
                    this.administrationToolStripMenuItem.Visible = IsAdmin; //  ToDo
                    this.buttonDescriptorTypes.Visible = IsAdmin;
                    this.buttonIDtypes.Visible = IsAdmin;

                    //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    //Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand("SELECT PERMISSIONS()", con);
                    //con.Open();
                    //int i = System.Int32.Parse(c.ExecuteScalar().ToString());
                    //con.Close();
                    //con.Dispose();
                    bool OK;
                    //if (i > 256) OK = true; else OK = false;
                    //this.administrationToolStripMenuItem.Visible = OK;
                    //this.setUpdateMenu();

                    if (DiversityWorkbench.Database.DatabaseRoles().Contains("DiversityWorkbenchUser")
                        || DiversityWorkbench.Settings.IsTrustedConnection)
                        this.changePasswordToolStripMenuItem.Visible = false;
                    else
                    {
                        this.changePasswordToolStripMenuItem.Visible = true;
                    }


                    // Renaming the database and the BaseURL
                    if (!DiversityWorkbench.Settings.IsTrustedConnection && DiversityWorkbench.Settings.DatabaseUser.Length > 0)
                        OK = DiversityWorkbench.Database.LoginIsSysAdmin(DiversityWorkbench.Settings.DatabaseUser);
                    else
                    {
                        string User = System.Environment.UserName;
                        OK = DiversityWorkbench.Database.LoginIsSysAdmin(User);
                        if (!OK)
                        {
                            string Sql = "select user_name()";
                            User = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(Sql);
                            OK = DiversityWorkbench.Database.LoginIsSysAdmin(User);
                            if (!OK && User == "dbo")
                                OK = true;
                        }
                    }
                    this.renameDatabaseToolStripMenuItem.Visible = OK;
                    this.setPublishedAddressToolStripMenuItem.Visible = OK;
                    this.identifierTypesToolStripMenuItem.Visible = OK;
                    this.jsonCacheToolStripMenuItem.Visible = false;

                    this.archiveFormatsToolStripMenuItem.Visible = false;
                    this.archiveMimeTypesToolStripMenuItem.Visible = false;
#if DEBUG
                    this.archiveFormatsToolStripMenuItem.Visible = OK;
                    this.archiveMimeTypesToolStripMenuItem.Visible = OK;
#endif


                    string SQL = "select user_name()";
                    string Login = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Login == "dbo")
                        this.databaseToolsToolStripMenuItem.Visible = true;
                    else this.databaseToolsToolStripMenuItem.Visible = false;
                }
                else this.administrationToolStripMenuItem.Visible = false;
            }
            catch (Exception ex)
            {
                this.administrationToolStripMenuItem.Visible = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormPassword f = new DiversityWorkbench.Forms.FormPassword();
                f.ShowDialog();
            }
            catch (System.Exception ex)
            { }
        }

        private void descriptorTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> ParentCodes = new List<string>();
            DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(
                DiversityProjects.Properties.Resources.KeyBlue,
                "ProjectDescriptorElement",
                "ElementID",
                "ParentElementID",
                "DisplayText",
                "Available of descriptor elements", false, "", ParentCodes);
            f.setHelp("Project descriptor");
            //f.PreventEditingData();
            System.Collections.Generic.List<string> LockedList = new List<string>();
            LockedList.Add("0");
            LockedList.Add("1");
            LockedList.Add("2");
            LockedList.Add("3");
            LockedList.Add("4");
            LockedList.Add("5");
            f.SetLockedList(LockedList);
            f.SetSqlKeyRetrieval("SELECT MAX([ElementID]) + 1 FROM [dbo].[ProjectDescriptorElement]");
            f.ShowDialog();
            if (f.DataHaveBeenChanged)
            {
                this.setDescriptorElementSource();
            }
            //System.Collections.Generic.List<string> ParentCodes = new List<string>();
            //DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(DiversityProjects.Properties.Resources.KeyBlue, "ProjectDescriptorElement", "ElementID", "ParentElementID", "DisplayText", "Administration of descriptor elements", false, "", ParentCodes);
            //f.setHelp("Project descriptor");
            //f.ShowDialog();
            //System.Windows.Forms.MessageBox.Show("If the descriptor elements had been edited, please reconnect to the database to make these changes available");
        }

        private void loginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormLoginAdministration f = new DiversityWorkbench.Forms.FormLoginAdministration();
            f.ShowDialog();
            this._ProjectIDList = null;
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormDocumentation f = new DiversityWorkbench.Forms.FormDocumentation();
            f.setHelp("Documentation");
            f.ShowDialog();
        }

        private void identifierTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> ParentCodes = new List<string>();
            DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(DiversityProjects.Properties.Resources.Identifier, "ProjectIdentifierType", "Type", "ParentType", "Type", "Administration of identifier types", false, "", ParentCodes);
            f.setHelp("Project identifier");
            f.ShowDialog();
            System.Windows.Forms.MessageBox.Show("If the identifier types had been edited, please reconnect to the database to make these changes available");
            //this.setProjectIdentifierTypeSource();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityProjects.FormSettings F = new FormSettings(null, null);
            F.ShowDialog();
        }

        private void renameDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.Database.RenameDatabase())
                this.setDatabase();
        }

        private void setPublishedAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Database.SetBaseURL();
        }

        private void databaseToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormRestricted FR = new DiversityWorkbench.Forms.FormRestricted();
            if (FR.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                System.Windows.Forms.MessageBox.Show("Obviously not qualified ...");
                return;
            }
            DiversityWorkbench.Forms.FormDatabaseTool f = new DiversityWorkbench.Forms.FormDatabaseTool();
            f.HideProcSetVersion();
            f.HideReplicationPreparation();
            f.setHelpProvider(this.helpProviderDiversityProjects.HelpNamespace, "Database tools");
            f.ShowDialog();
        }

        private void stableIdentifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormStableIdentifier fS = new DiversityWorkbench.Forms.FormStableIdentifier();
            fS.ShowDialog();
            return;



            string SQL = "SELECT [dbo].[StableIdentifierBase] ()";
            string StableIdentifier = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (StableIdentifier.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show("Stable identifier base already set to:\r\n" + StableIdentifier);
            }
            else
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Stable identifier", "Please enter the base URL for the stable identifier", StableIdentifier);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    StableIdentifier = f.String;
                    if (!StableIdentifier.EndsWith("/"))
                        StableIdentifier += "/";
                    string Message = "";
                    SQL = "CREATE FUNCTION [dbo].[StableIdentifierBase] () RETURNS  varchar (255) AS BEGIN declare @URL varchar(255) set @URL = '" + StableIdentifier + "' return @URL END";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                    {
                        SQL = "GRANT EXECUTE ON dbo.StableIdentifierBase TO [DiversityWorkbenchUser]";
                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                            System.Windows.Forms.MessageBox.Show("Stable identifier base created");
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("Stable identifier creation failed:\r\n" + Message);
                }
            }
        }

        private void resourcesDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.setWorkbenchDirectory();// .Settings.setResourcesDirectory();
        }

        private void archiveFormatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(DiversityProjects.Properties.Resources.OAIS128, "ProjectArchiveFormat_Enum", "Edit formats for the OAIP", "");
            f.ShowDialog();
        }

        private void archiveMimeTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(DiversityProjects.Properties.Resources.OAIS128, "ProjectArchiveMimeType_Enum", "Edit mime types for the OAIP", "");
            f.ShowDialog();
        }

        private void jsonCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Api.FormApi formApi = new DiversityWorkbench.Api.FormApi("Project", "ProjectID");
            formApi.setProject("Project");
            //formApi.setProject("TaxonNameProject");
            if (formApi != null && !formApi.IsDisposed)
                formApi.ShowDialog();
        }


        #endregion

        #endregion

        #region Query

        private void initQuery()
        {
            this.setSearchOptions();
            this.setQueryDisplayColumns();
            //this.setQueryControlEvents();

            this.initQueryOptimizing();
        }

        private void initQueryOptimizing()
        {
            DiversityWorkbench.UserControls.UserControlQueryList.QueryMainTable = "Project_Core";
            this.userControlQueryList.QueryMainTableLocal = "Project_Core";
        }

        private void setQueryDisplayColumns()
        {
            this.userControlQueryList.QueryDisplayColumns = this.DiversityProject.QueryDisplayColumns();
        }

        private void setSearchOptions()
        {
            this.userControlQueryList.setQueryConditions(this.DiversityProject.QueryConditions());
        }

        private void setQueryControlEvents()
        {
            try
            {
                // QueryList
                this.userControlQueryList.toolStripButtonConnection.Click += new System.EventHandler(this.databaseToolStripMenuItem_Click);
                this.userControlQueryList.toolStripButtonCopy.Click += new System.EventHandler(this.copyProject);
                this.userControlQueryList.toolStripButtonDelete.Click += new System.EventHandler(this.deleteProject);
                this.userControlQueryList.toolStripButtonNew.Click += new System.EventHandler(this.createNewProject);
                this.userControlQueryList.toolStripButtonSave.Click += new System.EventHandler(this.saveProject);
                this.userControlQueryList.toolStripButtonUndo.Click += new System.EventHandler(this.undoChangesInProject);
                this.userControlQueryList.toolStripButtonSwitchOrientation.Click += new System.EventHandler(this.switchQueryOrientation);
                this.userControlQueryList.listBoxQueryResult.SelectedIndexChanged += new System.EventHandler(this.listBoxQueryResult_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void switchQueryOrientation(object sender, System.EventArgs e)
        {
            try
            {
                if (this.userControlQueryList.splitContainerMain.Orientation == Orientation.Vertical)
                    this.splitContainerMain.SplitterDistance = (int)this.splitContainerMain.SplitterDistance / 2;
                else
                    this.splitContainerMain.SplitterDistance = (int)this.splitContainerMain.SplitterDistance * 2;
                this.userControlQueryList.switchOrientation();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxQueryResult_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.SuspendLayout();
                if (this.userControlQueryList.listBoxQueryResult.SelectedIndex > -1)
                    this.setProject(this.userControlQueryList.ID);
                this.ResumeLayout();
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region toolStripQuery

        #region Copy

        private void copyProject(object sender, System.EventArgs e)
        {
            string DisplayText = "copy of ";
            if (this.userControlQueryList.listBoxQueryResult.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.userControlQueryList.listBoxQueryResult.SelectedItem;
                if (R.Row.Table.Columns.Contains("Display") && R["Display"] != System.DBNull.Value)
                {
                    DisplayText += R["Display"].ToString();
                    try
                    {
                        if (System.Windows.Forms.MessageBox.Show("Do you want to create a " + DisplayText, "Copy project?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                        {
                            int OriginalID = int.Parse(R["ID"].ToString());
                            string Project = DisplayText;
                            string Message = "";
                            int ID = this.CopyProject(OriginalID, Project, ref Message);
                            if (Message.Length == 0)
                            {
                                if (this.userControlQueryList.ManyOrderByColumns())
                                {
                                    string Display = this.userControlQueryList.ManyOrderByColumns_DisplayText(ID);
                                    this.userControlQueryList.AddListItem(ID, Display);
                                }
                                else
                                    this.userControlQueryList.AddListItem(ID, Project);
                            }
                            else
                                System.Windows.Forms.MessageBox.Show("Copy failed");
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            else
            {
                if (this.dataSetDiversityProjects.Project.Rows[0]["Project"] == System.DBNull.Value)
                    DisplayText += this.dataSetDiversityProjects.Project.Rows[0][0].ToString();
                else
                    DisplayText += this.dataSetDiversityProjects.Project.Rows[0]["Project"].ToString();
                int OriginalID = (int)this.ID;
                try
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you want to create a " + DisplayText, "Copy project?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        string Project = DisplayText;
                        string Message = "";
                        int ID = this.CopyProject(OriginalID, Project, ref Message);
                        if (Message.Length == 0)
                            this.userControlQueryList.AddListItem(ID, Project);
                        else
                            System.Windows.Forms.MessageBox.Show("Copy failed");
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        /// <summary>
        /// Copy the data of a Project into a new dataset
        /// </summary>
        /// <param name="OriginalID">The ID of the dataset that should be copied</param>
        /// <param name="AccessionNumber">the accessionnumber of the new Project</param>
        /// <returns>the ID of the new Project</returns>
        private int CopyProject(int OriginalID, string Project, ref string Message)
        {
            string SQL = "execute dbo.procInsertProjectCopy NULL , " + OriginalID.ToString() + ", '" + Project + "'";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            int ID = 0;
            try
            {
                ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                this._ProjectIDList = null;
                this.setProject(ID);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                Message = ex.Message;
            }
            con.Close();
            return ID;
        }

        #endregion

        #region Delete

        private void deleteProject(object sender, System.EventArgs e)
        {
            if (this.dataSetDiversityProjects.Project.Rows.Count > 0)
            {
                int ID = (int)this.ID;
                string SQL = "ProjectID = " + ID.ToString();
                System.Data.DataRow[] RR = this.dataSetDiversityProjects.Project.Select("ProjectID = " + ID.ToString());
                string DisplayTextDel = RR[0]["Project"].ToString(); // this.dataSetDiversityProjects.Project.Rows[0]["Project"].ToString();
                try
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you want to delete the Project\r\n\r\n" + DisplayTextDel, "Delete project?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.userControlQueryList.RemoveSelectedListItem();
                        this.deleteProject(ID);
                        this.dataSetDiversityProjects.Clear();
                        this.splitContainerData.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
            }
        }

        /// <summary>
        /// delete a name from the database
        /// </summary>
        /// <param name="NameID">the Primary key of table Project corresponding to the name that should be deleted</param>
        private void deleteProject(int ID)
        {
            try
            {
                string SQL = "DELETE FROM ProjectUser WHERE ProjectID = " + ID.ToString();
                Microsoft.Data.SqlClient.SqlConnection Conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(SQL, Conn);
                com.CommandType = System.Data.CommandType.Text;
                Conn.Open();
                com.ExecuteNonQuery();
                com.CommandText = "DELETE FROM ProjectLicense WHERE ProjectID = " + ID.ToString();
                com.ExecuteNonQuery();
                com.CommandText = "DELETE FROM ProjectDescriptor WHERE ProjectID = " + ID.ToString();
                com.ExecuteNonQuery();
                com.CommandText = "DELETE FROM Project WHERE ProjectID = " + ID.ToString();
                com.ExecuteNonQuery();
                Conn.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        #endregion

        #region New

        private void createNewProject(object sender, System.EventArgs e)
        {
            try
            {
                string Project = "";
                if (System.Windows.Forms.MessageBox.Show("Do you want to create a new Project", "New Project?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int ID = this.InsertNewProject(ref Project, null);
                    if (ID > -1)
                    {
                        this.setProject(ID);
                        if (this.userControlQueryList.ManyOrderByColumns())
                        {
                            string Display = this.userControlQueryList.ManyOrderByColumns_DisplayText(ID);
                            this.userControlQueryList.AddListItem(ID, Display);
                        }
                        else
                        {
                            if (Project.Length > 0)
                                this.userControlQueryList.AddListItem(ID, Project);
                            else
                                this.userControlQueryList.AddListItem(ID, "ID: " + ID.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// inserting a new Project into the table CollectionProject
        /// </summary>
        /// <returns>the CollectionProjectID of the new Project</returns>
        //private int InsertNewProject(ref string Project)
        //{
        //    int ProjectID = -1;
        //    try
        //    {
        //        String SQL = "SELECT MAX(ProjectID) + 1 FROM Project";
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //        con.Open();
        //        int ID = int.Parse(C.ExecuteScalar().ToString());
        //        int NewID = ID;
        //        con.Close();
        //        DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New project", "Please enter the name of the new project", "");
        //        f.ShowDialog();
        //        if (f.DialogResult == DialogResult.OK)
        //        {
        //            Project = f.String;
        //            if (System.Windows.Forms.MessageBox.Show("The next free ID for the project is \r\n\r\n" + ID.ToString() + "\r\n\r\nDo you want to accept this ID?\r\n\r\nYes to accept\r\nNo to set your own ID", "New Project", MessageBoxButtons.YesNo) == DialogResult.No)
        //            {
        //                bool OK = false;
        //                DiversityWorkbench.Forms.FormGetString fID = new DiversityWorkbench.Forms.FormGetString("ProjectID", "Please give the ID for the new Project", ID.ToString());
        //                while (!OK)
        //                {
        //                    fID.ShowDialog();
        //                    if (f.DialogResult == DialogResult.OK)
        //                    {
        //                        bool IdOK = false;
        //                        int Count = 0;
        //                        if (int.TryParse(fID.String, out NewID))
        //                        {
        //                            C.CommandText = "SELECT COUNT(*) FROM Project WHERE ProjectID = " + NewID.ToString();
        //                            con.Open();
        //                            Count = int.Parse(C.ExecuteScalar().ToString());
        //                            con.Close();
        //                            if (Count == 0)
        //                                OK = true;
        //                            else
        //                            {
        //                                fID.Text = "ProjectID " + NewID.ToString() + " is used";
        //                                fID.String = ID.ToString();
        //                            }
        //                        }
        //                    }
        //                    else
        //                        OK = true;
        //                }
        //            }
        //            if (ID != NewID)
        //                C.CommandText = "SET IDENTITY_INSERT Project ON; INSERT INTO Project (ProjectID, Project) VALUES (" + NewID.ToString() + ", '" + Project + "'); SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
        //            else
        //                C.CommandText = "INSERT INTO Project (Project) VALUES ('" + Project + "'); SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
        //            con.Open();
        //            ProjectID = int.Parse(C.ExecuteScalar().ToString());
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        return -1;
        //    }
        //    return ProjectID;
        //}

        /// <summary>
        /// inserting a new Project into the table CollectionProject
        /// </summary>
        /// <param name="Project">the name of the project</param>
        /// <param name="ProjectParentID">opt.: the ID of the parent project</param>
        /// <returns>the ProjectID of the new Project</returns>
        private int InsertNewProject(ref string Project, int? ProjectParentID)
        {
            int ProjectID = -1;
            try
            {
                String SQL = "SELECT CASE WHEN MAX(ProjectID) IS NULL THEN 0 ELSE MAX(ProjectID) END + 1 FROM Project";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                int ID = int.Parse(C.ExecuteScalar().ToString());
                int NewID = ID;
                con.Close();
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New project", "Please enter the name of the new project", "");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    Project = f.String.Replace("'", "''");
                    bool ChooseOwnID = false;
                    if (System.Windows.Forms.MessageBox.Show("The next free ID for the project is \r\n\r\n" + ID.ToString() + "\r\n\r\nDo you want to accept this ID?\r\n\r\nYes to accept\r\nNo to set your own ID", "New Project", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        bool OK = false;
                        ChooseOwnID = true;
                        DiversityWorkbench.Forms.FormGetString fID = new DiversityWorkbench.Forms.FormGetString("ProjectID", "Please give the ID for the new Project", ID.ToString());
                        while (!OK)
                        {
                            fID.ShowDialog();
                            if (f.DialogResult == DialogResult.OK)
                            {
                                bool IdOK = false;
                                int Count = 0;
                                if (int.TryParse(fID.String, out NewID))
                                {
                                    C.CommandText = "SELECT COUNT(*) FROM Project WHERE ProjectID = " + NewID.ToString();
                                    con.Open();
                                    Count = int.Parse(C.ExecuteScalar().ToString());
                                    con.Close();
                                    if (Count == 0)
                                        OK = true;
                                    else
                                    {
                                        fID.Text = "ProjectID " + NewID.ToString() + " is used";
                                        fID.String = ID.ToString();
                                    }
                                }
                            }
                            else
                                OK = true;
                        }
                    }
                    if (ID != NewID || ChooseOwnID)
                    {
                        C.CommandText = "SET IDENTITY_INSERT Project ON; INSERT INTO Project (ProjectID, Project";
                        if (ProjectParentID == null)
                            C.CommandText += ") VALUES (" + NewID.ToString() + ", '" + Project + "')";
                        else
                            C.CommandText += ", ProjectParentID) VALUES (" + NewID.ToString() + ", '" + Project + "', " + ProjectParentID.ToString() + ")";
                        C.CommandText += "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                    }
                    else
                    {
                        C.CommandText = "INSERT INTO Project (Project";
                        if (ProjectParentID == null)
                            C.CommandText += ") VALUES ('" + Project + "')";
                        else
                            C.CommandText += ", ProjectParentID) VALUES ('" + Project + "', " + ProjectParentID.ToString() + ")";
                        C.CommandText += "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                    }
                    con.Open();
                    ProjectID = int.Parse(C.ExecuteScalar().ToString());
                    con.Close();
                    this.InsertProjectUser(ProjectID);
                    this._ProjectIDList = null;
                    this._Hierarchy.IDs = this.ProjectIDList;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return -1;
            }
            return ProjectID;
        }

        private void InsertProjectUser(int ProjectID)
        {
            try
            {
                // Markus 28.3.23: bugfix if user is present
                string SQL = "IF(SELECT COUNT(*) FROM UserProxy WHERE LoginName = USER_NAME()) = 0 BEGIN " +
                    "INSERT INTO UserProxy(LoginName, CombinedNameCache) " +
                    "SELECT USER_NAME(), USER_NAME() END";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                SQL = "INSERT INTO ProjectUser (LoginName, ProjectID) " +
                    "VALUES     (USER_NAME(), " + ProjectID.ToString() + ")";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #region Save

        private void saveProject(object sender, System.EventArgs e)
        {
            this.updateProject();
        }

        #endregion

        #region Undo

        private void undoChangesInProject(object sender, System.EventArgs e)
        {
            try
            {
                int i = System.Int32.Parse(this.dataSetDiversityProjects.Project.Rows[0]["ProjectID"].ToString());
                this.dataSetDiversityProjects.Clear();
                this.setProject(i);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Database

        private void setDatabase()
        {
            /// finding the position of the exception: Der Objektverweis wurde nicht auf eine Objektinstanz festgelegt
            int Position = 1;
            try
            {
                if (this.dataSetDiversityProjects != null)
                    this.dataSetDiversityProjects.Clear();
                DiversityWorkbench.Database.DatabaseRolesReset();
                Position = 2;
                try
                {
                    if (DiversityWorkbench.Settings.ConnectionString != "" && this.FormFunctions.DatabaseAccessible)
                    {
                        this.resetSqlAdapters();
                        Position = 3;
                        this.projectTableAdapter.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                        Position = 4;
                        this.initRemoteModules();
                        this.setMenu();
                        Position = 5;
                        if (this._ProjectIDList != null)
                            this._ProjectIDList = null;
                        if (this._Hierarchy != null)
                            this._Hierarchy.IDs = this.ProjectIDList;
                        this.SetEnumSources();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Position: " + Position.ToString());
                }
                this.FormFunctions.setTitle();
                this.setToolbarPermissions();
                Position = 6;
                //this.SetEnumSources();
                this.userControlQueryList.toolStripButtonConnection.ToolTipText = this.FormFunctions.ConnectionInfo();
                this.userControlQueryList.setConnection(DiversityWorkbench.Settings.ConnectionString, "Project");
                Position = 7;
                this.splitContainerData.Visible = false;
                DiversityWorkbench.Entity.setEntity(this, this.toolTip);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Position: " + Position.ToString());
            }
        }

        private void SetEnumSources()
        {
            try
            {
                this.setProjectIdentifierTypeSource();
                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxAgentRole, "ProjectAgentRole_Enum", DiversityWorkbench.Settings.Connection, true, true, true);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxAgentType, "ProjectAgentType_Enum", DiversityWorkbench.Settings.Connection, true, true, true);

                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxProjectType, "ProjectType_Enum", DiversityWorkbench.Settings.Connection, true, true, true);

                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxArchiveFormat, "ProjectArchiveFormat_Enum", DiversityWorkbench.Settings.Connection, true, true, true);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxArchiveMimeType, "ProjectArchiveMimeType_Enum", DiversityWorkbench.Settings.Connection, true, true, true);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxArchiveParentRelationType, "ProjectArchiveRelationType_Enum", DiversityWorkbench.Settings.Connection, true, true, true);

                System.Data.DataTable dtLang = new DataTable();
                string SQL = "SELECT Description, Code FROM Language_Enum ORDER BY DisplayOrder";
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtLang, ref Message);
                this.comboBoxDescriptorLanguage.DataSource = dtLang;
                this.comboBoxDescriptorLanguage.DisplayMember = "Description";
                this.comboBoxDescriptorLanguage.ValueMember = "Code";

                //if (this.dataSetDiversityProjects.Language_Enum.Rows.Count > 0)
                //    this.dataSetDiversityProjects.Language_Enum.Clear();
                //this.language_EnumTableAdapter.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                //this.language_EnumTableAdapter.Fill(this.dataSetDiversityProjects.Language_Enum);
                //this.comboBoxDescriptorLanguage.DataSource = this.dataSetDiversityProjects.Language_Enum;
                //this.comboBoxDescriptorLanguage.DisplayMember = "Description";
                //this.comboBoxDescriptorLanguage.ValueMember = "Code";

                this.setDescriptorElementSource();

                //if (this.dataSetDiversityProjects.ProjectDescriptorElement.Rows.Count > 0)
                //    this.dataSetDiversityProjects.ProjectDescriptorElement.Clear();
                //this.projectDescriptorElementTableAdapter.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                //this.projectDescriptorElementTableAdapter.Fill(this.dataSetDiversityProjects.ProjectDescriptorElement);
                //this.comboBoxDescriptorElement.DataSource = this.dataSetDiversityProjects.ProjectDescriptorElement;
                //this.comboBoxDescriptorElement.DisplayMember = "DisplayText";
                //this.comboBoxDescriptorElement.ValueMember = "ElementID";

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setDescriptorElementSource()
        {
            try
            {
                System.Data.DataTable dtEl = new DataTable();
                string SQL = "SELECT ElementID, DisplayText FROM ProjectDescriptorElement WHERE (DisplayEnabled = 1) ORDER BY DisplayOrder";
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtEl, ref Message);
                this.comboBoxDescriptorElement.DataSource = dtEl;
                this.comboBoxDescriptorElement.DisplayMember = "DisplayText";
                this.comboBoxDescriptorElement.ValueMember = "ElementID";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setProjectIdentifierTypeSource()
        {
            try
            {
                this._DtProjectIdentifierType = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(DiversityProjects.Project.SqlProjectIdentifierType, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this._DtProjectIdentifierType);
                this.comboBoxIDtype.DataSource = this._DtProjectIdentifierType;
                this.comboBoxIDtype.ValueMember = "Type";
                this.comboBoxIDtype.DisplayMember = "Type";
                int iLenMax = 0;
                foreach (System.Data.DataRow R in this._DtProjectIdentifierType.Rows)
                {
                    if (R["Type"].ToString().Length > iLenMax) iLenMax = R["Type"].ToString().Length;
                }
                iLenMax = (int)((float)iLenMax * (float)this.comboBoxIDtype.Font.SizeInPoints / 1.75);
                if (this.comboBoxIDtype.DropDownWidth < iLenMax)
                {
                    this.comboBoxIDtype.DropDownWidth = iLenMax;
                }
                else if (this.comboBoxIDtype.DropDownWidth < this.comboBoxIDtype.Width)
                    this.comboBoxIDtype.DropDownWidth = this.comboBoxIDtype.Width;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void resetSqlAdapters()
        {
            this._sqlDataAdapterProject = null;
            this._sqlDataAdapterProjectAgent = null;
            this._sqlDataAdapterProjectResource = null;
            this._sqlDataAdapterProjectReference = null;
            this._sqlDataAdapterProjectSetting = null;
            this._sqlDataAdapterProjectLicense = null;
            this._sqlDataAdapterProjectIdentifier = null;
            //this._sqlDataAdapterProjectLabel = null;
            this._sqlDataAdapterProjectDescriptor = null;
            this._sqlDataAdapterProjectArchive = null;
        }

        private void setConnection(string[] args)
        {
            try
            {
                DiversityWorkbench.Settings.IsTrustedConnection = bool.Parse(args[1]);
                DiversityWorkbench.Settings.DatabaseServer = args[2];
                DiversityWorkbench.Settings.DatabaseName = args[3];
                if (!DiversityWorkbench.Settings.IsTrustedConnection)
                {
                    DiversityWorkbench.Settings.DatabaseUser = args[4];
                    DiversityWorkbench.Settings.Password = args[5];
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region About

        private void About()
        {
            try
            {
                DiversityProjects.FormAbout f = new FormAbout();
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Form

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private void initForm()
        {
            try
            {
                this.AcceptButton = this.userControlDialogPanel.buttonOK;
                this.CancelButton = this.userControlDialogPanel.buttonCancel;
                this.FormFunctions.addEditOnDoubleClickToTextboxes();
                this.setDatabindings();
                this.listBoxAgents_SelectedIndexChanged(null, null);
                this.tableLayoutPanelReference.Enabled = false;
                this.FormFunctions.addEditOnDoubleClickToTextboxes();
                this.initModuleLinks();
                this.initBackLinks();

                //this.userControlQueryList.ManyOrderByColumns_Allow(true, DiversityWorkbench.Settings.ModuleName, "FormDiversityProjects", true);

                this.userControlQueryList.OptimizingAvailable(false);

                this.userControlQueryList.ManyOrderByColumns_Allow(true);
                this.userControlQueryList.ManyOrderByColumns_InitControls();

                this.textBoxIPR.Dock = DockStyle.Fill;
                //this.textBoxLicenseYear.Leave += textBoxLicenseYear_Leave;
                this.textBoxLicenseYear.Validating += textBoxLicenseYear_Validating;

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initModuleLinks()
        {
            // Database must still be enabled to store link to DP
            this.tabControlModules.TabPages.Remove(this.tabPageDiversityAgents);
            this.tabControlModules.TabPages.Remove(this.tabPageDiversityDescriptions);
            this.tabControlModules.TabPages.Remove(this.tabPageDiversityGazetteer);
            this.tabControlModules.TabPages.Remove(this.tabPageDiversityReferences);
            this.tabControlModules.TabPages.Remove(this.tabPageDiversitySamplingPlots);
            //this.tabControlModules.TabPages.Remove(this.tabPageDiversityScientificTerms);
            //this.tabControlModules.TabPages.Remove(this.tabPageDiversityTaxonNames);

            //this.userControlModuleLinksAgents.ShowCheckboxForScanning(true);
            //this.userControlModuleLinksAgents.SetModuleParameter(DiversityWorkbench.WorkbenchUnit.ModuleType.Agents, this, "ProjectProxy", "ProjectURI", "ProjectProxy");
            //bool ScanAgents = DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Agents);
            //this.userControlModuleLinksAgents.ScanModul(ScanAgents);

            //this.userControlModuleLinksCollection.ShowCheckboxForScanning(true);
            //this.userControlModuleLinksCollection.SetModuleParameter(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection, this, "ProjectProxy", "ProjectURI", "ProjectProxy");
            //bool ScanCollection = DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection);
            //this.userControlModuleLinksCollection.ScanModul(ScanCollection);

            //this.userControlModuleLinksTerms.ShowCheckboxForScanning(true);
            //this.userControlModuleLinksTerms.SetModuleParameter(DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms, this, "Terminology", "ProjectURI", "Terminology");
            //bool ScanTerms = DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms);
            //this.userControlModuleLinksTerms.ScanModul(ScanTerms);

            //this.userControlModuleLinksTaxa.ShowCheckboxForScanning(true);
            //this.userControlModuleLinksTaxa.SetModuleParameter(DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames, this, "ProjectProxy", "ProjectURI", "ProjectProxy");
            //bool ScanTaxa = DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames);
            //this.userControlModuleLinksTaxa.ScanModul(ScanTaxa);

            //this.userControlModuleLinksTaxaLists.ShowCheckboxForScanning(true);
            //this.userControlModuleLinksTaxaLists.SetModuleParameter(DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames, this, "TaxonNameListProjectProxy", "ProjectURI", "TaxonNameListProjectProxy");
            ////bool ScanTaxaList = DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames);
            //this.userControlModuleLinksTaxaLists.ScanModul(ScanTaxa);

        }

        private void FormDiversityProjects_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.Language_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.language_EnumTableAdapter.Fill(this.dataSetDiversityProjects.Language_Enum);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectReference". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectReferenceTableAdapter.Fill(this.dataSetDiversityProjects.ProjectReference);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectArchive". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectArchiveTableAdapter.Fill(this.dataSetDiversityProjects.ProjectArchive);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectIdentifier". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectIdentifierTableAdapter.Fill(this.dataSetDiversityProjects.ProjectIdentifier);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectDescriptorElement". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectDescriptorElementTableAdapter.Fill(this.dataSetDiversityProjects.ProjectDescriptorElement);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectDescriptor". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectDescriptorTableAdapter.Fill(this.dataSetDiversityProjects.ProjectDescriptor);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectResource". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectResourceTableAdapter.Fill(this.dataSetDiversityProjects.ProjectResource);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectLicense". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectLicenseTableAdapter.Fill(this.dataSetDiversityProjects.ProjectLicense);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectIdentifierType". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectIdentifierTypeTableAdapter.Fill(this.dataSetDiversityProjects.ProjectIdentifierType);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectAgentRole". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectAgentRoleTableAdapter.Fill(this.dataSetDiversityProjects.ProjectAgentRole);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectAgent". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectAgentTableAdapter.Fill(this.dataSetDiversityProjects.ProjectAgent);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectSetting". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectSettingTableAdapter.Fill(this.dataSetDiversityProjects.ProjectSetting);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.Project". Sie können sie bei Bedarf verschieben oder entfernen.
            ////this.projectTableAdapter.Fill(this.dataSetDiversityProjects.Project);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects1.ProjectArchiveIdentifier". Sie können sie bei Bedarf verschieben oder entfernen.
            ////this.projectArchiveIdentifierTableAdapter.Fill(this.dataSetDiversityProjects.ProjectArchiveIdentifier);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectArchive". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectArchiveTableAdapter.Fill(this.dataSetDiversityProjects.ProjectArchive);
            try
            {
                //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectDescriptorElement". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectDescriptorElementTableAdapter.Fill(this.dataSetDiversityProjects.ProjectDescriptorElement);
                //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.Language_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.language_EnumTableAdapter.Fill(this.dataSetDiversityProjects.Language_Enum);
                //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectDescriptor". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectDescriptorTableAdapter.Fill(this.dataSetDiversityProjects.ProjectDescriptor);
                // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.Project". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectTableAdapter.Fill(this.dataSetDiversityProjects.Project);
                // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectKeyword". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectKeywordTableAdapter.Fill(this.dataSetDiversityProjects.ProjectKeyword);
                // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectAgentRole". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectAgentRoleTableAdapter.Fill(this.dataSetDiversityProjects.ProjectAgentRole);
                // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectLabel". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectLabelTableAdapter.Fill(this.dataSetDiversityProjects.ProjectLabel);
                // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectIdentifierType". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectIdentifierTypeTableAdapter.Fill(this.dataSetDiversityProjects.ProjectIdentifierType);
                // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectIdentifier". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectIdentifierTableAdapter.Fill(this.dataSetDiversityProjects.ProjectIdentifier);
                // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectLicense". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectLicenseTableAdapter.Fill(this.dataSetDiversityProjects.ProjectLicense);
                // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectSetting". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectSettingTableAdapter.Fill(this.dataSetDiversityProjects.ProjectSetting);
                // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectReference". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectReferenceTableAdapter.Fill(this.dataSetDiversityProjects.ProjectReference);
                // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectResource". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectResourceTableAdapter.Fill(this.dataSetDiversityProjects.ProjectResource);
                // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetDiversityProjects.ProjectAgent". Sie können sie bei Bedarf verschieben oder entfernen.
                //this.projectAgentTableAdapter.Fill(this.dataSetDiversityProjects.ProjectAgent);
            }
            catch (System.Exception ex) { }
        }

        private void FormDiversityProjects_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.askOnExitToolStripMenuItem.Checked)
            {
                if (MessageBox.Show("Do you really want to exit DiversityProjects?", "Exit application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // Save data
            this.updateProject();

            // Save settings
            Properties.Settings.Default.AskOnExit = this.askOnExitToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();

            if (this.userControlQueryList.ManyOrderByColumns_Allow())
            {
                DiversityWorkbench.Settings.ManyOrderColumnSave(DiversityWorkbench.Settings.ModuleName, "FormCollectionSpecimen", this.userControlQueryList.ManyOrderByColumns_Widths());
            }
        }

        //private void setUserControlDatabindings()
        //{
        //    try
        //    {
        //        // References
        //        DiversityWorkbench.Reference L = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
        //        this.userControlModuleRelatedEntryReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        this.userControlModuleRelatedEntryReference.bindToData("ProjectReference", "ReferenceTitle", "RefURI", this.projectReferenceBindingSource);

        //        this.userControlModuleRelatedEntryHierarchyReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        this.userControlModuleRelatedEntryHierarchyReference.bindToData("TaxonHierarchy", "HierarchyRefText", "HierarchyRefURI", this.taxonHierarchyBindingSource);

        //        this.userControlModuleRelatedEntryReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        this.userControlModuleRelatedEntryReference.bindToData("TaxonName", "ReferenceTitle", "ReferenceURI", this.taxonNameBindingSource);

        //        //this.userControlModuleRelatedEntryProtologue.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        //this.userControlModuleRelatedEntryProtologue.bindToData("TaxonName", "Protologue", "ProtologueURI", this.taxonNameBindingSource);

        //        this.userControlModuleRelatedEntrySynonymReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        this.userControlModuleRelatedEntrySynonymReference.bindToData("TaxonSynonymy", "SynRefText", "SynRefURI", this.taxonSynonymyBindingSource);

        //        this.userControlModuleRelatedEntryTypificationReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        this.userControlModuleRelatedEntryTypificationReference.bindToData("TaxonNameTypification", "TypificationReferenceTitle", "TypificationReferenceURI", this.taxonNameTypificationBindingSource);

        //        this.userControlModuleRelatedEntryCommonNameReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        this.userControlModuleRelatedEntryCommonNameReference.bindToData("TaxonCommonName", "ReferenceTitle", "ReferenceURI", this.taxonCommonNameBindingSource);

        //        this.userControlModuleRelatedEntryTaxonListReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        this.userControlModuleRelatedEntryTaxonListReference.bindToData("TaxonNameListReference", "TaxonNameListRefText", "TaxonNameListRefURI", this.taxonNameListReferenceBindingSource);

        //        this.userControlModuleRelatedEntryTaxonReferences.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        this.userControlModuleRelatedEntryTaxonReferences.bindToData("TaxonNameReference", "TaxonNameRefText", "TaxonNameRefURI", this.taxonNameReferenceBindingSource);//.TaxonReferenceBindingSource);

        //        DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);

        //        this.userControlModuleRelatedEntryListDistributionPlace.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;
        //        this.userControlModuleRelatedEntryListDistributionPlace.bindToData("TaxonNameListDistribution", "PlaceNameCache", "PlaceURI", this.taxonNameListDistributionBindingSource);
        //        this.userControlModuleRelatedEntryListDistributionPlace.buttonDeleteURI.Enabled = false;

        //        //this.userControlModuleRelatedEntryListAnalysisReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        //this.userControlModuleRelatedEntryListAnalysisReference.bindToData("TaxonNameListAnalysis", "ReferenceTitle", "ReferenceURI", this.taxonNameListAnalysisBindingSource);

        //        //this.userControlModuleRelatedEntryListDistributionReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        //this.userControlModuleRelatedEntryListDistributionReference.bindToData("TaxonNameListDistribution", "ReferenceTitle", "ReferenceURI", this.taxonNameListDistributionBindingSource);


        //        DiversityWorkbench.CollectionSpecimen CS = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);

        //        this.userControlModuleRelatedEntryTaxonListCollectionSpecimen.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)CS;
        //        this.userControlModuleRelatedEntryTaxonListCollectionSpecimen.bindToData("TaxonNameListCollectionSpecimen", "DisplayText", "CollectionSpecimenURI", this.taxonNameListCollectionSpecimenBindingSource);

        //        // GENUS
        //        this.userControlLocalListGenus.textBoxValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonNameBindingSource, "GenusOrSupragenericName", true));
        //        // REFERENCE
        //        //  TaxonName - Reference
        //        //this.userControlModuleRelatedEntryReference.textBoxURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonNameBindingSource, "ReferenceURI", true));
        //        //this.userControlModuleRelatedEntryReference.textBoxValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonNameBindingSource, "ReferenceTitle", true));
        //        //this.userControlModuleRelatedEntryReference.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonNameBindingSource, "ReferenceTitle", true));
        //        //this.userControlModuleRelatedEntryReference.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.taxonNameBindingSource, "ReferenceTitle", true));
        //        //  TaxonName - Reference
        //        //this.userControlModuleRelatedEntryTypificationReference.textBoxURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonNameBindingSource, "TypificationReferenceURI", true));
        //        //this.userControlModuleRelatedEntryTypificationReference.textBoxValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonNameBindingSource, "TypificationReferenceTitle", true));
        //        //this.userControlModuleRelatedEntryTypificationReference.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonNameBindingSource, "TypificationReferenceTitle", true));
        //        //this.userControlModuleRelatedEntryTypificationReference.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.taxonNameBindingSource, "TypificationReferenceTitle", true));
        //        //  TaxonName - Protologue
        //        //this.userControlModuleRelatedEntryProtologue.textBoxURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonNameBindingSource, "ProtologueResourceURI", true));
        //        //this.userControlModuleRelatedEntryProtologue.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
        //        //this.userControlModuleRelatedEntryProtologue.bindToData("TaxonName", "ProtologueResourceURI", "ProtologueResourceURI", this.taxonNameBindingSource);
        //        //this.userControlModuleRelatedEntryProtologue.textBoxValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonNameBindingSource, "ProtologueResourceURI", true));
        //        //this.userControlModuleRelatedEntryProtologue.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonNameBindingSource, "ProtologueResourceURI", true));
        //        //this.userControlModuleRelatedEntryProtologue.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.taxonNameBindingSource, "ProtologueResourceURI", true));
        //        //  TaxonName - ProtologueURI
        //        this.userControlURIProtologue.textBoxURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonNameBindingSource, "ProtologueURI", true));
        //        //  Hierarchy
        //        //this.userControlModuleRelatedEntryHierarchyReference.textBoxURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonHierarchyBindingSource, "HierarchyRefURI", true));
        //        //this.userControlModuleRelatedEntryHierarchyReference.textBoxValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonHierarchyBindingSource, "HierarchyRefText", true));
        //        //this.userControlModuleRelatedEntryHierarchyReference.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonHierarchyBindingSource, "HierarchyRefText", true));
        //        //this.userControlModuleRelatedEntryHierarchyReference.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.taxonHierarchyBindingSource, "HierarchyRefText", true));

        //        /// Test 14.7.2016 -offenbar schon gebunden
        //        //  Accepted Name
        //        //this.userControlModuleRelatedEntryAcceptedNameReference.labelURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonAcceptedNameBindingSource, "RefURI", true));
        //        //this.userControlModuleRelatedEntryAcceptedNameReference.textBoxValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonAcceptedNameBindingSource, "RefText", true));
        //        //this.userControlModuleRelatedEntryAcceptedNameReference.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonAcceptedNameBindingSource, "RefText", true));
        //        //this.userControlModuleRelatedEntryAcceptedNameReference.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.taxonAcceptedNameBindingSource, "RefText", true));
        //        //  Synonymy
        //        //this.userControlModuleRelatedEntrySynonymReference.labelURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonSynonymyBindingSource, "SynRefURI", true));
        //        //this.userControlModuleRelatedEntrySynonymReference.textBoxValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonSynonymyBindingSource, "SynRefText", true));
        //        //this.userControlModuleRelatedEntrySynonymReference.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.taxonSynonymyBindingSource, "SynRefText", true));
        //        //this.userControlModuleRelatedEntrySynonymReference.comboBoxLocalValues.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.taxonSynonymyBindingSource, "SynRefText", true));

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}



        #endregion

        #region Toolbar and Permissions

        private void setToolbarPermissions()
        {
            try
            {
                bool OK;
                string TableName = "";
                TableName = "Project";
                // Check UPDATE
                OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                this.splitContainerData.Panel1.Enabled = OK;
                this.splitContainerSettings.Panel1.Enabled = OK;

                // Settings
                TableName = "ProjectSetting";
                OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                this.tableLayoutPanelSettings.Enabled = OK;

                // Agents
                TableName = "ProjectAgent";
                OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                this.splitContainerAgents.Panel2.Enabled = OK;
                this.toolStripAgents.Enabled = OK;

                // ProjectLicense
                TableName = "ProjectLicense";
                OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                this.splitContainerLicense.Panel2.Enabled = OK;
                this.toolStripLicense.Enabled = OK;

                // ProjectResource
                TableName = "ProjectResource";
                OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                this.splitContainerResources.Panel2.Enabled = OK;
                this.toolStripResources.Enabled = OK;

                // ProjectReference
                TableName = "ProjectReference";
                OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                this.splitContainerReference.Panel2.Enabled = OK;
                this.toolStripReference.Enabled = OK;

                // ProjectIdentifier
                TableName = "ProjectIdentifier";
                OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                this.splitContainerID.Panel2.Enabled = OK;
                this.toolStripID.Enabled = OK;

                // ProjectArchive
                TableName = "ProjectArchive";
                OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                this.splitContainerArchive.Panel2.Enabled = OK;
                this.toolStripArchive.Enabled = OK;

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private enum Permission { SELECT, INSERT, UPDATE, DELETE }
        private bool getObjectPermissions(string ObjectName, Permission Permission)
        {
            bool Permit = false;
            string SQL = "IF PERMISSIONS(OBJECT_ID('" + ObjectName + "'))&";
            switch (Permission)
            {
                case FormDiversityProjects.Permission.SELECT:
                    SQL += "1=1";
                    break;
                case FormDiversityProjects.Permission.INSERT:
                    SQL += "2=2";
                    break;
                case FormDiversityProjects.Permission.UPDATE:
                    SQL += "8=8";
                    break;
                case FormDiversityProjects.Permission.DELETE:
                    SQL += "16=16";
                    break;
            }
            SQL += " SELECT 'True' ELSE SELECT 'False'";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            C.CommandText = SQL;
            try
            {
                con.Open();
                Permit = System.Boolean.Parse(C.ExecuteScalar().ToString());
            }
            finally
            {
                con.Close();
            }
            return Permit;
        }

        #endregion

        #region Properties

        internal DiversityWorkbench.Project DiversityProject
        {
            get
            {
                DiversityWorkbench.Project P = new DiversityWorkbench.Project(this.ServerConnection);
                return P;
            }
        }

        public int ID { get { return this.ProjectID; } }

        public int ProjectID
        {
            get
            {
                int ID = -1;
                if (this.dataSetDiversityProjects.Project.Rows.Count > 0)
                {
                    if (this.treeViewProjects.SelectedNode != null)
                    {
                        if (this.treeViewProjects.SelectedNode.Tag != null)
                        {
                            System.Data.DataRow R = (System.Data.DataRow)this.treeViewProjects.SelectedNode.Tag;
                            ID = int.Parse(R["ProjectID"].ToString());
                            return ID;
                        }
                    }
                    ID = System.Int32.Parse(this.dataSetDiversityProjects.Project.Rows[0]["ProjectID"].ToString());
                }
                return ID;
            }
        }

        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get
            {
                DiversityWorkbench.ServerConnection S = DiversityWorkbench.Settings.ServerConnection;
                return S;
            }
        }

        #endregion

        #region Databindings and events

        private void setDatabindings()
        {
            try
            {
                this.setUserControlDatabindings();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setUserControlDatabindings()
        {
            try
            {
                // Agent
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryProjectAgent.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryProjectAgent.bindToData("ProjectAgent", "AgentName", "AgentURI", this.projectAgentBindingSource);

                this.userControlModuleRelatedEntryLicenseHolder.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryLicenseHolder.bindToData("ProjectLicense", "LicenseHolder", "LicenseHolderAgentURI", this.projectLicenseBindingSource);

                this.userControlModuleRelatedEntryCopyrightHolder.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryCopyrightHolder.bindToData("ProjectLicense", "CopyrightHolder", "CopyrightHolderAgentURI", this.projectLicenseBindingSource);

                this.userControlModuleRelatedEntryIPRHolder.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryIPRHolder.bindToData("ProjectLicense", "IPRHolder", "IPRHolderAgentURI", this.projectLicenseBindingSource);

                // Reference
                DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)R;
                this.userControlModuleRelatedEntryReference.bindToData("ProjectReference", "ReferenceTitle", "ReferenceURI", this.projectReferenceBindingSource);

                // Gazetteer
                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorGazetteer.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;
                this.userControlModuleRelatedEntryDescriptorGazetteer.bindToData("ProjectDescriptor", "Content", "ContentURI", this.projectDescriptorBindingSource);

                // Terms
                DiversityWorkbench.ScientificTerm S = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorTerm.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S;
                this.userControlModuleRelatedEntryDescriptorTerm.bindToData("ProjectDescriptor", "Content", "ContentURI", this.projectDescriptorBindingSource);

                // Taxa
                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorTaxon.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;
                this.userControlModuleRelatedEntryDescriptorTaxon.bindToData("ProjectDescriptor", "Content", "ContentURI", this.projectDescriptorBindingSource);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Modules

        private void initRemoteModules()
        {
            try
            {
                DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);

                // Agents
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                if (this.userControlModuleRelatedEntryAgent != null)
                    this.userControlModuleRelatedEntryAgent.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;

                this.userControlModuleRelatedEntryLicenseHolder.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryCopyrightHolder.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryIPRHolder.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;

                // Gazetteer
                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorGazetteer.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;

                // Reference
                DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)R;

                // Terms
                DiversityWorkbench.ScientificTerm S = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorTerm.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S;

                // Taxa
                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorTaxon.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;

                this.setHelpProviders();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setHelpProviders()
        {
            try
            {
                // Agents
                if (this.userControlModuleRelatedEntryAgent != null)
                    this.userControlModuleRelatedEntryAgent.HelpProvider.HelpNamespace = this.helpProviderDiversityProjects.HelpNamespace;

                this.userControlModuleRelatedEntryLicenseHolder.HelpProvider.HelpNamespace = this.helpProviderDiversityProjects.HelpNamespace;
                this.userControlModuleRelatedEntryIPRHolder.HelpProvider.HelpNamespace = this.helpProviderDiversityProjects.HelpNamespace;
                this.userControlModuleRelatedEntryCopyrightHolder.HelpProvider.HelpNamespace = this.helpProviderDiversityProjects.HelpNamespace;

                // Reference
                this.userControlModuleRelatedEntryReference.HelpProvider.HelpNamespace = this.helpProviderDiversityProjects.HelpNamespace;

                // Gazetteer
                this.userControlModuleRelatedEntryDescriptorGazetteer.HelpProvider.HelpNamespace = this.helpProviderDiversityProjects.HelpNamespace;

                // Terms
                this.userControlModuleRelatedEntryDescriptorTerm.HelpProvider.HelpNamespace = this.helpProviderDiversityProjects.HelpNamespace;

                // Taxa
                this.userControlModuleRelatedEntryDescriptorTaxon.HelpProvider.HelpNamespace = this.helpProviderDiversityProjects.HelpNamespace;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Project

        private void linkLabelURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.linkLabelURL.Text.Length > 0)
            {
                DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                DiversityWorkbench.Forms.FormRemoteQuery f = new DiversityWorkbench.Forms.FormRemoteQuery(this.ID, P);
                f.ShowDialog();
            }
        }

        private System.Collections.Generic.List<int> _ProjectIDList;
        private System.Collections.Generic.List<int> ProjectIDList
        {
            get
            {
                if (this._ProjectIDList == null)
                {
                    this._Hierarchy.IDs = new List<int>();
                    this._ProjectIDList = new List<int>();
                    string SQL = "SELECT ProjectID " +
                        "FROM  Project_Core";
                    System.Data.DataTable dt = new DataTable();
                    try
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            this._ProjectIDList.Add(int.Parse(R[0].ToString()));
                            this._Hierarchy.IDs.Add(int.Parse(R[0].ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        this._ProjectIDList = null;
                    }
                }
                return this._ProjectIDList;
            }
        }

        private void ResetProjectIDList()
        {
            this._ProjectIDList = null;
            int i = this.ProjectIDList.Count;
        }

        private bool setProject(int ProjectID)
        {
            bool OK = true;
            bool ReadOnly = false;
            try
            {
                if (this.userControlQueryList.listBoxQueryResult.SelectedIndex == -1)
                {
                    this.dataSetDiversityProjects.Clear();
                    return false;
                }
                if (!this.ProjectIDList.Contains(ProjectID) && this.userControlQueryList.listBoxQueryResult.SelectedIndex > -1 && ProjectID != -1)
                {
                    bool UpdateOfProjectListDetectsProject = false;
                    string SQL = "SELECT COUNT(*) FROM dbo.ProjectList() WHERE ProjectID = " + ProjectID.ToString();
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result == "1")
                    {
                        this.ResetProjectIDList();
                        if (this.ProjectIDList.Contains(ProjectID))
                        {
                            UpdateOfProjectListDetectsProject = true;
                        }
                    }
                    if (!UpdateOfProjectListDetectsProject)
                    {
                        System.Windows.Forms.MessageBox.Show("You have no access to this dataset");
                        return false;
                    }
                }
                if (this.dataSetDiversityProjects.Project.Rows.Count > 0)
                {
                    this.updateProject();
                }
                this.fillProject(ProjectID);
                if (this.dataSetDiversityProjects.Project.Rows.Count > 0)
                {
                    this.splitContainerData.Visible = true;
                    int i = 0;
                    foreach (System.Data.DataRow r in this.dataSetDiversityProjects.Project.Rows)
                    {
                        if (r[0].ToString() == ProjectID.ToString())
                            break;
                        i++;
                    }
                    this.projectBindingSource.Position = i;
                    this.initReferenceLists();
                    this.dateTimePickerEmbargoDate.Format = DateTimePickerFormat.Custom;
                    string SQL = "";
                    // Check if column ReadOnly exists
                    SQL = "select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'ProjectUser' and c.COLUMN_NAME = 'ReadOnly'";
                    int c;
                    bool ColumnReadOnlyPresent = int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out c) && c == 1;
                    if (ColumnReadOnlyPresent)
                    {
                        SQL = "SELECT ReadOnly FROM ProjectUser WHERE LoginName = '";
                        if (DiversityWorkbench.Settings.IsTrustedConnection)
                        {
                            SQL += System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        }
                        else
                        {
                            SQL += DiversityWorkbench.Settings.DatabaseUser;
                        }

                        SQL += "' AND ProjectID = " + ProjectID.ToString();
                        if (bool.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out ReadOnly) && ReadOnly)
                        {
                            this.splitContainerData.Enabled = false;
                        }
                        else
                            this.splitContainerData.Enabled = true;
                    }
                    else
                    {
                        this.splitContainerData.Enabled = true;
                    }
                }
                else
                {
                    this.splitContainerData.Visible = false;
                }
                this.scanModules();
                this.linkLabelURL.Text = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.ID.ToString();

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private void fillProject(int ProjectID)
        {
            try
            {
                this.dataSetDiversityProjects.Clear();
                this.treeViewProjects.Nodes.Clear();
                //this.textBoxXmlSettings.Text = "";
                //this.textBoxXmlSetting.Text = "";
                string WhereClause = " WHERE ProjectID = " + ProjectID.ToString();

                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProject, DiversityProjects.Project.SqlProject + WhereClause, this.dataSetDiversityProjects.Project);
                this.dataSetDiversityProjects.Project.Clear();

                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectAgent, DiversityProjects.Project.SqlProjectAgent + WhereClause, this.dataSetDiversityProjects.ProjectAgent);
                this.dataSetDiversityProjects.ProjectAgent.Clear();

                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectAgentRole, DiversityProjects.Project.SqlProjectAgentRole + WhereClause + " ORDER BY AgentRole", this.dataSetDiversityProjects.ProjectAgentRole);
                this.dataSetDiversityProjects.ProjectAgentRole.Clear();

                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectResource, DiversityProjects.Project.SqlProjectResource + WhereClause, this.dataSetDiversityProjects.ProjectResource);
                this.dataSetDiversityProjects.ProjectResource.Clear();

                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectReference, DiversityProjects.Project.SqlProjectReference + WhereClause, this.dataSetDiversityProjects.ProjectReference);
                this.dataSetDiversityProjects.ProjectReference.Clear();

                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectSetting, DiversityProjects.Project.SqlProjectSetting + WhereClause, this.dataSetDiversityProjects.ProjectSetting);
                this.dataSetDiversityProjects.ProjectSetting.Clear();

                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectLicense, DiversityProjects.Project.SqlProjectLicense + WhereClause, this.dataSetDiversityProjects.ProjectLicense, ConflictOption.OverwriteChanges);
                this.dataSetDiversityProjects.ProjectLicense.Clear();

                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectIdentifier, DiversityProjects.Project.SqlProjectIdentifier + WhereClause, this.dataSetDiversityProjects.ProjectIdentifier);
                this.dataSetDiversityProjects.ProjectIdentifier.Clear();

                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectDescriptor, DiversityProjects.Project.SqlProjectDescriptor + WhereClause, this.dataSetDiversityProjects.ProjectDescriptor);
                this.dataSetDiversityProjects.ProjectDescriptor.Clear();

                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectArchive, DiversityProjects.Project.SqlProjectArchive + WhereClause, this.dataSetDiversityProjects.ProjectArchive);
                this.dataSetDiversityProjects.ProjectArchive.Clear();

                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectArchiveIdentifier, DiversityProjects.Project.SqlProjectArchiveIdentifier + WhereClause, this.dataSetDiversityProjects.ProjectArchiveIdentifier);
                this.dataSetDiversityProjects.ProjectArchiveIdentifier.Clear();

                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT " + DiversityProjects.Project.SqlProjectFields + " FROM ProjectHierarchy (" + ProjectID.ToString() + ")", DiversityWorkbench.Settings.ConnectionString);
                a.Fill(this.dataSetDiversityProjects.Project);

                this._sqlDataAdapterProjectAgent.Fill(this.dataSetDiversityProjects.ProjectAgent);
                this._sqlDataAdapterProjectAgentRole.Fill(this.dataSetDiversityProjects.ProjectAgentRole);
                this.setAgentSequence();
                this._sqlDataAdapterProjectResource.Fill(this.dataSetDiversityProjects.ProjectResource);
                this._sqlDataAdapterProjectReference.Fill(this.dataSetDiversityProjects.ProjectReference);
                this._sqlDataAdapterProjectLicense.Fill(this.dataSetDiversityProjects.ProjectLicense);
                this._sqlDataAdapterProjectIdentifier.Fill(this.dataSetDiversityProjects.ProjectIdentifier);
                try
                {
                    this._sqlDataAdapterProjectDescriptor.Fill(this.dataSetDiversityProjects.ProjectDescriptor);
                    this._sqlDataAdapterProjectArchive.Fill(this.dataSetDiversityProjects.ProjectArchive);
                    this._sqlDataAdapterProjectArchiveIdentifier.Fill(this.dataSetDiversityProjects.ProjectArchiveIdentifier);
                    this.buildArchiveTree();
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                this._sqlDataAdapterProjectSetting.Fill(this.dataSetDiversityProjects.ProjectSetting);

                this._Hierarchy.buildHierarchy(ProjectID);
                //this.buildSettingTree();
                this.buildSettingTreeWithParents();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the Project", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        private void updateProject()
        {
            try
            {
                if (this.projectBindingSource.Current != null)
                {
                    System.Data.DataRowView RI = (System.Data.DataRowView)this.projectBindingSource.Current;
                    RI.BeginEdit();
                    RI.EndEdit();
                }
                this.projectBindingSource.EndEdit();
                this.projectAgentBindingSource.EndEdit();
                this.projectResourceBindingSource.EndEdit();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            string Message = "";
            System.Data.DataRowView RProject = (System.Data.DataRowView)this.projectBindingSource.Current;
            if (RProject != null)
            {
                try
                {
                    RProject.BeginEdit();
                    RProject.EndEdit();
                }
                catch { }
            }
            this.FormFunctions.updateTable(this.dataSetDiversityProjects, "Project", this._sqlDataAdapterProject, this.projectBindingSource);


            this.UpdateAgentAndRole();

            if (this.projectResourceBindingSource != null && this.projectResourceBindingSource.Current != null)
            {
                System.Data.DataRowView RResource = (System.Data.DataRowView)this.projectResourceBindingSource.Current;
                if (RResource != null)
                {
                    try
                    {
                        RResource.BeginEdit();
                        RResource.EndEdit();
                    }
                    catch { }
                }
                this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectResource", this._sqlDataAdapterProjectResource, this.projectResourceBindingSource);
            }

            if (this.projectLicenseBindingSource != null && this.projectLicenseBindingSource.Current != null)
            {
                System.Data.DataRowView RLicense = (System.Data.DataRowView)this.projectLicenseBindingSource.Current;
                if (RLicense != null)
                {
                    try
                    {
                        RLicense.BeginEdit();
                        RLicense.EndEdit();
                    }
                    catch { }
                }
                this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectLicense", this._sqlDataAdapterProjectLicense, this.projectLicenseBindingSource);
            }

            if (this.projectIdentifierBindingSource != null && this.projectIdentifierBindingSource.Current != null)
            {
                System.Data.DataRowView RIdentifier = (System.Data.DataRowView)this.projectIdentifierBindingSource.Current;
                if (RIdentifier != null)
                {
                    try
                    {
                        RIdentifier.BeginEdit();
                        RIdentifier.EndEdit();
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                else
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("FormDiversityProjects", "updateProject()", "this.projectIdentifierBindingSource.Current == null");
                }
                this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectIdentifier", this._sqlDataAdapterProjectIdentifier, this.projectIdentifierBindingSource);
            }

            if (this.projectDescriptorBindingSource != null && this.projectDescriptorBindingSource.Current != null)
            {
                System.Data.DataRowView RDescriptor = (System.Data.DataRowView)this.projectDescriptorBindingSource.Current;
                if (RDescriptor != null)
                {
                    try
                    {
                        RDescriptor.BeginEdit();
                        RDescriptor.EndEdit();
                    }
                    catch { }
                }
                this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectDescriptor", this._sqlDataAdapterProjectDescriptor, this.projectDescriptorBindingSource);
            }


            if (this.projectArchiveBindingSource != null && this.projectArchiveBindingSource.Current != null)
            {
                System.Data.DataRowView RArchive = (System.Data.DataRowView)this.projectArchiveBindingSource.Current;
                if (RArchive != null)
                {
                    try
                    {
                        RArchive.BeginEdit();
                        RArchive.EndEdit();
                    }
                    catch { }
                }
                this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectArchive", this._sqlDataAdapterProjectArchive, this.projectArchiveBindingSource);
            }


            if (this.projectArchiveIdentifierBindingSource != null && this.projectArchiveIdentifierBindingSource.Current != null)
            {
                System.Data.DataRowView RArchiveIdentifier = (System.Data.DataRowView)this.projectArchiveIdentifierBindingSource.Current;
                if (RArchiveIdentifier != null)
                {
                    try
                    {
                        RArchiveIdentifier.BeginEdit();
                        RArchiveIdentifier.EndEdit();
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectArchiveIdentifier", this._sqlDataAdapterProjectArchiveIdentifier, this.projectArchiveIdentifierBindingSource);
            }
            if (this.projectArchiveIdentifierBindingSource != null)
            {
                bool OK = this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectArchiveIdentifier", this._sqlDataAdapterProjectArchiveIdentifier, this.projectArchiveIdentifierBindingSource);
            }
            else
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("FormDiversityProjects", "updateProject()", "this.projectArchiveIdentifierBindingSource == null");


            try
            {
                if (this.projectReferenceBindingSource != null && this.projectReferenceBindingSource.Current != null)
                {
                    System.Data.DataRowView RReference = (System.Data.DataRowView)this.projectReferenceBindingSource.Current;
                    RReference.BeginEdit();
                    RReference.EndEdit();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectReference", this._sqlDataAdapterProjectReference, this.projectReferenceBindingSource);


            try
            {
                if (this.projectSettingBindingSource != null && this.projectSettingBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.projectSettingBindingSource.Current;
                    R.BeginEdit();
                    R.EndEdit();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectSetting", this._sqlDataAdapterProjectSetting, this.projectSettingBindingSource);
        }


        private void UpdateAgentAndRole()
        {
            System.Data.DataRowView RAgent = (System.Data.DataRowView)this.projectAgentBindingSource.Current;
            if (RAgent != null)
            {
                try
                {
                    RAgent.BeginEdit();
                    RAgent.EndEdit();
                }
                catch { }
            }
            if (!this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectAgent", this._sqlDataAdapterProjectAgent, this.projectAgentBindingSource))
            {

            }

            System.Data.DataRowView RAgentRole = (System.Data.DataRowView)this.projectAgentRoleBindingSource.Current;
            if (RAgentRole != null)
            {
                try
                {
                    RAgentRole.BeginEdit();
                    RAgentRole.EndEdit();
                }
                catch { }
            }
            if (!this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectAgentRole", this._sqlDataAdapterProjectAgentRole, this.projectAgentRoleBindingSource))
            {

            }
        }

        private void SetDependentTables(int ProjectID)
        {
            this.updateProject();
            this.dataSetDiversityProjects.ProjectAgent.Clear();
            this.dataSetDiversityProjects.ProjectAgentRole.Clear();
            this.dataSetDiversityProjects.ProjectIdentifier.Clear();
            //this.dataSetDiversityProjects.ProjectKeyword.Clear();
            //this.dataSetDiversityProjects.ProjectLabel.Clear();
            this.dataSetDiversityProjects.ProjectLicense.Clear();
            this.dataSetDiversityProjects.ProjectResource.Clear();
            this.dataSetDiversityProjects.ProjectReference.Clear();
            this.dataSetDiversityProjects.ProjectSetting.Clear();
            this.dataSetDiversityProjects.ProjectDescriptor.Clear();
            string WhereClause = " WHERE ProjectID = " + ProjectID.ToString();
            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectAgent, DiversityProjects.Project.SqlProjectAgent + WhereClause + " ORDER BY AgentSequence", this.dataSetDiversityProjects.ProjectAgent);
            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectAgentRole, DiversityProjects.Project.SqlProjectAgentRole + WhereClause, this.dataSetDiversityProjects.ProjectAgentRole);
            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectIdentifier, DiversityProjects.Project.SqlProjectIdentifier + WhereClause, this.dataSetDiversityProjects.ProjectIdentifier);
            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectDescriptor, DiversityProjects.Project.SqlProjectDescriptor + WhereClause + " ORDER BY Content", this.dataSetDiversityProjects.ProjectDescriptor);
            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectArchive, DiversityProjects.Project.SqlProjectArchive + WhereClause, this.dataSetDiversityProjects.ProjectArchive);
            //this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectKeyword, DiversityProjects.Project.SqlProjectKeyword + WhereClause, this.dataSetDiversityProjects.ProjectKeyword);
            //this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectLabel, DiversityProjects.Project.SqlProjectLabel + WhereClause, this.dataSetDiversityProjects.ProjectLabel);
            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectLicense, DiversityProjects.Project.SqlProjectLicense + WhereClause, this.dataSetDiversityProjects.ProjectLicense);
            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectResource, DiversityProjects.Project.SqlProjectResource + WhereClause, this.dataSetDiversityProjects.ProjectResource);
            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectReference, DiversityProjects.Project.SqlProjectReference + WhereClause, this.dataSetDiversityProjects.ProjectReference);
            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectSetting, DiversityProjects.Project.SqlProjectSetting + WhereClause, this.dataSetDiversityProjects.ProjectSetting);
        }

        private void buttonURL_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURL.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxURL.Text = f.URL;
        }

        private void comboBoxProjectDescriptionType_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT ";
            if (this.comboBoxProjectDescriptionType.Text.Length == 0)
                SQL += "NULL AS ProjectDescriptionType UNION SELECT ";
            SQL += " DISTINCT ProjectDescriptionType FROM Project WHERE ProjectDescriptionType <> '' ";
            if (this.comboBoxProjectDescriptionType.Text.Length > 0)
                SQL += " AND ProjectDescriptionType LIKE '" + this.comboBoxProjectDescriptionType.Text + "%' ";
            SQL += " ORDER BY ProjectDescriptionType";
            System.Data.DataTable dt = new DataTable();
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            this.comboBoxProjectDescriptionType.DataSource = dt;
            this.comboBoxProjectDescriptionType.DisplayMember = "ProjectDescriptionType";
            this.comboBoxProjectDescriptionType.ValueMember = "ProjectDescriptionType";
        }

        #region Embargo

        private void dateTimePickerEmbargoDate_ValueChanged(object sender, EventArgs e)
        {
            this.setEmbargoDateFormat();
        }

        private void dateTimePickerEmbargoDate_Validating(object sender, CancelEventArgs e)
        {
            this.setEmbargoDateFormat();
        }

        private void dateTimePickerEmbargoDate_CloseUp(object sender, EventArgs e)
        {
            if (this.projectBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectBindingSource.Current;
                R["EmbargoDate"] = dateTimePickerEmbargoDate.Value;
            }
            this.setEmbargoDateFormat();
        }

        private void buttonEmbargoDate_Click(object sender, EventArgs e)
        {
            if (this.projectBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectBindingSource.Current;
                R["EmbargoDate"] = System.DBNull.Value;
            }
            this.setEmbargoDateFormat();
        }

        private void setEmbargoDateFormat()
        {
            if (this.projectBindingSource.Current == null)
            {
                this.dateTimePickerEmbargoDate.CustomFormat = "--";
                this.dateTimePickerEmbargoDate.CalendarMonthBackground = System.Drawing.SystemColors.Window;
            }
            else
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectBindingSource.Current;
                if (R["EmbargoDate"].Equals(System.DBNull.Value))
                {
                    this.dateTimePickerEmbargoDate.CustomFormat = "--";
                    this.dateTimePickerEmbargoDate.CalendarMonthBackground = System.Drawing.SystemColors.Window;
                    this.labelEmbargoDate.BackColor = System.Drawing.SystemColors.Control;
                    this.labelEmbargoDate.ForeColor = System.Drawing.SystemColors.WindowText;
                }
                else
                {
                    this.dateTimePickerEmbargoDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                    System.DateTime DT;
                    System.DateTime.TryParse(R["EmbargoDate"].ToString(), out DT);
                    if (System.DateTime.Now > DT)
                    {
                        this.dateTimePickerEmbargoDate.CalendarMonthBackground = System.Drawing.SystemColors.Window;
                        this.labelEmbargoDate.BackColor = System.Drawing.SystemColors.Control;
                        this.labelEmbargoDate.ForeColor = System.Drawing.SystemColors.WindowText;
                    }
                    else
                    {
                        this.dateTimePickerEmbargoDate.CalendarMonthBackground = System.Drawing.Color.Pink;
                        this.labelEmbargoDate.BackColor = System.Drawing.Color.Red;
                        this.labelEmbargoDate.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
        }

        #endregion

        private void comboBoxProjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Data.DataRowView r = (System.Data.DataRowView)this.comboBoxProjectType.SelectedItem;
            if (r != null)
            {
                if (r["Code"].Equals(System.DBNull.Value))
                {
                    this.labelProjectType.Image = null;
                }
                else
                {
                    string Type = r["Code"].ToString().ToLower();
                    switch (Type)
                    {
                        case "agents":
                            this.labelProjectType.Image = DiversityProjects.Properties.Resources.DiversityAgents;
                            break;
                        case "collection":
                            this.labelProjectType.Image = DiversityProjects.Properties.Resources.DiversityCollection;
                            break;
                        case "descriptions":
                            this.labelProjectType.Image = DiversityProjects.Properties.Resources.DiversityDescriptions;
                            break;
                        case "gazetteer":
                            this.labelProjectType.Image = DiversityProjects.Properties.Resources.DiversityGazetteer;
                            break;
                        case "references":
                            this.labelProjectType.Image = DiversityProjects.Properties.Resources.DiversityReferences;
                            break;
                        case "regulation":
                            this.labelProjectType.Image = DiversityProjects.Properties.Resources.Paragraph;
                            break;
                        case "samplingplots":
                            this.labelProjectType.Image = DiversityProjects.Properties.Resources.DiversitySamplingPlots;
                            break;
                        case "scientificterms":
                            this.labelProjectType.Image = DiversityProjects.Properties.Resources.DiversityScientificTerms;
                            break;
                        case "taxonnames":
                            this.labelProjectType.Image = DiversityProjects.Properties.Resources.DiversityTaxonNames;
                            break;
                        default:
                            this.labelProjectType.Image = null;
                            break;
                    }
                }
            }
        }

        #endregion

        #region Agent

        private void toolStripButtonAgentNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.UpdateAgentAndRole();
                DiversityProjects.DataSetDiversityProjects.ProjectAgentRow R = this.dataSetDiversityProjects.ProjectAgent.NewProjectAgentRow();
                R.ProjectID = this.ID;
                R.AgentName = "Agent " + (this.dataSetDiversityProjects.ProjectAgent.Rows.Count + 1).ToString();
                R.AgentURI = "";
                int Sequence = 1;
                if (this.dataSetDiversityProjects.ProjectAgent.Rows.Count > 0)
                {
                    System.Data.DataRow[] rr = this.dataSetDiversityProjects.ProjectAgent.Select("", "AgentSequence DESC");
                    int i;
                    if (int.TryParse(rr[0]["AgentSequence"].ToString(), out i))
                        Sequence = i + 1;
                }
                R.AgentSequence = Sequence;
                this.dataSetDiversityProjects.ProjectAgent.Rows.Add(R);
                this.UpdateAgentAndRole();
                this.dataSetDiversityProjects.ProjectAgent.Clear();
                this._sqlDataAdapterProjectAgent.Fill(this.dataSetDiversityProjects.ProjectAgent);

                //this.updateProject();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonAgentDelete_Click(object sender, EventArgs e)
        {
            if (this.projectAgentBindingSource.Current != null)
                this.projectAgentBindingSource.RemoveCurrent();
            else System.Windows.Forms.MessageBox.Show("nothing selected");
        }

        private System.Data.DataView _ViewAgentRole;

        private void listBoxAgents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxAgents.SelectedIndex == -1)
                this.tableLayoutPanelAgent.Enabled = false;
            else
            {
                this.setAgentRoleView();
                ////this.UpdateAgentAndRole();
                //string Filter = "";
                //if (this.projectAgentBindingSource.Current != null)
                //{
                //    System.Data.DataRowView R = (System.Data.DataRowView)this.projectAgentBindingSource.Current;
                //    Filter = "ProjectAgentID = " + R["ProjectAgentID"].ToString();
                //    //Filter = "AgentName = '" + R["AgentName"].ToString() + "' AND AgentURI = '" + R["AgentURI"].ToString() + "'";
                //}
                //this._ViewAgentRole = new DataView(this.dataSetDiversityProjects.ProjectAgentRole, Filter, "AgentRole", DataViewRowState.CurrentRows);
                ////this._ViewAgentRole.RowFilter = Filter;
                ////this._ViewAgentRole.Sort = "AgentRole";
                //this.listBoxRoles.DataSource = this._ViewAgentRole;
                //this.listBoxRoles.DisplayMember = "AgentRole";
                //this.listBoxRoles.ValueMember = "AgentRole";
                //this.tableLayoutPanelAgent.Enabled = true;
            }
        }

        private void setAgentRoleView()
        {
            string Filter = "";
            if (this.projectAgentBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectAgentBindingSource.Current;
                Filter = "ProjectAgentID = " + R["ProjectAgentID"].ToString();
            }
            this._ViewAgentRole = new DataView(this.dataSetDiversityProjects.ProjectAgentRole, Filter, "AgentRole", DataViewRowState.CurrentRows);
            this.listBoxRoles.DataSource = this._ViewAgentRole;
            this.listBoxRoles.DisplayMember = "AgentRole";
            this.listBoxRoles.ValueMember = "AgentRole";
            this.tableLayoutPanelAgent.Enabled = true;
        }

        private void toolStripButtonAgentMoveToTop_Click(object sender, EventArgs e)
        {
            this.setAgentSequence(true);
        }

        private void toolStripButtonAgentMoveToBase_Click(object sender, EventArgs e)
        {
            this.setAgentSequence(false);
        }

        private void setAgentSequence()
        {
            if (this.dataSetDiversityProjects.ProjectAgent.Rows.Count > 0)
            {
                if (this.dataSetDiversityProjects.ProjectAgent.Rows[0]["AgentSequence"].Equals(System.DBNull.Value))
                {
                    this.projectAgentBindingSource.Position = 0;
                    this.setAgentSequence(true);
                }
            }
        }

        private void setAgentSequence(bool ToTop)
        {
            try
            {
                // getting the selected row
                System.Data.DataRowView RtoMove = (System.Data.DataRowView)this.projectAgentBindingSource.Current;
                // finding the min and max Sequence
                int iMin = 0;
                int iMax = 0;
                System.Data.DataRow[] RR = this.dataSetDiversityProjects.ProjectAgent.Select("not AgentSequence is null", "AgentSequence");
                if (RR.Length > 0)
                {
                    iMin = int.Parse(RR[0]["AgentSequence"].ToString());
                    iMax = int.Parse(RR[RR.Length - 1]["AgentSequence"].ToString());
                }
                // get rows with missing sequence
                System.Data.DataRow[] RRnull = this.dataSetDiversityProjects.ProjectAgent.Select("AgentSequence is null");
                if (RRnull.Length > 0)
                {
                    if (ToTop)
                    {
                        int i = iMax;
                        foreach (System.Data.DataRow R in RRnull)
                        {
                            i++;
                            R["AgentSequence"] = i;
                        }
                    }
                    else
                    {
                        int i = iMin;
                        foreach (System.Data.DataRow R in RRnull)
                        {
                            i--;
                            R["AgentSequence"] = i;
                        }
                    }
                }
                if (ToTop)
                    RtoMove["AgentSequence"] = iMin - 1;
                else
                    RtoMove["AgentSequence"] = iMax + 1;
                this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectAgent", this._sqlDataAdapterProjectAgent, this.projectAgentBindingSource);
                this.dataSetDiversityProjects.ProjectAgent.Clear();
                string WhereClause = " WHERE ProjectID = " + ProjectID.ToString() + " ORDER BY AgentSequence";
                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectAgent, DiversityProjects.Project.SqlProjectAgent + WhereClause, this.dataSetDiversityProjects.ProjectAgent);
            }
            catch (System.Exception ex)
            { }
        }

        private void listBoxAgents_Validated(object sender, EventArgs e)
        {
            //string Filter = "";
            //if (this.dataSetDiversityProjects.ProjectAgentRole.Count > 0)
            //{
            //    if (this.projectAgentBindingSource.Current != null)
            //    {
            //        System.Data.DataRowView R = (System.Data.DataRowView)this.projectAgentBindingSource.Current;
            //        Filter = "AgentName = '" + R["AgentName"].ToString() + "' AND AgentURI = '" + R["AgentURI"].ToString() + "'";
            //    }
            //    else
            //    {
            //        System.Data.DataRow R = this.dataSetDiversityProjects.ProjectAgentRole.Rows[0];
            //        Filter = "AgentName = '" + R["AgentName"].ToString() + "' AND AgentURI = '" + R["AgentURI"].ToString() + "'";
            //    }
            //}
            //this._ViewAgentRole = new DataView(this.dataSetDiversityProjects.ProjectAgentRole, Filter, "AgentRole", DataViewRowState.Unchanged);
            //this.listBoxRoles.DataSource = this._ViewAgentRole;
            //this.listBoxRoles.DisplayMember = "AgentRole";
            //this.listBoxRoles.ValueMember = "AgentRole";
        }

        #region Roles

        private void toolStripButtonRolesAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string Error = "";
                System.Data.DataTable dt = new DataTable();
                System.Data.DataRowView RV = (System.Data.DataRowView)this.projectAgentBindingSource.Current;
                string SQL = "SELECT Code, CASE WHEN Code = DisplayText THEN DisplayText ELSE DisplayText + ' (= ' + Code + ')' END AS DisplayText FROM ProjectAgentRole_Enum " +
                    "WHERE Code NOT IN (SELECT AgentRole FROM ProjectAgentRole " +
                    "WHERE ProjectID = " + RV["ProjectID"].ToString() +
                    " AND AgentName = '" + RV["AgentName"].ToString() + "' " +
                    " AND AgentURI = '" + RV["AgentURI"].ToString() + "')";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Error);
                if (dt.Rows.Count > 0)
                {
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "Code", "New role", "Please select the new role");
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        System.Data.DataRow R = this.dataSetDiversityProjects.ProjectAgentRole.NewRow();
                        R["ProjectAgentID"] = RV["ProjectAgentID"].ToString();
                        R["ProjectID"] = RV["ProjectID"].ToString();
                        R["AgentName"] = RV["AgentName"].ToString();
                        R["AgentURI"] = RV["AgentURI"].ToString();
                        R["AgentRole"] = f.SelectedValue;
                        this.dataSetDiversityProjects.ProjectAgentRole.Rows.Add(R);
                        //this.UpdateAgentAndRole();
                        //this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectAgentRole", this._sqlDataAdapterProjectAgentRole, this.projectAgentRoleBindingSource);
                    }
                }
                else
                    System.Windows.Forms.MessageBox.Show("No further role available");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonRolesDelete_Click(object sender, EventArgs e)
        {
            if (this.listBoxRoles.SelectedItem != null)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxRoles.SelectedItem;
                RV.Delete();
            }
        }

        private void listBoxRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxRoles.SelectedValue != null &&
                this.AgentRoles().ContainsKey(this.listBoxRoles.SelectedValue.ToString()))
            {
                this.labelAgentRoleDisplayText.Visible = true;
                this.labelAgentRoleDisplayText.Text = "= " + this.AgentRoles()[this.listBoxRoles.SelectedValue.ToString()];
            }
            else
            {
                this.labelAgentRoleDisplayText.Text = "";
                this.labelAgentRoleDisplayText.Visible = false;
            }
        }

        private System.Collections.Generic.Dictionary<string, string> _AgentRoles;

        private System.Collections.Generic.Dictionary<string, string> AgentRoles()
        {
            if (this._AgentRoles == null)
            {
                this._AgentRoles = new Dictionary<string, string>();
                string SQL = "SELECT Code, DisplayText FROM ProjectAgentRole_Enum " +
                    "WHERE Code <> DisplayText";
                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    this._AgentRoles.Add(R[0].ToString(), R[1].ToString());
                }
            }
            return this._AgentRoles;
        }
        #endregion

        #endregion

        #region History

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.projectBindingSource.Current;
            string Title = "History of " + R["Project"].ToString() + " (ProjectID: " + R["ProjectID"].ToString() + ")";
            try
            {
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProjectID", this.dataSetDiversityProjects.Project.TableName, ""));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProjectID", this.dataSetDiversityProjects.ProjectAgent.TableName, ""));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProjectID", this.dataSetDiversityProjects.ProjectResource.TableName, ""));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProjectID", this.dataSetDiversityProjects.ProjectReference.TableName, ""));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProjectID", this.dataSetDiversityProjects.ProjectLicense.TableName, ""));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProjectID", this.dataSetDiversityProjects.ProjectIdentifier.TableName, ""));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProjectID", this.dataSetDiversityProjects.ProjectSetting.TableName, ""));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProjectID", this.dataSetDiversityProjects.ProjectDescriptor.TableName, ""));

                //if (this.dataSetDiversityProjects.Project.Rows.Count > 0)
                //    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProjectID", this.dataSetDiversityProjects.Project.TableName, ""));
                //if (this.dataSetDiversityProjects.ProjectAgent.Rows.Count > 0)
                //    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProjectID", this.dataSetDiversityProjects.ProjectAgent.TableName, ""));
                //if (this.dataSetDiversityProjects.ProjectResource.Rows.Count > 0)
                //    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProjectID", this.dataSetDiversityProjects.ProjectResource.TableName, ""));

                //System.Data.DataTable dtProject_Log = new DataTable("Project");
                //string SqlCurrent = "SELECT " + DiversityProjects.Project.SqlProjectFields + ", 'current version' AS KindOfChange, " +
                //      "LogUpdatedWhen AS DateOfChange, case when LogUpdatedBy = 'dbo' then 'Administator' else LogUpdatedBy end AS ResponsibleUser " +
                //      "FROM Project " +
                //      "WHERE ProjectID = " + this.ID.ToString();
                //string SQL = "SELECT " + DiversityProjects.Project.SqlProjectFields + ", CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS KindOfChange, " +
                //      "LogDate AS DateOfChange, case when LogUser = 'dbo' then 'Administator' else LogUser end AS ResponsibleUser " +
                //      "FROM Project_log " +
                //      "WHERE ProjectID = " + this.ID.ToString() +
                //      " ORDER BY DateOfChange DESC";
                //try
                //{
                //    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                //    a.Fill(dtProject_Log);
                //    a.SelectCommand.CommandText = SQL;
                //    a.Fill(dtProject_Log);
                //    LogTables.Add(dtProject_Log);
                //}
                //catch (Exception ex)
                //{
                //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                //}

                DiversityWorkbench.Forms.FormHistory f = new DiversityWorkbench.Forms.FormHistory(Title, LogTables, System.Windows.Forms.Application.StartupPath + "\\DiversityProjects.chm");
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
            }

            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Hierarchy

        private System.Collections.Generic.Dictionary<string, int> _ImageKeys = null;
        System.Collections.Generic.Dictionary<string, int> ImageKeys
        {
            get
            {
                /*
                Agents
                Checklist
                Collection
                Descriptions
                Gazetteer
                References
                Regulation
                SamplingPlots
                ScientificTerms
                TaxonNames
                */

                if (_ImageKeys == null)
                {
                    _ImageKeys = new Dictionary<string, int>();
                    _ImageKeys.Add("", 0);
                    _ImageKeys.Add("Agents", 1);
                    _ImageKeys.Add("Collection", 2);
                    _ImageKeys.Add("Regulation", 3);
                    _ImageKeys.Add("Descriptions", 4);
                    _ImageKeys.Add("Gazetteer", 5);
                    _ImageKeys.Add("Projects", 6);
                    _ImageKeys.Add("References", 7);
                    _ImageKeys.Add("SamplingPlots", 8);
                    _ImageKeys.Add("ScientificTerms", 9);
                    _ImageKeys.Add("TaxonNames", 10);
                    _ImageKeys.Add("Checklist", 11);
                }
                return _ImageKeys;
            }
        }

        private void toolStripButtonProjectSetParent_Click(object sender, EventArgs e)
        {
            string SqlHierarchy = "SELECT ProjectID FROM dbo.ProjectHierarchy(" + this.ID.ToString() + ")";
            System.Data.DataTable dtProjectID = DiversityWorkbench.Forms.FormFunctions.DataTable(SqlHierarchy);
            string IDs = "";
            foreach(System.Data.DataRow R in dtProjectID.Rows)
            {
                if (IDs.Length > 0)
                    IDs += ", ";
                IDs += R[0].ToString();
            }

            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT NULL AS Project, NULL AS ProjectID UNION SELECT Project, ProjectID FROM Project WHERE ProjectID NOT IN(" + IDs + ") ORDER BY Project"; ;
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Select a project from the list");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                int ParentID;
                if (int.TryParse(f.SelectedValue.ToString(), out ParentID))
                {
                    if (ParentID != this.ID)
                    {
                        System.Data.DataRow R = this.dataSetDiversityProjects.Project.FindByProjectID(this.ID);
                        R["ProjectParentID"] = ParentID;
                        this.setProject(this.ID);
                    }
                }
                else if (f.SelectedValue.ToString() == "NULL" || f.SelectedValue.ToString() == "")
                {
                    System.Data.DataRow R = this.dataSetDiversityProjects.Project.FindByProjectID(this.ID);
                    R.BeginEdit();
                    R["ProjectParentID"] = System.DBNull.Value;
                    R.EndEdit();
                    this.setProject(this.ID);
                }
            }
        }

        private void toolStripButtonProjectNew_Click(object sender, EventArgs e)
        {
            if (this.treeViewProjects.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return;
            }
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewProjects.SelectedNode.Tag;
                int ParentID = int.Parse(R["ProjectID"].ToString());
                string Parent = R["Project"].ToString();
                string Project = "";
                if (System.Windows.Forms.MessageBox.Show(
                    "Do you want to create a new Project as child of the project " + Parent,
                    "New Project?",
                    System.Windows.Forms.MessageBoxButtons.OKCancel,
                    System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int ID = this.InsertNewProject(ref Project, ParentID);
                    if (ID > -1)
                    {
                        this.setProject(ID);
                        if (Project.Length > 0)
                            this.userControlQueryList.AddListItem(ID, Project);
                        else
                            this.userControlQueryList.AddListItem(ID, "ID: " + ID.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonProjectDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewProjects.SelectedNode.Tag;
                string Project = R["Project"].ToString();
                int ProjectID = int.Parse(R["ProjectID"].ToString());
                if (System.Windows.Forms.MessageBox.Show(
                    "Do you want to delete the Project " + Project,
                    "Delete Project?",
                    System.Windows.Forms.MessageBoxButtons.OKCancel,
                    System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    this.userControlQueryList.RemoveListItem(ProjectID);
                    this.deleteProject(ProjectID);
                    R.Delete();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void treeViewProjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            System.Data.DataRow R = (System.Data.DataRow)this.treeViewProjects.SelectedNode.Tag;
            int ProjectID = 0;
            if (int.TryParse(R["ProjectID"].ToString(), out ProjectID))
            {
                try
                {
                    this.SetDependentTables(ProjectID);
                    this.buildSettingTreeWithParents();
                    this.setHierarchyToolStripPermissions();
                    this.setEmbargoDateFormat();
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
        }

        private void toolStripButtonProjectRemoveParent_Click(object sender, EventArgs e)
        {
            System.Data.DataRow R = (System.Data.DataRow)this.treeViewProjects.SelectedNode.Tag;
            R.BeginEdit();
            R["ProjectParentID"] = System.DBNull.Value;
            R.EndEdit();
            this.setProject(this.ID);
        }

        private void setHierarchyToolStripPermissions()
        {
            this.toolStripButtonProjectNew.Enabled = DiversityWorkbench.Forms.FormFunctions.Permissions("Project", DiversityWorkbench.Forms.FormFunctions.Permission.INSERT);
            this.toolStripButtonProjectDelete.Enabled = DiversityWorkbench.Forms.FormFunctions.Permissions("Project", DiversityWorkbench.Forms.FormFunctions.Permission.DELETE);
            this.toolStripButtonProjectRemoveParent.Enabled = DiversityWorkbench.Forms.FormFunctions.Permissions("Project", DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE);
            this.toolStripButtonProjectSetParent.Enabled = DiversityWorkbench.Forms.FormFunctions.Permissions("Project", DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE);
        }

        #endregion

        #region XML Settings

        #region Obsolet

        private void buttonSettingsAdd_Click(object sender, EventArgs e)
        {
            //if (this.treeViewSettings.Nodes.Count > 0
            //    && this.treeViewSettings.SelectedNode == null)
            //{
            //    System.Windows.Forms.MessageBox.Show("Please select the parent entry for the new setting");
            //    return;
            //}
            //DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New setting", "Please enter the name of the new setting", "");
            //f.ShowDialog();
            //if (f.DialogResult == DialogResult.OK)
            //{
            //    if (this.treeViewSettings.Nodes.Count == 0)
            //        this.treeViewSettings.Nodes.Add(f.String);
            //    else
            //    {
            //        if (this.treeViewSettings.SelectedNode == null)
            //        {
            //            System.Windows.Forms.MessageBox.Show("Please select the parent entry for the new setting");
            //            return;
            //        }
            //        else
            //        {
            //            this.treeViewSettings.SelectedNode.Nodes.Add(f.String);
            //            this.treeViewSettings.SelectedNode.Expand();
            //        }
            //    }
            //    this.SaveSettings();
            //}
        }

        private void buttonSettingsRemove_Click(object sender, EventArgs e)
        {
            //if (this.treeViewSettings.SelectedNode != null)
            //{
            //    this.treeViewSettings.SelectedNode.Remove();
            //    this.SaveSettings();
            //}
            //else
            //    System.Windows.Forms.MessageBox.Show("Please select the setting you want to remove");
        }

        private void buttonSettingsCopyFromParent_Click(object sender, EventArgs e)
        {


            //System.Data.DataRowView RV = (System.Data.DataRowView)this.projectBindingSource.Current;
            //if (RV["ProjectParentID"].Equals(System.DBNull.Value))
            //{
            //    System.Windows.Forms.MessageBox.Show("No parent found");
            //    return;
            //}
            //System.Data.DataRow[] RR = this.dataSetDiversityProjects.Project.Select("ProjectID = " + RV["ProjectParentID"].ToString());
            //if (RR.Length > 0)
            //{
            //    if (RR[0]["ProjectSettings"].Equals(System.DBNull.Value)
            //        ||
            //        RR[0]["ProjectSettings"].ToString().Length == 0)
            //        System.Windows.Forms.MessageBox.Show("Parent contains no settings");
            //    else
            //    {
            //        RV["ProjectSettings"] = RR[0]["ProjectSettings"].ToString();
            //        this.buildSettingsTree();
            //    }
            //}
            //else
            //    System.Windows.Forms.MessageBox.Show("No parent found");
        }

        #endregion

        private void buildXmlSettingsTree()
        {
            //try
            //{
            //    System.Data.DataRow[] RR = this.dataSetDiversityProjects.Project.Select("ProjectID = " + ProjectID.ToString());
            //    if (RR.Length > 0)
            //    {
            //        if (!RR[0]["ProjectSettings"].Equals(System.DBNull.Value))
            //        {
            //            string Settings = RR[0]["ProjectSettings"].ToString();
            //            this.buildXmlSettingsTree(Settings);
            //        }
            //        else this.treeViewXmlSettings.Nodes.Clear();
            //    }
            //    else this.treeViewXmlSettings.Nodes.Clear();
            //}
            //catch { }
        }

        private void buildXmlSettingsTree(string XML)
        {
            //try
            //{
            //    this.treeViewXmlSettings.Nodes.Clear();
            //    System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(XML, System.Xml.XmlNodeType.Element, null);
            //    System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
            //    dom.Load(tr);
            //    if (dom.DocumentElement == null) return;
            //    if (dom.DocumentElement.ChildNodes.Count >= 0)
            //    {
            //        // SECTION 2. Initialize the TreeView control.
            //        this.treeViewXmlSettings.Nodes.Clear();
            //        //if (dom.DocumentElement.Name.Length > 0)
            //        this.treeViewXmlSettings.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name));
            //        System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
            //        tNode = this.treeViewXmlSettings.Nodes[0];
            //        // SECTION 3. Populate the TreeView with the DOM nodes.
            //        this.AddNodeForXmlTree(dom.DocumentElement, tNode);
            //        this.treeViewXmlSettings.ExpandAll();
            //    }
            //    else
            //    {
            //        if (dom.DocumentElement.Attributes["error_message"].InnerText.Length > 0)
            //        {
            //            string Message = dom.DocumentElement.Attributes["error_message"].InnerText;
            //            System.Windows.Forms.MessageBox.Show(Message);
            //        }
            //    }
            //}
            //catch { }
        }

        private void AddNodeForXmlTree(System.Xml.XmlNode inXmlNode, System.Windows.Forms.TreeNode inTreeNode)
        {
            System.Xml.XmlNode xNode;
            System.Windows.Forms.TreeNode tNode;
            System.Xml.XmlNodeList nodeList;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new System.Windows.Forms.TreeNode(xNode.Name));
                    if (i >= inTreeNode.Nodes.Count)
                        continue;
                    tNode = inTreeNode.Nodes[i];
                    this.AddNodeForXmlTree(xNode, tNode);
                }
            }
            else
            {
                if (inXmlNode.Value != null)
                {
                    inTreeNode.Parent.Tag = inXmlNode.Value;
                    inTreeNode.Parent.Text = inTreeNode.Parent.Text + ": " + inXmlNode.Value;
                    inTreeNode.Remove();
                }
            }
            this.SaveXmlSettings();
        }

        private void treeViewXmlSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (this.treeViewXmlSettings.SelectedNode.Tag != null)
            //    this.textBoxXmlSettings.Text = this.treeViewXmlSettings.SelectedNode.Tag.ToString();
            //else this.textBoxXmlSettings.Text = "";
            //string Node = this.treeViewXmlSettings.SelectedNode.Text;
            //if (Node.IndexOf(":") > -1)
            //    Node = Node.Substring(0, Node.IndexOf(":"));
            //this.textBoxXmlSetting.Text = Node;
        }

        private void textBoxXmlSettings_Leave(object sender, EventArgs e)
        {
            //bool SettingsMustBeSaved = false;
            //if ((this.treeViewXmlSettings.SelectedNode.Tag == null && this.textBoxXmlSettings.Text.Length > 0)
            //    || this.treeViewXmlSettings.SelectedNode.Tag.ToString() != this.textBoxXmlSettings.Text)
            //    SettingsMustBeSaved = true;
            //this.treeViewXmlSettings.SelectedNode.Tag = this.textBoxXmlSettings.Text;
            //if (this.treeViewXmlSettings.SelectedNode.Text.IndexOf(":") > -1)
            //    this.treeViewXmlSettings.SelectedNode.Text = this.treeViewXmlSettings.SelectedNode.Text.Substring(0, this.treeViewXmlSettings.SelectedNode.Text.IndexOf(":")) + ": " + this.textBoxXmlSettings.Text;
            //else
            //    this.treeViewXmlSettings.SelectedNode.Text = this.treeViewXmlSettings.SelectedNode.Text + ": " + this.textBoxXmlSettings.Text;
            //if (SettingsMustBeSaved)
            //    this.SaveXmlSettings();
        }

        private void textBoxXmlSetting_Leave(object sender, EventArgs e)
        {
            //bool SettingsMustBeSaved = false;
            //string Node = this.treeViewXmlSettings.SelectedNode.Text;
            //if (Node.IndexOf(":") > -1)
            //    Node = Node.Substring(0, Node.IndexOf(":"));
            //if ((this.textBoxXmlSetting.Text.Length > 0)
            //    || Node.ToString() != this.textBoxXmlSetting.Text)
            //    SettingsMustBeSaved = true;
            //this.treeViewXmlSettings.SelectedNode.Text = this.textBoxXmlSetting.Text;
            //if (this.textBoxXmlSettings.Text.Length > 0)
            //    this.treeViewXmlSettings.SelectedNode.Text += ": " + this.textBoxXmlSettings.Text;
            //if (SettingsMustBeSaved)
            //    this.SaveXmlSettings();
        }

        private string XmlFromTree()
        {
            //System.IO.FileInfo XmlFile = new System.IO.FileInfo(System.Windows.Forms.Application.StartupPath + "\\ProjectSettings.xml");

            string XML = "";
            //if (this.treeViewXmlSettings.Nodes.Count == 0) return "";
            //System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            //System.Xml.XmlWriter W;
            //System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            //settings.Encoding = System.Text.Encoding.Unicode;
            //W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
            //W.WriteStartElement(this.treeViewXmlSettings.Nodes[0].Text);
            //foreach (System.Windows.Forms.TreeNode N in this.treeViewXmlSettings.Nodes[0].Nodes)
            //{
            //    this.XmlFromTreeAddChild(N, ref W);
            //}
            //W.WriteEndElement();
            //W.Flush();
            //W.Close();
            //System.IO.StreamReader R = new System.IO.StreamReader(XmlFile.FullName);
            //XML = R.ReadToEnd();
            //R.Close();
            return XML;
        }

        private void XmlFromTreeAddChild(System.Windows.Forms.TreeNode TreeNode, ref System.Xml.XmlWriter W)
        {
            string Node = TreeNode.Text;
            if (Node.IndexOf(":") > -1) Node = Node.Substring(0, Node.IndexOf(":"));
            if (TreeNode.Tag != null)
                W.WriteElementString(Node, TreeNode.Tag.ToString());
            else
            {
                W.WriteStartElement(Node);
                foreach (System.Windows.Forms.TreeNode NChild in TreeNode.Nodes)
                    this.XmlFromTreeAddChild(NChild, ref W);
                W.WriteEndElement();
            }
        }

        private void treeViewXmlSettings_Leave(object sender, EventArgs e)
        {
            //this.SaveSettings();
        }

        private void SaveXmlSettings()
        {
            // Save settings
            System.Data.DataRowView RV = (System.Data.DataRowView)this.projectBindingSource.Current;
            if (RV["ProjectID"].ToString() == this.ProjectID.ToString())
            {
                string XML = this.XmlFromTree();
                RV["ProjectSettings"] = this.XmlFromTree();
            }
        }

        private string _XmlSettings;
        private string XmlSettings
        {
            get { return this._XmlSettings; }
            set
            {
                this._XmlSettings = value;
                this.buildXmlSettingsTree(this._XmlSettings);
            }
        }

        private void toolStripButtonXmlSettingAdd_Click(object sender, EventArgs e)
        {
            //if (this.treeViewXmlSettings.Nodes.Count > 0
            //    && this.treeViewXmlSettings.SelectedNode == null)
            //{
            //    System.Windows.Forms.MessageBox.Show("Please select the parent entry for the new setting");
            //    return;
            //}
            //DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New setting", "Please enter the name of the new setting", "");
            //f.ShowDialog();
            //if (f.DialogResult == DialogResult.OK)
            //{
            //    if (this.treeViewXmlSettings.Nodes.Count == 0)
            //        this.treeViewXmlSettings.Nodes.Add(f.String);
            //    else
            //    {
            //        if (this.treeViewXmlSettings.SelectedNode == null)
            //        {
            //            System.Windows.Forms.MessageBox.Show("Please select the parent entry for the new setting");
            //            return;
            //        }
            //        else
            //        {
            //            this.treeViewXmlSettings.SelectedNode.Nodes.Add(f.String);
            //            this.treeViewXmlSettings.SelectedNode.Expand();
            //        }
            //    }
            //    this.SaveXmlSettings();
            //}
        }

        private void toolStripButtonXmlSettingRemove_Click(object sender, EventArgs e)
        {
            //if (this.treeViewXmlSettings.SelectedNode != null)
            //{
            //    this.treeViewXmlSettings.SelectedNode.Remove();
            //    this.SaveXmlSettings();
            //}
            //else
            //    System.Windows.Forms.MessageBox.Show("Please select the setting you want to remove");
        }

        private void toolStripButtonXmlSettingSearch_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormSelectXmlTemplate f = new DiversityWorkbench.Forms.FormSelectXmlTemplate(
              this.DtXmlSettings, "Project", "ProjectSettings");
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.Template.Length > 0)
            {
                this.XmlSettings = f.Template;
            }
        }

        private System.Data.DataTable _dtXmlSettings;

        public System.Data.DataTable DtXmlSettings
        {
            get
            {
                if (this._dtXmlSettings == null)
                {
                    this._dtXmlSettings = new DataTable();
                    try
                    {
                        string SQL = "SELECT     Project, ProjectSettings " +
                            "FROM  Project " +
                            "WHERE (CAST(ProjectSettings AS nvarchar(MAX)) <> '')" +
                            "ORDER BY Project";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(this._dtXmlSettings);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return _dtXmlSettings;
            }
            //set { _dtSettings = value; }
        }

        #endregion

        #region Resources

        private void listBoxResources_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bool EnableControls = true;
            if (this.listBoxResources.SelectedIndex == -1)
            {
                //EnableControls = false;
                this.labelFileName.Text = "Resources";
                //this.groupBoxFile.Text = "Resources";
                this.pictureBoxResource.Image = null;
                this.pictureBoxResource.Width = 1;
                this.pictureBoxResource.Visible = false;
                this.tableLayoutPanelResources.Enabled = false;
            }
            else
            {
                try
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxResources.SelectedItem;
                    string File = R["URI"].ToString();
                    if (File.StartsWith("http"))
                    {

                    }
                    else
                    {
                        System.IO.FileInfo F = new System.IO.FileInfo(File);
                        switch (F.Extension.ToLower())
                        {
                            case ".jpg":
                            case ".png":
                            case ".gif":
                                System.Drawing.Bitmap B = new Bitmap(F.FullName);
                                this.pictureBoxResource.Image = B;
                                this.pictureBoxResource.Visible = true;
                                this.pictureBoxResource.Height = (int)(this.tableLayoutPanelResources.Height / 2);
                                break;
                            default:
                                this.pictureBoxResource.Height = 1;
                                this.pictureBoxResource.Visible = false;
                                break;
                        }
                    }
                    this.tableLayoutPanelResources.Enabled = true;
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void toolStripButtonResourcesNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.openFileDialog == null)
                {
                    this.openFileDialog = new OpenFileDialog();
                }
                if (this.openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (this.openFileDialog.FileName.Length > 0)
                    {
                        DiversityProjects.DataSetDiversityProjects.ProjectResourceRow R = this.dataSetDiversityProjects.ProjectResource.NewProjectResourceRow();
                        R.ProjectID = this.ID;
                        R.URI = this.openFileDialog.FileName;
                        this.dataSetDiversityProjects.ProjectResource.Rows.Add(R);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripButtonResourceAddLink_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser();
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.URL.Length > 0)
                {
                    DiversityProjects.DataSetDiversityProjects.ProjectResourceRow R = this.dataSetDiversityProjects.ProjectResource.NewProjectResourceRow();
                    R.ProjectID = this.ID;
                    R.URI = f.URL;
                    this.dataSetDiversityProjects.ProjectResource.Rows.Add(R);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripButtonResourceOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxResources.SelectedItem == null)
                {
                    MessageBox.Show("Please select a resource to open.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var rowView = (System.Data.DataRowView)this.listBoxResources.SelectedItem;
                string filePath = rowView["URI"]?.ToString();

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    MessageBox.Show("The resource path is invalid.", "Invalid Path",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"File not found:\n{filePath}", "File Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open resource:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButtonResourcesDelete_Click(object sender, EventArgs e)
        {
            if (this.listBoxResources.SelectedItem != null)
            {
                System.Data.DataRowView r = (System.Data.DataRowView)this.listBoxResources.SelectedItem;
                r.Row.Delete();
            }
            else
                System.Windows.Forms.MessageBox.Show("Nothing selected");
        }

        private void pictureBoxResource_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxResources.SelectedItem;
                string File = R["URI"].ToString();
                DiversityWorkbench.Forms.FormImage f = new DiversityWorkbench.Forms.FormImage(File);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        private void buttonResourceLicense_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxResourceLicense.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxResourceLicense.Text = f.URL;
        }

        private void buttonResourceOpen_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxResources.SelectedItem;
                string File = R["URI"].ToString();
                DiversityWorkbench.Forms.FormImage f = new DiversityWorkbench.Forms.FormImage(File);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region References

        private void initReferenceLists()
        {
            System.Data.DataView dvReference = new DataView(this.dataSetDiversityProjects.ProjectReference, "IsCitation = false", "ReferenceTitle", DataViewRowState.CurrentRows);
            this.listBoxReference.DataSource = dvReference;
            this.listBoxReference.DisplayMember = "ReferenceTitle";
            this.listBoxReference.ValueMember = "ReferenceTitle";

            System.Data.DataView dvCitation = new DataView(this.dataSetDiversityProjects.ProjectReference, "IsCitation = true", "ReferenceTitle", DataViewRowState.CurrentRows);
            this.listBoxCitations.DataSource = dvCitation;
            this.listBoxCitations.DisplayMember = "ReferenceTitle";
            this.listBoxCitations.ValueMember = "ReferenceTitle";
        }

        private void listBoxReference_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxReference.SelectedIndex == -1)
            {
                this.tableLayoutPanelReference.Enabled = false;
                return;
            }
            else
            {
                this.listBoxCitations.SelectedIndex = -1;
                this.tableLayoutPanelReference.Enabled = true;
                try
                {
                    int i = 0;
                    System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxReference.SelectedItem;
                    foreach (System.Data.DataRow R in this.dataSetDiversityProjects.ProjectReference.Rows)
                    {
                        if (RV.Row.RowState != DataRowState.Deleted && R.RowState != DataRowState.Deleted && RV["ReferenceTitle"].ToString() == R["ReferenceTitle"].ToString())
                        {
                            this.projectReferenceBindingSource.Position = i;
                            break;
                        }
                        i++;
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void toolStripButtonReferenceNew_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityProjects.DataSetDiversityProjects.ProjectReferenceRow R = this.dataSetDiversityProjects.ProjectReference.NewProjectReferenceRow();
                R.ProjectID = this.ID;
                R.ReferenceTitle = "Reference " + (this.dataSetDiversityProjects.ProjectReference.Rows.Count + 1).ToString();
                R.ReferenceURI = "";
                R.ReferenceDetails = "";
                R.IsCitation = false;
                this.dataSetDiversityProjects.ProjectReference.Rows.Add(R);
                this.initReferenceLists();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonReferenceDelete_Click(object sender, EventArgs e)
        {
            if (this.projectReferenceBindingSource.Current != null && this.listBoxReference.SelectedIndex > -1)
            {
                this.projectReferenceBindingSource.RemoveCurrent();
                this.UpdateReferences();
                this.initReferenceLists();
            }
            else System.Windows.Forms.MessageBox.Show("nothing selected");
        }

        private void comboBoxReferenceType_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT NULL AS [ReferenceType] " +
                "UNION ";
            if (this.comboBoxReferenceType.Text != null && this.comboBoxReferenceType.Text.Length > 0)
                SQL += "SELECT '" + this.comboBoxReferenceType.Text + "' AS [ReferenceType] " +
                "UNION ";
            SQL += "SELECT DISTINCT [ReferenceType] " +
                "FROM [dbo].[ProjectReference] ";
            foreach (string s in this.CitationTypes)
            {
                SQL += " UNION SELECT '" + s + "' AS  [ReferenceType] ";
            }
            SQL += "ORDER BY [ReferenceType]";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            System.Data.DataTable dt = new DataTable();
            ad.Fill(dt);
            this.comboBoxReferenceType.DataSource = dt;
            this.comboBoxReferenceType.DisplayMember = "ReferenceType";
            this.comboBoxReferenceType.ValueMember = "ReferenceType";
        }

        private void buttonReferenceUri_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxReferenceUri.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxReferenceUri.Text = f.URL;
        }

        private void UpdateReferences()
        {
            try
            {
                if (this.projectReferenceBindingSource != null && this.projectReferenceBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.projectReferenceBindingSource.Current;
                    R.BeginEdit();
                    R.EndEdit();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectReference", this._sqlDataAdapterProjectReference, this.projectReferenceBindingSource);
            string WhereClause = " WHERE ProjectID = " + ProjectID.ToString();
            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProjectReference, DiversityProjects.Project.SqlProjectReference + WhereClause, this.dataSetDiversityProjects.ProjectReference);
        }

        #region Citations

        private void toolStripButtonCitationAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityProjects.DataSetDiversityProjects.ProjectReferenceRow R = this.dataSetDiversityProjects.ProjectReference.NewProjectReferenceRow();
                R.ProjectID = this.ID;
                R.ReferenceTitle = "Reference " + (this.dataSetDiversityProjects.ProjectReference.Rows.Count + 1).ToString();
                R.ReferenceURI = "";
                R.ReferenceDetails = "";
                R.IsCitation = true;
                this.dataSetDiversityProjects.ProjectReference.Rows.Add(R);
                this.initReferenceLists();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonCitationDelete_Click(object sender, EventArgs e)
        {
            if (this.projectReferenceBindingSource.Current != null && this.listBoxCitations.SelectedIndex > -1)
            {
                this.projectReferenceBindingSource.RemoveCurrent();
                this.UpdateReferences();
                this.initReferenceLists();
            }
            else System.Windows.Forms.MessageBox.Show("nothing selected");
        }

        private void toolStripButtonCitationToReference_Click(object sender, EventArgs e)
        {
            if (this.listBoxCitations.SelectedIndex == -1)
                System.Windows.Forms.MessageBox.Show("Nothing selected");
            else
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectReferenceBindingSource.Current;
                R.BeginEdit();
                R["IsCitation"] = false;
                R.EndEdit();
                this.initReferenceLists();
            }
        }

        private void toolStripButtonReferenceToCitation_Click(object sender, EventArgs e)
        {
            if (this.listBoxReference.SelectedIndex == -1)
                System.Windows.Forms.MessageBox.Show("Nothing selected");
            else
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectReferenceBindingSource.Current;
                R.BeginEdit();
                R["IsCitation"] = true;
                R.EndEdit();
                this.initReferenceLists();
            }
        }

        private void listBoxCitations_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.labelReferenceTypeDependentCitation.Text = "";
            if (this.listBoxCitations.SelectedIndex == -1)
            {
                this.tableLayoutPanelReference.Enabled = false;
                return;
            }
            else
            {
                this.listBoxReference.SelectedIndex = -1;
                this.tableLayoutPanelReference.Enabled = true;
                int i = 0;
                System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxCitations.SelectedItem;
                foreach (System.Data.DataRow R in this.dataSetDiversityProjects.ProjectReference.Rows)
                {
                    if (RV["ReferenceTitle"].ToString() == R["ReferenceTitle"].ToString())
                    {
                        this.projectReferenceBindingSource.Position = i;
                        //this.labelReferenceTypeDependentCitation.Text = this.GenerateCitation();
                        break;
                    }
                    i++;
                }
            }
        }

        private System.Collections.Generic.List<string> _CitationTypes;
        private System.Collections.Generic.List<string> CitationTypes
        {
            get
            {
                if (this._CitationTypes == null)
                {
                    this._CitationTypes = new List<string>();
                    this._CitationTypes.Add("BioCASe (GFBio)");
                    this._CitationTypes.Add("BioCASe (GBIF)");
                }
                return _CitationTypes;
            }
        }

        private void comboBoxReferenceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.labelReferenceTypeDependentCitation.Text = this.GenerateCitation();
        }

        //private string GenerateCitation()
        //{
        //    System.Data.DataRowView R = (System.Data.DataRowView)this.projectReferenceBindingSource.Current;
        //    string Title = R["ReferenceTitle"].ToString();
        //    string SQL = "SELECT dbo.Citation (" + this.ProjectID.ToString() + ", '" + Title + "')";
        //    string Citation = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);

        //    //if (this.CitationTypes.Contains(Type))
        //    //{
        //    //    string Authors = "";
        //    //    System.Data.DataRow[] rrAuthor = this.dataSetDiversityProjects.ProjectAgent.Select("AgentRole = 'Author'", "AgentSequence");
        //    //    if(rrAuthor.Length > 0)
        //    //    {
        //    //        for(int i = 0; i < rrAuthor.Length; i++)
        //    //        {
        //    //            if (Authors.Length > 0)
        //    //                Authors += "; ";
        //    //            Authors += rrAuthor[i]["AgentName"].ToString();
        //    //        }
        //    //    }
        //    //    string Year = System.DateTime.Now.Year.ToString();
        //    //    string Title = this.textBoxProjectTitle.Text;
        //    //    string Publisher = "";
        //    //    System.Data.DataRow[] rrPublisher = this.dataSetDiversityProjects.ProjectAgent.Select("AgentRole = 'Publisher'", "AgentSequence");
        //    //    if (rrPublisher.Length > 0)
        //    //    {
        //    //        for (int i = 0; i < rrPublisher.Length; i++)
        //    //        {
        //    //            if (Publisher.Length > 0)
        //    //                Publisher += ", ";
        //    //            Publisher += rrPublisher[i]["AgentName"].ToString();
        //    //        }
        //    //    }
        //    //    string URI = this.textBoxReferenceUri.Text;
        //    //    if (Authors.Trim().Length > 0)
        //    //        Citation += Authors.Trim() + " ";
        //    //    Citation += "(" + Year + "). ";
        //    //    if (Title.Trim().Length > 0)
        //    //        Citation +=Title.Trim() + ". ";
        //    //    Citation += "[Dataset]  ";
        //    //    if (Publisher.Trim().Length > 0)
        //    //        Citation += "Data Publisher: " + Publisher.Trim() + ". ";
        //    //    if (URI.Trim().Length > 0)
        //    //        Citation += URI.Trim() + ".";
        //    //    Citation = Citation.Trim();
        //    //}
        //    return Citation;
        //}

        #endregion

        #endregion

        #region Setting

        private void toolStripButtonSettingAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.updateProject();
                DiversityProjects.FormSettings f = new FormSettings(this.ID, null);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.SelectedSettingID() != null)
                {
                    DiversityWorkbench.Forms.FormGetString fS = new DiversityWorkbench.Forms.FormGetString("New setting", "Please enter the value for the new setting", "");
                    fS.ShowDialog();
                    if (fS.DialogResult == System.Windows.Forms.DialogResult.OK && fS.String.Length > 0)
                    {
                        System.Data.DataRow R = this.dataSetDiversityProjects.ProjectSetting.NewProjectSettingRow();
                        R["ProjectID"] = this.ID;
                        R["SettingID"] = (int)f.SelectedSettingID();
                        R["Value"] = fS.String;
                        this.dataSetDiversityProjects.ProjectSetting.Rows.Add(R);
                        R.BeginEdit();
                        R.EndEdit();
                        this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectSetting", this._sqlDataAdapterProjectSetting, this.projectSettingBindingSource);
                        //this.buildSettingTree();
                        this.buildSettingTreeWithParents();
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private void toolStripButtonSettingRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeViewSettings.SelectedNode == null || this.treeViewSettings.SelectedNode.Tag == null)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    return;
                }
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
                System.Data.DataRow[] rr = this.dataSetDiversityProjects.ProjectSetting.Select("ProjectID = " + this.ID.ToString() + " AND SettingID = " + R["SettingID"].ToString());
                rr[0].Delete();
                this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectSetting", this._sqlDataAdapterProjectSetting, this.projectSettingBindingSource);
                //this.buildSettingTree();
                this.buildSettingTreeWithParents();
            }
            catch (System.Exception ex)
            { }
        }

        private void toolStripButtonSettingCopy_Click(object sender, EventArgs e)
        {
            try
            {
                bool RedrawTree = false;
                string SQL = "SELECT DISTINCT P.ProjectID, P.Project from Project P, dbo.ProjectList() L, ProjectSetting S " +
                    "where P.ProjectID = L.ProjectID AND S.ProjectID = P.ProjectID " +
                    "order by p.Project";
                System.Data.DataTable DtProjects = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(DtProjects);
                DiversityProjects.FormSettings F = new FormSettings(null, DtProjects);
                F.ShowDialog();
                if (F.DialogResult == System.Windows.Forms.DialogResult.OK && F.SelectedProjectID() != null)
                {
                    System.Collections.Generic.Dictionary<int, string> OverrideValues = new Dictionary<int, string>();
                    SQL = "SELECT PT.SettingID, PT.Value AS CurrentValue, PS.Value AS NewValue, S.Hierarchy " +
                        "FROM SettingHierarchyAll() AS S, ProjectSetting AS PT, ProjectSetting AS PS  " +
                        "WHERE PT.SettingID = PS.SettingID  " +
                        "AND PT.Value <> PS.Value  " +
                        "AND PT.SettingID = S.SettingID " +
                        "AND (PT.ProjectID = " + this.ID.ToString() + ") " +
                        "AND (PS.ProjectID = " + F.SelectedProjectID().ToString() + ") ";
                    System.Data.DataTable dt = new DataTable();
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            string Message = "The setting\r\n" + R[3].ToString() + "\r\nis allready present." +
                                "\r\n\r\nDo you want to change it?" +
                                "\r\n\r\nCurrent value:\r\n" + R[1].ToString() + "\r\n\r\nNew value:\r\n" + R[2].ToString();
                            if (System.Windows.Forms.MessageBox.Show(Message, "Change setting value?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                OverrideValues.Add(int.Parse(R[0].ToString()), R[2].ToString());
                        }
                        if (OverrideValues.Count > 0)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in OverrideValues)
                            {
                                System.Data.DataRow[] rr = this.dataSetDiversityProjects.ProjectSetting.Select("SettingID = " + KV.Key.ToString());
                                rr[0].BeginEdit();
                                rr[0]["Value"] = KV.Value;
                                rr[0].EndEdit();
                            }
                            RedrawTree = true;
                        }
                    }
                    SQL = "INSERT INTO ProjectSetting " +
                        "(ProjectID, SettingID, Value) " +
                        "SELECT " + this.ID.ToString() + ", SettingID, Value " +
                        "FROM ProjectSetting AS PS " +
                        "WHERE (PS.ProjectID = " + F.SelectedProjectID().ToString() + ") " +
                        "AND PS.SettingID NOT IN (SELECT SettingID " +
                        "FROM ProjectSetting AS P " +
                        "WHERE (P.ProjectID = " + this.ID.ToString() + "))";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL) || RedrawTree)
                    {
                        this.updateProject();
                        this.setProject(this.ID);
                        //this.FormFunctions.updateTable(this.dataSetDiversityProjects, "ProjectSetting", this._sqlDataAdapterProjectSetting, this.projectSettingBindingSource);
                        //this.buildSettingTreeWithParents();
                    }
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void toolStripButtonSettingEdit_Click(object sender, EventArgs e)
        {
            DiversityProjects.FormSettings f = new FormSettings(null, null);
            f.ShowDialog();
            this.buildSettingTreeWithParents();
        }

        private void treeViewSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
            int i = 0;
            foreach (System.Data.DataRow RR in this.dataSetDiversityProjects.ProjectSetting.Rows)
            {
                if (RR["SettingID"].ToString() == R["SettingID"].ToString())
                    break;
                i++;
            }
            this.projectSettingBindingSource.Position = i;
            if (!R["Value"].Equals(System.DBNull.Value) && R.Table.Columns.Contains("ProjectID") && R["ProjectID"].ToString() == this.ID.ToString())
            {
                //this.textBoxSettingValue.Text = R["Value"].ToString();
                this.textBoxSettingValue.Visible = true;
                this.labelSettingValue.Visible = true;
                this.toolStripButtonSettingRemove.Enabled = true;
            }
            else
            {
                //this.textBoxSettingValue.Text = "";
                this.textBoxSettingValue.Visible = false;
                this.labelSettingValue.Visible = false;
                this.toolStripButtonSettingRemove.Enabled = false;
            }
        }

        private DiversityProjects.Setting _Setting = new Setting();

        //private void buildSettingTree()
        //{
        //    this.treeViewSettings.Nodes.Clear();
        //    this._Setting.BuildHierarchyTree(ref this.treeViewSettings, this.ID, false);
        //    this.treeViewSettings.ExpandAll();
        //}

        private void buildSettingTreeWithParents()
        {
            try
            {
                this.treeViewSettings.Nodes.Clear();
                this._Setting.BuildHierarchyTreeWithParents(ref this.treeViewSettings, this.ID);
                this.treeViewSettings.ExpandAll();
                this.labelSettingValue.Visible = false;
                this.textBoxSettingValue.Visible = false;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonSettingValueSave_Click(object sender, EventArgs e)
        {
            System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
            R["Value"] = this.textBoxSettingValue.Text;
            string DisplayText = R["DisplayText"].ToString();
            if (this.textBoxSettingValue.Text.Length > 0)
                DisplayText += ": " + this.textBoxSettingValue.Text;
            this.treeViewSettings.SelectedNode.Text = DisplayText;
            System.Data.DataRow[] rr = this.dataSetDiversityProjects.ProjectSetting.Select("ProjectID = " + this.ID.ToString() + " AND SettingID = " + R["SettingID"].ToString());
            rr[0].BeginEdit();
            rr[0]["Value"] = this.textBoxSettingValue.Text;
            rr[0].EndEdit();
        }

        private void textBoxSettingValue_Leave(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.projectSettingBindingSource.Current;
            R.BeginEdit();
            if (R["Value"].ToString() != this.textBoxSettingValue.Text)
            {
                R["Value"] = this.textBoxSettingValue.Text;
                string NodeText = this.treeViewSettings.SelectedNode.Text.Substring(0, this.treeViewSettings.SelectedNode.Text.IndexOf(":")) + ": " + this.textBoxSettingValue.Text;
                this.treeViewSettings.SelectedNode.Text = NodeText;
            }
            R.EndEdit();
        }

        #endregion

        #region License

        private void buttonLicenseURI_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxLicenseURI.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxLicenseURI.Text = f.URL;
        }

        private void toolStripButtonLicenseAdd_Click(object sender, EventArgs e)
        {
            DiversityProjects.DataSetDiversityProjects.ProjectLicenseRow R = this.dataSetDiversityProjects.ProjectLicense.NewProjectLicenseRow();
            R.ProjectID = this.ID;
            R.DisplayText = (this.dataSetDiversityProjects.ProjectLicense.Rows.Count + 1).ToString() + ". License";
            this.dataSetDiversityProjects.ProjectLicense.Rows.Add(R);
        }

        private void toolStripButtonLicenseRemove_Click(object sender, EventArgs e)
        {
            if (listBoxLicense.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxLicense.SelectedItem;
                R.Delete();
            }
            else System.Windows.Forms.MessageBox.Show("nothing selected");
        }

        private void buttonLicenseDisplayTextEdit_Click(object sender, EventArgs e)
        {

        }

        private void textBoxLicenseYear_Leave(object sender, EventArgs e)
        {
            if  (this.textBoxLicenseYear.Text.Length > 0)
            {
                int Year = 0;
                bool IsYear = true;
                if (!int.TryParse(this.textBoxLicenseYear.Text, out Year))
                {
                    IsYear = false;
                }
                if(Year < 1900 || Year > System.DateTime.Now.Year)
                {
                    IsYear = false;
                }
                if (!IsYear)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a valid year");
                    this.textBoxLicenseYear.Focus();
                }
            }
        }

        private void textBoxLicenseYear_Validating(object sender, CancelEventArgs e)
        {
            if (this.textBoxLicenseYear.Text.Length > 0)
            {
                int year = 0;
                bool isYear = true;

                if (!int.TryParse(this.textBoxLicenseYear.Text, out year))
                {
                    isYear = false;
                }
                else if (year < 1900 || year > System.DateTime.Now.Year)
                {
                    isYear = false;
                }

                if (!isYear)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a valid year");
                    e.Cancel = true;
                }
            }
        }


        private void listBoxLicense_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool AllowEdit = false;
            if (this.listBoxLicense.SelectedIndex != -1)
                AllowEdit = true;
            this.groupBoxCopyright.Enabled = AllowEdit;
            this.groupBoxIPR.Enabled = AllowEdit;
            this.groupBoxLicense.Enabled = AllowEdit;
            this.groupBoxLicenseNotes.Enabled = AllowEdit;
            this.tableLayoutPanelLicense.Enabled = AllowEdit;
        }

        private void comboBoxLicenseContext_DropDown(object sender, EventArgs e)
        {
            this.SetDatasource(this.comboBoxLicenseContext, "ProjectLicense", "Context");
        }

        private void comboBoxLicenseType_DropDown(object sender, EventArgs e)
        {
            this.SetDatasource(this.comboBoxLicenseType, "ProjectLicense", "LicenseType");
        }

        #endregion

        #region ID

        private void toolStripButtonIDadd_Click(object sender, EventArgs e)
        {
            DiversityProjects.DataSetDiversityProjects.ProjectIdentifierRow R = this.dataSetDiversityProjects.ProjectIdentifier.NewProjectIdentifierRow();
            R.Identifier = "Identifier " + (this.dataSetDiversityProjects.ProjectIdentifier.Rows.Count + 1).ToString();
            R.ProjectID = this.ID;
            this.dataSetDiversityProjects.ProjectIdentifier.Rows.Add(R);
        }

        private void toolStripButtonIDremove_Click(object sender, EventArgs e)
        {
            if (this.projectIdentifierBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectIdentifierBindingSource.Current;
                R.Delete();
            }
            else System.Windows.Forms.MessageBox.Show("nothing selected");
        }

        private void buttonIDgetURL_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxIDurl.Text);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                this.textBoxIDurl.Text = f.URL;
        }

        private void listBoxID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxID.SelectedIndex == -1)
                this.tableLayoutPanelID.Enabled = false;
            else this.tableLayoutPanelID.Enabled = true;
            if (this.projectIdentifierBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectIdentifierBindingSource.Current;
                System.Data.DataRow[] rr = this.dataSetDiversityProjects.ProjectArchiveIdentifier.Select("Identifier = '" + R["Identifier"].ToString() + "'");
                if (rr.Length > 0)
                {
                    this.textBoxIdentifier.ReadOnly = true;
                    this.pictureBoxIdentifierLocked.Visible = true;
                }
                else
                {
                    this.textBoxIdentifier.ReadOnly = false;
                    this.pictureBoxIdentifierLocked.Visible = false;
                }
            }
        }

        private void buttonIDtypes_Click(object sender, EventArgs e)
        {
            this.identifierTypesToolStripMenuItem_Click(null, null);
        }

        #endregion

        #region Descriptor

        private void toolStripButtonDescriptorAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT DisplayText, ElementID FROM ProjectDescriptorElement ORDER BY DisplayText";
                System.Data.DataTable dt = new DataTable();
                string Exception = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Exception);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "ElementID", "Type of descriptor", "Please select the type of the new descriptor", "", false, true);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
                {
                    string Descriptor = "";
                    SQL = "SELECT DISTINCT [Content] FROM [dbo].[ProjectDescriptor] WHERE ElementID = " + f.SelectedValue.ToString() + " ORDER BY [Content]";
                    dt = new DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Exception);
                    if (dt.Rows.Count > 0)
                    {
                        DiversityWorkbench.Forms.FormGetStringFromList fL = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Content", "Content", "New descriptor", "Please select a " + f.SelectedString + " from the list or type a new one", "", false, false, true);
                        fL.ShowDialog();
                        if (fL.DialogResult == System.Windows.Forms.DialogResult.OK)
                            Descriptor = fL.SelectedString;
                    }
                    if (Descriptor.Length == 0)
                    {
                        DiversityWorkbench.Forms.FormGetString fS = new DiversityWorkbench.Forms.FormGetString("New " + f.SelectedString, "Please type the new " + f.SelectedString, "");
                        fS.ShowDialog();
                        if (fS.DialogResult == System.Windows.Forms.DialogResult.OK)
                            Descriptor = fS.String;
                        else
                            return;
                    }
                    DiversityProjects.DataSetDiversityProjects.ProjectDescriptorRow R = this.dataSetDiversityProjects.ProjectDescriptor.NewProjectDescriptorRow();
                    R.Content = Descriptor;
                    R.ElementID = int.Parse(f.SelectedValue);
                    R.ProjectID = this.ID;
                    R.Language = "en";
                    R.ContentURI = "";
                    this.dataSetDiversityProjects.ProjectDescriptor.Rows.Add(R);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonDescriptorDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.projectDescriptorBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.projectDescriptorBindingSource.Current;
                    R.Delete();
                }
                else System.Windows.Forms.MessageBox.Show("nothing selected");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxDescriptor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxDescriptor.SelectedIndex != -1)
                this.splitContainerDescriptor.Panel2.Enabled = true;
            else
                this.splitContainerDescriptor.Panel2.Enabled = false;
            if (comboBoxDescriptorElement.SelectedItem != null)
            {
                System.Data.DataRowView RE = (System.Data.DataRowView)comboBoxDescriptorElement.SelectedItem;
                string Type = RE["DisplayText"].ToString();
                switch (Type)
                {
                    case "Scientific term":
                    case "Taxon name":
                    case "Geographical name":
                        this.setDescriptorElementAccessibility(true);
                        break;
                    default:
                        this.setDescriptorElementAccessibility(false);
                        break;
                }
            }
        }

        private void comboBoxDescriptorElement_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBoxDescriptorContent.Visible = false;
            this.textBoxDescriptorContentUri.Visible = false;
            this.labelDescriptorContentUri.Visible = false;
            this.userControlModuleRelatedEntryDescriptorTerm.Visible = false;
            this.userControlModuleRelatedEntryDescriptorTaxon.Visible = false;
            this.userControlModuleRelatedEntryDescriptorGazetteer.Visible = false;
            if (comboBoxDescriptorElement.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)comboBoxDescriptorElement.SelectedItem;
                string Type = R["DisplayText"].ToString();
                switch (Type)
                {
                    case "Scientific term":
                        this.userControlModuleRelatedEntryDescriptorTerm.Visible = true;
                        //this.setDescriptorElementAccessibility(true);
                        break;
                    case "Taxon name":
                        this.userControlModuleRelatedEntryDescriptorTaxon.Visible = true;
                        //this.setDescriptorElementAccessibility(true);
                        break;
                    case "Geographical name":
                        this.userControlModuleRelatedEntryDescriptorGazetteer.Visible = true;
                        //this.setDescriptorElementAccessibility(true);
                        break;
                    default:
                        this.textBoxDescriptorContent.Visible = true;
                        this.textBoxDescriptorContentUri.Visible = true;
                        this.labelDescriptorContentUri.Visible = true;
                        //this.setDescriptorElementAccessibility(false);
                        break;
                }
            }
        }

        private void setDescriptorElementAccessibility(bool ModulRelated)
        {
            try
            {
                if (ModulRelated)
                {
                    if (this.projectDescriptorBindingSource.Current == null)
                        this.comboBoxDescriptorElement.Enabled = true;
                    else
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.projectDescriptorBindingSource.Current; //this.dataSetDiversityProjects.Tables["ProjectDescriptor"].Rows[0]["ContentURI"]
                        if (R["ContentURI"].Equals(System.DBNull.Value) || R["ContentURI"].ToString().Length == 0)
                            this.comboBoxDescriptorElement.Enabled = true;
                        else
                            this.comboBoxDescriptorElement.Enabled = false;
                    }
                }
                else
                    this.comboBoxDescriptorElement.Enabled = true;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonDescriptorTypes_Click(object sender, EventArgs e)
        {
            this.descriptorTypesToolStripMenuItem_Click(null, null);
        }

        #endregion

        #region Auxillary

        /// <summary>
        /// Setting a datasource for a combobox
        /// </summary>
        /// <param name="CB">The combobox</param>
        /// <param name="Table">The name of the table where the data should be taken from</param>
        /// <param name="Column">The name of the columne where the data should be taken from</param>
        private void SetDatasource(System.Windows.Forms.ComboBox CB, string Table, string Column)
        {
            try
            {
                string SQL = "SELECT DISTINCT T." + Column + " FROM [" + Table + "] T WHERE T." + Column + " <> ''";
                if (CB.Text.Length > 0)
                    SQL += " AND T." + Column + " LIKE '" + CB.Text + "%'";
                SQL += " ORDER BY " + Column;
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.Connection);
                ad.Fill(dt);
                CB.DataSource = dt;
                CB.DisplayMember = Column;
                CB.ValueMember = Column;
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region OAIP Archive

        private System.Data.DataView _ViewArchiveIdentifier;

        private void initArchiveControls()
        {
            try
            {
                if (this.projectArchiveBindingSource.Current != null)
                {
                    this.tableLayoutPanelArchive.Enabled = true;
                    System.Data.DataRowView R = (System.Data.DataRowView)this.projectArchiveBindingSource.Current;

                    this.dateTimePickerArchivePublicationDate.Format = DateTimePickerFormat.Custom;
                    if (R["PublicationDate"].Equals(System.DBNull.Value))
                    {
                        this.dateTimePickerArchivePublicationDate.CustomFormat = "--";
                    }
                    else
                        this.dateTimePickerArchivePublicationDate.CustomFormat = "yyyy-MM-dd";

                    this.dateTimePickerArchiveTransferDate.Format = DateTimePickerFormat.Custom;
                    if (R["TransferDate"].Equals(System.DBNull.Value))
                    {
                        this.dateTimePickerArchiveTransferDate.CustomFormat = "--";
                    }
                    else
                    {
                        this.dateTimePickerArchiveTransferDate.CustomFormat = "yyyy-MM-dd";
                        //System.DateTime dt;
                        //if (System.DateTime.TryParse(R["TransferDate"].ToString(), out dt))
                        //    this.labelArchiveTransfer.Text = this.IsoDateTime(dt);
                    }
                    bool Published = false;
                    bool.TryParse(R["Published"].ToString(), out Published);
                    this.LockArchiveControlsIfPublished(Published);
                    if (this._ViewArchiveIdentifier == null)
                        this._ViewArchiveIdentifier = new DataView(this.dataSetDiversityProjects.ProjectArchiveIdentifier, "ArchiveID = " + R["ArchiveID"].ToString(), "", DataViewRowState.CurrentRows);
                    else
                        this._ViewArchiveIdentifier.RowFilter = "ArchiveID = " + R["ArchiveID"].ToString();
                    this.listBoxArchiveIdentier.DataSource = this._ViewArchiveIdentifier;
                }
                else
                    this.tableLayoutPanelArchive.Enabled = false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void LockArchiveControlsIfPublished(bool IsPublished)
        {
            try
            {
                this.textBoxArchiveDataWithholdingReason.Enabled = !IsPublished;
                this.textBoxArchiveExternalStorageLocation.Enabled = !IsPublished;
                this.textBoxArchiveFileHash.Enabled = !IsPublished;
                this.textBoxArchiveFileSize.Enabled = !IsPublished;
                this.textBoxArchiveStorageLocation.Enabled = !IsPublished;
                this.comboBoxArchiveFormat.Enabled = !IsPublished;
                //this.comboBoxArchiveIdentifier.Enabled = !IsPublished;
                this.comboBoxArchiveMimeType.Enabled = !IsPublished;
                this.dateTimePickerArchivePublicationDate.Enabled = !IsPublished;
                this.dateTimePickerArchiveTransferDate.Enabled = !IsPublished;
                //this.buttonArchiveExternalStorageLocation.Enabled = !IsPublished;
                this.buttonArchiveStorageLocation.Enabled = !IsPublished;
                this.checkBoxArchivePublished.Enabled = !IsPublished;
                this.toolStripButtonArchiveDelete.Enabled = !IsPublished;
                this.toolStripArchiveIdentier.Enabled = !IsPublished;
                this.listBoxArchiveIdentier.Enabled = !IsPublished;
                if (IsPublished)
                    this.pictureBoxArchivePublished.Image = DiversityProjects.Properties.Resources.OAIS_16;
                else
                    this.pictureBoxArchivePublished.Image = DiversityProjects.Properties.Resources.ArchivOpen;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buildArchiveTree()
        {
            try
            {
                this.treeViewArchive.Nodes.Clear();
                System.Data.DataRow[] rr = this.dataSetDiversityProjects.ProjectArchive.Select("ParentArchiveID IS NULL", "ArchiveID");
                foreach (System.Data.DataRow R in rr)
                {
                    string TransferDate = "";
                    if (R["TransferDate"].Equals(System.DBNull.Value))
                        TransferDate = "[ID: " + R["ArchiveID"].ToString() + "]";
                    else
                        TransferDate = R["TransferDate"].ToString();
                    System.DateTime DT;
                    if (System.DateTime.TryParse(TransferDate, out DT))
                        TransferDate = this.IsoDateTime(DT);
                    System.Windows.Forms.TreeNode N = new TreeNode(TransferDate);
                    N.Tag = R;
                    this.treeViewArchive.Nodes.Add(N);
                    this.AddArchiveChildren(N);
                }
                this.treeViewArchive.ExpandAll();
                this.initArchiveControls();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string IsoDateTime(System.DateTime DT)
        {
            string Date = DT.Year.ToString() + "-";
            if (DT.Month < 10)
                Date += "0";
            Date += DT.Month.ToString() + "-";
            if (DT.Day < 10)
                Date += "0";
            Date += DT.Day.ToString() + " ";
            if (DT.Hour < 10)
                Date += "0";
            Date += DT.Hour.ToString() + ":";
            if (DT.Minute < 10)
                Date += "0";
            Date += DT.Minute.ToString() + ":";
            if (DT.Second < 10)
                Date += "0";
            Date += DT.Second.ToString();
            return Date;
        }

        private void AddArchiveChildren(System.Windows.Forms.TreeNode NP)
        {
            try
            {
                System.Data.DataRow RP = (System.Data.DataRow)NP.Tag;
                System.Data.DataRow[] rr = this.dataSetDiversityProjects.ProjectArchive.Select("ParentArchiveID = " + RP["ArchiveID"].ToString(), "ArchiveID");
                foreach (System.Data.DataRow R in rr)
                {
                    string TransferDate = "";
                    if (R["TransferDate"].Equals(System.DBNull.Value))
                        TransferDate = "[ID: " + R["ArchiveID"].ToString() + "]";
                    else
                        TransferDate = R["TransferDate"].ToString();
                    System.DateTime DT;
                    if (System.DateTime.TryParse(TransferDate, out DT))
                        TransferDate = DT.Year.ToString() + "-" + DT.Month.ToString() + "-" + DT.Day.ToString() + " " + DT.Hour.ToString() + ":" + DT.Minute.ToString() + ":" + DT.Second.ToString();
                    System.Windows.Forms.TreeNode N = new TreeNode(TransferDate);
                    N.Tag = R;
                    NP.Nodes.Add(N);
                    this.AddArchiveChildren(N);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonArchiveAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityProjects.DataSetDiversityProjects.ProjectArchiveRow R = this.dataSetDiversityProjects.ProjectArchive.NewProjectArchiveRow();
                R["ProjectID"] = this.ID;
                R["TransferDate"] = System.DateTime.Now;
                if (this.treeViewArchive.SelectedNode != null)
                {
                    System.Data.DataRow RParent = (System.Data.DataRow)this.treeViewArchive.SelectedNode.Tag;
                    string TD = "";
                    System.DateTime TransferDate;
                    if (!RParent["TransferDate"].Equals(System.DBNull.Value))
                    {
                        if (System.DateTime.TryParse(RParent["TransferDate"].ToString(), out TransferDate))
                            TD = this.IsoDateTime(TransferDate);
                    }
                    if (System.Windows.Forms.MessageBox.Show("Should the new archive depend on the selected archive " + TD, "Depending?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        R["ParentArchiveID"] = RParent["ArchiveID"];
                }
                this.dataSetDiversityProjects.ProjectArchive.Rows.Add(R);
                this.saveProject(null, null);
                this.buildArchiveTree();

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            // Old version dependent on Identifier
            //try
            //{
            //    string SQL = "SELECT Identifier FROM ProjectIdentifier WHERE Type = N'DOI' AND ProjectID = " + this.ID.ToString() +
            //        " AND Identifier NOT IN (SELECT Identifier FROM ProjectArchive WHERE ProjectID = " + this.ID.ToString() + ")" +
            //        " ORDER BY Identifier ";
            //    System.Data.DataTable dt = new DataTable();
            //    string Message = "";
            //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            //    if (dt.Rows.Count == 0)
            //        System.Windows.Forms.MessageBox.Show("No identifiers available");
            //    else
            //    {
            //        DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Please select identifier for the new archive", true);
            //        f.ShowDialog();
            //        if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.SelectedString.Length > 0)
            //        {
            //            DiversityProjects.DataSetDiversityProjects.ProjectArchiveRow R = this.dataSetDiversityProjects.ProjectArchive.NewProjectArchiveRow();
            //            R["ProjectID"] = this.ID;
            //            R["Identifier"] = f.SelectedString;
            //            if (this.treeViewArchive.SelectedNode != null)
            //            {
            //                System.Data.DataRow RParent = (System.Data.DataRow)this.treeViewArchive.SelectedNode.Tag;
            //                if (System.Windows.Forms.MessageBox.Show("Should the new archive depend no the selected archive " + RParent["Identifier"].ToString(), "Depending?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            //                    R["ParentArchiveID"] = RParent["ArchiveID"];
            //            }
            //            this.dataSetDiversityProjects.ProjectArchive.Rows.Add(R);
            //            this.saveProject(null, null);
            //            this.buildArchiveTree();
            //        }
            //    }
            //}
            //catch(System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}
        }

        private void toolStripButtonArchiveDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeViewArchive.SelectedNode != null)
                {
                    if (this.treeViewArchive.SelectedNode.Nodes.Count == 0)
                    {
                        System.Data.DataRow R = (System.Data.DataRow)this.treeViewArchive.SelectedNode.Tag;
                        R.BeginEdit();
                        R.Delete();
                        R.EndEdit();
                        this.saveProject(null, null);
                        this.buildArchiveTree();
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("Archives with depending archives can not be deleted");
                }
                else
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonArchiveStorageLocation_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    this.textBoxArchiveStorageLocation.Text = this.openFileDialog.FileName;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonArchiveExternalStorageLocation_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    this.textBoxArchiveExternalStorageLocation.Text = this.openFileDialog.FileName;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void treeViewArchive_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewArchive.SelectedNode.Tag;
                this.labelArchiveTransfer.Text = this.treeViewArchive.SelectedNode.Text;
                for (int i = 0; i < this.dataSetDiversityProjects.ProjectArchive.Rows.Count; i++)
                {
                    if (R["ArchiveID"].ToString() == this.dataSetDiversityProjects.ProjectArchive.Rows[i]["ArchiveID"].ToString())
                    {
                        this.projectArchiveBindingSource.Position = i;
                        this.initArchiveControls();
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void checkBoxArchivePublished_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBoxArchivePublished.Checked)
                {
                    if (System.Windows.Forms.MessageBox.Show("This will lock the archive data and prevent any further changes.\r\n\r\nAre you sure?", "Lock archive?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.saveProject(null, null);
                        this.initArchiveControls();
                    }
                    else
                        this.checkBoxArchivePublished.Checked = false;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void linkLabelArchiveFormat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (this.projectArchiveBindingSource.Current == null)
                {
                    MessageBox.Show("No archive format selected.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var rowView = (System.Data.DataRowView)this.projectArchiveBindingSource.Current;
                string format = rowView["Format"]?.ToString();

                if (string.IsNullOrWhiteSpace(format))
                {
                    MessageBox.Show("Archive format is not specified.", "Invalid Format",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sql = "SELECT URL FROM ProjectArchiveFormat_Enum WHERE Code = @Format";
                var parameters = new Dictionary<string, object> { { "@Format", format } };
                string url = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(sql, parameters);

                if (string.IsNullOrWhiteSpace(url))
                {
                    MessageBox.Show("No link to a description defined for this format.", "No Link",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                MessageBox.Show("Unable to open the link. See error log for details.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void linkLabelArchiveMimeType_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (this.projectArchiveBindingSource.Current == null)
                {
                    MessageBox.Show("No archive mime type selected.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var rowView = (System.Data.DataRowView)this.projectArchiveBindingSource.Current;
                string mimeType = rowView["MimeType"]?.ToString();

                if (string.IsNullOrWhiteSpace(mimeType))
                {
                    MessageBox.Show("Mime type is not specified.", "Invalid Mime Type",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Use parameterized query to prevent SQL injection
                string sql = "SELECT URL FROM ProjectArchiveMimeType_Enum WHERE Code = @MimeType";
                var parameters = new Dictionary<string, object> { { "@MimeType", mimeType } };
                string url = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(sql, parameters);

                if (string.IsNullOrWhiteSpace(url))
                {
                    MessageBox.Show("No link to a description defined for this mime type.", "No Link",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                MessageBox.Show("Unable to open the link. See error log for details.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxArchiveDataWithholdingReason_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBoxArchiveDataWithholdingReason.Text.Length > 0)
                {
                    this.textBoxArchiveDataWithholdingReason.BackColor = System.Drawing.Color.Pink;
                }
                else
                {
                    this.textBoxArchiveDataWithholdingReason.BackColor = System.Drawing.SystemColors.Window;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dateTimePickerArchiveTransferDate_CloseUp(object sender, EventArgs e)
        {
            this.saveProject(null, null);
            this.initArchiveControls();
        }

        private void dateTimePickerArchivePublicationDate_CloseUp(object sender, EventArgs e)
        {
            this.saveProject(null, null);
            this.initArchiveControls();
        }

        private void dateTimePickerArchiveEmbargoDate_CloseUp(object sender, EventArgs e)
        {
            this.saveProject(null, null);
            this.initArchiveControls();
        }

        #region ArchiveIdentier
        private void toolStripButtonArchiveIdentierAdd_Click(object sender, EventArgs e)
        {
            if (this.projectArchiveBindingSource.Current == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select an archive");
                return;
            }
            System.Data.DataRowView R = (System.Data.DataRowView)this.projectArchiveBindingSource.Current;
            int ArchiveID;
            if (int.TryParse(R["ArchiveID"].ToString(), out ArchiveID))
            {
                string SQL = "SELECT Identifier " +
                    "FROM ProjectIdentifier " +
                    "WHERE ProjectID = " + this.ID.ToString() +
                    "AND Identifier NOT IN " +
                    "(SELECT Identifier FROM ProjectArchiveIdentifier " +
                    "WHERE ProjectID = " + this.ID.ToString() + " AND ArchiveID = " + ArchiveID.ToString() + ") " +
                    "ORDER BY Identifier";
                System.Data.DataTable dtIdent = new DataTable();
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtIdent, ref Message);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtIdent, "Please select identifier", true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    DiversityProjects.DataSetDiversityProjects.ProjectArchiveIdentifierRow Rident = this.dataSetDiversityProjects.ProjectArchiveIdentifier.NewProjectArchiveIdentifierRow();
                    Rident.ProjectID = this.ID;
                    Rident.ArchiveID = ArchiveID;
                    Rident.Identifier = f.SelectedString;
                    this.dataSetDiversityProjects.ProjectArchiveIdentifier.Rows.Add(Rident);
                }
            }
        }

        private void toolStripButtonArchiveIdentierDelete_Click(object sender, EventArgs e)
        {
            if (this.projectArchiveIdentifierBindingSource.Current == null)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            else
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectArchiveIdentifierBindingSource.Current;
                if (System.Windows.Forms.MessageBox.Show("Do you really want to delete the identifier " + R["Identifier"].ToString() + " from the list", "Delete identifier", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    R.Delete();
            }
        }

        #endregion

        #endregion

        #region Backlinks

        private void scanModules()
        {
            try
            {
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Agents))
                {
                    string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.ID.ToString();
                    this.userControlModuleBackLinksAgents.SearchForBackLinks(URL);
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection))
                {
                    string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.ID.ToString();
                    this.userControlModuleBackLinksCollection.SearchForBackLinks(URL);
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Descriptions))
                {
                    string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.ID.ToString();
                    this.userControlModuleBackLinksDescriptions.SearchForBackLinks(URL);
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer))
                {
                    string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.ID.ToString();
                    this.userControlModuleBackLinksGazetteer.SearchForBackLinks(URL);
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.References))
                {
                    string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.ID.ToString();
                    this.userControlModuleBackLinksReferences.SearchForBackLinks(URL);
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots))
                {
                    string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.ID.ToString();
                    this.userControlModuleBackLinksPlots.SearchForBackLinks(URL);
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms))
                {
                    string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.ID.ToString();
                    this.userControlModuleBackLinksTerms.SearchForBackLinks(URL);
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames))
                {
                    string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.ID.ToString();
                    this.userControlModuleBackLinksTaxa.SearchForBackLinks(URL);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void SetHeight(int Height)
        {

        }

        public DiversityWorkbench.WorkbenchUnit.ModuleType GetModuleType()
        {
            return DiversityWorkbench.WorkbenchUnit.ModuleType.Projects;
        }

        private void initBackLinksAgent()
        {
            this.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.Agents, this.tabPageDiversityAgents, this.userControlModuleBackLinksAgents, this.diversityAgentsToolStripMenuItem);
        }

        private void initBackLinksCollection()
        {
            if (!DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey("DiversityCollection"))
            {
                DiversityWorkbench.CollectionSpecimen C = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
            }
            this.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection, this.tabPageDiversityCollection, this.userControlModuleBackLinksCollection, this.diversityCollectionToolStripMenuItem);
        }

        private void initBackLinksDescriptions()
        {
            this.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.Descriptions, this.tabPageDiversityDescriptions, this.userControlModuleBackLinksDescriptions, this.diversityDescriptionsToolStripMenuItem);
        }

        private void initBackLinksGazetteer()
        {
            this.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer, this.tabPageDiversityGazetteer, this.userControlModuleBackLinksGazetteer, this.diversityGazetteerToolStripMenuItem);
        }

        private void initBackLinksReferences()
        {
            this.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.References, this.tabPageDiversityReferences, this.userControlModuleBackLinksReferences, this.diversityReferencesToolStripMenuItem);
        }

        private void initBackLinksSamplingPlots()
        {
            this.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots, this.tabPageDiversitySamplingPlots, this.userControlModuleBackLinksPlots, this.diversitySamplingPlotsToolStripMenuItem);
        }

        private void initBackLinksScientificTerms()
        {
            this.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms, this.tabPageDiversityScientificTerms, this.userControlModuleBackLinksTerms, this.diversityScientificTermsToolStripMenuItem);
        }

        private void initBackLinksTaxonNames()
        {
            this.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames, this.tabPageDiversityTaxonNames, this.userControlModuleBackLinksTaxa, this.diversityTaxonNamesToolStripMenuItem);
        }

        private void initBackLinks()
        {
            this.initBackLinksAgent();
            this.initBackLinksCollection();
            this.initBackLinksDescriptions();
            this.initBackLinksGazetteer();
            this.initBackLinksReferences();
            this.initBackLinksSamplingPlots();
            this.initBackLinksScientificTerms();
            this.initBackLinksTaxonNames();
        }

        private void initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType Module, System.Windows.Forms.TabPage Page, DiversityWorkbench.UserControls.UserControlModuleBackLinks U, System.Windows.Forms.ToolStripMenuItem Menu)
        {
            if (DiversityWorkbench.Settings.ScannedModuleIsScanned(Module))
            {
                if (!this.tabControlModules.TabPages.Contains(Page))
                    this.tabControlModules.TabPages.Add(Page);
                U.SetModuleParameter(Module, this, false);
                Menu.Checked = true;
            }
            else
            {
                if (this.tabControlModules.TabPages.Contains(Page))
                    this.tabControlModules.TabPages.Remove(Page);
                Menu.Checked = false;
            }
        }



        #endregion

        #region Import wizard
        private void StartWizard(string Target)
        {
            try
            {
                this.Enabled = false;

                // Show start window
                DiversityWorkbench.Import.FormStartWizard fs = new DiversityWorkbench.Import.FormStartWizard();
                //fs.StartPosition = FormStartPosition.CenterParent;
                fs.Show();
                Application.DoEvents();

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                fs.ShowCurrentStep("Resetting the templates for the tables\r\n..........");
                DiversityWorkbench.Import.Import.ResetTemplate();
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> ImportSteps = null;
                fs.ShowCurrentStep("Getting the steps for " + Target + "\r\n|.........");
                DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn = null;
                switch (Target)
                {
                    case "Project":
                        ImportSteps = this.ImportStepsProject(fs);
                        break;
                }
                if (ImportSteps == null)
                    ImportSteps = new Dictionary<string, DiversityWorkbench.Import.Step>();
                fs.ShowCurrentStep("Import steps for " + Target + " created. Opening import form\r\n||||||||..");
                DiversityWorkbench.Import.FormWizard f = new DiversityWorkbench.Import.FormWizard(Target, DiversityProjects.Properties.Settings.Default.DatabaseVersion, ImportSteps, this.helpProviderDiversityProjects.HelpNamespace, fs);
                DiversityWorkbench.Import.iWizardInterface i = f;

                f.Height = this.Height - 10;
                f.StartPosition = FormStartPosition.CenterParent;

                fs.Close();

                f.ShowDialog();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Enabled = true;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private enum ImportWizardMainTables
        {
            Project, ProjectAgent, ProjectAgentRole, ProjectArchive, ProjectArchiveIdentifier,
            ProjectDescriptor, ProjectIdentifier, ProjectLicense, ProjectReference
        };

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> ImportStepsProject(DiversityWorkbench.Import.iStartingMessage iMessage)
        {
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> IS = new Dictionary<string, DiversityWorkbench.Import.Step>();

            try
            {
                iMessage.ShowCurrentStep("Creating the steps for the project\r\n||........");

                DiversityWorkbench.Import.DataTable DTProject = this.ImportWizardDataTable("Project", "", "Project", DiversityWorkbench.Import.DataTable.Parallelity.unique, (int)DiversityProjects.FormDiversityProjects.ImportWizardMainTables.Project,
                    "ProjectVersion, ProjectParentID, CitationReferenceTitle, CitationReferenceURI, CreateArchive, ArchiveProtocol", "ProjectID, Project", DiversityProjects.Properties.Resources.Project);
                DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn = DTProject.DataColumns["Project"];
                DiversityWorkbench.Import.Step SProject = DiversityWorkbench.Import.Step.GetStepTemplate(DTProject, DiversityProjects.Properties.Resources.Project, 0, true, null);
                //SProject.StepColumnGroups = this.ImportWizardColumnGroupsProject();
                SProject.MustSelect = true;
                IS.Add(DTProject.PositionKey, SProject);

                iMessage.ShowCurrentStep("Creating the steps for the agent \r\n|||||.....");
                DiversityWorkbench.Import.DataTable DTProjectAgent = this.ImportWizardDataTable("ProjectAgent", "Project", "Agent",
                    DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversityProjects.FormDiversityProjects.ImportWizardMainTables.ProjectAgent, "", "",
                    DiversityProjects.Properties.Resources.Agent);
                DiversityWorkbench.Import.Step SProjectAgent = DiversityWorkbench.Import.Step.GetStepTemplate(DTProjectAgent, DiversityProjects.Properties.Resources.Agent, 1, false, null);
                IS.Add(DTProjectAgent.PositionKey, SProjectAgent);

                iMessage.ShowCurrentStep("Creating the steps for the descriptor \r\n|||||.....");
                DiversityWorkbench.Import.DataTable DTProjectDescriptor = this.ImportWizardDataTable("ProjectDescriptor", "Project", "Descriptor",
                    DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversityProjects.FormDiversityProjects.ImportWizardMainTables.ProjectDescriptor, "", "",
                    DiversityProjects.Properties.Resources.KeyBlue);
                DiversityWorkbench.Import.Step SProjectDescriptor = DiversityWorkbench.Import.Step.GetStepTemplate(DTProjectDescriptor, DiversityProjects.Properties.Resources.KeyBlue, 1, false, null);
                IS.Add(DTProjectDescriptor.PositionKey, SProjectDescriptor);

                iMessage.ShowCurrentStep("Creating the steps for the license \r\n|||||||...");
                DiversityWorkbench.Import.DataTable DTProjectLicense = this.ImportWizardDataTable("ProjectLicense", "Project", "License", DiversityWorkbench.Import.DataTable.Parallelity.parallel,
                    (int)DiversityProjects.FormDiversityProjects.ImportWizardMainTables.ProjectLicense, "", "", DiversityProjects.Properties.Resources.Document);
                DiversityWorkbench.Import.Step SProjectLicense = DiversityWorkbench.Import.Step.GetStepTemplate(DTProjectLicense, DiversityProjects.Properties.Resources.Document, 1, false, null);
                IS.Add(DTProjectLicense.PositionKey, SProjectLicense);

                iMessage.ShowCurrentStep("Creating the steps for the reference \r\n||||||||..");
                DiversityWorkbench.Import.DataTable DTProjectReference = this.ImportWizardDataTable("ProjectReference", "Project", "Reference",
                    DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversityProjects.FormDiversityProjects.ImportWizardMainTables.ProjectReference, "", "",
                    DiversityProjects.Properties.Resources.References);
                DiversityWorkbench.Import.Step SProjectReference = DiversityWorkbench.Import.Step.GetStepTemplate(DTProjectReference, DiversityProjects.Properties.Resources.References, 1,
                    false, null);
                IS.Add(DTProjectReference.PositionKey, SProjectReference);

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return IS;
        }


        private DiversityWorkbench.Import.DataTable ImportWizardDataTable(string TableName, string ParentTableAlias, string DisplayText, DiversityWorkbench.Import.DataTable.Parallelity P, int SequencePosition, string IgnoredColumns, string AttachmentColumns, Image Image)
        { //1
            System.Collections.Generic.List<string> lAC = new List<string>();
            AttachmentColumns = AttachmentColumns.Replace(" ", "");
            string[] ACC = AttachmentColumns.Split(new char[] { ',' });
            for (int i = 0; i < ACC.Length; i++)
                lAC.Add(ACC[i]);
            System.Collections.Generic.List<string> IC = new List<string>();
            IgnoredColumns = IgnoredColumns.Replace(" ", "");
            string[] CC = IgnoredColumns.Split(new char[] { ',' });
            for (int i = 0; i < CC.Length; i++)
                IC.Add(CC[i]);
            DiversityWorkbench.Import.DataTable DT = DiversityWorkbench.Import.DataTable.GetTableTemplate(TableName, ParentTableAlias, DisplayText, Image, P, SequencePosition, IC, lAC);
            return DT;
        }



        #endregion

        #region Json Cache

        private void ShowJsonCache_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                string Json = "";
                if (e.Button == MouseButtons.Left)
                {
                    Json = DiversityWorkbench.Api.JsonCache.Json(this.ID);//( DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT Data FROM JsonCache WHERE ID = " + this.ID);
                }
                else if (e.Button == MouseButtons.Right && DiversityWorkbench.Api.JsonCache.ContainsColumnPublic())
                {
                    Json = DiversityWorkbench.Api.JsonCache.JsonPublic(ID);// DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT [Public] FROM JsonCache WHERE ID = " + this.ID);
                }
                DiversityWorkbench.Api.JsonCache.ShowJson(Json);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

    }
}