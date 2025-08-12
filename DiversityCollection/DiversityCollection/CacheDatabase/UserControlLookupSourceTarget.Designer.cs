namespace DiversityCollection.CacheDatabase
{
    partial class UserControlLookupSourceTarget
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
            this.checkBoxIncludeInTransfer = new System.Windows.Forms.CheckBox();
            this.labelTarget = new System.Windows.Forms.Label();
            this.buttonTransferProtocol = new System.Windows.Forms.Button();
            this.buttonTransferErrors = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel.Controls.Add(this.buttonTransferErrors, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferProtocol, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.labelTarget, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.checkBoxIncludeInTransfer, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(180, 17);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // checkBoxIncludeInTransfer
            // 
            this.checkBoxIncludeInTransfer.AutoSize = true;
            this.checkBoxIncludeInTransfer.BackColor = System.Drawing.Color.LightSteelBlue;
            this.checkBoxIncludeInTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxIncludeInTransfer.Enabled = false;
            this.checkBoxIncludeInTransfer.Location = new System.Drawing.Point(0, 1);
            this.checkBoxIncludeInTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxIncludeInTransfer.Name = "checkBoxIncludeInTransfer";
            this.checkBoxIncludeInTransfer.Padding = new System.Windows.Forms.Padding(3, 1, 0, 0);
            this.checkBoxIncludeInTransfer.Size = new System.Drawing.Size(20, 16);
            this.checkBoxIncludeInTransfer.TabIndex = 5;
            this.checkBoxIncludeInTransfer.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBoxIncludeInTransfer.UseVisualStyleBackColor = false;
            // 
            // labelTarget
            // 
            this.labelTarget.AutoSize = true;
            this.labelTarget.BackColor = System.Drawing.Color.LightSteelBlue;
            this.labelTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTarget.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelTarget.Location = new System.Drawing.Point(20, 1);
            this.labelTarget.Margin = new System.Windows.Forms.Padding(0);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(134, 16);
            this.labelTarget.TabIndex = 6;
            this.labelTarget.Text = "DiversityCollectionCache";
            this.labelTarget.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonTransferProtocol
            // 
            this.buttonTransferProtocol.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonTransferProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferProtocol.Enabled = false;
            this.buttonTransferProtocol.FlatAppearance.BorderSize = 0;
            this.buttonTransferProtocol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferProtocol.Image = global::DiversityCollection.Resource.List;
            this.buttonTransferProtocol.Location = new System.Drawing.Point(154, 1);
            this.buttonTransferProtocol.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferProtocol.Name = "buttonTransferProtocol";
            this.buttonTransferProtocol.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonTransferProtocol.Size = new System.Drawing.Size(13, 16);
            this.buttonTransferProtocol.TabIndex = 7;
            this.buttonTransferProtocol.UseVisualStyleBackColor = false;
            // 
            // buttonTransferErrors
            // 
            this.buttonTransferErrors.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonTransferErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferErrors.Enabled = false;
            this.buttonTransferErrors.FlatAppearance.BorderSize = 0;
            this.buttonTransferErrors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferErrors.Image = global::DiversityCollection.Resource.ListRed;
            this.buttonTransferErrors.Location = new System.Drawing.Point(167, 1);
            this.buttonTransferErrors.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferErrors.Name = "buttonTransferErrors";
            this.buttonTransferErrors.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonTransferErrors.Size = new System.Drawing.Size(13, 16);
            this.buttonTransferErrors.TabIndex = 8;
            this.buttonTransferErrors.UseVisualStyleBackColor = false;
            // 
            // UserControlLookupSourceTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlLookupSourceTarget";
            this.Size = new System.Drawing.Size(180, 17);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.CheckBox checkBoxIncludeInTransfer;
        private System.Windows.Forms.Label labelTarget;
        private System.Windows.Forms.Button buttonTransferProtocol;
        private System.Windows.Forms.Button buttonTransferErrors;
    }
}
