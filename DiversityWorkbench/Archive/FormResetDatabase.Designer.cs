namespace DiversityWorkbench.Archive
{
    partial class FormResetDatabase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResetDatabase));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.groupBoxTables = new System.Windows.Forms.GroupBox();
            this.panelTables = new System.Windows.Forms.Panel();
            this.labelState = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel.SuspendLayout();
            this.groupBoxTables.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonReset, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.groupBoxTables, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelState, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.progressBar, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(374, 532);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(368, 13);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Reset the content of the database to factory setting";
            // 
            // buttonReset
            // 
            this.buttonReset.BackColor = System.Drawing.Color.Pink;
            this.buttonReset.Image = global::DiversityWorkbench.ResourceWorkbench.CleanDatabase;
            this.buttonReset.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonReset.Location = new System.Drawing.Point(3, 498);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(108, 23);
            this.buttonReset.TabIndex = 1;
            this.buttonReset.Text = "Reset database";
            this.buttonReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonReset.UseVisualStyleBackColor = false;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // groupBoxTables
            // 
            this.tableLayoutPanel.SetColumnSpan(this.groupBoxTables, 2);
            this.groupBoxTables.Controls.Add(this.panelTables);
            this.groupBoxTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTables.Location = new System.Drawing.Point(3, 16);
            this.groupBoxTables.Name = "groupBoxTables";
            this.groupBoxTables.Size = new System.Drawing.Size(368, 476);
            this.groupBoxTables.TabIndex = 2;
            this.groupBoxTables.TabStop = false;
            this.groupBoxTables.Text = "Tables";
            // 
            // panelTables
            // 
            this.panelTables.AutoScroll = true;
            this.panelTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTables.Location = new System.Drawing.Point(3, 16);
            this.panelTables.Name = "panelTables";
            this.panelTables.Size = new System.Drawing.Size(362, 457);
            this.panelTables.TabIndex = 0;
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelState.Location = new System.Drawing.Point(120, 501);
            this.labelState.Margin = new System.Windows.Forms.Padding(6);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(248, 17);
            this.labelState.TabIndex = 3;
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFeedback.FlatAppearance.BorderSize = 0;
            this.buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpProvider.SetHelpString(this.buttonFeedback, "Send a feedback");
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(347, 0);
            this.buttonFeedback.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.helpProvider.SetShowHelp(this.buttonFeedback, true);
            this.buttonFeedback.Size = new System.Drawing.Size(24, 20);
            this.buttonFeedback.TabIndex = 1;
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // progressBar
            // 
            this.tableLayoutPanel.SetColumnSpan(this.progressBar, 2);
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(3, 524);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(368, 5);
            this.progressBar.TabIndex = 4;
            this.progressBar.Visible = false;
            // 
            // FormResetDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 532);
            this.Controls.Add(this.buttonFeedback);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormResetDatabase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reset database to factory setting";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.groupBoxTables.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.GroupBox groupBoxTables;
        private System.Windows.Forms.Panel panelTables;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}