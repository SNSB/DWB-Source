namespace DiversityWorkbench.Forms
{
    partial class FormUpdateDatabase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdateDatabase));
            this.splitContainerOutput = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelExpert = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.labelScript = new System.Windows.Forms.Label();
            this.textBoxScript = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxSingle = new System.Windows.Forms.CheckBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.checkBoxExpert = new System.Windows.Forms.CheckBox();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.buttonStartUpdate = new System.Windows.Forms.Button();
            this.buttonSetTimeout = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOutput)).BeginInit();
            this.splitContainerOutput.Panel1.SuspendLayout();
            this.splitContainerOutput.SuspendLayout();
            this.tableLayoutPanelExpert.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerOutput
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.splitContainerOutput, 3);
            this.splitContainerOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOutput.Location = new System.Drawing.Point(3, 33);
            this.splitContainerOutput.Name = "splitContainerOutput";
            this.splitContainerOutput.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerOutput.Panel1
            // 
            this.splitContainerOutput.Panel1.Controls.Add(this.tableLayoutPanelExpert);
            this.helpProvider.SetShowHelp(this.splitContainerOutput.Panel1, true);
            // 
            // splitContainerOutput.Panel2
            // 
            this.splitContainerOutput.Panel2.AutoScroll = true;
            this.helpProvider.SetShowHelp(this.splitContainerOutput.Panel2, true);
            this.helpProvider.SetShowHelp(this.splitContainerOutput, true);
            this.splitContainerOutput.Size = new System.Drawing.Size(786, 446);
            this.splitContainerOutput.SplitterDistance = 373;
            this.splitContainerOutput.TabIndex = 7;
            // 
            // tableLayoutPanelExpert
            // 
            this.tableLayoutPanelExpert.ColumnCount = 3;
            this.tableLayoutPanelExpert.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExpert.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelExpert.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExpert.Controls.Add(this.buttonOpenFile, 2, 0);
            this.tableLayoutPanelExpert.Controls.Add(this.textBoxFile, 1, 0);
            this.tableLayoutPanelExpert.Controls.Add(this.labelScript, 0, 0);
            this.tableLayoutPanelExpert.Controls.Add(this.textBoxScript, 0, 1);
            this.tableLayoutPanelExpert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelExpert.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelExpert.Name = "tableLayoutPanelExpert";
            this.tableLayoutPanelExpert.RowCount = 2;
            this.tableLayoutPanelExpert.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelExpert.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.helpProvider.SetShowHelp(this.tableLayoutPanelExpert, true);
            this.tableLayoutPanelExpert.Size = new System.Drawing.Size(786, 373);
            this.tableLayoutPanelExpert.TabIndex = 0;
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpenFile.Image = global::DiversityWorkbench.ResourceWorkbench.Open;
            this.buttonOpenFile.Location = new System.Drawing.Point(759, 3);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.helpProvider.SetShowHelp(this.buttonOpenFile, true);
            this.buttonOpenFile.Size = new System.Drawing.Size(24, 24);
            this.buttonOpenFile.TabIndex = 5;
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // textBoxFile
            // 
            this.textBoxFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFile.Location = new System.Drawing.Point(68, 6);
            this.textBoxFile.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.textBoxFile.Name = "textBoxFile";
            this.helpProvider.SetShowHelp(this.textBoxFile, true);
            this.textBoxFile.Size = new System.Drawing.Size(685, 20);
            this.textBoxFile.TabIndex = 2;
            this.toolTip.SetToolTip(this.textBoxFile, "The file name of the SQL script");
            this.textBoxFile.TextChanged += new System.EventHandler(this.textBoxFile_TextChanged);
            // 
            // labelScript
            // 
            this.labelScript.AutoSize = true;
            this.labelScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelScript.Location = new System.Drawing.Point(3, 0);
            this.labelScript.Name = "labelScript";
            this.helpProvider.SetShowHelp(this.labelScript, true);
            this.labelScript.Size = new System.Drawing.Size(59, 30);
            this.labelScript.TabIndex = 1;
            this.labelScript.Text = "SQL script:";
            this.labelScript.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxScript
            // 
            this.tableLayoutPanelExpert.SetColumnSpan(this.textBoxScript, 3);
            this.textBoxScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxScript.Location = new System.Drawing.Point(3, 33);
            this.textBoxScript.Multiline = true;
            this.textBoxScript.Name = "textBoxScript";
            this.textBoxScript.ReadOnly = true;
            this.textBoxScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.helpProvider.SetShowHelp(this.textBoxScript, true);
            this.textBoxScript.Size = new System.Drawing.Size(780, 337);
            this.textBoxScript.TabIndex = 4;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 3;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxSingle, 2, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.splitContainerOutput, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxExpert, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonFeedback, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonStartUpdate, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSetTimeout, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.helpProvider.SetShowHelp(this.tableLayoutPanelMain, true);
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(792, 518);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // checkBoxSingle
            // 
            this.checkBoxSingle.AutoSize = true;
            this.checkBoxSingle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxSingle.Location = new System.Drawing.Point(715, 485);
            this.checkBoxSingle.Name = "checkBoxSingle";
            this.helpProvider.SetShowHelp(this.checkBoxSingle, true);
            this.checkBoxSingle.Size = new System.Drawing.Size(74, 30);
            this.checkBoxSingle.TabIndex = 9;
            this.checkBoxSingle.Text = "Single step";
            this.checkBoxSingle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.checkBoxSingle, "Execute scripts in debugging mode");
            this.checkBoxSingle.UseVisualStyleBackColor = true;
            this.checkBoxSingle.Visible = false;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelHeader.Location = new System.Drawing.Point(103, 6);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.labelHeader.Name = "labelHeader";
            this.helpProvider.SetShowHelp(this.labelHeader, true);
            this.labelHeader.Size = new System.Drawing.Size(606, 18);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Start update";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBoxExpert
            // 
            this.checkBoxExpert.AutoSize = true;
            this.checkBoxExpert.Checked = true;
            this.checkBoxExpert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExpert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxExpert.Location = new System.Drawing.Point(3, 485);
            this.checkBoxExpert.Name = "checkBoxExpert";
            this.helpProvider.SetShowHelp(this.checkBoxExpert, true);
            this.checkBoxExpert.Size = new System.Drawing.Size(94, 30);
            this.checkBoxExpert.TabIndex = 8;
            this.checkBoxExpert.Text = "Expert\r\n mode";
            this.checkBoxExpert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.checkBoxExpert, "Execute scripts in debugging mode");
            this.checkBoxExpert.UseVisualStyleBackColor = true;
            this.checkBoxExpert.CheckedChanged += new System.EventHandler(this.checkBoxExpert_CheckedChanged);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonFeedback.Dock = System.Windows.Forms.DockStyle.Right;
            this.helpProvider.SetHelpKeyword(this.buttonFeedback, "Update");
            this.helpProvider.SetHelpNavigator(this.buttonFeedback, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.buttonFeedback.Image = global::DiversityWorkbench.ResourceWorkbench.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(765, 3);
            this.buttonFeedback.Name = "buttonFeedback";
            this.helpProvider.SetShowHelp(this.buttonFeedback, true);
            this.buttonFeedback.Size = new System.Drawing.Size(24, 24);
            this.buttonFeedback.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send Feedback");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // buttonStartUpdate
            // 
            this.buttonStartUpdate.Image = global::DiversityWorkbench.Properties.Resources.UpdateDatabase;
            this.buttonStartUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStartUpdate.Location = new System.Drawing.Point(360, 485);
            this.buttonStartUpdate.Margin = new System.Windows.Forms.Padding(260, 3, 3, 3);
            this.buttonStartUpdate.MinimumSize = new System.Drawing.Size(99, 24);
            this.buttonStartUpdate.Name = "buttonStartUpdate";
            this.helpProvider.SetShowHelp(this.buttonStartUpdate, true);
            this.buttonStartUpdate.Size = new System.Drawing.Size(112, 30);
            this.buttonStartUpdate.TabIndex = 3;
            this.buttonStartUpdate.Text = "Start update";
            this.buttonStartUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonStartUpdate, "Start update");
            this.buttonStartUpdate.UseVisualStyleBackColor = true;
            this.buttonStartUpdate.Click += new System.EventHandler(this.buttonStartUpdate_Click);
            // 
            // buttonSetTimeout
            // 
            this.buttonSetTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetTimeout.Image = global::DiversityWorkbench.Properties.Resources.Time;
            this.buttonSetTimeout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSetTimeout.Location = new System.Drawing.Point(3, 3);
            this.buttonSetTimeout.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.buttonSetTimeout.Name = "buttonSetTimeout";
            this.buttonSetTimeout.Size = new System.Drawing.Size(97, 24);
            this.buttonSetTimeout.TabIndex = 10;
            this.buttonSetTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonSetTimeout, "Set the timeout for the commands");
            this.buttonSetTimeout.UseVisualStyleBackColor = true;
            this.buttonSetTimeout.Click += new System.EventHandler(this.buttonSetTimeout_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "SQL scripts|*.sql";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // FormUpdateDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 518);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.helpProvider.SetHelpKeyword(this, "Update");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormUpdateDatabase";
            this.helpProvider.SetShowHelp(this, true);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update database";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUpdateDatabase_FormClosing);
            this.splitContainerOutput.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOutput)).EndInit();
            this.splitContainerOutput.ResumeLayout(false);
            this.tableLayoutPanelExpert.ResumeLayout(false);
            this.tableLayoutPanelExpert.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelScript;
        private System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.TextBox textBoxScript;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.SplitContainer splitContainerOutput;
        private System.Windows.Forms.CheckBox checkBoxExpert;
        private System.Windows.Forms.Button buttonStartUpdate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelExpert;
        private System.Windows.Forms.CheckBox checkBoxSingle;
        private System.Windows.Forms.Button buttonSetTimeout;
    }
}