namespace DiversityWorkbench.Forms
{
    partial class FormEnumAdministration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEnumAdministration));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.treeViewEnum = new System.Windows.Forms.TreeView();
            this.toolStripList = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSetParent = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveParent = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTableEditor = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelEnum = new System.Windows.Forms.TableLayoutPanel();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelDisplayText = new System.Windows.Forms.Label();
            this.textBoxDisplayText = new System.Windows.Forms.TextBox();
            this.labelInternalNotes = new System.Windows.Forms.Label();
            this.textBoxInternalNotes = new System.Windows.Forms.TextBox();
            this.labelCode = new System.Windows.Forms.Label();
            this.checkBoxDisplayEnable = new System.Windows.Forms.CheckBox();
            this.labelDisplayOrder = new System.Windows.Forms.Label();
            this.numericUpDownDisplayOrder = new System.Windows.Forms.NumericUpDown();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.buttonGetImage = new System.Windows.Forms.Button();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.labelURL = new System.Windows.Forms.Label();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.buttonGetURL = new System.Windows.Forms.Button();
            this.labelOption = new System.Windows.Forms.Label();
            this.pictureBoxOption = new System.Windows.Forms.PictureBox();
            this.labelAbbreviation = new System.Windows.Forms.Label();
            this.textBoxAbbreviation = new System.Windows.Forms.TextBox();
            this.labelModuleName = new System.Windows.Forms.Label();
            this.comboBoxModuleName = new System.Windows.Forms.ComboBox();
            this.labelParentRelation = new System.Windows.Forms.Label();
            this.comboBoxParentRelation = new System.Windows.Forms.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainerSupplements = new System.Windows.Forms.SplitContainer();
            this.tabControlSupplements = new System.Windows.Forms.TabControl();
            this.tabPageEntity = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelRepresentation = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewRepresentation = new System.Windows.Forms.DataGridView();
            this.labelEntity = new System.Windows.Forms.Label();
            this.buttonRepresentationNew = new System.Windows.Forms.Button();
            this.tabPageProject = new System.Windows.Forms.TabPage();
            this.listBoxProjects = new System.Windows.Forms.ListBox();
            this.toolStripProjects = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonProjectAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonProjectDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonProjectList = new System.Windows.Forms.ToolStripButton();
            this.imageListSupplements = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.toolStripList.SuspendLayout();
            this.tableLayoutPanelEnum.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplayOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSupplements)).BeginInit();
            this.splitContainerSupplements.Panel1.SuspendLayout();
            this.splitContainerSupplements.Panel2.SuspendLayout();
            this.splitContainerSupplements.SuspendLayout();
            this.tabControlSupplements.SuspendLayout();
            this.tabPageEntity.SuspendLayout();
            this.tableLayoutPanelRepresentation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRepresentation)).BeginInit();
            this.tabPageProject.SuspendLayout();
            this.toolStripProjects.SuspendLayout();
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
            this.splitContainerMain.Panel1.Controls.Add(this.treeViewEnum);
            this.splitContainerMain.Panel1.Controls.Add(this.toolStripList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelEnum);
            this.splitContainerMain.Size = new System.Drawing.Size(431, 421);
            this.splitContainerMain.SplitterDistance = 197;
            this.splitContainerMain.TabIndex = 0;
            // 
            // treeViewEnum
            // 
            this.treeViewEnum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewEnum.Location = new System.Drawing.Point(0, 0);
            this.treeViewEnum.Name = "treeViewEnum";
            this.treeViewEnum.Size = new System.Drawing.Size(197, 396);
            this.treeViewEnum.TabIndex = 1;
            this.treeViewEnum.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewEnum_AfterSelect);
            // 
            // toolStripList
            // 
            this.toolStripList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripList.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonDelete,
            this.toolStripButtonSetParent,
            this.toolStripButtonRemoveParent,
            this.toolStripButtonTableEditor});
            this.toolStripList.Location = new System.Drawing.Point(0, 396);
            this.toolStripList.Name = "toolStripList";
            this.toolStripList.Size = new System.Drawing.Size(197, 25);
            this.toolStripList.TabIndex = 0;
            this.toolStripList.Text = "toolStrip1";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAdd.Text = "Add a new entry";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDelete.Text = "Delete the selected entry";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonSetParent
            // 
            this.toolStripButtonSetParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSetParent.Image = global::DiversityWorkbench.Properties.Resources.SetParent;
            this.toolStripButtonSetParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSetParent.Name = "toolStripButtonSetParent";
            this.toolStripButtonSetParent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSetParent.Text = "Set the parent of the selected entry";
            this.toolStripButtonSetParent.Click += new System.EventHandler(this.toolStripButtonSetParent_Click);
            // 
            // toolStripButtonRemoveParent
            // 
            this.toolStripButtonRemoveParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveParent.Image = global::DiversityWorkbench.Properties.Resources.RemoveParent;
            this.toolStripButtonRemoveParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveParent.Name = "toolStripButtonRemoveParent";
            this.toolStripButtonRemoveParent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemoveParent.Text = "Remove the relation to the parent item";
            this.toolStripButtonRemoveParent.Click += new System.EventHandler(this.toolStripButtonRemoveParent_Click);
            // 
            // toolStripButtonTableEditor
            // 
            this.toolStripButtonTableEditor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTableEditor.Image = global::DiversityWorkbench.Properties.Resources.EditInSpeadsheet;
            this.toolStripButtonTableEditor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTableEditor.Name = "toolStripButtonTableEditor";
            this.toolStripButtonTableEditor.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTableEditor.ToolTipText = "Edit data in table editor";
            this.toolStripButtonTableEditor.Click += new System.EventHandler(this.toolStripButtonTableEditor_Click);
            // 
            // tableLayoutPanelEnum
            // 
            this.tableLayoutPanelEnum.ColumnCount = 3;
            this.tableLayoutPanelEnum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEnum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEnum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEnum.Controls.Add(this.labelDescription, 0, 8);
            this.tableLayoutPanelEnum.Controls.Add(this.textBoxDescription, 0, 9);
            this.tableLayoutPanelEnum.Controls.Add(this.labelDisplayText, 0, 4);
            this.tableLayoutPanelEnum.Controls.Add(this.textBoxDisplayText, 0, 5);
            this.tableLayoutPanelEnum.Controls.Add(this.labelInternalNotes, 0, 10);
            this.tableLayoutPanelEnum.Controls.Add(this.textBoxInternalNotes, 0, 11);
            this.tableLayoutPanelEnum.Controls.Add(this.labelCode, 0, 0);
            this.tableLayoutPanelEnum.Controls.Add(this.checkBoxDisplayEnable, 1, 3);
            this.tableLayoutPanelEnum.Controls.Add(this.labelDisplayOrder, 0, 2);
            this.tableLayoutPanelEnum.Controls.Add(this.numericUpDownDisplayOrder, 0, 3);
            this.tableLayoutPanelEnum.Controls.Add(this.pictureBoxImage, 1, 2);
            this.tableLayoutPanelEnum.Controls.Add(this.buttonGetImage, 2, 2);
            this.tableLayoutPanelEnum.Controls.Add(this.buttonFeedback, 2, 0);
            this.tableLayoutPanelEnum.Controls.Add(this.labelURL, 0, 12);
            this.tableLayoutPanelEnum.Controls.Add(this.textBoxURL, 0, 13);
            this.tableLayoutPanelEnum.Controls.Add(this.buttonGetURL, 2, 13);
            this.tableLayoutPanelEnum.Controls.Add(this.labelOption, 0, 1);
            this.tableLayoutPanelEnum.Controls.Add(this.pictureBoxOption, 2, 1);
            this.tableLayoutPanelEnum.Controls.Add(this.labelAbbreviation, 0, 6);
            this.tableLayoutPanelEnum.Controls.Add(this.textBoxAbbreviation, 1, 6);
            this.tableLayoutPanelEnum.Controls.Add(this.labelModuleName, 0, 14);
            this.tableLayoutPanelEnum.Controls.Add(this.comboBoxModuleName, 1, 14);
            this.tableLayoutPanelEnum.Controls.Add(this.labelParentRelation, 0, 7);
            this.tableLayoutPanelEnum.Controls.Add(this.comboBoxParentRelation, 1, 7);
            this.tableLayoutPanelEnum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEnum.Enabled = false;
            this.tableLayoutPanelEnum.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelEnum.Name = "tableLayoutPanelEnum";
            this.tableLayoutPanelEnum.RowCount = 15;
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEnum.Size = new System.Drawing.Size(230, 421);
            this.tableLayoutPanelEnum.TabIndex = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.tableLayoutPanelEnum.SetColumnSpan(this.labelDescription, 3);
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(3, 188);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(224, 13);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "Description:";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxDescription
            // 
            this.tableLayoutPanelEnum.SetColumnSpan(this.textBoxDescription, 3);
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(3, 204);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(224, 64);
            this.textBoxDescription.TabIndex = 2;
            this.toolTip.SetToolTip(this.textBoxDescription, "The description of the entry");
            // 
            // labelDisplayText
            // 
            this.labelDisplayText.AutoSize = true;
            this.tableLayoutPanelEnum.SetColumnSpan(this.labelDisplayText, 3);
            this.labelDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayText.Location = new System.Drawing.Point(3, 90);
            this.labelDisplayText.Name = "labelDisplayText";
            this.labelDisplayText.Size = new System.Drawing.Size(224, 13);
            this.labelDisplayText.TabIndex = 3;
            this.labelDisplayText.Text = "Display text:";
            this.labelDisplayText.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxDisplayText
            // 
            this.tableLayoutPanelEnum.SetColumnSpan(this.textBoxDisplayText, 3);
            this.textBoxDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDisplayText.Location = new System.Drawing.Point(3, 106);
            this.textBoxDisplayText.Name = "textBoxDisplayText";
            this.textBoxDisplayText.Size = new System.Drawing.Size(224, 20);
            this.textBoxDisplayText.TabIndex = 4;
            this.toolTip.SetToolTip(this.textBoxDisplayText, "The text as shown in an interface");
            // 
            // labelInternalNotes
            // 
            this.labelInternalNotes.AutoSize = true;
            this.tableLayoutPanelEnum.SetColumnSpan(this.labelInternalNotes, 3);
            this.labelInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInternalNotes.Location = new System.Drawing.Point(3, 271);
            this.labelInternalNotes.Name = "labelInternalNotes";
            this.labelInternalNotes.Size = new System.Drawing.Size(224, 13);
            this.labelInternalNotes.TabIndex = 5;
            this.labelInternalNotes.Text = "Internal notes:";
            this.labelInternalNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxInternalNotes
            // 
            this.tableLayoutPanelEnum.SetColumnSpan(this.textBoxInternalNotes, 3);
            this.textBoxInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInternalNotes.Location = new System.Drawing.Point(3, 287);
            this.textBoxInternalNotes.Multiline = true;
            this.textBoxInternalNotes.Name = "textBoxInternalNotes";
            this.textBoxInternalNotes.Size = new System.Drawing.Size(224, 64);
            this.textBoxInternalNotes.TabIndex = 6;
            this.toolTip.SetToolTip(this.textBoxInternalNotes, "Internal notes about this entry");
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.tableLayoutPanelEnum.SetColumnSpan(this.labelCode, 2);
            this.labelCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCode.Location = new System.Drawing.Point(6, 6);
            this.labelCode.Margin = new System.Windows.Forms.Padding(6);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(192, 13);
            this.labelCode.TabIndex = 0;
            this.labelCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBoxDisplayEnable
            // 
            this.checkBoxDisplayEnable.AutoSize = true;
            this.tableLayoutPanelEnum.SetColumnSpan(this.checkBoxDisplayEnable, 2);
            this.checkBoxDisplayEnable.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxDisplayEnable.Location = new System.Drawing.Point(162, 67);
            this.checkBoxDisplayEnable.Name = "checkBoxDisplayEnable";
            this.checkBoxDisplayEnable.Size = new System.Drawing.Size(65, 20);
            this.checkBoxDisplayEnable.TabIndex = 7;
            this.checkBoxDisplayEnable.Text = "Enabled";
            this.toolTip.SetToolTip(this.checkBoxDisplayEnable, "If the entry should be enabled");
            this.checkBoxDisplayEnable.UseVisualStyleBackColor = true;
            this.checkBoxDisplayEnable.Click += new System.EventHandler(this.checkBoxDisplayEnable_Click);
            // 
            // labelDisplayOrder
            // 
            this.labelDisplayOrder.AutoSize = true;
            this.labelDisplayOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayOrder.Location = new System.Drawing.Point(3, 41);
            this.labelDisplayOrder.Name = "labelDisplayOrder";
            this.labelDisplayOrder.Size = new System.Drawing.Size(61, 23);
            this.labelDisplayOrder.TabIndex = 8;
            this.labelDisplayOrder.Text = "Position:";
            this.labelDisplayOrder.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // numericUpDownDisplayOrder
            // 
            this.numericUpDownDisplayOrder.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownDisplayOrder.Location = new System.Drawing.Point(3, 67);
            this.numericUpDownDisplayOrder.Name = "numericUpDownDisplayOrder";
            this.numericUpDownDisplayOrder.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownDisplayOrder.TabIndex = 9;
            this.numericUpDownDisplayOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.numericUpDownDisplayOrder, "The position within a list if not sorted by the display text");
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxImage.Location = new System.Drawing.Point(181, 41);
            this.pictureBoxImage.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Padding = new System.Windows.Forms.Padding(7, 3, 0, 7);
            this.pictureBoxImage.Size = new System.Drawing.Size(23, 23);
            this.pictureBoxImage.TabIndex = 10;
            this.pictureBoxImage.TabStop = false;
            this.pictureBoxImage.Visible = false;
            // 
            // buttonGetImage
            // 
            this.buttonGetImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGetImage.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.buttonGetImage.Location = new System.Drawing.Point(204, 41);
            this.buttonGetImage.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonGetImage.Name = "buttonGetImage";
            this.buttonGetImage.Size = new System.Drawing.Size(23, 23);
            this.buttonGetImage.TabIndex = 11;
            this.toolTip.SetToolTip(this.buttonGetImage, "Find an image for this entry");
            this.buttonGetImage.UseVisualStyleBackColor = true;
            this.buttonGetImage.Visible = false;
            this.buttonGetImage.Click += new System.EventHandler(this.buttonGetImage_Click);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFeedback.FlatAppearance.BorderSize = 0;
            this.buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(207, 3);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(20, 19);
            this.buttonFeedback.TabIndex = 12;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send a feedback to the administrator");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // labelURL
            // 
            this.labelURL.AutoSize = true;
            this.tableLayoutPanelEnum.SetColumnSpan(this.labelURL, 3);
            this.labelURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelURL.Location = new System.Drawing.Point(3, 354);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(224, 13);
            this.labelURL.TabIndex = 13;
            this.labelURL.Text = "URL";
            // 
            // textBoxURL
            // 
            this.tableLayoutPanelEnum.SetColumnSpan(this.textBoxURL, 2);
            this.textBoxURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxURL.Location = new System.Drawing.Point(3, 370);
            this.textBoxURL.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(201, 20);
            this.textBoxURL.TabIndex = 14;
            // 
            // buttonGetURL
            // 
            this.buttonGetURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGetURL.FlatAppearance.BorderSize = 0;
            this.buttonGetURL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetURL.Image = global::DiversityWorkbench.Properties.Resources.Browse;
            this.buttonGetURL.Location = new System.Drawing.Point(204, 369);
            this.buttonGetURL.Margin = new System.Windows.Forms.Padding(0, 2, 3, 3);
            this.buttonGetURL.Name = "buttonGetURL";
            this.buttonGetURL.Size = new System.Drawing.Size(23, 22);
            this.buttonGetURL.TabIndex = 15;
            this.toolTip.SetToolTip(this.buttonGetURL, "Search for an URL");
            this.buttonGetURL.UseVisualStyleBackColor = true;
            this.buttonGetURL.Click += new System.EventHandler(this.buttonGetURL_Click);
            // 
            // labelOption
            // 
            this.labelOption.AutoSize = true;
            this.tableLayoutPanelEnum.SetColumnSpan(this.labelOption, 2);
            this.labelOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOption.Location = new System.Drawing.Point(3, 25);
            this.labelOption.Name = "labelOption";
            this.labelOption.Size = new System.Drawing.Size(198, 16);
            this.labelOption.TabIndex = 16;
            this.labelOption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelOption.Visible = false;
            // 
            // pictureBoxOption
            // 
            this.pictureBoxOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxOption.Location = new System.Drawing.Point(204, 25);
            this.pictureBoxOption.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxOption.Name = "pictureBoxOption";
            this.pictureBoxOption.Size = new System.Drawing.Size(26, 16);
            this.pictureBoxOption.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxOption.TabIndex = 17;
            this.pictureBoxOption.TabStop = false;
            this.pictureBoxOption.Visible = false;
            // 
            // labelAbbreviation
            // 
            this.labelAbbreviation.AutoSize = true;
            this.labelAbbreviation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAbbreviation.Location = new System.Drawing.Point(3, 129);
            this.labelAbbreviation.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelAbbreviation.Name = "labelAbbreviation";
            this.labelAbbreviation.Size = new System.Drawing.Size(64, 26);
            this.labelAbbreviation.TabIndex = 18;
            this.labelAbbreviation.Text = "Abbrev.:";
            this.labelAbbreviation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelAbbreviation.Visible = false;
            // 
            // textBoxAbbreviation
            // 
            this.tableLayoutPanelEnum.SetColumnSpan(this.textBoxAbbreviation, 2);
            this.textBoxAbbreviation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAbbreviation.Location = new System.Drawing.Point(67, 132);
            this.textBoxAbbreviation.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxAbbreviation.Name = "textBoxAbbreviation";
            this.textBoxAbbreviation.Size = new System.Drawing.Size(160, 20);
            this.textBoxAbbreviation.TabIndex = 19;
            this.textBoxAbbreviation.Visible = false;
            // 
            // labelModuleName
            // 
            this.labelModuleName.AutoSize = true;
            this.labelModuleName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModuleName.Image = global::DiversityWorkbench.Properties.Resources.DiversityWorkbench16;
            this.labelModuleName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelModuleName.Location = new System.Drawing.Point(0, 394);
            this.labelModuleName.Margin = new System.Windows.Forms.Padding(0);
            this.labelModuleName.Name = "labelModuleName";
            this.labelModuleName.Size = new System.Drawing.Size(67, 27);
            this.labelModuleName.TabIndex = 20;
            this.labelModuleName.Text = "Module:";
            this.labelModuleName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelModuleName.Visible = false;
            // 
            // comboBoxModuleName
            // 
            this.tableLayoutPanelEnum.SetColumnSpan(this.comboBoxModuleName, 2);
            this.comboBoxModuleName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxModuleName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxModuleName.FormattingEnabled = true;
            this.comboBoxModuleName.Location = new System.Drawing.Point(70, 397);
            this.comboBoxModuleName.Name = "comboBoxModuleName";
            this.comboBoxModuleName.Size = new System.Drawing.Size(157, 21);
            this.comboBoxModuleName.TabIndex = 21;
            this.comboBoxModuleName.Visible = false;
            // 
            // labelParentRelation
            // 
            this.labelParentRelation.AutoSize = true;
            this.labelParentRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParentRelation.Location = new System.Drawing.Point(3, 155);
            this.labelParentRelation.Name = "labelParentRelation";
            this.labelParentRelation.Size = new System.Drawing.Size(61, 27);
            this.labelParentRelation.TabIndex = 22;
            this.labelParentRelation.Text = "Relation:";
            this.labelParentRelation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelParentRelation.Visible = false;
            // 
            // comboBoxParentRelation
            // 
            this.tableLayoutPanelEnum.SetColumnSpan(this.comboBoxParentRelation, 2);
            this.comboBoxParentRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxParentRelation.FormattingEnabled = true;
            this.comboBoxParentRelation.Location = new System.Drawing.Point(70, 158);
            this.comboBoxParentRelation.Name = "comboBoxParentRelation";
            this.comboBoxParentRelation.Size = new System.Drawing.Size(157, 21);
            this.comboBoxParentRelation.TabIndex = 23;
            this.comboBoxParentRelation.Visible = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContainerSupplements
            // 
            this.splitContainerSupplements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSupplements.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSupplements.Name = "splitContainerSupplements";
            this.splitContainerSupplements.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerSupplements.Panel1
            // 
            this.splitContainerSupplements.Panel1.Controls.Add(this.splitContainerMain);
            // 
            // splitContainerSupplements.Panel2
            // 
            this.splitContainerSupplements.Panel2.Controls.Add(this.tabControlSupplements);
            this.splitContainerSupplements.Size = new System.Drawing.Size(431, 533);
            this.splitContainerSupplements.SplitterDistance = 421;
            this.splitContainerSupplements.TabIndex = 1;
            // 
            // tabControlSupplements
            // 
            this.tabControlSupplements.Controls.Add(this.tabPageEntity);
            this.tabControlSupplements.Controls.Add(this.tabPageProject);
            this.tabControlSupplements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSupplements.ImageList = this.imageListSupplements;
            this.tabControlSupplements.Location = new System.Drawing.Point(0, 0);
            this.tabControlSupplements.Name = "tabControlSupplements";
            this.tabControlSupplements.SelectedIndex = 0;
            this.tabControlSupplements.Size = new System.Drawing.Size(431, 108);
            this.tabControlSupplements.TabIndex = 3;
            // 
            // tabPageEntity
            // 
            this.tabPageEntity.Controls.Add(this.tableLayoutPanelRepresentation);
            this.tabPageEntity.ImageIndex = 0;
            this.tabPageEntity.Location = new System.Drawing.Point(4, 23);
            this.tabPageEntity.Name = "tabPageEntity";
            this.tabPageEntity.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEntity.Size = new System.Drawing.Size(423, 81);
            this.tabPageEntity.TabIndex = 0;
            this.tabPageEntity.Text = "Description";
            this.tabPageEntity.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelRepresentation
            // 
            this.tableLayoutPanelRepresentation.ColumnCount = 2;
            this.tableLayoutPanelRepresentation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRepresentation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRepresentation.Controls.Add(this.dataGridViewRepresentation, 0, 1);
            this.tableLayoutPanelRepresentation.Controls.Add(this.labelEntity, 0, 0);
            this.tableLayoutPanelRepresentation.Controls.Add(this.buttonRepresentationNew, 1, 0);
            this.tableLayoutPanelRepresentation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRepresentation.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelRepresentation.Name = "tableLayoutPanelRepresentation";
            this.tableLayoutPanelRepresentation.RowCount = 2;
            this.tableLayoutPanelRepresentation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRepresentation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRepresentation.Size = new System.Drawing.Size(417, 75);
            this.tableLayoutPanelRepresentation.TabIndex = 2;
            // 
            // dataGridViewRepresentation
            // 
            this.dataGridViewRepresentation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelRepresentation.SetColumnSpan(this.dataGridViewRepresentation, 2);
            this.dataGridViewRepresentation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRepresentation.Location = new System.Drawing.Point(3, 29);
            this.dataGridViewRepresentation.Name = "dataGridViewRepresentation";
            this.dataGridViewRepresentation.RowHeadersWidth = 25;
            this.dataGridViewRepresentation.Size = new System.Drawing.Size(411, 43);
            this.dataGridViewRepresentation.TabIndex = 0;
            this.dataGridViewRepresentation.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewRepresentation_DataError);
            // 
            // labelEntity
            // 
            this.labelEntity.AutoSize = true;
            this.labelEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEntity.Location = new System.Drawing.Point(3, 0);
            this.labelEntity.Name = "labelEntity";
            this.labelEntity.Size = new System.Drawing.Size(265, 26);
            this.labelEntity.TabIndex = 1;
            this.labelEntity.Text = "Representation of an entity in a certain context in the selected language";
            this.labelEntity.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonRepresentationNew
            // 
            this.buttonRepresentationNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRepresentationNew.Location = new System.Drawing.Point(274, 3);
            this.buttonRepresentationNew.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonRepresentationNew.Name = "buttonRepresentationNew";
            this.buttonRepresentationNew.Size = new System.Drawing.Size(140, 23);
            this.buttonRepresentationNew.TabIndex = 2;
            this.buttonRepresentationNew.Text = "Insert new representation";
            this.buttonRepresentationNew.UseVisualStyleBackColor = true;
            this.buttonRepresentationNew.Click += new System.EventHandler(this.buttonRepresentationNew_Click);
            // 
            // tabPageProject
            // 
            this.tabPageProject.Controls.Add(this.listBoxProjects);
            this.tabPageProject.Controls.Add(this.toolStripProjects);
            this.tabPageProject.ImageIndex = 1;
            this.tabPageProject.Location = new System.Drawing.Point(4, 23);
            this.tabPageProject.Name = "tabPageProject";
            this.tabPageProject.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProject.Size = new System.Drawing.Size(423, 81);
            this.tabPageProject.TabIndex = 1;
            this.tabPageProject.Text = "Projects";
            this.tabPageProject.UseVisualStyleBackColor = true;
            // 
            // listBoxProjects
            // 
            this.listBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjects.FormattingEnabled = true;
            this.listBoxProjects.Location = new System.Drawing.Point(3, 3);
            this.listBoxProjects.Name = "listBoxProjects";
            this.listBoxProjects.Size = new System.Drawing.Size(393, 75);
            this.listBoxProjects.TabIndex = 1;
            // 
            // toolStripProjects
            // 
            this.toolStripProjects.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripProjects.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripProjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonProjectAdd,
            this.toolStripButtonProjectDelete,
            this.toolStripButtonProjectList});
            this.toolStripProjects.Location = new System.Drawing.Point(396, 3);
            this.toolStripProjects.Name = "toolStripProjects";
            this.toolStripProjects.Size = new System.Drawing.Size(24, 75);
            this.toolStripProjects.TabIndex = 0;
            this.toolStripProjects.Text = "toolStrip1";
            // 
            // toolStripButtonProjectAdd
            // 
            this.toolStripButtonProjectAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonProjectAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectAdd.Name = "toolStripButtonProjectAdd";
            this.toolStripButtonProjectAdd.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonProjectAdd.Text = "Add a project";
            this.toolStripButtonProjectAdd.Click += new System.EventHandler(this.toolStripButtonProjectAdd_Click);
            // 
            // toolStripButtonProjectDelete
            // 
            this.toolStripButtonProjectDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonProjectDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectDelete.Name = "toolStripButtonProjectDelete";
            this.toolStripButtonProjectDelete.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonProjectDelete.Text = "Remove selected Project";
            this.toolStripButtonProjectDelete.Click += new System.EventHandler(this.toolStripButtonProjectDelete_Click);
            // 
            // toolStripButtonProjectList
            // 
            this.toolStripButtonProjectList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectList.Image = global::DiversityWorkbench.Properties.Resources.Find;
            this.toolStripButtonProjectList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectList.Name = "toolStripButtonProjectList";
            this.toolStripButtonProjectList.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonProjectList.Text = "List all entries for the selected project";
            this.toolStripButtonProjectList.Click += new System.EventHandler(this.toolStripButtonProjectList_Click);
            // 
            // imageListSupplements
            // 
            this.imageListSupplements.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSupplements.ImageStream")));
            this.imageListSupplements.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSupplements.Images.SetKeyName(0, "Documentation.ico");
            this.imageListSupplements.Images.SetKeyName(1, "Project.ico");
            // 
            // FormEnumAdministration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 533);
            this.Controls.Add(this.splitContainerSupplements);
            this.Name = "FormEnumAdministration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EnumAdministration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEnumAdministration_FormClosing);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.toolStripList.ResumeLayout(false);
            this.toolStripList.PerformLayout();
            this.tableLayoutPanelEnum.ResumeLayout(false);
            this.tableLayoutPanelEnum.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplayOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOption)).EndInit();
            this.splitContainerSupplements.Panel1.ResumeLayout(false);
            this.splitContainerSupplements.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSupplements)).EndInit();
            this.splitContainerSupplements.ResumeLayout(false);
            this.tabControlSupplements.ResumeLayout(false);
            this.tabPageEntity.ResumeLayout(false);
            this.tableLayoutPanelRepresentation.ResumeLayout(false);
            this.tableLayoutPanelRepresentation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRepresentation)).EndInit();
            this.tabPageProject.ResumeLayout(false);
            this.tabPageProject.PerformLayout();
            this.toolStripProjects.ResumeLayout(false);
            this.toolStripProjects.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ToolStrip toolStripList;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEnum;
        private System.Windows.Forms.TreeView treeViewEnum;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelDisplayText;
        private System.Windows.Forms.TextBox textBoxDisplayText;
        private System.Windows.Forms.Label labelInternalNotes;
        private System.Windows.Forms.TextBox textBoxInternalNotes;
        private System.Windows.Forms.CheckBox checkBoxDisplayEnable;
        private System.Windows.Forms.Label labelDisplayOrder;
        private System.Windows.Forms.NumericUpDown numericUpDownDisplayOrder;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.Button buttonGetImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripButton toolStripButtonSetParent;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.SplitContainer splitContainerSupplements;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRepresentation;
        private System.Windows.Forms.DataGridView dataGridViewRepresentation;
        private System.Windows.Forms.Label labelEntity;
        private System.Windows.Forms.Button buttonRepresentationNew;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.Button buttonGetURL;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveParent;
        private System.Windows.Forms.Label labelOption;
        private System.Windows.Forms.PictureBox pictureBoxOption;
        public System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.Label labelAbbreviation;
        private System.Windows.Forms.TextBox textBoxAbbreviation;
        private System.Windows.Forms.Label labelModuleName;
        private System.Windows.Forms.ComboBox comboBoxModuleName;
        private System.Windows.Forms.Label labelParentRelation;
        private System.Windows.Forms.ComboBox comboBoxParentRelation;
        private System.Windows.Forms.TabControl tabControlSupplements;
        private System.Windows.Forms.TabPage tabPageEntity;
        private System.Windows.Forms.TabPage tabPageProject;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.ToolStrip toolStripProjects;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectDelete;
        private System.Windows.Forms.ImageList imageListSupplements;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectList;
        private System.Windows.Forms.ToolStripButton toolStripButtonTableEditor;
    }
}