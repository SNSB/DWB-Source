namespace DiversityProjects
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.treeViewSettings = new System.Windows.Forms.TreeView();
            this.toolStripSettings = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonParentSet = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonParentRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFeedback = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxSetting = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelDisplayOrder = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxDisplayOrder = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHistory = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxID = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelID = new System.Windows.Forms.ToolStripLabel();
            this.listBoxProjects = new System.Windows.Forms.ListBox();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanelMain.SuspendLayout();
            this.toolStripSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.treeViewSettings, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.toolStripSettings, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxProjects, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(616, 336);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // treeViewSettings
            // 
            this.treeViewSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSettings.Location = new System.Drawing.Point(129, 28);
            this.treeViewSettings.Name = "treeViewSettings";
            this.treeViewSettings.Size = new System.Drawing.Size(484, 305);
            this.treeViewSettings.TabIndex = 0;
            this.treeViewSettings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewSettings_AfterSelect);
            // 
            // toolStripSettings
            // 
            this.toolStripSettings.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripSettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonParentSet,
            this.toolStripButtonParentRemove,
            this.toolStripButtonDelete,
            this.toolStripButtonFeedback,
            this.toolStripTextBoxSetting,
            this.toolStripLabelDisplayOrder,
            this.toolStripTextBoxDisplayOrder,
            this.toolStripButtonSave,
            this.toolStripButtonHistory,
            this.toolStripTextBoxID,
            this.toolStripLabelID});
            this.toolStripSettings.Location = new System.Drawing.Point(126, 0);
            this.toolStripSettings.Name = "toolStripSettings";
            this.toolStripSettings.Size = new System.Drawing.Size(490, 25);
            this.toolStripSettings.TabIndex = 1;
            this.toolStripSettings.Text = "toolStrip1";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAdd.Image = global::DiversityProjects.Properties.Resources.Add;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAdd.Text = "Add setting";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonParentSet
            // 
            this.toolStripButtonParentSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonParentSet.Image = global::DiversityProjects.Properties.Resources.SetParent;
            this.toolStripButtonParentSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonParentSet.Name = "toolStripButtonParentSet";
            this.toolStripButtonParentSet.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonParentSet.Text = "Change the superior setting";
            this.toolStripButtonParentSet.Click += new System.EventHandler(this.toolStripButtonParentSet_Click);
            // 
            // toolStripButtonParentRemove
            // 
            this.toolStripButtonParentRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonParentRemove.Image = global::DiversityProjects.Properties.Resources.RemoveParent;
            this.toolStripButtonParentRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonParentRemove.Name = "toolStripButtonParentRemove";
            this.toolStripButtonParentRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonParentRemove.Text = "Remove the relation to the superior setting";
            this.toolStripButtonParentRemove.Click += new System.EventHandler(this.toolStripButtonParentRemove_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityProjects.Properties.Resources.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDelete.Text = "Remove the selected setting";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonFeedback
            // 
            this.toolStripButtonFeedback.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonFeedback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFeedback.Image = global::DiversityProjects.Properties.Resources.Feedback;
            this.toolStripButtonFeedback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFeedback.Name = "toolStripButtonFeedback";
            this.toolStripButtonFeedback.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFeedback.Text = "Send a feedback";
            this.toolStripButtonFeedback.Click += new System.EventHandler(this.toolStripButtonFeedback_Click);
            // 
            // toolStripTextBoxSetting
            // 
            this.toolStripTextBoxSetting.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBoxSetting.Name = "toolStripTextBoxSetting";
            this.toolStripTextBoxSetting.Size = new System.Drawing.Size(200, 25);
            this.toolStripTextBoxSetting.ToolTipText = "The display text of the setting";
            this.toolStripTextBoxSetting.Leave += new System.EventHandler(this.toolStripTextBoxSetting_Leave);
            // 
            // toolStripLabelDisplayOrder
            // 
            this.toolStripLabelDisplayOrder.Name = "toolStripLabelDisplayOrder";
            this.toolStripLabelDisplayOrder.Size = new System.Drawing.Size(33, 22);
            this.toolStripLabelDisplayOrder.Text = "Ord.:";
            // 
            // toolStripTextBoxDisplayOrder
            // 
            this.toolStripTextBoxDisplayOrder.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBoxDisplayOrder.Name = "toolStripTextBoxDisplayOrder";
            this.toolStripTextBoxDisplayOrder.Size = new System.Drawing.Size(30, 25);
            this.toolStripTextBoxDisplayOrder.ToolTipText = "The sequence of the setting as shown in a user interface";
            this.toolStripTextBoxDisplayOrder.Leave += new System.EventHandler(this.toolStripTextBoxDisplayOrder_Leave);
            this.toolStripTextBoxDisplayOrder.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBoxDisplayOrder_KeyUp);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = global::DiversityProjects.Properties.Resources.Save;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save changes in settings";
            this.toolStripButtonSave.Visible = false;
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonHistory
            // 
            this.toolStripButtonHistory.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonHistory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHistory.Image = global::DiversityProjects.Properties.Resources.History;
            this.toolStripButtonHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHistory.Name = "toolStripButtonHistory";
            this.toolStripButtonHistory.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHistory.Text = "Show the history of the selected setting";
            this.toolStripButtonHistory.Click += new System.EventHandler(this.toolStripButtonHistory_Click);
            // 
            // toolStripTextBoxID
            // 
            this.toolStripTextBoxID.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBoxID.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBoxID.Name = "toolStripTextBoxID";
            this.toolStripTextBoxID.ReadOnly = true;
            this.toolStripTextBoxID.Size = new System.Drawing.Size(40, 25);
            // 
            // toolStripLabelID
            // 
            this.toolStripLabelID.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelID.Name = "toolStripLabelID";
            this.toolStripLabelID.Size = new System.Drawing.Size(21, 22);
            this.toolStripLabelID.Text = "ID:";
            // 
            // listBoxProjects
            // 
            this.listBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjects.FormattingEnabled = true;
            this.listBoxProjects.Location = new System.Drawing.Point(3, 28);
            this.listBoxProjects.Name = "listBoxProjects";
            this.listBoxProjects.Size = new System.Drawing.Size(120, 305);
            this.listBoxProjects.TabIndex = 2;
            this.listBoxProjects.SelectedIndexChanged += new System.EventHandler(this.listBoxProjects_SelectedIndexChanged);
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 336);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(616, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 363);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.toolStripSettings.ResumeLayout(false);
            this.toolStripSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TreeView treeViewSettings;
        private System.Windows.Forms.ToolStrip toolStripSettings;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.ToolStripButton toolStripButtonFeedback;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxSetting;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonParentSet;
        private System.Windows.Forms.ToolStripButton toolStripButtonParentRemove;
        private System.Windows.Forms.ToolStripLabel toolStripLabelDisplayOrder;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxDisplayOrder;
        private System.Windows.Forms.ToolStripButton toolStripButtonHistory;
        private System.Windows.Forms.ToolStripLabel toolStripLabelID;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxID;
    }
}