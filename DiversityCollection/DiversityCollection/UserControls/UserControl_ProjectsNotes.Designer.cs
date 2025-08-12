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
            this.splitContainerOverviewProject = new System.Windows.Forms.SplitContainer();
            this.pictureBoxProject = new System.Windows.Forms.PictureBox();
            this.groupBoxProjects = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelProjects = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxProjectsNoAccess = new System.Windows.Forms.ListBox();
            this.listBoxProjectsReadOnly = new System.Windows.Forms.ListBox();
            this.listBoxProjects = new System.Windows.Forms.ListBox();
            this.toolStripNoAccess = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNoAccessDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripReadOnly = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonReadOnlyDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripProjects = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonProjectNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonProjectNoAccessNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonProjectDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorProject = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonProjectOpen = new System.Windows.Forms.ToolStripButton();
            this.splitContainerOverviewNotesExternal = new System.Windows.Forms.SplitContainer();
            this.groupBoxNotes = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelNotes = new System.Windows.Forms.TableLayoutPanel();
            this.labelInternalNotes = new System.Windows.Forms.Label();
            this.labelOriginalNotes = new System.Windows.Forms.Label();
            this.labelAdditionalNotes = new System.Windows.Forms.Label();
            this.labelProblems = new System.Windows.Forms.Label();
            this.textBoxOriginalNotes = new System.Windows.Forms.TextBox();
            this.textBoxAdditionalNotes = new System.Windows.Forms.TextBox();
            this.textBoxProblems = new System.Windows.Forms.TextBox();
            this.textBoxInternalNotes = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOverviewProject)).BeginInit();
            this.splitContainerOverviewProject.Panel1.SuspendLayout();
            this.splitContainerOverviewProject.Panel2.SuspendLayout();
            this.splitContainerOverviewProject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProject)).BeginInit();
            this.groupBoxProjects.SuspendLayout();
            this.tableLayoutPanelProjects.SuspendLayout();
            this.toolStripNoAccess.SuspendLayout();
            this.toolStripReadOnly.SuspendLayout();
            this.toolStripProjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOverviewNotesExternal)).BeginInit();
            this.splitContainerOverviewNotesExternal.Panel1.SuspendLayout();
            this.splitContainerOverviewNotesExternal.SuspendLayout();
            this.groupBoxNotes.SuspendLayout();
            this.tableLayoutPanelNotes.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // splitContainerOverviewProject
            // 
            this.splitContainerOverviewProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOverviewProject.Location = new System.Drawing.Point(0, 0);
            this.splitContainerOverviewProject.Name = "splitContainerOverviewProject";
            // 
            // splitContainerOverviewProject.Panel1
            // 
            this.splitContainerOverviewProject.Panel1.Controls.Add(this.pictureBoxProject);
            this.splitContainerOverviewProject.Panel1.Controls.Add(this.groupBoxProjects);
            // 
            // splitContainerOverviewProject.Panel2
            // 
            this.splitContainerOverviewProject.Panel2.Controls.Add(this.splitContainerOverviewNotesExternal);
            this.splitContainerOverviewProject.Size = new System.Drawing.Size(638, 256);
            this.splitContainerOverviewProject.SplitterDistance = 194;
            this.splitContainerOverviewProject.TabIndex = 1;
            // 
            // pictureBoxProject
            // 
            this.pictureBoxProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxProject.Image = global::DiversityCollection.Resource.Project1;
            this.pictureBoxProject.Location = new System.Drawing.Point(179, 0);
            this.pictureBoxProject.Name = "pictureBoxProject";
            this.pictureBoxProject.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxProject.TabIndex = 27;
            this.pictureBoxProject.TabStop = false;
            // 
            // groupBoxProjects
            // 
            this.groupBoxProjects.AccessibleName = "CollectionProject";
            this.groupBoxProjects.Controls.Add(this.tableLayoutPanelProjects);
            this.groupBoxProjects.Controls.Add(this.toolStripProjects);
            this.groupBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxProjects.ForeColor = System.Drawing.Color.Red;
            this.groupBoxProjects.Location = new System.Drawing.Point(0, 0);
            this.groupBoxProjects.Name = "groupBoxProjects";
            this.groupBoxProjects.Size = new System.Drawing.Size(194, 256);
            this.groupBoxProjects.TabIndex = 26;
            this.groupBoxProjects.TabStop = false;
            this.groupBoxProjects.Text = "Projects";
            // 
            // tableLayoutPanelProjects
            // 
            this.tableLayoutPanelProjects.ColumnCount = 2;
            this.tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProjects.Controls.Add(this.listBoxProjectsNoAccess, 0, 0);
            this.tableLayoutPanelProjects.Controls.Add(this.listBoxProjectsReadOnly, 0, 1);
            this.tableLayoutPanelProjects.Controls.Add(this.listBoxProjects, 0, 2);
            this.tableLayoutPanelProjects.Controls.Add(this.toolStripNoAccess, 1, 0);
            this.tableLayoutPanelProjects.Controls.Add(this.toolStripReadOnly, 1, 1);
            this.tableLayoutPanelProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProjects.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelProjects.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelProjects.Name = "tableLayoutPanelProjects";
            this.tableLayoutPanelProjects.RowCount = 3;
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProjects.Size = new System.Drawing.Size(188, 214);
            this.tableLayoutPanelProjects.TabIndex = 30;
            // 
            // listBoxProjectsNoAccess
            // 
            this.listBoxProjectsNoAccess.BackColor = System.Drawing.Color.Pink;
            this.listBoxProjectsNoAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectsNoAccess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxProjectsNoAccess.ForeColor = System.Drawing.Color.Red;
            this.listBoxProjectsNoAccess.FormattingEnabled = true;
            this.listBoxProjectsNoAccess.IntegralHeight = false;
            this.listBoxProjectsNoAccess.Location = new System.Drawing.Point(0, 0);
            this.listBoxProjectsNoAccess.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxProjectsNoAccess.Name = "listBoxProjectsNoAccess";
            this.listBoxProjectsNoAccess.Size = new System.Drawing.Size(164, 30);
            this.listBoxProjectsNoAccess.TabIndex = 28;
            // 
            // listBoxProjectsReadOnly
            // 
            this.listBoxProjectsReadOnly.BackColor = System.Drawing.SystemColors.ControlLight;
            this.listBoxProjectsReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectsReadOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxProjectsReadOnly.ForeColor = System.Drawing.Color.DimGray;
            this.listBoxProjectsReadOnly.FormattingEnabled = true;
            this.listBoxProjectsReadOnly.IntegralHeight = false;
            this.listBoxProjectsReadOnly.Location = new System.Drawing.Point(0, 30);
            this.listBoxProjectsReadOnly.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxProjectsReadOnly.Name = "listBoxProjectsReadOnly";
            this.listBoxProjectsReadOnly.Size = new System.Drawing.Size(164, 44);
            this.listBoxProjectsReadOnly.TabIndex = 29;
            // 
            // listBoxProjects
            // 
            this.tableLayoutPanelProjects.SetColumnSpan(this.listBoxProjects, 2);
            this.listBoxProjects.DisplayMember = "Project";
            this.listBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxProjects.IntegralHeight = false;
            this.listBoxProjects.Location = new System.Drawing.Point(0, 74);
            this.listBoxProjects.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxProjects.Name = "listBoxProjects";
            this.listBoxProjects.Size = new System.Drawing.Size(188, 140);
            this.listBoxProjects.TabIndex = 26;
            this.listBoxProjects.ValueMember = "ProjectID";
            // 
            // toolStripNoAccess
            // 
            this.toolStripNoAccess.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripNoAccess.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripNoAccess.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNoAccessDelete});
            this.toolStripNoAccess.Location = new System.Drawing.Point(164, 0);
            this.toolStripNoAccess.Name = "toolStripNoAccess";
            this.toolStripNoAccess.Size = new System.Drawing.Size(24, 30);
            this.toolStripNoAccess.TabIndex = 30;
            this.toolStripNoAccess.Text = "toolStrip1";
            // 
            // toolStripButtonNoAccessDelete
            // 
            this.toolStripButtonNoAccessDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNoAccessDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonNoAccessDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNoAccessDelete.Name = "toolStripButtonNoAccessDelete";
            this.toolStripButtonNoAccessDelete.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonNoAccessDelete.Text = "Remove dataset from not accessible project";
            this.toolStripButtonNoAccessDelete.Click += new System.EventHandler(this.toolStripButtonNoAccessDelete_Click);
            // 
            // toolStripReadOnly
            // 
            this.toolStripReadOnly.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripReadOnly.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripReadOnly.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonReadOnlyDelete});
            this.toolStripReadOnly.Location = new System.Drawing.Point(164, 30);
            this.toolStripReadOnly.Name = "toolStripReadOnly";
            this.toolStripReadOnly.Size = new System.Drawing.Size(24, 44);
            this.toolStripReadOnly.TabIndex = 31;
            this.toolStripReadOnly.Text = "toolStrip1";
            // 
            // toolStripButtonReadOnlyDelete
            // 
            this.toolStripButtonReadOnlyDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReadOnlyDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonReadOnlyDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReadOnlyDelete.Name = "toolStripButtonReadOnlyDelete";
            this.toolStripButtonReadOnlyDelete.Size = new System.Drawing.Size(29, 20);
            this.toolStripButtonReadOnlyDelete.Text = "Remove dataset from ReadOnly project";
            this.toolStripButtonReadOnlyDelete.Click += new System.EventHandler(this.toolStripButtonReadOnlyDelete_Click);
            // 
            // toolStripProjects
            // 
            this.toolStripProjects.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripProjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonProjectNew,
            this.toolStripButtonProjectNoAccessNew,
            this.toolStripButtonProjectDelete,
            this.toolStripSeparatorProject,
            this.toolStripButtonProjectOpen});
            this.toolStripProjects.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripProjects.Location = new System.Drawing.Point(3, 230);
            this.toolStripProjects.Name = "toolStripProjects";
            this.toolStripProjects.Size = new System.Drawing.Size(188, 23);
            this.toolStripProjects.TabIndex = 27;
            this.toolStripProjects.Text = "toolStripProjects";
            // 
            // toolStripButtonProjectNew
            // 
            this.toolStripButtonProjectNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonProjectNew.Image")));
            this.toolStripButtonProjectNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectNew.Name = "toolStripButtonProjectNew";
            this.toolStripButtonProjectNew.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonProjectNew.Text = "Enter a new project for the specimen";
            this.toolStripButtonProjectNew.Click += new System.EventHandler(this.toolStripButtonProjectNew_Click);
            // 
            // toolStripButtonProjectNoAccessNew
            // 
            this.toolStripButtonProjectNoAccessNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectNoAccessNew.Image = global::DiversityCollection.Resource.NewRed;
            this.toolStripButtonProjectNoAccessNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectNoAccessNew.Name = "toolStripButtonProjectNoAccessNew";
            this.toolStripButtonProjectNoAccessNew.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonProjectNoAccessNew.Text = "Add a project where you have no access to";
            this.toolStripButtonProjectNoAccessNew.Click += new System.EventHandler(this.toolStripButtonProjectNoAccessNew_Click);
            // 
            // toolStripButtonProjectDelete
            // 
            this.toolStripButtonProjectDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonProjectDelete.Image")));
            this.toolStripButtonProjectDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectDelete.Name = "toolStripButtonProjectDelete";
            this.toolStripButtonProjectDelete.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonProjectDelete.Text = "Remove specimen from select project";
            this.toolStripButtonProjectDelete.Click += new System.EventHandler(this.toolStripButtonProjectDelete_Click);
            // 
            // toolStripSeparatorProject
            // 
            this.toolStripSeparatorProject.Name = "toolStripSeparatorProject";
            this.toolStripSeparatorProject.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonProjectOpen
            // 
            this.toolStripButtonProjectOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonProjectOpen.Image")));
            this.toolStripButtonProjectOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectOpen.Name = "toolStripButtonProjectOpen";
            this.toolStripButtonProjectOpen.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonProjectOpen.Text = "Open DiversityProjects";
            this.toolStripButtonProjectOpen.Click += new System.EventHandler(this.toolStripButtonProjectOpen_Click);
            // 
            // splitContainerOverviewNotesExternal
            // 
            this.splitContainerOverviewNotesExternal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOverviewNotesExternal.Location = new System.Drawing.Point(0, 0);
            this.splitContainerOverviewNotesExternal.Name = "splitContainerOverviewNotesExternal";
            this.splitContainerOverviewNotesExternal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerOverviewNotesExternal.Panel1
            // 
            this.splitContainerOverviewNotesExternal.Panel1.Controls.Add(this.groupBoxNotes);
            this.splitContainerOverviewNotesExternal.Panel2Collapsed = true;
            this.splitContainerOverviewNotesExternal.Size = new System.Drawing.Size(440, 256);
            this.splitContainerOverviewNotesExternal.SplitterDistance = 25;
            this.splitContainerOverviewNotesExternal.TabIndex = 29;
            // 
            // groupBoxNotes
            // 
            this.groupBoxNotes.AccessibleName = "CollectionSpecimen.Notes";
            this.groupBoxNotes.Controls.Add(this.tableLayoutPanelNotes);
            this.groupBoxNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxNotes.ForeColor = System.Drawing.Color.Black;
            this.groupBoxNotes.Location = new System.Drawing.Point(0, 0);
            this.groupBoxNotes.Name = "groupBoxNotes";
            this.groupBoxNotes.Size = new System.Drawing.Size(440, 256);
            this.groupBoxNotes.TabIndex = 28;
            this.groupBoxNotes.TabStop = false;
            this.groupBoxNotes.Text = "Notes";
            // 
            // tableLayoutPanelNotes
            // 
            this.tableLayoutPanelNotes.ColumnCount = 2;
            this.tableLayoutPanelNotes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelNotes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelNotes.Controls.Add(this.labelInternalNotes, 0, 2);
            this.tableLayoutPanelNotes.Controls.Add(this.labelOriginalNotes, 0, 0);
            this.tableLayoutPanelNotes.Controls.Add(this.labelAdditionalNotes, 0, 1);
            this.tableLayoutPanelNotes.Controls.Add(this.labelProblems, 0, 3);
            this.tableLayoutPanelNotes.Controls.Add(this.textBoxOriginalNotes, 1, 0);
            this.tableLayoutPanelNotes.Controls.Add(this.textBoxAdditionalNotes, 1, 1);
            this.tableLayoutPanelNotes.Controls.Add(this.textBoxProblems, 1, 3);
            this.tableLayoutPanelNotes.Controls.Add(this.textBoxInternalNotes, 1, 2);
            this.tableLayoutPanelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelNotes.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelNotes.Name = "tableLayoutPanelNotes";
            this.tableLayoutPanelNotes.RowCount = 4;
            this.tableLayoutPanelNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanelNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanelNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.tableLayoutPanelNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanelNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelNotes.Size = new System.Drawing.Size(434, 237);
            this.tableLayoutPanelNotes.TabIndex = 0;
            // 
            // labelInternalNotes
            // 
            this.labelInternalNotes.AccessibleName = "CollectionSpecimen.InternalNotes";
            this.labelInternalNotes.AutoSize = true;
            this.labelInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInternalNotes.Location = new System.Drawing.Point(3, 121);
            this.labelInternalNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelInternalNotes.Name = "labelInternalNotes";
            this.labelInternalNotes.Size = new System.Drawing.Size(56, 56);
            this.labelInternalNotes.TabIndex = 6;
            this.labelInternalNotes.Text = "Internal:";
            this.labelInternalNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelOriginalNotes
            // 
            this.labelOriginalNotes.AccessibleName = "CollectionSpecimen.OriginalNotes";
            this.labelOriginalNotes.AutoSize = true;
            this.labelOriginalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOriginalNotes.Location = new System.Drawing.Point(3, 3);
            this.labelOriginalNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelOriginalNotes.Name = "labelOriginalNotes";
            this.labelOriginalNotes.Size = new System.Drawing.Size(56, 56);
            this.labelOriginalNotes.TabIndex = 0;
            this.labelOriginalNotes.Text = "Original:";
            this.labelOriginalNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelAdditionalNotes
            // 
            this.labelAdditionalNotes.AccessibleName = "CollectionSpecimen.AdditionalNotes";
            this.labelAdditionalNotes.AutoSize = true;
            this.labelAdditionalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAdditionalNotes.Location = new System.Drawing.Point(3, 62);
            this.labelAdditionalNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelAdditionalNotes.Name = "labelAdditionalNotes";
            this.labelAdditionalNotes.Size = new System.Drawing.Size(56, 56);
            this.labelAdditionalNotes.TabIndex = 1;
            this.labelAdditionalNotes.Text = "Additional:";
            this.labelAdditionalNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelProblems
            // 
            this.labelProblems.AccessibleName = "CollectionSpecimen.Problems";
            this.labelProblems.AutoSize = true;
            this.labelProblems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProblems.Location = new System.Drawing.Point(3, 180);
            this.labelProblems.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelProblems.Name = "labelProblems";
            this.labelProblems.Size = new System.Drawing.Size(56, 57);
            this.labelProblems.TabIndex = 2;
            this.labelProblems.Text = "Problems:";
            this.labelProblems.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxOriginalNotes
            // 
            this.textBoxOriginalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOriginalNotes.Location = new System.Drawing.Point(59, 0);
            this.textBoxOriginalNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxOriginalNotes.Multiline = true;
            this.textBoxOriginalNotes.Name = "textBoxOriginalNotes";
            this.textBoxOriginalNotes.Size = new System.Drawing.Size(372, 56);
            this.textBoxOriginalNotes.TabIndex = 3;
            // 
            // textBoxAdditionalNotes
            // 
            this.textBoxAdditionalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAdditionalNotes.Location = new System.Drawing.Point(59, 59);
            this.textBoxAdditionalNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxAdditionalNotes.Multiline = true;
            this.textBoxAdditionalNotes.Name = "textBoxAdditionalNotes";
            this.textBoxAdditionalNotes.Size = new System.Drawing.Size(372, 56);
            this.textBoxAdditionalNotes.TabIndex = 4;
            // 
            // textBoxProblems
            // 
            this.textBoxProblems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProblems.Location = new System.Drawing.Point(59, 177);
            this.textBoxProblems.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxProblems.Multiline = true;
            this.textBoxProblems.Name = "textBoxProblems";
            this.textBoxProblems.Size = new System.Drawing.Size(372, 57);
            this.textBoxProblems.TabIndex = 5;
            // 
            // textBoxInternalNotes
            // 
            this.textBoxInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInternalNotes.Location = new System.Drawing.Point(59, 118);
            this.textBoxInternalNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxInternalNotes.Multiline = true;
            this.textBoxInternalNotes.Name = "textBoxInternalNotes";
            this.textBoxInternalNotes.Size = new System.Drawing.Size(372, 56);
            this.textBoxInternalNotes.TabIndex = 7;
            // 
            // UserControl_ProjectsNotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerOverviewProject);
            this.Name = "UserControl_ProjectsNotes";
            this.Size = new System.Drawing.Size(638, 256);
            this.splitContainerOverviewProject.Panel1.ResumeLayout(false);
            this.splitContainerOverviewProject.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOverviewProject)).EndInit();
            this.splitContainerOverviewProject.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProject)).EndInit();
            this.groupBoxProjects.ResumeLayout(false);
            this.groupBoxProjects.PerformLayout();
            this.tableLayoutPanelProjects.ResumeLayout(false);
            this.tableLayoutPanelProjects.PerformLayout();
            this.toolStripNoAccess.ResumeLayout(false);
            this.toolStripNoAccess.PerformLayout();
            this.toolStripReadOnly.ResumeLayout(false);
            this.toolStripReadOnly.PerformLayout();
            this.toolStripProjects.ResumeLayout(false);
            this.toolStripProjects.PerformLayout();
            this.splitContainerOverviewNotesExternal.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOverviewNotesExternal)).EndInit();
            this.splitContainerOverviewNotesExternal.ResumeLayout(false);
            this.groupBoxNotes.ResumeLayout(false);
            this.tableLayoutPanelNotes.ResumeLayout(false);
            this.tableLayoutPanelNotes.PerformLayout();
            this.ResumeLayout(false);

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
