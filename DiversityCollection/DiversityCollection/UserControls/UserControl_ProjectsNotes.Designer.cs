namespace DiversityCollection.UserControls
{
    partial class UserControl_ProjectsNotes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_ProjectsNotes));
            splitContainerOverviewProject = new System.Windows.Forms.SplitContainer();
            groupBoxProjects = new System.Windows.Forms.GroupBox();
            pictureBoxProject = new System.Windows.Forms.PictureBox();
            tableLayoutPanelProjects = new System.Windows.Forms.TableLayoutPanel();
            listBoxProjectsNoAccess = new System.Windows.Forms.ListBox();
            listBoxProjectsReadOnly = new System.Windows.Forms.ListBox();
            listBoxProjects = new System.Windows.Forms.ListBox();
            toolStripNoAccess = new System.Windows.Forms.ToolStrip();
            toolStripButtonNoAccessDelete = new System.Windows.Forms.ToolStripButton();
            toolStripReadOnly = new System.Windows.Forms.ToolStrip();
            toolStripButtonReadOnlyDelete = new System.Windows.Forms.ToolStripButton();
            toolStripProjects = new System.Windows.Forms.ToolStrip();
            toolStripButtonProjectNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonProjectNoAccessNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonProjectDelete = new System.Windows.Forms.ToolStripButton();
            toolStripSeparatorProject = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonProjectOpen = new System.Windows.Forms.ToolStripButton();
            splitContainerOverviewNotesExternal = new System.Windows.Forms.SplitContainer();
            groupBoxNotes = new System.Windows.Forms.GroupBox();
            tableLayoutPanelNotes = new System.Windows.Forms.TableLayoutPanel();
            labelInternalNotes = new System.Windows.Forms.Label();
            labelOriginalNotes = new System.Windows.Forms.Label();
            labelAdditionalNotes = new System.Windows.Forms.Label();
            labelProblems = new System.Windows.Forms.Label();
            textBoxOriginalNotes = new System.Windows.Forms.TextBox();
            textBoxAdditionalNotes = new System.Windows.Forms.TextBox();
            textBoxProblems = new System.Windows.Forms.TextBox();
            textBoxInternalNotes = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainerOverviewProject).BeginInit();
            splitContainerOverviewProject.Panel1.SuspendLayout();
            splitContainerOverviewProject.Panel2.SuspendLayout();
            splitContainerOverviewProject.SuspendLayout();
            groupBoxProjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxProject).BeginInit();
            tableLayoutPanelProjects.SuspendLayout();
            toolStripNoAccess.SuspendLayout();
            toolStripReadOnly.SuspendLayout();
            toolStripProjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerOverviewNotesExternal).BeginInit();
            splitContainerOverviewNotesExternal.Panel1.SuspendLayout();
            splitContainerOverviewNotesExternal.SuspendLayout();
            groupBoxNotes.SuspendLayout();
            tableLayoutPanelNotes.SuspendLayout();
            SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            imageListDataWithholding.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListDataWithholding.ImageStream");
            imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // splitContainerOverviewProject
            // 
            splitContainerOverviewProject.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerOverviewProject.Location = new System.Drawing.Point(0, 0);
            splitContainerOverviewProject.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerOverviewProject.Name = "splitContainerOverviewProject";
            // 
            // splitContainerOverviewProject.Panel1
            // 
            splitContainerOverviewProject.Panel1.Controls.Add(groupBoxProjects);
            // 
            // splitContainerOverviewProject.Panel2
            // 
            splitContainerOverviewProject.Panel2.Controls.Add(splitContainerOverviewNotesExternal);
            splitContainerOverviewProject.Size = new System.Drawing.Size(744, 295);
            splitContainerOverviewProject.SplitterDistance = 226;
            splitContainerOverviewProject.SplitterWidth = 5;
            splitContainerOverviewProject.TabIndex = 1;
            // 
            // groupBoxProjects
            // 
            groupBoxProjects.AccessibleName = "CollectionProject";
            groupBoxProjects.Controls.Add(pictureBoxProject);
            groupBoxProjects.Controls.Add(tableLayoutPanelProjects);
            groupBoxProjects.Controls.Add(toolStripProjects);
            groupBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxProjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            groupBoxProjects.ForeColor = System.Drawing.Color.Red;
            groupBoxProjects.Location = new System.Drawing.Point(0, 0);
            groupBoxProjects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxProjects.Name = "groupBoxProjects";
            groupBoxProjects.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxProjects.Size = new System.Drawing.Size(226, 295);
            groupBoxProjects.TabIndex = 26;
            groupBoxProjects.TabStop = false;
            groupBoxProjects.Text = "Projects";
            // 
            // pictureBoxProject
            // 
            pictureBoxProject.Image = Resource.Project1;
            pictureBoxProject.Location = new System.Drawing.Point(211, 0);
            pictureBoxProject.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxProject.Name = "pictureBoxProject";
            pictureBoxProject.Size = new System.Drawing.Size(16, 16);
            pictureBoxProject.TabIndex = 27;
            pictureBoxProject.TabStop = false;
            pictureBoxProject.Visible = false;
            // 
            // tableLayoutPanelProjects
            // 
            tableLayoutPanelProjects.ColumnCount = 2;
            tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProjects.Controls.Add(listBoxProjectsNoAccess, 0, 0);
            tableLayoutPanelProjects.Controls.Add(listBoxProjectsReadOnly, 0, 1);
            tableLayoutPanelProjects.Controls.Add(listBoxProjects, 0, 2);
            tableLayoutPanelProjects.Controls.Add(toolStripNoAccess, 1, 0);
            tableLayoutPanelProjects.Controls.Add(toolStripReadOnly, 1, 1);
            tableLayoutPanelProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelProjects.Location = new System.Drawing.Point(4, 16);
            tableLayoutPanelProjects.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanelProjects.Name = "tableLayoutPanelProjects";
            tableLayoutPanelProjects.RowCount = 3;
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelProjects.Size = new System.Drawing.Size(218, 253);
            tableLayoutPanelProjects.TabIndex = 30;
            // 
            // listBoxProjectsNoAccess
            // 
            listBoxProjectsNoAccess.BackColor = System.Drawing.Color.Pink;
            listBoxProjectsNoAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectsNoAccess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            listBoxProjectsNoAccess.ForeColor = System.Drawing.Color.Red;
            listBoxProjectsNoAccess.FormattingEnabled = true;
            listBoxProjectsNoAccess.IntegralHeight = false;
            listBoxProjectsNoAccess.ItemHeight = 13;
            listBoxProjectsNoAccess.Location = new System.Drawing.Point(0, 0);
            listBoxProjectsNoAccess.Margin = new System.Windows.Forms.Padding(0);
            listBoxProjectsNoAccess.Name = "listBoxProjectsNoAccess";
            listBoxProjectsNoAccess.Size = new System.Drawing.Size(194, 34);
            listBoxProjectsNoAccess.TabIndex = 28;
            // 
            // listBoxProjectsReadOnly
            // 
            listBoxProjectsReadOnly.BackColor = System.Drawing.SystemColors.ControlLight;
            listBoxProjectsReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectsReadOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            listBoxProjectsReadOnly.ForeColor = System.Drawing.Color.DimGray;
            listBoxProjectsReadOnly.FormattingEnabled = true;
            listBoxProjectsReadOnly.IntegralHeight = false;
            listBoxProjectsReadOnly.ItemHeight = 13;
            listBoxProjectsReadOnly.Location = new System.Drawing.Point(0, 34);
            listBoxProjectsReadOnly.Margin = new System.Windows.Forms.Padding(0);
            listBoxProjectsReadOnly.Name = "listBoxProjectsReadOnly";
            listBoxProjectsReadOnly.Size = new System.Drawing.Size(194, 50);
            listBoxProjectsReadOnly.TabIndex = 29;
            // 
            // listBoxProjects
            // 
            tableLayoutPanelProjects.SetColumnSpan(listBoxProjects, 2);
            listBoxProjects.DisplayMember = "Project";
            listBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            listBoxProjects.IntegralHeight = false;
            listBoxProjects.ItemHeight = 13;
            listBoxProjects.Location = new System.Drawing.Point(0, 84);
            listBoxProjects.Margin = new System.Windows.Forms.Padding(0);
            listBoxProjects.Name = "listBoxProjects";
            listBoxProjects.Size = new System.Drawing.Size(218, 169);
            listBoxProjects.TabIndex = 26;
            listBoxProjects.ValueMember = "ProjectID";
            // 
            // toolStripNoAccess
            // 
            toolStripNoAccess.Dock = System.Windows.Forms.DockStyle.Right;
            toolStripNoAccess.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripNoAccess.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonNoAccessDelete });
            toolStripNoAccess.Location = new System.Drawing.Point(194, 0);
            toolStripNoAccess.Name = "toolStripNoAccess";
            toolStripNoAccess.Size = new System.Drawing.Size(24, 34);
            toolStripNoAccess.TabIndex = 30;
            toolStripNoAccess.Text = "toolStrip1";
            // 
            // toolStripButtonNoAccessDelete
            // 
            toolStripButtonNoAccessDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonNoAccessDelete.Image = Resource.Delete;
            toolStripButtonNoAccessDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonNoAccessDelete.Name = "toolStripButtonNoAccessDelete";
            toolStripButtonNoAccessDelete.Size = new System.Drawing.Size(21, 20);
            toolStripButtonNoAccessDelete.Text = "Remove dataset from not accessible project";
            toolStripButtonNoAccessDelete.Click += toolStripButtonNoAccessDelete_Click;
            // 
            // toolStripReadOnly
            // 
            toolStripReadOnly.Dock = System.Windows.Forms.DockStyle.Right;
            toolStripReadOnly.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripReadOnly.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonReadOnlyDelete });
            toolStripReadOnly.Location = new System.Drawing.Point(194, 34);
            toolStripReadOnly.Name = "toolStripReadOnly";
            toolStripReadOnly.Size = new System.Drawing.Size(24, 50);
            toolStripReadOnly.TabIndex = 31;
            toolStripReadOnly.Text = "toolStrip1";
            // 
            // toolStripButtonReadOnlyDelete
            // 
            toolStripButtonReadOnlyDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReadOnlyDelete.Image = Resource.Delete;
            toolStripButtonReadOnlyDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReadOnlyDelete.Name = "toolStripButtonReadOnlyDelete";
            toolStripButtonReadOnlyDelete.Size = new System.Drawing.Size(21, 20);
            toolStripButtonReadOnlyDelete.Text = "Remove dataset from ReadOnly project";
            toolStripButtonReadOnlyDelete.Click += toolStripButtonReadOnlyDelete_Click;
            // 
            // toolStripProjects
            // 
            toolStripProjects.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripProjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonProjectNew, toolStripButtonProjectNoAccessNew, toolStripButtonProjectDelete, toolStripSeparatorProject, toolStripButtonProjectOpen });
            toolStripProjects.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStripProjects.Location = new System.Drawing.Point(4, 269);
            toolStripProjects.Name = "toolStripProjects";
            toolStripProjects.Size = new System.Drawing.Size(218, 23);
            toolStripProjects.TabIndex = 27;
            toolStripProjects.Text = "toolStripProjects";
            // 
            // toolStripButtonProjectNew
            // 
            toolStripButtonProjectNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectNew.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonProjectNew.Image");
            toolStripButtonProjectNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonProjectNew.Name = "toolStripButtonProjectNew";
            toolStripButtonProjectNew.Size = new System.Drawing.Size(23, 20);
            toolStripButtonProjectNew.Text = "Enter a new project for the specimen";
            toolStripButtonProjectNew.Click += toolStripButtonProjectNew_Click;
            // 
            // toolStripButtonProjectNoAccessNew
            // 
            toolStripButtonProjectNoAccessNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectNoAccessNew.Image = Resource.NewRed;
            toolStripButtonProjectNoAccessNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonProjectNoAccessNew.Name = "toolStripButtonProjectNoAccessNew";
            toolStripButtonProjectNoAccessNew.Size = new System.Drawing.Size(23, 20);
            toolStripButtonProjectNoAccessNew.Text = "Add a project where you have no access to";
            toolStripButtonProjectNoAccessNew.Click += toolStripButtonProjectNoAccessNew_Click;
            // 
            // toolStripButtonProjectDelete
            // 
            toolStripButtonProjectDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectDelete.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonProjectDelete.Image");
            toolStripButtonProjectDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonProjectDelete.Name = "toolStripButtonProjectDelete";
            toolStripButtonProjectDelete.Size = new System.Drawing.Size(23, 20);
            toolStripButtonProjectDelete.Text = "Remove specimen from select project";
            toolStripButtonProjectDelete.Click += toolStripButtonProjectDelete_Click;
            // 
            // toolStripSeparatorProject
            // 
            toolStripSeparatorProject.Name = "toolStripSeparatorProject";
            toolStripSeparatorProject.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonProjectOpen
            // 
            toolStripButtonProjectOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectOpen.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonProjectOpen.Image");
            toolStripButtonProjectOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonProjectOpen.Name = "toolStripButtonProjectOpen";
            toolStripButtonProjectOpen.Size = new System.Drawing.Size(23, 20);
            toolStripButtonProjectOpen.Text = "Open DiversityProjects";
            toolStripButtonProjectOpen.Click += toolStripButtonProjectOpen_Click;
            // 
            // splitContainerOverviewNotesExternal
            // 
            splitContainerOverviewNotesExternal.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerOverviewNotesExternal.Location = new System.Drawing.Point(0, 0);
            splitContainerOverviewNotesExternal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerOverviewNotesExternal.Name = "splitContainerOverviewNotesExternal";
            splitContainerOverviewNotesExternal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerOverviewNotesExternal.Panel1
            // 
            splitContainerOverviewNotesExternal.Panel1.Controls.Add(groupBoxNotes);
            splitContainerOverviewNotesExternal.Panel2Collapsed = true;
            splitContainerOverviewNotesExternal.Size = new System.Drawing.Size(513, 295);
            splitContainerOverviewNotesExternal.SplitterDistance = 29;
            splitContainerOverviewNotesExternal.SplitterWidth = 5;
            splitContainerOverviewNotesExternal.TabIndex = 29;
            // 
            // groupBoxNotes
            // 
            groupBoxNotes.AccessibleName = "CollectionSpecimen.Notes";
            groupBoxNotes.Controls.Add(tableLayoutPanelNotes);
            groupBoxNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            groupBoxNotes.ForeColor = System.Drawing.Color.Black;
            groupBoxNotes.Location = new System.Drawing.Point(0, 0);
            groupBoxNotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxNotes.Name = "groupBoxNotes";
            groupBoxNotes.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxNotes.Size = new System.Drawing.Size(513, 295);
            groupBoxNotes.TabIndex = 28;
            groupBoxNotes.TabStop = false;
            groupBoxNotes.Text = "Notes";
            // 
            // tableLayoutPanelNotes
            // 
            tableLayoutPanelNotes.ColumnCount = 2;
            tableLayoutPanelNotes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelNotes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelNotes.Controls.Add(labelInternalNotes, 0, 2);
            tableLayoutPanelNotes.Controls.Add(labelOriginalNotes, 0, 0);
            tableLayoutPanelNotes.Controls.Add(labelAdditionalNotes, 0, 1);
            tableLayoutPanelNotes.Controls.Add(labelProblems, 0, 3);
            tableLayoutPanelNotes.Controls.Add(textBoxOriginalNotes, 1, 0);
            tableLayoutPanelNotes.Controls.Add(textBoxAdditionalNotes, 1, 1);
            tableLayoutPanelNotes.Controls.Add(textBoxProblems, 1, 3);
            tableLayoutPanelNotes.Controls.Add(textBoxInternalNotes, 1, 2);
            tableLayoutPanelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tableLayoutPanelNotes.Location = new System.Drawing.Point(4, 16);
            tableLayoutPanelNotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelNotes.Name = "tableLayoutPanelNotes";
            tableLayoutPanelNotes.RowCount = 4;
            tableLayoutPanelNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            tableLayoutPanelNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            tableLayoutPanelNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            tableLayoutPanelNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            tableLayoutPanelNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelNotes.Size = new System.Drawing.Size(505, 276);
            tableLayoutPanelNotes.TabIndex = 0;
            // 
            // labelInternalNotes
            // 
            labelInternalNotes.AccessibleName = "CollectionSpecimen.InternalNotes";
            labelInternalNotes.AutoSize = true;
            labelInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInternalNotes.Location = new System.Drawing.Point(4, 141);
            labelInternalNotes.Margin = new System.Windows.Forms.Padding(4, 3, 0, 0);
            labelInternalNotes.Name = "labelInternalNotes";
            labelInternalNotes.Size = new System.Drawing.Size(56, 65);
            labelInternalNotes.TabIndex = 6;
            labelInternalNotes.Text = "Internal:";
            labelInternalNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelOriginalNotes
            // 
            labelOriginalNotes.AccessibleName = "CollectionSpecimen.OriginalNotes";
            labelOriginalNotes.AutoSize = true;
            labelOriginalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelOriginalNotes.Location = new System.Drawing.Point(4, 3);
            labelOriginalNotes.Margin = new System.Windows.Forms.Padding(4, 3, 0, 0);
            labelOriginalNotes.Name = "labelOriginalNotes";
            labelOriginalNotes.Size = new System.Drawing.Size(56, 66);
            labelOriginalNotes.TabIndex = 0;
            labelOriginalNotes.Text = "Original:";
            labelOriginalNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelAdditionalNotes
            // 
            labelAdditionalNotes.AccessibleName = "CollectionSpecimen.AdditionalNotes";
            labelAdditionalNotes.AutoSize = true;
            labelAdditionalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelAdditionalNotes.Location = new System.Drawing.Point(4, 72);
            labelAdditionalNotes.Margin = new System.Windows.Forms.Padding(4, 3, 0, 0);
            labelAdditionalNotes.Name = "labelAdditionalNotes";
            labelAdditionalNotes.Size = new System.Drawing.Size(56, 66);
            labelAdditionalNotes.TabIndex = 1;
            labelAdditionalNotes.Text = "Additional:";
            labelAdditionalNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelProblems
            // 
            labelProblems.AccessibleName = "CollectionSpecimen.Problems";
            labelProblems.AutoSize = true;
            labelProblems.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProblems.Location = new System.Drawing.Point(4, 209);
            labelProblems.Margin = new System.Windows.Forms.Padding(4, 3, 0, 0);
            labelProblems.Name = "labelProblems";
            labelProblems.Size = new System.Drawing.Size(56, 67);
            labelProblems.TabIndex = 2;
            labelProblems.Text = "Problems:";
            labelProblems.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxOriginalNotes
            // 
            textBoxOriginalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxOriginalNotes.Location = new System.Drawing.Point(60, 0);
            textBoxOriginalNotes.Margin = new System.Windows.Forms.Padding(0, 0, 4, 3);
            textBoxOriginalNotes.Multiline = true;
            textBoxOriginalNotes.Name = "textBoxOriginalNotes";
            textBoxOriginalNotes.Size = new System.Drawing.Size(441, 66);
            textBoxOriginalNotes.TabIndex = 3;
            // 
            // textBoxAdditionalNotes
            // 
            textBoxAdditionalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxAdditionalNotes.Location = new System.Drawing.Point(60, 69);
            textBoxAdditionalNotes.Margin = new System.Windows.Forms.Padding(0, 0, 4, 3);
            textBoxAdditionalNotes.Multiline = true;
            textBoxAdditionalNotes.Name = "textBoxAdditionalNotes";
            textBoxAdditionalNotes.Size = new System.Drawing.Size(441, 66);
            textBoxAdditionalNotes.TabIndex = 4;
            // 
            // textBoxProblems
            // 
            textBoxProblems.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProblems.Location = new System.Drawing.Point(60, 206);
            textBoxProblems.Margin = new System.Windows.Forms.Padding(0, 0, 4, 3);
            textBoxProblems.Multiline = true;
            textBoxProblems.Name = "textBoxProblems";
            textBoxProblems.Size = new System.Drawing.Size(441, 67);
            textBoxProblems.TabIndex = 5;
            // 
            // textBoxInternalNotes
            // 
            textBoxInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxInternalNotes.Location = new System.Drawing.Point(60, 138);
            textBoxInternalNotes.Margin = new System.Windows.Forms.Padding(0, 0, 4, 3);
            textBoxInternalNotes.Multiline = true;
            textBoxInternalNotes.Name = "textBoxInternalNotes";
            textBoxInternalNotes.Size = new System.Drawing.Size(441, 65);
            textBoxInternalNotes.TabIndex = 7;
            // 
            // UserControl_ProjectsNotes
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainerOverviewProject);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "UserControl_ProjectsNotes";
            Size = new System.Drawing.Size(744, 295);
            splitContainerOverviewProject.Panel1.ResumeLayout(false);
            splitContainerOverviewProject.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerOverviewProject).EndInit();
            splitContainerOverviewProject.ResumeLayout(false);
            groupBoxProjects.ResumeLayout(false);
            groupBoxProjects.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxProject).EndInit();
            tableLayoutPanelProjects.ResumeLayout(false);
            tableLayoutPanelProjects.PerformLayout();
            toolStripNoAccess.ResumeLayout(false);
            toolStripNoAccess.PerformLayout();
            toolStripReadOnly.ResumeLayout(false);
            toolStripReadOnly.PerformLayout();
            toolStripProjects.ResumeLayout(false);
            toolStripProjects.PerformLayout();
            splitContainerOverviewNotesExternal.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerOverviewNotesExternal).EndInit();
            splitContainerOverviewNotesExternal.ResumeLayout(false);
            groupBoxNotes.ResumeLayout(false);
            tableLayoutPanelNotes.ResumeLayout(false);
            tableLayoutPanelNotes.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerOverviewProject;
        private System.Windows.Forms.GroupBox groupBoxProjects;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.ListBox listBoxProjectsReadOnly;
        private System.Windows.Forms.ListBox listBoxProjectsNoAccess;
        private System.Windows.Forms.ToolStrip toolStripProjects;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectNoAccessNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorProject;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectOpen;
        private System.Windows.Forms.SplitContainer splitContainerOverviewNotesExternal;
        private System.Windows.Forms.GroupBox groupBoxNotes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNotes;
        private System.Windows.Forms.Label labelInternalNotes;
        private System.Windows.Forms.Label labelOriginalNotes;
        private System.Windows.Forms.Label labelAdditionalNotes;
        private System.Windows.Forms.Label labelProblems;
        private System.Windows.Forms.TextBox textBoxOriginalNotes;
        private System.Windows.Forms.TextBox textBoxAdditionalNotes;
        private System.Windows.Forms.TextBox textBoxProblems;
        private System.Windows.Forms.TextBox textBoxInternalNotes;
        private System.Windows.Forms.PictureBox pictureBoxProject;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProjects;
        private System.Windows.Forms.ToolStrip toolStripNoAccess;
        private System.Windows.Forms.ToolStripButton toolStripButtonNoAccessDelete;
        private System.Windows.Forms.ToolStrip toolStripReadOnly;
        private System.Windows.Forms.ToolStripButton toolStripButtonReadOnlyDelete;
    }
}
