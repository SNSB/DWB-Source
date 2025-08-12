namespace DiversityWorkbench.Forms
{
    partial class FormGetRowFromTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGetRowFromTable));
            this.labelHeader = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelFilter = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxColumn = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripComboBoxOperator = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripTextBoxValue = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveFilter = new System.Windows.Forms.ToolStripButton();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.toolStripButtonDetails = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHeader.Location = new System.Drawing.Point(0, 0);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(531, 25);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "label1";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 50);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(531, 194);
            this.dataGridView.TabIndex = 2;
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelFilter,
            this.toolStripComboBoxColumn,
            this.toolStripComboBoxOperator,
            this.toolStripTextBoxValue,
            this.toolStripButtonFilter,
            this.toolStripButtonRemoveFilter,
            this.toolStripButtonDetails});
            this.toolStrip.Location = new System.Drawing.Point(0, 25);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(531, 25);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripLabelFilter
            // 
            this.toolStripLabelFilter.Name = "toolStripLabelFilter";
            this.toolStripLabelFilter.Size = new System.Drawing.Size(36, 22);
            this.toolStripLabelFilter.Text = "Filter:";
            // 
            // toolStripComboBoxColumn
            // 
            this.toolStripComboBoxColumn.Name = "toolStripComboBoxColumn";
            this.toolStripComboBoxColumn.Size = new System.Drawing.Size(200, 25);
            this.toolStripComboBoxColumn.ToolTipText = "Select the column for the filte";
            // 
            // toolStripComboBoxOperator
            // 
            this.toolStripComboBoxOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxOperator.DropDownWidth = 30;
            this.toolStripComboBoxOperator.Name = "toolStripComboBoxOperator";
            this.toolStripComboBoxOperator.Size = new System.Drawing.Size(75, 25);
            this.toolStripComboBoxOperator.ToolTipText = "The operator for the filtering";
            // 
            // toolStripTextBoxValue
            // 
            this.toolStripTextBoxValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBoxValue.Name = "toolStripTextBoxValue";
            this.toolStripTextBoxValue.Size = new System.Drawing.Size(100, 25);
            this.toolStripTextBoxValue.ToolTipText = "The value of the filter";
            // 
            // toolStripButtonFilter
            // 
            this.toolStripButtonFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFilter.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            this.toolStripButtonFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFilter.Name = "toolStripButtonFilter";
            this.toolStripButtonFilter.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFilter.Text = "Filter the data in the table for values in the selected column";
            this.toolStripButtonFilter.Click += new System.EventHandler(this.toolStripButtonFilter_Click);
            // 
            // toolStripButtonRemoveFilter
            // 
            this.toolStripButtonRemoveFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveFilter.Image = global::DiversityWorkbench.Properties.Resources.FilterClear;
            this.toolStripButtonRemoveFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveFilter.Name = "toolStripButtonRemoveFilter";
            this.toolStripButtonRemoveFilter.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemoveFilter.Text = "Remove the filter";
            this.toolStripButtonRemoveFilter.Click += new System.EventHandler(this.toolStripButtonRemoveFilter_Click);
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 244);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(531, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // toolStripButtonDetails
            // 
            this.toolStripButtonDetails.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDetails.Image = global::DiversityWorkbench.Properties.Resources.Lupe;
            this.toolStripButtonDetails.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDetails.Name = "toolStripButtonDetails";
            this.toolStripButtonDetails.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDetails.Text = "Show details";
            this.toolStripButtonDetails.Visible = false;
            this.toolStripButtonDetails.Click += new System.EventHandler(this.toolStripButtonDetails_Click);
            // 
            // FormGetRowFromTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 271);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormGetRowFromTable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormGetRowFromTable";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxColumn;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxOperator;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxValue;
        private System.Windows.Forms.ToolStripButton toolStripButtonFilter;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveFilter;
        private System.Windows.Forms.ToolStripLabel toolStripLabelFilter;
        private System.Windows.Forms.ToolStripButton toolStripButtonDetails;
    }
}