namespace DiversityWorkbench.Import
{
    partial class UserControlStep
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
            this.panelIndent = new System.Windows.Forms.Panel();
            this.pictureBoxStep = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelStep = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.panelSpacer = new System.Windows.Forms.Panel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStep)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelIndent
            // 
            this.panelIndent.Location = new System.Drawing.Point(0, 6);
            this.panelIndent.Margin = new System.Windows.Forms.Padding(0);
            this.panelIndent.Name = "panelIndent";
            this.panelIndent.Size = new System.Drawing.Size(10, 18);
            this.panelIndent.TabIndex = 0;
            // 
            // pictureBoxStep
            // 
            this.pictureBoxStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxStep.Location = new System.Drawing.Point(10, 7);
            this.pictureBoxStep.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.pictureBoxStep.Name = "pictureBoxStep";
            this.pictureBoxStep.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxStep.TabIndex = 1;
            this.pictureBoxStep.TabStop = false;
            this.pictureBoxStep.Click += new System.EventHandler(this.pictureBoxStep_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.panelIndent, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxStep, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelStep, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.labelMessage, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.panelSpacer, 3, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(294, 24);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStep.Location = new System.Drawing.Point(29, 6);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(29, 18);
            this.labelStep.TabIndex = 3;
            this.labelStep.Text = "Step";
            this.labelStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelStep.Click += new System.EventHandler(this.labelStep_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMessage.Location = new System.Drawing.Point(64, 6);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(227, 18);
            this.labelMessage.TabIndex = 4;
            this.labelMessage.Text = "OK";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelMessage.Click += new System.EventHandler(this.labelMessage_Click);
            // 
            // panelSpacer
            // 
            this.panelSpacer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSpacer.Location = new System.Drawing.Point(61, 0);
            this.panelSpacer.Margin = new System.Windows.Forms.Padding(0);
            this.panelSpacer.Name = "panelSpacer";
            this.panelSpacer.Size = new System.Drawing.Size(233, 6);
            this.panelSpacer.TabIndex = 5;
            // 
            // UserControlStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlStep";
            this.Size = new System.Drawing.Size(294, 24);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStep)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelIndent;
        private System.Windows.Forms.PictureBox pictureBoxStep;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Panel panelSpacer;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
