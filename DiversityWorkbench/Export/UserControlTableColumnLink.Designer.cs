namespace DiversityWorkbench.Export
{
    partial class UserControlTableColumnLink
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAddLink = new System.Windows.Forms.Button();
            this.pictureBoxModule = new System.Windows.Forms.PictureBox();
            this.buttonAddUnitValue = new System.Windows.Forms.Button();
            this.labelValue = new System.Windows.Forms.Label();
            this.labelSource = new System.Windows.Forms.Label();
            this.panelUnitValues = new System.Windows.Forms.Panel();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxModule)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.buttonAddLink, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxModule, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonAddUnitValue, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelValue, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelSource, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.panelUnitValues, 2, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(419, 49);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // buttonAddLink
            // 
            this.buttonAddLink.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonAddLink.Location = new System.Drawing.Point(0, 0);
            this.buttonAddLink.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddLink.Name = "buttonAddLink";
            this.buttonAddLink.Size = new System.Drawing.Size(24, 24);
            this.buttonAddLink.TabIndex = 0;
            this.buttonAddLink.UseVisualStyleBackColor = true;
            this.buttonAddLink.Click += new System.EventHandler(this.buttonAddLink_Click);
            // 
            // pictureBoxModule
            // 
            this.pictureBoxModule.Image = global::DiversityWorkbench.Properties.Resources.DiversityWorkbench_3;
            this.pictureBoxModule.Location = new System.Drawing.Point(4, 28);
            this.pictureBoxModule.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxModule.Name = "pictureBoxModule";
            this.pictureBoxModule.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxModule.TabIndex = 1;
            this.pictureBoxModule.TabStop = false;
            // 
            // buttonAddUnitValue
            // 
            this.buttonAddUnitValue.Image = global::DiversityWorkbench.Properties.Resources.AddChild;
            this.buttonAddUnitValue.Location = new System.Drawing.Point(24, 24);
            this.buttonAddUnitValue.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddUnitValue.Name = "buttonAddUnitValue";
            this.buttonAddUnitValue.Size = new System.Drawing.Size(24, 24);
            this.buttonAddUnitValue.TabIndex = 2;
            this.buttonAddUnitValue.UseVisualStyleBackColor = true;
            this.buttonAddUnitValue.Click += new System.EventHandler(this.buttonAddUnitValue_Click);
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelValue, 2);
            this.labelValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelValue.ForeColor = System.Drawing.Color.Blue;
            this.labelValue.Location = new System.Drawing.Point(27, 0);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(348, 24);
            this.labelValue.TabIndex = 3;
            this.labelValue.Text = "label1";
            this.labelValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSource.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelSource.Location = new System.Drawing.Point(381, 0);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(35, 24);
            this.labelSource.TabIndex = 4;
            this.labelSource.Text = "label1";
            this.labelSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelUnitValues
            // 
            this.tableLayoutPanel.SetColumnSpan(this.panelUnitValues, 2);
            this.panelUnitValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUnitValues.Location = new System.Drawing.Point(48, 24);
            this.panelUnitValues.Margin = new System.Windows.Forms.Padding(0);
            this.panelUnitValues.Name = "panelUnitValues";
            this.panelUnitValues.Size = new System.Drawing.Size(371, 25);
            this.panelUnitValues.TabIndex = 5;
            // 
            // UserControlTableColumnLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlTableColumnLink";
            this.Size = new System.Drawing.Size(419, 49);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxModule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonAddLink;
        private System.Windows.Forms.PictureBox pictureBoxModule;
        private System.Windows.Forms.Button buttonAddUnitValue;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Panel panelUnitValues;
    }
}
