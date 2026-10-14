namespace DiversityProjects
{
    partial class FormDiversityProjects
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDiversityProjects));
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            userControlQueryList = new DiversityWorkbench.UserControls.UserControlQueryList();
            splitContainerData = new System.Windows.Forms.SplitContainer();
            treeViewProjects = new System.Windows.Forms.TreeView();
            imageListTreeview = new System.Windows.Forms.ImageList(components);
            toolStripHierarchy = new System.Windows.Forms.ToolStrip();
            toolStripButtonProjectNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonProjectDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonProjectSetParent = new System.Windows.Forms.ToolStripButton();
            toolStripButtonProjectRemoveParent = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            labelHeaderProject = new System.Windows.Forms.Label();
            textBoxHeaderProject = new System.Windows.Forms.TextBox();
            projectBindingSource = new System.Windows.Forms.BindingSource(components);
            dataSetDiversityProjects = new DataSetDiversityProjects();
            labelHeaderProjectID = new System.Windows.Forms.Label();
            textBoxHeaderProjectID = new System.Windows.Forms.TextBox();
            buttonHistory = new System.Windows.Forms.Button();
            linkLabelURL = new System.Windows.Forms.LinkLabel();
            ShowJsonCache = new System.Windows.Forms.Button();
            splitContainerSettings = new System.Windows.Forms.SplitContainer();
            splitContainerDetailsAndAgents = new System.Windows.Forms.SplitContainer();
            tableLayoutPanelProject = new System.Windows.Forms.TableLayoutPanel();
            labelURL = new System.Windows.Forms.Label();
            labelProjectTitle = new System.Windows.Forms.Label();
            textBoxProjectTitle = new System.Windows.Forms.TextBox();
            labelProjectDescription = new System.Windows.Forms.Label();
            textBoxProjectDescription = new System.Windows.Forms.TextBox();
            labelProjectVersion = new System.Windows.Forms.Label();
            textBoxProjectVersion = new System.Windows.Forms.TextBox();
            buttonURL = new System.Windows.Forms.Button();
            textBoxURL = new System.Windows.Forms.TextBox();
            labelProjectDescriptionPublic = new System.Windows.Forms.Label();
            textBoxProjectDescriptionPublic = new System.Windows.Forms.TextBox();
            labelProjectDescriptionInternal = new System.Windows.Forms.Label();
            textBoxProjectDescriptionInternal = new System.Windows.Forms.TextBox();
            labelProjectType = new System.Windows.Forms.Label();
            comboBoxProjectType = new System.Windows.Forms.ComboBox();
            labelProjectDescriptionType = new System.Windows.Forms.Label();
            comboBoxProjectDescriptionType = new System.Windows.Forms.ComboBox();
            splitContainerNotesAndLabel = new System.Windows.Forms.SplitContainer();
            textBoxProjectNotes = new System.Windows.Forms.TextBox();
            labelProjectNotes = new System.Windows.Forms.Label();
            labelEmbargoDate = new System.Windows.Forms.Label();
            dateTimePickerEmbargoDate = new System.Windows.Forms.DateTimePicker();
            checkBoxCreateArchive = new System.Windows.Forms.CheckBox();
            textBoxArchiveProtocoll = new System.Windows.Forms.TextBox();
            buttonEmbargoDate = new System.Windows.Forms.Button();
            tabControlSettingsAgentsResources = new System.Windows.Forms.TabControl();
            tabPageSettings = new System.Windows.Forms.TabPage();
            tableLayoutPanelSettings = new System.Windows.Forms.TableLayoutPanel();
            treeViewSettings = new System.Windows.Forms.TreeView();
            toolStripSettings = new System.Windows.Forms.ToolStrip();
            toolStripButtonSettingAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSettingRemove = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSettingCopy = new System.Windows.Forms.ToolStripButton();
            toolStripSeparatorSetting = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonSettingEdit = new System.Windows.Forms.ToolStripButton();
            labelSettingValue = new System.Windows.Forms.Label();
            textBoxSettingValue = new System.Windows.Forms.TextBox();
            projectSettingBindingSource = new System.Windows.Forms.BindingSource(components);
            buttonSettingValueSave = new System.Windows.Forms.Button();
            tabPageAgents = new System.Windows.Forms.TabPage();
            splitContainerAgents = new System.Windows.Forms.SplitContainer();
            listBoxAgents = new System.Windows.Forms.ListBox();
            projectAgentBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripAgents = new System.Windows.Forms.ToolStrip();
            toolStripButtonAgentNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonAgentDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonAgentMoveToTop = new System.Windows.Forms.ToolStripButton();
            toolStripButtonAgentMoveToBase = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelAgent = new System.Windows.Forms.TableLayoutPanel();
            labelAgentRole = new System.Windows.Forms.Label();
            labelAgentNotes = new System.Windows.Forms.Label();
            textBoxAgentNotes = new System.Windows.Forms.TextBox();
            labelAgentType = new System.Windows.Forms.Label();
            comboBoxAgentType = new System.Windows.Forms.ComboBox();
            toolStripRoles = new System.Windows.Forms.ToolStrip();
            toolStripButtonRolesAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonRolesDelete = new System.Windows.Forms.ToolStripButton();
            listBoxRoles = new System.Windows.Forms.ListBox();
            projectAgentRoleBindingSource = new System.Windows.Forms.BindingSource(components);
            userControlModuleRelatedEntryProjectAgent = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            labelAgentRoleDisplayText = new System.Windows.Forms.Label();
            tabPageLicenses = new System.Windows.Forms.TabPage();
            splitContainerLicense = new System.Windows.Forms.SplitContainer();
            listBoxLicense = new System.Windows.Forms.ListBox();
            projectLicenseBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripLicense = new System.Windows.Forms.ToolStrip();
            toolStripButtonLicenseAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonLicenseRemove = new System.Windows.Forms.ToolStripButton();
            groupBoxLicenseNotes = new System.Windows.Forms.GroupBox();
            textBoxLicenseNotes = new System.Windows.Forms.TextBox();
            groupBoxCopyright = new System.Windows.Forms.GroupBox();
            tableLayoutPanelCopyrightHolder = new System.Windows.Forms.TableLayoutPanel();
            userControlModuleRelatedEntryCopyrightHolder = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            pictureBoxCopyrightHolder = new System.Windows.Forms.PictureBox();
            textBoxCopyright = new System.Windows.Forms.TextBox();
            groupBoxIPR = new System.Windows.Forms.GroupBox();
            tableLayoutPanelIPR = new System.Windows.Forms.TableLayoutPanel();
            textBoxIPR = new System.Windows.Forms.TextBox();
            pictureBoxIPRHolder = new System.Windows.Forms.PictureBox();
            userControlModuleRelatedEntryIPRHolder = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            groupBoxLicense = new System.Windows.Forms.GroupBox();
            tableLayoutPanelLicense = new System.Windows.Forms.TableLayoutPanel();
            labelLicenseYear = new System.Windows.Forms.Label();
            textBoxLicenseYear = new System.Windows.Forms.TextBox();
            labelLicenseType = new System.Windows.Forms.Label();
            comboBoxLicenseContext = new System.Windows.Forms.ComboBox();
            labelLicenseContext = new System.Windows.Forms.Label();
            comboBoxLicenseType = new System.Windows.Forms.ComboBox();
            textBoxLicenseDisplayText = new System.Windows.Forms.TextBox();
            buttonLicenseURI = new System.Windows.Forms.Button();
            textBoxLicenseURI = new System.Windows.Forms.TextBox();
            labelLicenseURI = new System.Windows.Forms.Label();
            labelLicenseDetails = new System.Windows.Forms.Label();
            textBoxLicenseDetails = new System.Windows.Forms.TextBox();
            pictureBoxLicenseHolder = new System.Windows.Forms.PictureBox();
            textBoxLicenseID = new System.Windows.Forms.TextBox();
            userControlModuleRelatedEntryLicenseHolder = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            tabPageResources = new System.Windows.Forms.TabPage();
            splitContainerResources = new System.Windows.Forms.SplitContainer();
            listBoxResources = new System.Windows.Forms.ListBox();
            projectResourceBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripResources = new System.Windows.Forms.ToolStrip();
            toolStripButtonResourcesNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonResourceAddLink = new System.Windows.Forms.ToolStripButton();
            toolStripButtonResourcesDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonResourceOpen = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelResources = new System.Windows.Forms.TableLayoutPanel();
            labelFileName = new System.Windows.Forms.Label();
            textBoxResourceNotes = new System.Windows.Forms.TextBox();
            pictureBoxResource = new System.Windows.Forms.PictureBox();
            labelResourceNotes = new System.Windows.Forms.Label();
            labelResourceRights = new System.Windows.Forms.Label();
            textBoxResourceRights = new System.Windows.Forms.TextBox();
            labelResourceLicense = new System.Windows.Forms.Label();
            textBoxResourceLicense = new System.Windows.Forms.TextBox();
            buttonResourceLicense = new System.Windows.Forms.Button();
            buttonResourceOpen = new System.Windows.Forms.Button();
            tabPageReferences = new System.Windows.Forms.TabPage();
            splitContainerReference = new System.Windows.Forms.SplitContainer();
            tableLayoutPanelReferenceLists = new System.Windows.Forms.TableLayoutPanel();
            listBoxReference = new System.Windows.Forms.ListBox();
            projectReferenceBindingSource = new System.Windows.Forms.BindingSource(components);
            labelCitation = new System.Windows.Forms.Label();
            labelReferences = new System.Windows.Forms.Label();
            toolStripCitations = new System.Windows.Forms.ToolStrip();
            toolStripButtonCitationAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonCitationDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonCitationToReference = new System.Windows.Forms.ToolStripButton();
            toolStripButtonReferenceToCitation = new System.Windows.Forms.ToolStripButton();
            listBoxCitations = new System.Windows.Forms.ListBox();
            toolStripReference = new System.Windows.Forms.ToolStrip();
            toolStripButtonReferenceNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonReferenceDelete = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelReference = new System.Windows.Forms.TableLayoutPanel();
            labelReferenceDetails = new System.Windows.Forms.Label();
            labelReferenceNotes = new System.Windows.Forms.Label();
            textBoxReferenceNotes = new System.Windows.Forms.TextBox();
            textBoxReferenceDetails = new System.Windows.Forms.TextBox();
            labelReferenceType = new System.Windows.Forms.Label();
            comboBoxReferenceType = new System.Windows.Forms.ComboBox();
            labelReferenceUri = new System.Windows.Forms.Label();
            textBoxReferenceUri = new System.Windows.Forms.TextBox();
            buttonReferenceUri = new System.Windows.Forms.Button();
            labelReferenceCitation = new System.Windows.Forms.Label();
            textBoxReferenceCitation = new System.Windows.Forms.TextBox();
            labelReferenceTypeDependentCitation = new System.Windows.Forms.Label();
            userControlModuleRelatedEntryReference = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            tabPageDescriptor = new System.Windows.Forms.TabPage();
            splitContainerDescriptor = new System.Windows.Forms.SplitContainer();
            listBoxDescriptor = new System.Windows.Forms.ListBox();
            projectDescriptorBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripDescriptor = new System.Windows.Forms.ToolStrip();
            toolStripButtonDescriptorAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonDescriptorDelete = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelDescriptor = new System.Windows.Forms.TableLayoutPanel();
            labelDescriptorLanguage = new System.Windows.Forms.Label();
            comboBoxDescriptorLanguage = new System.Windows.Forms.ComboBox();
            languageEnumBindingSource = new System.Windows.Forms.BindingSource(components);
            textBoxDescriptorContent = new System.Windows.Forms.TextBox();
            labelDescriptorContent = new System.Windows.Forms.Label();
            labelDescriptorElement = new System.Windows.Forms.Label();
            labelDescriptorContentUri = new System.Windows.Forms.Label();
            textBoxDescriptorContentUri = new System.Windows.Forms.TextBox();
            comboBoxDescriptorElement = new System.Windows.Forms.ComboBox();
            projectDescriptorElementBindingSource = new System.Windows.Forms.BindingSource(components);
            userControlModuleRelatedEntryDescriptorGazetteer = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            userControlModuleRelatedEntryDescriptorTerm = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            userControlModuleRelatedEntryDescriptorTaxon = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            buttonDescriptorTypes = new System.Windows.Forms.Button();
            tabPageID = new System.Windows.Forms.TabPage();
            splitContainerID = new System.Windows.Forms.SplitContainer();
            listBoxID = new System.Windows.Forms.ListBox();
            projectIdentifierBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripID = new System.Windows.Forms.ToolStrip();
            toolStripButtonRequestPID = new System.Windows.Forms.ToolStripButton();
            toolStripButtonIDadd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonIDremove = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelID = new System.Windows.Forms.TableLayoutPanel();
            textBoxIdentifier = new System.Windows.Forms.TextBox();
            labelIDtype = new System.Windows.Forms.Label();
            comboBoxIDtype = new System.Windows.Forms.ComboBox();
            projectIdentifierTypeBindingSource = new System.Windows.Forms.BindingSource(components);
            labelIDurl = new System.Windows.Forms.Label();
            textBoxIDurl = new System.Windows.Forms.TextBox();
            labelIDnotes = new System.Windows.Forms.Label();
            buttonIDgetURL = new System.Windows.Forms.Button();
            textBoxIDnotes = new System.Windows.Forms.TextBox();
            pictureBoxIdentifierLocked = new System.Windows.Forms.PictureBox();
            buttonIDtypes = new System.Windows.Forms.Button();
            tabPageArchive = new System.Windows.Forms.TabPage();
            splitContainerArchive = new System.Windows.Forms.SplitContainer();
            treeViewArchive = new System.Windows.Forms.TreeView();
            toolStripArchive = new System.Windows.Forms.ToolStrip();
            toolStripButtonArchiveAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonArchiveDelete = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelArchive = new System.Windows.Forms.TableLayoutPanel();
            labelArchiveTransfer = new System.Windows.Forms.Label();
            comboBoxArchiveMimeType = new System.Windows.Forms.ComboBox();
            projectArchiveBindingSource = new System.Windows.Forms.BindingSource(components);
            comboBoxArchiveFormat = new System.Windows.Forms.ComboBox();
            labelArchiveFileSize = new System.Windows.Forms.Label();
            textBoxArchiveFileSize = new System.Windows.Forms.TextBox();
            labelArchiveFileHash = new System.Windows.Forms.Label();
            textBoxArchiveFileHash = new System.Windows.Forms.TextBox();
            labelArchiveStorageLocation = new System.Windows.Forms.Label();
            labelArchiveExternalStorageLocation = new System.Windows.Forms.Label();
            labelArchiveTransferDate = new System.Windows.Forms.Label();
            dateTimePickerArchiveTransferDate = new System.Windows.Forms.DateTimePicker();
            labelArchivePublicationDate = new System.Windows.Forms.Label();
            dateTimePickerArchivePublicationDate = new System.Windows.Forms.DateTimePicker();
            labelArchiveDataWithholdingReason = new System.Windows.Forms.Label();
            pictureBoxArchiveDataWithholdingReason = new System.Windows.Forms.PictureBox();
            textBoxArchiveDataWithholdingReason = new System.Windows.Forms.TextBox();
            buttonArchiveStorageLocation = new System.Windows.Forms.Button();
            buttonArchiveExternalStorageLocation = new System.Windows.Forms.Button();
            checkBoxArchivePublished = new System.Windows.Forms.CheckBox();
            labelArchiveNotes = new System.Windows.Forms.Label();
            textBoxArchiveNotes = new System.Windows.Forms.TextBox();
            textBoxArchiveStorageLocation = new System.Windows.Forms.TextBox();
            textBoxArchiveExternalStorageLocation = new System.Windows.Forms.TextBox();
            pictureBoxArchivePublished = new System.Windows.Forms.PictureBox();
            linkLabelArchiveFormat = new System.Windows.Forms.LinkLabel();
            linkLabelArchiveMimeType = new System.Windows.Forms.LinkLabel();
            labelArchiveIdentifier = new System.Windows.Forms.Label();
            toolStripArchiveIdentier = new System.Windows.Forms.ToolStrip();
            toolStripButtonArchiveIdentierAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonArchiveIdentierDelete = new System.Windows.Forms.ToolStripButton();
            listBoxArchiveIdentier = new System.Windows.Forms.ListBox();
            projectArchiveIdentifierBindingSource = new System.Windows.Forms.BindingSource(components);
            labelArchiveParentRelationType = new System.Windows.Forms.Label();
            comboBoxArchiveParentRelationType = new System.Windows.Forms.ComboBox();
            tabPageModules = new System.Windows.Forms.TabPage();
            tabControlModules = new System.Windows.Forms.TabControl();
            tabPageDiversityAgents = new System.Windows.Forms.TabPage();
            userControlModuleBackLinksAgents = new DiversityWorkbench.UserControls.UserControlModuleBackLinks();
            tabPageDiversityCollection = new System.Windows.Forms.TabPage();
            userControlModuleBackLinksCollection = new DiversityWorkbench.UserControls.UserControlModuleBackLinks();
            tabPageDiversityDescriptions = new System.Windows.Forms.TabPage();
            userControlModuleBackLinksDescriptions = new DiversityWorkbench.UserControls.UserControlModuleBackLinks();
            tabPageDiversityGazetteer = new System.Windows.Forms.TabPage();
            userControlModuleBackLinksGazetteer = new DiversityWorkbench.UserControls.UserControlModuleBackLinks();
            tabPageDiversityReferences = new System.Windows.Forms.TabPage();
            userControlModuleBackLinksReferences = new DiversityWorkbench.UserControls.UserControlModuleBackLinks();
            tabPageDiversitySamplingPlots = new System.Windows.Forms.TabPage();
            userControlModuleBackLinksPlots = new DiversityWorkbench.UserControls.UserControlModuleBackLinks();
            tabPageDiversityScientificTerms = new System.Windows.Forms.TabPage();
            userControlModuleBackLinksTerms = new DiversityWorkbench.UserControls.UserControlModuleBackLinks();
            tabPageDiversityTaxonNames = new System.Windows.Forms.TabPage();
            userControlModuleBackLinksTaxa = new DiversityWorkbench.UserControls.UserControlModuleBackLinks();
            imageListTabcontrol = new System.Windows.Forms.ImageList(components);
            toolStripButtonApplicationsNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonApplicationsDelete = new System.Windows.Forms.ToolStripButton();
            menuStripMain = new System.Windows.Forms.MenuStrip();
            connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            createDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            moduleConnectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            queryTimeoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            connectionTimeoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparatorTransfer = new System.Windows.Forms.ToolStripSeparator();
            transferPreviousSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            currentServerActivityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemSeparatorQuit = new System.Windows.Forms.ToolStripSeparator();
            askOnExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exportWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exportDatabaseAsCsvbcpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            gFBioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            importWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tableEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            agentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            descriptorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            iDsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            iPRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            labelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            keywordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            referencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            resourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            projectSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            backupDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            xMLarchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            administrateArchivesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            createArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            resetDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            restoreArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            createSchemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            scanModulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            diversityAgentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            diversityCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            diversityDescriptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            diversityGazetteerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            diversityReferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            diversitySamplingPlotsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            diversityScientificTermsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            diversityTaxonNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            administrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            archiveFormatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            archiveMimeTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            databaseAdministrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            loginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            renameDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            setPublishedAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            databaseToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            descriptorTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            identifierTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            resourcesDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            stableIdentifierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            databaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            descriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            proceduresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            jsonCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            feedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            websitesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            downloadApplicationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            informationModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            errorLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            keepErrorlogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clearErrorlogAtProgramStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clearErrorlogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            metadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            windowSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            xGA1024X768ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            updateDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            updateClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpProviderDiversityProjects = new System.Windows.Forms.HelpProvider();
            toolTip = new System.Windows.Forms.ToolTip(components);
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            projectArchiveIdentifierTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectArchiveIdentifierTableAdapter();
            userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            projectTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectTableAdapter();
            projectSettingTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectSettingTableAdapter();
            projectAgentTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectAgentTableAdapter();
            projectAgentRoleTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectAgentRoleTableAdapter();
            projectIdentifierTypeTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectIdentifierTypeTableAdapter();
            projectLicenseTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectLicenseTableAdapter();
            projectResourceTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectResourceTableAdapter();
            projectDescriptorTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectDescriptorTableAdapter();
            projectDescriptorElementTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectDescriptorElementTableAdapter();
            projectIdentifierTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectIdentifierTableAdapter();
            projectArchiveTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectArchiveTableAdapter();
            projectReferenceTableAdapter = new DataSetDiversityProjectsTableAdapters.ProjectReferenceTableAdapter();
            language_EnumTableAdapter = new DataSetDiversityProjectsTableAdapters.Language_EnumTableAdapter();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerData).BeginInit();
            splitContainerData.Panel1.SuspendLayout();
            splitContainerData.Panel2.SuspendLayout();
            splitContainerData.SuspendLayout();
            toolStripHierarchy.SuspendLayout();
            tableLayoutPanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataSetDiversityProjects).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerSettings).BeginInit();
            splitContainerSettings.Panel1.SuspendLayout();
            splitContainerSettings.Panel2.SuspendLayout();
            splitContainerSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerDetailsAndAgents).BeginInit();
            splitContainerDetailsAndAgents.Panel1.SuspendLayout();
            splitContainerDetailsAndAgents.SuspendLayout();
            tableLayoutPanelProject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerNotesAndLabel).BeginInit();
            splitContainerNotesAndLabel.Panel1.SuspendLayout();
            splitContainerNotesAndLabel.SuspendLayout();
            tabControlSettingsAgentsResources.SuspendLayout();
            tabPageSettings.SuspendLayout();
            tableLayoutPanelSettings.SuspendLayout();
            toolStripSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectSettingBindingSource).BeginInit();
            tabPageAgents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerAgents).BeginInit();
            splitContainerAgents.Panel1.SuspendLayout();
            splitContainerAgents.Panel2.SuspendLayout();
            splitContainerAgents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectAgentBindingSource).BeginInit();
            toolStripAgents.SuspendLayout();
            tableLayoutPanelAgent.SuspendLayout();
            toolStripRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectAgentRoleBindingSource).BeginInit();
            tabPageLicenses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerLicense).BeginInit();
            splitContainerLicense.Panel1.SuspendLayout();
            splitContainerLicense.Panel2.SuspendLayout();
            splitContainerLicense.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectLicenseBindingSource).BeginInit();
            toolStripLicense.SuspendLayout();
            groupBoxLicenseNotes.SuspendLayout();
            groupBoxCopyright.SuspendLayout();
            tableLayoutPanelCopyrightHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCopyrightHolder).BeginInit();
            groupBoxIPR.SuspendLayout();
            tableLayoutPanelIPR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIPRHolder).BeginInit();
            groupBoxLicense.SuspendLayout();
            tableLayoutPanelLicense.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLicenseHolder).BeginInit();
            tabPageResources.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerResources).BeginInit();
            splitContainerResources.Panel1.SuspendLayout();
            splitContainerResources.Panel2.SuspendLayout();
            splitContainerResources.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectResourceBindingSource).BeginInit();
            toolStripResources.SuspendLayout();
            tableLayoutPanelResources.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxResource).BeginInit();
            tabPageReferences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerReference).BeginInit();
            splitContainerReference.Panel1.SuspendLayout();
            splitContainerReference.Panel2.SuspendLayout();
            splitContainerReference.SuspendLayout();
            tableLayoutPanelReferenceLists.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectReferenceBindingSource).BeginInit();
            toolStripCitations.SuspendLayout();
            toolStripReference.SuspendLayout();
            tableLayoutPanelReference.SuspendLayout();
            tabPageDescriptor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerDescriptor).BeginInit();
            splitContainerDescriptor.Panel1.SuspendLayout();
            splitContainerDescriptor.Panel2.SuspendLayout();
            splitContainerDescriptor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectDescriptorBindingSource).BeginInit();
            toolStripDescriptor.SuspendLayout();
            tableLayoutPanelDescriptor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)languageEnumBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)projectDescriptorElementBindingSource).BeginInit();
            tabPageID.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerID).BeginInit();
            splitContainerID.Panel1.SuspendLayout();
            splitContainerID.Panel2.SuspendLayout();
            splitContainerID.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectIdentifierBindingSource).BeginInit();
            toolStripID.SuspendLayout();
            tableLayoutPanelID.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectIdentifierTypeBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIdentifierLocked).BeginInit();
            tabPageArchive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerArchive).BeginInit();
            splitContainerArchive.Panel1.SuspendLayout();
            splitContainerArchive.Panel2.SuspendLayout();
            splitContainerArchive.SuspendLayout();
            toolStripArchive.SuspendLayout();
            tableLayoutPanelArchive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectArchiveBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxArchiveDataWithholdingReason).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxArchivePublished).BeginInit();
            toolStripArchiveIdentier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectArchiveIdentifierBindingSource).BeginInit();
            tabPageModules.SuspendLayout();
            tabControlModules.SuspendLayout();
            tabPageDiversityAgents.SuspendLayout();
            tabPageDiversityCollection.SuspendLayout();
            tabPageDiversityDescriptions.SuspendLayout();
            tabPageDiversityGazetteer.SuspendLayout();
            tabPageDiversityReferences.SuspendLayout();
            tabPageDiversitySamplingPlots.SuspendLayout();
            tabPageDiversityScientificTerms.SuspendLayout();
            tabPageDiversityTaxonNames.SuspendLayout();
            menuStripMain.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(0, 24);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(userControlQueryList);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(splitContainerData);
            splitContainerMain.Size = new System.Drawing.Size(1171, 754);
            splitContainerMain.SplitterDistance = 247;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 1;
            // 
            // userControlQueryList
            // 
            userControlQueryList.BacklinkUpdateEnabled = false;
            userControlQueryList.Connection = null;
            userControlQueryList.DisplayTextSelectedItem = "";
            userControlQueryList.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlQueryList.IDisNumeric = true;
            userControlQueryList.ImageList = null;
            userControlQueryList.IsPredefinedQuery = false;
            userControlQueryList.Location = new System.Drawing.Point(0, 0);
            userControlQueryList.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlQueryList.MaximalNumberOfResults = 100;
            userControlQueryList.Name = "userControlQueryList";
            userControlQueryList.Optimizing_UsedForQueryList = false;
            userControlQueryList.ProjectID = -1;
            userControlQueryList.QueryConditionVisiblity = "";
            userControlQueryList.QueryDisplayColumns = null;
            userControlQueryList.QueryMainTableLocal = null;
            userControlQueryList.QueryRestriction = "";
            userControlQueryList.RememberQuerySettingsIdentifier = "QueryList";
            userControlQueryList.SelectedProjectID = null;
            userControlQueryList.Size = new System.Drawing.Size(247, 754);
            userControlQueryList.TabIndex = 0;
            userControlQueryList.TableColors = null;
            userControlQueryList.TableImageIndex = null;
            userControlQueryList.WhereClause = null;
            // 
            // splitContainerData
            // 
            splitContainerData.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerData.Location = new System.Drawing.Point(0, 0);
            splitContainerData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerData.Name = "splitContainerData";
            splitContainerData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerData.Panel1
            // 
            splitContainerData.Panel1.Controls.Add(treeViewProjects);
            splitContainerData.Panel1.Controls.Add(toolStripHierarchy);
            splitContainerData.Panel1.Controls.Add(tableLayoutPanelHeader);
            // 
            // splitContainerData.Panel2
            // 
            splitContainerData.Panel2.Controls.Add(splitContainerSettings);
            splitContainerData.Size = new System.Drawing.Size(919, 754);
            splitContainerData.SplitterDistance = 135;
            splitContainerData.SplitterWidth = 5;
            splitContainerData.TabIndex = 0;
            // 
            // treeViewProjects
            // 
            treeViewProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProviderDiversityProjects.SetHelpKeyword(treeViewProjects, "Hierarchy");
            helpProviderDiversityProjects.SetHelpNavigator(treeViewProjects, System.Windows.Forms.HelpNavigator.KeywordIndex);
            treeViewProjects.ImageIndex = 0;
            treeViewProjects.ImageList = imageListTreeview;
            treeViewProjects.Location = new System.Drawing.Point(0, 29);
            treeViewProjects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeViewProjects.Name = "treeViewProjects";
            treeViewProjects.SelectedImageIndex = 0;
            helpProviderDiversityProjects.SetShowHelp(treeViewProjects, true);
            treeViewProjects.Size = new System.Drawing.Size(895, 106);
            treeViewProjects.TabIndex = 0;
            treeViewProjects.AfterSelect += treeViewProjects_AfterSelect;
            // 
            // imageListTreeview
            // 
            imageListTreeview.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListTreeview.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListTreeview.ImageStream");
            imageListTreeview.TransparentColor = System.Drawing.Color.Transparent;
            imageListTreeview.Images.SetKeyName(0, "NULL.ico");
            imageListTreeview.Images.SetKeyName(1, "DiversityAgents.ico");
            imageListTreeview.Images.SetKeyName(2, "DiversityCollection.ico");
            imageListTreeview.Images.SetKeyName(3, "Paragraph.ico");
            imageListTreeview.Images.SetKeyName(4, "DiversityDescriptions.ico");
            imageListTreeview.Images.SetKeyName(5, "DiversityGazetteer.ico");
            imageListTreeview.Images.SetKeyName(6, "DiversityProjects.ico");
            imageListTreeview.Images.SetKeyName(7, "DiversityReferences.ico");
            imageListTreeview.Images.SetKeyName(8, "DiversitySamplingPlots.ico");
            imageListTreeview.Images.SetKeyName(9, "DiversityScientificTerms.ico");
            imageListTreeview.Images.SetKeyName(10, "DiversityTaxonNames.ico");
            imageListTreeview.Images.SetKeyName(11, "Checklist.ico");
            // 
            // toolStripHierarchy
            // 
            toolStripHierarchy.Dock = System.Windows.Forms.DockStyle.Right;
            helpProviderDiversityProjects.SetHelpKeyword(toolStripHierarchy, "Hierarchy");
            helpProviderDiversityProjects.SetHelpNavigator(toolStripHierarchy, System.Windows.Forms.HelpNavigator.KeywordIndex);
            toolStripHierarchy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonProjectNew, toolStripButtonProjectDelete, toolStripButtonProjectSetParent, toolStripButtonProjectRemoveParent });
            toolStripHierarchy.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            toolStripHierarchy.Location = new System.Drawing.Point(895, 29);
            toolStripHierarchy.Name = "toolStripHierarchy";
            helpProviderDiversityProjects.SetShowHelp(toolStripHierarchy, true);
            toolStripHierarchy.Size = new System.Drawing.Size(24, 106);
            toolStripHierarchy.TabIndex = 1;
            toolStripHierarchy.Text = "toolStrip1";
            // 
            // toolStripButtonProjectNew
            // 
            toolStripButtonProjectNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectNew.Image = Properties.Resources.New;
            toolStripButtonProjectNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonProjectNew.Name = "toolStripButtonProjectNew";
            toolStripButtonProjectNew.Size = new System.Drawing.Size(23, 20);
            toolStripButtonProjectNew.Text = "Add a project as child of the selected project";
            toolStripButtonProjectNew.Click += toolStripButtonProjectNew_Click;
            // 
            // toolStripButtonProjectDelete
            // 
            toolStripButtonProjectDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectDelete.Image = Properties.Resources.Delete;
            toolStripButtonProjectDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonProjectDelete.Name = "toolStripButtonProjectDelete";
            toolStripButtonProjectDelete.Size = new System.Drawing.Size(23, 20);
            toolStripButtonProjectDelete.Text = "Delete the selected project";
            toolStripButtonProjectDelete.Click += toolStripButtonProjectDelete_Click;
            // 
            // toolStripButtonProjectSetParent
            // 
            toolStripButtonProjectSetParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectSetParent.Image = Properties.Resources.SetParent;
            toolStripButtonProjectSetParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonProjectSetParent.Name = "toolStripButtonProjectSetParent";
            toolStripButtonProjectSetParent.Size = new System.Drawing.Size(23, 20);
            toolStripButtonProjectSetParent.Text = "Set the parent project of the current project";
            toolStripButtonProjectSetParent.Click += toolStripButtonProjectSetParent_Click;
            // 
            // toolStripButtonProjectRemoveParent
            // 
            toolStripButtonProjectRemoveParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectRemoveParent.Image = Properties.Resources.RemoveParent;
            toolStripButtonProjectRemoveParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonProjectRemoveParent.Name = "toolStripButtonProjectRemoveParent";
            toolStripButtonProjectRemoveParent.Size = new System.Drawing.Size(23, 20);
            toolStripButtonProjectRemoveParent.Text = "Remove the relation to the superior project";
            toolStripButtonProjectRemoveParent.Click += toolStripButtonProjectRemoveParent_Click;
            // 
            // tableLayoutPanelHeader
            // 
            tableLayoutPanelHeader.ColumnCount = 7;
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.Controls.Add(labelHeaderProject, 0, 0);
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderProject, 1, 0);
            tableLayoutPanelHeader.Controls.Add(labelHeaderProjectID, 2, 0);
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderProjectID, 3, 0);
            tableLayoutPanelHeader.Controls.Add(buttonHistory, 6, 0);
            tableLayoutPanelHeader.Controls.Add(linkLabelURL, 5, 0);
            tableLayoutPanelHeader.Controls.Add(ShowJsonCache, 4, 0);
            tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            tableLayoutPanelHeader.RowCount = 1;
            tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelHeader.Size = new System.Drawing.Size(919, 29);
            tableLayoutPanelHeader.TabIndex = 2;
            // 
            // labelHeaderProject
            // 
            labelHeaderProject.AutoSize = true;
            labelHeaderProject.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeaderProject.Location = new System.Drawing.Point(4, 0);
            labelHeaderProject.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelHeaderProject.Name = "labelHeaderProject";
            labelHeaderProject.Size = new System.Drawing.Size(47, 29);
            labelHeaderProject.TabIndex = 0;
            labelHeaderProject.Text = "Project:";
            labelHeaderProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxHeaderProject
            // 
            textBoxHeaderProject.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "Project", true));
            textBoxHeaderProject.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxHeaderProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            textBoxHeaderProject.Location = new System.Drawing.Point(55, 3);
            textBoxHeaderProject.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxHeaderProject.Name = "textBoxHeaderProject";
            textBoxHeaderProject.Size = new System.Drawing.Size(654, 20);
            textBoxHeaderProject.TabIndex = 1;
            textBoxHeaderProject.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // projectBindingSource
            // 
            projectBindingSource.DataMember = "Project";
            projectBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // dataSetDiversityProjects
            // 
            dataSetDiversityProjects.DataSetName = "DataSetDiversityProjects";
            dataSetDiversityProjects.Namespace = "http://tempuri.org/DataSetDiversityProjects.xsd";
            dataSetDiversityProjects.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelHeaderProjectID
            // 
            labelHeaderProjectID.AutoSize = true;
            labelHeaderProjectID.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeaderProjectID.Location = new System.Drawing.Point(717, 0);
            labelHeaderProjectID.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelHeaderProjectID.Name = "labelHeaderProjectID";
            labelHeaderProjectID.Size = new System.Drawing.Size(21, 29);
            labelHeaderProjectID.TabIndex = 2;
            labelHeaderProjectID.Text = "ID:";
            labelHeaderProjectID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxHeaderProjectID
            // 
            textBoxHeaderProjectID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxHeaderProjectID.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "ProjectID", true));
            textBoxHeaderProjectID.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxHeaderProjectID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            textBoxHeaderProjectID.Location = new System.Drawing.Point(742, 7);
            textBoxHeaderProjectID.Margin = new System.Windows.Forms.Padding(4, 7, 4, 3);
            textBoxHeaderProjectID.Name = "textBoxHeaderProjectID";
            textBoxHeaderProjectID.ReadOnly = true;
            textBoxHeaderProjectID.Size = new System.Drawing.Size(47, 13);
            textBoxHeaderProjectID.TabIndex = 3;
            // 
            // buttonHistory
            // 
            buttonHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonHistory.Image = Properties.Resources.History;
            buttonHistory.Location = new System.Drawing.Point(890, 2);
            buttonHistory.Margin = new System.Windows.Forms.Padding(4, 2, 0, 2);
            buttonHistory.Name = "buttonHistory";
            buttonHistory.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            buttonHistory.Size = new System.Drawing.Size(29, 25);
            buttonHistory.TabIndex = 4;
            buttonHistory.UseVisualStyleBackColor = true;
            buttonHistory.Click += buttonHistory_Click;
            // 
            // linkLabelURL
            // 
            linkLabelURL.Dock = System.Windows.Forms.DockStyle.Fill;
            linkLabelURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            linkLabelURL.Location = new System.Drawing.Point(820, 0);
            linkLabelURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelURL.Name = "linkLabelURL";
            linkLabelURL.Size = new System.Drawing.Size(62, 29);
            linkLabelURL.TabIndex = 5;
            linkLabelURL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            linkLabelURL.LinkClicked += linkLabelURL_LinkClicked;
            // 
            // ShowJsonCache
            // 
            ShowJsonCache.Dock = System.Windows.Forms.DockStyle.Fill;
            ShowJsonCache.Visible = false;
            ShowJsonCache.FlatAppearance.BorderSize = 0;
            ShowJsonCache.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ShowJsonCache.Image = Properties.Resources.JsonApi;
            ShowJsonCache.Location = new System.Drawing.Point(793, 0);
            ShowJsonCache.Margin = new System.Windows.Forms.Padding(0);
            ShowJsonCache.Name = "ShowJsonCache";
            ShowJsonCache.Size = new System.Drawing.Size(23, 29);
            ShowJsonCache.TabIndex = 6;
            ShowJsonCache.UseVisualStyleBackColor = true;
            ShowJsonCache.MouseUp += ShowJsonCache_MouseUp;
            // 
            // splitContainerSettings
            // 
            splitContainerSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerSettings.Location = new System.Drawing.Point(0, 0);
            splitContainerSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerSettings.Name = "splitContainerSettings";
            splitContainerSettings.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerSettings.Panel1
            // 
            splitContainerSettings.Panel1.Controls.Add(splitContainerDetailsAndAgents);
            // 
            // splitContainerSettings.Panel2
            // 
            splitContainerSettings.Panel2.Controls.Add(tabControlSettingsAgentsResources);
            splitContainerSettings.Size = new System.Drawing.Size(919, 614);
            splitContainerSettings.SplitterDistance = 297;
            splitContainerSettings.SplitterWidth = 5;
            splitContainerSettings.TabIndex = 1;
            // 
            // splitContainerDetailsAndAgents
            // 
            splitContainerDetailsAndAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerDetailsAndAgents.Location = new System.Drawing.Point(0, 0);
            splitContainerDetailsAndAgents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerDetailsAndAgents.Name = "splitContainerDetailsAndAgents";
            splitContainerDetailsAndAgents.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerDetailsAndAgents.Panel1
            // 
            splitContainerDetailsAndAgents.Panel1.Controls.Add(tableLayoutPanelProject);
            splitContainerDetailsAndAgents.Panel2Collapsed = true;
            splitContainerDetailsAndAgents.Size = new System.Drawing.Size(919, 297);
            splitContainerDetailsAndAgents.SplitterDistance = 188;
            splitContainerDetailsAndAgents.SplitterWidth = 5;
            splitContainerDetailsAndAgents.TabIndex = 1;
            // 
            // tableLayoutPanelProject
            // 
            tableLayoutPanelProject.ColumnCount = 9;
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProject.Controls.Add(labelURL, 0, 4);
            tableLayoutPanelProject.Controls.Add(labelProjectTitle, 0, 0);
            tableLayoutPanelProject.Controls.Add(textBoxProjectTitle, 1, 0);
            tableLayoutPanelProject.Controls.Add(labelProjectDescription, 0, 1);
            tableLayoutPanelProject.Controls.Add(textBoxProjectDescription, 0, 2);
            tableLayoutPanelProject.Controls.Add(labelProjectVersion, 0, 8);
            tableLayoutPanelProject.Controls.Add(textBoxProjectVersion, 1, 8);
            tableLayoutPanelProject.Controls.Add(buttonURL, 8, 4);
            tableLayoutPanelProject.Controls.Add(textBoxURL, 1, 4);
            tableLayoutPanelProject.Controls.Add(labelProjectDescriptionPublic, 4, 1);
            tableLayoutPanelProject.Controls.Add(textBoxProjectDescriptionPublic, 4, 2);
            tableLayoutPanelProject.Controls.Add(labelProjectDescriptionInternal, 6, 1);
            tableLayoutPanelProject.Controls.Add(textBoxProjectDescriptionInternal, 6, 2);
            tableLayoutPanelProject.Controls.Add(labelProjectType, 3, 8);
            tableLayoutPanelProject.Controls.Add(comboBoxProjectType, 5, 8);
            tableLayoutPanelProject.Controls.Add(labelProjectDescriptionType, 4, 3);
            tableLayoutPanelProject.Controls.Add(comboBoxProjectDescriptionType, 5, 3);
            tableLayoutPanelProject.Controls.Add(splitContainerNotesAndLabel, 0, 6);
            tableLayoutPanelProject.Controls.Add(labelEmbargoDate, 0, 11);
            tableLayoutPanelProject.Controls.Add(dateTimePickerEmbargoDate, 1, 11);
            tableLayoutPanelProject.Controls.Add(checkBoxCreateArchive, 3, 11);
            tableLayoutPanelProject.Controls.Add(textBoxArchiveProtocoll, 5, 11);
            tableLayoutPanelProject.Controls.Add(buttonEmbargoDate, 2, 11);
            tableLayoutPanelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProviderDiversityProjects.SetHelpKeyword(tableLayoutPanelProject, "Project");
            helpProviderDiversityProjects.SetHelpNavigator(tableLayoutPanelProject, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tableLayoutPanelProject.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelProject.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelProject.Name = "tableLayoutPanelProject";
            tableLayoutPanelProject.RowCount = 12;
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            helpProviderDiversityProjects.SetShowHelp(tableLayoutPanelProject, true);
            tableLayoutPanelProject.Size = new System.Drawing.Size(919, 297);
            tableLayoutPanelProject.TabIndex = 0;
            // 
            // labelURL
            // 
            labelURL.AutoSize = true;
            labelURL.Dock = System.Windows.Forms.DockStyle.Fill;
            labelURL.Location = new System.Drawing.Point(4, 151);
            labelURL.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelURL.Name = "labelURL";
            labelURL.Size = new System.Drawing.Size(62, 30);
            labelURL.TabIndex = 19;
            labelURL.Text = "URI:";
            labelURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelProjectTitle
            // 
            labelProjectTitle.AutoSize = true;
            labelProjectTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectTitle.Location = new System.Drawing.Point(4, 0);
            labelProjectTitle.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelProjectTitle.Name = "labelProjectTitle";
            labelProjectTitle.Size = new System.Drawing.Size(62, 29);
            labelProjectTitle.TabIndex = 2;
            labelProjectTitle.Text = "Title:";
            labelProjectTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProjectTitle
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProjectTitle, 8);
            textBoxProjectTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "ProjectTitle", true));
            textBoxProjectTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectTitle.Location = new System.Drawing.Point(66, 3);
            textBoxProjectTitle.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            textBoxProjectTitle.Name = "textBoxProjectTitle";
            textBoxProjectTitle.Size = new System.Drawing.Size(849, 23);
            textBoxProjectTitle.TabIndex = 3;
            // 
            // labelProjectDescription
            // 
            labelProjectDescription.AutoSize = true;
            tableLayoutPanelProject.SetColumnSpan(labelProjectDescription, 4);
            labelProjectDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectDescription.Location = new System.Drawing.Point(0, 29);
            labelProjectDescription.Margin = new System.Windows.Forms.Padding(0);
            labelProjectDescription.Name = "labelProjectDescription";
            labelProjectDescription.Size = new System.Drawing.Size(336, 15);
            labelProjectDescription.TabIndex = 4;
            labelProjectDescription.Text = "Description";
            labelProjectDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxProjectDescription
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProjectDescription, 4);
            textBoxProjectDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "ProjectDescription", true));
            textBoxProjectDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectDescription.Location = new System.Drawing.Point(4, 44);
            textBoxProjectDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            textBoxProjectDescription.MaxLength = int.MaxValue;
            textBoxProjectDescription.Multiline = true;
            textBoxProjectDescription.Name = "textBoxProjectDescription";
            tableLayoutPanelProject.SetRowSpan(textBoxProjectDescription, 2);
            textBoxProjectDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxProjectDescription.Size = new System.Drawing.Size(328, 104);
            textBoxProjectDescription.TabIndex = 5;
            // 
            // labelProjectVersion
            // 
            labelProjectVersion.AutoSize = true;
            labelProjectVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectVersion.Location = new System.Drawing.Point(4, 235);
            labelProjectVersion.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelProjectVersion.Name = "labelProjectVersion";
            labelProjectVersion.Size = new System.Drawing.Size(62, 26);
            labelProjectVersion.TabIndex = 12;
            labelProjectVersion.Text = "Version:";
            labelProjectVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxProjectVersion
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProjectVersion, 2);
            textBoxProjectVersion.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "ProjectVersion", true));
            textBoxProjectVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectVersion.Location = new System.Drawing.Point(66, 235);
            textBoxProjectVersion.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            textBoxProjectVersion.Name = "textBoxProjectVersion";
            textBoxProjectVersion.Size = new System.Drawing.Size(266, 23);
            textBoxProjectVersion.TabIndex = 13;
            // 
            // buttonURL
            // 
            buttonURL.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonURL.Image = Properties.Resources.Browse;
            buttonURL.Location = new System.Drawing.Point(820, 151);
            buttonURL.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonURL.Name = "buttonURL";
            buttonURL.Size = new System.Drawing.Size(95, 30);
            buttonURL.TabIndex = 20;
            toolTip.SetToolTip(buttonURL, "Search for the URL of the project");
            buttonURL.UseVisualStyleBackColor = true;
            buttonURL.Click += buttonURL_Click;
            // 
            // textBoxURL
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxURL, 7);
            textBoxURL.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "ProjectURL", true));
            textBoxURL.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxURL.Location = new System.Drawing.Point(66, 154);
            textBoxURL.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            textBoxURL.Name = "textBoxURL";
            textBoxURL.Size = new System.Drawing.Size(754, 23);
            textBoxURL.TabIndex = 21;
            // 
            // labelProjectDescriptionPublic
            // 
            labelProjectDescriptionPublic.AutoSize = true;
            tableLayoutPanelProject.SetColumnSpan(labelProjectDescriptionPublic, 2);
            labelProjectDescriptionPublic.Location = new System.Drawing.Point(340, 29);
            labelProjectDescriptionPublic.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectDescriptionPublic.Name = "labelProjectDescriptionPublic";
            labelProjectDescriptionPublic.Size = new System.Drawing.Size(48, 15);
            labelProjectDescriptionPublic.TabIndex = 24;
            labelProjectDescriptionPublic.Text = "- public";
            // 
            // textBoxProjectDescriptionPublic
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProjectDescriptionPublic, 2);
            textBoxProjectDescriptionPublic.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "PublicDescription", true));
            textBoxProjectDescriptionPublic.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectDescriptionPublic.Location = new System.Drawing.Point(340, 44);
            textBoxProjectDescriptionPublic.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            textBoxProjectDescriptionPublic.Multiline = true;
            textBoxProjectDescriptionPublic.Name = "textBoxProjectDescriptionPublic";
            textBoxProjectDescriptionPublic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxProjectDescriptionPublic.Size = new System.Drawing.Size(276, 81);
            textBoxProjectDescriptionPublic.TabIndex = 25;
            // 
            // labelProjectDescriptionInternal
            // 
            labelProjectDescriptionInternal.AutoSize = true;
            tableLayoutPanelProject.SetColumnSpan(labelProjectDescriptionInternal, 3);
            labelProjectDescriptionInternal.Location = new System.Drawing.Point(624, 29);
            labelProjectDescriptionInternal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectDescriptionInternal.Name = "labelProjectDescriptionInternal";
            labelProjectDescriptionInternal.Size = new System.Drawing.Size(55, 15);
            labelProjectDescriptionInternal.TabIndex = 26;
            labelProjectDescriptionInternal.Text = "- internal";
            // 
            // textBoxProjectDescriptionInternal
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProjectDescriptionInternal, 3);
            textBoxProjectDescriptionInternal.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "InternalDescription", true));
            textBoxProjectDescriptionInternal.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectDescriptionInternal.Location = new System.Drawing.Point(624, 44);
            textBoxProjectDescriptionInternal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            textBoxProjectDescriptionInternal.Multiline = true;
            textBoxProjectDescriptionInternal.Name = "textBoxProjectDescriptionInternal";
            tableLayoutPanelProject.SetRowSpan(textBoxProjectDescriptionInternal, 2);
            textBoxProjectDescriptionInternal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxProjectDescriptionInternal.Size = new System.Drawing.Size(291, 104);
            textBoxProjectDescriptionInternal.TabIndex = 27;
            // 
            // labelProjectType
            // 
            labelProjectType.AutoSize = true;
            tableLayoutPanelProject.SetColumnSpan(labelProjectType, 2);
            labelProjectType.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectType.Image = Properties.Resources.Project;
            labelProjectType.Location = new System.Drawing.Point(336, 235);
            labelProjectType.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelProjectType.Name = "labelProjectType";
            labelProjectType.Size = new System.Drawing.Size(134, 26);
            labelProjectType.TabIndex = 28;
            labelProjectType.Text = "Type:";
            labelProjectType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxProjectType
            // 
            tableLayoutPanelProject.SetColumnSpan(comboBoxProjectType, 4);
            comboBoxProjectType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", projectBindingSource, "ProjectType", true));
            comboBoxProjectType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxProjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxProjectType.FormattingEnabled = true;
            comboBoxProjectType.Location = new System.Drawing.Point(470, 235);
            comboBoxProjectType.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            comboBoxProjectType.Name = "comboBoxProjectType";
            comboBoxProjectType.Size = new System.Drawing.Size(445, 23);
            comboBoxProjectType.TabIndex = 29;
            comboBoxProjectType.SelectedIndexChanged += comboBoxProjectType_SelectedIndexChanged;
            // 
            // labelProjectDescriptionType
            // 
            labelProjectDescriptionType.AutoSize = true;
            labelProjectDescriptionType.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectDescriptionType.Location = new System.Drawing.Point(336, 125);
            labelProjectDescriptionType.Margin = new System.Windows.Forms.Padding(0);
            labelProjectDescriptionType.Name = "labelProjectDescriptionType";
            labelProjectDescriptionType.Size = new System.Drawing.Size(134, 26);
            labelProjectDescriptionType.TabIndex = 30;
            labelProjectDescriptionType.Text = "Type:";
            labelProjectDescriptionType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxProjectDescriptionType
            // 
            comboBoxProjectDescriptionType.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "ProjectDescriptionType", true));
            comboBoxProjectDescriptionType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxProjectDescriptionType.FormattingEnabled = true;
            comboBoxProjectDescriptionType.Location = new System.Drawing.Point(470, 125);
            comboBoxProjectDescriptionType.Margin = new System.Windows.Forms.Padding(0, 0, 4, 3);
            comboBoxProjectDescriptionType.Name = "comboBoxProjectDescriptionType";
            comboBoxProjectDescriptionType.Size = new System.Drawing.Size(146, 23);
            comboBoxProjectDescriptionType.TabIndex = 31;
            comboBoxProjectDescriptionType.DropDown += comboBoxProjectDescriptionType_DropDown;
            // 
            // splitContainerNotesAndLabel
            // 
            tableLayoutPanelProject.SetColumnSpan(splitContainerNotesAndLabel, 9);
            splitContainerNotesAndLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerNotesAndLabel.Location = new System.Drawing.Point(4, 184);
            splitContainerNotesAndLabel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerNotesAndLabel.Name = "splitContainerNotesAndLabel";
            // 
            // splitContainerNotesAndLabel.Panel1
            // 
            splitContainerNotesAndLabel.Panel1.Controls.Add(textBoxProjectNotes);
            splitContainerNotesAndLabel.Panel1.Controls.Add(labelProjectNotes);
            splitContainerNotesAndLabel.Panel2Collapsed = true;
            splitContainerNotesAndLabel.Size = new System.Drawing.Size(911, 48);
            splitContainerNotesAndLabel.SplitterDistance = 340;
            splitContainerNotesAndLabel.SplitterWidth = 5;
            splitContainerNotesAndLabel.TabIndex = 35;
            // 
            // textBoxProjectNotes
            // 
            textBoxProjectNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "ProjectNotes", true));
            textBoxProjectNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectNotes.Location = new System.Drawing.Point(0, 15);
            textBoxProjectNotes.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            textBoxProjectNotes.Multiline = true;
            textBoxProjectNotes.Name = "textBoxProjectNotes";
            textBoxProjectNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxProjectNotes.Size = new System.Drawing.Size(911, 33);
            textBoxProjectNotes.TabIndex = 9;
            // 
            // labelProjectNotes
            // 
            labelProjectNotes.AutoSize = true;
            labelProjectNotes.Dock = System.Windows.Forms.DockStyle.Top;
            labelProjectNotes.Location = new System.Drawing.Point(0, 0);
            labelProjectNotes.Margin = new System.Windows.Forms.Padding(4, 7, 0, 0);
            labelProjectNotes.Name = "labelProjectNotes";
            labelProjectNotes.Size = new System.Drawing.Size(41, 15);
            labelProjectNotes.TabIndex = 8;
            labelProjectNotes.Text = "Notes:";
            labelProjectNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelEmbargoDate
            // 
            labelEmbargoDate.AutoSize = true;
            labelEmbargoDate.Dock = System.Windows.Forms.DockStyle.Fill;
            labelEmbargoDate.Location = new System.Drawing.Point(4, 261);
            labelEmbargoDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelEmbargoDate.Name = "labelEmbargoDate";
            labelEmbargoDate.Size = new System.Drawing.Size(58, 36);
            labelEmbargoDate.TabIndex = 36;
            labelEmbargoDate.Text = "Embargo:";
            labelEmbargoDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerEmbargoDate
            // 
            dateTimePickerEmbargoDate.CustomFormat = "--";
            dateTimePickerEmbargoDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", projectBindingSource, "EmbargoDate", true));
            dateTimePickerEmbargoDate.Dock = System.Windows.Forms.DockStyle.Fill;
            dateTimePickerEmbargoDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dateTimePickerEmbargoDate.Location = new System.Drawing.Point(66, 264);
            dateTimePickerEmbargoDate.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            dateTimePickerEmbargoDate.Name = "dateTimePickerEmbargoDate";
            dateTimePickerEmbargoDate.Size = new System.Drawing.Size(250, 23);
            dateTimePickerEmbargoDate.TabIndex = 37;
            dateTimePickerEmbargoDate.CloseUp += dateTimePickerEmbargoDate_CloseUp;
            dateTimePickerEmbargoDate.ValueChanged += dateTimePickerEmbargoDate_ValueChanged;
            dateTimePickerEmbargoDate.Validating += dateTimePickerEmbargoDate_Validating;
            // 
            // checkBoxCreateArchive
            // 
            tableLayoutPanelProject.SetColumnSpan(checkBoxCreateArchive, 2);
            checkBoxCreateArchive.DataBindings.Add(new System.Windows.Forms.Binding("Checked", projectBindingSource, "CreateArchive", true));
            checkBoxCreateArchive.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxCreateArchive.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            checkBoxCreateArchive.Location = new System.Drawing.Point(332, 261);
            checkBoxCreateArchive.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            checkBoxCreateArchive.Name = "checkBoxCreateArchive";
            checkBoxCreateArchive.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            checkBoxCreateArchive.Size = new System.Drawing.Size(134, 36);
            checkBoxCreateArchive.TabIndex = 38;
            checkBoxCreateArchive.Text = "Create archive";
            checkBoxCreateArchive.UseVisualStyleBackColor = true;
            // 
            // textBoxArchiveProtocoll
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxArchiveProtocoll, 4);
            textBoxArchiveProtocoll.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "ArchiveProtocol", true));
            textBoxArchiveProtocoll.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxArchiveProtocoll.Location = new System.Drawing.Point(470, 264);
            textBoxArchiveProtocoll.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            textBoxArchiveProtocoll.Name = "textBoxArchiveProtocoll";
            textBoxArchiveProtocoll.Size = new System.Drawing.Size(445, 23);
            textBoxArchiveProtocoll.TabIndex = 39;
            // 
            // buttonEmbargoDate
            // 
            buttonEmbargoDate.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonEmbargoDate.FlatAppearance.BorderSize = 0;
            buttonEmbargoDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonEmbargoDate.Image = Properties.Resources.Delete;
            buttonEmbargoDate.Location = new System.Drawing.Point(316, 264);
            buttonEmbargoDate.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonEmbargoDate.Name = "buttonEmbargoDate";
            buttonEmbargoDate.Size = new System.Drawing.Size(16, 30);
            buttonEmbargoDate.TabIndex = 40;
            toolTip.SetToolTip(buttonEmbargoDate, "Remove embargo");
            buttonEmbargoDate.UseVisualStyleBackColor = true;
            buttonEmbargoDate.Click += buttonEmbargoDate_Click;
            // 
            // tabControlSettingsAgentsResources
            // 
            tabControlSettingsAgentsResources.Controls.Add(tabPageSettings);
            tabControlSettingsAgentsResources.Controls.Add(tabPageAgents);
            tabControlSettingsAgentsResources.Controls.Add(tabPageLicenses);
            tabControlSettingsAgentsResources.Controls.Add(tabPageResources);
            tabControlSettingsAgentsResources.Controls.Add(tabPageReferences);
            tabControlSettingsAgentsResources.Controls.Add(tabPageDescriptor);
            tabControlSettingsAgentsResources.Controls.Add(tabPageID);
            tabControlSettingsAgentsResources.Controls.Add(tabPageArchive);
            tabControlSettingsAgentsResources.Controls.Add(tabPageModules);
            tabControlSettingsAgentsResources.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProviderDiversityProjects.SetHelpKeyword(tabControlSettingsAgentsResources, "License");
            helpProviderDiversityProjects.SetHelpNavigator(tabControlSettingsAgentsResources, System.Windows.Forms.HelpNavigator.KeywordIndex);
            helpProviderDiversityProjects.SetHelpString(tabControlSettingsAgentsResources, "License");
            tabControlSettingsAgentsResources.ImageList = imageListTabcontrol;
            tabControlSettingsAgentsResources.Location = new System.Drawing.Point(0, 0);
            tabControlSettingsAgentsResources.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControlSettingsAgentsResources.Name = "tabControlSettingsAgentsResources";
            tabControlSettingsAgentsResources.SelectedIndex = 0;
            helpProviderDiversityProjects.SetShowHelp(tabControlSettingsAgentsResources, true);
            tabControlSettingsAgentsResources.Size = new System.Drawing.Size(919, 312);
            tabControlSettingsAgentsResources.TabIndex = 2;
            // 
            // tabPageSettings
            // 
            tabPageSettings.Controls.Add(tableLayoutPanelSettings);
            tabPageSettings.ImageIndex = 1;
            tabPageSettings.Location = new System.Drawing.Point(4, 24);
            tabPageSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageSettings.Name = "tabPageSettings";
            tabPageSettings.Size = new System.Drawing.Size(911, 284);
            tabPageSettings.TabIndex = 4;
            tabPageSettings.Text = "Settings";
            tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSettings
            // 
            tableLayoutPanelSettings.ColumnCount = 4;
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.Controls.Add(treeViewSettings, 0, 1);
            tableLayoutPanelSettings.Controls.Add(toolStripSettings, 0, 0);
            tableLayoutPanelSettings.Controls.Add(labelSettingValue, 1, 0);
            tableLayoutPanelSettings.Controls.Add(textBoxSettingValue, 2, 0);
            tableLayoutPanelSettings.Controls.Add(buttonSettingValueSave, 3, 0);
            tableLayoutPanelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProviderDiversityProjects.SetHelpKeyword(tableLayoutPanelSettings, "Settings");
            helpProviderDiversityProjects.SetHelpNavigator(tableLayoutPanelSettings, System.Windows.Forms.HelpNavigator.KeywordIndex);
            helpProviderDiversityProjects.SetHelpString(tableLayoutPanelSettings, "Settings");
            tableLayoutPanelSettings.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelSettings.Name = "tableLayoutPanelSettings";
            tableLayoutPanelSettings.RowCount = 2;
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            helpProviderDiversityProjects.SetShowHelp(tableLayoutPanelSettings, true);
            tableLayoutPanelSettings.Size = new System.Drawing.Size(911, 284);
            tableLayoutPanelSettings.TabIndex = 0;
            // 
            // treeViewSettings
            // 
            tableLayoutPanelSettings.SetColumnSpan(treeViewSettings, 4);
            treeViewSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewSettings.Location = new System.Drawing.Point(4, 33);
            treeViewSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeViewSettings.Name = "treeViewSettings";
            treeViewSettings.Size = new System.Drawing.Size(903, 248);
            treeViewSettings.TabIndex = 0;
            treeViewSettings.AfterSelect += treeViewSettings_AfterSelect;
            // 
            // toolStripSettings
            // 
            toolStripSettings.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripSettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonSettingAdd, toolStripButtonSettingRemove, toolStripButtonSettingCopy, toolStripSeparatorSetting, toolStripButtonSettingEdit });
            toolStripSettings.Location = new System.Drawing.Point(0, 0);
            toolStripSettings.Name = "toolStripSettings";
            toolStripSettings.Size = new System.Drawing.Size(101, 25);
            toolStripSettings.TabIndex = 1;
            toolStripSettings.Text = "toolStrip1";
            // 
            // toolStripButtonSettingAdd
            // 
            toolStripButtonSettingAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSettingAdd.Image = Properties.Resources.Add;
            toolStripButtonSettingAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSettingAdd.Name = "toolStripButtonSettingAdd";
            toolStripButtonSettingAdd.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSettingAdd.Text = "Add a setting";
            toolStripButtonSettingAdd.Click += toolStripButtonSettingAdd_Click;
            // 
            // toolStripButtonSettingRemove
            // 
            toolStripButtonSettingRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSettingRemove.Image = Properties.Resources.Delete;
            toolStripButtonSettingRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSettingRemove.Name = "toolStripButtonSettingRemove";
            toolStripButtonSettingRemove.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSettingRemove.Text = "Remove the selected setting";
            toolStripButtonSettingRemove.Click += toolStripButtonSettingRemove_Click;
            // 
            // toolStripButtonSettingCopy
            // 
            toolStripButtonSettingCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSettingCopy.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonSettingCopy.Image");
            toolStripButtonSettingCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSettingCopy.Name = "toolStripButtonSettingCopy";
            toolStripButtonSettingCopy.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSettingCopy.Text = "Copy settings from another project";
            toolStripButtonSettingCopy.Click += toolStripButtonSettingCopy_Click;
            // 
            // toolStripSeparatorSetting
            // 
            toolStripSeparatorSetting.Name = "toolStripSeparatorSetting";
            toolStripSeparatorSetting.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSettingEdit
            // 
            toolStripButtonSettingEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSettingEdit.Image = Properties.Resources.Settings;
            toolStripButtonSettingEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSettingEdit.Name = "toolStripButtonSettingEdit";
            toolStripButtonSettingEdit.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSettingEdit.Text = "Edit the settings";
            toolStripButtonSettingEdit.Click += toolStripButtonSettingEdit_Click;
            // 
            // labelSettingValue
            // 
            labelSettingValue.AutoSize = true;
            labelSettingValue.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSettingValue.Location = new System.Drawing.Point(105, 0);
            labelSettingValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSettingValue.Name = "labelSettingValue";
            labelSettingValue.Size = new System.Drawing.Size(38, 30);
            labelSettingValue.TabIndex = 2;
            labelSettingValue.Text = "Value:";
            labelSettingValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            labelSettingValue.Visible = false;
            // 
            // textBoxSettingValue
            // 
            textBoxSettingValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectSettingBindingSource, "Value", true));
            textBoxSettingValue.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxSettingValue.Location = new System.Drawing.Point(147, 3);
            textBoxSettingValue.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            textBoxSettingValue.Name = "textBoxSettingValue";
            textBoxSettingValue.Size = new System.Drawing.Size(759, 23);
            textBoxSettingValue.TabIndex = 3;
            textBoxSettingValue.Visible = false;
            textBoxSettingValue.Leave += textBoxSettingValue_Leave;
            // 
            // projectSettingBindingSource
            // 
            projectSettingBindingSource.DataMember = "ProjectSetting";
            projectSettingBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // buttonSettingValueSave
            // 
            buttonSettingValueSave.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSettingValueSave.Location = new System.Drawing.Point(906, 3);
            buttonSettingValueSave.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            buttonSettingValueSave.Name = "buttonSettingValueSave";
            buttonSettingValueSave.Size = new System.Drawing.Size(1, 27);
            buttonSettingValueSave.TabIndex = 4;
            buttonSettingValueSave.UseVisualStyleBackColor = true;
            buttonSettingValueSave.Visible = false;
            buttonSettingValueSave.Click += buttonSettingValueSave_Click;
            // 
            // tabPageAgents
            // 
            tabPageAgents.Controls.Add(splitContainerAgents);
            helpProviderDiversityProjects.SetHelpKeyword(tabPageAgents, "Agents");
            helpProviderDiversityProjects.SetHelpNavigator(tabPageAgents, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tabPageAgents.ImageIndex = 0;
            tabPageAgents.Location = new System.Drawing.Point(4, 24);
            tabPageAgents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageAgents.Name = "tabPageAgents";
            tabPageAgents.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            helpProviderDiversityProjects.SetShowHelp(tabPageAgents, true);
            tabPageAgents.Size = new System.Drawing.Size(192, 72);
            tabPageAgents.TabIndex = 1;
            tabPageAgents.Text = "Agents";
            tabPageAgents.UseVisualStyleBackColor = true;
            // 
            // splitContainerAgents
            // 
            splitContainerAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerAgents.Location = new System.Drawing.Point(4, 3);
            splitContainerAgents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerAgents.Name = "splitContainerAgents";
            // 
            // splitContainerAgents.Panel1
            // 
            splitContainerAgents.Panel1.Controls.Add(listBoxAgents);
            splitContainerAgents.Panel1.Controls.Add(toolStripAgents);
            // 
            // splitContainerAgents.Panel2
            // 
            splitContainerAgents.Panel2.Controls.Add(tableLayoutPanelAgent);
            splitContainerAgents.Size = new System.Drawing.Size(184, 66);
            splitContainerAgents.SplitterDistance = 60;
            splitContainerAgents.SplitterWidth = 5;
            splitContainerAgents.TabIndex = 0;
            // 
            // listBoxAgents
            // 
            listBoxAgents.DataSource = projectAgentBindingSource;
            listBoxAgents.DisplayMember = "AgentName";
            listBoxAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxAgents.FormattingEnabled = true;
            listBoxAgents.IntegralHeight = false;
            listBoxAgents.ItemHeight = 15;
            listBoxAgents.Location = new System.Drawing.Point(0, 0);
            listBoxAgents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxAgents.Name = "listBoxAgents";
            listBoxAgents.Size = new System.Drawing.Size(60, 41);
            listBoxAgents.TabIndex = 1;
            listBoxAgents.ValueMember = "AgentURI";
            listBoxAgents.SelectedIndexChanged += listBoxAgents_SelectedIndexChanged;
            listBoxAgents.Validated += listBoxAgents_Validated;
            // 
            // projectAgentBindingSource
            // 
            projectAgentBindingSource.DataMember = "ProjectAgent";
            projectAgentBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // toolStripAgents
            // 
            toolStripAgents.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripAgents.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripAgents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonAgentNew, toolStripButtonAgentDelete, toolStripButtonAgentMoveToTop, toolStripButtonAgentMoveToBase });
            toolStripAgents.Location = new System.Drawing.Point(0, 41);
            toolStripAgents.Name = "toolStripAgents";
            toolStripAgents.Size = new System.Drawing.Size(60, 25);
            toolStripAgents.TabIndex = 0;
            toolStripAgents.Text = "Add a new agent";
            // 
            // toolStripButtonAgentNew
            // 
            toolStripButtonAgentNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonAgentNew.Image = Properties.Resources.Add;
            toolStripButtonAgentNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonAgentNew.Name = "toolStripButtonAgentNew";
            toolStripButtonAgentNew.Size = new System.Drawing.Size(23, 22);
            toolStripButtonAgentNew.Text = "Add a new agent";
            toolStripButtonAgentNew.Click += toolStripButtonAgentNew_Click;
            // 
            // toolStripButtonAgentDelete
            // 
            toolStripButtonAgentDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonAgentDelete.Image = Properties.Resources.Delete;
            toolStripButtonAgentDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonAgentDelete.Name = "toolStripButtonAgentDelete";
            toolStripButtonAgentDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonAgentDelete.Text = "Remove the selected agent";
            toolStripButtonAgentDelete.Click += toolStripButtonAgentDelete_Click;
            // 
            // toolStripButtonAgentMoveToTop
            // 
            toolStripButtonAgentMoveToTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonAgentMoveToTop.Image = Properties.Resources.ArrowUp;
            toolStripButtonAgentMoveToTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonAgentMoveToTop.Name = "toolStripButtonAgentMoveToTop";
            toolStripButtonAgentMoveToTop.Size = new System.Drawing.Size(23, 22);
            toolStripButtonAgentMoveToTop.Text = "Move the selected agent to the top of the list";
            toolStripButtonAgentMoveToTop.Click += toolStripButtonAgentMoveToTop_Click;
            // 
            // toolStripButtonAgentMoveToBase
            // 
            toolStripButtonAgentMoveToBase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonAgentMoveToBase.Image = Properties.Resources.ArrowDown;
            toolStripButtonAgentMoveToBase.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonAgentMoveToBase.Name = "toolStripButtonAgentMoveToBase";
            toolStripButtonAgentMoveToBase.Size = new System.Drawing.Size(23, 22);
            toolStripButtonAgentMoveToBase.Text = "Move the selected agent to the bottom of the list";
            toolStripButtonAgentMoveToBase.Click += toolStripButtonAgentMoveToBase_Click;
            // 
            // tableLayoutPanelAgent
            // 
            tableLayoutPanelAgent.ColumnCount = 4;
            tableLayoutPanelAgent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelAgent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelAgent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelAgent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelAgent.Controls.Add(labelAgentRole, 2, 2);
            tableLayoutPanelAgent.Controls.Add(labelAgentNotes, 0, 3);
            tableLayoutPanelAgent.Controls.Add(textBoxAgentNotes, 0, 5);
            tableLayoutPanelAgent.Controls.Add(labelAgentType, 0, 2);
            tableLayoutPanelAgent.Controls.Add(comboBoxAgentType, 1, 2);
            tableLayoutPanelAgent.Controls.Add(toolStripRoles, 2, 3);
            tableLayoutPanelAgent.Controls.Add(listBoxRoles, 3, 2);
            tableLayoutPanelAgent.Controls.Add(userControlModuleRelatedEntryProjectAgent, 0, 0);
            tableLayoutPanelAgent.Controls.Add(labelAgentRoleDisplayText, 3, 4);
            tableLayoutPanelAgent.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelAgent.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelAgent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelAgent.Name = "tableLayoutPanelAgent";
            tableLayoutPanelAgent.RowCount = 6;
            tableLayoutPanelAgent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelAgent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelAgent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelAgent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelAgent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            tableLayoutPanelAgent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelAgent.Size = new System.Drawing.Size(119, 66);
            tableLayoutPanelAgent.TabIndex = 0;
            // 
            // labelAgentRole
            // 
            labelAgentRole.AutoSize = true;
            labelAgentRole.Dock = System.Windows.Forms.DockStyle.Fill;
            labelAgentRole.Location = new System.Drawing.Point(61, 31);
            labelAgentRole.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelAgentRole.Name = "labelAgentRole";
            labelAgentRole.Size = new System.Drawing.Size(45, 29);
            labelAgentRole.TabIndex = 1;
            labelAgentRole.Text = "Roles";
            labelAgentRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAgentNotes
            // 
            labelAgentNotes.AutoSize = true;
            labelAgentNotes.Dock = System.Windows.Forms.DockStyle.Bottom;
            labelAgentNotes.Location = new System.Drawing.Point(4, 91);
            labelAgentNotes.Margin = new System.Windows.Forms.Padding(4, 7, 0, 0);
            labelAgentNotes.Name = "labelAgentNotes";
            tableLayoutPanelAgent.SetRowSpan(labelAgentNotes, 2);
            labelAgentNotes.Size = new System.Drawing.Size(41, 15);
            labelAgentNotes.TabIndex = 2;
            labelAgentNotes.Text = "Notes:";
            labelAgentNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxAgentNotes
            // 
            tableLayoutPanelAgent.SetColumnSpan(textBoxAgentNotes, 4);
            textBoxAgentNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectAgentBindingSource, "Notes", true));
            textBoxAgentNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxAgentNotes.Location = new System.Drawing.Point(4, 109);
            textBoxAgentNotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxAgentNotes.Multiline = true;
            textBoxAgentNotes.Name = "textBoxAgentNotes";
            textBoxAgentNotes.Size = new System.Drawing.Size(111, 1);
            textBoxAgentNotes.TabIndex = 4;
            // 
            // labelAgentType
            // 
            labelAgentType.AutoSize = true;
            labelAgentType.Dock = System.Windows.Forms.DockStyle.Fill;
            labelAgentType.Location = new System.Drawing.Point(4, 31);
            labelAgentType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelAgentType.Name = "labelAgentType";
            labelAgentType.Size = new System.Drawing.Size(37, 29);
            labelAgentType.TabIndex = 5;
            labelAgentType.Text = "Type:";
            labelAgentType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxAgentType
            // 
            comboBoxAgentType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", projectAgentBindingSource, "AgentType", true));
            comboBoxAgentType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxAgentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxAgentType.FormattingEnabled = true;
            comboBoxAgentType.Location = new System.Drawing.Point(49, 34);
            comboBoxAgentType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxAgentType.Name = "comboBoxAgentType";
            comboBoxAgentType.Size = new System.Drawing.Size(4, 23);
            comboBoxAgentType.TabIndex = 6;
            // 
            // toolStripRoles
            // 
            toolStripRoles.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripRoles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonRolesAdd, toolStripButtonRolesDelete });
            toolStripRoles.Location = new System.Drawing.Point(57, 60);
            toolStripRoles.Name = "toolStripRoles";
            tableLayoutPanelAgent.SetRowSpan(toolStripRoles, 2);
            toolStripRoles.Size = new System.Drawing.Size(49, 25);
            toolStripRoles.TabIndex = 7;
            toolStripRoles.Text = "toolStrip1";
            // 
            // toolStripButtonRolesAdd
            // 
            toolStripButtonRolesAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRolesAdd.Image = Properties.Resources.Add;
            toolStripButtonRolesAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRolesAdd.Name = "toolStripButtonRolesAdd";
            toolStripButtonRolesAdd.Size = new System.Drawing.Size(23, 22);
            toolStripButtonRolesAdd.Text = "Add a new role";
            toolStripButtonRolesAdd.Click += toolStripButtonRolesAdd_Click;
            // 
            // toolStripButtonRolesDelete
            // 
            toolStripButtonRolesDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRolesDelete.Image = Properties.Resources.Delete;
            toolStripButtonRolesDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRolesDelete.Name = "toolStripButtonRolesDelete";
            toolStripButtonRolesDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonRolesDelete.Text = "Remove the selected role";
            toolStripButtonRolesDelete.Click += toolStripButtonRolesDelete_Click;
            // 
            // listBoxRoles
            // 
            listBoxRoles.DataSource = projectAgentRoleBindingSource;
            listBoxRoles.DisplayMember = "AgentRole";
            listBoxRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxRoles.FormattingEnabled = true;
            listBoxRoles.IntegralHeight = false;
            listBoxRoles.ItemHeight = 15;
            listBoxRoles.Location = new System.Drawing.Point(106, 34);
            listBoxRoles.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            listBoxRoles.Name = "listBoxRoles";
            tableLayoutPanelAgent.SetRowSpan(listBoxRoles, 2);
            listBoxRoles.Size = new System.Drawing.Size(9, 57);
            listBoxRoles.TabIndex = 8;
            listBoxRoles.SelectedIndexChanged += listBoxRoles_SelectedIndexChanged;
            // 
            // projectAgentRoleBindingSource
            // 
            projectAgentRoleBindingSource.DataMember = "ProjectAgentRole";
            projectAgentRoleBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // userControlModuleRelatedEntryProjectAgent
            // 
            userControlModuleRelatedEntryProjectAgent.CanDeleteConnectionToModule = true;
            tableLayoutPanelAgent.SetColumnSpan(userControlModuleRelatedEntryProjectAgent, 4);
            userControlModuleRelatedEntryProjectAgent.DependsOnUri = "";
            userControlModuleRelatedEntryProjectAgent.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryProjectAgent.Domain = "";
            userControlModuleRelatedEntryProjectAgent.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryProjectAgent.Location = new System.Drawing.Point(5, 3);
            userControlModuleRelatedEntryProjectAgent.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleRelatedEntryProjectAgent.Module = null;
            userControlModuleRelatedEntryProjectAgent.Name = "userControlModuleRelatedEntryProjectAgent";
            userControlModuleRelatedEntryProjectAgent.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryProjectAgent.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryProjectAgent.ShowInfo = false;
            userControlModuleRelatedEntryProjectAgent.Size = new System.Drawing.Size(109, 25);
            userControlModuleRelatedEntryProjectAgent.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryProjectAgent.TabIndex = 9;
            // 
            // labelAgentRoleDisplayText
            // 
            labelAgentRoleDisplayText.AutoSize = true;
            labelAgentRoleDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            labelAgentRoleDisplayText.Location = new System.Drawing.Point(110, 91);
            labelAgentRoleDisplayText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelAgentRoleDisplayText.Name = "labelAgentRoleDisplayText";
            labelAgentRoleDisplayText.Size = new System.Drawing.Size(5, 15);
            labelAgentRoleDisplayText.TabIndex = 10;
            labelAgentRoleDisplayText.Text = "= Author";
            labelAgentRoleDisplayText.Visible = false;
            // 
            // tabPageLicenses
            // 
            tabPageLicenses.Controls.Add(splitContainerLicense);
            tabPageLicenses.ImageIndex = 3;
            tabPageLicenses.Location = new System.Drawing.Point(4, 24);
            tabPageLicenses.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageLicenses.Name = "tabPageLicenses";
            tabPageLicenses.Size = new System.Drawing.Size(192, 72);
            tabPageLicenses.TabIndex = 5;
            tabPageLicenses.Text = "IPR & Licenses";
            tabPageLicenses.UseVisualStyleBackColor = true;
            // 
            // splitContainerLicense
            // 
            splitContainerLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProviderDiversityProjects.SetHelpKeyword(splitContainerLicense, "License");
            helpProviderDiversityProjects.SetHelpNavigator(splitContainerLicense, System.Windows.Forms.HelpNavigator.KeywordIndex);
            helpProviderDiversityProjects.SetHelpString(splitContainerLicense, "License");
            splitContainerLicense.Location = new System.Drawing.Point(0, 0);
            splitContainerLicense.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerLicense.Name = "splitContainerLicense";
            // 
            // splitContainerLicense.Panel1
            // 
            splitContainerLicense.Panel1.Controls.Add(listBoxLicense);
            splitContainerLicense.Panel1.Controls.Add(toolStripLicense);
            // 
            // splitContainerLicense.Panel2
            // 
            splitContainerLicense.Panel2.Controls.Add(groupBoxLicenseNotes);
            splitContainerLicense.Panel2.Controls.Add(groupBoxCopyright);
            splitContainerLicense.Panel2.Controls.Add(groupBoxIPR);
            splitContainerLicense.Panel2.Controls.Add(groupBoxLicense);
            helpProviderDiversityProjects.SetShowHelp(splitContainerLicense, true);
            splitContainerLicense.Size = new System.Drawing.Size(192, 72);
            splitContainerLicense.SplitterDistance = 40;
            splitContainerLicense.SplitterWidth = 5;
            splitContainerLicense.TabIndex = 0;
            // 
            // listBoxLicense
            // 
            listBoxLicense.DataSource = projectLicenseBindingSource;
            listBoxLicense.DisplayMember = "DisplayText";
            listBoxLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxLicense.FormattingEnabled = true;
            listBoxLicense.ItemHeight = 15;
            listBoxLicense.Location = new System.Drawing.Point(0, 0);
            listBoxLicense.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxLicense.Name = "listBoxLicense";
            listBoxLicense.Size = new System.Drawing.Size(40, 47);
            listBoxLicense.TabIndex = 0;
            listBoxLicense.ValueMember = "LicenseID";
            listBoxLicense.SelectedIndexChanged += listBoxLicense_SelectedIndexChanged;
            // 
            // projectLicenseBindingSource
            // 
            projectLicenseBindingSource.DataMember = "ProjectLicense";
            projectLicenseBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // toolStripLicense
            // 
            toolStripLicense.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripLicense.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripLicense.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonLicenseAdd, toolStripButtonLicenseRemove });
            toolStripLicense.Location = new System.Drawing.Point(0, 47);
            toolStripLicense.Name = "toolStripLicense";
            toolStripLicense.Size = new System.Drawing.Size(40, 25);
            toolStripLicense.TabIndex = 1;
            toolStripLicense.Text = "toolStrip1";
            // 
            // toolStripButtonLicenseAdd
            // 
            toolStripButtonLicenseAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonLicenseAdd.Image = Properties.Resources.Add;
            toolStripButtonLicenseAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonLicenseAdd.Name = "toolStripButtonLicenseAdd";
            toolStripButtonLicenseAdd.Size = new System.Drawing.Size(23, 22);
            toolStripButtonLicenseAdd.Text = "Add a new license";
            toolStripButtonLicenseAdd.Click += toolStripButtonLicenseAdd_Click;
            // 
            // toolStripButtonLicenseRemove
            // 
            toolStripButtonLicenseRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonLicenseRemove.Image = Properties.Resources.Delete;
            toolStripButtonLicenseRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonLicenseRemove.Name = "toolStripButtonLicenseRemove";
            toolStripButtonLicenseRemove.Size = new System.Drawing.Size(23, 22);
            toolStripButtonLicenseRemove.Text = "Remove the selected license";
            toolStripButtonLicenseRemove.Click += toolStripButtonLicenseRemove_Click;
            // 
            // groupBoxLicenseNotes
            // 
            groupBoxLicenseNotes.Controls.Add(textBoxLicenseNotes);
            groupBoxLicenseNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxLicenseNotes.Enabled = false;
            groupBoxLicenseNotes.Location = new System.Drawing.Point(0, 248);
            groupBoxLicenseNotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxLicenseNotes.Name = "groupBoxLicenseNotes";
            groupBoxLicenseNotes.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxLicenseNotes.Size = new System.Drawing.Size(147, 0);
            groupBoxLicenseNotes.TabIndex = 4;
            groupBoxLicenseNotes.TabStop = false;
            groupBoxLicenseNotes.Text = "Notes";
            // 
            // textBoxLicenseNotes
            // 
            textBoxLicenseNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectLicenseBindingSource, "Notes", true));
            textBoxLicenseNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxLicenseNotes.Location = new System.Drawing.Point(4, 19);
            textBoxLicenseNotes.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            textBoxLicenseNotes.Multiline = true;
            textBoxLicenseNotes.Name = "textBoxLicenseNotes";
            textBoxLicenseNotes.Size = new System.Drawing.Size(139, 0);
            textBoxLicenseNotes.TabIndex = 13;
            // 
            // groupBoxCopyright
            // 
            groupBoxCopyright.Controls.Add(tableLayoutPanelCopyrightHolder);
            groupBoxCopyright.Dock = System.Windows.Forms.DockStyle.Top;
            groupBoxCopyright.Enabled = false;
            groupBoxCopyright.Location = new System.Drawing.Point(0, 201);
            groupBoxCopyright.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxCopyright.Name = "groupBoxCopyright";
            groupBoxCopyright.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxCopyright.Size = new System.Drawing.Size(147, 47);
            groupBoxCopyright.TabIndex = 3;
            groupBoxCopyright.TabStop = false;
            groupBoxCopyright.Text = "Copyright";
            // 
            // tableLayoutPanelCopyrightHolder
            // 
            tableLayoutPanelCopyrightHolder.ColumnCount = 3;
            tableLayoutPanelCopyrightHolder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelCopyrightHolder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelCopyrightHolder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelCopyrightHolder.Controls.Add(userControlModuleRelatedEntryCopyrightHolder, 2, 0);
            tableLayoutPanelCopyrightHolder.Controls.Add(pictureBoxCopyrightHolder, 1, 0);
            tableLayoutPanelCopyrightHolder.Controls.Add(textBoxCopyright, 0, 0);
            tableLayoutPanelCopyrightHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelCopyrightHolder.Location = new System.Drawing.Point(4, 19);
            tableLayoutPanelCopyrightHolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelCopyrightHolder.Name = "tableLayoutPanelCopyrightHolder";
            tableLayoutPanelCopyrightHolder.RowCount = 1;
            tableLayoutPanelCopyrightHolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelCopyrightHolder.Size = new System.Drawing.Size(139, 25);
            tableLayoutPanelCopyrightHolder.TabIndex = 30;
            // 
            // userControlModuleRelatedEntryCopyrightHolder
            // 
            userControlModuleRelatedEntryCopyrightHolder.CanDeleteConnectionToModule = true;
            userControlModuleRelatedEntryCopyrightHolder.DependsOnUri = "";
            userControlModuleRelatedEntryCopyrightHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryCopyrightHolder.Domain = "";
            userControlModuleRelatedEntryCopyrightHolder.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryCopyrightHolder.Location = new System.Drawing.Point(80, 0);
            userControlModuleRelatedEntryCopyrightHolder.Margin = new System.Windows.Forms.Padding(0);
            userControlModuleRelatedEntryCopyrightHolder.Module = null;
            userControlModuleRelatedEntryCopyrightHolder.Name = "userControlModuleRelatedEntryCopyrightHolder";
            userControlModuleRelatedEntryCopyrightHolder.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryCopyrightHolder.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryCopyrightHolder.ShowInfo = false;
            userControlModuleRelatedEntryCopyrightHolder.Size = new System.Drawing.Size(59, 25);
            userControlModuleRelatedEntryCopyrightHolder.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryCopyrightHolder.TabIndex = 0;
            // 
            // pictureBoxCopyrightHolder
            // 
            pictureBoxCopyrightHolder.Image = Properties.Resources.Agent;
            pictureBoxCopyrightHolder.Location = new System.Drawing.Point(58, 0);
            pictureBoxCopyrightHolder.Margin = new System.Windows.Forms.Padding(0);
            pictureBoxCopyrightHolder.Name = "pictureBoxCopyrightHolder";
            pictureBoxCopyrightHolder.Padding = new System.Windows.Forms.Padding(4, 3, 0, 0);
            pictureBoxCopyrightHolder.Size = new System.Drawing.Size(22, 22);
            pictureBoxCopyrightHolder.TabIndex = 29;
            pictureBoxCopyrightHolder.TabStop = false;
            // 
            // textBoxCopyright
            // 
            textBoxCopyright.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectLicenseBindingSource, "CopyrightStatement", true));
            textBoxCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxCopyright.Location = new System.Drawing.Point(0, 0);
            textBoxCopyright.Margin = new System.Windows.Forms.Padding(0);
            textBoxCopyright.Name = "textBoxCopyright";
            textBoxCopyright.Size = new System.Drawing.Size(58, 23);
            textBoxCopyright.TabIndex = 11;
            // 
            // groupBoxIPR
            // 
            groupBoxIPR.Controls.Add(tableLayoutPanelIPR);
            groupBoxIPR.Dock = System.Windows.Forms.DockStyle.Top;
            groupBoxIPR.Enabled = false;
            groupBoxIPR.Location = new System.Drawing.Point(0, 153);
            groupBoxIPR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxIPR.Name = "groupBoxIPR";
            groupBoxIPR.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxIPR.Size = new System.Drawing.Size(147, 48);
            groupBoxIPR.TabIndex = 2;
            groupBoxIPR.TabStop = false;
            groupBoxIPR.Text = "IPR";
            // 
            // tableLayoutPanelIPR
            // 
            tableLayoutPanelIPR.ColumnCount = 3;
            tableLayoutPanelIPR.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelIPR.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelIPR.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelIPR.Controls.Add(textBoxIPR, 0, 0);
            tableLayoutPanelIPR.Controls.Add(pictureBoxIPRHolder, 1, 0);
            tableLayoutPanelIPR.Controls.Add(userControlModuleRelatedEntryIPRHolder, 2, 0);
            tableLayoutPanelIPR.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelIPR.Location = new System.Drawing.Point(4, 19);
            tableLayoutPanelIPR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelIPR.Name = "tableLayoutPanelIPR";
            tableLayoutPanelIPR.RowCount = 1;
            tableLayoutPanelIPR.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelIPR.Size = new System.Drawing.Size(139, 26);
            tableLayoutPanelIPR.TabIndex = 29;
            // 
            // textBoxIPR
            // 
            textBoxIPR.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectLicenseBindingSource, "IPR", true));
            textBoxIPR.Location = new System.Drawing.Point(0, 0);
            textBoxIPR.Margin = new System.Windows.Forms.Padding(0);
            textBoxIPR.Name = "textBoxIPR";
            textBoxIPR.Size = new System.Drawing.Size(58, 23);
            textBoxIPR.TabIndex = 9;
            // 
            // pictureBoxIPRHolder
            // 
            pictureBoxIPRHolder.Image = Properties.Resources.Agent;
            pictureBoxIPRHolder.Location = new System.Drawing.Point(58, 0);
            pictureBoxIPRHolder.Margin = new System.Windows.Forms.Padding(0);
            pictureBoxIPRHolder.Name = "pictureBoxIPRHolder";
            pictureBoxIPRHolder.Padding = new System.Windows.Forms.Padding(4, 3, 0, 0);
            pictureBoxIPRHolder.Size = new System.Drawing.Size(22, 23);
            pictureBoxIPRHolder.TabIndex = 28;
            pictureBoxIPRHolder.TabStop = false;
            // 
            // userControlModuleRelatedEntryIPRHolder
            // 
            userControlModuleRelatedEntryIPRHolder.CanDeleteConnectionToModule = true;
            userControlModuleRelatedEntryIPRHolder.DependsOnUri = "";
            userControlModuleRelatedEntryIPRHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryIPRHolder.Domain = "";
            userControlModuleRelatedEntryIPRHolder.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryIPRHolder.Location = new System.Drawing.Point(80, 0);
            userControlModuleRelatedEntryIPRHolder.Margin = new System.Windows.Forms.Padding(0);
            userControlModuleRelatedEntryIPRHolder.Module = null;
            userControlModuleRelatedEntryIPRHolder.Name = "userControlModuleRelatedEntryIPRHolder";
            userControlModuleRelatedEntryIPRHolder.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryIPRHolder.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryIPRHolder.ShowInfo = false;
            userControlModuleRelatedEntryIPRHolder.Size = new System.Drawing.Size(59, 26);
            userControlModuleRelatedEntryIPRHolder.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryIPRHolder.TabIndex = 29;
            // 
            // groupBoxLicense
            // 
            groupBoxLicense.Controls.Add(tableLayoutPanelLicense);
            groupBoxLicense.Dock = System.Windows.Forms.DockStyle.Top;
            groupBoxLicense.Enabled = false;
            groupBoxLicense.Location = new System.Drawing.Point(0, 0);
            groupBoxLicense.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxLicense.Name = "groupBoxLicense";
            groupBoxLicense.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxLicense.Size = new System.Drawing.Size(147, 153);
            groupBoxLicense.TabIndex = 1;
            groupBoxLicense.TabStop = false;
            groupBoxLicense.Text = "License";
            // 
            // tableLayoutPanelLicense
            // 
            tableLayoutPanelLicense.ColumnCount = 5;
            tableLayoutPanelLicense.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelLicense.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47F));
            tableLayoutPanelLicense.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelLicense.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53F));
            tableLayoutPanelLicense.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelLicense.Controls.Add(labelLicenseYear, 0, 4);
            tableLayoutPanelLicense.Controls.Add(textBoxLicenseYear, 0, 5);
            tableLayoutPanelLicense.Controls.Add(labelLicenseType, 3, 4);
            tableLayoutPanelLicense.Controls.Add(comboBoxLicenseContext, 1, 5);
            tableLayoutPanelLicense.Controls.Add(labelLicenseContext, 1, 4);
            tableLayoutPanelLicense.Controls.Add(comboBoxLicenseType, 3, 5);
            tableLayoutPanelLicense.Controls.Add(textBoxLicenseDisplayText, 0, 0);
            tableLayoutPanelLicense.Controls.Add(buttonLicenseURI, 4, 6);
            tableLayoutPanelLicense.Controls.Add(textBoxLicenseURI, 1, 6);
            tableLayoutPanelLicense.Controls.Add(labelLicenseURI, 0, 6);
            tableLayoutPanelLicense.Controls.Add(labelLicenseDetails, 0, 7);
            tableLayoutPanelLicense.Controls.Add(textBoxLicenseDetails, 1, 7);
            tableLayoutPanelLicense.Controls.Add(pictureBoxLicenseHolder, 2, 7);
            tableLayoutPanelLicense.Controls.Add(textBoxLicenseID, 4, 0);
            tableLayoutPanelLicense.Controls.Add(userControlModuleRelatedEntryLicenseHolder, 3, 7);
            tableLayoutPanelLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelLicense.Enabled = false;
            tableLayoutPanelLicense.Location = new System.Drawing.Point(4, 19);
            tableLayoutPanelLicense.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelLicense.Name = "tableLayoutPanelLicense";
            tableLayoutPanelLicense.RowCount = 11;
            tableLayoutPanelLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLicense.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelLicense.Size = new System.Drawing.Size(139, 131);
            tableLayoutPanelLicense.TabIndex = 0;
            // 
            // labelLicenseYear
            // 
            labelLicenseYear.AutoSize = true;
            labelLicenseYear.Dock = System.Windows.Forms.DockStyle.Fill;
            labelLicenseYear.Location = new System.Drawing.Point(4, 29);
            labelLicenseYear.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelLicenseYear.Name = "labelLicenseYear";
            labelLicenseYear.Size = new System.Drawing.Size(62, 105);
            labelLicenseYear.TabIndex = 4;
            labelLicenseYear.Text = "Year:";
            labelLicenseYear.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // textBoxLicenseYear
            // 
            textBoxLicenseYear.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectLicenseBindingSource, "LicenseYear", true));
            textBoxLicenseYear.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxLicenseYear.Location = new System.Drawing.Point(4, 134);
            textBoxLicenseYear.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            textBoxLicenseYear.Name = "textBoxLicenseYear";
            textBoxLicenseYear.Size = new System.Drawing.Size(62, 23);
            textBoxLicenseYear.TabIndex = 5;
            // 
            // labelLicenseType
            // 
            labelLicenseType.AutoSize = true;
            tableLayoutPanelLicense.SetColumnSpan(labelLicenseType, 2);
            labelLicenseType.Dock = System.Windows.Forms.DockStyle.Fill;
            labelLicenseType.Location = new System.Drawing.Point(98, 29);
            labelLicenseType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelLicenseType.Name = "labelLicenseType";
            labelLicenseType.Size = new System.Drawing.Size(37, 105);
            labelLicenseType.TabIndex = 6;
            labelLicenseType.Text = "Type:";
            labelLicenseType.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // comboBoxLicenseContext
            // 
            tableLayoutPanelLicense.SetColumnSpan(comboBoxLicenseContext, 2);
            comboBoxLicenseContext.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectLicenseBindingSource, "Context", true));
            comboBoxLicenseContext.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxLicenseContext.FormattingEnabled = true;
            comboBoxLicenseContext.Location = new System.Drawing.Point(74, 134);
            comboBoxLicenseContext.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            comboBoxLicenseContext.Name = "comboBoxLicenseContext";
            comboBoxLicenseContext.Size = new System.Drawing.Size(16, 23);
            comboBoxLicenseContext.TabIndex = 14;
            comboBoxLicenseContext.DropDown += comboBoxLicenseContext_DropDown;
            // 
            // labelLicenseContext
            // 
            labelLicenseContext.AutoSize = true;
            tableLayoutPanelLicense.SetColumnSpan(labelLicenseContext, 2);
            labelLicenseContext.Dock = System.Windows.Forms.DockStyle.Fill;
            labelLicenseContext.Location = new System.Drawing.Point(74, 29);
            labelLicenseContext.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelLicenseContext.Name = "labelLicenseContext";
            labelLicenseContext.Size = new System.Drawing.Size(16, 105);
            labelLicenseContext.TabIndex = 15;
            labelLicenseContext.Text = "Context:";
            labelLicenseContext.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // comboBoxLicenseType
            // 
            tableLayoutPanelLicense.SetColumnSpan(comboBoxLicenseType, 2);
            comboBoxLicenseType.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectLicenseBindingSource, "LicenseType", true));
            comboBoxLicenseType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxLicenseType.FormattingEnabled = true;
            comboBoxLicenseType.Location = new System.Drawing.Point(98, 134);
            comboBoxLicenseType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            comboBoxLicenseType.Name = "comboBoxLicenseType";
            comboBoxLicenseType.Size = new System.Drawing.Size(37, 23);
            comboBoxLicenseType.TabIndex = 16;
            comboBoxLicenseType.DropDown += comboBoxLicenseType_DropDown;
            // 
            // textBoxLicenseDisplayText
            // 
            tableLayoutPanelLicense.SetColumnSpan(textBoxLicenseDisplayText, 4);
            textBoxLicenseDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectLicenseBindingSource, "DisplayText", true));
            textBoxLicenseDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxLicenseDisplayText.Location = new System.Drawing.Point(4, 3);
            textBoxLicenseDisplayText.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            textBoxLicenseDisplayText.Name = "textBoxLicenseDisplayText";
            textBoxLicenseDisplayText.Size = new System.Drawing.Size(96, 23);
            textBoxLicenseDisplayText.TabIndex = 17;
            // 
            // buttonLicenseURI
            // 
            buttonLicenseURI.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonLicenseURI.Image = Properties.Resources.Browse;
            buttonLicenseURI.Location = new System.Drawing.Point(100, 161);
            buttonLicenseURI.Margin = new System.Windows.Forms.Padding(0, 1, 4, 1);
            buttonLicenseURI.Name = "buttonLicenseURI";
            buttonLicenseURI.Size = new System.Drawing.Size(35, 31);
            buttonLicenseURI.TabIndex = 26;
            toolTip.SetToolTip(buttonLicenseURI, "Search for the website containing the license of the project");
            buttonLicenseURI.UseVisualStyleBackColor = true;
            buttonLicenseURI.Click += buttonLicenseURI_Click;
            // 
            // textBoxLicenseURI
            // 
            tableLayoutPanelLicense.SetColumnSpan(textBoxLicenseURI, 3);
            textBoxLicenseURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectLicenseBindingSource, "LicenseURI", true));
            textBoxLicenseURI.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxLicenseURI.Location = new System.Drawing.Point(74, 163);
            textBoxLicenseURI.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            textBoxLicenseURI.Name = "textBoxLicenseURI";
            textBoxLicenseURI.Size = new System.Drawing.Size(26, 23);
            textBoxLicenseURI.TabIndex = 25;
            // 
            // labelLicenseURI
            // 
            labelLicenseURI.AutoSize = true;
            labelLicenseURI.Dock = System.Windows.Forms.DockStyle.Fill;
            labelLicenseURI.Location = new System.Drawing.Point(4, 160);
            labelLicenseURI.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelLicenseURI.Name = "labelLicenseURI";
            labelLicenseURI.Size = new System.Drawing.Size(66, 33);
            labelLicenseURI.TabIndex = 24;
            labelLicenseURI.Text = "URI:";
            labelLicenseURI.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLicenseDetails
            // 
            labelLicenseDetails.AutoSize = true;
            labelLicenseDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            labelLicenseDetails.Location = new System.Drawing.Point(4, 193);
            labelLicenseDetails.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelLicenseDetails.Name = "labelLicenseDetails";
            labelLicenseDetails.Size = new System.Drawing.Size(66, 36);
            labelLicenseDetails.TabIndex = 31;
            labelLicenseDetails.Text = "Details:";
            labelLicenseDetails.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLicenseDetails
            // 
            textBoxLicenseDetails.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectLicenseBindingSource, "LicenseDetails", true));
            textBoxLicenseDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxLicenseDetails.Location = new System.Drawing.Point(74, 196);
            textBoxLicenseDetails.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            textBoxLicenseDetails.Name = "textBoxLicenseDetails";
            textBoxLicenseDetails.Size = new System.Drawing.Size(1, 23);
            textBoxLicenseDetails.TabIndex = 32;
            // 
            // pictureBoxLicenseHolder
            // 
            pictureBoxLicenseHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxLicenseHolder.Image = Properties.Resources.Agent;
            pictureBoxLicenseHolder.Location = new System.Drawing.Point(75, 200);
            pictureBoxLicenseHolder.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            pictureBoxLicenseHolder.Name = "pictureBoxLicenseHolder";
            pictureBoxLicenseHolder.Size = new System.Drawing.Size(19, 29);
            pictureBoxLicenseHolder.TabIndex = 33;
            pictureBoxLicenseHolder.TabStop = false;
            // 
            // textBoxLicenseID
            // 
            textBoxLicenseID.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectLicenseBindingSource, "LicenseID", true));
            textBoxLicenseID.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxLicenseID.Location = new System.Drawing.Point(100, 3);
            textBoxLicenseID.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            textBoxLicenseID.Name = "textBoxLicenseID";
            textBoxLicenseID.ReadOnly = true;
            textBoxLicenseID.Size = new System.Drawing.Size(35, 23);
            textBoxLicenseID.TabIndex = 34;
            // 
            // userControlModuleRelatedEntryLicenseHolder
            // 
            userControlModuleRelatedEntryLicenseHolder.CanDeleteConnectionToModule = true;
            tableLayoutPanelLicense.SetColumnSpan(userControlModuleRelatedEntryLicenseHolder, 2);
            userControlModuleRelatedEntryLicenseHolder.DependsOnUri = "";
            userControlModuleRelatedEntryLicenseHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryLicenseHolder.Domain = "";
            userControlModuleRelatedEntryLicenseHolder.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryLicenseHolder.Location = new System.Drawing.Point(99, 196);
            userControlModuleRelatedEntryLicenseHolder.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleRelatedEntryLicenseHolder.Module = null;
            userControlModuleRelatedEntryLicenseHolder.Name = "userControlModuleRelatedEntryLicenseHolder";
            userControlModuleRelatedEntryLicenseHolder.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryLicenseHolder.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryLicenseHolder.ShowInfo = false;
            userControlModuleRelatedEntryLicenseHolder.Size = new System.Drawing.Size(35, 30);
            userControlModuleRelatedEntryLicenseHolder.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryLicenseHolder.TabIndex = 35;
            // 
            // tabPageResources
            // 
            tabPageResources.Controls.Add(splitContainerResources);
            tabPageResources.ImageIndex = 8;
            tabPageResources.Location = new System.Drawing.Point(4, 24);
            tabPageResources.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageResources.Name = "tabPageResources";
            tabPageResources.Size = new System.Drawing.Size(192, 72);
            tabPageResources.TabIndex = 2;
            tabPageResources.Text = "Resources";
            tabPageResources.UseVisualStyleBackColor = true;
            // 
            // splitContainerResources
            // 
            splitContainerResources.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProviderDiversityProjects.SetHelpKeyword(splitContainerResources, "Resources");
            helpProviderDiversityProjects.SetHelpNavigator(splitContainerResources, System.Windows.Forms.HelpNavigator.KeywordIndex);
            splitContainerResources.Location = new System.Drawing.Point(0, 0);
            splitContainerResources.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerResources.Name = "splitContainerResources";
            // 
            // splitContainerResources.Panel1
            // 
            splitContainerResources.Panel1.Controls.Add(listBoxResources);
            splitContainerResources.Panel1.Controls.Add(toolStripResources);
            // 
            // splitContainerResources.Panel2
            // 
            splitContainerResources.Panel2.Controls.Add(tableLayoutPanelResources);
            helpProviderDiversityProjects.SetShowHelp(splitContainerResources, true);
            splitContainerResources.Size = new System.Drawing.Size(192, 72);
            splitContainerResources.SplitterDistance = 41;
            splitContainerResources.SplitterWidth = 5;
            splitContainerResources.TabIndex = 0;
            // 
            // listBoxResources
            // 
            listBoxResources.DataSource = projectResourceBindingSource;
            listBoxResources.DisplayMember = "URI";
            listBoxResources.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxResources.FormattingEnabled = true;
            listBoxResources.IntegralHeight = false;
            listBoxResources.ItemHeight = 15;
            listBoxResources.Location = new System.Drawing.Point(0, 0);
            listBoxResources.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxResources.Name = "listBoxResources";
            listBoxResources.Size = new System.Drawing.Size(41, 47);
            listBoxResources.TabIndex = 1;
            listBoxResources.ValueMember = "URI";
            listBoxResources.SelectedIndexChanged += listBoxResources_SelectedIndexChanged;
            // 
            // projectResourceBindingSource
            // 
            projectResourceBindingSource.DataMember = "ProjectResource";
            projectResourceBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // toolStripResources
            // 
            toolStripResources.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripResources.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripResources.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonResourcesNew, toolStripButtonResourceAddLink, toolStripButtonResourcesDelete, toolStripButtonResourceOpen });
            toolStripResources.Location = new System.Drawing.Point(0, 47);
            toolStripResources.Name = "toolStripResources";
            toolStripResources.Size = new System.Drawing.Size(41, 25);
            toolStripResources.TabIndex = 0;
            toolStripResources.Text = "Add a new resource";
            // 
            // toolStripButtonResourcesNew
            // 
            toolStripButtonResourcesNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonResourcesNew.Image = Properties.Resources.Add;
            toolStripButtonResourcesNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonResourcesNew.Name = "toolStripButtonResourcesNew";
            toolStripButtonResourcesNew.Size = new System.Drawing.Size(23, 22);
            toolStripButtonResourcesNew.Text = "Insert a new resource";
            toolStripButtonResourcesNew.Click += toolStripButtonResourcesNew_Click;
            // 
            // toolStripButtonResourceAddLink
            // 
            toolStripButtonResourceAddLink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonResourceAddLink.Image = Properties.Resources.AddLink;
            toolStripButtonResourceAddLink.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonResourceAddLink.Name = "toolStripButtonResourceAddLink";
            toolStripButtonResourceAddLink.Size = new System.Drawing.Size(23, 22);
            toolStripButtonResourceAddLink.Text = "Add a link";
            toolStripButtonResourceAddLink.Click += toolStripButtonResourceAddLink_Click;
            // 
            // toolStripButtonResourcesDelete
            // 
            toolStripButtonResourcesDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonResourcesDelete.Image = Properties.Resources.Delete;
            toolStripButtonResourcesDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonResourcesDelete.Name = "toolStripButtonResourcesDelete";
            toolStripButtonResourcesDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonResourcesDelete.Text = "Delete the selected resource";
            toolStripButtonResourcesDelete.Click += toolStripButtonResourcesDelete_Click;
            // 
            // toolStripButtonResourceOpen
            // 
            toolStripButtonResourceOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonResourceOpen.Image = Properties.Resources.OpenFolder;
            toolStripButtonResourceOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonResourceOpen.Name = "toolStripButtonResourceOpen";
            toolStripButtonResourceOpen.Size = new System.Drawing.Size(23, 22);
            toolStripButtonResourceOpen.Text = "Open the selected resource";
            toolStripButtonResourceOpen.Visible = false;
            toolStripButtonResourceOpen.Click += toolStripButtonResourceOpen_Click;
            // 
            // tableLayoutPanelResources
            // 
            tableLayoutPanelResources.ColumnCount = 3;
            tableLayoutPanelResources.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelResources.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelResources.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelResources.Controls.Add(labelFileName, 0, 0);
            tableLayoutPanelResources.Controls.Add(textBoxResourceNotes, 1, 4);
            tableLayoutPanelResources.Controls.Add(pictureBoxResource, 0, 1);
            tableLayoutPanelResources.Controls.Add(labelResourceNotes, 0, 4);
            tableLayoutPanelResources.Controls.Add(labelResourceRights, 0, 2);
            tableLayoutPanelResources.Controls.Add(textBoxResourceRights, 1, 2);
            tableLayoutPanelResources.Controls.Add(labelResourceLicense, 0, 3);
            tableLayoutPanelResources.Controls.Add(textBoxResourceLicense, 1, 3);
            tableLayoutPanelResources.Controls.Add(buttonResourceLicense, 2, 3);
            tableLayoutPanelResources.Controls.Add(buttonResourceOpen, 2, 0);
            tableLayoutPanelResources.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelResources.Enabled = false;
            helpProviderDiversityProjects.SetHelpKeyword(tableLayoutPanelResources, "Resources");
            helpProviderDiversityProjects.SetHelpNavigator(tableLayoutPanelResources, System.Windows.Forms.HelpNavigator.KeywordIndex);
            helpProviderDiversityProjects.SetHelpString(tableLayoutPanelResources, "Resources");
            tableLayoutPanelResources.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelResources.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelResources.Name = "tableLayoutPanelResources";
            tableLayoutPanelResources.RowCount = 5;
            tableLayoutPanelResources.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelResources.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelResources.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelResources.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelResources.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            helpProviderDiversityProjects.SetShowHelp(tableLayoutPanelResources, true);
            tableLayoutPanelResources.Size = new System.Drawing.Size(146, 72);
            tableLayoutPanelResources.TabIndex = 0;
            // 
            // labelFileName
            // 
            labelFileName.AutoSize = true;
            tableLayoutPanelResources.SetColumnSpan(labelFileName, 2);
            labelFileName.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectResourceBindingSource, "URI", true));
            labelFileName.Dock = System.Windows.Forms.DockStyle.Top;
            labelFileName.Location = new System.Drawing.Point(4, 7);
            labelFileName.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            labelFileName.Name = "labelFileName";
            labelFileName.Size = new System.Drawing.Size(106, 15);
            labelFileName.TabIndex = 1;
            labelFileName.Text = "Resource";
            // 
            // textBoxResourceNotes
            // 
            tableLayoutPanelResources.SetColumnSpan(textBoxResourceNotes, 2);
            textBoxResourceNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectResourceBindingSource, "Notes", true));
            textBoxResourceNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxResourceNotes.Location = new System.Drawing.Point(61, 103);
            textBoxResourceNotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxResourceNotes.Multiline = true;
            textBoxResourceNotes.Name = "textBoxResourceNotes";
            textBoxResourceNotes.Size = new System.Drawing.Size(81, 1);
            textBoxResourceNotes.TabIndex = 2;
            // 
            // pictureBoxResource
            // 
            tableLayoutPanelResources.SetColumnSpan(pictureBoxResource, 3);
            pictureBoxResource.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxResource.Location = new System.Drawing.Point(4, 34);
            pictureBoxResource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxResource.Name = "pictureBoxResource";
            pictureBoxResource.Size = new System.Drawing.Size(138, 1);
            pictureBoxResource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBoxResource.TabIndex = 0;
            pictureBoxResource.TabStop = false;
            pictureBoxResource.DoubleClick += pictureBoxResource_DoubleClick;
            // 
            // labelResourceNotes
            // 
            labelResourceNotes.AutoSize = true;
            labelResourceNotes.Dock = System.Windows.Forms.DockStyle.Top;
            labelResourceNotes.Location = new System.Drawing.Point(4, 107);
            labelResourceNotes.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            labelResourceNotes.Name = "labelResourceNotes";
            labelResourceNotes.Size = new System.Drawing.Size(49, 1);
            labelResourceNotes.TabIndex = 3;
            labelResourceNotes.Text = "Notes:";
            // 
            // labelResourceRights
            // 
            labelResourceRights.AutoSize = true;
            labelResourceRights.Dock = System.Windows.Forms.DockStyle.Fill;
            labelResourceRights.Location = new System.Drawing.Point(4, 38);
            labelResourceRights.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelResourceRights.Name = "labelResourceRights";
            labelResourceRights.Size = new System.Drawing.Size(49, 29);
            labelResourceRights.TabIndex = 4;
            labelResourceRights.Text = "Rights:";
            labelResourceRights.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxResourceRights
            // 
            tableLayoutPanelResources.SetColumnSpan(textBoxResourceRights, 2);
            textBoxResourceRights.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectResourceBindingSource, "SpecificRights", true));
            textBoxResourceRights.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxResourceRights.Location = new System.Drawing.Point(61, 41);
            textBoxResourceRights.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxResourceRights.Name = "textBoxResourceRights";
            textBoxResourceRights.Size = new System.Drawing.Size(81, 23);
            textBoxResourceRights.TabIndex = 5;
            // 
            // labelResourceLicense
            // 
            labelResourceLicense.AutoSize = true;
            labelResourceLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            labelResourceLicense.Location = new System.Drawing.Point(4, 67);
            labelResourceLicense.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelResourceLicense.Name = "labelResourceLicense";
            labelResourceLicense.Size = new System.Drawing.Size(49, 33);
            labelResourceLicense.TabIndex = 6;
            labelResourceLicense.Text = "License:";
            labelResourceLicense.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxResourceLicense
            // 
            textBoxResourceLicense.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectResourceBindingSource, "SpecificLicenseURI", true));
            textBoxResourceLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxResourceLicense.Location = new System.Drawing.Point(61, 70);
            textBoxResourceLicense.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            textBoxResourceLicense.Name = "textBoxResourceLicense";
            textBoxResourceLicense.Size = new System.Drawing.Size(53, 23);
            textBoxResourceLicense.TabIndex = 7;
            // 
            // buttonResourceLicense
            // 
            buttonResourceLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonResourceLicense.Image = Properties.Resources.Browse;
            buttonResourceLicense.Location = new System.Drawing.Point(114, 67);
            buttonResourceLicense.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonResourceLicense.Name = "buttonResourceLicense";
            buttonResourceLicense.Size = new System.Drawing.Size(28, 33);
            buttonResourceLicense.TabIndex = 8;
            toolTip.SetToolTip(buttonResourceLicense, "Search for the website containing the license of the resource");
            buttonResourceLicense.UseVisualStyleBackColor = true;
            buttonResourceLicense.Click += buttonResourceLicense_Click;
            // 
            // buttonResourceOpen
            // 
            buttonResourceOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonResourceOpen.Image = Properties.Resources.Lupe;
            buttonResourceOpen.Location = new System.Drawing.Point(114, 2);
            buttonResourceOpen.Margin = new System.Windows.Forms.Padding(0, 2, 4, 0);
            buttonResourceOpen.Name = "buttonResourceOpen";
            buttonResourceOpen.Size = new System.Drawing.Size(28, 29);
            buttonResourceOpen.TabIndex = 9;
            toolTip.SetToolTip(buttonResourceOpen, "Open the resource");
            buttonResourceOpen.UseVisualStyleBackColor = true;
            buttonResourceOpen.Click += buttonResourceOpen_Click;
            // 
            // tabPageReferences
            // 
            tabPageReferences.Controls.Add(splitContainerReference);
            tabPageReferences.ImageIndex = 2;
            tabPageReferences.Location = new System.Drawing.Point(4, 24);
            tabPageReferences.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageReferences.Name = "tabPageReferences";
            tabPageReferences.Size = new System.Drawing.Size(192, 72);
            tabPageReferences.TabIndex = 3;
            tabPageReferences.Text = "Citations & References";
            tabPageReferences.UseVisualStyleBackColor = true;
            // 
            // splitContainerReference
            // 
            splitContainerReference.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProviderDiversityProjects.SetHelpKeyword(splitContainerReference, "References");
            helpProviderDiversityProjects.SetHelpNavigator(splitContainerReference, System.Windows.Forms.HelpNavigator.KeywordIndex);
            helpProviderDiversityProjects.SetHelpString(splitContainerReference, "References");
            splitContainerReference.Location = new System.Drawing.Point(0, 0);
            splitContainerReference.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerReference.Name = "splitContainerReference";
            // 
            // splitContainerReference.Panel1
            // 
            splitContainerReference.Panel1.Controls.Add(tableLayoutPanelReferenceLists);
            splitContainerReference.Panel1.Controls.Add(toolStripReference);
            // 
            // splitContainerReference.Panel2
            // 
            splitContainerReference.Panel2.Controls.Add(tableLayoutPanelReference);
            helpProviderDiversityProjects.SetShowHelp(splitContainerReference, true);
            splitContainerReference.Size = new System.Drawing.Size(192, 72);
            splitContainerReference.SplitterDistance = 62;
            splitContainerReference.SplitterWidth = 5;
            splitContainerReference.TabIndex = 1;
            // 
            // tableLayoutPanelReferenceLists
            // 
            tableLayoutPanelReferenceLists.ColumnCount = 1;
            tableLayoutPanelReferenceLists.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelReferenceLists.Controls.Add(listBoxReference, 0, 4);
            tableLayoutPanelReferenceLists.Controls.Add(labelCitation, 0, 0);
            tableLayoutPanelReferenceLists.Controls.Add(labelReferences, 0, 3);
            tableLayoutPanelReferenceLists.Controls.Add(toolStripCitations, 0, 2);
            tableLayoutPanelReferenceLists.Controls.Add(listBoxCitations, 0, 1);
            tableLayoutPanelReferenceLists.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelReferenceLists.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelReferenceLists.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanelReferenceLists.Name = "tableLayoutPanelReferenceLists";
            tableLayoutPanelReferenceLists.RowCount = 5;
            tableLayoutPanelReferenceLists.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelReferenceLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelReferenceLists.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelReferenceLists.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelReferenceLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelReferenceLists.Size = new System.Drawing.Size(62, 47);
            tableLayoutPanelReferenceLists.TabIndex = 2;
            // 
            // listBoxReference
            // 
            listBoxReference.DataSource = projectReferenceBindingSource;
            listBoxReference.DisplayMember = "ReferenceTitle";
            listBoxReference.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxReference.FormattingEnabled = true;
            listBoxReference.IntegralHeight = false;
            listBoxReference.ItemHeight = 15;
            listBoxReference.Location = new System.Drawing.Point(0, 98);
            listBoxReference.Margin = new System.Windows.Forms.Padding(0);
            listBoxReference.Name = "listBoxReference";
            listBoxReference.Size = new System.Drawing.Size(62, 1);
            listBoxReference.TabIndex = 1;
            listBoxReference.ValueMember = "ReferenceURI";
            listBoxReference.SelectedIndexChanged += listBoxReference_SelectedIndexChanged;
            // 
            // projectReferenceBindingSource
            // 
            projectReferenceBindingSource.DataMember = "ProjectReference";
            projectReferenceBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // labelCitation
            // 
            labelCitation.AutoSize = true;
            labelCitation.Location = new System.Drawing.Point(0, 0);
            labelCitation.Margin = new System.Windows.Forms.Padding(0);
            labelCitation.Name = "labelCitation";
            labelCitation.Size = new System.Drawing.Size(57, 60);
            labelCitation.TabIndex = 2;
            labelCitation.Text = "Citations of the project data";
            // 
            // labelReferences
            // 
            labelReferences.AutoSize = true;
            labelReferences.Location = new System.Drawing.Point(0, 38);
            labelReferences.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            labelReferences.Name = "labelReferences";
            labelReferences.Size = new System.Drawing.Size(60, 60);
            labelReferences.TabIndex = 3;
            labelReferences.Text = "Literature related to the project";
            // 
            // toolStripCitations
            // 
            toolStripCitations.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripCitations.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonCitationAdd, toolStripButtonCitationDelete, toolStripButtonCitationToReference, toolStripButtonReferenceToCitation });
            toolStripCitations.Location = new System.Drawing.Point(0, 10);
            toolStripCitations.Name = "toolStripCitations";
            toolStripCitations.Size = new System.Drawing.Size(62, 25);
            toolStripCitations.TabIndex = 4;
            toolStripCitations.Text = "toolStrip1";
            // 
            // toolStripButtonCitationAdd
            // 
            toolStripButtonCitationAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonCitationAdd.Image = Properties.Resources.Add;
            toolStripButtonCitationAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonCitationAdd.Name = "toolStripButtonCitationAdd";
            toolStripButtonCitationAdd.Size = new System.Drawing.Size(23, 22);
            toolStripButtonCitationAdd.Text = "Add a new citation";
            toolStripButtonCitationAdd.Click += toolStripButtonCitationAdd_Click;
            // 
            // toolStripButtonCitationDelete
            // 
            toolStripButtonCitationDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonCitationDelete.Image = Properties.Resources.Delete;
            toolStripButtonCitationDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonCitationDelete.Name = "toolStripButtonCitationDelete";
            toolStripButtonCitationDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonCitationDelete.Text = "Delete the selected citation";
            toolStripButtonCitationDelete.Click += toolStripButtonCitationDelete_Click;
            // 
            // toolStripButtonCitationToReference
            // 
            toolStripButtonCitationToReference.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonCitationToReference.Image = Properties.Resources.ArrowDown;
            toolStripButtonCitationToReference.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonCitationToReference.Name = "toolStripButtonCitationToReference";
            toolStripButtonCitationToReference.Size = new System.Drawing.Size(23, 22);
            toolStripButtonCitationToReference.Text = "Convert the selected citation into a reference";
            toolStripButtonCitationToReference.Click += toolStripButtonCitationToReference_Click;
            // 
            // toolStripButtonReferenceToCitation
            // 
            toolStripButtonReferenceToCitation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReferenceToCitation.Image = Properties.Resources.ArrowUp;
            toolStripButtonReferenceToCitation.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReferenceToCitation.Name = "toolStripButtonReferenceToCitation";
            toolStripButtonReferenceToCitation.Size = new System.Drawing.Size(23, 22);
            toolStripButtonReferenceToCitation.Text = "Convert the selected reference into a citation";
            toolStripButtonReferenceToCitation.Click += toolStripButtonReferenceToCitation_Click;
            // 
            // listBoxCitations
            // 
            listBoxCitations.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxCitations.FormattingEnabled = true;
            listBoxCitations.IntegralHeight = false;
            listBoxCitations.ItemHeight = 15;
            listBoxCitations.Location = new System.Drawing.Point(0, 60);
            listBoxCitations.Margin = new System.Windows.Forms.Padding(0);
            listBoxCitations.Name = "listBoxCitations";
            listBoxCitations.Size = new System.Drawing.Size(62, 1);
            listBoxCitations.TabIndex = 5;
            listBoxCitations.SelectedIndexChanged += listBoxCitations_SelectedIndexChanged;
            // 
            // toolStripReference
            // 
            toolStripReference.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripReference.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripReference.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonReferenceNew, toolStripButtonReferenceDelete });
            toolStripReference.Location = new System.Drawing.Point(0, 47);
            toolStripReference.Name = "toolStripReference";
            toolStripReference.Size = new System.Drawing.Size(62, 25);
            toolStripReference.TabIndex = 0;
            toolStripReference.Text = "toolStrip1";
            // 
            // toolStripButtonReferenceNew
            // 
            toolStripButtonReferenceNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReferenceNew.Image = Properties.Resources.Add;
            toolStripButtonReferenceNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReferenceNew.Name = "toolStripButtonReferenceNew";
            toolStripButtonReferenceNew.Size = new System.Drawing.Size(23, 22);
            toolStripButtonReferenceNew.Text = "Add a new reference";
            toolStripButtonReferenceNew.Click += toolStripButtonReferenceNew_Click;
            // 
            // toolStripButtonReferenceDelete
            // 
            toolStripButtonReferenceDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReferenceDelete.Image = Properties.Resources.Delete;
            toolStripButtonReferenceDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReferenceDelete.Name = "toolStripButtonReferenceDelete";
            toolStripButtonReferenceDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonReferenceDelete.Text = "Remove the selected reference";
            toolStripButtonReferenceDelete.Click += toolStripButtonReferenceDelete_Click;
            // 
            // tableLayoutPanelReference
            // 
            tableLayoutPanelReference.ColumnCount = 3;
            tableLayoutPanelReference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelReference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelReference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelReference.Controls.Add(labelReferenceDetails, 0, 1);
            tableLayoutPanelReference.Controls.Add(labelReferenceNotes, 0, 6);
            tableLayoutPanelReference.Controls.Add(textBoxReferenceNotes, 1, 6);
            tableLayoutPanelReference.Controls.Add(textBoxReferenceDetails, 1, 1);
            tableLayoutPanelReference.Controls.Add(labelReferenceType, 0, 3);
            tableLayoutPanelReference.Controls.Add(comboBoxReferenceType, 1, 3);
            tableLayoutPanelReference.Controls.Add(labelReferenceUri, 0, 2);
            tableLayoutPanelReference.Controls.Add(textBoxReferenceUri, 1, 2);
            tableLayoutPanelReference.Controls.Add(buttonReferenceUri, 2, 2);
            tableLayoutPanelReference.Controls.Add(labelReferenceCitation, 0, 5);
            tableLayoutPanelReference.Controls.Add(textBoxReferenceCitation, 1, 5);
            tableLayoutPanelReference.Controls.Add(labelReferenceTypeDependentCitation, 0, 4);
            tableLayoutPanelReference.Controls.Add(userControlModuleRelatedEntryReference, 0, 0);
            tableLayoutPanelReference.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProviderDiversityProjects.SetHelpKeyword(tableLayoutPanelReference, "References");
            helpProviderDiversityProjects.SetHelpNavigator(tableLayoutPanelReference, System.Windows.Forms.HelpNavigator.KeywordIndex);
            helpProviderDiversityProjects.SetHelpString(tableLayoutPanelReference, "References");
            tableLayoutPanelReference.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelReference.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelReference.Name = "tableLayoutPanelReference";
            tableLayoutPanelReference.RowCount = 7;
            tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            helpProviderDiversityProjects.SetShowHelp(tableLayoutPanelReference, true);
            tableLayoutPanelReference.Size = new System.Drawing.Size(125, 72);
            tableLayoutPanelReference.TabIndex = 0;
            // 
            // labelReferenceDetails
            // 
            labelReferenceDetails.AutoSize = true;
            labelReferenceDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            labelReferenceDetails.Location = new System.Drawing.Point(4, 38);
            labelReferenceDetails.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelReferenceDetails.Name = "labelReferenceDetails";
            labelReferenceDetails.Size = new System.Drawing.Size(56, 29);
            labelReferenceDetails.TabIndex = 1;
            labelReferenceDetails.Text = "Details:";
            labelReferenceDetails.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelReferenceNotes
            // 
            labelReferenceNotes.AutoSize = true;
            labelReferenceNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelReferenceNotes.Location = new System.Drawing.Point(4, 116);
            labelReferenceNotes.Margin = new System.Windows.Forms.Padding(4, 7, 0, 0);
            labelReferenceNotes.Name = "labelReferenceNotes";
            labelReferenceNotes.Size = new System.Drawing.Size(56, 1);
            labelReferenceNotes.TabIndex = 2;
            labelReferenceNotes.Text = "Notes:";
            labelReferenceNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxReferenceNotes
            // 
            tableLayoutPanelReference.SetColumnSpan(textBoxReferenceNotes, 2);
            textBoxReferenceNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectReferenceBindingSource, "Notes", true));
            textBoxReferenceNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxReferenceNotes.Location = new System.Drawing.Point(64, 112);
            textBoxReferenceNotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxReferenceNotes.Multiline = true;
            textBoxReferenceNotes.Name = "textBoxReferenceNotes";
            textBoxReferenceNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxReferenceNotes.Size = new System.Drawing.Size(57, 1);
            textBoxReferenceNotes.TabIndex = 4;
            // 
            // textBoxReferenceDetails
            // 
            tableLayoutPanelReference.SetColumnSpan(textBoxReferenceDetails, 2);
            textBoxReferenceDetails.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectReferenceBindingSource, "ReferenceDetails", true));
            textBoxReferenceDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxReferenceDetails.Location = new System.Drawing.Point(64, 41);
            textBoxReferenceDetails.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxReferenceDetails.Name = "textBoxReferenceDetails";
            textBoxReferenceDetails.Size = new System.Drawing.Size(57, 23);
            textBoxReferenceDetails.TabIndex = 5;
            // 
            // labelReferenceType
            // 
            labelReferenceType.AutoSize = true;
            labelReferenceType.Dock = System.Windows.Forms.DockStyle.Fill;
            labelReferenceType.Location = new System.Drawing.Point(4, 100);
            labelReferenceType.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelReferenceType.Name = "labelReferenceType";
            labelReferenceType.Size = new System.Drawing.Size(56, 26);
            labelReferenceType.TabIndex = 6;
            labelReferenceType.Text = "Type:";
            labelReferenceType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxReferenceType
            // 
            tableLayoutPanelReference.SetColumnSpan(comboBoxReferenceType, 2);
            comboBoxReferenceType.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectReferenceBindingSource, "ReferenceType", true));
            comboBoxReferenceType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxReferenceType.FormattingEnabled = true;
            comboBoxReferenceType.Location = new System.Drawing.Point(64, 103);
            comboBoxReferenceType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            comboBoxReferenceType.Name = "comboBoxReferenceType";
            comboBoxReferenceType.Size = new System.Drawing.Size(57, 23);
            comboBoxReferenceType.TabIndex = 7;
            comboBoxReferenceType.DropDown += comboBoxReferenceType_DropDown;
            comboBoxReferenceType.SelectedIndexChanged += comboBoxReferenceType_SelectedIndexChanged;
            // 
            // labelReferenceUri
            // 
            labelReferenceUri.AutoSize = true;
            labelReferenceUri.Dock = System.Windows.Forms.DockStyle.Fill;
            labelReferenceUri.Location = new System.Drawing.Point(4, 67);
            labelReferenceUri.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelReferenceUri.Name = "labelReferenceUri";
            labelReferenceUri.Size = new System.Drawing.Size(56, 33);
            labelReferenceUri.TabIndex = 8;
            labelReferenceUri.Text = "URI:";
            labelReferenceUri.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxReferenceUri
            // 
            textBoxReferenceUri.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectReferenceBindingSource, "URI", true));
            textBoxReferenceUri.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxReferenceUri.Location = new System.Drawing.Point(64, 70);
            textBoxReferenceUri.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            textBoxReferenceUri.Name = "textBoxReferenceUri";
            textBoxReferenceUri.Size = new System.Drawing.Size(29, 23);
            textBoxReferenceUri.TabIndex = 9;
            // 
            // buttonReferenceUri
            // 
            buttonReferenceUri.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonReferenceUri.Image = Properties.Resources.Browse;
            buttonReferenceUri.Location = new System.Drawing.Point(93, 68);
            buttonReferenceUri.Margin = new System.Windows.Forms.Padding(0, 1, 4, 1);
            buttonReferenceUri.Name = "buttonReferenceUri";
            buttonReferenceUri.Size = new System.Drawing.Size(28, 31);
            buttonReferenceUri.TabIndex = 10;
            toolTip.SetToolTip(buttonReferenceUri, "Search for the URL of the reference");
            buttonReferenceUri.UseVisualStyleBackColor = true;
            buttonReferenceUri.Click += buttonReferenceUri_Click;
            // 
            // labelReferenceCitation
            // 
            labelReferenceCitation.AutoSize = true;
            labelReferenceCitation.Dock = System.Windows.Forms.DockStyle.Fill;
            labelReferenceCitation.Location = new System.Drawing.Point(4, 152);
            labelReferenceCitation.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            labelReferenceCitation.Name = "labelReferenceCitation";
            labelReferenceCitation.Size = new System.Drawing.Size(52, 1);
            labelReferenceCitation.TabIndex = 11;
            labelReferenceCitation.Text = "Citation:";
            labelReferenceCitation.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxReferenceCitation
            // 
            tableLayoutPanelReference.SetColumnSpan(textBoxReferenceCitation, 2);
            textBoxReferenceCitation.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectReferenceBindingSource, "ReferenceCitation", true));
            textBoxReferenceCitation.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxReferenceCitation.Location = new System.Drawing.Point(64, 148);
            textBoxReferenceCitation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxReferenceCitation.Multiline = true;
            textBoxReferenceCitation.Name = "textBoxReferenceCitation";
            textBoxReferenceCitation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxReferenceCitation.Size = new System.Drawing.Size(57, 1);
            textBoxReferenceCitation.TabIndex = 12;
            // 
            // labelReferenceTypeDependentCitation
            // 
            labelReferenceTypeDependentCitation.AutoSize = true;
            tableLayoutPanelReference.SetColumnSpan(labelReferenceTypeDependentCitation, 3);
            labelReferenceTypeDependentCitation.Dock = System.Windows.Forms.DockStyle.Fill;
            labelReferenceTypeDependentCitation.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            labelReferenceTypeDependentCitation.Location = new System.Drawing.Point(10, 127);
            labelReferenceTypeDependentCitation.Margin = new System.Windows.Forms.Padding(10, 1, 4, 3);
            labelReferenceTypeDependentCitation.Name = "labelReferenceTypeDependentCitation";
            labelReferenceTypeDependentCitation.Size = new System.Drawing.Size(111, 15);
            labelReferenceTypeDependentCitation.TabIndex = 13;
            labelReferenceTypeDependentCitation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            labelReferenceTypeDependentCitation.Visible = false;
            // 
            // userControlModuleRelatedEntryReference
            // 
            userControlModuleRelatedEntryReference.CanDeleteConnectionToModule = true;
            tableLayoutPanelReference.SetColumnSpan(userControlModuleRelatedEntryReference, 3);
            userControlModuleRelatedEntryReference.DependsOnUri = "";
            userControlModuleRelatedEntryReference.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryReference.Domain = "";
            userControlModuleRelatedEntryReference.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryReference.Location = new System.Drawing.Point(4, 7);
            userControlModuleRelatedEntryReference.Margin = new System.Windows.Forms.Padding(4, 7, 4, 3);
            userControlModuleRelatedEntryReference.Module = null;
            userControlModuleRelatedEntryReference.Name = "userControlModuleRelatedEntryReference";
            userControlModuleRelatedEntryReference.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryReference.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryReference.ShowInfo = false;
            userControlModuleRelatedEntryReference.Size = new System.Drawing.Size(117, 28);
            userControlModuleRelatedEntryReference.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryReference.TabIndex = 14;
            // 
            // tabPageDescriptor
            // 
            tabPageDescriptor.Controls.Add(splitContainerDescriptor);
            tabPageDescriptor.ImageIndex = 7;
            tabPageDescriptor.Location = new System.Drawing.Point(4, 24);
            tabPageDescriptor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDescriptor.Name = "tabPageDescriptor";
            tabPageDescriptor.Size = new System.Drawing.Size(192, 72);
            tabPageDescriptor.TabIndex = 8;
            tabPageDescriptor.Text = "Descriptor";
            tabPageDescriptor.UseVisualStyleBackColor = true;
            // 
            // splitContainerDescriptor
            // 
            splitContainerDescriptor.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProviderDiversityProjects.SetHelpKeyword(splitContainerDescriptor, "Descriptor");
            helpProviderDiversityProjects.SetHelpNavigator(splitContainerDescriptor, System.Windows.Forms.HelpNavigator.KeywordIndex);
            helpProviderDiversityProjects.SetHelpString(splitContainerDescriptor, "Descriptor");
            splitContainerDescriptor.Location = new System.Drawing.Point(0, 0);
            splitContainerDescriptor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerDescriptor.Name = "splitContainerDescriptor";
            // 
            // splitContainerDescriptor.Panel1
            // 
            splitContainerDescriptor.Panel1.Controls.Add(listBoxDescriptor);
            splitContainerDescriptor.Panel1.Controls.Add(toolStripDescriptor);
            // 
            // splitContainerDescriptor.Panel2
            // 
            splitContainerDescriptor.Panel2.Controls.Add(tableLayoutPanelDescriptor);
            splitContainerDescriptor.Panel2.Enabled = false;
            helpProviderDiversityProjects.SetShowHelp(splitContainerDescriptor, true);
            splitContainerDescriptor.Size = new System.Drawing.Size(192, 72);
            splitContainerDescriptor.SplitterDistance = 63;
            splitContainerDescriptor.SplitterWidth = 5;
            splitContainerDescriptor.TabIndex = 0;
            // 
            // listBoxDescriptor
            // 
            listBoxDescriptor.DataSource = projectDescriptorBindingSource;
            listBoxDescriptor.DisplayMember = "Content";
            listBoxDescriptor.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxDescriptor.FormattingEnabled = true;
            listBoxDescriptor.IntegralHeight = false;
            listBoxDescriptor.ItemHeight = 15;
            listBoxDescriptor.Location = new System.Drawing.Point(0, 0);
            listBoxDescriptor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxDescriptor.Name = "listBoxDescriptor";
            listBoxDescriptor.Size = new System.Drawing.Size(63, 47);
            listBoxDescriptor.TabIndex = 0;
            listBoxDescriptor.ValueMember = "ID";
            listBoxDescriptor.SelectedIndexChanged += listBoxDescriptor_SelectedIndexChanged;
            // 
            // projectDescriptorBindingSource
            // 
            projectDescriptorBindingSource.DataMember = "ProjectDescriptor";
            projectDescriptorBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // toolStripDescriptor
            // 
            toolStripDescriptor.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripDescriptor.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripDescriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonDescriptorAdd, toolStripButtonDescriptorDelete });
            toolStripDescriptor.Location = new System.Drawing.Point(0, 47);
            toolStripDescriptor.Name = "toolStripDescriptor";
            toolStripDescriptor.Size = new System.Drawing.Size(63, 25);
            toolStripDescriptor.TabIndex = 1;
            toolStripDescriptor.Text = "toolStrip1";
            // 
            // toolStripButtonDescriptorAdd
            // 
            toolStripButtonDescriptorAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonDescriptorAdd.Image = Properties.Resources.Add;
            toolStripButtonDescriptorAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonDescriptorAdd.Name = "toolStripButtonDescriptorAdd";
            toolStripButtonDescriptorAdd.Size = new System.Drawing.Size(23, 22);
            toolStripButtonDescriptorAdd.Text = "Add a new descriptor";
            toolStripButtonDescriptorAdd.Click += toolStripButtonDescriptorAdd_Click;
            // 
            // toolStripButtonDescriptorDelete
            // 
            toolStripButtonDescriptorDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonDescriptorDelete.Image = Properties.Resources.Delete;
            toolStripButtonDescriptorDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonDescriptorDelete.Name = "toolStripButtonDescriptorDelete";
            toolStripButtonDescriptorDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonDescriptorDelete.Text = "Remove the selected descriptor";
            toolStripButtonDescriptorDelete.Click += toolStripButtonDescriptorDelete_Click;
            // 
            // tableLayoutPanelDescriptor
            // 
            tableLayoutPanelDescriptor.ColumnCount = 5;
            tableLayoutPanelDescriptor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelDescriptor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelDescriptor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelDescriptor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelDescriptor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelDescriptor.Controls.Add(labelDescriptorLanguage, 3, 1);
            tableLayoutPanelDescriptor.Controls.Add(comboBoxDescriptorLanguage, 4, 1);
            tableLayoutPanelDescriptor.Controls.Add(textBoxDescriptorContent, 0, 5);
            tableLayoutPanelDescriptor.Controls.Add(labelDescriptorContent, 0, 0);
            tableLayoutPanelDescriptor.Controls.Add(labelDescriptorElement, 0, 1);
            tableLayoutPanelDescriptor.Controls.Add(labelDescriptorContentUri, 0, 6);
            tableLayoutPanelDescriptor.Controls.Add(textBoxDescriptorContentUri, 0, 7);
            tableLayoutPanelDescriptor.Controls.Add(comboBoxDescriptorElement, 1, 1);
            tableLayoutPanelDescriptor.Controls.Add(userControlModuleRelatedEntryDescriptorGazetteer, 0, 2);
            tableLayoutPanelDescriptor.Controls.Add(userControlModuleRelatedEntryDescriptorTerm, 0, 3);
            tableLayoutPanelDescriptor.Controls.Add(userControlModuleRelatedEntryDescriptorTaxon, 0, 4);
            tableLayoutPanelDescriptor.Controls.Add(buttonDescriptorTypes, 2, 1);
            tableLayoutPanelDescriptor.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelDescriptor.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelDescriptor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelDescriptor.Name = "tableLayoutPanelDescriptor";
            tableLayoutPanelDescriptor.RowCount = 9;
            tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelDescriptor.Size = new System.Drawing.Size(124, 72);
            tableLayoutPanelDescriptor.TabIndex = 0;
            // 
            // labelDescriptorLanguage
            // 
            labelDescriptorLanguage.AutoSize = true;
            labelDescriptorLanguage.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDescriptorLanguage.Location = new System.Drawing.Point(-20, 18);
            labelDescriptorLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelDescriptorLanguage.Name = "labelDescriptorLanguage";
            labelDescriptorLanguage.Size = new System.Drawing.Size(39, 33);
            labelDescriptorLanguage.TabIndex = 0;
            labelDescriptorLanguage.Text = "Lang.:";
            labelDescriptorLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxDescriptorLanguage
            // 
            comboBoxDescriptorLanguage.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", projectDescriptorBindingSource, "Language", true));
            comboBoxDescriptorLanguage.DataSource = languageEnumBindingSource;
            comboBoxDescriptorLanguage.DisplayMember = "Description";
            comboBoxDescriptorLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxDescriptorLanguage.FormattingEnabled = true;
            comboBoxDescriptorLanguage.Location = new System.Drawing.Point(27, 21);
            comboBoxDescriptorLanguage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxDescriptorLanguage.Name = "comboBoxDescriptorLanguage";
            comboBoxDescriptorLanguage.Size = new System.Drawing.Size(93, 23);
            comboBoxDescriptorLanguage.TabIndex = 1;
            comboBoxDescriptorLanguage.ValueMember = "Code";
            // 
            // languageEnumBindingSource
            // 
            languageEnumBindingSource.DataMember = "Language_Enum";
            languageEnumBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // textBoxDescriptorContent
            // 
            tableLayoutPanelDescriptor.SetColumnSpan(textBoxDescriptorContent, 5);
            textBoxDescriptorContent.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectDescriptorBindingSource, "Content", true));
            textBoxDescriptorContent.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxDescriptorContent.Location = new System.Drawing.Point(4, 147);
            textBoxDescriptorContent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxDescriptorContent.Multiline = true;
            textBoxDescriptorContent.Name = "textBoxDescriptorContent";
            textBoxDescriptorContent.Size = new System.Drawing.Size(116, 59);
            textBoxDescriptorContent.TabIndex = 2;
            // 
            // labelDescriptorContent
            // 
            labelDescriptorContent.AutoSize = true;
            tableLayoutPanelDescriptor.SetColumnSpan(labelDescriptorContent, 5);
            labelDescriptorContent.Location = new System.Drawing.Point(4, 3);
            labelDescriptorContent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            labelDescriptorContent.Name = "labelDescriptorContent";
            labelDescriptorContent.Size = new System.Drawing.Size(61, 15);
            labelDescriptorContent.TabIndex = 3;
            labelDescriptorContent.Text = "Descriptor";
            // 
            // labelDescriptorElement
            // 
            labelDescriptorElement.AutoSize = true;
            labelDescriptorElement.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDescriptorElement.Location = new System.Drawing.Point(4, 18);
            labelDescriptorElement.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelDescriptorElement.Name = "labelDescriptorElement";
            labelDescriptorElement.Size = new System.Drawing.Size(34, 33);
            labelDescriptorElement.TabIndex = 4;
            labelDescriptorElement.Text = "Type:";
            labelDescriptorElement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDescriptorContentUri
            // 
            labelDescriptorContentUri.AutoSize = true;
            labelDescriptorContentUri.Location = new System.Drawing.Point(4, 212);
            labelDescriptorContentUri.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            labelDescriptorContentUri.Name = "labelDescriptorContentUri";
            labelDescriptorContentUri.Size = new System.Drawing.Size(25, 15);
            labelDescriptorContentUri.TabIndex = 5;
            labelDescriptorContentUri.Text = "URI";
            // 
            // textBoxDescriptorContentUri
            // 
            tableLayoutPanelDescriptor.SetColumnSpan(textBoxDescriptorContentUri, 5);
            textBoxDescriptorContentUri.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectDescriptorBindingSource, "ContentURI", true));
            textBoxDescriptorContentUri.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxDescriptorContentUri.Location = new System.Drawing.Point(4, 230);
            textBoxDescriptorContentUri.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxDescriptorContentUri.Multiline = true;
            textBoxDescriptorContentUri.Name = "textBoxDescriptorContentUri";
            textBoxDescriptorContentUri.Size = new System.Drawing.Size(116, 46);
            textBoxDescriptorContentUri.TabIndex = 6;
            // 
            // comboBoxDescriptorElement
            // 
            comboBoxDescriptorElement.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", projectDescriptorBindingSource, "ElementID", true));
            comboBoxDescriptorElement.DataSource = projectDescriptorElementBindingSource;
            comboBoxDescriptorElement.DisplayMember = "DisplayText";
            comboBoxDescriptorElement.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxDescriptorElement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxDescriptorElement.FormattingEnabled = true;
            comboBoxDescriptorElement.Location = new System.Drawing.Point(46, 21);
            comboBoxDescriptorElement.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            comboBoxDescriptorElement.Name = "comboBoxDescriptorElement";
            comboBoxDescriptorElement.Size = new System.Drawing.Size(1, 23);
            comboBoxDescriptorElement.TabIndex = 7;
            comboBoxDescriptorElement.ValueMember = "ElementID";
            comboBoxDescriptorElement.SelectedIndexChanged += comboBoxDescriptorElement_SelectedIndexChanged;
            // 
            // projectDescriptorElementBindingSource
            // 
            projectDescriptorElementBindingSource.DataMember = "ProjectDescriptorElement";
            projectDescriptorElementBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // userControlModuleRelatedEntryDescriptorGazetteer
            // 
            userControlModuleRelatedEntryDescriptorGazetteer.CanDeleteConnectionToModule = true;
            tableLayoutPanelDescriptor.SetColumnSpan(userControlModuleRelatedEntryDescriptorGazetteer, 5);
            userControlModuleRelatedEntryDescriptorGazetteer.DependsOnUri = "";
            userControlModuleRelatedEntryDescriptorGazetteer.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryDescriptorGazetteer.Domain = "";
            userControlModuleRelatedEntryDescriptorGazetteer.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryDescriptorGazetteer.Location = new System.Drawing.Point(5, 54);
            userControlModuleRelatedEntryDescriptorGazetteer.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleRelatedEntryDescriptorGazetteer.Module = null;
            userControlModuleRelatedEntryDescriptorGazetteer.Name = "userControlModuleRelatedEntryDescriptorGazetteer";
            userControlModuleRelatedEntryDescriptorGazetteer.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryDescriptorGazetteer.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryDescriptorGazetteer.ShowInfo = false;
            userControlModuleRelatedEntryDescriptorGazetteer.Size = new System.Drawing.Size(114, 25);
            userControlModuleRelatedEntryDescriptorGazetteer.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryDescriptorGazetteer.TabIndex = 8;
            // 
            // userControlModuleRelatedEntryDescriptorTerm
            // 
            userControlModuleRelatedEntryDescriptorTerm.CanDeleteConnectionToModule = true;
            tableLayoutPanelDescriptor.SetColumnSpan(userControlModuleRelatedEntryDescriptorTerm, 5);
            userControlModuleRelatedEntryDescriptorTerm.DependsOnUri = "";
            userControlModuleRelatedEntryDescriptorTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryDescriptorTerm.Domain = "";
            userControlModuleRelatedEntryDescriptorTerm.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryDescriptorTerm.Location = new System.Drawing.Point(5, 85);
            userControlModuleRelatedEntryDescriptorTerm.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleRelatedEntryDescriptorTerm.Module = null;
            userControlModuleRelatedEntryDescriptorTerm.Name = "userControlModuleRelatedEntryDescriptorTerm";
            userControlModuleRelatedEntryDescriptorTerm.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryDescriptorTerm.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryDescriptorTerm.ShowInfo = false;
            userControlModuleRelatedEntryDescriptorTerm.Size = new System.Drawing.Size(114, 25);
            userControlModuleRelatedEntryDescriptorTerm.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryDescriptorTerm.TabIndex = 9;
            // 
            // userControlModuleRelatedEntryDescriptorTaxon
            // 
            userControlModuleRelatedEntryDescriptorTaxon.CanDeleteConnectionToModule = true;
            tableLayoutPanelDescriptor.SetColumnSpan(userControlModuleRelatedEntryDescriptorTaxon, 5);
            userControlModuleRelatedEntryDescriptorTaxon.DependsOnUri = "";
            userControlModuleRelatedEntryDescriptorTaxon.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryDescriptorTaxon.Domain = "";
            userControlModuleRelatedEntryDescriptorTaxon.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryDescriptorTaxon.Location = new System.Drawing.Point(5, 116);
            userControlModuleRelatedEntryDescriptorTaxon.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleRelatedEntryDescriptorTaxon.Module = null;
            userControlModuleRelatedEntryDescriptorTaxon.Name = "userControlModuleRelatedEntryDescriptorTaxon";
            userControlModuleRelatedEntryDescriptorTaxon.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryDescriptorTaxon.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryDescriptorTaxon.ShowInfo = false;
            userControlModuleRelatedEntryDescriptorTaxon.Size = new System.Drawing.Size(114, 25);
            userControlModuleRelatedEntryDescriptorTaxon.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryDescriptorTaxon.TabIndex = 10;
            // 
            // buttonDescriptorTypes
            // 
            buttonDescriptorTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonDescriptorTypes.FlatAppearance.BorderSize = 0;
            buttonDescriptorTypes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonDescriptorTypes.Image = Properties.Resources.KeyBlue;
            buttonDescriptorTypes.Location = new System.Drawing.Point(-50, 18);
            buttonDescriptorTypes.Margin = new System.Windows.Forms.Padding(0);
            buttonDescriptorTypes.Name = "buttonDescriptorTypes";
            buttonDescriptorTypes.Size = new System.Drawing.Size(26, 33);
            buttonDescriptorTypes.TabIndex = 11;
            toolTip.SetToolTip(buttonDescriptorTypes, "Edit descriptor types");
            buttonDescriptorTypes.UseVisualStyleBackColor = true;
            buttonDescriptorTypes.Click += buttonDescriptorTypes_Click;
            // 
            // tabPageID
            // 
            tabPageID.Controls.Add(splitContainerID);
            tabPageID.ImageIndex = 5;
            tabPageID.Location = new System.Drawing.Point(4, 24);
            tabPageID.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageID.Name = "tabPageID";
            tabPageID.Size = new System.Drawing.Size(192, 72);
            tabPageID.TabIndex = 6;
            tabPageID.Text = "IDs";
            tabPageID.UseVisualStyleBackColor = true;
            // 
            // splitContainerID
            // 
            splitContainerID.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerID.Location = new System.Drawing.Point(0, 0);
            splitContainerID.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerID.Name = "splitContainerID";
            // 
            // splitContainerID.Panel1
            // 
            splitContainerID.Panel1.Controls.Add(listBoxID);
            splitContainerID.Panel1.Controls.Add(toolStripID);
            // 
            // splitContainerID.Panel2
            // 
            splitContainerID.Panel2.Controls.Add(tableLayoutPanelID);
            splitContainerID.Size = new System.Drawing.Size(192, 72);
            splitContainerID.SplitterDistance = 63;
            splitContainerID.SplitterWidth = 5;
            splitContainerID.TabIndex = 0;
            // 
            // listBoxID
            // 
            listBoxID.DataSource = projectIdentifierBindingSource;
            listBoxID.DisplayMember = "Identifier";
            listBoxID.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxID.FormattingEnabled = true;
            listBoxID.ItemHeight = 15;
            listBoxID.Location = new System.Drawing.Point(0, 0);
            listBoxID.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxID.Name = "listBoxID";
            listBoxID.Size = new System.Drawing.Size(63, 47);
            listBoxID.TabIndex = 3;
            listBoxID.SelectedIndexChanged += listBoxID_SelectedIndexChanged;
            // 
            // projectIdentifierBindingSource
            // 
            projectIdentifierBindingSource.DataMember = "ProjectIdentifier";
            projectIdentifierBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // toolStripID
            // 
            toolStripID.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripID.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripID.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonRequestPID, toolStripButtonIDadd, toolStripButtonIDremove });
            toolStripID.Location = new System.Drawing.Point(0, 47);
            toolStripID.Name = "toolStripID";
            toolStripID.Size = new System.Drawing.Size(63, 25);
            toolStripID.TabIndex = 4;
            toolStripID.Text = "toolStrip1";
            // 
            // toolStripButtonRequestPID
            // 
            toolStripButtonRequestPID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRequestPID.Image = Properties.Resources.Identifier;
            toolStripButtonRequestPID.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRequestPID.Name = "toolStripButtonRequestPID";
            toolStripButtonRequestPID.Size = new System.Drawing.Size(23, 22);
            toolStripButtonRequestPID.Text = "Request PID";
            // 
            // toolStripButtonIDadd
            // 
            toolStripButtonIDadd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonIDadd.Image = Properties.Resources.Add;
            toolStripButtonIDadd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonIDadd.Name = "toolStripButtonIDadd";
            toolStripButtonIDadd.Size = new System.Drawing.Size(23, 22);
            toolStripButtonIDadd.Text = "Add a new identifier";
            toolStripButtonIDadd.Click += toolStripButtonIDadd_Click;
            // 
            // toolStripButtonIDremove
            // 
            toolStripButtonIDremove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonIDremove.Image = Properties.Resources.Delete;
            toolStripButtonIDremove.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonIDremove.Name = "toolStripButtonIDremove";
            toolStripButtonIDremove.Size = new System.Drawing.Size(23, 22);
            toolStripButtonIDremove.Text = "Remove the selected identifier";
            toolStripButtonIDremove.Click += toolStripButtonIDremove_Click;
            // 
            // tableLayoutPanelID
            // 
            tableLayoutPanelID.ColumnCount = 3;
            tableLayoutPanelID.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelID.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelID.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelID.Controls.Add(textBoxIdentifier, 0, 0);
            tableLayoutPanelID.Controls.Add(labelIDtype, 0, 1);
            tableLayoutPanelID.Controls.Add(comboBoxIDtype, 1, 1);
            tableLayoutPanelID.Controls.Add(labelIDurl, 0, 2);
            tableLayoutPanelID.Controls.Add(textBoxIDurl, 1, 2);
            tableLayoutPanelID.Controls.Add(labelIDnotes, 0, 3);
            tableLayoutPanelID.Controls.Add(buttonIDgetURL, 2, 2);
            tableLayoutPanelID.Controls.Add(textBoxIDnotes, 1, 3);
            tableLayoutPanelID.Controls.Add(pictureBoxIdentifierLocked, 2, 0);
            tableLayoutPanelID.Controls.Add(buttonIDtypes, 2, 1);
            tableLayoutPanelID.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelID.Enabled = false;
            tableLayoutPanelID.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelID.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelID.Name = "tableLayoutPanelID";
            tableLayoutPanelID.RowCount = 4;
            tableLayoutPanelID.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelID.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelID.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelID.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelID.Size = new System.Drawing.Size(124, 72);
            tableLayoutPanelID.TabIndex = 0;
            // 
            // textBoxIdentifier
            // 
            tableLayoutPanelID.SetColumnSpan(textBoxIdentifier, 2);
            textBoxIdentifier.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectIdentifierBindingSource, "Identifier", true));
            textBoxIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxIdentifier.Location = new System.Drawing.Point(4, 3);
            textBoxIdentifier.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            textBoxIdentifier.Name = "textBoxIdentifier";
            textBoxIdentifier.Size = new System.Drawing.Size(88, 23);
            textBoxIdentifier.TabIndex = 0;
            // 
            // labelIDtype
            // 
            labelIDtype.AutoSize = true;
            labelIDtype.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIDtype.Location = new System.Drawing.Point(4, 33);
            labelIDtype.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelIDtype.Name = "labelIDtype";
            labelIDtype.Size = new System.Drawing.Size(41, 33);
            labelIDtype.TabIndex = 1;
            labelIDtype.Text = "Type:";
            labelIDtype.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxIDtype
            // 
            comboBoxIDtype.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", projectIdentifierBindingSource, "Type", true));
            comboBoxIDtype.DataSource = projectIdentifierTypeBindingSource;
            comboBoxIDtype.DisplayMember = "Type";
            comboBoxIDtype.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxIDtype.FormattingEnabled = true;
            comboBoxIDtype.Location = new System.Drawing.Point(53, 36);
            comboBoxIDtype.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            comboBoxIDtype.Name = "comboBoxIDtype";
            comboBoxIDtype.Size = new System.Drawing.Size(39, 23);
            comboBoxIDtype.TabIndex = 2;
            comboBoxIDtype.ValueMember = "Type";
            // 
            // projectIdentifierTypeBindingSource
            // 
            projectIdentifierTypeBindingSource.DataMember = "ProjectIdentifierType";
            projectIdentifierTypeBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // labelIDurl
            // 
            labelIDurl.AutoSize = true;
            labelIDurl.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIDurl.Location = new System.Drawing.Point(4, 66);
            labelIDurl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelIDurl.Name = "labelIDurl";
            labelIDurl.Size = new System.Drawing.Size(41, 29);
            labelIDurl.TabIndex = 3;
            labelIDurl.Text = "URL:";
            labelIDurl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxIDurl
            // 
            textBoxIDurl.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectIdentifierBindingSource, "URL", true));
            textBoxIDurl.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxIDurl.Location = new System.Drawing.Point(53, 69);
            textBoxIDurl.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            textBoxIDurl.Name = "textBoxIDurl";
            textBoxIDurl.Size = new System.Drawing.Size(39, 23);
            textBoxIDurl.TabIndex = 4;
            // 
            // labelIDnotes
            // 
            labelIDnotes.AutoSize = true;
            labelIDnotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIDnotes.Location = new System.Drawing.Point(4, 98);
            labelIDnotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            labelIDnotes.Name = "labelIDnotes";
            labelIDnotes.Size = new System.Drawing.Size(41, 1);
            labelIDnotes.TabIndex = 5;
            labelIDnotes.Text = "Notes:";
            labelIDnotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonIDgetURL
            // 
            buttonIDgetURL.Image = Properties.Resources.Browse;
            buttonIDgetURL.Location = new System.Drawing.Point(92, 66);
            buttonIDgetURL.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonIDgetURL.Name = "buttonIDgetURL";
            buttonIDgetURL.Size = new System.Drawing.Size(28, 28);
            buttonIDgetURL.TabIndex = 6;
            toolTip.SetToolTip(buttonIDgetURL, "Search for the URL of the identifier");
            buttonIDgetURL.UseVisualStyleBackColor = true;
            buttonIDgetURL.Click += buttonIDgetURL_Click;
            // 
            // textBoxIDnotes
            // 
            tableLayoutPanelID.SetColumnSpan(textBoxIDnotes, 2);
            textBoxIDnotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectIdentifierBindingSource, "Notes", true));
            textBoxIDnotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxIDnotes.Location = new System.Drawing.Point(53, 98);
            textBoxIDnotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxIDnotes.Multiline = true;
            textBoxIDnotes.Name = "textBoxIDnotes";
            textBoxIDnotes.Size = new System.Drawing.Size(67, 1);
            textBoxIDnotes.TabIndex = 7;
            // 
            // pictureBoxIdentifierLocked
            // 
            pictureBoxIdentifierLocked.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxIdentifierLocked.Image = Properties.Resources.OAIS_16;
            pictureBoxIdentifierLocked.Location = new System.Drawing.Point(96, 3);
            pictureBoxIdentifierLocked.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxIdentifierLocked.Name = "pictureBoxIdentifierLocked";
            pictureBoxIdentifierLocked.Size = new System.Drawing.Size(24, 27);
            pictureBoxIdentifierLocked.TabIndex = 8;
            pictureBoxIdentifierLocked.TabStop = false;
            toolTip.SetToolTip(pictureBoxIdentifierLocked, "Locked because OAIP entries relate to this identifier");
            pictureBoxIdentifierLocked.Visible = false;
            // 
            // buttonIDtypes
            // 
            buttonIDtypes.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonIDtypes.FlatAppearance.BorderSize = 0;
            buttonIDtypes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonIDtypes.Image = Properties.Resources.Identifier;
            buttonIDtypes.Location = new System.Drawing.Point(92, 33);
            buttonIDtypes.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonIDtypes.Name = "buttonIDtypes";
            buttonIDtypes.Size = new System.Drawing.Size(28, 33);
            buttonIDtypes.TabIndex = 9;
            toolTip.SetToolTip(buttonIDtypes, "Edit identifier types");
            buttonIDtypes.UseVisualStyleBackColor = true;
            buttonIDtypes.Click += buttonIDtypes_Click;
            // 
            // tabPageArchive
            // 
            tabPageArchive.Controls.Add(splitContainerArchive);
            tabPageArchive.ImageIndex = 9;
            tabPageArchive.Location = new System.Drawing.Point(4, 24);
            tabPageArchive.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageArchive.Name = "tabPageArchive";
            tabPageArchive.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageArchive.Size = new System.Drawing.Size(192, 72);
            tabPageArchive.TabIndex = 9;
            tabPageArchive.Text = "OAIP";
            tabPageArchive.UseVisualStyleBackColor = true;
            // 
            // splitContainerArchive
            // 
            splitContainerArchive.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerArchive.Location = new System.Drawing.Point(4, 3);
            splitContainerArchive.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerArchive.Name = "splitContainerArchive";
            // 
            // splitContainerArchive.Panel1
            // 
            splitContainerArchive.Panel1.Controls.Add(treeViewArchive);
            splitContainerArchive.Panel1.Controls.Add(toolStripArchive);
            // 
            // splitContainerArchive.Panel2
            // 
            splitContainerArchive.Panel2.Controls.Add(tableLayoutPanelArchive);
            splitContainerArchive.Size = new System.Drawing.Size(184, 66);
            splitContainerArchive.SplitterDistance = 51;
            splitContainerArchive.SplitterWidth = 5;
            splitContainerArchive.TabIndex = 0;
            // 
            // treeViewArchive
            // 
            treeViewArchive.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewArchive.Location = new System.Drawing.Point(0, 0);
            treeViewArchive.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeViewArchive.Name = "treeViewArchive";
            treeViewArchive.Size = new System.Drawing.Size(51, 41);
            treeViewArchive.TabIndex = 1;
            treeViewArchive.AfterSelect += treeViewArchive_AfterSelect;
            // 
            // toolStripArchive
            // 
            toolStripArchive.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripArchive.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripArchive.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonArchiveAdd, toolStripButtonArchiveDelete });
            toolStripArchive.Location = new System.Drawing.Point(0, 41);
            toolStripArchive.Name = "toolStripArchive";
            toolStripArchive.Size = new System.Drawing.Size(51, 25);
            toolStripArchive.TabIndex = 0;
            toolStripArchive.Text = "toolStrip1";
            // 
            // toolStripButtonArchiveAdd
            // 
            toolStripButtonArchiveAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonArchiveAdd.Image = Properties.Resources.Add;
            toolStripButtonArchiveAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonArchiveAdd.Name = "toolStripButtonArchiveAdd";
            toolStripButtonArchiveAdd.Size = new System.Drawing.Size(23, 22);
            toolStripButtonArchiveAdd.Text = "Add a new archive";
            toolStripButtonArchiveAdd.Click += toolStripButtonArchiveAdd_Click;
            // 
            // toolStripButtonArchiveDelete
            // 
            toolStripButtonArchiveDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonArchiveDelete.Image = Properties.Resources.Delete;
            toolStripButtonArchiveDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonArchiveDelete.Name = "toolStripButtonArchiveDelete";
            toolStripButtonArchiveDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonArchiveDelete.Text = "Delete the selected archive";
            toolStripButtonArchiveDelete.Click += toolStripButtonArchiveDelete_Click;
            // 
            // tableLayoutPanelArchive
            // 
            tableLayoutPanelArchive.ColumnCount = 6;
            tableLayoutPanelArchive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelArchive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanelArchive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanelArchive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanelArchive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanelArchive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanelArchive.Controls.Add(labelArchiveTransfer, 0, 0);
            tableLayoutPanelArchive.Controls.Add(comboBoxArchiveMimeType, 2, 2);
            tableLayoutPanelArchive.Controls.Add(comboBoxArchiveFormat, 0, 2);
            tableLayoutPanelArchive.Controls.Add(labelArchiveFileSize, 3, 1);
            tableLayoutPanelArchive.Controls.Add(textBoxArchiveFileSize, 3, 2);
            tableLayoutPanelArchive.Controls.Add(labelArchiveFileHash, 4, 1);
            tableLayoutPanelArchive.Controls.Add(textBoxArchiveFileHash, 4, 2);
            tableLayoutPanelArchive.Controls.Add(labelArchiveStorageLocation, 0, 3);
            tableLayoutPanelArchive.Controls.Add(labelArchiveExternalStorageLocation, 0, 4);
            tableLayoutPanelArchive.Controls.Add(labelArchiveTransferDate, 0, 5);
            tableLayoutPanelArchive.Controls.Add(dateTimePickerArchiveTransferDate, 0, 6);
            tableLayoutPanelArchive.Controls.Add(labelArchivePublicationDate, 2, 5);
            tableLayoutPanelArchive.Controls.Add(dateTimePickerArchivePublicationDate, 2, 6);
            tableLayoutPanelArchive.Controls.Add(labelArchiveDataWithholdingReason, 4, 5);
            tableLayoutPanelArchive.Controls.Add(pictureBoxArchiveDataWithholdingReason, 5, 5);
            tableLayoutPanelArchive.Controls.Add(textBoxArchiveDataWithholdingReason, 4, 6);
            tableLayoutPanelArchive.Controls.Add(buttonArchiveStorageLocation, 5, 3);
            tableLayoutPanelArchive.Controls.Add(buttonArchiveExternalStorageLocation, 5, 4);
            tableLayoutPanelArchive.Controls.Add(checkBoxArchivePublished, 4, 0);
            tableLayoutPanelArchive.Controls.Add(labelArchiveNotes, 0, 7);
            tableLayoutPanelArchive.Controls.Add(textBoxArchiveNotes, 0, 8);
            tableLayoutPanelArchive.Controls.Add(textBoxArchiveStorageLocation, 1, 3);
            tableLayoutPanelArchive.Controls.Add(textBoxArchiveExternalStorageLocation, 1, 4);
            tableLayoutPanelArchive.Controls.Add(pictureBoxArchivePublished, 5, 0);
            tableLayoutPanelArchive.Controls.Add(linkLabelArchiveFormat, 0, 1);
            tableLayoutPanelArchive.Controls.Add(linkLabelArchiveMimeType, 2, 1);
            tableLayoutPanelArchive.Controls.Add(labelArchiveIdentifier, 3, 7);
            tableLayoutPanelArchive.Controls.Add(toolStripArchiveIdentier, 5, 8);
            tableLayoutPanelArchive.Controls.Add(listBoxArchiveIdentier, 3, 8);
            tableLayoutPanelArchive.Controls.Add(labelArchiveParentRelationType, 3, 5);
            tableLayoutPanelArchive.Controls.Add(comboBoxArchiveParentRelationType, 3, 6);
            tableLayoutPanelArchive.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelArchive.Enabled = false;
            tableLayoutPanelArchive.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelArchive.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelArchive.Name = "tableLayoutPanelArchive";
            tableLayoutPanelArchive.RowCount = 9;
            tableLayoutPanelArchive.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelArchive.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelArchive.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelArchive.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelArchive.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelArchive.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelArchive.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelArchive.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelArchive.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelArchive.Size = new System.Drawing.Size(128, 66);
            tableLayoutPanelArchive.TabIndex = 0;
            // 
            // labelArchiveTransfer
            // 
            labelArchiveTransfer.AutoSize = true;
            tableLayoutPanelArchive.SetColumnSpan(labelArchiveTransfer, 4);
            labelArchiveTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            labelArchiveTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelArchiveTransfer.Location = new System.Drawing.Point(0, 0);
            labelArchiveTransfer.Margin = new System.Windows.Forms.Padding(0);
            labelArchiveTransfer.Name = "labelArchiveTransfer";
            labelArchiveTransfer.Size = new System.Drawing.Size(88, 45);
            labelArchiveTransfer.TabIndex = 0;
            labelArchiveTransfer.Text = "Open Archive Information Package";
            labelArchiveTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxArchiveMimeType
            // 
            comboBoxArchiveMimeType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", projectArchiveBindingSource, "MimeType", true));
            comboBoxArchiveMimeType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxArchiveMimeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxArchiveMimeType.FormattingEnabled = true;
            comboBoxArchiveMimeType.Location = new System.Drawing.Point(64, 228);
            comboBoxArchiveMimeType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxArchiveMimeType.Name = "comboBoxArchiveMimeType";
            comboBoxArchiveMimeType.Size = new System.Drawing.Size(6, 23);
            comboBoxArchiveMimeType.TabIndex = 5;
            // 
            // projectArchiveBindingSource
            // 
            projectArchiveBindingSource.DataMember = "ProjectArchive";
            projectArchiveBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // comboBoxArchiveFormat
            // 
            tableLayoutPanelArchive.SetColumnSpan(comboBoxArchiveFormat, 2);
            comboBoxArchiveFormat.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", projectArchiveBindingSource, "Format", true));
            comboBoxArchiveFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxArchiveFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxArchiveFormat.FormattingEnabled = true;
            comboBoxArchiveFormat.Location = new System.Drawing.Point(4, 228);
            comboBoxArchiveFormat.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxArchiveFormat.Name = "comboBoxArchiveFormat";
            comboBoxArchiveFormat.Size = new System.Drawing.Size(52, 23);
            comboBoxArchiveFormat.TabIndex = 3;
            // 
            // labelArchiveFileSize
            // 
            labelArchiveFileSize.AutoSize = true;
            labelArchiveFileSize.Location = new System.Drawing.Point(78, 45);
            labelArchiveFileSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelArchiveFileSize.Name = "labelArchiveFileSize";
            labelArchiveFileSize.Size = new System.Drawing.Size(6, 180);
            labelArchiveFileSize.TabIndex = 6;
            labelArchiveFileSize.Text = "Size (in bytes)";
            // 
            // textBoxArchiveFileSize
            // 
            textBoxArchiveFileSize.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectArchiveBindingSource, "FileSize", true));
            textBoxArchiveFileSize.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxArchiveFileSize.Location = new System.Drawing.Point(78, 228);
            textBoxArchiveFileSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxArchiveFileSize.Name = "textBoxArchiveFileSize";
            textBoxArchiveFileSize.Size = new System.Drawing.Size(6, 23);
            textBoxArchiveFileSize.TabIndex = 7;
            // 
            // labelArchiveFileHash
            // 
            labelArchiveFileHash.AutoSize = true;
            labelArchiveFileHash.Location = new System.Drawing.Point(92, 45);
            labelArchiveFileHash.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelArchiveFileHash.Name = "labelArchiveFileHash";
            labelArchiveFileHash.Size = new System.Drawing.Size(3, 150);
            labelArchiveFileHash.TabIndex = 8;
            labelArchiveFileHash.Text = "SHA512 Hash";
            // 
            // textBoxArchiveFileHash
            // 
            tableLayoutPanelArchive.SetColumnSpan(textBoxArchiveFileHash, 2);
            textBoxArchiveFileHash.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectArchiveBindingSource, "FileSHA512Hash", true));
            textBoxArchiveFileHash.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxArchiveFileHash.Location = new System.Drawing.Point(92, 228);
            textBoxArchiveFileHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxArchiveFileHash.Name = "textBoxArchiveFileHash";
            textBoxArchiveFileHash.Size = new System.Drawing.Size(32, 23);
            textBoxArchiveFileHash.TabIndex = 9;
            // 
            // labelArchiveStorageLocation
            // 
            labelArchiveStorageLocation.AutoSize = true;
            labelArchiveStorageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            labelArchiveStorageLocation.Location = new System.Drawing.Point(0, 254);
            labelArchiveStorageLocation.Margin = new System.Windows.Forms.Padding(0);
            labelArchiveStorageLocation.Name = "labelArchiveStorageLocation";
            labelArchiveStorageLocation.Size = new System.Drawing.Size(53, 29);
            labelArchiveStorageLocation.TabIndex = 10;
            labelArchiveStorageLocation.Text = "Storage";
            labelArchiveStorageLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelArchiveExternalStorageLocation
            // 
            labelArchiveExternalStorageLocation.AutoSize = true;
            labelArchiveExternalStorageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            labelArchiveExternalStorageLocation.Location = new System.Drawing.Point(0, 283);
            labelArchiveExternalStorageLocation.Margin = new System.Windows.Forms.Padding(0);
            labelArchiveExternalStorageLocation.Name = "labelArchiveExternalStorageLocation";
            labelArchiveExternalStorageLocation.Size = new System.Drawing.Size(53, 29);
            labelArchiveExternalStorageLocation.TabIndex = 13;
            labelArchiveExternalStorageLocation.Text = "Ext. stor.";
            labelArchiveExternalStorageLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelArchiveTransferDate
            // 
            labelArchiveTransferDate.AutoSize = true;
            tableLayoutPanelArchive.SetColumnSpan(labelArchiveTransferDate, 2);
            labelArchiveTransferDate.Dock = System.Windows.Forms.DockStyle.Bottom;
            labelArchiveTransferDate.Location = new System.Drawing.Point(4, 507);
            labelArchiveTransferDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelArchiveTransferDate.Name = "labelArchiveTransferDate";
            labelArchiveTransferDate.Size = new System.Drawing.Size(52, 30);
            labelArchiveTransferDate.TabIndex = 16;
            labelArchiveTransferDate.Text = "Transfer date";
            // 
            // dateTimePickerArchiveTransferDate
            // 
            tableLayoutPanelArchive.SetColumnSpan(dateTimePickerArchiveTransferDate, 2);
            dateTimePickerArchiveTransferDate.CustomFormat = "--";
            dateTimePickerArchiveTransferDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", projectArchiveBindingSource, "TransferDate", true));
            dateTimePickerArchiveTransferDate.Dock = System.Windows.Forms.DockStyle.Fill;
            dateTimePickerArchiveTransferDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dateTimePickerArchiveTransferDate.Location = new System.Drawing.Point(4, 540);
            dateTimePickerArchiveTransferDate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dateTimePickerArchiveTransferDate.Name = "dateTimePickerArchiveTransferDate";
            dateTimePickerArchiveTransferDate.Size = new System.Drawing.Size(52, 23);
            dateTimePickerArchiveTransferDate.TabIndex = 17;
            dateTimePickerArchiveTransferDate.CloseUp += dateTimePickerArchiveTransferDate_CloseUp;
            // 
            // labelArchivePublicationDate
            // 
            labelArchivePublicationDate.AutoSize = true;
            labelArchivePublicationDate.Dock = System.Windows.Forms.DockStyle.Bottom;
            labelArchivePublicationDate.Location = new System.Drawing.Point(64, 342);
            labelArchivePublicationDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelArchivePublicationDate.Name = "labelArchivePublicationDate";
            labelArchivePublicationDate.Size = new System.Drawing.Size(6, 195);
            labelArchivePublicationDate.TabIndex = 20;
            labelArchivePublicationDate.Text = "Publication date";
            // 
            // dateTimePickerArchivePublicationDate
            // 
            dateTimePickerArchivePublicationDate.CustomFormat = "--";
            dateTimePickerArchivePublicationDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", projectArchiveBindingSource, "PublicationDate", true));
            dateTimePickerArchivePublicationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            dateTimePickerArchivePublicationDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dateTimePickerArchivePublicationDate.Location = new System.Drawing.Point(64, 540);
            dateTimePickerArchivePublicationDate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dateTimePickerArchivePublicationDate.Name = "dateTimePickerArchivePublicationDate";
            dateTimePickerArchivePublicationDate.Size = new System.Drawing.Size(6, 23);
            dateTimePickerArchivePublicationDate.TabIndex = 21;
            dateTimePickerArchivePublicationDate.CloseUp += dateTimePickerArchivePublicationDate_CloseUp;
            // 
            // labelArchiveDataWithholdingReason
            // 
            labelArchiveDataWithholdingReason.AutoSize = true;
            labelArchiveDataWithholdingReason.Dock = System.Windows.Forms.DockStyle.Bottom;
            labelArchiveDataWithholdingReason.Location = new System.Drawing.Point(92, 447);
            labelArchiveDataWithholdingReason.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelArchiveDataWithholdingReason.Name = "labelArchiveDataWithholdingReason";
            labelArchiveDataWithholdingReason.Size = new System.Drawing.Size(3, 90);
            labelArchiveDataWithholdingReason.TabIndex = 23;
            labelArchiveDataWithholdingReason.Text = "Withhold";
            // 
            // pictureBoxArchiveDataWithholdingReason
            // 
            pictureBoxArchiveDataWithholdingReason.Image = Properties.Resources.Stop3;
            pictureBoxArchiveDataWithholdingReason.Location = new System.Drawing.Point(99, 312);
            pictureBoxArchiveDataWithholdingReason.Margin = new System.Windows.Forms.Padding(0);
            pictureBoxArchiveDataWithholdingReason.Name = "pictureBoxArchiveDataWithholdingReason";
            pictureBoxArchiveDataWithholdingReason.Size = new System.Drawing.Size(19, 18);
            pictureBoxArchiveDataWithholdingReason.TabIndex = 24;
            pictureBoxArchiveDataWithholdingReason.TabStop = false;
            // 
            // textBoxArchiveDataWithholdingReason
            // 
            tableLayoutPanelArchive.SetColumnSpan(textBoxArchiveDataWithholdingReason, 2);
            textBoxArchiveDataWithholdingReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectArchiveBindingSource, "DataWithholdingReason", true));
            textBoxArchiveDataWithholdingReason.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxArchiveDataWithholdingReason.Location = new System.Drawing.Point(92, 540);
            textBoxArchiveDataWithholdingReason.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxArchiveDataWithholdingReason.Name = "textBoxArchiveDataWithholdingReason";
            textBoxArchiveDataWithholdingReason.Size = new System.Drawing.Size(32, 23);
            textBoxArchiveDataWithholdingReason.TabIndex = 25;
            textBoxArchiveDataWithholdingReason.TextChanged += textBoxArchiveDataWithholdingReason_TextChanged;
            // 
            // buttonArchiveStorageLocation
            // 
            buttonArchiveStorageLocation.FlatAppearance.BorderSize = 0;
            buttonArchiveStorageLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonArchiveStorageLocation.Image = Properties.Resources.OpenFolder;
            buttonArchiveStorageLocation.Location = new System.Drawing.Point(99, 257);
            buttonArchiveStorageLocation.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonArchiveStorageLocation.Name = "buttonArchiveStorageLocation";
            buttonArchiveStorageLocation.Size = new System.Drawing.Size(21, 21);
            buttonArchiveStorageLocation.TabIndex = 12;
            buttonArchiveStorageLocation.UseVisualStyleBackColor = true;
            buttonArchiveStorageLocation.Click += buttonArchiveStorageLocation_Click;
            // 
            // buttonArchiveExternalStorageLocation
            // 
            buttonArchiveExternalStorageLocation.FlatAppearance.BorderSize = 0;
            buttonArchiveExternalStorageLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonArchiveExternalStorageLocation.Image = Properties.Resources.OpenFolder;
            buttonArchiveExternalStorageLocation.Location = new System.Drawing.Point(99, 286);
            buttonArchiveExternalStorageLocation.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonArchiveExternalStorageLocation.Name = "buttonArchiveExternalStorageLocation";
            buttonArchiveExternalStorageLocation.Size = new System.Drawing.Size(21, 21);
            buttonArchiveExternalStorageLocation.TabIndex = 15;
            buttonArchiveExternalStorageLocation.UseVisualStyleBackColor = true;
            buttonArchiveExternalStorageLocation.Click += buttonArchiveExternalStorageLocation_Click;
            // 
            // checkBoxArchivePublished
            // 
            checkBoxArchivePublished.AutoSize = true;
            checkBoxArchivePublished.DataBindings.Add(new System.Windows.Forms.Binding("Checked", projectArchiveBindingSource, "Published", true));
            checkBoxArchivePublished.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxArchivePublished.Location = new System.Drawing.Point(88, 0);
            checkBoxArchivePublished.Margin = new System.Windows.Forms.Padding(0);
            checkBoxArchivePublished.Name = "checkBoxArchivePublished";
            checkBoxArchivePublished.Size = new System.Drawing.Size(11, 45);
            checkBoxArchivePublished.TabIndex = 22;
            checkBoxArchivePublished.Text = "Published";
            checkBoxArchivePublished.UseVisualStyleBackColor = true;
            checkBoxArchivePublished.Click += checkBoxArchivePublished_Click;
            // 
            // labelArchiveNotes
            // 
            labelArchiveNotes.AutoSize = true;
            labelArchiveNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelArchiveNotes.Location = new System.Drawing.Point(4, 566);
            labelArchiveNotes.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelArchiveNotes.Name = "labelArchiveNotes";
            labelArchiveNotes.Size = new System.Drawing.Size(49, 120);
            labelArchiveNotes.TabIndex = 26;
            labelArchiveNotes.Text = "Notes";
            labelArchiveNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxArchiveNotes
            // 
            tableLayoutPanelArchive.SetColumnSpan(textBoxArchiveNotes, 3);
            textBoxArchiveNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectArchiveBindingSource, "Notes", true));
            textBoxArchiveNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxArchiveNotes.Location = new System.Drawing.Point(0, 689);
            textBoxArchiveNotes.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            textBoxArchiveNotes.Multiline = true;
            textBoxArchiveNotes.Name = "textBoxArchiveNotes";
            textBoxArchiveNotes.Size = new System.Drawing.Size(70, 1);
            textBoxArchiveNotes.TabIndex = 27;
            // 
            // textBoxArchiveStorageLocation
            // 
            tableLayoutPanelArchive.SetColumnSpan(textBoxArchiveStorageLocation, 4);
            textBoxArchiveStorageLocation.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectArchiveBindingSource, "StorageLocation", true));
            textBoxArchiveStorageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxArchiveStorageLocation.Location = new System.Drawing.Point(53, 257);
            textBoxArchiveStorageLocation.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            textBoxArchiveStorageLocation.Name = "textBoxArchiveStorageLocation";
            textBoxArchiveStorageLocation.Size = new System.Drawing.Size(46, 23);
            textBoxArchiveStorageLocation.TabIndex = 11;
            // 
            // textBoxArchiveExternalStorageLocation
            // 
            tableLayoutPanelArchive.SetColumnSpan(textBoxArchiveExternalStorageLocation, 4);
            textBoxArchiveExternalStorageLocation.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectArchiveBindingSource, "ExternalStorageLocation", true));
            textBoxArchiveExternalStorageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxArchiveExternalStorageLocation.Location = new System.Drawing.Point(53, 286);
            textBoxArchiveExternalStorageLocation.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            textBoxArchiveExternalStorageLocation.Name = "textBoxArchiveExternalStorageLocation";
            textBoxArchiveExternalStorageLocation.Size = new System.Drawing.Size(46, 23);
            textBoxArchiveExternalStorageLocation.TabIndex = 14;
            // 
            // pictureBoxArchivePublished
            // 
            pictureBoxArchivePublished.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxArchivePublished.Image = Properties.Resources.ArchivOpen;
            pictureBoxArchivePublished.Location = new System.Drawing.Point(99, 3);
            pictureBoxArchivePublished.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            pictureBoxArchivePublished.Name = "pictureBoxArchivePublished";
            pictureBoxArchivePublished.Size = new System.Drawing.Size(29, 42);
            pictureBoxArchivePublished.TabIndex = 28;
            pictureBoxArchivePublished.TabStop = false;
            // 
            // linkLabelArchiveFormat
            // 
            linkLabelArchiveFormat.AutoSize = true;
            linkLabelArchiveFormat.Location = new System.Drawing.Point(4, 45);
            linkLabelArchiveFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelArchiveFormat.Name = "linkLabelArchiveFormat";
            linkLabelArchiveFormat.Size = new System.Drawing.Size(45, 15);
            linkLabelArchiveFormat.TabIndex = 29;
            linkLabelArchiveFormat.TabStop = true;
            linkLabelArchiveFormat.Text = "Format";
            linkLabelArchiveFormat.LinkClicked += linkLabelArchiveFormat_LinkClicked;
            // 
            // linkLabelArchiveMimeType
            // 
            linkLabelArchiveMimeType.AutoSize = true;
            linkLabelArchiveMimeType.Location = new System.Drawing.Point(64, 45);
            linkLabelArchiveMimeType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelArchiveMimeType.Name = "linkLabelArchiveMimeType";
            linkLabelArchiveMimeType.Size = new System.Drawing.Size(6, 105);
            linkLabelArchiveMimeType.TabIndex = 30;
            linkLabelArchiveMimeType.TabStop = true;
            linkLabelArchiveMimeType.Text = "Mime type";
            linkLabelArchiveMimeType.LinkClicked += linkLabelArchiveMimeType_LinkClicked;
            // 
            // labelArchiveIdentifier
            // 
            labelArchiveIdentifier.AutoSize = true;
            labelArchiveIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            labelArchiveIdentifier.Location = new System.Drawing.Point(78, 566);
            labelArchiveIdentifier.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelArchiveIdentifier.Name = "labelArchiveIdentifier";
            labelArchiveIdentifier.Size = new System.Drawing.Size(6, 120);
            labelArchiveIdentifier.TabIndex = 31;
            labelArchiveIdentifier.Text = "Identifier";
            labelArchiveIdentifier.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // toolStripArchiveIdentier
            // 
            toolStripArchiveIdentier.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonArchiveIdentierAdd, toolStripButtonArchiveIdentierDelete });
            toolStripArchiveIdentier.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            toolStripArchiveIdentier.Location = new System.Drawing.Point(99, 689);
            toolStripArchiveIdentier.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            toolStripArchiveIdentier.Name = "toolStripArchiveIdentier";
            toolStripArchiveIdentier.Size = new System.Drawing.Size(29, 1);
            toolStripArchiveIdentier.TabIndex = 32;
            toolStripArchiveIdentier.Text = "toolStrip1";
            // 
            // toolStripButtonArchiveIdentierAdd
            // 
            toolStripButtonArchiveIdentierAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonArchiveIdentierAdd.Image = Properties.Resources.Add;
            toolStripButtonArchiveIdentierAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonArchiveIdentierAdd.Name = "toolStripButtonArchiveIdentierAdd";
            toolStripButtonArchiveIdentierAdd.Size = new System.Drawing.Size(23, 20);
            toolStripButtonArchiveIdentierAdd.Text = "Add an identifier for the archive";
            toolStripButtonArchiveIdentierAdd.Click += toolStripButtonArchiveIdentierAdd_Click;
            // 
            // toolStripButtonArchiveIdentierDelete
            // 
            toolStripButtonArchiveIdentierDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonArchiveIdentierDelete.Image = Properties.Resources.Delete;
            toolStripButtonArchiveIdentierDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonArchiveIdentierDelete.Name = "toolStripButtonArchiveIdentierDelete";
            toolStripButtonArchiveIdentierDelete.Size = new System.Drawing.Size(23, 20);
            toolStripButtonArchiveIdentierDelete.Text = "Delete the selected identier for the archive";
            toolStripButtonArchiveIdentierDelete.Click += toolStripButtonArchiveIdentierDelete_Click;
            // 
            // listBoxArchiveIdentier
            // 
            tableLayoutPanelArchive.SetColumnSpan(listBoxArchiveIdentier, 2);
            listBoxArchiveIdentier.DataSource = projectArchiveIdentifierBindingSource;
            listBoxArchiveIdentier.DisplayMember = "Identifier";
            listBoxArchiveIdentier.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxArchiveIdentier.FormattingEnabled = true;
            listBoxArchiveIdentier.IntegralHeight = false;
            listBoxArchiveIdentier.ItemHeight = 15;
            listBoxArchiveIdentier.Location = new System.Drawing.Point(78, 689);
            listBoxArchiveIdentier.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            listBoxArchiveIdentier.Name = "listBoxArchiveIdentier";
            listBoxArchiveIdentier.Size = new System.Drawing.Size(21, 1);
            listBoxArchiveIdentier.TabIndex = 33;
            listBoxArchiveIdentier.ValueMember = "Identifier";
            // 
            // projectArchiveIdentifierBindingSource
            // 
            projectArchiveIdentifierBindingSource.DataMember = "ProjectArchiveIdentifier";
            projectArchiveIdentifierBindingSource.DataSource = dataSetDiversityProjects;
            // 
            // labelArchiveParentRelationType
            // 
            labelArchiveParentRelationType.AutoSize = true;
            labelArchiveParentRelationType.Dock = System.Windows.Forms.DockStyle.Fill;
            labelArchiveParentRelationType.Location = new System.Drawing.Point(78, 312);
            labelArchiveParentRelationType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelArchiveParentRelationType.Name = "labelArchiveParentRelationType";
            labelArchiveParentRelationType.Size = new System.Drawing.Size(6, 225);
            labelArchiveParentRelationType.TabIndex = 34;
            labelArchiveParentRelationType.Text = "Relation to parent";
            labelArchiveParentRelationType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxArchiveParentRelationType
            // 
            comboBoxArchiveParentRelationType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", projectArchiveBindingSource, "ParentRelationType", true));
            comboBoxArchiveParentRelationType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxArchiveParentRelationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxArchiveParentRelationType.FormattingEnabled = true;
            comboBoxArchiveParentRelationType.Location = new System.Drawing.Point(78, 540);
            comboBoxArchiveParentRelationType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxArchiveParentRelationType.Name = "comboBoxArchiveParentRelationType";
            comboBoxArchiveParentRelationType.Size = new System.Drawing.Size(6, 23);
            comboBoxArchiveParentRelationType.TabIndex = 35;
            // 
            // tabPageModules
            // 
            tabPageModules.Controls.Add(tabControlModules);
            tabPageModules.ImageIndex = 10;
            tabPageModules.Location = new System.Drawing.Point(4, 24);
            tabPageModules.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageModules.Name = "tabPageModules";
            tabPageModules.Size = new System.Drawing.Size(192, 72);
            tabPageModules.TabIndex = 10;
            tabPageModules.Text = "Modules";
            tabPageModules.UseVisualStyleBackColor = true;
            // 
            // tabControlModules
            // 
            tabControlModules.Controls.Add(tabPageDiversityAgents);
            tabControlModules.Controls.Add(tabPageDiversityCollection);
            tabControlModules.Controls.Add(tabPageDiversityDescriptions);
            tabControlModules.Controls.Add(tabPageDiversityGazetteer);
            tabControlModules.Controls.Add(tabPageDiversityReferences);
            tabControlModules.Controls.Add(tabPageDiversitySamplingPlots);
            tabControlModules.Controls.Add(tabPageDiversityScientificTerms);
            tabControlModules.Controls.Add(tabPageDiversityTaxonNames);
            tabControlModules.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlModules.Location = new System.Drawing.Point(0, 0);
            tabControlModules.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControlModules.Name = "tabControlModules";
            tabControlModules.SelectedIndex = 0;
            tabControlModules.Size = new System.Drawing.Size(192, 72);
            tabControlModules.TabIndex = 0;
            // 
            // tabPageDiversityAgents
            // 
            tabPageDiversityAgents.Controls.Add(userControlModuleBackLinksAgents);
            tabPageDiversityAgents.Location = new System.Drawing.Point(4, 24);
            tabPageDiversityAgents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDiversityAgents.Name = "tabPageDiversityAgents";
            tabPageDiversityAgents.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDiversityAgents.Size = new System.Drawing.Size(184, 44);
            tabPageDiversityAgents.TabIndex = 0;
            tabPageDiversityAgents.Text = "Agents";
            tabPageDiversityAgents.UseVisualStyleBackColor = true;
            // 
            // userControlModuleBackLinksAgents
            // 
            userControlModuleBackLinksAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleBackLinksAgents.Location = new System.Drawing.Point(4, 3);
            userControlModuleBackLinksAgents.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleBackLinksAgents.Name = "userControlModuleBackLinksAgents";
            userControlModuleBackLinksAgents.Size = new System.Drawing.Size(176, 38);
            userControlModuleBackLinksAgents.TabIndex = 0;
            // 
            // tabPageDiversityCollection
            // 
            tabPageDiversityCollection.Controls.Add(userControlModuleBackLinksCollection);
            tabPageDiversityCollection.Location = new System.Drawing.Point(4, 24);
            tabPageDiversityCollection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDiversityCollection.Name = "tabPageDiversityCollection";
            tabPageDiversityCollection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDiversityCollection.Size = new System.Drawing.Size(903, 254);
            tabPageDiversityCollection.TabIndex = 1;
            tabPageDiversityCollection.Text = "Collection";
            tabPageDiversityCollection.UseVisualStyleBackColor = true;
            // 
            // userControlModuleBackLinksCollection
            // 
            userControlModuleBackLinksCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleBackLinksCollection.Location = new System.Drawing.Point(4, 3);
            userControlModuleBackLinksCollection.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleBackLinksCollection.Name = "userControlModuleBackLinksCollection";
            userControlModuleBackLinksCollection.Size = new System.Drawing.Size(895, 248);
            userControlModuleBackLinksCollection.TabIndex = 0;
            // 
            // tabPageDiversityDescriptions
            // 
            tabPageDiversityDescriptions.Controls.Add(userControlModuleBackLinksDescriptions);
            tabPageDiversityDescriptions.Location = new System.Drawing.Point(4, 24);
            tabPageDiversityDescriptions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDiversityDescriptions.Name = "tabPageDiversityDescriptions";
            tabPageDiversityDescriptions.Size = new System.Drawing.Size(184, 44);
            tabPageDiversityDescriptions.TabIndex = 2;
            tabPageDiversityDescriptions.Text = "Descriptions";
            tabPageDiversityDescriptions.UseVisualStyleBackColor = true;
            // 
            // userControlModuleBackLinksDescriptions
            // 
            userControlModuleBackLinksDescriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleBackLinksDescriptions.Location = new System.Drawing.Point(0, 0);
            userControlModuleBackLinksDescriptions.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleBackLinksDescriptions.Name = "userControlModuleBackLinksDescriptions";
            userControlModuleBackLinksDescriptions.Size = new System.Drawing.Size(184, 44);
            userControlModuleBackLinksDescriptions.TabIndex = 0;
            // 
            // tabPageDiversityGazetteer
            // 
            tabPageDiversityGazetteer.Controls.Add(userControlModuleBackLinksGazetteer);
            tabPageDiversityGazetteer.Location = new System.Drawing.Point(4, 24);
            tabPageDiversityGazetteer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDiversityGazetteer.Name = "tabPageDiversityGazetteer";
            tabPageDiversityGazetteer.Size = new System.Drawing.Size(184, 44);
            tabPageDiversityGazetteer.TabIndex = 3;
            tabPageDiversityGazetteer.Text = "Gazetteer";
            tabPageDiversityGazetteer.UseVisualStyleBackColor = true;
            // 
            // userControlModuleBackLinksGazetteer
            // 
            userControlModuleBackLinksGazetteer.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleBackLinksGazetteer.Location = new System.Drawing.Point(0, 0);
            userControlModuleBackLinksGazetteer.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleBackLinksGazetteer.Name = "userControlModuleBackLinksGazetteer";
            userControlModuleBackLinksGazetteer.Size = new System.Drawing.Size(184, 44);
            userControlModuleBackLinksGazetteer.TabIndex = 0;
            // 
            // tabPageDiversityReferences
            // 
            tabPageDiversityReferences.Controls.Add(userControlModuleBackLinksReferences);
            tabPageDiversityReferences.Location = new System.Drawing.Point(4, 24);
            tabPageDiversityReferences.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDiversityReferences.Name = "tabPageDiversityReferences";
            tabPageDiversityReferences.Size = new System.Drawing.Size(184, 44);
            tabPageDiversityReferences.TabIndex = 4;
            tabPageDiversityReferences.Text = "References";
            tabPageDiversityReferences.UseVisualStyleBackColor = true;
            // 
            // userControlModuleBackLinksReferences
            // 
            userControlModuleBackLinksReferences.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleBackLinksReferences.Location = new System.Drawing.Point(0, 0);
            userControlModuleBackLinksReferences.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleBackLinksReferences.Name = "userControlModuleBackLinksReferences";
            userControlModuleBackLinksReferences.Size = new System.Drawing.Size(184, 44);
            userControlModuleBackLinksReferences.TabIndex = 0;
            // 
            // tabPageDiversitySamplingPlots
            // 
            tabPageDiversitySamplingPlots.Controls.Add(userControlModuleBackLinksPlots);
            tabPageDiversitySamplingPlots.Location = new System.Drawing.Point(4, 24);
            tabPageDiversitySamplingPlots.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDiversitySamplingPlots.Name = "tabPageDiversitySamplingPlots";
            tabPageDiversitySamplingPlots.Size = new System.Drawing.Size(184, 44);
            tabPageDiversitySamplingPlots.TabIndex = 5;
            tabPageDiversitySamplingPlots.Text = "Plots";
            tabPageDiversitySamplingPlots.UseVisualStyleBackColor = true;
            // 
            // userControlModuleBackLinksPlots
            // 
            userControlModuleBackLinksPlots.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleBackLinksPlots.Location = new System.Drawing.Point(0, 0);
            userControlModuleBackLinksPlots.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleBackLinksPlots.Name = "userControlModuleBackLinksPlots";
            userControlModuleBackLinksPlots.Size = new System.Drawing.Size(184, 44);
            userControlModuleBackLinksPlots.TabIndex = 0;
            // 
            // tabPageDiversityScientificTerms
            // 
            tabPageDiversityScientificTerms.Controls.Add(userControlModuleBackLinksTerms);
            tabPageDiversityScientificTerms.Location = new System.Drawing.Point(4, 24);
            tabPageDiversityScientificTerms.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDiversityScientificTerms.Name = "tabPageDiversityScientificTerms";
            tabPageDiversityScientificTerms.Size = new System.Drawing.Size(184, 44);
            tabPageDiversityScientificTerms.TabIndex = 6;
            tabPageDiversityScientificTerms.Text = "Terms";
            tabPageDiversityScientificTerms.UseVisualStyleBackColor = true;
            // 
            // userControlModuleBackLinksTerms
            // 
            userControlModuleBackLinksTerms.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleBackLinksTerms.Location = new System.Drawing.Point(0, 0);
            userControlModuleBackLinksTerms.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleBackLinksTerms.Name = "userControlModuleBackLinksTerms";
            userControlModuleBackLinksTerms.Size = new System.Drawing.Size(184, 44);
            userControlModuleBackLinksTerms.TabIndex = 0;
            // 
            // tabPageDiversityTaxonNames
            // 
            tabPageDiversityTaxonNames.Controls.Add(userControlModuleBackLinksTaxa);
            tabPageDiversityTaxonNames.Location = new System.Drawing.Point(4, 24);
            tabPageDiversityTaxonNames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDiversityTaxonNames.Name = "tabPageDiversityTaxonNames";
            tabPageDiversityTaxonNames.Size = new System.Drawing.Size(184, 44);
            tabPageDiversityTaxonNames.TabIndex = 7;
            tabPageDiversityTaxonNames.Text = "Taxa";
            tabPageDiversityTaxonNames.UseVisualStyleBackColor = true;
            // 
            // userControlModuleBackLinksTaxa
            // 
            userControlModuleBackLinksTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleBackLinksTaxa.Location = new System.Drawing.Point(0, 0);
            userControlModuleBackLinksTaxa.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleBackLinksTaxa.Name = "userControlModuleBackLinksTaxa";
            userControlModuleBackLinksTaxa.Size = new System.Drawing.Size(184, 44);
            userControlModuleBackLinksTaxa.TabIndex = 0;
            // 
            // imageListTabcontrol
            // 
            imageListTabcontrol.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            imageListTabcontrol.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListTabcontrol.ImageStream");
            imageListTabcontrol.TransparentColor = System.Drawing.Color.Transparent;
            imageListTabcontrol.Images.SetKeyName(0, "Agent.ico");
            imageListTabcontrol.Images.SetKeyName(1, "Settings.ico");
            imageListTabcontrol.Images.SetKeyName(2, "References.ico");
            imageListTabcontrol.Images.SetKeyName(3, "Document.ICO");
            imageListTabcontrol.Images.SetKeyName(4, "Resources_2.ico");
            imageListTabcontrol.Images.SetKeyName(5, "Identifier.ico");
            imageListTabcontrol.Images.SetKeyName(6, "KeyBig.ico");
            imageListTabcontrol.Images.SetKeyName(7, "KeyBlue.ico");
            imageListTabcontrol.Images.SetKeyName(8, "Resources.ico");
            imageListTabcontrol.Images.SetKeyName(9, "OAIS_16.ico");
            imageListTabcontrol.Images.SetKeyName(10, "Workbench.ico");
            // 
            // toolStripButtonApplicationsNew
            // 
            toolStripButtonApplicationsNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonApplicationsNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonApplicationsNew.Name = "toolStripButtonApplicationsNew";
            toolStripButtonApplicationsNew.Size = new System.Drawing.Size(23, 20);
            toolStripButtonApplicationsNew.Text = "Add a new application";
            // 
            // toolStripButtonApplicationsDelete
            // 
            toolStripButtonApplicationsDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonApplicationsDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonApplicationsDelete.Name = "toolStripButtonApplicationsDelete";
            toolStripButtonApplicationsDelete.Size = new System.Drawing.Size(23, 20);
            toolStripButtonApplicationsDelete.Text = "Remove the selected application";
            // 
            // menuStripMain
            // 
            menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { connectionToolStripMenuItem, dataToolStripMenuItem, administrationToolStripMenuItem, helpToolStripMenuItem, updateToolStripMenuItem });
            menuStripMain.Location = new System.Drawing.Point(0, 0);
            menuStripMain.Name = "menuStripMain";
            menuStripMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStripMain.Size = new System.Drawing.Size(1171, 24);
            menuStripMain.TabIndex = 2;
            menuStripMain.Text = "menuStrip1";
            // 
            // connectionToolStripMenuItem
            // 
            connectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { createDatabaseToolStripMenuItem, databaseToolStripMenuItem, moduleConnectionsToolStripMenuItem, queryTimeoutToolStripMenuItem, connectionTimeoutToolStripMenuItem, toolStripSeparatorTransfer, transferPreviousSettingsToolStripMenuItem, currentServerActivityToolStripMenuItem, toolStripMenuItemSeparatorQuit, askOnExitToolStripMenuItem, quitToolStripMenuItem });
            connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            connectionToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            connectionToolStripMenuItem.Text = "Connection";
            // 
            // createDatabaseToolStripMenuItem
            // 
            createDatabaseToolStripMenuItem.Image = Properties.Resources.DatabaseAdd;
            createDatabaseToolStripMenuItem.Name = "createDatabaseToolStripMenuItem";
            createDatabaseToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            createDatabaseToolStripMenuItem.Text = "Create database...";
            createDatabaseToolStripMenuItem.Click += createDatabaseToolStripMenuItem_Click;
            // 
            // databaseToolStripMenuItem
            // 
            databaseToolStripMenuItem.Image = Properties.Resources.Database;
            databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            databaseToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            databaseToolStripMenuItem.Text = "Database ...";
            databaseToolStripMenuItem.Click += databaseToolStripMenuItem_Click;
            // 
            // moduleConnectionsToolStripMenuItem
            // 
            moduleConnectionsToolStripMenuItem.Image = Properties.Resources.DataSource;
            moduleConnectionsToolStripMenuItem.Name = "moduleConnectionsToolStripMenuItem";
            moduleConnectionsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            moduleConnectionsToolStripMenuItem.Text = "Module connections ...";
            moduleConnectionsToolStripMenuItem.Click += moduleConnectionsToolStripMenuItem_Click;
            // 
            // queryTimeoutToolStripMenuItem
            // 
            queryTimeoutToolStripMenuItem.Image = Properties.Resources.Time;
            queryTimeoutToolStripMenuItem.Name = "queryTimeoutToolStripMenuItem";
            queryTimeoutToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            queryTimeoutToolStripMenuItem.Text = "Query timeout";
            queryTimeoutToolStripMenuItem.Click += queryTimeoutToolStripMenuItem_Click;
            // 
            // connectionTimeoutToolStripMenuItem
            // 
            connectionTimeoutToolStripMenuItem.Image = Properties.Resources.Time;
            connectionTimeoutToolStripMenuItem.Name = "connectionTimeoutToolStripMenuItem";
            connectionTimeoutToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            connectionTimeoutToolStripMenuItem.Text = "Connection timeout";
            connectionTimeoutToolStripMenuItem.Click += connectionTimeoutToolStripMenuItem_Click;
            // 
            // toolStripSeparatorTransfer
            // 
            toolStripSeparatorTransfer.Name = "toolStripSeparatorTransfer";
            toolStripSeparatorTransfer.Size = new System.Drawing.Size(204, 6);
            // 
            // transferPreviousSettingsToolStripMenuItem
            // 
            transferPreviousSettingsToolStripMenuItem.Image = Properties.Resources.Settings;
            transferPreviousSettingsToolStripMenuItem.Name = "transferPreviousSettingsToolStripMenuItem";
            transferPreviousSettingsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            transferPreviousSettingsToolStripMenuItem.Text = "Transfer previous settings";
            transferPreviousSettingsToolStripMenuItem.Click += transferPreviousSettingsToolStripMenuItem_Click;
            // 
            // currentServerActivityToolStripMenuItem
            // 
            currentServerActivityToolStripMenuItem.Image = Properties.Resources.ServerIO;
            currentServerActivityToolStripMenuItem.Name = "currentServerActivityToolStripMenuItem";
            currentServerActivityToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            currentServerActivityToolStripMenuItem.Text = "Current server activity ...";
            currentServerActivityToolStripMenuItem.Click += currentServerActivityToolStripMenuItem_Click;
            // 
            // toolStripMenuItemSeparatorQuit
            // 
            toolStripMenuItemSeparatorQuit.Name = "toolStripMenuItemSeparatorQuit";
            toolStripMenuItemSeparatorQuit.Size = new System.Drawing.Size(204, 6);
            // 
            // askOnExitToolStripMenuItem
            // 
            askOnExitToolStripMenuItem.CheckOnClick = true;
            askOnExitToolStripMenuItem.Name = "askOnExitToolStripMenuItem";
            askOnExitToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            askOnExitToolStripMenuItem.Text = "Ask on exit";
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.Image = Properties.Resources.Exit;
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            quitToolStripMenuItem.Text = "Quit";
            quitToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // dataToolStripMenuItem
            // 
            dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { exportToolStripMenuItem, importToolStripMenuItem, tableEditorToolStripMenuItem, backupDatabaseToolStripMenuItem, xMLarchiveToolStripMenuItem, scanModulesToolStripMenuItem });
            dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            dataToolStripMenuItem.Text = "Data";
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { exportWizardToolStripMenuItem, exportDatabaseAsCsvbcpToolStripMenuItem });
            exportToolStripMenuItem.Image = Properties.Resources.Export;
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            exportToolStripMenuItem.Text = "Export";
            // 
            // exportWizardToolStripMenuItem
            // 
            exportWizardToolStripMenuItem.Image = Properties.Resources.ExportWizard;
            exportWizardToolStripMenuItem.Name = "exportWizardToolStripMenuItem";
            exportWizardToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            exportWizardToolStripMenuItem.Text = "Export wizard";
            exportWizardToolStripMenuItem.Click += exportWizardToolStripMenuItem_Click;
            // 
            // exportDatabaseAsCsvbcpToolStripMenuItem
            // 
            exportDatabaseAsCsvbcpToolStripMenuItem.Image = Properties.Resources.CSV;
            exportDatabaseAsCsvbcpToolStripMenuItem.Name = "exportDatabaseAsCsvbcpToolStripMenuItem";
            exportDatabaseAsCsvbcpToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            exportDatabaseAsCsvbcpToolStripMenuItem.Text = "Export database as csv (bcp) ...";
            exportDatabaseAsCsvbcpToolStripMenuItem.Click += exportDatabaseAsCsvbcpToolStripMenuItem_Click;
            // 
            // importToolStripMenuItem
            // 
            importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { gFBioToolStripMenuItem, importWizardToolStripMenuItem });
            importToolStripMenuItem.Image = Properties.Resources.Import;
            importToolStripMenuItem.Name = "importToolStripMenuItem";
            importToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            importToolStripMenuItem.Text = "Import";
            // 
            // gFBioToolStripMenuItem
            // 
            gFBioToolStripMenuItem.Image = Properties.Resources.IcoGFBio1;
            gFBioToolStripMenuItem.Name = "gFBioToolStripMenuItem";
            gFBioToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            gFBioToolStripMenuItem.Text = "GFBio";
            gFBioToolStripMenuItem.Click += gFBioToolStripMenuItem_Click;
            // 
            // importWizardToolStripMenuItem
            // 
            importWizardToolStripMenuItem.Image = Properties.Resources.ImportWizard2;
            importWizardToolStripMenuItem.Name = "importWizardToolStripMenuItem";
            importWizardToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            importWizardToolStripMenuItem.Text = "Import wizard ...";
            importWizardToolStripMenuItem.Click += importWizardToolStripMenuItem_Click;
            // 
            // tableEditorToolStripMenuItem
            // 
            tableEditorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { projectToolStripMenuItem, agentToolStripMenuItem, descriptorsToolStripMenuItem, iDsToolStripMenuItem, iPRToolStripMenuItem, labelToolStripMenuItem, keywordsToolStripMenuItem, referencesToolStripMenuItem, resourcesToolStripMenuItem, projectSettingsToolStripMenuItem });
            tableEditorToolStripMenuItem.Image = Properties.Resources.EditInSpeadsheet;
            tableEditorToolStripMenuItem.Name = "tableEditorToolStripMenuItem";
            tableEditorToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            tableEditorToolStripMenuItem.Text = "Table editor";
            // 
            // projectToolStripMenuItem
            // 
            projectToolStripMenuItem.Image = Properties.Resources.Project;
            projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            projectToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            projectToolStripMenuItem.Text = "Project...";
            projectToolStripMenuItem.Click += projectToolStripMenuItem_Click;
            // 
            // agentToolStripMenuItem
            // 
            agentToolStripMenuItem.Image = Properties.Resources.Agent;
            agentToolStripMenuItem.Name = "agentToolStripMenuItem";
            agentToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            agentToolStripMenuItem.Text = "Agent...";
            agentToolStripMenuItem.Click += agentToolStripMenuItem_Click;
            // 
            // descriptorsToolStripMenuItem
            // 
            descriptorsToolStripMenuItem.Image = Properties.Resources.KeyBlue;
            descriptorsToolStripMenuItem.Name = "descriptorsToolStripMenuItem";
            descriptorsToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            descriptorsToolStripMenuItem.Text = "Descriptors...";
            descriptorsToolStripMenuItem.Click += descriptorsToolStripMenuItem_Click;
            // 
            // iDsToolStripMenuItem
            // 
            iDsToolStripMenuItem.Image = Properties.Resources.Identifier;
            iDsToolStripMenuItem.Name = "iDsToolStripMenuItem";
            iDsToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            iDsToolStripMenuItem.Text = "IDs";
            iDsToolStripMenuItem.Click += iDsToolStripMenuItem_Click;
            // 
            // iPRToolStripMenuItem
            // 
            iPRToolStripMenuItem.Image = Properties.Resources.Document;
            iPRToolStripMenuItem.Name = "iPRToolStripMenuItem";
            iPRToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            iPRToolStripMenuItem.Text = "IPR...";
            iPRToolStripMenuItem.Click += iPRToolStripMenuItem_Click;
            // 
            // labelToolStripMenuItem
            // 
            labelToolStripMenuItem.Image = Properties.Resources.Note;
            labelToolStripMenuItem.Name = "labelToolStripMenuItem";
            labelToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            labelToolStripMenuItem.Text = "Label";
            labelToolStripMenuItem.Visible = false;
            labelToolStripMenuItem.Click += labelToolStripMenuItem_Click;
            // 
            // keywordsToolStripMenuItem
            // 
            keywordsToolStripMenuItem.Image = Properties.Resources.KeyBig;
            keywordsToolStripMenuItem.Name = "keywordsToolStripMenuItem";
            keywordsToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            keywordsToolStripMenuItem.Text = "Keywords";
            keywordsToolStripMenuItem.Visible = false;
            keywordsToolStripMenuItem.Click += keywordsToolStripMenuItem_Click;
            // 
            // referencesToolStripMenuItem
            // 
            referencesToolStripMenuItem.Image = Properties.Resources.References;
            referencesToolStripMenuItem.Name = "referencesToolStripMenuItem";
            referencesToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            referencesToolStripMenuItem.Text = "References";
            referencesToolStripMenuItem.Click += referencesToolStripMenuItem_Click;
            // 
            // resourcesToolStripMenuItem
            // 
            resourcesToolStripMenuItem.Image = Properties.Resources.Resource;
            resourcesToolStripMenuItem.Name = "resourcesToolStripMenuItem";
            resourcesToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            resourcesToolStripMenuItem.Text = "Resources";
            resourcesToolStripMenuItem.Click += resourcesToolStripMenuItem_Click;
            // 
            // projectSettingsToolStripMenuItem
            // 
            projectSettingsToolStripMenuItem.Image = Properties.Resources.Settings;
            projectSettingsToolStripMenuItem.Name = "projectSettingsToolStripMenuItem";
            projectSettingsToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            projectSettingsToolStripMenuItem.Text = "Settings";
            projectSettingsToolStripMenuItem.Click += projectSettingsToolStripMenuItem_Click;
            // 
            // backupDatabaseToolStripMenuItem
            // 
            backupDatabaseToolStripMenuItem.Image = Properties.Resources.DatabaseBackup;
            backupDatabaseToolStripMenuItem.Name = "backupDatabaseToolStripMenuItem";
            backupDatabaseToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            backupDatabaseToolStripMenuItem.Text = "Backup database ...";
            backupDatabaseToolStripMenuItem.Click += backupDatabaseToolStripMenuItem_Click;
            // 
            // xMLarchiveToolStripMenuItem
            // 
            xMLarchiveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { administrateArchivesToolStripMenuItem, createArchiveToolStripMenuItem, resetDatabaseToolStripMenuItem, restoreArchiveToolStripMenuItem, createSchemaToolStripMenuItem });
            xMLarchiveToolStripMenuItem.Image = Properties.Resources.Archiv;
            xMLarchiveToolStripMenuItem.Name = "xMLarchiveToolStripMenuItem";
            xMLarchiveToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            xMLarchiveToolStripMenuItem.Text = "Archive";
            // 
            // administrateArchivesToolStripMenuItem
            // 
            administrateArchivesToolStripMenuItem.Image = Properties.Resources.Archiv;
            administrateArchivesToolStripMenuItem.Name = "administrateArchivesToolStripMenuItem";
            administrateArchivesToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            administrateArchivesToolStripMenuItem.Text = "Administrate archives ...";
            administrateArchivesToolStripMenuItem.Click += administrateArchivesToolStripMenuItem_Click;
            // 
            // createArchiveToolStripMenuItem
            // 
            createArchiveToolStripMenuItem.Image = Properties.Resources.ArchivCreate;
            createArchiveToolStripMenuItem.Name = "createArchiveToolStripMenuItem";
            createArchiveToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            createArchiveToolStripMenuItem.Text = "Create archive ...";
            createArchiveToolStripMenuItem.Click += createArchiveToolStripMenuItem_Click;
            // 
            // resetDatabaseToolStripMenuItem
            // 
            resetDatabaseToolStripMenuItem.Image = Properties.Resources.CleanDatabase;
            resetDatabaseToolStripMenuItem.Name = "resetDatabaseToolStripMenuItem";
            resetDatabaseToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            resetDatabaseToolStripMenuItem.Text = "Reset database ...";
            resetDatabaseToolStripMenuItem.Click += resetDatabaseToolStripMenuItem_Click;
            // 
            // restoreArchiveToolStripMenuItem
            // 
            restoreArchiveToolStripMenuItem.Image = Properties.Resources.ArchivRestore;
            restoreArchiveToolStripMenuItem.Name = "restoreArchiveToolStripMenuItem";
            restoreArchiveToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            restoreArchiveToolStripMenuItem.Text = "Restore archive ...";
            restoreArchiveToolStripMenuItem.Click += restoreArchiveToolStripMenuItem_Click;
            // 
            // createSchemaToolStripMenuItem
            // 
            createSchemaToolStripMenuItem.Image = Properties.Resources.ArchivOpen;
            createSchemaToolStripMenuItem.Name = "createSchemaToolStripMenuItem";
            createSchemaToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            createSchemaToolStripMenuItem.Text = "Create schema ...";
            createSchemaToolStripMenuItem.Click += createSchemaToolStripMenuItem_Click;
            // 
            // scanModulesToolStripMenuItem
            // 
            scanModulesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { diversityAgentsToolStripMenuItem, diversityCollectionToolStripMenuItem, diversityDescriptionsToolStripMenuItem, diversityGazetteerToolStripMenuItem, diversityReferencesToolStripMenuItem, diversitySamplingPlotsToolStripMenuItem, diversityScientificTermsToolStripMenuItem, diversityTaxonNamesToolStripMenuItem });
            scanModulesToolStripMenuItem.Image = Properties.Resources.Workbench;
            scanModulesToolStripMenuItem.Name = "scanModulesToolStripMenuItem";
            scanModulesToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            scanModulesToolStripMenuItem.Text = "Scan modules";
            // 
            // diversityAgentsToolStripMenuItem
            // 
            diversityAgentsToolStripMenuItem.Name = "diversityAgentsToolStripMenuItem";
            diversityAgentsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            diversityAgentsToolStripMenuItem.Text = "DiversityAgents";
            diversityAgentsToolStripMenuItem.Click += diversityAgentsToolStripMenuItem_Click;
            // 
            // diversityCollectionToolStripMenuItem
            // 
            diversityCollectionToolStripMenuItem.Name = "diversityCollectionToolStripMenuItem";
            diversityCollectionToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            diversityCollectionToolStripMenuItem.Text = "DiversityCollection";
            diversityCollectionToolStripMenuItem.Click += diversityCollectionToolStripMenuItem_Click;
            // 
            // diversityDescriptionsToolStripMenuItem
            // 
            diversityDescriptionsToolStripMenuItem.Name = "diversityDescriptionsToolStripMenuItem";
            diversityDescriptionsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            diversityDescriptionsToolStripMenuItem.Text = "DiversityDescriptions";
            diversityDescriptionsToolStripMenuItem.Click += diversityDescriptionsToolStripMenuItem_Click;
            // 
            // diversityGazetteerToolStripMenuItem
            // 
            diversityGazetteerToolStripMenuItem.Name = "diversityGazetteerToolStripMenuItem";
            diversityGazetteerToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            diversityGazetteerToolStripMenuItem.Text = "DiversityGazetteer";
            diversityGazetteerToolStripMenuItem.Click += diversityGazetteerToolStripMenuItem_Click;
            // 
            // diversityReferencesToolStripMenuItem
            // 
            diversityReferencesToolStripMenuItem.Name = "diversityReferencesToolStripMenuItem";
            diversityReferencesToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            diversityReferencesToolStripMenuItem.Text = "DiversityReferences";
            diversityReferencesToolStripMenuItem.Click += diversityReferencesToolStripMenuItem_Click;
            // 
            // diversitySamplingPlotsToolStripMenuItem
            // 
            diversitySamplingPlotsToolStripMenuItem.Name = "diversitySamplingPlotsToolStripMenuItem";
            diversitySamplingPlotsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            diversitySamplingPlotsToolStripMenuItem.Text = "DiversitySamplingPlots";
            diversitySamplingPlotsToolStripMenuItem.Click += diversitySamplingPlotsToolStripMenuItem_Click;
            // 
            // diversityScientificTermsToolStripMenuItem
            // 
            diversityScientificTermsToolStripMenuItem.Name = "diversityScientificTermsToolStripMenuItem";
            diversityScientificTermsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            diversityScientificTermsToolStripMenuItem.Text = "DiversityScientificTerms";
            diversityScientificTermsToolStripMenuItem.Click += diversityScientificTermsToolStripMenuItem_Click;
            // 
            // diversityTaxonNamesToolStripMenuItem
            // 
            diversityTaxonNamesToolStripMenuItem.Name = "diversityTaxonNamesToolStripMenuItem";
            diversityTaxonNamesToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            diversityTaxonNamesToolStripMenuItem.Text = "DiversityTaxonNames";
            diversityTaxonNamesToolStripMenuItem.Click += diversityTaxonNamesToolStripMenuItem_Click;
            // 
            // administrationToolStripMenuItem
            // 
            administrationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { archiveFormatsToolStripMenuItem, archiveMimeTypesToolStripMenuItem, changePasswordToolStripMenuItem, databaseAdministrationToolStripMenuItem, descriptorTypesToolStripMenuItem, identifierTypesToolStripMenuItem, settingsToolStripMenuItem, resourcesDirectoryToolStripMenuItem, stableIdentifierToolStripMenuItem, toolsToolStripMenuItem, jsonCacheToolStripMenuItem });
            administrationToolStripMenuItem.Name = "administrationToolStripMenuItem";
            administrationToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            administrationToolStripMenuItem.Text = "Administration";
            administrationToolStripMenuItem.Visible = false;
            // 
            // archiveFormatsToolStripMenuItem
            // 
            archiveFormatsToolStripMenuItem.Image = Properties.Resources.OAIS_16;
            archiveFormatsToolStripMenuItem.Name = "archiveFormatsToolStripMenuItem";
            archiveFormatsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            archiveFormatsToolStripMenuItem.Text = "OAIP formats ...";
            archiveFormatsToolStripMenuItem.Click += archiveFormatsToolStripMenuItem_Click;
            // 
            // archiveMimeTypesToolStripMenuItem
            // 
            archiveMimeTypesToolStripMenuItem.Image = Properties.Resources.OAIS_16;
            archiveMimeTypesToolStripMenuItem.Name = "archiveMimeTypesToolStripMenuItem";
            archiveMimeTypesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            archiveMimeTypesToolStripMenuItem.Text = "OAIP mime types ...";
            archiveMimeTypesToolStripMenuItem.Click += archiveMimeTypesToolStripMenuItem_Click;
            // 
            // changePasswordToolStripMenuItem
            // 
            changePasswordToolStripMenuItem.Image = Properties.Resources.KeyBig;
            changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            changePasswordToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            changePasswordToolStripMenuItem.Text = "Change password ...";
            changePasswordToolStripMenuItem.Click += changePasswordToolStripMenuItem_Click;
            // 
            // databaseAdministrationToolStripMenuItem
            // 
            databaseAdministrationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { documentationToolStripMenuItem, loginsToolStripMenuItem, renameDatabaseToolStripMenuItem, setPublishedAddressToolStripMenuItem, databaseToolsToolStripMenuItem });
            databaseAdministrationToolStripMenuItem.Image = Properties.Resources.Database;
            databaseAdministrationToolStripMenuItem.Name = "databaseAdministrationToolStripMenuItem";
            databaseAdministrationToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            databaseAdministrationToolStripMenuItem.Text = "Database";
            // 
            // documentationToolStripMenuItem
            // 
            documentationToolStripMenuItem.Image = Properties.Resources.sqltut;
            documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            documentationToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            documentationToolStripMenuItem.Text = "Documentation ...";
            documentationToolStripMenuItem.Click += documentationToolStripMenuItem_Click;
            // 
            // loginsToolStripMenuItem
            // 
            loginsToolStripMenuItem.Image = Properties.Resources.Login;
            loginsToolStripMenuItem.Name = "loginsToolStripMenuItem";
            loginsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            loginsToolStripMenuItem.Text = "Logins ...";
            loginsToolStripMenuItem.Click += loginsToolStripMenuItem_Click;
            // 
            // renameDatabaseToolStripMenuItem
            // 
            renameDatabaseToolStripMenuItem.Image = Properties.Resources.RenameDB;
            renameDatabaseToolStripMenuItem.Name = "renameDatabaseToolStripMenuItem";
            renameDatabaseToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            renameDatabaseToolStripMenuItem.Text = "Rename database ...";
            renameDatabaseToolStripMenuItem.Click += renameDatabaseToolStripMenuItem_Click;
            // 
            // setPublishedAddressToolStripMenuItem
            // 
            setPublishedAddressToolStripMenuItem.Image = Properties.Resources.RenameDB;
            setPublishedAddressToolStripMenuItem.Name = "setPublishedAddressToolStripMenuItem";
            setPublishedAddressToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            setPublishedAddressToolStripMenuItem.Text = "Set published address ...";
            setPublishedAddressToolStripMenuItem.Click += setPublishedAddressToolStripMenuItem_Click;
            // 
            // databaseToolsToolStripMenuItem
            // 
            databaseToolsToolStripMenuItem.Image = Properties.Resources.SystemTools;
            databaseToolsToolStripMenuItem.Name = "databaseToolsToolStripMenuItem";
            databaseToolsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            databaseToolsToolStripMenuItem.Text = "Database tools ...";
            databaseToolsToolStripMenuItem.Click += databaseToolsToolStripMenuItem_Click;
            // 
            // descriptorTypesToolStripMenuItem
            // 
            descriptorTypesToolStripMenuItem.Image = Properties.Resources.KeyBlue;
            descriptorTypesToolStripMenuItem.Name = "descriptorTypesToolStripMenuItem";
            descriptorTypesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            descriptorTypesToolStripMenuItem.Text = "Descriptor types...";
            descriptorTypesToolStripMenuItem.Click += descriptorTypesToolStripMenuItem_Click;
            // 
            // identifierTypesToolStripMenuItem
            // 
            identifierTypesToolStripMenuItem.Image = Properties.Resources.Identifier;
            identifierTypesToolStripMenuItem.Name = "identifierTypesToolStripMenuItem";
            identifierTypesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            identifierTypesToolStripMenuItem.Text = "Identifier types ...";
            identifierTypesToolStripMenuItem.Click += identifierTypesToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Image = Properties.Resources.Settings;
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            settingsToolStripMenuItem.Text = "Settings ...";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // resourcesDirectoryToolStripMenuItem
            // 
            resourcesDirectoryToolStripMenuItem.Image = Properties.Resources.Files;
            resourcesDirectoryToolStripMenuItem.Name = "resourcesDirectoryToolStripMenuItem";
            resourcesDirectoryToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            resourcesDirectoryToolStripMenuItem.Text = "Resources directory ...";
            resourcesDirectoryToolStripMenuItem.Click += resourcesDirectoryToolStripMenuItem_Click;
            // 
            // stableIdentifierToolStripMenuItem
            // 
            stableIdentifierToolStripMenuItem.Image = Properties.Resources.QRcode;
            stableIdentifierToolStripMenuItem.Name = "stableIdentifierToolStripMenuItem";
            stableIdentifierToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            stableIdentifierToolStripMenuItem.Text = "Stable identifier ...";
            stableIdentifierToolStripMenuItem.Click += stableIdentifierToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { databaseToolStripMenuItem1 });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            toolsToolStripMenuItem.Text = "Tools";
            toolsToolStripMenuItem.Visible = false;
            // 
            // databaseToolStripMenuItem1
            // 
            databaseToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { descriptionToolStripMenuItem, loggingToolStripMenuItem, proceduresToolStripMenuItem });
            databaseToolStripMenuItem1.Name = "databaseToolStripMenuItem1";
            databaseToolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            databaseToolStripMenuItem1.Text = "Database";
            databaseToolStripMenuItem1.Visible = false;
            // 
            // descriptionToolStripMenuItem
            // 
            descriptionToolStripMenuItem.Name = "descriptionToolStripMenuItem";
            descriptionToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            descriptionToolStripMenuItem.Text = "Description ...";
            // 
            // loggingToolStripMenuItem
            // 
            loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            loggingToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            loggingToolStripMenuItem.Text = "Logging ...";
            // 
            // proceduresToolStripMenuItem
            // 
            proceduresToolStripMenuItem.Name = "proceduresToolStripMenuItem";
            proceduresToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            proceduresToolStripMenuItem.Text = "Procedures ...";
            // 
            // jsonCacheToolStripMenuItem
            // 
            jsonCacheToolStripMenuItem.Image = Properties.Resources.JsonApi;
            jsonCacheToolStripMenuItem.Name = "jsonCacheToolStripMenuItem";
            jsonCacheToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            jsonCacheToolStripMenuItem.Text = "JsonCache...";
            jsonCacheToolStripMenuItem.Click += jsonCacheToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { manualToolStripMenuItem, feedbackToolStripMenuItem, websitesToolStripMenuItem, infoToolStripMenuItem, errorLogToolStripMenuItem, clearErrorlogToolStripMenuItem, metadataToolStripMenuItem, windowSizeToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // manualToolStripMenuItem
            // 
            manualToolStripMenuItem.Image = Properties.Resources.Manual;
            manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            manualToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            manualToolStripMenuItem.Text = "Manual ...";
            manualToolStripMenuItem.Click += manualToolStripMenuItem_Click;
            // 
            // feedbackToolStripMenuItem
            // 
            feedbackToolStripMenuItem.Image = Properties.Resources.Feedback;
            feedbackToolStripMenuItem.Name = "feedbackToolStripMenuItem";
            feedbackToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            feedbackToolStripMenuItem.Text = "Feedback ...";
            feedbackToolStripMenuItem.Click += feedbackToolStripMenuItem_Click;
            // 
            // websitesToolStripMenuItem
            // 
            websitesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { downloadApplicationsToolStripMenuItem, informationModelToolStripMenuItem });
            websitesToolStripMenuItem.Image = Properties.Resources.Browse;
            websitesToolStripMenuItem.Name = "websitesToolStripMenuItem";
            websitesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            websitesToolStripMenuItem.Text = "Websites";
            // 
            // downloadApplicationsToolStripMenuItem
            // 
            downloadApplicationsToolStripMenuItem.Image = Properties.Resources.DiversityWorkbench;
            downloadApplicationsToolStripMenuItem.Name = "downloadApplicationsToolStripMenuItem";
            downloadApplicationsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            downloadApplicationsToolStripMenuItem.Text = "Download applications ...";
            downloadApplicationsToolStripMenuItem.Click += downloadApplicationsToolStripMenuItem_Click;
            // 
            // informationModelToolStripMenuItem
            // 
            informationModelToolStripMenuItem.Image = Properties.Resources.Model;
            informationModelToolStripMenuItem.Name = "informationModelToolStripMenuItem";
            informationModelToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            informationModelToolStripMenuItem.Text = "Information model ...";
            informationModelToolStripMenuItem.Click += informationModelToolStripMenuItem_Click;
            // 
            // infoToolStripMenuItem
            // 
            infoToolStripMenuItem.Image = Properties.Resources.info;
            infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            infoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            infoToolStripMenuItem.Text = "Info ...";
            infoToolStripMenuItem.Click += infoToolStripMenuItem_Click;
            // 
            // errorLogToolStripMenuItem
            // 
            errorLogToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { keepErrorlogToolStripMenuItem, clearErrorlogAtProgramStartToolStripMenuItem });
            errorLogToolStripMenuItem.Image = Properties.Resources.IcoErrorLog;
            errorLogToolStripMenuItem.Name = "errorLogToolStripMenuItem";
            errorLogToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            errorLogToolStripMenuItem.Text = "ErrorLog";
            errorLogToolStripMenuItem.Click += errorLogToolStripMenuItem_Click;
            // 
            // keepErrorlogToolStripMenuItem
            // 
            keepErrorlogToolStripMenuItem.Name = "keepErrorlogToolStripMenuItem";
            keepErrorlogToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            keepErrorlogToolStripMenuItem.Text = "Keep Errorlog";
            keepErrorlogToolStripMenuItem.Click += keepErrorlogToolStripMenuItem_Click;
            // 
            // clearErrorlogAtProgramStartToolStripMenuItem
            // 
            clearErrorlogAtProgramStartToolStripMenuItem.Name = "clearErrorlogAtProgramStartToolStripMenuItem";
            clearErrorlogAtProgramStartToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            clearErrorlogAtProgramStartToolStripMenuItem.Text = "Clear Errorlog at program start";
            clearErrorlogAtProgramStartToolStripMenuItem.Click += clearErrorlogAtProgramStartToolStripMenuItem_Click;
            // 
            // clearErrorlogToolStripMenuItem
            // 
            clearErrorlogToolStripMenuItem.Image = Properties.Resources.IcoErrorLogClear;
            clearErrorlogToolStripMenuItem.Name = "clearErrorlogToolStripMenuItem";
            clearErrorlogToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            clearErrorlogToolStripMenuItem.Text = "Clear Errorlog";
            clearErrorlogToolStripMenuItem.Click += clearErrorlogToolStripMenuItem_Click;
            // 
            // metadataToolStripMenuItem
            // 
            metadataToolStripMenuItem.Image = Properties.Resources.EditInSpeadsheet;
            metadataToolStripMenuItem.Name = "metadataToolStripMenuItem";
            metadataToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            metadataToolStripMenuItem.Text = "Metadata ...";
            metadataToolStripMenuItem.Click += metadataToolStripMenuItem_Click;
            // 
            // windowSizeToolStripMenuItem
            // 
            windowSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { xGA1024X768ToolStripMenuItem });
            windowSizeToolStripMenuItem.Image = Properties.Resources.HD;
            windowSizeToolStripMenuItem.Name = "windowSizeToolStripMenuItem";
            windowSizeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            windowSizeToolStripMenuItem.Text = "Window size";
            // 
            // xGA1024X768ToolStripMenuItem
            // 
            xGA1024X768ToolStripMenuItem.Name = "xGA1024X768ToolStripMenuItem";
            xGA1024X768ToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            xGA1024X768ToolStripMenuItem.Text = "XGA (1024 x 768)";
            xGA1024X768ToolStripMenuItem.Click += xGA1024X768ToolStripMenuItem_Click;
            // 
            // updateToolStripMenuItem
            // 
            updateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { updateDatabaseToolStripMenuItem, updateClientToolStripMenuItem });
            updateToolStripMenuItem.Image = Properties.Resources.Update;
            updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            updateToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            updateToolStripMenuItem.Text = "Update";
            updateToolStripMenuItem.Visible = false;
            // 
            // updateDatabaseToolStripMenuItem
            // 
            updateDatabaseToolStripMenuItem.Image = Properties.Resources.UpdateDatabase;
            updateDatabaseToolStripMenuItem.Name = "updateDatabaseToolStripMenuItem";
            updateDatabaseToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            updateDatabaseToolStripMenuItem.Text = "Update database ...";
            updateDatabaseToolStripMenuItem.Click += updateDatabaseToolStripMenuItem_Click;
            // 
            // updateClientToolStripMenuItem
            // 
            updateClientToolStripMenuItem.Image = Properties.Resources.UpdateClient;
            updateClientToolStripMenuItem.Name = "updateClientToolStripMenuItem";
            updateClientToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            updateClientToolStripMenuItem.Text = "Update client ...";
            updateClientToolStripMenuItem.Click += updateClientToolStripMenuItem_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // projectArchiveIdentifierTableAdapter
            // 
            projectArchiveIdentifierTableAdapter.ClearBeforeFill = true;
            // 
            // userControlDialogPanel
            // 
            userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel.Location = new System.Drawing.Point(0, 778);
            userControlDialogPanel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlDialogPanel.Name = "userControlDialogPanel";
            userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel.Size = new System.Drawing.Size(1171, 31);
            userControlDialogPanel.TabIndex = 3;
            userControlDialogPanel.Visible = false;
            // 
            // projectTableAdapter
            // 
            projectTableAdapter.ClearBeforeFill = true;
            // 
            // projectSettingTableAdapter
            // 
            projectSettingTableAdapter.ClearBeforeFill = true;
            // 
            // projectAgentTableAdapter
            // 
            projectAgentTableAdapter.ClearBeforeFill = true;
            // 
            // projectAgentRoleTableAdapter
            // 
            projectAgentRoleTableAdapter.ClearBeforeFill = true;
            // 
            // projectIdentifierTypeTableAdapter
            // 
            projectIdentifierTypeTableAdapter.ClearBeforeFill = true;
            // 
            // projectLicenseTableAdapter
            // 
            projectLicenseTableAdapter.ClearBeforeFill = true;
            // 
            // projectResourceTableAdapter
            // 
            projectResourceTableAdapter.ClearBeforeFill = true;
            // 
            // projectDescriptorTableAdapter
            // 
            projectDescriptorTableAdapter.ClearBeforeFill = true;
            // 
            // projectDescriptorElementTableAdapter
            // 
            projectDescriptorElementTableAdapter.ClearBeforeFill = true;
            // 
            // projectIdentifierTableAdapter
            // 
            projectIdentifierTableAdapter.ClearBeforeFill = true;
            // 
            // projectArchiveTableAdapter
            // 
            projectArchiveTableAdapter.ClearBeforeFill = true;
            // 
            // projectReferenceTableAdapter
            // 
            projectReferenceTableAdapter.ClearBeforeFill = true;
            // 
            // language_EnumTableAdapter
            // 
            language_EnumTableAdapter.ClearBeforeFill = true;
            // 
            // FormDiversityProjects
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1171, 809);
            Controls.Add(splitContainerMain);
            Controls.Add(userControlDialogPanel);
            Controls.Add(menuStripMain);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStripMain;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormDiversityProjects";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "DiversityProjects";
            FormClosing += FormDiversityProjects_FormClosing;
            Load += FormDiversityProjects_Load;
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            splitContainerData.Panel1.ResumeLayout(false);
            splitContainerData.Panel1.PerformLayout();
            splitContainerData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerData).EndInit();
            splitContainerData.ResumeLayout(false);
            toolStripHierarchy.ResumeLayout(false);
            toolStripHierarchy.PerformLayout();
            tableLayoutPanelHeader.ResumeLayout(false);
            tableLayoutPanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)projectBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataSetDiversityProjects).EndInit();
            splitContainerSettings.Panel1.ResumeLayout(false);
            splitContainerSettings.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerSettings).EndInit();
            splitContainerSettings.ResumeLayout(false);
            splitContainerDetailsAndAgents.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerDetailsAndAgents).EndInit();
            splitContainerDetailsAndAgents.ResumeLayout(false);
            tableLayoutPanelProject.ResumeLayout(false);
            tableLayoutPanelProject.PerformLayout();
            splitContainerNotesAndLabel.Panel1.ResumeLayout(false);
            splitContainerNotesAndLabel.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerNotesAndLabel).EndInit();
            splitContainerNotesAndLabel.ResumeLayout(false);
            tabControlSettingsAgentsResources.ResumeLayout(false);
            tabPageSettings.ResumeLayout(false);
            tableLayoutPanelSettings.ResumeLayout(false);
            tableLayoutPanelSettings.PerformLayout();
            toolStripSettings.ResumeLayout(false);
            toolStripSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)projectSettingBindingSource).EndInit();
            tabPageAgents.ResumeLayout(false);
            splitContainerAgents.Panel1.ResumeLayout(false);
            splitContainerAgents.Panel1.PerformLayout();
            splitContainerAgents.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerAgents).EndInit();
            splitContainerAgents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)projectAgentBindingSource).EndInit();
            toolStripAgents.ResumeLayout(false);
            toolStripAgents.PerformLayout();
            tableLayoutPanelAgent.ResumeLayout(false);
            tableLayoutPanelAgent.PerformLayout();
            toolStripRoles.ResumeLayout(false);
            toolStripRoles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)projectAgentRoleBindingSource).EndInit();
            tabPageLicenses.ResumeLayout(false);
            splitContainerLicense.Panel1.ResumeLayout(false);
            splitContainerLicense.Panel1.PerformLayout();
            splitContainerLicense.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerLicense).EndInit();
            splitContainerLicense.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)projectLicenseBindingSource).EndInit();
            toolStripLicense.ResumeLayout(false);
            toolStripLicense.PerformLayout();
            groupBoxLicenseNotes.ResumeLayout(false);
            groupBoxLicenseNotes.PerformLayout();
            groupBoxCopyright.ResumeLayout(false);
            tableLayoutPanelCopyrightHolder.ResumeLayout(false);
            tableLayoutPanelCopyrightHolder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCopyrightHolder).EndInit();
            groupBoxIPR.ResumeLayout(false);
            tableLayoutPanelIPR.ResumeLayout(false);
            tableLayoutPanelIPR.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIPRHolder).EndInit();
            groupBoxLicense.ResumeLayout(false);
            tableLayoutPanelLicense.ResumeLayout(false);
            tableLayoutPanelLicense.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLicenseHolder).EndInit();
            tabPageResources.ResumeLayout(false);
            splitContainerResources.Panel1.ResumeLayout(false);
            splitContainerResources.Panel1.PerformLayout();
            splitContainerResources.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerResources).EndInit();
            splitContainerResources.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)projectResourceBindingSource).EndInit();
            toolStripResources.ResumeLayout(false);
            toolStripResources.PerformLayout();
            tableLayoutPanelResources.ResumeLayout(false);
            tableLayoutPanelResources.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxResource).EndInit();
            tabPageReferences.ResumeLayout(false);
            splitContainerReference.Panel1.ResumeLayout(false);
            splitContainerReference.Panel1.PerformLayout();
            splitContainerReference.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerReference).EndInit();
            splitContainerReference.ResumeLayout(false);
            tableLayoutPanelReferenceLists.ResumeLayout(false);
            tableLayoutPanelReferenceLists.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)projectReferenceBindingSource).EndInit();
            toolStripCitations.ResumeLayout(false);
            toolStripCitations.PerformLayout();
            toolStripReference.ResumeLayout(false);
            toolStripReference.PerformLayout();
            tableLayoutPanelReference.ResumeLayout(false);
            tableLayoutPanelReference.PerformLayout();
            tabPageDescriptor.ResumeLayout(false);
            splitContainerDescriptor.Panel1.ResumeLayout(false);
            splitContainerDescriptor.Panel1.PerformLayout();
            splitContainerDescriptor.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerDescriptor).EndInit();
            splitContainerDescriptor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)projectDescriptorBindingSource).EndInit();
            toolStripDescriptor.ResumeLayout(false);
            toolStripDescriptor.PerformLayout();
            tableLayoutPanelDescriptor.ResumeLayout(false);
            tableLayoutPanelDescriptor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)languageEnumBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)projectDescriptorElementBindingSource).EndInit();
            tabPageID.ResumeLayout(false);
            splitContainerID.Panel1.ResumeLayout(false);
            splitContainerID.Panel1.PerformLayout();
            splitContainerID.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerID).EndInit();
            splitContainerID.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)projectIdentifierBindingSource).EndInit();
            toolStripID.ResumeLayout(false);
            toolStripID.PerformLayout();
            tableLayoutPanelID.ResumeLayout(false);
            tableLayoutPanelID.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)projectIdentifierTypeBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIdentifierLocked).EndInit();
            tabPageArchive.ResumeLayout(false);
            splitContainerArchive.Panel1.ResumeLayout(false);
            splitContainerArchive.Panel1.PerformLayout();
            splitContainerArchive.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerArchive).EndInit();
            splitContainerArchive.ResumeLayout(false);
            toolStripArchive.ResumeLayout(false);
            toolStripArchive.PerformLayout();
            tableLayoutPanelArchive.ResumeLayout(false);
            tableLayoutPanelArchive.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)projectArchiveBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxArchiveDataWithholdingReason).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxArchivePublished).EndInit();
            toolStripArchiveIdentier.ResumeLayout(false);
            toolStripArchiveIdentier.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)projectArchiveIdentifierBindingSource).EndInit();
            tabPageModules.ResumeLayout(false);
            tabControlModules.ResumeLayout(false);
            tabPageDiversityAgents.ResumeLayout(false);
            tabPageDiversityCollection.ResumeLayout(false);
            tabPageDiversityDescriptions.ResumeLayout(false);
            tabPageDiversityGazetteer.ResumeLayout(false);
            tabPageDiversityReferences.ResumeLayout(false);
            tabPageDiversitySamplingPlots.ResumeLayout(false);
            tabPageDiversityScientificTerms.ResumeLayout(false);
            tabPageDiversityTaxonNames.ResumeLayout(false);
            menuStripMain.ResumeLayout(false);
            menuStripMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.HelpProvider helpProviderDiversityProjects;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItemSeparatorQuit;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStrip toolStripHierarchy;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectNew;
        private System.Windows.Forms.TreeView treeViewProjects;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProject;
        private System.Windows.Forms.Label labelProjectTitle;
        private System.Windows.Forms.TextBox textBoxProjectTitle;
        private System.Windows.Forms.Label labelProjectDescription;
        private System.Windows.Forms.TextBox textBoxProjectDescription;
        private System.Windows.Forms.Label labelProjectNotes;
        private System.Windows.Forms.TextBox textBoxProjectNotes;
        private System.Windows.Forms.Label labelProjectVersion;
        private System.Windows.Forms.TextBox textBoxProjectVersion;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.Label labelHeaderProject;
        private System.Windows.Forms.TextBox textBoxHeaderProject;
        private System.Windows.Forms.Label labelHeaderProjectID;
        private System.Windows.Forms.TextBox textBoxHeaderProjectID;
        private System.Windows.Forms.Button buttonHistory;
        private System.Windows.Forms.ToolStripButton toolStripButtonApplicationsNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonApplicationsDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectSetParent;
        private System.Windows.Forms.ToolStripMenuItem administrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.Button buttonURL;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.SplitContainer splitContainerSettings;
        private System.Windows.Forms.ToolStripMenuItem feedbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transferPreviousSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateDatabaseToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerDetailsAndAgents;
        private System.Windows.Forms.SplitContainer splitContainerAgents;
        private System.Windows.Forms.ListBox listBoxAgents;
        private System.Windows.Forms.ToolStrip toolStripAgents;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAgent;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryAgent;
        private System.Windows.Forms.ToolStripButton toolStripButtonAgentNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonAgentDelete;
        private System.Windows.Forms.Label labelAgentRole;
        private System.Windows.Forms.Label labelAgentNotes;
        private System.Windows.Forms.TextBox textBoxAgentNotes;
        private System.Windows.Forms.ToolStripMenuItem moduleConnectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backupDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateClientToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControlSettingsAgentsResources;
        private System.Windows.Forms.TabPage tabPageAgents;
        private System.Windows.Forms.TabPage tabPageResources;
        private System.Windows.Forms.SplitContainer splitContainerResources;
        private System.Windows.Forms.ImageList imageListTabcontrol;
        private System.Windows.Forms.ListBox listBoxResources;
        private System.Windows.Forms.ToolStrip toolStripResources;
        private System.Windows.Forms.ToolStripButton toolStripButtonResourcesNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonResourcesDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelResources;
        private System.Windows.Forms.ToolStripButton toolStripButtonResourceOpen;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.PictureBox pictureBoxResource;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.TextBox textBoxResourceNotes;
        private System.Windows.Forms.Label labelResourceNotes;
        private System.Windows.Forms.ToolStripMenuItem loginsToolStripMenuItem;
        private System.Windows.Forms.Label labelLicenseURI;
        private System.Windows.Forms.TextBox textBoxLicenseURI;
        private System.Windows.Forms.Button buttonLicenseURI;
        private System.Windows.Forms.Label labelResourceRights;
        private System.Windows.Forms.TextBox textBoxResourceRights;
        private System.Windows.Forms.Label labelResourceLicense;
        private System.Windows.Forms.TextBox textBoxResourceLicense;
        private System.Windows.Forms.Button buttonResourceLicense;
        private System.Windows.Forms.BindingSource projectResourceBindingSource;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem descriptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem proceduresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setPublishedAddressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem websitesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadApplicationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem informationModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDatabaseAsCsvbcpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolsToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageReferences;
        private System.Windows.Forms.SplitContainer splitContainerReference;
        private System.Windows.Forms.ListBox listBoxReference;
        private System.Windows.Forms.ToolStrip toolStripReference;
        private System.Windows.Forms.ToolStripButton toolStripButtonReferenceNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonReferenceDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReference;
        private System.Windows.Forms.Label labelReferenceDetails;
        private System.Windows.Forms.Label labelReferenceNotes;
        private System.Windows.Forms.TextBox textBoxReferenceNotes;
        private System.Windows.Forms.TextBox textBoxReferenceDetails;
        private System.Windows.Forms.BindingSource projectReferenceBindingSource;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSettings;
        private System.Windows.Forms.TreeView treeViewSettings;
        private System.Windows.Forms.ToolStrip toolStripSettings;
        private System.Windows.Forms.ToolStripButton toolStripButtonSettingAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonSettingRemove;
        private System.Windows.Forms.ToolStripButton toolStripButtonSettingCopy;
        private System.Windows.Forms.Label labelSettingValue;
        private System.Windows.Forms.TextBox textBoxSettingValue;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorSetting;
        private System.Windows.Forms.ToolStripButton toolStripButtonSettingEdit;
        private System.Windows.Forms.Button buttonSettingValueSave;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectRemoveParent;
        private System.Windows.Forms.TabPage tabPageLicenses;
        private System.Windows.Forms.SplitContainer splitContainerLicense;
        private System.Windows.Forms.ListBox listBoxLicense;
        private System.Windows.Forms.ToolStrip toolStripLicense;
        private System.Windows.Forms.ToolStripButton toolStripButtonLicenseAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonLicenseRemove;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLicense;
        private System.Windows.Forms.Label labelLicenseYear;
        private System.Windows.Forms.TextBox textBoxLicenseYear;
        private System.Windows.Forms.Label labelLicenseType;
        private System.Windows.Forms.TextBox textBoxIPR;
        private System.Windows.Forms.TextBox textBoxCopyright;
        private System.Windows.Forms.ComboBox comboBoxLicenseContext;
        private System.Windows.Forms.TextBox textBoxLicenseNotes;
        private System.Windows.Forms.Label labelLicenseContext;
        private System.Windows.Forms.ComboBox comboBoxLicenseType;
        private System.Windows.Forms.TextBox textBoxLicenseDisplayText;
        private System.Windows.Forms.BindingSource projectLicenseBindingSource;
        private System.Windows.Forms.Label labelLicenseDetails;
        private System.Windows.Forms.TextBox textBoxLicenseDetails;
        private System.Windows.Forms.GroupBox groupBoxLicenseNotes;
        private System.Windows.Forms.GroupBox groupBoxCopyright;
        private System.Windows.Forms.GroupBox groupBoxIPR;
        private System.Windows.Forms.GroupBox groupBoxLicense;
        private System.Windows.Forms.PictureBox pictureBoxLicenseHolder;
        private System.Windows.Forms.PictureBox pictureBoxCopyrightHolder;
        private System.Windows.Forms.PictureBox pictureBoxIPRHolder;
        private System.Windows.Forms.Label labelAgentType;
        private System.Windows.Forms.ComboBox comboBoxAgentType;
        private System.Windows.Forms.ToolStripMenuItem databaseAdministrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tableEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem agentToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonAgentMoveToTop;
        private System.Windows.Forms.ToolStripButton toolStripButtonAgentMoveToBase;
        private System.Windows.Forms.TabPage tabPageID;
        private System.Windows.Forms.SplitContainer splitContainerID;
        private System.Windows.Forms.ListBox listBoxID;
        private System.Windows.Forms.ToolStrip toolStripID;
        private System.Windows.Forms.ToolStripButton toolStripButtonIDadd;
        private System.Windows.Forms.ToolStripButton toolStripButtonIDremove;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelID;
        private System.Windows.Forms.TextBox textBoxIdentifier;
        private System.Windows.Forms.Label labelIDtype;
        private System.Windows.Forms.ComboBox comboBoxIDtype;
        private System.Windows.Forms.Label labelIDurl;
        private System.Windows.Forms.TextBox textBoxIDurl;
        private System.Windows.Forms.Label labelIDnotes;
        private System.Windows.Forms.Button buttonIDgetURL;
        private System.Windows.Forms.TextBox textBoxIDnotes;
        private System.Windows.Forms.Label labelReferenceType;
        private System.Windows.Forms.BindingSource projectIdentifierBindingSource;
        private System.Windows.Forms.ToolStripMenuItem identifierTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iPRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem referencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iDsToolStripMenuItem;
        private System.Windows.Forms.BindingSource projectIdentifierTypeBindingSource;
        private System.Windows.Forms.ComboBox comboBoxReferenceType;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportWizardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stableIdentifierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resourcesDirectoryToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxLicenseID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReferenceLists;
        private System.Windows.Forms.Label labelCitation;
        private System.Windows.Forms.Label labelReferences;
        private System.Windows.Forms.ToolStrip toolStripCitations;
        private System.Windows.Forms.ToolStripButton toolStripButtonCitationAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonCitationDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonCitationToReference;
        private System.Windows.Forms.ToolStripButton toolStripButtonReferenceToCitation;
        private System.Windows.Forms.ListBox listBoxCitations;
        private System.Windows.Forms.Label labelProjectDescriptionPublic;
        private System.Windows.Forms.TextBox textBoxProjectDescriptionPublic;
        private System.Windows.Forms.Label labelProjectDescriptionInternal;
        private System.Windows.Forms.TextBox textBoxProjectDescriptionInternal;
        private System.Windows.Forms.Label labelProjectType;
        private System.Windows.Forms.ComboBox comboBoxProjectType;
        private System.Windows.Forms.Label labelReferenceUri;
        private System.Windows.Forms.TextBox textBoxReferenceUri;
        private System.Windows.Forms.Button buttonReferenceUri;
        private System.Windows.Forms.Label labelReferenceCitation;
        private System.Windows.Forms.TextBox textBoxReferenceCitation;
        private System.Windows.Forms.Label labelProjectDescriptionType;
        private System.Windows.Forms.ComboBox comboBoxProjectDescriptionType;
        private System.Windows.Forms.Label labelReferenceTypeDependentCitation;
        private System.Windows.Forms.SplitContainer splitContainerNotesAndLabel;
        private System.Windows.Forms.ToolStrip toolStripRoles;
        private System.Windows.Forms.ToolStripButton toolStripButtonRolesAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonRolesDelete;
        private System.Windows.Forms.ListBox listBoxRoles;
        private System.Windows.Forms.BindingSource projectAgentRoleBindingSource;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gFBioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keywordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem labelToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonResourceAddLink;
        private System.Windows.Forms.Button buttonResourceOpen;
        private System.Windows.Forms.TabPage tabPageDescriptor;
        private System.Windows.Forms.SplitContainer splitContainerDescriptor;
        private System.Windows.Forms.ListBox listBoxDescriptor;
        private System.Windows.Forms.ToolStrip toolStripDescriptor;
        private System.Windows.Forms.ToolStripButton toolStripButtonDescriptorAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonDescriptorDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDescriptor;
        private System.Windows.Forms.Label labelDescriptorLanguage;
        private System.Windows.Forms.ComboBox comboBoxDescriptorLanguage;
        private System.Windows.Forms.TextBox textBoxDescriptorContent;
        private System.Windows.Forms.Label labelDescriptorContent;
        private System.Windows.Forms.Label labelDescriptorElement;
        private System.Windows.Forms.Label labelDescriptorContentUri;
        private System.Windows.Forms.TextBox textBoxDescriptorContentUri;
        private System.Windows.Forms.ComboBox comboBoxDescriptorElement;
        private System.Windows.Forms.ToolStripMenuItem descriptorTypesToolStripMenuItem;
        private System.Windows.Forms.BindingSource projectDescriptorBindingSource;
        private System.Windows.Forms.BindingSource languageEnumBindingSource;
        private System.Windows.Forms.BindingSource projectDescriptorElementBindingSource;
        private System.Windows.Forms.ToolStripMenuItem descriptorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem errorLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepErrorlogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearErrorlogAtProgramStartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearErrorlogToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageArchive;
        private System.Windows.Forms.SplitContainer splitContainerArchive;
        private System.Windows.Forms.ToolStrip toolStripArchive;
        private System.Windows.Forms.ToolStripButton toolStripButtonArchiveAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonArchiveDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelArchive;
        private System.Windows.Forms.Label labelArchiveTransfer;
        private System.Windows.Forms.ComboBox comboBoxArchiveFormat;
        private System.Windows.Forms.TreeView treeViewArchive;
        private System.Windows.Forms.ComboBox comboBoxArchiveMimeType;
        private System.Windows.Forms.Label labelArchiveFileSize;
        private System.Windows.Forms.TextBox textBoxArchiveFileSize;
        private System.Windows.Forms.Label labelArchiveFileHash;
        private System.Windows.Forms.TextBox textBoxArchiveFileHash;
        private System.Windows.Forms.Label labelArchiveStorageLocation;
        private System.Windows.Forms.TextBox textBoxArchiveStorageLocation;
        private System.Windows.Forms.Label labelArchiveExternalStorageLocation;
        private System.Windows.Forms.TextBox textBoxArchiveExternalStorageLocation;
        private System.Windows.Forms.Label labelArchiveTransferDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerArchiveTransferDate;
        private System.Windows.Forms.Label labelArchivePublicationDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerArchivePublicationDate;
        private System.Windows.Forms.CheckBox checkBoxArchivePublished;
        private System.Windows.Forms.Label labelArchiveNotes;
        private System.Windows.Forms.TextBox textBoxArchiveNotes;
        private System.Windows.Forms.Label labelArchiveDataWithholdingReason;
        private System.Windows.Forms.PictureBox pictureBoxArchiveDataWithholdingReason;
        private System.Windows.Forms.TextBox textBoxArchiveDataWithholdingReason;
        private System.Windows.Forms.Button buttonArchiveStorageLocation;
        private System.Windows.Forms.Button buttonArchiveExternalStorageLocation;
        private System.Windows.Forms.PictureBox pictureBoxArchivePublished;
        private System.Windows.Forms.BindingSource projectArchiveBindingSource;
        private System.Windows.Forms.LinkLabel linkLabelArchiveFormat;
        private System.Windows.Forms.LinkLabel linkLabelArchiveMimeType;
        private System.Windows.Forms.ToolStripMenuItem archiveFormatsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem archiveMimeTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xMLarchiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administrateArchivesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createArchiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreArchiveToolStripMenuItem;
        private System.Windows.Forms.Label labelEmbargoDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerEmbargoDate;
        private System.Windows.Forms.CheckBox checkBoxCreateArchive;
        private System.Windows.Forms.TextBox textBoxArchiveProtocoll;
        private System.Windows.Forms.TabPage tabPageModules;
        private System.Windows.Forms.TabControl tabControlModules;
        private System.Windows.Forms.TabPage tabPageDiversityAgents;
        private System.Windows.Forms.TabPage tabPageDiversityCollection;
        private System.Windows.Forms.TabPage tabPageDiversityDescriptions;
        private System.Windows.Forms.TabPage tabPageDiversityGazetteer;
        private System.Windows.Forms.TabPage tabPageDiversityReferences;
        private System.Windows.Forms.TabPage tabPageDiversitySamplingPlots;
        private System.Windows.Forms.TabPage tabPageDiversityScientificTerms;
        private System.Windows.Forms.TabPage tabPageDiversityTaxonNames;
        private System.Windows.Forms.ToolStripMenuItem scanModulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diversityAgentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diversityCollectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diversityDescriptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diversityGazetteerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diversityReferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diversitySamplingPlotsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diversityScientificTermsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diversityTaxonNamesToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonRequestPID;
        private System.Windows.Forms.Label labelArchiveIdentifier;
        private System.Windows.Forms.ToolStrip toolStripArchiveIdentier;
        private System.Windows.Forms.ToolStripButton toolStripButtonArchiveIdentierAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonArchiveIdentierDelete;
        private System.Windows.Forms.ListBox listBoxArchiveIdentier;
        private DataSetDiversityProjects dataSetDiversityProjects;
        private System.Windows.Forms.BindingSource projectArchiveIdentifierBindingSource;
        private DataSetDiversityProjectsTableAdapters.ProjectArchiveIdentifierTableAdapter projectArchiveIdentifierTableAdapter;
        private DiversityWorkbench.UserControls.UserControlQueryList userControlQueryList;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.BindingSource projectBindingSource;
        private DataSetDiversityProjectsTableAdapters.ProjectTableAdapter projectTableAdapter;
        private System.Windows.Forms.BindingSource projectSettingBindingSource;
        private DataSetDiversityProjectsTableAdapters.ProjectSettingTableAdapter projectSettingTableAdapter;
        private System.Windows.Forms.BindingSource projectAgentBindingSource;
        private DataSetDiversityProjectsTableAdapters.ProjectAgentTableAdapter projectAgentTableAdapter;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryProjectAgent;
        private DataSetDiversityProjectsTableAdapters.ProjectAgentRoleTableAdapter projectAgentRoleTableAdapter;
        private DataSetDiversityProjectsTableAdapters.ProjectIdentifierTypeTableAdapter projectIdentifierTypeTableAdapter;
        private DataSetDiversityProjectsTableAdapters.ProjectLicenseTableAdapter projectLicenseTableAdapter;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryLicenseHolder;
        private DiversityWorkbench.UserControls.UserControlModuleBackLinks userControlModuleBackLinksAgents;
        private DiversityWorkbench.UserControls.UserControlModuleBackLinks userControlModuleBackLinksCollection;
        private DiversityWorkbench.UserControls.UserControlModuleBackLinks userControlModuleBackLinksDescriptions;
        private DiversityWorkbench.UserControls.UserControlModuleBackLinks userControlModuleBackLinksGazetteer;
        private DiversityWorkbench.UserControls.UserControlModuleBackLinks userControlModuleBackLinksReferences;
        private DiversityWorkbench.UserControls.UserControlModuleBackLinks userControlModuleBackLinksPlots;
        private DiversityWorkbench.UserControls.UserControlModuleBackLinks userControlModuleBackLinksTerms;
        private DiversityWorkbench.UserControls.UserControlModuleBackLinks userControlModuleBackLinksTaxa;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCopyrightHolder;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryCopyrightHolder;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelIPR;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryIPRHolder;
        private DataSetDiversityProjectsTableAdapters.ProjectResourceTableAdapter projectResourceTableAdapter;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryReference;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDescriptorGazetteer;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDescriptorTerm;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDescriptorTaxon;
        private DataSetDiversityProjectsTableAdapters.ProjectDescriptorTableAdapter projectDescriptorTableAdapter;
        private DataSetDiversityProjectsTableAdapters.ProjectDescriptorElementTableAdapter projectDescriptorElementTableAdapter;
        private DataSetDiversityProjectsTableAdapters.ProjectIdentifierTableAdapter projectIdentifierTableAdapter;
        private DataSetDiversityProjectsTableAdapters.ProjectArchiveTableAdapter projectArchiveTableAdapter;
        private DataSetDiversityProjectsTableAdapters.ProjectReferenceTableAdapter projectReferenceTableAdapter;
        private DataSetDiversityProjectsTableAdapters.Language_EnumTableAdapter language_EnumTableAdapter;
        private System.Windows.Forms.ToolStripMenuItem createSchemaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importWizardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentServerActivityToolStripMenuItem;
        private System.Windows.Forms.Label labelAgentRoleDisplayText;
        private System.Windows.Forms.ToolStripMenuItem metadataToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBoxIdentifierLocked;
        private System.Windows.Forms.ToolStripMenuItem connectionTimeoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryTimeoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xGA1024X768ToolStripMenuItem;
        private System.Windows.Forms.Button buttonDescriptorTypes;
        private System.Windows.Forms.Button buttonIDtypes;
        private System.Windows.Forms.Label labelArchiveParentRelationType;
        private System.Windows.Forms.ComboBox comboBoxArchiveParentRelationType;
        private System.Windows.Forms.ToolStripMenuItem createDatabaseToolStripMenuItem;
        private System.Windows.Forms.LinkLabel linkLabelURL;
        private System.Windows.Forms.ImageList imageListTreeview;
        private System.Windows.Forms.Button buttonEmbargoDate;
        private System.Windows.Forms.ToolStripMenuItem projectSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem askOnExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorTransfer;
        private System.Windows.Forms.Button ShowJsonCache;
        private System.Windows.Forms.ToolStripMenuItem jsonCacheToolStripMenuItem;
    }
}

