using DiversityWorkbench;
using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.Windows.Forms;

namespace DiversityAgents
{
    public partial class FormDiversityAgents : Form, DiversityWorkbench.WorkbenchUnitUserInterface
    {

        #region Parameter

        private readonly bool _BackLinkUpdateImplemented = true;

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;

        private System.Data.DataTable _dtApplicationSearchSelectionStrings;
        private System.Data.DataTable _dtContactList;
        private System.Data.DataTable _dtProjects;
        private System.Data.DataTable _dtAgentList;
        private System.Data.DataTable _dtRelationList;
        private System.Data.DataTable _dtHierarchy;
        private System.Data.DataTable _dtSynonymy;

        private DiversityAgents.Agent _Agent;
        private DiversityWorkbench.ServerConnection _ServerConnection;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgent;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentContact;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentKeyword;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentReference;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentRelation;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentProject;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentExternalDataList;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentHierarchyOther;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentIdentifier;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentDescriptor;
        private readonly int _CumulativeContactDisplayOrder = 500;
        public enum RequestType { SuperiorAgent, ReplacingAgent };
        private FormDiversityAgents.RequestType _RequestType;

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

        private int _AgentIDselectedInList = 0;

        // Parameter for command line processing (von Toni übernommen)
        private enum ProcessingMode { RegularInteractive, InteractiveQuery, SetDatabase, UpdateDatabase, HiddenCache, HiddenArchive, IrregularAbort };
        private ProcessingMode _Processing = ProcessingMode.RegularInteractive;
        private int _ProcessingResult = 0;



        #endregion

        #region Construction

        public FormDiversityAgents(bool ShowProgress = true)
        {
            InitializeComponent();
            try { DiversityWorkbench.Settings.ModuleName = "DiversityAgents"; }
            catch { }

            this.KeyPreview = true;

            // Show start window
            DiversityWorkbench.Forms.FormStarting fs = ShowProgress ? new DiversityWorkbench.Forms.FormStarting(DiversityWorkbench.Forms.FormStarting.Unit.Agents, "Starting DiversityAgents", "DiversityAgents ...", 6, "Init error log") : null;
            if (fs != null)
            {
                fs.StartPosition = FormStartPosition.CenterScreen;
                fs.setMax(25);
                fs.Show();
            }
            Application.DoEvents();
            try
            {

                DiversityWorkbench.ExceptionHandling.InitErrorLogFile();

                fs?.ShowCurrentStep("Copy Workbench Directory");
                DiversityAgents.Agent.CopyWorkbenchDirectory();

                this.FixDatasetBug();

                this.setDatabase();
                if (DiversityWorkbench.Settings.DatabaseName.Length == 0)
                {
                    this.transferPreviousSettingsToolStripMenuItem_Click(null, null);
                }

                fs?.ShowCurrentStep("Init Form");
                this.initForm();
                fs?.ShowCurrentStep("set Query Control Events");
                this.setQueryControlEvents();
                fs?.ShowCurrentStep("init Query");
                this.initQuery();
                fs?.ShowCurrentStep("init Exception Handling");
                this.initExceptionHandling();
                this._Agent = new DiversityAgents.Agent(this.ServerConnection);
                this.askOnExitToolStripMenuItem.Checked = DiversityAgents.Forms.FormDiversityAgentsSettings.Default.AskOnExit;

                // online manual
                this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();// ...Windows.Forms.Application.StartupPath + "\\DiversityAgents.chm";
                DiversityWorkbench.Forms.FormFeedback.HelpProviderHelpNamespace = this.helpProvider.HelpNamespace;

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                fs?.setEnd();
                fs?.Close();
            }

        }

        public FormDiversityAgents(int? AgentID,
            DiversityWorkbench.ServerConnection ServerConnection,
            string Title,
            int? WidthParent,
            int? HeightParent,
            int? RequestingAgentID,
            FormDiversityAgents.RequestType RequestType)
        {
            InitializeComponent();

            this.KeyPreview = true;

            try
            {
                this.menuStripMain.Visible = false;
                if (WidthParent != null) this.Width = (int)WidthParent - 30;
                else this.Width = this.Width - 30;
                if (HeightParent != null) this.Height = (int)HeightParent - 30;
                else this.Height = this.Height - 30;
                this.StartPosition = FormStartPosition.CenterParent;
                DiversityWorkbench.Settings.DatabasePort = ServerConnection.DatabaseServerPort;
                DiversityWorkbench.Settings.DatabaseServer = ServerConnection.DatabaseServer;
                DiversityWorkbench.Settings.DatabaseUser = ServerConnection.DatabaseUser;
                DiversityWorkbench.Settings.IsTrustedConnection = ServerConnection.IsTrustedConnection;
                DiversityWorkbench.Settings.Password = ServerConnection.DatabasePassword;
                this.userControlDialogPanel.Visible = true;
                this.setDatabase();
                this.initForm();
                this.setQueryControlEvents();
                this.initQuery();
                if (Title.Length > 0) this.Text = Title;
                this.initExceptionHandling();
                if (AgentID != null && DiversityWorkbench.Settings.ConnectionString != "")
                {
                    this.setAgent((int)AgentID);
                }
                this._RequestType = RequestType;
                if (this._RequestType == RequestType.ReplacingAgent ||
                    this._RequestType == RequestType.SuperiorAgent)
                {
                    System.Collections.Generic.List<DiversityWorkbench.UserControls.QueryRestrictionItem> QueryRestrictions = new List<DiversityWorkbench.UserControls.QueryRestrictionItem>();
                    if (RequestingAgentID != null)
                    {
                        DiversityWorkbench.UserControls.QueryRestrictionItem QAgentID = new DiversityWorkbench.UserControls.QueryRestrictionItem();
                        QAgentID.TableName = "Agent_Core";
                        QAgentID.ColumnName = "AgentID";
                        QAgentID.Restriction = " <> " + RequestingAgentID.ToString();
                        QueryRestrictions.Add(QAgentID);
                    }
                    DiversityWorkbench.UserControls.QueryRestrictionItem QReplaced = new DiversityWorkbench.UserControls.QueryRestrictionItem();
                    QReplaced.TableName = "Agent_Core";
                    QReplaced.ColumnName = "SynonymToAgentID";
                    QReplaced.Restriction = "IS NULL";
                    QueryRestrictions.Add(QReplaced);
                    this.userControlQueryList.setQueryRestrictionList(QueryRestrictions);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public FormDiversityAgents(string[] args)
            : this(false)
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
                    case "archive":
                        try
                        {
                            this.Hide();
                            DiversityWorkbench.ExceptionHandling.InitErrorLogFile("Archive");
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("FormDiversityAgents", "FormDiversityAgents(string[] args)", "Starting archive with arguments " + ArgumentsString);
                            // Setting the connection
                            DiversityWorkbench.Settings.DatabaseServer = Arguments[1];
                            DiversityWorkbench.Settings.DatabasePort = int.Parse(Arguments[2].ToString());
                            DiversityWorkbench.Settings.DatabaseName = Arguments[3];
                            DiversityWorkbench.Settings.IsTrustedConnection = true;
                            string ArchiveDirectory = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule("Archive");
                            if (Arguments.Count > 4 && Arguments[4].Length > 0)
                                ArchiveDirectory = Arguments[4];
                            DiversityWorkbench.Archive.FormArchiveAdministration fA = new DiversityWorkbench.Archive.FormArchiveAdministration(
                                ArchiveDirectory, "ProjectProxy", "Project", "ProjectID", "CreateArchive", "ArchiveProtocol", "AgentProject", "AgentID", this.ArchiveTables());
                            fA.CreateArchives();
                            Application.Exit();
                            this.Close();
                            System.Environment.Exit(1);
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                        break;
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
                                DiversityAgents.Properties.Settings.Default.DatabaseVersion,
                                "DiversityAgentsUpdate_",
                                this.helpProvider.HelpNamespace,
                                Properties.Resources.ResourceManager))
                            {
                                // Mark success
                                _ProcessingResult = 1;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            string message = ex.Message + "\r\n\r\n" + ArgumentsString;
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(this.Name, "FormDiversityAgents(string[] args)", message);
                            _Processing = ProcessingMode.IrregularAbort;
                        }
                        finally
                        {
                            this.Close();
                        }
                        #endregion
                        break;
                    default:
                        this.showQueryToolStripMenuItem_Click(null, null);
                        this.setConnection(args);
                        this.setDatabase();
                        this.setAgent(int.Parse(args[0]));
                        break;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Exception Handling

        private void initExceptionHandling()
        {
            //string Logfile = ...Windows.Forms.Application.StartupPath + "\\" + ...Windows.Forms.Application.ProductName.ToString() + "Error.log";
            string Logfile = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.ErrorLogFile();// ...Windows.Forms.Application.StartupPath + "\\" + System.Windows.Forms.Application.ProductName.ToString() + "Error.log";
            if (System.IO.File.Exists(Logfile))
                try
                {
                    System.IO.File.Delete(Logfile);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
        }
        #endregion

        #region Form

        /// <summary>
        /// Adding columns missing in the main table of form based dataset
        /// </summary>
        private void FixDatasetBug()
        {
            if (!this.dataSetAgent.Agent.Columns.Contains("PlaceOfBirth"))
            {
                System.Data.DataColumn C = new DataColumn("PlaceOfBirth", typeof(string));
                this.dataSetAgent.Agent.Columns.Add(C);
            }
            if (!this.dataSetAgent.Agent.Columns.Contains("PlaceOfDeath"))
            {
                System.Data.DataColumn C = new DataColumn("PlaceOfDeath", typeof(string));
                this.dataSetAgent.Agent.Columns.Add(C);
            }
        }

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
#if !DEBUG
            //this.tabControlHierarchyEtc.TabPages.Remove(this.tabPageKeywords);
            //this.tabControlHierarchyEtc.TabPages.Remove(this.tabPageDescriptors);
#endif

            this.AcceptButton = this.userControlDialogPanel.buttonOK;
            this.CancelButton = this.userControlDialogPanel.buttonCancel;
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.userControlDatePanelValidFrom.setDataBindings(this.agentBindingSource, "ValidFromDay", "ValidFromMonth", "ValidFromYear", "");
            this.userControlDatePanelValidUntil.setDataBindings(this.agentBindingSource, "ValidUntilDay", "ValidUntilMonth", "ValidUntilYear", "ValidDateSupplement");
            this.setUserControlDatabindings();
            this.initRemoteModules();
            //this.initModuleConnections();
            this.initBackLinks();
            //this.userControlQueryList.toolStripButtonBacklinkUpdate.Click += new System.EventHandler(this.BacklinkUpdate);
            this.splitContainerKeywords.SplitterDistance = this.splitContainerKeywords.Width - 180; // #4
            this.initJsonCache(); //#11
        }

        private void setAutoCompletion()
        {
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxAddressType);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxAgentTitle);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxAgentType);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxRevisionLevel);

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxAbbreviation);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxAgentRole);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxCity);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxCountry);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxGivenName);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxInheritedName);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxlContactNotes);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxNotes);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxPostalCode);
        }

        private void FormDiversityAgents_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.askOnExitToolStripMenuItem.Checked)
            {
                if (MessageBox.Show("Do you really want to exit DiversityAgents?", "Exit application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
            // Save data
            saveAgent(sender, e);

            // Save settings
            DiversityAgents.Forms.FormDiversityAgentsSettings.Default.AskOnExit = this.askOnExitToolStripMenuItem.Checked;
            if (this.userControlQueryList.QueryConditionVisiblity.Length > 0)
                DiversityAgents.Forms.FormDiversityAgentsSettings.Default.QueryConditionVisibility = this.userControlQueryList.QueryConditionVisiblity;
            DiversityAgents.Forms.FormDiversityAgentsSettings.Default.Save();
            if (this.userControlQueryList.ManyOrderByColumns())
            {
                DiversityWorkbench.Settings.ManyOrderColumnSave(DiversityWorkbench.Settings.ModuleName, "FormDiversityAgents", this.userControlQueryList.ManyOrderByColumns_Widths());
            }
        }

        private void FormDiversityAgents_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentDescriptor". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentDescriptorTableAdapter.Fill(this.dataSetAgent.AgentDescriptor);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentIdentifier". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentIdentifierTableAdapter.Fill(this.dataSetAgent.AgentIdentifier);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentHierarchyOther". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentHierarchyOtherTableAdapter.Fill(this.dataSetAgent.AgentHierarchyOther);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentRelationList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentRelationListTableAdapter.Fill(this.dataSetAgent.AgentRelationList);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentProjectList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentProjectListTableAdapter.Fill(this.dataSetAgent.AgentProjectList);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentImage". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentImageTableAdapter.Fill(this.dataSetAgent.AgentImage);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentRelation". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentRelationTableAdapter.Fill(this.dataSetAgent.AgentRelation);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentReference". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentReferenceTableAdapter.Fill(this.dataSetAgent.AgentReference);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent1.Agent". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentTableAdapter.Fill(this.dataSetAgent.Agent);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentRelationList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentRelationListTableAdapter.Fill(this.dataSetAgent.AgentRelationList);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentRelation". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentRelationTableAdapter.Fill(this.dataSetAgent.AgentRelation);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentReference". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentReferenceTableAdapter.Fill(this.dataSetAgent.AgentReference);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentImage". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentImageTableAdapter.Fill(this.dataSetAgent.AgentImage);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAgent.AgentProjectList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentProjectListTableAdapter.Fill(this.dataSetAgent.AgentProjectList);

        }

        #endregion

        #region About
        //private void About()
        //{
        //    if (this.backgroundWorkerAbout.IsBusy)
        //        this.backgroundWorkerAbout.CancelAsync();
        //    if (!this.backgroundWorkerAbout.IsBusy)
        //        this.backgroundWorkerAbout.RunWorkerAsync();
        //    else
        //    {
        //        DiversityAgents.FormAbout f = new FormAbout();
        //        f.ShowDialog();
        //    }
        //}

        //private void backgroundWorkerAbout_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    DiversityAgents.FormAbout f = new FormAbout();
        //    f.ShowDialog();
        //}

        //private void FormDiversityAgents_Activated(object sender, EventArgs e)
        //{
        //    this.backgroundWorkerAbout.CancelAsync();
        //}

        #endregion

        #region Database

        private bool setDatabase()
        {
            bool OK = true;
            this.dataSetAgent.Clear();
            DiversityWorkbench.Database.DatabaseRolesReset();
            if (DiversityWorkbench.Settings.ConnectionString != "" && this.DatabaseAccessible)
            {
                try
                {
                    this.resetSqlAdapters();

                    // neu - analog zu Collection
                    DiversityWorkbench.WorkbenchUnit.ResetWorkbenchUnitConnections();

                    DiversityWorkbench.Settings.ServerConnection.DatabaseName = DiversityWorkbench.Settings.DatabaseName;
                    DiversityWorkbench.Settings.ServerConnection.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
                    DiversityWorkbench.Settings.ServerConnection.DatabasePassword = DiversityWorkbench.Settings.Password;
                    DiversityWorkbench.Settings.ServerConnection.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
                    DiversityWorkbench.Settings.ServerConnection.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                    DiversityWorkbench.Settings.ServerConnection.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;

                    // hier wird die komplette liste aller DB's erzeugt - oder es erscheint nix
                    this.Agent.ServerConnection = DiversityWorkbench.Settings.ServerConnection;

                    this.userControlQueryList.setConnection(DiversityWorkbench.Settings.ConnectionString, "Agent");
                    this.setToolbarPermissions();
                    this.setSearchOptions();
                    this.setEnumDataSource();
                    this.setAutoCompletion();
                    this.userControlQueryList.BacklinkUpdateEnabled = true; // set _BackLinkUpdateImplemented to true if finished
#if DEBUG
#endif


                    // neu - analog zu Collection
                    this.buildSearchMenu();
                    this.initRemoteModules();
                    this.FormFunctions.setDescriptions();

                    DiversityWorkbench.WorkbenchUnit.LoadAddedRemoteConnections();

                    this.initDescriptorTypeImages();

                }
                catch (Exception ex)
                {
                    OK = false;
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
                OK = false;
            this.splitContainerData.Visible = false;
            this.FormFunctions.setTitle();
            this.userControlQueryList.toolStripButtonConnection.ToolTipText = this.FormFunctions.ConnectionInfo();
            // weg - analog Collection
            //this.buildSearchMenu();
            this.FormFunctions.setDescriptions();
            this.setMenu();
            //this.setAdminMenu();
            //this.setUpdateMenu();
            this.userControlQueryList.setConnection(DiversityWorkbench.Settings.ConnectionString, "Agent");
            // weg - analog Collection
            //this.initRemoteModules();

            return OK;
        }

        private void resetSqlAdapters()
        {
            this._SqlDataAdapterAgent = null;
            this._SqlDataAdapterAgentContact = null;
            this._SqlDataAdapterAgentExternalDataList = null;
            this._SqlDataAdapterAgentKeyword = null;
            this._SqlDataAdapterAgentDescriptor = null;
            this._SqlDataAdapterAgentHierarchyOther = null;
            this._SqlDataAdapterAgentIdentifier = null;
            this._SqlDataAdapterAgentImage = null;
            this._SqlDataAdapterAgentProject = null;
            this._SqlDataAdapterAgentReference = null;
            this._SqlDataAdapterAgentRelation = null;
        }

        private bool DatabaseCompatible
        {
            get
            {
                bool OK = false;
                try
                {
                    if (this.setAgent(-1))
                        OK = true;
                }
                catch { }
                return OK;
            }
        }

        private bool DatabaseAccessible
        {
            get
            {
                bool OK = true;
                try
                {
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand("SELECT 1", con);
                    con.Open();
                    c.ExecuteNonQuery();
                    con.Close();
                }
                catch
                {
                    OK = false;
                }
                return OK;
            }
        }

        private void setConnection(string[] args)
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

        #endregion

        #region Picklists

        private void setEnumDataSource()
        {
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxAddressType, "AddressType_Enum", con);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxAgentTitle, "AgentTitle_Enum", con);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxAgentType, "AgentType_Enum", con);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxRevisionLevel, "RevisionLevel_Enum", con);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxSynonymisationType, "AgentSynonymisationType_Enum", con);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxRelationType, "AgentRelationType_Enum", con);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxGender, "AgentGender_Enum", con);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxIdentifierType, "AgentIdentifierType_Enum", con);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxImageType, "AgentImageType_Enum", con);

                this.comboBoxRelatedAgent.DataSource = this.DtAgentList;
                this.comboBoxRelatedAgent.DisplayMember = "AgentName";
                this.comboBoxRelatedAgent.ValueMember = "AgentID";
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

                // Agents
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorAgent.IWorkbenchUnit = A;

                // Collection
                DiversityWorkbench.CollectionSpecimen C = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorCollection.IWorkbenchUnit = C;

                // Gazetteer
                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorGazetteer.IWorkbenchUnit = G;

                // Project
                DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorProject.IWorkbenchUnit = P;

                // References
                DiversityWorkbench.Reference L = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
                this.userControlModuleRelatedEntryDescriptorReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;

                // Gazetteer
                DiversityWorkbench.SamplingPlot SP = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorPlots.IWorkbenchUnit = SP;

                // Gazetteer
                DiversityWorkbench.ScientificTerm ST = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorTerms.IWorkbenchUnit = ST;

                // Gazetteer
                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryDescriptorTaxa.IWorkbenchUnit = T;


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
                // References
                this.userControlModuleRelatedEntryReference.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;

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
                this.setUserControlModuleRelatedEntryDescriptor(this.userControlModuleRelatedEntryDescriptorAgent);
                this.setUserControlModuleRelatedEntryDescriptor(this.userControlModuleRelatedEntryDescriptorCollection);
                this.setUserControlModuleRelatedEntryDescriptor(this.userControlModuleRelatedEntryDescriptorGazetteer);
                this.setUserControlModuleRelatedEntryDescriptor(this.userControlModuleRelatedEntryDescriptorPlots);
                this.setUserControlModuleRelatedEntryDescriptor(this.userControlModuleRelatedEntryDescriptorProject);
                this.setUserControlModuleRelatedEntryDescriptor(this.userControlModuleRelatedEntryDescriptorReference);
                this.setUserControlModuleRelatedEntryDescriptor(this.userControlModuleRelatedEntryDescriptorTaxa);
                this.setUserControlModuleRelatedEntryDescriptor(this.userControlModuleRelatedEntryDescriptorTerms);

                // Reference
                this.userControlModuleRelatedEntryReference.bindToData("AgentReference", "ReferenceTitle", "ReferenceURI", this.agentReferenceBindingSource);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setUserControlModuleRelatedEntryDescriptor(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry U)
        {
            U.bindToData("AgentDescriptor", "Descriptor", "URL", this.agentDescriptorBindingSource);
            U.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
        }

        #endregion

        #region Menu

        private void setMenu()
        {
            this.setAdminMenu();
            this.setHelpMenu();
            this.setDataMenu();
            this.setUpdateMenu();
            this.setConnectionMenu();
        }

        #region Connection

        private void createDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DiversityWorkbench.Settings.ModuleName.Length == 0)
                {
                    string Name = System.Reflection.Assembly.GetExecutingAssembly().FullName;
                    Name = Name.Substring(0, Name.IndexOf(","));
                    DiversityWorkbench.Settings.ModuleName = Name;
                }
                System.Collections.Generic.Dictionary<string, string> Versions = this.createDatabase_Versions();
                new Dictionary<string, string>();
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
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
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
                            if (de.Key.ToString().StartsWith("DiversityAgentsUpdate_000000_to_"))
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

        private void setConnectionMenu()
        {
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                if (this._DatabaseRoles.Contains("DiversityWorkbenchAdministrator") || this._DatabaseRoles.Contains("db_owner"))
                {
                    this.currentServerActivityToolStripMenuItem.Visible = true;
                }
                else
                    this.currentServerActivityToolStripMenuItem.Visible = false;

            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void databaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase("DiversityAgents", this.createDatabase_Versions());
                f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "Login");// ...Windows.Forms.Application.StartupPath + "\\DiversityAgents.chm", "Login");
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this.setDatabase();
                    this._DatabaseRoles = DiversityWorkbench.Database.DatabaseRoles();
                    this.initQuery();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void reconnectToDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.Settings.ConnectionString != "" && this.FormFunctions.DatabaseAccessible)
            {
                DiversityAgents.Forms.FormDiversityAgentsSettings.Default.Save();
                DiversityWorkbench.Settings.SaveWorkbenchSettings();
                setDatabase();
            }
            else
            {
                MessageBox.Show("Connection to database is missing", "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void moduleConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormConnectionAdministration f = new DiversityWorkbench.Forms.FormConnectionAdministration(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), true);// ...Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm");
            f.ShowDialog();
        }

        private void transferPreviousSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Settings.TransferPreviousSettings();

                DiversityAgents.Forms.FormDiversityAgentsSettings.Default.Upgrade();
                DiversityAgents.Forms.FormDiversityAgentsSettings.Default.Reload();
                DiversityAgents.Forms.FormDiversityAgentsSettings.Default.Save();

                this.askOnExitToolStripMenuItem.Checked = DiversityAgents.Forms.FormDiversityAgentsSettings.Default.AskOnExit;
                this.setDatabase();
            }
            catch { }
        }

        private void timeoutForDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.TimeoutDatabase = this.setTimeout(timeoutForDatabaseToolStripMenuItem, "database queries", DiversityWorkbench.Settings.TimeoutDatabase, "0 = no limit");
        }

        private void timeoutForWebrequestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.TimeoutWebInSeconds = this.setTimeout(timeoutForWebrequestsToolStripMenuItem, "web requests", DiversityWorkbench.Settings.TimeoutWebInSeconds, "0 = do not search for web resources");
        }

        private int setTimeout(System.Windows.Forms.ToolStripMenuItem MenuItem, string Target, int CurrentValue, string Comment_0)
        {
            int Timeout = CurrentValue;
            string Message = "Please enter the timeout for " + Target + " in seconds";
            if (Comment_0.Length > 0)
                Message += "\r\n" + Comment_0;
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Timeout", Message);
            f.setIcon(DiversityAgents.Properties.Resources.Time);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null)
            {
                Timeout = (int)f.Integer;
                MenuItem.ToolTipText = Timeout.ToString() + " sec.";
            }
            return Timeout;
        }

        private void currentServerActivityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFunctions.ShowServerActiviy();
        }

        #endregion

        #region Query

        private void showQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.showQueryToolStripMenuItem.Checked)
                {
                    this.splitContainerMain.SplitterWidth = 1;
                    this.splitContainerMain.Panel1MinSize = 0;
                    this.showQueryToolStripMenuItem.Checked = false;
                    this.userControlQueryList.Visible = false;
                    this.splitContainerMain.SplitterDistance = 0;
                }
                else
                {
                    this.showQueryToolStripMenuItem.Checked = true;
                    this.userControlQueryList.Visible = true;
                    this.splitContainerMain.SplitterDistance = 181;
                    this.splitContainerMain.SplitterWidth = 4;
                    this.splitContainerMain.Panel1MinSize = 25;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private void showIgnoredDataToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (this.showIgnoredDataToolStripMenuItem.Checked)
        //        this.showIgnoredDataToolStripMenuItem.Checked = false;
        //    else
        //        this.showIgnoredDataToolStripMenuItem.Checked = true;
        //    this.setSearchOptions();
        //}

        #region Menu - Predefined statements

        /// <summary>
        /// Event for the MenuItems in the search menu
        /// </summary>
        /// <param name="sender">The Menu Item</param>
        /// <param name="e"></param>
        private void menuItemSearchOnClick(object sender, System.EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem m = (System.Windows.Forms.ToolStripMenuItem)sender;
            string s = m.Text;
            s = s.Substring(s.IndexOf(" - ") + 2).Trim();
            try
            {
                System.Data.DataRow[] rr = this._dtApplicationSearchSelectionStrings.Select("SqlStringIdentifier = '" + s.ToString() + "'");
                System.Data.DataRow r = rr[0];
                string SQL = r["SQLString"].ToString();
                string Description = r["Description"].ToString();
                string Table = r["ItemTable"].ToString();
                if (SQL.Length > 0)
                {
                    this.userControlQueryList.IsPredefinedQuery = true;
                    this.userControlQueryList.setQueryConditions(s, Table, SQL, Description);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// building the menu for editing the data. For predefined entries menuItems are added
        /// </summary>
        public void buildSearchMenu()
        {
            this.FormFunctions.buildSearchMenu(ref this.predefinedQueriesToolStripMenuItem, ref this._dtApplicationSearchSelectionStrings, this.menuItemSearchOnClick);
            this.userControlQueryList.setPredefinedQueryPersistentConditionList(this.Agent.PredefinedQueryPersistentConditionList());
        }

        #endregion

        #endregion

        #region View
        private void showImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.showImagesToolStripMenuItem.Checked)
                {
                    this.showImagesToolStripMenuItem.Checked = false;
                    this.splitContainerHierarchyImages.Panel2Collapsed = true;
                }
                else
                {
                    this.showImagesToolStripMenuItem.Checked = true;
                    this.splitContainerHierarchyImages.Panel2Collapsed = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void showProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.showProjectsToolStripMenuItem.Checked)
                {
                    this.showProjectsToolStripMenuItem.Checked = false;
                    this.splitContainerProjects.Panel2Collapsed = true;
                }
                else
                {
                    this.showProjectsToolStripMenuItem.Checked = true;
                    this.splitContainerProjects.Panel2Collapsed = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void showSynonymsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.showSynonymsToolStripMenuItem.Checked)
                //{
                //    this.showSynonymsToolStripMenuItem.Checked = false;
                //    this.splitContainerHierarchySynonyms.Panel2Collapsed = true;
                //}
                //else
                //{
                //    this.showSynonymsToolStripMenuItem.Checked = true;
                //    this.splitContainerHierarchySynonyms.Panel2Collapsed = false;
                //}
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Data

        private void setDataMenu()
        {
            string TableName = "Agent";
            bool OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
            this.tableEditorsToolStripMenuItem.Visible = OK;

            OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
            this.removeAgentsToolStripMenuItem.Visible = OK;

            OK = this.FormFunctions.getObjectPermissions("AgentProject", "DELETE");
            this.removeFromProjectToolStripMenuItem.Visible = OK;
#if DEBUG
#endif
        }

        #region Backup  
        private void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to create a backup of the whole database?", "Backup", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string Message;
                string Filename = "";
                if (DiversityWorkbench.Database.BackupCopy(DiversityWorkbench.Settings.DatabaseName, ref Filename, out Message))
                    System.Windows.Forms.MessageBox.Show($"Backup file \r\n'{Filename}' \r\ncreated at server.", "Backup success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(Message, "Backup error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cSVbcpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormExportToCSV f = new DiversityWorkbench.Forms.FormExportToCSV();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        #endregion

        #region Import

        private void agentImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("You are not connected to any database");
                return;
            }
            this.StartWizard("Agent");
        }

        private void StartWizard(string Target)
        {
            try
            {
                this.Enabled = false;

                // Show start window
                DiversityWorkbench.Import.FormStartWizard fs = new DiversityWorkbench.Import.FormStartWizard();
                fs.StartPosition = FormStartPosition.CenterParent;
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
                    case "Agent":
                        ImportSteps = this.ImportStepsAgent(fs);
                        break;
                }
                if (ImportSteps == null)
                    ImportSteps = new Dictionary<string, DiversityWorkbench.Import.Step>();
                fs.ShowCurrentStep("Import steps for " + Target + " created. Opening import form\r\n||||||||..");
                DiversityWorkbench.Import.FormWizard f = new DiversityWorkbench.Import.FormWizard(Target, DiversityAgents.Properties.Settings.Default.DatabaseVersion, ImportSteps, this.helpProvider.HelpNamespace, fs);
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

        private enum ImportWizardMainTables { Agent, AgentProject, AgentContactInformation, AgentExternalID, AgentIdentifier, AgentKeyword, AgentReference, AgentDescriptor };


        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> ImportStepsAgent(DiversityWorkbench.Import.iStartingMessage iMessage)
        {
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> IS = new Dictionary<string, DiversityWorkbench.Import.Step>();

            try
            {
                iMessage.ShowCurrentStep("Creating the steps for the agent\r\n|.........");

                DiversityWorkbench.Import.DataTable DTAgent = this.ImportWizardDataTable("Agent", "", "Agent", DiversityWorkbench.Import.DataTable.Parallelity.unique, (int)DiversityAgents.FormDiversityAgents.ImportWizardMainTables.Agent, "Version, AgentParentID, SynonymToAgentID, SynonymisationType, RevisionLevel", "AgentID, AgentName", DiversityAgents.Properties.Resources.Agent);
                DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn = DTAgent.DataColumns["AgentName"];
                DTAgent.DataColumns["AgentName"].AllowInsert = false;
                DiversityWorkbench.Import.Step SAgent = DiversityWorkbench.Import.Step.GetStepTemplate(DTAgent, DiversityAgents.Properties.Resources.Agent, 0, true, null);
                SAgent.StepColumnGroups = this.ImportWizardColumnGroupsAgent();
                SAgent.MustSelect = true;
                IS.Add(DTAgent.PositionKey, SAgent);

                // Project
                iMessage.ShowCurrentStep("Creating the steps for the project\r\n||........");
                DiversityWorkbench.Import.DataTable DTProjects = this.ImportWizardDataTable("AgentProject", "Agent", "Project", DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversityAgents.FormDiversityAgents.ImportWizardMainTables.AgentProject, "", "", DiversityAgents.Properties.Resources.Project);
                DTProjects.AddDatasource("ProjectID", "SELECT ProjectID AS Value, Project AS DisplayText " +
                    "FROM ProjectProxy " +
                    "ORDER BY DisplayText ");
                DiversityWorkbench.Import.Step SProjects = DiversityWorkbench.Import.Step.GetStepTemplate(DTProjects, DiversityAgents.Properties.Resources.Project, 1, false, null);
                SProjects.MustSelect = true;
                IS.Add(DTProjects.PositionKey, SProjects);

                // References
                iMessage.ShowCurrentStep("Creating the steps for the references \r\n|||.......");
                DiversityWorkbench.Import.DataTable DTAgentReference = this.ImportWizardDataTable("AgentReference", "Agent", "Reference", DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversityAgents.FormDiversityAgents.ImportWizardMainTables.AgentReference, "", "", DiversityAgents.Properties.Resources.References);
                DiversityWorkbench.Import.Step SAgentReference = DiversityWorkbench.Import.Step.GetStepTemplate(DTAgentReference, DiversityAgents.Properties.Resources.References, 1, false, null);
                IS.Add(DTAgentReference.PositionKey, SAgentReference);

                // Identifier
                iMessage.ShowCurrentStep("Creating the steps for the identifier \r\n||||......");
                DiversityWorkbench.Import.DataTable DTAgentIdentifier = this.ImportWizardDataTable("AgentIdentifier", "Agent", "Identifier", DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversityAgents.FormDiversityAgents.ImportWizardMainTables.AgentIdentifier, "", "", DiversityAgents.Properties.Resources.Identifier);
                DiversityWorkbench.Import.Step SAgentIdentifier = DiversityWorkbench.Import.Step.GetStepTemplate(DTAgentIdentifier, DiversityAgents.Properties.Resources.Identifier, 1, false, null);
                IS.Add(DTAgentIdentifier.PositionKey, SAgentIdentifier);

                // External IDs
                iMessage.ShowCurrentStep("Creating the steps for the external ID\r\n|||||.....");
                DiversityWorkbench.Import.DataTable DTAgentExternalID = this.ImportWizardDataTable("AgentExternalID", "Agent", "ExternalID", DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversityAgents.FormDiversityAgents.ImportWizardMainTables.AgentExternalID, "", "", DiversityAgents.Properties.Resources.Datenbank);
                DTAgentExternalID.AddDatasource("ExternalDatabaseID", "SELECT ExternalDatabaseID AS Value, ExternalDatabaseName AS DisplayText " +
                    "FROM AgentExternalDatabase " +
                    "ORDER BY DisplayText ");
                DiversityWorkbench.Import.Step SAgentExternalID = DiversityWorkbench.Import.Step.GetStepTemplate(DTAgentExternalID, DiversityAgents.Properties.Resources.Datenbank, 1, false, null);
                IS.Add(DTAgentExternalID.PositionKey, SAgentExternalID);

                // Keyword
                iMessage.ShowCurrentStep("Creating the steps for the keywords\r\n||||||....");
                DiversityWorkbench.Import.DataTable DTAgentKeyword = this.ImportWizardDataTable("AgentKeyword", "Agent", "Keyword", DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversityAgents.FormDiversityAgents.ImportWizardMainTables.AgentKeyword, "", "", DiversityAgents.Properties.Resources.KeyBig);
                DiversityWorkbench.Import.Step SAgentKeyword = DiversityWorkbench.Import.Step.GetStepTemplate(DTAgentKeyword, DiversityAgents.Properties.Resources.KeyBig, 1, false, null);
                IS.Add(DTAgentKeyword.PositionKey, SAgentKeyword);

                // Contacts
                iMessage.ShowCurrentStep("Creating the steps for the contacts\r\n||||||||..");
                DiversityWorkbench.Import.DataTable DTAgentContactInformation = this.ImportWizardDataTable("AgentContactInformation", "Agent", "Contacts", DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversityAgents.FormDiversityAgents.ImportWizardMainTables.AgentContactInformation, "", "", DiversityAgents.Properties.Resources.Adress);
                DiversityWorkbench.Import.Step SAgentContactInformation = DiversityWorkbench.Import.Step.GetStepTemplate(DTAgentContactInformation, DiversityAgents.Properties.Resources.Adress, 1, false, null);
                IS.Add(DTAgentContactInformation.PositionKey, SAgentContactInformation);

                // Descriptors
                iMessage.ShowCurrentStep("Creating the steps for the descriptors\r\n|||||||||.");
                DiversityWorkbench.Import.DataTable DTAgentDescriptor = this.ImportWizardDataTable("AgentDescriptor", "Agent", "Descriptor", DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversityAgents.FormDiversityAgents.ImportWizardMainTables.AgentDescriptor, "", "", DiversityAgents.Properties.Resources.KeyBlue);
                DiversityWorkbench.Import.Step SAgentDescriptor = DiversityWorkbench.Import.Step.GetStepTemplate(DTAgentDescriptor, DiversityAgents.Properties.Resources.KeyBlue, 1, false, null);
                IS.Add(DTAgentDescriptor.PositionKey, SAgentDescriptor);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return IS;
        }

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> ImportWizardColumnGroupsAgent()
        {

            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> ColumnGroups = new Dictionary<string, DiversityWorkbench.Import.StepColumnGroup>();

            DiversityWorkbench.Import.StepColumnGroup Gdates = this.ImportWizardStepColumnGroup("Dates",
                DiversityAgents.Properties.Resources.Time,
                "ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement");
            ColumnGroups.Add(Gdates.DisplayText, Gdates);

            return ColumnGroups;
        }

        private DiversityWorkbench.Import.StepColumnGroup ImportWizardStepColumnGroup(string DisplayText, System.Drawing.Image Image, string Columns)
        {
            System.Collections.Generic.List<string> L = new List<string>();
            Columns = Columns.Replace(" ", "");
            string[] CC = Columns.Split(new char[] { ',' });
            for (int i = 0; i < CC.Length; i++)
                L.Add(CC[i]);
            DiversityWorkbench.Import.StepColumnGroup G = new DiversityWorkbench.Import.StepColumnGroup(Image, DisplayText, L);
            return G;
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

        #region Export
        private void ExportWizardAgentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.userControlQueryList.ListOfIDs.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DiversityWorkbench.Export.Exporter.ResetAll();

            // getting the IDs
            System.Collections.Generic.List<int> L = new List<int>();
            System.Data.DataTable dt = new DataTable();
            string SQL = "";
            foreach (int ID in this.userControlQueryList.ListOfIDs)
            {
                if (SQL.Length > 0) SQL += ", ";
                SQL += ID.ToString();
            }
            SQL = "SELECT DISTINCT T.AgentID " +
                "FROM  Agent AS T " +
                "WHERE (T.AgentID IN (" + SQL + ")) ";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            foreach (System.Data.DataRow R in dt.Rows)
                L.Add(int.Parse(R[0].ToString()));
            if (L.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No data found");
                this.Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            // getting the tables
            // getting the template tables
            // Agents
            DiversityWorkbench.Export.Table T_Agent = DiversityWorkbench.Export.Exporter.AddTemplateTable("Agent ", "Agent ", DiversityWorkbench.Export.Table.Parallelity.unique, DiversityAgents.Properties.Resources.Agent, null, null, null, "");
            T_Agent.IsStartPoint = true;

            // dependent tables
            DiversityWorkbench.Export.Table TAgentContactInformation = DiversityWorkbench.Export.Exporter.AddTemplateTable("AgentContactInformation", "Contacts", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityAgents.Properties.Resources.Adress, T_Agent, null, null, "");
            DiversityWorkbench.Export.Table TAgentDescriptor = DiversityWorkbench.Export.Exporter.AddTemplateTable("AgentDescriptor", "Descriptor", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityAgents.Properties.Resources.KeyBlue, T_Agent, null, null, "");
            DiversityWorkbench.Export.Table TAgentIdentifier = DiversityWorkbench.Export.Exporter.AddTemplateTable("AgentIdentifier", "Identifier", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityAgents.Properties.Resources.Identifier, T_Agent, null, null, "");
            DiversityWorkbench.Export.Table TAgentImage = DiversityWorkbench.Export.Exporter.AddTemplateTable("AgentImage", "Images", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityAgents.Properties.Resources.Icones, T_Agent, null, null, "");
            DiversityWorkbench.Export.Table TAgentKeyword = DiversityWorkbench.Export.Exporter.AddTemplateTable("AgentKeyword", "Keywords", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityAgents.Properties.Resources.KeyBig, T_Agent, null, null, "");
            System.Collections.Generic.Dictionary<string, string> TM = new Dictionary<string, string>();
            TM.Add("ReferenceURI", "DiversityReferences");
            DiversityWorkbench.Export.Table TAgentReference = DiversityWorkbench.Export.Exporter.AddTemplateTable("AgentReference", "References", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityAgents.Properties.Resources.References, T_Agent, null, TM, "");
            DiversityWorkbench.Export.Table TAgentRelation = DiversityWorkbench.Export.Exporter.AddTemplateTable("AgentRelation", "Relations", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityAgents.Properties.Resources.Parent, T_Agent, null, null, "");


            // Table list
            System.Collections.Generic.List<DiversityWorkbench.Export.Table> TT = new List<DiversityWorkbench.Export.Table>();
            TT.Add(T_Agent);

            DiversityWorkbench.Export.FormExport f = new DiversityWorkbench.Export.FormExport(T_Agent, L, TT, this.helpProvider.HelpNamespace);
            f.Width = this.Width - 10;
            f.Height = this.Height - 10;
            f.StartPosition = FormStartPosition.CenterParent;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            f.ShowDialog();

        }

        #endregion

        #region Deleting

        private void removeAgentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<int> IDs = this.userControlQueryList.ListOfSelectedIDs;
            if (System.Windows.Forms.MessageBox.Show("Do you want to remove " + IDs.Count.ToString() + " selected agents?", "Delete agents?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int failed = 0;
                int deleted = 0;
                foreach (int ID in IDs)
                {
                    string SQL = "DELETE FROM Agent WHERE (AgentID = " + ID.ToString() + ")";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, true))
                        deleted++;
                    else failed++;
                }
                string Info = deleted.ToString() + " from " + IDs.Count.ToString() + " agents deleted";
                if (failed > 0) Info += "\r\n" + failed.ToString() + " agents could not be deleted";
                System.Windows.Forms.MessageBox.Show(Info);
                this.userControlQueryList.StartQuery();
            }
        }

        private void removeFromProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ProjectID = this.userControlQueryList.ProjectID;
            if (ProjectID > -1)
            {
                string Project = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT Project FROM ProjectProxy WHERE ProjectID = " + ProjectID.ToString());
                System.Collections.Generic.List<int> IDs = this.userControlQueryList.ListOfSelectedIDs;
                if (System.Windows.Forms.MessageBox.Show("Do you want to remove " + IDs.Count.ToString() + " selected agents from the selected project " + Project, "Remove from project", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    int failed = 0;
                    int deleted = 0;
                    foreach (int ID in IDs)
                    {
                        int i;
                        string SQL = "SELECT COUNT(*) AS Anzahl FROM AgentProject AS p WHERE (AgentID = " + ID.ToString() + ") AND (ProjectID = " + ProjectID.ToString() + ")";
                        if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i) && i > 1)
                        {
                            SQL = "DELETE FROM AgentProject AS p WHERE (AgentID = " + ID.ToString() + ") AND (ProjectID = " + ProjectID.ToString() + ")";
                            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, true))
                                deleted++;
                            else failed++;
                        }
                        else failed++;
                    }
                    string Info = deleted.ToString() + " from " + IDs.Count.ToString() + " agents removed from project " + Project;
                    if (failed > 0) Info += "\r\n" + failed.ToString() + " agents could not be removed\r\n(Last projects in the list can not be removed)";
                    System.Windows.Forms.MessageBox.Show(Info);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Please select a project");
        }


        #endregion


        #region TableEditors

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
                    TableName = "Agent";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    this.agentToolStripMenuItem.Visible = OK;
                    if (OK) Any = true;

                    // Contact
                    TableName = "AgentContactInformation";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    this.contactToolStripMenuItem.Visible = OK;
                    if (OK) Any = true;

                    // Reference
                    TableName = "AgentReference";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    this.referenceToolStripMenuItem.Visible = OK;
                    if (OK) Any = true;

                    this.tableEditorsToolStripMenuItem.Visible = Any;
                }
                else
                {
                    this.tableEditorsToolStripMenuItem.Visible = false;

                    this.agentToolStripMenuItem.Visible = false;
                    this.contactToolStripMenuItem.Visible = false;
                    this.referenceToolStripMenuItem.Visible = false;
                }
            }
            catch (Exception ex)
            {
                this.administrationToolStripMenuItem.Visible = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void agentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("Agent", DiversityAgents.Properties.Resources.Agent);
        }

        private void contactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("AgentContactInformation", DiversityAgents.Properties.Resources.Adress);
        }

        private void referenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("AgentReference", DiversityAgents.Properties.Resources.References);
        }

        private void otherHierarchyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("AgentHierarchyOther", DiversityAgents.Properties.Resources.Hierarchy);
        }

        private void identifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("AgentIdentifier", DiversityAgents.Properties.Resources.Identifier);
        }

        private void descriptorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("AgentDescriptor", DiversityAgents.Properties.Resources.KeyBlue);
        }

        private void relationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("AgentRelation", DiversityAgents.Properties.Resources.Parent);
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
            if (Table == "Agent")
            {
                ReadOnlyColumns.Add("AgentName");
                ReadOnlyColumns.Add("Version");
            }
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
            System.Data.DataTable dtIDs = new DataTable();
            string SQL = "SELECT AgentID FROM AgentID_FullAccess";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtIDs);
            foreach (int i in this.userControlQueryList.ListOfIDs)
            {
                System.Data.DataRow[] rr = dtIDs.Select("AgentID = " + i.ToString());
                if (rr.Length == 0) continue;
                if (IDs.Length > 0) IDs += ",";
                IDs += i.ToString();
            }
            SQL = "SELECT * FROM " + Table + " T WHERE T.AgentID IN (" + IDs + ");"; // AND T.AgentID IN (SELECT AgentID FROM AgentID_FullAccess)";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(Icon, ad, ReadOnlyColumns, 9999);
            f.StartPosition = FormStartPosition.CenterParent;
            f.Width = this.Width - 10;
            f.Height = this.Height - 10;
            f.setHelpProvider(this.helpProvider.HelpNamespace, "Table editor");
            bool SetTimeout = false;
            try
            {
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                SetTimeout = true;
            }
            //if (SetTimeout)
            //{
            //    int? Timeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout;
            //    DiversityWorkbench.FormGetInteger ftimeout = new DiversityWorkbench.FormGetInteger(Timeout, "Set timeout", "A timeout occured. Please set the seconds you are prepared to wait");
            //    ftimeout.ShowDialog();
            //    if (ftimeout.DialogResult == System.Windows.Forms.DialogResult.OK && ftimeout.Integer != null)
            //    {
            //        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout = (int)ftimeout.Integer;
            //        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
            //    }
            //}
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion        

        #region Backlinks

        private void diversityCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection, !this.diversityCollectionToolStripMenuItem.Checked);
            this.initBackLinksCollection();
        }

        private void diversityProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.Projects, !this.diversityProjectsToolStripMenuItem.Checked);
            this.initBackLinksProjects();
        }

        private void diversityReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return;
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.References, !this.diversityReferencesToolStripMenuItem.Checked);
            this.initBackLinksReferences();
        }

        private void diversitySamplingPlotsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return;
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots, !this.diversitySamplingPlotsToolStripMenuItem.Checked);
            this.initBackLinksPlots();
        }

        private void diversityScientificTermsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return;
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms, !this.diversityScientificTermsToolStripMenuItem.Checked);
            this.initBackLinksTerms();
        }

        #endregion

        #region Archive

        private void administrateArchivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Archive.FormArchiveAdministration f = new DiversityWorkbench.Archive.FormArchiveAdministration(
                this.ArchiveTables(),
                "Administrate the scheduler based creation of archives",
                "ProjectProxy",
                "Project",
                "ProjectID",
                "CreateArchive",
                "ArchiveProtocol",
                " WHERE ProjectID IN (SELECT ProjectID FROM ProjectList)");
            f.setHelp("Archive");
            f.ShowDialog();
        }

        private void createArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;

                // Show start window
                DiversityWorkbench.Import.FormStartWizard fs = new DiversityWorkbench.Import.FormStartWizard("Initialise archive ...", "Archive", DiversityAgents.Properties.Resources.ArchivCreate);
                fs.Show();
                Application.DoEvents();

                DiversityWorkbench.Archive.FormCreateArchive f = new DiversityWorkbench.Archive.FormCreateArchive(
                    "Select the project you want to create an archive from",
                    "ProjectProxy",
                    "Project",
                    "ProjectID",
                    " WHERE ProjectID IN (SELECT ProjectID FROM ProjectList)",
                    "AgentProject",
                    "AgentID",
                    "ArchiveProtocol",
                    this.ArchiveTables(),
                    "ProjectDataLastChanges");

                // Close start window
                fs.Close();

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

        private System.Collections.Generic.Dictionary<string, string> ArchiveTables()
        {
            System.Collections.Generic.Dictionary<string, string> DataTables = new Dictionary<string, string>();
            DataTables.Add("AgentProject", "ProjectProxy");
            DataTables.Add("Agent", "AgentProject");
            // Contact tables
            DataTables.Add("AgentContactInformation", "Agent");
            DataTables.Add("AgentExternalID", "Agent");
            DataTables.Add("AgentExternalDatabase", "AgentExternalID");
            // Specimen Identifier tables
            DataTables.Add("AgentIdentifier", "Agent");
            DataTables.Add("AgentIdentifierType_Enum", "AgentIdentifier");
            // other tables
            DataTables.Add("AgentImage", "Agent");
            DataTables.Add("AgentKeyword", "Agent");
            DataTables.Add("AgentReference", "Agent");
            DataTables.Add("AgentRelation", "Agent");
            return DataTables;
        }


        private void resetDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> Tables = new List<string>();
            Tables.Add("AgentContactInformation");
            Tables.Add("AgentExternalID");
            Tables.Add("AgentExternalDatabase");
            Tables.Add("AgentIdentifier");
            Tables.Add("AgentIdentifierType_Enum");
            Tables.Add("AgentImage");
            Tables.Add("AgentKeyword");
            Tables.Add("AgentReference");
            Tables.Add("AgentRelation");
            Tables.Add("AgentProject");
            Tables.Add("Agent");
            Tables.Add("ProjectUser");
            Tables.Add("ProjectProxy");
            Tables.Add("UserProxy");
            DiversityWorkbench.Archive.FormResetDatabase f = new DiversityWorkbench.Archive.FormResetDatabase(Tables);
            f.setHelp("Archive");
            f.ShowDialog();
        }

        private void restoreArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> TableList = new List<string>();
            // Project
            TableList.Add("ProjectProxy");
            // Method tables
            TableList.Add("Agent");
            TableList.Add("AgentExternalDatabase");
            TableList.Add("AgentExternalID");
            // Event tables
            TableList.Add("AgentIdentifierType_Enum");
            TableList.Add("AgentIdentifier");
            TableList.Add("AgentImage");
            TableList.Add("AgentKeyword");
            TableList.Add("AgentReference");
            TableList.Add("AgentRelation");
            TableList.Add("AgentProject");

            DiversityWorkbench.Archive.FormRestoreArchive f = new DiversityWorkbench.Archive.FormRestoreArchive(
                "Restore an archive of DiversityCollection", TableList);
            f.setHelp("Archive");
            f.ShowDialog();
            if (f.ProjectID() != null)
            {
                // Insert the current user
                string SQL = "INSERT INTO UserProxy (LoginName, CombinedNameCache, CurrentProjectID) " +
                    "VALUES (SUSER_SNAME(), SUSER_SNAME()," + f.ProjectID().ToString() + ")";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                SQL = "INSERT INTO UserProxy (LoginName, CombinedNameCache, CurrentProjectID) " +
                    "SELECT USER_NAME(), SUSER_SNAME(), " + f.ProjectID().ToString() + " WHERE SUSER_SNAME() != USER_NAME()";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                // Insert the projects for the user
                SQL = "INSERT INTO ProjectUser (LoginName, ProjectID, ReadOnly) " +
                    "select SUSER_SNAME(), " + f.ProjectID().ToString() + ", 0";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                SQL = "INSERT INTO ProjectUser (LoginName, ProjectID, ReadOnly) " +
                    "select USER_NAME(), " + f.ProjectID().ToString() + ", 0 WHERE SUSER_SNAME() != USER_NAME()";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                // Requery project list
                this.initQuery();
            }
        }

        private void createSchemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> Excludes = new List<string>();
            Excludes.Add("Application*");
            Excludes.Add("*_log");
            Excludes.Add("*_Enum");
            Excludes.Add("*Proxy");
            Excludes.Add("dtproperties");
            Excludes.Add("sysdiagrams");
            Excludes.Add("ProjectUser");
            DiversityWorkbench.Archive.FormCreateSchema f = new DiversityWorkbench.Archive.FormCreateSchema(Excludes);
            f.setHelp("Archive");
            f.ShowDialog();
        }

        #endregion

        #endregion

        #region Help

        private void setHelpMenu()
        {
            bool IsAdmin = false;
            if (DiversityWorkbench.Database.DatabaseRoles().Contains("DiversityWorkbenchAdministrator")
                || DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
            {
                IsAdmin = true;
            }
            this.feedbackToolStripMenuItem.Visible = true;

            // Markus 21.1.25: Alle feedback controls disabled
            this.editFeedbackToolStripMenuItem.Visible = false;// IsAdmin;

            string TableName = "Agent";
            bool OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
            // Markus 21.1.25: Alle feedback controls disabled
            this.feedbackHistoryToolStripMenuItem.Visible = false;// OK;

            this.clearErrorlogAsProgamStartToolStripMenuItem.Visible = !DiversityWorkbench.Settings.ErrorLogResetAtStartup;
            this.keepErrorlogToolStripMenuItem.Visible = DiversityWorkbench.Settings.ErrorLogResetAtStartup;
            this.clearErrorlogToolStripMenuItem.Visible = !DiversityWorkbench.Settings.ErrorLogResetAtStartup;

        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityAgents.Forms.FormAbout f = new Forms.FormAbout();
            f.ShowDialog();
            //this.About();

        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
                string urlManual = global::DiversityAgents.Properties.Settings.Default.ManualURL;
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
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this.userControlQueryList.QueryString(),
                this.ID.ToString());
        }

        private void feedbackHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.FeedbackHistory(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name);
        }

        private void editFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.EditFeedback();
        }

        private void downloadApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string urlManual = global::DiversityAgents.Properties.Settings.Default.DownloadApplicationURL;
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
                string urlManual = global::DiversityAgents.Properties.Settings.Default.InformationModelManualURL;
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

        private void clearErrorlogAsProgamStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ErrorLogResetAtStartup = true;
            this.setHelpMenu();
        }

        private void xGA1024X768ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Height = 768;
            this.Width = 1024;
        }

        #endregion

        #region Test
        //private void setSettingToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    DiversityWorkbench.Settings.DatabaseUser = "TestTax";
        //}

        //private void getSettingToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Settings.DatabaseUser);
        //}

        //private void setLocalSettingToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    DiversityTaxonNames.FormDiversityTaxonNamesSettings.Default.MaxItems = 44;
        //    DiversityTaxonNames.FormDiversityTaxonNamesSettings.Default.Save();
        //}

        //private void getLocalSettingToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    System.Windows.Forms.MessageBox.Show(DiversityTaxonNames.FormDiversityTaxonNamesSettings.Default.MaxItems.ToString());
        //}

        #endregion

        #region Administration

        private void setAdminMenu()
        {
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    // User
                    string TableName = "UserProxy";
                    bool OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                    this.userToolStripMenuItem.Visible = false;
                    this.loginsToolStripMenuItem.Visible = OK;
                    this.agentDisplayTypesToolStripMenuItem.Visible = OK;
                    this.defaultProjectsToolStripMenuItem.Visible = OK;
                    this.administrationToolStripMenuItem.Visible = OK;
                    this.jsonCacheToolStripMenuItem.Visible = false; // disable #11

                    // PW
                    if (DiversityWorkbench.Database.DatabaseRoles().Contains("DiversityWorkbenchUser")
                        || DiversityWorkbench.Settings.IsTrustedConnection)
                    {
                        this.changePasswordToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        this.changePasswordToolStripMenuItem.Visible = true;
                        this.administrationToolStripMenuItem.Visible = true;
                    }

                    // Projects
                    TableName = "ProjectProxy";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                    this.downloadProjectsToolStripMenuItem.Visible = OK;

                    // AgentExternalDatabase
                    TableName = "AgentExternalDatabase";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                    this.externalSourcesToolStripMenuItem.Visible = OK;

                    // IdentifierTypes
                    TableName = "AgentIdentifierType_Enum";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                    this.identifierTypesToolStripMenuItem.Visible = OK;

                    // DatabaseTools
                    TableName = "Agent_log";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                    this.databaseToolsToolStripMenuItem.Visible = OK;

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


                }
                else this.administrationToolStripMenuItem.Visible = false;
            }
            catch (Exception ex)
            {
                this.administrationToolStripMenuItem.Visible = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormUserAdministration f = new DiversityWorkbench.Forms.FormUserAdministration(DiversityWorkbench.Settings.ConnectionString, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());// ...Windows.Forms.Application.StartupPath + "\\DiversityAgents.chm");
            f.ShowDialog();
        }

        private void agentDisplayTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityAgents.Forms.FormAgentDisplayType f = new Forms.FormAgentDisplayType();
            f.ShowDialog();
        }

        private void customizeDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                DiversityAgents.Forms.FormCustomize f = new Forms.FormCustomize();
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowDialog();
                if (f.QueryDefaultsChanged())
                {
                    this.userControlQueryList.OptimizingAvailable(DiversityWorkbench.Settings.QueryOptimizedByDefault);
                    this.userControlQueryList.RememberSettingIsAvailable(true);
                    this.userControlQueryList.ManyOrderByColumns_SetSpacer();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void descriptorTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditDescriptorTypes();
        }

        private void identifierTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(DiversityAgents.Properties.Resources.Identifier, "AgentIdentifierType_Enum", "Types of identifier", "");
            f.ShowDialog();
        }

        private void jsonCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Api.FormApi formApi = new DiversityWorkbench.Api.FormApi("Agent", "AgentID");
                formApi.setProject("AgentProject", "ProjectID", "ProjectList");
                //formApi.setProject("TaxonNameProject");
                if (formApi != null && !formApi.IsDisposed)
                    formApi.ShowDialog();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        #region Projects
        private void defaultProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityAgents.Forms.FormDefaultProjects f = new Forms.FormDefaultProjects();
            f.ShowDialog();
        }

        private void defaultProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT ProjectID, Project FROM ProjectList ORDER BY Project";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count < 1)
                {
                    System.Windows.Forms.MessageBox.Show("There are no projects to choose. Please turn to your administrator");
                    return;
                }
                SQL = "SELECT MIN(ProjectList.Project) FROM UserInfo INNER JOIN ProjectList ON UserInfo.ProjectID = ProjectList.ProjectID";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                string Project = C.ExecuteScalar().ToString();
                con.Close();
                string Message = "Please select your default project from the list.\r\n\r\nYour current project is:     " + Project;
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "Default project", Message);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    int ProjectID;
                    if (int.TryParse(f.SelectedValue, out ProjectID))
                    {
                        SQL = "UPDATE UserInfo SET ProjectID = " + ProjectID.ToString();
                        C.CommandText = SQL;
                        con.Open();
                        C.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch { }
        }

        private void downloadProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dtProjectProxy = new DataTable("ProjectProxy");
                string SQL = "SELECT ProjectID, Project, ProjectURI " +
                    "FROM ProjectProxy";
                Microsoft.Data.SqlClient.SqlDataAdapter adProjectProxy = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                adProjectProxy.Fill(dtProjectProxy);
                DataSet DsProjectProxy = new DataSet();
                DsProjectProxy.Tables.Add(dtProjectProxy);
                Microsoft.Data.SqlClient.SqlCommandBuilder cb = new Microsoft.Data.SqlClient.SqlCommandBuilder(adProjectProxy);
                System.Windows.Forms.BindingSource ProjectBindingSource = new System.Windows.Forms.BindingSource(DsProjectProxy, "ProjectProxy");
                ((System.ComponentModel.ISupportInitialize)(ProjectBindingSource)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(ProjectBindingSource)).EndInit();
                DiversityWorkbench.Forms.FormLoginAdminGetProjects f = new DiversityWorkbench.Forms.FormLoginAdminGetProjects(dtProjectProxy, this.ServerConnection);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.AddedProjectRows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in f.AddedProjectRows)
                        {
                            System.Data.DataRow[] rr = dtProjectProxy.Select("ProjectID = " + R["ProjectID"].ToString());
                            if (rr.Length == 0)
                            {
                                System.Data.DataRow RN = dtProjectProxy.NewRow();
                                RN["ProjectID"] = R["ProjectID"];
                                RN["Project"] = R["Project"];
                                RN["ProjectURI"] = R["ProjectURI"];
                                dtProjectProxy.Rows.Add(RN);
                            }
                            else
                            {
                                if (R["ProjectID"].ToString() == rr[0]["Project"].ToString())
                                {
                                    rr[0]["ProjectURI"] = R["ProjectURI"];
                                }
                                else
                                {
                                    System.Windows.Forms.MessageBox.Show("The ProjectID " + R["ProjectID"].ToString() + " for the project " + R["Project"].ToString() + " is used for the project " + rr[0]["Project"].ToString());
                                }
                            }
                        }
                        ProjectBindingSource.EndEdit();
                        adProjectProxy.Update(dtProjectProxy);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region    Database

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
            f.setHelpProvider(this.helpProvider.HelpNamespace, "Database tools");
            f.ShowDialog();
        }

        private void loginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormLoginAdministration f = new DiversityWorkbench.Forms.FormLoginAdministration();
            f.ShowDialog();
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormDocumentation f = new DiversityWorkbench.Forms.FormDocumentation();
            f.Height = this.Height - 20;
            f.Width = this.Width - 20;
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
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

        #endregion

        private void externalSourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityAgents.Forms.FormExternalDatabase f = new Forms.FormExternalDatabase();
            f.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormPassword f = new DiversityWorkbench.Forms.FormPassword();
            f.ShowDialog();
        }

        private void resourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.setWorkbenchDirectory())
                DiversityAgents.Agent.CopyWorkbenchDirectory();
        }

        #endregion

        #region Update

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
                    if (this.DatabaseRoles.Contains("Administrator"))
                        IsAdmin = true;
                    if (User == "dbo" || IsAdmin)
                    {
                        string VerionOfDatabase = "";
                        UpdateDatabase = DiversityWorkbench.Database.CheckIfDatabaseNeedsAnUpdate(DiversityAgents.Properties.Settings.Default.DatabaseVersion, ref VerionOfDatabase);// this.UpdateCheckForDatabase();
                        this.updateDatabaseToolStripMenuItem.Visible = UpdateDatabase;
                    }
                    string Version = "";
                    UpdateClient = this.UpdateCheckForClient(ref Version);
                    this.updateClientToolStripMenuItem.Visible = (bool)UpdateClient;
                    if (UpdateDatabase)
                        this.updateToolStripMenuItem.Visible = true;
                    else this.updateToolStripMenuItem.Visible = false;
                }
                catch { }
            }
        }

        private void updateDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DiversityWorkbench.Database.UpdateDatabase(DiversityAgents.Properties.Settings.Default.DatabaseVersion,
                    "DiversityAgentsUpdate_",
                    DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), // ...Windows.Forms.Application.StartupPath + "\\DiversityAgents.chm",
                    Properties.Resources.ResourceManager))
                    this.setDatabase();
                this.setUpdateMenu();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void updateClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.UpdateClient();
        }

        private void UpdateClient()
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(DiversityWorkbench.Forms.FormFunctions.WikiDownloadPath("DiversityAgents"));
            f.ShowDialog();
        }



        /// <summary>
        /// Checks if a new version of the client is available
        /// This is documented in the database with the function dbo.VersionClient()
        /// </summary>
        /// <returns>true = New version is available. false = no new version</returns>
        private bool UpdateCheckForClient(ref string Version)
        {
            if (this._ClientUpToDate != null)
                return (bool)!this._ClientUpToDate;

            bool Update = false;
            int VersionMajor = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Major;
            int VersionMinor = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Minor;
            int VersionBuild = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Build;
            int VersionRevision = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Revision;
            //VersionRevision = 19; - nur fuer Test
            string CurrentClientVersion = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString();
            string ClientVersionAsInDB = "";
            Version = DiversityWorkbench.Forms.FormFunctions.LatestClientVersion;

            try
            {
                string[] VersionParts;
                if (Version.IndexOf('/') > -1)
                    VersionParts = Version.Split(new char[] { '/' });
                else
                    VersionParts = Version.Split(new char[] { '.' });

                int LastVersionMajor = int.Parse(VersionParts[0]);
                int LastVersionMinor = int.Parse(VersionParts[1]);
                int LastVersionBuild = int.Parse(VersionParts[2]);
                int LastVersionRevision = 0;
                if (VersionParts.Length > 3)
                    LastVersionRevision = int.Parse(VersionParts[3]);
                if (LastVersionMajor != VersionMajor
                    || LastVersionMinor != VersionMinor
                    || LastVersionBuild != VersionBuild
                    || LastVersionRevision != VersionRevision)
                {
                    if (LastVersionMajor > VersionMajor)
                        Update = true;
                    else if (LastVersionMajor == VersionMajor)
                    {
                        if (LastVersionMinor > VersionMinor)
                            Update = true;
                        else if (LastVersionMinor == VersionMinor)
                        {
                            if (LastVersionBuild > VersionBuild)
                                Update = true;
                            else if (LastVersionBuild == VersionBuild)
                            {
                                if (LastVersionRevision > VersionRevision)
                                    Update = true;
                                else Update = false;
                            }
                            else Update = false;
                        }
                        else Update = false;
                    }
                    else Update = false;
                }
            }
            catch { }

            if (Update)
            {
                this.updateClientToolStripMenuItem.ToolTipText = "The Version of your client is " + CurrentClientVersion + ". Please update to new client version " + ClientVersionAsInDB;
            }
            else
            {
                this.updateClientToolStripMenuItem.ToolTipText = "No updates for client";
            }
            this._ClientUpToDate = !Update;
            return Update;
        }



        #endregion

        #region ReadOnly

        private void setReadOnlyStateOfForm()
        {
            this._ReadOnly = this.IsReadOnly();
            // Agent
            this.tableLayoutPanelHeader.Enabled = !this._ReadOnly;
            this.tableLayoutPanelAgent.Enabled = !this._ReadOnly;
            this.tableLayoutPanelValidDate.Enabled = !this._ReadOnly;
            //this.buttonHeaderOpenReplacement.Enabled = !this._ReadOnly;

            // Contacts
            this.tableLayoutPanelContact.Enabled = !this._ReadOnly && this.dataSetAgent.AgentContactInformation.Rows.Count > 1;
            this.toolStripContact.Enabled = !this._ReadOnly;

            // Descriptor
            //this.tableLayoutPanelDescriptor.Enabled = !this._ReadOnly;
            this.toolStripDescriptor.Enabled = !this._ReadOnly;

            // Hierarchy
            this.tabControlHierarchy.Enabled = !this._ReadOnly;
            //this.tableLayoutPanelHierarchyAll.Enabled = !this._ReadOnly;
            //this.tableLayoutPanelHierarchyOther.Enabled = !this._ReadOnly;
            //this.toolStripHierarchy.Enabled = !this._ReadOnly;
            //this.toolStripHierarchyAll.Enabled = !this._ReadOnly;
            //this.toolStripHierarchyOther.Enabled = !this._ReadOnly;

            // References
            this.tableLayoutPanelReferences.Enabled = !this._ReadOnly && this.dataSetAgent.AgentReference.Rows.Count > 0;
            this.toolStripReferences.Enabled = !this._ReadOnly;

            // Relations
            this.tableLayoutPanelRelations.Enabled = !this._ReadOnly && this.dataSetAgent.AgentRelation.Rows.Count > 0;
            this.toolStripRelations.Enabled = !this._ReadOnly;

            // Identfier
            this.tableLayoutPanelIdentifier.Enabled = !this._ReadOnly && this.dataSetAgent.AgentIdentifier.Rows.Count > 0;
            this.toolStripIdentifier.Enabled = !this._ReadOnly;

            // Images
            this.tableLayoutPanelImages.Enabled = !this._ReadOnly;
            this.toolStripImages.Enabled = !this._ReadOnly;

            // Sources
            this.toolStripExternalDatasource.Enabled = !this._ReadOnly;
            this.dataGridViewExternalDataList.Enabled = !this._ReadOnly;

            // Keywords
            this.toolStripKeywords.Enabled = !this._ReadOnly;

            // Synonyms
            this.toolStripSynonymy.Enabled = !this._ReadOnly;

            // Projects
            this.toolStripProjects.Enabled = !this._ReadOnly;

            //this.splitContainerContactInformation.Enabled = !this._ReadOnly;
            //this.splitContainerProjects.Panel1.Enabled = !this._ReadOnly;
            //this.splitContainerData.Panel1.Enabled = !this._ReadOnly;
        }

        private bool? _ClientUpToDate = null;

        public bool ClientUpToDate()
        {
            if (this._ClientUpToDate != null)
                return (bool)this._ClientUpToDate;

            string Version = "";
            this._ClientUpToDate = !this.UpdateCheckForClient(ref Version);
            return (bool)this._ClientUpToDate;
        }


        private bool _ReadOnly = false;

        private DiversityWorkbench.Forms.FormFunctions.AvailabilityState _Availability = DiversityWorkbench.Forms.FormFunctions.AvailabilityState.Unknown;


        private bool IsReadOnly()
        {
            if (!this.ClientUpToDate())
                return true;
            if (!this.DataAvailable())
                return true;
            if (this._Availability == DiversityWorkbench.Forms.FormFunctions.AvailabilityState.Locked ||
                this._Availability == DiversityWorkbench.Forms.FormFunctions.AvailabilityState.ReadOnly)
                return true;
            this._ReadOnly = false;
            //if (this.DatasetIsBlocked())
            //    return true;
            return this._ReadOnly;
        }

        public bool DataAvailable()
        {
            //return true;

            this._Availability = DiversityWorkbench.Forms.FormFunctions.Availability(this.ID, "AgentID", "AgentID_FullAccess", "AgentID_ReadOnly", "AgentID_Locked"); //)// DiversityWorkbench.Forms.FormFunctions.AvailabilityState.Unknown)



            //{
            //    if (this.ID != -1)
            //        this._Availability = DiversityCollection.CollectionSpecimen.Availability(this.SpecimenID);
            //}
            //if (this.SpecimenID == -1 && this.userControlQueryList.ProjectIsSelected && this.userControlQueryList.listBoxQueryResult.SelectedIndex == -1)
            //    this._Availability = DiversityCollection.CollectionSpecimen.AvailabilityOfProject(this.userControlQueryList.ProjectID);

            //bool AllowAccess = true;
            //if (this._Availability != DiversityCollection.CollectionSpecimen.AvailabilityState.Available)
            //    AllowAccess = false;

            //if (AllowAccess)
            //    return true;
            //else
            //    return false;


            return true;
        }



        #endregion

        #endregion

        #region Query

        private void initQuery()
        {
            //this.setQueryControlEvents();
            try
            {
                this.userControlQueryList.OptimizingAllow(true);
                this.userControlQueryList.ManyOrderByColumns_Allow(true, "DiversityAgents", "FormDiversityAgents");
                this.userControlQueryList.RememberSettingIsAvailable(true);
                this.setSearchOptions();
                this.setQueryDisplayColumns();

                this.userControlQueryList.ManyOrderByColumns_Allow(true, DiversityWorkbench.Settings.ModuleName, "FormDiversityAgents", true);

                this.initQueryOptimizing();

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void initQueryOptimizing()
        {
            DiversityWorkbench.UserControls.UserControlQueryList.QueryMainTable = "Agent_Core";
            this.userControlQueryList.QueryMainTableLocal = "Agent_Core";
        }


        private void setQueryDisplayColumns()
        {
            this.userControlQueryList.QueryDisplayColumns = this.Agent.QueryDisplayColumns();
        }

        private void setSearchOptions()
        {
            this.userControlQueryList.setQueryConditions(this.Agent.QueryConditions(), DiversityAgents.Forms.FormDiversityAgentsSettings.Default.QueryConditionVisibility.ToString());
        }

        private void setQueryControlEvents()
        {
            try
            {
                // QueryList
                this.userControlQueryList.toolStripButtonConnection.Click += new System.EventHandler(this.databaseToolStripMenuItem_Click);
                this.userControlQueryList.toolStripButtonCopy.Click += new System.EventHandler(this.copyAgent);
                this.userControlQueryList.toolStripButtonDelete.Click += new System.EventHandler(this.deleteAgent);
                this.userControlQueryList.toolStripButtonNew.Click += new System.EventHandler(this.createNewAgent);
                this.userControlQueryList.toolStripButtonSave.Click += new System.EventHandler(this.saveAgent);
                this.userControlQueryList.toolStripButtonUndo.Click += new System.EventHandler(this.undoChangesInAgent);
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
            if (this.userControlQueryList.splitContainerMain.Orientation == Orientation.Vertical)
                this.splitContainerMain.SplitterDistance = (int)this.splitContainerMain.SplitterDistance / 2;
            else
                this.splitContainerMain.SplitterDistance = (int)this.splitContainerMain.SplitterDistance * 2;
            this.userControlQueryList.switchOrientation();
        }

        private void listBoxQueryResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.SuspendLayout();
                //if (this.userControlQueryList.ID > -1)
                {
                    this._AgentIDselectedInList = this.userControlQueryList.ID;
                    this.setAgent(this.userControlQueryList.ID);
                }
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

        private void copyAgent(object sender, System.EventArgs e)
        {
            string DisplayText = "copy of ";
            if (this.dataSetAgent.Agent.Rows[0]["AgentName"] == System.DBNull.Value)
                DisplayText += this.dataSetAgent.Agent.Rows[0][0].ToString();
            else
                DisplayText += this.dataSetAgent.Agent.Rows[0]["AgentName"].ToString();
            int OriginalID = (int)this.ID;
            try
            {
                int ID = this.CopyAgent(OriginalID, ref DisplayText);
                if (this.userControlQueryList.ManyOrderByColumns())
                {
                    string Display = this.userControlQueryList.ManyOrderByColumns_DisplayText(ID);
                    this.userControlQueryList.AddListItem(ID, Display);
                }
                else
                    this.userControlQueryList.AddListItem(ID, DisplayText);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Copy the data of a specimen into a new dataset
        /// </summary>
        /// <param name="OriginalID">The ID of the dataset that should be copied</param>
        /// <param name="AccessionNumber">the accessionnumber of the new specimen</param>
        /// <returns>the ID of the new specimen</returns>
        private int CopyAgent(int OriginalID, ref string AgentName)
        {
            string SQL = "execute dbo.procInsertAgentCopy NULL , " + OriginalID.ToString() + ", '" + AgentName + "'";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            int ID = 0;
            try
            {
                ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                SQL = "SELECT MAX(AgentID) FROM Agent";
                string Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (ID.ToString() != Test)
                    int.TryParse(Test, out ID);
                this.setAgent(ID);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            con.Close();
            return ID;
        }

        #endregion

        #region Delete

        private void deleteAgent(object sender, System.EventArgs e)
        {
            if (this.dataSetAgent.Agent.Rows.Count > 0)
            {
                int ID = (int)this.ID;
                string SQL = "SELECT COUNT(*) FROM Agent A WHERE A.AgentParentID = " + ID.ToString() + " OR A.SynonymToAgentID = " + ID.ToString();// "NameID = " + ID.ToString();
                int Count;
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out Count) && Count > 0)
                {
                    string Message = "There are " + Count.ToString() + " datasets that are dependent on the dataset you try to delete.\r\nPlease remove these dependencies first.";
                    if (Count == 1)
                        Message = "There is 1 dataset that is dependent on the dataset you try to delete.\r\nPlease remove this dependency first.";
                    System.Windows.Forms.MessageBox.Show(Message);
                    return;
                }
                string DisplayTextDel = this.dataSetAgent.Agent.Rows[0]["AgentName"].ToString();
                try
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you want to delete the agent\r\n\r\n" + DisplayTextDel, "Delete agent?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.userControlQueryList.RemoveSelectedListItem();
                        this.deleteAgent(ID);
                        this.dataSetAgent.Clear();
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
                if (this.userControlQueryList.listBoxQueryResult.Items.Count > 0)
                {
                    if (this.userControlQueryList.listBoxQueryResult.SelectedIndex > -1)
                        this.userControlQueryList.listBoxQueryResult.SelectedIndex = this.userControlQueryList.listBoxQueryResult.SelectedIndex;
                    //System.Windows.Forms.MessageBox.Show("Please select a taxon");
                }
            }
        }

        /// <summary>
        /// delete a name from the database
        /// </summary>
        /// <param name="NameID">the Primary key of table TaxonName corresponding to the name that should be deleted</param>
        private void deleteAgent(int ID)
        {
            try
            {
                string SQL = "DELETE FROM Agent WHERE AgentID = " + ID.ToString();
                Microsoft.Data.SqlClient.SqlConnection Conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(SQL, Conn);
                com.CommandType = System.Data.CommandType.Text;
                Conn.Open();
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

        private void createNewAgent(object sender, System.EventArgs e)
        {
            try
            {
                if (!this.userControlQueryList.ProjectIsSelected)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a project");
                    return;
                }
                if (System.Windows.Forms.MessageBox.Show("Do you want to create a new agent", "New agent?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    string GivenName = "";
                    string InheritedName = "";
                    string Type = "";
                    DiversityAgents.Forms.FormNewAgent f = new Forms.FormNewAgent();
                    //DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New agent", "Please give the name of the new agent", "");
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        GivenName = f.GivenName;
                        InheritedName = f.InheritedName;
                        Type = f.Type;
                        int ID = this.InsertNewAgent(ref GivenName, ref InheritedName, Type);
                        if (ID > -1)
                        {
                            this.setAgent(ID);
                            string Agent = InheritedName;
                            if (GivenName.Length > 0 && InheritedName.Length > 0)
                                Agent += ", ";
                            Agent += GivenName;
                            if (this.userControlQueryList.ManyOrderByColumns())
                            {
                                string Display = this.userControlQueryList.ManyOrderByColumns_DisplayText(ID);
                                this.userControlQueryList.AddListItem(ID, Display);
                            }
                            else
                                this.userControlQueryList.AddListItem(ID, Agent);
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
        /// inserting a new specimen into the table Agent
        /// </summary>
        /// <returns>the AgentID of the new specimen</returns>
        private int InsertNewAgent(ref string GivenName, ref string InheritedName)
        {
            try
            {
                int ID = -1;
                if (GivenName.Length == 0 && InheritedName.Length == 0)
                {
                    DiversityAgents.Forms.FormNewAgent f = new Forms.FormNewAgent();
                    //DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New agent", "Please give the name of the new agent", "");
                    f.ShowDialog();
                    if (f.DialogResult != DialogResult.OK) return ID;
                    GivenName = f.GivenName;
                    InheritedName = f.InheritedName;
                }
                string SQL = "";
                if (GivenName.Length == 0)
                    SQL = "INSERT INTO Agent (Version) VALUES (1) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                else
                    SQL = "INSERT INTO Agent (Version, GivenName, InheritedName) VALUES (1, '" + GivenName + "', '" + InheritedName + "') SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                int ProjectID = 0;
                //cmd.CommandText = "INSERT INTO AgentProject (AgentID, ProjectID) VALUES(" + ID.ToString() + ", " + ProjectID.ToString() + ")";
                //cmd.ExecuteNonQuery();
                try { ProjectID = this.userControlQueryList.ProjectID; }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                if (ProjectID != -1)
                {
                    cmd.CommandText = "INSERT INTO AgentProject (AgentID, ProjectID) VALUES(" + ID.ToString() + ", " + ProjectID.ToString() + ")";
                    try { cmd.ExecuteNonQuery(); }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                con.Close();
                return ID;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return -1;
            }
        }


        private int InsertNewAgent(ref string GivenName, ref string InheritedName, string Type)
        {
            try
            {
                int ID = -1;
                if (GivenName.Length == 0 && InheritedName.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No name was given");
                    return -1;
                }
                //string SQL = "SET NOCOUNT ON; DECLARE @T TABLE(ID int); INSERT INTO Agent (Version, GivenName, InheritedName, AgentType) OUTPUT INSERTED.AgentID INTO @T VALUES (1, '" + GivenName + "', '" + InheritedName + "', '" + Type + "'); SELECT MIN(ID) FROM @T;";// SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]"; ;
                string SQL = "SET NOCOUNT ON; INSERT INTO Agent (Version, GivenName, InheritedName, AgentType) VALUES (1, '" + GivenName.Replace("'", "''") + "', '" + InheritedName.Replace("'", "''") + "', '" + Type + "') SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                if (ID == 2) // Trigger returns extra line which interferes with expected result
                {
                    try
                    {
                        SQL = "SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                        cmd.CommandText = SQL;
                        int IDtest = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        if (IDtest != ID)
                        {
                            ID = IDtest;
                        }
                    }
                    catch (System.Exception ex)
                    { }
                }
                int ProjectID = 0;
                try { ProjectID = this.userControlQueryList.ProjectID; }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                if (ProjectID != -1)
                {
                    cmd.CommandText = "INSERT INTO AgentProject (AgentID, ProjectID) VALUES(" + ID.ToString() + ", " + ProjectID.ToString() + ")";
                    try { cmd.ExecuteNonQuery(); }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                con.Close();
                return ID;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return -1;
            }
        }

        #endregion

        #region Save
        private void saveAgent(object sender, System.EventArgs e)
        {
            this.updateAgent();
        }

        #endregion

        #region Undo
        private void undoChangesInAgent(object sender, System.EventArgs e)
        {
            try
            {
                int i = System.Int32.Parse(this.dataSetAgent.Agent.Rows[0]["AgentID"].ToString());
                this.dataSetAgent.Clear();
                this.setAgent(i);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Toolbar and Permissions

        private void setToolbarPermissions()
        {
            try
            {
                bool OK = true;
                string TableName = "";
                // Agent
                TableName = "Agent";

                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsAgentUpdate, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsAgentDelete, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);

                TableName = "AgentContactInformation";

                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsContactUpdate, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsContactInsert, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsContactDelete, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);

                TableName = "AgentKeyword";

                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsKeywordInsert, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsKeywordDelete, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);

                TableName = "AgentImage";

                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsImageInsert, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsImageDelete, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);

                TableName = "AgentProject";

                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsProjectInsert, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsProjectDelete, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);

                TableName = "AgentReference";

                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsReferenceInsert, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsReferenceDelete, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsReferenceUpdate, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);

                TableName = "AgentRelation";

                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsRelationInsert, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsRelationDelete, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsRelationUpdate, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);

                TableName = "AgentExternalID";

                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsExternalSourceInsert, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsExternalSourceDelete, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);
                this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsExternalSourceUpdate, TableName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Properties

        #region public

        public DiversityAgents.Agent Agent
        {
            get
            {
                if (this._Agent == null)
                {
                    if (this._ServerConnection == null)
                        this._Agent = new Agent(DiversityWorkbench.Settings.ServerConnection);
                    else
                        this._Agent = new Agent(this._ServerConnection);
                }
                return this._Agent;
            }
            set { this._Agent = value; }
        }

        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get
            {
                if (this._ServerConnection == null)
                {
                    if (this._Agent == null)
                        this._ServerConnection = DiversityWorkbench.Settings.ServerConnection;
                    else
                        this._ServerConnection = this._Agent.ServerConnection;
                }
                return this._ServerConnection;
            }
            set { _ServerConnection = value; }
        }

        public int ID
        {
            get
            {
                int ID = -1;
                if (this.dataSetAgent.Agent.Rows.Count > 0)
                    ID = System.Int32.Parse(this.dataSetAgent.Agent.Rows[0]["AgentID"].ToString());
                return ID;
            }
        }

        #endregion

        #region List for controls

        #region Agent

        private System.Collections.Generic.List<System.Object> ListControlsAgentUpdate
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.tableLayoutPanelAgent);
                L.Add(this.tableLayoutPanelHeader);
                L.Add(this.tableLayoutPanelValidDate);
                L.Add(this.toolStripButtonRemoveParent);
                L.Add(this.toolStripButtonSetParent);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsAgentDelete
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                //L.Add(this.buttonHeaderOpenReplacement);
                return L;
            }
        }

        #endregion

        #region Contact

        private System.Collections.Generic.List<System.Object> ListControlsContactUpdate
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.tableLayoutPanelContact);
                L.Add(this.toolStripButtonSetPreferred);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsContactDelete
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonContactDelete);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsContactInsert
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonContactNew);
                return L;
            }
        }

        #endregion

        #region Keyword

        private System.Collections.Generic.List<System.Object> ListControlsKeywordDelete
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonKeywordDelete);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsKeywordInsert
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonKeywordNew);
                return L;
            }
        }

        #endregion

        #region Project

        private System.Collections.Generic.List<System.Object> ListControlsProjectDelete
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonProjectDelete);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsProjectInsert
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonProjectNew);
                return L;
            }
        }

        #endregion

        #region Images

        private System.Collections.Generic.List<System.Object> ListControlsImageDelete
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonImageDelete);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsImageInsert
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonImageNew);
                return L;
            }
        }

        #endregion

        #region References

        private System.Collections.Generic.List<System.Object> ListControlsReferenceInsert
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonReferenceNew);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsReferenceUpdate
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.tableLayoutPanelReferences);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsReferenceDelete
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonReferencesDelete);
                return L;
            }
        }

        #endregion

        #region External source

        private System.Collections.Generic.List<System.Object> ListControlsExternalSourceInsert
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonExternalDatasourceAdd);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsExternalSourceUpdate
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.dataGridViewExternalDataList);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsExternalSourceDelete
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonExternalDatasourceDelete);
                return L;
            }
        }

        #endregion

        #region Relation

        private System.Collections.Generic.List<System.Object> ListControlsRelationInsert
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonRelationNew);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsRelationUpdate
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.tableLayoutPanelRelations);
                return L;
            }
        }

        private System.Collections.Generic.List<System.Object> ListControlsRelationDelete
        {
            get
            {
                System.Collections.Generic.List<System.Object> L = new List<Object>();
                L.Add(this.toolStripButtonRelationDelete);
                return L;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Agent

        private bool setAgent(int ID)
        {
            bool OK = false;
            this.labelHeaderAgent.Focus();
            int IDprevious = -1;
            if (this.dataSetAgent.Agent.Rows.Count > 0)
            {
                int.TryParse(this.dataSetAgent.Agent.Rows[0]["AgentID"].ToString(), out IDprevious);
            }
            if (this.dataSetAgent.Agent.Rows.Count > 0 && (this.dataSetAgent.HasChanges() || (IDprevious > -1 && ID != IDprevious)))
            {
                this.updateAgent();
            }
            this.SuspendLayout();
            this.fillAgent(ID);
            if (this.dataSetAgent.Agent.Rows.Count > 0)
                this.setSynonymyRemovalButtonVisibility(!this.dataSetAgent.Agent.Rows[0]["SynonymToAgentID"].Equals(System.DBNull.Value));
            this.setPersonControls();
            if (this.dataSetAgent.Agent.Rows.Count > 0 && this._AgentIDselectedInList == ID && this.panelNoAccess.Visible == false)
            {
                this.splitContainerData.Visible = true;
            }
            else if (this.dataSetAgent.Agent.Rows.Count == 1 && this.panelNoAccess.Visible == false)
                this.splitContainerData.Visible = true;
            else
            {
                this.splitContainerData.Visible = false;
            }
            if (this.splitContainerData.Visible)
                this.setReadOnlyStateOfForm();
            //this.getSpecimen();

            this.scanModules();

            //this.buildKeywordTree();

            this.buildDescriptorTree();
            this.setDescriptorControls();

            string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + ID.ToString();
            this.linkLabelURL.Text = URL;

            this.setLanguage();

            this.ResumeLayout();

            this.setHeaderImage();

            return OK;
        }

        private void fillAgent(int AgentID)
        {
            try
            {
                this.splitContainerMain.Panel2.BackColor = System.Drawing.SystemColors.Control;

                string SQL = "SELECT COUNT(*) FROM Agent WHERE AgentID = " + AgentID.ToString() +
                    " AND AgentID NOT IN (SELECT AgentID FROM AgentID_UserAvailable) ";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand COM = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                int i = 0;
                if (int.TryParse(COM.ExecuteScalar().ToString(), out i))
                {
                    if (i > 0)
                    {
                        this.panelNoAccess.Visible = true;
                        this.splitContainerData.Visible = false;
                        return;
                    }
                }
                if (AgentID == this._AgentIDselectedInList)
                {
                    this.panelNoAccess.Visible = false;
                    this.splitContainerData.Visible = true;
                }

                this.dataSetAgent.Clear();
                /// TODO: manual disabling of controls - unclear where these are set enabled
                this.tableLayoutPanelReferences.Enabled = false;
                this.tableLayoutPanelRelations.Enabled = false;


                string WhereClause = " WHERE AgentID = " + AgentID.ToString() +
                    " AND AgentID IN (SELECT AgentID FROM AgentID_UserAvailable)";

                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgent, this.dataSetAgent.Agent, DiversityAgents.Agent.SqlAgent + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                this.setDataWithholdingReason();
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentContact, this.dataSetAgent.AgentContactInformation, DiversityAgents.Agent.SqlAgentContactInformation + WhereClause + " ORDER BY DisplayOrder", DiversityWorkbench.Settings.ConnectionString);
                this.setDataWithholdingReasonContact();
                if (this.KeywordsIncludeType())
                {
                    string SqlKeywords = "SELECT AgentID, Keyword FROM AgentKeyword ";
                    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentKeyword, this.dataSetAgent.AgentKeyword, SqlKeywords + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                }
                else
                    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentKeyword, this.dataSetAgent.AgentKeyword, DiversityAgents.Agent.SqlAgentKeyword + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                if (!DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentImage, this.dataSetAgent.AgentImage, DiversityAgents.Agent.SqlAgentImage + WhereClause + " ORDER BY Sequence", DiversityWorkbench.Settings.ConnectionString))
                {
                    // previous version missing columns Description etc
                    string SqlImage = DiversityAgents.Agent.SqlAgentImage.Replace(", Description", "");
                    SqlImage = SqlImage.Replace(", Type", "");
                    SqlImage = SqlImage.Replace(", Sequence", "");
                    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentImage, this.dataSetAgent.AgentImage, SqlImage + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                    this.textBoxImageDescription.Enabled = false;
                }
                else
                    this.textBoxImageDescription.Enabled = true;
                this.setDataWithholdingReasonImage();
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentReference, this.dataSetAgent.AgentReference, DiversityAgents.Agent.SqlAgentReference + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentRelation, this.dataSetAgent.AgentRelation, DiversityAgents.Agent.SqlAgentRelation + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                this.setAgentRelationList();
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentProject, this.dataSetAgent.AgentProject, DiversityAgents.Agent.SqlAgentProject + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentExternalDataList, this.dataSetAgent.ExternalDataList, DiversityAgents.Agent.SqlExternalDataList + WhereClause, DiversityWorkbench.Settings.ConnectionString);

                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentDescriptor, this.dataSetAgent.AgentDescriptor, DiversityAgents.Agent.SqlAgentDescriptor + WhereClause, DiversityWorkbench.Settings.ConnectionString);

                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentIdentifier, this.dataSetAgent.AgentIdentifier, DiversityAgents.Agent.SqlAgentIdentifier + WhereClause, DiversityWorkbench.Settings.ConnectionString);

                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentHierarchyOther, this.dataSetAgent.AgentHierarchyOther, DiversityAgents.Agent.SqlAgentHierarchyOther + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                this.initHierarchyOther();

                if (this.dataSetAgent.Agent.Rows.Count > 0)
                {
                    if (this.dataSetAgent.Agent.Rows[0]["SynonymToAgentID"].Equals(System.DBNull.Value))
                    {
                        this.buttonHeaderOpenReplacement.Image = this.imageListReplacement.Images["Group.ico"];
                        this.toolTip.SetToolTip(this.buttonHeaderOpenReplacement, "Make the current agent a synonym or replace the current agent with an other one in this database");
                        this.labelSynonymisationType.Visible = false;
                        this.comboBoxSynonymisationType.Visible = false;
                        this.checkBoxIgnoreButKeepForReference.Visible = false;
                        this.labelHeaderAgentName.ForeColor = System.Drawing.SystemColors.ControlText;
                    }
                    else
                    {
                        this.buttonHeaderOpenReplacement.Image = this.imageListReplacement.Images["Find.ico"];
                        this.toolTip.SetToolTip(this.buttonHeaderOpenReplacement, "Change to the agent with which the current agent was replaced with or is a synonym to");
                        this.labelSynonymisationType.Visible = true;
                        this.comboBoxSynonymisationType.Visible = true;
                        this.checkBoxIgnoreButKeepForReference.Visible = true;
                        this.labelHeaderAgentName.ForeColor = System.Drawing.Color.Blue;
                        if (this.dataSetAgent.Agent.Rows[0]["SynonymisationType"].ToString().ToLower() == "replaced with")
                            this.splitContainerMain.Panel2.BackColor = System.Drawing.SystemColors.Info;
                    }

                    // get cumulative Address
                    if (this.dataSetAgent.Agent.Rows.Count > 0 && !this.dataSetAgent.Agent.Rows[0]["AgentParentID"].Equals(System.DBNull.Value))
                    {
                        string SqlAgentAddress = DiversityAgents.Agent.SqlAgentAddress + "(" + AgentID.ToString() + ")";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlAgentAddress, DiversityWorkbench.Settings.ConnectionString);
                        System.Data.DataTable dt = new DataTable();
                        ad.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            //DiversityAgents.Datasets.DataSetAgent.AgentContactInformationRow R = this.dataSetAgent.AgentContactInformation.NewAgentContactInformationRow();
                            DiversityAgents.Datasets.DataSetAgent.AgentContactInformationRow R = this.dataSetAgent.AgentContactInformation.NewAgentContactInformationRow();// this.dataSetAgent.AgentContactInformation.NewAgentContactInformationRow();
                            foreach (System.Data.DataColumn C in R.Table.Columns)
                            {
                                try
                                {
                                    if (dt.Columns.Contains(C.ColumnName))
                                        R[C.ColumnName] = dt.Rows[0][C.ColumnName];
                                }
                                catch (System.Exception ex)
                                { }
                            }
                            try
                            {
                                R["DisplayOrder"] = this._CumulativeContactDisplayOrder;
                                this.dataSetAgent.AgentContactInformation.Rows.Add(R);
                            }
                            catch { }
                        }
                    }

                    //this.buildHierarchy();
                    this._HierarchyLoopMessageMainShown = false;
                    this._HierarchyLoopMessageAllShown = false;
                    this._HierarchyLoopMessageOtherShown = false;
                    this._HierarchyLoopMessageSynonymShown = false;

                    this.HierarchyBuildTree(Hierarchy.Main);
                    this.HierarchyBuildTree(Hierarchy.Other);
                    this.HierarchyBuildTree(Hierarchy.All);
                    this.HierarchyBuildTree(Hierarchy.Synonym);

                    //this.buildSynonymy();

                    this.fillContactList();
                    this.fillProjectList();
                    this.FormFunctions.FillImageList(this.listBoxImages, this.imageList, this.imageListForm, this.dataSetAgent.AgentImage, "URI", this.pictureBoxImage);
                    //if (this.dataSetAgent.AgentImage.Rows.Count > 0)
                    //    this.tableLayoutPanelImages.Enabled = true;
                    //else
                    //    this.tableLayoutPanelImages.Enabled = false;
                    this.splitContainerImages.Panel2.Enabled = this.dataSetAgent.AgentImage.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool? _KeywordsIncludeType = null;
        private bool KeywordsIncludeType()
        {
            if (this._KeywordsIncludeType == null)
            {
                this._KeywordsIncludeType = false;
                string SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.COLUMN_NAME = 'KeywordType' AND C.TABLE_NAME = 'AgentKeyword'";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result == "1")
                    this._KeywordsIncludeType = true;
            }
            return (bool)this._KeywordsIncludeType;
        }

        private void updateAgent(bool Refill = false)
        {
            string SQL = "SELECT MIN(P.AgentNameDisplayType) AS DisplayType " +
                "FROM ProjectProxy AS P INNER JOIN " +
                "AgentProject AS A ON P.ProjectID = A.ProjectID " +
                "GROUP BY A.AgentID " +
                "HAVING A.AgentID = " + ID.ToString();
            string DefaultAgentNameDisplayType = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);

            SQL = "SELECT A.AgentName + ' = ' + N.AgentName FROM Agent A INNER JOIN dbo.AgentNames('" + DefaultAgentNameDisplayType + "') N ON N.AgentID = A.AgentID WHERE A.AgentID = " + this.ID.ToString();
            string previous = "";
            if (_BackLinkUpdateImplemented)
                previous = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //System.Collections.Generic.Dictionary<string, string> PreviousContent = new Dictionary<string, string>();
            //if (this.dataSetAgent.Agent.Rows.Count > 0)
            //{
            //    System.Data.DataRow Rprevious = this.dataSetAgent.Agent.Rows[0];
            //    PreviousContent.Add("AgentName", Rprevious["AgentName"].ToString());
            //    PreviousContent.Add("GivenName", Rprevious["GivenName"].ToString());
            //    PreviousContent.Add("GivenNamePostfix", Rprevious["GivenNamePostfix"].ToString());
            //    PreviousContent.Add("InheritedNamePrefix", Rprevious["InheritedNamePrefix"].ToString());
            //    PreviousContent.Add("InheritedName", Rprevious["InheritedName"].ToString());
            //    PreviousContent.Add("InheritedNamePostfix", Rprevious["InheritedNamePostfix"].ToString());
            //    PreviousContent.Add("AgentTitle", Rprevious["AgentTitle"].ToString());
            //}


            if (this.agentBindingSource != null && this.agentBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
            }
            try
            {
                if (!this.FormFunctions.updateTable(this.dataSetAgent, "Agent", this._SqlDataAdapterAgent, this.agentBindingSource))
                {
                    // Markus 27.7.23: Bugfix for DBConcurrencyException
                    try
                    {
                        Microsoft.Data.SqlClient.SqlCommandBuilder cb = new Microsoft.Data.SqlClient.SqlCommandBuilder(_SqlDataAdapterAgent);
                        cb.ConflictOption = ConflictOption.OverwriteChanges;
                        this._SqlDataAdapterAgent.Update(this.dataSetAgent, "Agent");
                    }
                    catch (System.Data.DBConcurrencyException ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Could not save changes. Please repeat editing");
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }


            System.Data.DataRow[] rr = this.dataSetAgent.AgentContactInformation.Select("DisplayOrder = " + this._CumulativeContactDisplayOrder.ToString());
            if (rr.Length > 0)
                rr[0].Delete();
            if (this.agentContactInformationBindingSource != null && this.agentContactInformationBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
            }
            this.FormFunctions.updateTable(this.dataSetAgent, "AgentContactInformation", this._SqlDataAdapterAgentContact, this.agentContactInformationBindingSource);

            if (this.agentKeywordBindingSource != null && this.agentKeywordBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentKeywordBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
            }
            this.FormFunctions.updateTable(this.dataSetAgent, "AgentKeyword", this._SqlDataAdapterAgentKeyword, this.agentKeywordBindingSource);

            if (this.agentImageBindingSource != null && this.agentImageBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentImageBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
            }
            this.FormFunctions.updateTable(this.dataSetAgent, "AgentImage", this._SqlDataAdapterAgentImage, this.agentImageBindingSource);

            this.FormFunctions.updateTable(this.dataSetAgent, "AgentProject", this._SqlDataAdapterAgentProject, this.agentKeywordBindingSource);

            if (this.agentRelationBindingSource != null && this.agentRelationBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentRelationBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
            }
            this.FormFunctions.updateTable(this.dataSetAgent, "AgentRelation", this._SqlDataAdapterAgentRelation, this.agentRelationBindingSource);

            if (this.agentReferenceBindingSource != null && this.agentReferenceBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentReferenceBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
            }
            this.FormFunctions.updateTable(this.dataSetAgent, "AgentReference", this._SqlDataAdapterAgentReference, this.agentReferenceBindingSource);

            if (this.agentHierarchyOtherBindingSource != null && this.agentHierarchyOtherBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentHierarchyOtherBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
            }
            this.FormFunctions.updateTable(this.dataSetAgent, "AgentHierarchyOther", this._SqlDataAdapterAgentHierarchyOther, this.agentHierarchyOtherBindingSource);

            if (this.agentIdentifierBindingSource != null && this.agentIdentifierBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentIdentifierBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
            }
            this.FormFunctions.updateTable(this.dataSetAgent, "AgentIdentifier", this._SqlDataAdapterAgentIdentifier, this.agentIdentifierBindingSource);

            if (this.agentDescriptorBindingSource != null && this.agentDescriptorBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentDescriptorBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
            }
            this.FormFunctions.updateTable(this.dataSetAgent, "AgentDescriptor", this._SqlDataAdapterAgentDescriptor, this.agentDescriptorBindingSource);

            string After = "";
            if (_BackLinkUpdateImplemented)
                After = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (previous != After)
            {
                DiversityWorkbench.WorkbenchUnit.BacklinkAddID((int)this.ID);
                this.userControlQueryList.BacklinkSetCount();
            }
            //System.Collections.Generic.Dictionary<string, string> CurrentContent = new Dictionary<string, string>();
            //System.Data.DataRow Rcurrent = this.dataSetAgent.Agent.Rows[0];
            //CurrentContent.Add("AgentName", Rcurrent["AgentName"].ToString());
            //CurrentContent.Add("GivenName", Rcurrent["GivenName"].ToString());
            //CurrentContent.Add("GivenNamePostfix", Rcurrent["GivenNamePostfix"].ToString());
            //CurrentContent.Add("InheritedNamePrefix", Rcurrent["InheritedNamePrefix"].ToString());
            //CurrentContent.Add("InheritedName", Rcurrent["InheritedName"].ToString());
            //CurrentContent.Add("InheritedNamePostfix", Rcurrent["InheritedNamePostfix"].ToString());
            //CurrentContent.Add("AgentTitle", Rcurrent["AgentTitle"].ToString());
            //if (CurrentContent["AgentName"] != PreviousContent["AgentName"] ||
            //    CurrentContent["GivenName"] != PreviousContent["GivenName"] ||
            //    CurrentContent["GivenNamePostfix"] != PreviousContent["GivenNamePostfix"] ||
            //    CurrentContent["InheritedNamePrefix"] != PreviousContent["InheritedNamePrefix"] ||
            //    CurrentContent["InheritedName"] != PreviousContent["InheritedName"] ||
            //    CurrentContent["InheritedNamePostfix"] != PreviousContent["InheritedNamePostfix"] ||
            //    CurrentContent["AgentTitle"] != PreviousContent["AgentTitle"])
            //{
            //    DiversityWorkbench.WorkbenchUnit.BacklinkAddID((int)this.ID);
            //    this.userControlQueryList.BacklinkSetCount();
            //}

            if (Refill)
                this.fillAgent(this.ID);
        }

        #endregion

        #region Header

        #region Synonymy
        private void buttonHeaderOpenReplacement_Click(object sender, EventArgs e)
        {
            if (this.agentBindingSource.Current == null)
                return;
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentBindingSource.Current;
                if (R["SynonymToAgentID"].Equals(System.DBNull.Value))
                {
                    DiversityWorkbench.ServerConnection S = new DiversityWorkbench.ServerConnection();
                    S.DatabaseName = DiversityWorkbench.Settings.DatabaseName;
                    S.DatabasePassword = DiversityWorkbench.Settings.Password;
                    S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
                    S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
                    S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                    S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
                    S.ModuleName = DiversityWorkbench.Settings.ModuleName;
                    string Title = "Search for replacing agent for " + R["AgentName"].ToString();
                    DiversityAgents.FormDiversityAgents f = new FormDiversityAgents(null, S, Title, this.Width, this.Height, this.ID, RequestType.ReplacingAgent);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK && f.ID > -1)
                    {
                        R["SynonymToAgentID"] = f.ID;
                        this.updateAgent(true);
                    }
                }
                else
                {
                    this.setAgent(int.Parse(R["SynonymToAgentID"].ToString()));
                }
                this.buildSynonymy();
            }
            catch { }
        }

        private void buttonHeaderRemoveSynonymy_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to remove the link to an agent that the current dataset is replaced with?", "Remove synonymy link", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentBindingSource.Current;
                R["SynonymToAgentID"] = System.DBNull.Value;
                this.updateAgent(true);
            }
        }

        private void setSynonymyRemovalButton(bool HasLoop = false)
        {
            if (HasLoop)
            {
                this.buttonHeaderOpenReplacement.Width = 18;
                this.buttonHeaderOpenReplacement.BackColor = System.Drawing.Color.Pink;
                this.buttonHeaderRemoveSynonymy.BackColor = System.Drawing.Color.Pink;
            }
            else
            {
                this.buttonHeaderOpenReplacement.Width = 34;
                this.buttonHeaderOpenReplacement.BackColor = System.Drawing.SystemColors.Control;
                this.buttonHeaderRemoveSynonymy.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        private void setSynonymyRemovalButtonVisibility(bool IsSynonym)
        {
            this.buttonHeaderRemoveSynonymy.Visible = IsSynonym;
        }

        #endregion

        private void comboBoxAgentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dataSetAgent.Agent.Rows.Count > 0)
            {
                this.setPersonControls();
            }
        }

        private void setPersonControls()
        {
            this.SuspendLayout();
            if (this.dataSetAgent.Agent.Rows.Count > 0)
            {
                bool ShowPersonControls = false;
                switch (this.dataSetAgent.Agent.Rows[0]["AgentType"].ToString().ToLower())
                {
                    case "person":
                        ShowPersonControls = true;
                        break;
                    default:
                        break;
                }
                this.labelGender.Visible = ShowPersonControls;
                this.comboBoxGender.Visible = ShowPersonControls;

                this.labelGivenName.Visible = ShowPersonControls;
                this.textBoxGivenName.Visible = ShowPersonControls;

                this.labelName.Visible = !ShowPersonControls;
                this.textBoxName.Visible = !ShowPersonControls;

                this.labelGivenNamePostfix.Visible = ShowPersonControls;
                this.textBoxGivenNamePostfix.Visible = ShowPersonControls;

                this.labelInheritedName.Visible = ShowPersonControls;
                this.textBoxInheritedName.Visible = ShowPersonControls;

                this.labelInheritedNamePostfix.Visible = ShowPersonControls;
                this.textBoxInheritedNamePostfix.Visible = ShowPersonControls;

                this.labelInheritedNamePrefix.Visible = ShowPersonControls;
                this.textBoxInheritedNamePrefix.Visible = ShowPersonControls;

                if (ShowPersonControls)
                {
                    this.labelPlaceOfBirth.Text = "Place of birth";
                    this.labelPlaceOfDeath.Text = "Place of death";
                    this.labelGivenName.Text = "First name";
                }
                else
                {
                    this.labelPlaceOfBirth.Text = "Place of foundation";
                    this.labelPlaceOfDeath.Text = "Place of termination";
                    this.labelGivenName.Text = "Name";
                }

                this.labelTitle.Visible = ShowPersonControls;
                this.comboBoxAgentTitle.Visible = ShowPersonControls;
            }
            this.ResumeLayout();
        }

        private void linkLabelURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.linkLabelURL.Text.Length > 0)
            {
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                //DiversityWorkbench.Forms.FormRemoteQuery f = new DiversityWorkbench.Forms.FormRemoteQuery(this.ID, A);
                DiversityWorkbench.Forms.FormRemoteQuery f = new DiversityWorkbench.Forms.FormRemoteQuery(A, this.ID, this.Height - 10);
                f.ShowDialog();
            }
        }
        #endregion

        #region Json

        private void initJsonCache()
        {
            try
            {
                bool ShowJsonCache = DiversityWorkbench.Api.JsonCache.DoesExist() && DiversityWorkbench.Api.JsonCache.ContainsColumnPublic();
                //#11
                ShowJsonCache = false;

                this.buttonShowJsonCache.Visible = ShowJsonCache;
                this.jsonCacheToolStripMenuItem.Visible = ShowJsonCache;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonShowJsonCache_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                string Json = "";
                if (e.Button == MouseButtons.Left)
                {
                    Json = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT Data FROM JsonCache WHERE ID = " + this.ID);
                }
                else if (e.Button == MouseButtons.Right && DiversityWorkbench.Api.JsonCache.ContainsColumnPublic())
                {
                    Json = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT [Public] FROM JsonCache WHERE ID = " + this.ID);
                }
                DiversityWorkbench.Api.JsonCache.ShowJson(Json);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

        }

        #endregion

        #region Hierarchies

        #region Hierarchy Main

        private void fillHierarchyMainTable()
        {
            string SQL = "SELECT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, " +
                "ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, " +
                "SynonymToAgentID, SynonymisationType, RevisionLevel FROM AgentHierarchy (" + this.ID.ToString() + ")";
            if (this._dtHierarchy == null) this._dtHierarchy = new DataTable();
            this._dtHierarchy.Clear();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(this._dtHierarchy);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buildHierarchy()
        {
            this.treeViewHierarchy.Nodes.Clear();
            this.fillHierarchyMainTable();
            this.HierarchyHasLoop_Fill(this._dtHierarchy);
            if (this.HierarchyHasLoop_Check())
            {
                if (!_HierarchyLoopMessageMainShown)
                {
                    string Message = "Hierarchy contains loop\r\n" + this.HierarchyHasLoopContent();
                    System.Windows.Forms.MessageBox.Show(Message);
                    _HierarchyLoopMessageMainShown = true;
                    return;
                }
            }
            this._CurrentNode = null;
            this._HierarchyLoopMessageMainShown = false;
            System.Data.DataRow[] rr = this._dtHierarchy.Select("AgentParentID IS NULL");
            // If failed due to loop
            if (rr.Length == 0 && this._dtHierarchy.Rows.Count > 0)
            {
                string TopID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo.AgentTopID(" + this.ID.ToString() + ") ");
                if (TopID.Length > 0)
                {
                    rr = this._dtHierarchy.Select("AgentParentID = " + TopID);
                }
            }
            if (rr.Length > 0)
            {
                System.Windows.Forms.TreeNode N = new TreeNode(rr[0]["AgentName"].ToString());
                N.Tag = rr[0][0].ToString();
                if (N.Tag.ToString() == this.ID.ToString())
                {
                    N.BackColor = System.Drawing.Color.Yellow;
                    this._CurrentNode = N;
                }
                this.treeViewHierarchy.Nodes.Add(N);
                this.addHierarchyChildNodes(N);
                this.treeViewHierarchy.ExpandAll();
            }
            if (this.dataSetAgent.Agent.Rows.Count > 0)
            {
                if (this.dataSetAgent.Agent.Rows[0]["AgentParentID"].Equals(System.DBNull.Value))
                {
                    this.toolStripButtonRemoveParent.Enabled = false;
                    this.toolStripButtonSetParent.Enabled = true;
                }
                else
                {
                    this.toolStripButtonRemoveParent.Enabled = true;
                    this.toolStripButtonSetParent.Enabled = false;
                }
            }
            if (_CurrentNode != null)
                _CurrentNode.EnsureVisible();
        }

        private bool addHierarchyChildNodes(System.Windows.Forms.TreeNode Node)
        {
            if (Node.Tag != null)
            {
                string ID = Node.Tag.ToString();
                System.Data.DataRow[] rr = this._dtHierarchy.Select("AgentParentID = " + ID);
                if (rr.Length > 0)
                {
                    for (int i = 0; i < rr.Length; i++)
                    {
                        int AgentID;
                        if (int.TryParse(rr[i]["AgentID"].ToString(), out AgentID))
                        {
                            if (_HierarchyNodeIDAdded.Contains(AgentID) && !_HierarchyLoopMessageMainShown)
                            {
                                string Message = "Hierarchy for\r\n" + rr[i]["AgentName"].ToString() + " [ID = " + AgentID.ToString() + "]\r\ncontains loop\r\n" + this.HierarchyHasLoopContent();
                                System.Windows.Forms.MessageBox.Show(Message);
                                _HierarchyLoopMessageMainShown = true;
                                return false;
                            }
                            _HierarchyNodeIDAdded.Add(AgentID);
                            //if (HierarchyHasLoop(int.Parse(ID), AgentID) && !_HierarchyLoopMessageShown)
                            //{
                            //    string Message = "Hierarchy for\r\n" + rr[i]["AgentName"].ToString() + " [ID = " + AgentID.ToString() + "]\r\ncontains loop\r\n" + this.HierarchyHasLoopContent();
                            //    System.Windows.Forms.MessageBox.Show(Message);
                            //    _HierarchyLoopMessageShown = true;
                            //    return false;
                            //}
                        }
                        System.Windows.Forms.TreeNode N = new TreeNode(rr[i]["AgentName"].ToString());
                        N.Tag = rr[i][0].ToString();
                        if (N.Tag.ToString() == this.ID.ToString())
                        {
                            N.BackColor = System.Drawing.Color.Yellow;
                            this._CurrentNode = N;
                        }
                        Node.Nodes.Add(N);
                        if (!this.addHierarchyChildNodes(N))
                            break;
                    }
                }
            }
            return true;
        }

        private System.Windows.Forms.TreeNode _CurrentNode = null;

        #region toolstrip

        private void toolStripButtonSetParent_Click(object sender, EventArgs e)
        {
            if (this.agentBindingSource.Current == null)
                return;
            System.Data.DataRowView R = (System.Data.DataRowView)this.agentBindingSource.Current;
            DiversityWorkbench.ServerConnection S = new DiversityWorkbench.ServerConnection();
            S.DatabaseName = DiversityWorkbench.Settings.DatabaseName;
            S.DatabasePassword = DiversityWorkbench.Settings.Password;
            S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
            S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
            S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
            S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
            S.ModuleName = DiversityWorkbench.Settings.ModuleName;
            string Title = "Search for superior agent for " + R["AgentName"].ToString();
            DiversityAgents.FormDiversityAgents f = new FormDiversityAgents(null, S, Title, this.Width, this.Height, this.ID, RequestType.SuperiorAgent);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                if (!R["AgentParentID"].Equals(System.DBNull.Value))
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you want to store the current hierarchy as other hierarchy?", "Keep old hierarchy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        DiversityAgents.Datasets.DataSetAgent.AgentHierarchyOtherRow Rother = this.dataSetAgent.AgentHierarchyOther.NewAgentHierarchyOtherRow();
                        Rother.AgentID = this.ID;
                        Rother.AgentParentID = int.Parse(R["AgentParentID"].ToString());
                        this.dataSetAgent.AgentHierarchyOther.Rows.Add(Rother);
                    }
                }
                R["AgentParentID"] = f.ID;
                this.updateAgent(true);
            }
        }

        private void toolStripButtonRemoveParent_Click(object sender, EventArgs e)
        {
            bool HierarchyHasLoop = this.HierarchyHasLoop_Check();
            System.Data.DataRowView RU = (System.Data.DataRowView)this.agentBindingSource.Current;
            if (RU["AgentParentID"].Equals(System.DBNull.Value)) return;
            if (HierarchyHasLoop)
            {
                System.Windows.Forms.MessageBox.Show("Current hierarchy will not be stored as other hierarchy to avoid loops", "Will not keep old hierarchy", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (System.Windows.Forms.MessageBox.Show("Do you want to store the current hierarchy as other hierarchy?", "Keep old hierarchy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    DiversityAgents.Datasets.DataSetAgent.AgentHierarchyOtherRow Rother = this.dataSetAgent.AgentHierarchyOther.NewAgentHierarchyOtherRow();
                    Rother.AgentID = this.ID;
                    Rother.AgentParentID = int.Parse(RU["AgentParentID"].ToString());
                    this.dataSetAgent.AgentHierarchyOther.Rows.Add(Rother);
                }
            }
            RU["AgentParentID"] = System.DBNull.Value;
            this.updateAgent(true);
            //this.buildHierarchy();
        }

        private void toolStripButtonFindAgent_Click(object sender, EventArgs e)
        {
            if (this.treeViewHierarchy.SelectedNode != null)
            {
                if (this.treeViewHierarchy.SelectedNode.Tag != null)
                {
                    int ID = int.Parse(this.treeViewHierarchy.SelectedNode.Tag.ToString());
                    this.setAgent(ID);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Please select an agent from the tree");

        }

        #endregion

        #endregion

        #region Hierarchy Other

        #region Building the tree

        private System.Data.DataTable fillHierarchyOtherTable(int ParentAgentID)
        {
            string SQL = "SELECT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, " +
                "ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, " +
                "SynonymToAgentID, SynonymisationType, RevisionLevel FROM AgentHierarchy (" + ParentAgentID.ToString() + ") H UNION " +
                "SELECT AgentID, " + ParentAgentID.ToString() + ", AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, " +
                "ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, " +
                "SynonymToAgentID, SynonymisationType, RevisionLevel FROM Agent A WHERE AgentID = " + this.ID.ToString();// + " AND NOT EXISTS (SELECT * FROM AgentHierarchy (" + ParentAgentID.ToString() + ") H WHERE A.AgentID = H.AgentID)";
            //System.Data.DataTable dtHierarchary = new DataTable();
            this._DtHierarchyOtherTree = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(this._DtHierarchyOtherTree);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return this._DtHierarchyOtherTree;
        }

        private void buildHierarchyOtherTree(int ParentAgentID)
        {
            try
            {
                this._CheckHierarchyLoops = new Dictionary<int, System.Collections.Generic.List<int>>();
                System.Data.DataTable dtHierarchy = this.fillHierarchyOtherTable(ParentAgentID);
                this.treeViewHierarchyOther.Nodes.Clear();
                System.Data.DataRow[] rr = dtHierarchy.Select("AgentParentID IS NULL");
                if (rr.Length == 0 && this._dtHierarchy.Rows.Count > 0)
                {
                    string TopID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo.AgentTopID(" + this.ID.ToString() + ") ");
                    if (TopID.Length > 0)
                    {
                        rr = dtHierarchy.Select("AgentParentID = " + TopID);
                    }
                }
                if (rr.Length > 0)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(rr[0]["AgentName"].ToString());
                    N.Tag = rr[0][0].ToString();
                    if (N.Tag.ToString() == this.ID.ToString())
                    {
                        N.BackColor = System.Drawing.Color.Yellow;
                        N.EnsureVisible();
                    }
                    this.treeViewHierarchyOther.Nodes.Add(N);
                    this.addHierarchyChildNodes(N, dtHierarchy);
                    this.treeViewHierarchyOther.ExpandAll();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void addHierarchyChildNodes(System.Windows.Forms.TreeNode Node, System.Data.DataTable dtHierarchy)
        {
            try
            {
                if (Node.Tag != null)
                {
                    string ID = Node.Tag.ToString();
                    System.Data.DataRow[] rr = dtHierarchy.Select("AgentParentID = " + ID);
                    if (rr.Length > 0)
                    {
                        for (int i = 0; i < rr.Length; i++)
                        {
                            int AgentID;
                            if (int.TryParse(rr[i]["AgentID"].ToString(), out AgentID))
                            {
                                //if (HierarchyHasLoop(int.Parse(ID), AgentID))
                                //{
                                //    System.Windows.Forms.MessageBox.Show("Hierarchy for\r\n" + rr[i]["AgentName"].ToString() + " [ID = " + AgentID.ToString()  + "]\r\ncontains loop");
                                //    return;
                                //}
                            }
                            System.Windows.Forms.TreeNode N = new TreeNode(rr[i]["AgentName"].ToString());
                            N.Tag = rr[i][0].ToString();
                            if (N.Tag.ToString() == this.ID.ToString()) N.BackColor = System.Drawing.Color.Yellow;
                            Node.Nodes.Add(N);
                            this.addHierarchyChildNodes(N, dtHierarchy);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        private System.Data.DataTable _DtHierarchyOtherTree;

        private System.Data.DataTable _DtHierarchyOtherList;

        #region events
        private bool initHierarchyOther()
        {
            if (this._DtHierarchyOtherList == null)
                this._DtHierarchyOtherList = new DataTable();
            this._DtHierarchyOtherList.Clear();
            string SQL = "SELECT H.HierarchyID, P.AgentName, H.AgentParentID, P.AgentID " +
                "FROM AgentHierarchyOther AS H INNER JOIN " +
                "Agent AS P ON H.AgentParentID = P.AgentID " +
                "WHERE H.AgentID = " + this.ID.ToString() +
                " ORDER BY P.AgentName";
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtHierarchyOtherList, ref Message);
            //this.HierarchyHasLoop_Fill(this._DtHierarchyOtherList);
            //if (HierarchyHasLoop_Check())
            //{
            //    Message = "Hierarchy contains loop:\r\n" + HierarchyHasLoopContent();
            //    System.Windows.Forms.MessageBox.Show(Message);
            //    return false;
            //}

            this.listBoxHierarchyOther.DataSource = this._DtHierarchyOtherList;
            this.listBoxHierarchyOther.DisplayMember = "AgentName";
            this.listBoxHierarchyOther.ValueMember = "HierarchyID";
            this.listBoxHierarchyOther_SelectedIndexChanged(null, null);

            //this.GetHierarchyAll();
            return true;
        }

        private void toolStripButtonHierarchyOtherAdd_Click(object sender, EventArgs e)
        {
            if (this.treeViewHierarchy.Nodes.Count == 1 && this.treeViewHierarchy.Nodes[0].Nodes.Count == 0) // this.dataSetAgent.Agent.Rows.Count == 1 && this.dataSetAgent.Agent.Rows[0]["AgentParentID"].Equals(System.DBNull.Value))
            {
                System.Windows.Forms.MessageBox.Show("The main hierarchy must be set first", "Main hierarchy missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int? AgentID = this.GetHierarchyParentAgentID(this.ID);
            if (AgentID != null && AgentID > -1)
            {
                // Check if no loop is created
                string SQL = "SELECT COUNT(*) FROM AgentHierarchy (" + ID.ToString() + ") WHERE AgentID = " + AgentID.ToString();
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result != "0")
                {
                    System.Windows.Forms.MessageBox.Show("This would create a circle relation", "Circle", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DiversityAgents.Datasets.DataSetAgent.AgentHierarchyOtherRow R = this.dataSetAgent.AgentHierarchyOther.NewAgentHierarchyOtherRow();
                R.AgentID = this.ID;
                R.AgentParentID = (int)AgentID;
                this.dataSetAgent.AgentHierarchyOther.Rows.Add(R);
                this.setAgent(this.ID);
            }
        }

        private int? GetHierarchyParentAgentID(int AgentID)
        {
            int ID = 0;
            if (this.agentBindingSource.Current == null)
                return null;
            System.Data.DataRowView R = (System.Data.DataRowView)this.agentBindingSource.Current;
            DiversityWorkbench.ServerConnection S = new DiversityWorkbench.ServerConnection();
            S.DatabaseName = DiversityWorkbench.Settings.DatabaseName;
            S.DatabasePassword = DiversityWorkbench.Settings.Password;
            S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
            S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
            S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
            S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
            S.ModuleName = DiversityWorkbench.Settings.ModuleName;
            string Title = "Search for superior agent for " + R["AgentName"].ToString();
            DiversityAgents.FormDiversityAgents f = new FormDiversityAgents(null, S, Title, this.Width, this.Height, this.ID, RequestType.SuperiorAgent);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                ID = f.ID;
                return ID;
            }
            else
                return null;
        }

        private void toolStripButtonHierarchyOtherDelete_Click(object sender, EventArgs e)
        {
            if (this.listBoxHierarchyOther.SelectedItem != null && this.agentHierarchyOtherBindingSource.Current != null)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure that you want to remove this entry", "Delete hierarchy", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.agentHierarchyOtherBindingSource.Current;
                    R.Row.Delete();
                    this.setAgent(this.ID);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
            }
        }

        private void toolStripButtonHierarchyOtherMakeMain_Click(object sender, EventArgs e)
        {
            if (this.listBoxHierarchyOther.SelectedItem != null && this.agentHierarchyOtherBindingSource.Current != null)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure that you want to make this entry the main entry for the hierarchy. The current main hierarchy will be transfer to the other hierarchies", "Set as main entry for hierarchy", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Data.DataRowView Ro = (System.Data.DataRowView)this.agentHierarchyOtherBindingSource.Current;
                    System.Data.DataRowView Ra = (System.Data.DataRowView)this.agentBindingSource.Current;
                    string SQL = "BEGIN TRY BEGIN TRANSACTION; ";
                    if (!Ra["AgentParentID"].Equals(System.DBNull.Value)) // the main hierarchy is set
                    {
                        string CurrentMainParentID = Ra["AgentParentID"].ToString();
                        System.Data.DataRow[] rr = this.dataSetAgent.AgentHierarchyOther.Select("AgentParentID = " + CurrentMainParentID);
                        if (rr.Length == 0) // the main hierarchy is missing in the other hierarchies
                        {
                            SQL += " INSERT INTO AgentHierarchyOther (AgentID, AgentParentID) " +
                                "VALUES(" + this.ID.ToString() + ", " + CurrentMainParentID + "); ";
                        }
                    }
                    if ((Ro["ValidFrom"].Equals(System.DBNull.Value) || Ro["ValidFrom"].ToString().Length == 0)
                        && (Ro["ValidUntil"].Equals(System.DBNull.Value) || Ro["ValidUntil"].ToString().Length == 0)
                        && (Ro["Notes"].Equals(System.DBNull.Value) || Ro["Notes"].ToString().Length == 0)) // the current entry in other hierarchies contains no information beyond the hierarchy itself
                    {
                        SQL += " DELETE FROM AgentHierarchyOther WHERE AgentID = " + this.ID.ToString() +
                            " AND HierarchyID = " + Ro["HierarchyID"].ToString() + "; ";
                    }
                    SQL += "UPDATE A SET AgentParentID = " + Ro["AgentParentID"].ToString() + " FROM Agent A WHERE A.AgentID = " + this.ID.ToString() + "; ";
                    SQL += "COMMIT TRANSACTION; END TRY BEGIN CATCH ROLLBACK TRANSACTION; END CATCH; ";
                    string Message = "";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                        this.setAgent(this.ID);
                    else if (Message.Length > 0)
                        System.Windows.Forms.MessageBox.Show("Transfer failed: " + Message);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
            }
        }

        private void listBoxHierarchyOther_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxHierarchyOther.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxHierarchyOther.SelectedItem;
                int AgentParentID;
                if (int.TryParse(R["AgentParentID"].ToString(), out AgentParentID))
                {
                    this.HierarchyBuildTree(Hierarchy.Other);
                    //this.buildHierarchyOtherTree(AgentParentID);
                }
                int HierarchyID;
                if (int.TryParse(R["HierarchyID"].ToString(), out HierarchyID))
                {
                    for (int i = 0; i < this.dataSetAgent.AgentHierarchyOther.Rows.Count; i++)
                    {
                        if (HierarchyID.ToString() == this.dataSetAgent.AgentHierarchyOther.Rows[i]["HierarchyID"].ToString())
                        {
                            this.agentHierarchyOtherBindingSource.Position = i;
                            if (this.dataSetAgent.AgentHierarchyOther.Rows[i]["ValidFrom"].Equals(System.DBNull.Value))
                                this.dateTimePickerHierarchyOtherValidFrom.CustomFormat = "--";
                            else
                                this.dateTimePickerHierarchyOtherValidFrom.CustomFormat = "yyyy-MM-dd";
                            if (this.dataSetAgent.AgentHierarchyOther.Rows[i]["ValidUntil"].Equals(System.DBNull.Value))
                                this.dateTimePickerHierarchyOtherValidUntil.CustomFormat = "--";
                            else
                                this.dateTimePickerHierarchyOtherValidUntil.CustomFormat = "yyyy-MM-dd";
                            break;
                        }
                    }
                }
                this.textBoxHierarchyOtherNotes.Enabled = true;
                this.dateTimePickerHierarchyOtherValidFrom.Enabled = true;
                this.dateTimePickerHierarchyOtherValidUntil.Enabled = true;
            }
            else
            {
                this.treeViewHierarchyOther.Nodes.Clear();
                this.dateTimePickerHierarchyOtherValidFrom.CustomFormat = "--";
                this.dateTimePickerHierarchyOtherValidUntil.CustomFormat = "--";
                this.textBoxHierarchyOtherNotes.Enabled = false;
                this.dateTimePickerHierarchyOtherValidFrom.Enabled = false;
                this.dateTimePickerHierarchyOtherValidUntil.Enabled = false;
            }
        }

        private void dateTimePickerHierarchyOtherValidFrom_CloseUp(object sender, EventArgs e)
        {
            if (this.agentHierarchyOtherBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentHierarchyOtherBindingSource.Current;
                R["ValidFrom"] = this.dateTimePickerHierarchyOtherValidFrom.Value;
                if (R["ValidFrom"].Equals(System.DBNull.Value))
                    this.dateTimePickerHierarchyOtherValidFrom.CustomFormat = "--";
                else
                    this.dateTimePickerHierarchyOtherValidFrom.CustomFormat = "yyyy-MM-dd";
            }
            else
                this.dateTimePickerHierarchyOtherValidFrom.CustomFormat = "--";
        }

        private void dateTimePickerHierarchyOtherValidUntil_CloseUp(object sender, EventArgs e)
        {
            if (this.agentHierarchyOtherBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentHierarchyOtherBindingSource.Current;
                R["ValidUntil"] = this.dateTimePickerHierarchyOtherValidUntil.Value;
                if (R["ValidUntil"].Equals(System.DBNull.Value))
                    this.dateTimePickerHierarchyOtherValidUntil.CustomFormat = "--";
                else
                    this.dateTimePickerHierarchyOtherValidUntil.CustomFormat = "yyyy-MM-dd";
            }
            else
                this.dateTimePickerHierarchyOtherValidUntil.CustomFormat = "--";
        }

        private void buttonHierarchyOtherSearch_Click(object sender, EventArgs e)
        {
            if (this.treeViewHierarchyOther.SelectedNode != null)
            {
                if (this.treeViewHierarchyOther.SelectedNode.Tag != null)
                {
                    //System.Data.DataRow R = (System.Data.DataRow)this.treeViewHierarchyOther.SelectedNode.Tag;
                    int AgentID;
                    if (int.TryParse(this.treeViewHierarchyOther.SelectedNode.Tag.ToString(), out AgentID))
                    {
                        this.setAgent(AgentID);
                    }
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
            }
        }

        #endregion

        #endregion

        #region Hierarchy All
        private void toolStripButtonHierarchyAllRefresh_Click(object sender, EventArgs e)
        {
            this.GetHierarchyAll();
        }

        private void toolStripButtonHierarchyAllFind_Click(object sender, EventArgs e)
        {
            if (this.treeViewHierarchyAll.SelectedNode != null)
            {
                if (this.treeViewHierarchyAll.SelectedNode.Tag != null)
                {
                    int ID = int.Parse(this.treeViewHierarchyAll.SelectedNode.Tag.ToString());
                    this.setAgent(ID);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Please select an agent from the tree");
        }

        private System.Data.DataTable _DtHierarchyAll;

        private bool GetHierarchyAll()
        {
            bool OK = true;
            try
            {
                if (FillHierarchyAllTable())
                {
                    this.BuildHierarchyAllTree();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool FillHierarchyAllTable()
        {
            bool OK = true;
            try
            {
                if (this._DtHierarchyAll == null)
                    this._DtHierarchyAll = new DataTable();
                this._DtHierarchyAll.Clear();
                string SQL = "SELECT AgentID, AgentParentID, AgentName FROM dbo.AgentHierarchyAllSuperior(" + this.ID.ToString() + ")";
                OK = DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtHierarchyAll);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        #region Tree
        private bool BuildHierarchyAllTree()
        {
            bool OK = true;
            this.treeViewHierarchyAll.Nodes.Clear();
            try
            {
                this._CheckHierarchyLoops = new Dictionary<int, System.Collections.Generic.List<int>>();
                System.Data.DataRow[] RR = this._DtHierarchyAll.Select("AgentParentID IS NULL", "AgentName");
                foreach (System.Data.DataRow R in RR)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R["AgentName"].ToString());
                    int AgentID;
                    if (int.TryParse(R["AgentID"].ToString(), out AgentID) && AgentID == this.ID)
                        N.BackColor = System.Drawing.Color.Yellow;
                    N.Tag = AgentID;
                    this.treeViewHierarchyAll.Nodes.Add(N);
                    this.GetHierarchyAllChildren(N);
                }
                this.treeViewHierarchyAll.ExpandAll();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool GetHierarchyAllChildren(System.Windows.Forms.TreeNode ParentNode)
        {
            bool OK = true;
            try
            {
                System.Data.DataRow[] RR = this._DtHierarchyAll.Select("AgentParentID = " + ParentNode.Tag.ToString(), "AgentName");
                foreach (System.Data.DataRow R in RR)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R["AgentName"].ToString());
                    int AgentID;
                    if (int.TryParse(R["AgentID"].ToString(), out AgentID))
                    {
                        //if (HierarchyHasLoop(int.Parse(ParentNode.Tag.ToString()), AgentID))
                        //{
                        //    string Message = "Hierarchy for\r\n" + R["AgentName"].ToString() + " [ID = " + AgentID.ToString() + "]\r\ncontains loop:\r\n" + HierarchyHasLoopContent();
                        //    System.Windows.Forms.MessageBox.Show(Message);
                        //    break;
                        //}
                        if (AgentID == this.ID)
                            N.BackColor = System.Drawing.Color.Yellow;
                    }
                    N.Tag = AgentID;
                    ParentNode.Nodes.Add(N);
                    this.GetHierarchyAllChildren(N);
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

        #endregion

        #region Common functions

        private enum Hierarchy { Main, Other, All, Synonym }

        private bool _HierarchyLoopMessageMainShown = false;
        private bool _HierarchyLoopMessageOtherShown = false;
        private bool _HierarchyLoopMessageAllShown = false;
        private bool _HierarchyLoopMessageSynonymShown = false;

        private void HierarchyLoopMessageIsShown(bool IsShown)
        {
            _HierarchyLoopMessageMainShown = IsShown;
            _HierarchyLoopMessageOtherShown = IsShown;
            _HierarchyLoopMessageAllShown = IsShown;
            _HierarchyLoopMessageSynonymShown = IsShown;
        }

        private bool HierarchyLoopMessageIsShown()
        {
            return (_HierarchyLoopMessageMainShown || _HierarchyLoopMessageOtherShown || _HierarchyLoopMessageAllShown || _HierarchyLoopMessageSynonymShown);
        }



        private void HierarchyBuildTree(Hierarchy hierarchy)
        {
            try
            {
                // prepare the tree view
                System.Windows.Forms.TreeView treeView;
                string HierarchyParentColumn = "AgentParentID";
                switch (hierarchy)
                {
                    case Hierarchy.Synonym:
                        treeView = this.treeViewSynonymy;
                        HierarchyParentColumn = "SynonymToAgentID";
                        this._HierarchyLoopMessageSynonymShown = false;
                        break;
                    case Hierarchy.Other:
                        treeView = this.treeViewHierarchyOther;
                        this._HierarchyLoopMessageOtherShown = false;
                        break;
                    case Hierarchy.All:
                        treeView = this.treeViewHierarchyAll;
                        this._HierarchyLoopMessageAllShown = false;
                        break;
                    default:
                        treeView = this.treeViewHierarchy;
                        this._HierarchyLoopMessageMainShown = false;
                        break;
                }
                treeView.Nodes.Clear();
                //HierarchyClearTree(hierarchy);

                // getting the data
                System.Data.DataTable dtHierarchy = HierarchyGetTable(hierarchy);

                // prepare check
                bool RestartForLoopCheck = (hierarchy == Hierarchy.Main);
                this.HierarchyHasLoop_Fill(dtHierarchy, RestartForLoopCheck);

                System.Windows.Forms.TreeNode currentNode = null;
                if (dtHierarchy.Rows.Count > 0)
                {
                    System.Data.DataRow[] RR = dtHierarchy.Select(HierarchyParentColumn + " IS NULL", "AgentName");
                    if (RR.Length == 0)
                    {
                        string TopID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo.AgentTopID(" + this.ID.ToString() + ") ");
                        if (TopID.Length > 0)
                        {
                            RR = dtHierarchy.Select("AgentID = " + TopID);
                        }
                    }
                    foreach (System.Data.DataRow R in RR)
                    {
                        System.Windows.Forms.TreeNode N = new TreeNode(R["AgentName"].ToString());
                        int AgentID;
                        if (int.TryParse(R["AgentID"].ToString(), out AgentID) && AgentID == this.ID)
                            N.BackColor = System.Drawing.Color.Yellow;
                        N.Tag = AgentID;
                        if (N.Tag.ToString() == this.ID.ToString())
                        {
                            currentNode = N;
                        }
                        treeView.Nodes.Add(N);
                        bool HasLoop = !this.HierarchyAddChildNodes(hierarchy, N, dtHierarchy, ref currentNode);
                        if (hierarchy == Hierarchy.Synonym)
                        {
                            this.setSynonymyRemovalButton(HasLoop);
                        }
                    }
                    treeView.ExpandAll();
                    if (currentNode != null)
                    {
                        currentNode.EnsureVisible();
                        currentNode.BackColor = System.Drawing.Color.Yellow;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool HierarchyAddChildNodes(Hierarchy hierarchy, System.Windows.Forms.TreeNode Node, System.Data.DataTable dtHierarchy, ref System.Windows.Forms.TreeNode currentNode)
        {
            bool OK = true;
            string HierarchyParentColumn = "AgentParentID";
            switch (hierarchy)
            {
                case Hierarchy.Synonym:
                    HierarchyParentColumn = "SynonymToAgentID";
                    break;
            }
            try
            {
                if (Node.Tag != null)
                {
                    string ID = Node.Tag.ToString();
                    System.Data.DataRow[] rr = dtHierarchy.Select(HierarchyParentColumn + " = " + ID);
                    if (rr.Length > 0)
                    {
                        for (int i = 0; i < rr.Length; i++)
                        {
                            int AgentID;
                            if (int.TryParse(rr[i]["AgentID"].ToString(), out AgentID))
                            {
                                if (_HierarchyNodeIDAdded.Contains(AgentID))
                                {
                                    switch (hierarchy)
                                    {
                                        case Hierarchy.All:
                                            if (!HierarchyLoopMessageIsShown())
                                            {
                                                string Message = hierarchy.ToString() + " hierarchy for\r\n" + rr[i]["AgentName"].ToString() + " [ID = " + AgentID.ToString() + "]\r\ncontains loop\r\n" + this.HierarchyHasLoopContent();
                                                System.Windows.Forms.MessageBox.Show(Message);
                                            }
                                            this._HierarchyLoopMessageAllShown = true;
                                            break;
                                        case Hierarchy.Other:
                                            if (!HierarchyLoopMessageIsShown())
                                            {
                                                string Message = hierarchy.ToString() + " hierarchy for\r\n" + rr[i]["AgentName"].ToString() + " [ID = " + AgentID.ToString() + "]\r\ncontains loop\r\n" + this.HierarchyHasLoopContent();
                                                System.Windows.Forms.MessageBox.Show(Message);
                                            }
                                            this._HierarchyLoopMessageOtherShown = true;
                                            break;
                                        case Hierarchy.Synonym:
                                            if (!HierarchyLoopMessageIsShown())
                                            {
                                                string Message = hierarchy.ToString() + " hierarchy for\r\n" + rr[i]["AgentName"].ToString() + " [ID = " + AgentID.ToString() + "]\r\ncontains loop\r\n" + this.HierarchyHasLoopContent();
                                                System.Windows.Forms.MessageBox.Show(Message);
                                                OK = false;
                                            }
                                            this._HierarchyLoopMessageSynonymShown = true;
                                            break;
                                        default:
                                            if (!HierarchyLoopMessageIsShown())
                                            {
                                                string Message = hierarchy.ToString() + " hierarchy for\r\n" + rr[i]["AgentName"].ToString() + " [ID = " + AgentID.ToString() + "]\r\ncontains loop\r\n" + this.HierarchyHasLoopContent();
                                                System.Windows.Forms.MessageBox.Show(Message, "Loop in hierarchy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            _HierarchyLoopMessageMainShown = true;
                                            //return false;
                                            break;
                                    }
                                    OK = false;
                                    return false;
                                }
                                _HierarchyNodeIDAdded.Add(AgentID);
                            }
                            System.Windows.Forms.TreeNode N = new TreeNode(rr[i]["AgentName"].ToString());
                            N.Tag = rr[i][0].ToString();
                            if (N.Tag.ToString() == this.ID.ToString()) N.BackColor = System.Drawing.Color.Yellow;
                            Node.Nodes.Add(N);
                            bool HasLoop = !this.HierarchyAddChildNodes(hierarchy, N, dtHierarchy, ref currentNode);
                            if (HasLoop && hierarchy == Hierarchy.Synonym)
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


        //private void HierarchyClearTree(Hierarchy hierarchy)
        //{
        //    switch (hierarchy)
        //    {
        //        case Hierarchy.Main:
        //            this.treeViewHierarchy.Nodes.Clear();
        //            break;
        //        case Hierarchy.Other:
        //            this.treeViewHierarchyOther.Nodes.Clear();
        //            break;
        //        case Hierarchy.All:
        //            this.treeViewHierarchyAll.Nodes.Clear();
        //            break;
        //    }
        //}

        private System.Data.DataTable HierarchyGetTable(Hierarchy hierarchy)
        {
            System.Data.DataTable dt = new DataTable();
            switch (hierarchy)
            {
                case Hierarchy.Main:
                    this.fillHierarchyMainTable();
                    return this._dtHierarchy;
                case Hierarchy.Other:
                    if (this.listBoxHierarchyOther.SelectedItem != null)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxHierarchyOther.SelectedItem;
                        int AgentParentID;
                        if (int.TryParse(R["AgentParentID"].ToString(), out AgentParentID))
                        {
                            dt = this.fillHierarchyOtherTable(AgentParentID);
                        }
                    }
                    else
                    {
                        dt = this.fillHierarchyOtherTable(-1);
                    }
                    break;
                case Hierarchy.All:
                    if (FillHierarchyAllTable())
                        return this._DtHierarchyAll;
                    break;
                case Hierarchy.Synonym:
                    this.fillSynonymyTable();
                    return this._dtSynonymy;
                    break;
            }
            return dt;
        }



        #endregion

        #region Checking for loops in hierarchy

        private System.Collections.Generic.List<int> _HierarchyNodeIDAdded = new List<int>();

        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> _CheckHierarchyLoops;

        #region Message
        private string HierarchyHasLoopContent()
        {
            string Message = "";
            if (_CheckHierarchyLoops != null && _CheckHierarchyLoops.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.List<int>> KV in _CheckHierarchyLoops)
                {
                    Message += "\r\n" + this.HierarchyHasLoopNode(KV.Key);
                    foreach (int i in KV.Value)
                    {
                        Message += "\r\n->\t" + HierarchyHasLoopNode(i);
                    }
                }
            }
            return Message;
        }

        private string HierarchyHasLoopNode(int ID)
        {
            if (ID < 0) return "";

            string SQL = "SELECT AgentName FROM Agent WHERE AgentID = " + ID.ToString();
            string Node = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL) + "  [ID: " + ID.ToString() + "]";
            return Node;
        }

        #endregion

        #region Filling the dictionary

        private void HierarchyHasLoop_Fill(System.Data.DataTable dataTable, bool Restart = true)
        {
            try
            {
                _HierarchyNodeIDAdded = new List<int>();
                if (Restart)
                {
                    _CheckHierarchyLoops = new Dictionary<int, List<int>>();
                }
                foreach (System.Data.DataRow R in dataTable.Rows)
                {
                    if (!R["AgentParentID"].Equals(System.DBNull.Value))
                    {
                        int P;
                        if (int.TryParse(R["AgentParentID"].ToString(), out P))
                        {
                            int A;
                            if (int.TryParse(R["AgentID"].ToString(), out A))
                            {
                                if (!_CheckHierarchyLoops.ContainsKey(P))
                                {
                                    List<int> L = new List<int>();
                                    L.Add(A);
                                    _CheckHierarchyLoops.Add(P, L);
                                }
                                else if (!_CheckHierarchyLoops[P].Contains(A))
                                {
                                    _CheckHierarchyLoops[P].Add(A);
                                }
                                if (_CheckHierarchyLoops.ContainsKey(A))
                                {
                                    try
                                    {
                                        int[] aa = new int[_CheckHierarchyLoops[A].Count];
                                        _CheckHierarchyLoops[A].CopyTo(aa);
                                        for (int i = 0; i < aa.Length; i++)
                                        {
                                            _CheckHierarchyLoops[P].Add(aa[i]);
                                        }
                                    }
                                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                                }
                            }
                        }
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<int, List<int>> KV in _CheckHierarchyLoops)
                {
                    this.HierarchyHasLoop_Fill(KV.Key);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void HierarchyHasLoop_Fill(int ParentID)
        {
            try
            {
                //if (HierarchyHasLoop_Check())
                //    return true;
                if (_CheckHierarchyLoops.ContainsKey(ParentID))
                {
                    System.Collections.Generic.List<int> ToAdd = new List<int>();
                    foreach (int iChild in _CheckHierarchyLoops[ParentID])
                    {
                        //if (HierarchyHasLoop_Fill(iChild))
                        //    return true;
                        if (_CheckHierarchyLoops.ContainsKey(iChild))
                        {
                            foreach (int iiChild in _CheckHierarchyLoops[iChild])
                            {
                                if (!_CheckHierarchyLoops[ParentID].Contains(iiChild))
                                    ToAdd.Add(iiChild);
                            }
                        }
                    }
                    foreach (int iAdd in ToAdd)
                    {
                        if (!_CheckHierarchyLoops[ParentID].Contains(iAdd))
                            _CheckHierarchyLoops[ParentID].Add(iAdd);
                        //if (HierarchyHasLoop_Check())
                        //    return true;
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            //return false;
        }

        #endregion

        private void HierarchyHasLoop(int ParentID, int ChildID)
        {
            //if (_CheckHierarchyLoops == null)
            //    return false;
            if (!_CheckHierarchyLoops.ContainsKey(ParentID))
            {
                System.Collections.Generic.List<int> vs = new List<int>();
                vs.Add(ChildID);
                _CheckHierarchyLoops.Add(ParentID, vs);
            }
            if (!_CheckHierarchyLoops[ParentID].Contains(ChildID))
                _CheckHierarchyLoops[ParentID].Add(ChildID);
            //foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.List<int>> KV in _CheckHierarchyLoops)
            //{
            //    if (HierarchyHasLoop_Fill(KV.Key))
            //        return true;
            //}
            //return HierarchyHasLoop_Check();
        }


        #region Checking for Loops
        private bool HierarchyHasLoop_Check()
        {
            foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.List<int>> KV in _CheckHierarchyLoops)
            {
                if (KV.Value.Contains(KV.Key))
                    return true;
            }
            return false;
        }

        #endregion

        //private bool HierarchyHasLoop(int ParentID, int ChildID)
        //{
        //    if (_CheckHierarchyLoops == null)
        //        return false;
        //    if (!_CheckHierarchyLoops.ContainsKey(ParentID))
        //    {
        //        System.Collections.Generic.List<int> vs = new List<int>();
        //        vs.Add(ChildID);
        //        _CheckHierarchyLoops.Add(ParentID, vs);
        //    }
        //    if (!_CheckHierarchyLoops[ParentID].Contains(ChildID))
        //        _CheckHierarchyLoops[ParentID].Add(ChildID);
        //    foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.List<int>> KV in _CheckHierarchyLoops)
        //    {
        //        if (HierarchyHasLoop_Fill(KV.Key))
        //            return true;
        //    }
        //    return HierarchyHasLoop_Check();
        //}

        //private bool HierarchyHasLoop_Fill(int ParentID)
        //{
        //    if (HierarchyHasLoop_Check())
        //        return true;
        //    if (_CheckHierarchyLoops.ContainsKey(ParentID))
        //    {
        //        System.Collections.Generic.List<int> ToAdd = new List<int>();
        //        foreach (int iChild in _CheckHierarchyLoops[ParentID])
        //        {
        //            if (HierarchyHasLoop_Fill(iChild))
        //                return true;
        //            if (_CheckHierarchyLoops.ContainsKey(iChild))
        //            {
        //                foreach (int iiChild in _CheckHierarchyLoops[iChild])
        //                {
        //                    if (!_CheckHierarchyLoops[ParentID].Contains(iiChild))
        //                        ToAdd.Add(iiChild);
        //                }
        //            }
        //        }
        //        foreach (int iAdd in ToAdd)
        //        {
        //            if (!_CheckHierarchyLoops[ParentID].Contains(iAdd))
        //                _CheckHierarchyLoops[ParentID].Add(iAdd);
        //            if (HierarchyHasLoop_Check())
        //                return true;
        //        }
        //    }
        //    return false;
        //}

        #endregion

        #endregion

        #region Contacts

        private void fillContactList()
        {
            try
            {
                if (this._dtContactList != null)
                    this._dtContactList.Clear();
                this._dtContactList = this.dataSetAgent.AgentContactInformation.Copy();
                foreach (System.Data.DataRow R in this._dtContactList.Rows)
                {
                    if (R.RowState == DataRowState.Deleted)
                        continue;
                    if (R["Address"].ToString().Trim().Length > 0)
                    {
                        string Address = "   " + R["Address"].ToString().Trim() + ".  ";
                        if (Address.Length > 255)
                            Address = Address.Substring(0, 255);
                        R["Address"] = Address;
                    }
                    if (R["DisplayOrder"].ToString() == this._CumulativeContactDisplayOrder.ToString())
                    {
                        string Address = "CUMULATIVE ADDRESS:   " + R["Address"].ToString();
                        if (Address.Length > 255)
                            Address = Address.Substring(0, 255);
                        R["Address"] = Address;
                    }
                    else
                    {
                        string Address = R["DisplayOrder"].ToString() + ":   " + R["Address"].ToString();
                        if (Address.Length > 255)
                            Address = Address.Substring(0, 255);
                        R["Address"] = Address;
                    }
                    //if (R["Address"].ToString().Length == 0)
                    //{

                    string Add = R["Address"].ToString().Trim() + "   " + R["Streetaddress"].ToString() + "   ";
                    if (R["PostalCode"].ToString().Length > 0 || R["City"].ToString().Length > 0)
                        Add = Add.Trim() + "   " + R["PostalCode"].ToString() + " " + R["City"].ToString() + "   ";
                    if (R["Telephone"].ToString().Length > 0)
                        Add = Add.Trim() + "   Tel.: " + R["Telephone"].ToString() + "   ";
                    if (R["Email"].ToString().Length > 0)
                        Add = Add.Trim() + "   E-mail.: " + R["Email"].ToString() + "   ";
                    if (Add.Length > 255)
                        Add = Add.Substring(0, 255);
                    R["Address"] = Add;


                    //R["Address"] = R["Address"].ToString().Trim() + "   " + R["Streetaddress"].ToString() + "   ";
                    //if (R["PostalCode"].ToString().Length > 0 || R["City"].ToString().Length > 0)
                    //    R["Address"] = R["Address"].ToString().Trim() + "   " + R["PostalCode"].ToString() + " " + R["City"].ToString() + "   ";
                    //if (R["Telephone"].ToString().Length > 0)
                    //    R["Address"] = R["Address"].ToString().Trim() + "   Tel.: " + R["Telephone"].ToString() + "   ";
                    //if (R["Email"].ToString().Length > 0)
                    //    R["Address"] = R["Address"].ToString().Trim() + "   E-mail.: " + R["Email"].ToString() + "   ";

                    //}
                    //if (R["Address"].ToString().Length == 0)
                    //    R["Address"] = R["DisplayOrder"].ToString();
                }
                this.listBoxContact.DataSource = this._dtContactList;
                this.listBoxContact.DisplayMember = "Address";
                this.listBoxContact.ValueMember = "DisplayOrder";
                if (this._dtContactList.Rows.Count == 0)
                    this.tableLayoutPanelContact.Enabled = false;
                else
                {
                    if (this.listBoxContact.SelectedIndex == 0)
                    {
                        this.agentContactInformationBindingSource.Position = 0;
                        System.Data.DataRowView RV = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                        if (RV["DisplayOrder"].ToString() == this._CumulativeContactDisplayOrder.ToString())
                        {
                            this.tableLayoutPanelContact.Enabled = false;
                            this.toolStripButtonContactDelete.Enabled = false;
                            this.toolStripButtonSetPreferred.Enabled = false;
                        }
                        else
                        {
                            this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsContactUpdate, "AgentContactInformation", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);
                            this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsContactDelete, "AgentContactInformation", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);
                            if (this.listBoxContact.SelectedIndex > 0 && this.FormFunctions.getObjectPermissions("AgentContactInformation", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update))
                                this.toolStripButtonSetPreferred.Enabled = true;
                            else
                                this.toolStripButtonSetPreferred.Enabled = false;
                        }
                    }
                    else
                    {
                        this.tableLayoutPanelContact.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxContact_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool IsCumulative = false;
            if (this.listBoxContact.SelectedIndex > -1)
            {
                this.agentContactInformationBindingSource.Position = this.listBoxContact.SelectedIndex;
                System.Data.DataRowView RV = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                if (RV["DisplayOrder"].ToString() == this._CumulativeContactDisplayOrder.ToString())
                {
                    this.tableLayoutPanelContact.Enabled = false;
                    this.toolStripButtonContactDelete.Enabled = false;
                    this.toolStripButtonSetPreferred.Enabled = false;
                    IsCumulative = true;
                }
                else
                {
                    this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsContactUpdate, "AgentContactInformation", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);
                    this.FormFunctions.setObjectsAccordingToPermission(this.ListControlsContactDelete, "AgentContactInformation", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);
                    if (this.listBoxContact.SelectedIndex > 0 && this.FormFunctions.getObjectPermissions("AgentContactInformation", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update))
                        this.toolStripButtonSetPreferred.Enabled = true;
                    else
                        this.toolStripButtonSetPreferred.Enabled = false;
                }
            }
            else
                this.tableLayoutPanelContact.Enabled = false;
            this.setDataWithholdingReasonContact(IsCumulative);
        }

        private void buttonOpenURI_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURI.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                if (this.agentContactInformationBindingSource.Current != null)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)agentContactInformationBindingSource.Current;
                    RV["URI"] = f.URL;
                    this.textBoxURI.Text = f.URL;
                }
            }
        }

        private void labelDisplayOrder_TextChanged(object sender, EventArgs e)
        {
            labelDisplayOrder.Visible = (labelDisplayOrder.Text != _CumulativeContactDisplayOrder.ToString());
        }

        #region Toolstrip

        private void toolStripButtonContactNew_Click(object sender, EventArgs e)
        {
            try
            {
                byte DisplayOrder = 1;
                DiversityAgents.Datasets.DataSetAgent.AgentContactInformationRow R = (DiversityAgents.Datasets.DataSetAgent.AgentContactInformationRow)this.dataSetAgent.AgentContactInformation.NewRow();
                if (this.dataSetAgent.AgentContactInformation.Rows.Count > 0)
                {
                    System.Data.DataRow[] rr = this.dataSetAgent.AgentContactInformation.Select("DisplayOrder < " + this._CumulativeContactDisplayOrder.ToString(), "DisplayOrder DESC");
                    if (rr.Length > 0)
                    {
                        DisplayOrder = byte.Parse(rr[0]["DisplayOrder"].ToString());
                        DisplayOrder++;
                    }
                }
                R.AgentID = this.ID;
                R.DisplayOrder = DisplayOrder;
                R.DataWithholdingReason = "Withhold by default";
                this.dataSetAgent.AgentContactInformation.Rows.Add(R);
                this.fillContactList();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void toolStripButtonContactDelete_Click(object sender, EventArgs e)
        {
            if (this.agentContactInformationBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                R.Delete();
                this.fillContactList();
            }
        }

        private void toolStripButtonSetPreferred_Click(object sender, EventArgs e)
        {
            if (this.agentContactInformationBindingSource.Current != null && this.listBoxContact.SelectedIndex > 0)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                System.Data.DataRow[] RR = this.dataSetAgent.AgentContactInformation.Select("DisplayOrder < " + this._CumulativeContactDisplayOrder.ToString(), "DisplayOrder");
                if (RR.Length == 0) return;
                int MinDisplayOrder = int.Parse(RR[0]["DisplayOrder"].ToString());
                System.Data.DataRow[] rr = this.dataSetAgent.AgentContactInformation.Select("DisplayOrder > 0 AND DisplayOrder < " + this._CumulativeContactDisplayOrder.ToString(), "DisplayOrder DESC");
                int MaxDisplayOrder = int.Parse(rr[0]["DisplayOrder"].ToString());
                MaxDisplayOrder++;
                RR[0]["DisplayOrder"] = MaxDisplayOrder;
                RV["DisplayOrder"] = MinDisplayOrder;
                this.updateAgent();
                System.Data.DataRow[] RRU = this.dataSetAgent.AgentContactInformation.Select("DisplayOrder < " + this._CumulativeContactDisplayOrder.ToString(), "DisplayOrder");
                for (int i = 0; i < RRU.Length; i++)
                {
                    RRU[i]["DisplayOrder"] = i + 1;
                }
                this.updateAgent(true);
            }
        }

        private void dateTimePickerContactValidFrom_CloseUp(object sender, EventArgs e)
        {
            if (this.agentContactInformationBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                R["ValidFrom"] = dateTimePickerContactValidFrom.Value.ToShortDateString();
                this.textBoxContactValidFrom.Text = dateTimePickerContactValidFrom.Value.ToShortDateString();
            }
        }

        private void dateTimePickerontactValidUntil_CloseUp(object sender, EventArgs e)
        {
            if (this.agentContactInformationBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                R["ValidUntil"] = dateTimePickerontactValidUntil.Value.ToShortDateString();
                this.textBoxContactValidUntil.Text = dateTimePickerontactValidUntil.Value.ToShortDateString();
            }
        }

        private void buttonValidFromRemove_Click(object sender, EventArgs e)
        {
            if (this.agentContactInformationBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                R["ValidFrom"] = System.DBNull.Value;
                this.textBoxContactValidFrom.Text = "";
            }
        }

        private void buttonValidUntilRemove_Click(object sender, EventArgs e)
        {
            if (this.agentContactInformationBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                R["ValidUntil"] = System.DBNull.Value;
                this.textBoxContactValidUntil.Text = "";
            }
        }

        #endregion

        #endregion

        #region Images

        private void toolStripButtonImageDelete_Click(object sender, EventArgs e)
        {
            if (this.agentImageBindingSource.Current == null)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentImageBindingSource.Current;
                if (R.Row.RowState != DataRowState.Deleted)
                {
                    R.Delete();
                    this.FormFunctions.FillImageList(this.listBoxImages, this.imageList, this.imageListForm, this.dataSetAgent.AgentImage, "URI", this.pictureBoxImage);
                    if (this.listBoxImages.Items.Count > 0)
                        this.listBoxImages.SelectedIndex = 0;
                    else
                    {
                        this.listBoxImages.SelectedIndex = -1;
                        this.pictureBoxImage.Image = null;
                    }
                }
                if (this.listBoxImages.Items.Count > 0)
                    this.tableLayoutPanelImages.Enabled = true;
                else
                    this.tableLayoutPanelImages.Enabled = false;

                /*
                System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this.dataSetAgent, "AgentImage"];
                int p = BMB.Position;
                System.Data.DataRow r = this.dataSetAgent.AgentImage.Rows[p];
                if (r.RowState != System.Data.DataRowState.Deleted)
                {
                    this.dataSetAgent.AgentImage.Rows[p].Delete();
                    this.FormFunctions.FillImageList(this.listBoxImages, this.imageList, this.imageListForm, this.dataSetAgent.AgentImage, "URI", this.pictureBoxImage);
                    if (this.listBoxImages.Items.Count > 0) this.listBoxImages.SelectedIndex = 0;
                    else
                    {
                        this.listBoxImages.SelectedIndex = -1;
                        this.pictureBoxImage.Image = null;
                    }
                }
                if (this.listBoxImages.Items.Count > 0)
                    this.tableLayoutPanelImages.Enabled = true;
                else
                    this.tableLayoutPanelImages.Enabled = false;
                */
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonImageNew_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage();
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.ImagePath.Length > 0)
                    {
                        DiversityAgents.Datasets.DataSetAgent.AgentImageRow R = this.dataSetAgent.AgentImage.NewAgentImageRow();
                        //DiversityAgents.Datasets.DataSetAgent.AgentImageRow R = this.dataSetAgent.AgentImage.NewAgentImageRow();
                        R.AgentID = this.ID;
                        R.URI = f.URIImage;
                        R.Sequence = this.dataSetAgent.AgentImage.Rows.Count;
                        R.DataWithholdingReason = "Withhold by default";
                        this.dataSetAgent.AgentImage.Rows.Add(R);
                        this.setAgent(this.ID);

                        //this.FormFunctions.FillImageList(this.listBoxImages, this.imageList, this.imageListForm, this.dataSetAgent.AgentImage, "URI", this.pictureBoxImage);

                        //if (this.dataSetAgent.AgentImage.Rows.Count > 0)
                        //    this.tableLayoutPanelImages.Enabled = true;
                        //else
                        //    this.tableLayoutPanelImages.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxImages_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                    this.imageList.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
            }
            catch { }
        }

        private void listBoxImages_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageList.ImageSize.Height;
            e.ItemWidth = this.imageList.ImageSize.Width;
        }

        private void listBoxImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //this.tableLayoutPanelImages.Enabled = this.listBoxImages.SelectedIndex > -1;
                //this.splitContainerImages.Panel2.Enabled = this.listBoxImages.SelectedIndex > -1;

                this.pictureBoxImage.Image = null;

                if (this.dataSetAgent.AgentImage.Rows.Count > 0)
                    this.tableLayoutPanelImages.Enabled = true;
                else
                    this.tableLayoutPanelImages.Enabled = false;

                int i = this.listBoxImages.SelectedIndex;
                for (int p = 0; p <= i; p++)
                {
                    if (this.dataSetAgent.AgentImage.Rows[p].RowState == System.Data.DataRowState.Deleted) i++;
                }
                if (this.dataSetAgent.AgentImage.Rows.Count > i && i > -1)
                {
                    //this.groupBoxImages.Enabled = true;
                    this.agentImageBindingSource.Position = i;
                    System.Data.DataRow r = this.dataSetAgent.AgentImage.Rows[i];
                    if (r["URI"].ToString().Length > 0)
                    {
                        DiversityWorkbench.Forms.FormFunctions.SetPictureBoxImage(r["URI"].ToString(), this.pictureBoxImage, this.imageList);
                        this.toolTip.SetToolTip(this.linkLabelImageURI, r["URI"].ToString());
                    }
                    else
                    {
                        this.pictureBoxImage.Image = null;
                    }
                }
                else
                {
                    //this.groupBoxImages.Enabled = false;
                }
                this.setDataWithholdingReasonImage();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                this.pictureBoxImage.Image = null;
                //this.groupBoxImages.Enabled = false;
            }
        }

        private void setHeaderImage()
        {
            if (this.dataSetAgent.AgentImage.Rows.Count > 0)
            {
                //this.groupBoxImages.Enabled = true;
                this.agentImageBindingSource.Position = 0;
                System.Data.DataRow r = this.dataSetAgent.AgentImage.Rows[0];
                if (r["URI"].ToString().Length > 0)
                {
                    DiversityWorkbench.Forms.FormFunctions.SetPictureBoxImage(r["URI"].ToString(), this.pictureBoxHeaderImage, this.imageListHeader);
                    this.pictureBoxHeaderImage.Visible = true;
                    this.pictureBoxHeaderImage.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    this.pictureBoxHeaderImage.Visible = false;
                }
            }
            else
            {
                this.pictureBoxHeaderImage.Visible = false;
            }
        }

        private void toolStripButtonImageNewForm_Click(object sender, EventArgs e)
        {
            if (this.agentImageBindingSource.Current != null)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.agentImageBindingSource.Current;
                if (RV["URI"].ToString().Length == 0) return;
                // #6 Open image in form
                DiversityWorkbench.Forms.FormImage f = new DiversityWorkbench.Forms.FormImage(RV["URI"].ToString());
                if (f != null)
                {
                    f.Location = new Point(this.Location.X + 40, this.Location.Y + 40);
                    f.StartPosition = FormStartPosition.Manual;
                    f.Show();
                }
                else System.Windows.Forms.MessageBox.Show("Could not open image");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
            }
        }

        private void linkLabelImageURI_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.linkLabelImageURI.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.linkLabelImageURI.Text = f.URL;
        }

        #endregion

        #region Projects

        private System.Data.DataTable DtProjects
        {
            get
            {
                try
                {
                    if (this._dtProjects == null)
                    {
                        this._dtProjects = new System.Data.DataTable("Projects");
                        string SQL = "SELECT ProjectID, Project FROM ProjectProxy ORDER BY Project";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(_dtProjects);
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return this._dtProjects;
            }
        }

        private void toolStripButtonProjectNew_Click(object sender, EventArgs e)
        {
            System.Data.DataTable Projects = this.DtProjects.Clone();// new DataTable();
            foreach (System.Data.DataRow R in this.DtProjects.Rows)
            {
                try
                {
                    System.Data.DataRow[] rr = this.dataSetAgent.AgentProject.Select("ProjectID = " + R["ProjectID"].ToString());
                    if (rr.Length == 0)
                    {
                        System.Data.DataRow rP = Projects.NewRow();
                        rP[0] = R[0];
                        rP[1] = R[1];
                        Projects.Rows.Add(rP);
                    }
                }
                catch (System.Exception ex) { }
            }
            if (Projects.Rows.Count > 0)
            {
                DiversityAgents.Forms.FormGetProject f = new Forms.FormGetProject(Projects);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    object[] rowVals = new object[2];
                    rowVals[0] = this.dataSetAgent.Agent.Rows[0]["AgentID"];
                    rowVals[1] = f.ProjectID;
                    try
                    {
                        this.dataSetAgent.AgentProject.Rows.Add(rowVals);
                        this.FormFunctions.updateTable(this.dataSetAgent, "AgentProject", this._SqlDataAdapterAgentProject, this.BindingContext);
                        this.fillProjectList();

                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("All projects included");
        }

        private void toolStripButtonProjectDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (System.Data.DataRow R in this.dataSetAgent.AgentProjectList.Rows)
            {
                if (R.RowState == DataRowState.Added || R.RowState == DataRowState.Modified || R.RowState == DataRowState.Unchanged)
                    i++;
            }
            if (i < 2)
            {
                System.Windows.Forms.MessageBox.Show("The last project can not be removed", "Last project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.listBoxProjects.SelectedItem != null)
            {
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
                try
                {
                    if (rv["ProjectID"].ToString() == "0")
                    {
                        System.Windows.Forms.MessageBox.Show("The default project can not be removed");
                        return;
                    }

                    string Project = rv["Project"].ToString();
                    if (System.Windows.Forms.MessageBox.Show("Do you want to remove this agent from the project \n\r\n\r" + Project, "Remove project?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int ProjectID = int.Parse(this.dataSetAgent.AgentProjectList.Rows[this.agentProjectListBindingSource.Position]["ProjectID"].ToString());
                        this.agentProjectListBindingSource.RemoveCurrent();
                        foreach (System.Data.DataRow R in this.dataSetAgent.AgentProject.Rows)
                        {
                            if (R["ProjectID"].ToString() == ProjectID.ToString())
                            {
                                R.Delete();
                                break;
                            }
                        }
                        this.FormFunctions.updateTable(this.dataSetAgent, "AgentProject", this._SqlDataAdapterAgentProject, this.BindingContext);
                        System.Windows.Forms.MessageBox.Show("Project " + Project, " Project removed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void toolStripButtonProjectOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormRemoteQuery f;
            try
            {
                if (this.agentProjectListBindingSource != null)
                {
                    System.Data.DataRowView RU = (System.Data.DataRowView)this.agentProjectListBindingSource.Current;
                    int ID = 0;
                    if (RU != null)
                    {
                        if (!RU["ProjectID"].Equals(System.DBNull.Value))
                        {
                            ID = int.Parse(RU["ProjectID"].ToString());
                            DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                            f = new DiversityWorkbench.Forms.FormRemoteQuery(ID, (DiversityWorkbench.IWorkbenchUnit)P);
                            f.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void fillProjectList()
        {
            try
            {
                if (this.ID > -1)
                {
                    // Available
                    string SQL = "SELECT A.AgentID, A.ProjectID, L.Project " +
                            "FROM AgentProject A INNER JOIN " +
                            "ProjectList L ON A.ProjectID = L.ProjectID AND (L.ReadOnly = 0 OR L.ReadOnly IS NULL) " +
                            "WHERE A.AgentID = " + this.ID.ToString() + " ORDER BY L.Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    this.dataSetAgent.AgentProjectList.Clear();
                    ad.Fill(this.dataSetAgent.AgentProjectList);
                    this.listBoxProjects.SelectedIndex = -1;

                    // Locked
                    SQL = "SELECT A.AgentID, A.ProjectID, P.Project " +
                            "FROM AgentProject A " +
                            //"INNER JOIN AgentID_Locked L ON A.AgentID = L.AgentID " +
                            "INNER JOIN ProjectProxy P ON A.ProjectID = P.ProjectID AND P.IsLocked = 1 AND A.AgentID = " + this.ID.ToString() +
                        " ORDER BY P.Project";
                    ad.SelectCommand.CommandText = SQL;
                    System.Data.DataTable dtProjectLocked = new DataTable();
                    ad.Fill(dtProjectLocked);
                    if (dtProjectLocked.Rows.Count > 0)
                    {
                        this.listBoxProjectsLocked.DataSource = dtProjectLocked;
                        this.listBoxProjectsLocked.DisplayMember = "Project";
                        this.listBoxProjectsLocked.ValueMember = "Project";
                        this.listBoxProjectsLocked.Height = (dtProjectLocked.Rows.Count * this.listBoxProjectsLocked.ItemHeight) + 10;
                        this.listBoxProjectsLocked.Visible = true;
                        this.listBoxProjectsLocked.SelectedIndex = -1;
                        this.pictureBoxProjectsLocked.Visible = true;
                        //this.toolStripReadOnly.Visible = AllowProjectRemoval;
                        //this.listBoxProjectsReadOnly.Enabled = AllowProjectRemoval;
                        //if (AllowProjectRemoval)
                        //    this.listBoxProjectsReadOnly.SelectionMode = SelectionMode.One;
                        //else
                        //    this.listBoxProjectsReadOnly.SelectionMode = SelectionMode.None;
                    }
                    else
                    {
                        this.listBoxProjectsLocked.Visible = false;
                        this.pictureBoxProjectsLocked.Visible = false;
                        //this.toolStripReadOnly.Visible = false;
                    }


                    // Read only
                    SQL = "SELECT A.AgentID, A.ProjectID, L.Project " +
                            "FROM AgentProject A INNER JOIN " +
                            "ProjectList  L ON A.ProjectID = L.ProjectID AND L.ReadOnly = 1 AND (L.IsLocked IS NULL OR L.IsLocked = 0) " +
                            "WHERE A.AgentID = " + this.ID.ToString() +
                        " ORDER BY L.Project";
                    ad.SelectCommand.CommandText = SQL;
                    System.Data.DataTable dtProjectReadOnly = new DataTable();
                    ad.Fill(dtProjectReadOnly);
                    if (dtProjectReadOnly.Rows.Count > 0)
                    {
                        this.listBoxProjectsReadOnly.DataSource = dtProjectReadOnly;
                        this.listBoxProjectsReadOnly.DisplayMember = "Project";
                        this.listBoxProjectsReadOnly.ValueMember = "Project";
                        this.listBoxProjectsReadOnly.Height = (dtProjectReadOnly.Rows.Count * this.listBoxProjectsReadOnly.ItemHeight) + 10;
                        this.listBoxProjectsReadOnly.Visible = true;
                        this.listBoxProjectsReadOnly.SelectedIndex = -1;
                        this.pictureBoxProjectReadOnly.Visible = true;
                        //this.toolStripReadOnly.Visible = AllowProjectRemoval;
                        //this.listBoxProjectsReadOnly.Enabled = AllowProjectRemoval;
                        //if (AllowProjectRemoval)
                        //    this.listBoxProjectsReadOnly.SelectionMode = SelectionMode.One;
                        //else
                        //    this.listBoxProjectsReadOnly.SelectionMode = SelectionMode.None;
                    }
                    else
                    {
                        this.listBoxProjectsReadOnly.Visible = false;
                        this.pictureBoxProjectReadOnly.Visible = false;
                        //this.toolStripReadOnly.Visible = false;
                    }

                    // No Access
                    SQL = "SELECT P.Project " +
                        "FROM AgentProject A INNER JOIN " +
                        "ProjectProxy P ON A.ProjectID = P.ProjectID " +
                        "WHERE (A.ProjectID NOT IN (SELECT ProjectID FROM ProjectList)) " +
                        "AND (A.AgentID = " + this.ID.ToString() + ") " +
                        "ORDER BY P.Project";
                    ad.SelectCommand.CommandText = SQL;
                    System.Data.DataTable dtProjectNoAccess = new DataTable();
                    ad.Fill(dtProjectNoAccess);
                    if (dtProjectNoAccess.Rows.Count > 0)
                    {
                        this.listBoxProjectsNoAccess.DataSource = dtProjectNoAccess;
                        this.listBoxProjectsNoAccess.DisplayMember = "Project";
                        this.listBoxProjectsNoAccess.ValueMember = "Project";
                        this.listBoxProjectsNoAccess.Height = (dtProjectNoAccess.Rows.Count * this.listBoxProjectsNoAccess.ItemHeight) + 10;
                        this.listBoxProjectsNoAccess.Visible = true;
                        this.listBoxProjectsNoAccess.SelectedIndex = -1;
                        this.pictureBoxProjectNoAccess.Visible = true;
                    }
                    else
                    {
                        this.pictureBoxProjectNoAccess.Visible = false;
                        this.listBoxProjectsNoAccess.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Collections.Generic.Dictionary<string, string> CurrentProject
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> D;
                try
                {
                    DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                    int ProjectID = this.userControlQueryList.ProjectID;
                    D = P.UnitValues(ProjectID);
                }
                catch
                {
                    D = new Dictionary<string, string>();
                }
                return D;
            }
        }

        private string ProjectTitle
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> D = this.CurrentProject;
                string P = "";
                try { P = D["Project"]; }
                catch { }
                return P;
            }
        }

        private string ProjectParentTitle
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> D = this.CurrentProject;
                string P = "";
                try { P = D["Superior project"]; }
                catch { }
                return P;
            }
        }

        #endregion

        //#region Keys - replacement for Keywords

        //private void setKeyControls()
        //{
        //    if (this.KeywordsIncludeType())
        //    {
        //        if (this.agentKeywordBindingSource.Current != null)
        //        {
        //            System.Data.DataRowView R = (System.Data.DataRowView)this.agentKeywordBindingSource.Current;
        //            string Type = R["KeywordType"].ToString();
        //            string KeyModule = this.KeyModule(Type);
        //            switch (KeyModule)
        //            {
        //                case "DiversityAgents":
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //        else
        //        {

        //        }
        //    }
        //    else
        //    {

        //    }
        //}

        //private void setKeyModuleControls(string Module)
        //{
        //    if (Module.Length == 0)
        //    {
        //        this.panelKeyModules.Visible = false;
        //    }
        //    else
        //    {
        //        this.panelKeyModules.Visible = true;
        //        this.userControlModuleRelatedEntryKeyAgents.Visible = false;
        //        this.userControlModuleRelatedEntryKeyCollection.Visible = false;
        //        this.userControlModuleRelatedEntryKeyGazetteer.Visible = false;
        //        this.userControlModuleRelatedEntryKeyReference.Visible = false;
        //        this.userControlModuleRelatedEntryKeyTerms.Visible = false;
        //        this.userControlModuleRelatedEntryKeyTaxa.Visible = false;
        //        switch (Module)
        //        {
        //            case "DiversityAgents":
        //                this.userControlModuleRelatedEntryKeyAgents.Visible = true;
        //                this.userControlModuleRelatedEntryKeyAgents.Dock = DockStyle.Fill;
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        //private string KeyModule(string Type)
        //{
        //    System.Data.DataRow[] RR = this.AgentKeywordType_Enum().Select("Code = '" + Type + "'");
        //    string ModuleName = RR[0]["ModuleName"].ToString();
        //    return ModuleName;
        //}

        //private System.Data.DataTable _AgentKeywordType_Enum;
        //private System.Data.DataTable AgentKeywordType_Enum()
        //{
        //    if (this._AgentKeywordType_Enum == null)
        //    {
        //        this._AgentKeywordType_Enum = new DataTable();
        //        string SQL = "SELECT Code, ParentCode, DisplayText, Icon, ModuleName FROM AgentKeywordType_Enum WHERE (DisplayEnable = 1) ORDER BY DisplayText";
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._AgentKeywordType_Enum);
        //    }
        //    return this._AgentKeywordType_Enum;
        //}

        //#region tree for key
        //private void buildKeywordTree()
        //{
        //    try
        //    {
        //        this.treeViewKey.Nodes.Clear();
        //        // Getting the basic tree according to the types of the current keywords
        //        int AI = this.ID;
        //        string SQL = "declare @Keys Table (Code nvarchar(50), ParentCode nvarchar(50)); " +
        //            "insert into @Keys(Code, ParentCode) " +
        //            "select distinct T.code, T.ParentCode from AgentKeyword K " +
        //            "inner join[dbo].[AgentKeywordType_Enum] T on K.KeywordType = T.Code " +
        //            "where K.AgentID = " + this.ID.ToString() +
        //            " declare @i int " +
        //            "set @i = (select count(*) from @Keys c where c.ParentCode not in (select Code from @Keys) ) " +
        //            "while @i > 0 " +
        //            "begin " +
        //            " insert into @Keys(Code, ParentCode) " +
        //            " select distinct T.code, T.ParentCode from @Keys K " +
        //            " inner join[dbo].[AgentKeywordType_Enum] T on K.ParentCode = T.Code " +
        //            " where K.ParentCode not in (select Code from @Keys) " +
        //            " set @i = (select count(*) from @Keys c where c.ParentCode not in (select Code from @Keys) ) " +
        //            "end " +
        //            "select * from @Keys";
        //        System.Data.DataTable dtTree = new DataTable();
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTree);
        //        System.Data.DataRow[] RR = dtTree.Select("ParentCode IS NULL");
        //        foreach (System.Data.DataRow R in RR)
        //        {
        //            System.Windows.Forms.TreeNode N = new TreeNode(R["Code"].ToString());
        //            N.ForeColor = System.Drawing.Color.Gray;
        //            this.treeViewKey.Nodes.Add(N);
        //            this.buildKeywordTreeAddData(N);
        //            this.buildKeywordTreeAddChild(N, dtTree);
        //        }
        //        this.treeViewKey.ExpandAll();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void buildKeywordTreeAddChild(System.Windows.Forms.TreeNode NParent, System.Data.DataTable dtTree)
        //{
        //    try
        //    {

        //        System.Data.DataRow[] RR = dtTree.Select("ParentCode = '" + NParent.Text + "'");
        //        foreach (System.Data.DataRow R in RR)
        //        {
        //            System.Windows.Forms.TreeNode N = new TreeNode(R["Code"].ToString());
        //            N.ForeColor = System.Drawing.Color.Gray;
        //            NParent.Nodes.Add(N);
        //            this.buildKeywordTreeAddData(N);
        //            this.buildKeywordTreeAddChild(N, dtTree);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void buildKeywordTreeAddData(System.Windows.Forms.TreeNode NParent)
        //{
        //    try
        //    {

        //        string SQL = "select AgentID, Keyword, URL, KeywordType from AgentKeyword K " +
        //                "where K.AgentID = " + this.ID.ToString() + " and K.KeywordType = '" + NParent.Text + "'";
        //        System.Data.DataTable dtKeys = new DataTable();
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtKeys);
        //        foreach (System.Data.DataRow R in dtKeys.Rows)
        //        {
        //            System.Windows.Forms.TreeNode N = new TreeNode(R["Keyword"].ToString());
        //            N.Tag = R;
        //            string Type = R["KeywordType"].ToString();
        //            string Module = this.KeyModule(Type);
        //            if (Module.Length > 0)
        //            {
        //                switch (Module)
        //                {
        //                    case "DiversityAgents":
        //                        N.ImageIndex = 2;
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //            else if (Type == "Keyword")
        //            {
        //                N.ImageIndex = 1;
        //            }
        //            N.SelectedImageIndex = N.ImageIndex;
        //            NParent.Nodes.Add(N);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void treeViewKey_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    try
        //    {
        //        if (this.treeViewKey.SelectedNode.Tag == null)
        //        {
        //            this.tableLayoutPanelKey.Visible = false;
        //        }
        //        else
        //        {
        //            this.tableLayoutPanelKey.Visible = true;
        //            System.Data.DataRow R = (System.Data.DataRow)this.treeViewKey.SelectedNode.Tag;
        //            string Type = R["KeywordType"].ToString();
        //            string KeyModule = this.KeyModule(Type);
        //            this.setKeyModuleControls(KeyModule);
        //            for (int i = 0; i < this.dataSetAgent.AgentKeyword.Rows.Count; i++)
        //            {
        //                if (R["Keyword"].ToString() == this.dataSetAgent.AgentKeyword.Rows[i]["Keyword"].ToString())
        //                {
        //                    this.agentKeywordBindingSource.Position = i;
        //                    break;
        //                }
        //            }
        //            //switch (KeyModule)
        //            //{
        //            //    case "DiversityAgents":
        //            //        break;
        //            //    default:
        //            //        break;
        //            //}
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //#endregion
        //private void toolStripButtonKeyAdd_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Data.DataTable dtKeyType = new DataTable();
        //        string SQL = "SELECT Code, ParentCode, DisplayText FROM AgentKeywordType_Enum WHERE (DisplayEnable = 1)";
        //        string Message = "";
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtKeyType, ref Message);
        //        DiversityWorkbench.FormGetItemFromHierarchy f = new DiversityWorkbench.FormGetItemFromHierarchy(dtKeyType, "Code", "ParentCode", "DisplayText", "DisplayText", "Code", "Please select a type", "Type of the keyword", false, true);
        //        f.ShowDialog();
        //        if (f.DialogResult == DialogResult.OK)
        //        {
        //            string Type = f.SelectedValue;
        //            System.Data.DataTable dtKeys = new DataTable();
        //            SQL = "SELECT DISTINCT Keyword " +
        //                "FROM AgentKeyword " +
        //                "WHERE(KeywordType = N'" + Type + "') AND(Keyword <> N'') " +
        //                "ORDER BY Keyword";
        //            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtKeys, ref Message);
        //            DiversityWorkbench.Forms.FormGetStringFromList fKey = new DiversityWorkbench.Forms.FormGetStringFromList(dtKeys, "Please enter or select the keyword", false, true);
        //            fKey.ShowDialog();
        //            if (fKey.DialogResult == DialogResult.OK && fKey.String.Length > 0)
        //            {
        //                System.Data.DataRow R = this.dataSetAgent.AgentKeyword.NewAgentKeywordRow();
        //                R["AgentID"] = this.ID;
        //                R["Keyword"] = fKey.String;
        //                R["KeywordType"] = Type;
        //                this.dataSetAgent.AgentKeyword.Rows.Add(R);
        //                this.buildKeywordTree();
        //            }
        //        }
        //    }
        //    catch(System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void toolStripButtonKeyDelete_Click(object sender, EventArgs e)
        //{

        //}

        //private void toolStripButtonKeyType_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(
        //            DiversityAgents.Properties.Resources.KeyBig,
        //            "AgentKeywordType_Enum",
        //            "Administration of keyword types",
        //            "");
        //        f.HierarchyChangesEnabled = true;
        //        f.setHelp("Keywords");
        //        System.Collections.Generic.List<string> Locked = new List<string>();
        //        Locked.Add("Keyword");
        //        f.SetLockedList(Locked);
        //        //f.labelCode.TextChanged += new EventHandler(this.TaxonomicGroup_TextChanged);
        //        //f.setOption("", DiversityCollection.Resource.DiversityWorkbench);
        //        f.ShowDialog();
        //        if (f.DataHaveBeenChanged)
        //        {
        //            //DiversityWorkbench.CollectionSpecimen.TaxonomyRelatedTaxonomicGroups = null;
        //            //DiversityCollection.Specimen.TaxonomicGroup_Images = f.Images;
        //            //this.setTaxonomicGroupSource();
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //#endregion

        #region Keywords

        private void toolStripButtonKeywordNew_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT DISTINCT Keyword FROM AgentKeyword WHERE (Keyword <> N'') ORDER BY Keyword";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "New Keyword", "Select a keyword from the list ... ", "or enter a new keyword ...");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                DiversityAgents.Datasets.DataSetAgent.AgentKeywordRow R = this.dataSetAgent.AgentKeyword.NewAgentKeywordRow();
                //DiversityAgents.Datasets.DataSetAgent.AgentKeywordRow R = this.dataSetAgent.AgentKeyword.NewAgentKeywordRow();
                R.AgentID = this.ID;
                R.Keyword = f.String;
                this.dataSetAgent.AgentKeyword.Rows.Add(R);
            }
        }

        private void toolStripButtonKeywordDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentKeywordBindingSource.Current;
                R.BeginEdit();
                R.Delete();
                R.EndEdit();

                //System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this.dataSetAgent, "AgentKeyword"];
                //if (BMB.Position > -1)
                //{
                //    int p = BMB.Position;
                //    System.Data.DataRow r = this.dataSetAgent.AgentKeyword.Rows[p];
                //    if (r.RowState != System.Data.DataRowState.Deleted)
                //    {
                //        this.dataSetAgent.AgentKeyword.Rows[p].Delete();
                //    }
                //}
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region History

        private void buttonHistory_Click(object sender, EventArgs e)
        {
#if xDEBUG
            string Title = "History of " + this.dataSetAgent.Agent.Rows[0]["AgentName"].ToString() + " (AgentID: " + this.dataSetAgent.Agent.Rows[0]["AgentID"].ToString() + ")";
            try
            {
                //System.Collections.Generic.List<DiversityWorkbench.LogDatabase.HistoryTable> LogTables = new List<DiversityWorkbench.LogDatabase.HistoryTable>();
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.LogDatabase.HistoryTable> LogTables = new Dictionary<string, DiversityWorkbench.LogDatabase.HistoryTable>();
                if (this.dataSetAgent.Agent.Rows.Count > 0)
                {
                    DiversityWorkbench.LogDatabase.HistoryTable TA = new DiversityWorkbench.LogDatabase.HistoryTable(this.ID, "AgentID", "Agent");
                    LogTables.Add("Agent", TA);
                }
                if (this.dataSetAgent.AgentContactInformation.Rows.Count > 0)
                {
                    DiversityWorkbench.LogDatabase.HistoryTable TC = new DiversityWorkbench.LogDatabase.HistoryTable(this.ID, "AgentID", "AgentContactInformation");
                    LogTables.Add("AgentContactInformation", TC);
                }
                if (this.dataSetAgent.AgentImage.Rows.Count > 0)
                {
                    DiversityWorkbench.LogDatabase.HistoryTable T = new DiversityWorkbench.LogDatabase.HistoryTable(this.ID, "AgentID", "AgentImage");
                    LogTables.Add("AgentImage", T);
                }
                if (this.dataSetAgent.AgentKeyword.Rows.Count > 0)
                {
                    DiversityWorkbench.LogDatabase.HistoryTable T = new DiversityWorkbench.LogDatabase.HistoryTable(this.ID, "AgentID", "AgentKeyword");
                    LogTables.Add("AgentKeyword", T);
                }
                if (this.dataSetAgent.AgentReference.Rows.Count > 0)
                {
                    DiversityWorkbench.LogDatabase.HistoryTable T = new DiversityWorkbench.LogDatabase.HistoryTable(this.ID, "AgentID", "AgentReference");
                    LogTables.Add("AgentReference", T);
                }
                if (this.dataSetAgent.AgentRelation.Rows.Count > 0)
                {
                    DiversityWorkbench.LogDatabase.HistoryTable T = new DiversityWorkbench.LogDatabase.HistoryTable(this.ID, "AgentID", "AgentRelation");
                    LogTables.Add("AgentRelation", T);
                }

                DiversityWorkbench.FormHistory f = new DiversityWorkbench.FormHistory(Title, LogTables,  DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
#else

            string Title = "History of " + this.dataSetAgent.Agent.Rows[0]["AgentName"].ToString() + " (AgentID: " + this.dataSetAgent.Agent.Rows[0]["AgentID"].ToString() + ")";
            try
            {
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();
                if (this.dataSetAgent.Agent.Rows.Count > 0)
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AgentID", "Agent", ""));
                //if (this.dataSetAgent.AgentContactInformation.Rows.Count > 0)
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AgentID", "AgentContactInformation", ""));
                //if (this.dataSetAgent.AgentImage.Rows.Count > 0)
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AgentID", "AgentImage", ""));
                //if (this.dataSetAgent.AgentKeyword.Rows.Count > 0)
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AgentID", "AgentKeyword", ""));
                //if (this.dataSetAgent.AgentDescriptor.Rows.Count > 0)
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AgentID", "AgentDescriptor", ""));
                //if (this.dataSetAgent.AgentReference.Rows.Count > 0)
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AgentID", "AgentReference", ""));
                //if (this.dataSetAgent.AgentRelation.Rows.Count > 0)
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AgentID", "AgentRelation", ""));
                //if (this.dataSetAgent.AgentIdentifier.Rows.Count > 0)
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AgentID", "AgentIdentifier", ""));
                //if (this.dataSetAgent.AgentHierarchyOther.Rows.Count > 0)
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AgentID", "AgentHierarchyOther", ""));
                //if (this.dataSetAgent.AgentProject.Rows.Count > 0)
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AgentID", "AgentProject", ""));

                DiversityWorkbench.Forms.FormHistory f = new DiversityWorkbench.Forms.FormHistory(Title, LogTables, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());// ...Windows.Forms.Application.StartupPath + "\\DiversityAgent.chm");
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
#endif
        }

        #endregion

        #region Synonymy


        private void fillSynonymyTable()
        {
            string SQL = "SELECT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, " +
                "ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, " +
                "SynonymToAgentID, SynonymisationType, RevisionLevel FROM AgentSynonymy (" + this.ID.ToString() + ")";
            if (this._dtSynonymy == null) this._dtSynonymy = new DataTable();
            this._dtSynonymy.Clear();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(this._dtSynonymy);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buildSynonymy()
        {
            this.fillSynonymyTable();
            this.HierarchyBuildTree(Hierarchy.Synonym);
            //this.treeViewSynonymy.Nodes.Clear();
            //System.Data.DataRow[] rr = this._dtSynonymy.Select("SynonymToAgentID IS NULL");
            //if (rr.Length > 0)
            //{
            //    System.Windows.Forms.TreeNode N = new TreeNode(rr[0]["AgentName"].ToString());
            //    N.Tag = rr[0][0].ToString();
            //    if (!rr[0]["SynonymisationType"].Equals(System.DBNull.Value)
            //        && rr[0]["SynonymisationType"].ToString() == "replaced with")
            //        N.ForeColor = System.Drawing.Color.Red;
            //    if (N.Tag.ToString() == this.ID.ToString()) N.BackColor = System.Drawing.Color.Yellow;
            //    this.treeViewSynonymy.Nodes.Add(N);
            //    this.addSynonymyChildNodes(N);
            //    this.treeViewSynonymy.ExpandAll();
            //}
        }

        private void addSynonymyChildNodes(System.Windows.Forms.TreeNode Node)
        {
            if (Node.Tag != null)
            {
                string ID = Node.Tag.ToString();
                System.Data.DataRow[] rr = this._dtSynonymy.Select("SynonymToAgentID = " + ID);
                if (rr.Length > 0)
                {
                    for (int i = 0; i < rr.Length; i++)
                    {
                        System.Windows.Forms.TreeNode N = new TreeNode(rr[i]["AgentName"].ToString());
                        N.Tag = rr[i][0].ToString();
                        if (N.Tag.ToString() == this.ID.ToString()) N.BackColor = System.Drawing.Color.Yellow;
                        if (!rr[i]["SynonymisationType"].Equals(System.DBNull.Value)
                            && rr[i]["SynonymisationType"].ToString() == "replaced with")
                            N.ForeColor = System.Drawing.Color.Red;
                        Node.Nodes.Add(N);
                        this.addSynonymyChildNodes(N);
                    }
                }
            }
        }

        private void treeViewSynonymy_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int AgentID;
            if (int.TryParse(e.Node.Tag.ToString(), out AgentID)
                && AgentID != this.ID)
                this.setAgent(AgentID);
        }

        private void toolStripButtonChangeToSynonym_Click(object sender, EventArgs e)
        {
            int AgentID;
            if (this.treeViewSynonymy.SelectedNode != null)
            {
                if (int.TryParse(this.treeViewSynonymy.SelectedNode.Tag.ToString(), out AgentID)
                    && AgentID != this.ID)
                    this.setAgent(AgentID);
            }
            else
                System.Windows.Forms.MessageBox.Show("Please select a synonym");
        }

        #endregion

        #region References

        private void toolStripButtonReferenceNew_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityAgents.Datasets.DataSetAgent.AgentReferenceRow R = this.dataSetAgent.AgentReference.NewAgentReferenceRow();
                //DiversityAgents.Datasets.DataSetAgent.AgentReferenceRow R = this.dataSetAgent.AgentReference.NewAgentReferenceRow();
                R.AgentID = this.ID;
                R.ReferenceTitle = "New reference";
                this.dataSetAgent.AgentReference.Rows.Add(R);

            }
            catch { }
        }

        private void toolStripButtonReferencesDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this.dataSetAgent, "AgentReference"];
                if (BMB.Position > -1)
                {
                    int p = BMB.Position;
                    System.Data.DataRow r = this.dataSetAgent.AgentReference.Rows[p];
                    if (r.RowState != System.Data.DataRowState.Deleted)
                    {
                        this.dataSetAgent.AgentReference.Rows[p].Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxReferences_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool CanUpdate = this.FormFunctions.getObjectPermissions("AgentReference", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);
                if (this.listBoxReferences.SelectedIndex > -1)
                {
                    System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this.dataSetAgent, "AgentReference"];
                    BMB.Position = this.listBoxReferences.SelectedIndex;
                    this.tableLayoutPanelReferences.Enabled = CanUpdate;
                }
                else
                    this.tableLayoutPanelReferences.Enabled = false;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        #endregion

        #region Relations

        private void toolStripButtonRelationFind_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this.dataSetAgent, "AgentRelation"];
                if (BMB.Position > -1)
                {
                    int p = BMB.Position;
                    System.Data.DataRow r = this.dataSetAgent.AgentRelation.Rows[p];
                    int RelatedID;
                    if (int.TryParse(r["RelatedAgentID"].ToString(), out RelatedID))
                    {
                        this.setAgent(RelatedID);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonRelationNew_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this.DtAgentList, "AgentName", "AgentID", "Related agent", "Select the related agent from the list");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    DiversityAgents.Datasets.DataSetAgent.AgentRelationRow R = this.dataSetAgent.AgentRelation.NewAgentRelationRow();
                    //DiversityAgents.Datasets.DataSetAgent.AgentRelationRow R = this.dataSetAgent.AgentRelation.NewAgentRelationRow();
                    R.AgentID = this.ID;
                    R.RelatedAgentID = int.Parse(f.SelectedValue);
                    this.dataSetAgent.AgentRelation.Rows.Add(R);
                    this.setAgentRelationList();
                }

            }
            catch { }
        }

        private void toolStripButtonRelationDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this.dataSetAgent, "AgentRelation"];
                if (BMB.Position > -1)
                {
                    int p = BMB.Position;
                    System.Data.DataRow r = this.dataSetAgent.AgentRelation.Rows[p];
                    if (r.RowState != System.Data.DataRowState.Deleted)
                    {
                        this.dataSetAgent.AgentRelation.Rows[p].Delete();
                        this.setAgentRelationList();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public System.Data.DataTable DtAgentList
        {
            get
            {
                try
                {
                    if (this._dtAgentList == null)
                        this._dtAgentList = new DataTable();
                    if (this._dtAgentList.Rows.Count == 0)
                    {
                        string SQL = "SELECT Agent.AgentName, AgentID_UserAvailable.AgentID " +
                            "FROM AgentID_UserAvailable INNER JOIN " +
                            "Agent ON AgentID_UserAvailable.AgentID = Agent.AgentID " +
                            "ORDER BY Agent.AgentName";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(this._dtAgentList);
                    }

                }
                catch { }
                return _dtAgentList;
            }
            //set { _dtAgentList = value; }
        }

        private void setAgentRelationList()
        {
            try
            {
                this.dataSetAgent.AgentRelationList.Clear();
                foreach (DiversityAgents.Datasets.DataSetAgent.AgentRelationRow R in this.dataSetAgent.AgentRelation.Rows)
                {
                    if (R.RowState != DataRowState.Deleted)
                    {
                        DiversityAgents.Datasets.DataSetAgent.AgentRelationListRow RList = this.dataSetAgent.AgentRelationList.NewAgentRelationListRow();
                        //DiversityAgents.Datasets.DataSetAgent.AgentRelationListRow RList = this.dataSetAgent.AgentRelationList.NewAgentRelationListRow();
                        RList.AgentID = R.AgentID;
                        string Agent = R.AgentID.ToString();
                        System.Data.DataRow[] rr = this.DtAgentList.Select("AgentID = " + R.RelatedAgentID.ToString());
                        if (rr.Length > 0)
                            Agent = rr[0]["AgentName"].ToString();
                        RList.RelatedAgent = Agent;
                        RList.RelatedAgentID = R.RelatedAgentID;
                        this.dataSetAgent.AgentRelationList.Rows.Add(RList);
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        private void listBoxRelations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool CanUpdate = this.FormFunctions.getObjectPermissions("AgentRelation", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);
                if (this.listBoxRelations.SelectedIndex > -1)
                {
                    this.agentRelationBindingSource.Position = this.listBoxRelations.SelectedIndex;

                    this.tableLayoutPanelRelations.Enabled = CanUpdate;
                    this.comboBoxDataWithholdingReason.Enabled = CanUpdate;
                    this.comboBoxRelatedAgent.Enabled = CanUpdate;
                }
                else
                    this.tableLayoutPanelRelations.Enabled = false;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxDataWithholdingReason_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT DISTINCT AgentRelation.DataWithholdingReason " +
                "FROM AgentRelation INNER JOIN " +
                "AgentID_UserAvailable ON AgentRelation.AgentID = AgentID_UserAvailable.AgentID " +
                "ORDER BY AgentRelation.DataWithholdingReason";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.comboBoxDataWithholdingReason.DataSource = dt;
            this.comboBoxDataWithholdingReason.DisplayMember = "DataWithholdingReason";
            this.comboBoxDataWithholdingReason.ValueMember = "DataWithholdingReason";
        }

        #endregion

        #region DataWithholdingReason

        private void buttonDataWithholdingReason_Click(object sender, EventArgs e)
        {
            string DataWithholdingReason = "";
            try { DataWithholdingReason = this.dataSetAgent.Agent.Rows[0]["DataWithholdingReason"].ToString(); }
            catch { }
            if (DataWithholdingReason.Length > 0)
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Data withholding reason", "Edit or delete the data withholding reason", DataWithholdingReason);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.dataSetAgent.Agent.Rows[0].BeginEdit();
                    this.dataSetAgent.Agent.Rows[0]["DataWithholdingReason"] = f.String;
                    this.dataSetAgent.Agent.Rows[0].EndEdit();
                    this.setDataWithholdingReason();
                }
            }
            else
            {
                string SQL = "SELECT DISTINCT Agent.DataWithholdingReason " +
                    "FROM Agent INNER JOIN " +
                    "AgentID_UserAvailable ON Agent.AgentID = AgentID_UserAvailable.AgentID " +
                    "ORDER BY Agent.DataWithholdingReason";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DataWithholdingReason", "DataWithholdingReason", "Data withholding reason", "Enter your reason of select it from the list", "", true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.dataSetAgent.Agent.Rows[0].BeginEdit();
                    this.dataSetAgent.Agent.Rows[0]["DataWithholdingReason"] = f.String;
                    this.dataSetAgent.Agent.Rows[0].EndEdit();
                    this.setDataWithholdingReason();
                }
            }
        }

        private void setDataWithholdingReason()
        {
            try
            {
                string DataWithholdingReason = "";
                if (this.dataSetAgent.Agent.Rows.Count > 0)
                {
                    try { DataWithholdingReason = this.dataSetAgent.Agent.Rows[0]["DataWithholdingReason"].ToString(); }
                    catch { }
                }
                if (DataWithholdingReason.Length > 0)
                {
                    this.buttonDataWithholdingReason.Image = this.imageListDataWithholding.Images[0];
                    this.toolTip.SetToolTip(this.buttonDataWithholdingReason, "Data withholding reason for this dataset: " + DataWithholdingReason);
                }
                else
                {
                    this.buttonDataWithholdingReason.Image = this.imageListDataWithholding.Images[1];
                    this.toolTip.SetToolTip(this.buttonDataWithholdingReason, "Click here to enter a data withholding reason for this dataset");
                }
            }
            catch (System.Exception ex)
            { }
        }

        private void buttonDatawithholdingContact_Click(object sender, EventArgs e)
        {
            string DataWithholdingReason = "";
            if (this.dataSetAgent.AgentContactInformation.Rows.Count > 0 && this.agentContactInformationBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                try { DataWithholdingReason = R["DataWithholdingReason"].ToString(); }
                catch { }
            }
            if (DataWithholdingReason.Length > 0)
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Data withholding reason", "Edit or delete the data withholding reason", DataWithholdingReason, DiversityAgents.Properties.Resources.Stop3);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                    R.BeginEdit();
                    R["DataWithholdingReason"] = f.String;
                    R.EndEdit();
                    this.setDataWithholdingReasonContact();
                }
            }
            else
            {
                string SQL = "SELECT DISTINCT A.DataWithholdingReason " +
                    "FROM AgentContactInformation A INNER JOIN " +
                    "AgentID_UserAvailable U ON A.AgentID = U.AgentID " +
                    "ORDER BY A.DataWithholdingReason";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DataWithholdingReason",
                    "DataWithholdingReason", "Data withholding reason", "Enter the withholding reason or select it from the list", "",
                    false, false, false, DiversityAgents.Properties.Resources.Stop3);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                    R.BeginEdit();
                    R["DataWithholdingReason"] = f.String;
                    R.EndEdit();
                    this.setDataWithholdingReasonContact();
                }
            }
        }

        private void setDataWithholdingReasonContact(bool IsCumulative = false)
        {
            string DataWithholdingReason = "";
            if (this.dataSetAgent.AgentContactInformation.Rows.Count > 0 && this.agentContactInformationBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentContactInformationBindingSource.Current;
                try { DataWithholdingReason = R["DataWithholdingReason"].ToString(); }
                catch { }
            }
            if (DataWithholdingReason.Length > 0)
            {
                this.buttonDatawithholdingContact.Image = this.imageListDataWithholding.Images[0];
                this.toolTip.SetToolTip(this.buttonDatawithholdingContact, "Data withholding reason for this dataset: " + DataWithholdingReason);
                this.buttonContactPublic.Visible = true;
                this.tableLayoutPanelContact.BackColor = System.Drawing.Color.Pink;
            }
            else
            {
                this.buttonDatawithholdingContact.Image = this.imageListDataWithholding.Images[1];
                this.toolTip.SetToolTip(this.buttonDatawithholdingContact, "Click here to enter a data withholding reason for this dataset");
                //if (!IsCumulative)
                //    this.buttonContactPublic.Visible = true;
                //else
                //    this.buttonContactPublic.Visible = false;
                this.buttonContactPublic.Visible = true;
                this.tableLayoutPanelContact.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        private void setDataWithholdingReasonImage()
        {
            string DataWithholdingReason = "";
            if (this.dataSetAgent.AgentImage.Rows.Count > 0 && this.agentImageBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentImageBindingSource.Current;
                try { DataWithholdingReason = R["DataWithholdingReason"].ToString(); }
                catch { }
            }
            if (DataWithholdingReason.Length > 0)
            {
                this.buttonImageDataWithholdingReason.Image = this.imageListDataWithholding.Images[0];
                this.toolTip.SetToolTip(this.buttonImageDataWithholdingReason, "Data withholding reason for this dataset: " + DataWithholdingReason);
            }
            else
            {
                this.buttonImageDataWithholdingReason.Image = this.imageListDataWithholding.Images[1];
                this.toolTip.SetToolTip(this.buttonImageDataWithholdingReason, "Click here to enter a data withholding reason for this dataset");
            }
        }

        private void buttonImageDataWithholdingReason_Click(object sender, EventArgs e)
        {
            string DataWithholdingReason = "";
            if (this.dataSetAgent.AgentImage.Rows.Count > 0 && this.agentImageBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentImageBindingSource.Current;
                try { DataWithholdingReason = R["DataWithholdingReason"].ToString(); }
                catch { }
            }
            if (DataWithholdingReason.Length > 0)
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Data withholding reason", "Edit or delete the data withholding reason", DataWithholdingReason);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.agentImageBindingSource.Current;
                    R.BeginEdit();
                    R["DataWithholdingReason"] = f.String;
                    R.EndEdit();
                    this.setDataWithholdingReasonImage();
                }
            }
            else
            {
                string SQL = "SELECT DISTINCT A.DataWithholdingReason " +
                    "FROM AgentImage A INNER JOIN " +
                    "AgentID_UserAvailable U ON A.AgentID = U.AgentID " +
                    "ORDER BY A.DataWithholdingReason";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DataWithholdingReason", "DataWithholdingReason", "Data withholding reason", "Enter your reason of select it from the list", "", true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.agentImageBindingSource.Current;
                    R.BeginEdit();
                    R["DataWithholdingReason"] = f.String;
                    R.EndEdit();
                    this.setDataWithholdingReasonImage();
                }
            }
        }

        private void buttonContactPublic_Click(object sender, EventArgs e)
        {
            // if several contacts are public, only the first public contact is presented
            try
            {
                int DisplayOrder = 0;
                string Title = (this.labelHeaderAgentName.Text);
                string CurrentDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                System.Data.DataRow[] RR = this.dataSetAgent.AgentContactInformation.Select("(DatawithholdingReason IS NULL OR DatawithholdingReason = '') " +
                    "AND (ValidFrom IS NULL OR ValidFrom <= '" + CurrentDate + " 00:00:00') " +
                    "AND (ValidUntil IS NULL OR ValidUntil >= '" + CurrentDate + " 00:00:00')", "DisplayOrder");
                if (RR.Length > 0)
                {
                    DisplayOrder = int.Parse(RR[0]["DisplayOrder"].ToString());
                    if (DisplayOrder != _CumulativeContactDisplayOrder)
                        Title += " (" + DisplayOrder.ToString() + ")";
                }
                if (DisplayOrder == _CumulativeContactDisplayOrder) // Cumulative address
                {
                    DisplayOrder = 0;
                    Title += " (Cumulative address)";
                }
                Forms.FormContactsPublic f = new Forms.FormContactsPublic(Title, this.ID, DisplayOrder);
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return;
            // old version showing every contact in the list
            //System.Collections.Generic.List<int> CC = new List<int>();
            //foreach(System.Data.DataRow R in this.dataSetAgent.AgentContactInformation.Rows)
            //{
            //    if (int.Parse(R["DisplayOrder"].ToString()) == 500)
            //        continue;
            //    CC.Add(int.Parse(R["DisplayOrder"].ToString()));
            //}
            //Forms.FormContactsPublic f = new Forms.FormContactsPublic(this.ID, CC);
            //f.ShowDialog();
        }

        #endregion

        #region ExternalDatasource

        private void toolStripButtonExternalDatasourceAdd_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;

            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT ExternalDatabaseID, ExternalDatabaseName " +
                "FROM dbo.AgentExternalDatabase " +
                "WHERE ExternalDatabaseID NOT IN (SELECT ExternalDatabaseID FROM AgentExternalID WHERE AgentID = " + this.ID.ToString() + ") " +
                "ORDER BY ExternalDatabaseName";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No external source available");
                return;
            }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "ExternalDatabaseName", "ExternalDatabaseID", "External source", "Please select the external source");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string AgentUri = "";
                DiversityWorkbench.Forms.FormGetString fUri = new DiversityWorkbench.Forms.FormGetString("URI", "Do you want to give an URL or similar identifier for the agent", "");
                fUri.ShowDialog();
                if (fUri.DialogResult == System.Windows.Forms.DialogResult.OK && fUri.String.Length > 0)
                    AgentUri = fUri.String;
                try
                {
                    SQL = "INSERT INTO AgentExternalID " +
                        "(AgentID, ExternalDatabaseID, ExternalAgentURI) " +
                        "VALUES (" + this.ID.ToString() + ", " + int.Parse(f.SelectedValue).ToString() + ", ";
                    if (AgentUri.Length > 0)
                        SQL += "'" + AgentUri + "')";
                    else SQL += "NULL)";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    {
                        DiversityAgents.Datasets.DataSetAgent.ExternalDataListRow R = this.dataSetAgent.ExternalDataList.NewExternalDataListRow();
                        //DiversityAgents.Datasets.DataSetAgent.ExternalDataListRow R = this.dataSetAgent.ExternalDataList.NewExternalDataListRow();
                        R.AgentID = this.ID;
                        R.ExternalDatabaseName = f.SelectedString;
                        R.ExternalDatabaseID = f.SelectedValue;
                        if (AgentUri.Length > 0)
                            R.ExternalAgentURI = AgentUri;
                        this.dataSetAgent.ExternalDataList.Rows.Add(R);
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
            }
        }

        private void toolStripButtonExternalDatasourceDelete_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;

            try
            {
                if (this.externalDataListBindingSource.Current != null) // Markus 7.7.23: Check if exists
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.externalDataListBindingSource.Current;
                    string SQL = "DELETE FROM AgentExternalID WHERE AgentID = " + this.ID.ToString() + " AND ExternalDatabaseID = " + R["ExternalDatabaseID"].ToString();
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                        this.externalDataListBindingSource.RemoveCurrent();
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        #endregion

        #region Backlinks

        public void SetHeight(int Height)
        {

        }

        public DiversityWorkbench.WorkbenchUnit.ModuleType GetModuleType()
        {
            return DiversityWorkbench.WorkbenchUnit.ModuleType.Agents;
        }

        private void initBackLinks()
        {
            try
            {
                this.initBackLinksCollection();
                this.initBackLinksPlots();
                this.initBackLinksProjects();
                this.initBackLinksReferences();
                this.initBackLinksTerms();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initBackLinksCollection()
        {
            DiversityWorkbench.WorkbenchUnit.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection, this, this.tabControlModuleConnections, this.tabPageDiversityCollection, this.userControlModuleBackLinksCollection, this.diversityCollectionToolStripMenuItem);
        }

        private void initBackLinksProjects()
        {
            DiversityWorkbench.WorkbenchUnit.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.Projects, this, this.tabControlModuleConnections, this.tabPageDiversityProjects, this.userControlModuleBackLinksProjects, this.diversityProjectsToolStripMenuItem);
        }

        private void initBackLinksReferences()
        {
            DiversityWorkbench.WorkbenchUnit.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.References, this, this.tabControlModuleConnections, this.tabPageDiversityReferences, this.userControlModuleBackLinksReferences, this.diversityReferencesToolStripMenuItem);
        }

        private void initBackLinksPlots()
        {
            DiversityWorkbench.WorkbenchUnit.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots, this, this.tabControlModuleConnections, this.tabPageDiversitySamplingPlots, this.userControlModuleBackLinksPlots, this.diversitySamplingPlotsToolStripMenuItem);
        }

        private void initBackLinksTerms()
        {
            DiversityWorkbench.WorkbenchUnit.initBackLinks(DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms, this, this.tabControlModuleConnections, this.tabPageDiversityScientificTerms, this.userControlModuleBackLinksTerms, this.diversityScientificTermsToolStripMenuItem);
        }


        private void scanModules()
        {
            try
            {
                string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.ID.ToString();
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection))
                {
                    this.userControlModuleBackLinksCollection.SearchForBackLinks(URL);
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Projects))
                {
                    this.userControlModuleBackLinksProjects.SearchForBackLinks(URL);
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.References))
                {
                    this.userControlModuleBackLinksReferences.SearchForBackLinks(URL);
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots))
                {
                    this.userControlModuleBackLinksPlots.SearchForBackLinks(URL);
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms))
                {
                    this.userControlModuleBackLinksTerms.SearchForBackLinks(URL);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Identifier

        private void toolStripButtonIdentifierAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New identifier", "Please enter the value of the new identifier", "");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    DiversityAgents.Datasets.DataSetAgent.AgentIdentifierRow R = this.dataSetAgent.AgentIdentifier.NewAgentIdentifierRow();
                    R.AgentID = this.ID;
                    R.Identifier = f.String;
                    this.dataSetAgent.AgentIdentifier.Rows.Add(R);
                    this.setAgent(this.ID);
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                //DiversityAgents.Datasets.DataSetAgent.agenti
            }
        }

        private void toolStripButtonIdentifierRemove_Click(object sender, EventArgs e)
        {
            if (this.agentIdentifierBindingSource.Current != null)
            {
                if (System.Windows.Forms.MessageBox.Show("Do you want to delete the selected identifier?", "Delete identifier", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.agentIdentifierBindingSource.Current;
                    R.Delete();
                    this.setAgent(this.ID);
                }
            }
        }

        private void listBoxIdentifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxIdentifier.SelectedIndex > -1 && this.agentIdentifierBindingSource != null && this.agentIdentifierBindingSource.Current != null)
            {
                this.tableLayoutPanelIdentifier.Enabled = true;
            }
            else
            {
                this.tableLayoutPanelIdentifier.Enabled = false;
            }
        }


        private void buttonIdentifierURI_Click(object sender, EventArgs e)
        {
            if (this.textBoxIdentifierURI.Text.Length > 0)
            {
                DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxIdentifierURI.Text);
                f.ShowDialog();
            }
            else
            {
                DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser();
                f.ShowDialog();
                if (f.URL.Length > 0)
                    this.textBoxIdentifierURI.Text = f.URL;
            }
        }

        #endregion

        #region Descriptor
        private void setDescriptorControls()
        {
            try
            {
                if (this.agentDescriptorBindingSource.Current != null && this.treeViewDescriptor.SelectedNode != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.agentDescriptorBindingSource.Current;
                    string Type = R["DescriptorType"].ToString();
                    DiversityWorkbench.WorkbenchUnit.ModuleType DescriptorModule = this.DescriptorModule(Type);
                    switch (DescriptorModule)
                    {
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.None:
                            this.setDescritorDefaultControlsVisibility(true);
                            break;
                        default:
                            this.setDescritorDefaultControlsVisibility(false);
                            break;
                    }
                    this.setDescriptorModuleControls(DescriptorModule);
                }
                else
                {
                    this.setDescritorDefaultControlsVisibility(false);
                    this.setDescriptorModuleControls(DiversityWorkbench.WorkbenchUnit.ModuleType.None);
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private DiversityWorkbench.WorkbenchUnit.ModuleType DescriptorModuleType(string Module)
        {
            DiversityWorkbench.WorkbenchUnit.ModuleType moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.None;
            try
            {
                switch (Module)
                {
                    case "DiversityAgents":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Agents;
                        break;
                    case "DiversityCollection":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Collection;
                        break;
                    case "DiversityGazetteer":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer;
                        break;
                    case "DiversityProjects":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Projects;
                        break;
                    case "DiversityReferences":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.References;
                        break;
                    case "DiversitySamplingPlots":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots;
                        break;
                    case "DiversityScientificTerms":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms;
                        break;
                    case "DiversityTaxonNames":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames;
                        break;
                    default:
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.None;
                        break;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return moduleType;
        }

        private void setDescriptorModuleControls(DiversityWorkbench.WorkbenchUnit.ModuleType moduleType)
        {
            try
            {
                if (moduleType == DiversityWorkbench.WorkbenchUnit.ModuleType.None)
                {
                    this.panelDescriptorModules.Visible = false;
                }
                else
                {
                    this.panelDescriptorModules.Visible = true;
                    this.userControlModuleRelatedEntryDescriptorAgent.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorCollection.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorGazetteer.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorProject.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorReference.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorPlots.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorTerms.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorTaxa.Visible = false;
                    switch (moduleType)
                    {
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.Agents:
                            this.userControlModuleRelatedEntryDescriptorAgent.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorAgent.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.Collection:
                            this.userControlModuleRelatedEntryDescriptorCollection.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorCollection.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer:
                            this.userControlModuleRelatedEntryDescriptorGazetteer.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorGazetteer.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.Projects:
                            this.userControlModuleRelatedEntryDescriptorProject.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorProject.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.References:
                            this.userControlModuleRelatedEntryDescriptorReference.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorReference.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots:
                            this.userControlModuleRelatedEntryDescriptorPlots.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorPlots.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms:
                            this.userControlModuleRelatedEntryDescriptorTerms.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorTerms.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames:
                            this.userControlModuleRelatedEntryDescriptorTaxa.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorTaxa.Dock = DockStyle.Fill;
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setDescritorDefaultControlsVisibility(bool isVisible)
        {
            this.textBoxDescriptor.Visible = isVisible;
            this.labelDescriptorURL.Visible = isVisible;
            this.textBoxDescriptorURL.Visible = isVisible;
            this.buttonDescriptorURL.Visible = isVisible;
        }

        private DiversityWorkbench.WorkbenchUnit.ModuleType DescriptorModule(string Type)
        {
            try
            {
                System.Data.DataRow[] RR = this.AgentDescriptorType_Enum().Select("Code = '" + Type + "'");
                if (RR.Length > 0)
                {
                    DiversityWorkbench.WorkbenchUnit.ModuleType Module = this.DescriptorModuleType(RR[0]["ModuleName"].ToString());
                    return Module;
                }
                else
                    return DiversityWorkbench.WorkbenchUnit.ModuleType.None;
            }
            catch (System.Exception ex)
            {
                return DiversityWorkbench.WorkbenchUnit.ModuleType.None;
            }

        }

        private System.Data.DataTable _AgentDescriptorType_Enum;
        private System.Data.DataTable AgentDescriptorType_Enum()
        {
            if (this._AgentDescriptorType_Enum == null || this._AgentDescriptorType_Enum.Columns.Count == 0)
            {
                this._AgentDescriptorType_Enum = new DataTable();
                string SQL = "SELECT Code, ParentCode, DisplayText, Icon, ModuleName FROM AgentDescriptorType_Enum WHERE (DisplayEnable = 1) ORDER BY DisplayText";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._AgentDescriptorType_Enum);
            }
            return this._AgentDescriptorType_Enum;
        }

        #region tree for Descriptor

        private void initDescriptorTypeImages()
        {
            DiversityAgents.Agent.DescriptorTypeImageReset();
            if (this.imageListDescriptor.Images.Count > 10)
            {
                for (int i = this.imageListDescriptor.Images.Count; i > 10; i--)
                    this.imageListDescriptor.Images.RemoveAt(i - 1);
            }
            int LengthOfImageList = this.imageListDescriptor.Images.Count;
            foreach (System.Data.DataRow R in this.AgentDescriptorType_Enum().Rows)
            {
                if (!R["Icon"].Equals(System.DBNull.Value))
                {
                    System.Byte[] B = (System.Byte[])R["Icon"];
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
                    System.Drawing.Image I = System.Drawing.Image.FromStream(ms);
                    System.Drawing.Bitmap BM = new Bitmap(I, 16, 16);
                    System.Drawing.Bitmap BG = DiversityWorkbench.Forms.FormFunctions.MakeGrayscale3(BM);
                    BM.MakeTransparent();
                    DiversityWorkbench.Agent.DescriptorTypeImageAdd(LengthOfImageList, R["Code"].ToString(), BM);
                }
            }
            if (DiversityAgents.Agent.DescriptorTypeImageDict.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Image> kv in DiversityAgents.Agent.DescriptorTypeImageDict)
                {
                    if (!this.imageListDescriptor.Images.ContainsKey(kv.Key))
                        this.imageListDescriptor.Images.Add(kv.Key, kv.Value);
                }
            }
        }


        private void buildDescriptorTree()
        {
            try
            {
                this.treeViewDescriptor.Nodes.Clear();
                // Getting the basic tree according to the types of the current Descriptors
                int AI = this.ID;
                string SQL = "declare @Descriptors Table (Code nvarchar(50), ParentCode nvarchar(50)); " +
                    "insert into @Descriptors(Code, ParentCode) " +
                    "select distinct T.code, T.ParentCode from AgentDescriptor K " +
                    "inner join[dbo].[AgentDescriptorType_Enum] T on K.DescriptorType = T.Code " +
                    "where K.AgentID = " + this.ID.ToString() +
                    "; declare @i int " +
                    "set @i = (select count(*) from @Descriptors c where c.ParentCode not in (select Code from @Descriptors) ); " +
                    "while @i > 0 " +
                    "begin " +
                    " insert into @Descriptors(Code, ParentCode) " +
                    " select distinct T.code, T.ParentCode from @Descriptors K " +
                    " inner join[dbo].[AgentDescriptorType_Enum] T on K.ParentCode = T.Code " +
                    " where K.ParentCode not in (select Code from @Descriptors) " +
                    " set @i = (select count(*) from @Descriptors c where c.ParentCode not in (select Code from @Descriptors) ) " +
                    "end; " +
                    "select * from @Descriptors";
                System.Data.DataTable dtTree = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTree);
                System.Data.DataRow[] RR = dtTree.Select("ParentCode IS NULL");
                foreach (System.Data.DataRow R in RR)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R["Code"].ToString());
                    N.ForeColor = System.Drawing.Color.Gray;
                    this.treeViewDescriptor.Nodes.Add(N);
                    this.buildDescriptorTreeAddData(N);
                    this.buildDescriptorTreeAddChild(N, dtTree);
                }
                this.treeViewDescriptor.ExpandAll();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buildDescriptorTreeAddChild(System.Windows.Forms.TreeNode NParent, System.Data.DataTable dtTree)
        {
            try
            {

                System.Data.DataRow[] RR = dtTree.Select("ParentCode = '" + NParent.Text + "'");
                foreach (System.Data.DataRow R in RR)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R["Code"].ToString());
                    N.ForeColor = System.Drawing.Color.Gray;
                    NParent.Nodes.Add(N);
                    this.buildDescriptorTreeAddData(N);
                    this.buildDescriptorTreeAddChild(N, dtTree);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private System.Collections.Generic.Dictionary<string, System.Drawing.Image> DescriptorImages()
        {
            System.Collections.Generic.Dictionary<string, System.Drawing.Image> images = new Dictionary<string, Image>();
            images.Add("", this.imageListDescriptor.Images[0]);
            images.Add("Descriptor", this.imageListDescriptor.Images[1]);
            images.Add("DiversityAgents", this.imageListDescriptor.Images[2]);
            images.Add("DiversityCollection", this.imageListDescriptor.Images[3]);
            images.Add("DiversityGazetteer", this.imageListDescriptor.Images[4]);
            images.Add("DiversityProjects", this.imageListDescriptor.Images[5]);
            images.Add("DiversityReferences", this.imageListDescriptor.Images[6]);
            images.Add("DiversitySamplingPlots", this.imageListDescriptor.Images[7]);
            images.Add("DiversityScientificTerms", this.imageListDescriptor.Images[8]);
            images.Add("DiversityTaxonNames", this.imageListDescriptor.Images[9]);
            foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Image> kv in DiversityAgents.Agent.DescriptorTypeImageDict)
            {
                if (!images.ContainsKey(kv.Key))
                    images.Add(kv.Key, kv.Value);
            }
            return images;
        }

        private void buildDescriptorTreeAddData(System.Windows.Forms.TreeNode NParent)
        {
            try
            {
                string SQL = "select AgentID, Descriptor, URL, DescriptorType from AgentDescriptor K " +
                        "where K.AgentID = " + this.ID.ToString() + " and K.DescriptorType = '" + NParent.Text + "'";
                System.Data.DataTable dtDescriptors = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtDescriptors);
                foreach (System.Data.DataRow R in dtDescriptors.Rows)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R["Descriptor"].ToString());
                    N.Tag = R;
                    string Type = R["DescriptorType"].ToString();
                    if (DiversityWorkbench.Agent.DescriptorTypeDict.ContainsKey(Type))
                    {
                        N.ImageIndex = DiversityAgents.Agent.DescriptorTypeDict[Type];
                        N.SelectedImageIndex = N.ImageIndex;
                    }
                    else
                    {

                        DiversityWorkbench.WorkbenchUnit.ModuleType Module = this.DescriptorModule(Type);
                        switch (Module)
                        {
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.Agents:
                                N.ImageIndex = 2;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.Collection:
                                N.ImageIndex = 3;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer:
                                N.ImageIndex = 4;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.Projects:
                                N.ImageIndex = 5;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.References:
                                N.ImageIndex = 6;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots:
                                N.ImageIndex = 7;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms:
                                N.ImageIndex = 8;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames:
                                N.ImageIndex = 9;
                                break;
                            default:
                                if (DiversityWorkbench.Agent.DescriptorTypeDict.ContainsKey(Type))
                                {
                                    N.ImageIndex = DiversityAgents.Agent.DescriptorTypeDict[Type];
                                }
                                else
                                    N.ImageIndex = 1;
                                break;
                        }
                        N.SelectedImageIndex = N.ImageIndex;
                    }
                    NParent.Nodes.Add(N);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void treeViewDescriptor_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (this.treeViewDescriptor.SelectedNode.Tag == null)
                {
                    this.setDescritorDefaultControlsVisibility(false);
                    this.setDescriptorModuleControls(DiversityWorkbench.WorkbenchUnit.ModuleType.None);
                }
                else
                {
                    System.Data.DataRow R = (System.Data.DataRow)this.treeViewDescriptor.SelectedNode.Tag;
                    for (int i = 0; i < this.dataSetAgent.AgentDescriptor.Rows.Count; i++)
                    {
                        if (R["Descriptor"].ToString() == this.dataSetAgent.AgentDescriptor.Rows[i]["Descriptor"].ToString())
                        {
                            this.agentDescriptorBindingSource.Position = i;
                            break;
                        }
                    }
                    this.setDescriptorControls();
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion
        private void buttonDescriptorURL_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxDescriptorURL.Text);
            f.Width = this.Width - 20;
            f.Height = this.Height - 20;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.textBoxDescriptorURL.Text = f.URL;
            }
        }

        private void toolStripButtonDescriptorAdd_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dtKeyType = new DataTable();
                string SQL = "SELECT Code, ParentCode, DisplayText FROM AgentDescriptorType_Enum WHERE (DisplayEnable = 1)";
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtKeyType, ref Message);
                if (dtKeyType.Rows.Count == 0)
                {
                    bool OK = this.FormFunctions.getObjectPermissions("AgentDescriptorType_Enum", "Insert");
                    if (OK)
                    {
                        if (System.Windows.Forms.MessageBox.Show("No types have been defined so far. Would you like to add descriptor types?", "No descriptor types", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            this.EditDescriptorTypes();
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("No descriptor types have been defined so far. Please turn to your administrator", "No descriptor types", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    DiversityWorkbench.Forms.FormGetItemFromHierarchy f = new DiversityWorkbench.Forms.FormGetItemFromHierarchy(dtKeyType, "Code", "ParentCode", "DisplayText", "DisplayText", "Code", "Please select a type", "Type of the Descriptor", false, true);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        string Type = f.SelectedValue;
                        System.Data.DataTable dtKeys = new DataTable();
                        SQL = "SELECT DISTINCT Descriptor " +
                            "FROM AgentDescriptor " +
                            "WHERE(DescriptorType = N'" + Type + "') AND(Descriptor <> N'') " +
                            "ORDER BY Descriptor";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtKeys, ref Message);
                        DiversityWorkbench.Forms.FormGetStringFromList fKey = new DiversityWorkbench.Forms.FormGetStringFromList(dtKeys, "Please enter or select the Descriptor", false, true);
                        fKey.ShowDialog();
                        if (fKey.DialogResult == DialogResult.OK && fKey.String.Length > 0)
                        {
                            System.Data.DataRow R = this.dataSetAgent.AgentDescriptor.NewAgentDescriptorRow();
                            R["AgentID"] = this.ID;
                            R["Descriptor"] = fKey.String;
                            R["DescriptorType"] = Type;
                            this.dataSetAgent.AgentDescriptor.Rows.Add(R);
                            this.updateAgent(true);
                            this.buildDescriptorTree();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonDescriptorDelete_Click(object sender, EventArgs e)
        {
            if (this.agentDescriptorBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentDescriptorBindingSource.Current;
                R.Delete();
                this.updateAgent(true);
                this.buildDescriptorTree();
            }
        }

        private void toolStripButtonDescriptorEditTypes_Click(object sender, EventArgs e)
        {
            this.EditDescriptorTypes();
        }

        private void EditDescriptorTypes()
        {
            try
            {
                DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(
                    DiversityAgents.Properties.Resources.KeyBlue,
                    "AgentDescriptorType_Enum",
                    "Administration of Descriptor types",
                    "", this.DescriptorImages());
                f.HierarchyChangesEnabled = true;
                f.ShowImage = true;
                f.setHelp("Descriptor");
                string SQL = "SELECT DescriptorType " +
                    "FROM AgentDescriptor " +
                    "WHERE(URL <> '') " +
                    "GROUP BY DescriptorType " +
                    "HAVING(DescriptorType <> N'')";

                //            string SQL = "SELECT D.DescriptorType " +
                //"FROM AgentDescriptor AS D INNER JOIN " +
                //"AgentDescriptorType_Enum AS T ON D.DescriptorType = T.Code " +
                //"WHERE (D.URL <> '')  AND (T.ModuleName <> '') " +
                //"GROUP BY D.DescriptorType " +
                //"HAVING(D.DescriptorType <> N'')";

                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count > 0)
                {
                    System.Collections.Generic.List<string> Blocked = new List<string>();
                    foreach (System.Data.DataRow R in dt.Rows)
                        Blocked.Add(R[0].ToString());
                    f.setBlockedModuleTypes(Blocked);
                }

                SQL = "SELECT DescriptorType " +
                    "FROM AgentDescriptor " +
                    "GROUP BY DescriptorType " +
                    "HAVING(DescriptorType <> N'')";
                dt.Clear();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count > 0)
                {
                    System.Collections.Generic.List<string> Blocked = new List<string>();
                    foreach (System.Data.DataRow R in dt.Rows)
                        Blocked.Add(R[0].ToString());
                    f.setDoNotDelete(Blocked);
                }
                //System.Collections.Generic.List<string> Locked = new List<string>();
                //Locked.Add("Descriptor");
                //f.SetLockedList(Locked);
                //f.labelCode.TextChanged += new EventHandler(this.TaxonomicGroup_TextChanged);
                //f.setOption("", DiversityCollection.Resource.DiversityWorkbench);
                f.ShowDialog();
                if (f.DataHaveBeenChanged)
                {
                    this._AgentDescriptorType_Enum = null;
                    //DiversityWorkbench.CollectionSpecimen.TaxonomyRelatedTaxonomicGroups = null;
                    //DiversityCollection.Specimen.TaxonomicGroup_Images = f.Images;
                    //this.setTaxonomicGroupSource();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        #endregion

        #region Language

        private void setLanguage()
        {
            try
            {
                if (this.dataSetAgent.Agent.Rows.Count > 0)
                {
                    string Code = this.dataSetAgent.Agent.Rows[0]["ContentLanguage"].ToString();
                    switch (Code)
                    {
                        case "de":
                            this.buttonLanguage.Image = this.imageListLanguage.Images[1];
                            break;
                        case "en":
                            this.buttonLanguage.Image = this.imageListLanguage.Images[2];
                            break;
                        case "fr":
                            this.buttonLanguage.Image = this.imageListLanguage.Images[3];
                            break;
                        case "it":
                            this.buttonLanguage.Image = this.imageListLanguage.Images[4];
                            break;
                        case "es":
                            this.buttonLanguage.Image = this.imageListLanguage.Images[5];
                            break;
                        case "cn":
                        case "zh":
                            this.buttonLanguage.Image = this.imageListLanguage.Images[6];
                            break;
                        default:
                            this.buttonLanguage.Image = this.imageListLanguage.Images[0];
                            break;
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonLanguage_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = DiversityWorkbench.EnumTable.EnumTableForQuery("LanguageCode_Enum");
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(DiversityWorkbench.EnumTable.EnumTableForQuery("LanguageCode_Enum"), "Display", "Value", "Language of content", "Select language from list", "", false, true);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.dataSetAgent.Agent.Rows[0]["ContentLanguage"] = f.SelectedValue;
                this.setLanguage();
            }
        }

        #endregion


        #region Enumerations

        private void relationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditEnumeration(this.relationTypeToolStripMenuItem, "AgentRelationType_Enum", "Relation of agents");
        }

        private void identifierTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditEnumeration(this.identifierTypeToolStripMenuItem, "AgentIdentifierType_Enum", "Types of identifier");
        }

        private void descritporTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditEnumeration(this.descritporTypeToolStripMenuItem, "AgentDescriptorType_Enum", "Types of descriptors");
        }

        private void agentTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditEnumeration(this.agentTypeToolStripMenuItem, "AgentType_Enum", "Types of agent");
        }

        private void displayTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditEnumeration(this.displayTypeToolStripMenuItem, "AgentNameDisplayType_Enum", "Types of agent display");
        }

        private void imageTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditEnumeration(this.imageTypeToolStripMenuItem, "AgentImageType_Enum", "Types of images");
        }

        private void synonymTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditEnumeration(this.synonymTypeToolStripMenuItem, "AgentSynonymisationType_Enum", "Types of synonymy");
        }

        private void keywordTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditEnumeration(this.keywordTypeToolStripMenuItem, "AgentKeywordType_Enum", "Types of keywords");
        }

        private bool EditEnumeration(System.Windows.Forms.ToolStripMenuItem toolStripMenuItem, string EnumTable, string Header)
        {
            bool Updated = false;
            try
            {

                DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(
                    toolStripMenuItem.Image,
                    EnumTable,
                    "Administration of " + Header,
                    "");
                f.HierarchyChangesEnabled = true;
                f.setHelp(Header);
                f.setWarning("Please keep in mind that any changes may have unpredicted effects within depending functions.\r\nA restart may be needed after changes");
                f.ShowDialog();
                if (f.DataHaveBeenChanged)
                {
                    Updated = true;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Updated;
        }


        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// Set help keywords for controls where it does not work automatically
        /// </summary>
        private void InitManualControls()
        {
            try
            {
                helpProvider.SetHelpKeyword(userControlQueryList, "query_da");

                //helpProvider.SetHelpKeyword(this.tabControlHierarchy, "agents_da");
                helpProvider.SetHelpKeyword(this.tabPageDescriptors, "descriptors_da");
                helpProvider.SetHelpKeyword(this.tabPageIdentifier, "identifier_da");
                helpProvider.SetHelpKeyword(this.tabPageImages, "images_da");
                //helpProvider.SetHelpKeyword(this.tabPageReferences, "references_da");
                //helpProvider.SetHelpKeyword(this.tabPageRelations, "relations_da");
                //helpProvider.SetHelpKeyword(this.tabPageSynonymy, "synonymy_da");
                helpProvider.SetHelpKeyword(this.groupBoxContactInformation, "contact_da");
                //helpProvider.SetHelpKeyword(this.groupBoxKeywords, "keywords_da");
                //helpProvider.SetHelpKeyword(this.groupBoxProjects, "projects_da");

            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    this.InitManualControls();
                    await this.InitManual();
                    _InitManualDone = true;
                }
                //else
                //{
                //    DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                //    if (manual != null)
                //    {
                //        await manual.addKeyDownF1ToForm();
                //    }
                //}
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion


    }
}