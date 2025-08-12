namespace DiversityWorkbench.Spreadsheet
{
    partial class FormColumnFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormColumnFilter));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.comboBoxFilterOperator = new System.Windows.Forms.ComboBox();
            this.buttonOrderBy = new System.Windows.Forms.Button();
            this.labelSorter = new System.Windows.Forms.Label();
            this.labelOperator = new System.Windows.Forms.Label();
            this.tableLayoutPanelFilter = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainerFilter = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelModuleInfo = new System.Windows.Forms.TableLayoutPanel();
            this.labelModuleInfo = new System.Windows.Forms.Label();
            this.buttonRemoveModuleFilter = new System.Windows.Forms.Button();
            this.textBoxModuleInfo = new System.Windows.Forms.TextBox();
            this.buttonModuleFilterEdit = new System.Windows.Forms.Button();
            this.labelModuleInfoOperator = new System.Windows.Forms.Label();
            this.labelModuleInfoSourceHeader = new System.Windows.Forms.Label();
            this.labelModuleInfoSource = new System.Windows.Forms.Label();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanelFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFilter)).BeginInit();
            this.splitContainerFilter.Panel1.SuspendLayout();
            this.splitContainerFilter.Panel2.SuspendLayout();
            this.splitContainerFilter.SuspendLayout();
            this.tableLayoutPanelModuleInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxFilterOperator, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonOrderBy, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelSorter, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelOperator, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelFilter, 3, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(451, 167);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 3);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(23, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(425, 13);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "label1";
            // 
            // comboBoxFilterOperator
            // 
            this.comboBoxFilterOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilterOperator.DropDownWidth = 300;
            this.comboBoxFilterOperator.FormattingEnabled = true;
            this.comboBoxFilterOperator.Location = new System.Drawing.Point(23, 16);
            this.comboBoxFilterOperator.Name = "comboBoxFilterOperator";
            this.comboBoxFilterOperator.Size = new System.Drawing.Size(18, 21);
            this.comboBoxFilterOperator.TabIndex = 1;
            this.comboBoxFilterOperator.DropDown += new System.EventHandler(this.comboBoxFilterOperator_DropDown);
            this.comboBoxFilterOperator.SelectionChangeCommitted += new System.EventHandler(this.comboBoxFilterOperator_SelectionChangeCommitted);
            // 
            // buttonOrderBy
            // 
            this.buttonOrderBy.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.buttonOrderBy.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonOrderBy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOrderBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOrderBy.Location = new System.Drawing.Point(1, 15);
            this.buttonOrderBy.Margin = new System.Windows.Forms.Padding(1, 2, 0, 3);
            this.buttonOrderBy.Name = "buttonOrderBy";
            this.buttonOrderBy.Size = new System.Drawing.Size(19, 22);
            this.buttonOrderBy.TabIndex = 3;
            this.buttonOrderBy.Text = "-";
            this.buttonOrderBy.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip.SetToolTip(this.buttonOrderBy, "Use this column to sort the result");
            this.buttonOrderBy.UseVisualStyleBackColor = false;
            this.buttonOrderBy.Click += new System.EventHandler(this.buttonOrderBy_Click);
            // 
            // labelSorter
            // 
            this.labelSorter.AutoSize = true;
            this.labelSorter.Location = new System.Drawing.Point(1, 0);
            this.labelSorter.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.labelSorter.Name = "labelSorter";
            this.labelSorter.Size = new System.Drawing.Size(19, 13);
            this.labelSorter.TabIndex = 4;
            this.labelSorter.Text = "↑↓";
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOperator.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOperator.Location = new System.Drawing.Point(47, 13);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.labelOperator.Size = new System.Drawing.Size(10, 154);
            this.labelOperator.TabIndex = 5;
            this.labelOperator.Text = "-";
            this.labelOperator.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelOperator.UseCompatibleTextRendering = true;
            // 
            // tableLayoutPanelFilter
            // 
            this.tableLayoutPanelFilter.ColumnCount = 2;
            this.tableLayoutPanelFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelFilter.Controls.Add(this.textBoxFilter, 0, 1);
            this.tableLayoutPanelFilter.Controls.Add(this.linkLabel, 0, 0);
            this.tableLayoutPanelFilter.Controls.Add(this.buttonAdd, 1, 1);
            this.tableLayoutPanelFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelFilter.Location = new System.Drawing.Point(60, 13);
            this.tableLayoutPanelFilter.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelFilter.Name = "tableLayoutPanelFilter";
            this.tableLayoutPanelFilter.RowCount = 2;
            this.tableLayoutPanelFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFilter.Size = new System.Drawing.Size(391, 154);
            this.tableLayoutPanelFilter.TabIndex = 6;
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFilter.Location = new System.Drawing.Point(0, 16);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(0, 3, 1, 3);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFilter.Size = new System.Drawing.Size(372, 20);
            this.textBoxFilter.TabIndex = 2;
            this.textBoxFilter.Click += new System.EventHandler(this.textBoxFilter_Click);
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // linkLabel
            // 
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(3, 0);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(23, 13);
            this.linkLabel.TabIndex = 3;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "link";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonAdd.FlatAppearance.BorderSize = 0;
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonAdd.Location = new System.Drawing.Point(373, 13);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(16, 23);
            this.buttonAdd.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonAdd, "Add filter value from list");
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Visible = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // splitContainerFilter
            // 
            this.splitContainerFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerFilter.Location = new System.Drawing.Point(0, 0);
            this.splitContainerFilter.Name = "splitContainerFilter";
            // 
            // splitContainerFilter.Panel1
            // 
            this.splitContainerFilter.Panel1.Controls.Add(this.tableLayoutPanel);
            // 
            // splitContainerFilter.Panel2
            // 
            this.splitContainerFilter.Panel2.Controls.Add(this.tableLayoutPanelModuleInfo);
            this.splitContainerFilter.Panel2Collapsed = true;
            this.splitContainerFilter.Size = new System.Drawing.Size(451, 167);
            this.splitContainerFilter.SplitterDistance = 150;
            this.splitContainerFilter.TabIndex = 2;
            // 
            // tableLayoutPanelModuleInfo
            // 
            this.tableLayoutPanelModuleInfo.ColumnCount = 2;
            this.tableLayoutPanelModuleInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelModuleInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelModuleInfo.Controls.Add(this.labelModuleInfo, 0, 0);
            this.tableLayoutPanelModuleInfo.Controls.Add(this.buttonRemoveModuleFilter, 1, 2);
            this.tableLayoutPanelModuleInfo.Controls.Add(this.textBoxModuleInfo, 0, 1);
            this.tableLayoutPanelModuleInfo.Controls.Add(this.buttonModuleFilterEdit, 1, 1);
            this.tableLayoutPanelModuleInfo.Controls.Add(this.labelModuleInfoOperator, 1, 0);
            this.tableLayoutPanelModuleInfo.Controls.Add(this.labelModuleInfoSourceHeader, 0, 3);
            this.tableLayoutPanelModuleInfo.Controls.Add(this.labelModuleInfoSource, 0, 4);
            this.tableLayoutPanelModuleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelModuleInfo.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelModuleInfo.Name = "tableLayoutPanelModuleInfo";
            this.tableLayoutPanelModuleInfo.RowCount = 5;
            this.tableLayoutPanelModuleInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelModuleInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelModuleInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelModuleInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelModuleInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelModuleInfo.Size = new System.Drawing.Size(96, 100);
            this.tableLayoutPanelModuleInfo.TabIndex = 0;
            // 
            // labelModuleInfo
            // 
            this.labelModuleInfo.AutoSize = true;
            this.labelModuleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModuleInfo.Location = new System.Drawing.Point(3, 0);
            this.labelModuleInfo.Name = "labelModuleInfo";
            this.labelModuleInfo.Size = new System.Drawing.Size(44, 26);
            this.labelModuleInfo.TabIndex = 0;
            this.labelModuleInfo.Text = "Current filter";
            // 
            // buttonRemoveModuleFilter
            // 
            this.buttonRemoveModuleFilter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonRemoveModuleFilter.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonRemoveModuleFilter.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonRemoveModuleFilter.Location = new System.Drawing.Point(53, 75);
            this.buttonRemoveModuleFilter.Name = "buttonRemoveModuleFilter";
            this.buttonRemoveModuleFilter.Size = new System.Drawing.Size(40, 1);
            this.buttonRemoveModuleFilter.TabIndex = 1;
            this.buttonRemoveModuleFilter.Text = "Clear filter";
            this.buttonRemoveModuleFilter.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonRemoveModuleFilter.UseVisualStyleBackColor = true;
            this.buttonRemoveModuleFilter.Click += new System.EventHandler(this.buttonRemoveModuleFilter_Click);
            // 
            // textBoxModuleInfo
            // 
            this.textBoxModuleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxModuleInfo.Location = new System.Drawing.Point(3, 29);
            this.textBoxModuleInfo.Multiline = true;
            this.textBoxModuleInfo.Name = "textBoxModuleInfo";
            this.textBoxModuleInfo.ReadOnly = true;
            this.tableLayoutPanelModuleInfo.SetRowSpan(this.textBoxModuleInfo, 2);
            this.textBoxModuleInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxModuleInfo.Size = new System.Drawing.Size(44, 42);
            this.textBoxModuleInfo.TabIndex = 2;
            // 
            // buttonModuleFilterEdit
            // 
            this.buttonModuleFilterEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonModuleFilterEdit.Image = global::DiversityWorkbench.Properties.Resources.Edit;
            this.buttonModuleFilterEdit.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonModuleFilterEdit.Location = new System.Drawing.Point(53, 29);
            this.buttonModuleFilterEdit.Name = "buttonModuleFilterEdit";
            this.buttonModuleFilterEdit.Size = new System.Drawing.Size(40, 40);
            this.buttonModuleFilterEdit.TabIndex = 3;
            this.buttonModuleFilterEdit.Text = "Edit";
            this.buttonModuleFilterEdit.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonModuleFilterEdit.UseVisualStyleBackColor = true;
            this.buttonModuleFilterEdit.Click += new System.EventHandler(this.buttonModuleFilterEdit_Click);
            // 
            // labelModuleInfoOperator
            // 
            this.labelModuleInfoOperator.AutoSize = true;
            this.labelModuleInfoOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModuleInfoOperator.Location = new System.Drawing.Point(53, 0);
            this.labelModuleInfoOperator.Name = "labelModuleInfoOperator";
            this.labelModuleInfoOperator.Size = new System.Drawing.Size(40, 26);
            this.labelModuleInfoOperator.TabIndex = 4;
            this.labelModuleInfoOperator.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelModuleInfoSourceHeader
            // 
            this.labelModuleInfoSourceHeader.AutoSize = true;
            this.labelModuleInfoSourceHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModuleInfoSourceHeader.Location = new System.Drawing.Point(3, 74);
            this.labelModuleInfoSourceHeader.Name = "labelModuleInfoSourceHeader";
            this.labelModuleInfoSourceHeader.Size = new System.Drawing.Size(44, 13);
            this.labelModuleInfoSourceHeader.TabIndex = 5;
            this.labelModuleInfoSourceHeader.Text = "Source:";
            // 
            // labelModuleInfoSource
            // 
            this.labelModuleInfoSource.AutoSize = true;
            this.tableLayoutPanelModuleInfo.SetColumnSpan(this.labelModuleInfoSource, 2);
            this.labelModuleInfoSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModuleInfoSource.Location = new System.Drawing.Point(3, 87);
            this.labelModuleInfoSource.Name = "labelModuleInfoSource";
            this.labelModuleInfoSource.Size = new System.Drawing.Size(90, 13);
            this.labelModuleInfoSource.TabIndex = 6;
            this.labelModuleInfoSource.Text = "?";
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 167);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(451, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // FormColumnFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 194);
            this.Controls.Add(this.splitContainerFilter);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormColumnFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Column Filter";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.tableLayoutPanelFilter.ResumeLayout(false);
            this.tableLayoutPanelFilter.PerformLayout();
            this.splitContainerFilter.Panel1.ResumeLayout(false);
            this.splitContainerFilter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFilter)).EndInit();
            this.splitContainerFilter.ResumeLayout(false);
            this.tableLayoutPanelModuleInfo.ResumeLayout(false);
            this.tableLayoutPanelModuleInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.ComboBox comboBoxFilterOperator;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonOrderBy;
        private System.Windows.Forms.Label labelSorter;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Label labelOperator;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFilter;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.SplitContainer splitContainerFilter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelModuleInfo;
        private System.Windows.Forms.Label labelModuleInfo;
        private System.Windows.Forms.Button buttonRemoveModuleFilter;
        private System.Windows.Forms.TextBox textBoxModuleInfo;
        private System.Windows.Forms.Button buttonModuleFilterEdit;
        private System.Windows.Forms.Label labelModuleInfoOperator;
        private System.Windows.Forms.Label labelModuleInfoSourceHeader;
        private System.Windows.Forms.Label labelModuleInfoSource;
        private System.Windows.Forms.Button buttonAdd;
    }
}