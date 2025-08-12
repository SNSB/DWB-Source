namespace DiversityCollection.Forms
{
    partial class FormCopyPart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCopyPart));
            this.splitContainerNumberOfCopies = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelNumberOfCopies = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownNumberOfCopies = new System.Windows.Forms.NumericUpDown();
            this.labelNumberOfCopiesHeader = new System.Windows.Forms.Label();
            this.buttonRequeryMultiCopyList = new System.Windows.Forms.Button();
            this.dataGridViewCopies = new System.Windows.Forms.DataGridView();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageMultiCopy = new System.Windows.Forms.TabPage();
            this.tabPageIncludedData = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelIncluded = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxIncludeExternalIdentifier = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeRegulation = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeRegulation = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeProcessing = new System.Windows.Forms.CheckBox();
            this.pictureBoxIncludeProcessing = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeSpecimenPartDescription = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeReferences = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeTransactions = new System.Windows.Forms.PictureBox();
            this.pictureBoxIncludeAnnotations = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeExternalIdentifier = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeReferences = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeAnnotations = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeTransactions = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeSpecimenPartDescription = new System.Windows.Forms.CheckBox();
            this.pictureBoxIncludeProcessingMethods = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeProcessingMethods = new System.Windows.Forms.CheckBox();
            this.labelInclude = new System.Windows.Forms.Label();
            this.pictureBoxIncludeRelations = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeRelations = new System.Windows.Forms.CheckBox();
            this.pictureBoxUnitInPart = new System.Windows.Forms.PictureBox();
            this.checkBoxIncludeUnitInPart = new System.Windows.Forms.CheckBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainerNumberOfCopies.Panel1.SuspendLayout();
            this.splitContainerNumberOfCopies.Panel2.SuspendLayout();
            this.splitContainerNumberOfCopies.SuspendLayout();
            this.tableLayoutPanelNumberOfCopies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumberOfCopies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCopies)).BeginInit();
            this.tabControlMain.SuspendLayout();
            this.tabPageMultiCopy.SuspendLayout();
            this.tabPageIncludedData.SuspendLayout();
            this.tableLayoutPanelIncluded.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeExternalIdentifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeRegulation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeProcessing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeSpecimenPartDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeReferences)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeTransactions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeAnnotations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeProcessingMethods)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeRelations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUnitInPart)).BeginInit();
            this.SuspendLayout();
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
            this.splitContainerNumberOfCopies.Panel2.Controls.Add(this.dataGridViewCopies);
            this.helpProvider.SetShowHelp(this.splitContainerNumberOfCopies.Panel2, ((bool)(resources.GetObject("splitContainerNumberOfCopies.Panel2.ShowHelp"))));
            this.helpProvider.SetShowHelp(this.splitContainerNumberOfCopies, ((bool)(resources.GetObject("splitContainerNumberOfCopies.ShowHelp"))));
            // 
            // tableLayoutPanelNumberOfCopies
            // 
            resources.ApplyResources(this.tableLayoutPanelNumberOfCopies, "tableLayoutPanelNumberOfCopies");
            this.tableLayoutPanelNumberOfCopies.Controls.Add(this.numericUpDownNumberOfCopies, 1, 0);
            this.tableLayoutPanelNumberOfCopies.Controls.Add(this.labelNumberOfCopiesHeader, 0, 0);
            this.tableLayoutPanelNumberOfCopies.Controls.Add(this.buttonRequeryMultiCopyList, 2, 0);
            this.tableLayoutPanelNumberOfCopies.Name = "tableLayoutPanelNumberOfCopies";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelNumberOfCopies, ((bool)(resources.GetObject("tableLayoutPanelNumberOfCopies.ShowHelp"))));
            // 
            // numericUpDownNumberOfCopies
            // 
            resources.ApplyResources(this.numericUpDownNumberOfCopies, "numericUpDownNumberOfCopies");
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
            // labelNumberOfCopiesHeader
            // 
            resources.ApplyResources(this.labelNumberOfCopiesHeader, "labelNumberOfCopiesHeader");
            this.labelNumberOfCopiesHeader.Name = "labelNumberOfCopiesHeader";
            this.helpProvider.SetShowHelp(this.labelNumberOfCopiesHeader, ((bool)(resources.GetObject("labelNumberOfCopiesHeader.ShowHelp"))));
            // 
            // buttonRequeryMultiCopyList
            // 
            resources.ApplyResources(this.buttonRequeryMultiCopyList, "buttonRequeryMultiCopyList");
            this.buttonRequeryMultiCopyList.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.buttonRequeryMultiCopyList.FlatAppearance.BorderSize = 0;
            this.buttonRequeryMultiCopyList.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRequeryMultiCopyList.Name = "buttonRequeryMultiCopyList";
            this.helpProvider.SetShowHelp(this.buttonRequeryMultiCopyList, ((bool)(resources.GetObject("buttonRequeryMultiCopyList.ShowHelp"))));
            this.buttonRequeryMultiCopyList.UseVisualStyleBackColor = true;
            this.buttonRequeryMultiCopyList.Click += new System.EventHandler(this.buttonRequeryMultiCopyList_Click);
            // 
            // dataGridViewCopies
            // 
            this.dataGridViewCopies.AllowUserToAddRows = false;
            this.dataGridViewCopies.AllowUserToDeleteRows = false;
            this.dataGridViewCopies.AllowUserToResizeRows = false;
            this.dataGridViewCopies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewCopies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dataGridViewCopies, "dataGridViewCopies");
            this.dataGridViewCopies.Name = "dataGridViewCopies";
            this.dataGridViewCopies.RowHeadersVisible = false;
            this.helpProvider.SetShowHelp(this.dataGridViewCopies, ((bool)(resources.GetObject("dataGridViewCopies.ShowHelp"))));
            // 
            // tabControlMain
            // 
            resources.ApplyResources(this.tabControlMain, "tabControlMain");
            this.tabControlMain.Controls.Add(this.tabPageMultiCopy);
            this.tabControlMain.Controls.Add(this.tabPageIncludedData);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.helpProvider.SetShowHelp(this.tabControlMain, ((bool)(resources.GetObject("tabControlMain.ShowHelp"))));
            // 
            // tabPageMultiCopy
            // 
            this.tabPageMultiCopy.Controls.Add(this.splitContainerNumberOfCopies);
            resources.ApplyResources(this.tabPageMultiCopy, "tabPageMultiCopy");
            this.tabPageMultiCopy.Name = "tabPageMultiCopy";
            this.helpProvider.SetShowHelp(this.tabPageMultiCopy, ((bool)(resources.GetObject("tabPageMultiCopy.ShowHelp"))));
            this.tabPageMultiCopy.UseVisualStyleBackColor = true;
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
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeExternalIdentifier, 0, 9);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeRegulation, 0, 2);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeRegulation, 1, 2);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeProcessing, 1, 3);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeProcessing, 0, 3);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeSpecimenPartDescription, 0, 5);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeReferences, 0, 7);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeTransactions, 0, 8);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeAnnotations, 0, 10);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeExternalIdentifier, 1, 9);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeReferences, 1, 7);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeAnnotations, 1, 10);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeTransactions, 1, 8);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeSpecimenPartDescription, 1, 5);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeProcessingMethods, 1, 4);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeProcessingMethods, 2, 4);
            this.tableLayoutPanelIncluded.Controls.Add(this.labelInclude, 0, 0);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxIncludeRelations, 0, 6);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeRelations, 1, 6);
            this.tableLayoutPanelIncluded.Controls.Add(this.pictureBoxUnitInPart, 0, 1);
            this.tableLayoutPanelIncluded.Controls.Add(this.checkBoxIncludeUnitInPart, 1, 1);
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
            // pictureBoxIncludeRegulation
            // 
            this.pictureBoxIncludeRegulation.Image = global::DiversityCollection.Resource.Paragraph;
            resources.ApplyResources(this.pictureBoxIncludeRegulation, "pictureBoxIncludeRegulation");
            this.pictureBoxIncludeRegulation.Name = "pictureBoxIncludeRegulation";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeRegulation, ((bool)(resources.GetObject("pictureBoxIncludeRegulation.ShowHelp"))));
            this.pictureBoxIncludeRegulation.TabStop = false;
            // 
            // checkBoxIncludeRegulation
            // 
            resources.ApplyResources(this.checkBoxIncludeRegulation, "checkBoxIncludeRegulation");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeRegulation, 2);
            this.checkBoxIncludeRegulation.Name = "checkBoxIncludeRegulation";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeRegulation, ((bool)(resources.GetObject("checkBoxIncludeRegulation.ShowHelp"))));
            this.checkBoxIncludeRegulation.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeProcessing
            // 
            resources.ApplyResources(this.checkBoxIncludeProcessing, "checkBoxIncludeProcessing");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeProcessing, 2);
            this.checkBoxIncludeProcessing.Name = "checkBoxIncludeProcessing";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeProcessing, ((bool)(resources.GetObject("checkBoxIncludeProcessing.ShowHelp"))));
            this.checkBoxIncludeProcessing.UseVisualStyleBackColor = true;
            this.checkBoxIncludeProcessing.CheckedChanged += new System.EventHandler(this.checkBoxIncludeProcessing_CheckedChanged);
            // 
            // pictureBoxIncludeProcessing
            // 
            this.pictureBoxIncludeProcessing.Image = global::DiversityCollection.Resource.Processing;
            resources.ApplyResources(this.pictureBoxIncludeProcessing, "pictureBoxIncludeProcessing");
            this.pictureBoxIncludeProcessing.Name = "pictureBoxIncludeProcessing";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeProcessing, ((bool)(resources.GetObject("pictureBoxIncludeProcessing.ShowHelp"))));
            this.pictureBoxIncludeProcessing.TabStop = false;
            // 
            // pictureBoxIncludeSpecimenPartDescription
            // 
            this.pictureBoxIncludeSpecimenPartDescription.Image = global::DiversityCollection.Resource.Dictionary;
            resources.ApplyResources(this.pictureBoxIncludeSpecimenPartDescription, "pictureBoxIncludeSpecimenPartDescription");
            this.pictureBoxIncludeSpecimenPartDescription.Name = "pictureBoxIncludeSpecimenPartDescription";
            this.helpProvider.SetShowHelp(this.pictureBoxIncludeSpecimenPartDescription, ((bool)(resources.GetObject("pictureBoxIncludeSpecimenPartDescription.ShowHelp"))));
            this.pictureBoxIncludeSpecimenPartDescription.TabStop = false;
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
            // checkBoxIncludeSpecimenPartDescription
            // 
            resources.ApplyResources(this.checkBoxIncludeSpecimenPartDescription, "checkBoxIncludeSpecimenPartDescription");
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeSpecimenPartDescription, 2);
            this.checkBoxIncludeSpecimenPartDescription.Name = "checkBoxIncludeSpecimenPartDescription";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeSpecimenPartDescription, ((bool)(resources.GetObject("checkBoxIncludeSpecimenPartDescription.ShowHelp"))));
            this.checkBoxIncludeSpecimenPartDescription.UseVisualStyleBackColor = true;
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
            // pictureBoxUnitInPart
            // 
            this.pictureBoxUnitInPart.Image = global::DiversityCollection.Resource.UnitInPart;
            resources.ApplyResources(this.pictureBoxUnitInPart, "pictureBoxUnitInPart");
            this.pictureBoxUnitInPart.Name = "pictureBoxUnitInPart";
            this.helpProvider.SetShowHelp(this.pictureBoxUnitInPart, ((bool)(resources.GetObject("pictureBoxUnitInPart.ShowHelp"))));
            this.pictureBoxUnitInPart.TabStop = false;
            // 
            // checkBoxIncludeUnitInPart
            // 
            resources.ApplyResources(this.checkBoxIncludeUnitInPart, "checkBoxIncludeUnitInPart");
            this.checkBoxIncludeUnitInPart.Checked = true;
            this.checkBoxIncludeUnitInPart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelIncluded.SetColumnSpan(this.checkBoxIncludeUnitInPart, 2);
            this.checkBoxIncludeUnitInPart.Name = "checkBoxIncludeUnitInPart";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeUnitInPart, ((bool)(resources.GetObject("checkBoxIncludeUnitInPart.ShowHelp"))));
            this.checkBoxIncludeUnitInPart.UseVisualStyleBackColor = true;
            // 
            // userControlDialogPanel
            // 
            resources.ApplyResources(this.userControlDialogPanel, "userControlDialogPanel");
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.helpProvider.SetShowHelp(this.userControlDialogPanel, ((bool)(resources.GetObject("userControlDialogPanel.ShowHelp"))));
            // 
            // FormCopyPart
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Name = "FormCopyPart";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.splitContainerNumberOfCopies.Panel1.ResumeLayout(false);
            this.splitContainerNumberOfCopies.Panel2.ResumeLayout(false);
            this.splitContainerNumberOfCopies.ResumeLayout(false);
            this.tableLayoutPanelNumberOfCopies.ResumeLayout(false);
            this.tableLayoutPanelNumberOfCopies.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumberOfCopies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCopies)).EndInit();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageMultiCopy.ResumeLayout(false);
            this.tabPageIncludedData.ResumeLayout(false);
            this.tableLayoutPanelIncluded.ResumeLayout(false);
            this.tableLayoutPanelIncluded.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeExternalIdentifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeRegulation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeProcessing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeSpecimenPartDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeReferences)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeTransactions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeAnnotations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeProcessingMethods)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIncludeRelations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUnitInPart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageMultiCopy;
        private System.Windows.Forms.SplitContainer splitContainerNumberOfCopies;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNumberOfCopies;
        private System.Windows.Forms.NumericUpDown numericUpDownNumberOfCopies;
        private System.Windows.Forms.Label labelNumberOfCopiesHeader;
        private System.Windows.Forms.Button buttonRequeryMultiCopyList;
        private System.Windows.Forms.TabPage tabPageIncludedData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelIncluded;
        private System.Windows.Forms.PictureBox pictureBoxIncludeExternalIdentifier;
        private System.Windows.Forms.CheckBox checkBoxIncludeProcessing;
        private System.Windows.Forms.PictureBox pictureBoxIncludeProcessing;
        private System.Windows.Forms.PictureBox pictureBoxIncludeSpecimenPartDescription;
        private System.Windows.Forms.PictureBox pictureBoxIncludeReferences;
        private System.Windows.Forms.PictureBox pictureBoxIncludeTransactions;
        private System.Windows.Forms.PictureBox pictureBoxIncludeAnnotations;
        private System.Windows.Forms.CheckBox checkBoxIncludeExternalIdentifier;
        private System.Windows.Forms.CheckBox checkBoxIncludeReferences;
        private System.Windows.Forms.CheckBox checkBoxIncludeAnnotations;
        private System.Windows.Forms.CheckBox checkBoxIncludeTransactions;
        private System.Windows.Forms.CheckBox checkBoxIncludeSpecimenPartDescription;
        private System.Windows.Forms.PictureBox pictureBoxIncludeProcessingMethods;
        private System.Windows.Forms.CheckBox checkBoxIncludeProcessingMethods;
        private System.Windows.Forms.Label labelInclude;
        private System.Windows.Forms.PictureBox pictureBoxIncludeRelations;
        private System.Windows.Forms.CheckBox checkBoxIncludeRelations;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.DataGridView dataGridViewCopies;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.PictureBox pictureBoxIncludeRegulation;
        private System.Windows.Forms.CheckBox checkBoxIncludeRegulation;
        private System.Windows.Forms.PictureBox pictureBoxUnitInPart;
        private System.Windows.Forms.CheckBox checkBoxIncludeUnitInPart;
        private System.Windows.Forms.ToolTip toolTip;
    }
}