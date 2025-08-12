namespace DiversityWorkbench.Forms
{
    partial class FormEnumProjects
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEnumProjects));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.listBoxProjects = new System.Windows.Forms.ListBox();
            this.labelProjects = new System.Windows.Forms.Label();
            this.listBoxEnum = new System.Windows.Forms.ListBox();
            this.labelEnum = new System.Windows.Forms.Label();
            this.toolStripEnum = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.toolStripEnum.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.listBoxProjects);
            this.splitContainerMain.Panel1.Controls.Add(this.labelProjects);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.listBoxEnum);
            this.splitContainerMain.Panel2.Controls.Add(this.labelEnum);
            this.splitContainerMain.Panel2.Controls.Add(this.toolStripEnum);
            this.splitContainerMain.Size = new System.Drawing.Size(365, 249);
            this.splitContainerMain.SplitterDistance = 170;
            this.splitContainerMain.TabIndex = 0;
            // 
            // listBoxProjects
            // 
            this.listBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjects.FormattingEnabled = true;
            this.listBoxProjects.IntegralHeight = false;
            this.listBoxProjects.Location = new System.Drawing.Point(0, 23);
            this.listBoxProjects.Name = "listBoxProjects";
            this.listBoxProjects.Size = new System.Drawing.Size(170, 226);
            this.listBoxProjects.TabIndex = 0;
            this.toolTip.SetToolTip(this.listBoxProjects, "List of projects");
            this.listBoxProjects.SelectedIndexChanged += new System.EventHandler(this.listBoxProjects_SelectedIndexChanged);
            // 
            // labelProjects
            // 
            this.labelProjects.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelProjects.Location = new System.Drawing.Point(0, 0);
            this.labelProjects.Margin = new System.Windows.Forms.Padding(3);
            this.labelProjects.Name = "labelProjects";
            this.labelProjects.Size = new System.Drawing.Size(170, 23);
            this.labelProjects.TabIndex = 1;
            this.labelProjects.Text = "Projects";
            this.labelProjects.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listBoxEnum
            // 
            this.listBoxEnum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxEnum.FormattingEnabled = true;
            this.listBoxEnum.IntegralHeight = false;
            this.listBoxEnum.Location = new System.Drawing.Point(0, 23);
            this.listBoxEnum.Name = "listBoxEnum";
            this.listBoxEnum.Size = new System.Drawing.Size(191, 201);
            this.listBoxEnum.TabIndex = 0;
            this.toolTip.SetToolTip(this.listBoxEnum, "List of items on which the selected project is restricted. If the list is empty a" +
        "ll items are available within the selected project");
            // 
            // labelEnum
            // 
            this.labelEnum.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEnum.Location = new System.Drawing.Point(0, 0);
            this.labelEnum.Margin = new System.Windows.Forms.Padding(3);
            this.labelEnum.Name = "labelEnum";
            this.labelEnum.Size = new System.Drawing.Size(191, 23);
            this.labelEnum.TabIndex = 2;
            this.labelEnum.Text = "label1";
            this.labelEnum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripEnum
            // 
            this.toolStripEnum.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripEnum.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripEnum.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonDelete});
            this.toolStripEnum.Location = new System.Drawing.Point(0, 224);
            this.toolStripEnum.Name = "toolStripEnum";
            this.toolStripEnum.Size = new System.Drawing.Size(191, 25);
            this.toolStripEnum.TabIndex = 1;
            this.toolStripEnum.Text = "toolStrip1";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAdd.Text = "Add an item";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDelete.Text = "Delete the selected item";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // FormEnumProjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 249);
            this.Controls.Add(this.splitContainerMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEnumProjects";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Projects - set selection for ";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.toolStripEnum.ResumeLayout(false);
            this.toolStripEnum.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.ListBox listBoxEnum;
        private System.Windows.Forms.ToolStrip toolStripEnum;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.Label labelProjects;
        private System.Windows.Forms.Label labelEnum;
        private System.Windows.Forms.ToolTip toolTip;
    }
}