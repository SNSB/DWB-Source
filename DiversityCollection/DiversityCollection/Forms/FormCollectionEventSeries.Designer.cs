namespace DiversityCollection.Forms
{
    partial class FormCollectionEventSeries
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCollectionEventSeries));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.userControlDialogPanel = new DiversityWorkbench.UserControlDialogPanel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.userControlQueryList = new DiversityWorkbench.UserControlQueryList();
            this.splitContainerData = new System.Windows.Forms.SplitContainer();
            this.groupBoxEventSeries = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelCollectionEventSeries = new System.Windows.Forms.TableLayoutPanel();
            this.treeViewCollection = new System.Windows.Forms.TreeView();
            this.toolStripCollection = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSpecimenList = new System.Windows.Forms.ToolStripButton();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelLocation = new System.Windows.Forms.Label();
            this.textBoxLocation = new System.Windows.Forms.TextBox();
            this.labelAdministrativeContactName = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryAdministrativeContactName = new DiversityWorkbench.UserControlModuleRelatedEntry();
            this.textBoxCollectionAcronym = new System.Windows.Forms.TextBox();
            this.labelCollectionOwner = new System.Windows.Forms.Label();
            this.textBoxCollectionOwner = new System.Windows.Forms.TextBox();
            this.labelDisplayOrder = new System.Windows.Forms.Label();
            this.textBoxDisplayOrder = new System.Windows.Forms.TextBox();
            this.userControlSpecimenList = new DiversityCollection.UserControlSpecimenList();
            this.tableLayoutPanelHierarchy = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainerEventSeries = new System.Windows.Forms.SplitContainer();
            this.splitContainerHierarchy = new System.Windows.Forms.SplitContainer();
            this.panelHeader.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.splitContainerData.Panel1.SuspendLayout();
            this.splitContainerData.Panel2.SuspendLayout();
            this.splitContainerData.SuspendLayout();
            this.groupBoxEventSeries.SuspendLayout();
            this.tableLayoutPanelCollectionEventSeries.SuspendLayout();
            this.toolStripCollection.SuspendLayout();
            this.tableLayoutPanelHierarchy.SuspendLayout();
            this.splitContainerEventSeries.Panel1.SuspendLayout();
            this.splitContainerEventSeries.SuspendLayout();
            this.splitContainerHierarchy.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(894, 24);
            this.panelHeader.TabIndex = 7;
            this.panelHeader.Visible = false;
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(0, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(894, 24);
            this.labelHeader.TabIndex = 3;
            this.labelHeader.Text = "Select a collection";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 506);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(894, 27);
            this.userControlDialogPanel.TabIndex = 8;
            this.userControlDialogPanel.Visible = false;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 24);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.userControlQueryList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerData);
            this.splitContainerMain.Size = new System.Drawing.Size(894, 482);
            this.splitContainerMain.SplitterDistance = 297;
            this.splitContainerMain.TabIndex = 9;
            // 
            // userControlQueryList
            // 
            this.userControlQueryList.Connection = null;
            this.userControlQueryList.DisplayTextSelectedItem = "";
            this.userControlQueryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlQueryList.IsPredefinedQuery = false;
            this.userControlQueryList.Location = new System.Drawing.Point(0, 0);
            this.userControlQueryList.MaximalNumberOfResults = 100;
            this.userControlQueryList.Name = "userControlQueryList";
            this.userControlQueryList.ProjectID = 0;
            this.userControlQueryList.QueryConditionVisiblity = "";
            this.userControlQueryList.QueryRestriction = "";
            this.userControlQueryList.Size = new System.Drawing.Size(297, 482);
            this.userControlQueryList.TabIndex = 2;
            this.userControlQueryList.WhereClause = null;
            // 
            // splitContainerData
            // 
            this.splitContainerData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerData.Location = new System.Drawing.Point(0, 0);
            this.splitContainerData.Name = "splitContainerData";
            // 
            // splitContainerData.Panel1
            // 
            this.splitContainerData.Panel1.Controls.Add(this.groupBoxEventSeries);
            // 
            // splitContainerData.Panel2
            // 
            this.splitContainerData.Panel2.Controls.Add(this.userControlSpecimenList);
            this.splitContainerData.Size = new System.Drawing.Size(593, 482);
            this.splitContainerData.SplitterDistance = 404;
            this.splitContainerData.TabIndex = 2;
            // 
            // groupBoxEventSeries
            // 
            this.groupBoxEventSeries.Controls.Add(this.splitContainerHierarchy);
            this.groupBoxEventSeries.Controls.Add(this.splitContainerEventSeries);
            this.groupBoxEventSeries.Controls.Add(this.tableLayoutPanelHierarchy);
            this.groupBoxEventSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxEventSeries.Location = new System.Drawing.Point(0, 0);
            this.groupBoxEventSeries.Name = "groupBoxEventSeries";
            this.groupBoxEventSeries.Size = new System.Drawing.Size(404, 482);
            this.groupBoxEventSeries.TabIndex = 1;
            this.groupBoxEventSeries.TabStop = false;
            this.groupBoxEventSeries.Text = "Collection event series";
            // 
            // tableLayoutPanelCollectionEventSeries
            // 
            this.tableLayoutPanelCollectionEventSeries.ColumnCount = 4;
            this.tableLayoutPanelCollectionEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollectionEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCollectionEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelCollectionEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.labelName, 0, 1);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.textBoxName, 1, 1);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.labelDescription, 0, 3);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.textBoxDescription, 1, 3);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.labelLocation, 0, 4);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.textBoxLocation, 1, 4);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.labelAdministrativeContactName, 0, 2);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.userControlModuleRelatedEntryAdministrativeContactName, 1, 2);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.textBoxCollectionAcronym, 2, 1);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.labelCollectionOwner, 0, 5);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.textBoxCollectionOwner, 1, 5);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.labelDisplayOrder, 0, 6);
            this.tableLayoutPanelCollectionEventSeries.Controls.Add(this.textBoxDisplayOrder, 1, 6);
            this.tableLayoutPanelCollectionEventSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCollectionEventSeries.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelCollectionEventSeries.Name = "tableLayoutPanelCollectionEventSeries";
            this.tableLayoutPanelCollectionEventSeries.RowCount = 7;
            this.tableLayoutPanelCollectionEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.5F));
            this.tableLayoutPanelCollectionEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollectionEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollectionEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanelCollectionEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollectionEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollectionEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollectionEventSeries.Size = new System.Drawing.Size(50, 100);
            this.tableLayoutPanelCollectionEventSeries.TabIndex = 0;
            // 
            // treeViewCollection
            // 
            this.treeViewCollection.AllowDrop = true;
            this.treeViewCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCollection.Location = new System.Drawing.Point(3, 3);
            this.treeViewCollection.Name = "treeViewCollection";
            this.treeViewCollection.Size = new System.Drawing.Size(368, 131);
            this.treeViewCollection.TabIndex = 0;
            // 
            // toolStripCollection
            // 
            this.toolStripCollection.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripCollection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonDelete,
            this.toolStripButtonSpecimenList});
            this.toolStripCollection.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripCollection.Location = new System.Drawing.Point(374, 0);
            this.toolStripCollection.Name = "toolStripCollection";
            this.toolStripCollection.Size = new System.Drawing.Size(24, 137);
            this.toolStripCollection.TabIndex = 1;
            this.toolStripCollection.Text = "toolStrip1";
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNew.Image = global::DiversityCollection.Resource.New1;
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonNew.Text = "Insert a new analysis dependent on the selected analysis";
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonDelete.Text = "delete the selected analysis";
            // 
            // toolStripButtonSpecimenList
            // 
            this.toolStripButtonSpecimenList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSpecimenList.Image = global::DiversityCollection.Resource.List;
            this.toolStripButtonSpecimenList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSpecimenList.Name = "toolStripButtonSpecimenList";
            this.toolStripButtonSpecimenList.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonSpecimenList.Text = "Show specimen list";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelName.Location = new System.Drawing.Point(3, -19);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(71, 26);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "Name:";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxName
            // 
            this.textBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxName.Location = new System.Drawing.Point(80, -16);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(1, 20);
            this.textBoxName.TabIndex = 3;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(3, 34);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(71, 1);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description:";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(80, 37);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(1, 1);
            this.textBoxDescription.TabIndex = 5;
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocation.Location = new System.Drawing.Point(3, 23);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(71, 26);
            this.labelLocation.TabIndex = 6;
            this.labelLocation.Text = "Location:";
            this.labelLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLocation
            // 
            this.textBoxLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocation.Location = new System.Drawing.Point(80, 26);
            this.textBoxLocation.Name = "textBoxLocation";
            this.textBoxLocation.Size = new System.Drawing.Size(1, 20);
            this.textBoxLocation.TabIndex = 7;
            // 
            // labelAdministrativeContactName
            // 
            this.labelAdministrativeContactName.AutoSize = true;
            this.labelAdministrativeContactName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAdministrativeContactName.Location = new System.Drawing.Point(3, 7);
            this.labelAdministrativeContactName.Name = "labelAdministrativeContactName";
            this.labelAdministrativeContactName.Size = new System.Drawing.Size(71, 27);
            this.labelAdministrativeContactName.TabIndex = 11;
            this.labelAdministrativeContactName.Text = "Contact:";
            this.labelAdministrativeContactName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryAdministrativeContactName
            // 
            this.tableLayoutPanelCollectionEventSeries.SetColumnSpan(this.userControlModuleRelatedEntryAdministrativeContactName, 3);
            this.userControlModuleRelatedEntryAdministrativeContactName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryAdministrativeContactName.Location = new System.Drawing.Point(80, 10);
            this.userControlModuleRelatedEntryAdministrativeContactName.Module = null;
            this.userControlModuleRelatedEntryAdministrativeContactName.Name = "userControlModuleRelatedEntryAdministrativeContactName";
            this.userControlModuleRelatedEntryAdministrativeContactName.Size = new System.Drawing.Size(1, 21);
            this.userControlModuleRelatedEntryAdministrativeContactName.TabIndex = 12;
            // 
            // textBoxCollectionAcronym
            // 
            this.tableLayoutPanelCollectionEventSeries.SetColumnSpan(this.textBoxCollectionAcronym, 2);
            this.textBoxCollectionAcronym.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionAcronym.Location = new System.Drawing.Point(10, -16);
            this.textBoxCollectionAcronym.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxCollectionAcronym.Name = "textBoxCollectionAcronym";
            this.textBoxCollectionAcronym.Size = new System.Drawing.Size(37, 20);
            this.textBoxCollectionAcronym.TabIndex = 13;
            // 
            // labelCollectionOwner
            // 
            this.labelCollectionOwner.AutoSize = true;
            this.labelCollectionOwner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionOwner.Location = new System.Drawing.Point(3, 49);
            this.labelCollectionOwner.Name = "labelCollectionOwner";
            this.labelCollectionOwner.Size = new System.Drawing.Size(71, 26);
            this.labelCollectionOwner.TabIndex = 14;
            this.labelCollectionOwner.Text = "Owner:";
            this.labelCollectionOwner.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCollectionOwner
            // 
            this.tableLayoutPanelCollectionEventSeries.SetColumnSpan(this.textBoxCollectionOwner, 3);
            this.textBoxCollectionOwner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionOwner.Location = new System.Drawing.Point(80, 52);
            this.textBoxCollectionOwner.Name = "textBoxCollectionOwner";
            this.textBoxCollectionOwner.Size = new System.Drawing.Size(1, 20);
            this.textBoxCollectionOwner.TabIndex = 15;
            // 
            // labelDisplayOrder
            // 
            this.labelDisplayOrder.AutoSize = true;
            this.labelDisplayOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayOrder.Location = new System.Drawing.Point(3, 75);
            this.labelDisplayOrder.Name = "labelDisplayOrder";
            this.labelDisplayOrder.Size = new System.Drawing.Size(71, 26);
            this.labelDisplayOrder.TabIndex = 16;
            this.labelDisplayOrder.Text = "Display order:";
            this.labelDisplayOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDisplayOrder
            // 
            this.tableLayoutPanelCollectionEventSeries.SetColumnSpan(this.textBoxDisplayOrder, 3);
            this.textBoxDisplayOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDisplayOrder.Location = new System.Drawing.Point(80, 78);
            this.textBoxDisplayOrder.Name = "textBoxDisplayOrder";
            this.textBoxDisplayOrder.Size = new System.Drawing.Size(1, 20);
            this.textBoxDisplayOrder.TabIndex = 17;
            // 
            // userControlSpecimenList
            // 
            this.userControlSpecimenList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSpecimenList.Location = new System.Drawing.Point(0, 0);
            this.userControlSpecimenList.Name = "userControlSpecimenList";
            this.userControlSpecimenList.Size = new System.Drawing.Size(185, 482);
            this.userControlSpecimenList.TabIndex = 0;
            // 
            // tableLayoutPanelHierarchy
            // 
            this.tableLayoutPanelHierarchy.ColumnCount = 2;
            this.tableLayoutPanelHierarchy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHierarchy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHierarchy.Controls.Add(this.toolStripCollection, 1, 0);
            this.tableLayoutPanelHierarchy.Controls.Add(this.treeViewCollection, 0, 0);
            this.tableLayoutPanelHierarchy.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelHierarchy.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelHierarchy.Name = "tableLayoutPanelHierarchy";
            this.tableLayoutPanelHierarchy.RowCount = 1;
            this.tableLayoutPanelHierarchy.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHierarchy.Size = new System.Drawing.Size(398, 137);
            this.tableLayoutPanelHierarchy.TabIndex = 1;
            // 
            // splitContainerEventSeries
            // 
            this.splitContainerEventSeries.Location = new System.Drawing.Point(49, 208);
            this.splitContainerEventSeries.Name = "splitContainerEventSeries";
            // 
            // splitContainerEventSeries.Panel1
            // 
            this.splitContainerEventSeries.Panel1.Controls.Add(this.tableLayoutPanelCollectionEventSeries);
            this.splitContainerEventSeries.Size = new System.Drawing.Size(150, 100);
            this.splitContainerEventSeries.TabIndex = 2;
            // 
            // splitContainerHierarchy
            // 
            this.splitContainerHierarchy.Location = new System.Drawing.Point(270, 220);
            this.splitContainerHierarchy.Name = "splitContainerHierarchy";
            this.splitContainerHierarchy.Size = new System.Drawing.Size(150, 100);
            this.splitContainerHierarchy.TabIndex = 3;
            // 
            // FormCollectionEventSeries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 533);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Controls.Add(this.panelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCollectionEventSeries";
            this.Text = "Collection event series";
            this.TopMost = true;
            this.panelHeader.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerData.Panel1.ResumeLayout(false);
            this.splitContainerData.Panel2.ResumeLayout(false);
            this.splitContainerData.ResumeLayout(false);
            this.groupBoxEventSeries.ResumeLayout(false);
            this.tableLayoutPanelCollectionEventSeries.ResumeLayout(false);
            this.tableLayoutPanelCollectionEventSeries.PerformLayout();
            this.toolStripCollection.ResumeLayout(false);
            this.toolStripCollection.PerformLayout();
            this.tableLayoutPanelHierarchy.ResumeLayout(false);
            this.tableLayoutPanelHierarchy.PerformLayout();
            this.splitContainerEventSeries.Panel1.ResumeLayout(false);
            this.splitContainerEventSeries.ResumeLayout(false);
            this.splitContainerHierarchy.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelHeader;
        private DiversityWorkbench.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private DiversityWorkbench.UserControlQueryList userControlQueryList;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.GroupBox groupBoxEventSeries;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCollectionEventSeries;
        private System.Windows.Forms.TreeView treeViewCollection;
        private System.Windows.Forms.ToolStrip toolStripCollection;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonSpecimenList;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.TextBox textBoxLocation;
        private System.Windows.Forms.Label labelAdministrativeContactName;
        private DiversityWorkbench.UserControlModuleRelatedEntry userControlModuleRelatedEntryAdministrativeContactName;
        private System.Windows.Forms.TextBox textBoxCollectionAcronym;
        private System.Windows.Forms.Label labelCollectionOwner;
        private System.Windows.Forms.TextBox textBoxCollectionOwner;
        private System.Windows.Forms.Label labelDisplayOrder;
        private System.Windows.Forms.TextBox textBoxDisplayOrder;
        private UserControlSpecimenList userControlSpecimenList;
        private System.Windows.Forms.SplitContainer splitContainerHierarchy;
        private System.Windows.Forms.SplitContainer splitContainerEventSeries;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHierarchy;
    }
}