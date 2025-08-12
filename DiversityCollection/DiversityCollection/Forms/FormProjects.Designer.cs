namespace DiversityCollection.Forms
{
    partial class FormProjects
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProjects));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelList = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripProjects = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNewProject = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveProject = new System.Windows.Forms.ToolStripButton();
            this.listBoxProjects = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelData = new System.Windows.Forms.TableLayoutPanel();
            this.labelProject = new System.Windows.Forms.Label();
            this.labelSpecimen = new System.Windows.Forms.Label();
            this.textBoxSpecimen = new System.Windows.Forms.TextBox();
            this.labelImageDescriptionTemplate = new System.Windows.Forms.Label();
            this.treeViewImageDescriptionTemplate = new System.Windows.Forms.TreeView();
            this.buttonEditImageDescriptionTemplate = new System.Windows.Forms.Button();
            this.pictureBoxProject = new System.Windows.Forms.PictureBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.imageListProject = new System.Windows.Forms.ImageList(this.components);
            this.buttonSetServerConnection = new System.Windows.Forms.Button();
            this.labelSetServerConnection = new System.Windows.Forms.Label();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelList.SuspendLayout();
            this.toolStripProjects.SuspendLayout();
            this.tableLayoutPanelData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProject)).BeginInit();
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
            this.splitContainerMain.Panel1.Controls.Add(this.tableLayoutPanelList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelData);
            this.splitContainerMain.Size = new System.Drawing.Size(442, 260);
            this.splitContainerMain.SplitterDistance = 146;
            this.splitContainerMain.TabIndex = 1;
            // 
            // tableLayoutPanelList
            // 
            this.tableLayoutPanelList.ColumnCount = 1;
            this.tableLayoutPanelList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelList.Controls.Add(this.toolStripProjects, 0, 1);
            this.tableLayoutPanelList.Controls.Add(this.listBoxProjects, 0, 0);
            this.tableLayoutPanelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelList.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelList.Name = "tableLayoutPanelList";
            this.tableLayoutPanelList.RowCount = 2;
            this.tableLayoutPanelList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelList.Size = new System.Drawing.Size(146, 260);
            this.tableLayoutPanelList.TabIndex = 0;
            // 
            // toolStripProjects
            // 
            this.toolStripProjects.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripProjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewProject,
            this.toolStripButtonRemoveProject});
            this.toolStripProjects.Location = new System.Drawing.Point(0, 235);
            this.toolStripProjects.Name = "toolStripProjects";
            this.toolStripProjects.Size = new System.Drawing.Size(146, 25);
            this.toolStripProjects.TabIndex = 0;
            this.toolStripProjects.Text = "toolStrip1";
            // 
            // toolStripButtonNewProject
            // 
            this.toolStripButtonNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewProject.Image = global::DiversityCollection.Resource.New;
            this.toolStripButtonNewProject.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewProject.Name = "toolStripButtonNewProject";
            this.toolStripButtonNewProject.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNewProject.Text = "Create a new local project";
            this.toolStripButtonNewProject.Click += new System.EventHandler(this.toolStripButtonNewProject_Click);
            // 
            // toolStripButtonRemoveProject
            // 
            this.toolStripButtonRemoveProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveProject.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonRemoveProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveProject.Name = "toolStripButtonRemoveProject";
            this.toolStripButtonRemoveProject.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemoveProject.Text = "Delete the selected project";
            this.toolStripButtonRemoveProject.Click += new System.EventHandler(this.toolStripButtonRemoveProject_Click);
            // 
            // listBoxProjects
            // 
            this.listBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjects.FormattingEnabled = true;
            this.listBoxProjects.IntegralHeight = false;
            this.listBoxProjects.Location = new System.Drawing.Point(3, 3);
            this.listBoxProjects.Name = "listBoxProjects";
            this.listBoxProjects.Size = new System.Drawing.Size(140, 229);
            this.listBoxProjects.TabIndex = 1;
            this.listBoxProjects.SelectedIndexChanged += new System.EventHandler(this.listBoxProjects_SelectedIndexChanged);
            // 
            // tableLayoutPanelData
            // 
            this.tableLayoutPanelData.ColumnCount = 3;
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelData.Controls.Add(this.labelProject, 0, 1);
            this.tableLayoutPanelData.Controls.Add(this.labelSpecimen, 0, 2);
            this.tableLayoutPanelData.Controls.Add(this.textBoxSpecimen, 1, 2);
            this.tableLayoutPanelData.Controls.Add(this.labelImageDescriptionTemplate, 0, 3);
            this.tableLayoutPanelData.Controls.Add(this.treeViewImageDescriptionTemplate, 0, 4);
            this.tableLayoutPanelData.Controls.Add(this.buttonEditImageDescriptionTemplate, 2, 3);
            this.tableLayoutPanelData.Controls.Add(this.pictureBoxProject, 2, 1);
            this.tableLayoutPanelData.Controls.Add(this.buttonSetServerConnection, 2, 0);
            this.tableLayoutPanelData.Controls.Add(this.labelSetServerConnection, 0, 0);
            this.tableLayoutPanelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelData.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelData.Name = "tableLayoutPanelData";
            this.tableLayoutPanelData.RowCount = 5;
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelData.Size = new System.Drawing.Size(292, 260);
            this.tableLayoutPanelData.TabIndex = 0;
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.tableLayoutPanelData.SetColumnSpan(this.labelProject, 2);
            this.labelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProject.Location = new System.Drawing.Point(3, 30);
            this.labelProject.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(262, 13);
            this.labelProject.TabIndex = 0;
            this.labelProject.Text = "Project";
            // 
            // labelSpecimen
            // 
            this.labelSpecimen.AutoSize = true;
            this.labelSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSpecimen.Location = new System.Drawing.Point(3, 43);
            this.labelSpecimen.Name = "labelSpecimen";
            this.labelSpecimen.Size = new System.Drawing.Size(57, 26);
            this.labelSpecimen.TabIndex = 1;
            this.labelSpecimen.Text = "Specimen:";
            this.labelSpecimen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSpecimen
            // 
            this.tableLayoutPanelData.SetColumnSpan(this.textBoxSpecimen, 2);
            this.textBoxSpecimen.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxSpecimen.Location = new System.Drawing.Point(66, 46);
            this.textBoxSpecimen.Name = "textBoxSpecimen";
            this.textBoxSpecimen.Size = new System.Drawing.Size(60, 20);
            this.textBoxSpecimen.TabIndex = 2;
            // 
            // labelImageDescriptionTemplate
            // 
            this.labelImageDescriptionTemplate.AutoSize = true;
            this.tableLayoutPanelData.SetColumnSpan(this.labelImageDescriptionTemplate, 2);
            this.labelImageDescriptionTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImageDescriptionTemplate.Location = new System.Drawing.Point(3, 69);
            this.labelImageDescriptionTemplate.Name = "labelImageDescriptionTemplate";
            this.labelImageDescriptionTemplate.Size = new System.Drawing.Size(262, 24);
            this.labelImageDescriptionTemplate.TabIndex = 3;
            this.labelImageDescriptionTemplate.Text = "Image description template:";
            this.labelImageDescriptionTemplate.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // treeViewImageDescriptionTemplate
            // 
            this.tableLayoutPanelData.SetColumnSpan(this.treeViewImageDescriptionTemplate, 3);
            this.treeViewImageDescriptionTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewImageDescriptionTemplate.Location = new System.Drawing.Point(3, 96);
            this.treeViewImageDescriptionTemplate.Name = "treeViewImageDescriptionTemplate";
            this.treeViewImageDescriptionTemplate.Size = new System.Drawing.Size(286, 161);
            this.treeViewImageDescriptionTemplate.TabIndex = 4;
            // 
            // buttonEditImageDescriptionTemplate
            // 
            this.buttonEditImageDescriptionTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEditImageDescriptionTemplate.Image = global::DiversityCollection.Resource.Edit1;
            this.buttonEditImageDescriptionTemplate.Location = new System.Drawing.Point(268, 69);
            this.buttonEditImageDescriptionTemplate.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditImageDescriptionTemplate.Name = "buttonEditImageDescriptionTemplate";
            this.buttonEditImageDescriptionTemplate.Size = new System.Drawing.Size(24, 24);
            this.buttonEditImageDescriptionTemplate.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonEditImageDescriptionTemplate, "Create or edit the image description template for the selected project");
            this.buttonEditImageDescriptionTemplate.UseVisualStyleBackColor = true;
            this.buttonEditImageDescriptionTemplate.Click += new System.EventHandler(this.buttonEditImageDescriptionTemplate_Click);
            // 
            // pictureBoxProject
            // 
            this.pictureBoxProject.Image = global::DiversityCollection.Resource.Project1;
            this.pictureBoxProject.Location = new System.Drawing.Point(268, 27);
            this.pictureBoxProject.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pictureBoxProject.Name = "pictureBoxProject";
            this.pictureBoxProject.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxProject.TabIndex = 6;
            this.pictureBoxProject.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProject, "Global project");
            // 
            // imageListProject
            // 
            this.imageListProject.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListProject.ImageStream")));
            this.imageListProject.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListProject.Images.SetKeyName(0, "Project.ico");
            this.imageListProject.Images.SetKeyName(1, "ProjectOpen.ico");
            // 
            // buttonSetServerConnection
            // 
            this.buttonSetServerConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetServerConnection.Image = global::DiversityCollection.Resource.Database;
            this.buttonSetServerConnection.Location = new System.Drawing.Point(268, 0);
            this.buttonSetServerConnection.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetServerConnection.Name = "buttonSetServerConnection";
            this.buttonSetServerConnection.Size = new System.Drawing.Size(24, 24);
            this.buttonSetServerConnection.TabIndex = 7;
            this.buttonSetServerConnection.UseVisualStyleBackColor = true;
            this.buttonSetServerConnection.Click += new System.EventHandler(this.buttonSetServerConnection_Click);
            // 
            // labelSetServerConnection
            // 
            this.labelSetServerConnection.AutoSize = true;
            this.tableLayoutPanelData.SetColumnSpan(this.labelSetServerConnection, 2);
            this.labelSetServerConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSetServerConnection.Location = new System.Drawing.Point(3, 0);
            this.labelSetServerConnection.Name = "labelSetServerConnection";
            this.labelSetServerConnection.Size = new System.Drawing.Size(262, 24);
            this.labelSetServerConnection.TabIndex = 8;
            this.labelSetServerConnection.Text = "Set the connection to the DiversityProjects database";
            this.labelSetServerConnection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormProjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 260);
            this.Controls.Add(this.splitContainerMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormProjects";
            this.Text = "Projects";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelList.ResumeLayout(false);
            this.tableLayoutPanelList.PerformLayout();
            this.toolStripProjects.ResumeLayout(false);
            this.toolStripProjects.PerformLayout();
            this.tableLayoutPanelData.ResumeLayout(false);
            this.tableLayoutPanelData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelData;
        private System.Windows.Forms.ToolStrip toolStripProjects;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewProject;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveProject;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.Label labelSpecimen;
        private System.Windows.Forms.TextBox textBoxSpecimen;
        private System.Windows.Forms.Label labelImageDescriptionTemplate;
        private System.Windows.Forms.TreeView treeViewImageDescriptionTemplate;
        private System.Windows.Forms.Button buttonEditImageDescriptionTemplate;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pictureBoxProject;
        private System.Windows.Forms.ImageList imageListProject;
        private System.Windows.Forms.Button buttonSetServerConnection;
        private System.Windows.Forms.Label labelSetServerConnection;
    }
}