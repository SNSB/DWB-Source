namespace DiversityCollection.Forms
{
    partial class FormImportWizardSplitSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportWizardSplitSettings));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.checkBoxSplitColumn = new System.Windows.Forms.CheckBox();
            this.dataGridViewTest = new System.Windows.Forms.DataGridView();
            this.ColumnSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTransformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonTest = new System.Windows.Forms.Button();
            this.tableLayoutPanelSplitting = new System.Windows.Forms.TableLayoutPanel();
            this.labelPosition = new System.Windows.Forms.Label();
            this.labelSplitters = new System.Windows.Forms.Label();
            this.numericUpDownPosition = new System.Windows.Forms.NumericUpDown();
            this.toolStripSplitters = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddSplitter = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveSplitter = new System.Windows.Forms.ToolStripButton();
            this.listBoxSplitters = new System.Windows.Forms.ListBox();
            this.checkBoxRegex = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanelRegex = new System.Windows.Forms.TableLayoutPanel();
            this.labelRegex = new System.Windows.Forms.Label();
            this.textBoxRegex = new System.Windows.Forms.TextBox();
            this.textBoxReplaceBy = new System.Windows.Forms.TextBox();
            this.labelReplaceBy = new System.Windows.Forms.Label();
            this.checkBoxTranslate = new System.Windows.Forms.CheckBox();
            this.buttonRefreshTranslationList = new System.Windows.Forms.Button();
            this.dataGridViewTranslation = new System.Windows.Forms.DataGridView();
            this.buttonClearTranslation = new System.Windows.Forms.Button();
            this.radioButtonTranslateErrors = new System.Windows.Forms.RadioButton();
            this.radioButtonTranslateAll = new System.Windows.Forms.RadioButton();
            this.toolStripTranslate = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonTranslationAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTranslationRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTranslationRefreshList = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTranslationClearList = new System.Windows.Forms.ToolStripButton();
            this.labelPrefix = new System.Windows.Forms.Label();
            this.textBoxPrefix = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.userControlDialogPanel = new DiversityWorkbench.UserControlDialogPanel();
            this.dataSetImportWizard = new DiversityCollection.Datasets.DataSetImportWizard();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTest)).BeginInit();
            this.tableLayoutPanelSplitting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPosition)).BeginInit();
            this.toolStripSplitters.SuspendLayout();
            this.tableLayoutPanelRegex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTranslation)).BeginInit();
            this.toolStripTranslate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetImportWizard)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 8;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.checkBoxSplitColumn, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.dataGridViewTest, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonTest, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelSplitting, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.checkBoxRegex, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelRegex, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.checkBoxTranslate, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonRefreshTranslationList, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.dataGridViewTranslation, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonClearTranslation, 7, 0);
            this.tableLayoutPanel.Controls.Add(this.radioButtonTranslateErrors, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.radioButtonTranslateAll, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.toolStripTranslate, 5, 1);
            this.tableLayoutPanel.Controls.Add(this.labelPrefix, 3, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxPrefix, 4, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(511, 519);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 5);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(0, 9);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(365, 13);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Header";
            // 
            // checkBoxSplitColumn
            // 
            this.checkBoxSplitColumn.AutoSize = true;
            this.checkBoxSplitColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxSplitColumn.Location = new System.Drawing.Point(3, 34);
            this.checkBoxSplitColumn.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkBoxSplitColumn.Name = "checkBoxSplitColumn";
            this.checkBoxSplitColumn.Size = new System.Drawing.Size(49, 22);
            this.checkBoxSplitColumn.TabIndex = 1;
            this.checkBoxSplitColumn.Text = "Split the column";
            this.toolTip.SetToolTip(this.checkBoxSplitColumn, "If a column in the file should be splitted");
            this.checkBoxSplitColumn.UseVisualStyleBackColor = true;
            this.checkBoxSplitColumn.CheckedChanged += new System.EventHandler(this.checkBoxSplitColumn_CheckedChanged);
            // 
            // dataGridViewTest
            // 
            this.dataGridViewTest.AllowUserToAddRows = false;
            this.dataGridViewTest.AllowUserToDeleteRows = false;
            this.dataGridViewTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTest.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSource,
            this.ColumnTransformation});
            this.tableLayoutPanel.SetColumnSpan(this.dataGridViewTest, 8);
            this.dataGridViewTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTest.Location = new System.Drawing.Point(3, 239);
            this.dataGridViewTest.Name = "dataGridViewTest";
            this.dataGridViewTest.Size = new System.Drawing.Size(505, 277);
            this.dataGridViewTest.TabIndex = 12;
            this.dataGridViewTest.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewTest_DataError);
            // 
            // ColumnSource
            // 
            this.ColumnSource.HeaderText = "Source";
            this.ColumnSource.Name = "ColumnSource";
            // 
            // ColumnTransformation
            // 
            this.ColumnTransformation.HeaderText = "Transformation";
            this.ColumnTransformation.Name = "ColumnTransformation";
            // 
            // buttonTest
            // 
            this.tableLayoutPanel.SetColumnSpan(this.buttonTest, 3);
            this.buttonTest.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonTest.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonTest.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonTest.Location = new System.Drawing.Point(3, 212);
            this.buttonTest.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(177, 24);
            this.buttonTest.TabIndex = 15;
            this.buttonTest.Text = "Test the transformation settings";
            this.buttonTest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // tableLayoutPanelSplitting
            // 
            this.tableLayoutPanelSplitting.ColumnCount = 1;
            this.tableLayoutPanelSplitting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSplitting.Controls.Add(this.labelPosition, 0, 0);
            this.tableLayoutPanelSplitting.Controls.Add(this.labelSplitters, 0, 2);
            this.tableLayoutPanelSplitting.Controls.Add(this.numericUpDownPosition, 0, 1);
            this.tableLayoutPanelSplitting.Controls.Add(this.toolStripSplitters, 0, 4);
            this.tableLayoutPanelSplitting.Controls.Add(this.listBoxSplitters, 0, 3);
            this.tableLayoutPanelSplitting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSplitting.Location = new System.Drawing.Point(0, 56);
            this.tableLayoutPanelSplitting.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.tableLayoutPanelSplitting.Name = "tableLayoutPanelSplitting";
            this.tableLayoutPanelSplitting.RowCount = 5;
            this.tableLayoutPanelSplitting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSplitting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSplitting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSplitting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSplitting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSplitting.Size = new System.Drawing.Size(52, 150);
            this.tableLayoutPanelSplitting.TabIndex = 13;
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPosition.Location = new System.Drawing.Point(0, 3);
            this.labelPosition.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(49, 13);
            this.labelPosition.TabIndex = 5;
            this.labelPosition.Text = "Position:";
            this.labelPosition.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelSplitters
            // 
            this.labelSplitters.AutoSize = true;
            this.labelSplitters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSplitters.Location = new System.Drawing.Point(0, 39);
            this.labelSplitters.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.labelSplitters.Name = "labelSplitters";
            this.labelSplitters.Size = new System.Drawing.Size(49, 13);
            this.labelSplitters.TabIndex = 3;
            this.labelSplitters.Text = "Splitter:";
            this.labelSplitters.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.toolTip.SetToolTip(this.labelSplitters, "Define the splitters for splitting the values in the file");
            // 
            // numericUpDownPosition
            // 
            this.numericUpDownPosition.BackColor = System.Drawing.SystemColors.Window;
            this.numericUpDownPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownPosition.Location = new System.Drawing.Point(3, 16);
            this.numericUpDownPosition.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.numericUpDownPosition.Name = "numericUpDownPosition";
            this.numericUpDownPosition.Size = new System.Drawing.Size(46, 20);
            this.numericUpDownPosition.TabIndex = 6;
            this.toolTip.SetToolTip(this.numericUpDownPosition, "The position after splitting the content according to the splitters");
            this.numericUpDownPosition.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // toolStripSplitters
            // 
            this.toolStripSplitters.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripSplitters.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripSplitters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddSplitter,
            this.toolStripButtonRemoveSplitter});
            this.toolStripSplitters.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripSplitters.Location = new System.Drawing.Point(3, 127);
            this.toolStripSplitters.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStripSplitters.Name = "toolStripSplitters";
            this.toolStripSplitters.Size = new System.Drawing.Size(47, 23);
            this.toolStripSplitters.TabIndex = 4;
            this.toolStripSplitters.Text = "toolStrip1";
            // 
            // toolStripButtonAddSplitter
            // 
            this.toolStripButtonAddSplitter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddSplitter.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonAddSplitter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddSplitter.Name = "toolStripButtonAddSplitter";
            this.toolStripButtonAddSplitter.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonAddSplitter.Text = "Add a splitter";
            this.toolStripButtonAddSplitter.Click += new System.EventHandler(this.toolStripButtonAddSplitter_Click);
            // 
            // toolStripButtonRemoveSplitter
            // 
            this.toolStripButtonRemoveSplitter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveSplitter.Image = global::DiversityCollection.Resource.Remove;
            this.toolStripButtonRemoveSplitter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveSplitter.Name = "toolStripButtonRemoveSplitter";
            this.toolStripButtonRemoveSplitter.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonRemoveSplitter.Text = "Remove the selected splitter";
            this.toolStripButtonRemoveSplitter.Click += new System.EventHandler(this.toolStripButtonRemoveSplitter_Click);
            // 
            // listBoxSplitters
            // 
            this.listBoxSplitters.BackColor = System.Drawing.SystemColors.Window;
            this.listBoxSplitters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSplitters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxSplitters.FormattingEnabled = true;
            this.listBoxSplitters.IntegralHeight = false;
            this.listBoxSplitters.Location = new System.Drawing.Point(3, 52);
            this.listBoxSplitters.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.listBoxSplitters.Name = "listBoxSplitters";
            this.listBoxSplitters.Size = new System.Drawing.Size(46, 75);
            this.listBoxSplitters.TabIndex = 7;
            // 
            // checkBoxRegex
            // 
            this.checkBoxRegex.AutoSize = true;
            this.checkBoxRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxRegex.Location = new System.Drawing.Point(58, 34);
            this.checkBoxRegex.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkBoxRegex.Name = "checkBoxRegex";
            this.checkBoxRegex.Size = new System.Drawing.Size(104, 22);
            this.checkBoxRegex.TabIndex = 16;
            this.checkBoxRegex.Text = "Transform";
            this.checkBoxRegex.UseVisualStyleBackColor = true;
            this.checkBoxRegex.CheckedChanged += new System.EventHandler(this.checkBoxRegex_CheckedChanged);
            // 
            // tableLayoutPanelRegex
            // 
            this.tableLayoutPanelRegex.ColumnCount = 1;
            this.tableLayoutPanelRegex.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRegex.Controls.Add(this.labelRegex, 0, 0);
            this.tableLayoutPanelRegex.Controls.Add(this.textBoxRegex, 0, 1);
            this.tableLayoutPanelRegex.Controls.Add(this.textBoxReplaceBy, 0, 3);
            this.tableLayoutPanelRegex.Controls.Add(this.labelReplaceBy, 0, 2);
            this.tableLayoutPanelRegex.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelRegex.Location = new System.Drawing.Point(55, 59);
            this.tableLayoutPanelRegex.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.tableLayoutPanelRegex.Name = "tableLayoutPanelRegex";
            this.tableLayoutPanelRegex.RowCount = 4;
            this.tableLayoutPanelRegex.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRegex.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRegex.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRegex.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRegex.Size = new System.Drawing.Size(107, 74);
            this.tableLayoutPanelRegex.TabIndex = 14;
            // 
            // labelRegex
            // 
            this.labelRegex.Location = new System.Drawing.Point(3, 0);
            this.labelRegex.Name = "labelRegex";
            this.labelRegex.Size = new System.Drawing.Size(101, 13);
            this.labelRegex.TabIndex = 8;
            this.labelRegex.Text = "Regular expression:";
            this.labelRegex.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxRegex
            // 
            this.textBoxRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRegex.Location = new System.Drawing.Point(3, 13);
            this.textBoxRegex.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.textBoxRegex.Name = "textBoxRegex";
            this.textBoxRegex.Size = new System.Drawing.Size(101, 20);
            this.textBoxRegex.TabIndex = 9;
            this.toolTip.SetToolTip(this.textBoxRegex, "Enter a regular expression for the transformation of the parts");
            this.textBoxRegex.TextChanged += new System.EventHandler(this.textBoxRegex_TextChanged);
            // 
            // textBoxReplaceBy
            // 
            this.textBoxReplaceBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReplaceBy.Location = new System.Drawing.Point(3, 46);
            this.textBoxReplaceBy.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxReplaceBy.Name = "textBoxReplaceBy";
            this.textBoxReplaceBy.Size = new System.Drawing.Size(101, 20);
            this.textBoxReplaceBy.TabIndex = 11;
            this.textBoxReplaceBy.TextChanged += new System.EventHandler(this.textBoxReplaceBy_TextChanged);
            // 
            // labelReplaceBy
            // 
            this.labelReplaceBy.AutoSize = true;
            this.labelReplaceBy.Location = new System.Drawing.Point(3, 33);
            this.labelReplaceBy.Name = "labelReplaceBy";
            this.labelReplaceBy.Size = new System.Drawing.Size(64, 13);
            this.labelReplaceBy.TabIndex = 10;
            this.labelReplaceBy.Text = "Replace by:";
            this.labelReplaceBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxTranslate
            // 
            this.checkBoxTranslate.AutoSize = true;
            this.checkBoxTranslate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxTranslate.Location = new System.Drawing.Point(168, 34);
            this.checkBoxTranslate.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkBoxTranslate.Name = "checkBoxTranslate";
            this.checkBoxTranslate.Size = new System.Drawing.Size(96, 22);
            this.checkBoxTranslate.TabIndex = 17;
            this.checkBoxTranslate.Text = "Translate";
            this.checkBoxTranslate.UseVisualStyleBackColor = true;
            this.checkBoxTranslate.CheckedChanged += new System.EventHandler(this.checkBoxTranslate_CheckedChanged);
            // 
            // buttonRefreshTranslationList
            // 
            this.buttonRefreshTranslationList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonRefreshTranslationList.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRefreshTranslationList.Location = new System.Drawing.Point(460, 32);
            this.buttonRefreshTranslationList.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefreshTranslationList.Name = "buttonRefreshTranslationList";
            this.tableLayoutPanel.SetRowSpan(this.buttonRefreshTranslationList, 2);
            this.buttonRefreshTranslationList.Size = new System.Drawing.Size(24, 24);
            this.buttonRefreshTranslationList.TabIndex = 19;
            this.toolTip.SetToolTip(this.buttonRefreshTranslationList, "Refresh the list of the values that should be translated");
            this.buttonRefreshTranslationList.UseVisualStyleBackColor = true;
            this.buttonRefreshTranslationList.Visible = false;
            this.buttonRefreshTranslationList.Click += new System.EventHandler(this.buttonRefreshTranslationList_Click);
            // 
            // dataGridViewTranslation
            // 
            this.dataGridViewTranslation.AllowUserToAddRows = false;
            this.dataGridViewTranslation.AllowUserToDeleteRows = false;
            this.dataGridViewTranslation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel.SetColumnSpan(this.dataGridViewTranslation, 6);
            this.dataGridViewTranslation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTranslation.Location = new System.Drawing.Point(168, 56);
            this.dataGridViewTranslation.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.dataGridViewTranslation.Name = "dataGridViewTranslation";
            this.dataGridViewTranslation.Size = new System.Drawing.Size(340, 150);
            this.dataGridViewTranslation.TabIndex = 20;
            this.dataGridViewTranslation.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewTranslation_DataError);
            // 
            // buttonClearTranslation
            // 
            this.buttonClearTranslation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonClearTranslation.Image = global::DiversityCollection.Resource.Delete;
            this.buttonClearTranslation.Location = new System.Drawing.Point(484, 32);
            this.buttonClearTranslation.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonClearTranslation.Name = "buttonClearTranslation";
            this.tableLayoutPanel.SetRowSpan(this.buttonClearTranslation, 2);
            this.buttonClearTranslation.Size = new System.Drawing.Size(24, 24);
            this.buttonClearTranslation.TabIndex = 21;
            this.buttonClearTranslation.UseVisualStyleBackColor = true;
            this.buttonClearTranslation.Visible = false;
            this.buttonClearTranslation.Click += new System.EventHandler(this.buttonClearTranslation_Click);
            // 
            // radioButtonTranslateErrors
            // 
            this.radioButtonTranslateErrors.AutoSize = true;
            this.radioButtonTranslateErrors.Checked = true;
            this.radioButtonTranslateErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonTranslateErrors.Location = new System.Drawing.Point(270, 34);
            this.radioButtonTranslateErrors.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.radioButtonTranslateErrors.Name = "radioButtonTranslateErrors";
            this.radioButtonTranslateErrors.Size = new System.Drawing.Size(51, 22);
            this.radioButtonTranslateErrors.TabIndex = 22;
            this.radioButtonTranslateErrors.TabStop = true;
            this.radioButtonTranslateErrors.Text = "errors";
            this.radioButtonTranslateErrors.UseVisualStyleBackColor = true;
            // 
            // radioButtonTranslateAll
            // 
            this.radioButtonTranslateAll.AutoSize = true;
            this.radioButtonTranslateAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonTranslateAll.Location = new System.Drawing.Point(327, 34);
            this.radioButtonTranslateAll.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.radioButtonTranslateAll.Name = "radioButtonTranslateAll";
            this.radioButtonTranslateAll.Size = new System.Drawing.Size(35, 22);
            this.radioButtonTranslateAll.TabIndex = 23;
            this.radioButtonTranslateAll.TabStop = true;
            this.radioButtonTranslateAll.Text = "all";
            this.radioButtonTranslateAll.UseVisualStyleBackColor = true;
            // 
            // toolStripTranslate
            // 
            this.toolStripTranslate.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTranslate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonTranslationAdd,
            this.toolStripButtonTranslationRemove,
            this.toolStripButtonTranslationRefreshList,
            this.toolStripButtonTranslationClearList});
            this.toolStripTranslate.Location = new System.Drawing.Point(365, 31);
            this.toolStripTranslate.Name = "toolStripTranslate";
            this.toolStripTranslate.Size = new System.Drawing.Size(95, 25);
            this.toolStripTranslate.TabIndex = 24;
            this.toolStripTranslate.Text = "toolStrip1";
            // 
            // toolStripButtonTranslationAdd
            // 
            this.toolStripButtonTranslationAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTranslationAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonTranslationAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTranslationAdd.Name = "toolStripButtonTranslationAdd";
            this.toolStripButtonTranslationAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTranslationAdd.Text = "Add a new value to the list, that should be translated";
            this.toolStripButtonTranslationAdd.Click += new System.EventHandler(this.toolStripButtonTranslationAdd_Click);
            // 
            // toolStripButtonTranslationRemove
            // 
            this.toolStripButtonTranslationRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTranslationRemove.Image = global::DiversityCollection.Resource.Remove;
            this.toolStripButtonTranslationRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTranslationRemove.Name = "toolStripButtonTranslationRemove";
            this.toolStripButtonTranslationRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTranslationRemove.Text = "Remove the selected value from the list";
            this.toolStripButtonTranslationRemove.Click += new System.EventHandler(this.toolStripButtonTranslationRemove_Click);
            // 
            // toolStripButtonTranslationRefreshList
            // 
            this.toolStripButtonTranslationRefreshList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTranslationRefreshList.Image = global::DiversityCollection.Resource.Transfrom;
            this.toolStripButtonTranslationRefreshList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTranslationRefreshList.Name = "toolStripButtonTranslationRefreshList";
            this.toolStripButtonTranslationRefreshList.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTranslationRefreshList.Text = "Reload the translation list based on the entries in the file";
            this.toolStripButtonTranslationRefreshList.Click += new System.EventHandler(this.toolStripButtonTranslationRefreshList_Click);
            // 
            // toolStripButtonTranslationClearList
            // 
            this.toolStripButtonTranslationClearList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTranslationClearList.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonTranslationClearList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTranslationClearList.Name = "toolStripButtonTranslationClearList";
            this.toolStripButtonTranslationClearList.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTranslationClearList.Text = "Clear the translation list";
            this.toolStripButtonTranslationClearList.Click += new System.EventHandler(this.toolStripButtonTranslationClearList_Click);
            // 
            // labelPrefix
            // 
            this.labelPrefix.AutoSize = true;
            this.labelPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrefix.Location = new System.Drawing.Point(270, 209);
            this.labelPrefix.Name = "labelPrefix";
            this.labelPrefix.Size = new System.Drawing.Size(51, 27);
            this.labelPrefix.TabIndex = 25;
            this.labelPrefix.Text = "Prefix:";
            this.labelPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPrefix
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxPrefix, 4);
            this.textBoxPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPrefix.Enabled = false;
            this.textBoxPrefix.Location = new System.Drawing.Point(327, 212);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(181, 20);
            this.textBoxPrefix.TabIndex = 26;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 519);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(511, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // dataSetImportWizard
            // 
            this.dataSetImportWizard.DataSetName = "DataSetImportWizard";
            this.dataSetImportWizard.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FormImportWizardSplitSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 546);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormImportWizardSplitSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transformation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormImportWizardSplitSettings_FormClosing);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTest)).EndInit();
            this.tableLayoutPanelSplitting.ResumeLayout(false);
            this.tableLayoutPanelSplitting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPosition)).EndInit();
            this.toolStripSplitters.ResumeLayout(false);
            this.toolStripSplitters.PerformLayout();
            this.tableLayoutPanelRegex.ResumeLayout(false);
            this.tableLayoutPanelRegex.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTranslation)).EndInit();
            this.toolStripTranslate.ResumeLayout(false);
            this.toolStripTranslate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetImportWizard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeader;
        private DiversityWorkbench.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.CheckBox checkBoxSplitColumn;
        private System.Windows.Forms.Label labelSplitters;
        private System.Windows.Forms.ToolStrip toolStripSplitters;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddSplitter;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveSplitter;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.NumericUpDown numericUpDownPosition;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelRegex;
        private System.Windows.Forms.TextBox textBoxRegex;
        private System.Windows.Forms.Label labelReplaceBy;
        private System.Windows.Forms.TextBox textBoxReplaceBy;
        private System.Windows.Forms.DataGridView dataGridViewTest;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSplitting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRegex;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.CheckBox checkBoxRegex;
        private System.Windows.Forms.ListBox listBoxSplitters;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTransformation;
        private System.Windows.Forms.CheckBox checkBoxTranslate;
        private System.Windows.Forms.Button buttonRefreshTranslationList;
        private System.Windows.Forms.DataGridView dataGridViewTranslation;
        private DiversityCollection.Datasets.DataSetImportWizard dataSetImportWizard;
        private System.Windows.Forms.Button buttonClearTranslation;
        private System.Windows.Forms.RadioButton radioButtonTranslateErrors;
        private System.Windows.Forms.RadioButton radioButtonTranslateAll;
        private System.Windows.Forms.ToolStrip toolStripTranslate;
        private System.Windows.Forms.ToolStripButton toolStripButtonTranslationAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonTranslationRemove;
        private System.Windows.Forms.ToolStripButton toolStripButtonTranslationRefreshList;
        private System.Windows.Forms.ToolStripButton toolStripButtonTranslationClearList;
        private System.Windows.Forms.Label labelPrefix;
        private System.Windows.Forms.TextBox textBoxPrefix;
    }
}