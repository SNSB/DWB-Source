namespace DiversityWorkbench.UserControls
{
    partial class UserControlModuleLinks
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
            this.tableLayoutPanelData = new System.Windows.Forms.TableLayoutPanel();
            this.labelNothingFound = new System.Windows.Forms.Label();
            this.groupBoxData = new System.Windows.Forms.GroupBox();
            this.splitContainerData = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelDetails = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOpenModule = new System.Windows.Forms.Button();
            this.checkBoxRestrictToLocalServer = new System.Windows.Forms.CheckBox();
            this.listBoxModuleLinks = new System.Windows.Forms.ListBox();
            this.panelUnitValues = new System.Windows.Forms.Panel();
            this.buttonCreateEntry = new System.Windows.Forms.Button();
            this.panelModulePath = new System.Windows.Forms.Panel();
            this.textBoxModulePath = new System.Windows.Forms.TextBox();
            this.buttonSetModulePath = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxScanModule = new System.Windows.Forms.CheckBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            this.panelOptionalRestrictions = new System.Windows.Forms.Panel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanelData.SuspendLayout();
            this.groupBoxData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).BeginInit();
            this.splitContainerData.Panel1.SuspendLayout();
            this.splitContainerData.Panel2.SuspendLayout();
            this.splitContainerData.SuspendLayout();
            this.tableLayoutPanelDetails.SuspendLayout();
            this.panelModulePath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelData
            // 
            this.tableLayoutPanelData.ColumnCount = 1;
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelData.Controls.Add(this.labelNothingFound, 0, 0);
            this.tableLayoutPanelData.Controls.Add(this.groupBoxData, 0, 1);
            this.tableLayoutPanelData.Controls.Add(this.buttonCreateEntry, 0, 4);
            this.tableLayoutPanelData.Controls.Add(this.panelModulePath, 0, 3);
            this.tableLayoutPanelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelData.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelData.Name = "tableLayoutPanelData";
            this.tableLayoutPanelData.RowCount = 5;
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.Size = new System.Drawing.Size(332, 221);
            this.tableLayoutPanelData.TabIndex = 2;
            // 
            // labelNothingFound
            // 
            this.labelNothingFound.AutoSize = true;
            this.labelNothingFound.Location = new System.Drawing.Point(9, 9);
            this.labelNothingFound.Margin = new System.Windows.Forms.Padding(9);
            this.labelNothingFound.Name = "labelNothingFound";
            this.labelNothingFound.Size = new System.Drawing.Size(237, 13);
            this.labelNothingFound.TabIndex = 2;
            this.labelNothingFound.Text = "No links as defined in the module could be found";
            // 
            // groupBoxData
            // 
            this.groupBoxData.Controls.Add(this.splitContainerData);
            this.groupBoxData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxData.Location = new System.Drawing.Point(0, 31);
            this.groupBoxData.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxData.Name = "groupBoxData";
            this.groupBoxData.Size = new System.Drawing.Size(332, 138);
            this.groupBoxData.TabIndex = 3;
            this.groupBoxData.TabStop = false;
            this.groupBoxData.Text = "Diversity...";
            // 
            // splitContainerData
            // 
            this.splitContainerData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerData.Location = new System.Drawing.Point(3, 16);
            this.splitContainerData.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.splitContainerData.Name = "splitContainerData";
            // 
            // splitContainerData.Panel1
            // 
            this.splitContainerData.Panel1.Controls.Add(this.tableLayoutPanelDetails);
            // 
            // splitContainerData.Panel2
            // 
            this.splitContainerData.Panel2.Controls.Add(this.panelUnitValues);
            this.splitContainerData.Size = new System.Drawing.Size(326, 119);
            this.splitContainerData.SplitterDistance = 157;
            this.splitContainerData.TabIndex = 0;
            // 
            // tableLayoutPanelDetails
            // 
            this.tableLayoutPanelDetails.ColumnCount = 3;
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDetails.Controls.Add(this.buttonOpenModule, 0, 1);
            this.tableLayoutPanelDetails.Controls.Add(this.checkBoxRestrictToLocalServer, 0, 2);
            this.tableLayoutPanelDetails.Controls.Add(this.listBoxModuleLinks, 0, 0);
            this.tableLayoutPanelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDetails.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDetails.Name = "tableLayoutPanelDetails";
            this.tableLayoutPanelDetails.RowCount = 3;
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDetails.Size = new System.Drawing.Size(157, 119);
            this.tableLayoutPanelDetails.TabIndex = 1;
            // 
            // buttonOpenModule
            // 
            this.tableLayoutPanelDetails.SetColumnSpan(this.buttonOpenModule, 3);
            this.buttonOpenModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpenModule.FlatAppearance.BorderSize = 0;
            this.buttonOpenModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpenModule.Image = global::DiversityWorkbench.Properties.Resources.DiversityWorkbench16;
            this.buttonOpenModule.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOpenModule.Location = new System.Drawing.Point(0, 79);
            this.buttonOpenModule.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOpenModule.Name = "buttonOpenModule";
            this.buttonOpenModule.Size = new System.Drawing.Size(157, 23);
            this.buttonOpenModule.TabIndex = 0;
            this.buttonOpenModule.Text = "Open in separate window";
            this.buttonOpenModule.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonOpenModule, "Open module software for selected data");
            this.buttonOpenModule.UseVisualStyleBackColor = true;
            this.buttonOpenModule.Click += new System.EventHandler(this.buttonOpenModule_Click);
            // 
            // checkBoxRestrictToLocalServer
            // 
            this.checkBoxRestrictToLocalServer.AutoSize = true;
            this.checkBoxRestrictToLocalServer.Checked = true;
            this.checkBoxRestrictToLocalServer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelDetails.SetColumnSpan(this.checkBoxRestrictToLocalServer, 2);
            this.checkBoxRestrictToLocalServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxRestrictToLocalServer.Enabled = false;
            this.checkBoxRestrictToLocalServer.Location = new System.Drawing.Point(0, 102);
            this.checkBoxRestrictToLocalServer.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxRestrictToLocalServer.Name = "checkBoxRestrictToLocalServer";
            this.checkBoxRestrictToLocalServer.Size = new System.Drawing.Size(131, 17);
            this.checkBoxRestrictToLocalServer.TabIndex = 1;
            this.checkBoxRestrictToLocalServer.Text = "Restrict to local server";
            this.checkBoxRestrictToLocalServer.UseVisualStyleBackColor = true;
            this.checkBoxRestrictToLocalServer.Visible = false;
            // 
            // listBoxModuleLinks
            // 
            this.tableLayoutPanelDetails.SetColumnSpan(this.listBoxModuleLinks, 3);
            this.listBoxModuleLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxModuleLinks.FormattingEnabled = true;
            this.listBoxModuleLinks.IntegralHeight = false;
            this.listBoxModuleLinks.Location = new System.Drawing.Point(0, 0);
            this.listBoxModuleLinks.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.listBoxModuleLinks.Name = "listBoxModuleLinks";
            this.listBoxModuleLinks.Size = new System.Drawing.Size(157, 76);
            this.listBoxModuleLinks.TabIndex = 0;
            this.listBoxModuleLinks.SelectedIndexChanged += new System.EventHandler(this.listBoxModuleLinks_SelectedIndexChanged);
            // 
            // panelUnitValues
            // 
            this.panelUnitValues.AutoScroll = true;
            this.panelUnitValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUnitValues.Location = new System.Drawing.Point(0, 0);
            this.panelUnitValues.Name = "panelUnitValues";
            this.panelUnitValues.Size = new System.Drawing.Size(165, 119);
            this.panelUnitValues.TabIndex = 0;
            // 
            // buttonCreateEntry
            // 
            this.buttonCreateEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCreateEntry.Image = global::DiversityWorkbench.Properties.Resources.DiversityWorkbench16;
            this.buttonCreateEntry.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCreateEntry.Location = new System.Drawing.Point(3, 195);
            this.buttonCreateEntry.Name = "buttonCreateEntry";
            this.buttonCreateEntry.Size = new System.Drawing.Size(326, 23);
            this.buttonCreateEntry.TabIndex = 5;
            this.buttonCreateEntry.Text = "Create ";
            this.buttonCreateEntry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCreateEntry.UseVisualStyleBackColor = true;
            this.buttonCreateEntry.Visible = false;
            this.buttonCreateEntry.Click += new System.EventHandler(this.buttonCreateEntry_Click);
            // 
            // panelModulePath
            // 
            this.panelModulePath.Controls.Add(this.textBoxModulePath);
            this.panelModulePath.Controls.Add(this.buttonSetModulePath);
            this.panelModulePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelModulePath.Location = new System.Drawing.Point(0, 169);
            this.panelModulePath.Margin = new System.Windows.Forms.Padding(0);
            this.panelModulePath.Name = "panelModulePath";
            this.panelModulePath.Size = new System.Drawing.Size(332, 23);
            this.panelModulePath.TabIndex = 6;
            // 
            // textBoxModulePath
            // 
            this.textBoxModulePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxModulePath.Location = new System.Drawing.Point(25, 0);
            this.textBoxModulePath.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxModulePath.Name = "textBoxModulePath";
            this.textBoxModulePath.ReadOnly = true;
            this.textBoxModulePath.Size = new System.Drawing.Size(307, 20);
            this.textBoxModulePath.TabIndex = 4;
            this.textBoxModulePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // buttonSetModulePath
            // 
            this.buttonSetModulePath.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSetModulePath.FlatAppearance.BorderSize = 0;
            this.buttonSetModulePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetModulePath.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.buttonSetModulePath.Location = new System.Drawing.Point(0, 0);
            this.buttonSetModulePath.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetModulePath.Name = "buttonSetModulePath";
            this.buttonSetModulePath.Size = new System.Drawing.Size(25, 23);
            this.buttonSetModulePath.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonSetModulePath, "Set path for the module software");
            this.buttonSetModulePath.UseVisualStyleBackColor = true;
            this.buttonSetModulePath.Click += new System.EventHandler(this.buttonSetModulePath_Click);
            // 
            // checkBoxScanModule
            // 
            this.checkBoxScanModule.AutoSize = true;
            this.checkBoxScanModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxScanModule.Location = new System.Drawing.Point(3, 0);
            this.checkBoxScanModule.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.checkBoxScanModule.Name = "checkBoxScanModule";
            this.checkBoxScanModule.Size = new System.Drawing.Size(91, 26);
            this.checkBoxScanModule.TabIndex = 0;
            this.checkBoxScanModule.Text = "Scan module ";
            this.toolTip.SetToolTip(this.checkBoxScanModule, "If databases of the module should be scanned for links to the selected dataset");
            this.checkBoxScanModule.UseVisualStyleBackColor = true;
            this.checkBoxScanModule.Click += new System.EventHandler(this.checkBoxScanModule_Click);
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCount.Location = new System.Drawing.Point(323, 0);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(1, 26);
            this.labelCount.TabIndex = 6;
            this.labelCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.labelCount, "Number of datasets");
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tableLayoutPanelHeader);
            this.splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelData);
            this.splitContainerMain.Size = new System.Drawing.Size(332, 257);
            this.splitContainerMain.SplitterDistance = 32;
            this.splitContainerMain.TabIndex = 3;
            // 
            // tableLayoutPanelHeader
            // 
            this.tableLayoutPanelHeader.ColumnCount = 3;
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.Controls.Add(this.checkBoxScanModule, 0, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.panelOptionalRestrictions, 1, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.labelCount, 2, 0);
            this.tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelHeader.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            this.tableLayoutPanelHeader.RowCount = 1;
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.Size = new System.Drawing.Size(326, 26);
            this.tableLayoutPanelHeader.TabIndex = 6;
            // 
            // panelOptionalRestrictions
            // 
            this.panelOptionalRestrictions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOptionalRestrictions.Location = new System.Drawing.Point(94, 0);
            this.panelOptionalRestrictions.Margin = new System.Windows.Forms.Padding(0);
            this.panelOptionalRestrictions.Name = "panelOptionalRestrictions";
            this.panelOptionalRestrictions.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelOptionalRestrictions.Size = new System.Drawing.Size(226, 26);
            this.panelOptionalRestrictions.TabIndex = 5;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // UserControlModuleLinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Name = "UserControlModuleLinks";
            this.Size = new System.Drawing.Size(332, 257);
            this.tableLayoutPanelData.ResumeLayout(false);
            this.tableLayoutPanelData.PerformLayout();
            this.groupBoxData.ResumeLayout(false);
            this.splitContainerData.Panel1.ResumeLayout(false);
            this.splitContainerData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).EndInit();
            this.splitContainerData.ResumeLayout(false);
            this.tableLayoutPanelDetails.ResumeLayout(false);
            this.tableLayoutPanelDetails.PerformLayout();
            this.panelModulePath.ResumeLayout(false);
            this.panelModulePath.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelHeader.ResumeLayout(false);
            this.tableLayoutPanelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelData;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.ListBox listBoxModuleLinks;
        private System.Windows.Forms.Button buttonOpenModule;
        private System.Windows.Forms.CheckBox checkBoxRestrictToLocalServer;
        private System.Windows.Forms.Label labelNothingFound;
        private System.Windows.Forms.Button buttonSetModulePath;
        private System.Windows.Forms.TextBox textBoxModulePath;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelUnitValues;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.CheckBox checkBoxScanModule;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDetails;
        private System.Windows.Forms.Panel panelOptionalRestrictions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.GroupBox groupBoxData;
        private System.Windows.Forms.Button buttonCreateEntry;
        private System.Windows.Forms.Panel panelModulePath;
    }
}
