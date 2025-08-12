namespace DiversityCollection.Forms
{
    partial class FormCopyDataset
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCopyDataset));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonNoEvent = new System.Windows.Forms.Button();
            this.buttonNewEvent = new System.Windows.Forms.Button();
            this.treeViewNoEvent = new System.Windows.Forms.TreeView();
            this.buttonSameEvent = new System.Windows.Forms.Button();
            this.treeViewSameEvent = new System.Windows.Forms.TreeView();
            this.treeViewNewEvent = new System.Windows.Forms.TreeView();
            this.labelHeaderEvent = new System.Windows.Forms.Label();
            this.groupBoxCopy = new System.Windows.Forms.GroupBox();
            this.treeViewCopy = new System.Windows.Forms.TreeView();
            this.groupBoxOriginal = new System.Windows.Forms.GroupBox();
            this.treeViewOriginal = new System.Windows.Forms.TreeView();
            this.buttonOnlyEvent = new System.Windows.Forms.Button();
            this.treeViewOnlyEvent = new System.Windows.Forms.TreeView();
            this.labelAccessionNumber = new System.Windows.Forms.Label();
            this.textBoxAccessionNumber = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonRequeryMultiCopyList = new System.Windows.Forms.Button();
            this.buttonIncludeAll = new System.Windows.Forms.Button();
            this.buttonIncludeNone = new System.Windows.Forms.Button();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageEvent = new System.Windows.Forms.TabPage();
            this.tabPageAccNr = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelAccNr = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeaderAccNr = new System.Windows.Forms.Label();
            this.buttonAccessionNumberSearch = new System.Windows.Forms.Button();
            this.textBoxAccNrInitials = new System.Windows.Forms.TextBox();
            this.checkBoxCopyIdentifactions = new System.Windows.Forms.CheckBox();
            this.checkBoxAccNr = new System.Windows.Forms.CheckBox();
            this.labelAccNrExplain1 = new System.Windows.Forms.Label();
            this.pictureBoxAccNr = new System.Windows.Forms.PictureBox();
            this.labelAccNrExplain2 = new System.Windows.Forms.Label();
            this.labelAccNrExplain3 = new System.Windows.Forms.Label();
            this.checkBoxAccessionNumberSearchIncludePart = new System.Windows.Forms.CheckBox();
            this.tabPageMultiCopy = new System.Windows.Forms.TabPage();
            this.splitContainerNumberOfCopies = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelNumberOfCopies = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownNumberOfCopies = new System.Windows.Forms.NumericUpDown();
            this.labelNumberOfCopies = new System.Windows.Forms.Label();
            this.labelNumberOfCopiesHeader = new System.Windows.Forms.Label();
            this.checkedListBoxMultiCopyAccNr = new System.Windows.Forms.CheckedListBox();
            this.tabPageProjects = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelProjects = new System.Windows.Forms.TableLayoutPanel();
            this.labelProjectsHeader = new System.Windows.Forms.Label();
            this.checkedListBoxProjects = new System.Windows.Forms.CheckedListBox();
            this.tabPageRelations = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelRelations = new System.Windows.Forms.TableLayoutPanel();
            this.labelRelationsHeader = new System.Windows.Forms.Label();
            this.groupBoxRelationFromOri = new System.Windows.Forms.GroupBox();
            this.treeViewRelationToOriginal = new System.Windows.Forms.TreeView();
            this.imageListRelations = new System.Windows.Forms.ImageList(this.components);
            this.checkBoxRelationToOriginal = new System.Windows.Forms.CheckBox();
            this.comboBoxRelationTypeToOriginal = new System.Windows.Forms.ComboBox();
            this.groupBoxRelationToOri = new System.Windows.Forms.GroupBox();
            this.treeViewRelationFromOriginal = new System.Windows.Forms.TreeView();
            this.checkBoxRelationFromOriginal = new System.Windows.Forms.CheckBox();
            this.comboBoxRelationTypeFromOriginal = new System.Windows.Forms.ComboBox();
            this.tabPageIncludedData = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelIncluded = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxIncludeExternalIdentifier = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeIdentification = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeAnalysis = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeIdentification = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeAnalysis = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeProcessing = new System.Windows.Forms.CheckBox();
            this.pictureBoxIncludeProcessing = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeAgent = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeAgent = new System.Windows.Forms.CheckBox();
            this.pictureBoxIncludeSpecimenImages = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeSpecimenImages = new System.Windows.Forms.CheckBox();
            this.pictureBoxIncludeEventImages = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeReferences = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeTransactions = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeAnnotations = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeImageProperties = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeImageProperties = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeExternalIdentifier = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeReferences = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeAnnotations = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeTransactions = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeEventImages = new System.Windows.Forms.CheckBox();
            this.pictureBoxIncludeAnalysisMethods = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeAnalysisMethods = new System.Windows.Forms.CheckBox();
            this.pictureBoxIncludeProcessingMethods = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeProcessingMethods = new System.Windows.Forms.CheckBox();
            this.labelInclude = new System.Windows.Forms.Label();
            this.pictureBoxIncludeRelations = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeRelations = new System.Windows.Forms.CheckBox();
            this.pictureBoxIncludeEventMethod = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeEventMethod = new System.Windows.Forms.CheckBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanelMain.SuspendLayout();
            this.groupBoxCopy.SuspendLayout();
            this.groupBoxOriginal.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageEvent.SuspendLayout();
            this.tabPageAccNr.SuspendLayout();
            this.tableLayoutPanelAccNr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAccNr)).BeginInit();
            this.tabPageMultiCopy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNumberOfCopies)).BeginInit();
            this.splitContainerNumberOfCopies.Panel1.SuspendLayout();
            this.splitContainerNumberOfCopies.Panel2.SuspendLayout();
            this.splitContainerNumberOfCopies.SuspendLayout();
            this.tableLayoutPanelNumberOfCopies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumberOfCopies)).BeginInit();
            this.tabPageProjects.SuspendLayout();
            this.tableLayoutPanelProjects.SuspendLayout();
            this.tabPageRelations.SuspendLayout();
            this.tableLayoutPanelRelations.SuspendLayout();
            this.groupBoxRelationFromOri.SuspendLayout();
            this.groupBoxRelationToOri.SuspendLayout();
            this.tabPageIncludedData.SuspendLayout();
            this.tableLayoutPanelIncluded.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeExternalIdentifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeIdentification)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeAnalysis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeProcessing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeAgent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeSpecimenImages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeEventImages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeReferences)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeTransactions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeAnnotations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeImageProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeAnalysisMethods)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeProcessingMethods)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeRelations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeEventMethod)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            resources.ApplyResources(this.tableLayoutPanelMain, "tableLayoutPanelMain");
            this.tableLayoutPanelMain.Controls.Add(this.buttonNoEvent, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonNewEvent, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.treeViewNoEvent, 2, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSameEvent, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.treeViewSameEvent, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.treeViewNewEvent, 2, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeaderEvent, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxCopy, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxOriginal, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOnlyEvent, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.treeViewOnlyEvent, 2, 5);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelMain, ((bool)(resources.GetObject("tableLayoutPanelMain.ShowHelp"))));
            this.tableLayoutPanelMain.TabStop = true;
            // 
            // buttonNoEvent
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonNoEvent, 2);
            resources.ApplyResources(this.buttonNoEvent, "buttonNoEvent");
            this.buttonNoEvent.Name = "buttonNoEvent";
            this.helpProvider.SetShowHelp(this.buttonNoEvent, ((bool)(resources.GetObject("buttonNoEvent.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonNoEvent, resources.GetString("buttonNoEvent.ToolTip"));
            this.buttonNoEvent.UseVisualStyleBackColor = true;
            this.buttonNoEvent.Click += new System.EventHandler(this.buttonNoEvent_Click);
            // 
            // buttonNewEvent
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonNewEvent, 2);
            resources.ApplyResources(this.buttonNewEvent, "buttonNewEvent");
            this.buttonNewEvent.Name = "buttonNewEvent";
            this.helpProvider.SetShowHelp(this.buttonNewEvent, ((bool)(resources.GetObject("buttonNewEvent.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonNewEvent, resources.GetString("buttonNewEvent.ToolTip"));
            this.buttonNewEvent.UseVisualStyleBackColor = true;
            this.buttonNewEvent.Click += new System.EventHandler(this.buttonNewEvent_Click);
            // 
            // treeViewNoEvent
            // 
            this.treeViewNoEvent.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewNoEvent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.treeViewNoEvent, 3);
            resources.ApplyResources(this.treeViewNoEvent, "treeViewNoEvent");
            this.treeViewNoEvent.Name = "treeViewNoEvent";
            this.treeViewNoEvent.Scrollable = false;
            this.helpProvider.SetShowHelp(this.treeViewNoEvent, ((bool)(resources.GetObject("treeViewNoEvent.ShowHelp"))));
            this.treeViewNoEvent.TabStop = false;
            // 
            // buttonSameEvent
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonSameEvent, 2);
            resources.ApplyResources(this.buttonSameEvent, "buttonSameEvent");
            this.buttonSameEvent.Name = "buttonSameEvent";
            this.helpProvider.SetShowHelp(this.buttonSameEvent, ((bool)(resources.GetObject("buttonSameEvent.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonSameEvent, resources.GetString("buttonSameEvent.ToolTip"));
            this.buttonSameEvent.UseVisualStyleBackColor = true;
            this.buttonSameEvent.Click += new System.EventHandler(this.buttonSameEvent_Click);
            // 
            // treeViewSameEvent
            // 
            this.treeViewSameEvent.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewSameEvent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.treeViewSameEvent, 3);
            resources.ApplyResources(this.treeViewSameEvent, "treeViewSameEvent");
            this.treeViewSameEvent.Name = "treeViewSameEvent";
            this.treeViewSameEvent.Scrollable = false;
            this.helpProvider.SetShowHelp(this.treeViewSameEvent, ((bool)(resources.GetObject("treeViewSameEvent.ShowHelp"))));
            this.treeViewSameEvent.TabStop = false;
            // 
            // treeViewNewEvent
            // 
            this.treeViewNewEvent.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewNewEvent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.treeViewNewEvent, 3);
            resources.ApplyResources(this.treeViewNewEvent, "treeViewNewEvent");
            this.treeViewNewEvent.Name = "treeViewNewEvent";
            this.treeViewNewEvent.Scrollable = false;
            this.helpProvider.SetShowHelp(this.treeViewNewEvent, ((bool)(resources.GetObject("treeViewNewEvent.ShowHelp"))));
            this.treeViewNewEvent.TabStop = false;
            // 
            // labelHeaderEvent
            // 
            resources.ApplyResources(this.labelHeaderEvent, "labelHeaderEvent");
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeaderEvent, 2);
            this.labelHeaderEvent.Name = "labelHeaderEvent";
            this.tableLayoutPanelMain.SetRowSpan(this.labelHeaderEvent, 2);
            this.helpProvider.SetShowHelp(this.labelHeaderEvent, ((bool)(resources.GetObject("labelHeaderEvent.ShowHelp"))));
            // 
            // groupBoxCopy
            // 
            this.groupBoxCopy.Controls.Add(this.treeViewCopy);
            resources.ApplyResources(this.groupBoxCopy, "groupBoxCopy");
            this.groupBoxCopy.Name = "groupBoxCopy";
            this.helpProvider.SetShowHelp(this.groupBoxCopy, ((bool)(resources.GetObject("groupBoxCopy.ShowHelp"))));
            this.groupBoxCopy.TabStop = false;
            // 
            // treeViewCopy
            // 
            this.treeViewCopy.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewCopy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.treeViewCopy, "treeViewCopy");
            this.treeViewCopy.Name = "treeViewCopy";
            this.treeViewCopy.Scrollable = false;
            this.helpProvider.SetShowHelp(this.treeViewCopy, ((bool)(resources.GetObject("treeViewCopy.ShowHelp"))));
            this.treeViewCopy.TabStop = false;
            // 
            // groupBoxOriginal
            // 
            this.groupBoxOriginal.Controls.Add(this.treeViewOriginal);
            resources.ApplyResources(this.groupBoxOriginal, "groupBoxOriginal");
            this.groupBoxOriginal.ForeColor = System.Drawing.Color.Gray;
            this.groupBoxOriginal.Name = "groupBoxOriginal";
            this.helpProvider.SetShowHelp(this.groupBoxOriginal, ((bool)(resources.GetObject("groupBoxOriginal.ShowHelp"))));
            this.groupBoxOriginal.TabStop = false;
            // 
            // treeViewOriginal
            // 
            this.treeViewOriginal.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewOriginal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.treeViewOriginal, "treeViewOriginal");
            this.treeViewOriginal.Name = "treeViewOriginal";
            this.treeViewOriginal.Scrollable = false;
            this.helpProvider.SetShowHelp(this.treeViewOriginal, ((bool)(resources.GetObject("treeViewOriginal.ShowHelp"))));
            this.treeViewOriginal.TabStop = false;
            // 
            // buttonOnlyEvent
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonOnlyEvent, 2);
            resources.ApplyResources(this.buttonOnlyEvent, "buttonOnlyEvent");
            this.buttonOnlyEvent.Name = "buttonOnlyEvent";
            this.buttonOnlyEvent.UseVisualStyleBackColor = true;
            this.buttonOnlyEvent.Click += new System.EventHandler(this.buttonOnlyEvent_Click);
            // 
            // treeViewOnlyEvent
            // 
            this.treeViewOnlyEvent.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewOnlyEvent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.treeViewOnlyEvent, 3);
            resources.ApplyResources(this.treeViewOnlyEvent, "treeViewOnlyEvent");
            this.treeViewOnlyEvent.Name = "treeViewOnlyEvent";
            // 
            // labelAccessionNumber
            // 
            resources.ApplyResources(this.labelAccessionNumber, "labelAccessionNumber");
            this.labelAccessionNumber.Name = "labelAccessionNumber";
            this.helpProvider.SetShowHelp(this.labelAccessionNumber, ((bool)(resources.GetObject("labelAccessionNumber.ShowHelp"))));
            // 
            // textBoxAccessionNumber
            // 
            this.textBoxAccessionNumber.BackColor = System.Drawing.Color.Pink;
            resources.ApplyResources(this.textBoxAccessionNumber, "textBoxAccessionNumber");
            this.textBoxAccessionNumber.Name = "textBoxAccessionNumber";
            this.helpProvider.SetShowHelp(this.textBoxAccessionNumber, ((bool)(resources.GetObject("textBoxAccessionNumber.ShowHelp"))));
            this.toolTip.SetToolTip(this.textBoxAccessionNumber, resources.GetString("textBoxAccessionNumber.ToolTip"));
            this.textBoxAccessionNumber.TextChanged += new System.EventHandler(this.textBoxAccessionNumber_TextChanged);
            // 
            // buttonRequeryMultiCopyList
            // 
            resources.ApplyResources(this.buttonRequeryMultiCopyList, "buttonRequeryMultiCopyList");
            this.buttonRequeryMultiCopyList.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.buttonRequeryMultiCopyList.FlatAppearance.BorderSize = 0;
            this.buttonRequeryMultiCopyList.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRequeryMultiCopyList.Name = "buttonRequeryMultiCopyList";
            this.helpProvider.SetShowHelp(this.buttonRequeryMultiCopyList, ((bool)(resources.GetObject("buttonRequeryMultiCopyList.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonRequeryMultiCopyList, resources.GetString("buttonRequeryMultiCopyList.ToolTip"));
            this.buttonRequeryMultiCopyList.UseVisualStyleBackColor = true;
            this.buttonRequeryMultiCopyList.Click += new System.EventHandler(this.buttonRequeryMultiCopyList_Click);
            // 
            // buttonIncludeAll
            // 
            resources.ApplyResources(this.buttonIncludeAll, "buttonIncludeAll");
            this.buttonIncludeAll.FlatAppearance.BorderSize = 0;
            this.buttonIncludeAll.Image = global::DiversityCollection.Resource.CheckYes;
            this.buttonIncludeAll.Name = "buttonIncludeAll";
            this.helpProvider.SetShowHelp(this.buttonIncludeAll, ((bool)(resources.GetObject("buttonIncludeAll.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonIncludeAll, resources.GetString("buttonIncludeAll.ToolTip"));
            this.buttonIncludeAll.UseVisualStyleBackColor = true;
            this.buttonIncludeAll.Click += new System.EventHandler(this.buttonIncludeAll_Click);
            // 
            // buttonIncludeNone
            // 
            this.buttonIncludeNone.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.buttonIncludeNone, "buttonIncludeNone");
            this.buttonIncludeNone.Image = global::DiversityCollection.Resource.CheckNo;
            this.buttonIncludeNone.Name = "buttonIncludeNone";
            this.helpProvider.SetShowHelp(this.buttonIncludeNone, ((bool)(resources.GetObject("buttonIncludeNone.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonIncludeNone, resources.GetString("buttonIncludeNone.ToolTip"));
            this.buttonIncludeNone.UseVisualStyleBackColor = true;
            this.buttonIncludeNone.Click += new System.EventHandler(this.buttonIncludeNone_Click);
            // 
            // buttonFeedback
            // 
            resources.ApplyResources(this.buttonFeedback, "buttonFeedback");
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonFeedback.Name = "buttonFeedback";
            this.helpProvider.SetShowHelp(this.buttonFeedback, ((bool)(resources.GetObject("buttonFeedback.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonFeedback, resources.GetString("buttonFeedback.ToolTip"));
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // tabControlMain
            // 
            resources.ApplyResources(this.tabControlMain, "tabControlMain");
            this.tabControlMain.Controls.Add(this.tabPageEvent);
            this.tabControlMain.Controls.Add(this.tabPageAccNr);
            this.tabControlMain.Controls.Add(this.tabPageMultiCopy);
            this.tabControlMain.Controls.Add(this.tabPageProjects);
            this.tabControlMain.Controls.Add(this.tabPageRelations);
            this.tabControlMain.Controls.Add(this.tabPageIncludedData);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.helpProvider.SetShowHelp(this.tabControlMain, ((bool)(resources.GetObject("tabControlMain.ShowHelp"))));
            // 
            // tabPageEvent
            // 
            this.tabPageEvent.Controls.Add(this.tableLayoutPanelMain);
            resources.ApplyResources(this.tabPageEvent, "tabPageEvent");
            this.tabPageEvent.Name = "tabPageEvent";
            this.helpProvider.SetShowHelp(this.tabPageEvent, ((bool)(resources.GetObject("tabPageEvent.ShowHelp"))));
            this.tabPageEvent.UseVisualStyleBackColor = true;
            // 
            // tabPageAccNr
            // 
            this.tabPageAccNr.Controls.Add(this.tableLayoutPanelAccNr);
            resources.ApplyResources(this.tabPageAccNr, "tabPageAccNr");
            this.tabPageAccNr.Name = "tabPageAccNr";
            this.helpProvider.SetShowHelp(this.tabPageAccNr, ((bool)(resources.GetObject("tabPageAccNr.ShowHelp"))));
            this.tabPageAccNr.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelAccNr
            // 
            resources.ApplyResources(this.tableLayoutPanelAccNr, "tableLayoutPanelAccNr");
            this.tableLayoutPanelAccNr.Controls.Add(this.labelHeaderAccNr, 1, 1);
            this.tableLayoutPanelAccNr.Controls.Add(this.buttonAccessionNumberSearch, 1, 2);
            this.tableLayoutPanelAccNr.Controls.Add(this.textBoxAccNrInitials, 2, 2);
            this.tableLayoutPanelAccNr.Controls.Add(this.labelAccessionNumber, 1, 3);
            this.tableLayoutPanelAccNr.Controls.Add(this.textBoxAccessionNumber, 2, 3);
            this.tableLayoutPanelAccNr.Controls.Add(this.checkBoxCopyIdentifactions, 0, 5);
            this.tableLayoutPanelAccNr.Controls.Add(this.checkBoxAccNr, 0, 0);
            this.tableLayoutPanelAccNr.Controls.Add(this.labelAccNrExplain1, 3, 0);
            this.tableLayoutPanelAccNr.Controls.Add(this.pictureBoxAccNr, 0, 1);
            this.tableLayoutPanelAccNr.Controls.Add(this.labelAccNrExplain2, 3, 2);
            this.tableLayoutPanelAccNr.Controls.Add(this.labelAccNrExplain3, 3, 3);
            this.tableLayoutPanelAccNr.Controls.Add(this.checkBoxAccessionNumberSearchIncludePart, 1, 4);
            this.tableLayoutPanelAccNr.Name = "tableLayoutPanelAccNr";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelAccNr, ((bool)(resources.GetObject("tableLayoutPanelAccNr.ShowHelp"))));
            // 
            // labelHeaderAccNr
            // 
            resources.ApplyResources(this.labelHeaderAccNr, "labelHeaderAccNr");
            this.tableLayoutPanelAccNr.SetColumnSpan(this.labelHeaderAccNr, 2);
            this.labelHeaderAccNr.Name = "labelHeaderAccNr";
            this.helpProvider.SetShowHelp(this.labelHeaderAccNr, ((bool)(resources.GetObject("labelHeaderAccNr.ShowHelp"))));
            // 
            // buttonAccessionNumberSearch
            // 
            resources.ApplyResources(this.buttonAccessionNumberSearch, "buttonAccessionNumberSearch");
            this.buttonAccessionNumberSearch.Name = "buttonAccessionNumberSearch";
            this.helpProvider.SetShowHelp(this.buttonAccessionNumberSearch, ((bool)(resources.GetObject("buttonAccessionNumberSearch.ShowHelp"))));
            this.buttonAccessionNumberSearch.UseVisualStyleBackColor = true;
            this.buttonAccessionNumberSearch.Click += new System.EventHandler(this.buttonAccessionNumberSearch_Click);
            // 
            // textBoxAccNrInitials
            // 
            this.textBoxAccNrInitials.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxAccNrInitials, "textBoxAccNrInitials");
            this.textBoxAccNrInitials.Name = "textBoxAccNrInitials";
            this.helpProvider.SetShowHelp(this.textBoxAccNrInitials, ((bool)(resources.GetObject("textBoxAccNrInitials.ShowHelp"))));
            // 
            // checkBoxCopyIdentifactions
            // 
            resources.ApplyResources(this.checkBoxCopyIdentifactions, "checkBoxCopyIdentifactions");
            this.checkBoxCopyIdentifactions.Checked = true;
            this.checkBoxCopyIdentifactions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelAccNr.SetColumnSpan(this.checkBoxCopyIdentifactions, 3);
            this.checkBoxCopyIdentifactions.Name = "checkBoxCopyIdentifactions";
            this.helpProvider.SetShowHelp(this.checkBoxCopyIdentifactions, ((bool)(resources.GetObject("checkBoxCopyIdentifactions.ShowHelp"))));
            this.checkBoxCopyIdentifactions.UseVisualStyleBackColor = true;
            // 
            // checkBoxAccNr
            // 
            resources.ApplyResources(this.checkBoxAccNr, "checkBoxAccNr");
            this.checkBoxAccNr.Checked = true;
            this.checkBoxAccNr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelAccNr.SetColumnSpan(this.checkBoxAccNr, 3);
            this.checkBoxAccNr.Name = "checkBoxAccNr";
            this.helpProvider.SetShowHelp(this.checkBoxAccNr, ((bool)(resources.GetObject("checkBoxAccNr.ShowHelp"))));
            this.checkBoxAccNr.UseVisualStyleBackColor = true;
            this.checkBoxAccNr.CheckedChanged += new System.EventHandler(this.checkBoxAccNr_CheckedChanged);
            // 
            // labelAccNrExplain1
            // 
            resources.ApplyResources(this.labelAccNrExplain1, "labelAccNrExplain1");
            this.labelAccNrExplain1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelAccNrExplain1.Name = "labelAccNrExplain1";
            this.tableLayoutPanelAccNr.SetRowSpan(this.labelAccNrExplain1, 2);
            this.helpProvider.SetShowHelp(this.labelAccNrExplain1, ((bool)(resources.GetObject("labelAccNrExplain1.ShowHelp"))));
            // 
            // pictureBoxAccNr
            // 
            resources.ApplyResources(this.pictureBoxAccNr, "pictureBoxAccNr");
            this.pictureBoxAccNr.Image = global::DiversityCollection.Resource.CollectionSpecimen;
            this.pictureBoxAccNr.Name = "pictureBoxAccNr";
            this.helpProvider.SetShowHelp(this.pictureBoxAccNr, ((bool)(resources.GetObject("pictureBoxAccNr.ShowHelp"))));
            this.pictureBoxAccNr.TabStop = false;
            // 
            // labelAccNrExplain2
            // 
            resources.ApplyResources(this.labelAccNrExplain2, "labelAccNrExplain2");
            this.labelAccNrExplain2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelAccNrExplain2.Name = "labelAccNrExplain2";
            this.helpProvider.SetShowHelp(this.labelAccNrExplain2, ((bool)(resources.GetObject("labelAccNrExplain2.ShowHelp"))));
            // 
            // labelAccNrExplain3
            // 
            resources.ApplyResources(this.labelAccNrExplain3, "labelAccNrExplain3");
            this.labelAccNrExplain3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelAccNrExplain3.Name = "labelAccNrExplain3";
            this.helpProvider.SetShowHelp(this.labelAccNrExplain3, ((bool)(resources.GetObject("labelAccNrExplain3.ShowHelp"))));
            // 
            // checkBoxAccessionNumberSearchIncludePart
            // 
            resources.ApplyResources(this.checkBoxAccessionNumberSearchIncludePart, "checkBoxAccessionNumberSearchIncludePart");
            this.tableLayoutPanelAccNr.SetColumnSpan(this.checkBoxAccessionNumberSearchIncludePart, 3);
            this.checkBoxAccessionNumberSearchIncludePart.Name = "checkBoxAccessionNumberSearchIncludePart";
            this.helpProvider.SetShowHelp(this.checkBoxAccessionNumberSearchIncludePart, ((bool)(resources.GetObject("checkBoxAccessionNumberSearchIncludePart.ShowHelp"))));
            this.checkBoxAccessionNumberSearchIncludePart.UseVisualStyleBackColor = true;
            this.checkBoxAccessionNumberSearchIncludePart.Click += new System.EventHandler(this.checkBoxAccessionNumberSearchIncludePart_Click);
            // 
            // tabPageMultiCopy
            // 
            this.tabPageMultiCopy.Controls.Add(this.splitContainerNumberOfCopies);
            resources.ApplyResources(this.tabPageMultiCopy, "tabPageMultiCopy");
            this.tabPageMultiCopy.Name = "tabPageMultiCopy";
            this.helpProvider.SetShowHelp(this.tabPageMultiCopy, ((bool)(resources.GetObject("tabPageMultiCopy.ShowHelp"))));
            this.tabPageMultiCopy.UseVisualStyleBackColor = true;
            // 
            // splitContainerNumberOfCopies
            // 
            resources.ApplyResources(this.splitContainerNumberOfCopies, "splitContainerNumberOfCopies");
            this.splitContainerNumberOfCopies.Name = "splitContainerNumberOfCopies";
            // 
            // splitContainerNumberOfCopies.Panel1
            // 
            this.splitContainerNumberOfCopies.Panel1.Controls.Add(this.tableLayoutPanelNumberOfCopies);
            this.helpProvider.SetShowHelp(this.splitContainerNumberOfCopies.Panel1, ((bool)(resources.GetObject("splitContainerNumberOfCopies.Panel1.ShowHelp"))));
            // 
            // splitContainerNumberOfCopies.Panel2
            // 
            this.splitContainerNumberOfCopies.Panel2.Controls.Add(this.checkedListBoxMultiCopyAccNr);
            this.helpProvider.SetShowHelp(this.splitContainerNumberOfCopies.Panel2, ((bool)(resources.GetObject("splitContainerNumberOfCopies.Panel2.ShowHelp"))));
            this.helpProvider.SetShowHelp(this.splitContainerNumberOfCopies, ((bool)(resources.GetObject("splitContainerNumberOfCopies.ShowHelp"))));
            // 
            // tableLayoutPanelNumberOfCopies
            // 
            resources.ApplyResources(this.tableLayoutPanelNumberOfCopies, "tableLayoutPanelNumberOfCopies");
            this.tableLayoutPanelNumberOfCopies.Controls.Add(this.numericUpDownNumberOfCopies, 1, 1);
            this.tableLayoutPanelNumberOfCopies.Controls.Add(this.labelNumberOfCopies, 0, 1);
            this.tableLayoutPanelNumberOfCopies.Controls.Add(this.labelNumberOfCopiesHeader, 0, 0);
            this.tableLayoutPanelNumberOfCopies.Controls.Add(this.buttonRequeryMultiCopyList, 1, 2);
            this.tableLayoutPanelNumberOfCopies.Name = "tableLayoutPanelNumberOfCopies";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelNumberOfCopies, ((bool)(resources.GetObject("tableLayoutPanelNumberOfCopies.ShowHelp"))));
            // 
            // numericUpDownNumberOfCopies
            // 
            resources.ApplyResources(this.numericUpDownNumberOfCopies, "numericUpDownNumberOfCopies");
            this.numericUpDownNumberOfCopies.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownNumberOfCopies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNumberOfCopies.Name = "numericUpDownNumberOfCopies";
            this.helpProvider.SetShowHelp(this.numericUpDownNumberOfCopies, ((bool)(resources.GetObject("numericUpDownNumberOfCopies.ShowHelp"))));
            this.numericUpDownNumberOfCopies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNumberOfCopies.ValueChanged += new System.EventHandler(this.numericUpDownNumberOfCopies_ValueChanged);
            this.numericUpDownNumberOfCopies.Leave += new System.EventHandler(this.numericUpDownNumberOfCopies_Leave);
            // 
            // labelNumberOfCopies
            // 
            resources.ApplyResources(this.labelNumberOfCopies, "labelNumberOfCopies");
            this.labelNumberOfCopies.Name = "labelNumberOfCopies";
            this.helpProvider.SetShowHelp(this.labelNumberOfCopies, ((bool)(resources.GetObject("labelNumberOfCopies.ShowHelp"))));
            // 
            // labelNumberOfCopiesHeader
            // 
            resources.ApplyResources(this.labelNumberOfCopiesHeader, "labelNumberOfCopiesHeader");
            this.tableLayoutPanelNumberOfCopies.SetColumnSpan(this.labelNumberOfCopiesHeader, 2);
            this.labelNumberOfCopiesHeader.Name = "labelNumberOfCopiesHeader";
            this.helpProvider.SetShowHelp(this.labelNumberOfCopiesHeader, ((bool)(resources.GetObject("labelNumberOfCopiesHeader.ShowHelp"))));
            // 
            // checkedListBoxMultiCopyAccNr
            // 
            this.checkedListBoxMultiCopyAccNr.CheckOnClick = true;
            resources.ApplyResources(this.checkedListBoxMultiCopyAccNr, "checkedListBoxMultiCopyAccNr");
            this.checkedListBoxMultiCopyAccNr.FormattingEnabled = true;
            this.checkedListBoxMultiCopyAccNr.Name = "checkedListBoxMultiCopyAccNr";
            this.helpProvider.SetShowHelp(this.checkedListBoxMultiCopyAccNr, ((bool)(resources.GetObject("checkedListBoxMultiCopyAccNr.ShowHelp"))));
            // 
            // tabPageProjects
            // 
            this.tabPageProjects.Controls.Add(this.tableLayoutPanelProjects);
            resources.ApplyResources(this.tabPageProjects, "tabPageProjects");
            this.tabPageProjects.Name = "tabPageProjects";
            this.helpProvider.SetShowHelp(this.tabPageProjects, ((bool)(resources.GetObject("tabPageProjects.ShowHelp"))));
            this.tabPageProjects.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelProjects
            // 
            resources.ApplyResources(this.tableLayoutPanelProjects, "tableLayoutPanelProjects");
            this.tableLayoutPanelProjects.Controls.Add(this.labelProjectsHeader, 0, 0);
            this.tableLayoutPanelProjects.Controls.Add(this.checkedListBoxProjects, 0, 1);
            this.tableLayoutPanelProjects.Name = "tableLayoutPanelProjects";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelProjects, ((bool)(resources.GetObject("tableLayoutPanelProjects.ShowHelp"))));
            // 
            // labelProjectsHeader
            // 
            resources.ApplyResources(this.labelProjectsHeader, "labelProjectsHeader");
            this.labelProjectsHeader.Name = "labelProjectsHeader";
            this.helpProvider.SetShowHelp(this.labelProjectsHeader, ((bool)(resources.GetObject("labelProjectsHeader.ShowHelp"))));
            // 
            // checkedListBoxProjects
            // 
            this.checkedListBoxProjects.CheckOnClick = true;
            resources.ApplyResources(this.checkedListBoxProjects, "checkedListBoxProjects");
            this.checkedListBoxProjects.FormattingEnabled = true;
            this.checkedListBoxProjects.Name = "checkedListBoxProjects";
            this.helpProvider.SetShowHelp(this.checkedListBoxProjects, ((bool)(resources.GetObject("checkedListBoxProjects.ShowHelp"))));
            // 
            // tabPageRelations
            // 
            this.tabPageRelations.Controls.Add(this.tableLayoutPanelRelations);
            resources.ApplyResources(this.tabPageRelations, "tabPageRelations");
            this.tabPageRelations.Name = "tabPageRelations";
            this.helpProvider.SetShowHelp(this.tabPageRelations, ((bool)(resources.GetObject("tabPageRelations.ShowHelp"))));
            this.tabPageRelations.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelRelations
            // 
            resources.ApplyResources(this.tableLayoutPanelRelations, "tableLayoutPanelRelations");
            this.tableLayoutPanelRelations.Controls.Add(this.labelRelationsHeader, 0, 0);
            this.tableLayoutPanelRelations.Controls.Add(this.groupBoxRelationFromOri, 0, 1);
            this.tableLayoutPanelRelations.Controls.Add(this.groupBoxRelationToOri, 0, 2);
            this.tableLayoutPanelRelations.Name = "tableLayoutPanelRelations";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelRelations, ((bool)(resources.GetObject("tableLayoutPanelRelations.ShowHelp"))));
            // 
            // labelRelationsHeader
            // 
            resources.ApplyResources(this.labelRelationsHeader, "labelRelationsHeader");
            this.labelRelationsHeader.Name = "labelRelationsHeader";
            this.helpProvider.SetShowHelp(this.labelRelationsHeader, ((bool)(resources.GetObject("labelRelationsHeader.ShowHelp"))));
            // 
            // groupBoxRelationFromOri
            // 
            this.groupBoxRelationFromOri.Controls.Add(this.treeViewRelationToOriginal);
            this.groupBoxRelationFromOri.Controls.Add(this.checkBoxRelationToOriginal);
            this.groupBoxRelationFromOri.Controls.Add(this.comboBoxRelationTypeToOriginal);
            resources.ApplyResources(this.groupBoxRelationFromOri, "groupBoxRelationFromOri");
            this.groupBoxRelationFromOri.Name = "groupBoxRelationFromOri";
            this.helpProvider.SetShowHelp(this.groupBoxRelationFromOri, ((bool)(resources.GetObject("groupBoxRelationFromOri.ShowHelp"))));
            this.groupBoxRelationFromOri.TabStop = false;
            // 
            // treeViewRelationToOriginal
            // 
            this.treeViewRelationToOriginal.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewRelationToOriginal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.treeViewRelationToOriginal, "treeViewRelationToOriginal");
            this.treeViewRelationToOriginal.ImageList = this.imageListRelations;
            this.treeViewRelationToOriginal.Name = "treeViewRelationToOriginal";
            this.treeViewRelationToOriginal.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeViewRelationToOriginal.Nodes")))});
            this.helpProvider.SetShowHelp(this.treeViewRelationToOriginal, ((bool)(resources.GetObject("treeViewRelationToOriginal.ShowHelp"))));
            // 
            // imageListRelations
            // 
            this.imageListRelations.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListRelations.ImageStream")));
            this.imageListRelations.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListRelations.Images.SetKeyName(0, "New.ico");
            this.imageListRelations.Images.SetKeyName(1, "Copy.ico");
            this.imageListRelations.Images.SetKeyName(2, "Relation.ico");
            // 
            // checkBoxRelationToOriginal
            // 
            resources.ApplyResources(this.checkBoxRelationToOriginal, "checkBoxRelationToOriginal");
            this.checkBoxRelationToOriginal.Name = "checkBoxRelationToOriginal";
            this.helpProvider.SetShowHelp(this.checkBoxRelationToOriginal, ((bool)(resources.GetObject("checkBoxRelationToOriginal.ShowHelp"))));
            this.checkBoxRelationToOriginal.UseVisualStyleBackColor = true;
            // 
            // comboBoxRelationTypeToOriginal
            // 
            resources.ApplyResources(this.comboBoxRelationTypeToOriginal, "comboBoxRelationTypeToOriginal");
            this.comboBoxRelationTypeToOriginal.FormattingEnabled = true;
            this.comboBoxRelationTypeToOriginal.Name = "comboBoxRelationTypeToOriginal";
            this.helpProvider.SetShowHelp(this.comboBoxRelationTypeToOriginal, ((bool)(resources.GetObject("comboBoxRelationTypeToOriginal.ShowHelp"))));
            // 
            // groupBoxRelationToOri
            // 
            this.groupBoxRelationToOri.Controls.Add(this.treeViewRelationFromOriginal);
            this.groupBoxRelationToOri.Controls.Add(this.checkBoxRelationFromOriginal);
            this.groupBoxRelationToOri.Controls.Add(this.comboBoxRelationTypeFromOriginal);
            resources.ApplyResources(this.groupBoxRelationToOri, "groupBoxRelationToOri");
            this.groupBoxRelationToOri.Name = "groupBoxRelationToOri";
            this.helpProvider.SetShowHelp(this.groupBoxRelationToOri, ((bool)(resources.GetObject("groupBoxRelationToOri.ShowHelp"))));
            this.groupBoxRelationToOri.TabStop = false;
            // 
            // treeViewRelationFromOriginal
            // 
            this.treeViewRelationFromOriginal.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewRelationFromOriginal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.treeViewRelationFromOriginal, "treeViewRelationFromOriginal");
            this.treeViewRelationFromOriginal.ImageList = this.imageListRelations;
            this.treeViewRelationFromOriginal.Name = "treeViewRelationFromOriginal";
            this.treeViewRelationFromOriginal.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeViewRelationFromOriginal.Nodes")))});
            this.helpProvider.SetShowHelp(this.treeViewRelationFromOriginal, ((bool)(resources.GetObject("treeViewRelationFromOriginal.ShowHelp"))));
            // 
            // checkBoxRelationFromOriginal
            // 
            resources.ApplyResources(this.checkBoxRelationFromOriginal, "checkBoxRelationFromOriginal");
            this.checkBoxRelationFromOriginal.Name = "checkBoxRelationFromOriginal";
            this.helpProvider.SetShowHelp(this.checkBoxRelationFromOriginal, ((bool)(resources.GetObject("checkBoxRelationFromOriginal.ShowHelp"))));
            this.checkBoxRelationFromOriginal.UseVisualStyleBackColor = true;
            // 
            // comboBoxRelationTypeFromOriginal
            // 
            resources.ApplyResources(this.comboBoxRelationTypeFromOriginal, "comboBoxRelationTypeFromOriginal");
            this.comboBoxRelationTypeFromOriginal.FormattingEnabled = true;
            this.comboBoxRelationTypeFromOriginal.Name = "comboBoxRelationTypeFromOriginal";
            this.helpProvider.SetShowHelp(this.comboBoxRelationTypeFromOriginal, ((bool)(resources.GetObject("comboBoxRelationTypeFromOriginal.ShowHelp"))));
            // 
            // tabPageIncludedData
            // 
            this.tabPageIncludedData.Controls.Add(this.tableLayoutPanelIncluded);
            resources.ApplyResources(this.tabPageIncludedData, "tabPageIncludedData");
            this.tabPageIncludedData.Name = "tabPageIncludedData";
            this.helpProvider.SetShowHelp(this.tabPageIncludedData, ((bool)(resources.GetObject("tabPageIncludedData.ShowHelp"))));
            this.tabPageIncludedData.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelIncluded
            // 
            resources.ApplyResources(this.tableLayoutPanelIncluded, "tableLayoutPanelIncluded");
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeExternalIdentifier, 0, 14);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeIdentification, 0, 1);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeAnalysis, 0, 2);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeIdentification, 1, 1);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeAnalysis, 1, 2);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeProcessing, 1, 4);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeProcessing, 0, 4);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeAgent, 0, 6);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeAgent, 1, 6);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeSpecimenImages, 0, 7);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeSpecimenImages, 1, 7);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeEventImages, 0, 10);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeReferences, 0, 12);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeTransactions, 0, 13);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeAnnotations, 0, 15);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeImageProperties, 1, 8);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeImageProperties, 2, 8);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeExternalIdentifier, 1, 14);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeReferences, 1, 12);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeAnnotations, 1, 15);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeTransactions, 1, 13);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeEventImages, 1, 10);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeAnalysisMethods, 1, 3);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeAnalysisMethods, 2, 3);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeProcessingMethods, 1, 5);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeProcessingMethods, 2, 5);
            this.tableLayoutPanelIncluded.Controls.Add(this.labelInclude, 0, 0);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeRelations, 0, 11);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeRelations, 1, 11);
            this.tableLayoutPanelIncluded.Controls.Add(this.buttonIncludeAll, 3, 0);
            this.tableLayoutPanelIncluded.Controls.Add(this.buttonIncludeNone, 3, 1);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeEventMethod, 0, 9);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeEventMethod, 1, 9);
            this.tableLayoutPanelIncluded.Name = "tableLayoutPanelIncluded";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelIncluded, ((bool)(resources.GetObject("tableLayoutPanelIncluded.ShowHelp"))));
            // 
            // pictureBoxIncludeExternalIdentifier
            // 
            this.pictureBoxIncludeExternalIdentifier.Image = global::DiversityCollection.Resource.Identifier;
            resources.ApplyResources(this.pictureBoxIncludeExternalIdentifier, "pictureBoxIncludeExternalIdentifier");
            this.pictureBoxIncludeExternalIdentifier.Name = "pictureBoxIncludeExternalIdentifier";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeExternalIdentifier, ((bool)(resources.GetObject("pictureBoxIncludeExternalIdentifier.ShowHelp"))));
            this.pictureBoxIncludeExternalIdentifier.TabStop = false;
            // 
            // pictureBoxIncludeIdentification
            // 
            this.pictureBoxIncludeIdentification.Image = global::DiversityCollection.Resource.Identification;
            resources.ApplyResources(this.pictureBoxIncludeIdentification, "pictureBoxIncludeIdentification");
            this.pictureBoxIncludeIdentification.Name = "pictureBoxIncludeIdentification";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeIdentification, ((bool)(resources.GetObject("pictureBoxIncludeIdentification.ShowHelp"))));
            this.pictureBoxIncludeIdentification.TabStop = false;
            // 
            // pictureBoxIncludeAnalysis
            // 
            this.pictureBoxIncludeAnalysis.Image = global::DiversityCollection.Resource.Analysis;
            resources.ApplyResources(this.pictureBoxIncludeAnalysis, "pictureBoxIncludeAnalysis");
            this.pictureBoxIncludeAnalysis.Name = "pictureBoxIncludeAnalysis";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeAnalysis, ((bool)(resources.GetObject("pictureBoxIncludeAnalysis.ShowHelp"))));
            this.pictureBoxIncludeAnalysis.TabStop = false;
            // 
            // checkBoxIncludeIdentification
            // 
            resources.ApplyResources(this.checkBoxIncludeIdentification, "checkBoxIncludeIdentification");
            this.checkBoxIncludeIdentification.Checked = true;
            this.checkBoxIncludeIdentification.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeIdentification, 2);
            this.checkBoxIncludeIdentification.Name = "checkBoxIncludeIdentification";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeIdentification, ((bool)(resources.GetObject("checkBoxIncludeIdentification.ShowHelp"))));
            this.checkBoxIncludeIdentification.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeAnalysis
            // 
            resources.ApplyResources(this.checkBoxIncludeAnalysis, "checkBoxIncludeAnalysis");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeAnalysis, 2);
            this.checkBoxIncludeAnalysis.Name = "checkBoxIncludeAnalysis";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeAnalysis, ((bool)(resources.GetObject("checkBoxIncludeAnalysis.ShowHelp"))));
            this.checkBoxIncludeAnalysis.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeProcessing
            // 
            resources.ApplyResources(this.checkBoxIncludeProcessing, "checkBoxIncludeProcessing");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeProcessing, 2);
            this.checkBoxIncludeProcessing.Name = "checkBoxIncludeProcessing";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeProcessing, ((bool)(resources.GetObject("checkBoxIncludeProcessing.ShowHelp"))));
            this.checkBoxIncludeProcessing.UseVisualStyleBackColor = true;
            // 
            // pictureBoxIncludeProcessing
            // 
            this.pictureBoxIncludeProcessing.Image = global::DiversityCollection.Resource.Processing;
            resources.ApplyResources(this.pictureBoxIncludeProcessing, "pictureBoxIncludeProcessing");
            this.pictureBoxIncludeProcessing.Name = "pictureBoxIncludeProcessing";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeProcessing, ((bool)(resources.GetObject("pictureBoxIncludeProcessing.ShowHelp"))));
            this.pictureBoxIncludeProcessing.TabStop = false;
            // 
            // pictureBoxIncludeAgent
            // 
            this.pictureBoxIncludeAgent.Image = global::DiversityCollection.Resource.Agent;
            resources.ApplyResources(this.pictureBoxIncludeAgent, "pictureBoxIncludeAgent");
            this.pictureBoxIncludeAgent.Name = "pictureBoxIncludeAgent";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeAgent, ((bool)(resources.GetObject("pictureBoxIncludeAgent.ShowHelp"))));
            this.pictureBoxIncludeAgent.TabStop = false;
            // 
            // checkBoxIncludeAgent
            // 
            resources.ApplyResources(this.checkBoxIncludeAgent, "checkBoxIncludeAgent");
            this.checkBoxIncludeAgent.Checked = true;
            this.checkBoxIncludeAgent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeAgent, 2);
            this.checkBoxIncludeAgent.Name = "checkBoxIncludeAgent";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeAgent, ((bool)(resources.GetObject("checkBoxIncludeAgent.ShowHelp"))));
            this.checkBoxIncludeAgent.UseVisualStyleBackColor = true;
            // 
            // pictureBoxIncludeSpecimenImages
            // 
            this.pictureBoxIncludeSpecimenImages.Image = global::DiversityCollection.Resource.Icones;
            resources.ApplyResources(this.pictureBoxIncludeSpecimenImages, "pictureBoxIncludeSpecimenImages");
            this.pictureBoxIncludeSpecimenImages.Name = "pictureBoxIncludeSpecimenImages";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeSpecimenImages, ((bool)(resources.GetObject("pictureBoxIncludeSpecimenImages.ShowHelp"))));
            this.pictureBoxIncludeSpecimenImages.TabStop = false;
            // 
            // checkBoxIncludeSpecimenImages
            // 
            resources.ApplyResources(this.checkBoxIncludeSpecimenImages, "checkBoxIncludeSpecimenImages");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeSpecimenImages, 2);
            this.checkBoxIncludeSpecimenImages.Name = "checkBoxIncludeSpecimenImages";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeSpecimenImages, ((bool)(resources.GetObject("checkBoxIncludeSpecimenImages.ShowHelp"))));
            this.checkBoxIncludeSpecimenImages.UseVisualStyleBackColor = true;
            // 
            // pictureBoxIncludeEventImages
            // 
            this.pictureBoxIncludeEventImages.Image = global::DiversityCollection.Resource.EventImage;
            resources.ApplyResources(this.pictureBoxIncludeEventImages, "pictureBoxIncludeEventImages");
            this.pictureBoxIncludeEventImages.Name = "pictureBoxIncludeEventImages";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeEventImages, ((bool)(resources.GetObject("pictureBoxIncludeEventImages.ShowHelp"))));
            this.pictureBoxIncludeEventImages.TabStop = false;
            // 
            // pictureBoxIncludeReferences
            // 
            this.pictureBoxIncludeReferences.Image = global::DiversityCollection.Resource.References;
            resources.ApplyResources(this.pictureBoxIncludeReferences, "pictureBoxIncludeReferences");
            this.pictureBoxIncludeReferences.Name = "pictureBoxIncludeReferences";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeReferences, ((bool)(resources.GetObject("pictureBoxIncludeReferences.ShowHelp"))));
            this.pictureBoxIncludeReferences.TabStop = false;
            // 
            // pictureBoxIncludeTransactions
            // 
            this.pictureBoxIncludeTransactions.Image = global::DiversityCollection.Resource.Transaction;
            resources.ApplyResources(this.pictureBoxIncludeTransactions, "pictureBoxIncludeTransactions");
            this.pictureBoxIncludeTransactions.Name = "pictureBoxIncludeTransactions";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeTransactions, ((bool)(resources.GetObject("pictureBoxIncludeTransactions.ShowHelp"))));
            this.pictureBoxIncludeTransactions.TabStop = false;
            // 
            // pictureBoxIncludeAnnotations
            // 
            this.pictureBoxIncludeAnnotations.Image = global::DiversityCollection.Resource.Note1;
            resources.ApplyResources(this.pictureBoxIncludeAnnotations, "pictureBoxIncludeAnnotations");
            this.pictureBoxIncludeAnnotations.InitialImage = global::DiversityCollection.Resource.Note1;
            this.pictureBoxIncludeAnnotations.Name = "pictureBoxIncludeAnnotations";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeAnnotations, ((bool)(resources.GetObject("pictureBoxIncludeAnnotations.ShowHelp"))));
            this.pictureBoxIncludeAnnotations.TabStop = false;
            // 
            // pictureBoxIncludeImageProperties
            // 
            this.pictureBoxIncludeImageProperties.Image = global::DiversityCollection.Resource.ImageArea;
            resources.ApplyResources(this.pictureBoxIncludeImageProperties, "pictureBoxIncludeImageProperties");
            this.pictureBoxIncludeImageProperties.Name = "pictureBoxIncludeImageProperties";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeImageProperties, ((bool)(resources.GetObject("pictureBoxIncludeImageProperties.ShowHelp"))));
            this.pictureBoxIncludeImageProperties.TabStop = false;
            // 
            // checkBoxIncludeImageProperties
            // 
            resources.ApplyResources(this.checkBoxIncludeImageProperties, "checkBoxIncludeImageProperties");
            this.checkBoxIncludeImageProperties.Name = "checkBoxIncludeImageProperties";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeImageProperties, ((bool)(resources.GetObject("checkBoxIncludeImageProperties.ShowHelp"))));
            this.checkBoxIncludeImageProperties.UseVisualStyleBackColor = true;
            this.checkBoxIncludeImageProperties.CheckedChanged += new System.EventHandler(this.checkBoxIncludeImageProperties_CheckedChanged);
            // 
            // checkBoxIncludeExternalIdentifier
            // 
            resources.ApplyResources(this.checkBoxIncludeExternalIdentifier, "checkBoxIncludeExternalIdentifier");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeExternalIdentifier, 2);
            this.checkBoxIncludeExternalIdentifier.Name = "checkBoxIncludeExternalIdentifier";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeExternalIdentifier, ((bool)(resources.GetObject("checkBoxIncludeExternalIdentifier.ShowHelp"))));
            this.checkBoxIncludeExternalIdentifier.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeReferences
            // 
            resources.ApplyResources(this.checkBoxIncludeReferences, "checkBoxIncludeReferences");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeReferences, 2);
            this.checkBoxIncludeReferences.Name = "checkBoxIncludeReferences";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeReferences, ((bool)(resources.GetObject("checkBoxIncludeReferences.ShowHelp"))));
            this.checkBoxIncludeReferences.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeAnnotations
            // 
            resources.ApplyResources(this.checkBoxIncludeAnnotations, "checkBoxIncludeAnnotations");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeAnnotations, 2);
            this.checkBoxIncludeAnnotations.Name = "checkBoxIncludeAnnotations";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeAnnotations, ((bool)(resources.GetObject("checkBoxIncludeAnnotations.ShowHelp"))));
            this.checkBoxIncludeAnnotations.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeTransactions
            // 
            resources.ApplyResources(this.checkBoxIncludeTransactions, "checkBoxIncludeTransactions");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeTransactions, 2);
            this.checkBoxIncludeTransactions.Name = "checkBoxIncludeTransactions";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeTransactions, ((bool)(resources.GetObject("checkBoxIncludeTransactions.ShowHelp"))));
            this.checkBoxIncludeTransactions.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeEventImages
            // 
            resources.ApplyResources(this.checkBoxIncludeEventImages, "checkBoxIncludeEventImages");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeEventImages, 2);
            this.checkBoxIncludeEventImages.Name = "checkBoxIncludeEventImages";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeEventImages, ((bool)(resources.GetObject("checkBoxIncludeEventImages.ShowHelp"))));
            this.checkBoxIncludeEventImages.UseVisualStyleBackColor = true;
            // 
            // pictureBoxIncludeAnalysisMethods
            // 
            this.pictureBoxIncludeAnalysisMethods.Image = global::DiversityCollection.Resource.Tools;
            resources.ApplyResources(this.pictureBoxIncludeAnalysisMethods, "pictureBoxIncludeAnalysisMethods");
            this.pictureBoxIncludeAnalysisMethods.Name = "pictureBoxIncludeAnalysisMethods";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeAnalysisMethods, ((bool)(resources.GetObject("pictureBoxIncludeAnalysisMethods.ShowHelp"))));
            this.pictureBoxIncludeAnalysisMethods.TabStop = false;
            // 
            // checkBoxIncludeAnalysisMethods
            // 
            resources.ApplyResources(this.checkBoxIncludeAnalysisMethods, "checkBoxIncludeAnalysisMethods");
            this.checkBoxIncludeAnalysisMethods.Name = "checkBoxIncludeAnalysisMethods";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeAnalysisMethods, ((bool)(resources.GetObject("checkBoxIncludeAnalysisMethods.ShowHelp"))));
            this.checkBoxIncludeAnalysisMethods.UseVisualStyleBackColor = true;
            this.checkBoxIncludeAnalysisMethods.CheckedChanged += new System.EventHandler(this.checkBoxIncludeAnalysisMethods_CheckedChanged);
            // 
            // pictureBoxIncludeProcessingMethods
            // 
            this.pictureBoxIncludeProcessingMethods.Image = global::DiversityCollection.Resource.Tools;
            resources.ApplyResources(this.pictureBoxIncludeProcessingMethods, "pictureBoxIncludeProcessingMethods");
            this.pictureBoxIncludeProcessingMethods.Name = "pictureBoxIncludeProcessingMethods";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeProcessingMethods, ((bool)(resources.GetObject("pictureBoxIncludeProcessingMethods.ShowHelp"))));
            this.pictureBoxIncludeProcessingMethods.TabStop = false;
            // 
            // checkBoxIncludeProcessingMethods
            // 
            resources.ApplyResources(this.checkBoxIncludeProcessingMethods, "checkBoxIncludeProcessingMethods");
            this.checkBoxIncludeProcessingMethods.Name = "checkBoxIncludeProcessingMethods";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeProcessingMethods, ((bool)(resources.GetObject("checkBoxIncludeProcessingMethods.ShowHelp"))));
            this.checkBoxIncludeProcessingMethods.UseVisualStyleBackColor = true;
            this.checkBoxIncludeProcessingMethods.CheckedChanged += new System.EventHandler(this.checkBoxIncludeProcessingMethods_CheckedChanged);
            // 
            // labelInclude
            // 
            resources.ApplyResources(this.labelInclude, "labelInclude");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.labelInclude, 3);
            this.labelInclude.Name = "labelInclude";
            this.helpProvider.SetShowHelp(this.labelInclude, ((bool)(resources.GetObject("labelInclude.ShowHelp"))));
            // 
            // pictureBoxIncludeRelations
            // 
            resources.ApplyResources(this.pictureBoxIncludeRelations, "pictureBoxIncludeRelations");
            this.pictureBoxIncludeRelations.Image = global::DiversityCollection.Resource.Relation;
            this.pictureBoxIncludeRelations.Name = "pictureBoxIncludeRelations";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeRelations, ((bool)(resources.GetObject("pictureBoxIncludeRelations.ShowHelp"))));
            this.pictureBoxIncludeRelations.TabStop = false;
            // 
            // checkBoxIncludeRelations
            // 
            resources.ApplyResources(this.checkBoxIncludeRelations, "checkBoxIncludeRelations");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeRelations, 2);
            this.checkBoxIncludeRelations.Name = "checkBoxIncludeRelations";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeRelations, ((bool)(resources.GetObject("checkBoxIncludeRelations.ShowHelp"))));
            this.checkBoxIncludeRelations.UseVisualStyleBackColor = true;
            // 
            // pictureBoxIncludeEventMethod
            // 
            resources.ApplyResources(this.pictureBoxIncludeEventMethod, "pictureBoxIncludeEventMethod");
            this.pictureBoxIncludeEventMethod.Image = global::DiversityCollection.Resource.EventMethod;
            this.pictureBoxIncludeEventMethod.Name = "pictureBoxIncludeEventMethod";
            this.pictureBoxIncludeEventMethod.TabStop = false;
            // 
            // checkBoxIncludeEventMethod
            // 
            resources.ApplyResources(this.checkBoxIncludeEventMethod, "checkBoxIncludeEventMethod");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeEventMethod, 2);
            this.checkBoxIncludeEventMethod.Name = "checkBoxIncludeEventMethod";
            this.checkBoxIncludeEventMethod.UseVisualStyleBackColor = true;
            // 
            // userControlDialogPanel
            // 
            resources.ApplyResources(this.userControlDialogPanel, "userControlDialogPanel");
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.helpProvider.SetShowHelp(this.userControlDialogPanel, ((bool)(resources.GetObject("userControlDialogPanel.ShowHelp"))));
            // 
            // FormCopyDataset
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonFeedback);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.helpProvider.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
            this.helpProvider.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.Name = "FormCopyDataset";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.ShowInTaskbar = false;
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.groupBoxCopy.ResumeLayout(false);
            this.groupBoxOriginal.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageEvent.ResumeLayout(false);
            this.tabPageAccNr.ResumeLayout(false);
            this.tableLayoutPanelAccNr.ResumeLayout(false);
            this.tableLayoutPanelAccNr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAccNr)).EndInit();
            this.tabPageMultiCopy.ResumeLayout(false);
            this.splitContainerNumberOfCopies.Panel1.ResumeLayout(false);
            this.splitContainerNumberOfCopies.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNumberOfCopies)).EndInit();
            this.splitContainerNumberOfCopies.ResumeLayout(false);
            this.tableLayoutPanelNumberOfCopies.ResumeLayout(false);
            this.tableLayoutPanelNumberOfCopies.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumberOfCopies)).EndInit();
            this.tabPageProjects.ResumeLayout(false);
            this.tableLayoutPanelProjects.ResumeLayout(false);
            this.tableLayoutPanelProjects.PerformLayout();
            this.tabPageRelations.ResumeLayout(false);
            this.tableLayoutPanelRelations.ResumeLayout(false);
            this.tableLayoutPanelRelations.PerformLayout();
            this.groupBoxRelationFromOri.ResumeLayout(false);
            this.groupBoxRelationFromOri.PerformLayout();
            this.groupBoxRelationToOri.ResumeLayout(false);
            this.groupBoxRelationToOri.PerformLayout();
            this.tabPageIncludedData.ResumeLayout(false);
            this.tableLayoutPanelIncluded.ResumeLayout(false);
            this.tableLayoutPanelIncluded.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeExternalIdentifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeIdentification)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeAnalysis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeProcessing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeAgent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeSpecimenImages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeEventImages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeReferences)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeTransactions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeAnnotations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeImageProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeAnalysisMethods)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeProcessingMethods)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeRelations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeEventMethod)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TreeView treeViewNewEvent;
        private System.Windows.Forms.TreeView treeViewSameEvent;
        private System.Windows.Forms.Button buttonSameEvent;
        private System.Windows.Forms.TreeView treeViewNoEvent;
        private System.Windows.Forms.Button buttonNoEvent;
        private System.Windows.Forms.Button buttonNewEvent;
        private System.Windows.Forms.Label labelAccessionNumber;
        private System.Windows.Forms.TextBox textBoxAccessionNumber;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageAccNr;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAccNr;
        private System.Windows.Forms.TabPage tabPageEvent;
        private System.Windows.Forms.Label labelHeaderEvent;
        private System.Windows.Forms.Label labelHeaderAccNr;
        private System.Windows.Forms.CheckBox checkBoxCopyIdentifactions;
        private System.Windows.Forms.PictureBox pictureBoxAccNr;
        private System.Windows.Forms.Button buttonAccessionNumberSearch;
        private System.Windows.Forms.TextBox textBoxAccNrInitials;
        private System.Windows.Forms.CheckBox checkBoxAccNr;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelAccNrExplain1;
        private System.Windows.Forms.Label labelAccNrExplain2;
        private System.Windows.Forms.Label labelAccNrExplain3;
        private System.Windows.Forms.TreeView treeViewOriginal;
        private System.Windows.Forms.TreeView treeViewCopy;
        private System.Windows.Forms.GroupBox groupBoxCopy;
        private System.Windows.Forms.GroupBox groupBoxOriginal;
        private System.Windows.Forms.TabPage tabPageMultiCopy;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNumberOfCopies;
        private System.Windows.Forms.NumericUpDown numericUpDownNumberOfCopies;
        private System.Windows.Forms.Label labelNumberOfCopies;
        private System.Windows.Forms.Label labelNumberOfCopiesHeader;
        private System.Windows.Forms.CheckedListBox checkedListBoxMultiCopyAccNr;
        private System.Windows.Forms.TabPage tabPageProjects;
        private System.Windows.Forms.TabPage tabPageRelations;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProjects;
        private System.Windows.Forms.Label labelProjectsHeader;
        private System.Windows.Forms.CheckedListBox checkedListBoxProjects;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRelations;
        private System.Windows.Forms.Label labelRelationsHeader;
        private System.Windows.Forms.TreeView treeViewRelationToOriginal;
        private System.Windows.Forms.ImageList imageListRelations;
        private System.Windows.Forms.CheckBox checkBoxRelationToOriginal;
        private System.Windows.Forms.ComboBox comboBoxRelationTypeToOriginal;
        private System.Windows.Forms.CheckBox checkBoxRelationFromOriginal;
        private System.Windows.Forms.ComboBox comboBoxRelationTypeFromOriginal;
        private System.Windows.Forms.TreeView treeViewRelationFromOriginal;
        private System.Windows.Forms.GroupBox groupBoxRelationFromOri;
        private System.Windows.Forms.GroupBox groupBoxRelationToOri;
        private System.Windows.Forms.Button buttonRequeryMultiCopyList;
        private System.Windows.Forms.SplitContainer splitContainerNumberOfCopies;
        private System.Windows.Forms.TabPage tabPageIncludedData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelIncluded;
        private System.Windows.Forms.CheckBox checkBoxIncludeSpecimenImages;
        private System.Windows.Forms.PictureBox pictureBoxIncludeSpecimenImages;
        private System.Windows.Forms.CheckBox checkBoxIncludeAnalysis;
        private System.Windows.Forms.PictureBox pictureBoxIncludeAnalysis;
        private System.Windows.Forms.CheckBox checkBoxIncludeProcessing;
        private System.Windows.Forms.PictureBox pictureBoxIncludeProcessing;
        private System.Windows.Forms.CheckBox checkBoxIncludeExternalIdentifier;
        private System.Windows.Forms.PictureBox pictureBoxIncludeExternalIdentifier;
        private System.Windows.Forms.PictureBox pictureBoxIncludeAnnotations;
        private System.Windows.Forms.CheckBox checkBoxIncludeAnnotations;
        private System.Windows.Forms.PictureBox pictureBoxIncludeTransactions;
        private System.Windows.Forms.PictureBox pictureBoxIncludeReferences;
        private System.Windows.Forms.CheckBox checkBoxIncludeTransactions;
        private System.Windows.Forms.CheckBox checkBoxIncludeReferences;
        private System.Windows.Forms.PictureBox pictureBoxIncludeAgent;
        private System.Windows.Forms.PictureBox pictureBoxIncludeIdentification;
        private System.Windows.Forms.CheckBox checkBoxIncludeIdentification;
        private System.Windows.Forms.CheckBox checkBoxIncludeAgent;
        private System.Windows.Forms.PictureBox pictureBoxIncludeEventImages;
        private System.Windows.Forms.PictureBox pictureBoxIncludeImageProperties;
        private System.Windows.Forms.CheckBox checkBoxIncludeImageProperties;
        private System.Windows.Forms.CheckBox checkBoxIncludeEventImages;
        private System.Windows.Forms.PictureBox pictureBoxIncludeAnalysisMethods;
        private System.Windows.Forms.CheckBox checkBoxIncludeAnalysisMethods;
        private System.Windows.Forms.PictureBox pictureBoxIncludeProcessingMethods;
        private System.Windows.Forms.CheckBox checkBoxIncludeProcessingMethods;
        private System.Windows.Forms.Label labelInclude;
        private System.Windows.Forms.PictureBox pictureBoxIncludeRelations;
        private System.Windows.Forms.CheckBox checkBoxIncludeRelations;
        private System.Windows.Forms.CheckBox checkBoxAccessionNumberSearchIncludePart;
        private System.Windows.Forms.Button buttonIncludeAll;
        private System.Windows.Forms.Button buttonIncludeNone;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.PictureBox pictureBoxIncludeEventMethod;
        private System.Windows.Forms.CheckBox checkBoxIncludeEventMethod;
        private System.Windows.Forms.Button buttonOnlyEvent;
        private System.Windows.Forms.TreeView treeViewOnlyEvent;
    }
}