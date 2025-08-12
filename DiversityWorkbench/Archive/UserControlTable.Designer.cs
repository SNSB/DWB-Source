namespace DiversityWorkbench.Archive
{
    partial class UserControlTable
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelTableName = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelCount = new System.Windows.Forms.Label();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.buttonMessage = new System.Windows.Forms.Button();
            this.labelCountFailed = new System.Windows.Forms.Label();
            this.labelCountPresent = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelCountLog = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel.Controls.Add(this.labelTableName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.progressBar, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelCount, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonInfo, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonMessage, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCountFailed, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCountPresent, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCountLog, 4, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(325, 24);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelTableName
            // 
            this.labelTableName.AutoSize = true;
            this.labelTableName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTableName.Location = new System.Drawing.Point(3, 3);
            this.labelTableName.Margin = new System.Windows.Forms.Padding(3);
            this.labelTableName.Name = "labelTableName";
            this.labelTableName.Size = new System.Drawing.Size(194, 13);
            this.labelTableName.TabIndex = 0;
            this.labelTableName.Text = "Table";
            this.labelTableName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.tableLayoutPanel.SetColumnSpan(this.progressBar, 4);
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(0, 19);
            this.progressBar.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(257, 5);
            this.progressBar.TabIndex = 1;
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCount.Location = new System.Drawing.Point(241, 0);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(13, 19);
            this.labelCount.TabIndex = 2;
            this.labelCount.Text = "0";
            this.labelCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonInfo
            // 
            this.buttonInfo.Image = global::DiversityWorkbench.Properties.Resources.Lupe;
            this.buttonInfo.Location = new System.Drawing.Point(300, 0);
            this.buttonInfo.Margin = new System.Windows.Forms.Padding(0);
            this.buttonInfo.Name = "buttonInfo";
            this.tableLayoutPanel.SetRowSpan(this.buttonInfo, 2);
            this.buttonInfo.Size = new System.Drawing.Size(24, 24);
            this.buttonInfo.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonInfo, "Show content of the table");
            this.buttonInfo.UseVisualStyleBackColor = true;
            this.buttonInfo.Click += new System.EventHandler(this.buttonInfo_Click);
            // 
            // buttonMessage
            // 
            this.buttonMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMessage.Image = global::DiversityWorkbench.Properties.Resources.Manual;
            this.buttonMessage.Location = new System.Drawing.Point(276, 0);
            this.buttonMessage.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMessage.Name = "buttonMessage";
            this.tableLayoutPanel.SetRowSpan(this.buttonMessage, 2);
            this.buttonMessage.Size = new System.Drawing.Size(24, 24);
            this.buttonMessage.TabIndex = 4;
            this.buttonMessage.UseVisualStyleBackColor = true;
            this.buttonMessage.Visible = false;
            this.buttonMessage.Click += new System.EventHandler(this.buttonMessage_Click);
            // 
            // labelCountFailed
            // 
            this.labelCountFailed.AutoSize = true;
            this.labelCountFailed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCountFailed.ForeColor = System.Drawing.Color.Red;
            this.labelCountFailed.Location = new System.Drawing.Point(203, 0);
            this.labelCountFailed.Name = "labelCountFailed";
            this.labelCountFailed.Size = new System.Drawing.Size(13, 19);
            this.labelCountFailed.TabIndex = 5;
            this.labelCountFailed.Text = "0";
            this.labelCountFailed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelCountFailed.Visible = false;
            // 
            // labelCountPresent
            // 
            this.labelCountPresent.AutoSize = true;
            this.labelCountPresent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCountPresent.ForeColor = System.Drawing.Color.Blue;
            this.labelCountPresent.Location = new System.Drawing.Point(222, 0);
            this.labelCountPresent.Name = "labelCountPresent";
            this.labelCountPresent.Size = new System.Drawing.Size(13, 19);
            this.labelCountPresent.TabIndex = 6;
            this.labelCountPresent.Text = "0";
            this.labelCountPresent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelCountPresent.Visible = false;
            // 
            // labelCountLog
            // 
            this.labelCountLog.AutoSize = true;
            this.labelCountLog.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelCountLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCountLog.ForeColor = System.Drawing.SystemColors.Window;
            this.labelCountLog.Location = new System.Drawing.Point(260, 0);
            this.labelCountLog.Name = "labelCountLog";
            this.labelCountLog.Size = new System.Drawing.Size(13, 19);
            this.labelCountLog.TabIndex = 7;
            this.labelCountLog.Text = "0";
            this.labelCountLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelCountLog.Visible = false;
            // 
            // UserControlTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlTable";
            this.Size = new System.Drawing.Size(325, 24);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelTableName;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonMessage;
        private System.Windows.Forms.Label labelCountFailed;
        private System.Windows.Forms.Label labelCountPresent;
        private System.Windows.Forms.Label labelCountLog;
    }
}
