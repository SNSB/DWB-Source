namespace DiversityWorkbench.Forms
{
    partial class FormHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHistory));
            this.tabControlHistory = new System.Windows.Forms.TabControl();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.buttonRollBack = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.buttonRestoreDeleted = new System.Windows.Forms.Button();
            this.buttonIncludeSavedLog = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlHistory
            // 
            this.tableLayoutPanel.SetColumnSpan(this.tabControlHistory, 5);
            this.tabControlHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlHistory.Location = new System.Drawing.Point(3, 29);
            this.tabControlHistory.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tabControlHistory.Name = "tabControlHistory";
            this.tabControlHistory.SelectedIndex = 0;
            this.tabControlHistory.Size = new System.Drawing.Size(752, 445);
            this.tabControlHistory.TabIndex = 1;
            this.tabControlHistory.SelectedIndexChanged += new System.EventHandler(this.tabControlHistory_SelectedIndexChanged);
            // 
            // buttonRollBack
            // 
            this.buttonRollBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRollBack.Image = global::DiversityWorkbench.Properties.Resources.RestoreLine;
            this.buttonRollBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRollBack.Location = new System.Drawing.Point(542, 3);
            this.buttonRollBack.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonRollBack.Name = "buttonRollBack";
            this.buttonRollBack.Size = new System.Drawing.Size(185, 26);
            this.buttonRollBack.TabIndex = 2;
            this.buttonRollBack.Text = "     Restore data as in selected line";
            this.buttonRollBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRollBack.UseVisualStyleBackColor = true;
            this.buttonRollBack.Click += new System.EventHandler(this.buttonRollBack_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.tabControlHistory, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonRollBack, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonFeedback, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonRestoreDeleted, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonIncludeSavedLog, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(758, 477);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(730, 3);
            this.buttonFeedback.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(25, 26);
            this.buttonFeedback.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send feedback to the administrator");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // buttonRestoreDeleted
            // 
            this.buttonRestoreDeleted.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRestoreDeleted.Image = global::DiversityWorkbench.Properties.Resources.Restore;
            this.buttonRestoreDeleted.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRestoreDeleted.Location = new System.Drawing.Point(3, 3);
            this.buttonRestoreDeleted.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.buttonRestoreDeleted.Name = "buttonRestoreDeleted";
            this.buttonRestoreDeleted.Size = new System.Drawing.Size(106, 25);
            this.buttonRestoreDeleted.TabIndex = 4;
            this.buttonRestoreDeleted.Text = "Restore deleted";
            this.buttonRestoreDeleted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonRestoreDeleted, "Restore a deleted dataset");
            this.buttonRestoreDeleted.UseVisualStyleBackColor = true;
            this.buttonRestoreDeleted.Visible = false;
            this.buttonRestoreDeleted.Click += new System.EventHandler(this.buttonRestoreDeleted_Click);
            // 
            // buttonIncludeSavedLog
            // 
            this.buttonIncludeSavedLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonIncludeSavedLog.Image = global::DiversityWorkbench.Properties.Resources.LogRestore;
            this.buttonIncludeSavedLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonIncludeSavedLog.Location = new System.Drawing.Point(376, 3);
            this.buttonIncludeSavedLog.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.buttonIncludeSavedLog.Name = "buttonIncludeSavedLog";
            this.buttonIncludeSavedLog.Size = new System.Drawing.Size(120, 25);
            this.buttonIncludeSavedLog.TabIndex = 5;
            this.buttonIncludeSavedLog.Text = "Include saved log";
            this.buttonIncludeSavedLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonIncludeSavedLog, "Include data saved in Log database");
            this.buttonIncludeSavedLog.UseVisualStyleBackColor = true;
            this.buttonIncludeSavedLog.Click += new System.EventHandler(this.buttonIncludeSavedLog_Click);
            // 
            // FormHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 477);
            this.Controls.Add(this.tableLayoutPanel);
            this.helpProvider.SetHelpKeyword(this, "History");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormHistory";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormHistory";
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlHistory;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonRollBack;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonRestoreDeleted;
        private System.Windows.Forms.Button buttonIncludeSavedLog;
    }
}