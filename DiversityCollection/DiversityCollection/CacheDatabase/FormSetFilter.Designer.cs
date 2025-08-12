namespace DiversityCollection.CacheDatabase
{
    partial class FormSetFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetFilter));
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            comboBoxColumn = new System.Windows.Forms.ComboBox();
            comboBoxOperator = new System.Windows.Forms.ComboBox();
            textBoxValue = new System.Windows.Forms.TextBox();
            labelHeader = new System.Windows.Forms.Label();
            labelFilter = new System.Windows.Forms.Label();
            buttonAddFilter = new System.Windows.Forms.Button();
            buttonClearFilter = new System.Windows.Forms.Button();
            userControlDialogPanel1 = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            helpProvider = new System.Windows.Forms.HelpProvider();
            tableLayoutPanelMain.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 4;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            tableLayoutPanelMain.Controls.Add(comboBoxColumn, 0, 1);
            tableLayoutPanelMain.Controls.Add(comboBoxOperator, 1, 1);
            tableLayoutPanelMain.Controls.Add(textBoxValue, 2, 1);
            tableLayoutPanelMain.Controls.Add(labelHeader, 0, 0);
            tableLayoutPanelMain.Controls.Add(labelFilter, 0, 2);
            tableLayoutPanelMain.Controls.Add(buttonAddFilter, 3, 1);
            tableLayoutPanelMain.Controls.Add(buttonClearFilter, 3, 2);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 3;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.Size = new System.Drawing.Size(358, 103);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // comboBoxColumn
            // 
            comboBoxColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxColumn.FormattingEnabled = true;
            comboBoxColumn.Location = new System.Drawing.Point(4, 18);
            comboBoxColumn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxColumn.Name = "comboBoxColumn";
            comboBoxColumn.Size = new System.Drawing.Size(138, 23);
            comboBoxColumn.TabIndex = 0;
            // 
            // comboBoxOperator
            // 
            comboBoxOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxOperator.FormattingEnabled = true;
            comboBoxOperator.Items.AddRange(new object[] { "=", "~", "<", ">" });
            comboBoxOperator.Location = new System.Drawing.Point(150, 18);
            comboBoxOperator.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxOperator.Name = "comboBoxOperator";
            comboBoxOperator.Size = new System.Drawing.Size(34, 23);
            comboBoxOperator.TabIndex = 1;
            // 
            // textBoxValue
            // 
            textBoxValue.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxValue.Location = new System.Drawing.Point(192, 18);
            textBoxValue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxValue.Name = "textBoxValue";
            textBoxValue.Size = new System.Drawing.Size(138, 23);
            textBoxValue.TabIndex = 2;
            textBoxValue.TextChanged += textBoxValue_TextChanged;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelHeader, 3);
            labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeader.Location = new System.Drawing.Point(4, 0);
            labelHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new System.Drawing.Size(326, 15);
            labelHeader.TabIndex = 3;
            labelHeader.Text = "Set filter";
            // 
            // labelFilter
            // 
            labelFilter.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelFilter, 3);
            labelFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            labelFilter.Location = new System.Drawing.Point(4, 48);
            labelFilter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelFilter.Name = "labelFilter";
            labelFilter.Size = new System.Drawing.Size(326, 55);
            labelFilter.TabIndex = 4;
            labelFilter.Text = "WHERE ";
            // 
            // buttonAddFilter
            // 
            buttonAddFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonAddFilter.FlatAppearance.BorderSize = 0;
            buttonAddFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonAddFilter.Image = Resource.FilterAdd;
            buttonAddFilter.Location = new System.Drawing.Point(338, 18);
            buttonAddFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonAddFilter.Name = "buttonAddFilter";
            buttonAddFilter.Size = new System.Drawing.Size(16, 27);
            buttonAddFilter.TabIndex = 5;
            buttonAddFilter.UseVisualStyleBackColor = true;
            buttonAddFilter.Click += buttonAddFilter_Click;
            // 
            // buttonClearFilter
            // 
            buttonClearFilter.Dock = System.Windows.Forms.DockStyle.Top;
            buttonClearFilter.FlatAppearance.BorderSize = 0;
            buttonClearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonClearFilter.Image = Resource.FilterClear;
            buttonClearFilter.Location = new System.Drawing.Point(338, 51);
            buttonClearFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonClearFilter.Name = "buttonClearFilter";
            buttonClearFilter.Size = new System.Drawing.Size(16, 27);
            buttonClearFilter.TabIndex = 6;
            buttonClearFilter.UseVisualStyleBackColor = true;
            buttonClearFilter.Click += buttonClearFilter_Click;
            // 
            // userControlDialogPanel1
            // 
            userControlDialogPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel1.Location = new System.Drawing.Point(0, 103);
            userControlDialogPanel1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlDialogPanel1.Name = "userControlDialogPanel1";
            userControlDialogPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel1.Size = new System.Drawing.Size(358, 31);
            userControlDialogPanel1.TabIndex = 1;
            // 
            // FormSetFilter
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(358, 134);
            Controls.Add(tableLayoutPanelMain);
            Controls.Add(userControlDialogPanel1);
            helpProvider.SetHelpKeyword(this, "cachedatabase_projects_dc#filter");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormSetFilter";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Set filter";
            KeyDown += Form_KeyDown;
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ComboBox comboBoxColumn;
        private System.Windows.Forms.ComboBox comboBoxOperator;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.Button buttonAddFilter;
        private System.Windows.Forms.Button buttonClearFilter;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel1;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}