namespace DiversityWorkbench.Forms
{
    partial class FormQueryOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQueryOptions));
            this.treeViewQueryOptions = new System.Windows.Forms.TreeView();
            this.textBoxMaxItems = new System.Windows.Forms.TextBox();
            this.labelMaxItems = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelQueryLimitDropdownList = new System.Windows.Forms.Label();
            this.textBoxQueryLimitDropdownList = new System.Windows.Forms.TextBox();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonSelectNone = new System.Windows.Forms.Button();
            this.buttonSearchNode = new System.Windows.Forms.Button();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.checkBoxLimitCharacterCount = new System.Windows.Forms.CheckBox();
            this.numericUpDownLimitCharacterCount = new System.Windows.Forms.NumericUpDown();
            this.buttonExpand = new System.Windows.Forms.Button();
            this.buttonCollapse = new System.Windows.Forms.Button();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.checkBoxIncludeAbbreviation = new System.Windows.Forms.CheckBox();
            this.labelQueryLimitHierarchy = new System.Windows.Forms.Label();
            this.textBoxQueryLimitHierarchy = new System.Windows.Forms.TextBox();
            this.pictureBoxHierarchy = new System.Windows.Forms.PictureBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLimitCharacterCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHierarchy)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewQueryOptions
            // 
            this.treeViewQueryOptions.CheckBoxes = true;
            this.tableLayoutPanel.SetColumnSpan(this.treeViewQueryOptions, 4);
            this.treeViewQueryOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewQueryOptions.Location = new System.Drawing.Point(3, 131);
            this.treeViewQueryOptions.Name = "treeViewQueryOptions";
            this.treeViewQueryOptions.Size = new System.Drawing.Size(288, 148);
            this.treeViewQueryOptions.TabIndex = 2;
            this.treeViewQueryOptions.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewQueryOptions_AfterCheck);
            this.treeViewQueryOptions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeViewQueryOptions_MouseUp);
            // 
            // textBoxMaxItems
            // 
            this.textBoxMaxItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMaxItems.Location = new System.Drawing.Point(175, 3);
            this.textBoxMaxItems.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxMaxItems.Name = "textBoxMaxItems";
            this.textBoxMaxItems.Size = new System.Drawing.Size(87, 20);
            this.textBoxMaxItems.TabIndex = 1;
            this.textBoxMaxItems.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip.SetToolTip(this.textBoxMaxItems, "The maximal number of query results");
            // 
            // labelMaxItems
            // 
            this.labelMaxItems.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelMaxItems, 2);
            this.labelMaxItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxItems.Location = new System.Drawing.Point(3, 0);
            this.labelMaxItems.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelMaxItems.Name = "labelMaxItems";
            this.labelMaxItems.Size = new System.Drawing.Size(172, 26);
            this.labelMaxItems.TabIndex = 0;
            this.labelMaxItems.Text = "Maximal number of results:";
            this.labelMaxItems.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel.SetColumnSpan(this.textBoxDescription, 4);
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(3, 285);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDescription.Size = new System.Drawing.Size(288, 60);
            this.textBoxDescription.TabIndex = 5;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.textBoxMaxItems, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.treeViewQueryOptions, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.labelMaxItems, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelQueryLimitDropdownList, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxQueryLimitDropdownList, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonSelectAll, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonSelectNone, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonSearchNode, 0, 7);
            this.tableLayoutPanel.Controls.Add(this.textBoxSearch, 2, 7);
            this.tableLayoutPanel.Controls.Add(this.checkBoxLimitCharacterCount, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.numericUpDownLimitCharacterCount, 2, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonExpand, 2, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonCollapse, 3, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonFeedback, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.checkBoxIncludeAbbreviation, 2, 8);
            this.tableLayoutPanel.Controls.Add(this.labelQueryLimitHierarchy, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxQueryLimitHierarchy, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxHierarchy, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.comboBox1, 3, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 9;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(294, 392);
            this.tableLayoutPanel.TabIndex = 7;
            // 
            // labelQueryLimitDropdownList
            // 
            this.labelQueryLimitDropdownList.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelQueryLimitDropdownList, 2);
            this.labelQueryLimitDropdownList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelQueryLimitDropdownList.Location = new System.Drawing.Point(3, 52);
            this.labelQueryLimitDropdownList.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelQueryLimitDropdownList.Name = "labelQueryLimitDropdownList";
            this.labelQueryLimitDropdownList.Size = new System.Drawing.Size(172, 24);
            this.labelQueryLimitDropdownList.TabIndex = 6;
            this.labelQueryLimitDropdownList.Text = "Limit for drop down lists:";
            this.labelQueryLimitDropdownList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxQueryLimitDropdownList
            // 
            this.textBoxQueryLimitDropdownList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxQueryLimitDropdownList.Location = new System.Drawing.Point(175, 55);
            this.textBoxQueryLimitDropdownList.Margin = new System.Windows.Forms.Padding(0, 3, 3, 1);
            this.textBoxQueryLimitDropdownList.Name = "textBoxQueryLimitDropdownList";
            this.textBoxQueryLimitDropdownList.Size = new System.Drawing.Size(87, 20);
            this.textBoxQueryLimitDropdownList.TabIndex = 7;
            this.textBoxQueryLimitDropdownList.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip.SetToolTip(this.textBoxQueryLimitDropdownList, "The limit of entries in the database for the creation of a drop down list");
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSelectAll.Image = global::DiversityWorkbench.Properties.Resources.CheckYes;
            this.buttonSelectAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSelectAll.Location = new System.Drawing.Point(3, 102);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectAll.TabIndex = 8;
            this.buttonSelectAll.Text = "Check all";
            this.buttonSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonSelectAll, "Select all query conditions");
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // buttonSelectNone
            // 
            this.buttonSelectNone.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSelectNone.Image = global::DiversityWorkbench.Properties.Resources.CheckNo;
            this.buttonSelectNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSelectNone.Location = new System.Drawing.Point(84, 102);
            this.buttonSelectNone.Name = "buttonSelectNone";
            this.buttonSelectNone.Size = new System.Drawing.Size(88, 23);
            this.buttonSelectNone.TabIndex = 9;
            this.buttonSelectNone.Text = "Check none";
            this.buttonSelectNone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonSelectNone, "Select none of the query conditions");
            this.buttonSelectNone.UseVisualStyleBackColor = true;
            this.buttonSelectNone.Click += new System.EventHandler(this.buttonSelectNone_Click);
            // 
            // buttonSearchNode
            // 
            this.tableLayoutPanel.SetColumnSpan(this.buttonSearchNode, 2);
            this.buttonSearchNode.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSearchNode.Image = global::DiversityWorkbench.Properties.Resources.Find;
            this.buttonSearchNode.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSearchNode.Location = new System.Drawing.Point(75, 348);
            this.buttonSearchNode.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSearchNode.Name = "buttonSearchNode";
            this.tableLayoutPanel.SetRowSpan(this.buttonSearchNode, 2);
            this.buttonSearchNode.Size = new System.Drawing.Size(100, 44);
            this.buttonSearchNode.TabIndex = 10;
            this.buttonSearchNode.Text = "Search for option:";
            this.buttonSearchNode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.buttonSearchNode.UseVisualStyleBackColor = true;
            this.buttonSearchNode.Click += new System.EventHandler(this.buttonSearchNode_Click);
            // 
            // textBoxSearch
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxSearch, 2);
            this.textBoxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSearch.Location = new System.Drawing.Point(175, 351);
            this.textBoxSearch.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(116, 20);
            this.textBoxSearch.TabIndex = 11;
            // 
            // checkBoxLimitCharacterCount
            // 
            this.checkBoxLimitCharacterCount.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.checkBoxLimitCharacterCount, 2);
            this.checkBoxLimitCharacterCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxLimitCharacterCount.Location = new System.Drawing.Point(16, 76);
            this.checkBoxLimitCharacterCount.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.checkBoxLimitCharacterCount.Name = "checkBoxLimitCharacterCount";
            this.checkBoxLimitCharacterCount.Size = new System.Drawing.Size(159, 20);
            this.checkBoxLimitCharacterCount.TabIndex = 12;
            this.checkBoxLimitCharacterCount.Text = "Min. char. for drop down list:";
            this.checkBoxLimitCharacterCount.UseVisualStyleBackColor = true;
            // 
            // numericUpDownLimitCharacterCount
            // 
            this.numericUpDownLimitCharacterCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownLimitCharacterCount.Location = new System.Drawing.Point(175, 76);
            this.numericUpDownLimitCharacterCount.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.numericUpDownLimitCharacterCount.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownLimitCharacterCount.Name = "numericUpDownLimitCharacterCount";
            this.numericUpDownLimitCharacterCount.Size = new System.Drawing.Size(87, 20);
            this.numericUpDownLimitCharacterCount.TabIndex = 13;
            this.numericUpDownLimitCharacterCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownLimitCharacterCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // buttonExpand
            // 
            this.buttonExpand.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExpand.Image = global::DiversityWorkbench.Properties.Resources.Expand;
            this.buttonExpand.Location = new System.Drawing.Point(239, 102);
            this.buttonExpand.Name = "buttonExpand";
            this.buttonExpand.Size = new System.Drawing.Size(23, 23);
            this.buttonExpand.TabIndex = 14;
            this.toolTip.SetToolTip(this.buttonExpand, "Expand all");
            this.buttonExpand.UseVisualStyleBackColor = true;
            this.buttonExpand.Click += new System.EventHandler(this.buttonExpand_Click);
            // 
            // buttonCollapse
            // 
            this.buttonCollapse.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCollapse.Image = global::DiversityWorkbench.Properties.Resources.Collapse;
            this.buttonCollapse.Location = new System.Drawing.Point(268, 102);
            this.buttonCollapse.Name = "buttonCollapse";
            this.buttonCollapse.Size = new System.Drawing.Size(23, 23);
            this.buttonCollapse.TabIndex = 15;
            this.toolTip.SetToolTip(this.buttonCollapse, "Collapse all");
            this.buttonCollapse.UseVisualStyleBackColor = true;
            this.buttonCollapse.Click += new System.EventHandler(this.buttonCollapse_Click);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(267, 3);
            this.buttonFeedback.Margin = new System.Windows.Forms.Padding(2, 3, 3, 0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(24, 23);
            this.buttonFeedback.TabIndex = 16;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send a feedback to the administrator");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // checkBoxIncludeAbbreviation
            // 
            this.checkBoxIncludeAbbreviation.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.checkBoxIncludeAbbreviation, 2);
            this.checkBoxIncludeAbbreviation.Location = new System.Drawing.Point(175, 371);
            this.checkBoxIncludeAbbreviation.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxIncludeAbbreviation.Name = "checkBoxIncludeAbbreviation";
            this.checkBoxIncludeAbbreviation.Size = new System.Drawing.Size(100, 17);
            this.checkBoxIncludeAbbreviation.TabIndex = 17;
            this.checkBoxIncludeAbbreviation.Text = "Include abbrev.";
            this.toolTip.SetToolTip(this.checkBoxIncludeAbbreviation, "Include abbreviation for query option in search");
            this.checkBoxIncludeAbbreviation.UseVisualStyleBackColor = true;
            // 
            // labelQueryLimitHierarchy
            // 
            this.labelQueryLimitHierarchy.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelQueryLimitHierarchy, 2);
            this.labelQueryLimitHierarchy.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelQueryLimitHierarchy.Location = new System.Drawing.Point(75, 26);
            this.labelQueryLimitHierarchy.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelQueryLimitHierarchy.Name = "labelQueryLimitHierarchy";
            this.labelQueryLimitHierarchy.Size = new System.Drawing.Size(100, 26);
            this.labelQueryLimitHierarchy.TabIndex = 18;
            this.labelQueryLimitHierarchy.Text = "Limit for hierarchies:";
            this.labelQueryLimitHierarchy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxQueryLimitHierarchy
            // 
            this.textBoxQueryLimitHierarchy.Location = new System.Drawing.Point(175, 29);
            this.textBoxQueryLimitHierarchy.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxQueryLimitHierarchy.Name = "textBoxQueryLimitHierarchy";
            this.textBoxQueryLimitHierarchy.Size = new System.Drawing.Size(87, 20);
            this.textBoxQueryLimitHierarchy.TabIndex = 19;
            this.textBoxQueryLimitHierarchy.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBoxHierarchy
            // 
            this.pictureBoxHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxHierarchy.Enabled = false;
            this.pictureBoxHierarchy.Image = global::DiversityWorkbench.Properties.Resources.HierarchyGrey;
            this.pictureBoxHierarchy.Location = new System.Drawing.Point(265, 32);
            this.pictureBoxHierarchy.Margin = new System.Windows.Forms.Padding(0, 6, 3, 3);
            this.pictureBoxHierarchy.Name = "pictureBoxHierarchy";
            this.pictureBoxHierarchy.Size = new System.Drawing.Size(26, 17);
            this.pictureBoxHierarchy.TabIndex = 20;
            this.pictureBoxHierarchy.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(265, 66);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(0, 14, 12, 3);
            this.comboBox1.Name = "comboBox1";
            this.tableLayoutPanel.SetRowSpan(this.comboBox1, 2);
            this.comboBox1.Size = new System.Drawing.Size(17, 21);
            this.comboBox1.TabIndex = 22;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(2, 394);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(294, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // FormQueryOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 423);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormQueryOptions";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set query options";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLimitCharacterCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHierarchy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TreeView treeViewQueryOptions;
        private System.Windows.Forms.Label labelMaxItems;
        private System.Windows.Forms.TextBox textBoxMaxItems;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelQueryLimitDropdownList;
        private System.Windows.Forms.TextBox textBoxQueryLimitDropdownList;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonSelectNone;
        private System.Windows.Forms.Button buttonSearchNode;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.CheckBox checkBoxLimitCharacterCount;
        private System.Windows.Forms.NumericUpDown numericUpDownLimitCharacterCount;
        private System.Windows.Forms.Button buttonExpand;
        private System.Windows.Forms.Button buttonCollapse;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.CheckBox checkBoxIncludeAbbreviation;
        private System.Windows.Forms.Label labelQueryLimitHierarchy;
        private System.Windows.Forms.TextBox textBoxQueryLimitHierarchy;
        private System.Windows.Forms.PictureBox pictureBoxHierarchy;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}