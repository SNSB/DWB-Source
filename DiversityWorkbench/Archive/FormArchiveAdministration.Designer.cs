namespace DiversityWorkbench.Archive
{
    partial class FormArchiveAdministration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormArchiveAdministration));
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            labelHeader = new System.Windows.Forms.Label();
            textBoxProtocol = new System.Windows.Forms.TextBox();
            treeViewArchives = new System.Windows.Forms.TreeView();
            buttonCreateArchives = new System.Windows.Forms.Button();
            labelProtocol = new System.Windows.Forms.Label();
            buttonDirectory = new System.Windows.Forms.Button();
            textBoxDirectory = new System.Windows.Forms.TextBox();
            labelCurrentStep = new System.Windows.Forms.Label();
            buttonCheckAll = new System.Windows.Forms.Button();
            buttonCheckNone = new System.Windows.Forms.Button();
            buttonClearProtocol = new System.Windows.Forms.Button();
            buttonClearAllProtocols = new System.Windows.Forms.Button();
            buttonFeedback = new System.Windows.Forms.Button();
            radioButtonFull = new System.Windows.Forms.RadioButton();
            radioButtonIncremental = new System.Windows.Forms.RadioButton();
            toolTip = new System.Windows.Forms.ToolTip(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            tableLayoutPanelMain.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 8;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanelMain.Controls.Add(labelHeader, 0, 0);
            tableLayoutPanelMain.Controls.Add(textBoxProtocol, 5, 2);
            tableLayoutPanelMain.Controls.Add(treeViewArchives, 0, 1);
            tableLayoutPanelMain.Controls.Add(buttonCreateArchives, 0, 3);
            tableLayoutPanelMain.Controls.Add(labelProtocol, 5, 1);
            tableLayoutPanelMain.Controls.Add(buttonDirectory, 5, 3);
            tableLayoutPanelMain.Controls.Add(textBoxDirectory, 6, 3);
            tableLayoutPanelMain.Controls.Add(labelCurrentStep, 3, 3);
            tableLayoutPanelMain.Controls.Add(buttonCheckAll, 1, 3);
            tableLayoutPanelMain.Controls.Add(buttonCheckNone, 1, 4);
            tableLayoutPanelMain.Controls.Add(buttonClearProtocol, 7, 1);
            tableLayoutPanelMain.Controls.Add(buttonClearAllProtocols, 4, 3);
            tableLayoutPanelMain.Controls.Add(buttonFeedback, 7, 0);
            tableLayoutPanelMain.Controls.Add(radioButtonFull, 2, 4);
            tableLayoutPanelMain.Controls.Add(radioButtonIncremental, 2, 3);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 5;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.Size = new System.Drawing.Size(804, 424);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelHeader, 7);
            labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeader.Location = new System.Drawing.Point(4, 0);
            labelHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new System.Drawing.Size(761, 18);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "label1";
            // 
            // textBoxProtocol
            // 
            tableLayoutPanelMain.SetColumnSpan(textBoxProtocol, 3);
            textBoxProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProtocol.Location = new System.Drawing.Point(403, 42);
            textBoxProtocol.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxProtocol.Multiline = true;
            textBoxProtocol.Name = "textBoxProtocol";
            textBoxProtocol.ReadOnly = true;
            textBoxProtocol.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            textBoxProtocol.Size = new System.Drawing.Size(397, 335);
            textBoxProtocol.TabIndex = 2;
            // 
            // treeViewArchives
            // 
            treeViewArchives.CheckBoxes = true;
            tableLayoutPanelMain.SetColumnSpan(treeViewArchives, 5);
            treeViewArchives.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewArchives.Location = new System.Drawing.Point(4, 21);
            treeViewArchives.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeViewArchives.Name = "treeViewArchives";
            tableLayoutPanelMain.SetRowSpan(treeViewArchives, 2);
            treeViewArchives.ShowLines = false;
            treeViewArchives.ShowPlusMinus = false;
            treeViewArchives.ShowRootLines = false;
            treeViewArchives.Size = new System.Drawing.Size(391, 356);
            treeViewArchives.TabIndex = 3;
            treeViewArchives.AfterCheck += treeViewArchives_AfterCheck;
            treeViewArchives.AfterSelect += treeViewArchives_AfterSelect;
            // 
            // buttonCreateArchives
            // 
            buttonCreateArchives.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonCreateArchives.Image = Properties.Resources.ArchivCreate;
            buttonCreateArchives.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonCreateArchives.Location = new System.Drawing.Point(4, 383);
            buttonCreateArchives.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonCreateArchives.Name = "buttonCreateArchives";
            tableLayoutPanelMain.SetRowSpan(buttonCreateArchives, 2);
            buttonCreateArchives.Size = new System.Drawing.Size(121, 38);
            buttonCreateArchives.TabIndex = 4;
            buttonCreateArchives.Text = "Create archives";
            buttonCreateArchives.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolTip.SetToolTip(buttonCreateArchives, "Create archives for all seletec items");
            buttonCreateArchives.UseVisualStyleBackColor = true;
            buttonCreateArchives.Click += buttonCreateArchives_Click;
            // 
            // labelProtocol
            // 
            labelProtocol.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelProtocol, 2);
            labelProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProtocol.Location = new System.Drawing.Point(403, 18);
            labelProtocol.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProtocol.Name = "labelProtocol";
            labelProtocol.Size = new System.Drawing.Size(362, 21);
            labelProtocol.TabIndex = 5;
            labelProtocol.Text = "Protocol";
            labelProtocol.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonDirectory
            // 
            buttonDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonDirectory.FlatAppearance.BorderSize = 0;
            buttonDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonDirectory.Image = Properties.Resources.OpenFolder;
            buttonDirectory.Location = new System.Drawing.Point(403, 383);
            buttonDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            buttonDirectory.Name = "buttonDirectory";
            tableLayoutPanelMain.SetRowSpan(buttonDirectory, 2);
            buttonDirectory.Size = new System.Drawing.Size(23, 38);
            buttonDirectory.TabIndex = 6;
            toolTip.SetToolTip(buttonDirectory, "Open the archive directory");
            buttonDirectory.UseVisualStyleBackColor = true;
            buttonDirectory.Click += buttonDirectory_Click;
            // 
            // textBoxDirectory
            // 
            tableLayoutPanelMain.SetColumnSpan(textBoxDirectory, 2);
            textBoxDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxDirectory.Location = new System.Drawing.Point(426, 383);
            textBoxDirectory.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            textBoxDirectory.Multiline = true;
            textBoxDirectory.Name = "textBoxDirectory";
            textBoxDirectory.ReadOnly = true;
            tableLayoutPanelMain.SetRowSpan(textBoxDirectory, 2);
            textBoxDirectory.Size = new System.Drawing.Size(374, 38);
            textBoxDirectory.TabIndex = 7;
            textBoxDirectory.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            toolTip.SetToolTip(textBoxDirectory, "The directory for the archives");
            // 
            // labelCurrentStep
            // 
            labelCurrentStep.AutoSize = true;
            labelCurrentStep.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCurrentStep.Location = new System.Drawing.Point(233, 380);
            labelCurrentStep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelCurrentStep.Name = "labelCurrentStep";
            tableLayoutPanelMain.SetRowSpan(labelCurrentStep, 2);
            labelCurrentStep.Size = new System.Drawing.Size(139, 44);
            labelCurrentStep.TabIndex = 8;
            labelCurrentStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCheckAll
            // 
            buttonCheckAll.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonCheckAll.FlatAppearance.BorderSize = 0;
            buttonCheckAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonCheckAll.Image = Properties.Resources.CheckYes;
            buttonCheckAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonCheckAll.Location = new System.Drawing.Point(129, 380);
            buttonCheckAll.Margin = new System.Windows.Forms.Padding(0);
            buttonCheckAll.Name = "buttonCheckAll";
            buttonCheckAll.Size = new System.Drawing.Size(56, 22);
            buttonCheckAll.TabIndex = 9;
            buttonCheckAll.Text = "All";
            buttonCheckAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(buttonCheckAll, "Select all projects");
            buttonCheckAll.UseVisualStyleBackColor = true;
            buttonCheckAll.Click += buttonCheckAll_Click;
            // 
            // buttonCheckNone
            // 
            buttonCheckNone.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonCheckNone.FlatAppearance.BorderSize = 0;
            buttonCheckNone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonCheckNone.Image = Properties.Resources.CheckNo;
            buttonCheckNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonCheckNone.Location = new System.Drawing.Point(129, 402);
            buttonCheckNone.Margin = new System.Windows.Forms.Padding(0);
            buttonCheckNone.Name = "buttonCheckNone";
            buttonCheckNone.Size = new System.Drawing.Size(56, 22);
            buttonCheckNone.TabIndex = 10;
            buttonCheckNone.Text = "No.";
            buttonCheckNone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(buttonCheckNone, "Select no project");
            buttonCheckNone.UseVisualStyleBackColor = true;
            buttonCheckNone.Click += buttonCheckNone_Click;
            // 
            // buttonClearProtocol
            // 
            buttonClearProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonClearProtocol.FlatAppearance.BorderSize = 0;
            buttonClearProtocol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonClearProtocol.Image = Properties.Resources.ListNot;
            buttonClearProtocol.Location = new System.Drawing.Point(769, 18);
            buttonClearProtocol.Margin = new System.Windows.Forms.Padding(0);
            buttonClearProtocol.Name = "buttonClearProtocol";
            buttonClearProtocol.Size = new System.Drawing.Size(35, 21);
            buttonClearProtocol.TabIndex = 11;
            toolTip.SetToolTip(buttonClearProtocol, "Clear the current protocol");
            buttonClearProtocol.UseVisualStyleBackColor = true;
            buttonClearProtocol.Click += buttonClearProtocol_Click;
            // 
            // buttonClearAllProtocols
            // 
            buttonClearAllProtocols.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonClearAllProtocols.FlatAppearance.BorderSize = 0;
            buttonClearAllProtocols.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonClearAllProtocols.Image = Properties.Resources.ListNot;
            buttonClearAllProtocols.Location = new System.Drawing.Point(376, 380);
            buttonClearAllProtocols.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonClearAllProtocols.Name = "buttonClearAllProtocols";
            tableLayoutPanelMain.SetRowSpan(buttonClearAllProtocols, 2);
            buttonClearAllProtocols.Size = new System.Drawing.Size(19, 44);
            buttonClearAllProtocols.TabIndex = 12;
            toolTip.SetToolTip(buttonClearAllProtocols, "Clear all protocols");
            buttonClearAllProtocols.UseVisualStyleBackColor = true;
            buttonClearAllProtocols.Click += buttonClearAllProtocols_Click;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Right;
            buttonFeedback.FlatAppearance.BorderSize = 0;
            buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonFeedback.Image = Properties.Resources.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(779, 0);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(21, 18);
            buttonFeedback.TabIndex = 13;
            toolTip.SetToolTip(buttonFeedback, "Send a feedback to the administrator");
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // radioButtonFull
            // 
            radioButtonFull.AutoSize = true;
            radioButtonFull.Checked = true;
            radioButtonFull.Enabled = false;
            radioButtonFull.Location = new System.Drawing.Point(185, 402);
            radioButtonFull.Margin = new System.Windows.Forms.Padding(0);
            radioButtonFull.Name = "radioButtonFull";
            radioButtonFull.Size = new System.Drawing.Size(44, 19);
            radioButtonFull.TabIndex = 14;
            radioButtonFull.TabStop = true;
            radioButtonFull.Text = "Full";
            toolTip.SetToolTip(radioButtonFull, "Create a full archive");
            radioButtonFull.UseVisualStyleBackColor = true;
            radioButtonFull.Visible = false;
            // 
            // radioButtonIncremental
            // 
            radioButtonIncremental.AutoSize = true;
            radioButtonIncremental.Enabled = false;
            radioButtonIncremental.Location = new System.Drawing.Point(185, 380);
            radioButtonIncremental.Margin = new System.Windows.Forms.Padding(0);
            radioButtonIncremental.Name = "radioButtonIncremental";
            radioButtonIncremental.Size = new System.Drawing.Size(44, 19);
            radioButtonIncremental.TabIndex = 15;
            radioButtonIncremental.Text = "Inc.";
            toolTip.SetToolTip(radioButtonIncremental, "Create an incremental archive");
            radioButtonIncremental.UseVisualStyleBackColor = true;
            radioButtonIncremental.Visible = false;
            // 
            // userControlDialogPanel
            // 
            userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel.Location = new System.Drawing.Point(0, 424);
            userControlDialogPanel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlDialogPanel.Name = "userControlDialogPanel";
            userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel.Size = new System.Drawing.Size(804, 31);
            userControlDialogPanel.TabIndex = 1;
            // 
            // FormArchiveAdministration
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(804, 455);
            Controls.Add(tableLayoutPanelMain);
            Controls.Add(userControlDialogPanel);
            helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormArchiveAdministration";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Archive administration";
            KeyDown += Form_KeyDown;
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TextBox textBoxProtocol;
        private System.Windows.Forms.TreeView treeViewArchives;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonCreateArchives;
        private System.Windows.Forms.Label labelProtocol;
        private System.Windows.Forms.Button buttonDirectory;
        private System.Windows.Forms.TextBox textBoxDirectory;
        private System.Windows.Forms.Label labelCurrentStep;
        private System.Windows.Forms.Button buttonCheckAll;
        private System.Windows.Forms.Button buttonCheckNone;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonClearProtocol;
        private System.Windows.Forms.Button buttonClearAllProtocols;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.RadioButton radioButtonFull;
        private System.Windows.Forms.RadioButton radioButtonIncremental;
    }
}