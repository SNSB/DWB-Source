namespace DiversityCollection.DistributionMap
{
    partial class FormDistributionMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDistributionMap));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddSetting = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoadMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGenerateMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoadSettings = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(3, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.panelSettings);
            this.splitContainerMain.Panel2.Controls.Add(this.toolStripMain);
            this.splitContainerMain.Size = new System.Drawing.Size(881, 644);
            this.splitContainerMain.SplitterDistance = 621;
            this.splitContainerMain.TabIndex = 0;
            // 
            // panelSettings
            // 
            this.panelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSettings.Location = new System.Drawing.Point(0, 25);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(256, 619);
            this.panelSettings.TabIndex = 1;
            // 
            // toolStripMain
            // 
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddSetting,
            this.toolStripButtonLoadMap,
            this.toolStripButtonGenerateMap,
            this.toolStripButtonSaveSettings,
            this.toolStripButtonLoadSettings});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(256, 25);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripButtonAddSetting
            // 
            this.toolStripButtonAddSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddSetting.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonAddSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddSetting.Name = "toolStripButtonAddSetting";
            this.toolStripButtonAddSetting.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAddSetting.Text = "Add a new setting";
            this.toolStripButtonAddSetting.Click += new System.EventHandler(this.toolStripButtonAddSetting_Click);
            // 
            // toolStripButtonLoadMap
            // 
            this.toolStripButtonLoadMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLoadMap.Image = global::DiversityCollection.Resource.EventAssign;
            this.toolStripButtonLoadMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoadMap.Name = "toolStripButtonLoadMap";
            this.toolStripButtonLoadMap.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLoadMap.Text = "Load background map";
            // 
            // toolStripButtonGenerateMap
            // 
            this.toolStripButtonGenerateMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGenerateMap.Image = global::DiversityCollection.Resource.DistributionMap;
            this.toolStripButtonGenerateMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGenerateMap.Name = "toolStripButtonGenerateMap";
            this.toolStripButtonGenerateMap.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonGenerateMap.Text = "Generate the distribution map";
            this.toolStripButtonGenerateMap.Click += new System.EventHandler(this.toolStripButtonGenerateMap_Click);
            // 
            // toolStripButtonSaveSettings
            // 
            this.toolStripButtonSaveSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSaveSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveSettings.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonSaveSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSaveSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveSettings.Name = "toolStripButtonSaveSettings";
            this.toolStripButtonSaveSettings.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSaveSettings.Text = "Save the current settings";
            this.toolStripButtonSaveSettings.Click += new System.EventHandler(this.toolStripButtonSaveSettings_Click);
            // 
            // toolStripButtonLoadSettings
            // 
            this.toolStripButtonLoadSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonLoadSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLoadSettings.Image = global::DiversityCollection.Resource.Lupe;
            this.toolStripButtonLoadSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoadSettings.Name = "toolStripButtonLoadSettings";
            this.toolStripButtonLoadSettings.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLoadSettings.Text = "Load settings";
            this.toolStripButtonLoadSettings.Click += new System.EventHandler(this.toolStripButtonLoadSettings_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // FormDistributionMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 644);
            this.Controls.Add(this.splitContainerMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDistributionMap";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Distribution map";
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel2.PerformLayout();
            this.splitContainerMain.ResumeLayout(false);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonGenerateMap;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoadSettings;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddSetting;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveSettings;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoadMap;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}