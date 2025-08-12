namespace DiversityWorkbench.Forms
{
    partial class FormDatabaseRoles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseRoles));
            this.labelHeader = new System.Windows.Forms.Label();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.groupBoxPermissions = new System.Windows.Forms.GroupBox();
            this.dataGridViewPermissions = new System.Windows.Forms.DataGridView();
            this.toolStripPermissions = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPermissionsAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPermissionsRemove = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelRoleInRole = new System.Windows.Forms.TableLayoutPanel();
            this.labelContainedRoles = new System.Windows.Forms.Label();
            this.labelContainedInRoles = new System.Windows.Forms.Label();
            this.listBoxContainedInRoles = new System.Windows.Forms.ListBox();
            this.listBoxContainedRoles = new System.Windows.Forms.ListBox();
            this.toolStripContainedRoles = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonContainedRolesAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonContainedRolesRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripInRoles = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonInRolesAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonInRolesRemove = new System.Windows.Forms.ToolStripButton();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.labelSql = new System.Windows.Forms.Label();
            this.textBoxSQL = new System.Windows.Forms.TextBox();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.groupBoxPermissions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermissions)).BeginInit();
            this.toolStripPermissions.SuspendLayout();
            this.tableLayoutPanelRoleInRole.SuspendLayout();
            this.toolStripContainedRoles.SuspendLayout();
            this.toolStripInRoles.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHeader.Location = new System.Drawing.Point(0, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(667, 20);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "label1";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 20);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.groupBoxPermissions);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelRoleInRole);
            this.splitContainerMain.Size = new System.Drawing.Size(667, 482);
            this.splitContainerMain.SplitterDistance = 312;
            this.splitContainerMain.TabIndex = 1;
            // 
            // groupBoxPermissions
            // 
            this.groupBoxPermissions.Controls.Add(this.dataGridViewPermissions);
            this.groupBoxPermissions.Controls.Add(this.toolStripPermissions);
            this.groupBoxPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPermissions.Location = new System.Drawing.Point(0, 0);
            this.groupBoxPermissions.Name = "groupBoxPermissions";
            this.groupBoxPermissions.Size = new System.Drawing.Size(667, 312);
            this.groupBoxPermissions.TabIndex = 2;
            this.groupBoxPermissions.TabStop = false;
            this.groupBoxPermissions.Text = "Permissions";
            // 
            // dataGridViewPermissions
            // 
            this.dataGridViewPermissions.AllowUserToAddRows = false;
            this.dataGridViewPermissions.AllowUserToDeleteRows = false;
            this.dataGridViewPermissions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPermissions.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewPermissions.Name = "dataGridViewPermissions";
            this.dataGridViewPermissions.ReadOnly = true;
            this.dataGridViewPermissions.RowHeadersVisible = false;
            this.dataGridViewPermissions.Size = new System.Drawing.Size(637, 293);
            this.dataGridViewPermissions.TabIndex = 1;
            // 
            // toolStripPermissions
            // 
            this.toolStripPermissions.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripPermissions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripPermissions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPermissionsAdd,
            this.toolStripButtonPermissionsRemove});
            this.toolStripPermissions.Location = new System.Drawing.Point(640, 16);
            this.toolStripPermissions.Name = "toolStripPermissions";
            this.toolStripPermissions.Size = new System.Drawing.Size(24, 293);
            this.toolStripPermissions.TabIndex = 2;
            this.toolStripPermissions.Text = "toolStrip1";
            // 
            // toolStripButtonPermissionsAdd
            // 
            this.toolStripButtonPermissionsAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPermissionsAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonPermissionsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPermissionsAdd.Name = "toolStripButtonPermissionsAdd";
            this.toolStripButtonPermissionsAdd.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonPermissionsAdd.Text = "Add a new permission";
            this.toolStripButtonPermissionsAdd.Click += new System.EventHandler(this.toolStripButtonPermissionsAdd_Click);
            // 
            // toolStripButtonPermissionsRemove
            // 
            this.toolStripButtonPermissionsRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPermissionsRemove.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonPermissionsRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPermissionsRemove.Name = "toolStripButtonPermissionsRemove";
            this.toolStripButtonPermissionsRemove.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonPermissionsRemove.Text = "Remove the selected permission";
            this.toolStripButtonPermissionsRemove.Click += new System.EventHandler(this.toolStripButtonPermissionsRemove_Click);
            // 
            // tableLayoutPanelRoleInRole
            // 
            this.tableLayoutPanelRoleInRole.ColumnCount = 2;
            this.tableLayoutPanelRoleInRole.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoleInRole.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoleInRole.Controls.Add(this.labelContainedRoles, 0, 0);
            this.tableLayoutPanelRoleInRole.Controls.Add(this.labelContainedInRoles, 1, 0);
            this.tableLayoutPanelRoleInRole.Controls.Add(this.listBoxContainedInRoles, 1, 1);
            this.tableLayoutPanelRoleInRole.Controls.Add(this.listBoxContainedRoles, 0, 1);
            this.tableLayoutPanelRoleInRole.Controls.Add(this.toolStripContainedRoles, 0, 2);
            this.tableLayoutPanelRoleInRole.Controls.Add(this.toolStripInRoles, 1, 2);
            this.tableLayoutPanelRoleInRole.Controls.Add(this.labelSql, 0, 3);
            this.tableLayoutPanelRoleInRole.Controls.Add(this.textBoxSQL, 0, 4);
            this.tableLayoutPanelRoleInRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRoleInRole.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRoleInRole.Name = "tableLayoutPanelRoleInRole";
            this.tableLayoutPanelRoleInRole.RowCount = 5;
            this.tableLayoutPanelRoleInRole.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRoleInRole.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRoleInRole.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRoleInRole.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRoleInRole.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRoleInRole.Size = new System.Drawing.Size(667, 166);
            this.tableLayoutPanelRoleInRole.TabIndex = 0;
            // 
            // labelContainedRoles
            // 
            this.labelContainedRoles.AutoSize = true;
            this.labelContainedRoles.Location = new System.Drawing.Point(3, 0);
            this.labelContainedRoles.Name = "labelContainedRoles";
            this.labelContainedRoles.Size = new System.Drawing.Size(45, 13);
            this.labelContainedRoles.TabIndex = 0;
            this.labelContainedRoles.Text = "Roles in";
            // 
            // labelContainedInRoles
            // 
            this.labelContainedInRoles.AutoSize = true;
            this.labelContainedInRoles.Location = new System.Drawing.Point(336, 0);
            this.labelContainedInRoles.Name = "labelContainedInRoles";
            this.labelContainedInRoles.Size = new System.Drawing.Size(91, 13);
            this.labelContainedInRoles.TabIndex = 1;
            this.labelContainedInRoles.Text = "Contained in roles";
            // 
            // listBoxContainedInRoles
            // 
            this.listBoxContainedInRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxContainedInRoles.FormattingEnabled = true;
            this.listBoxContainedInRoles.Location = new System.Drawing.Point(336, 16);
            this.listBoxContainedInRoles.Name = "listBoxContainedInRoles";
            this.listBoxContainedInRoles.Size = new System.Drawing.Size(328, 73);
            this.listBoxContainedInRoles.TabIndex = 2;
            // 
            // listBoxContainedRoles
            // 
            this.listBoxContainedRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxContainedRoles.FormattingEnabled = true;
            this.listBoxContainedRoles.Location = new System.Drawing.Point(3, 16);
            this.listBoxContainedRoles.Name = "listBoxContainedRoles";
            this.listBoxContainedRoles.Size = new System.Drawing.Size(327, 73);
            this.listBoxContainedRoles.TabIndex = 3;
            // 
            // toolStripContainedRoles
            // 
            this.toolStripContainedRoles.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripContainedRoles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonContainedRolesAdd,
            this.toolStripButtonContainedRolesRemove});
            this.toolStripContainedRoles.Location = new System.Drawing.Point(3, 92);
            this.toolStripContainedRoles.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.toolStripContainedRoles.Name = "toolStripContainedRoles";
            this.toolStripContainedRoles.Size = new System.Drawing.Size(327, 25);
            this.toolStripContainedRoles.TabIndex = 4;
            this.toolStripContainedRoles.Text = "toolStrip1";
            // 
            // toolStripButtonContainedRolesAdd
            // 
            this.toolStripButtonContainedRolesAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonContainedRolesAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonContainedRolesAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonContainedRolesAdd.Name = "toolStripButtonContainedRolesAdd";
            this.toolStripButtonContainedRolesAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonContainedRolesAdd.Text = "Add a new role";
            this.toolStripButtonContainedRolesAdd.Click += new System.EventHandler(this.toolStripButtonContainedRolesAdd_Click);
            // 
            // toolStripButtonContainedRolesRemove
            // 
            this.toolStripButtonContainedRolesRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonContainedRolesRemove.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonContainedRolesRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonContainedRolesRemove.Name = "toolStripButtonContainedRolesRemove";
            this.toolStripButtonContainedRolesRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonContainedRolesRemove.Text = "Remove the selected role";
            this.toolStripButtonContainedRolesRemove.Click += new System.EventHandler(this.toolStripButtonContainedRolesRemove_Click);
            // 
            // toolStripInRoles
            // 
            this.toolStripInRoles.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripInRoles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonInRolesAdd,
            this.toolStripButtonInRolesRemove});
            this.toolStripInRoles.Location = new System.Drawing.Point(336, 92);
            this.toolStripInRoles.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.toolStripInRoles.Name = "toolStripInRoles";
            this.toolStripInRoles.Size = new System.Drawing.Size(328, 25);
            this.toolStripInRoles.TabIndex = 5;
            this.toolStripInRoles.Text = "toolStrip1";
            // 
            // toolStripButtonInRolesAdd
            // 
            this.toolStripButtonInRolesAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonInRolesAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonInRolesAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInRolesAdd.Name = "toolStripButtonInRolesAdd";
            this.toolStripButtonInRolesAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonInRolesAdd.Text = "Add a new role";
            this.toolStripButtonInRolesAdd.Click += new System.EventHandler(this.toolStripButtonInRolesAdd_Click);
            // 
            // toolStripButtonInRolesRemove
            // 
            this.toolStripButtonInRolesRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonInRolesRemove.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonInRolesRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInRolesRemove.Name = "toolStripButtonInRolesRemove";
            this.toolStripButtonInRolesRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonInRolesRemove.Text = "Remove selected role";
            this.toolStripButtonInRolesRemove.Click += new System.EventHandler(this.toolStripButtonInRolesRemove_Click);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(640, 1);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(27, 23);
            this.buttonFeedback.TabIndex = 3;
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // labelSql
            // 
            this.labelSql.AutoSize = true;
            this.tableLayoutPanelRoleInRole.SetColumnSpan(this.labelSql, 2);
            this.labelSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSql.ForeColor = System.Drawing.Color.Red;
            this.labelSql.Location = new System.Drawing.Point(3, 117);
            this.labelSql.Name = "labelSql";
            this.labelSql.Size = new System.Drawing.Size(661, 13);
            this.labelSql.TabIndex = 6;
            this.labelSql.Text = "SQL code to be included in a update script:";
            this.labelSql.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.labelSql.Visible = false;
            // 
            // textBoxSQL
            // 
            this.tableLayoutPanelRoleInRole.SetColumnSpan(this.textBoxSQL, 2);
            this.textBoxSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSQL.ForeColor = System.Drawing.Color.Red;
            this.textBoxSQL.Location = new System.Drawing.Point(3, 133);
            this.textBoxSQL.Multiline = true;
            this.textBoxSQL.Name = "textBoxSQL";
            this.textBoxSQL.ReadOnly = true;
            this.textBoxSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSQL.Size = new System.Drawing.Size(661, 30);
            this.textBoxSQL.TabIndex = 7;
            this.textBoxSQL.Visible = false;
            // 
            // FormDatabaseRoles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 502);
            this.Controls.Add(this.buttonFeedback);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.labelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDatabaseRoles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database role";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.groupBoxPermissions.ResumeLayout(false);
            this.groupBoxPermissions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermissions)).EndInit();
            this.toolStripPermissions.ResumeLayout(false);
            this.toolStripPermissions.PerformLayout();
            this.tableLayoutPanelRoleInRole.ResumeLayout(false);
            this.tableLayoutPanelRoleInRole.PerformLayout();
            this.toolStripContainedRoles.ResumeLayout(false);
            this.toolStripContainedRoles.PerformLayout();
            this.toolStripInRoles.ResumeLayout(false);
            this.toolStripInRoles.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.DataGridView dataGridViewPermissions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRoleInRole;
        private System.Windows.Forms.Label labelContainedRoles;
        private System.Windows.Forms.Label labelContainedInRoles;
        private System.Windows.Forms.ListBox listBoxContainedInRoles;
        private System.Windows.Forms.ListBox listBoxContainedRoles;
        private System.Windows.Forms.GroupBox groupBoxPermissions;
        private System.Windows.Forms.ToolStrip toolStripPermissions;
        private System.Windows.Forms.ToolStripButton toolStripButtonPermissionsAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonPermissionsRemove;
        private System.Windows.Forms.ToolStrip toolStripContainedRoles;
        private System.Windows.Forms.ToolStripButton toolStripButtonContainedRolesAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonContainedRolesRemove;
        private System.Windows.Forms.ToolStrip toolStripInRoles;
        private System.Windows.Forms.ToolStripButton toolStripButtonInRolesAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonInRolesRemove;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Label labelSql;
        private System.Windows.Forms.TextBox textBoxSQL;
    }
}