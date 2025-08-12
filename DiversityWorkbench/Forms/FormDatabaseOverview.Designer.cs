namespace DiversityWorkbench.Forms
{
    partial class FormDatabaseOverview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseOverview));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.tabControlProperties = new System.Windows.Forms.TabControl();
            this.tabPageUser = new System.Windows.Forms.TabPage();
            this.listBoxUser = new System.Windows.Forms.ListBox();
            this.toolStripUser = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDeleteUser = new System.Windows.Forms.ToolStripButton();
            this.tabPageRoles = new System.Windows.Forms.TabPage();
            this.listBoxRoles = new System.Windows.Forms.ListBox();
            this.tabPageProjects = new System.Windows.Forms.TabPage();
            this.splitContainerProjects = new System.Windows.Forms.SplitContainer();
            this.listBoxProjects = new System.Windows.Forms.ListBox();
            this.listBoxProjectUser = new System.Windows.Forms.ListBox();
            this.labelProjectUser = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            this.tabControlProperties.SuspendLayout();
            this.tabPageUser.SuspendLayout();
            this.toolStripUser.SuspendLayout();
            this.tabPageRoles.SuspendLayout();
            this.tabPageProjects.SuspendLayout();
            this.splitContainerProjects.Panel1.SuspendLayout();
            this.splitContainerProjects.Panel2.SuspendLayout();
            this.splitContainerProjects.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tabControlProperties, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(258, 322);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(53, 22);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Database";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControlProperties
            // 
            this.tabControlProperties.Controls.Add(this.tabPageUser);
            this.tabControlProperties.Controls.Add(this.tabPageRoles);
            this.tabControlProperties.Controls.Add(this.tabPageProjects);
            this.tabControlProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlProperties.ImageList = this.imageList;
            this.tabControlProperties.Location = new System.Drawing.Point(3, 25);
            this.tabControlProperties.Name = "tabControlProperties";
            this.tabControlProperties.SelectedIndex = 0;
            this.tabControlProperties.Size = new System.Drawing.Size(252, 294);
            this.tabControlProperties.TabIndex = 1;
            // 
            // tabPageUser
            // 
            this.tabPageUser.Controls.Add(this.listBoxUser);
            this.tabPageUser.Controls.Add(this.toolStripUser);
            this.tabPageUser.ImageIndex = 0;
            this.tabPageUser.Location = new System.Drawing.Point(4, 23);
            this.tabPageUser.Name = "tabPageUser";
            this.tabPageUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUser.Size = new System.Drawing.Size(244, 267);
            this.tabPageUser.TabIndex = 0;
            this.tabPageUser.Text = "User";
            this.tabPageUser.UseVisualStyleBackColor = true;
            // 
            // listBoxUser
            // 
            this.listBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUser.FormattingEnabled = true;
            this.listBoxUser.Location = new System.Drawing.Point(3, 3);
            this.listBoxUser.Name = "listBoxUser";
            this.listBoxUser.Size = new System.Drawing.Size(238, 236);
            this.listBoxUser.TabIndex = 1;
            // 
            // toolStripUser
            // 
            this.toolStripUser.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripUser.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDeleteUser});
            this.toolStripUser.Location = new System.Drawing.Point(3, 239);
            this.toolStripUser.Name = "toolStripUser";
            this.toolStripUser.Size = new System.Drawing.Size(238, 25);
            this.toolStripUser.TabIndex = 0;
            this.toolStripUser.Text = "toolStrip1";
            // 
            // toolStripButtonDeleteUser
            // 
            this.toolStripButtonDeleteUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDeleteUser.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonDeleteUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteUser.Name = "toolStripButtonDeleteUser";
            this.toolStripButtonDeleteUser.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDeleteUser.Text = "Remove the selected user from the database";
            this.toolStripButtonDeleteUser.Click += new System.EventHandler(this.toolStripButtonDeleteUser_Click);
            // 
            // tabPageRoles
            // 
            this.tabPageRoles.Controls.Add(this.listBoxRoles);
            this.tabPageRoles.ImageIndex = 1;
            this.tabPageRoles.Location = new System.Drawing.Point(4, 23);
            this.tabPageRoles.Name = "tabPageRoles";
            this.tabPageRoles.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRoles.Size = new System.Drawing.Size(244, 267);
            this.tabPageRoles.TabIndex = 1;
            this.tabPageRoles.Text = "Roles";
            this.tabPageRoles.UseVisualStyleBackColor = true;
            // 
            // listBoxRoles
            // 
            this.listBoxRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxRoles.FormattingEnabled = true;
            this.listBoxRoles.Location = new System.Drawing.Point(3, 3);
            this.listBoxRoles.Name = "listBoxRoles";
            this.listBoxRoles.Size = new System.Drawing.Size(238, 261);
            this.listBoxRoles.TabIndex = 0;
            // 
            // tabPageProjects
            // 
            this.tabPageProjects.Controls.Add(this.splitContainerProjects);
            this.tabPageProjects.ImageIndex = 2;
            this.tabPageProjects.Location = new System.Drawing.Point(4, 23);
            this.tabPageProjects.Name = "tabPageProjects";
            this.tabPageProjects.Size = new System.Drawing.Size(244, 267);
            this.tabPageProjects.TabIndex = 2;
            this.tabPageProjects.Text = "Projects";
            this.tabPageProjects.UseVisualStyleBackColor = true;
            // 
            // splitContainerProjects
            // 
            this.splitContainerProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerProjects.Location = new System.Drawing.Point(0, 0);
            this.splitContainerProjects.Name = "splitContainerProjects";
            // 
            // splitContainerProjects.Panel1
            // 
            this.splitContainerProjects.Panel1.Controls.Add(this.listBoxProjects);
            // 
            // splitContainerProjects.Panel2
            // 
            this.splitContainerProjects.Panel2.Controls.Add(this.listBoxProjectUser);
            this.splitContainerProjects.Panel2.Controls.Add(this.labelProjectUser);
            this.splitContainerProjects.Size = new System.Drawing.Size(244, 267);
            this.splitContainerProjects.SplitterDistance = 81;
            this.splitContainerProjects.TabIndex = 0;
            // 
            // listBoxProjects
            // 
            this.listBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjects.FormattingEnabled = true;
            this.listBoxProjects.Location = new System.Drawing.Point(0, 0);
            this.listBoxProjects.Name = "listBoxProjects";
            this.listBoxProjects.Size = new System.Drawing.Size(81, 267);
            this.listBoxProjects.TabIndex = 0;
            this.listBoxProjects.SelectedIndexChanged += new System.EventHandler(this.listBoxProjects_SelectedIndexChanged);
            // 
            // listBoxProjectUser
            // 
            this.listBoxProjectUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectUser.FormattingEnabled = true;
            this.listBoxProjectUser.Location = new System.Drawing.Point(0, 13);
            this.listBoxProjectUser.Name = "listBoxProjectUser";
            this.listBoxProjectUser.Size = new System.Drawing.Size(159, 254);
            this.listBoxProjectUser.TabIndex = 0;
            // 
            // labelProjectUser
            // 
            this.labelProjectUser.AutoSize = true;
            this.labelProjectUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelProjectUser.Location = new System.Drawing.Point(0, 0);
            this.labelProjectUser.Name = "labelProjectUser";
            this.labelProjectUser.Size = new System.Drawing.Size(75, 13);
            this.labelProjectUser.TabIndex = 1;
            this.labelProjectUser.Text = "User in project";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Agent.ico");
            this.imageList.Images.SetKeyName(1, "Group.ico");
            this.imageList.Images.SetKeyName(2, "Project.ico");
            // 
            // FormDatabaseOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 322);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDatabaseOverview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database overview";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.tabControlProperties.ResumeLayout(false);
            this.tabPageUser.ResumeLayout(false);
            this.tabPageUser.PerformLayout();
            this.toolStripUser.ResumeLayout(false);
            this.toolStripUser.PerformLayout();
            this.tabPageRoles.ResumeLayout(false);
            this.tabPageProjects.ResumeLayout(false);
            this.splitContainerProjects.Panel1.ResumeLayout(false);
            this.splitContainerProjects.Panel2.ResumeLayout(false);
            this.splitContainerProjects.Panel2.PerformLayout();
            this.splitContainerProjects.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.TabControl tabControlProperties;
        private System.Windows.Forms.TabPage tabPageUser;
        private System.Windows.Forms.TabPage tabPageRoles;
        private System.Windows.Forms.TabPage tabPageProjects;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStrip toolStripUser;
        private System.Windows.Forms.ListBox listBoxUser;
        private System.Windows.Forms.ListBox listBoxRoles;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteUser;
        private System.Windows.Forms.SplitContainer splitContainerProjects;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.ListBox listBoxProjectUser;
        private System.Windows.Forms.Label labelProjectUser;
    }
}