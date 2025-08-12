namespace DiversityCollection.Forms
{
    partial class FormTaxonList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaxonList));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelTaxonomicGroup = new System.Windows.Forms.Label();
            this.comboBoxTaxonomicGroup = new System.Windows.Forms.ComboBox();
            this.dataGridViewTaxa = new System.Windows.Forms.DataGridView();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.checkBoxIncludeTaxonLists = new System.Windows.Forms.CheckBox();
            this.comboBoxDatabase = new System.Windows.Forms.ComboBox();
            this.labelTaxonLists = new System.Windows.Forms.Label();
            this.labelAnalysis = new System.Windows.Forms.Label();
            this.comboBoxTaxonList = new System.Windows.Forms.ComboBox();
            this.comboBoxAnalysis = new System.Windows.Forms.ComboBox();
            this.buttonRequeryTaxonList = new System.Windows.Forms.Button();
            this.pictureBoxTaxonomicGroup = new System.Windows.Forms.PictureBox();
            this.pictureBoxSource = new System.Windows.Forms.PictureBox();
            this.textBoxTaxonListFile = new System.Windows.Forms.TextBox();
            this.labelSaveAs = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.radioButtonCountNumberOfUnits = new System.Windows.Forms.RadioButton();
            this.radioButtonCountUnits = new System.Windows.Forms.RadioButton();
            this.checkBoxIncludeGender = new System.Windows.Forms.CheckBox();
            this.checkBoxFrom = new System.Windows.Forms.CheckBox();
            this.checkBoxUntil = new System.Windows.Forms.CheckBox();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerUntil = new System.Windows.Forms.DateTimePicker();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.labelAnalysisDescription = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTaxa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTaxonomicGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 7;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.labelTaxonomicGroup, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxTaxonomicGroup, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.dataGridViewTaxa, 0, 8);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelDatabase, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxIncludeTaxonLists, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxDatabase, 2, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelTaxonLists, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.labelAnalysis, 0, 6);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxTaxonList, 2, 5);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxAnalysis, 2, 6);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRequeryTaxonList, 6, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxTaxonomicGroup, 6, 1);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxSource, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTaxonListFile, 2, 9);
            this.tableLayoutPanelMain.Controls.Add(this.labelSaveAs, 0, 9);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSave, 6, 9);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonCountNumberOfUnits, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonCountUnits, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxIncludeGender, 3, 2);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxFrom, 4, 2);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxUntil, 4, 3);
            this.tableLayoutPanelMain.Controls.Add(this.dateTimePickerFrom, 5, 2);
            this.tableLayoutPanelMain.Controls.Add(this.dateTimePickerUntil, 5, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelAnalysisDescription, 2, 7);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpKeyword(this.tableLayoutPanelMain, "Taxon list");
            this.helpProvider.SetHelpNavigator(this.tableLayoutPanelMain, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 10;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.helpProvider.SetShowHelp(this.tableLayoutPanelMain, true);
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(554, 531);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // labelTaxonomicGroup
            // 
            this.labelTaxonomicGroup.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelTaxonomicGroup, 2);
            this.labelTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaxonomicGroup.Location = new System.Drawing.Point(3, 24);
            this.labelTaxonomicGroup.Name = "labelTaxonomicGroup";
            this.labelTaxonomicGroup.Size = new System.Drawing.Size(92, 27);
            this.labelTaxonomicGroup.TabIndex = 1;
            this.labelTaxonomicGroup.Text = "Taxonomic group:";
            this.labelTaxonomicGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxTaxonomicGroup
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.comboBoxTaxonomicGroup, 4);
            this.comboBoxTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTaxonomicGroup.FormattingEnabled = true;
            this.comboBoxTaxonomicGroup.Location = new System.Drawing.Point(101, 27);
            this.comboBoxTaxonomicGroup.Name = "comboBoxTaxonomicGroup";
            this.comboBoxTaxonomicGroup.Size = new System.Drawing.Size(423, 21);
            this.comboBoxTaxonomicGroup.TabIndex = 2;
            this.comboBoxTaxonomicGroup.SelectionChangeCommitted += new System.EventHandler(this.comboBoxTaxonomicGroup_SelectionChangeCommitted);
            // 
            // dataGridViewTaxa
            // 
            this.dataGridViewTaxa.AllowUserToAddRows = false;
            this.dataGridViewTaxa.AllowUserToDeleteRows = false;
            this.dataGridViewTaxa.AllowUserToResizeRows = false;
            this.dataGridViewTaxa.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelMain.SetColumnSpan(this.dataGridViewTaxa, 7);
            this.dataGridViewTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTaxa.Location = new System.Drawing.Point(3, 200);
            this.dataGridViewTaxa.Name = "dataGridViewTaxa";
            this.dataGridViewTaxa.ReadOnly = true;
            this.dataGridViewTaxa.RowHeadersVisible = false;
            this.dataGridViewTaxa.Size = new System.Drawing.Size(548, 302);
            this.dataGridViewTaxa.TabIndex = 3;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 5);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(25, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(499, 24);
            this.labelHeader.TabIndex = 4;
            this.labelHeader.Text = "Taxon list";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelDatabase, 2);
            this.labelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDatabase.Location = new System.Drawing.Point(3, 103);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(92, 27);
            this.labelDatabase.TabIndex = 5;
            this.labelDatabase.Text = "Database:";
            this.labelDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelDatabase.Visible = false;
            // 
            // checkBoxIncludeTaxonLists
            // 
            this.checkBoxIncludeTaxonLists.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxIncludeTaxonLists, 3);
            this.checkBoxIncludeTaxonLists.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxIncludeTaxonLists.Location = new System.Drawing.Point(3, 80);
            this.checkBoxIncludeTaxonLists.Name = "checkBoxIncludeTaxonLists";
            this.checkBoxIncludeTaxonLists.Size = new System.Drawing.Size(209, 20);
            this.checkBoxIncludeTaxonLists.TabIndex = 6;
            this.checkBoxIncludeTaxonLists.Text = "Include information from taxonomic lists";
            this.checkBoxIncludeTaxonLists.UseVisualStyleBackColor = true;
            this.checkBoxIncludeTaxonLists.Visible = false;
            this.checkBoxIncludeTaxonLists.CheckedChanged += new System.EventHandler(this.checkBoxIncludeTaxonLists_CheckedChanged);
            // 
            // comboBoxDatabase
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.comboBoxDatabase, 5);
            this.comboBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDatabase.FormattingEnabled = true;
            this.comboBoxDatabase.Location = new System.Drawing.Point(101, 106);
            this.comboBoxDatabase.Name = "comboBoxDatabase";
            this.comboBoxDatabase.Size = new System.Drawing.Size(450, 21);
            this.comboBoxDatabase.TabIndex = 7;
            this.comboBoxDatabase.Visible = false;
            this.comboBoxDatabase.SelectionChangeCommitted += new System.EventHandler(this.comboBoxDatabase_SelectionChangeCommitted);
            // 
            // labelTaxonLists
            // 
            this.labelTaxonLists.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelTaxonLists, 2);
            this.labelTaxonLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaxonLists.Location = new System.Drawing.Point(3, 130);
            this.labelTaxonLists.Name = "labelTaxonLists";
            this.labelTaxonLists.Size = new System.Drawing.Size(92, 27);
            this.labelTaxonLists.TabIndex = 8;
            this.labelTaxonLists.Text = "Taxon list:";
            this.labelTaxonLists.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelTaxonLists.Visible = false;
            // 
            // labelAnalysis
            // 
            this.labelAnalysis.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelAnalysis, 2);
            this.labelAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnalysis.Location = new System.Drawing.Point(3, 157);
            this.labelAnalysis.Name = "labelAnalysis";
            this.labelAnalysis.Size = new System.Drawing.Size(92, 27);
            this.labelAnalysis.TabIndex = 9;
            this.labelAnalysis.Text = "Analysis:";
            this.labelAnalysis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelAnalysis.Visible = false;
            // 
            // comboBoxTaxonList
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.comboBoxTaxonList, 5);
            this.comboBoxTaxonList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTaxonList.FormattingEnabled = true;
            this.comboBoxTaxonList.Location = new System.Drawing.Point(101, 133);
            this.comboBoxTaxonList.Name = "comboBoxTaxonList";
            this.comboBoxTaxonList.Size = new System.Drawing.Size(450, 21);
            this.comboBoxTaxonList.TabIndex = 10;
            this.comboBoxTaxonList.Visible = false;
            this.comboBoxTaxonList.SelectionChangeCommitted += new System.EventHandler(this.comboBoxTaxonList_SelectionChangeCommitted);
            // 
            // comboBoxAnalysis
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.comboBoxAnalysis, 5);
            this.comboBoxAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAnalysis.FormattingEnabled = true;
            this.comboBoxAnalysis.Location = new System.Drawing.Point(101, 160);
            this.comboBoxAnalysis.Name = "comboBoxAnalysis";
            this.comboBoxAnalysis.Size = new System.Drawing.Size(450, 21);
            this.comboBoxAnalysis.TabIndex = 11;
            this.comboBoxAnalysis.Visible = false;
            this.comboBoxAnalysis.SelectionChangeCommitted += new System.EventHandler(this.comboBoxAnalysis_SelectionChangeCommitted);
            // 
            // buttonRequeryTaxonList
            // 
            this.buttonRequeryTaxonList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRequeryTaxonList.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRequeryTaxonList.Location = new System.Drawing.Point(527, 0);
            this.buttonRequeryTaxonList.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRequeryTaxonList.Name = "buttonRequeryTaxonList";
            this.buttonRequeryTaxonList.Size = new System.Drawing.Size(27, 24);
            this.buttonRequeryTaxonList.TabIndex = 12;
            this.buttonRequeryTaxonList.UseVisualStyleBackColor = true;
            this.buttonRequeryTaxonList.Click += new System.EventHandler(this.buttonRequeryTaxonList_Click);
            // 
            // pictureBoxTaxonomicGroup
            // 
            this.pictureBoxTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTaxonomicGroup.Location = new System.Drawing.Point(527, 24);
            this.pictureBoxTaxonomicGroup.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxTaxonomicGroup.Name = "pictureBoxTaxonomicGroup";
            this.pictureBoxTaxonomicGroup.Padding = new System.Windows.Forms.Padding(4);
            this.pictureBoxTaxonomicGroup.Size = new System.Drawing.Size(27, 27);
            this.pictureBoxTaxonomicGroup.TabIndex = 13;
            this.pictureBoxTaxonomicGroup.TabStop = false;
            // 
            // pictureBoxSource
            // 
            this.pictureBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSource.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSource.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxSource.Name = "pictureBoxSource";
            this.pictureBoxSource.Padding = new System.Windows.Forms.Padding(3);
            this.pictureBoxSource.Size = new System.Drawing.Size(22, 24);
            this.pictureBoxSource.TabIndex = 14;
            this.pictureBoxSource.TabStop = false;
            // 
            // textBoxTaxonListFile
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxTaxonListFile, 4);
            this.textBoxTaxonListFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTaxonListFile.Location = new System.Drawing.Point(101, 508);
            this.textBoxTaxonListFile.Name = "textBoxTaxonListFile";
            this.textBoxTaxonListFile.Size = new System.Drawing.Size(423, 20);
            this.textBoxTaxonListFile.TabIndex = 15;
            // 
            // labelSaveAs
            // 
            this.labelSaveAs.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelSaveAs, 2);
            this.labelSaveAs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSaveAs.Location = new System.Drawing.Point(3, 505);
            this.labelSaveAs.Name = "labelSaveAs";
            this.labelSaveAs.Size = new System.Drawing.Size(92, 26);
            this.labelSaveAs.TabIndex = 16;
            this.labelSaveAs.Text = "Save as:";
            this.labelSaveAs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Image = global::DiversityCollection.Resource.Save;
            this.buttonSave.Location = new System.Drawing.Point(527, 505);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(24, 23);
            this.buttonSave.TabIndex = 17;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // radioButtonCountNumberOfUnits
            // 
            this.radioButtonCountNumberOfUnits.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.radioButtonCountNumberOfUnits, 2);
            this.radioButtonCountNumberOfUnits.Dock = System.Windows.Forms.DockStyle.Right;
            this.radioButtonCountNumberOfUnits.Location = new System.Drawing.Point(16, 54);
            this.radioButtonCountNumberOfUnits.Name = "radioButtonCountNumberOfUnits";
            this.radioButtonCountNumberOfUnits.Size = new System.Drawing.Size(79, 20);
            this.radioButtonCountNumberOfUnits.TabIndex = 18;
            this.radioButtonCountNumberOfUnits.Text = "No. of units";
            this.radioButtonCountNumberOfUnits.UseVisualStyleBackColor = true;
            // 
            // radioButtonCountUnits
            // 
            this.radioButtonCountUnits.AutoSize = true;
            this.radioButtonCountUnits.Checked = true;
            this.radioButtonCountUnits.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonCountUnits.Location = new System.Drawing.Point(101, 54);
            this.radioButtonCountUnits.Name = "radioButtonCountUnits";
            this.radioButtonCountUnits.Size = new System.Drawing.Size(198, 20);
            this.radioButtonCountUnits.TabIndex = 19;
            this.radioButtonCountUnits.TabStop = true;
            this.radioButtonCountUnits.Text = "Organisms as basis for number count";
            this.radioButtonCountUnits.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeGender
            // 
            this.checkBoxIncludeGender.AutoSize = true;
            this.checkBoxIncludeGender.Location = new System.Drawing.Point(305, 54);
            this.checkBoxIncludeGender.Name = "checkBoxIncludeGender";
            this.checkBoxIncludeGender.Size = new System.Drawing.Size(80, 17);
            this.checkBoxIncludeGender.TabIndex = 20;
            this.checkBoxIncludeGender.Text = "Include sex";
            this.checkBoxIncludeGender.UseVisualStyleBackColor = true;
            // 
            // checkBoxFrom
            // 
            this.checkBoxFrom.AutoSize = true;
            this.checkBoxFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxFrom.Location = new System.Drawing.Point(391, 54);
            this.checkBoxFrom.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.checkBoxFrom.Name = "checkBoxFrom";
            this.checkBoxFrom.Size = new System.Drawing.Size(49, 20);
            this.checkBoxFrom.TabIndex = 21;
            this.checkBoxFrom.Text = "From";
            this.toolTip.SetToolTip(this.checkBoxFrom, "Restrict data starting with selected date");
            this.checkBoxFrom.UseVisualStyleBackColor = true;
            // 
            // checkBoxUntil
            // 
            this.checkBoxUntil.AutoSize = true;
            this.checkBoxUntil.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxUntil.Location = new System.Drawing.Point(391, 80);
            this.checkBoxUntil.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.checkBoxUntil.Name = "checkBoxUntil";
            this.checkBoxUntil.Size = new System.Drawing.Size(49, 20);
            this.checkBoxUntil.TabIndex = 22;
            this.checkBoxUntil.Text = "Until";
            this.toolTip.SetToolTip(this.checkBoxUntil, "Restict to data not after the selected date");
            this.checkBoxUntil.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerFrom
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.dateTimePickerFrom, 2);
            this.dateTimePickerFrom.CustomFormat = "yyyy-MM-dd";
            this.dateTimePickerFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(440, 54);
            this.dateTimePickerFrom.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(111, 20);
            this.dateTimePickerFrom.TabIndex = 23;
            // 
            // dateTimePickerUntil
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.dateTimePickerUntil, 2);
            this.dateTimePickerUntil.CustomFormat = "yyyy-MM-dd";
            this.dateTimePickerUntil.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerUntil.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerUntil.Location = new System.Drawing.Point(440, 80);
            this.dateTimePickerUntil.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.dateTimePickerUntil.Name = "dateTimePickerUntil";
            this.dateTimePickerUntil.Size = new System.Drawing.Size(111, 20);
            this.dateTimePickerUntil.TabIndex = 24;
            // 
            // labelAnalysisDescription
            // 
            this.labelAnalysisDescription.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelAnalysisDescription, 5);
            this.labelAnalysisDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnalysisDescription.Location = new System.Drawing.Point(101, 184);
            this.labelAnalysisDescription.Name = "labelAnalysisDescription";
            this.labelAnalysisDescription.Size = new System.Drawing.Size(450, 13);
            this.labelAnalysisDescription.TabIndex = 25;
            this.labelAnalysisDescription.Visible = false;
            // 
            // FormTaxonList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 531);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTaxonList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Taxon list";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTaxonList_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTaxa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTaxonomicGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelTaxonomicGroup;
        private System.Windows.Forms.ComboBox comboBoxTaxonomicGroup;
        private System.Windows.Forms.DataGridView dataGridViewTaxa;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.CheckBox checkBoxIncludeTaxonLists;
        private System.Windows.Forms.ComboBox comboBoxDatabase;
        private System.Windows.Forms.Label labelTaxonLists;
        private System.Windows.Forms.Label labelAnalysis;
        private System.Windows.Forms.ComboBox comboBoxTaxonList;
        private System.Windows.Forms.ComboBox comboBoxAnalysis;
        private System.Windows.Forms.Button buttonRequeryTaxonList;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pictureBoxTaxonomicGroup;
        private System.Windows.Forms.PictureBox pictureBoxSource;
        private System.Windows.Forms.TextBox textBoxTaxonListFile;
        private System.Windows.Forms.Label labelSaveAs;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.RadioButton radioButtonCountNumberOfUnits;
        private System.Windows.Forms.RadioButton radioButtonCountUnits;
        private System.Windows.Forms.CheckBox checkBoxIncludeGender;
        private System.Windows.Forms.CheckBox checkBoxFrom;
        private System.Windows.Forms.CheckBox checkBoxUntil;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerUntil;
        private System.Windows.Forms.Label labelAnalysisDescription;
    }
}