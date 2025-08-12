namespace DiversityWorkbench.UserControls
{
    partial class UserControlProjects
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
            components = new System.ComponentModel.Container();
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            listBoxProjects = new System.Windows.Forms.ListBox();
            fKProjectProjectBindingSource = new System.Windows.Forms.BindingSource(components);
            projectBindingSource = new System.Windows.Forms.BindingSource(components);
            splitContainerData = new System.Windows.Forms.SplitContainer();
            treeViewProjects = new System.Windows.Forms.TreeView();
            toolStripHierarchy = new System.Windows.Forms.ToolStrip();
            toolStripButtonProjectNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonProjectDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonProjectSetParent = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            labelHeaderProject = new System.Windows.Forms.Label();
            textBoxHeaderProject = new System.Windows.Forms.TextBox();
            labelHeaderProjectID = new System.Windows.Forms.Label();
            textBoxHeaderProjectID = new System.Windows.Forms.TextBox();
            buttonHistory = new System.Windows.Forms.Button();
            splitContainerSettings = new System.Windows.Forms.SplitContainer();
            tableLayoutPanelProject = new System.Windows.Forms.TableLayoutPanel();
            labelURL = new System.Windows.Forms.Label();
            labelInstitution = new System.Windows.Forms.Label();
            labelProject = new System.Windows.Forms.Label();
            textBoxProject = new System.Windows.Forms.TextBox();
            labelProjectTitle = new System.Windows.Forms.Label();
            textBoxProjectTitle = new System.Windows.Forms.TextBox();
            labelProjectDescription = new System.Windows.Forms.Label();
            textBoxProjectDescription = new System.Windows.Forms.TextBox();
            labelProjectEditors = new System.Windows.Forms.Label();
            textBoxProjectEditors = new System.Windows.Forms.TextBox();
            labelProjectNotes = new System.Windows.Forms.Label();
            textBoxProjectNotes = new System.Windows.Forms.TextBox();
            labelProjectCopyright = new System.Windows.Forms.Label();
            textBoxProjectCopyright = new System.Windows.Forms.TextBox();
            labelProjectVersion = new System.Windows.Forms.Label();
            textBoxProjectVersion = new System.Windows.Forms.TextBox();
            textBoxInstitution = new System.Windows.Forms.TextBox();
            buttonURL = new System.Windows.Forms.Button();
            textBoxURL = new System.Windows.Forms.TextBox();
            tableLayoutPanelSettings = new System.Windows.Forms.TableLayoutPanel();
            labelSettings = new System.Windows.Forms.Label();
            treeViewSettings = new System.Windows.Forms.TreeView();
            buttonSettingsAdd = new System.Windows.Forms.Button();
            textBoxSettings = new System.Windows.Forms.TextBox();
            labelSettingValue = new System.Windows.Forms.Label();
            buttonSettingsRemove = new System.Windows.Forms.Button();
            buttonSettingsCopyFromParent = new System.Windows.Forms.Button();
            textBoxSetting = new System.Windows.Forms.TextBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fKProjectProjectBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)projectBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerData).BeginInit();
            splitContainerData.Panel1.SuspendLayout();
            splitContainerData.Panel2.SuspendLayout();
            splitContainerData.SuspendLayout();
            toolStripHierarchy.SuspendLayout();
            tableLayoutPanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerSettings).BeginInit();
            splitContainerSettings.Panel1.SuspendLayout();
            splitContainerSettings.Panel2.SuspendLayout();
            splitContainerSettings.SuspendLayout();
            tableLayoutPanelProject.SuspendLayout();
            tableLayoutPanelSettings.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(0, 0);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(listBoxProjects);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(splitContainerData);
            splitContainerMain.Size = new System.Drawing.Size(1160, 897);
            splitContainerMain.SplitterDistance = 386;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 0;
            // 
            // listBoxProjects
            // 
            listBoxProjects.DataSource = fKProjectProjectBindingSource;
            listBoxProjects.DisplayMember = "Project";
            listBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjects.FormattingEnabled = true;
            listBoxProjects.IntegralHeight = false;
            listBoxProjects.Location = new System.Drawing.Point(0, 0);
            listBoxProjects.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            listBoxProjects.Name = "listBoxProjects";
            listBoxProjects.Size = new System.Drawing.Size(386, 897);
            listBoxProjects.TabIndex = 0;
            listBoxProjects.ValueMember = "ProjectID";
            // 
            // fKProjectProjectBindingSource
            // 
            fKProjectProjectBindingSource.DataMember = "FK_Project_Project";
            fKProjectProjectBindingSource.DataSource = projectBindingSource;
            // 
            // projectBindingSource
            // 
            projectBindingSource.DataMember = "Project";
            // 
            // splitContainerData
            // 
            splitContainerData.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerData.Location = new System.Drawing.Point(0, 0);
            splitContainerData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            splitContainerData.Name = "splitContainerData";
            splitContainerData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerData.Panel1
            // 
            splitContainerData.Panel1.Controls.Add(treeViewProjects);
            splitContainerData.Panel1.Controls.Add(toolStripHierarchy);
            splitContainerData.Panel1.Controls.Add(tableLayoutPanelHeader);
            // 
            // splitContainerData.Panel2
            // 
            splitContainerData.Panel2.Controls.Add(splitContainerSettings);
            splitContainerData.Size = new System.Drawing.Size(769, 897);
            splitContainerData.SplitterDistance = 149;
            splitContainerData.SplitterWidth = 6;
            splitContainerData.TabIndex = 1;
            // 
            // treeViewProjects
            // 
            treeViewProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewProjects.Location = new System.Drawing.Point(0, 38);
            treeViewProjects.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            treeViewProjects.Name = "treeViewProjects";
            treeViewProjects.Size = new System.Drawing.Size(739, 111);
            treeViewProjects.TabIndex = 0;
            // 
            // toolStripHierarchy
            // 
            toolStripHierarchy.Dock = System.Windows.Forms.DockStyle.Right;
            toolStripHierarchy.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStripHierarchy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonProjectNew, toolStripButtonProjectDelete, toolStripButtonProjectSetParent });
            toolStripHierarchy.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            toolStripHierarchy.Location = new System.Drawing.Point(739, 38);
            toolStripHierarchy.Name = "toolStripHierarchy";
            toolStripHierarchy.Size = new System.Drawing.Size(30, 111);
            toolStripHierarchy.TabIndex = 1;
            toolStripHierarchy.Text = "toolStrip1";
            // 
            // toolStripButtonProjectNew
            // 
            toolStripButtonProjectNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectNew.Image = ResourceWorkbench.New;
            toolStripButtonProjectNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonProjectNew.Name = "toolStripButtonProjectNew";
            toolStripButtonProjectNew.Size = new System.Drawing.Size(29, 24);
            toolStripButtonProjectNew.Text = "Add a project as child of the selected project";
            // 
            // toolStripButtonProjectDelete
            // 
            toolStripButtonProjectDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectDelete.Image = ResourceWorkbench.Delete;
            toolStripButtonProjectDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonProjectDelete.Name = "toolStripButtonProjectDelete";
            toolStripButtonProjectDelete.Size = new System.Drawing.Size(29, 24);
            toolStripButtonProjectDelete.Text = "Delete the selected project";
            // 
            // toolStripButtonProjectSetParent
            // 
            toolStripButtonProjectSetParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectSetParent.Image = ResourceWorkbench.SetParent;
            toolStripButtonProjectSetParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonProjectSetParent.Name = "toolStripButtonProjectSetParent";
            toolStripButtonProjectSetParent.Size = new System.Drawing.Size(29, 24);
            toolStripButtonProjectSetParent.Text = "Set the parent project of the current project";
            // 
            // tableLayoutPanelHeader
            // 
            tableLayoutPanelHeader.ColumnCount = 5;
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.Controls.Add(labelHeaderProject, 0, 0);
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderProject, 1, 0);
            tableLayoutPanelHeader.Controls.Add(labelHeaderProjectID, 2, 0);
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderProjectID, 3, 0);
            tableLayoutPanelHeader.Controls.Add(buttonHistory, 4, 0);
            tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            tableLayoutPanelHeader.RowCount = 1;
            tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelHeader.Size = new System.Drawing.Size(769, 38);
            tableLayoutPanelHeader.TabIndex = 2;
            // 
            // labelHeaderProject
            // 
            labelHeaderProject.AutoSize = true;
            labelHeaderProject.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeaderProject.Location = new System.Drawing.Point(4, 0);
            labelHeaderProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelHeaderProject.Name = "labelHeaderProject";
            labelHeaderProject.Size = new System.Drawing.Size(58, 38);
            labelHeaderProject.TabIndex = 0;
            labelHeaderProject.Text = "Project:";
            labelHeaderProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxHeaderProject
            // 
            textBoxHeaderProject.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxHeaderProject.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "Project", true));
            textBoxHeaderProject.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxHeaderProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            textBoxHeaderProject.Location = new System.Drawing.Point(70, 9);
            textBoxHeaderProject.Margin = new System.Windows.Forms.Padding(4, 9, 4, 5);
            textBoxHeaderProject.Name = "textBoxHeaderProject";
            textBoxHeaderProject.ReadOnly = true;
            textBoxHeaderProject.Size = new System.Drawing.Size(562, 16);
            textBoxHeaderProject.TabIndex = 1;
            textBoxHeaderProject.Text = "GBIF";
            textBoxHeaderProject.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelHeaderProjectID
            // 
            labelHeaderProjectID.AutoSize = true;
            labelHeaderProjectID.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeaderProjectID.Location = new System.Drawing.Point(640, 0);
            labelHeaderProjectID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelHeaderProjectID.Name = "labelHeaderProjectID";
            labelHeaderProjectID.Size = new System.Drawing.Size(27, 38);
            labelHeaderProjectID.TabIndex = 2;
            labelHeaderProjectID.Text = "ID:";
            labelHeaderProjectID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxHeaderProjectID
            // 
            textBoxHeaderProjectID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxHeaderProjectID.DataBindings.Add(new System.Windows.Forms.Binding("Text", projectBindingSource, "ProjectID", true));
            textBoxHeaderProjectID.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxHeaderProjectID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            textBoxHeaderProjectID.Location = new System.Drawing.Point(675, 9);
            textBoxHeaderProjectID.Margin = new System.Windows.Forms.Padding(4, 9, 4, 5);
            textBoxHeaderProjectID.Name = "textBoxHeaderProjectID";
            textBoxHeaderProjectID.ReadOnly = true;
            textBoxHeaderProjectID.Size = new System.Drawing.Size(53, 16);
            textBoxHeaderProjectID.TabIndex = 3;
            textBoxHeaderProjectID.Text = "555";
            // 
            // buttonHistory
            // 
            buttonHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonHistory.Image = ResourceWorkbench.History;
            buttonHistory.Location = new System.Drawing.Point(736, 3);
            buttonHistory.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            buttonHistory.Name = "buttonHistory";
            buttonHistory.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            buttonHistory.Size = new System.Drawing.Size(33, 32);
            buttonHistory.TabIndex = 4;
            buttonHistory.UseVisualStyleBackColor = true;
            // 
            // splitContainerSettings
            // 
            splitContainerSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerSettings.Location = new System.Drawing.Point(0, 0);
            splitContainerSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            splitContainerSettings.Name = "splitContainerSettings";
            splitContainerSettings.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerSettings.Panel1
            // 
            splitContainerSettings.Panel1.Controls.Add(tableLayoutPanelProject);
            // 
            // splitContainerSettings.Panel2
            // 
            splitContainerSettings.Panel2.Controls.Add(tableLayoutPanelSettings);
            splitContainerSettings.Size = new System.Drawing.Size(769, 742);
            splitContainerSettings.SplitterDistance = 398;
            splitContainerSettings.SplitterWidth = 6;
            splitContainerSettings.TabIndex = 1;
            // 
            // tableLayoutPanelProject
            // 
            tableLayoutPanelProject.ColumnCount = 3;
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProject.Controls.Add(labelURL, 0, 5);
            tableLayoutPanelProject.Controls.Add(labelInstitution, 0, 4);
            tableLayoutPanelProject.Controls.Add(labelProject, 0, 0);
            tableLayoutPanelProject.Controls.Add(textBoxProject, 1, 0);
            tableLayoutPanelProject.Controls.Add(labelProjectTitle, 0, 1);
            tableLayoutPanelProject.Controls.Add(textBoxProjectTitle, 1, 1);
            tableLayoutPanelProject.Controls.Add(labelProjectDescription, 0, 2);
            tableLayoutPanelProject.Controls.Add(textBoxProjectDescription, 1, 2);
            tableLayoutPanelProject.Controls.Add(labelProjectEditors, 0, 3);
            tableLayoutPanelProject.Controls.Add(textBoxProjectEditors, 1, 3);
            tableLayoutPanelProject.Controls.Add(labelProjectNotes, 0, 6);
            tableLayoutPanelProject.Controls.Add(textBoxProjectNotes, 1, 6);
            tableLayoutPanelProject.Controls.Add(labelProjectCopyright, 0, 7);
            tableLayoutPanelProject.Controls.Add(textBoxProjectCopyright, 1, 7);
            tableLayoutPanelProject.Controls.Add(labelProjectVersion, 0, 8);
            tableLayoutPanelProject.Controls.Add(textBoxProjectVersion, 1, 8);
            tableLayoutPanelProject.Controls.Add(textBoxInstitution, 1, 4);
            tableLayoutPanelProject.Controls.Add(buttonURL, 2, 5);
            tableLayoutPanelProject.Controls.Add(textBoxURL, 1, 5);
            tableLayoutPanelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelProject.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelProject.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tableLayoutPanelProject.Name = "tableLayoutPanelProject";
            tableLayoutPanelProject.RowCount = 9;
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProject.Size = new System.Drawing.Size(769, 398);
            tableLayoutPanelProject.TabIndex = 0;
            // 
            // labelURL
            // 
            labelURL.AutoSize = true;
            labelURL.Dock = System.Windows.Forms.DockStyle.Fill;
            labelURL.Location = new System.Drawing.Point(4, 185);
            labelURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelURL.Name = "labelURL";
            labelURL.Size = new System.Drawing.Size(90, 51);
            labelURL.TabIndex = 19;
            labelURL.Text = "URL:";
            labelURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelInstitution
            // 
            labelInstitution.AutoSize = true;
            labelInstitution.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInstitution.Location = new System.Drawing.Point(4, 148);
            labelInstitution.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelInstitution.Name = "labelInstitution";
            labelInstitution.Size = new System.Drawing.Size(90, 37);
            labelInstitution.TabIndex = 17;
            labelInstitution.Text = "Institution:";
            labelInstitution.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelProject
            // 
            labelProject.AutoSize = true;
            labelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProject.Location = new System.Drawing.Point(4, 0);
            labelProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProject.Name = "labelProject";
            labelProject.Size = new System.Drawing.Size(90, 37);
            labelProject.TabIndex = 0;
            labelProject.Text = "Display text:";
            labelProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProject
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProject, 2);
            textBoxProject.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProject.Location = new System.Drawing.Point(102, 5);
            textBoxProject.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxProject.Name = "textBoxProject";
            textBoxProject.Size = new System.Drawing.Size(663, 27);
            textBoxProject.TabIndex = 1;
            // 
            // labelProjectTitle
            // 
            labelProjectTitle.AutoSize = true;
            labelProjectTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectTitle.Location = new System.Drawing.Point(4, 37);
            labelProjectTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectTitle.Name = "labelProjectTitle";
            labelProjectTitle.Size = new System.Drawing.Size(90, 37);
            labelProjectTitle.TabIndex = 2;
            labelProjectTitle.Text = "Title:";
            labelProjectTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProjectTitle
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProjectTitle, 2);
            textBoxProjectTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectTitle.Location = new System.Drawing.Point(102, 42);
            textBoxProjectTitle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxProjectTitle.Name = "textBoxProjectTitle";
            textBoxProjectTitle.Size = new System.Drawing.Size(663, 27);
            textBoxProjectTitle.TabIndex = 3;
            // 
            // labelProjectDescription
            // 
            labelProjectDescription.AutoSize = true;
            labelProjectDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectDescription.Location = new System.Drawing.Point(4, 74);
            labelProjectDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectDescription.Name = "labelProjectDescription";
            labelProjectDescription.Size = new System.Drawing.Size(90, 37);
            labelProjectDescription.TabIndex = 4;
            labelProjectDescription.Text = "Description:";
            labelProjectDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProjectDescription
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProjectDescription, 2);
            textBoxProjectDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectDescription.Location = new System.Drawing.Point(102, 79);
            textBoxProjectDescription.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxProjectDescription.Name = "textBoxProjectDescription";
            textBoxProjectDescription.Size = new System.Drawing.Size(663, 27);
            textBoxProjectDescription.TabIndex = 5;
            // 
            // labelProjectEditors
            // 
            labelProjectEditors.AutoSize = true;
            labelProjectEditors.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectEditors.Location = new System.Drawing.Point(4, 111);
            labelProjectEditors.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectEditors.Name = "labelProjectEditors";
            labelProjectEditors.Size = new System.Drawing.Size(90, 37);
            labelProjectEditors.TabIndex = 6;
            labelProjectEditors.Text = "Editors:";
            labelProjectEditors.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProjectEditors
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProjectEditors, 2);
            textBoxProjectEditors.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectEditors.Location = new System.Drawing.Point(102, 116);
            textBoxProjectEditors.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxProjectEditors.Name = "textBoxProjectEditors";
            textBoxProjectEditors.Size = new System.Drawing.Size(663, 27);
            textBoxProjectEditors.TabIndex = 7;
            // 
            // labelProjectNotes
            // 
            labelProjectNotes.AutoSize = true;
            labelProjectNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectNotes.Location = new System.Drawing.Point(4, 245);
            labelProjectNotes.Margin = new System.Windows.Forms.Padding(4, 9, 4, 0);
            labelProjectNotes.Name = "labelProjectNotes";
            labelProjectNotes.Size = new System.Drawing.Size(90, 79);
            labelProjectNotes.TabIndex = 8;
            labelProjectNotes.Text = "Notes:";
            labelProjectNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxProjectNotes
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProjectNotes, 2);
            textBoxProjectNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectNotes.Location = new System.Drawing.Point(102, 241);
            textBoxProjectNotes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxProjectNotes.Multiline = true;
            textBoxProjectNotes.Name = "textBoxProjectNotes";
            textBoxProjectNotes.Size = new System.Drawing.Size(663, 78);
            textBoxProjectNotes.TabIndex = 9;
            // 
            // labelProjectCopyright
            // 
            labelProjectCopyright.AutoSize = true;
            labelProjectCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectCopyright.Location = new System.Drawing.Point(4, 324);
            labelProjectCopyright.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectCopyright.Name = "labelProjectCopyright";
            labelProjectCopyright.Size = new System.Drawing.Size(90, 37);
            labelProjectCopyright.TabIndex = 10;
            labelProjectCopyright.Text = "Copyright:";
            labelProjectCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProjectCopyright
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProjectCopyright, 2);
            textBoxProjectCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectCopyright.Location = new System.Drawing.Point(102, 329);
            textBoxProjectCopyright.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxProjectCopyright.Name = "textBoxProjectCopyright";
            textBoxProjectCopyright.Size = new System.Drawing.Size(663, 27);
            textBoxProjectCopyright.TabIndex = 11;
            // 
            // labelProjectVersion
            // 
            labelProjectVersion.AutoSize = true;
            labelProjectVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectVersion.Location = new System.Drawing.Point(4, 361);
            labelProjectVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectVersion.Name = "labelProjectVersion";
            labelProjectVersion.Size = new System.Drawing.Size(90, 37);
            labelProjectVersion.TabIndex = 12;
            labelProjectVersion.Text = "Version:";
            labelProjectVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProjectVersion
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxProjectVersion, 2);
            textBoxProjectVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProjectVersion.Location = new System.Drawing.Point(102, 366);
            textBoxProjectVersion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxProjectVersion.Name = "textBoxProjectVersion";
            textBoxProjectVersion.Size = new System.Drawing.Size(663, 27);
            textBoxProjectVersion.TabIndex = 13;
            // 
            // textBoxInstitution
            // 
            tableLayoutPanelProject.SetColumnSpan(textBoxInstitution, 2);
            textBoxInstitution.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxInstitution.Location = new System.Drawing.Point(102, 153);
            textBoxInstitution.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxInstitution.Name = "textBoxInstitution";
            textBoxInstitution.Size = new System.Drawing.Size(663, 27);
            textBoxInstitution.TabIndex = 18;
            // 
            // buttonURL
            // 
            buttonURL.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonURL.Image = Properties.Resources.Browse;
            buttonURL.Location = new System.Drawing.Point(732, 185);
            buttonURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            buttonURL.Name = "buttonURL";
            buttonURL.Size = new System.Drawing.Size(33, 51);
            buttonURL.TabIndex = 20;
            buttonURL.UseVisualStyleBackColor = true;
            // 
            // textBoxURL
            // 
            textBoxURL.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxURL.Location = new System.Drawing.Point(102, 190);
            textBoxURL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxURL.Name = "textBoxURL";
            textBoxURL.Size = new System.Drawing.Size(622, 27);
            textBoxURL.TabIndex = 21;
            // 
            // tableLayoutPanelSettings
            // 
            tableLayoutPanelSettings.ColumnCount = 5;
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSettings.Controls.Add(labelSettings, 0, 0);
            tableLayoutPanelSettings.Controls.Add(treeViewSettings, 0, 1);
            tableLayoutPanelSettings.Controls.Add(buttonSettingsAdd, 3, 0);
            tableLayoutPanelSettings.Controls.Add(textBoxSettings, 2, 2);
            tableLayoutPanelSettings.Controls.Add(labelSettingValue, 1, 2);
            tableLayoutPanelSettings.Controls.Add(buttonSettingsRemove, 2, 0);
            tableLayoutPanelSettings.Controls.Add(buttonSettingsCopyFromParent, 4, 0);
            tableLayoutPanelSettings.Controls.Add(textBoxSetting, 0, 2);
            tableLayoutPanelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelSettings.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tableLayoutPanelSettings.Name = "tableLayoutPanelSettings";
            tableLayoutPanelSettings.RowCount = 3;
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSettings.Size = new System.Drawing.Size(769, 338);
            tableLayoutPanelSettings.TabIndex = 0;
            // 
            // labelSettings
            // 
            labelSettings.AutoSize = true;
            labelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSettings.Location = new System.Drawing.Point(4, 0);
            labelSettings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSettings.Name = "labelSettings";
            labelSettings.Size = new System.Drawing.Size(244, 35);
            labelSettings.TabIndex = 0;
            labelSettings.Text = "Settings";
            labelSettings.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // treeViewSettings
            // 
            tableLayoutPanelSettings.SetColumnSpan(treeViewSettings, 5);
            treeViewSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewSettings.Location = new System.Drawing.Point(4, 40);
            treeViewSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            treeViewSettings.Name = "treeViewSettings";
            treeViewSettings.Size = new System.Drawing.Size(761, 256);
            treeViewSettings.TabIndex = 1;
            // 
            // buttonSettingsAdd
            // 
            buttonSettingsAdd.Dock = System.Windows.Forms.DockStyle.Right;
            buttonSettingsAdd.Location = new System.Drawing.Point(453, 0);
            buttonSettingsAdd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            buttonSettingsAdd.Name = "buttonSettingsAdd";
            buttonSettingsAdd.Size = new System.Drawing.Size(133, 35);
            buttonSettingsAdd.TabIndex = 2;
            buttonSettingsAdd.Text = "Add setting";
            buttonSettingsAdd.UseVisualStyleBackColor = true;
            // 
            // textBoxSettings
            // 
            tableLayoutPanelSettings.SetColumnSpan(textBoxSettings, 3);
            textBoxSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxSettings.Location = new System.Drawing.Point(312, 306);
            textBoxSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxSettings.Name = "textBoxSettings";
            textBoxSettings.Size = new System.Drawing.Size(453, 27);
            textBoxSettings.TabIndex = 3;
            // 
            // labelSettingValue
            // 
            labelSettingValue.AutoSize = true;
            labelSettingValue.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSettingValue.Location = new System.Drawing.Point(256, 301);
            labelSettingValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSettingValue.Name = "labelSettingValue";
            labelSettingValue.Size = new System.Drawing.Size(48, 37);
            labelSettingValue.TabIndex = 4;
            labelSettingValue.Text = "Value:";
            labelSettingValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonSettingsRemove
            // 
            buttonSettingsRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSettingsRemove.Location = new System.Drawing.Point(312, 0);
            buttonSettingsRemove.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            buttonSettingsRemove.Name = "buttonSettingsRemove";
            buttonSettingsRemove.Size = new System.Drawing.Size(133, 35);
            buttonSettingsRemove.TabIndex = 5;
            buttonSettingsRemove.Text = "Remove setting";
            buttonSettingsRemove.UseVisualStyleBackColor = true;
            // 
            // buttonSettingsCopyFromParent
            // 
            buttonSettingsCopyFromParent.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSettingsCopyFromParent.Location = new System.Drawing.Point(594, 0);
            buttonSettingsCopyFromParent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            buttonSettingsCopyFromParent.Name = "buttonSettingsCopyFromParent";
            buttonSettingsCopyFromParent.Size = new System.Drawing.Size(171, 35);
            buttonSettingsCopyFromParent.TabIndex = 6;
            buttonSettingsCopyFromParent.Text = "Copy settings from parent";
            buttonSettingsCopyFromParent.UseVisualStyleBackColor = true;
            // 
            // textBoxSetting
            // 
            textBoxSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxSetting.Location = new System.Drawing.Point(4, 306);
            textBoxSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxSetting.Name = "textBoxSetting";
            textBoxSetting.Size = new System.Drawing.Size(244, 27);
            textBoxSetting.TabIndex = 7;
            // 
            // UserControlProjects
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainerMain);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "UserControlProjects";
            Size = new System.Drawing.Size(1160, 897);
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)fKProjectProjectBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)projectBindingSource).EndInit();
            splitContainerData.Panel1.ResumeLayout(false);
            splitContainerData.Panel1.PerformLayout();
            splitContainerData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerData).EndInit();
            splitContainerData.ResumeLayout(false);
            toolStripHierarchy.ResumeLayout(false);
            toolStripHierarchy.PerformLayout();
            tableLayoutPanelHeader.ResumeLayout(false);
            tableLayoutPanelHeader.PerformLayout();
            splitContainerSettings.Panel1.ResumeLayout(false);
            splitContainerSettings.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerSettings).EndInit();
            splitContainerSettings.ResumeLayout(false);
            tableLayoutPanelProject.ResumeLayout(false);
            tableLayoutPanelProject.PerformLayout();
            tableLayoutPanelSettings.ResumeLayout(false);
            tableLayoutPanelSettings.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.TreeView treeViewProjects;
        private System.Windows.Forms.ToolStrip toolStripHierarchy;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectSetParent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.Label labelHeaderProject;
        private System.Windows.Forms.TextBox textBoxHeaderProject;
        private System.Windows.Forms.Label labelHeaderProjectID;
        private System.Windows.Forms.TextBox textBoxHeaderProjectID;
        private System.Windows.Forms.Button buttonHistory;
        private System.Windows.Forms.SplitContainer splitContainerSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProject;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.Label labelInstitution;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.TextBox textBoxProject;
        private System.Windows.Forms.Label labelProjectTitle;
        private System.Windows.Forms.TextBox textBoxProjectTitle;
        private System.Windows.Forms.Label labelProjectDescription;
        private System.Windows.Forms.TextBox textBoxProjectDescription;
        private System.Windows.Forms.Label labelProjectEditors;
        private System.Windows.Forms.TextBox textBoxProjectEditors;
        private System.Windows.Forms.Label labelProjectNotes;
        private System.Windows.Forms.TextBox textBoxProjectNotes;
        private System.Windows.Forms.Label labelProjectCopyright;
        private System.Windows.Forms.TextBox textBoxProjectCopyright;
        private System.Windows.Forms.Label labelProjectVersion;
        private System.Windows.Forms.TextBox textBoxProjectVersion;
        private System.Windows.Forms.TextBox textBoxInstitution;
        private System.Windows.Forms.Button buttonURL;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSettings;
        private System.Windows.Forms.Label labelSettings;
        private System.Windows.Forms.TreeView treeViewSettings;
        private System.Windows.Forms.Button buttonSettingsAdd;
        private System.Windows.Forms.TextBox textBoxSettings;
        private System.Windows.Forms.Label labelSettingValue;
        private System.Windows.Forms.Button buttonSettingsRemove;
        private System.Windows.Forms.Button buttonSettingsCopyFromParent;
        private System.Windows.Forms.TextBox textBoxSetting;
        private System.Windows.Forms.ToolTip toolTip;
        private DiversityWorkbench.Datasets.DataSetProjects dataSetProjects;
        private System.Windows.Forms.BindingSource projectBindingSource;
        private DiversityWorkbench.Datasets.DataSetProjectsTableAdapters.ProjectTableAdapter projectTableAdapter;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.BindingSource fKProjectProjectBindingSource;
    }
}
