namespace DiversityCollection.CacheDatabase.Packages
{
    partial class UserControlPackage
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
            components = new System.ComponentModel.Container();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            buttonDeletePackage = new System.Windows.Forms.Button();
            labelName = new System.Windows.Forms.Label();
            buttonUpdate = new System.Windows.Forms.Button();
            buttonInfo = new System.Windows.Forms.Button();
            buttonViewContent = new System.Windows.Forms.Button();
            buttonExport = new System.Windows.Forms.Button();
            buttonAddOn = new System.Windows.Forms.Button();
            panelAddOns = new System.Windows.Forms.Panel();
            buttonTransferToMaterializedTables = new System.Windows.Forms.Button();
            panelSeparator = new System.Windows.Forms.Panel();
            labelTransferState = new System.Windows.Forms.Label();
            buttonTimeout = new System.Windows.Forms.Button();
            buttonHistory = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 10;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel.Controls.Add(buttonDeletePackage, 9, 0);
            tableLayoutPanel.Controls.Add(labelName, 0, 0);
            tableLayoutPanel.Controls.Add(buttonUpdate, 3, 0);
            tableLayoutPanel.Controls.Add(buttonInfo, 8, 0);
            tableLayoutPanel.Controls.Add(buttonViewContent, 5, 0);
            tableLayoutPanel.Controls.Add(buttonExport, 7, 0);
            tableLayoutPanel.Controls.Add(buttonAddOn, 1, 1);
            tableLayoutPanel.Controls.Add(panelAddOns, 2, 1);
            tableLayoutPanel.Controls.Add(buttonTransferToMaterializedTables, 2, 2);
            tableLayoutPanel.Controls.Add(panelSeparator, 0, 3);
            tableLayoutPanel.Controls.Add(labelTransferState, 4, 2);
            tableLayoutPanel.Controls.Add(buttonTimeout, 0, 2);
            tableLayoutPanel.Controls.Add(buttonHistory, 6, 0);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 4;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            tableLayoutPanel.Size = new System.Drawing.Size(414, 93);
            tableLayoutPanel.TabIndex = 0;
            // 
            // buttonDeletePackage
            // 
            buttonDeletePackage.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonDeletePackage.Image = Resource.Delete;
            buttonDeletePackage.Location = new System.Drawing.Point(383, 3);
            buttonDeletePackage.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            buttonDeletePackage.Name = "buttonDeletePackage";
            buttonDeletePackage.Size = new System.Drawing.Size(27, 29);
            buttonDeletePackage.TabIndex = 0;
            buttonDeletePackage.UseVisualStyleBackColor = true;
            buttonDeletePackage.Click += buttonDeletePackage_Click;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(labelName, 3);
            labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            labelName.Location = new System.Drawing.Point(4, 0);
            labelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelName.Name = "labelName";
            labelName.Size = new System.Drawing.Size(132, 32);
            labelName.TabIndex = 1;
            labelName.Text = "Package";
            labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonUpdate
            // 
            tableLayoutPanel.SetColumnSpan(buttonUpdate, 2);
            buttonUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonUpdate.Image = Resource.Update;
            buttonUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonUpdate.Location = new System.Drawing.Point(144, 3);
            buttonUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new System.Drawing.Size(123, 29);
            buttonUpdate.TabIndex = 2;
            buttonUpdate.Text = "Upd. to vers. ";
            buttonUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonUpdate.UseVisualStyleBackColor = true;
            buttonUpdate.Visible = false;
            buttonUpdate.Click += buttonUpdate_Click;
            // 
            // buttonInfo
            // 
            buttonInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonInfo.Image = Resource.Manual;
            buttonInfo.Location = new System.Drawing.Point(355, 3);
            buttonInfo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            buttonInfo.Name = "buttonInfo";
            buttonInfo.Size = new System.Drawing.Size(28, 29);
            buttonInfo.TabIndex = 3;
            buttonInfo.UseVisualStyleBackColor = true;
            buttonInfo.Click += buttonInfo_Click;
            // 
            // buttonViewContent
            // 
            buttonViewContent.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonViewContent.Image = Resource.Lupe;
            buttonViewContent.Location = new System.Drawing.Point(271, 3);
            buttonViewContent.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            buttonViewContent.Name = "buttonViewContent";
            buttonViewContent.Size = new System.Drawing.Size(28, 29);
            buttonViewContent.TabIndex = 4;
            toolTip.SetToolTip(buttonViewContent, "View the content of the objects of the package");
            buttonViewContent.UseVisualStyleBackColor = true;
            buttonViewContent.Click += buttonViewContent_Click;
            // 
            // buttonExport
            // 
            buttonExport.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonExport.Image = Resource.Export;
            buttonExport.Location = new System.Drawing.Point(327, 3);
            buttonExport.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            buttonExport.Name = "buttonExport";
            buttonExport.Size = new System.Drawing.Size(28, 29);
            buttonExport.TabIndex = 5;
            toolTip.SetToolTip(buttonExport, "Export the data of the package");
            buttonExport.UseVisualStyleBackColor = true;
            buttonExport.Click += buttonExport_Click;
            // 
            // buttonAddOn
            // 
            buttonAddOn.Dock = System.Windows.Forms.DockStyle.Top;
            buttonAddOn.Image = Resource.PackageAddOn;
            buttonAddOn.Location = new System.Drawing.Point(58, 32);
            buttonAddOn.Margin = new System.Windows.Forms.Padding(0);
            buttonAddOn.Name = "buttonAddOn";
            buttonAddOn.Size = new System.Drawing.Size(28, 28);
            buttonAddOn.TabIndex = 6;
            toolTip.SetToolTip(buttonAddOn, "Add an add-on");
            buttonAddOn.UseVisualStyleBackColor = true;
            buttonAddOn.Click += buttonAddOn_Click;
            // 
            // panelAddOns
            // 
            tableLayoutPanel.SetColumnSpan(panelAddOns, 7);
            panelAddOns.Dock = System.Windows.Forms.DockStyle.Fill;
            panelAddOns.Location = new System.Drawing.Point(86, 32);
            panelAddOns.Margin = new System.Windows.Forms.Padding(0);
            panelAddOns.Name = "panelAddOns";
            panelAddOns.Size = new System.Drawing.Size(297, 29);
            panelAddOns.TabIndex = 7;
            // 
            // buttonTransferToMaterializedTables
            // 
            tableLayoutPanel.SetColumnSpan(buttonTransferToMaterializedTables, 2);
            buttonTransferToMaterializedTables.Dock = System.Windows.Forms.DockStyle.Right;
            buttonTransferToMaterializedTables.Image = Resource.PackageTransfer;
            buttonTransferToMaterializedTables.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonTransferToMaterializedTables.Location = new System.Drawing.Point(90, 61);
            buttonTransferToMaterializedTables.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            buttonTransferToMaterializedTables.Name = "buttonTransferToMaterializedTables";
            buttonTransferToMaterializedTables.Size = new System.Drawing.Size(84, 28);
            buttonTransferToMaterializedTables.TabIndex = 8;
            buttonTransferToMaterializedTables.Text = "Transfer";
            buttonTransferToMaterializedTables.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonTransferToMaterializedTables.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            toolTip.SetToolTip(buttonTransferToMaterializedTables, "Transfer data to package tables resp. run package related procedures");
            buttonTransferToMaterializedTables.UseVisualStyleBackColor = true;
            buttonTransferToMaterializedTables.Click += buttonTransferToMaterializedTables_Click;
            // 
            // panelSeparator
            // 
            panelSeparator.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            tableLayoutPanel.SetColumnSpan(panelSeparator, 10);
            panelSeparator.Dock = System.Windows.Forms.DockStyle.Fill;
            panelSeparator.Location = new System.Drawing.Point(0, 92);
            panelSeparator.Margin = new System.Windows.Forms.Padding(0);
            panelSeparator.Name = "panelSeparator";
            panelSeparator.Size = new System.Drawing.Size(414, 1);
            panelSeparator.TabIndex = 9;
            // 
            // labelTransferState
            // 
            labelTransferState.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(labelTransferState, 6);
            labelTransferState.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTransferState.Location = new System.Drawing.Point(178, 61);
            labelTransferState.Margin = new System.Windows.Forms.Padding(0);
            labelTransferState.Name = "labelTransferState";
            labelTransferState.Size = new System.Drawing.Size(236, 31);
            labelTransferState.TabIndex = 10;
            labelTransferState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            labelTransferState.Visible = false;
            // 
            // buttonTimeout
            // 
            tableLayoutPanel.SetColumnSpan(buttonTimeout, 2);
            buttonTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonTimeout.Image = Resource.Time;
            buttonTimeout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonTimeout.Location = new System.Drawing.Point(4, 61);
            buttonTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 0, 3);
            buttonTimeout.Name = "buttonTimeout";
            buttonTimeout.Size = new System.Drawing.Size(82, 28);
            buttonTimeout.TabIndex = 11;
            buttonTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(buttonTimeout, "set timeout");
            buttonTimeout.UseVisualStyleBackColor = true;
            buttonTimeout.Click += buttonTimeout_Click;
            // 
            // buttonHistory
            // 
            buttonHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonHistory.Image = Resource.History;
            buttonHistory.Location = new System.Drawing.Point(299, 3);
            buttonHistory.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            buttonHistory.Name = "buttonHistory";
            buttonHistory.Size = new System.Drawing.Size(28, 29);
            buttonHistory.TabIndex = 12;
            toolTip.SetToolTip(buttonHistory, "Show the transfer history for this package");
            buttonHistory.UseVisualStyleBackColor = true;
            buttonHistory.Click += buttonHistory_Click;
            // 
            // UserControlPackage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "UserControlPackage";
            Size = new System.Drawing.Size(414, 93);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonDeletePackage;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.Button buttonViewContent;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Panel panelAddOns;
        public System.Windows.Forms.Button buttonUpdate;
        public System.Windows.Forms.Button buttonAddOn;
        private System.Windows.Forms.Button buttonTransferToMaterializedTables;
        private System.Windows.Forms.Panel panelSeparator;
        private System.Windows.Forms.Label labelTransferState;
        private System.Windows.Forms.Button buttonTimeout;
        private System.Windows.Forms.Button buttonHistory;
    }
}
