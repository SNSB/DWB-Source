namespace DiversityCollection.CacheDatabase
{
    partial class UserControlProjectPostgresTargets
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
            this.labelTarget = new System.Windows.Forms.Label();
            this.buttonTransferProtocol = new System.Windows.Forms.Button();
            this.buttonTransferErrors = new System.Windows.Forms.Button();
            this.listBoxPackages = new System.Windows.Forms.ListBox();
            this.checkBoxIncludeInTransfer = new System.Windows.Forms.CheckBox();
            this.buttonServer = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 10;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.labelTarget, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferProtocol, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferErrors, 5, 1);
            this.tableLayoutPanel.Controls.Add(this.listBoxPackages, 8, 1);
            this.tableLayoutPanel.Controls.Add(this.checkBoxIncludeInTransfer, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonServer, 7, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonDelete, 6, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(380, 17);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelTarget
            // 
            this.labelTarget.AutoSize = true;
            this.labelTarget.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanel.SetColumnSpan(this.labelTarget, 3);
            this.labelTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTarget.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelTarget.Location = new System.Drawing.Point(24, 1);
            this.labelTarget.Margin = new System.Windows.Forms.Padding(0);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(154, 16);
            this.labelTarget.TabIndex = 0;
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
            this.buttonTransferProtocol.Location = new System.Drawing.Point(178, 1);
            this.buttonTransferProtocol.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferProtocol.Name = "buttonTransferProtocol";
            this.buttonTransferProtocol.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonTransferProtocol.Size = new System.Drawing.Size(12, 16);
            this.buttonTransferProtocol.TabIndex = 1;
            this.buttonTransferProtocol.UseVisualStyleBackColor = false;
            this.buttonTransferProtocol.Click += new System.EventHandler(this.buttonTransferProtocol_Click);
            // 
            // buttonTransferErrors
            // 
            this.buttonTransferErrors.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonTransferErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferErrors.Enabled = false;
            this.buttonTransferErrors.FlatAppearance.BorderSize = 0;
            this.buttonTransferErrors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferErrors.Image = global::DiversityCollection.Resource.ListRed;
            this.buttonTransferErrors.Location = new System.Drawing.Point(190, 1);
            this.buttonTransferErrors.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferErrors.Name = "buttonTransferErrors";
            this.buttonTransferErrors.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonTransferErrors.Size = new System.Drawing.Size(12, 16);
            this.buttonTransferErrors.TabIndex = 2;
            this.buttonTransferErrors.UseVisualStyleBackColor = false;
            this.buttonTransferErrors.Click += new System.EventHandler(this.buttonTransferErrors_Click);
            // 
            // listBoxPackages
            // 
            this.listBoxPackages.BackColor = System.Drawing.Color.PaleTurquoise;
            this.listBoxPackages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel.SetColumnSpan(this.listBoxPackages, 2);
            this.listBoxPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPackages.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.listBoxPackages.FormattingEnabled = true;
            this.listBoxPackages.IntegralHeight = false;
            this.listBoxPackages.Location = new System.Drawing.Point(226, 1);
            this.listBoxPackages.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxPackages.Name = "listBoxPackages";
            this.listBoxPackages.Size = new System.Drawing.Size(154, 16);
            this.listBoxPackages.TabIndex = 3;
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
            this.checkBoxIncludeInTransfer.Size = new System.Drawing.Size(24, 16);
            this.checkBoxIncludeInTransfer.TabIndex = 4;
            this.checkBoxIncludeInTransfer.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBoxIncludeInTransfer.UseVisualStyleBackColor = false;
            // 
            // buttonServer
            // 
            this.buttonServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonServer.FlatAppearance.BorderSize = 0;
            this.buttonServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonServer.Image = global::DiversityCollection.Resource.Server1;
            this.buttonServer.Location = new System.Drawing.Point(214, 1);
            this.buttonServer.Margin = new System.Windows.Forms.Padding(0);
            this.buttonServer.Name = "buttonServer";
            this.buttonServer.Size = new System.Drawing.Size(12, 16);
            this.buttonServer.TabIndex = 5;
            this.buttonServer.UseVisualStyleBackColor = true;
            this.buttonServer.Visible = false;
            this.buttonServer.Click += new System.EventHandler(this.buttonServer_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDelete.FlatAppearance.BorderSize = 0;
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.Image = global::DiversityCollection.Resource.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(202, 1);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(12, 16);
            this.buttonDelete.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonDelete, "Remove this target from the list");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // UserControlProjectPostgresTargets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlProjectPostgresTargets";
            this.Size = new System.Drawing.Size(380, 17);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelTarget;
        private System.Windows.Forms.Button buttonTransferProtocol;
        private System.Windows.Forms.Button buttonTransferErrors;
        private System.Windows.Forms.ListBox listBoxPackages;
        private System.Windows.Forms.CheckBox checkBoxIncludeInTransfer;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonServer;
        private System.Windows.Forms.Button buttonDelete;
    }
}
