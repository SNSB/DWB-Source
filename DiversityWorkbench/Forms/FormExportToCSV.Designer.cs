namespace DiversityWorkbench.Forms
{
    partial class FormExportToCSV
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportToCSV));
            groupBoxTables = new System.Windows.Forms.GroupBox();
            treeViewTables = new System.Windows.Forms.TreeView();
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            groupBoxOptions = new System.Windows.Forms.GroupBox();
            comboBoxRow = new System.Windows.Forms.ComboBox();
            labelRow = new System.Windows.Forms.Label();
            comboBoxColumn = new System.Windows.Forms.ComboBox();
            labelSeparator = new System.Windows.Forms.Label();
            checkBoxXML = new System.Windows.Forms.CheckBox();
            checkBoxSchema = new System.Windows.Forms.CheckBox();
            groupBoxSelection = new System.Windows.Forms.GroupBox();
            buttonAll = new System.Windows.Forms.Button();
            buttonNone = new System.Windows.Forms.Button();
            checkBoxEnum = new System.Windows.Forms.CheckBox();
            checkBoxLog = new System.Windows.Forms.CheckBox();
            groupBoxOutput = new System.Windows.Forms.GroupBox();
            tableLayoutPanelDirectory = new System.Windows.Forms.TableLayoutPanel();
            textBoxDirectory = new System.Windows.Forms.TextBox();
            buttonDirectory = new System.Windows.Forms.Button();
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            labelHeadline = new System.Windows.Forms.Label();
            buttonFeedback = new System.Windows.Forms.Button();
            groupBoxResult = new System.Windows.Forms.GroupBox();
            textBoxResult = new System.Windows.Forms.TextBox();
            buttonStart = new System.Windows.Forms.Button();
            groupBoxProgress = new System.Windows.Forms.GroupBox();
            progressBar = new System.Windows.Forms.ProgressBar();
            toolTip = new System.Windows.Forms.ToolTip(components);
            folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            helpProvider = new System.Windows.Forms.HelpProvider();
            groupBoxTables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            groupBoxOptions.SuspendLayout();
            groupBoxSelection.SuspendLayout();
            groupBoxOutput.SuspendLayout();
            tableLayoutPanelDirectory.SuspendLayout();
            tableLayoutPanelMain.SuspendLayout();
            groupBoxResult.SuspendLayout();
            groupBoxProgress.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxTables
            // 
            groupBoxTables.Controls.Add(treeViewTables);
            groupBoxTables.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxTables.Location = new System.Drawing.Point(0, 0);
            groupBoxTables.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxTables.Name = "groupBoxTables";
            groupBoxTables.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxTables.Size = new System.Drawing.Size(212, 253);
            groupBoxTables.TabIndex = 1;
            groupBoxTables.TabStop = false;
            groupBoxTables.Text = "Tables for export";
            // 
            // treeViewTables
            // 
            treeViewTables.CheckBoxes = true;
            treeViewTables.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewTables.Location = new System.Drawing.Point(4, 19);
            treeViewTables.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeViewTables.Name = "treeViewTables";
            treeViewTables.ShowPlusMinus = false;
            treeViewTables.ShowRootLines = false;
            treeViewTables.Size = new System.Drawing.Size(204, 231);
            treeViewTables.TabIndex = 0;
            treeViewTables.NodeMouseClick += treeViewTables_NodeMouseClick;
            // 
            // splitContainerMain
            // 
            tableLayoutPanelMain.SetColumnSpan(splitContainerMain, 5);
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(4, 37);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(groupBoxTables);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(groupBoxOptions);
            splitContainerMain.Panel2.Controls.Add(groupBoxSelection);
            splitContainerMain.Size = new System.Drawing.Size(398, 253);
            splitContainerMain.SplitterDistance = 212;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 0;
            // 
            // groupBoxOptions
            // 
            groupBoxOptions.Controls.Add(comboBoxRow);
            groupBoxOptions.Controls.Add(labelRow);
            groupBoxOptions.Controls.Add(comboBoxColumn);
            groupBoxOptions.Controls.Add(labelSeparator);
            groupBoxOptions.Controls.Add(checkBoxXML);
            groupBoxOptions.Controls.Add(checkBoxSchema);
            groupBoxOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxOptions.Location = new System.Drawing.Point(0, 120);
            groupBoxOptions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxOptions.Name = "groupBoxOptions";
            groupBoxOptions.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxOptions.Size = new System.Drawing.Size(181, 133);
            groupBoxOptions.TabIndex = 3;
            groupBoxOptions.TabStop = false;
            groupBoxOptions.Text = "Export options";
            // 
            // comboBoxRow
            // 
            comboBoxRow.FormattingEnabled = true;
            comboBoxRow.Items.AddRange(new object[] { "ENDL", "NULL" });
            comboBoxRow.Location = new System.Drawing.Point(114, 96);
            comboBoxRow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxRow.Name = "comboBoxRow";
            comboBoxRow.Size = new System.Drawing.Size(62, 23);
            comboBoxRow.TabIndex = 4;
            toolTip.SetToolTip(comboBoxRow, "Row deparator, default is \"ENDL\"");
            comboBoxRow.SelectedIndexChanged += comboBoxRow_SelectedIndexChanged;
            // 
            // labelRow
            // 
            labelRow.AutoSize = true;
            labelRow.Location = new System.Drawing.Point(4, 99);
            labelRow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelRow.Name = "labelRow";
            labelRow.Size = new System.Drawing.Size(88, 15);
            labelRow.TabIndex = 4;
            labelRow.Text = "Row separator: ";
            // 
            // comboBoxColumn
            // 
            comboBoxColumn.FormattingEnabled = true;
            comboBoxColumn.Items.AddRange(new object[] { "TAB", ",", ";" });
            comboBoxColumn.Location = new System.Drawing.Point(114, 65);
            comboBoxColumn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxColumn.Name = "comboBoxColumn";
            comboBoxColumn.Size = new System.Drawing.Size(62, 23);
            comboBoxColumn.TabIndex = 3;
            toolTip.SetToolTip(comboBoxColumn, "Column separator, default is \"TAB\"");
            comboBoxColumn.SelectedIndexChanged += comboBoxColumn_SelectedIndexChanged;
            // 
            // labelSeparator
            // 
            labelSeparator.AutoSize = true;
            labelSeparator.Location = new System.Drawing.Point(4, 68);
            labelSeparator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSeparator.Name = "labelSeparator";
            labelSeparator.Size = new System.Drawing.Size(108, 15);
            labelSeparator.TabIndex = 3;
            labelSeparator.Text = "Column separator: ";
            // 
            // checkBoxXML
            // 
            checkBoxXML.AutoSize = true;
            checkBoxXML.Checked = true;
            checkBoxXML.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxXML.Dock = System.Windows.Forms.DockStyle.Top;
            checkBoxXML.Location = new System.Drawing.Point(4, 38);
            checkBoxXML.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxXML.Name = "checkBoxXML";
            checkBoxXML.Size = new System.Drawing.Size(173, 19);
            checkBoxXML.TabIndex = 2;
            checkBoxXML.Text = "Save schema as XML";
            toolTip.SetToolTip(checkBoxXML, "Generate table schema information in XML format");
            checkBoxXML.UseVisualStyleBackColor = true;
            // 
            // checkBoxSchema
            // 
            checkBoxSchema.AutoSize = true;
            checkBoxSchema.Checked = true;
            checkBoxSchema.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxSchema.Dock = System.Windows.Forms.DockStyle.Top;
            checkBoxSchema.Location = new System.Drawing.Point(4, 19);
            checkBoxSchema.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxSchema.Name = "checkBoxSchema";
            checkBoxSchema.Size = new System.Drawing.Size(173, 19);
            checkBoxSchema.TabIndex = 1;
            checkBoxSchema.Text = "Save table schema";
            toolTip.SetToolTip(checkBoxSchema, "Generate file with table schema information");
            checkBoxSchema.UseVisualStyleBackColor = true;
            checkBoxSchema.CheckedChanged += checkBoxSchema_CheckedChanged;
            // 
            // groupBoxSelection
            // 
            groupBoxSelection.Controls.Add(buttonAll);
            groupBoxSelection.Controls.Add(buttonNone);
            groupBoxSelection.Controls.Add(checkBoxEnum);
            groupBoxSelection.Controls.Add(checkBoxLog);
            groupBoxSelection.Dock = System.Windows.Forms.DockStyle.Top;
            groupBoxSelection.Location = new System.Drawing.Point(0, 0);
            groupBoxSelection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxSelection.Name = "groupBoxSelection";
            groupBoxSelection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxSelection.Size = new System.Drawing.Size(181, 120);
            groupBoxSelection.TabIndex = 2;
            groupBoxSelection.TabStop = false;
            groupBoxSelection.Text = "Selection criteria";
            // 
            // buttonAll
            // 
            buttonAll.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            buttonAll.Image = Properties.Resources.CheckYes;
            buttonAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonAll.Location = new System.Drawing.Point(4, 63);
            buttonAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonAll.Name = "buttonAll";
            buttonAll.Size = new System.Drawing.Size(173, 27);
            buttonAll.TabIndex = 3;
            buttonAll.Text = "Select all";
            toolTip.SetToolTip(buttonAll, "Select all entries in esport list");
            buttonAll.UseVisualStyleBackColor = true;
            buttonAll.Click += buttonAll_Click;
            // 
            // buttonNone
            // 
            buttonNone.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonNone.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            buttonNone.Image = Properties.Resources.CheckNo;
            buttonNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonNone.Location = new System.Drawing.Point(4, 90);
            buttonNone.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonNone.Name = "buttonNone";
            buttonNone.Size = new System.Drawing.Size(173, 27);
            buttonNone.TabIndex = 4;
            buttonNone.Text = "Select none";
            toolTip.SetToolTip(buttonNone, "Deselect all entries in export list");
            buttonNone.UseVisualStyleBackColor = true;
            buttonNone.Click += buttonNone_Click;
            // 
            // checkBoxEnum
            // 
            checkBoxEnum.AutoSize = true;
            checkBoxEnum.Checked = true;
            checkBoxEnum.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxEnum.Dock = System.Windows.Forms.DockStyle.Top;
            checkBoxEnum.Location = new System.Drawing.Point(4, 38);
            checkBoxEnum.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxEnum.Name = "checkBoxEnum";
            checkBoxEnum.Size = new System.Drawing.Size(173, 19);
            checkBoxEnum.TabIndex = 2;
            checkBoxEnum.Text = "Include enum tables";
            toolTip.SetToolTip(checkBoxEnum, "Select enumeration tables in export list");
            checkBoxEnum.UseVisualStyleBackColor = true;
            checkBoxEnum.CheckedChanged += checkBoxEnum_CheckedChanged;
            // 
            // checkBoxLog
            // 
            checkBoxLog.AutoSize = true;
            checkBoxLog.Dock = System.Windows.Forms.DockStyle.Top;
            checkBoxLog.Location = new System.Drawing.Point(4, 19);
            checkBoxLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxLog.Name = "checkBoxLog";
            checkBoxLog.Size = new System.Drawing.Size(173, 19);
            checkBoxLog.TabIndex = 1;
            checkBoxLog.Text = "Include logging tables";
            toolTip.SetToolTip(checkBoxLog, "Select log tales in export list");
            checkBoxLog.UseVisualStyleBackColor = true;
            checkBoxLog.CheckedChanged += checkBoxLog_CheckedChanged;
            // 
            // groupBoxOutput
            // 
            tableLayoutPanelMain.SetColumnSpan(groupBoxOutput, 5);
            groupBoxOutput.Controls.Add(tableLayoutPanelDirectory);
            groupBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxOutput.Location = new System.Drawing.Point(4, 296);
            groupBoxOutput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxOutput.Name = "groupBoxOutput";
            groupBoxOutput.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxOutput.Size = new System.Drawing.Size(398, 57);
            groupBoxOutput.TabIndex = 4;
            groupBoxOutput.TabStop = false;
            groupBoxOutput.Text = "Output directory";
            // 
            // tableLayoutPanelDirectory
            // 
            tableLayoutPanelDirectory.ColumnCount = 2;
            tableLayoutPanelDirectory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelDirectory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanelDirectory.Controls.Add(textBoxDirectory, 0, 0);
            tableLayoutPanelDirectory.Controls.Add(buttonDirectory, 1, 0);
            tableLayoutPanelDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelDirectory.Location = new System.Drawing.Point(4, 19);
            tableLayoutPanelDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelDirectory.Name = "tableLayoutPanelDirectory";
            tableLayoutPanelDirectory.RowCount = 1;
            tableLayoutPanelDirectory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelDirectory.Size = new System.Drawing.Size(390, 35);
            tableLayoutPanelDirectory.TabIndex = 0;
            // 
            // textBoxDirectory
            // 
            textBoxDirectory.Dock = System.Windows.Forms.DockStyle.Top;
            textBoxDirectory.Location = new System.Drawing.Point(4, 3);
            textBoxDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxDirectory.Name = "textBoxDirectory";
            textBoxDirectory.ReadOnly = true;
            textBoxDirectory.Size = new System.Drawing.Size(347, 23);
            textBoxDirectory.TabIndex = 0;
            // 
            // buttonDirectory
            // 
            buttonDirectory.Dock = System.Windows.Forms.DockStyle.Top;
            buttonDirectory.Image = ResourceWorkbench.OpenFolder;
            buttonDirectory.Location = new System.Drawing.Point(359, 3);
            buttonDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonDirectory.Name = "buttonDirectory";
            buttonDirectory.Size = new System.Drawing.Size(27, 24);
            buttonDirectory.TabIndex = 0;
            toolTip.SetToolTip(buttonDirectory, "Select output directory");
            buttonDirectory.UseVisualStyleBackColor = true;
            buttonDirectory.Click += buttonDirectory_Click;
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 5;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanelMain.Controls.Add(groupBoxOutput, 0, 2);
            tableLayoutPanelMain.Controls.Add(splitContainerMain, 0, 1);
            tableLayoutPanelMain.Controls.Add(labelHeadline, 1, 0);
            tableLayoutPanelMain.Controls.Add(buttonFeedback, 4, 0);
            tableLayoutPanelMain.Controls.Add(groupBoxResult, 0, 3);
            tableLayoutPanelMain.Controls.Add(buttonStart, 2, 5);
            tableLayoutPanelMain.Controls.Add(groupBoxProgress, 0, 4);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 6;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.Size = new System.Drawing.Size(406, 482);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelHeadline
            // 
            labelHeadline.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelHeadline, 3);
            labelHeadline.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeadline.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelHeadline.Location = new System.Drawing.Point(39, 0);
            labelHeadline.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelHeadline.Name = "labelHeadline";
            labelHeadline.Size = new System.Drawing.Size(328, 34);
            labelHeadline.TabIndex = 3;
            labelHeadline.Text = "Start export";
            labelHeadline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonFeedback.Image = ResourceWorkbench.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(375, 3);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(27, 28);
            buttonFeedback.TabIndex = 99;
            toolTip.SetToolTip(buttonFeedback, "Send feedback");
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // groupBoxResult
            // 
            tableLayoutPanelMain.SetColumnSpan(groupBoxResult, 5);
            groupBoxResult.Controls.Add(textBoxResult);
            groupBoxResult.Dock = System.Windows.Forms.DockStyle.Top;
            groupBoxResult.Location = new System.Drawing.Point(4, 359);
            groupBoxResult.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxResult.Name = "groupBoxResult";
            groupBoxResult.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxResult.Size = new System.Drawing.Size(398, 40);
            groupBoxResult.TabIndex = 6;
            groupBoxResult.TabStop = false;
            groupBoxResult.Text = "Export result";
            // 
            // textBoxResult
            // 
            textBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxResult.Dock = System.Windows.Forms.DockStyle.Top;
            textBoxResult.Location = new System.Drawing.Point(4, 19);
            textBoxResult.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxResult.Name = "textBoxResult";
            textBoxResult.ReadOnly = true;
            textBoxResult.Size = new System.Drawing.Size(390, 16);
            textBoxResult.TabIndex = 11;
            toolTip.SetToolTip(textBoxResult, "Click here to view result file");
            textBoxResult.Click += textBoxResult_Click;
            textBoxResult.TextChanged += textBoxResult_TextChanged;
            // 
            // buttonStart
            // 
            buttonStart.Image = Properties.Resources.CSV;
            buttonStart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonStart.Location = new System.Drawing.Point(153, 451);
            buttonStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new System.Drawing.Size(100, 28);
            buttonStart.TabIndex = 5;
            buttonStart.Text = "Start Export";
            buttonStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolTip.SetToolTip(buttonStart, "Click here to start export");
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            // 
            // groupBoxProgress
            // 
            tableLayoutPanelMain.SetColumnSpan(groupBoxProgress, 5);
            groupBoxProgress.Controls.Add(progressBar);
            groupBoxProgress.Dock = System.Windows.Forms.DockStyle.Top;
            groupBoxProgress.Location = new System.Drawing.Point(4, 405);
            groupBoxProgress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxProgress.Name = "groupBoxProgress";
            groupBoxProgress.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxProgress.Size = new System.Drawing.Size(398, 40);
            groupBoxProgress.TabIndex = 100;
            groupBoxProgress.TabStop = false;
            groupBoxProgress.Text = "Export progress";
            groupBoxProgress.Visible = false;
            // 
            // progressBar
            // 
            progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            progressBar.Location = new System.Drawing.Point(4, 19);
            progressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(390, 18);
            progressBar.Step = 1;
            progressBar.TabIndex = 0;
            // 
            // folderBrowserDialog
            // 
            folderBrowserDialog.Description = "Output directory";
            // 
            // FormExportToCSV
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(406, 482);
            Controls.Add(tableLayoutPanelMain);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(422, 475);
            Name = "FormExportToCSV";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Export database to CSV";
            KeyDown += Form_KeyDown;
            groupBoxTables.ResumeLayout(false);
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            groupBoxOptions.ResumeLayout(false);
            groupBoxOptions.PerformLayout();
            groupBoxSelection.ResumeLayout(false);
            groupBoxSelection.PerformLayout();
            groupBoxOutput.ResumeLayout(false);
            tableLayoutPanelDirectory.ResumeLayout(false);
            tableLayoutPanelDirectory.PerformLayout();
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            groupBoxResult.ResumeLayout(false);
            groupBoxResult.PerformLayout();
            groupBoxProgress.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTables;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.GroupBox groupBoxSelection;
        private System.Windows.Forms.CheckBox checkBoxEnum;
        private System.Windows.Forms.CheckBox checkBoxLog;
        private System.Windows.Forms.Button buttonNone;
        private System.Windows.Forms.Button buttonAll;
        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.CheckBox checkBoxXML;
        private System.Windows.Forms.CheckBox checkBoxSchema;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.ComboBox comboBoxColumn;
        private System.Windows.Forms.Label labelSeparator;
        private System.Windows.Forms.ComboBox comboBoxRow;
        private System.Windows.Forms.Label labelRow;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDirectory;
        private System.Windows.Forms.TextBox textBoxDirectory;
        private System.Windows.Forms.Button buttonDirectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label labelHeadline;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.GroupBox groupBoxResult;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.GroupBox groupBoxProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TreeView treeViewTables;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}