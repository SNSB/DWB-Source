namespace DiversityWorkbench.UserControls
{
    partial class UserControlUserSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlUserSettings));
            this.treeViewSettings = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelUserSettingsReset = new System.Windows.Forms.Label();
            this.buttonResetAll = new System.Windows.Forms.Button();
            this.buttonSetNode = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewSettings
            // 
            this.tableLayoutPanel.SetColumnSpan(this.treeViewSettings, 3);
            this.treeViewSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSettings.Enabled = false;
            this.treeViewSettings.ImageIndex = 0;
            this.treeViewSettings.ImageList = this.imageList;
            this.treeViewSettings.Location = new System.Drawing.Point(3, 3);
            this.treeViewSettings.Name = "treeViewSettings";
            this.treeViewSettings.SelectedImageIndex = 0;
            this.treeViewSettings.Size = new System.Drawing.Size(396, 120);
            this.treeViewSettings.TabIndex = 0;
            this.treeViewSettings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewSettings_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Settings.ico");
            this.imageList.Images.SetKeyName(1, "Workbench.ico");
            this.imageList.Images.SetKeyName(2, "Speadsheet.ico");
            this.imageList.Images.SetKeyName(3, "TableColumn.ico");
            this.imageList.Images.SetKeyName(4, "TableCell.ico");
            this.imageList.Images.SetKeyName(5, "Pin_3.ico");
            this.imageList.Images.SetKeyName(6, "Database.ico");
            this.imageList.Images.SetKeyName(7, "CacheDB.ico");
            this.imageList.Images.SetKeyName(8, "Project.ico");
            this.imageList.Images.SetKeyName(9, "Webservice.ico");
            this.imageList.Images.SetKeyName(10, "NULL.ico");
            this.imageList.Images.SetKeyName(11, "Checklist.ico");
            this.imageList.Images.SetKeyName(12, "Section.ico");
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelUserSettingsReset, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonResetAll, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonSetNode, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.treeViewSettings, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(402, 150);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // labelUserSettingsReset
            // 
            this.labelUserSettingsReset.AutoSize = true;
            this.labelUserSettingsReset.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelUserSettingsReset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelUserSettingsReset.Location = new System.Drawing.Point(29, 126);
            this.labelUserSettingsReset.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelUserSettingsReset.MaximumSize = new System.Drawing.Size(300, 24);
            this.labelUserSettingsReset.MinimumSize = new System.Drawing.Size(300, 24);
            this.labelUserSettingsReset.Name = "labelUserSettingsReset";
            this.labelUserSettingsReset.Size = new System.Drawing.Size(300, 24);
            this.labelUserSettingsReset.TabIndex = 5;
            this.labelUserSettingsReset.Text = "Clear column Settings in table UserProxy for the current user:";
            this.labelUserSettingsReset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelUserSettingsReset.Visible = false;
            // 
            // buttonResetAll
            // 
            this.buttonResetAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonResetAll.ForeColor = System.Drawing.Color.Red;
            this.buttonResetAll.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonResetAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonResetAll.Location = new System.Drawing.Point(329, 126);
            this.buttonResetAll.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonResetAll.MaximumSize = new System.Drawing.Size(70, 24);
            this.buttonResetAll.MinimumSize = new System.Drawing.Size(70, 24);
            this.buttonResetAll.Name = "buttonResetAll";
            this.buttonResetAll.Size = new System.Drawing.Size(70, 24);
            this.buttonResetAll.TabIndex = 4;
            this.buttonResetAll.Text = "Reset all";
            this.buttonResetAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonResetAll, "Clear column Settings in table UserProxy for the current user");
            this.buttonResetAll.UseVisualStyleBackColor = true;
            this.buttonResetAll.Click += new System.EventHandler(this.buttonResetAll_Click);
            // 
            // buttonSetNode
            // 
            this.buttonSetNode.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSetNode.Image = global::DiversityWorkbench.Properties.Resources.Edit;
            this.buttonSetNode.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSetNode.Location = new System.Drawing.Point(0, 126);
            this.buttonSetNode.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetNode.MaximumSize = new System.Drawing.Size(0, 24);
            this.buttonSetNode.Name = "buttonSetNode";
            this.buttonSetNode.Size = new System.Drawing.Size(0, 24);
            this.buttonSetNode.TabIndex = 3;
            this.buttonSetNode.Text = "Set Value";
            this.buttonSetNode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSetNode.UseVisualStyleBackColor = true;
            this.buttonSetNode.Visible = false;
            this.buttonSetNode.Click += new System.EventHandler(this.buttonSetNode_Click);
            // 
            // UserControlUserSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlUserSettings";
            this.Size = new System.Drawing.Size(402, 150);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        public System.Windows.Forms.TreeView treeViewSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonSetNode;
        private System.Windows.Forms.Button buttonResetAll;
        private System.Windows.Forms.Label labelUserSettingsReset;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
