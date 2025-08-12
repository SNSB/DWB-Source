namespace DiversityCollection.Forms
{
    partial class FormRegulation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegulation));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerQuery = new System.Windows.Forms.SplitContainer();
            this.treeViewRegulation = new System.Windows.Forms.TreeView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSetParent = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveParent = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.listBoxQueryResult = new System.Windows.Forms.ListBox();
            this.regulationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetRegulation = new DiversityCollection.Datasets.DataSetRegulation();
            this.toolStripQuery = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelQuery = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxQuery = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonQuery = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.userControlModuleRelatedEntryProject = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelNotes = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.comboBoxStatus = new System.Windows.Forms.ComboBox();
            this.checkBoxHierarchyOnly = new System.Windows.Forms.CheckBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.regulationTableAdapter = new DiversityCollection.Datasets.DataSetRegulationTableAdapters.RegulationTableAdapter();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.splitContainerQuery.Panel1.SuspendLayout();
            this.splitContainerQuery.Panel2.SuspendLayout();
            this.splitContainerQuery.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.regulationBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetRegulation)).BeginInit();
            this.toolStripQuery.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
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
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerQuery);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanel);
            this.splitContainerMain.Size = new System.Drawing.Size(551, 265);
            this.splitContainerMain.SplitterDistance = 245;
            this.splitContainerMain.TabIndex = 0;
            // 
            // splitContainerQuery
            // 
            this.splitContainerQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerQuery.Location = new System.Drawing.Point(0, 0);
            this.splitContainerQuery.Name = "splitContainerQuery";
            // 
            // splitContainerQuery.Panel1
            // 
            this.splitContainerQuery.Panel1.Controls.Add(this.treeViewRegulation);
            this.splitContainerQuery.Panel1.Controls.Add(this.toolStrip);
            this.splitContainerQuery.Panel1Collapsed = true;
            // 
            // splitContainerQuery.Panel2
            // 
            this.splitContainerQuery.Panel2.Controls.Add(this.listBoxQueryResult);
            this.splitContainerQuery.Panel2.Controls.Add(this.toolStripQuery);
            this.splitContainerQuery.Size = new System.Drawing.Size(245, 265);
            this.splitContainerQuery.SplitterDistance = 116;
            this.splitContainerQuery.TabIndex = 3;
            // 
            // treeViewRegulation
            // 
            this.treeViewRegulation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewRegulation.Location = new System.Drawing.Point(0, 0);
            this.treeViewRegulation.Name = "treeViewRegulation";
            this.treeViewRegulation.Size = new System.Drawing.Size(116, 75);
            this.treeViewRegulation.TabIndex = 2;
            this.treeViewRegulation.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewRegulation_AfterSelect);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonSetParent,
            this.toolStripButtonRemoveParent,
            this.toolStripButtonDelete,
            this.toolStripButtonSave});
            this.toolStrip.Location = new System.Drawing.Point(0, 75);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(116, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAdd.Text = "Add a new regulation";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonSetParent
            // 
            this.toolStripButtonSetParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSetParent.Image = global::DiversityCollection.Resource.SetParent;
            this.toolStripButtonSetParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSetParent.Name = "toolStripButtonSetParent";
            this.toolStripButtonSetParent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSetParent.Text = "Set the parent for the selected regulation";
            this.toolStripButtonSetParent.Click += new System.EventHandler(this.toolStripButtonSetParent_Click);
            // 
            // toolStripButtonRemoveParent
            // 
            this.toolStripButtonRemoveParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveParent.Image = global::DiversityCollection.Resource.RemoveParent;
            this.toolStripButtonRemoveParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveParent.Name = "toolStripButtonRemoveParent";
            this.toolStripButtonRemoveParent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemoveParent.Text = "Remove the parent of the selected regulation";
            this.toolStripButtonRemoveParent.Click += new System.EventHandler(this.toolStripButtonRemoveParent_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDelete.Text = "Delete the selected regulation";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save changes";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // listBoxQueryResult
            // 
            this.listBoxQueryResult.DataSource = this.regulationBindingSource;
            this.listBoxQueryResult.DisplayMember = "Regulation";
            this.listBoxQueryResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxQueryResult.FormattingEnabled = true;
            this.listBoxQueryResult.Location = new System.Drawing.Point(0, 0);
            this.listBoxQueryResult.Name = "listBoxQueryResult";
            this.listBoxQueryResult.Size = new System.Drawing.Size(245, 240);
            this.listBoxQueryResult.TabIndex = 0;
            this.listBoxQueryResult.ValueMember = "Regulation";
            this.listBoxQueryResult.SelectedIndexChanged += new System.EventHandler(this.listBoxQueryResult_SelectedIndexChanged);
            // 
            // regulationBindingSource
            // 
            this.regulationBindingSource.DataMember = "Regulation";
            this.regulationBindingSource.DataSource = this.dataSetRegulation;
            // 
            // dataSetRegulation
            // 
            this.dataSetRegulation.DataSetName = "DataSetRegulation";
            this.dataSetRegulation.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // toolStripQuery
            // 
            this.toolStripQuery.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripQuery.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripQuery.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelQuery,
            this.toolStripTextBoxQuery,
            this.toolStripButtonQuery});
            this.toolStripQuery.Location = new System.Drawing.Point(0, 240);
            this.toolStripQuery.Name = "toolStripQuery";
            this.toolStripQuery.Size = new System.Drawing.Size(245, 25);
            this.toolStripQuery.TabIndex = 1;
            this.toolStripQuery.Text = "toolStrip1";
            // 
            // toolStripLabelQuery
            // 
            this.toolStripLabelQuery.Name = "toolStripLabelQuery";
            this.toolStripLabelQuery.Size = new System.Drawing.Size(63, 22);
            this.toolStripLabelQuery.Text = "Search for:";
            // 
            // toolStripTextBoxQuery
            // 
            this.toolStripTextBoxQuery.Name = "toolStripTextBoxQuery";
            this.toolStripTextBoxQuery.Size = new System.Drawing.Size(100, 25);
            this.toolStripTextBoxQuery.ToolTipText = "Search string for the regulation, use wildcards % or _";
            // 
            // toolStripButtonQuery
            // 
            this.toolStripButtonQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonQuery.Image = global::DiversityCollection.Resource.Search;
            this.toolStripButtonQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonQuery.Name = "toolStripButtonQuery";
            this.toolStripButtonQuery.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonQuery.Text = "Start search";
            this.toolStripButtonQuery.Click += new System.EventHandler(this.toolStripButtonQuery_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.labelType, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.comboBoxType, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.userControlModuleRelatedEntryProject, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelNotes, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxNotes, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.labelStatus, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.comboBoxStatus, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.checkBoxHierarchyOnly, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Enabled = false;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(302, 265);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Location = new System.Drawing.Point(3, 28);
            this.labelType.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(40, 27);
            this.labelType.TabIndex = 2;
            this.labelType.Text = "Type:";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxType
            // 
            this.tableLayoutPanel.SetColumnSpan(this.comboBoxType, 3);
            this.comboBoxType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.regulationBindingSource, "Type", true));
            this.comboBoxType.DisplayMember = "DisplayText";
            this.comboBoxType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(43, 31);
            this.comboBoxType.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(256, 21);
            this.comboBoxType.TabIndex = 3;
            this.toolTip.SetToolTip(this.comboBoxType, "The type of the regulation");
            this.comboBoxType.ValueMember = "Code";
            // 
            // userControlModuleRelatedEntryProject
            // 
            this.userControlModuleRelatedEntryProject.CanDeleteConnectionToModule = true;
            this.tableLayoutPanel.SetColumnSpan(this.userControlModuleRelatedEntryProject, 4);
            this.userControlModuleRelatedEntryProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryProject.Domain = "";
            this.userControlModuleRelatedEntryProject.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryProject.Location = new System.Drawing.Point(3, 3);
            this.userControlModuleRelatedEntryProject.Module = null;
            this.userControlModuleRelatedEntryProject.Name = "userControlModuleRelatedEntryProject";
            this.userControlModuleRelatedEntryProject.ShowInfo = false;
            this.userControlModuleRelatedEntryProject.Size = new System.Drawing.Size(296, 22);
            this.userControlModuleRelatedEntryProject.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryProject.TabIndex = 4;
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelNotes, 4);
            this.labelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNotes.Location = new System.Drawing.Point(3, 105);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(296, 13);
            this.labelNotes.TabIndex = 5;
            this.labelNotes.Text = "Notes";
            // 
            // textBoxNotes
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxNotes, 4);
            this.textBoxNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.regulationBindingSource, "Notes", true));
            this.textBoxNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNotes.Location = new System.Drawing.Point(3, 121);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.Size = new System.Drawing.Size(296, 141);
            this.textBoxNotes.TabIndex = 6;
            this.toolTip.SetToolTip(this.textBoxNotes, "Notes about the regulation");
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatus.Location = new System.Drawing.Point(3, 55);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(40, 27);
            this.labelStatus.TabIndex = 8;
            this.labelStatus.Text = "Status:";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.labelStatus, "The status of the regulation");
            // 
            // comboBoxStatus
            // 
            this.tableLayoutPanel.SetColumnSpan(this.comboBoxStatus, 3);
            this.comboBoxStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.regulationBindingSource, "Status", true));
            this.comboBoxStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStatus.FormattingEnabled = true;
            this.comboBoxStatus.Location = new System.Drawing.Point(43, 58);
            this.comboBoxStatus.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxStatus.Name = "comboBoxStatus";
            this.comboBoxStatus.Size = new System.Drawing.Size(256, 21);
            this.comboBoxStatus.TabIndex = 9;
            this.comboBoxStatus.DropDown += new System.EventHandler(this.comboBoxStatus_DropDown);
            // 
            // checkBoxHierarchyOnly
            // 
            this.checkBoxHierarchyOnly.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.checkBoxHierarchyOnly, 4);
            this.checkBoxHierarchyOnly.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.regulationBindingSource, "HierarchyOnly", true));
            this.checkBoxHierarchyOnly.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxHierarchyOnly.Location = new System.Drawing.Point(3, 85);
            this.checkBoxHierarchyOnly.Name = "checkBoxHierarchyOnly";
            this.checkBoxHierarchyOnly.Size = new System.Drawing.Size(126, 17);
            this.checkBoxHierarchyOnly.TabIndex = 10;
            this.checkBoxHierarchyOnly.Text = "Only for the hierarchy";
            this.checkBoxHierarchyOnly.ThreeState = true;
            this.toolTip.SetToolTip(this.checkBoxHierarchyOnly, "If the entry is only for hierarchy");
            this.checkBoxHierarchyOnly.UseVisualStyleBackColor = true;
            // 
            // regulationTableAdapter
            // 
            this.regulationTableAdapter.ClearBeforeFill = true;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 265);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(551, 27);
            this.userControlDialogPanel.TabIndex = 1;
            this.userControlDialogPanel.Visible = false;
            // 
            // FormRegulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 292);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormRegulation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Regulations";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRegulation_FormClosing);
            this.Load += new System.EventHandler(this.FormRegulation_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerQuery.Panel1.ResumeLayout(false);
            this.splitContainerQuery.Panel1.PerformLayout();
            this.splitContainerQuery.Panel2.ResumeLayout(false);
            this.splitContainerQuery.Panel2.PerformLayout();
            this.splitContainerQuery.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.regulationBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetRegulation)).EndInit();
            this.toolStripQuery.ResumeLayout(false);
            this.toolStripQuery.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private Datasets.DataSetRegulation dataSetRegulation;
        private System.Windows.Forms.BindingSource regulationBindingSource;
        private Datasets.DataSetRegulationTableAdapters.RegulationTableAdapter regulationTableAdapter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox comboBoxType;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryProject;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.TreeView treeViewRegulation;
        private System.Windows.Forms.ToolStripButton toolStripButtonSetParent;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveParent;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.SplitContainer splitContainerQuery;
        private System.Windows.Forms.ListBox listBoxQueryResult;
        private System.Windows.Forms.ToolStrip toolStripQuery;
        private System.Windows.Forms.ToolStripLabel toolStripLabelQuery;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxQuery;
        private System.Windows.Forms.ToolStripButton toolStripButtonQuery;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ComboBox comboBoxStatus;
        private System.Windows.Forms.CheckBox checkBoxHierarchyOnly;
    }
}