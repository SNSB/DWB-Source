namespace DiversityWorkbench.Forms
{
    partial class FormTemplateEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTemplateEditor));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.panelColumns = new System.Windows.Forms.Panel();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.buttonCheckNone = new System.Windows.Forms.Button();
            this.buttonCheckAll = new System.Windows.Forms.Button();
            this.toolStripCopy = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSaveTemplate = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonCopy = new System.Windows.Forms.ToolStripDropDownButton();
            this.copyAllValuesIntoTheTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonClearTemplate = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonCopyModeForTemplate = new System.Windows.Forms.ToolStripDropDownButton();
            this.copyOnlyInEmptyFieldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.askIfFieldsContainDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyWholeTemplateContentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListTemplate = new System.Windows.Forms.ImageList(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripButtonFeedback = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel.SuspendLayout();
            this.toolStripCopy.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.panelColumns, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.userControlDialogPanel, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonCheckNone, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonCheckAll, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.toolStripCopy, 3, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(555, 412);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.Location = new System.Drawing.Point(3, 6);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(314, 17);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "label1";
            // 
            // panelColumns
            // 
            this.panelColumns.AutoScroll = true;
            this.tableLayoutPanel.SetColumnSpan(this.panelColumns, 4);
            this.panelColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelColumns.Location = new System.Drawing.Point(3, 32);
            this.panelColumns.Name = "panelColumns";
            this.panelColumns.Size = new System.Drawing.Size(549, 350);
            this.panelColumns.TabIndex = 1;
            // 
            // userControlDialogPanel
            // 
            this.tableLayoutPanel.SetColumnSpan(this.userControlDialogPanel, 4);
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 385);
            this.userControlDialogPanel.Margin = new System.Windows.Forms.Padding(0);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(555, 27);
            this.userControlDialogPanel.TabIndex = 2;
            // 
            // buttonCheckNone
            // 
            this.buttonCheckNone.Image = global::DiversityWorkbench.Properties.Resources.CheckNo;
            this.buttonCheckNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCheckNone.Location = new System.Drawing.Point(369, 3);
            this.buttonCheckNone.Name = "buttonCheckNone";
            this.buttonCheckNone.Size = new System.Drawing.Size(53, 23);
            this.buttonCheckNone.TabIndex = 4;
            this.buttonCheckNone.Text = "none";
            this.buttonCheckNone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonCheckNone, "Select none of the fields");
            this.buttonCheckNone.UseVisualStyleBackColor = true;
            this.buttonCheckNone.Click += new System.EventHandler(this.buttonCheckNone_Click);
            // 
            // buttonCheckAll
            // 
            this.buttonCheckAll.Image = global::DiversityWorkbench.Properties.Resources.CheckYes;
            this.buttonCheckAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCheckAll.Location = new System.Drawing.Point(323, 3);
            this.buttonCheckAll.Name = "buttonCheckAll";
            this.buttonCheckAll.Size = new System.Drawing.Size(40, 23);
            this.buttonCheckAll.TabIndex = 3;
            this.buttonCheckAll.Text = "all";
            this.buttonCheckAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonCheckAll, "Select all fields");
            this.buttonCheckAll.UseVisualStyleBackColor = true;
            this.buttonCheckAll.Click += new System.EventHandler(this.buttonCheckAll_Click);
            // 
            // toolStripCopy
            // 
            this.toolStripCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripCopy.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSaveTemplate,
            this.toolStripDropDownButtonCopy,
            this.toolStripButtonClearTemplate,
            this.toolStripDropDownButtonCopyModeForTemplate,
            this.toolStripButtonFeedback});
            this.toolStripCopy.Location = new System.Drawing.Point(425, 0);
            this.toolStripCopy.Name = "toolStripCopy";
            this.toolStripCopy.Size = new System.Drawing.Size(130, 29);
            this.toolStripCopy.TabIndex = 5;
            this.toolStripCopy.Text = "Set the option for filling data with values from the template";
            // 
            // toolStripButtonSaveTemplate
            // 
            this.toolStripButtonSaveTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveTemplate.Image = global::DiversityWorkbench.ResourceWorkbench.Save;
            this.toolStripButtonSaveTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveTemplate.Name = "toolStripButtonSaveTemplate";
            this.toolStripButtonSaveTemplate.Size = new System.Drawing.Size(23, 26);
            this.toolStripButtonSaveTemplate.Text = "Save the changes to the template";
            this.toolStripButtonSaveTemplate.Click += new System.EventHandler(this.toolStripButtonSaveTemplate_Click);
            // 
            // toolStripDropDownButtonCopy
            // 
            this.toolStripDropDownButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonCopy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyAllValuesIntoTheTemplateToolStripMenuItem,
            this.copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem,
            this.copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem});
            this.toolStripDropDownButtonCopy.Image = global::DiversityWorkbench.ResourceWorkbench.Copy;
            this.toolStripDropDownButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonCopy.Name = "toolStripDropDownButtonCopy";
            this.toolStripDropDownButtonCopy.Size = new System.Drawing.Size(29, 26);
            this.toolStripDropDownButtonCopy.Text = "Copy values from the data into the template";
            // 
            // copyAllValuesIntoTheTemplateToolStripMenuItem
            // 
            this.copyAllValuesIntoTheTemplateToolStripMenuItem.Name = "copyAllValuesIntoTheTemplateToolStripMenuItem";
            this.copyAllValuesIntoTheTemplateToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.copyAllValuesIntoTheTemplateToolStripMenuItem.Text = "Copy all values into the template";
            this.copyAllValuesIntoTheTemplateToolStripMenuItem.ToolTipText = "This copies all values from the data into the template and removes current values" +
    " from the template";
            this.copyAllValuesIntoTheTemplateToolStripMenuItem.Click += new System.EventHandler(this.copyAllValuesIntoTheTemplateToolStripMenuItem_Click);
            // 
            // copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem
            // 
            this.copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem.Name = "copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem";
            this.copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem.Text = "Copy only not empty values into the template";
            this.copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem.ToolTipText = "This replaces current values in the template with values from the data where valu" +
    "es in the data do exist";
            this.copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem.Click += new System.EventHandler(this.copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem_Click);
            // 
            // copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem
            // 
            this.copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem.Name = "copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem";
            this.copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem.Text = "Copy only missing values into the template";
            this.copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem.ToolTipText = "This adds values to the template with values from the data where the template is " +
    "empty";
            this.copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem.Click += new System.EventHandler(this.copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem_Click);
            // 
            // toolStripButtonClearTemplate
            // 
            this.toolStripButtonClearTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonClearTemplate.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonClearTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClearTemplate.Name = "toolStripButtonClearTemplate";
            this.toolStripButtonClearTemplate.Size = new System.Drawing.Size(23, 26);
            this.toolStripButtonClearTemplate.Text = "Removes all values from the template";
            this.toolStripButtonClearTemplate.Click += new System.EventHandler(this.toolStripButtonClearTemplate_Click);
            // 
            // toolStripDropDownButtonCopyModeForTemplate
            // 
            this.toolStripDropDownButtonCopyModeForTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonCopyModeForTemplate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyOnlyInEmptyFieldsToolStripMenuItem,
            this.askIfFieldsContainDataToolStripMenuItem,
            this.copyWholeTemplateContentToolStripMenuItem});
            this.toolStripDropDownButtonCopyModeForTemplate.Image = global::DiversityWorkbench.Properties.Resources.Radiobutton;
            this.toolStripDropDownButtonCopyModeForTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonCopyModeForTemplate.Name = "toolStripDropDownButtonCopyModeForTemplate";
            this.toolStripDropDownButtonCopyModeForTemplate.Size = new System.Drawing.Size(29, 26);
            this.toolStripDropDownButtonCopyModeForTemplate.Text = "Set option for filling data with values from the template";
            // 
            // copyOnlyInEmptyFieldsToolStripMenuItem
            // 
            this.copyOnlyInEmptyFieldsToolStripMenuItem.Name = "copyOnlyInEmptyFieldsToolStripMenuItem";
            this.copyOnlyInEmptyFieldsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.copyOnlyInEmptyFieldsToolStripMenuItem.Text = "Copy only in empty fields";
            this.copyOnlyInEmptyFieldsToolStripMenuItem.ToolTipText = "Copy values from the template only if there are no entries in the data";
            this.copyOnlyInEmptyFieldsToolStripMenuItem.Click += new System.EventHandler(this.copyOnlyInEmptyFieldsToolStripMenuItem_Click);
            // 
            // askIfFieldsContainDataToolStripMenuItem
            // 
            this.askIfFieldsContainDataToolStripMenuItem.Name = "askIfFieldsContainDataToolStripMenuItem";
            this.askIfFieldsContainDataToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.askIfFieldsContainDataToolStripMenuItem.Text = "Ask if fields contain data";
            this.askIfFieldsContainDataToolStripMenuItem.ToolTipText = "Ask the user if there are differing contents in data and template whether the tem" +
    "plate values should be copied into the data";
            this.askIfFieldsContainDataToolStripMenuItem.Click += new System.EventHandler(this.askIfFieldsContainDataToolStripMenuItem_Click);
            // 
            // copyWholeTemplateContentToolStripMenuItem
            // 
            this.copyWholeTemplateContentToolStripMenuItem.Name = "copyWholeTemplateContentToolStripMenuItem";
            this.copyWholeTemplateContentToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.copyWholeTemplateContentToolStripMenuItem.Text = "Copy whole template content";
            this.copyWholeTemplateContentToolStripMenuItem.ToolTipText = "Copy the values from the template irrespective of any content in the data";
            this.copyWholeTemplateContentToolStripMenuItem.Click += new System.EventHandler(this.copyWholeTemplateContentToolStripMenuItem_Click);
            // 
            // imageListTemplate
            // 
            this.imageListTemplate.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTemplate.ImageStream")));
            this.imageListTemplate.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTemplate.Images.SetKeyName(0, "Template.ico");
            this.imageListTemplate.Images.SetKeyName(1, "TemplateEditor.ico");
            this.imageListTemplate.Images.SetKeyName(2, "Radiobutton.ICO");
            // 
            // toolStripButtonFeedback
            // 
            this.toolStripButtonFeedback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.toolStripButtonFeedback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFeedback.Name = "toolStripButtonFeedback";
            this.toolStripButtonFeedback.Size = new System.Drawing.Size(23, 26);
            this.toolStripButtonFeedback.Text = "Send a feedback";
            this.toolStripButtonFeedback.Click += new System.EventHandler(this.toolStripButtonFeedback_Click);
            // 
            // FormTemplateEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 412);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTemplateEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Template editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTemplateEditor_FormClosing);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.toolStripCopy.ResumeLayout(false);
            this.toolStripCopy.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Panel panelColumns;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.ImageList imageListTemplate;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonCheckNone;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonCheckAll;
        private System.Windows.Forms.ToolStrip toolStripCopy;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonCopy;
        private System.Windows.Forms.ToolStripMenuItem copyAllValuesIntoTheTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonClearTemplate;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveTemplate;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonCopyModeForTemplate;
        private System.Windows.Forms.ToolStripMenuItem copyOnlyInEmptyFieldsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem askIfFieldsContainDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyWholeTemplateContentToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonFeedback;
    }
}