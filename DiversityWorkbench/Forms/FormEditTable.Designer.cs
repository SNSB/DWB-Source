namespace DiversityWorkbench.Forms
{
    partial class FormEditTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditTable));
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAll = new System.Windows.Forms.Button();
            this.buttonNone = new System.Windows.Forms.Button();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.comboBoxFilter = new System.Windows.Forms.ComboBox();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.labelFilterColumn = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(4, 388);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(618, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 3);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 6);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(479, 13);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "label1";
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel.SetColumnSpan(this.dataGridView, 5);
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(3, 57);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(612, 324);
            this.dataGridView.TabIndex = 2;
            this.dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.dataGridView, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonAll, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonNone, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonFilter, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxFilter, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxFilter, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.labelFilterColumn, 4, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(618, 384);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // buttonAll
            // 
            this.buttonAll.Location = new System.Drawing.Point(3, 28);
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Size = new System.Drawing.Size(59, 23);
            this.buttonAll.TabIndex = 3;
            this.buttonAll.Text = "Select all";
            this.buttonAll.UseVisualStyleBackColor = true;
            this.buttonAll.Visible = false;
            this.buttonAll.Click += new System.EventHandler(this.buttonAll_Click);
            // 
            // buttonNone
            // 
            this.buttonNone.Location = new System.Drawing.Point(68, 28);
            this.buttonNone.Name = "buttonNone";
            this.buttonNone.Size = new System.Drawing.Size(39, 23);
            this.buttonNone.TabIndex = 4;
            this.buttonNone.Text = "none";
            this.buttonNone.UseVisualStyleBackColor = true;
            this.buttonNone.Visible = false;
            this.buttonNone.Click += new System.EventHandler(this.buttonNone_Click);
            // 
            // buttonFilter
            // 
            this.buttonFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFilter.FlatAppearance.BorderSize = 0;
            this.buttonFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFilter.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            this.buttonFilter.Location = new System.Drawing.Point(485, 0);
            this.buttonFilter.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(30, 25);
            this.buttonFilter.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonFilter, "Apply filter to the selected column");
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Visible = false;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // comboBoxFilter
            // 
            this.comboBoxFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilter.FormattingEnabled = true;
            this.comboBoxFilter.Items.AddRange(new object[] {
            "=",
            ">",
            ">=",
            "<",
            "<=",
            "~"});
            this.comboBoxFilter.Location = new System.Drawing.Point(485, 28);
            this.comboBoxFilter.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.comboBoxFilter.Name = "comboBoxFilter";
            this.comboBoxFilter.Size = new System.Drawing.Size(30, 21);
            this.comboBoxFilter.TabIndex = 6;
            this.toolTip.SetToolTip(this.comboBoxFilter, "Operator for the filter");
            this.comboBoxFilter.Visible = false;
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(515, 28);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(100, 20);
            this.textBoxFilter.TabIndex = 7;
            this.toolTip.SetToolTip(this.textBoxFilter, "Value for the filter");
            this.textBoxFilter.Visible = false;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // labelFilterColumn
            // 
            this.labelFilterColumn.AutoSize = true;
            this.labelFilterColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFilterColumn.Location = new System.Drawing.Point(518, 0);
            this.labelFilterColumn.Name = "labelFilterColumn";
            this.labelFilterColumn.Size = new System.Drawing.Size(97, 25);
            this.labelFilterColumn.TabIndex = 8;
            this.labelFilterColumn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormEditTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 419);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEditTable";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormEditTable";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditTable_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonAll;
        private System.Windows.Forms.Button buttonNone;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonFilter;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ComboBox comboBoxFilter;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Label labelFilterColumn;
    }
}