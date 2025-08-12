namespace DiversityWorkbench.UserControls
{
    partial class UserControlModuleBackLinks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlModuleBackLinks));
            this.tableLayoutPanelData = new System.Windows.Forms.TableLayoutPanel();
            this.labelNothingFound = new System.Windows.Forms.Label();
            this.buttonCreateEntry = new System.Windows.Forms.Button();
            this.groupBoxData = new System.Windows.Forms.GroupBox();
            this.splitContainerData = new System.Windows.Forms.SplitContainer();
            this.treeViewLinks = new System.Windows.Forms.TreeView();
            this.tableLayoutPanelUnitValues = new System.Windows.Forms.TableLayoutPanel();
            this.panelUnitValues = new System.Windows.Forms.Panel();
            this.buttonOpenModule = new System.Windows.Forms.Button();
            this.buttonShowHtml = new System.Windows.Forms.Button();
            this.panelModulePath = new System.Windows.Forms.Panel();
            this.textBoxModulePath = new System.Windows.Forms.TextBox();
            this.buttonSetModulePath = new System.Windows.Forms.Button();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanelData.SuspendLayout();
            this.groupBoxData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).BeginInit();
            this.splitContainerData.Panel1.SuspendLayout();
            this.splitContainerData.Panel2.SuspendLayout();
            this.splitContainerData.SuspendLayout();
            this.tableLayoutPanelUnitValues.SuspendLayout();
            this.panelModulePath.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelData
            // 
            this.tableLayoutPanelData.ColumnCount = 1;
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelData.Controls.Add(this.labelNothingFound, 0, 0);
            this.tableLayoutPanelData.Controls.Add(this.buttonCreateEntry, 0, 3);
            this.tableLayoutPanelData.Controls.Add(this.groupBoxData, 0, 1);
            this.tableLayoutPanelData.Controls.Add(this.panelModulePath, 0, 2);
            this.tableLayoutPanelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelData.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelData.Name = "tableLayoutPanelData";
            this.tableLayoutPanelData.RowCount = 4;
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelData.Size = new System.Drawing.Size(260, 358);
            this.tableLayoutPanelData.TabIndex = 2;
            // 
            // labelNothingFound
            // 
            this.labelNothingFound.AutoSize = true;
            this.labelNothingFound.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelNothingFound.Location = new System.Drawing.Point(9, 9);
            this.labelNothingFound.Margin = new System.Windows.Forms.Padding(9);
            this.labelNothingFound.Name = "labelNothingFound";
            this.labelNothingFound.Size = new System.Drawing.Size(237, 13);
            this.labelNothingFound.TabIndex = 2;
            this.labelNothingFound.Text = "No links as defined in the module could be found";
            // 
            // buttonCreateEntry
            // 
            this.buttonCreateEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCreateEntry.Image = global::DiversityWorkbench.Properties.Resources.DiversityWorkbench16;
            this.buttonCreateEntry.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCreateEntry.Location = new System.Drawing.Point(3, 321);
            this.buttonCreateEntry.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.buttonCreateEntry.Name = "buttonCreateEntry";
            this.buttonCreateEntry.Size = new System.Drawing.Size(254, 34);
            this.buttonCreateEntry.TabIndex = 5;
            this.buttonCreateEntry.Text = "Create ";
            this.buttonCreateEntry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonCreateEntry, "To create a new entry please select the database where the entry should be create" +
        "d in the tree above");
            this.buttonCreateEntry.UseVisualStyleBackColor = true;
            this.buttonCreateEntry.Visible = false;
            this.buttonCreateEntry.Click += new System.EventHandler(this.buttonCreateEntry_Click);
            // 
            // groupBoxData
            // 
            this.groupBoxData.Controls.Add(this.splitContainerData);
            this.groupBoxData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxData.Location = new System.Drawing.Point(0, 31);
            this.groupBoxData.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxData.Name = "groupBoxData";
            this.groupBoxData.Size = new System.Drawing.Size(260, 267);
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
            this.splitContainerData.Panel1.Controls.Add(this.treeViewLinks);
            // 
            // splitContainerData.Panel2
            // 
            this.splitContainerData.Panel2.Controls.Add(this.tableLayoutPanelUnitValues);
            this.splitContainerData.Size = new System.Drawing.Size(254, 248);
            this.splitContainerData.SplitterDistance = 127;
            this.splitContainerData.TabIndex = 0;
            // 
            // treeViewLinks
            // 
            this.treeViewLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLinks.Location = new System.Drawing.Point(0, 0);
            this.treeViewLinks.Name = "treeViewLinks";
            this.treeViewLinks.Size = new System.Drawing.Size(127, 248);
            this.treeViewLinks.TabIndex = 1;
            this.treeViewLinks.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewLinks_AfterSelect);
            // 
            // tableLayoutPanelUnitValues
            // 
            this.tableLayoutPanelUnitValues.ColumnCount = 2;
            this.tableLayoutPanelUnitValues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUnitValues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelUnitValues.Controls.Add(this.panelUnitValues, 0, 0);
            this.tableLayoutPanelUnitValues.Controls.Add(this.buttonOpenModule, 0, 1);
            this.tableLayoutPanelUnitValues.Controls.Add(this.buttonShowHtml, 1, 1);
            this.tableLayoutPanelUnitValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelUnitValues.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelUnitValues.Name = "tableLayoutPanelUnitValues";
            this.tableLayoutPanelUnitValues.RowCount = 2;
            this.tableLayoutPanelUnitValues.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUnitValues.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelUnitValues.Size = new System.Drawing.Size(123, 248);
            this.tableLayoutPanelUnitValues.TabIndex = 1;
            // 
            // panelUnitValues
            // 
            this.panelUnitValues.AutoScroll = true;
            this.tableLayoutPanelUnitValues.SetColumnSpan(this.panelUnitValues, 2);
            this.panelUnitValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUnitValues.Location = new System.Drawing.Point(3, 3);
            this.panelUnitValues.Name = "panelUnitValues";
            this.panelUnitValues.Size = new System.Drawing.Size(117, 219);
            this.panelUnitValues.TabIndex = 0;
            // 
            // buttonOpenModule
            // 
            this.buttonOpenModule.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonOpenModule.FlatAppearance.BorderSize = 0;
            this.buttonOpenModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpenModule.Image = global::DiversityWorkbench.Properties.Resources.DiversityWorkbench16;
            this.buttonOpenModule.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOpenModule.Location = new System.Drawing.Point(0, 225);
            this.buttonOpenModule.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOpenModule.Name = "buttonOpenModule";
            this.buttonOpenModule.Size = new System.Drawing.Size(100, 23);
            this.buttonOpenModule.TabIndex = 0;
            this.buttonOpenModule.Text = "      Open in separate window";
            this.buttonOpenModule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOpenModule.UseVisualStyleBackColor = true;
            this.buttonOpenModule.Click += new System.EventHandler(this.buttonOpenModule_Click);
            // 
            // buttonShowHtml
            // 
            this.buttonShowHtml.FlatAppearance.BorderSize = 0;
            this.buttonShowHtml.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShowHtml.Image = global::DiversityWorkbench.Properties.Resources.Browse;
            this.buttonShowHtml.Location = new System.Drawing.Point(100, 225);
            this.buttonShowHtml.Margin = new System.Windows.Forms.Padding(0);
            this.buttonShowHtml.Name = "buttonShowHtml";
            this.buttonShowHtml.Size = new System.Drawing.Size(23, 23);
            this.buttonShowHtml.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonShowHtml, "Show values in browser");
            this.buttonShowHtml.UseVisualStyleBackColor = true;
            this.buttonShowHtml.Click += new System.EventHandler(this.buttonShowHtml_Click);
            // 
            // panelModulePath
            // 
            this.panelModulePath.Controls.Add(this.textBoxModulePath);
            this.panelModulePath.Controls.Add(this.buttonSetModulePath);
            this.panelModulePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelModulePath.Location = new System.Drawing.Point(3, 298);
            this.panelModulePath.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelModulePath.Name = "panelModulePath";
            this.panelModulePath.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.panelModulePath.Size = new System.Drawing.Size(257, 23);
            this.panelModulePath.TabIndex = 6;
            // 
            // textBoxModulePath
            // 
            this.textBoxModulePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxModulePath.Location = new System.Drawing.Point(25, 0);
            this.textBoxModulePath.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxModulePath.Name = "textBoxModulePath";
            this.textBoxModulePath.ReadOnly = true;
            this.textBoxModulePath.Size = new System.Drawing.Size(229, 20);
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
            this.buttonSetModulePath.UseVisualStyleBackColor = true;
            this.buttonSetModulePath.Click += new System.EventHandler(this.buttonSetModulePath_Click);
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "Database.ico");
            this.imageListTree.Images.SetKeyName(1, "Speadsheet.ico");
            this.imageListTree.Images.SetKeyName(2, "Link.ico");
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // UserControlModuleBackLinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelData);
            this.Name = "UserControlModuleBackLinks";
            this.Size = new System.Drawing.Size(260, 358);
            this.tableLayoutPanelData.ResumeLayout(false);
            this.tableLayoutPanelData.PerformLayout();
            this.groupBoxData.ResumeLayout(false);
            this.splitContainerData.Panel1.ResumeLayout(false);
            this.splitContainerData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).EndInit();
            this.splitContainerData.ResumeLayout(false);
            this.tableLayoutPanelUnitValues.ResumeLayout(false);
            this.panelModulePath.ResumeLayout(false);
            this.panelModulePath.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelData;
        private System.Windows.Forms.Label labelNothingFound;
        private System.Windows.Forms.GroupBox groupBoxData;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.Button buttonOpenModule;
        private System.Windows.Forms.Panel panelUnitValues;
        private System.Windows.Forms.Button buttonCreateEntry;
        private System.Windows.Forms.Panel panelModulePath;
        private System.Windows.Forms.TextBox textBoxModulePath;
        private System.Windows.Forms.Button buttonSetModulePath;
        private System.Windows.Forms.TreeView treeViewLinks;
        private System.Windows.Forms.ImageList imageListTree;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUnitValues;
        private System.Windows.Forms.Button buttonShowHtml;
    }
}
