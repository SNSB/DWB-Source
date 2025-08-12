namespace DiversityWorkbench.Forms
{
    partial class FormDatabaseTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseTool));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageDescription = new System.Windows.Forms.TabPage();
            this.splitContainerDescription = new System.Windows.Forms.SplitContainer();
            this.treeViewDescription = new System.Windows.Forms.TreeView();
            this.tableLayoutPanelDescription = new System.Windows.Forms.TableLayoutPanel();
            this.labelObjectName = new System.Windows.Forms.Label();
            this.textBoxObjectName = new System.Windows.Forms.TextBox();
            this.labelObjectType = new System.Windows.Forms.Label();
            this.textBoxObjectDescription = new System.Windows.Forms.TextBox();
            this.textBoxObjectType = new System.Windows.Forms.TextBox();
            this.labelObjectDefinition = new System.Windows.Forms.Label();
            this.labelObjectDescription = new System.Windows.Forms.Label();
            this.textBoxObjectDefinition = new System.Windows.Forms.TextBox();
            this.labelObjectDatatype = new System.Windows.Forms.Label();
            this.textBoxObjectDatatype = new System.Windows.Forms.TextBox();
            this.textBoxObjectAction = new System.Windows.Forms.TextBox();
            this.labelObjectAction = new System.Windows.Forms.Label();
            this.buttonObjectDescriptionSQLadd = new System.Windows.Forms.Button();
            this.buttonObjectDescriptionSQLupdate = new System.Windows.Forms.Button();
            this.buttonObjectDescriptionSQLany = new System.Windows.Forms.Button();
            this.buttonFillDescriptionCache = new System.Windows.Forms.Button();
            this.buttonViewDescriptionCache = new System.Windows.Forms.Button();
            this.checkBoxDescriptionAddExistenceCheck = new System.Windows.Forms.CheckBox();
            this.buttonDescriptionAddDeprecated = new System.Windows.Forms.Button();
            this.tabPageLogging = new System.Windows.Forms.TabPage();
            this.splitContainerLogging = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelLogging = new System.Windows.Forms.TableLayoutPanel();
            this.buttonListTablesForTrigger = new System.Windows.Forms.Button();
            this.labelLoggingTableList = new System.Windows.Forms.Label();
            this.labelLoggingVersionMaster = new System.Windows.Forms.Label();
            this.comboBoxLoggingVersionMasterTable = new System.Windows.Forms.ComboBox();
            this.listBoxLoggingTables = new System.Windows.Forms.ListBox();
            this.tabControlLoggingDefinitions = new System.Windows.Forms.TabControl();
            this.tabPageTable = new System.Windows.Forms.TabPage();
            this.dataGridViewTable = new System.Windows.Forms.DataGridView();
            this.ColumnOK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panelTableButtons = new System.Windows.Forms.Panel();
            this.checkBoxAddRowGUID = new System.Windows.Forms.CheckBox();
            this.checkBoxAddInsertColumns = new System.Windows.Forms.CheckBox();
            this.buttonAttachLogColumns = new System.Windows.Forms.Button();
            this.checkBoxLogcolumnsDSGVO = new System.Windows.Forms.CheckBox();
            this.tabPageLogTable = new System.Windows.Forms.TabPage();
            this.textBoxLogTable = new System.Windows.Forms.TextBox();
            this.panelLogTableButtons = new System.Windows.Forms.Panel();
            this.checkBoxLogTableDSGVO = new System.Windows.Forms.CheckBox();
            this.checkBoxAddVersion = new System.Windows.Forms.CheckBox();
            this.checkBoxKeepOldLogTable = new System.Windows.Forms.CheckBox();
            this.buttonLogTableCreate = new System.Windows.Forms.Button();
            this.buttonLogTableShowSQL = new System.Windows.Forms.Button();
            this.tabPageInsertTrigger = new System.Windows.Forms.TabPage();
            this.textBoxInsertTriggerNew = new System.Windows.Forms.TextBox();
            this.panelInsertTriggerButtons = new System.Windows.Forms.Panel();
            this.checkBoxInsertTriggerAddVersion = new System.Windows.Forms.CheckBox();
            this.buttonInsertTriggerCreate = new System.Windows.Forms.Button();
            this.buttonInsertTriggerShowSQL = new System.Windows.Forms.Button();
            this.textBoxInsertTrigger = new System.Windows.Forms.TextBox();
            this.tabPageUpdateTrigger = new System.Windows.Forms.TabPage();
            this.textBoxUpdateTriggerNew = new System.Windows.Forms.TextBox();
            this.panelUpdateTriggerButtons = new System.Windows.Forms.Panel();
            this.checkBoxUpdateTriggerAddDsgvo = new System.Windows.Forms.CheckBox();
            this.checkBoxUpdateTriggerAddVersion = new System.Windows.Forms.CheckBox();
            this.buttonUpdateTriggerCreate = new System.Windows.Forms.Button();
            this.buttonUpdateTriggerShowSQL = new System.Windows.Forms.Button();
            this.textBoxUpdateTrigger = new System.Windows.Forms.TextBox();
            this.tabPageDeleteTrigger = new System.Windows.Forms.TabPage();
            this.textBoxDeleteTriggerNew = new System.Windows.Forms.TextBox();
            this.panelDeleteTriggerButtons = new System.Windows.Forms.Panel();
            this.checkBoxDeleteTriggerAddDsgvo = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteTriggerAddVersion = new System.Windows.Forms.CheckBox();
            this.buttonDeleteTriggerCreate = new System.Windows.Forms.Button();
            this.buttonDeleteTriggerShowSql = new System.Windows.Forms.Button();
            this.textBoxDeleteTrigger = new System.Windows.Forms.TextBox();
            this.tabPageProcSetVersion = new System.Windows.Forms.TabPage();
            this.splitContainerProcVersion = new System.Windows.Forms.SplitContainer();
            this.textBoxProcSetVersion = new System.Windows.Forms.TextBox();
            this.textBoxProcSetVersionNew = new System.Windows.Forms.TextBox();
            this.panelProcSetVersionButtons = new System.Windows.Forms.Panel();
            this.checkBoxProcSetVersionDsgvo = new System.Windows.Forms.CheckBox();
            this.buttonProcSetVersionCreate = new System.Windows.Forms.Button();
            this.buttonProcSetVersionShowSql = new System.Windows.Forms.Button();
            this.tabPageClearLog = new System.Windows.Forms.TabPage();
            this.splitContainerClearLog = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelClearLog = new System.Windows.Forms.TableLayoutPanel();
            this.buttonClearLogListTables = new System.Windows.Forms.Button();
            this.labelClearLogTableList = new System.Windows.Forms.Label();
            this.checkedListBoxClearLog = new System.Windows.Forms.CheckedListBox();
            this.buttonClearLogSelectAll = new System.Windows.Forms.Button();
            this.buttonClearLogSelectNone = new System.Windows.Forms.Button();
            this.buttonClearLogStart = new System.Windows.Forms.Button();
            this.dataGridViewClearLog = new System.Windows.Forms.DataGridView();
            this.tabPageSaveLog = new System.Windows.Forms.TabPage();
            this.tabControlSaveLog = new System.Windows.Forms.TabControl();
            this.tabPageSaveToLogDB = new System.Windows.Forms.TabPage();
            this.splitContainerSaveLog = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelSaveLog = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSaveLog = new System.Windows.Forms.Button();
            this.labelSaveLogStartDate = new System.Windows.Forms.Label();
            this.buttonSaveLogListTables = new System.Windows.Forms.Button();
            this.dateTimePickerSaveLogStartDate = new System.Windows.Forms.DateTimePicker();
            this.buttonSaveLogUserAdministration = new System.Windows.Forms.Button();
            this.buttonSaveLogCopyUser = new System.Windows.Forms.Button();
            this.dataGridViewSaveLog = new System.Windows.Forms.DataGridView();
            this.labelSaveLogTableListHeader = new System.Windows.Forms.Label();
            this.labelSaveLogMainMessage = new System.Windows.Forms.Label();
            this.tabPageProcedures = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelProcedures = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelProcedureResult = new System.Windows.Forms.TableLayoutPanel();
            this.labelProcedureResult = new System.Windows.Forms.Label();
            this.splitContainerProcedureResult = new System.Windows.Forms.SplitContainer();
            this.dataGridViewProcedureResult = new System.Windows.Forms.DataGridView();
            this.textBoxProcedureResult = new System.Windows.Forms.TextBox();
            this.textBoxProcedureSQL = new System.Windows.Forms.TextBox();
            this.labelProcedureCall = new System.Windows.Forms.Label();
            this.textBoxProcedureDefinition = new System.Windows.Forms.TextBox();
            this.textBoxProcedureDescription = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelProcedureParameter = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxProcedureParameter6 = new System.Windows.Forms.TextBox();
            this.labelProcedureParameter6 = new System.Windows.Forms.Label();
            this.textBoxProcedureParameter5 = new System.Windows.Forms.TextBox();
            this.labelProcedureParameter5 = new System.Windows.Forms.Label();
            this.labelProcedureParameter = new System.Windows.Forms.Label();
            this.labelProcedureParameterValue = new System.Windows.Forms.Label();
            this.labelProcedureParameter1 = new System.Windows.Forms.Label();
            this.labelProcedureParameter2 = new System.Windows.Forms.Label();
            this.labelProcedureParameter3 = new System.Windows.Forms.Label();
            this.labelProcedureParameter4 = new System.Windows.Forms.Label();
            this.textBoxProcedureParameter1 = new System.Windows.Forms.TextBox();
            this.textBoxProcedureParameter2 = new System.Windows.Forms.TextBox();
            this.textBoxProcedureParameter3 = new System.Windows.Forms.TextBox();
            this.textBoxProcedureParameter4 = new System.Windows.Forms.TextBox();
            this.labelTimeElapsed = new System.Windows.Forms.Label();
            this.textBoxProcedureReturns = new System.Windows.Forms.TextBox();
            this.labelProcedureList = new System.Windows.Forms.Label();
            this.buttonStartDataTransfer = new System.Windows.Forms.Button();
            this.labelTimeout = new System.Windows.Forms.Label();
            this.labelProcedureReturns = new System.Windows.Forms.Label();
            this.comboBoxProcedureList = new System.Windows.Forms.ComboBox();
            this.textBoxProcedureType = new System.Windows.Forms.TextBox();
            this.labelProcedureType = new System.Windows.Forms.Label();
            this.numericUpDownTimeout = new System.Windows.Forms.NumericUpDown();
            this.textBoxTimeElapsed = new System.Windows.Forms.TextBox();
            this.tabPageRowGUID = new System.Windows.Forms.TabPage();
            this.splitContainerReplication = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelReplicationTableList = new System.Windows.Forms.TableLayoutPanel();
            this.buttonReplicationListTables = new System.Windows.Forms.Button();
            this.labelReplicationTableList = new System.Windows.Forms.Label();
            this.checkedListBoxRepPrep = new System.Windows.Forms.CheckedListBox();
            this.buttonRepPrepAll = new System.Windows.Forms.Button();
            this.buttonRepPrepNone = new System.Windows.Forms.Button();
            this.tabControlReplication = new System.Windows.Forms.TabControl();
            this.tabPageRepPrepScript = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelRepPrepScript = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxRepPrepDsgvo = new System.Windows.Forms.CheckBox();
            this.textBoxRepPrepScript = new System.Windows.Forms.TextBox();
            this.buttonRepPrepScript = new System.Windows.Forms.Button();
            this.buttonRepPrepScriptSave = new System.Windows.Forms.Button();
            this.buttonRepPrepScriptFile = new System.Windows.Forms.Button();
            this.textBoxRepPrepScriptFile = new System.Windows.Forms.TextBox();
            this.tabPageRepPrep = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelReplicationPreparationSteps = new System.Windows.Forms.TableLayoutPanel();
            this.labelRepPrepHeader = new System.Windows.Forms.Label();
            this.buttonStartReplicationPreparation = new System.Windows.Forms.Button();
            this.userControlReplicationPreparation02AddRowGUID = new DiversityWorkbench.UserControls.UserControlReplicationPreparation();
            this.userControlReplicationPreparation03CreateDefaults = new DiversityWorkbench.UserControls.UserControlReplicationPreparation();
            this.userControlReplicationPreparation04CreateTemporaryTable = new DiversityWorkbench.UserControls.UserControlReplicationPreparation();
            this.userControlReplicationPreparation05ReadData = new DiversityWorkbench.UserControls.UserControlReplicationPreparation();
            this.userControlReplicationPreparation06DeactivateUpdateTrigger = new DiversityWorkbench.UserControls.UserControlReplicationPreparation();
            this.userControlReplicationPreparation07WriteRowGUID = new DiversityWorkbench.UserControls.UserControlReplicationPreparation();
            this.userControlReplicationPreparation08WriteDate = new DiversityWorkbench.UserControls.UserControlReplicationPreparation();
            this.userControlReplicationPreparation09ActivateUpdateTrigger = new DiversityWorkbench.UserControls.UserControlReplicationPreparation();
            this.userControlReplicationPreparation10DeleteTempTable = new DiversityWorkbench.UserControls.UserControlReplicationPreparation();
            this.labelRepPrepMessage = new System.Windows.Forms.Label();
            this.userControlReplicationPreparation01CreateReplPublTable = new DiversityWorkbench.UserControls.UserControlReplicationPreparation();
            this.tabPageEuDsgvo = new System.Windows.Forms.TabPage();
            this.splitContainerEuDsgvo = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelEuDsgvo = new System.Windows.Forms.TableLayoutPanel();
            this.checkedListBoxEuDsgvo = new System.Windows.Forms.CheckedListBox();
            this.buttonEuDsgvoListTables = new System.Windows.Forms.Button();
            this.buttonEuDsgvoSelectAll = new System.Windows.Forms.Button();
            this.buttonEuDsgvoSelectNone = new System.Windows.Forms.Button();
            this.tabControlEuDsgvo = new System.Windows.Forms.TabControl();
            this.tabPageEuDsgvoScript = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelEuDsgvoScript = new System.Windows.Forms.TableLayoutPanel();
            this.buttonEuDsgvoScriptCreate = new System.Windows.Forms.Button();
            this.textBoxEuDsgvoScript = new System.Windows.Forms.TextBox();
            this.buttonEuDsgvoScriptSave = new System.Windows.Forms.Button();
            this.textBoxEuDsgvoScriptFile = new System.Windows.Forms.TextBox();
            this.buttonEuDsgvoScriptFolder = new System.Windows.Forms.Button();
            this.radioButtonEuDsgvoScriptTriggerOld = new System.Windows.Forms.RadioButton();
            this.radioButtonEuDsgvoScriptTriggerNew = new System.Windows.Forms.RadioButton();
            this.checkBoxEuDsgvoScriptTriggerNewVersion = new System.Windows.Forms.CheckBox();
            this.comboBoxEuDsgvoScriptTriggerNewVersion = new System.Windows.Forms.ComboBox();
            this.checkBoxEuDsgvoScriptIncludeBasics = new System.Windows.Forms.CheckBox();
            this.tabPageEuDsgvoScriptTriggerEtc = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc = new System.Windows.Forms.TableLayoutPanel();
            this.buttonEuDsgvoScriptForTriggerEtcCreateScript = new System.Windows.Forms.Button();
            this.textBoxEuDsgvoScriptForTriggerEtcScript = new System.Windows.Forms.TextBox();
            this.buttonEuDsgvoScriptForTriggerEtcSaveScript = new System.Windows.Forms.Button();
            this.textBoxEuDsgvoScriptForTriggerEtcFile = new System.Windows.Forms.TextBox();
            this.buttonEuDsgvoScriptForTriggerEtcFolder = new System.Windows.Forms.Button();
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions = new System.Windows.Forms.CheckBox();
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures = new System.Windows.Forms.CheckBox();
            this.labelEuDsgvoScriptForTriggerEtc = new System.Windows.Forms.Label();
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger = new System.Windows.Forms.CheckBox();
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews = new System.Windows.Forms.CheckBox();
            this.tabPageEuDsgvoInfoURL = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelEuDsgvoInfoURL = new System.Windows.Forms.TableLayoutPanel();
            this.labelEuDsgvoInfoURL = new System.Windows.Forms.Label();
            this.buttonEuDsgvoInfoURLSetValue = new System.Windows.Forms.Button();
            this.linkLabelEuDsgvoInfoURL = new System.Windows.Forms.LinkLabel();
            this.userControlWebViewEuDsgvoInfoURL = new DiversityWorkbench.UserControls.UserControlWebView();
            this.tabPageEuDsgvoRemoveUser = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelEuDsgvoRemoveUser = new System.Windows.Forms.TableLayoutPanel();
            this.labelEuDsgvoRemoveUser = new System.Windows.Forms.Label();
            this.comboBoxEuDsgvoRemoveUser = new System.Windows.Forms.ComboBox();
            this.buttonEuDsgvoRemoveUser = new System.Windows.Forms.Button();
            this.textBoxEuDsgvoRemoveUserInfo = new System.Windows.Forms.TextBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.labelEuDsgvo = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelLogTable = new System.Windows.Forms.TableLayoutPanel();
            this.tabControlMain.SuspendLayout();
            this.tabPageDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDescription)).BeginInit();
            this.splitContainerDescription.Panel1.SuspendLayout();
            this.splitContainerDescription.Panel2.SuspendLayout();
            this.splitContainerDescription.SuspendLayout();
            this.tableLayoutPanelDescription.SuspendLayout();
            this.tabPageLogging.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogging)).BeginInit();
            this.splitContainerLogging.Panel1.SuspendLayout();
            this.splitContainerLogging.Panel2.SuspendLayout();
            this.splitContainerLogging.SuspendLayout();
            this.tableLayoutPanelLogging.SuspendLayout();
            this.tabControlLoggingDefinitions.SuspendLayout();
            this.tabPageTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).BeginInit();
            this.panelTableButtons.SuspendLayout();
            this.tabPageLogTable.SuspendLayout();
            this.panelLogTableButtons.SuspendLayout();
            this.tabPageInsertTrigger.SuspendLayout();
            this.panelInsertTriggerButtons.SuspendLayout();
            this.tabPageUpdateTrigger.SuspendLayout();
            this.panelUpdateTriggerButtons.SuspendLayout();
            this.tabPageDeleteTrigger.SuspendLayout();
            this.panelDeleteTriggerButtons.SuspendLayout();
            this.tabPageProcSetVersion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProcVersion)).BeginInit();
            this.splitContainerProcVersion.Panel1.SuspendLayout();
            this.splitContainerProcVersion.Panel2.SuspendLayout();
            this.splitContainerProcVersion.SuspendLayout();
            this.panelProcSetVersionButtons.SuspendLayout();
            this.tabPageClearLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerClearLog)).BeginInit();
            this.splitContainerClearLog.Panel1.SuspendLayout();
            this.splitContainerClearLog.Panel2.SuspendLayout();
            this.splitContainerClearLog.SuspendLayout();
            this.tableLayoutPanelClearLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClearLog)).BeginInit();
            this.tabPageSaveLog.SuspendLayout();
            this.tabControlSaveLog.SuspendLayout();
            this.tabPageSaveToLogDB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSaveLog)).BeginInit();
            this.splitContainerSaveLog.Panel1.SuspendLayout();
            this.splitContainerSaveLog.Panel2.SuspendLayout();
            this.splitContainerSaveLog.SuspendLayout();
            this.tableLayoutPanelSaveLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSaveLog)).BeginInit();
            this.tabPageProcedures.SuspendLayout();
            this.tableLayoutPanelProcedures.SuspendLayout();
            this.tableLayoutPanelProcedureResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProcedureResult)).BeginInit();
            this.splitContainerProcedureResult.Panel1.SuspendLayout();
            this.splitContainerProcedureResult.Panel2.SuspendLayout();
            this.splitContainerProcedureResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcedureResult)).BeginInit();
            this.tableLayoutPanelProcedureParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).BeginInit();
            this.tabPageRowGUID.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerReplication)).BeginInit();
            this.splitContainerReplication.Panel1.SuspendLayout();
            this.splitContainerReplication.Panel2.SuspendLayout();
            this.splitContainerReplication.SuspendLayout();
            this.tableLayoutPanelReplicationTableList.SuspendLayout();
            this.tabControlReplication.SuspendLayout();
            this.tabPageRepPrepScript.SuspendLayout();
            this.tableLayoutPanelRepPrepScript.SuspendLayout();
            this.tabPageRepPrep.SuspendLayout();
            this.tableLayoutPanelReplicationPreparationSteps.SuspendLayout();
            this.tabPageEuDsgvo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerEuDsgvo)).BeginInit();
            this.splitContainerEuDsgvo.Panel1.SuspendLayout();
            this.splitContainerEuDsgvo.Panel2.SuspendLayout();
            this.splitContainerEuDsgvo.SuspendLayout();
            this.tableLayoutPanelEuDsgvo.SuspendLayout();
            this.tabControlEuDsgvo.SuspendLayout();
            this.tabPageEuDsgvoScript.SuspendLayout();
            this.tableLayoutPanelEuDsgvoScript.SuspendLayout();
            this.tabPageEuDsgvoScriptTriggerEtc.SuspendLayout();
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.SuspendLayout();
            this.tabPageEuDsgvoInfoURL.SuspendLayout();
            this.tableLayoutPanelEuDsgvoInfoURL.SuspendLayout();
            this.tabPageEuDsgvoRemoveUser.SuspendLayout();
            this.tableLayoutPanelEuDsgvoRemoveUser.SuspendLayout();
            this.tableLayoutPanelLogTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageDescription);
            this.tabControlMain.Controls.Add(this.tabPageLogging);
            this.tabControlMain.Controls.Add(this.tabPageClearLog);
            this.tabControlMain.Controls.Add(this.tabPageSaveLog);
            this.tabControlMain.Controls.Add(this.tabPageProcedures);
            this.tabControlMain.Controls.Add(this.tabPageRowGUID);
            this.tabControlMain.Controls.Add(this.tabPageEuDsgvo);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageList;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(871, 552);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageDescription
            // 
            this.tabPageDescription.Controls.Add(this.splitContainerDescription);
            this.tabPageDescription.ImageIndex = 0;
            this.tabPageDescription.Location = new System.Drawing.Point(4, 23);
            this.tabPageDescription.Name = "tabPageDescription";
            this.tabPageDescription.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDescription.Size = new System.Drawing.Size(863, 525);
            this.tabPageDescription.TabIndex = 0;
            this.tabPageDescription.Text = "Description";
            this.tabPageDescription.UseVisualStyleBackColor = true;
            // 
            // splitContainerDescription
            // 
            this.splitContainerDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDescription.Location = new System.Drawing.Point(3, 3);
            this.splitContainerDescription.Name = "splitContainerDescription";
            // 
            // splitContainerDescription.Panel1
            // 
            this.splitContainerDescription.Panel1.Controls.Add(this.treeViewDescription);
            // 
            // splitContainerDescription.Panel2
            // 
            this.splitContainerDescription.Panel2.Controls.Add(this.tableLayoutPanelDescription);
            this.splitContainerDescription.Size = new System.Drawing.Size(857, 519);
            this.splitContainerDescription.SplitterDistance = 194;
            this.splitContainerDescription.TabIndex = 1;
            // 
            // treeViewDescription
            // 
            this.treeViewDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDescription.Location = new System.Drawing.Point(0, 0);
            this.treeViewDescription.Name = "treeViewDescription";
            this.treeViewDescription.Size = new System.Drawing.Size(194, 519);
            this.treeViewDescription.TabIndex = 0;
            this.treeViewDescription.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDescription_AfterSelect);
            // 
            // tableLayoutPanelDescription
            // 
            this.tableLayoutPanelDescription.ColumnCount = 4;
            this.tableLayoutPanelDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanelDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelDescription.Controls.Add(this.labelObjectName, 0, 0);
            this.tableLayoutPanelDescription.Controls.Add(this.textBoxObjectName, 1, 0);
            this.tableLayoutPanelDescription.Controls.Add(this.labelObjectType, 0, 1);
            this.tableLayoutPanelDescription.Controls.Add(this.textBoxObjectDescription, 1, 4);
            this.tableLayoutPanelDescription.Controls.Add(this.textBoxObjectType, 1, 1);
            this.tableLayoutPanelDescription.Controls.Add(this.labelObjectDefinition, 0, 2);
            this.tableLayoutPanelDescription.Controls.Add(this.labelObjectDescription, 0, 3);
            this.tableLayoutPanelDescription.Controls.Add(this.textBoxObjectDefinition, 1, 2);
            this.tableLayoutPanelDescription.Controls.Add(this.labelObjectDatatype, 2, 1);
            this.tableLayoutPanelDescription.Controls.Add(this.textBoxObjectDatatype, 3, 1);
            this.tableLayoutPanelDescription.Controls.Add(this.textBoxObjectAction, 3, 0);
            this.tableLayoutPanelDescription.Controls.Add(this.labelObjectAction, 2, 0);
            this.tableLayoutPanelDescription.Controls.Add(this.buttonObjectDescriptionSQLadd, 0, 7);
            this.tableLayoutPanelDescription.Controls.Add(this.buttonObjectDescriptionSQLupdate, 0, 8);
            this.tableLayoutPanelDescription.Controls.Add(this.buttonObjectDescriptionSQLany, 0, 9);
            this.tableLayoutPanelDescription.Controls.Add(this.buttonFillDescriptionCache, 0, 4);
            this.tableLayoutPanelDescription.Controls.Add(this.buttonViewDescriptionCache, 0, 5);
            this.tableLayoutPanelDescription.Controls.Add(this.checkBoxDescriptionAddExistenceCheck, 0, 6);
            this.tableLayoutPanelDescription.Controls.Add(this.buttonDescriptionAddDeprecated, 1, 3);
            this.tableLayoutPanelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDescription.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDescription.Name = "tableLayoutPanelDescription";
            this.tableLayoutPanelDescription.RowCount = 10;
            this.tableLayoutPanelDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDescription.Size = new System.Drawing.Size(659, 519);
            this.tableLayoutPanelDescription.TabIndex = 1;
            // 
            // labelObjectName
            // 
            this.labelObjectName.AutoSize = true;
            this.labelObjectName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelObjectName.Location = new System.Drawing.Point(3, 0);
            this.labelObjectName.Name = "labelObjectName";
            this.labelObjectName.Size = new System.Drawing.Size(75, 26);
            this.labelObjectName.TabIndex = 1;
            this.labelObjectName.Text = "Name:";
            this.labelObjectName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxObjectName
            // 
            this.textBoxObjectName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxObjectName.Location = new System.Drawing.Point(84, 3);
            this.textBoxObjectName.Name = "textBoxObjectName";
            this.textBoxObjectName.ReadOnly = true;
            this.textBoxObjectName.Size = new System.Drawing.Size(340, 20);
            this.textBoxObjectName.TabIndex = 2;
            // 
            // labelObjectType
            // 
            this.labelObjectType.AutoSize = true;
            this.labelObjectType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelObjectType.Location = new System.Drawing.Point(3, 26);
            this.labelObjectType.Name = "labelObjectType";
            this.labelObjectType.Size = new System.Drawing.Size(75, 26);
            this.labelObjectType.TabIndex = 3;
            this.labelObjectType.Text = "Type:";
            this.labelObjectType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxObjectDescription
            // 
            this.tableLayoutPanelDescription.SetColumnSpan(this.textBoxObjectDescription, 3);
            this.textBoxObjectDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxObjectDescription.Location = new System.Drawing.Point(84, 315);
            this.textBoxObjectDescription.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxObjectDescription.Multiline = true;
            this.textBoxObjectDescription.Name = "textBoxObjectDescription";
            this.tableLayoutPanelDescription.SetRowSpan(this.textBoxObjectDescription, 6);
            this.textBoxObjectDescription.Size = new System.Drawing.Size(572, 201);
            this.textBoxObjectDescription.TabIndex = 0;
            this.textBoxObjectDescription.Leave += new System.EventHandler(this.textBoxObjectDescription_Leave);
            // 
            // textBoxObjectType
            // 
            this.textBoxObjectType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxObjectType.Location = new System.Drawing.Point(84, 29);
            this.textBoxObjectType.Name = "textBoxObjectType";
            this.textBoxObjectType.ReadOnly = true;
            this.textBoxObjectType.Size = new System.Drawing.Size(340, 20);
            this.textBoxObjectType.TabIndex = 4;
            // 
            // labelObjectDefinition
            // 
            this.labelObjectDefinition.AutoSize = true;
            this.labelObjectDefinition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelObjectDefinition.Location = new System.Drawing.Point(3, 58);
            this.labelObjectDefinition.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelObjectDefinition.Name = "labelObjectDefinition";
            this.labelObjectDefinition.Size = new System.Drawing.Size(75, 235);
            this.labelObjectDefinition.TabIndex = 5;
            this.labelObjectDefinition.Text = "Definition:";
            this.labelObjectDefinition.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelObjectDescription
            // 
            this.labelObjectDescription.AutoSize = true;
            this.labelObjectDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelObjectDescription.Location = new System.Drawing.Point(3, 293);
            this.labelObjectDescription.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.labelObjectDescription.Name = "labelObjectDescription";
            this.labelObjectDescription.Size = new System.Drawing.Size(75, 19);
            this.labelObjectDescription.TabIndex = 6;
            this.labelObjectDescription.Text = "Description:";
            this.labelObjectDescription.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // textBoxObjectDefinition
            // 
            this.tableLayoutPanelDescription.SetColumnSpan(this.textBoxObjectDefinition, 3);
            this.textBoxObjectDefinition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxObjectDefinition.Location = new System.Drawing.Point(84, 55);
            this.textBoxObjectDefinition.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxObjectDefinition.Multiline = true;
            this.textBoxObjectDefinition.Name = "textBoxObjectDefinition";
            this.textBoxObjectDefinition.ReadOnly = true;
            this.textBoxObjectDefinition.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxObjectDefinition.Size = new System.Drawing.Size(572, 238);
            this.textBoxObjectDefinition.TabIndex = 7;
            // 
            // labelObjectDatatype
            // 
            this.labelObjectDatatype.AutoSize = true;
            this.labelObjectDatatype.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelObjectDatatype.Location = new System.Drawing.Point(430, 26);
            this.labelObjectDatatype.Name = "labelObjectDatatype";
            this.labelObjectDatatype.Size = new System.Drawing.Size(53, 26);
            this.labelObjectDatatype.TabIndex = 8;
            this.labelObjectDatatype.Text = "Datatype:";
            this.labelObjectDatatype.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxObjectDatatype
            // 
            this.textBoxObjectDatatype.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxObjectDatatype.Location = new System.Drawing.Point(489, 29);
            this.textBoxObjectDatatype.Name = "textBoxObjectDatatype";
            this.textBoxObjectDatatype.ReadOnly = true;
            this.textBoxObjectDatatype.Size = new System.Drawing.Size(167, 20);
            this.textBoxObjectDatatype.TabIndex = 9;
            // 
            // textBoxObjectAction
            // 
            this.textBoxObjectAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxObjectAction.Location = new System.Drawing.Point(489, 3);
            this.textBoxObjectAction.Name = "textBoxObjectAction";
            this.textBoxObjectAction.ReadOnly = true;
            this.textBoxObjectAction.Size = new System.Drawing.Size(167, 20);
            this.textBoxObjectAction.TabIndex = 10;
            // 
            // labelObjectAction
            // 
            this.labelObjectAction.AutoSize = true;
            this.labelObjectAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelObjectAction.Location = new System.Drawing.Point(430, 0);
            this.labelObjectAction.Name = "labelObjectAction";
            this.labelObjectAction.Size = new System.Drawing.Size(53, 26);
            this.labelObjectAction.TabIndex = 11;
            this.labelObjectAction.Text = "Action:";
            this.labelObjectAction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonObjectDescriptionSQLadd
            // 
            this.buttonObjectDescriptionSQLadd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonObjectDescriptionSQLadd.Location = new System.Drawing.Point(3, 416);
            this.buttonObjectDescriptionSQLadd.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.buttonObjectDescriptionSQLadd.Name = "buttonObjectDescriptionSQLadd";
            this.buttonObjectDescriptionSQLadd.Size = new System.Drawing.Size(78, 42);
            this.buttonObjectDescriptionSQLadd.TabIndex = 12;
            this.buttonObjectDescriptionSQLadd.Text = "SQL for adding";
            this.buttonObjectDescriptionSQLadd.UseVisualStyleBackColor = true;
            this.buttonObjectDescriptionSQLadd.Click += new System.EventHandler(this.buttonObjectDescriptionSQLadd_Click);
            // 
            // buttonObjectDescriptionSQLupdate
            // 
            this.buttonObjectDescriptionSQLupdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonObjectDescriptionSQLupdate.Location = new System.Drawing.Point(3, 458);
            this.buttonObjectDescriptionSQLupdate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.buttonObjectDescriptionSQLupdate.Name = "buttonObjectDescriptionSQLupdate";
            this.buttonObjectDescriptionSQLupdate.Size = new System.Drawing.Size(78, 23);
            this.buttonObjectDescriptionSQLupdate.TabIndex = 13;
            this.buttonObjectDescriptionSQLupdate.Text = "update";
            this.buttonObjectDescriptionSQLupdate.UseVisualStyleBackColor = true;
            this.buttonObjectDescriptionSQLupdate.Click += new System.EventHandler(this.buttonObjectDescriptionSQLupdate_Click);
            // 
            // buttonObjectDescriptionSQLany
            // 
            this.buttonObjectDescriptionSQLany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonObjectDescriptionSQLany.Location = new System.Drawing.Point(3, 481);
            this.buttonObjectDescriptionSQLany.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.buttonObjectDescriptionSQLany.Name = "buttonObjectDescriptionSQLany";
            this.buttonObjectDescriptionSQLany.Size = new System.Drawing.Size(78, 35);
            this.buttonObjectDescriptionSQLany.TabIndex = 14;
            this.buttonObjectDescriptionSQLany.Text = "both";
            this.buttonObjectDescriptionSQLany.UseVisualStyleBackColor = true;
            this.buttonObjectDescriptionSQLany.Click += new System.EventHandler(this.buttonObjectDescriptionSQLany_Click);
            // 
            // buttonFillDescriptionCache
            // 
            this.buttonFillDescriptionCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFillDescriptionCache.Enabled = false;
            this.buttonFillDescriptionCache.Image = global::DiversityWorkbench.Properties.Resources.Insert;
            this.buttonFillDescriptionCache.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonFillDescriptionCache.Location = new System.Drawing.Point(3, 315);
            this.buttonFillDescriptionCache.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.buttonFillDescriptionCache.Name = "buttonFillDescriptionCache";
            this.buttonFillDescriptionCache.Size = new System.Drawing.Size(78, 33);
            this.buttonFillDescriptionCache.TabIndex = 15;
            this.buttonFillDescriptionCache.Text = "Fill Cache";
            this.buttonFillDescriptionCache.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonFillDescriptionCache, "Filling the table CacheDescription");
            this.buttonFillDescriptionCache.UseVisualStyleBackColor = true;
            this.buttonFillDescriptionCache.Click += new System.EventHandler(this.buttonFillDescriptionCache_Click);
            // 
            // buttonViewDescriptionCache
            // 
            this.buttonViewDescriptionCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewDescriptionCache.Image = global::DiversityWorkbench.Properties.Resources.Lupe;
            this.buttonViewDescriptionCache.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonViewDescriptionCache.Location = new System.Drawing.Point(3, 348);
            this.buttonViewDescriptionCache.Margin = new System.Windows.Forms.Padding(3, 0, 0, 9);
            this.buttonViewDescriptionCache.Name = "buttonViewDescriptionCache";
            this.buttonViewDescriptionCache.Size = new System.Drawing.Size(78, 36);
            this.buttonViewDescriptionCache.TabIndex = 16;
            this.buttonViewDescriptionCache.Text = "View Cache";
            this.buttonViewDescriptionCache.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip.SetToolTip(this.buttonViewDescriptionCache, "Showing the content of table CacheDescription");
            this.buttonViewDescriptionCache.UseVisualStyleBackColor = true;
            this.buttonViewDescriptionCache.Click += new System.EventHandler(this.buttonViewDescriptionCache_Click);
            // 
            // checkBoxDescriptionAddExistenceCheck
            // 
            this.checkBoxDescriptionAddExistenceCheck.AutoSize = true;
            this.checkBoxDescriptionAddExistenceCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxDescriptionAddExistenceCheck.Location = new System.Drawing.Point(6, 396);
            this.checkBoxDescriptionAddExistenceCheck.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.checkBoxDescriptionAddExistenceCheck.Name = "checkBoxDescriptionAddExistenceCheck";
            this.checkBoxDescriptionAddExistenceCheck.Size = new System.Drawing.Size(72, 17);
            this.checkBoxDescriptionAddExistenceCheck.TabIndex = 17;
            this.checkBoxDescriptionAddExistenceCheck.Text = "If exists";
            this.toolTip.SetToolTip(this.checkBoxDescriptionAddExistenceCheck, "Adding a clause checking the existence of the object");
            this.checkBoxDescriptionAddExistenceCheck.UseVisualStyleBackColor = true;
            // 
            // buttonDescriptionAddDeprecated
            // 
            this.buttonDescriptionAddDeprecated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDescriptionAddDeprecated.FlatAppearance.BorderSize = 0;
            this.buttonDescriptionAddDeprecated.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDescriptionAddDeprecated.Image = global::DiversityWorkbench.Properties.Resources.ArrowDown;
            this.buttonDescriptionAddDeprecated.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonDescriptionAddDeprecated.Location = new System.Drawing.Point(84, 293);
            this.buttonDescriptionAddDeprecated.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonDescriptionAddDeprecated.Name = "buttonDescriptionAddDeprecated";
            this.buttonDescriptionAddDeprecated.Size = new System.Drawing.Size(340, 22);
            this.buttonDescriptionAddDeprecated.TabIndex = 18;
            this.buttonDescriptionAddDeprecated.Text = "      Add deprecated";
            this.buttonDescriptionAddDeprecated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonDescriptionAddDeprecated, "Add deprecated clause to description");
            this.buttonDescriptionAddDeprecated.UseVisualStyleBackColor = true;
            this.buttonDescriptionAddDeprecated.Click += new System.EventHandler(this.buttonDescriptionAddDeprecated_Click);
            // 
            // tabPageLogging
            // 
            this.tabPageLogging.Controls.Add(this.splitContainerLogging);
            this.tabPageLogging.ImageIndex = 1;
            this.tabPageLogging.Location = new System.Drawing.Point(4, 23);
            this.tabPageLogging.Name = "tabPageLogging";
            this.tabPageLogging.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLogging.Size = new System.Drawing.Size(863, 525);
            this.tabPageLogging.TabIndex = 1;
            this.tabPageLogging.Text = "Log table and trigger";
            this.tabPageLogging.UseVisualStyleBackColor = true;
            // 
            // splitContainerLogging
            // 
            this.splitContainerLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLogging.Location = new System.Drawing.Point(3, 3);
            this.splitContainerLogging.Name = "splitContainerLogging";
            // 
            // splitContainerLogging.Panel1
            // 
            this.splitContainerLogging.Panel1.Controls.Add(this.tableLayoutPanelLogging);
            // 
            // splitContainerLogging.Panel2
            // 
            this.splitContainerLogging.Panel2.Controls.Add(this.tabControlLoggingDefinitions);
            this.splitContainerLogging.Size = new System.Drawing.Size(857, 519);
            this.splitContainerLogging.SplitterDistance = 222;
            this.splitContainerLogging.TabIndex = 1;
            // 
            // tableLayoutPanelLogging
            // 
            this.tableLayoutPanelLogging.ColumnCount = 3;
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogging.Controls.Add(this.buttonListTablesForTrigger, 2, 0);
            this.tableLayoutPanelLogging.Controls.Add(this.labelLoggingTableList, 0, 0);
            this.tableLayoutPanelLogging.Controls.Add(this.labelLoggingVersionMaster, 0, 2);
            this.tableLayoutPanelLogging.Controls.Add(this.comboBoxLoggingVersionMasterTable, 1, 2);
            this.tableLayoutPanelLogging.Controls.Add(this.listBoxLoggingTables, 0, 1);
            this.tableLayoutPanelLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLogging.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLogging.Name = "tableLayoutPanelLogging";
            this.tableLayoutPanelLogging.RowCount = 3;
            this.tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLogging.Size = new System.Drawing.Size(222, 519);
            this.tableLayoutPanelLogging.TabIndex = 31;
            // 
            // buttonListTablesForTrigger
            // 
            this.buttonListTablesForTrigger.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonListTablesForTrigger.Location = new System.Drawing.Point(157, 3);
            this.buttonListTablesForTrigger.Name = "buttonListTablesForTrigger";
            this.buttonListTablesForTrigger.Size = new System.Drawing.Size(62, 23);
            this.buttonListTablesForTrigger.TabIndex = 39;
            this.buttonListTablesForTrigger.Text = "List tables";
            this.buttonListTablesForTrigger.UseVisualStyleBackColor = true;
            this.buttonListTablesForTrigger.Click += new System.EventHandler(this.buttonListTablesForTrigger_Click);
            // 
            // labelLoggingTableList
            // 
            this.labelLoggingTableList.AutoSize = true;
            this.tableLayoutPanelLogging.SetColumnSpan(this.labelLoggingTableList, 2);
            this.labelLoggingTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoggingTableList.Location = new System.Drawing.Point(3, 0);
            this.labelLoggingTableList.Name = "labelLoggingTableList";
            this.labelLoggingTableList.Size = new System.Drawing.Size(148, 29);
            this.labelLoggingTableList.TabIndex = 30;
            this.labelLoggingTableList.Text = "Tables in database";
            this.labelLoggingTableList.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelLoggingVersionMaster
            // 
            this.labelLoggingVersionMaster.AutoSize = true;
            this.labelLoggingVersionMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoggingVersionMaster.Location = new System.Drawing.Point(3, 492);
            this.labelLoggingVersionMaster.Name = "labelLoggingVersionMaster";
            this.labelLoggingVersionMaster.Size = new System.Drawing.Size(71, 27);
            this.labelLoggingVersionMaster.TabIndex = 36;
            this.labelLoggingVersionMaster.Text = "Version table:";
            this.labelLoggingVersionMaster.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxLoggingVersionMasterTable
            // 
            this.tableLayoutPanelLogging.SetColumnSpan(this.comboBoxLoggingVersionMasterTable, 2);
            this.comboBoxLoggingVersionMasterTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLoggingVersionMasterTable.FormattingEnabled = true;
            this.comboBoxLoggingVersionMasterTable.Location = new System.Drawing.Point(80, 495);
            this.comboBoxLoggingVersionMasterTable.MaxDropDownItems = 20;
            this.comboBoxLoggingVersionMasterTable.Name = "comboBoxLoggingVersionMasterTable";
            this.comboBoxLoggingVersionMasterTable.Size = new System.Drawing.Size(139, 21);
            this.comboBoxLoggingVersionMasterTable.TabIndex = 37;
            this.comboBoxLoggingVersionMasterTable.DropDown += new System.EventHandler(this.comboBoxLoggingVersionMasterTable_DropDown);
            // 
            // listBoxLoggingTables
            // 
            this.tableLayoutPanelLogging.SetColumnSpan(this.listBoxLoggingTables, 3);
            this.listBoxLoggingTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLoggingTables.FormattingEnabled = true;
            this.listBoxLoggingTables.IntegralHeight = false;
            this.listBoxLoggingTables.Location = new System.Drawing.Point(3, 32);
            this.listBoxLoggingTables.Name = "listBoxLoggingTables";
            this.listBoxLoggingTables.Size = new System.Drawing.Size(216, 457);
            this.listBoxLoggingTables.TabIndex = 38;
            this.listBoxLoggingTables.SelectedIndexChanged += new System.EventHandler(this.listBoxLoggingTables_SelectedIndexChanged);
            // 
            // tabControlLoggingDefinitions
            // 
            this.tabControlLoggingDefinitions.Controls.Add(this.tabPageTable);
            this.tabControlLoggingDefinitions.Controls.Add(this.tabPageLogTable);
            this.tabControlLoggingDefinitions.Controls.Add(this.tabPageInsertTrigger);
            this.tabControlLoggingDefinitions.Controls.Add(this.tabPageUpdateTrigger);
            this.tabControlLoggingDefinitions.Controls.Add(this.tabPageDeleteTrigger);
            this.tabControlLoggingDefinitions.Controls.Add(this.tabPageProcSetVersion);
            this.tabControlLoggingDefinitions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlLoggingDefinitions.Location = new System.Drawing.Point(0, 0);
            this.tabControlLoggingDefinitions.Name = "tabControlLoggingDefinitions";
            this.tabControlLoggingDefinitions.SelectedIndex = 0;
            this.tabControlLoggingDefinitions.Size = new System.Drawing.Size(631, 519);
            this.tabControlLoggingDefinitions.TabIndex = 41;
            // 
            // tabPageTable
            // 
            this.tabPageTable.Controls.Add(this.dataGridViewTable);
            this.tabPageTable.Controls.Add(this.panelTableButtons);
            this.tabPageTable.Location = new System.Drawing.Point(4, 22);
            this.tabPageTable.Name = "tabPageTable";
            this.tabPageTable.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTable.Size = new System.Drawing.Size(623, 493);
            this.tabPageTable.TabIndex = 0;
            this.tabPageTable.Text = "Table";
            this.tabPageTable.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTable
            // 
            this.dataGridViewTable.AllowUserToAddRows = false;
            this.dataGridViewTable.AllowUserToDeleteRows = false;
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle25;
            this.dataGridViewTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnOK});
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTable.DefaultCellStyle = dataGridViewCellStyle26;
            this.dataGridViewTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTable.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewTable.Name = "dataGridViewTable";
            this.dataGridViewTable.ReadOnly = true;
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle27;
            this.dataGridViewTable.Size = new System.Drawing.Size(617, 443);
            this.dataGridViewTable.TabIndex = 15;
            // 
            // ColumnOK
            // 
            this.ColumnOK.FillWeight = 50F;
            this.ColumnOK.HeaderText = "Exclude";
            this.ColumnOK.Name = "ColumnOK";
            this.ColumnOK.ReadOnly = true;
            this.ColumnOK.Width = 50;
            // 
            // panelTableButtons
            // 
            this.panelTableButtons.Controls.Add(this.checkBoxAddRowGUID);
            this.panelTableButtons.Controls.Add(this.checkBoxAddInsertColumns);
            this.panelTableButtons.Controls.Add(this.buttonAttachLogColumns);
            this.panelTableButtons.Controls.Add(this.checkBoxLogcolumnsDSGVO);
            this.panelTableButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTableButtons.Location = new System.Drawing.Point(3, 446);
            this.panelTableButtons.Name = "panelTableButtons";
            this.panelTableButtons.Size = new System.Drawing.Size(617, 44);
            this.panelTableButtons.TabIndex = 16;
            // 
            // checkBoxAddRowGUID
            // 
            this.checkBoxAddRowGUID.AutoSize = true;
            this.checkBoxAddRowGUID.Location = new System.Drawing.Point(489, 18);
            this.checkBoxAddRowGUID.Name = "checkBoxAddRowGUID";
            this.checkBoxAddRowGUID.Size = new System.Drawing.Size(97, 17);
            this.checkBoxAddRowGUID.TabIndex = 44;
            this.checkBoxAddRowGUID.Text = "Add RowGUID";
            this.checkBoxAddRowGUID.UseVisualStyleBackColor = true;
            // 
            // checkBoxAddInsertColumns
            // 
            this.checkBoxAddInsertColumns.AutoSize = true;
            this.checkBoxAddInsertColumns.Checked = true;
            this.checkBoxAddInsertColumns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAddInsertColumns.Location = new System.Drawing.Point(172, 18);
            this.checkBoxAddInsertColumns.Name = "checkBoxAddInsertColumns";
            this.checkBoxAddInsertColumns.Size = new System.Drawing.Size(166, 17);
            this.checkBoxAddInsertColumns.TabIndex = 43;
            this.checkBoxAddInsertColumns.Text = "Add LogInsert -When and -By";
            this.checkBoxAddInsertColumns.UseVisualStyleBackColor = true;
            // 
            // buttonAttachLogColumns
            // 
            this.buttonAttachLogColumns.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonAttachLogColumns.Location = new System.Drawing.Point(0, 0);
            this.buttonAttachLogColumns.Name = "buttonAttachLogColumns";
            this.buttonAttachLogColumns.Size = new System.Drawing.Size(166, 44);
            this.buttonAttachLogColumns.TabIndex = 42;
            this.buttonAttachLogColumns.Text = "Attach the columns LogUpdated -When and -By";
            this.buttonAttachLogColumns.UseVisualStyleBackColor = true;
            this.buttonAttachLogColumns.Click += new System.EventHandler(this.buttonAttachLogColumns_Click);
            // 
            // checkBoxLogcolumnsDSGVO
            // 
            this.checkBoxLogcolumnsDSGVO.Checked = true;
            this.checkBoxLogcolumnsDSGVO.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogcolumnsDSGVO.Image = global::DiversityWorkbench.Properties.Resources.Paragraf;
            this.checkBoxLogcolumnsDSGVO.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxLogcolumnsDSGVO.Location = new System.Drawing.Point(355, 13);
            this.checkBoxLogcolumnsDSGVO.Name = "checkBoxLogcolumnsDSGVO";
            this.checkBoxLogcolumnsDSGVO.Size = new System.Drawing.Size(139, 26);
            this.checkBoxLogcolumnsDSGVO.TabIndex = 45;
            this.checkBoxLogcolumnsDSGVO.Text = "     Include DSGVO";
            this.checkBoxLogcolumnsDSGVO.UseVisualStyleBackColor = true;
            // 
            // tabPageLogTable
            // 
            this.tabPageLogTable.Controls.Add(this.textBoxLogTable);
            this.tabPageLogTable.Controls.Add(this.panelLogTableButtons);
            this.tabPageLogTable.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogTable.Name = "tabPageLogTable";
            this.tabPageLogTable.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLogTable.Size = new System.Drawing.Size(623, 493);
            this.tabPageLogTable.TabIndex = 4;
            this.tabPageLogTable.Text = "Log table";
            this.tabPageLogTable.UseVisualStyleBackColor = true;
            // 
            // textBoxLogTable
            // 
            this.textBoxLogTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogTable.Location = new System.Drawing.Point(3, 3);
            this.textBoxLogTable.Multiline = true;
            this.textBoxLogTable.Name = "textBoxLogTable";
            this.textBoxLogTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLogTable.Size = new System.Drawing.Size(617, 435);
            this.textBoxLogTable.TabIndex = 0;
            // 
            // panelLogTableButtons
            // 
            this.panelLogTableButtons.Controls.Add(this.tableLayoutPanelLogTable);
            this.panelLogTableButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLogTableButtons.Location = new System.Drawing.Point(3, 438);
            this.panelLogTableButtons.Name = "panelLogTableButtons";
            this.panelLogTableButtons.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.panelLogTableButtons.Size = new System.Drawing.Size(617, 52);
            this.panelLogTableButtons.TabIndex = 1;
            // 
            // checkBoxLogTableDSGVO
            // 
            this.checkBoxLogTableDSGVO.Checked = true;
            this.checkBoxLogTableDSGVO.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelLogTable.SetColumnSpan(this.checkBoxLogTableDSGVO, 2);
            this.checkBoxLogTableDSGVO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxLogTableDSGVO.Image = global::DiversityWorkbench.Properties.Resources.Paragraf;
            this.checkBoxLogTableDSGVO.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxLogTableDSGVO.Location = new System.Drawing.Point(144, 0);
            this.checkBoxLogTableDSGVO.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxLogTableDSGVO.Name = "checkBoxLogTableDSGVO";
            this.checkBoxLogTableDSGVO.Size = new System.Drawing.Size(312, 26);
            this.checkBoxLogTableDSGVO.TabIndex = 37;
            this.checkBoxLogTableDSGVO.Text = "      User according to DSGVO";
            this.checkBoxLogTableDSGVO.UseVisualStyleBackColor = true;
            // 
            // checkBoxAddVersion
            // 
            this.checkBoxAddVersion.AutoSize = true;
            this.checkBoxAddVersion.Checked = true;
            this.checkBoxAddVersion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAddVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxAddVersion.Location = new System.Drawing.Point(144, 26);
            this.checkBoxAddVersion.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxAddVersion.Name = "checkBoxAddVersion";
            this.checkBoxAddVersion.Size = new System.Drawing.Size(201, 22);
            this.checkBoxAddVersion.TabIndex = 36;
            this.checkBoxAddVersion.Text = "Add the column LogVersion";
            this.checkBoxAddVersion.UseVisualStyleBackColor = true;
            // 
            // checkBoxKeepOldLogTable
            // 
            this.checkBoxKeepOldLogTable.AutoSize = true;
            this.checkBoxKeepOldLogTable.Checked = true;
            this.checkBoxKeepOldLogTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxKeepOldLogTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxKeepOldLogTable.Location = new System.Drawing.Point(345, 26);
            this.checkBoxKeepOldLogTable.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxKeepOldLogTable.Name = "checkBoxKeepOldLogTable";
            this.checkBoxKeepOldLogTable.Size = new System.Drawing.Size(111, 22);
            this.checkBoxKeepOldLogTable.TabIndex = 35;
            this.checkBoxKeepOldLogTable.Text = "Keep old log table";
            this.checkBoxKeepOldLogTable.UseVisualStyleBackColor = true;
            // 
            // buttonLogTableCreate
            // 
            this.buttonLogTableCreate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLogTableCreate.Location = new System.Drawing.Point(459, 3);
            this.buttonLogTableCreate.Name = "buttonLogTableCreate";
            this.tableLayoutPanelLogTable.SetRowSpan(this.buttonLogTableCreate, 2);
            this.buttonLogTableCreate.Size = new System.Drawing.Size(155, 42);
            this.buttonLogTableCreate.TabIndex = 1;
            this.buttonLogTableCreate.Text = "Create LogTable as defined in the SQL-statement above";
            this.buttonLogTableCreate.UseVisualStyleBackColor = true;
            this.buttonLogTableCreate.Click += new System.EventHandler(this.buttonLogTableCreate_Click);
            // 
            // buttonLogTableShowSQL
            // 
            this.buttonLogTableShowSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLogTableShowSQL.Location = new System.Drawing.Point(3, 3);
            this.buttonLogTableShowSQL.Name = "buttonLogTableShowSQL";
            this.tableLayoutPanelLogTable.SetRowSpan(this.buttonLogTableShowSQL, 2);
            this.buttonLogTableShowSQL.Size = new System.Drawing.Size(138, 42);
            this.buttonLogTableShowSQL.TabIndex = 0;
            this.buttonLogTableShowSQL.Text = "Show SQL for the creation of the log table";
            this.buttonLogTableShowSQL.UseVisualStyleBackColor = true;
            this.buttonLogTableShowSQL.Click += new System.EventHandler(this.buttonLogTableShowSQL_Click);
            // 
            // tabPageInsertTrigger
            // 
            this.tabPageInsertTrigger.Controls.Add(this.textBoxInsertTriggerNew);
            this.tabPageInsertTrigger.Controls.Add(this.panelInsertTriggerButtons);
            this.tabPageInsertTrigger.Controls.Add(this.textBoxInsertTrigger);
            this.tabPageInsertTrigger.Location = new System.Drawing.Point(4, 22);
            this.tabPageInsertTrigger.Name = "tabPageInsertTrigger";
            this.tabPageInsertTrigger.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInsertTrigger.Size = new System.Drawing.Size(623, 493);
            this.tabPageInsertTrigger.TabIndex = 5;
            this.tabPageInsertTrigger.Text = "Insert Trigger";
            this.tabPageInsertTrigger.UseVisualStyleBackColor = true;
            // 
            // textBoxInsertTriggerNew
            // 
            this.textBoxInsertTriggerNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInsertTriggerNew.Location = new System.Drawing.Point(3, 306);
            this.textBoxInsertTriggerNew.Multiline = true;
            this.textBoxInsertTriggerNew.Name = "textBoxInsertTriggerNew";
            this.textBoxInsertTriggerNew.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInsertTriggerNew.Size = new System.Drawing.Size(617, 151);
            this.textBoxInsertTriggerNew.TabIndex = 23;
            // 
            // panelInsertTriggerButtons
            // 
            this.panelInsertTriggerButtons.Controls.Add(this.checkBoxInsertTriggerAddVersion);
            this.panelInsertTriggerButtons.Controls.Add(this.buttonInsertTriggerCreate);
            this.panelInsertTriggerButtons.Controls.Add(this.buttonInsertTriggerShowSQL);
            this.panelInsertTriggerButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInsertTriggerButtons.Location = new System.Drawing.Point(3, 457);
            this.panelInsertTriggerButtons.Name = "panelInsertTriggerButtons";
            this.panelInsertTriggerButtons.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.panelInsertTriggerButtons.Size = new System.Drawing.Size(617, 33);
            this.panelInsertTriggerButtons.TabIndex = 22;
            // 
            // checkBoxInsertTriggerAddVersion
            // 
            this.checkBoxInsertTriggerAddVersion.AutoSize = true;
            this.checkBoxInsertTriggerAddVersion.Checked = true;
            this.checkBoxInsertTriggerAddVersion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxInsertTriggerAddVersion.Location = new System.Drawing.Point(260, 8);
            this.checkBoxInsertTriggerAddVersion.Name = "checkBoxInsertTriggerAddVersion";
            this.checkBoxInsertTriggerAddVersion.Size = new System.Drawing.Size(160, 17);
            this.checkBoxInsertTriggerAddVersion.TabIndex = 34;
            this.checkBoxInsertTriggerAddVersion.Text = "Add version setting to trigger";
            this.checkBoxInsertTriggerAddVersion.UseVisualStyleBackColor = true;
            // 
            // buttonInsertTriggerCreate
            // 
            this.buttonInsertTriggerCreate.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonInsertTriggerCreate.Location = new System.Drawing.Point(525, 4);
            this.buttonInsertTriggerCreate.Name = "buttonInsertTriggerCreate";
            this.buttonInsertTriggerCreate.Size = new System.Drawing.Size(92, 29);
            this.buttonInsertTriggerCreate.TabIndex = 1;
            this.buttonInsertTriggerCreate.Text = "Create Trigger";
            this.buttonInsertTriggerCreate.UseVisualStyleBackColor = true;
            this.buttonInsertTriggerCreate.Click += new System.EventHandler(this.buttonInsertTriggerCreate_Click);
            // 
            // buttonInsertTriggerShowSQL
            // 
            this.buttonInsertTriggerShowSQL.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonInsertTriggerShowSQL.Location = new System.Drawing.Point(0, 4);
            this.buttonInsertTriggerShowSQL.Name = "buttonInsertTriggerShowSQL";
            this.buttonInsertTriggerShowSQL.Size = new System.Drawing.Size(75, 29);
            this.buttonInsertTriggerShowSQL.TabIndex = 0;
            this.buttonInsertTriggerShowSQL.Text = "Show SQL";
            this.buttonInsertTriggerShowSQL.UseVisualStyleBackColor = true;
            this.buttonInsertTriggerShowSQL.Click += new System.EventHandler(this.buttonInsertTriggerShowSQL_Click);
            // 
            // textBoxInsertTrigger
            // 
            this.textBoxInsertTrigger.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxInsertTrigger.Location = new System.Drawing.Point(3, 3);
            this.textBoxInsertTrigger.Multiline = true;
            this.textBoxInsertTrigger.Name = "textBoxInsertTrigger";
            this.textBoxInsertTrigger.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInsertTrigger.Size = new System.Drawing.Size(617, 303);
            this.textBoxInsertTrigger.TabIndex = 21;
            // 
            // tabPageUpdateTrigger
            // 
            this.tabPageUpdateTrigger.Controls.Add(this.textBoxUpdateTriggerNew);
            this.tabPageUpdateTrigger.Controls.Add(this.panelUpdateTriggerButtons);
            this.tabPageUpdateTrigger.Controls.Add(this.textBoxUpdateTrigger);
            this.tabPageUpdateTrigger.Location = new System.Drawing.Point(4, 22);
            this.tabPageUpdateTrigger.Name = "tabPageUpdateTrigger";
            this.tabPageUpdateTrigger.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpdateTrigger.Size = new System.Drawing.Size(623, 493);
            this.tabPageUpdateTrigger.TabIndex = 1;
            this.tabPageUpdateTrigger.Text = "Update trigger";
            this.tabPageUpdateTrigger.UseVisualStyleBackColor = true;
            // 
            // textBoxUpdateTriggerNew
            // 
            this.textBoxUpdateTriggerNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUpdateTriggerNew.Location = new System.Drawing.Point(3, 306);
            this.textBoxUpdateTriggerNew.Multiline = true;
            this.textBoxUpdateTriggerNew.Name = "textBoxUpdateTriggerNew";
            this.textBoxUpdateTriggerNew.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxUpdateTriggerNew.Size = new System.Drawing.Size(617, 151);
            this.textBoxUpdateTriggerNew.TabIndex = 20;
            // 
            // panelUpdateTriggerButtons
            // 
            this.panelUpdateTriggerButtons.Controls.Add(this.checkBoxUpdateTriggerAddDsgvo);
            this.panelUpdateTriggerButtons.Controls.Add(this.checkBoxUpdateTriggerAddVersion);
            this.panelUpdateTriggerButtons.Controls.Add(this.buttonUpdateTriggerCreate);
            this.panelUpdateTriggerButtons.Controls.Add(this.buttonUpdateTriggerShowSQL);
            this.panelUpdateTriggerButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelUpdateTriggerButtons.Location = new System.Drawing.Point(3, 457);
            this.panelUpdateTriggerButtons.Name = "panelUpdateTriggerButtons";
            this.panelUpdateTriggerButtons.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.panelUpdateTriggerButtons.Size = new System.Drawing.Size(617, 33);
            this.panelUpdateTriggerButtons.TabIndex = 19;
            // 
            // checkBoxUpdateTriggerAddDsgvo
            // 
            this.checkBoxUpdateTriggerAddDsgvo.Checked = true;
            this.checkBoxUpdateTriggerAddDsgvo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUpdateTriggerAddDsgvo.Image = global::DiversityWorkbench.Properties.Resources.Paragraf;
            this.checkBoxUpdateTriggerAddDsgvo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxUpdateTriggerAddDsgvo.Location = new System.Drawing.Point(106, 5);
            this.checkBoxUpdateTriggerAddDsgvo.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxUpdateTriggerAddDsgvo.Name = "checkBoxUpdateTriggerAddDsgvo";
            this.checkBoxUpdateTriggerAddDsgvo.Size = new System.Drawing.Size(102, 28);
            this.checkBoxUpdateTriggerAddDsgvo.TabIndex = 35;
            this.checkBoxUpdateTriggerAddDsgvo.Text = "Add DSGVO";
            this.checkBoxUpdateTriggerAddDsgvo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxUpdateTriggerAddDsgvo.UseVisualStyleBackColor = true;
            this.checkBoxUpdateTriggerAddDsgvo.Visible = false;
            // 
            // checkBoxUpdateTriggerAddVersion
            // 
            this.checkBoxUpdateTriggerAddVersion.AutoSize = true;
            this.checkBoxUpdateTriggerAddVersion.Checked = true;
            this.checkBoxUpdateTriggerAddVersion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUpdateTriggerAddVersion.Location = new System.Drawing.Point(235, 11);
            this.checkBoxUpdateTriggerAddVersion.Name = "checkBoxUpdateTriggerAddVersion";
            this.checkBoxUpdateTriggerAddVersion.Size = new System.Drawing.Size(160, 17);
            this.checkBoxUpdateTriggerAddVersion.TabIndex = 34;
            this.checkBoxUpdateTriggerAddVersion.Text = "Add version setting to trigger";
            this.checkBoxUpdateTriggerAddVersion.UseVisualStyleBackColor = true;
            // 
            // buttonUpdateTriggerCreate
            // 
            this.buttonUpdateTriggerCreate.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonUpdateTriggerCreate.Location = new System.Drawing.Point(525, 4);
            this.buttonUpdateTriggerCreate.Name = "buttonUpdateTriggerCreate";
            this.buttonUpdateTriggerCreate.Size = new System.Drawing.Size(92, 29);
            this.buttonUpdateTriggerCreate.TabIndex = 1;
            this.buttonUpdateTriggerCreate.Text = "Create Trigger";
            this.buttonUpdateTriggerCreate.UseVisualStyleBackColor = true;
            this.buttonUpdateTriggerCreate.Click += new System.EventHandler(this.buttonUpdateTriggerCreate_Click);
            // 
            // buttonUpdateTriggerShowSQL
            // 
            this.buttonUpdateTriggerShowSQL.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonUpdateTriggerShowSQL.Location = new System.Drawing.Point(0, 4);
            this.buttonUpdateTriggerShowSQL.Name = "buttonUpdateTriggerShowSQL";
            this.buttonUpdateTriggerShowSQL.Size = new System.Drawing.Size(75, 29);
            this.buttonUpdateTriggerShowSQL.TabIndex = 0;
            this.buttonUpdateTriggerShowSQL.Text = "Show SQL";
            this.buttonUpdateTriggerShowSQL.UseVisualStyleBackColor = true;
            this.buttonUpdateTriggerShowSQL.Click += new System.EventHandler(this.buttonUpdateTriggerShowSQL_Click);
            // 
            // textBoxUpdateTrigger
            // 
            this.textBoxUpdateTrigger.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxUpdateTrigger.Location = new System.Drawing.Point(3, 3);
            this.textBoxUpdateTrigger.Multiline = true;
            this.textBoxUpdateTrigger.Name = "textBoxUpdateTrigger";
            this.textBoxUpdateTrigger.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxUpdateTrigger.Size = new System.Drawing.Size(617, 303);
            this.textBoxUpdateTrigger.TabIndex = 18;
            // 
            // tabPageDeleteTrigger
            // 
            this.tabPageDeleteTrigger.Controls.Add(this.textBoxDeleteTriggerNew);
            this.tabPageDeleteTrigger.Controls.Add(this.panelDeleteTriggerButtons);
            this.tabPageDeleteTrigger.Controls.Add(this.textBoxDeleteTrigger);
            this.tabPageDeleteTrigger.Location = new System.Drawing.Point(4, 22);
            this.tabPageDeleteTrigger.Name = "tabPageDeleteTrigger";
            this.tabPageDeleteTrigger.Size = new System.Drawing.Size(623, 493);
            this.tabPageDeleteTrigger.TabIndex = 3;
            this.tabPageDeleteTrigger.Text = "Delete trigger";
            this.tabPageDeleteTrigger.UseVisualStyleBackColor = true;
            // 
            // textBoxDeleteTriggerNew
            // 
            this.textBoxDeleteTriggerNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDeleteTriggerNew.Location = new System.Drawing.Point(0, 304);
            this.textBoxDeleteTriggerNew.Multiline = true;
            this.textBoxDeleteTriggerNew.Name = "textBoxDeleteTriggerNew";
            this.textBoxDeleteTriggerNew.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDeleteTriggerNew.Size = new System.Drawing.Size(623, 157);
            this.textBoxDeleteTriggerNew.TabIndex = 20;
            // 
            // panelDeleteTriggerButtons
            // 
            this.panelDeleteTriggerButtons.Controls.Add(this.checkBoxDeleteTriggerAddDsgvo);
            this.panelDeleteTriggerButtons.Controls.Add(this.checkBoxDeleteTriggerAddVersion);
            this.panelDeleteTriggerButtons.Controls.Add(this.buttonDeleteTriggerCreate);
            this.panelDeleteTriggerButtons.Controls.Add(this.buttonDeleteTriggerShowSql);
            this.panelDeleteTriggerButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDeleteTriggerButtons.Location = new System.Drawing.Point(0, 461);
            this.panelDeleteTriggerButtons.Name = "panelDeleteTriggerButtons";
            this.panelDeleteTriggerButtons.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.panelDeleteTriggerButtons.Size = new System.Drawing.Size(623, 32);
            this.panelDeleteTriggerButtons.TabIndex = 19;
            // 
            // checkBoxDeleteTriggerAddDsgvo
            // 
            this.checkBoxDeleteTriggerAddDsgvo.Checked = true;
            this.checkBoxDeleteTriggerAddDsgvo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteTriggerAddDsgvo.Image = global::DiversityWorkbench.Properties.Resources.Paragraf;
            this.checkBoxDeleteTriggerAddDsgvo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxDeleteTriggerAddDsgvo.Location = new System.Drawing.Point(115, 2);
            this.checkBoxDeleteTriggerAddDsgvo.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxDeleteTriggerAddDsgvo.Name = "checkBoxDeleteTriggerAddDsgvo";
            this.checkBoxDeleteTriggerAddDsgvo.Size = new System.Drawing.Size(102, 28);
            this.checkBoxDeleteTriggerAddDsgvo.TabIndex = 36;
            this.checkBoxDeleteTriggerAddDsgvo.Text = "Add DSGVO";
            this.checkBoxDeleteTriggerAddDsgvo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxDeleteTriggerAddDsgvo.UseVisualStyleBackColor = true;
            this.checkBoxDeleteTriggerAddDsgvo.Visible = false;
            // 
            // checkBoxDeleteTriggerAddVersion
            // 
            this.checkBoxDeleteTriggerAddVersion.AutoSize = true;
            this.checkBoxDeleteTriggerAddVersion.Checked = true;
            this.checkBoxDeleteTriggerAddVersion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteTriggerAddVersion.Location = new System.Drawing.Point(263, 8);
            this.checkBoxDeleteTriggerAddVersion.Name = "checkBoxDeleteTriggerAddVersion";
            this.checkBoxDeleteTriggerAddVersion.Size = new System.Drawing.Size(160, 17);
            this.checkBoxDeleteTriggerAddVersion.TabIndex = 34;
            this.checkBoxDeleteTriggerAddVersion.Text = "Add version setting to trigger";
            this.checkBoxDeleteTriggerAddVersion.UseVisualStyleBackColor = true;
            // 
            // buttonDeleteTriggerCreate
            // 
            this.buttonDeleteTriggerCreate.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonDeleteTriggerCreate.Location = new System.Drawing.Point(521, 4);
            this.buttonDeleteTriggerCreate.Name = "buttonDeleteTriggerCreate";
            this.buttonDeleteTriggerCreate.Size = new System.Drawing.Size(102, 28);
            this.buttonDeleteTriggerCreate.TabIndex = 1;
            this.buttonDeleteTriggerCreate.Text = "Create Trigger";
            this.buttonDeleteTriggerCreate.UseVisualStyleBackColor = true;
            this.buttonDeleteTriggerCreate.Click += new System.EventHandler(this.buttonDeleteTriggerCreate_Click);
            // 
            // buttonDeleteTriggerShowSql
            // 
            this.buttonDeleteTriggerShowSql.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonDeleteTriggerShowSql.Location = new System.Drawing.Point(0, 4);
            this.buttonDeleteTriggerShowSql.Name = "buttonDeleteTriggerShowSql";
            this.buttonDeleteTriggerShowSql.Size = new System.Drawing.Size(75, 28);
            this.buttonDeleteTriggerShowSql.TabIndex = 0;
            this.buttonDeleteTriggerShowSql.Text = "Show SQL";
            this.buttonDeleteTriggerShowSql.UseVisualStyleBackColor = true;
            this.buttonDeleteTriggerShowSql.Click += new System.EventHandler(this.buttonDeleteTriggerShowSql_Click);
            // 
            // textBoxDeleteTrigger
            // 
            this.textBoxDeleteTrigger.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxDeleteTrigger.Location = new System.Drawing.Point(0, 0);
            this.textBoxDeleteTrigger.Multiline = true;
            this.textBoxDeleteTrigger.Name = "textBoxDeleteTrigger";
            this.textBoxDeleteTrigger.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDeleteTrigger.Size = new System.Drawing.Size(623, 304);
            this.textBoxDeleteTrigger.TabIndex = 18;
            // 
            // tabPageProcSetVersion
            // 
            this.tabPageProcSetVersion.Controls.Add(this.splitContainerProcVersion);
            this.tabPageProcSetVersion.Controls.Add(this.panelProcSetVersionButtons);
            this.tabPageProcSetVersion.Location = new System.Drawing.Point(4, 22);
            this.tabPageProcSetVersion.Name = "tabPageProcSetVersion";
            this.tabPageProcSetVersion.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProcSetVersion.Size = new System.Drawing.Size(623, 493);
            this.tabPageProcSetVersion.TabIndex = 2;
            this.tabPageProcSetVersion.Text = "Proc. for setting the version";
            this.tabPageProcSetVersion.UseVisualStyleBackColor = true;
            // 
            // splitContainerProcVersion
            // 
            this.splitContainerProcVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerProcVersion.Location = new System.Drawing.Point(3, 3);
            this.splitContainerProcVersion.Name = "splitContainerProcVersion";
            this.splitContainerProcVersion.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerProcVersion.Panel1
            // 
            this.splitContainerProcVersion.Panel1.Controls.Add(this.textBoxProcSetVersion);
            // 
            // splitContainerProcVersion.Panel2
            // 
            this.splitContainerProcVersion.Panel2.Controls.Add(this.textBoxProcSetVersionNew);
            this.splitContainerProcVersion.Size = new System.Drawing.Size(617, 462);
            this.splitContainerProcVersion.SplitterDistance = 212;
            this.splitContainerProcVersion.TabIndex = 3;
            // 
            // textBoxProcSetVersion
            // 
            this.textBoxProcSetVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcSetVersion.Location = new System.Drawing.Point(0, 0);
            this.textBoxProcSetVersion.Multiline = true;
            this.textBoxProcSetVersion.Name = "textBoxProcSetVersion";
            this.textBoxProcSetVersion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxProcSetVersion.Size = new System.Drawing.Size(617, 212);
            this.textBoxProcSetVersion.TabIndex = 0;
            // 
            // textBoxProcSetVersionNew
            // 
            this.textBoxProcSetVersionNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcSetVersionNew.Location = new System.Drawing.Point(0, 0);
            this.textBoxProcSetVersionNew.Multiline = true;
            this.textBoxProcSetVersionNew.Name = "textBoxProcSetVersionNew";
            this.textBoxProcSetVersionNew.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxProcSetVersionNew.Size = new System.Drawing.Size(617, 246);
            this.textBoxProcSetVersionNew.TabIndex = 2;
            // 
            // panelProcSetVersionButtons
            // 
            this.panelProcSetVersionButtons.Controls.Add(this.checkBoxProcSetVersionDsgvo);
            this.panelProcSetVersionButtons.Controls.Add(this.buttonProcSetVersionCreate);
            this.panelProcSetVersionButtons.Controls.Add(this.buttonProcSetVersionShowSql);
            this.panelProcSetVersionButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelProcSetVersionButtons.Location = new System.Drawing.Point(3, 465);
            this.panelProcSetVersionButtons.Name = "panelProcSetVersionButtons";
            this.panelProcSetVersionButtons.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.panelProcSetVersionButtons.Size = new System.Drawing.Size(617, 25);
            this.panelProcSetVersionButtons.TabIndex = 1;
            // 
            // checkBoxProcSetVersionDsgvo
            // 
            this.checkBoxProcSetVersionDsgvo.Image = global::DiversityWorkbench.Properties.Resources.Paragraf;
            this.checkBoxProcSetVersionDsgvo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxProcSetVersionDsgvo.Location = new System.Drawing.Point(244, -2);
            this.checkBoxProcSetVersionDsgvo.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxProcSetVersionDsgvo.Name = "checkBoxProcSetVersionDsgvo";
            this.checkBoxProcSetVersionDsgvo.Size = new System.Drawing.Size(102, 28);
            this.checkBoxProcSetVersionDsgvo.TabIndex = 36;
            this.checkBoxProcSetVersionDsgvo.Text = "Add DSGVO";
            this.checkBoxProcSetVersionDsgvo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxProcSetVersionDsgvo.UseVisualStyleBackColor = true;
            this.checkBoxProcSetVersionDsgvo.Visible = false;
            // 
            // buttonProcSetVersionCreate
            // 
            this.buttonProcSetVersionCreate.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonProcSetVersionCreate.Location = new System.Drawing.Point(503, 4);
            this.buttonProcSetVersionCreate.Name = "buttonProcSetVersionCreate";
            this.buttonProcSetVersionCreate.Size = new System.Drawing.Size(114, 21);
            this.buttonProcSetVersionCreate.TabIndex = 1;
            this.buttonProcSetVersionCreate.Text = "Create Procedure";
            this.buttonProcSetVersionCreate.UseVisualStyleBackColor = true;
            this.buttonProcSetVersionCreate.Click += new System.EventHandler(this.buttonProcSetVersionCreate_Click);
            // 
            // buttonProcSetVersionShowSql
            // 
            this.buttonProcSetVersionShowSql.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonProcSetVersionShowSql.Location = new System.Drawing.Point(0, 4);
            this.buttonProcSetVersionShowSql.Name = "buttonProcSetVersionShowSql";
            this.buttonProcSetVersionShowSql.Size = new System.Drawing.Size(75, 21);
            this.buttonProcSetVersionShowSql.TabIndex = 0;
            this.buttonProcSetVersionShowSql.Text = "Show SQL";
            this.buttonProcSetVersionShowSql.UseVisualStyleBackColor = true;
            this.buttonProcSetVersionShowSql.Click += new System.EventHandler(this.buttonProcSetVersionShowSql_Click);
            // 
            // tabPageClearLog
            // 
            this.tabPageClearLog.Controls.Add(this.splitContainerClearLog);
            this.tabPageClearLog.ImageIndex = 2;
            this.tabPageClearLog.Location = new System.Drawing.Point(4, 23);
            this.tabPageClearLog.Name = "tabPageClearLog";
            this.tabPageClearLog.Size = new System.Drawing.Size(863, 525);
            this.tabPageClearLog.TabIndex = 4;
            this.tabPageClearLog.Text = "Clear log";
            this.tabPageClearLog.UseVisualStyleBackColor = true;
            // 
            // splitContainerClearLog
            // 
            this.splitContainerClearLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerClearLog.Location = new System.Drawing.Point(0, 0);
            this.splitContainerClearLog.Name = "splitContainerClearLog";
            // 
            // splitContainerClearLog.Panel1
            // 
            this.splitContainerClearLog.Panel1.Controls.Add(this.tableLayoutPanelClearLog);
            // 
            // splitContainerClearLog.Panel2
            // 
            this.splitContainerClearLog.Panel2.Controls.Add(this.dataGridViewClearLog);
            this.splitContainerClearLog.Size = new System.Drawing.Size(863, 525);
            this.splitContainerClearLog.SplitterDistance = 287;
            this.splitContainerClearLog.TabIndex = 0;
            // 
            // tableLayoutPanelClearLog
            // 
            this.tableLayoutPanelClearLog.ColumnCount = 2;
            this.tableLayoutPanelClearLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelClearLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelClearLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelClearLog.Controls.Add(this.buttonClearLogListTables, 1, 0);
            this.tableLayoutPanelClearLog.Controls.Add(this.labelClearLogTableList, 0, 0);
            this.tableLayoutPanelClearLog.Controls.Add(this.checkedListBoxClearLog, 0, 1);
            this.tableLayoutPanelClearLog.Controls.Add(this.buttonClearLogSelectAll, 0, 2);
            this.tableLayoutPanelClearLog.Controls.Add(this.buttonClearLogSelectNone, 1, 2);
            this.tableLayoutPanelClearLog.Controls.Add(this.buttonClearLogStart, 0, 3);
            this.tableLayoutPanelClearLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelClearLog.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelClearLog.Name = "tableLayoutPanelClearLog";
            this.tableLayoutPanelClearLog.RowCount = 4;
            this.tableLayoutPanelClearLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelClearLog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelClearLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelClearLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelClearLog.Size = new System.Drawing.Size(287, 525);
            this.tableLayoutPanelClearLog.TabIndex = 32;
            // 
            // buttonClearLogListTables
            // 
            this.buttonClearLogListTables.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClearLogListTables.Location = new System.Drawing.Point(222, 3);
            this.buttonClearLogListTables.Name = "buttonClearLogListTables";
            this.buttonClearLogListTables.Size = new System.Drawing.Size(62, 23);
            this.buttonClearLogListTables.TabIndex = 39;
            this.buttonClearLogListTables.Text = "List tables";
            this.buttonClearLogListTables.UseVisualStyleBackColor = true;
            this.buttonClearLogListTables.Click += new System.EventHandler(this.buttonClearLogListTables_Click);
            // 
            // labelClearLogTableList
            // 
            this.labelClearLogTableList.AutoSize = true;
            this.labelClearLogTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelClearLogTableList.Location = new System.Drawing.Point(3, 0);
            this.labelClearLogTableList.Name = "labelClearLogTableList";
            this.labelClearLogTableList.Size = new System.Drawing.Size(137, 29);
            this.labelClearLogTableList.TabIndex = 30;
            this.labelClearLogTableList.Text = "Tables in database";
            this.labelClearLogTableList.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // checkedListBoxClearLog
            // 
            this.checkedListBoxClearLog.CheckOnClick = true;
            this.tableLayoutPanelClearLog.SetColumnSpan(this.checkedListBoxClearLog, 2);
            this.checkedListBoxClearLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxClearLog.FormattingEnabled = true;
            this.checkedListBoxClearLog.Location = new System.Drawing.Point(3, 32);
            this.checkedListBoxClearLog.Name = "checkedListBoxClearLog";
            this.checkedListBoxClearLog.Size = new System.Drawing.Size(281, 432);
            this.checkedListBoxClearLog.TabIndex = 40;
            this.checkedListBoxClearLog.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxClearLog_SelectedIndexChanged);
            // 
            // buttonClearLogSelectAll
            // 
            this.buttonClearLogSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClearLogSelectAll.Image = global::DiversityWorkbench.Properties.Resources.CheckYes;
            this.buttonClearLogSelectAll.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonClearLogSelectAll.Location = new System.Drawing.Point(3, 470);
            this.buttonClearLogSelectAll.Name = "buttonClearLogSelectAll";
            this.buttonClearLogSelectAll.Size = new System.Drawing.Size(137, 23);
            this.buttonClearLogSelectAll.TabIndex = 41;
            this.buttonClearLogSelectAll.Text = "Select all";
            this.buttonClearLogSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonClearLogSelectAll.UseVisualStyleBackColor = true;
            this.buttonClearLogSelectAll.Click += new System.EventHandler(this.buttonClearLogSelectAll_Click);
            // 
            // buttonClearLogSelectNone
            // 
            this.buttonClearLogSelectNone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClearLogSelectNone.Image = global::DiversityWorkbench.Properties.Resources.CheckNo;
            this.buttonClearLogSelectNone.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonClearLogSelectNone.Location = new System.Drawing.Point(146, 470);
            this.buttonClearLogSelectNone.Name = "buttonClearLogSelectNone";
            this.buttonClearLogSelectNone.Size = new System.Drawing.Size(138, 23);
            this.buttonClearLogSelectNone.TabIndex = 42;
            this.buttonClearLogSelectNone.Text = "SelectNone";
            this.buttonClearLogSelectNone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonClearLogSelectNone.UseVisualStyleBackColor = true;
            this.buttonClearLogSelectNone.Click += new System.EventHandler(this.buttonClearLogSelectNone_Click);
            // 
            // buttonClearLogStart
            // 
            this.tableLayoutPanelClearLog.SetColumnSpan(this.buttonClearLogStart, 2);
            this.buttonClearLogStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClearLogStart.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonClearLogStart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonClearLogStart.Location = new System.Drawing.Point(3, 499);
            this.buttonClearLogStart.Name = "buttonClearLogStart";
            this.buttonClearLogStart.Size = new System.Drawing.Size(281, 23);
            this.buttonClearLogStart.TabIndex = 43;
            this.buttonClearLogStart.Text = "Clear log of selected tables";
            this.buttonClearLogStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonClearLogStart.UseVisualStyleBackColor = true;
            this.buttonClearLogStart.Click += new System.EventHandler(this.buttonClearLogStart_Click);
            // 
            // dataGridViewClearLog
            // 
            this.dataGridViewClearLog.AllowUserToAddRows = false;
            this.dataGridViewClearLog.AllowUserToDeleteRows = false;
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle28.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle28.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle28.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle28.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle28.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewClearLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle28;
            this.dataGridViewClearLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewClearLog.DefaultCellStyle = dataGridViewCellStyle29;
            this.dataGridViewClearLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewClearLog.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewClearLog.Name = "dataGridViewClearLog";
            this.dataGridViewClearLog.ReadOnly = true;
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle30.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle30.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle30.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle30.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewClearLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle30;
            this.dataGridViewClearLog.RowHeadersVisible = false;
            this.dataGridViewClearLog.Size = new System.Drawing.Size(572, 525);
            this.dataGridViewClearLog.TabIndex = 0;
            // 
            // tabPageSaveLog
            // 
            this.tabPageSaveLog.Controls.Add(this.tabControlSaveLog);
            this.tabPageSaveLog.ImageIndex = 10;
            this.tabPageSaveLog.Location = new System.Drawing.Point(4, 23);
            this.tabPageSaveLog.Name = "tabPageSaveLog";
            this.tabPageSaveLog.Size = new System.Drawing.Size(863, 525);
            this.tabPageSaveLog.TabIndex = 6;
            this.tabPageSaveLog.Text = "Transfer log";
            this.tabPageSaveLog.UseVisualStyleBackColor = true;
            // 
            // tabControlSaveLog
            // 
            this.tabControlSaveLog.Controls.Add(this.tabPageSaveToLogDB);
            this.tabControlSaveLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSaveLog.Location = new System.Drawing.Point(0, 0);
            this.tabControlSaveLog.Name = "tabControlSaveLog";
            this.tabControlSaveLog.SelectedIndex = 0;
            this.tabControlSaveLog.Size = new System.Drawing.Size(863, 525);
            this.tabControlSaveLog.TabIndex = 3;
            // 
            // tabPageSaveToLogDB
            // 
            this.tabPageSaveToLogDB.Controls.Add(this.splitContainerSaveLog);
            this.tabPageSaveToLogDB.Controls.Add(this.labelSaveLogMainMessage);
            this.tabPageSaveToLogDB.Location = new System.Drawing.Point(4, 22);
            this.tabPageSaveToLogDB.Name = "tabPageSaveToLogDB";
            this.tabPageSaveToLogDB.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSaveToLogDB.Size = new System.Drawing.Size(855, 499);
            this.tabPageSaveToLogDB.TabIndex = 0;
            this.tabPageSaveToLogDB.Text = "Current log tables";
            this.tabPageSaveToLogDB.UseVisualStyleBackColor = true;
            // 
            // splitContainerSaveLog
            // 
            this.splitContainerSaveLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSaveLog.Location = new System.Drawing.Point(3, 26);
            this.splitContainerSaveLog.Name = "splitContainerSaveLog";
            // 
            // splitContainerSaveLog.Panel1
            // 
            this.splitContainerSaveLog.Panel1.Controls.Add(this.tableLayoutPanelSaveLog);
            // 
            // splitContainerSaveLog.Panel2
            // 
            this.splitContainerSaveLog.Panel2.Controls.Add(this.dataGridViewSaveLog);
            this.splitContainerSaveLog.Panel2.Controls.Add(this.labelSaveLogTableListHeader);
            this.splitContainerSaveLog.Size = new System.Drawing.Size(849, 470);
            this.splitContainerSaveLog.SplitterDistance = 197;
            this.splitContainerSaveLog.TabIndex = 1;
            // 
            // tableLayoutPanelSaveLog
            // 
            this.tableLayoutPanelSaveLog.ColumnCount = 1;
            this.tableLayoutPanelSaveLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSaveLog.Controls.Add(this.buttonSaveLog, 0, 3);
            this.tableLayoutPanelSaveLog.Controls.Add(this.labelSaveLogStartDate, 0, 0);
            this.tableLayoutPanelSaveLog.Controls.Add(this.buttonSaveLogListTables, 0, 2);
            this.tableLayoutPanelSaveLog.Controls.Add(this.dateTimePickerSaveLogStartDate, 0, 1);
            this.tableLayoutPanelSaveLog.Controls.Add(this.buttonSaveLogUserAdministration, 0, 5);
            this.tableLayoutPanelSaveLog.Controls.Add(this.buttonSaveLogCopyUser, 0, 6);
            this.tableLayoutPanelSaveLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSaveLog.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSaveLog.Name = "tableLayoutPanelSaveLog";
            this.tableLayoutPanelSaveLog.RowCount = 7;
            this.tableLayoutPanelSaveLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSaveLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSaveLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSaveLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSaveLog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSaveLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSaveLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSaveLog.Size = new System.Drawing.Size(197, 470);
            this.tableLayoutPanelSaveLog.TabIndex = 32;
            // 
            // buttonSaveLog
            // 
            this.buttonSaveLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveLog.Enabled = false;
            this.buttonSaveLog.Image = global::DiversityWorkbench.Properties.Resources.LogSave;
            this.buttonSaveLog.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSaveLog.Location = new System.Drawing.Point(3, 96);
            this.buttonSaveLog.Name = "buttonSaveLog";
            this.buttonSaveLog.Size = new System.Drawing.Size(191, 54);
            this.buttonSaveLog.TabIndex = 43;
            this.buttonSaveLog.Text = "Transfer content of log tables";
            this.buttonSaveLog.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip.SetToolTip(this.buttonSaveLog, "Transfer the selected data into the log database");
            this.buttonSaveLog.UseVisualStyleBackColor = true;
            this.buttonSaveLog.Click += new System.EventHandler(this.buttonSaveLog_Click);
            // 
            // labelSaveLogStartDate
            // 
            this.labelSaveLogStartDate.AutoSize = true;
            this.labelSaveLogStartDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSaveLogStartDate.Location = new System.Drawing.Point(6, 6);
            this.labelSaveLogStartDate.Margin = new System.Windows.Forms.Padding(6);
            this.labelSaveLogStartDate.Name = "labelSaveLogStartDate";
            this.labelSaveLogStartDate.Size = new System.Drawing.Size(185, 26);
            this.labelSaveLogStartDate.TabIndex = 45;
            this.labelSaveLogStartDate.Text = "Set the start date (older datasets will be transferred into the log database):";
            this.labelSaveLogStartDate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // buttonSaveLogListTables
            // 
            this.buttonSaveLogListTables.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonSaveLogListTables.Location = new System.Drawing.Point(3, 67);
            this.buttonSaveLogListTables.Name = "buttonSaveLogListTables";
            this.buttonSaveLogListTables.Size = new System.Drawing.Size(191, 23);
            this.buttonSaveLogListTables.TabIndex = 39;
            this.buttonSaveLogListTables.Text = "List tables";
            this.toolTip.SetToolTip(this.buttonSaveLogListTables, "List the log tables containing data older than the selected date that will be tra" +
        "nsferred into the log database");
            this.buttonSaveLogListTables.UseVisualStyleBackColor = true;
            this.buttonSaveLogListTables.Click += new System.EventHandler(this.buttonSaveLogListTables_Click);
            // 
            // dateTimePickerSaveLogStartDate
            // 
            this.dateTimePickerSaveLogStartDate.CustomFormat = "yyyy-MM-dd";
            this.dateTimePickerSaveLogStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerSaveLogStartDate.Location = new System.Drawing.Point(3, 41);
            this.dateTimePickerSaveLogStartDate.Name = "dateTimePickerSaveLogStartDate";
            this.dateTimePickerSaveLogStartDate.Size = new System.Drawing.Size(113, 20);
            this.dateTimePickerSaveLogStartDate.TabIndex = 44;
            this.toolTip.SetToolTip(this.dateTimePickerSaveLogStartDate, "Logged data older or equal to the selecte date will be transferred into the log d" +
        "atabase");
            this.dateTimePickerSaveLogStartDate.ValueChanged += new System.EventHandler(this.dateTimePickerSaveLogStartDate_ValueChanged);
            // 
            // buttonSaveLogUserAdministration
            // 
            this.buttonSaveLogUserAdministration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveLogUserAdministration.Image = global::DiversityWorkbench.Properties.Resources.Login;
            this.buttonSaveLogUserAdministration.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSaveLogUserAdministration.Location = new System.Drawing.Point(3, 415);
            this.buttonSaveLogUserAdministration.Name = "buttonSaveLogUserAdministration";
            this.buttonSaveLogUserAdministration.Size = new System.Drawing.Size(191, 23);
            this.buttonSaveLogUserAdministration.TabIndex = 46;
            this.buttonSaveLogUserAdministration.Text = "Logins";
            this.buttonSaveLogUserAdministration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonSaveLogUserAdministration, "Administration of logins in Log database");
            this.buttonSaveLogUserAdministration.UseVisualStyleBackColor = true;
            this.buttonSaveLogUserAdministration.Click += new System.EventHandler(this.buttonSaveLogUserAdministration_Click);
            // 
            // buttonSaveLogCopyUser
            // 
            this.buttonSaveLogCopyUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveLogCopyUser.Image = global::DiversityWorkbench.Properties.Resources.CopyAgent;
            this.buttonSaveLogCopyUser.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSaveLogCopyUser.Location = new System.Drawing.Point(3, 444);
            this.buttonSaveLogCopyUser.Name = "buttonSaveLogCopyUser";
            this.buttonSaveLogCopyUser.Size = new System.Drawing.Size(191, 23);
            this.buttonSaveLogCopyUser.TabIndex = 47;
            this.buttonSaveLogCopyUser.Text = "Copy user";
            this.buttonSaveLogCopyUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonSaveLogCopyUser, "Copy the user of the current database into the logging database");
            this.buttonSaveLogCopyUser.UseVisualStyleBackColor = true;
            this.buttonSaveLogCopyUser.Click += new System.EventHandler(this.buttonSaveLogCopyUser_Click);
            // 
            // dataGridViewSaveLog
            // 
            this.dataGridViewSaveLog.AllowUserToAddRows = false;
            this.dataGridViewSaveLog.AllowUserToDeleteRows = false;
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle31.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle31.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle31.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle31.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle31.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSaveLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle31;
            this.dataGridViewSaveLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle32.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle32.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle32.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle32.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle32.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSaveLog.DefaultCellStyle = dataGridViewCellStyle32;
            this.dataGridViewSaveLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSaveLog.Location = new System.Drawing.Point(0, 23);
            this.dataGridViewSaveLog.Name = "dataGridViewSaveLog";
            this.dataGridViewSaveLog.ReadOnly = true;
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle33.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle33.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle33.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle33.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle33.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSaveLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle33;
            this.dataGridViewSaveLog.RowHeadersVisible = false;
            this.dataGridViewSaveLog.Size = new System.Drawing.Size(648, 447);
            this.dataGridViewSaveLog.TabIndex = 0;
            // 
            // labelSaveLogTableListHeader
            // 
            this.labelSaveLogTableListHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSaveLogTableListHeader.Location = new System.Drawing.Point(0, 0);
            this.labelSaveLogTableListHeader.Margin = new System.Windows.Forms.Padding(6);
            this.labelSaveLogTableListHeader.Name = "labelSaveLogTableListHeader";
            this.labelSaveLogTableListHeader.Size = new System.Drawing.Size(648, 23);
            this.labelSaveLogTableListHeader.TabIndex = 1;
            this.labelSaveLogTableListHeader.Text = "The list shows the number of datasets transferred into the logging database and t" +
    "he number kept in the current database";
            this.labelSaveLogTableListHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.labelSaveLogTableListHeader, "The list shows the number of datasets transferred into the logging database and t" +
        "he number kept in the current database");
            // 
            // labelSaveLogMainMessage
            // 
            this.labelSaveLogMainMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSaveLogMainMessage.Location = new System.Drawing.Point(3, 3);
            this.labelSaveLogMainMessage.Name = "labelSaveLogMainMessage";
            this.labelSaveLogMainMessage.Size = new System.Drawing.Size(849, 23);
            this.labelSaveLogMainMessage.TabIndex = 2;
            this.labelSaveLogMainMessage.Text = "To reduce the size of the database the logged data may be transferred into a sepa" +
    "rate database located e.g. on a separate harddisc";
            this.labelSaveLogMainMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageProcedures
            // 
            this.tabPageProcedures.Controls.Add(this.tableLayoutPanelProcedures);
            this.tabPageProcedures.ImageIndex = 3;
            this.tabPageProcedures.Location = new System.Drawing.Point(4, 23);
            this.tabPageProcedures.Name = "tabPageProcedures";
            this.tabPageProcedures.Size = new System.Drawing.Size(863, 525);
            this.tabPageProcedures.TabIndex = 2;
            this.tabPageProcedures.Text = "Procedures";
            this.tabPageProcedures.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelProcedures
            // 
            this.tableLayoutPanelProcedures.ColumnCount = 5;
            this.tableLayoutPanelProcedures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProcedures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProcedures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProcedures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProcedures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProcedures.Controls.Add(this.tableLayoutPanelProcedureResult, 0, 7);
            this.tableLayoutPanelProcedures.Controls.Add(this.textBoxProcedureDefinition, 4, 3);
            this.tableLayoutPanelProcedures.Controls.Add(this.textBoxProcedureDescription, 4, 0);
            this.tableLayoutPanelProcedures.Controls.Add(this.tableLayoutPanelProcedureParameter, 3, 0);
            this.tableLayoutPanelProcedures.Controls.Add(this.labelTimeElapsed, 0, 6);
            this.tableLayoutPanelProcedures.Controls.Add(this.textBoxProcedureReturns, 1, 3);
            this.tableLayoutPanelProcedures.Controls.Add(this.labelProcedureList, 0, 0);
            this.tableLayoutPanelProcedures.Controls.Add(this.buttonStartDataTransfer, 0, 5);
            this.tableLayoutPanelProcedures.Controls.Add(this.labelTimeout, 0, 4);
            this.tableLayoutPanelProcedures.Controls.Add(this.labelProcedureReturns, 0, 3);
            this.tableLayoutPanelProcedures.Controls.Add(this.comboBoxProcedureList, 0, 1);
            this.tableLayoutPanelProcedures.Controls.Add(this.textBoxProcedureType, 1, 2);
            this.tableLayoutPanelProcedures.Controls.Add(this.labelProcedureType, 0, 2);
            this.tableLayoutPanelProcedures.Controls.Add(this.numericUpDownTimeout, 2, 4);
            this.tableLayoutPanelProcedures.Controls.Add(this.textBoxTimeElapsed, 2, 6);
            this.tableLayoutPanelProcedures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProcedures.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelProcedures.Name = "tableLayoutPanelProcedures";
            this.tableLayoutPanelProcedures.RowCount = 8;
            this.tableLayoutPanelProcedures.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedures.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedures.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedures.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedures.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedures.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedures.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedures.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProcedures.Size = new System.Drawing.Size(863, 525);
            this.tableLayoutPanelProcedures.TabIndex = 0;
            // 
            // tableLayoutPanelProcedureResult
            // 
            this.tableLayoutPanelProcedureResult.ColumnCount = 1;
            this.tableLayoutPanelProcedures.SetColumnSpan(this.tableLayoutPanelProcedureResult, 5);
            this.tableLayoutPanelProcedureResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProcedureResult.Controls.Add(this.labelProcedureResult, 0, 2);
            this.tableLayoutPanelProcedureResult.Controls.Add(this.splitContainerProcedureResult, 0, 3);
            this.tableLayoutPanelProcedureResult.Controls.Add(this.textBoxProcedureSQL, 0, 1);
            this.tableLayoutPanelProcedureResult.Controls.Add(this.labelProcedureCall, 0, 0);
            this.tableLayoutPanelProcedureResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProcedureResult.Location = new System.Drawing.Point(3, 182);
            this.tableLayoutPanelProcedureResult.Name = "tableLayoutPanelProcedureResult";
            this.tableLayoutPanelProcedureResult.RowCount = 4;
            this.tableLayoutPanelProcedureResult.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedureResult.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedureResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProcedureResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProcedureResult.Size = new System.Drawing.Size(857, 340);
            this.tableLayoutPanelProcedureResult.TabIndex = 12;
            // 
            // labelProcedureResult
            // 
            this.labelProcedureResult.AutoSize = true;
            this.labelProcedureResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcedureResult.Location = new System.Drawing.Point(3, 32);
            this.labelProcedureResult.Name = "labelProcedureResult";
            this.labelProcedureResult.Size = new System.Drawing.Size(926, 20);
            this.labelProcedureResult.TabIndex = 2;
            this.labelProcedureResult.Text = "Result";
            this.labelProcedureResult.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // splitContainerProcedureResult
            // 
            this.splitContainerProcedureResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerProcedureResult.Location = new System.Drawing.Point(3, 55);
            this.splitContainerProcedureResult.Name = "splitContainerProcedureResult";
            // 
            // splitContainerProcedureResult.Panel1
            // 
            this.splitContainerProcedureResult.Panel1.Controls.Add(this.dataGridViewProcedureResult);
            // 
            // splitContainerProcedureResult.Panel2
            // 
            this.splitContainerProcedureResult.Panel2.Controls.Add(this.textBoxProcedureResult);
            this.splitContainerProcedureResult.Size = new System.Drawing.Size(926, 282);
            this.splitContainerProcedureResult.SplitterDistance = 308;
            this.splitContainerProcedureResult.TabIndex = 3;
            // 
            // dataGridViewProcedureResult
            // 
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle34.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle34.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle34.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle34.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewProcedureResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle34;
            this.dataGridViewProcedureResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle35.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle35.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle35.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle35.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle35.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewProcedureResult.DefaultCellStyle = dataGridViewCellStyle35;
            this.dataGridViewProcedureResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProcedureResult.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewProcedureResult.Name = "dataGridViewProcedureResult";
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle36.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle36.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle36.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle36.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle36.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewProcedureResult.RowHeadersDefaultCellStyle = dataGridViewCellStyle36;
            this.dataGridViewProcedureResult.Size = new System.Drawing.Size(308, 282);
            this.dataGridViewProcedureResult.TabIndex = 0;
            // 
            // textBoxProcedureResult
            // 
            this.textBoxProcedureResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcedureResult.Location = new System.Drawing.Point(0, 0);
            this.textBoxProcedureResult.Multiline = true;
            this.textBoxProcedureResult.Name = "textBoxProcedureResult";
            this.textBoxProcedureResult.Size = new System.Drawing.Size(614, 282);
            this.textBoxProcedureResult.TabIndex = 1;
            // 
            // textBoxProcedureSQL
            // 
            this.textBoxProcedureSQL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxProcedureSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcedureSQL.Location = new System.Drawing.Point(3, 16);
            this.textBoxProcedureSQL.Name = "textBoxProcedureSQL";
            this.textBoxProcedureSQL.ReadOnly = true;
            this.textBoxProcedureSQL.Size = new System.Drawing.Size(926, 13);
            this.textBoxProcedureSQL.TabIndex = 4;
            // 
            // labelProcedureCall
            // 
            this.labelProcedureCall.AutoSize = true;
            this.labelProcedureCall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcedureCall.Location = new System.Drawing.Point(3, 0);
            this.labelProcedureCall.Name = "labelProcedureCall";
            this.labelProcedureCall.Size = new System.Drawing.Size(926, 13);
            this.labelProcedureCall.TabIndex = 5;
            this.labelProcedureCall.Text = "SQL-Statement for the call of the procedure";
            // 
            // textBoxProcedureDefinition
            // 
            this.textBoxProcedureDefinition.AcceptsReturn = true;
            this.textBoxProcedureDefinition.Location = new System.Drawing.Point(519, 71);
            this.textBoxProcedureDefinition.Multiline = true;
            this.textBoxProcedureDefinition.Name = "textBoxProcedureDefinition";
            this.textBoxProcedureDefinition.ReadOnly = true;
            this.tableLayoutPanelProcedures.SetRowSpan(this.textBoxProcedureDefinition, 4);
            this.textBoxProcedureDefinition.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxProcedureDefinition.Size = new System.Drawing.Size(307, 97);
            this.textBoxProcedureDefinition.TabIndex = 15;
            // 
            // textBoxProcedureDescription
            // 
            this.textBoxProcedureDescription.AcceptsReturn = true;
            this.textBoxProcedureDescription.Location = new System.Drawing.Point(519, 3);
            this.textBoxProcedureDescription.Multiline = true;
            this.textBoxProcedureDescription.Name = "textBoxProcedureDescription";
            this.textBoxProcedureDescription.ReadOnly = true;
            this.tableLayoutPanelProcedures.SetRowSpan(this.textBoxProcedureDescription, 3);
            this.textBoxProcedureDescription.Size = new System.Drawing.Size(307, 62);
            this.textBoxProcedureDescription.TabIndex = 6;
            // 
            // tableLayoutPanelProcedureParameter
            // 
            this.tableLayoutPanelProcedureParameter.ColumnCount = 2;
            this.tableLayoutPanelProcedureParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProcedureParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.textBoxProcedureParameter6, 1, 6);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.labelProcedureParameter6, 0, 6);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.textBoxProcedureParameter5, 1, 5);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.labelProcedureParameter5, 0, 5);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.labelProcedureParameter, 0, 0);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.labelProcedureParameterValue, 1, 0);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.labelProcedureParameter1, 0, 1);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.labelProcedureParameter2, 0, 2);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.labelProcedureParameter3, 0, 3);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.labelProcedureParameter4, 0, 4);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.textBoxProcedureParameter1, 1, 1);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.textBoxProcedureParameter2, 1, 2);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.textBoxProcedureParameter3, 1, 3);
            this.tableLayoutPanelProcedureParameter.Controls.Add(this.textBoxProcedureParameter4, 1, 4);
            this.tableLayoutPanelProcedureParameter.Location = new System.Drawing.Point(265, 3);
            this.tableLayoutPanelProcedureParameter.Name = "tableLayoutPanelProcedureParameter";
            this.tableLayoutPanelProcedureParameter.RowCount = 7;
            this.tableLayoutPanelProcedures.SetRowSpan(this.tableLayoutPanelProcedureParameter, 7);
            this.tableLayoutPanelProcedureParameter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedureParameter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedureParameter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedureParameter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedureParameter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedureParameter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcedureParameter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProcedureParameter.Size = new System.Drawing.Size(248, 173);
            this.tableLayoutPanelProcedureParameter.TabIndex = 9;
            // 
            // textBoxProcedureParameter6
            // 
            this.textBoxProcedureParameter6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcedureParameter6.Location = new System.Drawing.Point(64, 146);
            this.textBoxProcedureParameter6.Name = "textBoxProcedureParameter6";
            this.textBoxProcedureParameter6.Size = new System.Drawing.Size(181, 20);
            this.textBoxProcedureParameter6.TabIndex = 14;
            // 
            // labelProcedureParameter6
            // 
            this.labelProcedureParameter6.AutoSize = true;
            this.labelProcedureParameter6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcedureParameter6.Location = new System.Drawing.Point(3, 149);
            this.labelProcedureParameter6.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelProcedureParameter6.Name = "labelProcedureParameter6";
            this.labelProcedureParameter6.Size = new System.Drawing.Size(55, 24);
            this.labelProcedureParameter6.TabIndex = 13;
            this.labelProcedureParameter6.Text = "label3";
            // 
            // textBoxProcedureParameter5
            // 
            this.textBoxProcedureParameter5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcedureParameter5.Location = new System.Drawing.Point(64, 120);
            this.textBoxProcedureParameter5.Name = "textBoxProcedureParameter5";
            this.textBoxProcedureParameter5.Size = new System.Drawing.Size(181, 20);
            this.textBoxProcedureParameter5.TabIndex = 12;
            // 
            // labelProcedureParameter5
            // 
            this.labelProcedureParameter5.AutoSize = true;
            this.labelProcedureParameter5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcedureParameter5.Location = new System.Drawing.Point(3, 123);
            this.labelProcedureParameter5.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelProcedureParameter5.Name = "labelProcedureParameter5";
            this.labelProcedureParameter5.Size = new System.Drawing.Size(55, 20);
            this.labelProcedureParameter5.TabIndex = 11;
            this.labelProcedureParameter5.Text = "label3";
            // 
            // labelProcedureParameter
            // 
            this.labelProcedureParameter.AutoSize = true;
            this.labelProcedureParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcedureParameter.Location = new System.Drawing.Point(3, 0);
            this.labelProcedureParameter.Name = "labelProcedureParameter";
            this.labelProcedureParameter.Size = new System.Drawing.Size(55, 13);
            this.labelProcedureParameter.TabIndex = 0;
            this.labelProcedureParameter.Text = "Parameter";
            this.labelProcedureParameter.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelProcedureParameterValue
            // 
            this.labelProcedureParameterValue.AutoSize = true;
            this.labelProcedureParameterValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcedureParameterValue.Location = new System.Drawing.Point(64, 0);
            this.labelProcedureParameterValue.Name = "labelProcedureParameterValue";
            this.labelProcedureParameterValue.Size = new System.Drawing.Size(181, 13);
            this.labelProcedureParameterValue.TabIndex = 1;
            this.labelProcedureParameterValue.Text = "Value";
            this.labelProcedureParameterValue.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelProcedureParameter1
            // 
            this.labelProcedureParameter1.AutoSize = true;
            this.labelProcedureParameter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcedureParameter1.Location = new System.Drawing.Point(3, 13);
            this.labelProcedureParameter1.Name = "labelProcedureParameter1";
            this.labelProcedureParameter1.Size = new System.Drawing.Size(55, 26);
            this.labelProcedureParameter1.TabIndex = 2;
            this.labelProcedureParameter1.Text = "label3";
            this.labelProcedureParameter1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProcedureParameter2
            // 
            this.labelProcedureParameter2.AutoSize = true;
            this.labelProcedureParameter2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcedureParameter2.Location = new System.Drawing.Point(3, 39);
            this.labelProcedureParameter2.Name = "labelProcedureParameter2";
            this.labelProcedureParameter2.Size = new System.Drawing.Size(55, 26);
            this.labelProcedureParameter2.TabIndex = 3;
            this.labelProcedureParameter2.Text = "label3";
            this.labelProcedureParameter2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProcedureParameter3
            // 
            this.labelProcedureParameter3.AutoSize = true;
            this.labelProcedureParameter3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcedureParameter3.Location = new System.Drawing.Point(3, 65);
            this.labelProcedureParameter3.Name = "labelProcedureParameter3";
            this.labelProcedureParameter3.Size = new System.Drawing.Size(55, 26);
            this.labelProcedureParameter3.TabIndex = 4;
            this.labelProcedureParameter3.Text = "label3";
            this.labelProcedureParameter3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProcedureParameter4
            // 
            this.labelProcedureParameter4.AutoSize = true;
            this.labelProcedureParameter4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcedureParameter4.Location = new System.Drawing.Point(3, 97);
            this.labelProcedureParameter4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelProcedureParameter4.Name = "labelProcedureParameter4";
            this.labelProcedureParameter4.Size = new System.Drawing.Size(55, 20);
            this.labelProcedureParameter4.TabIndex = 5;
            this.labelProcedureParameter4.Text = "label3";
            // 
            // textBoxProcedureParameter1
            // 
            this.textBoxProcedureParameter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcedureParameter1.Location = new System.Drawing.Point(64, 16);
            this.textBoxProcedureParameter1.Name = "textBoxProcedureParameter1";
            this.textBoxProcedureParameter1.Size = new System.Drawing.Size(181, 20);
            this.textBoxProcedureParameter1.TabIndex = 7;
            // 
            // textBoxProcedureParameter2
            // 
            this.textBoxProcedureParameter2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcedureParameter2.Location = new System.Drawing.Point(64, 42);
            this.textBoxProcedureParameter2.Name = "textBoxProcedureParameter2";
            this.textBoxProcedureParameter2.Size = new System.Drawing.Size(181, 20);
            this.textBoxProcedureParameter2.TabIndex = 8;
            // 
            // textBoxProcedureParameter3
            // 
            this.textBoxProcedureParameter3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcedureParameter3.Location = new System.Drawing.Point(64, 68);
            this.textBoxProcedureParameter3.Name = "textBoxProcedureParameter3";
            this.textBoxProcedureParameter3.Size = new System.Drawing.Size(181, 20);
            this.textBoxProcedureParameter3.TabIndex = 9;
            // 
            // textBoxProcedureParameter4
            // 
            this.textBoxProcedureParameter4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcedureParameter4.Location = new System.Drawing.Point(64, 94);
            this.textBoxProcedureParameter4.Name = "textBoxProcedureParameter4";
            this.textBoxProcedureParameter4.Size = new System.Drawing.Size(181, 20);
            this.textBoxProcedureParameter4.TabIndex = 10;
            // 
            // labelTimeElapsed
            // 
            this.labelTimeElapsed.AutoSize = true;
            this.tableLayoutPanelProcedures.SetColumnSpan(this.labelTimeElapsed, 2);
            this.labelTimeElapsed.Location = new System.Drawing.Point(3, 149);
            this.labelTimeElapsed.Name = "labelTimeElapsed";
            this.labelTimeElapsed.Size = new System.Drawing.Size(136, 13);
            this.labelTimeElapsed.TabIndex = 20;
            this.labelTimeElapsed.Text = "Time needed for execution:";
            // 
            // textBoxProcedureReturns
            // 
            this.tableLayoutPanelProcedures.SetColumnSpan(this.textBoxProcedureReturns, 2);
            this.textBoxProcedureReturns.Location = new System.Drawing.Point(56, 71);
            this.textBoxProcedureReturns.Name = "textBoxProcedureReturns";
            this.textBoxProcedureReturns.ReadOnly = true;
            this.textBoxProcedureReturns.Size = new System.Drawing.Size(197, 20);
            this.textBoxProcedureReturns.TabIndex = 24;
            // 
            // labelProcedureList
            // 
            this.labelProcedureList.AutoSize = true;
            this.tableLayoutPanelProcedures.SetColumnSpan(this.labelProcedureList, 3);
            this.labelProcedureList.Location = new System.Drawing.Point(3, 0);
            this.labelProcedureList.Name = "labelProcedureList";
            this.labelProcedureList.Size = new System.Drawing.Size(159, 13);
            this.labelProcedureList.TabIndex = 17;
            this.labelProcedureList.Text = "Choose a procedure from the list";
            // 
            // buttonStartDataTransfer
            // 
            this.tableLayoutPanelProcedures.SetColumnSpan(this.buttonStartDataTransfer, 3);
            this.buttonStartDataTransfer.Location = new System.Drawing.Point(3, 123);
            this.buttonStartDataTransfer.Name = "buttonStartDataTransfer";
            this.buttonStartDataTransfer.Size = new System.Drawing.Size(256, 23);
            this.buttonStartDataTransfer.TabIndex = 14;
            this.buttonStartDataTransfer.Text = "Start procedure";
            this.buttonStartDataTransfer.UseVisualStyleBackColor = true;
            this.buttonStartDataTransfer.Click += new System.EventHandler(this.buttonStartDataTransfer_Click);
            // 
            // labelTimeout
            // 
            this.labelTimeout.AutoSize = true;
            this.tableLayoutPanelProcedures.SetColumnSpan(this.labelTimeout, 2);
            this.labelTimeout.Location = new System.Drawing.Point(3, 94);
            this.labelTimeout.Name = "labelTimeout";
            this.labelTimeout.Size = new System.Drawing.Size(114, 13);
            this.labelTimeout.TabIndex = 18;
            this.labelTimeout.Text = "Timeout for procedure:";
            // 
            // labelProcedureReturns
            // 
            this.labelProcedureReturns.AutoSize = true;
            this.labelProcedureReturns.Location = new System.Drawing.Point(3, 68);
            this.labelProcedureReturns.Name = "labelProcedureReturns";
            this.labelProcedureReturns.Size = new System.Drawing.Size(47, 13);
            this.labelProcedureReturns.TabIndex = 23;
            this.labelProcedureReturns.Text = "Returns:";
            // 
            // comboBoxProcedureList
            // 
            this.tableLayoutPanelProcedures.SetColumnSpan(this.comboBoxProcedureList, 3);
            this.comboBoxProcedureList.FormattingEnabled = true;
            this.comboBoxProcedureList.Location = new System.Drawing.Point(3, 16);
            this.comboBoxProcedureList.Name = "comboBoxProcedureList";
            this.comboBoxProcedureList.Size = new System.Drawing.Size(256, 21);
            this.comboBoxProcedureList.TabIndex = 15;
            this.comboBoxProcedureList.SelectedIndexChanged += new System.EventHandler(this.comboBoxProcedureList_SelectedIndexChanged);
            // 
            // textBoxProcedureType
            // 
            this.tableLayoutPanelProcedures.SetColumnSpan(this.textBoxProcedureType, 2);
            this.textBoxProcedureType.Location = new System.Drawing.Point(56, 43);
            this.textBoxProcedureType.Name = "textBoxProcedureType";
            this.textBoxProcedureType.ReadOnly = true;
            this.textBoxProcedureType.Size = new System.Drawing.Size(197, 20);
            this.textBoxProcedureType.TabIndex = 22;
            // 
            // labelProcedureType
            // 
            this.labelProcedureType.AutoSize = true;
            this.labelProcedureType.Location = new System.Drawing.Point(3, 40);
            this.labelProcedureType.Name = "labelProcedureType";
            this.labelProcedureType.Size = new System.Drawing.Size(34, 13);
            this.labelProcedureType.TabIndex = 21;
            this.labelProcedureType.Text = "Type:";
            // 
            // numericUpDownTimeout
            // 
            this.numericUpDownTimeout.Increment = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownTimeout.Location = new System.Drawing.Point(177, 97);
            this.numericUpDownTimeout.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownTimeout.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownTimeout.Name = "numericUpDownTimeout";
            this.numericUpDownTimeout.Size = new System.Drawing.Size(75, 20);
            this.numericUpDownTimeout.TabIndex = 16;
            this.numericUpDownTimeout.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // textBoxTimeElapsed
            // 
            this.textBoxTimeElapsed.Location = new System.Drawing.Point(177, 152);
            this.textBoxTimeElapsed.Name = "textBoxTimeElapsed";
            this.textBoxTimeElapsed.ReadOnly = true;
            this.textBoxTimeElapsed.Size = new System.Drawing.Size(76, 20);
            this.textBoxTimeElapsed.TabIndex = 19;
            this.textBoxTimeElapsed.Text = "0";
            // 
            // tabPageRowGUID
            // 
            this.tabPageRowGUID.Controls.Add(this.splitContainerReplication);
            this.tabPageRowGUID.ImageIndex = 4;
            this.tabPageRowGUID.Location = new System.Drawing.Point(4, 23);
            this.tabPageRowGUID.Name = "tabPageRowGUID";
            this.tabPageRowGUID.Size = new System.Drawing.Size(863, 525);
            this.tabPageRowGUID.TabIndex = 3;
            this.tabPageRowGUID.Text = "Replication (RowGUID etc.)";
            this.tabPageRowGUID.UseVisualStyleBackColor = true;
            // 
            // splitContainerReplication
            // 
            this.splitContainerReplication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerReplication.Location = new System.Drawing.Point(0, 0);
            this.splitContainerReplication.Name = "splitContainerReplication";
            // 
            // splitContainerReplication.Panel1
            // 
            this.splitContainerReplication.Panel1.Controls.Add(this.tableLayoutPanelReplicationTableList);
            // 
            // splitContainerReplication.Panel2
            // 
            this.splitContainerReplication.Panel2.Controls.Add(this.tabControlReplication);
            this.splitContainerReplication.Size = new System.Drawing.Size(863, 525);
            this.splitContainerReplication.SplitterDistance = 223;
            this.splitContainerReplication.TabIndex = 2;
            // 
            // tableLayoutPanelReplicationTableList
            // 
            this.tableLayoutPanelReplicationTableList.ColumnCount = 3;
            this.tableLayoutPanelReplicationTableList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelReplicationTableList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelReplicationTableList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReplicationTableList.Controls.Add(this.buttonReplicationListTables, 2, 0);
            this.tableLayoutPanelReplicationTableList.Controls.Add(this.labelReplicationTableList, 0, 0);
            this.tableLayoutPanelReplicationTableList.Controls.Add(this.checkedListBoxRepPrep, 0, 1);
            this.tableLayoutPanelReplicationTableList.Controls.Add(this.buttonRepPrepAll, 0, 2);
            this.tableLayoutPanelReplicationTableList.Controls.Add(this.buttonRepPrepNone, 2, 2);
            this.tableLayoutPanelReplicationTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelReplicationTableList.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelReplicationTableList.Name = "tableLayoutPanelReplicationTableList";
            this.tableLayoutPanelReplicationTableList.RowCount = 3;
            this.tableLayoutPanelReplicationTableList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationTableList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReplicationTableList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationTableList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelReplicationTableList.Size = new System.Drawing.Size(223, 525);
            this.tableLayoutPanelReplicationTableList.TabIndex = 31;
            // 
            // buttonReplicationListTables
            // 
            this.buttonReplicationListTables.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonReplicationListTables.Location = new System.Drawing.Point(158, 3);
            this.buttonReplicationListTables.Name = "buttonReplicationListTables";
            this.buttonReplicationListTables.Size = new System.Drawing.Size(62, 23);
            this.buttonReplicationListTables.TabIndex = 39;
            this.buttonReplicationListTables.Text = "List tables";
            this.buttonReplicationListTables.UseVisualStyleBackColor = true;
            this.buttonReplicationListTables.Click += new System.EventHandler(this.buttonReplicationListTables_Click);
            // 
            // labelReplicationTableList
            // 
            this.labelReplicationTableList.AutoSize = true;
            this.tableLayoutPanelReplicationTableList.SetColumnSpan(this.labelReplicationTableList, 2);
            this.labelReplicationTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReplicationTableList.Location = new System.Drawing.Point(3, 0);
            this.labelReplicationTableList.Name = "labelReplicationTableList";
            this.labelReplicationTableList.Size = new System.Drawing.Size(97, 29);
            this.labelReplicationTableList.TabIndex = 30;
            this.labelReplicationTableList.Text = "Tables in database";
            this.labelReplicationTableList.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // checkedListBoxRepPrep
            // 
            this.checkedListBoxRepPrep.CheckOnClick = true;
            this.tableLayoutPanelReplicationTableList.SetColumnSpan(this.checkedListBoxRepPrep, 3);
            this.checkedListBoxRepPrep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxRepPrep.FormattingEnabled = true;
            this.checkedListBoxRepPrep.Location = new System.Drawing.Point(3, 32);
            this.checkedListBoxRepPrep.Name = "checkedListBoxRepPrep";
            this.checkedListBoxRepPrep.Size = new System.Drawing.Size(217, 461);
            this.checkedListBoxRepPrep.TabIndex = 40;
            this.checkedListBoxRepPrep.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxRepPrep_SelectedIndexChanged);
            // 
            // buttonRepPrepAll
            // 
            this.tableLayoutPanelReplicationTableList.SetColumnSpan(this.buttonRepPrepAll, 2);
            this.buttonRepPrepAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRepPrepAll.Image = global::DiversityWorkbench.Properties.Resources.CheckYes;
            this.buttonRepPrepAll.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRepPrepAll.Location = new System.Drawing.Point(3, 499);
            this.buttonRepPrepAll.Name = "buttonRepPrepAll";
            this.buttonRepPrepAll.Size = new System.Drawing.Size(97, 23);
            this.buttonRepPrepAll.TabIndex = 41;
            this.buttonRepPrepAll.Text = "Select all";
            this.buttonRepPrepAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRepPrepAll.UseVisualStyleBackColor = true;
            this.buttonRepPrepAll.Click += new System.EventHandler(this.buttonRepPrepAll_Click);
            // 
            // buttonRepPrepNone
            // 
            this.buttonRepPrepNone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRepPrepNone.Image = global::DiversityWorkbench.Properties.Resources.CheckNo;
            this.buttonRepPrepNone.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRepPrepNone.Location = new System.Drawing.Point(106, 499);
            this.buttonRepPrepNone.Name = "buttonRepPrepNone";
            this.buttonRepPrepNone.Size = new System.Drawing.Size(114, 23);
            this.buttonRepPrepNone.TabIndex = 42;
            this.buttonRepPrepNone.Text = "SelectNone";
            this.buttonRepPrepNone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRepPrepNone.UseVisualStyleBackColor = true;
            this.buttonRepPrepNone.Click += new System.EventHandler(this.buttonRepPrepNone_Click);
            // 
            // tabControlReplication
            // 
            this.tabControlReplication.Controls.Add(this.tabPageRepPrepScript);
            this.tabControlReplication.Controls.Add(this.tabPageRepPrep);
            this.tabControlReplication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlReplication.Location = new System.Drawing.Point(0, 0);
            this.tabControlReplication.Name = "tabControlReplication";
            this.tabControlReplication.SelectedIndex = 0;
            this.tabControlReplication.Size = new System.Drawing.Size(636, 525);
            this.tabControlReplication.TabIndex = 41;
            // 
            // tabPageRepPrepScript
            // 
            this.tabPageRepPrepScript.Controls.Add(this.tableLayoutPanelRepPrepScript);
            this.tabPageRepPrepScript.Location = new System.Drawing.Point(4, 22);
            this.tabPageRepPrepScript.Name = "tabPageRepPrepScript";
            this.tabPageRepPrepScript.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRepPrepScript.Size = new System.Drawing.Size(628, 499);
            this.tabPageRepPrepScript.TabIndex = 7;
            this.tabPageRepPrepScript.Text = "Script";
            this.tabPageRepPrepScript.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelRepPrepScript
            // 
            this.tableLayoutPanelRepPrepScript.ColumnCount = 3;
            this.tableLayoutPanelRepPrepScript.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRepPrepScript.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRepPrepScript.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRepPrepScript.Controls.Add(this.checkBoxRepPrepDsgvo, 2, 0);
            this.tableLayoutPanelRepPrepScript.Controls.Add(this.textBoxRepPrepScript, 0, 1);
            this.tableLayoutPanelRepPrepScript.Controls.Add(this.buttonRepPrepScript, 0, 0);
            this.tableLayoutPanelRepPrepScript.Controls.Add(this.buttonRepPrepScriptSave, 2, 2);
            this.tableLayoutPanelRepPrepScript.Controls.Add(this.buttonRepPrepScriptFile, 0, 2);
            this.tableLayoutPanelRepPrepScript.Controls.Add(this.textBoxRepPrepScriptFile, 1, 2);
            this.tableLayoutPanelRepPrepScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRepPrepScript.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelRepPrepScript.Name = "tableLayoutPanelRepPrepScript";
            this.tableLayoutPanelRepPrepScript.RowCount = 3;
            this.tableLayoutPanelRepPrepScript.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRepPrepScript.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRepPrepScript.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelRepPrepScript.Size = new System.Drawing.Size(622, 493);
            this.tableLayoutPanelRepPrepScript.TabIndex = 0;
            // 
            // checkBoxRepPrepDsgvo
            // 
            this.checkBoxRepPrepDsgvo.Image = global::DiversityWorkbench.Properties.Resources.Paragraf;
            this.checkBoxRepPrepDsgvo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxRepPrepDsgvo.Location = new System.Drawing.Point(539, 0);
            this.checkBoxRepPrepDsgvo.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxRepPrepDsgvo.Name = "checkBoxRepPrepDsgvo";
            this.checkBoxRepPrepDsgvo.Size = new System.Drawing.Size(80, 28);
            this.checkBoxRepPrepDsgvo.TabIndex = 36;
            this.checkBoxRepPrepDsgvo.Text = "DSGVO";
            this.checkBoxRepPrepDsgvo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxRepPrepDsgvo.UseVisualStyleBackColor = true;
            // 
            // textBoxRepPrepScript
            // 
            this.tableLayoutPanelRepPrepScript.SetColumnSpan(this.textBoxRepPrepScript, 3);
            this.textBoxRepPrepScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRepPrepScript.Location = new System.Drawing.Point(3, 32);
            this.textBoxRepPrepScript.Multiline = true;
            this.textBoxRepPrepScript.Name = "textBoxRepPrepScript";
            this.textBoxRepPrepScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxRepPrepScript.Size = new System.Drawing.Size(616, 434);
            this.textBoxRepPrepScript.TabIndex = 1;
            // 
            // buttonRepPrepScript
            // 
            this.tableLayoutPanelRepPrepScript.SetColumnSpan(this.buttonRepPrepScript, 2);
            this.buttonRepPrepScript.Location = new System.Drawing.Point(3, 3);
            this.buttonRepPrepScript.Name = "buttonRepPrepScript";
            this.buttonRepPrepScript.Size = new System.Drawing.Size(198, 23);
            this.buttonRepPrepScript.TabIndex = 2;
            this.buttonRepPrepScript.Text = "Create a script for the selected tables";
            this.buttonRepPrepScript.UseVisualStyleBackColor = true;
            this.buttonRepPrepScript.Click += new System.EventHandler(this.buttonRepPrepScript_Click);
            // 
            // buttonRepPrepScriptSave
            // 
            this.buttonRepPrepScriptSave.Image = global::DiversityWorkbench.ResourceWorkbench.Save;
            this.buttonRepPrepScriptSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRepPrepScriptSave.Location = new System.Drawing.Point(539, 469);
            this.buttonRepPrepScriptSave.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRepPrepScriptSave.Name = "buttonRepPrepScriptSave";
            this.buttonRepPrepScriptSave.Size = new System.Drawing.Size(83, 23);
            this.buttonRepPrepScriptSave.TabIndex = 3;
            this.buttonRepPrepScriptSave.Text = "Save script";
            this.buttonRepPrepScriptSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRepPrepScriptSave.UseVisualStyleBackColor = true;
            this.buttonRepPrepScriptSave.Click += new System.EventHandler(this.buttonRepPrepScriptSave_Click);
            // 
            // buttonRepPrepScriptFile
            // 
            this.buttonRepPrepScriptFile.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.buttonRepPrepScriptFile.Location = new System.Drawing.Point(0, 469);
            this.buttonRepPrepScriptFile.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRepPrepScriptFile.Name = "buttonRepPrepScriptFile";
            this.buttonRepPrepScriptFile.Size = new System.Drawing.Size(24, 23);
            this.buttonRepPrepScriptFile.TabIndex = 4;
            this.buttonRepPrepScriptFile.UseVisualStyleBackColor = true;
            this.buttonRepPrepScriptFile.Click += new System.EventHandler(this.buttonRepPrepScriptFile_Click);
            // 
            // textBoxRepPrepScriptFile
            // 
            this.textBoxRepPrepScriptFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRepPrepScriptFile.Location = new System.Drawing.Point(24, 472);
            this.textBoxRepPrepScriptFile.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxRepPrepScriptFile.Name = "textBoxRepPrepScriptFile";
            this.textBoxRepPrepScriptFile.ReadOnly = true;
            this.textBoxRepPrepScriptFile.Size = new System.Drawing.Size(512, 20);
            this.textBoxRepPrepScriptFile.TabIndex = 5;
            // 
            // tabPageRepPrep
            // 
            this.tabPageRepPrep.Controls.Add(this.tableLayoutPanelReplicationPreparationSteps);
            this.tabPageRepPrep.Location = new System.Drawing.Point(4, 22);
            this.tabPageRepPrep.Name = "tabPageRepPrep";
            this.tabPageRepPrep.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRepPrep.Size = new System.Drawing.Size(628, 499);
            this.tabPageRepPrep.TabIndex = 6;
            this.tabPageRepPrep.Text = "Replication preparation";
            this.tabPageRepPrep.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelReplicationPreparationSteps
            // 
            this.tableLayoutPanelReplicationPreparationSteps.ColumnCount = 1;
            this.tableLayoutPanelReplicationPreparationSteps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.labelRepPrepHeader, 0, 0);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.buttonStartReplicationPreparation, 0, 12);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.userControlReplicationPreparation02AddRowGUID, 0, 2);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.userControlReplicationPreparation03CreateDefaults, 0, 3);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.userControlReplicationPreparation04CreateTemporaryTable, 0, 4);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.userControlReplicationPreparation05ReadData, 0, 5);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.userControlReplicationPreparation06DeactivateUpdateTrigger, 0, 6);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.userControlReplicationPreparation07WriteRowGUID, 0, 7);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.userControlReplicationPreparation08WriteDate, 0, 8);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.userControlReplicationPreparation09ActivateUpdateTrigger, 0, 9);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.userControlReplicationPreparation10DeleteTempTable, 0, 10);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.labelRepPrepMessage, 0, 11);
            this.tableLayoutPanelReplicationPreparationSteps.Controls.Add(this.userControlReplicationPreparation01CreateReplPublTable, 0, 1);
            this.tableLayoutPanelReplicationPreparationSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelReplicationPreparationSteps.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelReplicationPreparationSteps.Name = "tableLayoutPanelReplicationPreparationSteps";
            this.tableLayoutPanelReplicationPreparationSteps.RowCount = 13;
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelReplicationPreparationSteps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReplicationPreparationSteps.Size = new System.Drawing.Size(622, 493);
            this.tableLayoutPanelReplicationPreparationSteps.TabIndex = 0;
            // 
            // labelRepPrepHeader
            // 
            this.labelRepPrepHeader.AutoSize = true;
            this.labelRepPrepHeader.Location = new System.Drawing.Point(3, 6);
            this.labelRepPrepHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.labelRepPrepHeader.Name = "labelRepPrepHeader";
            this.labelRepPrepHeader.Size = new System.Drawing.Size(318, 13);
            this.labelRepPrepHeader.TabIndex = 1;
            this.labelRepPrepHeader.Text = "The preparation for the replication includes the  steps listed below:";
            // 
            // buttonStartReplicationPreparation
            // 
            this.buttonStartReplicationPreparation.Enabled = false;
            this.buttonStartReplicationPreparation.ForeColor = System.Drawing.Color.Red;
            this.buttonStartReplicationPreparation.Image = global::DiversityWorkbench.Properties.Resources.UpdateDatabase;
            this.buttonStartReplicationPreparation.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStartReplicationPreparation.Location = new System.Drawing.Point(3, 368);
            this.buttonStartReplicationPreparation.Name = "buttonStartReplicationPreparation";
            this.buttonStartReplicationPreparation.Size = new System.Drawing.Size(190, 24);
            this.buttonStartReplicationPreparation.TabIndex = 0;
            this.buttonStartReplicationPreparation.Text = "Start the preparation steps";
            this.buttonStartReplicationPreparation.UseVisualStyleBackColor = true;
            this.buttonStartReplicationPreparation.Click += new System.EventHandler(this.buttonStartReplicationPreparation_Click);
            // 
            // userControlReplicationPreparation02AddRowGUID
            // 
            this.userControlReplicationPreparation02AddRowGUID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlReplicationPreparation02AddRowGUID.ErrorMessage = null;
            this.userControlReplicationPreparation02AddRowGUID.Location = new System.Drawing.Point(3, 60);
            this.userControlReplicationPreparation02AddRowGUID.Name = "userControlReplicationPreparation02AddRowGUID";
            this.userControlReplicationPreparation02AddRowGUID.Size = new System.Drawing.Size(616, 26);
            this.userControlReplicationPreparation02AddRowGUID.TabIndex = 23;
            // 
            // userControlReplicationPreparation03CreateDefaults
            // 
            this.userControlReplicationPreparation03CreateDefaults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlReplicationPreparation03CreateDefaults.ErrorMessage = null;
            this.userControlReplicationPreparation03CreateDefaults.Location = new System.Drawing.Point(3, 92);
            this.userControlReplicationPreparation03CreateDefaults.Name = "userControlReplicationPreparation03CreateDefaults";
            this.userControlReplicationPreparation03CreateDefaults.Size = new System.Drawing.Size(616, 26);
            this.userControlReplicationPreparation03CreateDefaults.TabIndex = 24;
            // 
            // userControlReplicationPreparation04CreateTemporaryTable
            // 
            this.userControlReplicationPreparation04CreateTemporaryTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlReplicationPreparation04CreateTemporaryTable.ErrorMessage = null;
            this.userControlReplicationPreparation04CreateTemporaryTable.Location = new System.Drawing.Point(3, 124);
            this.userControlReplicationPreparation04CreateTemporaryTable.Name = "userControlReplicationPreparation04CreateTemporaryTable";
            this.userControlReplicationPreparation04CreateTemporaryTable.Size = new System.Drawing.Size(616, 26);
            this.userControlReplicationPreparation04CreateTemporaryTable.TabIndex = 25;
            // 
            // userControlReplicationPreparation05ReadData
            // 
            this.userControlReplicationPreparation05ReadData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlReplicationPreparation05ReadData.ErrorMessage = null;
            this.userControlReplicationPreparation05ReadData.Location = new System.Drawing.Point(3, 156);
            this.userControlReplicationPreparation05ReadData.Name = "userControlReplicationPreparation05ReadData";
            this.userControlReplicationPreparation05ReadData.Size = new System.Drawing.Size(616, 26);
            this.userControlReplicationPreparation05ReadData.TabIndex = 26;
            // 
            // userControlReplicationPreparation06DeactivateUpdateTrigger
            // 
            this.userControlReplicationPreparation06DeactivateUpdateTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlReplicationPreparation06DeactivateUpdateTrigger.ErrorMessage = null;
            this.userControlReplicationPreparation06DeactivateUpdateTrigger.Location = new System.Drawing.Point(3, 188);
            this.userControlReplicationPreparation06DeactivateUpdateTrigger.Name = "userControlReplicationPreparation06DeactivateUpdateTrigger";
            this.userControlReplicationPreparation06DeactivateUpdateTrigger.Size = new System.Drawing.Size(616, 26);
            this.userControlReplicationPreparation06DeactivateUpdateTrigger.TabIndex = 27;
            // 
            // userControlReplicationPreparation07WriteRowGUID
            // 
            this.userControlReplicationPreparation07WriteRowGUID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlReplicationPreparation07WriteRowGUID.ErrorMessage = null;
            this.userControlReplicationPreparation07WriteRowGUID.Location = new System.Drawing.Point(3, 220);
            this.userControlReplicationPreparation07WriteRowGUID.Name = "userControlReplicationPreparation07WriteRowGUID";
            this.userControlReplicationPreparation07WriteRowGUID.Size = new System.Drawing.Size(616, 26);
            this.userControlReplicationPreparation07WriteRowGUID.TabIndex = 28;
            // 
            // userControlReplicationPreparation08WriteDate
            // 
            this.userControlReplicationPreparation08WriteDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlReplicationPreparation08WriteDate.ErrorMessage = null;
            this.userControlReplicationPreparation08WriteDate.Location = new System.Drawing.Point(3, 252);
            this.userControlReplicationPreparation08WriteDate.Name = "userControlReplicationPreparation08WriteDate";
            this.userControlReplicationPreparation08WriteDate.Size = new System.Drawing.Size(616, 26);
            this.userControlReplicationPreparation08WriteDate.TabIndex = 29;
            // 
            // userControlReplicationPreparation09ActivateUpdateTrigger
            // 
            this.userControlReplicationPreparation09ActivateUpdateTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlReplicationPreparation09ActivateUpdateTrigger.ErrorMessage = null;
            this.userControlReplicationPreparation09ActivateUpdateTrigger.Location = new System.Drawing.Point(3, 284);
            this.userControlReplicationPreparation09ActivateUpdateTrigger.Name = "userControlReplicationPreparation09ActivateUpdateTrigger";
            this.userControlReplicationPreparation09ActivateUpdateTrigger.Size = new System.Drawing.Size(616, 26);
            this.userControlReplicationPreparation09ActivateUpdateTrigger.TabIndex = 30;
            // 
            // userControlReplicationPreparation10DeleteTempTable
            // 
            this.userControlReplicationPreparation10DeleteTempTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlReplicationPreparation10DeleteTempTable.ErrorMessage = null;
            this.userControlReplicationPreparation10DeleteTempTable.Location = new System.Drawing.Point(3, 316);
            this.userControlReplicationPreparation10DeleteTempTable.Name = "userControlReplicationPreparation10DeleteTempTable";
            this.userControlReplicationPreparation10DeleteTempTable.Size = new System.Drawing.Size(616, 26);
            this.userControlReplicationPreparation10DeleteTempTable.TabIndex = 31;
            // 
            // labelRepPrepMessage
            // 
            this.labelRepPrepMessage.AutoSize = true;
            this.labelRepPrepMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRepPrepMessage.ForeColor = System.Drawing.Color.Red;
            this.labelRepPrepMessage.Location = new System.Drawing.Point(3, 345);
            this.labelRepPrepMessage.Name = "labelRepPrepMessage";
            this.labelRepPrepMessage.Size = new System.Drawing.Size(616, 20);
            this.labelRepPrepMessage.TabIndex = 32;
            this.labelRepPrepMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userControlReplicationPreparation01CreateReplPublTable
            // 
            this.userControlReplicationPreparation01CreateReplPublTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlReplicationPreparation01CreateReplPublTable.ErrorMessage = null;
            this.userControlReplicationPreparation01CreateReplPublTable.Location = new System.Drawing.Point(3, 28);
            this.userControlReplicationPreparation01CreateReplPublTable.Name = "userControlReplicationPreparation01CreateReplPublTable";
            this.userControlReplicationPreparation01CreateReplPublTable.Size = new System.Drawing.Size(616, 26);
            this.userControlReplicationPreparation01CreateReplPublTable.TabIndex = 33;
            // 
            // tabPageEuDsgvo
            // 
            this.tabPageEuDsgvo.Controls.Add(this.splitContainerEuDsgvo);
            this.tabPageEuDsgvo.Controls.Add(this.labelEuDsgvo);
            this.tabPageEuDsgvo.ImageIndex = 5;
            this.tabPageEuDsgvo.Location = new System.Drawing.Point(4, 23);
            this.tabPageEuDsgvo.Name = "tabPageEuDsgvo";
            this.tabPageEuDsgvo.Size = new System.Drawing.Size(863, 525);
            this.tabPageEuDsgvo.TabIndex = 5;
            this.tabPageEuDsgvo.Text = "Data protection";
            this.tabPageEuDsgvo.UseVisualStyleBackColor = true;
            // 
            // splitContainerEuDsgvo
            // 
            this.splitContainerEuDsgvo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerEuDsgvo.Location = new System.Drawing.Point(0, 32);
            this.splitContainerEuDsgvo.Name = "splitContainerEuDsgvo";
            // 
            // splitContainerEuDsgvo.Panel1
            // 
            this.splitContainerEuDsgvo.Panel1.Controls.Add(this.tableLayoutPanelEuDsgvo);
            // 
            // splitContainerEuDsgvo.Panel2
            // 
            this.splitContainerEuDsgvo.Panel2.Controls.Add(this.tabControlEuDsgvo);
            this.splitContainerEuDsgvo.Size = new System.Drawing.Size(863, 493);
            this.splitContainerEuDsgvo.SplitterDistance = 287;
            this.splitContainerEuDsgvo.TabIndex = 0;
            // 
            // tableLayoutPanelEuDsgvo
            // 
            this.tableLayoutPanelEuDsgvo.ColumnCount = 2;
            this.tableLayoutPanelEuDsgvo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEuDsgvo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEuDsgvo.Controls.Add(this.checkedListBoxEuDsgvo, 0, 1);
            this.tableLayoutPanelEuDsgvo.Controls.Add(this.buttonEuDsgvoListTables, 0, 0);
            this.tableLayoutPanelEuDsgvo.Controls.Add(this.buttonEuDsgvoSelectAll, 0, 2);
            this.tableLayoutPanelEuDsgvo.Controls.Add(this.buttonEuDsgvoSelectNone, 1, 2);
            this.tableLayoutPanelEuDsgvo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEuDsgvo.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelEuDsgvo.Name = "tableLayoutPanelEuDsgvo";
            this.tableLayoutPanelEuDsgvo.RowCount = 3;
            this.tableLayoutPanelEuDsgvo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEuDsgvo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvo.Size = new System.Drawing.Size(287, 493);
            this.tableLayoutPanelEuDsgvo.TabIndex = 0;
            // 
            // checkedListBoxEuDsgvo
            // 
            this.checkedListBoxEuDsgvo.CheckOnClick = true;
            this.tableLayoutPanelEuDsgvo.SetColumnSpan(this.checkedListBoxEuDsgvo, 2);
            this.checkedListBoxEuDsgvo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxEuDsgvo.FormattingEnabled = true;
            this.checkedListBoxEuDsgvo.Location = new System.Drawing.Point(3, 32);
            this.checkedListBoxEuDsgvo.Name = "checkedListBoxEuDsgvo";
            this.checkedListBoxEuDsgvo.Size = new System.Drawing.Size(281, 429);
            this.checkedListBoxEuDsgvo.TabIndex = 0;
            // 
            // buttonEuDsgvoListTables
            // 
            this.tableLayoutPanelEuDsgvo.SetColumnSpan(this.buttonEuDsgvoListTables, 2);
            this.buttonEuDsgvoListTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEuDsgvoListTables.Location = new System.Drawing.Point(3, 3);
            this.buttonEuDsgvoListTables.Name = "buttonEuDsgvoListTables";
            this.buttonEuDsgvoListTables.Size = new System.Drawing.Size(281, 23);
            this.buttonEuDsgvoListTables.TabIndex = 1;
            this.buttonEuDsgvoListTables.Text = "List tables in database";
            this.buttonEuDsgvoListTables.UseVisualStyleBackColor = true;
            this.buttonEuDsgvoListTables.Click += new System.EventHandler(this.buttonEuDsgvoListTables_Click);
            // 
            // buttonEuDsgvoSelectAll
            // 
            this.buttonEuDsgvoSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEuDsgvoSelectAll.Image = global::DiversityWorkbench.Properties.Resources.CheckYes;
            this.buttonEuDsgvoSelectAll.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonEuDsgvoSelectAll.Location = new System.Drawing.Point(3, 467);
            this.buttonEuDsgvoSelectAll.Name = "buttonEuDsgvoSelectAll";
            this.buttonEuDsgvoSelectAll.Size = new System.Drawing.Size(137, 23);
            this.buttonEuDsgvoSelectAll.TabIndex = 2;
            this.buttonEuDsgvoSelectAll.Text = "Select all";
            this.buttonEuDsgvoSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonEuDsgvoSelectAll.UseVisualStyleBackColor = true;
            this.buttonEuDsgvoSelectAll.Click += new System.EventHandler(this.buttonEuDsgvoSelectAll_Click);
            // 
            // buttonEuDsgvoSelectNone
            // 
            this.buttonEuDsgvoSelectNone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEuDsgvoSelectNone.Image = global::DiversityWorkbench.Properties.Resources.CheckNo;
            this.buttonEuDsgvoSelectNone.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonEuDsgvoSelectNone.Location = new System.Drawing.Point(146, 467);
            this.buttonEuDsgvoSelectNone.Name = "buttonEuDsgvoSelectNone";
            this.buttonEuDsgvoSelectNone.Size = new System.Drawing.Size(138, 23);
            this.buttonEuDsgvoSelectNone.TabIndex = 3;
            this.buttonEuDsgvoSelectNone.Text = "Select none";
            this.buttonEuDsgvoSelectNone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonEuDsgvoSelectNone.UseVisualStyleBackColor = true;
            this.buttonEuDsgvoSelectNone.Click += new System.EventHandler(this.buttonEuDsgvoSelectNone_Click);
            // 
            // tabControlEuDsgvo
            // 
            this.tabControlEuDsgvo.Controls.Add(this.tabPageEuDsgvoScript);
            this.tabControlEuDsgvo.Controls.Add(this.tabPageEuDsgvoScriptTriggerEtc);
            this.tabControlEuDsgvo.Controls.Add(this.tabPageEuDsgvoInfoURL);
            this.tabControlEuDsgvo.Controls.Add(this.tabPageEuDsgvoRemoveUser);
            this.tabControlEuDsgvo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEuDsgvo.ImageList = this.imageList;
            this.tabControlEuDsgvo.Location = new System.Drawing.Point(0, 0);
            this.tabControlEuDsgvo.Name = "tabControlEuDsgvo";
            this.tabControlEuDsgvo.SelectedIndex = 0;
            this.tabControlEuDsgvo.Size = new System.Drawing.Size(572, 493);
            this.tabControlEuDsgvo.TabIndex = 0;
            // 
            // tabPageEuDsgvoScript
            // 
            this.tabPageEuDsgvoScript.Controls.Add(this.tableLayoutPanelEuDsgvoScript);
            this.tabPageEuDsgvoScript.ImageIndex = 6;
            this.tabPageEuDsgvoScript.Location = new System.Drawing.Point(4, 23);
            this.tabPageEuDsgvoScript.Name = "tabPageEuDsgvoScript";
            this.tabPageEuDsgvoScript.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEuDsgvoScript.Size = new System.Drawing.Size(564, 466);
            this.tabPageEuDsgvoScript.TabIndex = 0;
            this.tabPageEuDsgvoScript.Text = "Script";
            this.tabPageEuDsgvoScript.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelEuDsgvoScript
            // 
            this.tableLayoutPanelEuDsgvoScript.ColumnCount = 5;
            this.tableLayoutPanelEuDsgvoScript.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoScript.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEuDsgvoScript.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoScript.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoScript.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoScript.Controls.Add(this.buttonEuDsgvoScriptCreate, 0, 2);
            this.tableLayoutPanelEuDsgvoScript.Controls.Add(this.textBoxEuDsgvoScript, 0, 3);
            this.tableLayoutPanelEuDsgvoScript.Controls.Add(this.buttonEuDsgvoScriptSave, 4, 4);
            this.tableLayoutPanelEuDsgvoScript.Controls.Add(this.textBoxEuDsgvoScriptFile, 1, 4);
            this.tableLayoutPanelEuDsgvoScript.Controls.Add(this.buttonEuDsgvoScriptFolder, 0, 4);
            this.tableLayoutPanelEuDsgvoScript.Controls.Add(this.radioButtonEuDsgvoScriptTriggerOld, 0, 0);
            this.tableLayoutPanelEuDsgvoScript.Controls.Add(this.radioButtonEuDsgvoScriptTriggerNew, 2, 0);
            this.tableLayoutPanelEuDsgvoScript.Controls.Add(this.checkBoxEuDsgvoScriptTriggerNewVersion, 2, 1);
            this.tableLayoutPanelEuDsgvoScript.Controls.Add(this.comboBoxEuDsgvoScriptTriggerNewVersion, 3, 1);
            this.tableLayoutPanelEuDsgvoScript.Controls.Add(this.checkBoxEuDsgvoScriptIncludeBasics, 0, 1);
            this.tableLayoutPanelEuDsgvoScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEuDsgvoScript.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelEuDsgvoScript.Name = "tableLayoutPanelEuDsgvoScript";
            this.tableLayoutPanelEuDsgvoScript.RowCount = 5;
            this.tableLayoutPanelEuDsgvoScript.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoScript.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoScript.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoScript.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEuDsgvoScript.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoScript.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEuDsgvoScript.Size = new System.Drawing.Size(558, 460);
            this.tableLayoutPanelEuDsgvoScript.TabIndex = 0;
            // 
            // buttonEuDsgvoScriptCreate
            // 
            this.tableLayoutPanelEuDsgvoScript.SetColumnSpan(this.buttonEuDsgvoScriptCreate, 5);
            this.buttonEuDsgvoScriptCreate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEuDsgvoScriptCreate.Location = new System.Drawing.Point(3, 47);
            this.buttonEuDsgvoScriptCreate.Name = "buttonEuDsgvoScriptCreate";
            this.buttonEuDsgvoScriptCreate.Size = new System.Drawing.Size(552, 42);
            this.buttonEuDsgvoScriptCreate.TabIndex = 0;
            this.buttonEuDsgvoScriptCreate.Text = "Create script for the selected tables to replace user information in logging colu" +
    "mns with an ID from the table UserProxy";
            this.buttonEuDsgvoScriptCreate.UseVisualStyleBackColor = true;
            this.buttonEuDsgvoScriptCreate.Click += new System.EventHandler(this.buttonEuDsgvoScriptCreate_Click);
            // 
            // textBoxEuDsgvoScript
            // 
            this.tableLayoutPanelEuDsgvoScript.SetColumnSpan(this.textBoxEuDsgvoScript, 5);
            this.textBoxEuDsgvoScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEuDsgvoScript.Location = new System.Drawing.Point(3, 95);
            this.textBoxEuDsgvoScript.Multiline = true;
            this.textBoxEuDsgvoScript.Name = "textBoxEuDsgvoScript";
            this.textBoxEuDsgvoScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxEuDsgvoScript.Size = new System.Drawing.Size(552, 333);
            this.textBoxEuDsgvoScript.TabIndex = 2;
            // 
            // buttonEuDsgvoScriptSave
            // 
            this.buttonEuDsgvoScriptSave.Location = new System.Drawing.Point(480, 434);
            this.buttonEuDsgvoScriptSave.Name = "buttonEuDsgvoScriptSave";
            this.buttonEuDsgvoScriptSave.Size = new System.Drawing.Size(75, 23);
            this.buttonEuDsgvoScriptSave.TabIndex = 1;
            this.buttonEuDsgvoScriptSave.Text = "Save script";
            this.buttonEuDsgvoScriptSave.UseVisualStyleBackColor = true;
            this.buttonEuDsgvoScriptSave.Click += new System.EventHandler(this.buttonEuDsgvoScriptSave_Click);
            // 
            // textBoxEuDsgvoScriptFile
            // 
            this.tableLayoutPanelEuDsgvoScript.SetColumnSpan(this.textBoxEuDsgvoScriptFile, 3);
            this.textBoxEuDsgvoScriptFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEuDsgvoScriptFile.Location = new System.Drawing.Point(32, 434);
            this.textBoxEuDsgvoScriptFile.Name = "textBoxEuDsgvoScriptFile";
            this.textBoxEuDsgvoScriptFile.Size = new System.Drawing.Size(442, 20);
            this.textBoxEuDsgvoScriptFile.TabIndex = 3;
            // 
            // buttonEuDsgvoScriptFolder
            // 
            this.buttonEuDsgvoScriptFolder.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.buttonEuDsgvoScriptFolder.Location = new System.Drawing.Point(3, 434);
            this.buttonEuDsgvoScriptFolder.Name = "buttonEuDsgvoScriptFolder";
            this.buttonEuDsgvoScriptFolder.Size = new System.Drawing.Size(23, 23);
            this.buttonEuDsgvoScriptFolder.TabIndex = 4;
            this.buttonEuDsgvoScriptFolder.UseVisualStyleBackColor = true;
            this.buttonEuDsgvoScriptFolder.Click += new System.EventHandler(this.buttonEuDsgvoScriptFolder_Click);
            // 
            // radioButtonEuDsgvoScriptTriggerOld
            // 
            this.radioButtonEuDsgvoScriptTriggerOld.AutoSize = true;
            this.radioButtonEuDsgvoScriptTriggerOld.Checked = true;
            this.tableLayoutPanelEuDsgvoScript.SetColumnSpan(this.radioButtonEuDsgvoScriptTriggerOld, 2);
            this.radioButtonEuDsgvoScriptTriggerOld.Location = new System.Drawing.Point(3, 0);
            this.radioButtonEuDsgvoScriptTriggerOld.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.radioButtonEuDsgvoScriptTriggerOld.Name = "radioButtonEuDsgvoScriptTriggerOld";
            this.radioButtonEuDsgvoScriptTriggerOld.Size = new System.Drawing.Size(212, 17);
            this.radioButtonEuDsgvoScriptTriggerOld.TabIndex = 5;
            this.radioButtonEuDsgvoScriptTriggerOld.TabStop = true;
            this.radioButtonEuDsgvoScriptTriggerOld.Text = "Update old trigger according to DSGVO";
            this.radioButtonEuDsgvoScriptTriggerOld.UseVisualStyleBackColor = true;
            this.radioButtonEuDsgvoScriptTriggerOld.Visible = false;
            // 
            // radioButtonEuDsgvoScriptTriggerNew
            // 
            this.radioButtonEuDsgvoScriptTriggerNew.AutoSize = true;
            this.tableLayoutPanelEuDsgvoScript.SetColumnSpan(this.radioButtonEuDsgvoScriptTriggerNew, 3);
            this.radioButtonEuDsgvoScriptTriggerNew.Location = new System.Drawing.Point(291, 0);
            this.radioButtonEuDsgvoScriptTriggerNew.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.radioButtonEuDsgvoScriptTriggerNew.Name = "radioButtonEuDsgvoScriptTriggerNew";
            this.radioButtonEuDsgvoScriptTriggerNew.Size = new System.Drawing.Size(214, 17);
            this.radioButtonEuDsgvoScriptTriggerNew.TabIndex = 6;
            this.radioButtonEuDsgvoScriptTriggerNew.Text = "Create new trigger according to DSGVO";
            this.radioButtonEuDsgvoScriptTriggerNew.UseVisualStyleBackColor = true;
            this.radioButtonEuDsgvoScriptTriggerNew.Visible = false;
            this.radioButtonEuDsgvoScriptTriggerNew.CheckedChanged += new System.EventHandler(this.radioButtonEuDsgvoScriptTriggerNew_CheckedChanged);
            // 
            // checkBoxEuDsgvoScriptTriggerNewVersion
            // 
            this.checkBoxEuDsgvoScriptTriggerNewVersion.AutoSize = true;
            this.checkBoxEuDsgvoScriptTriggerNewVersion.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxEuDsgvoScriptTriggerNewVersion.Enabled = false;
            this.checkBoxEuDsgvoScriptTriggerNewVersion.Location = new System.Drawing.Point(294, 20);
            this.checkBoxEuDsgvoScriptTriggerNewVersion.Margin = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.checkBoxEuDsgvoScriptTriggerNewVersion.Name = "checkBoxEuDsgvoScriptTriggerNewVersion";
            this.checkBoxEuDsgvoScriptTriggerNewVersion.Size = new System.Drawing.Size(111, 21);
            this.checkBoxEuDsgvoScriptTriggerNewVersion.TabIndex = 7;
            this.checkBoxEuDsgvoScriptTriggerNewVersion.Text = "Use version table:";
            this.checkBoxEuDsgvoScriptTriggerNewVersion.UseVisualStyleBackColor = true;
            this.checkBoxEuDsgvoScriptTriggerNewVersion.Visible = false;
            this.checkBoxEuDsgvoScriptTriggerNewVersion.CheckedChanged += new System.EventHandler(this.checkBoxEuDsgvoScriptTriggerNewVersion_CheckedChanged);
            // 
            // comboBoxEuDsgvoScriptTriggerNewVersion
            // 
            this.tableLayoutPanelEuDsgvoScript.SetColumnSpan(this.comboBoxEuDsgvoScriptTriggerNewVersion, 2);
            this.comboBoxEuDsgvoScriptTriggerNewVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxEuDsgvoScriptTriggerNewVersion.DropDownWidth = 300;
            this.comboBoxEuDsgvoScriptTriggerNewVersion.Enabled = false;
            this.comboBoxEuDsgvoScriptTriggerNewVersion.FormattingEnabled = true;
            this.comboBoxEuDsgvoScriptTriggerNewVersion.Location = new System.Drawing.Point(405, 20);
            this.comboBoxEuDsgvoScriptTriggerNewVersion.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxEuDsgvoScriptTriggerNewVersion.Name = "comboBoxEuDsgvoScriptTriggerNewVersion";
            this.comboBoxEuDsgvoScriptTriggerNewVersion.Size = new System.Drawing.Size(150, 21);
            this.comboBoxEuDsgvoScriptTriggerNewVersion.TabIndex = 8;
            this.comboBoxEuDsgvoScriptTriggerNewVersion.Visible = false;
            this.comboBoxEuDsgvoScriptTriggerNewVersion.DropDown += new System.EventHandler(this.comboBoxEuDsgvoScriptTriggerNewVersion_DropDown);
            // 
            // checkBoxEuDsgvoScriptIncludeBasics
            // 
            this.checkBoxEuDsgvoScriptIncludeBasics.AutoSize = true;
            this.checkBoxEuDsgvoScriptIncludeBasics.Checked = true;
            this.checkBoxEuDsgvoScriptIncludeBasics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelEuDsgvoScript.SetColumnSpan(this.checkBoxEuDsgvoScriptIncludeBasics, 2);
            this.checkBoxEuDsgvoScriptIncludeBasics.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxEuDsgvoScriptIncludeBasics.Location = new System.Drawing.Point(3, 20);
            this.checkBoxEuDsgvoScriptIncludeBasics.Name = "checkBoxEuDsgvoScriptIncludeBasics";
            this.checkBoxEuDsgvoScriptIncludeBasics.Size = new System.Drawing.Size(126, 21);
            this.checkBoxEuDsgvoScriptIncludeBasics.TabIndex = 9;
            this.checkBoxEuDsgvoScriptIncludeBasics.Text = "Include basic objects";
            this.toolTip.SetToolTip(this.checkBoxEuDsgvoScriptIncludeBasics, "Include additional columns in table UserProxy, function UserID etc.");
            this.checkBoxEuDsgvoScriptIncludeBasics.UseVisualStyleBackColor = true;
            // 
            // tabPageEuDsgvoScriptTriggerEtc
            // 
            this.tabPageEuDsgvoScriptTriggerEtc.Controls.Add(this.tableLayoutPanelEuDsgvoScriptForTriggerEtc);
            this.tabPageEuDsgvoScriptTriggerEtc.ImageIndex = 6;
            this.tabPageEuDsgvoScriptTriggerEtc.Location = new System.Drawing.Point(4, 23);
            this.tabPageEuDsgvoScriptTriggerEtc.Name = "tabPageEuDsgvoScriptTriggerEtc";
            this.tabPageEuDsgvoScriptTriggerEtc.Size = new System.Drawing.Size(564, 466);
            this.tabPageEuDsgvoScriptTriggerEtc.TabIndex = 3;
            this.tabPageEuDsgvoScriptTriggerEtc.Text = "Script for triggers etc.";
            this.tabPageEuDsgvoScriptTriggerEtc.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelEuDsgvoScriptForTriggerEtc
            // 
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.ColumnCount = 7;
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Controls.Add(this.buttonEuDsgvoScriptForTriggerEtcCreateScript, 0, 2);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Controls.Add(this.textBoxEuDsgvoScriptForTriggerEtcScript, 0, 3);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Controls.Add(this.buttonEuDsgvoScriptForTriggerEtcSaveScript, 6, 4);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Controls.Add(this.textBoxEuDsgvoScriptForTriggerEtcFile, 1, 4);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Controls.Add(this.buttonEuDsgvoScriptForTriggerEtcFolder, 0, 4);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Controls.Add(this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions, 3, 0);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Controls.Add(this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures, 2, 0);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Controls.Add(this.labelEuDsgvoScriptForTriggerEtc, 0, 0);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Controls.Add(this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger, 4, 0);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Controls.Add(this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews, 5, 0);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Name = "tableLayoutPanelEuDsgvoScriptForTriggerEtc";
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.RowCount = 5;
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.Size = new System.Drawing.Size(564, 466);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.TabIndex = 1;
            // 
            // buttonEuDsgvoScriptForTriggerEtcCreateScript
            // 
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.SetColumnSpan(this.buttonEuDsgvoScriptForTriggerEtcCreateScript, 7);
            this.buttonEuDsgvoScriptForTriggerEtcCreateScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEuDsgvoScriptForTriggerEtcCreateScript.Location = new System.Drawing.Point(3, 26);
            this.buttonEuDsgvoScriptForTriggerEtcCreateScript.Name = "buttonEuDsgvoScriptForTriggerEtcCreateScript";
            this.buttonEuDsgvoScriptForTriggerEtcCreateScript.Size = new System.Drawing.Size(558, 22);
            this.buttonEuDsgvoScriptForTriggerEtcCreateScript.TabIndex = 0;
            this.buttonEuDsgvoScriptForTriggerEtcCreateScript.Text = "Create script for the selected objects to replace user information in with an ID " +
    "from the table UserProxy";
            this.buttonEuDsgvoScriptForTriggerEtcCreateScript.UseVisualStyleBackColor = true;
            this.buttonEuDsgvoScriptForTriggerEtcCreateScript.Click += new System.EventHandler(this.buttonEuDsgvoScriptForTriggerEtcCreateScript_Click);
            // 
            // textBoxEuDsgvoScriptForTriggerEtcScript
            // 
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.SetColumnSpan(this.textBoxEuDsgvoScriptForTriggerEtcScript, 7);
            this.textBoxEuDsgvoScriptForTriggerEtcScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEuDsgvoScriptForTriggerEtcScript.Location = new System.Drawing.Point(3, 54);
            this.textBoxEuDsgvoScriptForTriggerEtcScript.Multiline = true;
            this.textBoxEuDsgvoScriptForTriggerEtcScript.Name = "textBoxEuDsgvoScriptForTriggerEtcScript";
            this.textBoxEuDsgvoScriptForTriggerEtcScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxEuDsgvoScriptForTriggerEtcScript.Size = new System.Drawing.Size(558, 380);
            this.textBoxEuDsgvoScriptForTriggerEtcScript.TabIndex = 2;
            // 
            // buttonEuDsgvoScriptForTriggerEtcSaveScript
            // 
            this.buttonEuDsgvoScriptForTriggerEtcSaveScript.Location = new System.Drawing.Point(486, 440);
            this.buttonEuDsgvoScriptForTriggerEtcSaveScript.Name = "buttonEuDsgvoScriptForTriggerEtcSaveScript";
            this.buttonEuDsgvoScriptForTriggerEtcSaveScript.Size = new System.Drawing.Size(75, 23);
            this.buttonEuDsgvoScriptForTriggerEtcSaveScript.TabIndex = 1;
            this.buttonEuDsgvoScriptForTriggerEtcSaveScript.Text = "Save script";
            this.buttonEuDsgvoScriptForTriggerEtcSaveScript.UseVisualStyleBackColor = true;
            this.buttonEuDsgvoScriptForTriggerEtcSaveScript.Click += new System.EventHandler(this.buttonEuDsgvoScriptForTriggerEtcSaveScript_Click);
            // 
            // textBoxEuDsgvoScriptForTriggerEtcFile
            // 
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.SetColumnSpan(this.textBoxEuDsgvoScriptForTriggerEtcFile, 5);
            this.textBoxEuDsgvoScriptForTriggerEtcFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEuDsgvoScriptForTriggerEtcFile.Location = new System.Drawing.Point(32, 440);
            this.textBoxEuDsgvoScriptForTriggerEtcFile.Name = "textBoxEuDsgvoScriptForTriggerEtcFile";
            this.textBoxEuDsgvoScriptForTriggerEtcFile.Size = new System.Drawing.Size(448, 20);
            this.textBoxEuDsgvoScriptForTriggerEtcFile.TabIndex = 3;
            // 
            // buttonEuDsgvoScriptForTriggerEtcFolder
            // 
            this.buttonEuDsgvoScriptForTriggerEtcFolder.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.buttonEuDsgvoScriptForTriggerEtcFolder.Location = new System.Drawing.Point(3, 440);
            this.buttonEuDsgvoScriptForTriggerEtcFolder.Name = "buttonEuDsgvoScriptForTriggerEtcFolder";
            this.buttonEuDsgvoScriptForTriggerEtcFolder.Size = new System.Drawing.Size(23, 23);
            this.buttonEuDsgvoScriptForTriggerEtcFolder.TabIndex = 4;
            this.buttonEuDsgvoScriptForTriggerEtcFolder.UseVisualStyleBackColor = true;
            this.buttonEuDsgvoScriptForTriggerEtcFolder.Click += new System.EventHandler(this.buttonEuDsgvoScriptForTriggerEtcFolder_Click);
            // 
            // checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions
            // 
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions.AutoSize = true;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions.Checked = true;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions.Location = new System.Drawing.Point(140, 3);
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions.Name = "checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions";
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions.Size = new System.Drawing.Size(72, 17);
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions.TabIndex = 10;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions.Text = "Functions";
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions.UseVisualStyleBackColor = true;
            // 
            // checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures
            // 
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures.AutoSize = true;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures.Checked = true;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures.Location = new System.Drawing.Point(54, 3);
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures.Name = "checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures";
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures.Size = new System.Drawing.Size(80, 17);
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures.TabIndex = 12;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures.Text = "Procedures";
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures.UseVisualStyleBackColor = true;
            // 
            // labelEuDsgvoScriptForTriggerEtc
            // 
            this.labelEuDsgvoScriptForTriggerEtc.AutoSize = true;
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.SetColumnSpan(this.labelEuDsgvoScriptForTriggerEtc, 2);
            this.labelEuDsgvoScriptForTriggerEtc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEuDsgvoScriptForTriggerEtc.Location = new System.Drawing.Point(3, 0);
            this.labelEuDsgvoScriptForTriggerEtc.Name = "labelEuDsgvoScriptForTriggerEtc";
            this.labelEuDsgvoScriptForTriggerEtc.Size = new System.Drawing.Size(45, 23);
            this.labelEuDsgvoScriptForTriggerEtc.TabIndex = 13;
            this.labelEuDsgvoScriptForTriggerEtc.Text = "Include:";
            this.labelEuDsgvoScriptForTriggerEtc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger
            // 
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger.AutoSize = true;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger.Checked = true;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger.Location = new System.Drawing.Point(218, 3);
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger.Name = "checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger";
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger.Size = new System.Drawing.Size(59, 17);
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger.TabIndex = 9;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger.Text = "Trigger";
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger.UseVisualStyleBackColor = true;
            // 
            // checkBoxEuDsgvoScriptForTriggerEtcIncludeViews
            // 
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews.AutoSize = true;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews.Checked = true;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.SetColumnSpan(this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews, 2);
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews.Location = new System.Drawing.Point(283, 3);
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews.Name = "checkBoxEuDsgvoScriptForTriggerEtcIncludeViews";
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews.Size = new System.Drawing.Size(54, 17);
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews.TabIndex = 11;
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews.Text = "Views";
            this.checkBoxEuDsgvoScriptForTriggerEtcIncludeViews.UseVisualStyleBackColor = true;
            // 
            // tabPageEuDsgvoInfoURL
            // 
            this.tabPageEuDsgvoInfoURL.Controls.Add(this.tableLayoutPanelEuDsgvoInfoURL);
            this.tabPageEuDsgvoInfoURL.ImageIndex = 8;
            this.tabPageEuDsgvoInfoURL.Location = new System.Drawing.Point(4, 23);
            this.tabPageEuDsgvoInfoURL.Name = "tabPageEuDsgvoInfoURL";
            this.tabPageEuDsgvoInfoURL.Size = new System.Drawing.Size(564, 466);
            this.tabPageEuDsgvoInfoURL.TabIndex = 2;
            this.tabPageEuDsgvoInfoURL.Text = "Info site";
            this.tabPageEuDsgvoInfoURL.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelEuDsgvoInfoURL
            // 
            this.tableLayoutPanelEuDsgvoInfoURL.ColumnCount = 2;
            this.tableLayoutPanelEuDsgvoInfoURL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEuDsgvoInfoURL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoInfoURL.Controls.Add(this.labelEuDsgvoInfoURL, 0, 0);
            this.tableLayoutPanelEuDsgvoInfoURL.Controls.Add(this.buttonEuDsgvoInfoURLSetValue, 1, 0);
            this.tableLayoutPanelEuDsgvoInfoURL.Controls.Add(this.linkLabelEuDsgvoInfoURL, 0, 1);
            this.tableLayoutPanelEuDsgvoInfoURL.Controls.Add(this.userControlWebViewEuDsgvoInfoURL, 0, 2);
            this.tableLayoutPanelEuDsgvoInfoURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEuDsgvoInfoURL.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelEuDsgvoInfoURL.Name = "tableLayoutPanelEuDsgvoInfoURL";
            this.tableLayoutPanelEuDsgvoInfoURL.RowCount = 3;
            this.tableLayoutPanelEuDsgvoInfoURL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoInfoURL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoInfoURL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEuDsgvoInfoURL.Size = new System.Drawing.Size(564, 466);
            this.tableLayoutPanelEuDsgvoInfoURL.TabIndex = 0;
            // 
            // labelEuDsgvoInfoURL
            // 
            this.labelEuDsgvoInfoURL.AutoSize = true;
            this.labelEuDsgvoInfoURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEuDsgvoInfoURL.Location = new System.Drawing.Point(3, 0);
            this.labelEuDsgvoInfoURL.Name = "labelEuDsgvoInfoURL";
            this.labelEuDsgvoInfoURL.Size = new System.Drawing.Size(533, 24);
            this.labelEuDsgvoInfoURL.TabIndex = 1;
            this.labelEuDsgvoInfoURL.Text = "Web site containing further information";
            this.labelEuDsgvoInfoURL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonEuDsgvoInfoURLSetValue
            // 
            this.buttonEuDsgvoInfoURLSetValue.FlatAppearance.BorderSize = 0;
            this.buttonEuDsgvoInfoURLSetValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEuDsgvoInfoURLSetValue.Image = global::DiversityWorkbench.Properties.Resources.Browse;
            this.buttonEuDsgvoInfoURLSetValue.Location = new System.Drawing.Point(539, 0);
            this.buttonEuDsgvoInfoURLSetValue.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEuDsgvoInfoURLSetValue.Name = "buttonEuDsgvoInfoURLSetValue";
            this.buttonEuDsgvoInfoURLSetValue.Size = new System.Drawing.Size(25, 24);
            this.buttonEuDsgvoInfoURLSetValue.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonEuDsgvoInfoURLSetValue, "Set website containing further information");
            this.buttonEuDsgvoInfoURLSetValue.UseVisualStyleBackColor = true;
            this.buttonEuDsgvoInfoURLSetValue.Click += new System.EventHandler(this.buttonEuDsgvoInfoURLSetValue_Click);
            // 
            // linkLabelEuDsgvoInfoURL
            // 
            this.linkLabelEuDsgvoInfoURL.AutoSize = true;
            this.tableLayoutPanelEuDsgvoInfoURL.SetColumnSpan(this.linkLabelEuDsgvoInfoURL, 2);
            this.linkLabelEuDsgvoInfoURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabelEuDsgvoInfoURL.Location = new System.Drawing.Point(3, 24);
            this.linkLabelEuDsgvoInfoURL.Name = "linkLabelEuDsgvoInfoURL";
            this.linkLabelEuDsgvoInfoURL.Size = new System.Drawing.Size(558, 13);
            this.linkLabelEuDsgvoInfoURL.TabIndex = 3;
            this.linkLabelEuDsgvoInfoURL.TabStop = true;
            this.linkLabelEuDsgvoInfoURL.Text = "https://...";
            this.linkLabelEuDsgvoInfoURL.TextChanged += new System.EventHandler(this.linkLabelEuDsgvoInfoURL_TextChanged);
            // 
            // userControlWebViewEuDsgvoInfoURL
            // 
            this.userControlWebViewEuDsgvoInfoURL.AllowScripting = false;
            this.tableLayoutPanelEuDsgvoInfoURL.SetColumnSpan(this.userControlWebViewEuDsgvoInfoURL, 2);
            this.userControlWebViewEuDsgvoInfoURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlWebViewEuDsgvoInfoURL.Location = new System.Drawing.Point(0, 37);
            this.userControlWebViewEuDsgvoInfoURL.Margin = new System.Windows.Forms.Padding(0);
            this.userControlWebViewEuDsgvoInfoURL.Name = "userControlWebViewEuDsgvoInfoURL";
            this.userControlWebViewEuDsgvoInfoURL.ScriptErrorsSuppressed = false;
            this.userControlWebViewEuDsgvoInfoURL.Size = new System.Drawing.Size(564, 429);
            this.userControlWebViewEuDsgvoInfoURL.TabIndex = 4;
            this.userControlWebViewEuDsgvoInfoURL.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // tabPageEuDsgvoRemoveUser
            // 
            this.tabPageEuDsgvoRemoveUser.Controls.Add(this.tableLayoutPanelEuDsgvoRemoveUser);
            this.tabPageEuDsgvoRemoveUser.ImageIndex = 7;
            this.tabPageEuDsgvoRemoveUser.Location = new System.Drawing.Point(4, 23);
            this.tabPageEuDsgvoRemoveUser.Name = "tabPageEuDsgvoRemoveUser";
            this.tabPageEuDsgvoRemoveUser.Size = new System.Drawing.Size(564, 466);
            this.tabPageEuDsgvoRemoveUser.TabIndex = 1;
            this.tabPageEuDsgvoRemoveUser.Text = "Remove user";
            this.tabPageEuDsgvoRemoveUser.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelEuDsgvoRemoveUser
            // 
            this.tableLayoutPanelEuDsgvoRemoveUser.ColumnCount = 2;
            this.tableLayoutPanelEuDsgvoRemoveUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEuDsgvoRemoveUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEuDsgvoRemoveUser.Controls.Add(this.labelEuDsgvoRemoveUser, 0, 0);
            this.tableLayoutPanelEuDsgvoRemoveUser.Controls.Add(this.comboBoxEuDsgvoRemoveUser, 1, 0);
            this.tableLayoutPanelEuDsgvoRemoveUser.Controls.Add(this.buttonEuDsgvoRemoveUser, 0, 2);
            this.tableLayoutPanelEuDsgvoRemoveUser.Controls.Add(this.textBoxEuDsgvoRemoveUserInfo, 0, 1);
            this.tableLayoutPanelEuDsgvoRemoveUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEuDsgvoRemoveUser.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelEuDsgvoRemoveUser.Name = "tableLayoutPanelEuDsgvoRemoveUser";
            this.tableLayoutPanelEuDsgvoRemoveUser.RowCount = 3;
            this.tableLayoutPanelEuDsgvoRemoveUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoRemoveUser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEuDsgvoRemoveUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEuDsgvoRemoveUser.Size = new System.Drawing.Size(564, 466);
            this.tableLayoutPanelEuDsgvoRemoveUser.TabIndex = 0;
            // 
            // labelEuDsgvoRemoveUser
            // 
            this.labelEuDsgvoRemoveUser.AutoSize = true;
            this.labelEuDsgvoRemoveUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEuDsgvoRemoveUser.Location = new System.Drawing.Point(3, 0);
            this.labelEuDsgvoRemoveUser.Name = "labelEuDsgvoRemoveUser";
            this.labelEuDsgvoRemoveUser.Size = new System.Drawing.Size(261, 27);
            this.labelEuDsgvoRemoveUser.TabIndex = 0;
            this.labelEuDsgvoRemoveUser.Text = "Select the user of which the name should be removed";
            this.labelEuDsgvoRemoveUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxEuDsgvoRemoveUser
            // 
            this.comboBoxEuDsgvoRemoveUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxEuDsgvoRemoveUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEuDsgvoRemoveUser.FormattingEnabled = true;
            this.comboBoxEuDsgvoRemoveUser.Location = new System.Drawing.Point(270, 3);
            this.comboBoxEuDsgvoRemoveUser.Name = "comboBoxEuDsgvoRemoveUser";
            this.comboBoxEuDsgvoRemoveUser.Size = new System.Drawing.Size(291, 21);
            this.comboBoxEuDsgvoRemoveUser.TabIndex = 1;
            this.comboBoxEuDsgvoRemoveUser.DropDown += new System.EventHandler(this.comboBoxEuDsgvoRemoveUser_DropDown);
            this.comboBoxEuDsgvoRemoveUser.SelectedIndexChanged += new System.EventHandler(this.comboBoxEuDsgvoRemoveUser_SelectedIndexChanged);
            // 
            // buttonEuDsgvoRemoveUser
            // 
            this.tableLayoutPanelEuDsgvoRemoveUser.SetColumnSpan(this.buttonEuDsgvoRemoveUser, 2);
            this.buttonEuDsgvoRemoveUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEuDsgvoRemoveUser.ForeColor = System.Drawing.Color.Red;
            this.buttonEuDsgvoRemoveUser.Image = global::DiversityWorkbench.Properties.Resources.LoginMissing;
            this.buttonEuDsgvoRemoveUser.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonEuDsgvoRemoveUser.Location = new System.Drawing.Point(3, 440);
            this.buttonEuDsgvoRemoveUser.Name = "buttonEuDsgvoRemoveUser";
            this.buttonEuDsgvoRemoveUser.Size = new System.Drawing.Size(558, 23);
            this.buttonEuDsgvoRemoveUser.TabIndex = 2;
            this.buttonEuDsgvoRemoveUser.Text = "Remove the name of the user in the database";
            this.buttonEuDsgvoRemoveUser.UseVisualStyleBackColor = true;
            this.buttonEuDsgvoRemoveUser.Click += new System.EventHandler(this.buttonEuDsgvoRemoveUser_Click);
            // 
            // textBoxEuDsgvoRemoveUserInfo
            // 
            this.tableLayoutPanelEuDsgvoRemoveUser.SetColumnSpan(this.textBoxEuDsgvoRemoveUserInfo, 2);
            this.textBoxEuDsgvoRemoveUserInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEuDsgvoRemoveUserInfo.Location = new System.Drawing.Point(3, 30);
            this.textBoxEuDsgvoRemoveUserInfo.Multiline = true;
            this.textBoxEuDsgvoRemoveUserInfo.Name = "textBoxEuDsgvoRemoveUserInfo";
            this.textBoxEuDsgvoRemoveUserInfo.ReadOnly = true;
            this.textBoxEuDsgvoRemoveUserInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxEuDsgvoRemoveUserInfo.Size = new System.Drawing.Size(558, 404);
            this.textBoxEuDsgvoRemoveUserInfo.TabIndex = 3;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Dictionary.ICO");
            this.imageList.Images.SetKeyName(1, "History.ico");
            this.imageList.Images.SetKeyName(2, "Delete.ico");
            this.imageList.Images.SetKeyName(3, "Function.ico");
            this.imageList.Images.SetKeyName(4, "Synchronize.ico");
            this.imageList.Images.SetKeyName(5, "Paragraf.ico");
            this.imageList.Images.SetKeyName(6, "List.ico");
            this.imageList.Images.SetKeyName(7, "LoginMissing.ico");
            this.imageList.Images.SetKeyName(8, "Browse.ico");
            this.imageList.Images.SetKeyName(9, "Manual.ico");
            this.imageList.Images.SetKeyName(10, "LogSave.ico");
            // 
            // labelEuDsgvo
            // 
            this.labelEuDsgvo.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEuDsgvo.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelEuDsgvo.Location = new System.Drawing.Point(0, 0);
            this.labelEuDsgvo.Margin = new System.Windows.Forms.Padding(3);
            this.labelEuDsgvo.Name = "labelEuDsgvo";
            this.labelEuDsgvo.Size = new System.Drawing.Size(863, 32);
            this.labelEuDsgvo.TabIndex = 1;
            this.labelEuDsgvo.Text = "Regulation (EU) 2016/679 of the European Parliament and of the Council on the pro" +
    "tection of natural persons with regard to the processing of personal data and on" +
    " the free movement of such data";
            this.labelEuDsgvo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(844, 0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(25, 23);
            this.buttonFeedback.TabIndex = 1;
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // tableLayoutPanelLogTable
            // 
            this.tableLayoutPanelLogTable.ColumnCount = 4;
            this.tableLayoutPanelLogTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLogTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogTable.Controls.Add(this.buttonLogTableShowSQL, 0, 0);
            this.tableLayoutPanelLogTable.Controls.Add(this.checkBoxKeepOldLogTable, 2, 1);
            this.tableLayoutPanelLogTable.Controls.Add(this.checkBoxAddVersion, 1, 1);
            this.tableLayoutPanelLogTable.Controls.Add(this.checkBoxLogTableDSGVO, 1, 0);
            this.tableLayoutPanelLogTable.Controls.Add(this.buttonLogTableCreate, 3, 0);
            this.tableLayoutPanelLogTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLogTable.Location = new System.Drawing.Point(0, 4);
            this.tableLayoutPanelLogTable.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelLogTable.Name = "tableLayoutPanelLogTable";
            this.tableLayoutPanelLogTable.RowCount = 2;
            this.tableLayoutPanelLogTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLogTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLogTable.Size = new System.Drawing.Size(617, 48);
            this.tableLayoutPanelLogTable.TabIndex = 38;
            // 
            // FormDatabaseTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 552);
            this.Controls.Add(this.buttonFeedback);
            this.Controls.Add(this.tabControlMain);
            this.helpProvider.SetHelpKeyword(this, "Database tools");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this, "Database tools");
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDatabaseTool";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database tools";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageDescription.ResumeLayout(false);
            this.splitContainerDescription.Panel1.ResumeLayout(false);
            this.splitContainerDescription.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDescription)).EndInit();
            this.splitContainerDescription.ResumeLayout(false);
            this.tableLayoutPanelDescription.ResumeLayout(false);
            this.tableLayoutPanelDescription.PerformLayout();
            this.tabPageLogging.ResumeLayout(false);
            this.splitContainerLogging.Panel1.ResumeLayout(false);
            this.splitContainerLogging.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogging)).EndInit();
            this.splitContainerLogging.ResumeLayout(false);
            this.tableLayoutPanelLogging.ResumeLayout(false);
            this.tableLayoutPanelLogging.PerformLayout();
            this.tabControlLoggingDefinitions.ResumeLayout(false);
            this.tabPageTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).EndInit();
            this.panelTableButtons.ResumeLayout(false);
            this.panelTableButtons.PerformLayout();
            this.tabPageLogTable.ResumeLayout(false);
            this.tabPageLogTable.PerformLayout();
            this.panelLogTableButtons.ResumeLayout(false);
            this.tabPageInsertTrigger.ResumeLayout(false);
            this.tabPageInsertTrigger.PerformLayout();
            this.panelInsertTriggerButtons.ResumeLayout(false);
            this.panelInsertTriggerButtons.PerformLayout();
            this.tabPageUpdateTrigger.ResumeLayout(false);
            this.tabPageUpdateTrigger.PerformLayout();
            this.panelUpdateTriggerButtons.ResumeLayout(false);
            this.panelUpdateTriggerButtons.PerformLayout();
            this.tabPageDeleteTrigger.ResumeLayout(false);
            this.tabPageDeleteTrigger.PerformLayout();
            this.panelDeleteTriggerButtons.ResumeLayout(false);
            this.panelDeleteTriggerButtons.PerformLayout();
            this.tabPageProcSetVersion.ResumeLayout(false);
            this.splitContainerProcVersion.Panel1.ResumeLayout(false);
            this.splitContainerProcVersion.Panel1.PerformLayout();
            this.splitContainerProcVersion.Panel2.ResumeLayout(false);
            this.splitContainerProcVersion.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProcVersion)).EndInit();
            this.splitContainerProcVersion.ResumeLayout(false);
            this.panelProcSetVersionButtons.ResumeLayout(false);
            this.tabPageClearLog.ResumeLayout(false);
            this.splitContainerClearLog.Panel1.ResumeLayout(false);
            this.splitContainerClearLog.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerClearLog)).EndInit();
            this.splitContainerClearLog.ResumeLayout(false);
            this.tableLayoutPanelClearLog.ResumeLayout(false);
            this.tableLayoutPanelClearLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClearLog)).EndInit();
            this.tabPageSaveLog.ResumeLayout(false);
            this.tabControlSaveLog.ResumeLayout(false);
            this.tabPageSaveToLogDB.ResumeLayout(false);
            this.splitContainerSaveLog.Panel1.ResumeLayout(false);
            this.splitContainerSaveLog.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSaveLog)).EndInit();
            this.splitContainerSaveLog.ResumeLayout(false);
            this.tableLayoutPanelSaveLog.ResumeLayout(false);
            this.tableLayoutPanelSaveLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSaveLog)).EndInit();
            this.tabPageProcedures.ResumeLayout(false);
            this.tableLayoutPanelProcedures.ResumeLayout(false);
            this.tableLayoutPanelProcedures.PerformLayout();
            this.tableLayoutPanelProcedureResult.ResumeLayout(false);
            this.tableLayoutPanelProcedureResult.PerformLayout();
            this.splitContainerProcedureResult.Panel1.ResumeLayout(false);
            this.splitContainerProcedureResult.Panel2.ResumeLayout(false);
            this.splitContainerProcedureResult.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProcedureResult)).EndInit();
            this.splitContainerProcedureResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcedureResult)).EndInit();
            this.tableLayoutPanelProcedureParameter.ResumeLayout(false);
            this.tableLayoutPanelProcedureParameter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).EndInit();
            this.tabPageRowGUID.ResumeLayout(false);
            this.splitContainerReplication.Panel1.ResumeLayout(false);
            this.splitContainerReplication.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerReplication)).EndInit();
            this.splitContainerReplication.ResumeLayout(false);
            this.tableLayoutPanelReplicationTableList.ResumeLayout(false);
            this.tableLayoutPanelReplicationTableList.PerformLayout();
            this.tabControlReplication.ResumeLayout(false);
            this.tabPageRepPrepScript.ResumeLayout(false);
            this.tableLayoutPanelRepPrepScript.ResumeLayout(false);
            this.tableLayoutPanelRepPrepScript.PerformLayout();
            this.tabPageRepPrep.ResumeLayout(false);
            this.tableLayoutPanelReplicationPreparationSteps.ResumeLayout(false);
            this.tableLayoutPanelReplicationPreparationSteps.PerformLayout();
            this.tabPageEuDsgvo.ResumeLayout(false);
            this.splitContainerEuDsgvo.Panel1.ResumeLayout(false);
            this.splitContainerEuDsgvo.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerEuDsgvo)).EndInit();
            this.splitContainerEuDsgvo.ResumeLayout(false);
            this.tableLayoutPanelEuDsgvo.ResumeLayout(false);
            this.tabControlEuDsgvo.ResumeLayout(false);
            this.tabPageEuDsgvoScript.ResumeLayout(false);
            this.tableLayoutPanelEuDsgvoScript.ResumeLayout(false);
            this.tableLayoutPanelEuDsgvoScript.PerformLayout();
            this.tabPageEuDsgvoScriptTriggerEtc.ResumeLayout(false);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.ResumeLayout(false);
            this.tableLayoutPanelEuDsgvoScriptForTriggerEtc.PerformLayout();
            this.tabPageEuDsgvoInfoURL.ResumeLayout(false);
            this.tableLayoutPanelEuDsgvoInfoURL.ResumeLayout(false);
            this.tableLayoutPanelEuDsgvoInfoURL.PerformLayout();
            this.tabPageEuDsgvoRemoveUser.ResumeLayout(false);
            this.tableLayoutPanelEuDsgvoRemoveUser.ResumeLayout(false);
            this.tableLayoutPanelEuDsgvoRemoveUser.PerformLayout();
            this.tableLayoutPanelLogTable.ResumeLayout(false);
            this.tableLayoutPanelLogTable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageDescription;
        private System.Windows.Forms.TabPage tabPageLogging;
        private System.Windows.Forms.SplitContainer splitContainerDescription;
        private System.Windows.Forms.TreeView treeViewDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDescription;
        private System.Windows.Forms.Label labelObjectName;
        private System.Windows.Forms.TextBox textBoxObjectName;
        private System.Windows.Forms.Label labelObjectType;
        private System.Windows.Forms.TextBox textBoxObjectDescription;
        private System.Windows.Forms.TextBox textBoxObjectType;
        private System.Windows.Forms.Label labelObjectDefinition;
        private System.Windows.Forms.Label labelObjectDescription;
        private System.Windows.Forms.TextBox textBoxObjectDefinition;
        private System.Windows.Forms.Label labelObjectDatatype;
        private System.Windows.Forms.TextBox textBoxObjectDatatype;
        private System.Windows.Forms.TextBox textBoxObjectAction;
        private System.Windows.Forms.Label labelObjectAction;
        private System.Windows.Forms.SplitContainer splitContainerLogging;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLogging;
        private System.Windows.Forms.Button buttonListTablesForTrigger;
        private System.Windows.Forms.Label labelLoggingTableList;
        private System.Windows.Forms.Label labelLoggingVersionMaster;
        private System.Windows.Forms.ComboBox comboBoxLoggingVersionMasterTable;
        private System.Windows.Forms.ListBox listBoxLoggingTables;
        private System.Windows.Forms.TabControl tabControlLoggingDefinitions;
        private System.Windows.Forms.TabPage tabPageTable;
        private System.Windows.Forms.Panel panelTableButtons;
        private System.Windows.Forms.CheckBox checkBoxAddInsertColumns;
        private System.Windows.Forms.Button buttonAttachLogColumns;
        private System.Windows.Forms.DataGridView dataGridViewTable;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnOK;
        private System.Windows.Forms.TabPage tabPageLogTable;
        private System.Windows.Forms.TextBox textBoxLogTable;
        private System.Windows.Forms.Panel panelLogTableButtons;
        private System.Windows.Forms.CheckBox checkBoxAddVersion;
        private System.Windows.Forms.CheckBox checkBoxKeepOldLogTable;
        private System.Windows.Forms.Button buttonLogTableCreate;
        private System.Windows.Forms.Button buttonLogTableShowSQL;
        private System.Windows.Forms.TabPage tabPageInsertTrigger;
        private System.Windows.Forms.TextBox textBoxInsertTriggerNew;
        private System.Windows.Forms.Panel panelInsertTriggerButtons;
        private System.Windows.Forms.CheckBox checkBoxInsertTriggerAddVersion;
        private System.Windows.Forms.Button buttonInsertTriggerCreate;
        private System.Windows.Forms.Button buttonInsertTriggerShowSQL;
        private System.Windows.Forms.TextBox textBoxInsertTrigger;
        private System.Windows.Forms.TabPage tabPageUpdateTrigger;
        private System.Windows.Forms.TextBox textBoxUpdateTriggerNew;
        private System.Windows.Forms.Panel panelUpdateTriggerButtons;
        private System.Windows.Forms.CheckBox checkBoxUpdateTriggerAddVersion;
        private System.Windows.Forms.Button buttonUpdateTriggerCreate;
        private System.Windows.Forms.Button buttonUpdateTriggerShowSQL;
        private System.Windows.Forms.TextBox textBoxUpdateTrigger;
        private System.Windows.Forms.TabPage tabPageDeleteTrigger;
        private System.Windows.Forms.TextBox textBoxDeleteTriggerNew;
        private System.Windows.Forms.Panel panelDeleteTriggerButtons;
        private System.Windows.Forms.CheckBox checkBoxDeleteTriggerAddVersion;
        private System.Windows.Forms.Button buttonDeleteTriggerCreate;
        private System.Windows.Forms.Button buttonDeleteTriggerShowSql;
        private System.Windows.Forms.TextBox textBoxDeleteTrigger;
        private System.Windows.Forms.TabPage tabPageProcSetVersion;
        private System.Windows.Forms.TextBox textBoxProcSetVersionNew;
        private System.Windows.Forms.TextBox textBoxProcSetVersion;
        private System.Windows.Forms.Panel panelProcSetVersionButtons;
        private System.Windows.Forms.Button buttonProcSetVersionCreate;
        private System.Windows.Forms.Button buttonProcSetVersionShowSql;
        private System.Windows.Forms.TabPage tabPageProcedures;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProcedures;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProcedureResult;
        private System.Windows.Forms.Label labelProcedureResult;
        private System.Windows.Forms.SplitContainer splitContainerProcedureResult;
        private System.Windows.Forms.DataGridView dataGridViewProcedureResult;
        private System.Windows.Forms.TextBox textBoxProcedureResult;
        private System.Windows.Forms.TextBox textBoxProcedureSQL;
        private System.Windows.Forms.Label labelProcedureCall;
        private System.Windows.Forms.TextBox textBoxProcedureDefinition;
        private System.Windows.Forms.TextBox textBoxProcedureDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProcedureParameter;
        private System.Windows.Forms.TextBox textBoxProcedureParameter6;
        private System.Windows.Forms.Label labelProcedureParameter6;
        private System.Windows.Forms.TextBox textBoxProcedureParameter5;
        private System.Windows.Forms.Label labelProcedureParameter5;
        private System.Windows.Forms.Label labelProcedureParameter;
        private System.Windows.Forms.Label labelProcedureParameterValue;
        private System.Windows.Forms.Label labelProcedureParameter1;
        private System.Windows.Forms.Label labelProcedureParameter2;
        private System.Windows.Forms.Label labelProcedureParameter3;
        private System.Windows.Forms.Label labelProcedureParameter4;
        private System.Windows.Forms.TextBox textBoxProcedureParameter1;
        private System.Windows.Forms.TextBox textBoxProcedureParameter2;
        private System.Windows.Forms.TextBox textBoxProcedureParameter3;
        private System.Windows.Forms.TextBox textBoxProcedureParameter4;
        private System.Windows.Forms.Label labelTimeElapsed;
        private System.Windows.Forms.TextBox textBoxProcedureReturns;
        private System.Windows.Forms.Label labelProcedureList;
        private System.Windows.Forms.Button buttonStartDataTransfer;
        private System.Windows.Forms.Label labelTimeout;
        private System.Windows.Forms.Label labelProcedureReturns;
        private System.Windows.Forms.ComboBox comboBoxProcedureList;
        private System.Windows.Forms.TextBox textBoxProcedureType;
        private System.Windows.Forms.Label labelProcedureType;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeout;
        private System.Windows.Forms.TextBox textBoxTimeElapsed;
        private System.Windows.Forms.SplitContainer splitContainerProcVersion;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.TabPage tabPageRowGUID;
        private System.Windows.Forms.SplitContainer splitContainerReplication;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReplicationTableList;
        private System.Windows.Forms.Button buttonReplicationListTables;
        private System.Windows.Forms.Label labelReplicationTableList;
        private System.Windows.Forms.TabControl tabControlReplication;
        private System.Windows.Forms.TabPage tabPageRepPrep;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReplicationPreparationSteps;
        private System.Windows.Forms.Button buttonStartReplicationPreparation;
        private System.Windows.Forms.Label labelRepPrepHeader;
        private UserControls.UserControlReplicationPreparation userControlReplicationPreparation02AddRowGUID;
        private UserControls.UserControlReplicationPreparation userControlReplicationPreparation03CreateDefaults;
        private UserControls.UserControlReplicationPreparation userControlReplicationPreparation04CreateTemporaryTable;
        private UserControls.UserControlReplicationPreparation userControlReplicationPreparation05ReadData;
        private UserControls.UserControlReplicationPreparation userControlReplicationPreparation06DeactivateUpdateTrigger;
        private UserControls.UserControlReplicationPreparation userControlReplicationPreparation07WriteRowGUID;
        private UserControls.UserControlReplicationPreparation userControlReplicationPreparation08WriteDate;
        private UserControls.UserControlReplicationPreparation userControlReplicationPreparation09ActivateUpdateTrigger;
        private UserControls.UserControlReplicationPreparation userControlReplicationPreparation10DeleteTempTable;
        private System.Windows.Forms.Label labelRepPrepMessage;
        private UserControls.UserControlReplicationPreparation userControlReplicationPreparation01CreateReplPublTable;
        private System.Windows.Forms.CheckedListBox checkedListBoxRepPrep;
        private System.Windows.Forms.Button buttonRepPrepAll;
        private System.Windows.Forms.Button buttonRepPrepNone;
        private System.Windows.Forms.TabPage tabPageRepPrepScript;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRepPrepScript;
        private System.Windows.Forms.TextBox textBoxRepPrepScript;
        private System.Windows.Forms.Button buttonRepPrepScript;
        private System.Windows.Forms.Button buttonRepPrepScriptSave;
        private System.Windows.Forms.Button buttonRepPrepScriptFile;
        private System.Windows.Forms.TextBox textBoxRepPrepScriptFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.CheckBox checkBoxAddRowGUID;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.TabPage tabPageClearLog;
        private System.Windows.Forms.SplitContainer splitContainerClearLog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelClearLog;
        private System.Windows.Forms.Button buttonClearLogListTables;
        private System.Windows.Forms.Label labelClearLogTableList;
        private System.Windows.Forms.CheckedListBox checkedListBoxClearLog;
        private System.Windows.Forms.Button buttonClearLogSelectAll;
        private System.Windows.Forms.Button buttonClearLogSelectNone;
        private System.Windows.Forms.Button buttonClearLogStart;
        private System.Windows.Forms.DataGridView dataGridViewClearLog;
        private System.Windows.Forms.Button buttonObjectDescriptionSQLadd;
        private System.Windows.Forms.Button buttonObjectDescriptionSQLupdate;
        private System.Windows.Forms.Button buttonObjectDescriptionSQLany;
        private System.Windows.Forms.TabPage tabPageEuDsgvo;
        private System.Windows.Forms.SplitContainer splitContainerEuDsgvo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEuDsgvo;
        private System.Windows.Forms.CheckedListBox checkedListBoxEuDsgvo;
        private System.Windows.Forms.Button buttonEuDsgvoListTables;
        private System.Windows.Forms.Button buttonEuDsgvoSelectAll;
        private System.Windows.Forms.Button buttonEuDsgvoSelectNone;
        private System.Windows.Forms.TabControl tabControlEuDsgvo;
        private System.Windows.Forms.TabPage tabPageEuDsgvoScript;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEuDsgvoScript;
        private System.Windows.Forms.Button buttonEuDsgvoScriptCreate;
        private System.Windows.Forms.TextBox textBoxEuDsgvoScript;
        private System.Windows.Forms.Button buttonEuDsgvoScriptSave;
        private System.Windows.Forms.TextBox textBoxEuDsgvoScriptFile;
        private System.Windows.Forms.Button buttonEuDsgvoScriptFolder;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TabPage tabPageEuDsgvoRemoveUser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEuDsgvoRemoveUser;
        private System.Windows.Forms.Label labelEuDsgvoRemoveUser;
        private System.Windows.Forms.ComboBox comboBoxEuDsgvoRemoveUser;
        private System.Windows.Forms.Button buttonEuDsgvoRemoveUser;
        private System.Windows.Forms.TextBox textBoxEuDsgvoRemoveUserInfo;
        private System.Windows.Forms.Label labelEuDsgvo;
        private System.Windows.Forms.TabPage tabPageEuDsgvoInfoURL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEuDsgvoInfoURL;
        private System.Windows.Forms.Label labelEuDsgvoInfoURL;
        private System.Windows.Forms.Button buttonEuDsgvoInfoURLSetValue;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.LinkLabel linkLabelEuDsgvoInfoURL;
        private System.Windows.Forms.RadioButton radioButtonEuDsgvoScriptTriggerOld;
        private System.Windows.Forms.RadioButton radioButtonEuDsgvoScriptTriggerNew;
        private System.Windows.Forms.CheckBox checkBoxUpdateTriggerAddDsgvo;
        private System.Windows.Forms.CheckBox checkBoxDeleteTriggerAddDsgvo;
        private System.Windows.Forms.CheckBox checkBoxRepPrepDsgvo;
        private System.Windows.Forms.CheckBox checkBoxProcSetVersionDsgvo;
        private System.Windows.Forms.CheckBox checkBoxEuDsgvoScriptTriggerNewVersion;
        private System.Windows.Forms.ComboBox comboBoxEuDsgvoScriptTriggerNewVersion;
        private System.Windows.Forms.CheckBox checkBoxEuDsgvoScriptIncludeBasics;
        private System.Windows.Forms.TabPage tabPageEuDsgvoScriptTriggerEtc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEuDsgvoScriptForTriggerEtc;
        private System.Windows.Forms.Button buttonEuDsgvoScriptForTriggerEtcCreateScript;
        private System.Windows.Forms.TextBox textBoxEuDsgvoScriptForTriggerEtcScript;
        private System.Windows.Forms.Button buttonEuDsgvoScriptForTriggerEtcSaveScript;
        private System.Windows.Forms.TextBox textBoxEuDsgvoScriptForTriggerEtcFile;
        private System.Windows.Forms.Button buttonEuDsgvoScriptForTriggerEtcFolder;
        private System.Windows.Forms.CheckBox checkBoxEuDsgvoScriptForTriggerEtcIncludeFunctions;
        private System.Windows.Forms.CheckBox checkBoxEuDsgvoScriptForTriggerEtcIncludeProcedures;
        private System.Windows.Forms.Label labelEuDsgvoScriptForTriggerEtc;
        private System.Windows.Forms.CheckBox checkBoxEuDsgvoScriptForTriggerEtcIncludeTrigger;
        private System.Windows.Forms.CheckBox checkBoxEuDsgvoScriptForTriggerEtcIncludeViews;
        private System.Windows.Forms.TabPage tabPageSaveLog;
        private System.Windows.Forms.SplitContainer splitContainerSaveLog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSaveLog;
        private System.Windows.Forms.Button buttonSaveLogListTables;
        private System.Windows.Forms.Button buttonSaveLog;
        private System.Windows.Forms.DataGridView dataGridViewSaveLog;
        private System.Windows.Forms.DateTimePicker dateTimePickerSaveLogStartDate;
        private System.Windows.Forms.Label labelSaveLogStartDate;
        private System.Windows.Forms.Button buttonSaveLogUserAdministration;
        private System.Windows.Forms.Label labelSaveLogTableListHeader;
        private System.Windows.Forms.Label labelSaveLogMainMessage;
        private System.Windows.Forms.Button buttonSaveLogCopyUser;
        private System.Windows.Forms.TabControl tabControlSaveLog;
        private System.Windows.Forms.TabPage tabPageSaveToLogDB;
        private System.Windows.Forms.CheckBox checkBoxLogTableDSGVO;
        private System.Windows.Forms.CheckBox checkBoxLogcolumnsDSGVO;
        private System.Windows.Forms.Button buttonFillDescriptionCache;
        private System.Windows.Forms.Button buttonViewDescriptionCache;
        private UserControls.UserControlWebView userControlWebViewEuDsgvoInfoURL;
        private System.Windows.Forms.CheckBox checkBoxDescriptionAddExistenceCheck;
        private System.Windows.Forms.Button buttonDescriptionAddDeprecated;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLogTable;
    }
}