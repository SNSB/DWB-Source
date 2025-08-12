namespace DiversityWorkbench.Archive
{
    partial class FormCreateSchema
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreateSchema));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.buttonCreateSchema = new System.Windows.Forms.Button();
            this.textBoxArchiveFolder = new System.Windows.Forms.TextBox();
            this.checkedListBoxTables = new System.Windows.Forms.CheckedListBox();
            this.checkBoxTableListAll = new System.Windows.Forms.CheckBox();
            this.checkBoxTableListNone = new System.Windows.Forms.CheckBox();
            this.labelAddToSelection = new System.Windows.Forms.Label();
            this.labelRemoveFromSelection = new System.Windows.Forms.Label();
            this.textBoxAddToSelection = new System.Windows.Forms.TextBox();
            this.buttonAddToSelection = new System.Windows.Forms.Button();
            this.textBoxRemoveFromSelection = new System.Windows.Forms.TextBox();
            this.buttonRemoveFromSelection = new System.Windows.Forms.Button();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.buttonDirectory = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCreateSchema, 1, 6);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxArchiveFolder, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.checkedListBoxTables, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxTableListAll, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxTableListNone, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelAddToSelection, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelRemoveFromSelection, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxAddToSelection, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAddToSelection, 2, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxRemoveFromSelection, 1, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRemoveFromSelection, 2, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonFeedback, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonDirectory, 3, 5);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 7;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(301, 590);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 3);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(263, 20);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "label1";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCreateSchema
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonCreateSchema, 3);
            this.buttonCreateSchema.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCreateSchema.Image = global::DiversityWorkbench.Properties.Resources.ArchivOpen;
            this.buttonCreateSchema.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCreateSchema.Location = new System.Drawing.Point(178, 564);
            this.buttonCreateSchema.Name = "buttonCreateSchema";
            this.buttonCreateSchema.Size = new System.Drawing.Size(120, 23);
            this.buttonCreateSchema.TabIndex = 4;
            this.buttonCreateSchema.Text = "Create schemata";
            this.buttonCreateSchema.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonCreateSchema, "Create the table schemata");
            this.buttonCreateSchema.UseVisualStyleBackColor = true;
            this.buttonCreateSchema.Click += new System.EventHandler(this.buttonCreateSchema_Click);
            // 
            // textBoxArchiveFolder
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxArchiveFolder, 3);
            this.textBoxArchiveFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxArchiveFolder.Location = new System.Drawing.Point(3, 535);
            this.textBoxArchiveFolder.Name = "textBoxArchiveFolder";
            this.textBoxArchiveFolder.ReadOnly = true;
            this.textBoxArchiveFolder.Size = new System.Drawing.Size(263, 20);
            this.textBoxArchiveFolder.TabIndex = 6;
            // 
            // checkedListBoxTables
            // 
            this.checkedListBoxTables.CheckOnClick = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkedListBoxTables, 4);
            this.checkedListBoxTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxTables.FormattingEnabled = true;
            this.checkedListBoxTables.Location = new System.Drawing.Point(3, 23);
            this.checkedListBoxTables.Name = "checkedListBoxTables";
            this.checkedListBoxTables.Size = new System.Drawing.Size(295, 435);
            this.checkedListBoxTables.TabIndex = 11;
            // 
            // checkBoxTableListAll
            // 
            this.checkBoxTableListAll.AutoCheck = false;
            this.checkBoxTableListAll.AutoSize = true;
            this.checkBoxTableListAll.Checked = true;
            this.checkBoxTableListAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxTableListAll, 2);
            this.checkBoxTableListAll.Location = new System.Drawing.Point(3, 461);
            this.checkBoxTableListAll.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.checkBoxTableListAll.Name = "checkBoxTableListAll";
            this.checkBoxTableListAll.Size = new System.Drawing.Size(36, 17);
            this.checkBoxTableListAll.TabIndex = 34;
            this.checkBoxTableListAll.Text = "all";
            this.checkBoxTableListAll.UseVisualStyleBackColor = true;
            this.checkBoxTableListAll.Click += new System.EventHandler(this.checkBoxTableListAll_Click);
            // 
            // checkBoxTableListNone
            // 
            this.checkBoxTableListNone.AutoCheck = false;
            this.checkBoxTableListNone.AutoSize = true;
            this.checkBoxTableListNone.Location = new System.Drawing.Point(89, 461);
            this.checkBoxTableListNone.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxTableListNone.Name = "checkBoxTableListNone";
            this.checkBoxTableListNone.Size = new System.Drawing.Size(50, 17);
            this.checkBoxTableListNone.TabIndex = 35;
            this.checkBoxTableListNone.Text = "none";
            this.checkBoxTableListNone.UseVisualStyleBackColor = true;
            this.checkBoxTableListNone.Click += new System.EventHandler(this.checkBoxTableListNone_Click);
            // 
            // labelAddToSelection
            // 
            this.labelAddToSelection.AutoSize = true;
            this.labelAddToSelection.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelAddToSelection.Location = new System.Drawing.Point(9, 478);
            this.labelAddToSelection.Name = "labelAddToSelection";
            this.labelAddToSelection.Size = new System.Drawing.Size(29, 27);
            this.labelAddToSelection.TabIndex = 36;
            this.labelAddToSelection.Text = "Add:";
            this.labelAddToSelection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelRemoveFromSelection
            // 
            this.labelRemoveFromSelection.AutoSize = true;
            this.labelRemoveFromSelection.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelRemoveFromSelection.Location = new System.Drawing.Point(3, 505);
            this.labelRemoveFromSelection.Name = "labelRemoveFromSelection";
            this.labelRemoveFromSelection.Size = new System.Drawing.Size(35, 27);
            this.labelRemoveFromSelection.TabIndex = 39;
            this.labelRemoveFromSelection.Text = "Rem.:";
            this.labelRemoveFromSelection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxAddToSelection
            // 
            this.textBoxAddToSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAddToSelection.Location = new System.Drawing.Point(44, 481);
            this.textBoxAddToSelection.Name = "textBoxAddToSelection";
            this.textBoxAddToSelection.Size = new System.Drawing.Size(42, 20);
            this.textBoxAddToSelection.TabIndex = 40;
            this.textBoxAddToSelection.Text = "*";
            this.toolTip.SetToolTip(this.textBoxAddToSelection, "Filter string with * as wildcard");
            // 
            // buttonAddToSelection
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonAddToSelection, 2);
            this.buttonAddToSelection.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonAddToSelection.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAddToSelection.Location = new System.Drawing.Point(92, 479);
            this.buttonAddToSelection.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.buttonAddToSelection.Name = "buttonAddToSelection";
            this.buttonAddToSelection.Size = new System.Drawing.Size(136, 23);
            this.buttonAddToSelection.TabIndex = 41;
            this.buttonAddToSelection.Text = "Add to selection";
            this.buttonAddToSelection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonAddToSelection, "Add objects according to the filter");
            this.buttonAddToSelection.UseVisualStyleBackColor = true;
            this.buttonAddToSelection.Click += new System.EventHandler(this.buttonAddToSelection_Click);
            // 
            // textBoxRemoveFromSelection
            // 
            this.textBoxRemoveFromSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRemoveFromSelection.Location = new System.Drawing.Point(44, 508);
            this.textBoxRemoveFromSelection.Name = "textBoxRemoveFromSelection";
            this.textBoxRemoveFromSelection.Size = new System.Drawing.Size(42, 20);
            this.textBoxRemoveFromSelection.TabIndex = 42;
            this.textBoxRemoveFromSelection.Text = "*";
            this.toolTip.SetToolTip(this.textBoxRemoveFromSelection, "Filter string with * as wildcard");
            // 
            // buttonRemoveFromSelection
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonRemoveFromSelection, 2);
            this.buttonRemoveFromSelection.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRemoveFromSelection.Image = global::DiversityWorkbench.Properties.Resources.Minus;
            this.buttonRemoveFromSelection.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemoveFromSelection.Location = new System.Drawing.Point(92, 506);
            this.buttonRemoveFromSelection.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.buttonRemoveFromSelection.Name = "buttonRemoveFromSelection";
            this.buttonRemoveFromSelection.Size = new System.Drawing.Size(136, 23);
            this.buttonRemoveFromSelection.TabIndex = 43;
            this.buttonRemoveFromSelection.Text = "Remove from selection";
            this.buttonRemoveFromSelection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonRemoveFromSelection, "Remove objects according to the filter");
            this.buttonRemoveFromSelection.UseVisualStyleBackColor = true;
            this.buttonRemoveFromSelection.Click += new System.EventHandler(this.buttonRemoveFromSelection_Click);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.FlatAppearance.BorderSize = 0;
            this.buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(269, 0);
            this.buttonFeedback.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(32, 20);
            this.buttonFeedback.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send a feedback to the software developer");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // buttonDirectory
            // 
            this.buttonDirectory.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.buttonDirectory.Location = new System.Drawing.Point(272, 535);
            this.buttonDirectory.Name = "buttonDirectory";
            this.buttonDirectory.Size = new System.Drawing.Size(26, 23);
            this.buttonDirectory.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonDirectory, "Open folder containing the table schemata");
            this.buttonDirectory.UseVisualStyleBackColor = true;
            this.buttonDirectory.Click += new System.EventHandler(this.buttonDirectory_Click);
            // 
            // FormCreateSchema
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 590);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCreateSchema";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create table schemata";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Button buttonCreateSchema;
        private System.Windows.Forms.Button buttonDirectory;
        private System.Windows.Forms.TextBox textBoxArchiveFolder;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.CheckedListBox checkedListBoxTables;
        private System.Windows.Forms.CheckBox checkBoxTableListAll;
        private System.Windows.Forms.CheckBox checkBoxTableListNone;
        private System.Windows.Forms.Label labelAddToSelection;
        private System.Windows.Forms.Label labelRemoveFromSelection;
        private System.Windows.Forms.TextBox textBoxAddToSelection;
        private System.Windows.Forms.Button buttonAddToSelection;
        private System.Windows.Forms.TextBox textBoxRemoveFromSelection;
        private System.Windows.Forms.Button buttonRemoveFromSelection;
    }
}