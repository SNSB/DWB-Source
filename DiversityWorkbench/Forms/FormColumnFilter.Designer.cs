namespace DiversityWorkbench.Forms
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
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.panelColumns = new System.Windows.Forms.Panel();
            this.tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            this.buttonShowData = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonShowFilter = new System.Windows.Forms.Button();
            this.tableLayoutPanelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 42);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(308, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(251, 32);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Set column filter";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelColumns
            // 
            this.panelColumns.AutoScroll = true;
            this.panelColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelColumns.Location = new System.Drawing.Point(0, 32);
            this.panelColumns.Name = "panelColumns";
            this.panelColumns.Size = new System.Drawing.Size(308, 10);
            this.panelColumns.TabIndex = 2;
            // 
            // tableLayoutPanelHeader
            // 
            this.tableLayoutPanelHeader.ColumnCount = 3;
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelHeader.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.buttonShowData, 1, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.buttonShowFilter, 2, 0);
            this.tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            this.tableLayoutPanelHeader.RowCount = 1;
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.Size = new System.Drawing.Size(308, 32);
            this.tableLayoutPanelHeader.TabIndex = 3;
            // 
            // buttonShowData
            // 
            this.buttonShowData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowData.Image = global::DiversityWorkbench.Properties.Resources.Lupe;
            this.buttonShowData.Location = new System.Drawing.Point(260, 3);
            this.buttonShowData.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.buttonShowData.Name = "buttonShowData";
            this.buttonShowData.Size = new System.Drawing.Size(25, 26);
            this.buttonShowData.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonShowData, "Show filtered data");
            this.buttonShowData.UseVisualStyleBackColor = true;
            this.buttonShowData.Click += new System.EventHandler(this.buttonShowData_Click);
            // 
            // buttonShowFilter
            // 
            this.buttonShowFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowFilter.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            this.buttonShowFilter.Location = new System.Drawing.Point(286, 3);
            this.buttonShowFilter.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.buttonShowFilter.Name = "buttonShowFilter";
            this.buttonShowFilter.Size = new System.Drawing.Size(19, 26);
            this.buttonShowFilter.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonShowFilter, "Show the current filter");
            this.buttonShowFilter.UseVisualStyleBackColor = true;
            this.buttonShowFilter.Click += new System.EventHandler(this.buttonShowFilter_Click);
            // 
            // FormColumnFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 69);
            this.Controls.Add(this.panelColumns);
            this.Controls.Add(this.tableLayoutPanelHeader);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormColumnFilter";
            this.Text = "Filter for table data";
            this.tableLayoutPanelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Panel panelColumns;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.Button buttonShowData;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonShowFilter;
    }
}