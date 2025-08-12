namespace DiversityWorkbench.Forms
{
    partial class FormLoginOverview
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoginOverview));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelTarget = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxTargetLogin = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButtonTransferAllSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTransferDatabaseSettings = new System.Windows.Forms.ToolStripButton();
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelTarget,
            this.toolStripComboBoxTargetLogin,
            this.toolStripButtonTransferAllSettings,
            this.toolStripButtonTransferDatabaseSettings});
            this.toolStrip.Location = new System.Drawing.Point(0, 646);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(342, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripLabelTarget
            // 
            this.toolStripLabelTarget.Name = "toolStripLabelTarget";
            this.toolStripLabelTarget.Size = new System.Drawing.Size(136, 22);
            this.toolStripLabelTarget.Text = "Copy settings into login:";
            // 
            // toolStripComboBoxTargetLogin
            // 
            this.toolStripComboBoxTargetLogin.Name = "toolStripComboBoxTargetLogin";
            this.toolStripComboBoxTargetLogin.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxTargetLogin.ToolTipText = "The target login where the settings of the current login should be copied to";
            this.toolStripComboBoxTargetLogin.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxTargetLogin_SelectedIndexChanged);
            // 
            // toolStripButtonTransferAllSettings
            // 
            this.toolStripButtonTransferAllSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTransferAllSettings.Image = global::DiversityWorkbench.Properties.Resources.ToServer;
            this.toolStripButtonTransferAllSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTransferAllSettings.Name = "toolStripButtonTransferAllSettings";
            this.toolStripButtonTransferAllSettings.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTransferAllSettings.Text = "Copy all settings and roles in any database to the selected login";
            this.toolStripButtonTransferAllSettings.Click += new System.EventHandler(this.toolStripButtonTransferAllSettings_Click);
            // 
            // toolStripButtonTransferDatabaseSettings
            // 
            this.toolStripButtonTransferDatabaseSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTransferDatabaseSettings.Image = global::DiversityWorkbench.Properties.Resources.Import;
            this.toolStripButtonTransferDatabaseSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTransferDatabaseSettings.Name = "toolStripButtonTransferDatabaseSettings";
            this.toolStripButtonTransferDatabaseSettings.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTransferDatabaseSettings.Text = "Copy all settings and roles in the selected database to the selected login";
            this.toolStripButtonTransferDatabaseSettings.Click += new System.EventHandler(this.toolStripButtonTransferDatabaseSettings_Click);
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageListTree;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(342, 646);
            this.treeView.TabIndex = 1;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "DiversityWorkbench.ico");
            this.imageListTree.Images.SetKeyName(1, "Database.ico");
            this.imageListTree.Images.SetKeyName(2, "Group.ico");
            this.imageListTree.Images.SetKeyName(3, "Project.ico");
            this.imageListTree.Images.SetKeyName(4, "ProjectGrey.ico");
            this.imageListTree.Images.SetKeyName(5, "NULL.ico");
            this.imageListTree.Images.SetKeyName(6, "AgentDisplayType.ico");
            // 
            // FormLoginOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 671);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLoginOverview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login overview";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ImageList imageListTree;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTarget;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxTargetLogin;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransferAllSettings;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransferDatabaseSettings;
    }
}